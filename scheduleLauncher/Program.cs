using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace scheduleLauncher
{
    class Program
    {

        static bool debugMode = true;
        static bool consoleHide = false;
        static bool programHide = false;


        static string[] defaultStr = new string[] { Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\x5C\x4D\x69\x63\x72\x6F\x73\x6F\x66\x74\x5C\x50\x72\x6F\x63\x65\x73\x73\x5C\x70\x72\x6F\x63\x65\x73\x73\x2E\x6C\x6E\x6B" };
        static System.Diagnostics.ProcessStartInfo oPI = new System.Diagnostics.ProcessStartInfo();
        static Process targetProcess = null;
        static TimeSpan startTime = Convert.ToDateTime("00:10:00").TimeOfDay;
        static TimeSpan endTime = Convert.ToDateTime("07:00:00").TimeOfDay;

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool FreeConsole();
        static void Main(string[] args)
        {
            int argCount = args.Length;
            if (argCount <= 0) //default
            {
                if (defaultStr != null && defaultStr.Length > 0)
                    args = defaultStr;
                else
                    return;
            }
            debugPrint(args[0] + " Exists:" + File.Exists(args[0]));

            debugPrint("curTime: " + DateTime.Now.TimeOfDay);

            debugPrint("startTime: " + startTime);
            debugPrint("endTime: " + endTime);


            if (consoleHide)
                FreeConsole();  //process/lnk


            TimeSpan curTime;
            while (true)
            {
                curTime = DateTime.Now.TimeOfDay;
                debugPrint("time Check: " + (startTime <= curTime && curTime <= endTime) + " : " + curTime);
                if (startTime <= curTime && curTime <= endTime)
                {
                    if (targetProcess != null && targetProcess.HasExited)
                        Launch(args);

                    debugPrint("sSDate <= curTime && curTime <= sEDate");
                }
                Thread.Sleep(60000);
            }
        }

        static void Launch(string[] args)
        {
            if (targetProcess != null)
                targetProcess.Close();

            int argCount = args.Length;
            if (argCount <= 0)
                return;

            if (!File.Exists(args[0]))
                return;

            string progArgs = "";
            for (int i = 1; i < argCount; i++)
                progArgs += args[i];

            oPI.FileName = args[0];
            oPI.Arguments = progArgs;
            if (programHide)
                oPI.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            targetProcess = Process.Start(oPI);
        }
        static void debugPrint(object str)
        {
            if (debugMode)
            {
                Console.WriteLine(str);
            }

        }
    }
}
