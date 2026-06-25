using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace CyberSafetyBotGUI_Fixed
{
    public class DatabaseHelper
    {
        private string connectionString;

        public DatabaseHelper()
        {
            connectionString = "Server=localhost;Database=cybersecurity_bot;Uid=root;Pwd=Maluleka@01;";
        }

        public void AddTask(string title, string description, DateTime? reminderDate)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO tasks (title, description, reminder_date) VALUES (@title, @desc, @reminder)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@desc", description);

                    if (reminderDate.HasValue)
                        cmd.Parameters.AddWithValue("@reminder", reminderDate.Value);
                    else
                        cmd.Parameters.AddWithValue("@reminder", DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Task> GetTasks()
        {
            var tasks = new List<Task>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM tasks ORDER BY created_at DESC";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Task task = new Task();
                        task.Id = reader.GetInt32("id");
                        task.Title = reader.GetString("title");
                        task.Description = reader.GetString("description");
                        task.IsCompleted = reader.GetBoolean("is_completed");

                        if (reader.IsDBNull(reader.GetOrdinal("reminder_date")))
                            task.ReminderDate = null;
                        else
                            task.ReminderDate = reader.GetDateTime("reminder_date");

                        tasks.Add(task);
                    }
                }
            }
            return tasks;
        }

        public void DeleteTask(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM tasks WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CompleteTask(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE tasks SET is_completed = TRUE WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void LogActivity(string action, string details)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO activity_log (action, details) VALUES (@action, @details)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@action", action);
                    cmd.Parameters.AddWithValue("@details", details);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<ActivityLogEntry> GetActivityLog(int limit = 10)
        {
            var logs = new List<ActivityLogEntry>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM activity_log ORDER BY created_at DESC LIMIT @limit";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@limit", limit);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ActivityLogEntry log = new ActivityLogEntry();
                            log.Id = reader.GetInt32("id");
                            log.Action = reader.GetString("action");
                            log.Details = reader.GetString("details");
                            log.CreatedAt = reader.GetDateTime("created_at");
                            logs.Add(log);
                        }
                    }
                }
            }
            return logs;
        }
    }

    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class ActivityLogEntry
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}