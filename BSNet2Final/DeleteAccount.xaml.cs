using System;
using System.Collections.Generic;
using System.Data.OleDb;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BSNet2Final
{
    /// <summary>
    /// Interaction logic for DeleteAccount.xaml
    /// </summary>
    public partial class DeleteAccount : Page
    {
        public DeleteAccount()
        {
            InitializeComponent();
        }

        

        //delete the account from the BSNet2 Database
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            //Establish the string variables needed for a relative path
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            AppDomain.CurrentDomain.SetData("DataDirectory", path);

            //Connect to the database

            //connection string to connect to the DB using the absolute path of DataDirectory made from my AppDomain 
            string ConnString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source=|DataDirectory|\BSNet2Final.accdb";
            //connection object to fully connect to the database
            using (OleDbConnection conn = new OleDbConnection(ConnString))
            {
                try {
                    //open connection
                    conn.Open();

                    //Create an oledbcommand that will delete the account where the username and password = given parameters
                    using (OleDbCommand deleteCmd = new OleDbCommand("Delete From BSNet2 where AccountName = @Username", conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@Username", usernameTextBox.Text);
                        

                        //Create a reader object to read the sql command
                        using (OleDbDataReader deleteReader = deleteCmd.ExecuteReader())
                        {
                            MessageBox.Show("You have successfully deleted the account " + usernameTextBox.Text);
                            usernameTextBox.Clear();
                            this.NavigationService.Navigate(new Admin());
                        }
                    }
                }
                catch(Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                //close the connection
                conn.Close();
            }

        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Admin());
        }
    }
}
