using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaUI.Models;
public class Account : INotifyPropertyChanged
{
    private string _state;
    public string AccountName { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string TwoFA { get; set; }
    public string Cookie { get; set; }
    public string Token { get; set; }
    public string Email { get; set; }
    public string EmailPassword { get; set; }
    public string RecoveryEmail { get; set; }
    public string Group { get; set; }
    public string State
    {
        get => _state;
        set { _state = value; OnPropertyChanged(nameof(State)); }
    }
    public string LastActivity { get; set; }
    public string Result { get; set; }
    public string Status { get; set; }

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            OnPropertyChanged(nameof(IsSelected));
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
