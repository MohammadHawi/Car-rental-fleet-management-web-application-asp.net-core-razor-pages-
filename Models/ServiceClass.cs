using System;
using System.ComponentModel.DataAnnotations;


namespace V4.Models
{
    public class ServiceClass
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Plate { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int KiloMeter { get; set; }
        [Required]
        public String Type { get; set; }
        public String Note { get; set; }

    }
}
