using System;
using System.Collections.Generic;

namespace CyberSafetyBotGUI_Fixed
{
    public class QuizManager
    {
        private List<QuizQuestion> questions;
        private int currentIndex;
        private int score;
        private bool quizActive;

        public QuizManager()
        {
            LoadQuestions();
            currentIndex = 0;
            score = 0;
            quizActive = false;
        }

        private void LoadQuestions()
        {
            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> {
                        "A) Reply with your password",
                        "B) Delete the email",
                        "C) Report the email as phishing",
                        "D) Ignore it"
                    },
                    CorrectAnswer = 2,
                    Explanation = "Reporting phishing emails helps prevent scams and protects others."
                },
                new QuizQuestion
                {
                    Question = "Which of these is a strong password?",
                    Options = new List<string> {
                        "A) password123",
                        "B) MyBirthday1990",
                        "C) Tr0ub4dor&3",
                        "D) 12345678"
                    },
                    CorrectAnswer = 2,
                    Explanation = "A strong password uses uppercase, lowercase, numbers, and symbols."
                },
                new QuizQuestion
                {
                    Question = "What does 'HTTPS' mean when browsing?",
                    Options = new List<string> {
                        "A) HyperText Transfer Protocol Secure",
                        "B) HyperText Transfer Protocol Standard",
                        "C) High-Tech Transfer Protocol System",
                        "D) None of the above"
                    },
                    CorrectAnswer = 0,
                    Explanation = "HTTPS means your connection to the website is encrypted and secure."
                },
                new QuizQuestion
                {
                    Question = "True or False: You should use the same password for all your accounts.",
                    Options = new List<string> {
                        "A) True",
                        "B) False"
                    },
                    CorrectAnswer = 1,
                    Explanation = "Using the same password everywhere puts all your accounts at risk."
                },
                new QuizQuestion
                {
                    Question = "What is phishing?",
                    Options = new List<string> {
                        "A) A type of computer virus",
                        "B) A scam where attackers trick you into giving personal information",
                        "C) A type of firewall",
                        "D) A security update"
                    },
                    CorrectAnswer = 1,
                    Explanation = "Phishing scams use fake emails or messages to steal your personal information."
                },
                new QuizQuestion
                {
                    Question = "True or False: Public Wi-Fi is always safe to use for banking.",
                    Options = new List<string> {
                        "A) True",
                        "B) False"
                    },
                    CorrectAnswer = 1,
                    Explanation = "Public Wi-Fi can be insecure. Avoid banking or sensitive transactions on it."
                },
                new QuizQuestion
                {
                    Question = "What is Two-Factor Authentication (2FA)?",
                    Options = new List<string> {
                        "A) A type of password",
                        "B) An extra layer of security that requires two forms of verification",
                        "C) A type of malware",
                        "D) A type of firewall"
                    },
                    CorrectAnswer = 1,
                    Explanation = "2FA adds an extra layer of security by requiring a second form of verification."
                },
                new QuizQuestion
                {
                    Question = "True or False: You should click on links from unknown senders.",
                    Options = new List<string> {
                        "A) True",
                        "B) False"
                    },
                    CorrectAnswer = 1,
                    Explanation = "Never click on links from unknown senders. They could lead to phishing sites."
                },
                new QuizQuestion
                {
                    Question = "What should you do if you think you've been hacked?",
                    Options = new List<string> {
                        "A) Change all your passwords immediately",
                        "B) Run a virus scan",
                        "C) Contact your bank",
                        "D) All of the above"
                    },
                    CorrectAnswer = 3,
                    Explanation = "If hacked, change passwords, run virus scan, and contact your bank."
                },
                new QuizQuestion
                {
                    Question = "True or False: It's safe to share your password with friends.",
                    Options = new List<string> {
                        "A) True",
                        "B) False"
                    },
                    CorrectAnswer = 1,
                    Explanation = "Never share your passwords with anyone. Keep them private."
                },
                new QuizQuestion
                {
                    Question = "What is social engineering?",
                    Options = new List<string> {
                        "A) A type of engineering",
                        "B) Manipulating people into revealing confidential information",
                        "C) A type of software",
                        "D) A type of encryption"
                    },
                    CorrectAnswer = 1,
                    Explanation = "Social engineering tricks people into giving up sensitive information."
                }
            };
        }

        public string StartQuiz()
        {
            currentIndex = 0;
            score = 0;
            quizActive = true;
            return GetNextQuestion();
        }

        public string GetNextQuestion()
        {
            if (currentIndex >= questions.Count)
            {
                quizActive = false;
                string result = $"🏆 Quiz Complete!\nYour Score: {score}/{questions.Count}\n";
                if (score >= 9) result += "🌟 Great job! You're a cybersecurity pro!";
                else if (score >= 6) result += "👍 Good effort! Keep learning to stay safe online!";
                else result += "📚 Keep studying! Cybersecurity is important for everyone.";
                return result;
            }

            var q = questions[currentIndex];
            string questionText = $"📝 Question {currentIndex + 1} of {questions.Count}:\n";
            questionText += q.Question + "\n";
            foreach (var option in q.Options)
            {
                questionText += option + "\n";
            }
            return questionText;
        }

        public string SubmitAnswer(string answer)
        {
            if (!quizActive)
                return "The quiz hasn't started. Type 'start quiz' to begin.";

            if (currentIndex >= questions.Count)
                return "Quiz is already complete!";

            var q = questions[currentIndex];
            int selectedIndex = -1;

            // Parse answer (A=0, B=1, C=2, D=3)
            string upper = answer.ToUpper().Trim();
            if (upper == "A") selectedIndex = 0;
            else if (upper == "B") selectedIndex = 1;
            else if (upper == "C") selectedIndex = 2;
            else if (upper == "D") selectedIndex = 3;

            if (selectedIndex == -1)
                return "Invalid answer. Please type A, B, C, or D.";

            bool correct = (selectedIndex == q.CorrectAnswer);
            if (correct) score++;

            string result = correct ? "✅ Correct!" : $"❌ Incorrect. The correct answer was {q.Options[q.CorrectAnswer]}";
            result += $"\n{q.Explanation}\n";

            currentIndex++;
            string next = GetNextQuestion();
            if (next.Contains("Quiz Complete"))
                result += "\n" + next;
            else
                result += "\n" + next;

            return result;
        }

        public bool IsQuizActive() => quizActive;
    }

    public class QuizQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public int CorrectAnswer { get; set; }
        public string Explanation { get; set; }
    }
}