using System.Windows.Input;

namespace Client.ViewModels;

public class MainViewModel
{
	private string username;

	public string Username
	{
		get { return username; }
		set { username = value; }
	}

	private string password;

	public string Password
	{
		get { return password; }
		set { password = value; }	
	}

    public ICommand LoginCommand
	{
		get
		{
			return new LoginCommand(this);
        }
	}
}
