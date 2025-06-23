using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMS_API_FE.Models
{
    public class NewsArticleViewModel
    {
        [Key]
        [Required]
        public string NewsArticleId { get; set; }
        public string? NewsTitle { get; set; }
        [Required]
        public string Headline { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? NewsContent { get; set; }
        public string? NewsSource { get; set; }
        public bool? NewsStatus { get; set; } = true;
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedById { get; set; }
        public virtual SystemAccountViewModel? CreatedBy { get; set; } // Navigation for CreatedBy
        public int? UpdatedById { get; set; }
        public virtual SystemAccountViewModel? UpdatedBy { get; set; } // Navigation for UpdatedBy
        public virtual ICollection<NewsTagViewModel> NewsTags { get; set; } = new List<NewsTagViewModel>(); // Many-to-many relationship
        public int CategoryId { get; set; }
        public virtual CategoryViewModel? Category { get; set; }
    }
}
