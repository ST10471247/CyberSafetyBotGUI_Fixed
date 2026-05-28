using System;
using System.Drawing;
using System.Windows.Forms;

namespace CyberSafetyBotGUI_Fixed
{
    public  class Form1 : Form
    {
        private RichTextBox chatDisplay;
        private TextBox inputTextBox;
        private Button sendButton;
        private Label titleLabel;
        private Chatbot chatbot;
        private bool waitingForName = true;

        public Form1()
        {
            // Window settings
            this.Text = "Cybersecurity Awareness Bot";
            this.Size = new Size(850, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 35);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Title label
            titleLabel = new Label();
            titleLabel.Text = " MY CYBER BOT - STAY SAFE ONLINE! ";
            titleLabel.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(0, 183, 255);
            titleLabel.BackColor = Color.FromArgb(45, 45, 50);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 55;

            // Chat display
            chatDisplay = new RichTextBox();
            chatDisplay.Location = new Point(15, 75);
            chatDisplay.Size = new Size(805, 470);
            chatDisplay.ReadOnly = true;
            chatDisplay.BackColor = Color.FromArgb(40, 40, 45);
            chatDisplay.ForeColor = Color.White;
            chatDisplay.Font = new Font("Segoe UI", 11);
            chatDisplay.BorderStyle = BorderStyle.None;

            // Input text box (no PlaceholderText)
            inputTextBox = new TextBox();
            inputTextBox.Location = new Point(15, 555);
            inputTextBox.Size = new Size(690, 35);
            inputTextBox.Font = new Font("Segoe UI", 12);
            inputTextBox.BackColor = Color.FromArgb(60, 60, 65);
            inputTextBox.ForeColor = Color.White;
            inputTextBox.BorderStyle = BorderStyle.FixedSingle;
            inputTextBox.KeyPress += InputTextBox_KeyPress;
            // Add hint text manually
            inputTextBox.Text = "  Type here... ask about passwords, phishing, safe browsing";
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

            // Add controls
            this.Controls.Add(titleLabel);
            this.Controls.Add(chatDisplay);
            this.Controls.Add(inputTextBox);
            this.Controls.Add(sendButton);

            // Setup chatbot
            chatbot = new Chatbot();
            chatbot.BotMessage += AppendMessage;
            chatbot.Start();
        }

        private void InputTextBox_Enter(object sender, EventArgs e)
        {
            if (inputTextBox.Text == "  Type here... ask about passwords, phishing, safe browsing")
            {
                inputTextBox.Text = "";
                inputTextBox.ForeColor = Color.White;
            }
        }

        private void InputTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(inputTextBox.Text))
            {
                inputTextBox.Text = "  Type here... ask about passwords, phishing, safe browsing";
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

            // Don't send if it's the placeholder text
            if (userInput == "Type here... ask about passwords, phishing, safe browsing" || string.IsNullOrEmpty(userInput))
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
