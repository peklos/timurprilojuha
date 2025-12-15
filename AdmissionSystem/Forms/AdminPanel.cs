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
    public partial class AdminPanel : Form
    {
        private User currentUser;
        private Panel headerPanel;
        private Panel navigationPanel;
        private Panel contentPanel;
        private Panel applicationsPanel;
        private Panel specialtiesPanel;
        private Panel usersPanel;
        private FlowLayoutPanel appsCardsPanel;
        private DataGridView dgvUsers;
        private DataGridView dgvSpecialties;
        private Button btnApplicationsNav;
        private Button btnSpecialtiesNav;
        private Button btnUsersNav;
        private Label lblPageTitle;
        private Panel selectedApplicationCard;

        public AdminPanel(User user)
        {
            currentUser = user;
            InitializeComponent();
            LoadData();
            ShowApplicationsPanel();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1500, 900);
            this.Text = "Панель администратора";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ModernUIHelper.DarkBackground;
            this.DoubleBuffered = true;

            // Верхняя панель заголовка (Header Bar)
            headerPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(1500, 50),
                BackColor = ModernUIHelper.SidebarBackground
            };

            // Логотип/Заголовок слева
            Label lblLogo = new Label
            {
                Text = "Панель Управления",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = ModernUIHelper.PrimaryAccent,
                Size = new Size(350, 50),
                Location = new Point(30, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            // Имя пользователя в центре
            Label lblUserName = new Label
            {
                Text = currentUser.FullName,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = ModernUIHelper.TextPrimary,
                Size = new Size(400, 50),
                Location = new Point(550, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            // Кнопка выхода справа
            Button btnLogout = new Button
            {
                Text = "ВЫХОД",
                Location = new Point(1330, 10),
                Size = new Size(140, 30),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ModernUIHelper.DangerColor,
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#e63946");
            btnLogout.Click += (s, e) =>
            {
                this.Close();
            };

            headerPanel.Controls.Add(lblLogo);
            headerPanel.Controls.Add(lblUserName);
            headerPanel.Controls.Add(btnLogout);

            // Панель горизонтальной навигации (Navigation Bar)
            navigationPanel = new Panel
            {
                Location = new Point(0, 50),
                Size = new Size(1500, 50),
                BackColor = ModernUIHelper.CardBackground
            };

            // Кнопки навигации (табы)
            btnApplicationsNav = CreateTopNavButton("Заявления", 0);
            btnApplicationsNav.Click += (s, e) => ShowApplicationsPanel();

            btnSpecialtiesNav = CreateTopNavButton("Специальности", 250);
            btnSpecialtiesNav.Click += (s, e) => ShowSpecialtiesPanel();

            btnUsersNav = CreateTopNavButton("Пользователи", 500);
            btnUsersNav.Click += (s, e) => ShowUsersPanel();

            navigationPanel.Controls.Add(btnApplicationsNav);
            navigationPanel.Controls.Add(btnSpecialtiesNav);
            navigationPanel.Controls.Add(btnUsersNav);

            // Панель контента - теперь начинается с top:100px
            contentPanel = new Panel
            {
                Location = new Point(0, 100),
                Size = new Size(1500, 800),
                BackColor = ModernUIHelper.DarkBackground,
                AutoScroll = true
            };

            // Заголовок страницы
            lblPageTitle = new Label
            {
                Text = "УПРАВЛЕНИЕ ЗАЯВЛЕНИЯМИ",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextPrimary,
                Size = new Size(1400, 60),
                Location = new Point(50, 20),
                BackColor = Color.Transparent
            };
            contentPanel.Controls.Add(lblPageTitle);

            // Создаем панели для разных разделов
            CreateApplicationsPanel();
            CreateSpecialtiesPanel();
            CreateUsersPanel();

            this.Controls.Add(headerPanel);
            this.Controls.Add(navigationPanel);
            this.Controls.Add(contentPanel);
        }

        private Button CreateTopNavButton(string text, int x)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(x, 0),
                Size = new Size(250, 50),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextSecondary,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#e8f4f8");
            return btn;
        }

        private void CreateApplicationsPanel()
        {
            applicationsPanel = new Panel
            {
                Location = new Point(50, 100),
                Size = new Size(1400, 670),
                BackColor = Color.Transparent,
                Visible = true
            };

            // Заголовок панели заявлений
            Label lblAppsTitle = new Label
            {
                Text = "ВСЕ ЗАЯВЛЕНИЯ",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextSecondary,
                Location = new Point(0, 0),
                Size = new Size(1400, 40),
                BackColor = Color.Transparent
            };

            // Контейнер для карточек с прокруткой
            Panel scrollPanel = new Panel
            {
                Location = new Point(0, 50),
                Size = new Size(1400, 470),
                BackColor = Color.Transparent,
                AutoScroll = true
            };

            // FlowLayoutPanel для автоматического расположения карточек
            appsCardsPanel = new FlowLayoutPanel
            {
                Location = new Point(0, 0),
                Size = new Size(1380, 470),
                BackColor = Color.Transparent,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(5)
            };

            scrollPanel.Controls.Add(appsCardsPanel);

            // Панель с кнопками действий - изменен дизайн расположения
            Panel buttonPanel = new Panel
            {
                Location = new Point(0, 530),
                Size = new Size(1400, 130),
                BackColor = ModernUIHelper.CardBackground
            };

            // Верхний ряд кнопок
            Button btnApprove = ModernUIHelper.CreateGradientButton(
                "ОДОБРИТЬ ЗАЯВЛЕНИЕ",
                new Point(20, 15),
                new Size(280, 50),
                ModernUIHelper.SuccessColor,
                ColorTranslator.FromHtml("#06a77d")
            );
            btnApprove.Click += (s, e) => ChangeApplicationStatus("Одобрено");

            Button btnReject = ModernUIHelper.CreateGradientButton(
                "ОТКЛОНИТЬ ЗАЯВЛЕНИЕ",
                new Point(320, 15),
                new Size(280, 50),
                ModernUIHelper.DangerColor,
                ColorTranslator.FromHtml("#d7263d")
            );
            btnReject.Click += (s, e) => ChangeApplicationStatus("Отклонено");

            Button btnRefresh = ModernUIHelper.CreateGradientButton(
                "ОБНОВИТЬ СПИСОК",
                new Point(620, 15),
                new Size(280, 50),
                ModernUIHelper.SecondaryAccent,
                ColorTranslator.FromHtml("#003d6b")
            );
            btnRefresh.Click += (s, e) => LoadApplicationsCards();

            // Нижний ряд
            Button btnDelete = ModernUIHelper.CreateGradientButton(
                "УДАЛИТЬ ЗАЯВЛЕНИЕ",
                new Point(20, 70),
                new Size(280, 50),
                ColorTranslator.FromHtml("#7a7a7a"),
                ColorTranslator.FromHtml("#6a6a6a")
            );
            btnDelete.Click += (s, e) => DeleteApplication();

            // Фильтр справа внизу
            Label lblFilter = new Label
            {
                Text = "ФИЛЬТР:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextSecondary,
                Location = new Point(950, 80),
                Size = new Size(100, 30),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            ComboBox cmbFilter = new ComboBox
            {
                Location = new Point(1050, 78),
                Size = new Size(200, 40),
                Font = new Font("Segoe UI", 11),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            cmbFilter.Items.AddRange(new object[] { "Все", "На рассмотрении", "Одобрено", "Отклонено" });
            cmbFilter.SelectedIndex = 0;
            cmbFilter.SelectedIndexChanged += (s, e) => LoadApplicationsCards();

            buttonPanel.Controls.Add(btnApprove);
            buttonPanel.Controls.Add(btnReject);
            buttonPanel.Controls.Add(btnRefresh);
            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(lblFilter);
            buttonPanel.Controls.Add(cmbFilter);

            applicationsPanel.Controls.Add(lblAppsTitle);
            applicationsPanel.Controls.Add(scrollPanel);
            applicationsPanel.Controls.Add(buttonPanel);

            contentPanel.Controls.Add(applicationsPanel);
        }

        private void CreateSpecialtiesPanel()
        {
            specialtiesPanel = new Panel
            {
                Location = new Point(50, 100),
                Size = new Size(1400, 670),
                BackColor = Color.Transparent,
                Visible = false
            };

            // DataGridView для специальностей
            dgvSpecialties = new DataGridView
            {
                Location = new Point(0, 20),
                Size = new Size(1400, 530),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            ModernUIHelper.StyleDataGridView(dgvSpecialties);

            // Панель с кнопками - новый дизайн
            Panel buttonPanel = new Panel
            {
                Location = new Point(0, 560),
                Size = new Size(1400, 100),
                BackColor = ModernUIHelper.CardBackground
            };

            Button btnAdd = ModernUIHelper.CreateGradientButton(
                "ДОБАВИТЬ СПЕЦИАЛЬНОСТЬ",
                new Point(50, 25),
                new Size(300, 50),
                ModernUIHelper.SuccessColor,
                ColorTranslator.FromHtml("#06a77d")
            );
            btnAdd.Click += (s, e) => AddSpecialty();

            Button btnEdit = ModernUIHelper.CreateGradientButton(
                "РЕДАКТИРОВАТЬ",
                new Point(380, 25),
                new Size(300, 50),
                ModernUIHelper.WarningColor,
                ColorTranslator.FromHtml("#f77f00")
            );
            btnEdit.Click += (s, e) => EditSpecialty();

            Button btnDelete = ModernUIHelper.CreateGradientButton(
                "УДАЛИТЬ",
                new Point(710, 25),
                new Size(300, 50),
                ModernUIHelper.DangerColor,
                ColorTranslator.FromHtml("#d7263d")
            );
            btnDelete.Click += (s, e) => DeleteSpecialty();

            Button btnRefresh = ModernUIHelper.CreateGradientButton(
                "ОБНОВИТЬ",
                new Point(1040, 25),
                new Size(300, 50),
                ModernUIHelper.SecondaryAccent,
                ColorTranslator.FromHtml("#003d6b")
            );
            btnRefresh.Click += (s, e) => LoadSpecialties();

            buttonPanel.Controls.Add(btnAdd);
            buttonPanel.Controls.Add(btnEdit);
            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnRefresh);

            specialtiesPanel.Controls.Add(dgvSpecialties);
            specialtiesPanel.Controls.Add(buttonPanel);

            contentPanel.Controls.Add(specialtiesPanel);
        }

        private void CreateUsersPanel()
        {
            usersPanel = new Panel
            {
                Location = new Point(50, 100),
                Size = new Size(1400, 670),
                BackColor = Color.Transparent,
                Visible = false
            };

            // DataGridView для пользователей
            dgvUsers = new DataGridView
            {
                Location = new Point(0, 20),
                Size = new Size(1400, 530),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            ModernUIHelper.StyleDataGridView(dgvUsers);

            // Панель с кнопками - новый дизайн
            Panel buttonPanel = new Panel
            {
                Location = new Point(0, 560),
                Size = new Size(1400, 100),
                BackColor = ModernUIHelper.CardBackground
            };

            Button btnDelete = ModernUIHelper.CreateGradientButton(
                "УДАЛИТЬ ПОЛЬЗОВАТЕЛЯ",
                new Point(300, 25),
                new Size(400, 50),
                ModernUIHelper.DangerColor,
                ColorTranslator.FromHtml("#d7263d")
            );
            btnDelete.Click += (s, e) => DeleteUser();

            Button btnRefresh = ModernUIHelper.CreateGradientButton(
                "ОБНОВИТЬ СПИСОК",
                new Point(730, 25),
                new Size(400, 50),
                ModernUIHelper.SecondaryAccent,
                ColorTranslator.FromHtml("#003d6b")
            );
            btnRefresh.Click += (s, e) => LoadUsers();

            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnRefresh);

            usersPanel.Controls.Add(dgvUsers);
            usersPanel.Controls.Add(buttonPanel);

            contentPanel.Controls.Add(usersPanel);
        }

        private void ShowApplicationsPanel()
        {
            applicationsPanel.Visible = true;
            specialtiesPanel.Visible = false;
            usersPanel.Visible = false;

            // Стилизация активной вкладки
            btnApplicationsNav.BackColor = ModernUIHelper.PrimaryAccent;
            btnApplicationsNav.ForeColor = Color.White;
            btnSpecialtiesNav.BackColor = Color.Transparent;
            btnSpecialtiesNav.ForeColor = ModernUIHelper.TextSecondary;
            btnUsersNav.BackColor = Color.Transparent;
            btnUsersNav.ForeColor = ModernUIHelper.TextSecondary;

            lblPageTitle.Text = "УПРАВЛЕНИЕ ЗАЯВЛЕНИЯМИ";

            // Обновляем карточки при переходе на вкладку
            LoadApplicationsCards();
        }

        private void ShowSpecialtiesPanel()
        {
            applicationsPanel.Visible = false;
            specialtiesPanel.Visible = true;
            usersPanel.Visible = false;

            // Стилизация активной вкладки
            btnApplicationsNav.BackColor = Color.Transparent;
            btnApplicationsNav.ForeColor = ModernUIHelper.TextSecondary;
            btnSpecialtiesNav.BackColor = ModernUIHelper.PrimaryAccent;
            btnSpecialtiesNav.ForeColor = Color.White;
            btnUsersNav.BackColor = Color.Transparent;
            btnUsersNav.ForeColor = ModernUIHelper.TextSecondary;

            lblPageTitle.Text = "УПРАВЛЕНИЕ СПЕЦИАЛЬНОСТЯМИ";
        }

        private void ShowUsersPanel()
        {
            applicationsPanel.Visible = false;
            specialtiesPanel.Visible = false;
            usersPanel.Visible = true;

            // Стилизация активной вкладки
            btnApplicationsNav.BackColor = Color.Transparent;
            btnApplicationsNav.ForeColor = ModernUIHelper.TextSecondary;
            btnSpecialtiesNav.BackColor = Color.Transparent;
            btnSpecialtiesNav.ForeColor = ModernUIHelper.TextSecondary;
            btnUsersNav.BackColor = ModernUIHelper.PrimaryAccent;
            btnUsersNav.ForeColor = Color.White;

            lblPageTitle.Text = "УПРАВЛЕНИЕ ПОЛЬЗОВАТЕЛЯМИ";
        }

        private void LoadData()
        {
            try
            {
                LoadSpecialties();
                LoadUsers();
            }
            catch (Exception)
            {
                // Не показываем ошибку пользователю
                // Можно добавить логирование если нужно
            }
        }

        private void LoadApplicationsCards()
        {
            // Очищаем старые карточки
            appsCardsPanel.Controls.Clear();
            selectedApplicationCard = null;

            try
            {
                // Получаем все заявления
                List<Models.Application> applications = DatabaseHelper.GetAllApplications();

                if (applications.Count == 0)
                {
                    // Сообщение если нет заявлений
                    Label lblNoApps = new Label
                    {
                        Text = "Заявлений пока нет.",
                        Font = new Font("Segoe UI", 12),
                        ForeColor = ModernUIHelper.TextSecondary,
                        Size = new Size(1380, 100),
                        TextAlign = ContentAlignment.MiddleCenter,
                        BackColor = Color.Transparent
                    };
                    appsCardsPanel.Controls.Add(lblNoApps);
                    return;
                }

                // Создаем карточки для каждого заявления
                foreach (var app in applications)
                {
                    // Создаем локальную копию для использования в лямбда-выражении
                    var currentApp = app;

                    Panel card = ModernUIHelper.CreateApplicationCard(currentApp, (s, e) =>
                    {
                        var clickedCard = (Panel)s;

                        // Снимаем выделение с предыдущей карточки
                        if (selectedApplicationCard != null && selectedApplicationCard != clickedCard)
                        {
                            selectedApplicationCard.BackColor = ModernUIHelper.CardBackground;
                            selectedApplicationCard.Refresh();
                        }

                        // Выделяем текущую карточку
                        clickedCard.BackColor = ColorTranslator.FromHtml("#f5f9fb");
                        clickedCard.Refresh();
                        selectedApplicationCard = clickedCard;

                        // При клике открываем детали (без двойного клика)
                        ApplicationDetailsForm detailsForm = new ApplicationDetailsForm(currentApp, true);
                        detailsForm.ShowDialog();

                        // Обновляем карточки после закрытия формы
                        if (detailsForm.DialogResult == DialogResult.OK)
                        {
                            LoadApplicationsCards();
                        }
                    });

                    appsCardsPanel.Controls.Add(card);
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
                    Size = new Size(1380, 100),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent
                };
                appsCardsPanel.Controls.Add(lblError);
            }
        }

        private void LoadSpecialties()
        {
            try
            {
                List<Specialty> specialties = DatabaseHelper.GetAllSpecialties();

                // Проверяем, что DataGridView инициализирован
                if (dgvSpecialties == null) return;

                dgvSpecialties.DataSource = null;
                dgvSpecialties.DataSource = specialties;

                // Проверяем наличие столбцов перед доступом к ним
                if (dgvSpecialties.Columns.Count > 0)
                {
                    // Используем проверку индексов для безопасного доступа
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

        private void LoadUsers()
        {
            try
            {
                List<User> users = DatabaseHelper.GetAllUsers();

                // Проверяем, что DataGridView инициализирован
                if (dgvUsers == null) return;

                dgvUsers.DataSource = null;
                dgvUsers.DataSource = users;

                // Проверяем наличие столбцов перед доступом к ним
                if (dgvUsers.Columns.Count > 0)
                {
                    // Используем проверку индексов для безопасного доступа
                    if (dgvUsers.Columns.Contains("Id"))
                    {
                        var idColumn = dgvUsers.Columns["Id"];
                        if (idColumn != null)
                        {
                            idColumn.HeaderText = "ID";
                        }
                    }

                    if (dgvUsers.Columns.Contains("Login"))
                    {
                        var loginColumn = dgvUsers.Columns["Login"];
                        if (loginColumn != null)
                            loginColumn.HeaderText = "Логин";
                    }

                    if (dgvUsers.Columns.Contains("Password"))
                    {
                        var passColumn = dgvUsers.Columns["Password"];
                        if (passColumn != null)
                            passColumn.Visible = false;
                    }

                    if (dgvUsers.Columns.Contains("FullName"))
                    {
                        var nameColumn = dgvUsers.Columns["FullName"];
                        if (nameColumn != null)
                            nameColumn.HeaderText = "ФИО";
                    }

                    if (dgvUsers.Columns.Contains("Role"))
                    {
                        var roleColumn = dgvUsers.Columns["Role"];
                        if (roleColumn != null)
                            roleColumn.HeaderText = "Роль";
                    }
                }
            }
            catch (Exception)
            {
                // Не показываем ошибку пользователю
                if (dgvUsers != null)
                {
                    dgvUsers.DataSource = null;
                }
            }
        }

        private void ChangeApplicationStatus(string status)
        {
            if (selectedApplicationCard == null)
            {
                MessageBox.Show("Выберите заявление (кликните на карточку)!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var application = (Models.Application)selectedApplicationCard.Tag;

            if (MessageBox.Show($"Изменить статус заявления на '{status}'?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper.UpdateApplicationStatus(application.Id, status);
                    LoadApplicationsCards();
                    MessageBox.Show("Статус успешно изменен!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось изменить статус заявления", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DeleteApplication()
        {
            if (selectedApplicationCard == null)
            {
                MessageBox.Show("Выберите заявление для удаления (кликните на карточку)!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var application = (Models.Application)selectedApplicationCard.Tag;

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

        private void AddSpecialty()
        {
            try
            {
                SpecialtyEditForm form = new SpecialtyEditForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadSpecialties();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось открыть форму добавления специальности", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditSpecialty()
        {
            if (dgvSpecialties.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите специальность для редактирования!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var specialty = (Specialty)dgvSpecialties.SelectedRows[0].DataBoundItem;
                SpecialtyEditForm form = new SpecialtyEditForm(specialty);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadSpecialties();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось открыть форму редактирования специальности", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteSpecialty()
        {
            if (dgvSpecialties.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите специальность для удаления!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var specialty = (Specialty)dgvSpecialties.SelectedRows[0].DataBoundItem;

                if (MessageBox.Show($"Удалить специальность '{specialty.Name}'?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DatabaseHelper.DeleteSpecialty(specialty.Id);
                    LoadSpecialties();
                    MessageBox.Show("Специальность успешно удалена!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось удалить специальность", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteUser()
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для удаления!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var user = (User)dgvUsers.SelectedRows[0].DataBoundItem;

                if (user.Role == "Admin")
                {
                    MessageBox.Show("Невозможно удалить администратора!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show($"Удалить пользователя '{user.FullName}'?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DatabaseHelper.DeleteUser(user.Id);
                    LoadUsers();
                    MessageBox.Show("Пользователь успешно удален!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось удалить пользователя", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
