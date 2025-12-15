using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AdmissionSystem.Database;
using AdmissionSystem.Models;
using AdmissionSystem.UI;

namespace AdmissionSystem.Forms
{
    public partial class RegisterForm : Form
    {
        private TextBox txtLogin;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private TextBox txtFullName;
        private Button btnRegister;
        private Button btnCancel;
        private CheckBox chkShowPassword;

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(700, 550);
            this.Text = "Регистрация";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = ModernUIHelper.DarkBackground;
            this.DoubleBuffered = true;

            // Верхняя декоративная панель
            Panel topPanel = new Panel
            {
                Size = new Size(700, 120),
                Location = new Point(0, 0),
                BackColor = ModernUIHelper.SuccessColor
            };

            Label lblIcon = new Label
            {
                Text = "👥",
                Font = new Font("Segoe UI", 48),
                ForeColor = Color.White,
                Size = new Size(700, 80),
                Location = new Point(0, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            topPanel.Controls.Add(lblIcon);

            // Заголовок
            Label lblTitle = new Label
            {
                Text = "РЕГИСТРАЦИЯ НОВОГО ПОЛЬЗОВАТЕЛЯ",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextPrimary,
                Size = new Size(660, 35),
                Location = new Point(20, 140),
                BackColor = Color.Transparent
            };

            // Двухколоночная разметка
            int leftColumnX = 20;
            int rightColumnX = 360;
            int columnWidth = 320;
            int yPos = 190;

            // ФИО (верхняя строка, на всю ширину)
            Label lblFullName = new Label
            {
                Text = "Полное имя",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextSecondary,
                Size = new Size(660, 20),
                Location = new Point(20, yPos),
                BackColor = Color.Transparent
            };

            Panel panelFullNameBox = new Panel
            {
                Location = new Point(20, yPos + 25),
                Size = new Size(660, 45),
                BackColor = ModernUIHelper.CardBackground
            };

            txtFullName = new TextBox
            {
                Location = new Point(15, 11),
                Size = new Size(630, 30),
                Font = new Font("Segoe UI", 11),
                BackColor = ModernUIHelper.CardBackground,
                ForeColor = ModernUIHelper.TextPrimary,
                BorderStyle = BorderStyle.None
            };
            panelFullNameBox.Controls.Add(txtFullName);

            yPos += 90;

            // Левая колонка - Логин
            Label lblLogin = new Label
            {
                Text = "Логин",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextSecondary,
                Size = new Size(columnWidth, 20),
                Location = new Point(leftColumnX, yPos),
                BackColor = Color.Transparent
            };

            Panel panelLoginBox = new Panel
            {
                Location = new Point(leftColumnX, yPos + 25),
                Size = new Size(columnWidth, 45),
                BackColor = ModernUIHelper.CardBackground
            };

            txtLogin = new TextBox
            {
                Location = new Point(15, 11),
                Size = new Size(290, 30),
                Font = new Font("Segoe UI", 11),
                BackColor = ModernUIHelper.CardBackground,
                ForeColor = ModernUIHelper.TextPrimary,
                BorderStyle = BorderStyle.None
            };
            panelLoginBox.Controls.Add(txtLogin);

            // Правая колонка - Показать пароли
            chkShowPassword = new CheckBox
            {
                Text = "Показать пароли",
                Font = new Font("Segoe UI", 9),
                Size = new Size(columnWidth, 25),
                Location = new Point(rightColumnX, yPos + 40),
                ForeColor = ModernUIHelper.TextSecondary,
                BackColor = Color.Transparent
            };
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
                txtConfirmPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            };

            yPos += 85;

            // Левая колонка - Пароль
            Label lblPassword = new Label
            {
                Text = "Пароль",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextSecondary,
                Size = new Size(columnWidth, 20),
                Location = new Point(leftColumnX, yPos),
                BackColor = Color.Transparent
            };

            Panel panelPasswordBox = new Panel
            {
                Location = new Point(leftColumnX, yPos + 25),
                Size = new Size(columnWidth, 45),
                BackColor = ModernUIHelper.CardBackground
            };

            txtPassword = new TextBox
            {
                Location = new Point(15, 11),
                Size = new Size(290, 30),
                Font = new Font("Segoe UI", 11),
                BackColor = ModernUIHelper.CardBackground,
                ForeColor = ModernUIHelper.TextPrimary,
                BorderStyle = BorderStyle.None,
                UseSystemPasswordChar = true
            };
            panelPasswordBox.Controls.Add(txtPassword);

            // Правая колонка - Подтверждение пароля
            Label lblConfirmPassword = new Label
            {
                Text = "Повторите пароль",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextSecondary,
                Size = new Size(columnWidth, 20),
                Location = new Point(rightColumnX, yPos),
                BackColor = Color.Transparent
            };

            Panel panelConfirmPasswordBox = new Panel
            {
                Location = new Point(rightColumnX, yPos + 25),
                Size = new Size(columnWidth, 45),
                BackColor = ModernUIHelper.CardBackground
            };

            txtConfirmPassword = new TextBox
            {
                Location = new Point(15, 11),
                Size = new Size(290, 30),
                Font = new Font("Segoe UI", 11),
                BackColor = ModernUIHelper.CardBackground,
                ForeColor = ModernUIHelper.TextPrimary,
                BorderStyle = BorderStyle.None,
                UseSystemPasswordChar = true
            };
            panelConfirmPasswordBox.Controls.Add(txtConfirmPassword);

            yPos += 90;

            // Кнопки
            btnRegister = new Button
            {
                Text = "ЗАРЕГИСТРИРОВАТЬСЯ",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(660, 50),
                Location = new Point(20, yPos),
                BackColor = ModernUIHelper.SuccessColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;

            btnCancel = new Button
            {
                Text = "Отмена",
                Font = new Font("Segoe UI", 10),
                Size = new Size(660, 40),
                Location = new Point(20, yPos + 60),
                BackColor = Color.Transparent,
                ForeColor = ModernUIHelper.TextMuted,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();

            // Добавление элементов
            this.Controls.Add(topPanel);
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblFullName);
            this.Controls.Add(panelFullNameBox);
            this.Controls.Add(lblLogin);
            this.Controls.Add(panelLoginBox);
            this.Controls.Add(lblPassword);
            this.Controls.Add(panelPasswordBox);
            this.Controls.Add(lblConfirmPassword);
            this.Controls.Add(panelConfirmPasswordBox);
            this.Controls.Add(chkShowPassword);
            this.Controls.Add(btnRegister);
            this.Controls.Add(btnCancel);

            // Enter для регистрации
            this.AcceptButton = btnRegister;
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // Валидация
            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Введите ваше ФИО!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Введите логин!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (login.Length < 3)
            {
                MessageBox.Show("Логин должен содержать минимум 3 символа!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите пароль!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 4)
            {
                MessageBox.Show("Пароль должен содержать минимум 4 символа!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка уникальности логина
            if (DatabaseHelper.UserExists(login))
            {
                MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                User newUser = new User
                {
                    Login = login,
                    Password = password,
                    FullName = fullName,
                    Role = "User"
                };

                DatabaseHelper.RegisterUser(newUser);

                MessageBox.Show("Регистрация прошла успешно!\nТеперь вы можете войти в систему.",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
