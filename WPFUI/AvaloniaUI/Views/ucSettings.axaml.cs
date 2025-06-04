using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace AvaloniaUI.Views
{
    public partial class ucSettings : UserControl, INotifyPropertyChanged
    {
        public ucSettings()
        {
            InitializeComponent();
            // Đặt DataContext của chính UserControl này là nó
            this.DataContext = this;

            InitializeData();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        // --- Implement INotifyPropertyChanged ---
        // Sử dụng từ khóa 'new' để báo hiệu rằng chúng ta đang cố tình ẩn đi
        // PropertyChanged event của AvaloniaObject.
        public new event PropertyChangedEventHandler? PropertyChanged;

        // Phương thức để gọi khi một thuộc tính thay đổi
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // --- Các thuộc tính (properties) cho DataContext ---

        private string _displayName = "Tên người dùng"; // Dữ liệu mẫu
        public string DisplayName
        {
            get => _displayName;
            set
            {
                if (_displayName != value)
                {
                    _displayName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _email = "user@example.com"; // Dữ liệu mẫu
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _newPassword = "";
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                if (_newPassword != value)
                {
                    _newPassword = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Version => "1.0.0"; // Dữ liệu chỉ đọc
        public DateTime UpdateDate => new DateTime(2024, 6, 3); // Dữ liệu chỉ đọc
        public DateTime ExpiryDate => new DateTime(2025, 12, 31); // Dữ liệu chỉ đọc

        // Job Options
        private List<JobOption> _jobOptions = new List<JobOption>();
        public List<JobOption> JobOptions
        {
            get => _jobOptions;
            private set
            {
                _jobOptions = value;
                OnPropertyChanged();
            }
        }

        private JobOption? _selectedJob;
        public JobOption? SelectedJob
        {
            get => _selectedJob;
            set
            {
                if (_selectedJob != value)
                {
                    _selectedJob = value;
                    OnPropertyChanged();
                    // Load tasks for the selected job if needed
                    // UpdateJobTasks(value);
                }
            }
        }

        // Job Tasks (dữ liệu mẫu)
        private List<JobTask> _jobTasks = new List<JobTask>();
        public List<JobTask> JobTasks
        {
            get => _jobTasks;
            private set
            {
                _jobTasks = value;
                OnPropertyChanged();
            }
        }

        // Phương thức khởi tạo dữ liệu mẫu
        private void InitializeData()
        {
            JobOptions = new List<JobOption>
            {
                new JobOption { Id = 1, Name = "Job Option 1" },
                new JobOption { Id = 2, Name = "Job Option 2" },
                new JobOption { Id = 3, Name = "Job Option 3" }
            };
            SelectedJob = JobOptions[0]; // Chọn mặc định
            JobTasks = new List<JobTask>
            {
                new JobTask { Id = 101, Name = "Task A.1" },
                new JobTask { Id = 102, Name = "Task A.2" },
                new JobTask { Id = 103, Name = "Task B.1" },
                new JobTask { Id = 104, Name = "Task C.1" },
            };
        }


        // --- Event Handlers from your XAML ---

        private void PasswordBox_PasswordChanged(object? sender, Avalonia.Input.TextInputEventArgs e)
        {
            if (sender is TextBox passwordBox && passwordBox.Text != null)
            {
                // Gán giá trị mật khẩu vào thuộc tính NewPassword.
                // LƯU Ý: Đây là cách lấy mật khẩu từ UI. Trong ứng dụng thực tế,
                // bạn cần xử lý mật khẩu một cách an toàn hơn (ví dụ: hash nó ngay lập tức,
                // không lưu trữ plaintext, hoặc sử dụng SecureString).
                NewPassword = passwordBox.Text;
                Console.WriteLine($"Password changed (from UI event). Length: {passwordBox.Text.Length}");
            }
        }

        private void SaveAccountSettings_Click(object? sender, RoutedEventArgs e)
        {
            Console.WriteLine("Save Account Settings Clicked.");
            Console.WriteLine($"DisplayName: {DisplayName}, Email: {Email}, NewPassword length: {NewPassword.Length}");
            // Thêm logic lưu cài đặt tài khoản của bạn vào đây
        }

        private void CheckForUpdates_Click(object? sender, RoutedEventArgs e)
        {
            Console.WriteLine("Check For Updates Clicked.");
            // Logic để kiểm tra cập nhật
        }

        private void CreateBackup_Click(object? sender, RoutedEventArgs e)
        {
            Console.WriteLine("Create Backup Clicked.");
            // Logic để tạo bản sao lưu
        }

        private void RestoreFromFile_Click(object? sender, RoutedEventArgs e)
        {
            Console.WriteLine("Restore From File Clicked.");
            // Logic để khôi phục từ file
        }

        private void ClearLoginData_Click(object? sender, RoutedEventArgs e)
        {
            Console.WriteLine("Clear Login Data Clicked.");
            // Logic để xóa dữ liệu đăng nhập
            // Cẩn thận với thao tác này!
        }

        private void ResetSystem_Click(object? sender, RoutedEventArgs e)
        {
            Console.WriteLine("Reset System Clicked.");
            // Logic để đặt lại toàn bộ hệ thống.
            // Đây là một hành động rất phá hoại! Luôn yêu cầu xác nhận từ người dùng.
        }
    }

    // Các lớp model nhỏ dùng cho binding
    // Bạn nên đặt chúng trong một thư mục Model riêng biệt (ví dụ: AvaloniaUI/Models)
    public class JobOption : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class JobTask : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}