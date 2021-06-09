using project8.Data;
using project8.Entities;
using project8.Models.TaskViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace project8.Controllers
{
    public class TaskController : Controller
    {
        private ApplicationDbContext dbContext;

        public TaskController()
        {
            dbContext = new ApplicationDbContext();
        }


        public ActionResult Details(int id)
        {
            var task = dbContext.Tasks.Find(id);

            if (task == null)
            {
                return HttpNotFound();
            }

            var viewModel = new TaskDetailsViewModel
            {
                Name = task.Name,
                Comment = task.Comment,
                CreatedAt = task.CreatedAt,
                CreatedBy = task.CreatedBy,
                Duration = task.Duration,
                FinishedAt = task.FinishedAt,
                IsCompleted = task.IsCompleted
            };

            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(TaskCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var task = new Task
            {
                Name = model.Name,
                CreatedAt = DateTime.Now,
                CreatedBy = User.Identity.Name,
                IsPaused = true,
                IsCompleted = false
            };

            dbContext.Tasks.Add(task);
            dbContext.SaveChanges();

            return RedirectToAction("Details", "Task", new { id = task.Id });
        }

        [HttpPost]
        public ActionResult ResumeTaskWork(int taskId)
        {
            var task = dbContext.Tasks.Find(taskId);

            if (task == null)
            {
                return Json("failed");
            }

            task.ResumedAt = DateTime.Now;
            task.IsPaused = false;
            dbContext.SaveChanges();

            return Json("success");
        }

        [HttpPost]
        public ActionResult PauseTaskWork(int taskId)
        {
            var task = dbContext.Tasks.Find(taskId);

            if (task == null || !task.ResumedAt.HasValue)
            {
                return Json("failed");
            }

            task.Duration += GetLastWorkSessionSeconds(task);
            task.IsPaused = true;
            dbContext.SaveChanges();

            return Json("success");
        }

        [HttpPost]
        public ActionResult FinishTaskWork(int taskId)
        {
            var task = dbContext.Tasks.Find(taskId);

            if (task == null || !task.ResumedAt.HasValue)
            {
                return Json("failed");
            }

            if (!task.IsPaused)
            {
                task.Duration += GetLastWorkSessionSeconds(task);
            }

            task.FinishedAt = DateTime.Now;
            task.IsCompleted = true;
            dbContext.SaveChanges();

            return Json("success");
        }

        private long GetLastWorkSessionSeconds(Task task)
        {
            return (long)Math.Round(DateTime.Now.Subtract(task.ResumedAt.Value).TotalSeconds);
        }
    }
}