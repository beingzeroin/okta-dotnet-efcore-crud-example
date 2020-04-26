using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace V_Okta.Controllers
{
    public class UserController : Controller
    {
        Data.VoktaContext _context;

        public UserController(Data.VoktaContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            var user = HttpContext.User.Claims;
            return RedirectToAction("Index", "Movies", null);
        }
    }
}