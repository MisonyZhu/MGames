using System;
using System.Diagnostics;
using System.Text;

namespace ProtoMessageToCS
{
    public static class ProcessTools
    {
        static Encoding encoding => Encoding.GetEncoding("GB2312");

        public static bool Run(string fileName, string arguments, string workingDir = null)
        {
            string output = string.Empty;
            string error = string.Empty;
            var p = new Process();
            if (!string.IsNullOrEmpty(workingDir))
            {
                p.StartInfo.WorkingDirectory = workingDir;
            }

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.StandardOutputEncoding = encoding;
            p.StartInfo.StandardErrorEncoding = encoding;
            p.OutputDataReceived += (sender, args) => { output += args.Data; };
            p.ErrorDataReceived += (sender, args) => { error += args.Data; };
            p.StartInfo.FileName = fileName;
            p.StartInfo.Arguments = arguments;
            p.StartInfo.Verb = "runas";
            p.Start();
            p.StandardInput.AutoFlush = true;
            p.BeginErrorReadLine();
            p.BeginOutputReadLine();
            p.WaitForExit();
            p.Close();

            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine($"Execute {fileName} {arguments} error:{error}");
            }

            return int.TryParse(output, out int resultCode) && resultCode == 1;
        }

        public static bool RunCmd(string cmd, string workingDir = null)
        {
            Console.WriteLine(cmd);
            return Run("cmd", @"/C" + cmd + "&&echo 1", workingDir);
        }
    }
}