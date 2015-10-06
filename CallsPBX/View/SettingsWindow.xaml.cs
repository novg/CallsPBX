using System;
using System.Collections.Generic;
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

namespace CallsPBX
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Host = txtHost.Text;
            Properties.Settings.Default.Database = txtBase.Text;
            Properties.Settings.Default.Username = txtUser.Text;
            Properties.Settings.Default.Password = pswPassword.Password;
            Properties.Settings.Default.Save();

            if (string.IsNullOrWhiteSpace(txtHost.Text) ||
                string.IsNullOrWhiteSpace(txtBase.Text) ||
                string.IsNullOrWhiteSpace(txtUser.Text) ||
                string.IsNullOrWhiteSpace(pswPassword.Password))
            {
                MessageBox.Show("Поле значения не может быть пустым", "Ошибка!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtHost.Text = Properties.Settings.Default.Host;
            txtBase.Text = Properties.Settings.Default.Database;
            txtUser.Text = Properties.Settings.Default.Username;
            pswPassword.Password = Properties.Settings.Default.Password;
        }
    }
}
