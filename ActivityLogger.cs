using System;
using System.Collections.Generic;

namespace CyberSafetyBotGUI_Fixed
{
    public class ActivityLogger
    {
        private DatabaseHelper db;
        private List<string> recentLogs;

        public ActivityLogger()
        {
            db = new DatabaseHelper();
            recentLogs = new List<string>();
        }

        public void Log(string action, string details)
        {
            string logEntry = $"{DateTime.Now:HH:mm:ss} - {action}: {details}";
            recentLogs.Insert(0, logEntry);
            if (recentLogs.Count > 20)
            {
                recentLogs.RemoveAt(recentLogs.Count - 1);
            }
            db.LogActivity(action, details);
        }

        public string GetRecentLogs()
        {
            var dbLogs = db.GetActivityLog(10);
            if (dbLogs.Count == 0)
                return "📋 No recent activity found.";

            string result = "📋 Recent Activity Log:\n";
            int count = 1;
            foreach (var log in dbLogs)
            {
                result += $"{count}. {log.Action} - {log.Details} ({log.CreatedAt:yyyy-MM-dd HH:mm})\n";
                count++;
            }
            return result;
        }

        public string GetFullLog()
        {
            var dbLogs = db.GetActivityLog(100);
            if (dbLogs.Count == 0)
                return "📋 No activity found.";

            string result = "📋 Full Activity History:\n";
            int count = 1;
            foreach (var log in dbLogs)
            {
                result += $"{count}. {log.Action} - {log.Details} ({log.CreatedAt:yyyy-MM-dd HH:mm})\n";
                count++;
            }
            return result;
        }
    }
}