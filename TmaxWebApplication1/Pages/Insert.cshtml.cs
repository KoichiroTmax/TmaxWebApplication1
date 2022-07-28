using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Web.Helpers;

namespace TmaxWebApplication1.Pages
{
    public class InsertModel : PageModel
    {
        private ILogger<PrivacyModel> _logger;

        public InsertModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}