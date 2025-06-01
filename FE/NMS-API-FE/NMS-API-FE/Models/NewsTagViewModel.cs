using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMS_API_FE.Models
{
    public class NewsTagViewModel
    {
        [Key]
        [Required]
        public string NewsArticleId { get; set; } = Guid.NewGuid().ToString();
        public virtual NewsArticleViewModel NewsArticle { get; set; }
        [Required]
        public int TagId { get; set; }
        public virtual TagViewModel Tag { get; set; } // Navigation to Tag
    }
}
