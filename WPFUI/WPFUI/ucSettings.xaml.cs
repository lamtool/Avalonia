using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for ucSettings.xaml
    /// </summary>
    public partial class ucSettings : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<SettingItem> GeneralSettings { get; set; }
        public ObservableCollection<JobOption> JobOptions { get; set; }
        public ObservableCollection<string> JobTasks { get; set; }
        public JobOption SelectedJob { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Version { get; } = "1.0.0";
        public DateTime UpdateDate { get; } = new DateTime(2024, 5, 15);
        public DateTime ExpiryDate { get; } = new DateTime(2024, 6, 30);
        private string _newPassword;

        public ucSettings()
        {
            GeneralSettings = new ObservableCollection<SettingItem>
            {
                new SettingItem { Title = "Option 1", Icon = "Settings" },
                new SettingItem { Title = "Option 2", Icon = "Settings" },
                new SettingItem { Title = "Option 3", Icon = "Settings" }
            };
            JobOptions = new ObservableCollection<JobOption>
            {
                new JobOption { Name = "Traodoisub.com" },
                new JobOption { Name = "Tuongtaccheo.net" },
                new JobOption { Name = "Auto.golike.net" }
            };
            JobTasks = new ObservableCollection<string>
            {
                "Like bài viết", "Bình luận", "Theo dõi", "Kết bạn", "Phản ứng cảm xúc"
            };
            SelectedJob = JobOptions[0];
            DisplayName = "User Name";
            Email = "user@example.com";

            InitializeComponent();
            DataContext = this;
        }

      
        private void SaveAccountSettings_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Saving account settings: DisplayName={DisplayName}, Email={Email}, Password={(string.IsNullOrEmpty(_newPassword) ? "Unchanged" : "Changed")}");
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                _newPassword = passwordBox.Password;
            }
        }

        private void CheckForUpdates_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Checking for updates...");
        }

        private void CreateBackup_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Creating backup...");
        }

        private void RestoreFromFile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Restoring from file...");
        }

        private void ClearLoginData_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clearing login data...");
        }

        private void ResetSystem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Resetting system...");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class SettingItem : INotifyPropertyChanged
    {
        public string Title { get; set; }
        public string Icon { get; set; } 

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class JobOption
    {
        public string Name { get; set; }
    }

}
