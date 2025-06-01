using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class CategoryDTO
    {
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public int? ParentCategoryId { get; set; }
    }

    public class CategoryCreateDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public bool IsActive { get; set; } = true;
        public int? ParentCategoryId { get; set; } = null;
    }
}
