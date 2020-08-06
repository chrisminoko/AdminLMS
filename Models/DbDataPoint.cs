using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    public class DbDataPoint
    {
        [Key]
        public int Id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }
}