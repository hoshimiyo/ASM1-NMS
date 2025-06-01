using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMS_API_FE.Models
{
    public class SystemAccountViewModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Mark as auto-increment (identity)
        public int AccountId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string AccountName { get; set; }

        [Required]
        [EmailAddress]
        public string AccountEmail { get; set; }

        [Required]
        [Range(1, 3)]
        public int AccountRole { get; set; }

        [Required]
        public string AccountPasswordHash { get; set; }

        public virtual ICollection<NewsArticleViewModel> CreatedArticles { get; set; } = new List<NewsArticleViewModel>(); // Reverse navigation
        public virtual ICollection<NewsArticleViewModel> UpdatedArticles { get; set; } = new List<NewsArticleViewModel>(); // Reverse navigation
    }
}
