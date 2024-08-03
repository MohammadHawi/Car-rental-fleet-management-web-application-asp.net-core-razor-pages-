using System;
using System.ComponentModel.DataAnnotations;

namespace V4.Models
{
    public class Sales
    {
        [Key]
        public int Id { get; set; }
        public string Cname { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set;}
        public int Plate { get; set; }             
    }
}
