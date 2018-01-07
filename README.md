# XBox One Controller Battery Indicator
A tray application that shows a battery indicator for an Xbox-ish controller and gives a notification when the battery level drops to (almost) empty. 

**Download link for a "ready to run" version at the bottom of this page!**  
(The green download button at the top is for the source code package)

It was originally written for the XBox One controller since Microsoft dropped all visual hints for low battery, but it should work with any gamepad that can be addressed via XInput (which should be all controllers that work in XBox-controller-enabled games).

With version 1.1.0 the polling routine was changed to support up to 4 controllers at once. When more than one controller is present, the tray icon will cycle through the status display every 5 seconds.

![Tray icon](https://i.imgur.com/rxWAsu8.gif "Tray icon cycling through multiple controllers")

Also with version 1.1.0, the notification was changed from the "old" balloon note system (used in Windows versions up to 7) to the newer "toast" messages introduced with Windows 8. This (probably) means that battery warning won't work on Windows 7 any longer.

![Toast](https://i.imgur.com/jUmqs6f.png "Toast message with low battery warning")

Controllers reported as working/being recognized so far:
* XBOne + dongle
* XBOne Elite + dongle
* XBOne S + dongle 
* XBOne S + Bluetooth
* XB360 

Currently known issues/limitations:
* initial recognition of a newly connected controller can take a while. It will be displayed as "waiting for battery level data" at first but should switch to battery level after ~10 seconds and a button press. (This might be a limitation of the XInput API.)

**[You can download the latest version here](https://github.com/NiyaShy/XB1ControllerBatteryIndicator/releases).**