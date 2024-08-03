using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace V4.Pages
{
    [Authorize(Policy = "MustSignIn")]

    public class EditCModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
