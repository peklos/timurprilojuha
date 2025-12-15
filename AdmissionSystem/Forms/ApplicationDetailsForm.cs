using System;
using System.Drawing;
using System.Windows.Forms;
using AdmissionSystem.Database;
using AdmissionSystem.Models;

namespace AdmissionSystem.Forms
{
    public partial class ApplicationDetailsForm : Form
    {
        private AdmissionSystem.Models.Application application;
        private bool isAdminMode;

        public ApplicationDetailsForm(AdmissionSystem.Models.Application app, bool isAdmin = false)
        {
            application = app;
            isAdminMode = isAdmin;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(600, 700);
            this.Text = "Детали заявления";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = ColorTranslator.FromHtml("#e8f4f8");
            this.Padding = new Padding(20);

            // Заголовок
            Label lblTitle = new Label
            {
                Text = "ПОДРОБНАЯ ИНФОРМАЦИЯ О ЗАЯВЛЕНИИ",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = UI.ModernUIHelper.TextPrimary,
                Size = new Size(550, 40),
                Location = new Point(0, 10),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Основная панель с информацией
            Panel infoPanel = new Panel
            {
                Location = new Point(0, 60),
                Size = new Size(560, 500),
                BackColor = UI.ModernUIHelper.CardBackground,
                Padding = new Padding(20)
            };

            int yPos = 20;
            int labelWidth = 150;
            int valueWidth = 350;

            // ФИО
            AddInfoRow(infoPanel, "ФИО:", application.FullName, ref yPos, labelWidth, valueWidth);
            
            // Специальность
            AddInfoRow(infoPanel, "Специальность:", application.SpecialtyName, ref yPos, labelWidth, valueWidth);
            
            // Дата рождения
            AddInfoRow(infoPanel, "Дата рождения:", application.BirthDate.ToString("dd.MM.yyyy"), ref yPos, labelWidth, valueWidth);
            
            // Паспортные данные
            AddInfoRow(infoPanel, "Паспорт:", $"{application.PassportSeries} {application.PassportNumber}", ref yPos, labelWidth, valueWidth);
            
            // Адрес
            AddInfoRow(infoPanel, "Адрес:", application.Address, ref yPos, labelWidth, valueWidth);
            
            // Телефон
            AddInfoRow(infoPanel, "Телефон:", application.Phone, ref yPos, labelWidth, valueWidth);
            
            // Email
            AddInfoRow(infoPanel, "Email:", application.Email, ref yPos, labelWidth, valueWidth);
            
            // Средний балл
            AddInfoRow(infoPanel, "Средний балл:", application.ExamScore.ToString("F2"), ref yPos, labelWidth, valueWidth);
            
            // Статус
            Color statusColor = application.Status == "Одобрено" ? UI.ModernUIHelper.SuccessColor :
                               application.Status == "Отклонено" ? UI.ModernUIHelper.DangerColor : UI.ModernUIHelper.WarningColor;
            AddInfoRow(infoPanel, "Статус:", application.Status, ref yPos, labelWidth, valueWidth, statusColor);
            
            // Дата подачи
            AddInfoRow(infoPanel, "Дата подачи:", application.SubmittedAt, ref yPos, labelWidth, valueWidth);
            
            // Заметки (только для админа)
            if (!string.IsNullOrEmpty(application.Notes) && isAdminMode)
            {
                AddInfoRow(infoPanel, "Заметки:", application.Notes, ref yPos, labelWidth, valueWidth);
            }

            // Кнопки
            Panel buttonPanel = new Panel
            {
                Location = new Point(0, 570),
                Size = new Size(560, 80),
                BackColor = Color.Transparent
            };

            Button btnClose = UI.ModernUIHelper.CreateGradientButton(
                "ЗАКРЫТЬ",
                new Point(200, 20),
                new Size(160, 45),
                UI.ModernUIHelper.SecondaryAccent,
                ColorTranslator.FromHtml("#003d6b")
            );
            btnClose.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            // Кнопки действий для админа
            if (isAdminMode && application.Status == "На рассмотрении")
            {
                Button btnApprove = UI.ModernUIHelper.CreateGradientButton(
                    "ОДОБРИТЬ",
                    new Point(20, 20),
                    new Size(160, 45),
                    UI.ModernUIHelper.SuccessColor,
                    ColorTranslator.FromHtml("#06a77d")
                );
                btnApprove.Click += (s, e) => ApproveApplication();

                Button btnReject = UI.ModernUIHelper.CreateGradientButton(
                    "ОТКЛОНИТЬ",
                    new Point(380, 20),
                    new Size(160, 45),
                    UI.ModernUIHelper.DangerColor,
                    ColorTranslator.FromHtml("#d7263d")
                );
                btnReject.Click += (s, e) => RejectApplication();

                buttonPanel.Controls.Add(btnApprove);
                buttonPanel.Controls.Add(btnReject);
            }
            else
            {
                btnClose.Location = new Point(200, 20);
            }

            buttonPanel.Controls.Add(btnClose);

            // Добавляем контролы
            this.Controls.Add(lblTitle);
            this.Controls.Add(infoPanel);
            this.Controls.Add(buttonPanel);
        }

        private void AddInfoRow(Panel panel, string label, string value, ref int yPos, int labelWidth, int valueWidth, Color? valueColor = null)
        {
            // Метка
            Label lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = UI.ModernUIHelper.TextSecondary,
                Location = new Point(0, yPos),
                Size = new Size(labelWidth, 25),
                BackColor = Color.Transparent
            };

            // Значение
            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 10),
                ForeColor = valueColor ?? UI.ModernUIHelper.TextPrimary,
                Location = new Point(labelWidth, yPos),
                Size = new Size(valueWidth, 25),
                BackColor = Color.Transparent
            };

            panel.Controls.Add(lblLabel);
            panel.Controls.Add(lblValue);
            yPos += 30;
        }

        private void ApproveApplication()
        {
            if (MessageBox.Show("Одобрить это заявление?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper.UpdateApplicationStatus(application.Id, "Одобрено");
                    application.Status = "Одобрено";
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось одобрить заявление", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RejectApplication()
        {
            if (MessageBox.Show("Отклонить это заявление?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper.UpdateApplicationStatus(application.Id, "Отклонено");
                    application.Status = "Отклонено";
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось отклонить заявление", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
