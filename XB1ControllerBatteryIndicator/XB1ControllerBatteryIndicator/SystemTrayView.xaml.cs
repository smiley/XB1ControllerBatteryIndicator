using System.Windows;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows.Data;
using System.Xml;
using XB1ControllerBatteryIndicator.Localization;

namespace XB1ControllerBatteryIndicator
{
    /// <summary>
    ///     Interaction logic for SystemTrayView.xaml
    /// </summary>
    public partial class SystemTrayView : Window
    {
        public SystemTrayView()
        {
            InitializeComponent();
        }
        RegistryKey autoStartKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        private string appID = "XB1ControllerBatteryIndicator";
        string xmlUrl = "http://xb1cbi.kienai.de/current_version.xml";
        
        //create autostart registry key
        private void StartWithWindows()
        {
            String exePath = Process.GetCurrentProcess().MainModule.FileName;
            autoStartKey.SetValue(appID, exePath);
        }
        //remove autostart key
        private void RemoveAutoStart()
        {
            autoStartKey.DeleteValue(appID, false);
        }
        //check if a newer version is available
        private void CheckForUpdate()
        {
            bool update_check = Properties.Settings.Default.UpdateCheck;
            if (update_check == true)
            {
                Version newVersion = null;
                string update_url = "";
                XmlTextReader reader;
                reader = new XmlTextReader(xmlUrl);
                try
                {
                    reader.MoveToContent();
                    string elementName = "";
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == appID))
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                                elementName = reader.Name;
                            else
                            {
                                if ((reader.NodeType == XmlNodeType.Text) && (reader.HasValue))
                                {
                                    switch (elementName)
                                    {
                                        case "version":
                                            newVersion = new Version(reader.Value);
                                            break;
                                        case "url":
                                            update_url = reader.Value;
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (reader != null) reader.Close();
                }
                if ((newVersion != null) && (update_url != ""))
                {
                    Version curVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                    if(curVersion.CompareTo(newVersion) < 0)
                    {
                        string title = Strings.NewVersionAvailable_Title;
                        string question = string.Format(Strings.NewVersionAvailable_Body, appID);
                        if (MessageBoxResult.Yes == System.Windows.MessageBox.Show(this, question, title, MessageBoxButton.YesNo, MessageBoxImage.Question))
                        {
                            System.Diagnostics.Process.Start(update_url);
                        }
                    }
                }
            }
        }
        //autostart-checkbox was clicked
        private void AutoStart_Click(object sender, RoutedEventArgs e)
        {
            //for whatever reason the autostart-Bool always had the reverse value here, so I had to negate it for the check to work...
            bool autorun_check = !Properties.Settings.Default.AutoStart;
            if (autorun_check == false)
            {
                Properties.Settings.Default.AutoStart = true;
                Properties.Settings.Default.Save();
                this.StartWithWindows();
            }
            else
            {
                Properties.Settings.Default.AutoStart = false;
                Properties.Settings.Default.Save();
                this.RemoveAutoStart();
            }
        }
        //update-checkbox was clicked
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            //as with the autostart-bool, this one has to be negated too...
            bool update_check = !Properties.Settings.Default.UpdateCheck;
            if(update_check == false)
            {
                Properties.Settings.Default.UpdateCheck = true;
                Properties.Settings.Default.Save();
                this.CheckForUpdate();
            }
            else
            {
                Properties.Settings.Default.UpdateCheck = false;
                Properties.Settings.Default.Save();
            }
        }
    }
    //this enabled using the values stored in the settings file to be used in XAML
    public class SettingBindingExtension : Binding
    {
        public SettingBindingExtension()
        {
            Initialize();
        }

        public SettingBindingExtension(string path)
            : base(path)
        {
            Initialize();
        }

        private void Initialize()
        {
            this.Source = Properties.Settings.Default;
            this.Mode = BindingMode.TwoWay;
        }
    }
}