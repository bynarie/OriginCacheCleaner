using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OriginCacheCleaner
{

    static class Program
    {
        static void Main(String[] args)
        {
            /** I gotta poop **/
            Console.Title = "Origin cache cleaner";
            String username = Environment.UserName; // Need username for the correct directory

            //BEGIN origin directory variables
            String originDir = @"C:\ProgramData\Origin";
            String appdataRoaming = @"C:\Users\" + username + @"\AppData\Roaming\Origin";
            String appdataLocal = @"C:\Users\" + username + @"\AppData\Local\Origin";
            // END Origin directoriy variables


            // Output the directories to be modified
            Console.WriteLine("The following directories will be modified:");
            Console.WriteLine(originDir);
            Console.WriteLine(appdataRoaming);
            Console.WriteLine(appdataLocal);

            // Double checking to make sure user wants to modify
            Console.WriteLine("\nIs this ok? (Y/n)");

            // Store pressed key into 'ck1' variable
            ConsoleKeyInfo ck1 = Console.ReadKey();
       

            // Check if the 'Y' key is pressed
            if (ck1.Key == ConsoleKey.Y)
            {

                "\nRunning Tasks...".WriteLn();

                try
                {
                    Task p1 = Task.Run(() => EndProcess());
                    p1.Wait();

                }
                catch (Exception e)
                {
                    e.Message.WriteLn();
                }

                try
                {
                    Task t1 = Task.Factory.StartNew(() => DeleteCache(originDir));
                    Task t2 = Task.Factory.StartNew(() => DeleteCache(appdataRoaming));
                    Task t3 = Task.Factory.StartNew(() => DeleteCache(appdataLocal));
                    Task.WaitAll(t1, t2, t3);



                    if (t1.IsCompletedSuccessfully && t2.IsCompletedSuccessfully && t3.IsCompletedSuccessfully)
                    {
                        "Cache has successfully been cleaned...".WriteLn();
                        "Press any key to exit...".WriteLn();
                        Console.ReadKey();

                    }
                    else
                    {
                        if (t1.IsFaulted) t1.Status.ToString().WriteLn();
                        if (t2.IsFaulted) t2.Status.ToString().WriteLn();
                        if (t2.IsFaulted) t3.Status.ToString().WriteLn();


                    }
                }
                catch (Exception e)
                {
                    e.Message.WriteLn();
                }

            }


            else

            {
                "Exiting process...\n".WriteLn();
                Environment.Exit(0);

            }

        }

        public static void WriteLn(this string str)
        {
            Console.WriteLine(str);

        }

        // Method to kill all Origin processes, so content can be deleted
        public static void EndProcess()
        {
            foreach (var process in Process.GetProcessesByName("Origin"))
            {
                process.Kill();
            }
            foreach (var process in Process.GetProcessesByName("OriginWebHelperService"))
            {
                process.Kill();
            }

            Process.GetProcesses().Where(x => x.ProcessName.ToLower().Contains("origin")).ToList().ForEach(x => x.Kill());
        }
        public static void DeleteCache(string dir1)
        {
            string[] files = Directory.GetFiles(dir1);
            string[] dirs = Directory.GetDirectories(dir1);


            foreach (string file in files)
            {
                File.Delete(file);
                file.WriteLn();

            }

            foreach (string dir in dirs)
            {
                if (dir.Contains("LocalContent"))
                {

                }
                else
                {
                    Directory.Delete(dir, true);
                    (dir + "\\").WriteLn();
                }
            }

        }
    }
}
