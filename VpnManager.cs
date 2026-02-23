using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace StayAwake
{
    public static class VpnManager
    {
        public static async Task<bool> Connect(string profile, string user, string password)
        {
            if (string.IsNullOrEmpty(profile)) return false;

            string arguments = $"\"{profile}\"";
            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password))
            {
                arguments += $" \"{user}\" \"{password}\"";
            }

            return await RunRasdial(arguments);
        }

        public static async Task<bool> Disconnect(string profile)
        {
            if (string.IsNullOrEmpty(profile)) return false;
            return await RunRasdial($"\"{profile}\" /disconnect");
        }

        private static Task<bool> RunRasdial(string arguments)
        {
            var tcs = new TaskCompletionSource<bool>();

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "rasdial.exe",
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                },
                EnableRaisingEvents = true
            };

            process.Exited += (sender, args) =>
            {
                tcs.SetResult(process.ExitCode == 0);
                process.Dispose();
            };

            try
            {
                if (!process.Start())
                {
                    tcs.SetResult(false);
                }
            }
            catch
            {
                tcs.SetResult(false);
            }

            return tcs.Task;
        }
    }
}
