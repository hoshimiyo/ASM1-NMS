﻿using BLL.DTOs;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface INewsArticleService
    {
        Task<NewsArticle> CreateNewsArticleAsync(NewsArticleCreateDTO dto, int userId);
        Task UpdateNewsArticleAsync(string id, NewsArticleUpdateDTO dto, int userId);
        Task DeactiveNewsArticleAsync(string id);
        Task<NewsArticle> GetNewsArticleByIdAsync(string id);
        Task<IEnumerable<NewsArticle>> GetAllNewsArticlesAsync();
        Task<IEnumerable<NewsArticle>> GetActiveNewsArticlesAsync();
        Task<IEnumerable<NewsArticle>> GetNewsArticlesByUserIdAsync(int userId);
        Task<IEnumerable<NewsArticle>> GenerateReport(DateTime startDate, DateTime endDate);
        Task<IEnumerable<NewsArticle>> GetArticlesWithActiveCategories();

    }
}
