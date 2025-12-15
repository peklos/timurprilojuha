using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using AdmissionSystem.Database;
using AdmissionSystem.Models;
using AdmissionSystem.UI;

namespace AdmissionSystem.Forms
{
    public partial class LoginForm : Form
    {
        private TextBox txtLogin;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;
        private LinkLabel linkInstruction;
        private CheckBox chkShowPassword;

        public LoginForm()
        {
            // Инициализируем базу данных в конструкторе
            try
            {
                DatabaseHelper.InitializeDatabase();
            }
            catch (Exception ex)
            {
                // Логируем ошибку, но не показываем пользователю
                Console.WriteLine($"Ошибка инициализации базы данных: {ex.Message}");
            }

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(500, 700);
            this.Text = "Вход в систему";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = ModernUIHelper.CardBackground;
            this.DoubleBuffered = true;

            // Верхняя цветная панель
            Panel topPanel = new Panel
            {
                Size = new Size(500, 180),
                Location = new Point(0, 0),
                BackColor = ModernUIHelper.PrimaryAccent
            };

            topPanel.Paint += (s, e) =>
            {
                using (var brush = new LinearGradientBrush(
                    topPanel.ClientRectangle,
                    ModernUIHelper.PrimaryAccent,
                    ModernUIHelper.SecondaryAccent,
                    LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush, topPanel.ClientRectangle);
                }
            };

            // Иконка входа
            Label lblIcon = new Label
            {
                Text = "👤",
                Font = new Font("Segoe UI", 64),
                ForeColor = Color.White,
                Size = new Size(500, 100),
                Location = new Point(0, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            topPanel.Controls.Add(lblIcon);

            // Заголовок
            Label lblTitle = new Label
            {
                Text = "АВТОРИЗАЦИЯ",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextPrimary,
                Size = new Size(460, 40),
                Location = new Point(20, 200),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Подзаголовок
            Label lblSubtitle = new Label
            {
                Text = "Система приёмной комиссии",
                Font = new Font("Segoe UI", 10),
                ForeColor = ModernUIHelper.TextMuted,
                Size = new Size(460, 25),
                Location = new Point(20, 235),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Логин
            Label lblLoginLabel = new Label
            {
                Text = "Имя пользователя",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextSecondary,
                Size = new Size(460, 20),
                Location = new Point(20, 280),
                BackColor = Color.Transparent
            };

            Panel panelLoginBox = new Panel
            {
                Location = new Point(20, 305),
                Size = new Size(460, 50),
                BackColor = ModernUIHelper.DarkBackground
            };

            txtLogin = new TextBox
            {
                Location = new Point(15, 13),
                Size = new Size(430, 30),
                Font = new Font("Segoe UI", 12),
                BackColor = ModernUIHelper.DarkBackground,
                ForeColor = ModernUIHelper.TextPrimary,
                BorderStyle = BorderStyle.None
            };
            panelLoginBox.Controls.Add(txtLogin);

            // Пароль
            Label lblPasswordLabel = new Label
            {
                Text = "Пароль",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextSecondary,
                Size = new Size(460, 20),
                Location = new Point(20, 375),
                BackColor = Color.Transparent
            };

            Panel panelPasswordBox = new Panel
            {
                Location = new Point(20, 400),
                Size = new Size(460, 50),
                BackColor = ModernUIHelper.DarkBackground
            };

            txtPassword = new TextBox
            {
                Location = new Point(15, 13),
                Size = new Size(430, 30),
                Font = new Font("Segoe UI", 12),
                BackColor = ModernUIHelper.DarkBackground,
                ForeColor = ModernUIHelper.TextPrimary,
                BorderStyle = BorderStyle.None,
                UseSystemPasswordChar = true
            };
            panelPasswordBox.Controls.Add(txtPassword);

            // Показать пароль
            chkShowPassword = new CheckBox
            {
                Text = "Показать пароль",
                Font = new Font("Segoe UI", 9),
                Size = new Size(200, 25),
                Location = new Point(20, 465),
                ForeColor = ModernUIHelper.TextSecondary,
                BackColor = Color.Transparent
            };
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            };

            // Кнопка входа
            btnLogin = new Button
            {
                Text = "ВОЙТИ",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Size = new Size(460, 50),
                Location = new Point(20, 510),
                BackColor = ModernUIHelper.SecondaryAccent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            // Кнопка регистрации
            btnRegister = new Button
            {
                Text = "Создать новый аккаунт",
                Font = new Font("Segoe UI", 10),
                Size = new Size(460, 40),
                Location = new Point(20, 575),
                BackColor = Color.Transparent,
                ForeColor = ModernUIHelper.SecondaryAccent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRegister.FlatAppearance.BorderSize = 1;
            btnRegister.FlatAppearance.BorderColor = ModernUIHelper.SecondaryAccent;
            btnRegister.Click += BtnRegister_Click;

            // Ссылка на инструкцию
            linkInstruction = new LinkLabel
            {
                Text = "📖 Руководство пользователя",
                Font = new Font("Segoe UI", 9),
                Size = new Size(460, 25),
                Location = new Point(20, 635),
                TextAlign = ContentAlignment.MiddleCenter,
                LinkColor = ModernUIHelper.TextMuted,
                ActiveLinkColor = ModernUIHelper.SecondaryAccent,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            linkInstruction.LinkClicked += LinkInstruction_LinkClicked;

            // Добавление элементов
            this.Controls.Add(topPanel);
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(lblLoginLabel);
            this.Controls.Add(panelLoginBox);
            this.Controls.Add(lblPasswordLabel);
            this.Controls.Add(panelPasswordBox);
            this.Controls.Add(chkShowPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnRegister);
            this.Controls.Add(linkInstruction);

            // Enter для входа
            this.AcceptButton = btnLogin;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                User user = DatabaseHelper.GetUser(login, password);

                if (user != null)
                {
                    this.Hide();

                    if (user.Role == "Admin")
                    {
                        AdminPanel adminPanel = new AdminPanel(user);
                        adminPanel.FormClosed += (s, args) => this.Close();
                        adminPanel.Show();
                    }
                    else
                    {
                        UserPanel userPanel = new UserPanel(user);
                        userPanel.FormClosed += (s, args) => this.Close();
                        userPanel.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка подключения к базе данных. Пожалуйста, попробуйте позже.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                RegisterForm registerForm = new RegisterForm();
                registerForm.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось открыть форму регистрации", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LinkInstruction_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Инструкция_пользователя.docx");
            string resourcesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Инструкция_пользователя.docx");

            string instructionPath = File.Exists(rootPath) ? rootPath : resourcesPath;

            if (File.Exists(instructionPath))
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = Path.GetFullPath(instructionPath),
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось открыть инструкцию: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Файл инструкции не найден!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
