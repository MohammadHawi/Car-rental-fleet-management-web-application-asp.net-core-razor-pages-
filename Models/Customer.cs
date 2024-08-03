
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace V4.Models
{
    public class Customer
    {

        public int Id { get; set; }
        [Required]
        [Display(Name = "Customer name")]
        public string Cname { get; set; }
        [Required]
        [Display(Name = "Customer phone number")]
        public string phone { get; set; }
        [Display(Name = "Customer nationality")]
        public string nationality { get; set; }
        [NotMapped]
        [Display(Name = "Upload passport")]
        public IFormFile PassPort { get; set; }
        public string ImageDate { get; set; }


    }
}
