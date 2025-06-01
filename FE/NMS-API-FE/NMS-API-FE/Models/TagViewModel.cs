using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMS_API_FE.Models
{
    public class TagViewModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Mark as auto-increment (identity)
        public int TagId { get; set; }
        public string? TagName { get; set; }
        public string? Note { get; set; }
        public virtual ICollection<NewsTagViewModel> NewsTags { get; set; } = new List<NewsTagViewModel>(); // Many-to-many relationship
    }
}
