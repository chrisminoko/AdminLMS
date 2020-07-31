using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    public class PackageType
    {
        [Key]
        public int PackageTypeID { get; set; }
        public string PackageName { get; set; }
    }
}