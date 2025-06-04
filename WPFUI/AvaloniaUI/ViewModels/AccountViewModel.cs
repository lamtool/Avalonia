using Avalonia;
using Avalonia.Threading;
using AvaloniaUI.Commons;
using AvaloniaUI.Models;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvaloniaUI.ViewModels;
public class AccountViewModel : INotifyPropertyChanged
{
    private List<Account> _allAccounts;
    private List<Account> _filteredAccounts;
    public RangeObservableCollection<Account> Accounts { get; }
    private readonly DispatcherTimer _searchTimer;

    public const int PageSize = 100;
    private int _currentPageIndex;
    private bool _isLoading;
    private string _searchText;

    public ICommand StopProfileCmd { get; }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            if (_isLoading != value)
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            _searchTimer.Stop();
            _searchTimer.Start();
        }
    }

    public int TotalAccounts => _allAccounts.Count;
    public int LiveAccounts => _allAccounts.Count(a => a.State == "LIVE");
    public int DieAccounts => _allAccounts.Count(a => a.State == "DIE");
    public int WarnAccounts => _allAccounts.Count(a => !string.IsNullOrEmpty(a.State) && a.State != "LIVE" && a.State != "DIE");
    public ObservableCollection<string> RunningJobs { get; } = new();
    public bool ControlsEnabled { get; set; } = true;

    public ICommand AddFolderCommand { get; }
    public ICommand ReloadCommand { get; }
    public ICommand AddAccountCommand { get; }


    public AccountViewModel(List<Account> accounts)
    {
        _allAccounts = accounts;
        _filteredAccounts = _allAccounts;
        Accounts = new RangeObservableCollection<Account>();
        _currentPageIndex = 0;

        _searchTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(300)
        };
        _searchTimer.Tick += (s, e) =>
        {
            _searchTimer.Stop();
            ApplySearch();
        };
        AddFolderCommand = new RelayCommand(OnAddFolder);
        ReloadCommand = new RelayCommand(OnReload);
        AddAccountCommand = new RelayCommand(OnAddAccount);

        OnPropertyChanged(nameof(TotalAccounts));
        OnPropertyChanged(nameof(LiveAccounts));
        OnPropertyChanged(nameof(DieAccounts));
        OnPropertyChanged(nameof(WarnAccounts));

        LoadPage(_currentPageIndex);
    }

    private void OnAddFolder()
    {
        // Implement your logic to add a folder here
    }

    private void OnReload()
    {
        // Implement reload logic here
    }

    private void OnAddAccount()
    {
        // Implement add account logic here
    }
    private void ApplySearch()
    {
        string search = SearchText?.Trim().ToLower() ?? "";
        if (string.IsNullOrEmpty(search))
        {
            _filteredAccounts = _allAccounts;
        }
        else
        {
            var props = typeof(Account).GetProperties()
                .Where(p => p.PropertyType == typeof(string))
                .ToList();

            _filteredAccounts = _allAccounts.Where(account =>
            {
                foreach (var prop in props)
                {
                    var value = prop.GetValue(account) as string;
                    if (!string.IsNullOrEmpty(value) && value.ToLower().Contains(search))
                        return true;
                }
                return false;
            }).ToList();
        }
        _currentPageIndex = 0;
        _ = LoadPage(_currentPageIndex);
    }

    public async Task<bool> LoadPage(int pageIndex)
    {
        if (pageIndex < 0)
            return false;

        var startIndex = pageIndex * PageSize;
        if (_isLoading || startIndex >= _filteredAccounts.Count)
            return false;

        try
        {
            IsLoading = true;
            await Task.Delay(100); // Simulate loading
            var newAccounts = await Task.Run(() =>
                _filteredAccounts.Skip(startIndex).Take(PageSize).ToList());

            // With this corrected line:
            await Dispatcher.UIThread.InvokeAsync(() =>            
            {
                int overflow = Accounts.Count + newAccounts.Count - PageSize;
                if (overflow > 0)
                {
                    Accounts.RemoveRange(0, overflow);
                }
                Accounts.AddRange(newAccounts);

                _currentPageIndex = pageIndex;
            });

            return true;
        }
        finally
        {
            IsLoading = false;
        }
    }

    public Task<bool> LoadNextPage() => LoadPage(_currentPageIndex + 1);
    public Task<bool> LoadPreviousPage() => LoadPage(_currentPageIndex - 1);

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

public class RelayCommand<T> : ICommand
{
    private readonly Action<T> _execute;
    public RelayCommand(Action<T> execute) => _execute = execute;
    public event EventHandler CanExecuteChanged;
    public bool CanExecute(object parameter) => true;
    public void Execute(object parameter) => _execute((T)parameter);
}
