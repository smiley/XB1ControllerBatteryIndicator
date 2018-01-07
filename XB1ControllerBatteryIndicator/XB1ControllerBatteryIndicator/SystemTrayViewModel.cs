using System.Linq;
using System.Threading;
using SharpDX.XInput;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using System.Collections.Generic;


namespace XB1ControllerBatteryIndicator
{
    public class SystemTrayViewModel : Caliburn.Micro.Screen
    {
        private string _activeIcon;
        private Controller _controller;
        private string _tooltipText;
        private const string APP_ID = "XB1ControllerBatteryIndicator";
        private bool[] toast_shown = new bool[5];
        private Dictionary<string, int> numdict = new Dictionary<string, int>();
        

        public SystemTrayViewModel()
        {
            ActiveIcon = "Resources/battery_disconnected.ico";
            Thread th = new Thread(RefreshControllerState);
            th.IsBackground = true;
            th.Start();
            numdict["One"] = 1;
            numdict["Two"] = 2;
            numdict["Three"] = 3;
            numdict["Four"] = 4;
        }

        public string ActiveIcon
        {
            get { return _activeIcon; }
            set
            {
                _activeIcon = value;
                NotifyOfPropertyChange();
            }
        }

        public string TooltipText
        {
            get { return _tooltipText; }
            set
            {
                _tooltipText = value;
                NotifyOfPropertyChange();
            }
        }

        private void RefreshControllerState()
        {
            while (true)
            {
                //Initialize controllers
                var controllers = new[]
                {
                    new Controller(UserIndex.One), new Controller(UserIndex.Two), new Controller(UserIndex.Three),
                    new Controller(UserIndex.Four)
                };
                //Check if at least one is present
                _controller = controllers.FirstOrDefault(selectControler => selectControler.IsConnected);

                if (_controller != null)
                {
                    //cycle through all recognized controllers
                    foreach (var CurrentController in controllers)
                    {
                        if (CurrentController.IsConnected)
                        {
                            var batteryInfo = CurrentController.GetBatteryInformation(BatteryDeviceType.Gamepad);
                            //wired
                            if (batteryInfo.BatteryType == BatteryType.Wired)
                            {
                                TooltipText = $"Controller {CurrentController.UserIndex} - {batteryInfo.BatteryType}";
                                ActiveIcon = $"Resources/battery_wired_{CurrentController.UserIndex.ToString().ToLower()}.ico";
                            }
                            //"disconnected", a controller that was detected but hasn't sent battery data yet has this state
                            else if (batteryInfo.BatteryType == BatteryType.Disconnected)
                            {
                                TooltipText = $"Controller {CurrentController.UserIndex} - Found but still waiting for battery data...";
                                ActiveIcon = $"Resources/battery_disconnected_{CurrentController.UserIndex.ToString().ToLower()}.ico";
                            }
                            //this state should never happen
                            else if (batteryInfo.BatteryType == BatteryType.Unknown)
                            {
                                TooltipText = $"Controller {CurrentController.UserIndex} - {batteryInfo.BatteryType}";
                                ActiveIcon = $"Resources/battery_disconnected_{CurrentController.UserIndex.ToString().ToLower()}.ico";
                            }
                            //a battery level was detected
                            else
                            {
                                TooltipText = $"Controller {CurrentController.UserIndex} - Battery level: {batteryInfo.BatteryLevel}";
                                ActiveIcon = $"Resources/battery_{batteryInfo.BatteryLevel.ToString().ToLower()}_{CurrentController.UserIndex.ToString().ToLower()}.ico";
                                //when "empty" state is detected...
                                if (batteryInfo.BatteryLevel == BatteryLevel.Empty)
                                {
                                    //check if toast (notification) for current controller was already triggered
                                    if (toast_shown[numdict[$"{CurrentController.UserIndex}"]] == false)
                                    {
                                        //if not, trigger it
                                        toast_shown[numdict[$"{CurrentController.UserIndex}"]] = true;
                                        string ToastHeadline = "XB1ControllerBatteryIndicator";
                                        string ToastText = $"Battery of controller {CurrentController.UserIndex} is (almost) empty.";
                                        ShowToast(ToastHeadline, ToastText);
                                    }
                                }

                                else
                                {
                                    //battery back to a good level, return toast check to false
                                    toast_shown[numdict[$"{CurrentController.UserIndex}"]] = false;
                                }
                            }
                            Thread.Sleep(5000);
                        }
                    }
                }
                else
                {
                    TooltipText = $"No controller detected";
                    ActiveIcon = $"Resources/battery_unknown.ico";
                }
                Thread.Sleep(1000);
            }
        }

        private void ShowToast(string ToastHeadline, string ToastText)
        {
            // Get a toast XML template
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

            // Fill in the text elements
            XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
            stringElements[0].AppendChild(toastXml.CreateTextNode(ToastHeadline));
            stringElements[1].AppendChild(toastXml.CreateTextNode(ToastText));

            // Create the toast and attach event listeners
            ToastNotification toast = new ToastNotification(toastXml);
            toast.Activated += ToastActivated;

            // Show the toast
            ToastNotificationManager.CreateToastNotifier(APP_ID).Show(toast);
        }

        private void ToastActivated(ToastNotification sender, object e)
        {
            //do nothing
        }

        public void ExitApplication()
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}