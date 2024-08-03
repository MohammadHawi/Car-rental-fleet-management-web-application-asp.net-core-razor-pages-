using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace V4.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        public int CID { get; set; }
        public string Phone { get; set; }
        public string Cname { get; set; }
        public DateTime CheckIn { get; set; }
        public string Car_Type { get; set; }


    }
}
