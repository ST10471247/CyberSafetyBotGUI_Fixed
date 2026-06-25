using System;
using System.Drawing;
using System.Windows.Forms;

namespace CyberSafetyBotGUI_Fixed
{
    public class Form1 : Form
    {
        private RichTextBox chatDisplay;
        private TextBox inputTextBox;
        private Button sendButton;
        private Button tasksButton;
        private Button quizButton;
        private Button logButton;
        private Label titleLabel;
        private Chatbot chatbot;
        private bool waitingForName = true;

        public Form1()
        {
            // Window settings
            this.Text = "Cybersecurity Awareness Bot - Part 3";
            this.Size = new Size(850, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 35);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Title label
            titleLabel = new Label();
            titleLabel.Text = "  SHIELD - CYBERSECURITY AWARENESS  ";
            titleLabel.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(0, 183, 255);
            titleLabel.BackColor = Color.FromArgb(45, 45, 50);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 55;

            // Chat display
            chatDisplay = new RichTextBox();
            chatDisplay.Location = new Point(15, 75);
            chatDisplay.Size = new Size(805, 430);
            chatDisplay.ReadOnly = true;
            chatDisplay.BackColor = Color.FromArgb(40, 40, 45);
            chatDisplay.ForeColor = Color.White;
            chatDisplay.Font = new Font("Segoe UI", 11);
            chatDisplay.BorderStyle = BorderStyle.None;

            // Input text box
            inputTextBox = new TextBox();
            inputTextBox.Location = new Point(15, 555);
            inputTextBox.Size = new Size(690, 35);
            inputTextBox.Font = new Font("Segoe UI", 12);
            inputTextBox.BackColor = Color.FromArgb(60, 60, 65);
            inputTextBox.ForeColor = Color.White;
            inputTextBox.BorderStyle = BorderStyle.FixedSingle;
            inputTextBox.KeyPress += InputTextBox_KeyPress;
            inputTextBox.Text = "  Type here... ask about passwords, phishing, tasks, quiz";
            inputTextBox.Enter += InputTextBox_Enter;
            inputTextBox.Leave += InputTextBox_Leave;

            // Send button
            sendButton = new Button();
            sendButton.Location = new Point(715, 553);
            sendButton.Size = new Size(105, 40);
            sendButton.Text = "SEND";
            sendButton.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            sendButton.BackColor = Color.FromArgb(0, 120, 215);
            sendButton.ForeColor = Color.White;
            sendButton.FlatStyle = FlatStyle.Flat;
            sendButton.FlatAppearance.BorderSize = 0;
            sendButton.Cursor = Cursors.Hand;
            sendButton.Click += SendButton_Click;

            // Tasks Button
            tasksButton = new Button();
            tasksButton.Location = new Point(15, 600);
            tasksButton.Size = new Size(130, 35);
            tasksButton.Text = "📋 My Tasks";
            tasksButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            tasksButton.FlatStyle = FlatStyle.Flat;
            tasksButton.BackColor = Color.FromArgb(45, 45, 50);
            tasksButton.ForeColor = Color.White;
            tasksButton.FlatAppearance.BorderSize = 0;
            tasksButton.Cursor = Cursors.Hand;
            tasksButton.Click += TasksButton_Click;

            // Quiz Button
            quizButton = new Button();
            quizButton.Location = new Point(155, 600);
            quizButton.Size = new Size(130, 35);
            quizButton.Text = "🎯 Start Quiz";
            quizButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            quizButton.FlatStyle = FlatStyle.Flat;
            quizButton.BackColor = Color.FromArgb(45, 45, 50);
            quizButton.ForeColor = Color.White;
            quizButton.FlatAppearance.BorderSize = 0;
            quizButton.Cursor = Cursors.Hand;
            quizButton.Click += QuizButton_Click;

            // Log Button
            logButton = new Button();
            logButton.Location = new Point(295, 600);
            logButton.Size = new Size(130, 35);
            logButton.Text = "📋 Activity Log";
            logButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            logButton.FlatStyle = FlatStyle.Flat;
            logButton.BackColor = Color.FromArgb(45, 45, 50);
            logButton.ForeColor = Color.White;
            logButton.FlatAppearance.BorderSize = 0;
            logButton.Cursor = Cursors.Hand;
            logButton.Click += LogButton_Click;

            // Add controls to the form
            this.Controls.Add(titleLabel);
            this.Controls.Add(chatDisplay);
            this.Controls.Add(inputTextBox);
            this.Controls.Add(sendButton);
            this.Controls.Add(tasksButton);
            this.Controls.Add(quizButton);
            this.Controls.Add(logButton);

            // ============================================================
            // Setup chatbot - window shows first, then voice plays
            // ============================================================
            chatbot = new Chatbot();
            chatbot.BotMessage += AppendMessage;
            chatbot.Start();

            // Play voice greeting AFTER the window is fully shown
            this.Shown += (s, e) => chatbot.PlayGreeting();
        }

        private void InputTextBox_Enter(object sender, EventArgs e)
        {
            if (inputTextBox.Text == "  Type here... ask about passwords, phishing, tasks, quiz")
            {
                inputTextBox.Text = "";
                inputTextBox.ForeColor = Color.White;
            }
        }

        private void InputTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(inputTextBox.Text))
            {
                inputTextBox.Text = "  Type here... ask about passwords, phishing, tasks, quiz";
                inputTextBox.ForeColor = Color.Gray;
            }
        }

        private void InputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SendButton_Click(sender, e);
                e.Handled = true;
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            string userInput = inputTextBox.Text.Trim();

            if (userInput == "Type here... ask about passwords, phishing, tasks, quiz" || string.IsNullOrEmpty(userInput))
                return;

            AppendMessage($"You: {userInput}");
            inputTextBox.Clear();
            inputTextBox.Focus();

            if (waitingForName)
            {
                waitingForName = false;
                chatbot.SetUserName(userInput);
            }
            else
            {
                chatbot.ProcessInput(userInput);
            }

            chatDisplay.ScrollToCaret();
        }

        private void TasksButton_Click(object sender, EventArgs e)
        {
            AppendMessage($"You: view tasks");
            chatbot.ProcessInput("view tasks");
        }

        private void QuizButton_Click(object sender, EventArgs e)
        {
            AppendMessage($"You: start quiz");
            chatbot.ProcessInput("start quiz");
        }

        private void LogButton_Click(object sender, EventArgs e)
        {
            AppendMessage($"You: activity log");
            chatbot.ProcessInput("activity log");
        }

        private void AppendMessage(string message)
        {
            if (chatDisplay.InvokeRequired)
            {
                chatDisplay.Invoke(new Action(() => AppendMessage(message)));
                return;
            }

            if (message.StartsWith("Bot:"))
            {
                chatDisplay.SelectionColor = Color.FromArgb(0, 183, 255);
                chatDisplay.SelectionFont = new Font("Segoe UI", 11, FontStyle.Bold);
                chatDisplay.AppendText("BOT > ");
                chatDisplay.SelectionFont = new Font("Segoe UI", 11, FontStyle.Regular);
                chatDisplay.SelectionColor = Color.White;
                chatDisplay.AppendText(message.Substring(5) + "\n\n");
            }
            else if (message.StartsWith("You:"))
            {
                chatDisplay.SelectionColor = Color.FromArgb(100, 200, 100);
                chatDisplay.SelectionFont = new Font("Segoe UI", 11, FontStyle.Bold);
                chatDisplay.AppendText("YOU > ");
                chatDisplay.SelectionFont = new Font("Segoe UI", 11, FontStyle.Regular);
                chatDisplay.SelectionColor = Color.White;
                chatDisplay.AppendText(message.Substring(5) + "\n\n");
            }
            else
            {
                chatDisplay.SelectionColor = Color.Gray;
                chatDisplay.AppendText(message + "\n");
            }

            chatDisplay.ScrollToCaret();
        }
    }
}