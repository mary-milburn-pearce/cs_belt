using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cs_belt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace cs_belt.Controllers
{
    public class BeltController : Controller
    {
        private Context dbContext;

        public BeltController(Context context) 
        {
            dbContext = context;
        }

        [Route("belt")]
        [HttpGet]
        public IActionResult Belt()
        {
            int? currUserId = HttpContext.Session.GetInt32("UserId");
            if (currUserId == null) {
                return Redirect("/");
            }
            // WallViewModel vm = new WallViewModel();
            // vm.messages = dbContext.Messages
            //     .Include(m => m.user)
            //     .Include(c => c.Comments)
            //     .ThenInclude(cu => cu.user)
            //     .OrderByDescending(d => d.CreatedAt).ToList();
            // vm.currUser = dbContext.Users.FirstOrDefault(u => u.UserId == currUserId);
            // return View("Wall", vm);
            return View ("Belt");
        }
    }
}