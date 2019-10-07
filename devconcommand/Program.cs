using System;
using System.Diagnostics;
using System.IO;

namespace devconcommand
{
    class Program
    {
        static void Main(string[] args)
        {
            //ProcessStartInfo info = new ProcessStartInfo(@"C:\Users\Zachary Stevenson\Desktop\devcon");
            //info.Arguments = "disable @\"HID\\DLL06E5&COL02\\5&6C30449&0&0001\"";
            //Process.Start(info);

            testc();
        }

        static void testc()
        {
            //ProcessStartInfo info = new ProcessStartInfo(@"C:\Users\Zachary Stevenson\Desktop\disabletrackpad.bat");
            string devcon = @"C:\Users\Zachary Stevenson\Desktop\devcon.exe";
            string desktop = @"C:\Users\Zachary Stevenson\Desktop\";
            DirectoryInfo directory = new DirectoryInfo(desktop);
            //if (Directory.Exists(directory))
            if (Directory.Exists(desktop))
            {
                FileInfo file = new FileInfo(devcon);
                if (File.Exists(devcon))
                {
                    ProcessStartInfo info = new ProcessStartInfo(@"C:\Projects\toggle_trackpad\devcon.exe");
                    //info.Arguments = @"C:\Users\Zachary Stevenson\Desktop\disabletrackpad.bat";
                    info.Arguments = "disable @" + '"' + @"HID\DLL06E5&COL02\5&6C30449&0&0001" + '"';
                    try
                    {
                        Process.Start(info);
                    }
                    catch (Exception ex)
                    {
                        string d = ex.Message.ToString();
                    }
                }
            }
        }
    }
}
