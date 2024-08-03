using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using V4.Pages;

namespace V4.Pages
{
    public class NeedServiceListModel : PageModel
    {
        public List<int> l { get; set; }

        public void OnGet()
        {
            l = AddContractModel.needServiceList;

        }

         
    }
}
