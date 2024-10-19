using FM6_H.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FM6_H.Helpers
{
    public static class DatabaseHelper
    {


        private static string connectionString = "connectionString";



        public static string GetConnectionString()

        {
            return connectionString;
        }



        public static List<Notification> GetNotifications(string role, int userId, string etat = null, string sujet = null)
        {
            List<Notification> notifications = new List<Notification>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Notification WHERE 1=1";

                if (role == "admin")
                {
                    if (!string.IsNullOrEmpty(etat))
                    {
                        query += " AND etat = @Etat";
                    }

                    if (!string.IsNullOrEmpty(sujet))
                    {
                        if (sujet == "أخرى")
                        {
                            query += " AND sujet NOT IN ('طلب تجديد المعطيات', 'طلب تجديد كلمة المرور', 'نسيت كلمة المرور')";
                        }
                        else
                        {
                            query += " AND sujet = @Sujet";
                        }
                    }
                }
                else
                {
                    query += " AND id_user = @UserId AND (etat = @Etat1 OR etat = @Etat2 OR etat = @Etat3)";

                    if (!string.IsNullOrEmpty(sujet))
                    {
                        if (sujet == "أخرى")
                        {
                            query += " AND sujet NOT IN ('طلب تجديد المعطيات', 'طلب تجديد كلمة المرور', 'نسيت كلمة المرور')";
                        }
                        else
                        {
                            query += " AND sujet = @Sujet";
                        }
                    }
                }

                SqlCommand command = new SqlCommand(query, connection);

                if (role != "admin")
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Etat1", "Validé");
                    command.Parameters.AddWithValue("@Etat2", "Refusé");
                    command.Parameters.AddWithValue("@Etat3", "VU");
                }

                if (!string.IsNullOrEmpty(etat))
                {
                    command.Parameters.AddWithValue("@Etat", etat);
                }

                if (!string.IsNullOrEmpty(sujet) && sujet != "أخرى")
                {
                    command.Parameters.AddWithValue("@Sujet", sujet);
                }

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        notifications.Add(new Notification
                        {
                            Destinataire = reader["Destinataire"].ToString(),
                            Redacteur = reader["Redacteur"].ToString(),
                            Sujet = reader["Sujet"].ToString(),
                            Message = reader["Message"].ToString(),
                            Etat = reader["Etat"].ToString(),
                            DATE_EXECUTION = reader["DATE_EXECUTION"] as DateTime?,
                            DATE_NOTIFICATION = (DateTime)reader["DATE_NOTIFICATION"],
                            ID_USER = reader["id_user"] == DBNull.Value ? 0 : (int)reader["id_user"],
                            ID_NOTIFICATION = (int)reader["ID_NOTIFICATION"]
                        });
                    }
                }
            }

            return notifications;
        }

        public static void UpdateNotificationEtat(Notification notification)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE Notification SET Etat = @Etat WHERE ID_NOTIFICATION = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Etat", notification.Etat);
                command.Parameters.AddWithValue("@Id", notification.ID_NOTIFICATION);

                command.ExecuteNonQuery();
            }
        }

        public static List<Notification> GetspecifNotifications(string sujet)
        {
            List<Notification> notifications = new List<Notification>();
            string _sujet = sujet;

            if (_sujet == "طلب تجديد المعطيات")
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Notification WHERE sujet = @sujet AND ETAT in (@ETAT1,@ETAT2)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@sujet", "طلب تجديد المعطيات");
                    command.Parameters.AddWithValue("@ETAT1", "Rédiger");
                    command.Parameters.AddWithValue("@ETAT2", "vu");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            notifications.Add(new Notification
                            {
                                Destinataire = reader["Destinataire"].ToString(),
                                Redacteur = reader["Redacteur"].ToString(),
                                Sujet = reader["Sujet"].ToString(),
                                Message = reader["Message"].ToString(),
                                Etat = reader["Etat"].ToString(),
                                DATE_EXECUTION = reader["DATE_EXECUTION"] as DateTime?,
                                DATE_NOTIFICATION = (DateTime)reader["DATE_NOTIFICATION"],
                                ID_USER = reader["id_user"] == DBNull.Value ? 0 : (int)reader["id_user"]
                            });
                        }
                    }
                }

            }

            else if (_sujet == "طلب تجديد كلمة المرور")
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Notification WHERE sujet in (@sujet1,@sujet) AND ETAT in (@ETAT1,@ETAT2)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@sujet1", "نسيت كلمة المرور");
                    command.Parameters.AddWithValue("@sujet", "طلب تجديد كلمة المرور");
                    command.Parameters.AddWithValue("@ETAT1", "Rédiger");
                    command.Parameters.AddWithValue("@ETAT2", "VU");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            notifications.Add(new Notification
                            {
                                Destinataire = reader["Destinataire"].ToString(),
                                Redacteur = reader["Redacteur"].ToString(),
                                Sujet = reader["Sujet"].ToString(),
                                Message = reader["Message"].ToString(),
                                Etat = reader["Etat"].ToString(),
                                DATE_EXECUTION = reader["DATE_EXECUTION"] as DateTime?,
                                DATE_NOTIFICATION = (DateTime)reader["DATE_NOTIFICATION"],
                                ID_USER = reader["id_user"] == DBNull.Value ? 0 : (int)reader["id_user"]
                            });
                        }
                    }
                }
            }





            return notifications;
        }













        public static void UpdateNotificationStatus(string notificationDEST, string etat)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Notification SET Etat = @Etat WHERE Destinataire = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Etat", etat);
                command.Parameters.AddWithValue("@Id", notificationDEST);
                command.ExecuteNonQuery();
            }
        }

        public static void InsertNotification(Notification notification)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Notification (Destinataire, Redacteur, ID_USER, Sujet, Message, Etat, DATE_EXECUTION, DATE_NOTIFICATION) " +
                               "VALUES (@Destinataire, @Redacteur, @ID_USER, @Sujet, @Message, @Etat, @DATE_EXECUTION, @DATE_NOTIFICATION)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Destinataire", notification.Destinataire);
                command.Parameters.AddWithValue("@Redacteur", notification.Redacteur);
                command.Parameters.AddWithValue("@ID_USER", notification.ID_USER);
                command.Parameters.AddWithValue("@Sujet", notification.Sujet);
                command.Parameters.AddWithValue("@Message", notification.Message);
                command.Parameters.AddWithValue("@Etat", notification.Etat);
                command.Parameters.AddWithValue("@DATE_EXECUTION", (object)notification.DATE_EXECUTION ?? DBNull.Value);
                command.Parameters.AddWithValue("@DATE_NOTIFICATION", notification.DATE_NOTIFICATION);
                command.ExecuteNonQuery();
            }
        }
    }
}
