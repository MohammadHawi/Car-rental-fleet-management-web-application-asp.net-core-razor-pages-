using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace V4.Models
{
    public class Contract
    {
        [Key]
        public int Id { get; set; }
        public int CID { get; set; }
        [Required]
        public string Cname { get; set; }
        [Required]
        public int Plate { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public DateTime CheckIn { get; set; }
        [Required]
        public DateTime CheckOut { get; set; }
        [Required]
        public float Deposit { get; set; }
        public String Note { get; set; }
        [Required]
        public int Paid { get; set; }

        public int KiloMeter { get; set; }

    }
}
