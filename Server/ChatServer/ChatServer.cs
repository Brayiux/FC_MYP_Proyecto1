using ChatServer.Views;
using Controller;
using System.Diagnostics;
using System.Net;

namespace ChatServer;

public class ChatServer
{
    private static ServerController? _controller;
    private static HandleMsgsView? _view;
    public async static Task Main(string[] args)
    {
        Console.WriteLine("Bienvenido al server");

        string port;

        Debugger.Launch();

        Console.CancelKeyPress += Console_CancelKeyPress;

        // Lee y valida el puerto en consola o lo inicializamos en 1234 por defecto
        if (args.Length == 0)
        {
            port = "1234";
        }
        else if (!int.TryParse(args[0], out int p) || p is < 0 or > 65535)
        {
            Console.WriteLine("Error: Puerto no válido.");
            return;
        }
        else
        {
            port = args[0];
        }

        _controller = new(IPAddress.Any, port);

        // Se crea la vista para reaccionar a eventos del controlador
        _view = new(_controller);

        await _controller.RunAsync();

        _view.Dispose();
    }

    private static void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        e.Cancel = true;

        _controller?.Stop();
        _view?.Dispose();

        Environment.Exit(0);
        
    }
}