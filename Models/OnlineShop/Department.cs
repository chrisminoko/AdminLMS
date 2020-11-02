using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models.OnlineShop
{
    public class Department
    {
        [Key]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Department_ID { get; set; }
        [Required]
        [Display(Name = "Name")]
        [Index("Department_Index", IsUnique = true)]
        [MinLength(3)]
        [MaxLength(80)]
        public string Department_Name { get; set; }
        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [MinLength(3)]
        public string Description { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}