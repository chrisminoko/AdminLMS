using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    public class Package
    {
        [Key]
        public int PackageID { get; set; }
        public int PackageTypeID { get; set; }
        public virtual PackageType PackageType { get; set; }
        public  string  Storage { get; set; }
        public  int NumTeachers { get; set; }
        public int NumStudents { get; set; }
        public int NumEmails { get; set; }
        public string PackageDescription { get; set; }
        public decimal PackagePrice { get; set; }
        public byte[] UserPhoto { get; set; }

    }
}