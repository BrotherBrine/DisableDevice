# DisableDevice
A utility for disabling a hardware device

Usage:
Originally dedesinged for disabling my Dell Touchpad programmatically. Choosing a hardware class type from the left column populates a list of hardware devices in the class on the right column. Clicking the Disable/Enable button will handle that action for the selected device. Additionally, hitting "F7" at any time while the program is running will toggle the selected device.

Note: This program utilizes [devcon](https://docs.microsoft.com/en-us/windows-hardware/drivers/devtest/devcon) which is included in this project for now until I can figure out how to reference and the C++ code from C#. This is basically a simple UI to execute commands to list devices by class, and enable or disable them. Some hardware items require a reboot to turn on or off. Those devices will not be altered by this program because the reboot command will not be sent. 

Next steps:
- Clean up the UI and make it pretty
- Add functionality for modifiers and other hotkeys
- Add functionality for adding/removing devices to a favorites screen and make that default
- Add option to launch at startup
- Add option for reboot command (no hotkey)
- Provide alert (or some indication) that device cannot be disabled due to reboot option not provided.
