﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChartIQ.Finsemble;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Bloomberglp.TerminalApiEx;

namespace WPFExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Finsemble FSBL;

        private void SpawnChart_Click(object sender, RoutedEventArgs e)
        {
            FSBL.RPC("LauncherClient.spawn", new List<JToken> {
                "Welcome Component",
                new JObject { ["addToWorkspace"] = true }
            }, (s, a) => { });
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            FSBL.RPC("LinkerClient.publish", new List<JToken>
            {
                new JObject {
                    ["dataType"] = "symbol",
                    ["data"] = DataToSend.Text
                }
            }, (s, a) => { });
        }

        /// <summary>
        /// The MainWindow is created by the App so that we can get command line arguments passed from Finsemble.
        /// </summary>
        /// <param name="args"></param>
        public MainWindow(string[] args)
        {
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif
            // Trigger actions on close when requested by Finsemble, e.g.:
            this.Closing += MainWindow_Closing;

            FSBL = new Finsemble(args, this); // Finsemble needs the command line arguments to connect and also this Window to manage snapping, docking etc.
            FSBL.Connect();
            FSBL.Connected += Finsemble_Connected;
        }

        private void Finsemble_Connected(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(delegate //main thread
            {
                // Initialize this Window and show it
                InitializeComponent(); // Initialize after Finsemble is connected
                FinsembleHeader.SetBridge(FSBL); // The Header Control needs a connected finsemble instance

                BlpApi.Register();

                //Styling the Finsemble Header
                /*
                FinsembleHeader.SetActiveBackground(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3C4C58")));
                FinsembleHeader.SetInactiveBackground(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#303D47")));
                FinsembleHeader.SetButtonHoverBackground(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#005BC5")));
                FinsembleHeader.SetInactiveButtonHoverBackground(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#004BA3")));
                FinsembleHeader.SetCloseButtonHoverBackground(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D30E2D")));
                FinsembleHeader.SetInactiveCloseButtonHoverBackground(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D30E2D")));
                FinsembleHeader.SetDockingButtonDockedBackground(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#005BC5")));
                FinsembleHeader.SetTitleForeground(new SolidColorBrush(Colors.White));
                FinsembleHeader.SetButtonForeground(new SolidColorBrush(Colors.White));

                FinsembleHeader.SetButtonFont(null, 14, FontStyles.Normal, FontWeights.Normal);
                FinsembleHeader.SetTitleFont(null, 14, FontStyles.Normal, FontWeights.Normal);
                */
                //FSBL.RPC("WindowClient.setWindowTitle", new List<JToken> { "Bloomberg Commander" });
                FSBL.DragAndDropClient.SetScrim(Scrim); // The Scrim Label Control is used for drag and drop.

                // Receivers for dropped data.
                FSBL.DragAndDropClient.AddReceivers(new List<KeyValuePair<string, EventHandler<FinsembleEventArgs>>>()
                {
                new KeyValuePair<string, EventHandler<FinsembleEventArgs>>("symbol", (s, args) =>
                {
                    var data = args.response["data"]?["symbol"]?["symbol"];
                    if(data != null)
                    {
                        Application.Current.Dispatcher.Invoke((Action)delegate //main thread
                        {
                            DroppedData.Content = data.ToString();
                            DataToSend.Text = data.ToString();
                        });
                    };
                })
                });

                // Emitters for data that can be dragged using the drag icon.
                FSBL.DragAndDropClient.SetEmitters(new List<KeyValuePair<string, DragAndDropClient.emitter>>()
                {
                    new KeyValuePair<string, DragAndDropClient.emitter>("symbol", () =>
                    {
                        return new JObject
                        {
                            ["symbol"] = DataToSend.Text,
                            ["description"] = "Symbol " + DataToSend.Text
                        };
                    })
                });

                FSBL.LinkerClient.LinkToChannel("group3", null, (s, a) => { });

                this.Show();

            });

            // Subscribe to Finsemble Linker Channels
            FSBL.RPC("LinkerClient.subscribe", new List<JToken>
            {
                "symbol"
            }, (error, response) =>
            {
                Application.Current.Dispatcher.Invoke(delegate //main thread
                {
                    DataToSend.Text = response?["data"]?.ToString();
                    DroppedData.Content = response?["data"]?.ToString();
                });
            });

            // Logging to the Finsemble Central Console
            /*FSBL.RPC("Logger.error", new List<JToken> {
                "Error Test"
            });

            FSBL.RPC("Logger.log", new List<JToken> {
                "Log Test"
            });

            FSBL.RPC("Logger.debug", new List<JToken> {
                "Debug Test"
            });

            FSBL.RPC("Logger.info", new List<JToken> {
                "Info Test"
            });

            FSBL.RPC("Logger.verbose", new List<JToken> {
                "Verbose Test"
            });
            */

        }

        /// <summary>
        /// Example window close handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /*if (MessageBox.Show("Close Application?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                // Cancel Closing
                e.Cancel = true;
                return;
            }*/
        }

        private void LinkToGroup_Click(object sender, RoutedEventArgs e)
        {
            FSBL.LinkerClient.LinkToChannel("group1", null, (s, r) => { });
        }

        private void SendCommand_Click(object sender, RoutedEventArgs e)
        {
            var blp_cmd = DataToSend.Text;
            var blp_security = SecurityToSend.Text;
            var blp_panel = PanelToSend.Text;
            var enumSecurity = new String[1] { blp_security };
            FSBL.RPC("Logger.info", new List<JToken>
            {
                "Running BLP command with ", blp_cmd, blp_security, blp_panel
            });
            BlpTerminal.RunFunction(blp_cmd, blp_panel, enumSecurity, null);
        }
    }
}