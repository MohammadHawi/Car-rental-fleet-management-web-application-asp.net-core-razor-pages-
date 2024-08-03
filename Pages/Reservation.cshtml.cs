using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using V4.Models;

namespace V4.Pages
{
    [Authorize(Policy = "MustSignIn")]
    public class ReservationModel : PageModel
    {
        public readonly IConfiguration _configuration;
        public static string conn { get; set; }
        public ReservationModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void OnGet()
        {
            conn = _configuration.GetConnectionString("DefaultConnection");
            GetReservations = DisplayReservations();
        }
        [BindProperty]
        public Reservation reservation { get; set; }
        public IEnumerable<Reservation> GetReservations { get; set; }

        public static List<Reservation> DisplayReservations()
        {
            List<Reservation> reservations = new List<Reservation>();
            //string conn = "Data Source=DESKTOP-O5QVOR0;Initial Catalog=DanielRentV4;Integrated Security=True";
            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                string query = "select * from Reservation,Customer where Reservation.CID = Customer.Id  order by Reservation.CheckIn";


                using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                {
                    sqlconn.Open();
                    using (SqlDataReader sdr = sqlcomm.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            Reservation r = new Reservation();
                            r.Id = Convert.ToInt32(sdr["Id"]);
                            r.Car_Type = Convert.ToString(sdr["Car_Type"]);
                            r.Phone = Convert.ToString(sdr["Phone"]);
                            r.Cname = Convert.ToString(sdr["Cname"]);
                            r.CID = Convert.ToInt32(sdr["CID"]);
                            r.CheckIn = Convert.ToDateTime(sdr["CheckIn"]);

                            reservations.Add(r);
                        }
                    }
                    return reservations;
                }
            }
        }
        public IActionResult OnPostAdd()
        {
            
            using (SqlConnection sqlconn = new SqlConnection(conn))
            {

                int getCID()
                {
                    string query = "select Id from Customer where Cname =(N'" + reservation.Cname + "')";
                    using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                    {
                        sqlconn.Open();
                        using (SqlDataReader sdr = sqlcomm.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                return Convert.ToInt32(sdr["Id"]);
                            }
                        }

                    }
                    return 0;
                }
                


                string q = "insert into Reservation values ('" +  getCID() + "','" + reservation.Car_Type + "','"+reservation.CheckIn+ "')";

                using (SqlCommand sqlcomm = new SqlCommand(q, sqlconn))
                {

                    sqlcomm.ExecuteNonQuery();
                }


            }
            GetReservations = DisplayReservations();
            return Page();

        }

        public IActionResult OnPostDelete(int id)
        {

            using (SqlConnection sqlconn = new SqlConnection(conn))
            {
                string query = "Delete from Reservation where Id = " + id;

                using (SqlCommand sqlcomm = new SqlCommand(query, sqlconn))
                {
                    try
                    {
                        sqlconn.Open();
                        sqlcomm.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                    }
                }

            }

            return RedirectToPage("Reservation");
        }

    }
}
