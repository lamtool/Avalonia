using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.TextFormatting;
using Avalonia.Styling;
using System.Collections.Generic;
using System;

namespace AvaloniaUI.Views;

public partial class fAddAccount : Window
{
    public fAddAccount()
    {
        InitializeComponent();
    }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        LoadCombobox();
    }
    private void LoadCombobox()
    {
        List<string> listField = new List<string>
            {
                FieldsModel.Empty,
                FieldsModel.Uid,
                FieldsModel.Password,
                FieldsModel._2FA,
                FieldsModel.Token,
                FieldsModel.Cookie,
                FieldsModel.Proxy,
                FieldsModel.Email,
                FieldsModel.PassMail,
                FieldsModel.UserAgent,
                FieldsModel.MailAdress,
                FieldsModel.UsernameFB,
            };

        for (int i = 0; i < listField.Count - 1; i++)
        {
            var cbx = new ComboBox
            {
                //Style = FindResource("CustomComboBoxStyle") as Style,
                ItemsSource = listField,
                Width = 90,
                SelectedIndex = i + 1
            };
            cbx.DropDownOpened += (s, e) => { cbx.Width = 200; };
            cbx.DropDownClosed += (s, e) => { cbx.Width = 75; };

            TextBlock lb = new TextBlock
            {
                Text = "|",
                FontSize = 14,
                Margin = new Thickness(5, 0, 5, 0),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
            };

            flowLayoutPanel2.Children.Add(cbx);
            flowLayoutPanel2.Children.Add(lb);
        }

        // Remove the last separator
        if (flowLayoutPanel2.Children.Count > 0)
        {
            flowLayoutPanel2.Children.RemoveAt(flowLayoutPanel2.Children.Count - 1);
        }
    }
    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void btnExit_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
        //// Retrieve text from RichTextBox
        //TextRange textRange = new TextRange(
        //    rtbAdditionalInfo.Document.ContentStart,
        //    rtbAdditionalInfo.Document.ContentEnd
        //);
        var additionalInfo = rtbAdditionalInfo.Text;

        // Count the number of lines in the TextRange
        int lineCount = 0;
        if (!string.IsNullOrEmpty(additionalInfo))
        {
            // Split by Windows-style newline (\r\n) and count non-empty lines
            string[] lines = additionalInfo.Split(new[] { "\r\n" }, StringSplitOptions.None);
            lineCount = lines.Length;
            // If the last line is empty (due to trailing \r\n), adjust the count
            if (lines.Length > 0 && string.IsNullOrEmpty(lines[lines.Length - 1]))
            {
                lineCount--;
            }
        }
        else
        {
            // Handle empty RichTextBox (considered as 0 lines)
            lineCount = 0;
        }

        // Example: Log or display the line count (replace with your desired action)
        //MessageBox.Show($"The RichTextBox contains {lineCount} line(s).", "Line Count", MessageBoxButton.OK, MessageBoxImage.Information);

        // Retrieve TextBox content (previously assigned to unused variable 's')

    }


    private void TextBlock_Tapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {// Handle title click (if needed)
    }
}
public static class FieldsModel
{
    public static string Empty => "";
    public static string Uid => "Uid";
    public static string Password => "Password";
    public static string _2FA => "2FA";
    public static string Token => "Token";
    public static string Cookie => "Cookie";
    public static string Proxy => "Proxy";
    public static string Email => "Email";
    public static string PassMail => "PassMail";
    public static string UserAgent => "UserAgent";
    public static string MailAdress => "MailAdress";
    public static string UsernameFB => "UsernameFB";
}