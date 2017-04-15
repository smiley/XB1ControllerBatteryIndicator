# XBox One Controller Battery Indicator
A tray application that shows a battery indicator for an Xbox-ish controller and gives a notification when the battery level drops to (almost) empty. 

It was originally written for the XBox One controller since Microsoft dropped all visual hints for low battery, but it should work with any gamepad that can be addressed via XInput (which should be all controllers that work in XBox-controller-enabled games).

![Screenshot](http://i.imgur.com/Fo7DTMa.jpg "Screenshot of tray icon")

Controllers reported as working/being recognized so far:
* XBOne + dongle
* XBOne Elite + dongle
* XBOne S + dongle 
* XBOne S + Bluetooth
* XB360 

Currently known issues/limitations:
* initial recognition of a newly connected controller can take a while. It will be displayed as "disconnected" at first but should switch to battery level after ~10 seconds and a button press
* Only one controller is recognized/monitored

A [binary release](https://github.com/NiyaShy/XB1ControllerBatteryIndicator/releases) is available for download.
