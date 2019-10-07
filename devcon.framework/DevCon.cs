using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devcon.framework
{
    public class DevCon
    {
        public bool Toggle(bool enable, string path)
        {
            bool enabled = false;
            string args = enable ? "enable @\"" + path : "disable @\"" + path;
            var process = new Process();
            var startinfo = new ProcessStartInfo(@"devcon");
            startinfo.Arguments = args;
            startinfo.RedirectStandardOutput = true;
            startinfo.UseShellExecute = false;
            startinfo.CreateNoWindow = true;
            process.StartInfo = startinfo;
            process.OutputDataReceived += (sender, argus) => {
                if (argus != null)
                {
                    if (argus.Data != null)
                    {
                        var data = argus.Data;
                        if (data.Contains("Disabled"))
                        {
                            enabled = false;
                        }
                        if (data.Contains("Enabled"))
                        {
                            enabled = true;
                        }
                    }
                }
            };
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
            return enabled;
        }

        public bool DeviceStatus(string path)
        {
            Device tempDevice = new Device();
            bool enabled = false;
            string getStatus = "status @\"" + path;
            var process = new Process();
            var startinfo = new ProcessStartInfo(@"devcon");
            startinfo.Arguments = getStatus;
            startinfo.RedirectStandardOutput = true;
            startinfo.UseShellExecute = false;
            startinfo.CreateNoWindow = true;
            process.StartInfo = startinfo;
            process.OutputDataReceived += (sender, argus) => {
                    var data = argus.Data;
                if (data != null)
                {
                    if (data.Contains("Driver is running."))
                    {
                        enabled = true;
                    }
                }
            };
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
            return enabled;
        }
        
        public List<Device> Devices(string className)
        {
            List<Device> DeviceList = new List<Device>();
            Device tempDevice = new Device();
            string findHIDClass = $"find ={className}";
            var process = new Process();
            var startinfo = new ProcessStartInfo(@"devcon");
            startinfo.Arguments = findHIDClass;
            startinfo.RedirectStandardOutput = true;
            startinfo.UseShellExecute = false;
            startinfo.CreateNoWindow = true;
            process.StartInfo = startinfo;
            process.OutputDataReceived += (sender, argus) => {
                try
                {
                    tempDevice = new Device();
                    var id = 0;
                    if (argus.Data != null)
                    {
                        var data = argus.Data;
                        if (data.Contains(":"))
                        {
                            var parts = data.Split(':');
                            if (parts.Length == 2)
                            {
                                tempDevice.Name = parts[1].Trim();
                                tempDevice.Path = parts[0].Trim();
                                tempDevice.Id = id;
                                DeviceList.Add(tempDevice);
                                id++;
                            }
                        }
                        else
                        {

                        }
                    }
                }
                catch(Exception ex)
                {

                }
            };
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();

            return DeviceList;
        }
    }
}
