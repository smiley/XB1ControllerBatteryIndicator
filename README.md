# XBox One Controller Battery Indicator
A simple tray application that shows a battery indicator for an Xbox One controller of choice.

The style is in line with the rest of the default Windows 10 tray icons:

![Screenshot](http://i.imgur.com/MZl1F2I.png "Screenshot")

Since SpoinkyNL hasn't worked on this for a while I took the liberty of forking and tinkering with the code a bit. 
So far I've mainly tried to add a notification when the controller battery reaches the "empty" state, you now get a balloon message (or for windows 10, an info center notification).
It's still far from perfect and there's probably better solutions to what I did, but it works for now.

Only draw-back of my current approach: the notification stays even when the battery level changes back to good, so you'd have to close the program to get rid of it.

A [binary release](https://github.com/NiyaShy/XB1ControllerBatteryIndicator/releases) is available for download.
