using FM6_H.Helpers;
using FM6_H.ViewModels;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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

namespace FM6_H
{
    /// <summary>
    /// Interaction logic for admin_dash1.xaml
    /// </summary>
    public partial class admin_dash1 : Window
    {
        private Dictionary<int, Button> appButtons; // Declare appButtons as a class-level field
        private string _email;
        private string _role;
        private int _userId;



        public admin_dash1(string email, int userId)
        {
            InitializeComponent();

            lblGreeting.Content = email + "   مرحبا"; // Set the greeting message
            _email = email;
            _userId = userId;
            _role = DetermineRole(email);


            DataContext = new adminDashViewModel(userId, "admin");

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
            startclock();


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




        private void LoadUserAccess(int userId)
        {
            List<int> userAppIds = new List<int>();

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString()))
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




        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            admin_dash f1 = new admin_dash();
            f1.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }



        private void to_admin_statistiques(object sender, RoutedEventArgs e)
        {

            ClickCountModel clickCountModel = new ClickCountModel();
            admin_history admin_History = new admin_history(clickCountModel);
            admin_History.Show();
        }

        private void logoutBtn(object sender, RoutedEventArgs e)
        {



            string ap = Process.GetCurrentProcess().MainModule.FileName;


            Process.Start(

                new ProcessStartInfo
                {
                    FileName = ap,
                    UseShellExecute = true,


                });



            Application.Current.Shutdown();


        }








        private void Viewnotifclick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                UpdateNotificationsEtat();
                NotificationTab notificationTab = new NotificationTab(_email);
                notificationTab.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }




        private string DetermineRole(string email)
        {
            using (SqlConnection connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT TYPE FROM u_ser WHERE Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", _email);

                var _role = command.ExecuteScalar() as string;
                return _role;
            }
        }



        private void UpdateNotificationsEtat()
        {
            using (SqlConnection connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();

                string query;
                SqlCommand command;



                query = "UPDATE Notification SET Etat = 'Vu' WHERE Etat = 'Rédiger' AND Destinataire = 'admin'";


                command = new SqlCommand(query, connection);


                command.Parameters.AddWithValue("@UserId", _userId);


                command.ExecuteNonQuery();
            }
        }






        private void Sendnotifclickk(object sender, MouseButtonEventArgs e)
        {
            SendNotiffication sendnotifclickk = new SendNotiffication(_email);
            sendnotifclickk.ShowDialog();
        }

        private void showCalculator(object sender, RoutedEventArgs e)
        {
            Process.Start("calc.exe");


        }

        private void mailApp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void projectApp_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
