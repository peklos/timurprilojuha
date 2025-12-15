using System;
using System.Drawing;
using System.Windows.Forms;
using AdmissionSystem.Database;
using AdmissionSystem.Models;

namespace AdmissionSystem.Forms
{
    public partial class ApplicationForm : Form
    {
        private User currentUser;
        private Specialty specialty;

        private TextBox txtLastName;
        private TextBox txtFirstName;
        private TextBox txtMiddleName;
        private DateTimePicker dtpBirthDate;
        private TextBox txtPassportSeries;
        private TextBox txtPassportNumber;
        private TextBox txtAddress;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private NumericUpDown numExamScore;
        private Label lblSpecialty;
        private Button btnSubmit;
        private Button btnCancel;

        public ApplicationForm(User user, Specialty selectedSpecialty)
        {
            currentUser = user;
            specialty = selectedSpecialty;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(600, 750);
            this.Text = "Подача заявления";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;
            this.AutoScroll = true;

            int yPosition = 20;

            // Заголовок
            Label lblTitle = new Label
            {
                Text = "ПОДАЧА ЗАЯВЛЕНИЯ",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#4caf50"),
                Size = new Size(550, 40),
                Location = new Point(25, yPosition),
                TextAlign = ContentAlignment.MiddleCenter
            };
            yPosition += 50;

            // Специальность
            lblSpecialty = new Label
            {
                Text = $"Специальность: {specialty.Name} ({specialty.Code})\nМинимальный балл: {specialty.MinScore}",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#1976d2"),
                Size = new Size(550, 40),
                Location = new Point(25, yPosition),
                TextAlign = ContentAlignment.TopLeft
            };
            yPosition += 50;

            // Фамилия
            Label lblLastName = CreateLabel("Фамилия:", yPosition);
            txtLastName = CreateTextBox(yPosition + 25);
            yPosition += 70;

            // Имя
            Label lblFirstName = CreateLabel("Имя:", yPosition);
            txtFirstName = CreateTextBox(yPosition + 25);
            yPosition += 70;

            // Отчество
            Label lblMiddleName = CreateLabel("Отчество:", yPosition);
            txtMiddleName = CreateTextBox(yPosition + 25);
            yPosition += 70;

            // Дата рождения
            Label lblBirthDate = CreateLabel("Дата рождения:", yPosition);
            dtpBirthDate = new DateTimePicker
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, yPosition + 25),
                Size = new Size(550, 30),
                Format = DateTimePickerFormat.Short,
                MaxDate = DateTime.Now.AddYears(-14),
                Value = DateTime.Now.AddYears(-17)
            };
            yPosition += 70;

            // Серия паспорта
            Label lblPassportSeries = CreateLabel("Серия паспорта:", yPosition);
            txtPassportSeries = CreateTextBox(yPosition + 25);
            txtPassportSeries.MaxLength = 4;
            yPosition += 70;

            // Номер паспорта
            Label lblPassportNumber = CreateLabel("Номер паспорта:", yPosition);
            txtPassportNumber = CreateTextBox(yPosition + 25);
            txtPassportNumber.MaxLength = 6;
            yPosition += 70;

            // Адрес
            Label lblAddress = CreateLabel("Адрес:", yPosition);
            txtAddress = CreateTextBox(yPosition + 25);
            yPosition += 70;

            // Телефон
            Label lblPhone = CreateLabel("Телефон:", yPosition);
            txtPhone = CreateTextBox(yPosition + 25);
            yPosition += 70;

            // Email
            Label lblEmail = CreateLabel("Email:", yPosition);
            txtEmail = CreateTextBox(yPosition + 25);
            yPosition += 70;

            // Средний балл аттестата
            Label lblExamScore = CreateLabel($"Средний балл аттестата (минимум {specialty.MinScore}):", yPosition);
            numExamScore = new NumericUpDown
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, yPosition + 25),
                Size = new Size(550, 30),
                Minimum = 2.0M,
                Maximum = 5.0M,
                DecimalPlaces = 2,
                Increment = 0.01M,
                Value = (decimal)specialty.MinScore
            };
            yPosition += 70;

            // Кнопки
            btnSubmit = new Button
            {
                Text = "ПОДАТЬ ЗАЯВЛЕНИЕ",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(265, 45),
                Location = new Point(25, yPosition),
                BackColor = ColorTranslator.FromHtml("#4caf50"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSubmit.FlatAppearance.BorderSize = 0;
            btnSubmit.Click += BtnSubmit_Click;

            btnCancel = new Button
            {
                Text = "ОТМЕНА",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(265, 45),
                Location = new Point(310, yPosition),
                BackColor = ColorTranslator.FromHtml("#757575"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            // Добавление контролов
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSpecialty);
            this.Controls.Add(lblLastName);
            this.Controls.Add(txtLastName);
            this.Controls.Add(lblFirstName);
            this.Controls.Add(txtFirstName);
            this.Controls.Add(lblMiddleName);
            this.Controls.Add(txtMiddleName);
            this.Controls.Add(lblBirthDate);
            this.Controls.Add(dtpBirthDate);
            this.Controls.Add(lblPassportSeries);
            this.Controls.Add(txtPassportSeries);
            this.Controls.Add(lblPassportNumber);
            this.Controls.Add(txtPassportNumber);
            this.Controls.Add(lblAddress);
            this.Controls.Add(txtAddress);
            this.Controls.Add(lblPhone);
            this.Controls.Add(txtPhone);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblExamScore);
            this.Controls.Add(numExamScore);
            this.Controls.Add(btnSubmit);
            this.Controls.Add(btnCancel);
        }

        private Label CreateLabel(string text, int yPosition)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, yPosition),
                Size = new Size(550, 20)
            };
        }

        private TextBox CreateTextBox(int yPosition)
        {
            return new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, yPosition),
                Size = new Size(550, 30),
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtPassportSeries.Text) ||
                string.IsNullOrWhiteSpace(txtPassportNumber.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPassportSeries.Text.Length != 4)
            {
                MessageBox.Show("Серия паспорта должна содержать 4 цифры!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPassportNumber.Text.Length != 6)
            {
                MessageBox.Show("Номер паспорта должен содержать 6 цифр!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if ((double)numExamScore.Value < specialty.MinScore)
            {
                MessageBox.Show($"Средний балл аттестата должен быть не менее {specialty.MinScore}!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Models.Application application = new Models.Application
                {
                    UserId = currentUser.Id,
                    SpecialtyId = specialty.Id,
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    MiddleName = txtMiddleName.Text.Trim(),
                    BirthDate = dtpBirthDate.Value,
                    PassportSeries = txtPassportSeries.Text.Trim(),
                    PassportNumber = txtPassportNumber.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    ExamScore = (double)numExamScore.Value
                };

                DatabaseHelper.AddApplication(application);

                MessageBox.Show("Заявление успешно подано!\nОжидайте рассмотрения администратором.",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при подаче заявления: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
