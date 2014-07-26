using System;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using DarkDefenders.Client;
using DarkDefenders.Game.Model.Other;
using Infrastructure.Runtime;

namespace DarkDefenders.ConsoleClient
{
    static class Program
    {
        private const int MaxFps = 100;
        private static readonly TimeSpan _elapsedLimit = TimeSpan.FromSeconds(1);

        static void Main()
        {
            var address = GetIp();

            var source = new CancellationTokenSource();

            var client = GameClientFactory.Create();

            client.RunAsync(source.Token, address, 1337);

            new Loop(MaxFps, _elapsedLimit).Run(elapsed => Frame(client, elapsed, source), source.Token);
        }

        private static IPAddress GetIp()
        {
            Console.Write("Enter ip to connect: ");
            var input = Console.ReadLine();

            var defaultIp = new byte[] { 192, 168, 1, 42 };
            var result = defaultIp;
            var parts = input.Split('.');

            if (parts.Length == 4)
            {
                for (var i = 0; i < 4; i++)
                {
                    var part = parts[i];
                    byte partByte;
                    if (!byte.TryParse(part, out partByte))
                    {
                        result = defaultIp;
                        break;
                    }

                    result[i] = partByte;
                }
            }

            return new IPAddress(result);
        }

        private static void Frame(IGameClient client, TimeSpan elapsed, CancellationTokenSource source)
        {
            ProcessKeyboard(client, source);

            client.Pulse(elapsed);
        }

        private static void ProcessKeyboard(IGameClient client, CancellationTokenSource source)
        {
            var leftIsPressed = NativeKeyboard.IsKeyDown(Keys.Left);
            var rightIsPressed = NativeKeyboard.IsKeyDown(Keys.Right);
            if (leftIsPressed && !rightIsPressed)
            {
                client.Publish(player => player.ChangeMovement(Movement.Left));
            }
            else if (rightIsPressed && !leftIsPressed)
            {
                client.Publish(player => player.ChangeMovement(Movement.Right));
            }
            else
            {
                client.Publish(player => player.ChangeMovement(Movement.Stop));
            }

            if (NativeKeyboard.IsKeyDown(Keys.Up))
            {
                client.Publish(player => player.Jump());
            }

            if (NativeKeyboard.IsKeyDown(Keys.LControlKey) || NativeKeyboard.IsKeyDown(Keys.RControlKey))
            {
                client.Publish(player => player.Fire());
            }

            if (NativeKeyboard.IsKeyDown(Keys.Escape))
            {
                source.Cancel();
            }
        }
    }
}
