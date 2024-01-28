using System.Windows;

namespace Client;

internal static class Program
{
    [STAThread]
    public static void Main()
    {
        Application app = new Application();

        app.Run(new MainWindow());
    }
}
