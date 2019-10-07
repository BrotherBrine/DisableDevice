using devcon.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ToggleHID
{
    /// <summary>
    /// Interaction logic for EditDevice.xaml
    /// </summary>
    
    public partial class EditDevice : Window
    {
        public Device EditingDevice;
        public EditDevice(Device device)
        {
            InitializeComponent();
            EditingDevice = device;
            DeviceName.Text = !string.IsNullOrEmpty(device.FriendlyName) ? device.FriendlyName : device.Name;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //EditingDevice.FriendlyName = DeviceName.Text;
            ((MainWindow)Application.Current.MainWindow).UpdateDeviceFriendlyName(EditingDevice, DeviceName.Text);
            Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
