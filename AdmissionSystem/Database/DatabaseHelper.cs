using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Dapper;
using AdmissionSystem.Models;

namespace AdmissionSystem.Database
{
    public static class DatabaseHelper
    {
        private static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "admission.db");
        private static string ConnectionString => $"Data Source={dbPath};Version=3;";

        public static void InitializeDatabase()
        {
            bool isNewDatabase = !File.Exists(dbPath);

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                // Создание таблицы пользователей
                connection.Execute(@"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Login TEXT NOT NULL UNIQUE,
                        Password TEXT NOT NULL,
                        FullName TEXT NOT NULL,
                        Role TEXT NOT NULL,
                        RegistrationDate TEXT NOT NULL
                    )");

                // Создание таблицы специальностей
                connection.Execute(@"
                    CREATE TABLE IF NOT EXISTS Specialties (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Code TEXT NOT NULL UNIQUE,
                        PlacesCount INTEGER NOT NULL,
                        MinScore REAL NOT NULL,
                        Description TEXT
                    )");

                // Создание таблицы заявлений
                connection.Execute(@"
                    CREATE TABLE IF NOT EXISTS Applications (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId INTEGER NOT NULL,
                        SpecialtyId INTEGER NOT NULL,
                        FirstName TEXT NOT NULL,
                        LastName TEXT NOT NULL,
                        MiddleName TEXT,
                        BirthDate TEXT NOT NULL,
                        PassportSeries TEXT NOT NULL,
                        PassportNumber TEXT NOT NULL,
                        Address TEXT NOT NULL,
                        Phone TEXT NOT NULL,
                        Email TEXT NOT NULL,
                        ExamScore REAL NOT NULL,
                        Status TEXT NOT NULL,
                        SubmissionDate TEXT NOT NULL,
                        Notes TEXT,
                        FOREIGN KEY (UserId) REFERENCES Users(Id),
                        FOREIGN KEY (SpecialtyId) REFERENCES Specialties(Id)
                    )");

                // Создание администратора по умолчанию
                if (isNewDatabase)
                {
                    var adminExists = connection.ExecuteScalar<int>(
                        "SELECT COUNT(*) FROM Users WHERE Login = 'admin'");

                    if (adminExists == 0)
                    {
                        connection.Execute(@"
                            INSERT INTO Users (Login, Password, FullName, Role, RegistrationDate)
                            VALUES ('admin', 'admin', 'Администратор', 'Admin', @date)",
                            new { date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                    }

                    // Добавление специальностей колледжа
                    connection.Execute(@"
                        INSERT INTO Specialties (Name, Code, PlacesCount, MinScore, Description)
                        VALUES
                        ('Информационные системы', '09.02.07', 25, 4.35, 'Разработка и администрирование информационных систем'),
                        ('Обеспечение безопасности', '10.02.05', 20, 4.42, 'Информационная безопасность автоматизированных систем'),
                        ('Дошкольное образование', '44.02.01', 30, 4.28, 'Воспитание и обучение детей дошкольного возраста'),
                        ('Начальные классы', '44.02.02', 30, 4.56, 'Преподавание в начальных классах')");
                }
            }
        }

        // Методы для работы с пользователями
        public static User GetUser(string login, string password)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.QueryFirstOrDefault<User>(
                    "SELECT * FROM Users WHERE Login = @Login AND Password = @Password",
                    new { Login = login, Password = password });
            }
        }

        public static bool RegisterUser(User user)
        {
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Execute(@"
                        INSERT INTO Users (Login, Password, FullName, Role, RegistrationDate)
                        VALUES (@Login, @Password, @FullName, @Role, @RegistrationDate)",
                        user);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static List<User> GetAllUsers()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<User>("SELECT * FROM Users").ToList();
            }
        }

        public static void UpdateUser(User user)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute(@"
                    UPDATE Users
                    SET Login = @Login, Password = @Password, FullName = @FullName, Role = @Role
                    WHERE Id = @Id", user);
            }
        }

        public static void DeleteUser(int userId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("DELETE FROM Users WHERE Id = @Id", new { Id = userId });
            }
        }

        // Методы для работы со специальностями
        public static List<Specialty> GetAllSpecialties()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<Specialty>("SELECT * FROM Specialties").ToList();
            }
        }

        public static void AddSpecialty(Specialty specialty)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute(@"
                    INSERT INTO Specialties (Name, Code, PlacesCount, MinScore, Description)
                    VALUES (@Name, @Code, @PlacesCount, @MinScore, @Description)",
                    specialty);
            }
        }

        public static void UpdateSpecialty(Specialty specialty)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute(@"
                    UPDATE Specialties
                    SET Name = @Name, Code = @Code, PlacesCount = @PlacesCount,
                        MinScore = @MinScore, Description = @Description
                    WHERE Id = @Id", specialty);
            }
        }

        public static void DeleteSpecialty(int specialtyId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("DELETE FROM Specialties WHERE Id = @Id", new { Id = specialtyId });
            }
        }

        // Методы для работы с заявлениями
        public static List<Application> GetAllApplications()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<Application>(@"
                    SELECT a.*, s.Name as SpecialtyName
                    FROM Applications a
                    LEFT JOIN Specialties s ON a.SpecialtyId = s.Id").ToList();
            }
        }

        public static List<Application> GetUserApplications(int userId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<Application>(@"
                    SELECT a.*, s.Name as SpecialtyName
                    FROM Applications a
                    LEFT JOIN Specialties s ON a.SpecialtyId = s.Id
                    WHERE a.UserId = @UserId",
                    new { UserId = userId }).ToList();
            }
        }

        public static void AddApplication(Application application)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute(@"
                    INSERT INTO Applications
                    (UserId, SpecialtyId, FirstName, LastName, MiddleName, BirthDate,
                     PassportSeries, PassportNumber, Address, Phone, Email, ExamScore,
                     Status, SubmissionDate, Notes)
                    VALUES
                    (@UserId, @SpecialtyId, @FirstName, @LastName, @MiddleName, @BirthDate,
                     @PassportSeries, @PassportNumber, @Address, @Phone, @Email, @ExamScore,
                     @Status, @SubmissionDate, @Notes)",
                    application);
            }
        }

        public static void UpdateApplication(Application application)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute(@"
                    UPDATE Applications
                    SET SpecialtyId = @SpecialtyId, FirstName = @FirstName, LastName = @LastName,
                        MiddleName = @MiddleName, BirthDate = @BirthDate, PassportSeries = @PassportSeries,
                        PassportNumber = @PassportNumber, Address = @Address, Phone = @Phone,
                        Email = @Email, ExamScore = @ExamScore, Status = @Status, Notes = @Notes
                    WHERE Id = @Id", application);
            }
        }

        public static void DeleteApplication(int applicationId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("DELETE FROM Applications WHERE Id = @Id", new { Id = applicationId });
            }
        }

        public static Specialty GetSpecialtyById(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.QueryFirstOrDefault<Specialty>(
                    "SELECT * FROM Specialties WHERE Id = @Id", new { Id = id });
            }
        }

        public static bool UserExists(string login)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var count = connection.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM Users WHERE Login = @Login",
                    new { Login = login });
                return count > 0;
            }
        }

        public static void UpdateApplicationStatus(int applicationId, string status)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute(@"
                    UPDATE Applications
                    SET Status = @Status
                    WHERE Id = @Id",
                    new { Id = applicationId, Status = status });
            }
        }
    }
}
