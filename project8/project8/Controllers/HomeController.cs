using project8.Data;
using project8.Models.TaskViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace project8.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext dbContext;

        public HomeController()
        {
            dbContext = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var taskNames = dbContext.Tasks
                .Where(t => !t.IsCompleted)
                .Select(t => new TaskBasicViewModel { Id = t.Id, Name = t.Name })
                .ToArray();

            return View(taskNames);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}