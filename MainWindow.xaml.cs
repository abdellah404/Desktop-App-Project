using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FM6_H
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Window previousWindow;
        private string connectionString = "your_connection_string_here"; // Update with your connection string

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnLogin(object sender, RoutedEventArgs e)
        {
            string email = loginEmail.Text;
            string password = loginPassword.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both email and password.");
                return;
            }

            try
            {
                // Validate user credentials and retrieve user ID and user type
                Tuple<int, string> userData = GetUserDataFromCredentials(email, password);

                if (userData == null)
                {
                    MessageBox.Show("Invalid email or password.");
                    return;
                }

                int userId = userData.Item1;
                string userType = userData.Item2;

                // Open the appropriate dashboard window based on user type
                if (userType.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    admin_dash1 adminDash = new admin_dash1(email, userId);
                    adminDash.Show();
                }
                else if (userType.Equals("user", StringComparison.OrdinalIgnoreCase))

                {

                    ClickCountModel clickCountModel = new ClickCountModel();
                    user_dash dashboard = new user_dash(email, userId, clickCountModel);
                    dashboard.Show();
                }

                // Close the current login window
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private Tuple<int, string> GetUserDataFromCredentials(string email, string password)
        {
            Tuple<int, string> userData = null;

            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-NLLSHRK\\SQLEXPRESS;Initial Catalog=DB_FM6;Integrated Security=True"))
            {
                conn.Open();
                string query = "SELECT id_user, TYPE FROM U_SER WHERE EMAIL = @Email AND MDP = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int userId = reader.GetInt32(0);
                    string userType = reader.GetString(1);
                    userData = Tuple.Create(userId, userType);
                }
            }

            return userData;
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }



        private void mdpOblier_click(object sender, MouseButtonEventArgs e)
        {
            mdpOblier mdpOblier = new mdpOblier();
            mdpOblier.Show();
            this.Close();
        }
    }
}
