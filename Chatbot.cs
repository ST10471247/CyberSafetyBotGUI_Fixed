using System;

namespace CyberSafetyBotGUI_Fixed
{
    public class Chatbot
    {
        private AudioService audio;
        private ResponseHandler responder;
        private TaskManager taskManager;
        private QuizManager quizManager;
        private ActivityLogger logger;
        private string userName = "";
        private bool waitingForQuizAnswer = false;

        public event Action<string> BotMessage;

        public Chatbot()
        {
            audio = new AudioService();
            responder = new ResponseHandler();
            taskManager = new TaskManager();
            quizManager = new QuizManager();
            logger = new ActivityLogger();
        }

        public void Start()
        {
            // Voice greeting removed from here - now called separately after window loads
            SendMessage("🛡️ Welcome to the Cybersecurity Awareness Bot!");
            SendMessage("I'm here to help you stay safe online.");
            SendMessage("What is your name?");
            logger.Log("Bot Started", "User launched the application");
        }

        // NEW METHOD - Call this after the window is shown
        public void PlayGreeting()
        {
            audio.PlayGreeting();
        }

        public void SetUserName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                userName = name;
                SendMessage($"Hello {userName}! Nice to meet you.");
                SendMessage("I can help you with:");
                SendMessage("🔹 Cybersecurity tips (passwords, phishing, safe browsing)");
                SendMessage("🔹 Task Management (add, view, complete tasks)");
                SendMessage("🔹 Quiz Game (test your knowledge)");
                SendMessage("🔹 Activity Log (see what I've been doing)");
                SendMessage("Type 'help' to see all available commands.");
                logger.Log("User Identified", $"Name set to {name}");
            }
        }

        public void ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                SendMessage("Please type something!");
                return;
            }

            if (input.ToLower() == "exit" || input.ToLower() == "bye")
            {
                SendMessage($"Goodbye {userName}! Stay safe online! 🔒");
                logger.Log("User Exit", "User closed the application");
                return;
            }

            string lowerInput = input.ToLower();

            // ---- NLP SIMULATION: Keyword Detection ----

            // Task Commands
            if (lowerInput.Contains("add task") || lowerInput.Contains("new task") || lowerInput.Contains("create task"))
            {
                ProcessTaskCommand(input);
                return;
            }

            if (lowerInput.Contains("view task") || lowerInput.Contains("show task") || lowerInput.Contains("my tasks") || lowerInput.Contains("list tasks"))
            {
                SendMessage(taskManager.GetTasksDisplay());
                logger.Log("View Tasks", "User viewed their task list");
                return;
            }

            if (lowerInput.Contains("complete task") || lowerInput.Contains("done task") || lowerInput.Contains("finish task"))
            {
                ProcessCompleteTask(input);
                return;
            }

            if (lowerInput.Contains("delete task") || lowerInput.Contains("remove task"))
            {
                ProcessDeleteTask(input);
                return;
            }

            // Quiz Commands
            if (lowerInput.Contains("start quiz") || lowerInput.Contains("begin quiz") || lowerInput.Contains("play quiz"))
            {
                SendMessage("🎯 Starting the Cybersecurity Quiz!");
                SendMessage(quizManager.StartQuiz());
                waitingForQuizAnswer = true;
                logger.Log("Quiz Started", "User started the cybersecurity quiz");
                return;
            }

            if (waitingForQuizAnswer && (lowerInput == "a" || lowerInput == "b" || lowerInput == "c" || lowerInput == "d"))
            {
                string result = quizManager.SubmitAnswer(input.ToUpper());
                SendMessage(result);
                if (result.Contains("Quiz Complete"))
                {
                    waitingForQuizAnswer = false;
                    logger.Log("Quiz Completed", "User finished the cybersecurity quiz");
                }
                return;
            }

            // Activity Log Commands
            if (lowerInput.Contains("activity log") || lowerInput.Contains("show log") || lowerInput.Contains("what have you done"))
            {
                SendMessage(logger.GetRecentLogs());
                return;
            }

            if (lowerInput.Contains("full log") || lowerInput.Contains("all logs"))
            {
                SendMessage(logger.GetFullLog());
                return;
            }

            // Help Command
            if (lowerInput.Contains("help"))
            {
                ShowHelp();
                return;
            }

            // Greetings
            if (lowerInput.Contains("hello") || lowerInput.Contains("hi") || lowerInput.Contains("hey"))
            {
                SendMessage($"Hi {userName}! How can I help you today?");
                return;
            }

            if (lowerInput.Contains("thank"))
            {
                SendMessage($"You're welcome {userName}! Stay safe online!");
                return;
            }

            // Cybersecurity Topics
            if (lowerInput.Contains("password") || lowerInput.Contains("phishing") ||
                lowerInput.Contains("safe browsing") || lowerInput.Contains("link") ||
                lowerInput.Contains("2fa") || lowerInput.Contains("social engineering") ||
                lowerInput.Contains("malware") || lowerInput.Contains("scam") ||
                lowerInput.Contains("hacked") || lowerInput.Contains("south africa"))
            {
                string response = responder.GetResponse(input, userName);
                SendMessage(response);
                logger.Log("Topic Query", $"User asked about: {input}");
                return;
            }

            // Default response
            SendMessage("I'm not sure I understand. Type 'help' to see what I can do for you.");
        }

        private void ProcessTaskCommand(string input)
        {
            int startIndex = input.IndexOf("-");
            if (startIndex > 0)
            {
                string taskInfo = input.Substring(startIndex + 1).Trim();
                string[] parts = taskInfo.Split('|');
                string title = "General Task";
                string description = taskInfo;
                string reminder = null;

                if (parts.Length == 1)
                {
                    title = parts[0].Trim();
                }
                else if (parts.Length == 2)
                {
                    title = parts[0].Trim();
                    description = parts[1].Trim();
                }
                else if (parts.Length >= 3)
                {
                    title = parts[0].Trim();
                    description = parts[1].Trim();
                    reminder = parts[2].Trim();
                }

                string result = taskManager.AddTask(title, description, reminder);
                SendMessage(result);
                logger.Log("Task Added", $"Title: {title}");
            }
            else
            {
                SendMessage("Please use format: 'add task - [title] | [description] | [days]'");
                SendMessage("Example: 'add task - Update password | Change my password | 7'");
            }
        }

        private void ProcessCompleteTask(string input)
        {
            string[] parts = input.Split(' ');
            bool foundId = false;
            foreach (string part in parts)
            {
                if (int.TryParse(part, out int taskId))
                {
                    string result = taskManager.CompleteTask(taskId);
                    SendMessage(result);
                    foundId = true;
                    break;
                }
            }
            if (!foundId)
            {
                SendMessage("Please specify the task ID. Type 'view tasks' to see IDs.");
            }
        }

        private void ProcessDeleteTask(string input)
        {
            string[] parts = input.Split(' ');
            bool foundId = false;
            foreach (string part in parts)
            {
                if (int.TryParse(part, out int taskId))
                {
                    string result = taskManager.DeleteTask(taskId);
                    SendMessage(result);
                    foundId = true;
                    break;
                }
            }
            if (!foundId)
            {
                SendMessage("Please specify the task ID. Type 'view tasks' to see IDs.");
            }
        }

        private void ShowHelp()
        {
            SendMessage("📚 COMMAND HELP:");
            SendMessage("1. Cybersecurity Topics:");
            SendMessage("   - 'password', 'phishing', 'safe browsing', 'link', '2fa'");
            SendMessage("2. Task Management:");
            SendMessage("   - 'add task - [title] | [description] | [days]'");
            SendMessage("   - 'view tasks'");
            SendMessage("   - 'complete task [ID]'");
            SendMessage("   - 'delete task [ID]'");
            SendMessage("3. Quiz Game:");
            SendMessage("   - 'start quiz'");
            SendMessage("   - Answer with 'a', 'b', 'c', or 'd'");
            SendMessage("4. Activity Log:");
            SendMessage("   - 'activity log'");
            SendMessage("   - 'full log'");
            SendMessage("5. General:");
            SendMessage("   - 'help', 'hello', 'bye'");
        }

        private void SendMessage(string message)
        {
            BotMessage?.Invoke($"Bot: {message}");
        }
    }
}
