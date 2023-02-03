using NewLife;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LuYao.Toolkit.PortProxy;

public static class CmdRunner
{
    public static string Execute(string cmd)
    {
        var proc = Process.Start(new ProcessStartInfo
        {
            FileName = "cmd",
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
        });
        proc.Start();

        proc.StandardInput.WriteLine($"{cmd} & exit");
        var output = proc.StandardOutput.ReadToEnd();

        return output;
    }

    public static void Run(List<string> cmd)
    {
        if (cmd == null || cmd.Count <= 0) return;
        var proc = Process.Start(new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/C {cmd.Join("&")}",
            UseShellExecute = true,
            CreateNoWindow = false,
            WindowStyle = ProcessWindowStyle.Normal,
            Verb = "runas",
        });
        if (proc != null) proc.WaitForExit();
    }
}