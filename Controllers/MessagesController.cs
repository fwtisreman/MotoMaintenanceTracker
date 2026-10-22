
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoMaintenanceTracker.Data;
using MotoMaintenanceTracker.Models;

namespace MotoMaintenanceTracker.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display active messages
        public async Task<IActionResult> Index()
        {
            await GenerateMaintenanceReminders();

            var messages = await _context.Messages
                .OrderBy(m => m.IsDismissed)
                .ThenByDescending(m => m.CreatedAt)
                .ToListAsync();

            return View(messages);
        }




        private async Task GenerateMaintenanceReminders()
        {
            var today = DateTime.Today;

            var tasks = await _context.MaintenanceTasks
                .Include(t => t.Motorcycle)
                .Where(t =>
                    !t.IsCompleted &&
                    t.DueDate != null &&
                    t.Motorcycle != null &&
                    !t.Motorcycle.IsDeleted)
                .ToListAsync();

            foreach (var task in tasks)
            {
                var dueDate = task.DueDate!.Value.Date;
                var daysUntilDue = (dueDate - today).Days;

                // Do not generate reminders after the due date.
                if (daysUntilDue < 0)
                    continue;

                string? title = null;
                string? type = null;
                string? stage = null;

                if (daysUntilDue == 7 && !task.SevenDayReminderSent)
                {
                    title = "Maintenance coming up in 7 days";
                    type = "Maintenance";
                    stage = "7-day";
                }
                else if (daysUntilDue == 1 && !task.OneDayReminderSent)
                {
                    title = "Maintenance due tomorrow";
                    type = "Maintenance";
                    stage = "1-day";
                }
                else if (daysUntilDue == 0 && !task.DueDateReminderSent)
                {
                    title = "Maintenance due today";
                    type = "Maintenance";
                    stage = "Due date";
                }

                if (stage == null)
                    continue;

                var motorcycleName =
                    $"{task.Motorcycle!.Brand} {task.Motorcycle.Model}";

                _context.Messages.Add(new Message
                {
                    Title = title!,
                    Content = $"{task.Title} for {motorcycleName} is due on {dueDate:dd MMM yyyy}.",
                    Type = type!,
                    MaintenanceTaskId = task.Id,
                    CreatedAt = DateTime.Now,
                    IsDismissed = false,
                    IsRead = false
                });

                if (stage == "7-day")
                    task.SevenDayReminderSent = true;
                else if (stage == "1-day")
                    task.OneDayReminderSent = true;
                else if (stage == "Due date")
                    task.DueDateReminderSent = true;
            }

            await _context.SaveChangesAsync();
        }




        // Dismiss a message without permanently deleting it
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dismiss(int id, string? returnUrl)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
                return NotFound();

            message.IsDismissed = true;

            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
        }

        // Permanently delete a message
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
                return NotFound();

            _context.Messages.Remove(message);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAll()
        {
            var messages = await _context.Messages.ToListAsync();

            _context.Messages.RemoveRange(messages);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
