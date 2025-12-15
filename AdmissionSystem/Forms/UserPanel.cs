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
        private Panel sidebarPanel;
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

            // Боковая панель навигации
            sidebarPanel = ModernUIHelper.CreateSidebar(new Size(280, 900));

            // Логотип и приветствие
            Label lblLogo = new Label
            {
                Text = "ПК",
                Font = new Font("Segoe UI", 48, FontStyle.Bold),
                ForeColor = ModernUIHelper.SecondaryAccent,
                Size = new Size(280, 80),
                Location = new Point(0, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label lblWelcome = new Label
            {
                Text = "ЛИЧНЫЙ\nКАБИНЕТ",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextPrimary,
                Size = new Size(280, 70),
                Location = new Point(0, 120),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label lblUserName = new Label
            {
                Text = currentUser.FullName,
                Font = new Font("Segoe UI", 11),
                ForeColor = ModernUIHelper.TextSecondary,
                Size = new Size(260, 40),
                Location = new Point(10, 190),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Panel divider = ModernUIHelper.CreateDivider(new Point(20, 240), 240);

            // Кнопки навигации
            btnSpecialtiesNav = ModernUIHelper.CreateSidebarButton("Специальности", 270, true);
            btnSpecialtiesNav.Click += (s, e) => ShowSpecialtiesPanel();

            btnApplicationsNav = ModernUIHelper.CreateSidebarButton("Мои заявления", 340);
            btnApplicationsNav.Click += (s, e) => ShowApplicationsPanel();

            // Кнопка выхода
            Button btnLogout = new Button
            {
                Text = "Выход",
                Location = new Point(0, 800),
                Size = new Size(280, 55),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12),
                ForeColor = ModernUIHelper.DangerColor,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#2d3561");
            btnLogout.Click += (s, e) =>
            {
                this.Close();
            };

            sidebarPanel.Controls.Add(lblLogo);
            sidebarPanel.Controls.Add(lblWelcome);
            sidebarPanel.Controls.Add(lblUserName);
            sidebarPanel.Controls.Add(divider);
            sidebarPanel.Controls.Add(btnSpecialtiesNav);
            sidebarPanel.Controls.Add(btnApplicationsNav);
            sidebarPanel.Controls.Add(btnLogout);

            // Панель контента
            contentPanel = new Panel
            {
                Location = new Point(280, 0),
                Size = new Size(1220, 900),
                BackColor = ModernUIHelper.CardBackground
            };

            // Заголовок страницы
            lblPageTitle = new Label
            {
                Text = "ДОСТУПНЫЕ СПЕЦИАЛЬНОСТИ",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextPrimary,
                Size = new Size(1200, 60),
                Location = new Point(40, 30),
                BackColor = Color.Transparent
            };
            contentPanel.Controls.Add(lblPageTitle);

            // Создаем панели для разных разделов
            CreateSpecialtiesPanel();
            CreateApplicationsPanel();

            this.Controls.Add(sidebarPanel);
            this.Controls.Add(contentPanel);
        }

        private void CreateSpecialtiesPanel()
        {
            specialtiesPanel = new Panel
            {
                Location = new Point(40, 110),
                Size = new Size(1160, 750),
                BackColor = Color.Transparent,
                Visible = true
            };

            // DataGridView для специальностей
            dgvSpecialties = new DataGridView
            {
                Location = new Point(0, 70),
                Size = new Size(1160, 550),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            ModernUIHelper.StyleDataGridView(dgvSpecialties);

            // Панель с кнопками
            Panel buttonPanel = new Panel
            {
                Location = new Point(0, 640),
                Size = new Size(1160, 80),
                BackColor = Color.Transparent
            };

            Button btnSubmit = ModernUIHelper.CreateGradientButton(
                "ПОДАТЬ ЗАЯВЛЕНИЕ",
                new Point(0, 10),
                new Size(280, 50),
                ModernUIHelper.PrimaryAccent,
                ColorTranslator.FromHtml("#5f4dd4")
            );
            btnSubmit.Click += (s, e) => SubmitApplication();

            Button btnRefresh = ModernUIHelper.CreateGradientButton(
                "ОБНОВИТЬ",
                new Point(300, 10),
                new Size(220, 50),
                ModernUIHelper.SecondaryAccent,
                ColorTranslator.FromHtml("#00b5ad")
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
                Location = new Point(40, 110),
                Size = new Size(1160, 750),
                BackColor = Color.Transparent,
                Visible = false
            };

            // Заголовок панели заявлений
            Label lblAppsTitle = new Label
            {
                Text = "МОИ ЗАЯВЛЕНИЯ",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextSecondary,
                Location = new Point(0, 10),
                Size = new Size(1160, 40),
                BackColor = Color.Transparent
            };

            // Контейнер для карточек с прокруткой
            Panel scrollPanel = new Panel
            {
                Location = new Point(0, 60),
                Size = new Size(1140, 550),
                BackColor = Color.Transparent,
                AutoScroll = true
            };

            // FlowLayoutPanel для автоматического расположения карточек
            cardsFlowPanel = new FlowLayoutPanel
            {
                Location = new Point(0, 0),
                Size = new Size(1120, 550),
                BackColor = Color.Transparent,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(10)
            };

            scrollPanel.Controls.Add(cardsFlowPanel);

            // Панель с кнопками
            Panel buttonPanel = new Panel
            {
                Location = new Point(0, 640),
                Size = new Size(1160, 80),
                BackColor = Color.Transparent
            };

            Button btnDelete = ModernUIHelper.CreateGradientButton(
                "УДАЛИТЬ",
                new Point(0, 10),
                new Size(220, 50),
                ModernUIHelper.DangerColor,
                ColorTranslator.FromHtml("#e66565")
            );
            btnDelete.Click += (s, e) => DeleteApplication();

            btnRefreshCards = ModernUIHelper.CreateGradientButton(
                "ОБНОВИТЬ",
                new Point(240, 10),
                new Size(220, 50),
                ModernUIHelper.SecondaryAccent,
                ColorTranslator.FromHtml("#00b5ad")
            );
            btnRefreshCards.Click += (s, e) => LoadApplicationsCards();

            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnRefreshCards);

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
                        Size = new Size(1100, 100),
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
                    Size = new Size(1100, 100),
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
                if (control is Panel panel && panel.BackColor == ColorTranslator.FromHtml("#21254d"))
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
