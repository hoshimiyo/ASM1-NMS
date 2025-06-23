using BLL.Interfaces;
using DAL.Entities;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class NewsTagService : INewsTagService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NewsTagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddNewsTagAsync(string newsArticleId, int tagId)
        {
            var article = await _unitOfWork.NewsArticles.GetByIdAsync(newsArticleId);
            var tag = await _unitOfWork.Tags.GetByIdAsync(tagId);

            // If article or tag not found, return false (indicating failure to add)
            if (article == null || tag == null)
            {
                return false;
            }

            // Check if the association already exists to prevent duplicates
            var existingNewsTag = await _unitOfWork.NewsTags.FirstOrDefaultAsync(nt => nt.NewsArticleId == newsArticleId && nt.TagId == tagId);
            if (existingNewsTag != null)
            {
                return false; // Or true, depending on if you consider "already exists" as a successful "add"
            }

            var newsTag = new NewsTag
            {
                NewsArticleId = newsArticleId,
                TagId = tagId
            };

            await _unitOfWork.NewsTags.AddAsync(newsTag);
            await _unitOfWork.SaveChangesAsync();
            return true; // Successfully added
        }

        public async Task<bool> DeleteNewsTagAsync(string newsArticleId, int tagId)
        {
            var newsTag = await _unitOfWork.NewsTags.FirstOrDefaultAsync(nt => nt.NewsArticleId == newsArticleId && nt.TagId == tagId);

            if (newsTag == null)
            {
                return false;
            }

            await _unitOfWork.NewsTags.DeleteAsync(newsTag);
            await _unitOfWork.SaveChangesAsync();
            return true; // Successfully deleted
        }

        public async Task<IEnumerable<Tag>> GetTagsOfArticleAsync(string newsArticleId)
        {
            var article = await _unitOfWork.NewsArticles.GetByIdAsync(newsArticleId);
            if (article == null)
            {
                return Enumerable.Empty<Tag>();
            }

            var tags = await _unitOfWork.NewsTags.GetTagsFromArticleAsync(newsArticleId);

            return tags ?? Enumerable.Empty<Tag>();
        }

        public async Task<IEnumerable<NewsArticle>> GetArticlesFromTagAsync(int tagId)
        {
            var tag = await _unitOfWork.Tags.GetByIdAsync(tagId);
            if (tag == null)
            {
                return Enumerable.Empty<NewsArticle>();
            }

            var articles = await _unitOfWork.NewsTags.GetArticlesFromTagAsync(tagId);

            return articles ?? Enumerable.Empty<NewsArticle>();
        }
    }
}