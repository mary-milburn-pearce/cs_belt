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

        [Route("dashboard")]
        [HttpGet]
        public IActionResult Index()
        {
            int? currUserId = HttpContext.Session.GetInt32("UserId");
            if (currUserId == null) {
                return Redirect("Login");
            }
            BeltViewModel vm = new BeltViewModel();
            vm.currUser = dbContext.Users.FirstOrDefault(u => u.UserId == currUserId);
            List<Event> events = dbContext.Events
                .Where(d => DateTime.Compare(d.EventDateTime, DateTime.Now)>0)
                .OrderBy(d => d.EventDateTime)
                .Include(c => c.Creator)
                .Include(g => g.Guests)
                .ThenInclude(a => a.Attendee).ToList();
            List<ViewModel> allEvents = new List<ViewModel>();
            foreach (var ev in events) {
                ViewModel newEv = new ViewModel();
                newEv.activity = ev;
                newEv.totalGuests = ev.Guests.Sum(g => g.NumGuests) + 1;
                if (ev.CreatorId == HttpContext.Session.GetInt32("UserId")) {
                    newEv.action = "Delete";
                }
                else {
                    bool going = ev.Guests.Any(g => g.UserId == HttpContext.Session.GetInt32("UserId"));
                    if (going) {
                        newEv.action = "Leave";
                    }
                    else {
                        newEv.action = "Join";
                    }
                }
                allEvents.Add(newEv);
            }
            vm.eventList = allEvents;
            return View("dashboard", vm);
        }
        [Route("new_event")]
        [HttpGet]
        public IActionResult new_event()
        {
            int? currUserId = HttpContext.Session.GetInt32("UserId");
            if (currUserId == null) {
                return Redirect("Login");
            }
           return View("new_event");
        }
        [Route("add_event")]
        [HttpPost]
        public IActionResult add_event(NewEvent activity)
        {
            if (ModelState.IsValid) {
                int? currUserId = HttpContext.Session.GetInt32("UserId");
                if (currUserId == null) {
                    ModelState.AddModelError("Title", "Please log in to add event");
                    return View("new_event", activity);
                }
                DateTime dtNow = DateTime.Now;
                DateTime dtEvent = activity.newActivity.EventDateTime.Date + activity.time.TimeOfDay;
                if (DateTime.Compare(dtNow, dtEvent)>0) {
                    ModelState.AddModelError("time", "Activity must be in the future");
                    return View("new_event", activity);
                }
                activity.newActivity.CreatorId = (int)currUserId;
                activity.newActivity.EventDateTime = dtEvent;
                dbContext.Events.Add(activity.newActivity);
                dbContext.SaveChanges();
                return Redirect("dashboard");
            } else
            {
                return View("new_event", activity);
            }
        }
        [Route("/detail/{eventId:int}")]
        [HttpGet]
        public IActionResult detail(int eventId)
        {
            int? currUserId = HttpContext.Session.GetInt32("UserId");
            if (currUserId == null) {
                return Redirect("/Login");
            }
            Event activity = dbContext.Events
                .Include(response => response.Guests)
                .ThenInclude(guest => guest.Attendee)
                .Include(c => c.Creator)
                .FirstOrDefault(d => d.EventId == eventId);
                
            return View("detail", activity);
        }
        [Route("delete/{eventId:int}")]
        [HttpGet]
        public IActionResult delete(int eventId)
        {
            int? currUserId = HttpContext.Session.GetInt32("UserId");
            if (currUserId == null) {
                return Redirect("Login");
            }
            Event activity = dbContext.Events
                .Include(response => response.Guests)
                .FirstOrDefault(d => d.EventId == eventId);
            if ((int)currUserId == activity.CreatorId) {
                dbContext.Events.Remove(activity);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [Route("accept/{eventId:int}")]
        [HttpGet]
        public IActionResult accept(int eventId)
        {
            int? currUserId = HttpContext.Session.GetInt32("UserId");
            if (currUserId == null) {
                return Redirect("Login");
            }
            Event activity = dbContext.Events.FirstOrDefault(d => d.EventId == eventId);
            User me = dbContext.Users.FirstOrDefault(u => u.UserId == currUserId);
            Response resp = new Response();
            resp.UserId = (int)currUserId;
            resp.Attendee = me;
            resp.EventId = eventId;
            resp.Activity = activity;
            //Random rand = new Random();
            resp.NumGuests = 1;
            dbContext.Responses.Add(resp);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        [Route("decline/{eventId:int}")]
        [HttpGet]
        public IActionResult decline(int eventId)
        {
            int? currUserId = HttpContext.Session.GetInt32("UserId");
            if (currUserId == null) {
                return Redirect("Login");
            }
            Response resp = dbContext.Responses
                .FirstOrDefault(r => (r.EventId == eventId && r.UserId == (int)currUserId));
            if (resp != null) {
                dbContext.Responses.Remove(resp);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}