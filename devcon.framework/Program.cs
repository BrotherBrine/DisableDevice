using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Reflection;

namespace devcon.framework
{
    class Program
    {
        static void Main(string[] args)
        {
            //devcon listclass HIDClass
            //devcon hwids =HIDClass
            Device tempDevice = new Device();
            string getStatus = "status @\"HID\\DLL06E5&COL02\\5&6C30449&0&0001\"";
            string enable = "enable @\"HID\\DLL06E5&COL02\\5&6C30449&0&0001\"";
            string disable = "disable @\"HID\\DLL06E5&COL02\\5&6C30449&0&0001\"";
            var process = new Process();
            var startinfo = new ProcessStartInfo(@"C:\Users\Zachary Stevenson\Desktop\devcon");
            startinfo.Arguments = getStatus;
            startinfo.RedirectStandardOutput = true;
            startinfo.UseShellExecute = false;
            startinfo.CreateNoWindow = true;
            process.StartInfo = startinfo;
            process.OutputDataReceived += (sender, argus) => {

                var returnString = argus.Data;

                }; // do whatever processing you need to do in this handler
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
            //ProcessStartInfo info = new ProcessStartInfo(@"C:\Users\Zachary Stevenson\Desktop\devcon");
            //info.Arguments = "disable @\"HID\\DLL06E5&COL02\\5&6C30449&0&0001\"";
            //Process.Start(info);
        }
    }
}
