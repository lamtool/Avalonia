using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using AvaloniaUI.Models;
using AvaloniaUI.ViewModels;
using System.Collections.Generic;
using System;
using System.Management;
using System.Diagnostics;
using Avalonia.Input.TextInput;

namespace AvaloniaUI.Views;

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
        try
        {
            var searcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem");
            foreach (ManagementObject mo in searcher.Get())
            {
                double totalKb = Convert.ToDouble(mo["TotalVisibleMemorySize"]);
                double freeKb = Convert.ToDouble(mo["FreePhysicalMemory"]);
                totalGB = totalKb / 1024 / 1024;
                usedGB = (totalKb - freeKb) / 1024 / 1024;
            }
        }
        catch
        {
            // Optionally log or handle error
            totalGB = usedGB = 0;
        }
    }

    private void DataGridAccount_Loaded(object? sender, RoutedEventArgs e)
    {
        //if (StatusColumn != null)
        //{
        //    // Set to auto width
        //    StatusColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
        //    // Enforce minimum width
        //    StatusColumn.MinWidth = 200;
        //}
    }
    private async void DataGridAccount_ScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (!(DataContext is AccountViewModel viewModel) || viewModel.IsLoading)
            return;

        // Get the ScrollViewer (sender is usually the ScrollViewer)
        var scrollViewer = sender as ScrollViewer;
        if (scrollViewer == null)
            return;

        // Calculate how close to the bottom we are
        double verticalOffset = e.OffsetDelta.Y;
        double maxVerticalOffset = e.ExtentDelta.Y - e.ViewportDelta.Y;

        // --- XỬ LÝ CUỘN XUỐNG ĐỂ TẢI TRANG TIẾP THEO ---
        if (maxVerticalOffset > 0 && verticalOffset >= maxVerticalOffset * 0.95)
        {
            if (await viewModel.LoadNextPage())
            {
                scrollViewer.Offset = new Avalonia.Vector(scrollViewer.Offset.X, scrollViewer.Offset.Y + 10);
            }
        }
        // --- XỬ LÝ CUỘN LÊN ĐỂ TẢI TRANG TRƯỚC ĐÓ ---
        else if (verticalOffset == 0)
        {
            if (await viewModel.LoadPreviousPage())
            {
                scrollViewer.Offset = new Avalonia.Vector(scrollViewer.Offset.X, Math.Max(0, scrollViewer.Offset.Y - 10));
            }
        }
    }

    private void btnAddFolder_Click(object sender, RoutedEventArgs e) { }
    private void ButtonReload_Click(object sender, RoutedEventArgs e)
    {
        DataContext = new AccountViewModel(TestClass.CreateList());
    }
    private void btnAddAccount_Click(object sender, RoutedEventArgs e) { }
    private void NumericUpDown_PreviewTextInput(object sender, TextInputEventArgs e) { }
    private void NumericUpDown_PreviewKeyDown(object sender, KeyEventArgs e) { }

}
