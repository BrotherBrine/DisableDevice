using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devcon.framework
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsEnabled { get; set; }
        public string FriendlyName { get; set; }

        public static explicit operator Device(string v)
        {
            throw new NotImplementedException();
        }
    }
}
