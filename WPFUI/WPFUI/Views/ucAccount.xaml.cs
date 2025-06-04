using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using WPFUI.Models;

namespace WPFUI
{
    public partial class ucAccount : UserControl
    {
        private readonly PerformanceCounter cpuCounter;
        private readonly DispatcherTimer timer;
        public ucAccount()
        {
            InitializeComponent();
            DataContext = new AccountViewModel(new List<Account>());
            DataGridAccount.Loaded += DataGridAccount_Loaded;
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue(); // Bỏ giá trị đầu tiên vì nó luôn là 0

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            float cpuUsage = cpuCounter.NextValue();
            CpuProgressBar.Value = cpuUsage;
            CpuText.Text = $"{cpuUsage:0.0}%";

            // Lấy RAM bằng WMI
            GetRamInfo(out double totalGB, out double usedGB);
            double ramUsagePercent = usedGB / totalGB * 100;

            RamProgressBar.Value = ramUsagePercent;
            RamText.Text = $"{usedGB:0.0}/{totalGB:0.0} GB";
        }
        private void GetRamInfo(out double totalGB, out double usedGB)
        {
            totalGB = usedGB = 0;

            var searcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem");
            foreach (ManagementObject mo in searcher.Get())
            {
                double totalKb = Convert.ToDouble(mo["TotalVisibleMemorySize"]);
                double freeKb = Convert.ToDouble(mo["FreePhysicalMemory"]);

                totalGB = totalKb / 1024 / 1024;
                usedGB = (totalKb - freeKb) / 1024 / 1024;
            }
        }
        private void DataGridAccount_Loaded(object sender, RoutedEventArgs e)
        {
            if (StatusColumn != null)
            {
                StatusColumn.Width = double.NaN;
                DataGridAccount.UpdateLayout();
                if (StatusColumn.ActualWidth < 200)
                {
                    StatusColumn.Width = 200;
                }

                DependencyPropertyDescriptor.FromProperty(GridViewColumn.WidthProperty, typeof(GridViewColumn))
                    .AddValueChanged(StatusColumn, (s, args) =>
                    {
                        if (StatusColumn.ActualWidth < 200)
                        {
                            StatusColumn.Width = 200;
                        }
                    });
            }
        }
        private async void DataGridAccount_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (!(DataContext is AccountViewModel viewModel) || viewModel.IsLoading)
            {
                return; // Nếu không có ViewModel hoặc đang tải thì không làm gì cả
            }

            var scrollViewer = (ScrollViewer)e.OriginalSource;

            // --- XỬ LÝ CUỘN XUỐNG ĐỂ TẢI TRANG TIẾP THEO ---
            if (e.VerticalChange > 0) // e.VerticalChange > 0 nghĩa là đang cuộn xuống
            {
                // Kiểm tra nếu đang ở gần cuối
                if (scrollViewer.ScrollableHeight > 0 &&
                    scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight * 0.95)
                {
                    // Chờ cho đến khi trang tiếp theo được tải xong
                    if (await viewModel.LoadNextPage())
                    {
                        scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + 10);
                    }
                }
            }
            // --- XỬ LÝ CUỘN LÊN ĐỂ TẢI TRANG TRƯỚC ĐÓ ---
            else if (e.VerticalChange < 0) // e.VerticalChange < 0 nghĩa là đang cuộn lên
            {
                // Kiểm tra nếu đang ở ngay đầu danh sách
                if (scrollViewer.VerticalOffset == 0)
                {
                    // Chờ cho đến khi trang trước được tải xong
                    if (await viewModel.LoadPreviousPage())
                    {
                        scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - 10);
                    }
                }
            }
        }

        private void btnAddFolder_Click(object sender, RoutedEventArgs e) { }
        private void ButtonReload_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new AccountViewModel(TestClass.CreateList());
        }
        private void btnAddAccount_Click(object sender, RoutedEventArgs e) { }
        private void NumericUpDown_PreviewTextInput(object sender, TextCompositionEventArgs e) { }
        private void NumericUpDown_PreviewKeyDown(object sender, KeyEventArgs e) { }

    }
    public class WarnStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string state = value?.ToString();
            return !string.IsNullOrWhiteSpace(state) && state != "LIVE" && state != "DIE";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

}