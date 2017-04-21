using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers
{
    public class WeddingController : Controller
    {
        private WeddingPlannerContext _context;

        public WeddingController(WeddingPlannerContext context)
        {
            _context = context;
        }


        // GET: /Dashboard/
        [HttpGet]
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("CurUserId") == null ) {
                string Error = "Dont try to steal my cookies";
                TempData["sesErrors"] = Error;

                return RedirectToAction("Index", "User");

            } else {
                int? currUserId = HttpContext.Session.GetInt32("CurUserId");
                ViewBag.sessID = currUserId;
                Users CurUser = _context.Users.SingleOrDefault( user => user.UserId == currUserId );
                ViewBag.CurUser = CurUser;
                

                List<Weddings> CheckAllWeddings = _context.Weddings.Include( p => p.Guests ).ThenInclude( s => s.UsersUser ).ToList();
                foreach( var i in CheckAllWeddings )
                {
                    if(i.date < DateTime.Now)
                    {
                        _context.Weddings.Remove(i);
                        _context.SaveChanges();
                    }
                }
                List<Weddings> allWeddings = _context.Weddings.Include( p => p.Guests ).ThenInclude( s => s.UsersUser ).ToList();
                ViewBag.allWeddings = allWeddings;

                return View();
            }
        }
        // Get: / Add New Wedding to plan Page /
        [HttpGet]
        [Route("NewWedding")]
        public IActionResult NewWedding()
        {
            if(HttpContext.Session.GetInt32("CurUserId") == null ) {
                string Error = "Dont try to steal my cookies";
                TempData["sesErrors"] = Error;

                return RedirectToAction("Index", "User");

            } else {

                if(TempData["dateError"] != null ) 
                {       
                    ViewBag.dateError = TempData["dateError"];
                }
                int? currUserId = HttpContext.Session.GetInt32("CurUserId");
                Users CurUser = _context.Users.SingleOrDefault( user => user.UserId == currUserId );   
                ViewBag.CurUser = CurUser;

                return View();
            }
        }

        // Post: /Create/
        [HttpPost]
        [Route("Create")]
        public IActionResult Create ( WeddingViewModel model )
        {
            int? UserId = HttpContext.Session.GetInt32("CurUserId");
            if(model.date < DateTime.Now) 
            {
                TempData["dateError"] = "Cant create an event with past date!";
                return RedirectToAction("NewWedding");
            }
            if(ModelState.IsValid) {
                Weddings newWedding = new Weddings 
                {
                    wedderOne = model.wedderOne,
                    wedderTwo = model.wedderTwo,
                    date = model.date.Value,
                    weddingAddress = model.weddingAddress,
                    UsersUserId = (int)HttpContext.Session.GetInt32("CurUserId")

                };

                _context.Weddings.Add(newWedding);
                _context.SaveChanges();

                Weddings createdId = _context.Weddings.Where(x => x.wedderOne == model.wedderOne).Where( s => s.wedderTwo == model.wedderTwo).SingleOrDefault();
                int WEDid = createdId.WeddingId;
                ViewBag.WEDid = WEDid;
                System.Console.WriteLine("**************");
                System.Console.WriteLine(WEDid);
                System.Console.WriteLine("**************");
                RSVPs newRSVP = new RSVPs
                {
                    UsersUserId = (int)HttpContext.Session.GetInt32("CurUserId"),
                    WeddingsWeddingId = WEDid

                };
                _context.RSVPs.Add(newRSVP);
                _context.SaveChanges();

                int? currUserId = HttpContext.Session.GetInt32("CurUserId");
                Users CurUser = _context.Users.SingleOrDefault( user => user.UserId == currUserId );

                return RedirectToAction("Dashboard");

            } else {

                int? currUserId = HttpContext.Session.GetInt32("CurUserId");
                Users CurUser = _context.Users.SingleOrDefault( user => user.UserId == currUserId );
                ViewBag.CurUser = CurUser;

                return View("NewWedding");
            }
        }

        // Get: /RSVP/
        [HttpGet]
        [Route("RSVP/{id}")]
        public IActionResult RSVP ( int id)
        {
            int? UserId = HttpContext.Session.GetInt32("CurUserId");
            RSVPs newRSVP = new RSVPs
            {
                WeddingsWeddingId = id,
                UsersUserId = (int)HttpContext.Session.GetInt32("CurUserId")
            };
            _context.RSVPs.Add(newRSVP);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        // Get: /UnRSVP/
        [HttpGet]
        [Route("UnRSVP/{id}")]
        public IActionResult UnRSVP ( int id)
        {
            int? UserId = HttpContext.Session.GetInt32("CurUserId");
            RSVPs curRSVP = _context.RSVPs.SingleOrDefault( x => x.WeddingsWeddingId == id && x.UsersUserId == UserId );
           
            _context.RSVPs.Remove(curRSVP);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        // Get: /Delete the event/
        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete ( int id)
        {
            int? UserId = HttpContext.Session.GetInt32("CurUserId");
            List<RSVPs> curRSVP = _context.RSVPs.Where( x => x.WeddingsWeddingId == id ).ToList();
            foreach( var x in curRSVP) 
            {
                _context.RSVPs.Remove(x);
                _context.SaveChanges();
            }
            Weddings CurWedding = _context.Weddings.SingleOrDefault( wedd => wedd.WeddingId == id );
            _context.Weddings.Remove(CurWedding);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        // Get: / Info /
        [HttpGet]
        [Route("Info/{wedID}")]
        public IActionResult Info( int wedID )
        {
            if(HttpContext.Session.GetInt32("CurUserId") == null ) {
                string Error = "Dont try to steal my cookies";
                TempData["sesErrors"] = Error;

                return RedirectToAction("Index", "User");

            } else {

                int? currUserId = HttpContext.Session.GetInt32("CurUserId");
                Users CurUser = _context.Users.SingleOrDefault( user => user.UserId == currUserId ); 
                ViewBag.CurUser = CurUser;
                //current guests 
                List<RSVPs> allGuests = _context.RSVPs.Where( p => p.WeddingsWeddingId == wedID ).Include( s => s.UsersUser ).ToList();
                ViewBag.allGuests = allGuests;
                // user who created the event
                Weddings selectedWed = _context.Weddings.Where( wedd => wedd.WeddingId == wedID)
                .Include( usr => usr.UsersUser)
                .SingleOrDefault();
                ViewBag.selectedWed = selectedWed;

                return View();
            }
        }


    }
}