using Microsoft.AspNetCore.Antiforgery;
using App.Caliset.Controllers;

namespace App.Caliset.Web.Host.Controllers
{
    public class AntiForgeryController : CalisetControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
