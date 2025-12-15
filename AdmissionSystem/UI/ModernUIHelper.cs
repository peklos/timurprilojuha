using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AdmissionSystem.UI
{
    /// <summary>
    /// Современный UI helper с уникальной темной темой и градиентами
    /// </summary>
    public static class ModernUIHelper
    {
        // Темная цветовая схема
        public static readonly Color DarkBackground = ColorTranslator.FromHtml("#0a0e27");
        public static readonly Color CardBackground = ColorTranslator.FromHtml("#1a1d3a");
        public static readonly Color SidebarBackground = ColorTranslator.FromHtml("#16192e");

        // Акцентные цвета
        public static readonly Color PrimaryAccent = ColorTranslator.FromHtml("#6c5ce7");    // Фиолетовый
        public static readonly Color SecondaryAccent = ColorTranslator.FromHtml("#00cec9");  // Бирюзовый
        public static readonly Color SuccessColor = ColorTranslator.FromHtml("#00b894");     // Зеленый
        public static readonly Color DangerColor = ColorTranslator.FromHtml("#ff7675");      // Красный
        public static readonly Color WarningColor = ColorTranslator.FromHtml("#fdcb6e");     // Желтый

        // Текст
        public static readonly Color TextPrimary = ColorTranslator.FromHtml("#ffffff");
        public static readonly Color TextSecondary = ColorTranslator.FromHtml("#b2bec3");
        public static readonly Color TextMuted = ColorTranslator.FromHtml("#636e72");

        /// <summary>
        /// Создает стильную кнопку с градиентом
        /// </summary>
        public static Button CreateGradientButton(string text, Point location, Size size, Color startColor, Color endColor)
        {
            var button = new Button
            {
                Text = text,
                Location = location,
                Size = size,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = TextPrimary,
                Cursor = Cursors.Hand,
                BackColor = startColor
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = endColor;

            // Рисуем градиент при отрисовке
            button.Paint += (s, e) =>
            {
                var btn = (Button)s;
                using (var brush = new LinearGradientBrush(
                    btn.ClientRectangle,
                    startColor,
                    endColor,
                    LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush, btn.ClientRectangle);
                }

                // Рисуем текст
                TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, btn.ClientRectangle,
                    btn.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            return button;
        }

        /// <summary>
        /// Создает стильное текстовое поле
        /// </summary>
        public static TextBox CreateModernTextBox(Point location, Size size, string placeholder = "")
        {
            var textBox = new TextBox
            {
                Location = location,
                Size = size,
                Font = new Font("Segoe UI", 11),
                BackColor = CardBackground,
                ForeColor = TextPrimary,
                BorderStyle = BorderStyle.None,
                Tag = placeholder
            };

            // Создаем панель-контейнер для границы
            var panel = new Panel
            {
                Location = location,
                Size = new Size(size.Width, size.Height + 10),
                BackColor = SidebarBackground,
                Padding = new Padding(2)
            };

            textBox.Location = new Point(10, 5);
            panel.Controls.Add(textBox);

            return textBox;
        }

        /// <summary>
        /// Создает современную метку
        /// </summary>
        public static Label CreateModernLabel(string text, Point location, int fontSize = 10,
            FontStyle style = FontStyle.Regular, Color? color = null)
        {
            return new Label
            {
                Text = text,
                Location = location,
                Font = new Font("Segoe UI", fontSize, style),
                ForeColor = color ?? TextSecondary,
                AutoSize = true,
                BackColor = Color.Transparent
            };
        }

        /// <summary>
        /// Создает панель-карточку
        /// </summary>
        public static Panel CreateCard(Point location, Size size)
        {
            var panel = new Panel
            {
                Location = location,
                Size = size,
                BackColor = CardBackground,
                Padding = new Padding(20)
            };

            // Добавляем эффект тени через границу
            panel.Paint += (s, e) =>
            {
                var p = (Panel)s;
                using (var pen = new Pen(Color.FromArgb(30, 255, 255, 255), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
                }
            };

            return panel;
        }

        /// <summary>
        /// Создает боковую панель навигации
        /// </summary>
        public static Panel CreateSidebar(Size size)
        {
            var sidebar = new Panel
            {
                Location = new Point(0, 0),
                Size = size,
                BackColor = SidebarBackground,
                Dock = DockStyle.Left
            };

            // Градиент на боковой панели
            sidebar.Paint += (s, e) =>
            {
                using (var brush = new LinearGradientBrush(
                    sidebar.ClientRectangle,
                    ColorTranslator.FromHtml("#16192e"),
                    ColorTranslator.FromHtml("#0a0e27"),
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, sidebar.ClientRectangle);
                }
            };

            return sidebar;
        }

        /// <summary>
        /// Создает кнопку для боковой панели
        /// </summary>
        public static Button CreateSidebarButton(string text, int yPosition, bool isActive = false)
        {
            var button = new Button
            {
                Text = "    " + text,
                Location = new Point(0, yPosition),
                Size = new Size(250, 55),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = isActive ? TextPrimary : TextSecondary,
                BackColor = isActive ? ColorTranslator.FromHtml("#6c5ce7") : Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#2d3561");

            return button;
        }

        /// <summary>
        /// Стилизует DataGridView в темной теме
        /// </summary>
        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = CardBackground;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.EnableHeadersVisualStyles = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowTemplate.Height = 45;

            // Стиль заголовков
            dgv.ColumnHeadersDefaultCellStyle.BackColor = SidebarBackground;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = TextPrimary;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = SidebarBackground;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10);
            dgv.ColumnHeadersHeight = 50;

            // Стиль ячеек
            dgv.DefaultCellStyle.BackColor = CardBackground;
            dgv.DefaultCellStyle.ForeColor = TextSecondary;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.DefaultCellStyle.SelectionBackColor = PrimaryAccent;
            dgv.DefaultCellStyle.SelectionForeColor = TextPrimary;
            dgv.DefaultCellStyle.Padding = new Padding(10, 5, 10, 5);

            dgv.GridColor = ColorTranslator.FromHtml("#2d3561");
        }

        /// <summary>
        /// Создает стильный NumericUpDown
        /// </summary>
        public static NumericUpDown CreateModernNumericUpDown(Point location, Size size,
            decimal min, decimal max, decimal value, int decimalPlaces = 0)
        {
            var numericUpDown = new NumericUpDown
            {
                Location = location,
                Size = size,
                Font = new Font("Segoe UI", 11),
                BackColor = CardBackground,
                ForeColor = TextPrimary,
                BorderStyle = BorderStyle.FixedSingle,
                Minimum = min,
                Maximum = max,
                Value = value,
                DecimalPlaces = decimalPlaces
            };

            return numericUpDown;
        }

        /// <summary>
        /// Создает разделитель
        /// </summary>
        public static Panel CreateDivider(Point location, int width)
        {
            return new Panel
            {
                Location = location,
                Size = new Size(width, 1),
                BackColor = ColorTranslator.FromHtml("#2d3561")
            };
        }

        /// <summary>
        /// Создает карточку заявления
        /// </summary>
        public static Panel CreateApplicationCard(Models.Application app, EventHandler onClick = null)
        {
            var card = new Panel
            {
                Size = new Size(350, 200),
                BackColor = CardBackground,
                Padding = new Padding(15),
                Cursor = Cursors.Hand,
                Tag = app // Сохраняем объект заявления
            };

            // Определяем цвет статуса
            Color statusColor;
            string status = app.Status ?? "На рассмотрении";
            switch (status)
            {
                case "Одобрено":
                    statusColor = SuccessColor;
                    break;
                case "Отклонено":
                    statusColor = DangerColor;
                    break;
                default:
                    statusColor = WarningColor;
                    break;
            }

            // Стиль карточки
            card.Paint += (s, e) =>
            {
                var panel = (Panel)s;
                // Заливка
                using (var brush = new SolidBrush(panel.BackColor))
                {
                    e.Graphics.FillRectangle(brush, panel.ClientRectangle);
                }
                
                // Рамка с цветом статуса сверху
                using (var pen = new Pen(statusColor, 3))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
                }
                
                // Тень
                ControlPaint.DrawBorder(e.Graphics, panel.ClientRectangle,
                    Color.FromArgb(40, 0, 0, 0), 0, ButtonBorderStyle.None,
                    Color.FromArgb(40, 0, 0, 0), 5, ButtonBorderStyle.None,
                    Color.FromArgb(40, 0, 0, 0), 0, ButtonBorderStyle.None,
                    Color.FromArgb(40, 0, 0, 0), 5, ButtonBorderStyle.None);
            };

            // ФИО (крупно)
            string fullName = app.FullName ?? "Не указано";
            Label lblName = new Label
            {
                Text = fullName.Length > 25 ? fullName.Substring(0, 22) + "..." : fullName,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = TextPrimary,
                Location = new Point(10, 10),
                Size = new Size(310, 30),
                BackColor = Color.Transparent
            };

            // Специальность
            string specialtyName = app.SpecialtyName ?? "Не указана";
            Label lblSpecialty = new Label
            {
                Text = "Специальность: " + (specialtyName.Length > 25 ? specialtyName.Substring(0, 22) + "..." : specialtyName),
                Font = new Font("Segoe UI", 9),
                ForeColor = TextSecondary,
                Location = new Point(10, 45),
                Size = new Size(310, 25),
                BackColor = Color.Transparent
            };

            // Балл
            Label lblScore = new Label
            {
                Text = $"Балл: {app.ExamScore:F2}",
                Font = new Font("Segoe UI", 9),
                ForeColor = TextSecondary,
                Location = new Point(10, 70),
                Size = new Size(150, 25),
                BackColor = Color.Transparent
            };

            // Дата
            string submittedAt = app.SubmittedAt ?? "Не указана";
            Label lblDate = new Label
            {
                Text = $"Дата: {submittedAt}",
                Font = new Font("Segoe UI", 9),
                ForeColor = TextMuted,
                Location = new Point(10, 95),
                Size = new Size(310, 25),
                BackColor = Color.Transparent
            };

            // Паспортные данные
            string passportSeries = app.PassportSeries ?? "";
            string passportNumber = app.PassportNumber ?? "";
            Label lblPassport = new Label
            {
                Text = $"Паспорт: {passportSeries} {passportNumber}",
                Font = new Font("Segoe UI", 9),
                ForeColor = TextMuted,
                Location = new Point(10, 120),
                Size = new Size(310, 25),
                BackColor = Color.Transparent
            };

            // Статус (в правом нижнем углу)
            Label lblStatus = new Label
            {
                Text = status.ToUpper(),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = statusColor,
                Location = new Point(170, 155),
                Size = new Size(150, 30),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };

            // Добавляем все элементы на карточку
            card.Controls.Add(lblName);
            card.Controls.Add(lblSpecialty);
            card.Controls.Add(lblScore);
            card.Controls.Add(lblDate);
            card.Controls.Add(lblPassport);
            card.Controls.Add(lblStatus);

            // Обработчик клика
            if (onClick != null)
            {
                card.Click += onClick;
                foreach (Control control in card.Controls)
                {
                    control.Click += onClick;
                    control.Cursor = Cursors.Hand;
                }
            }

            // Эффект при наведении
            card.MouseEnter += (s, e) =>
            {
                card.BackColor = ColorTranslator.FromHtml("#21254d");
                card.Refresh();
            };

            card.MouseLeave += (s, e) =>
            {
                card.BackColor = CardBackground;
                card.Refresh();
            };

            return card;
        }
    }
}
