using FM6_H.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static FM6_H.admin_dash;

namespace FM6_H
{
    /// <summary>
    /// Interaction logic for dash.xaml
    /// </summary>
    public partial class dash : Window
    {
        private DispatcherTimer timer;
        private Dictionary<int, Button> appButtons; // Declare appButtons as a class-level field
        private string userEmail; // Store the email
        private string _role;
        private int _userId;
        private ClickCountModel _clickCountModel;


        public dash(string email, int userId, ClickCountModel clickCountModel)
        {
            InitializeComponent();
            startclock();


            // Assuming you have a label named lblGreeting in your XAML
            lblGreeting.Content = email + "   مرحبا"; // Set the greeting message
            userEmail = email;
            _role = DetermineRole(email);
            _userId = userId;
            DataContext = new adminDashViewModel(userId, "user");
            _clickCountModel = clickCountModel;

            appButtons = new Dictionary<int, Button>
            {
                { 1, websiteApp },
                { 2, projectApp },
                { 3, fm6carteApp },
                { 4, mailApp },
                { 5, financeApp },
                { 6, boApp },
                { 7, archiveApp },
                { 8, servicesApp },
                { 9, medigesApp }
            };

            LoadUserAccess(userId);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("calc.exe");



        }




        private void LoadUserAccess(int userId)
        {
            List<int> userAppIds = new List<int>();

            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-NLLSHRK\\SQLEXPRESS;Initial Catalog=DB_FM6;Integrated Security=True"))
            {
                conn.Open();
                string query = "SELECT id_app FROM acces WHERE id_user = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userAppIds.Add(reader.GetInt32(0));
                }
            }

            foreach (var appButton in appButtons)
            {
                if (!userAppIds.Contains(appButton.Key))
                {
                    appButton.Value.Visibility = Visibility.Collapsed;
                }
            }
        }


        private void startclock()
        {

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMicroseconds(1);
            timer.Tick += tickevent;
            timer.Start();
        }

        private void tickevent(object sender, EventArgs e)
        {

            lbldate.Content = DateTime.Now.ToString();

        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            lbldate.Content = DateTime.Now.ToLongTimeString();
        }



        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string ap = Process.GetCurrentProcess().MainModule.FileName;

            Process.Start(new ProcessStartInfo
            {
                FileName = ap,
                UseShellExecute = true,
            });

            Application.Current.Shutdown();
        }




        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //admin_dash f1 = new admin_dash();
            //f1.Show();


            account f1 = new account(userEmail);
            f1.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //resetpass1 h = new resetpass1();
            history h1 = new history();
            h1.Show();
        }

        //private void notifclick(object sender, MouseButtonEventArgs e)
        //{


        //    try
        //    {
        //        UpdateNotificationsEtat();
        //        NotificationTab notificationTab = new NotificationTab(userEmail);
        //        notificationTab.Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("An error occurred: " + ex.Message);
        //    }


        //}





        private string DetermineRole(string email)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-NLLSHRK\\SQLEXPRESS;Initial Catalog=DB_FM6;Integrated Security=True"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT TYPE FROM u_ser WHERE Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", userEmail);

                var _role = command.ExecuteScalar() as string;
                return _role;
            }
        }








        private void notifclick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                UpdateNotificationsEtat();
                NotificationTab notificationTab = new NotificationTab(userEmail);
                notificationTab.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }








        private void UpdateNotificationsEtat()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-NLLSHRK\\SQLEXPRESS;Initial Catalog=DB_FM6;Integrated Security=True"))
            {
                connection.Open();

                string query = "UPDATE Notification SET Etat = 'Vu' WHERE Etat = 'Rédiger' AND redacteur = 'admin' ";
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }






        //// Update UI to reflect changes in notifications
        //((dashViewModel)DataContext).LoadNotifications();









        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void showCalculator(object sender, RoutedEventArgs e)
        {
            Process.Start("calc.exe");


        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

        }


        private string _connectionString = "Data Source=DESKTOP-NLLSHRK\\SQLEXPRESS;Initial Catalog=DB_FM6;Integrated Security=True";




        private void ApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string applicationName = button.Tag.ToString();

            // Update the click count in the database
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string updateQuery = @"
                    IF EXISTS (SELECT 1 FROM ClickCounts WHERE ApplicationName = @ApplicationName)
                        UPDATE ClickCounts SET ClickCount = ClickCount + 1 WHERE ApplicationName = @ApplicationName
                    ELSE
                        INSERT INTO ClickCounts (ApplicationName, ClickCount) VALUES (@ApplicationName, 1)";

                using (var command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationName", applicationName);
                    command.ExecuteNonQuery();
                }
            }

            // Retrieve the updated click count from the database
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT ClickCount FROM ClickCounts WHERE ApplicationName = @ApplicationName";

                using (var command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationName", applicationName);
                    int clickCount = (int)command.ExecuteScalar();

                    // Update the respective property in the shared data model
                    switch (applicationName)
                    {
                        case "websiteApp":
                            _clickCountModel.WebsiteAppCount = clickCount;
                            break;
                        case "fm6carteApp":
                            _clickCountModel.Fm6carteAppCount = clickCount;
                            break;
                        case "projectApp":
                            _clickCountModel.ProjectAppCount = clickCount;
                            break;
                        case "mailApp":
                            _clickCountModel.MailAppCount = clickCount;
                            break;
                        case "financeApp":
                            _clickCountModel.FinanceAppCount = clickCount;
                            break;
                        case "boApp":
                            _clickCountModel.BoAppCount = clickCount;
                            break;
                        case "archiveApp":
                            _clickCountModel.ArchiveAppCount = clickCount;
                            break;
                        case "servicesApp":
                            _clickCountModel.ServicesAppCount = clickCount;
                            break;
                        case "medigesApp":
                            _clickCountModel.MedigesAppCount = clickCount;
                            break;
                    }

                }
            }


        }




    }
}
