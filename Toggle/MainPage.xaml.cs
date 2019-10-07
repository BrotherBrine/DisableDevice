using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Toggle
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DevCon dc = new DevCon();
        public MainPage()
        {
            this.InitializeComponent();
            //Process.Start(@"C:\Projects\toggle_trackpad\ToggleHID\disabletrackpad.bat");
            testc();
        }
        public async void testc()
        {
            Device tempDevice = new Device();
            tempDevice = dc.TempDevice();
            DeviceTextBlock.Text = tempDevice.Name;
        }
    }
}
