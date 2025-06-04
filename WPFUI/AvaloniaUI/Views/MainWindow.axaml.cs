using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AvaloniaUI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem clickedItem)
        {
            // Lấy menu cha chứa tất cả menuitem
            var menu = ItemsControl.ItemsControlFromItemContainer(clickedItem) as Menu;

            if (menu == null)
                return;

            // Bỏ chọn tất cả menuitem khác
            foreach (var obj in menu.Items)
            {
                if (obj is MenuItem item && item != clickedItem)
                {
                    item.IsChecked = false;
                }
            }

            // Đảm bảo item click hiện tại được chọn
            clickedItem.IsChecked = true;
        }
    }

    private void MenuItem_TaiKhoan_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new ucAccount();
    }
    public void ResizeDataGrid(double containerHeight)
    {

    }

    private void MenuItem_Dashboard_Click(object sender, RoutedEventArgs e)
    {

    }

    private void MenuItem_NhomTaiKhoan_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new ucGroups();
    }

    private void MenuItem_Settings_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new ucSettings();
    }
}
