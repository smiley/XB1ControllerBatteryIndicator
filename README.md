# XBox One Controller Battery Indicator
A tray application that shows a battery indicator for an Xbox-ish controller and gives a notification when the battery level drops to (almost) empty. 

It was originally written for the XBox One controller since Microsoft dropped all visual hints for low battery, but it should work with any gamepad that can be addressed via XInput (which should be all controllers that work in XBox-controller-enabled games).

This is the "old" polling routine that only supports a single controller. The low battery warning uses the older balloon notification system that was superseded by the toast system introduced with Windows 8.

The last release version that still uses this polling/notification combo is [v1.0.2](https://github.com/NiyaShy/XB1ControllerBatteryIndicator/releases/tag/v1.0.2). All newer versions will use multi-controller/toast notify.

Controllers reported as working/being recognized so far:
* XBOne + dongle
* XBOne Elite + dongle
* XBOne S + dongle 
* XBOne S + Bluetooth
* XB360 

Known issues/limitations:
* initial recognition of a newly connected controller can take a while. It will be displayed as "disconnected" at first but should switch to battery level after ~10 seconds and a button press. (This might be a limitation of the XInput API.)