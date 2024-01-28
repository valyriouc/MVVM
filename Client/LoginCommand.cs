using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using BusinessLogic;

namespace Client.ViewModels;

internal class LoginCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public MainViewModel ViewModel { get; }

    public LoginCommand(MainViewModel vm)
    {
        ViewModel = vm;
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        if (string.IsNullOrWhiteSpace(ViewModel.Username))
        {
            throw new Exception("Empty username is not allowed!");
        }

        if (string.IsNullOrWhiteSpace(ViewModel.Password))
        {
            throw new Exception("Empty password is not allowed!");
        }

        IAsyncEnumerable<User> users = DbAccess.Instance.GetAsync<User>("SELECT * FROM User;");

        User? user = users.ToBlockingEnumerable().FirstOrDefault(x => x.Username == ViewModel.Username);
        if (user is null)
        {
            DbAccess.Instance.Create($"INSERT INTO User(username, password) VALUES('{ViewModel.Username}', '{ViewModel.Password}'");
        }
    }
}
