using System;
using System.Collections.Generic;

namespace CyberSafetyBotGUI_Fixed
{
    public class TaskManager
    {
        private DatabaseHelper db;
        private List<Task> taskHistory;

        public TaskManager()
        {
            db = new DatabaseHelper();
            taskHistory = new List<Task>();
        }

        public string AddTask(string title, string description, string reminderDays = null)
        {
            DateTime? reminderDate = null;
            if (!string.IsNullOrEmpty(reminderDays) && int.TryParse(reminderDays, out int days))
            {
                reminderDate = DateTime.Now.AddDays(days);
            }

            db.AddTask(title, description, reminderDate);
            db.LogActivity("Task Added", $"Title: {title}, Reminder: {reminderDate?.ToString() ?? "None"}");

            string response = $"Task '{title}' added successfully!";
            if (reminderDate.HasValue)
            {
                response += $" I'll remind you in {reminderDays} days.";
            }
            return response;
        }

        public List<Task> GetTasks()
        {
            return db.GetTasks();
        }

        public string DeleteTask(int id)
        {
            db.DeleteTask(id);
            db.LogActivity("Task Deleted", $"Task ID: {id}");
            return "Task deleted successfully.";
        }

        public string CompleteTask(int id)
        {
            db.CompleteTask(id);
            db.LogActivity("Task Completed", $"Task ID: {id}");
            return "Task marked as completed! Great job!";
        }

        public string GetTasksDisplay()
        {
            var tasks = db.GetTasks();
            if (tasks.Count == 0)
            {
                return "No tasks found. Add a task by typing 'add task - [title]'";
            }

            string result = "📋 Your Tasks:\n";
            foreach (var task in tasks)
            {
                string status = task.IsCompleted ? "✅" : "⬜";
                result += $"{status} {task.Title} - {task.Description}";
                if (task.ReminderDate.HasValue)
                {
                    result += $" (Reminder: {task.ReminderDate.Value.ToShortDateString()})";
                }
                result += $" [ID: {task.Id}]\n";
            }
            return result;
        }
    }
}