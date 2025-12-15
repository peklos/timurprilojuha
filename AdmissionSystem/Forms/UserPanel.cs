using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AdmissionSystem.Database;
using AdmissionSystem.Models;
using AdmissionSystem.UI;

namespace AdmissionSystem.Forms
{
    public partial class UserPanel : Form
    {
        private User currentUser;
        private Panel headerPanel;
        private Panel navigationPanel;
        private Panel contentPanel;
        private Panel specialtiesPanel;
        private Panel applicationsPanel;
        private FlowLayoutPanel cardsFlowPanel;
        private DataGridView dgvSpecialties;
        private Button btnSpecialtiesNav;
        private Button btnApplicationsNav;
        private Label lblPageTitle;
        private Button btnRefreshCards;

        public UserPanel(User user)
        {
            currentUser = user;
            InitializeComponent();
            LoadData();
            ShowSpecialtiesPanel();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1500, 900);
            this.Text = "Панель пользователя";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ModernUIHelper.DarkBackground;
            this.DoubleBuffered = true;

            // Верхняя панель заголовка (Header Bar)
            headerPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(1500, 60),
                BackColor = ModernUIHelper.SecondaryAccent
            };

            // Логотип/Заголовок слева
            Label lblLogo = new Label
            {
                Text = "Личный Кабинет",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextPrimary,
                Size = new Size(300, 60),
                Location = new Point(30, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            // Имя пользователя в центре
            Label lblUserName = new Label
            {
                Text = currentUser.FullName,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextPrimary,
                Size = new Size(600, 60),
                Location = new Point(450, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            // Кнопка выхода справа
            Button btnLogout = new Button
            {
                Text = "ВЫХОД",
                Location = new Point(1330, 12),
                Size = new Size(140, 36),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextPrimary,
                BackColor = ModernUIHelper.DangerColor,
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderColor = ModernUIHelper.TextPrimary;
            btnLogout.FlatAppearance.BorderSize = 2;
            btnLogout.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#d7263d");
            btnLogout.Click += (s, e) =>
            {
                this.Close();
            };

            headerPanel.Controls.Add(lblLogo);
            headerPanel.Controls.Add(lblUserName);
            headerPanel.Controls.Add(btnLogout);

            // Панель навигации (горизонтальные вкладки)
            navigationPanel = new Panel
            {
                Location = new Point(0, 60),
                Size = new Size(1500, 40),
                BackColor = ModernUIHelper.DarkBackground
            };

            // Вкладка "Специальности"
            btnSpecialtiesNav = new Button
            {
                Text = "Специальности",
                Location = new Point(40, 4),
                Size = new Size(220, 32),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextPrimary,
                BackColor = ModernUIHelper.PrimaryAccent,
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };
            btnSpecialtiesNav.FlatAppearance.BorderSize = 0;
            btnSpecialtiesNav.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#e85d24");
            btnSpecialtiesNav.Click += (s, e) => ShowSpecialtiesPanel();

            // Вкладка "Мои заявления"
            btnApplicationsNav = new Button
            {
                Text = "Мои заявления",
                Location = new Point(270, 4),
                Size = new Size(220, 32),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextSecondary,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };
            btnApplicationsNav.FlatAppearance.BorderSize = 0;
            btnApplicationsNav.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#d0e8f2");
            btnApplicationsNav.Click += (s, e) => ShowApplicationsPanel();

            navigationPanel.Controls.Add(btnSpecialtiesNav);
            navigationPanel.Controls.Add(btnApplicationsNav);

            // Панель контента (начинается с top:100px)
            contentPanel = new Panel
            {
                Location = new Point(0, 100),
                Size = new Size(1500, 800),
                BackColor = ModernUIHelper.CardBackground
            };

            // Заголовок страницы
            lblPageTitle = new Label
            {
                Text = "ДОСТУПНЫЕ СПЕЦИАЛЬНОСТИ",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextPrimary,
                Size = new Size(1400, 70),
                Location = new Point(50, 20),
                BackColor = Color.Transparent
            };
            contentPanel.Controls.Add(lblPageTitle);

            // Создаем панели для разных разделов
            CreateSpecialtiesPanel();
            CreateApplicationsPanel();

            this.Controls.Add(headerPanel);
            this.Controls.Add(navigationPanel);
            this.Controls.Add(contentPanel);
        }

        private void CreateSpecialtiesPanel()
        {
            specialtiesPanel = new Panel
            {
                Location = new Point(50, 100),
                Size = new Size(1400, 680),
                BackColor = Color.Transparent,
                Visible = true
            };

            // DataGridView для специальностей
            dgvSpecialties = new DataGridView
            {
                Location = new Point(0, 0),
                Size = new Size(1400, 580),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            ModernUIHelper.StyleDataGridView(dgvSpecialties);

            // Панель с кнопками (другое расположение)
            Panel buttonPanel = new Panel
            {
                Location = new Point(0, 600),
                Size = new Size(1400, 80),
                BackColor = Color.Transparent
            };

            Button btnSubmit = ModernUIHelper.CreateGradientButton(
                "ПОДАТЬ ЗАЯВЛЕНИЕ",
                new Point(900, 15),
                new Size(250, 50),
                ModernUIHelper.PrimaryAccent,
                ColorTranslator.FromHtml("#e85d24")
            );
            btnSubmit.Click += (s, e) => SubmitApplication();

            Button btnRefresh = ModernUIHelper.CreateGradientButton(
                "ОБНОВИТЬ",
                new Point(1170, 15),
                new Size(230, 50),
                ModernUIHelper.SecondaryAccent,
                ColorTranslator.FromHtml("#003d6b")
            );
            btnRefresh.Click += (s, e) => LoadSpecialties();

            buttonPanel.Controls.Add(btnSubmit);
            buttonPanel.Controls.Add(btnRefresh);

            specialtiesPanel.Controls.Add(dgvSpecialties);
            specialtiesPanel.Controls.Add(buttonPanel);

            contentPanel.Controls.Add(specialtiesPanel);
        }

        private void CreateApplicationsPanel()
        {
            applicationsPanel = new Panel
            {
                Location = new Point(50, 100),
                Size = new Size(1400, 680),
                BackColor = Color.Transparent,
                Visible = false
            };

            // Заголовок панели заявлений
            Label lblAppsTitle = new Label
            {
                Text = "МОИ ЗАЯВЛЕНИЯ",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextSecondary,
                Location = new Point(0, 0),
                Size = new Size(1400, 50),
                BackColor = Color.Transparent
            };

            // Контейнер для карточек с прокруткой
            Panel scrollPanel = new Panel
            {
                Location = new Point(0, 60),
                Size = new Size(1380, 520),
                BackColor = Color.Transparent,
                AutoScroll = true
            };

            // FlowLayoutPanel для автоматического расположения карточек
            cardsFlowPanel = new FlowLayoutPanel
            {
                Location = new Point(0, 0),
                Size = new Size(1360, 520),
                BackColor = Color.Transparent,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(10)
            };

            scrollPanel.Controls.Add(cardsFlowPanel);

            // Панель с кнопками (другое расположение)
            Panel buttonPanel = new Panel
            {
                Location = new Point(0, 600),
                Size = new Size(1400, 80),
                BackColor = Color.Transparent
            };

            btnRefreshCards = ModernUIHelper.CreateGradientButton(
                "ОБНОВИТЬ",
                new Point(900, 15),
                new Size(250, 50),
                ModernUIHelper.SecondaryAccent,
                ColorTranslator.FromHtml("#003d6b")
            );
            btnRefreshCards.Click += (s, e) => LoadApplicationsCards();

            Button btnDelete = ModernUIHelper.CreateGradientButton(
                "УДАЛИТЬ",
                new Point(1170, 15),
                new Size(230, 50),
                ModernUIHelper.DangerColor,
                ColorTranslator.FromHtml("#d7263d")
            );
            btnDelete.Click += (s, e) => DeleteApplication();

            buttonPanel.Controls.Add(btnRefreshCards);
            buttonPanel.Controls.Add(btnDelete);

            applicationsPanel.Controls.Add(lblAppsTitle);
            applicationsPanel.Controls.Add(scrollPanel);
            applicationsPanel.Controls.Add(buttonPanel);

            contentPanel.Controls.Add(applicationsPanel);
        }

        private void ShowSpecialtiesPanel()
        {
            specialtiesPanel.Visible = true;
            applicationsPanel.Visible = false;

            btnSpecialtiesNav.BackColor = ModernUIHelper.PrimaryAccent;
            btnSpecialtiesNav.ForeColor = ModernUIHelper.TextPrimary;
            btnApplicationsNav.BackColor = Color.Transparent;
            btnApplicationsNav.ForeColor = ModernUIHelper.TextSecondary;

            lblPageTitle.Text = "ДОСТУПНЫЕ СПЕЦИАЛЬНОСТИ";
        }

        private void ShowApplicationsPanel()
        {
            specialtiesPanel.Visible = false;
            applicationsPanel.Visible = true;

            btnSpecialtiesNav.BackColor = Color.Transparent;
            btnSpecialtiesNav.ForeColor = ModernUIHelper.TextSecondary;
            btnApplicationsNav.BackColor = ModernUIHelper.PrimaryAccent;
            btnApplicationsNav.ForeColor = ModernUIHelper.TextPrimary;

            lblPageTitle.Text = "МОИ ЗАЯВЛЕНИЯ";

            // Обновляем карточки при переходе на вкладку
            LoadApplicationsCards();
        }

        private void LoadData()
        {
            try
            {
                LoadSpecialties();
            }
            catch (Exception)
            {
                // Не показываем ошибку пользователю
            }
        }

        private void LoadSpecialties()
        {
            try
            {
                List<Specialty> specialties = DatabaseHelper.GetAllSpecialties();

                if (dgvSpecialties == null) return;

                dgvSpecialties.DataSource = null;
                dgvSpecialties.DataSource = specialties;

                if (dgvSpecialties.Columns.Count > 0)
                {
                    if (dgvSpecialties.Columns.Contains("Id"))
                    {
                        var idColumn = dgvSpecialties.Columns["Id"];
                        if (idColumn != null)
                        {
                            idColumn.HeaderText = "ID";
                        }
                    }

                    if (dgvSpecialties.Columns.Contains("Name"))
                    {
                        var nameColumn = dgvSpecialties.Columns["Name"];
                        if (nameColumn != null)
                            nameColumn.HeaderText = "Название";
                    }

                    if (dgvSpecialties.Columns.Contains("Code"))
                    {
                        var codeColumn = dgvSpecialties.Columns["Code"];
                        if (codeColumn != null)
                            codeColumn.HeaderText = "Код";
                    }

                    if (dgvSpecialties.Columns.Contains("PlacesCount"))
                    {
                        var placesColumn = dgvSpecialties.Columns["PlacesCount"];
                        if (placesColumn != null)
                            placesColumn.HeaderText = "Мест";
                    }

                    if (dgvSpecialties.Columns.Contains("MinScore"))
                    {
                        var scoreColumn = dgvSpecialties.Columns["MinScore"];
                        if (scoreColumn != null)
                            scoreColumn.HeaderText = "Мин. балл";
                    }

                    if (dgvSpecialties.Columns.Contains("Description"))
                    {
                        var descColumn = dgvSpecialties.Columns["Description"];
                        if (descColumn != null)
                            descColumn.HeaderText = "Описание";
                    }
                }
            }
            catch (Exception)
            {
                // Не показываем ошибку пользователю
                if (dgvSpecialties != null)
                {
                    dgvSpecialties.DataSource = null;
                }
            }
        }

        private void LoadApplicationsCards()
        {
            // Очищаем старые карточки
            cardsFlowPanel.Controls.Clear();

            try
            {
                // Получаем заявления пользователя
                List<Models.Application> applications = DatabaseHelper.GetUserApplications(currentUser.Id);

                if (applications.Count == 0)
                {
                    // Сообщение если нет заявлений
                    Label lblNoApps = new Label
                    {
                        Text = "У вас пока нет заявлений.\nПерейдите в раздел 'Специальности' чтобы подать заявление.",
                        Font = new Font("Segoe UI", 12),
                        ForeColor = ModernUIHelper.TextSecondary,
                        Size = new Size(1340, 100),
                        TextAlign = ContentAlignment.MiddleCenter,
                        BackColor = Color.Transparent
                    };
                    cardsFlowPanel.Controls.Add(lblNoApps);
                    return;
                }

                // Создаем карточки для каждого заявления
                foreach (var app in applications)
                {
                    var card = ModernUIHelper.CreateApplicationCard(app, (s, e) =>
                    {
                        // При клике на карточку открываем детали
                        ApplicationDetailsForm detailsForm = new ApplicationDetailsForm(app, false);
                        detailsForm.ShowDialog();

                        // Обновляем карточки после закрытия формы (если статус изменился)
                        if (detailsForm.DialogResult == DialogResult.OK)
                        {
                            LoadApplicationsCards();
                        }
                    });

                    cardsFlowPanel.Controls.Add(card);
                }
            }
            catch (Exception)
            {
                // Не показываем ошибку пользователю
                Label lblError = new Label
                {
                    Text = "Не удалось загрузить заявления",
                    Font = new Font("Segoe UI", 12),
                    ForeColor = ModernUIHelper.TextSecondary,
                    Size = new Size(1340, 100),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent
                };
                cardsFlowPanel.Controls.Add(lblError);
            }
        }

        private void SubmitApplication()
        {
            if (dgvSpecialties.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите специальность!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var specialty = (Specialty)dgvSpecialties.SelectedRows[0].DataBoundItem;
                ApplicationForm form = new ApplicationForm(currentUser, specialty);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadApplicationsCards();
                    ShowApplicationsPanel();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось открыть форму подачи заявления", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteApplication()
        {
            // Находим выбранную карточку
            Panel selectedCard = null;
            foreach (Control control in cardsFlowPanel.Controls)
            {
                if (control is Panel panel && panel.BackColor == ColorTranslator.FromHtml("#f5f9fb"))
                {
                    selectedCard = panel;
                    break;
                }
            }

            if (selectedCard == null)
            {
                MessageBox.Show("Выберите заявление для удаления (кликните на карточку)!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var application = (Models.Application)selectedCard.Tag;

            if (MessageBox.Show("Удалить выбранное заявление?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper.DeleteApplication(application.Id);
                    LoadApplicationsCards();
                    MessageBox.Show("Заявление успешно удалено!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось удалить заявление", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
