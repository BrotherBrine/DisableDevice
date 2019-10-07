using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toggle
{
    public class DevCon
    {
        public Device TempDevice()
        {
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
                var data = argus.Data;
                if (data.Contains("Name"))
                {
                    tempDevice.Name = data.Substring(data.IndexOf("Name: "));
                }
            }; // do whatever processing you need to do in this handler
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
            return tempDevice;
        }
    }
}
