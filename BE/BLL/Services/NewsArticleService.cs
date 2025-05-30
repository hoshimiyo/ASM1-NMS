﻿using BLL.DTOs;
using BLL.Interfaces;
using BLL.Utils;
using DAL.Entities;
using DAL.Interfaces;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserUtils _userUtils;
        private readonly INewsArticleRepository _newsArticleRepository;

        public NewsArticleService(IUnitOfWork unitOfWork, UserUtils userUtils, INewsArticleRepository newsArticleRepository)
        {
            _unitOfWork = unitOfWork;
            _userUtils = userUtils;
            _newsArticleRepository = newsArticleRepository;
        }
        public async Task CreateNewsArticleAsync(NewsArticleCreateDTO dto, HttpContext httpContext)
        {
            var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);
            Console.WriteLine("UserId: " + userId);
            var user = await _unitOfWork.SystemAccounts.GetByIdAsync(userId); // Fetch user by ID
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            // Ensure required fields are not empty
            if (string.IsNullOrWhiteSpace(dto.Headline))
            {
                throw new ArgumentException("Headline is required.");
            }

            // Create new article
            var article = new NewsArticle
            {
                NewsTitle = dto.NewsTitle,  // Optional, can be null
                Headline = dto.Headline,
                CreatedDate = DateTime.UtcNow,
                NewsContent = dto.NewsContent,  // Optional
                NewsSource = dto.NewsSource,  // Optional
                NewsStatus = dto.NewsStatus,  // Default to true if not provided
                CategoryId = dto.CategoryId,
                ModifiedDate = DateTime.UtcNow,
                CreatedById = userId
            };

            // Add article to the repository
            await _unitOfWork.NewsArticles.AddAsync(article);

            // Handle tags if provided
            if (dto.NewsTagIds != null && dto.NewsTagIds.Any())
            {
                foreach (var tagId in dto.NewsTagIds)
                {
                    var tag = await _unitOfWork.Tags.GetByIdAsync(tagId);
                    if (tag != null)
                    {
                        var newsTag = new NewsTag
                        {
                            NewsArticleId = article.NewsArticleId,  // This assumes NewsArticleId is generated when saved
                            TagId = tagId,
                        };
                        await _unitOfWork.NewsTags.AddAsync(newsTag);
                    }
                }
            }

            // Save changes
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeactiveNewsArticleAsync(string id)
        {
            var article = await _unitOfWork.NewsArticles.GetByIdAsync(id);
            if (article != null)
            {
                article.NewsStatus = false;
                await _unitOfWork.NewsArticles.UpdateAsync(article);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<NewsArticle>> GetActiveNewsArticlesAsync() => await _newsArticleRepository.GetActiveNewsArticlesAsync();


        public async Task<NewsArticle> GetNewsArticleByIdAsync(string id) => await _newsArticleRepository.GetNewsArticleByIdAsync(id);

        public async Task<IEnumerable<NewsArticle>> GetNewsArticlesByUserIdAsync(int userId)
        {
            return await _newsArticleRepository.GetActiveNewsArticlesByUserIdAsync(userId);

        }

        public async Task<IEnumerable<NewsArticle>> GetAllNewsArticlesAsync()
        {
            return await _unitOfWork.NewsArticles.GetAllArticlesAsync();
        }

        public async Task UpdateNewsArticleAsync(string id, NewsArticleUpdateDTO dto, HttpContext httpContext)
        {
            var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);

            // Fetch the existing article by ID
            var article = await _unitOfWork.NewsArticles.GetByIdAsync(id);

            // Check if the article exists
            if (article == null)
            {
                throw new KeyNotFoundException("News article not found.");
            }

            // Update article properties based on DTO, only if the field is not null or empty
            if (!string.IsNullOrWhiteSpace(dto.NewsTitle))
            {
                article.NewsTitle = dto.NewsTitle;
            }

            if (!string.IsNullOrWhiteSpace(dto.Headline))
            {
                article.Headline = dto.Headline;
            }

            if (!string.IsNullOrWhiteSpace(dto.NewsContent))
            {
                article.NewsContent = dto.NewsContent;
            }

            if (!string.IsNullOrWhiteSpace(dto.NewsSource))
            {
                article.NewsSource = dto.NewsSource;
            }

            if (dto.NewsStatus.HasValue) // Only update if NewsStatus is provided
            {
                article.NewsStatus = dto.NewsStatus.Value;
            }

            article.UpdatedById = userId;

            // Only update ModifiedDate if there's any change in the article
            article.ModifiedDate = DateTime.UtcNow;

            // If CategoryId is provided and is different from the existing category, update it
            if (dto.CategoryId.HasValue && article.CategoryId != dto.CategoryId.Value)
            {
                article.CategoryId = dto.CategoryId.Value;
            }

            // Update tags if provided in the DTO (preserving unmodified tags)
            if (dto.NewsTagIds != null)
            {
                // Get existing tags for this article
                var existingTags = await _unitOfWork.NewsTags
                    .GetAllByListAsync(nt => nt.NewsArticleId == id);
                var existingTagIds = existingTags.Select(et => et.TagId).ToList();

                // Find tags to add (tags in the new list but not in existing tags)
                var tagsToAdd = dto.NewsTagIds
                    .Where(tagId => !existingTagIds.Contains(tagId))
                    .ToList();

                // Find tags to remove (tags in existing tags but not in the new list)
                var tagsToRemove = existingTags
                    .Where(et => !dto.NewsTagIds.Contains(et.TagId))
                    .ToList();

                // Remove tags that are no longer needed
                if (tagsToRemove.Any())
                {
                    _unitOfWork.NewsTags.RemoveRange(tagsToRemove);
                }

                // Add new tags
                foreach (var tagId in tagsToAdd)
                {
                    // Verify the tag exists before adding it
                    var tag = await _unitOfWork.Tags.GetByIdAsync(tagId);
                    if (tag != null)
                    {
                        var newsTag = new NewsTag
                        {
                            NewsArticleId = article.NewsArticleId,
                            TagId = tagId,
                        };
                        await _unitOfWork.NewsTags.AddAsync(newsTag);
                    }
                }
            }

            // Save the changes to the database
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<IEnumerable<NewsArticle>> GenerateReport(DateTime startDate, DateTime endDate)
        {

            return (IEnumerable<NewsArticle>)await _unitOfWork.NewsArticles.GetByConditionAsync(a => a.ModifiedDate >= startDate && a.ModifiedDate <= endDate);
        }

        public async Task<IEnumerable<NewsArticle>> GetArticlesWithActiveCategories()
        {
            return await _unitOfWork.NewsArticles.GetArticlesWithActiveCategories();
        }

    }
}
