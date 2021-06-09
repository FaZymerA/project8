using project8.Data;
using project8.Entities;
using project8.Models.TaskViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace project8.Controllers
{
    [Authorize]
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
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"There is no task with id: {taskId}.");
            }

            task.ResumedAt = DateTime.Now;
            task.IsPaused = false;
            dbContext.SaveChanges();

            return Json($"The task with id {taskId} was started.");
        }

        [HttpPost]
        public ActionResult PauseTaskWork(int taskId)
        {
            var task = dbContext.Tasks.Find(taskId);

            if (task == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"There is no task with id: {taskId}.");
            }

            if (!task.ResumedAt.HasValue)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"The task has not been started yet!");
            }

            task.Duration += GetLastWorkSessionSeconds(task);
            task.IsPaused = true;
            dbContext.SaveChanges();

            return Json($"The task with id {taskId} was paused.");
        }

        [HttpPost]
        public ActionResult FinishTaskWork(TaskFinishViewModel model)
        {
            var task = dbContext.Tasks.Find(model.TaskId);

            if (task == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"There is no task with id: {model.TaskId}.");
            }

            if (!task.ResumedAt.HasValue)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"The task has not been started yet!");
            }

            if (!task.IsPaused)
            {
                task.Duration += GetLastWorkSessionSeconds(task);
            }

            task.FinishedAt = DateTime.Now;
            task.IsCompleted = true;
            task.Comment = model.Comment;
            dbContext.SaveChanges();

            return Json($"The task with id {model.TaskId} was finished.");
        }

        private long GetLastWorkSessionSeconds(Task task)
        {
            return (long)Math.Round(DateTime.Now.Subtract(task.ResumedAt.Value).TotalSeconds);
        }
    }
}