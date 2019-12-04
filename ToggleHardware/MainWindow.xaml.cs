using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using devcon.framework;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static ToggleHardware.GlobalHotkey;
using System.Threading;
using Gma.System.MouseKeyHook;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using Application = System.Windows.Application;
using System.ComponentModel;

namespace ToggleHardware
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        int HotkeyStrokeCount = 0;
        string SelectedHotkeys = "";
        ObservableCollection<string> oc = new ObservableCollection<string>();
        List<Device> Devices = new List<Device>();
        Dictionary<string, string> DeviceDictionary;
        Dictionary<string, string> SelectedItemDictionary;
        List<string> Classes = new List<string>();

        ObservableCollection<string> TempHotkeys = new ObservableCollection<string>();

        public bool Recording = false;


        public DevCon dc = new DevCon();
        public bool Enabled = false;

        private IKeyboardMouseEvents m_Events;

        public NotifyIcon notifyIcon;


        private GlobalHotkey ghk;

        private static KeyboardListener KListener = null;
        private static IKeyboardMouseEvents HookEvents = null;


        private IKeyboardMouseEvents m_GlobalHook;

        public void Subscribe(IKeyboardMouseEvents events)
        {
            m_Events = events;
            m_Events.KeyDown += OnKeyDown;

        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {

            if (Recording & TempHotkeys.Count <= 3)
            {
                TempHotkeys.Add(e.KeyCode.ToString());
            }
            else if (Recording & TempHotkeys.Count > 3)
            {
                Recording = !Recording;
                StopRecording();
            }
            else
            {
                CheckForHotkeys(e.KeyCode);
            }
            //if (e.KeyCode != e.Modifiers)
            //if (e.KeyCode == Keys.F7 && devicelist.SelectedIndex > -1)
            //{
            //    dc.Toggle(!Enabled, (Devices[devicelist.SelectedIndex]).Path);
            //    GetStatus();
            //}
        }

        public void CheckForHotkeys(Keys keyCode)
        {

        }

        public void Unsubscribe(object sender, CancelEventArgs e)
        {
            m_Events.KeyDown -= OnKeyDown;
        }


        public MainWindow()
        {
            InitializeComponent();
            Subscribe(Hook.GlobalEvents());
            Classes.Add("USBDevice");
            Classes.Add("Camera");
            Classes.Add("Bluetooth");
            Classes.Add("WPD");
            Classes.Add("HIDClass");
            Classes.Add("Mouse");
            classlist.ItemsSource = Classes;
            notifyIcon = new NotifyIcon()
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = new System.Drawing.Icon($@"C:\Projects\ToggleHardware\ToggleHardware\Z3Utilities.ico"),
                Text = "ToggleDevice",
                Visible = false
            };
            notifyIcon.DoubleClick += new EventHandler(NotifyIconDoubleClickEvent);
            if (JsonConvert.DeserializeObject<Dictionary<string, string>>(Properties.Settings.Default.deviceDictionary) != null)
            {
                DeviceDictionary = new Dictionary<string, string>(JsonConvert.DeserializeObject<Dictionary<string, string>>(Properties.Settings.Default.deviceDictionary));
            }
            else
            {
                DeviceDictionary = new Dictionary<string, string>();
            }
            if (JsonConvert.DeserializeObject<Dictionary<string, string>>(Properties.Settings.Default.selectedItemDictionary) != null)
            {
                SelectedItemDictionary = new Dictionary<string, string>(JsonConvert.DeserializeObject<Dictionary<string, string>>(Properties.Settings.Default.selectedItemDictionary));
            }
            else
            {
                SelectedItemDictionary = new Dictionary<string, string>();
            }
            try
            {
                classlist.SelectedItem = Properties.Settings.Default.selectedClass;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }


        public void UpdateDeviceFriendlyName(Device device, string FriendlyName)
        {
            if (DeviceDictionary.ContainsKey(device.Path))
            {
                DeviceDictionary[device.Path] = FriendlyName;
            }
            else { 
                DeviceDictionary.Add(device.Path, FriendlyName); 
            }
            Properties.Settings.Default.deviceDictionary = JsonConvert.SerializeObject(DeviceDictionary);
            Properties.Settings.Default.Save();

            ListDevices(classlist.SelectedItem.ToString()); 
            try
            {
                devicelist.SelectedItem = JsonConvert.DeserializeObject<Device>(Properties.Settings.Default.selectedDevice).Name;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void WindowStateChangedEvent(object sender, EventArgs e)
        {
            MainWindow window = sender as MainWindow;
            if (window.WindowState == WindowState.Minimized)
            {
                Hide();
                notifyIcon.Visible = true;
            }
        }

        private void NotifyIconDoubleClickEvent(object sender, EventArgs e)
        {
            Show();
            notifyIcon.Visible = false;
            this.WindowState = WindowState.Normal;
        }

        private void ListDevices(string className)
        {            
            Devices.Clear();
            Devices = dc.Devices(className);
            if (DeviceDictionary != null)
            {
                foreach (Device d in Devices)
                {
                    d.FriendlyName = DeviceDictionary.ContainsKey(d.Path) ? DeviceDictionary[d.Path]: d.Name;
                }
            }
            devicelist.ItemsSource = Devices;
            if (SelectedItemDictionary.ContainsKey(classlist.SelectedItem.ToString()))
            {
                Device dev = Devices.FirstOrDefault<Device>(d => d.Path == SelectedItemDictionary[classlist.SelectedItem.ToString()]);
                devicelist.ItemsSource = Devices;
                devicelist.SelectedItem = dev;
            }
        }

        private void GetStatus()
        {            
            if (devicelist.SelectedIndex >= 0)
            {
                Enabled = dc.DeviceStatus((Devices[devicelist.SelectedIndex]).Path);
                ToggleButton.IsEnabled = true;
                DeviceName.Text = $"{Devices[devicelist.SelectedIndex].FriendlyName}: ";
            }
            else
            {
                ToggleButton.IsEnabled = false;
                DeviceEnabledTextBlock.Text = "";
                DeviceName.Text = "";
            }
            DeviceEnabledTextBlock.Text = Enabled ? "  Enabled" : "  Disabled";
            DeviceEnabledTextBlock.Background = Enabled ? Brushes.Green : Brushes.Red;
            DeviceEnabledTextBlock.Foreground = Brushes.White;
            ToggleButton.Content = Enabled ? "Disable" : "Enable";
        }

        private void Devicelist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetStatus();
            if (classlist.SelectedItem != null && devicelist.SelectedIndex >= 0)
            {
                if (SelectedItemDictionary.ContainsKey(classlist.SelectedItem.ToString())) { 
                    SelectedItemDictionary[classlist.SelectedItem.ToString()] = Devices[devicelist.SelectedIndex].Path;
                }
                else { 
                    SelectedItemDictionary.Add(classlist.SelectedItem.ToString(), Devices[devicelist.SelectedIndex].Path); 
                }
                Properties.Settings.Default.selectedDevice = JsonConvert.SerializeObject(Devices[devicelist.SelectedIndex]);
                Properties.Settings.Default.selectedItemDictionary = JsonConvert.SerializeObject(SelectedItemDictionary);
                Properties.Settings.Default.Save();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dc.Toggle(!Enabled, (Devices[devicelist.SelectedIndex]).Path);
            GetStatus();
        }

        private void classlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ListBox listBox = sender as System.Windows.Controls.ListBox;
            string selected = listBox.SelectedItem.ToString();
            ListDevices(selected);
            Properties.Settings.Default.selectedClass = selected;
            Properties.Settings.Default.Save();
            try 
            {
                devicelist.SelectedItem = JsonConvert.DeserializeObject<Device>(Properties.Settings.Default.selectedDevice).Name;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void devicelist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditDevice editDevice = new EditDevice((Device) devicelist.SelectedItem);
            editDevice.Show();
        }

        private void RecordHotkeys_Click(object sender, RoutedEventArgs e)
        {
            Recording = !Recording;
            if (Recording)
            {
                StartRecording();
            }
            else
            {
                StopRecording();
            }

        }

        private void StartRecording()
        {
            RecordHotkeys.Content = "Stop";
            TempHotkeys = new ObservableCollection<string>();
        }

        private void StopRecording()
        {
            RecordHotkeys.Content = "Record";
            foreach (string key in TempHotkeys)
            {
                HotkeysTextblock.Text += $"{key} +";
            }
            HotkeysTextblock.Text += "++";
            HotkeysTextblock.Text.Replace("++", "");
        }
    }
}
