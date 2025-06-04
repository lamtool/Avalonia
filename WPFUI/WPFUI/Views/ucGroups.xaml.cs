using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ucGroups.xaml
    /// </summary>
    public partial class ucGroups : UserControl
    {
        public ObservableCollection<Group> Groups { get; set; } = new ObservableCollection<Group>();
        public ucGroups()
        {
            InitializeComponent();
            DataContext = this; // Gán DataContext để binding hoạt động
            for (int i = 0; i < 100; i++)
            {
                Groups.Add(new Group
                {
                    Name = $"Nhóm VIP {i}",
                    CreationDate = $"{i}/05/2024",
                    AccountCount = i,
                    Status = "Đang hoạt động"
                });
            }
        }
        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Groups == null || Groups.Count == 0) return; // Skip if Groups is empty

            if (SortComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string sortOption = selectedItem.Content.ToString();
                List<Group> sortedGroups;

                switch (sortOption)
                {
                    case "Tên (A-Z)":
                        sortedGroups = Groups.OrderBy(g => g.Name).ToList();
                        break;
                    case "Số lượng tài khoản":
                        sortedGroups = Groups.OrderByDescending(g => g.AccountCount).ToList();
                        break;
                    case "Ngày tạo":
                        sortedGroups = Groups.OrderByDescending(g => g.CreationDate).ToList();
                        break;
                    default:
                        return; // No sorting if invalid option
                }

                // Update the ObservableCollection
                Groups.Clear();
                foreach (var group in sortedGroups)
                {
                    Groups.Add(group);
                }
            }
        }
    }
    public class Group
    {
        public string Name { get; set; }
        public string CreationDate { get; set; }
        public int AccountCount { get; set; }
        public string Status { get; set; }
    }
}
