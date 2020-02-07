using System;
using System.Windows.Forms;
using System.Drawing;
using System.Timers;
using System.Diagnostics;
using System.Security.Principal;

namespace ConvertibleTray
{
    //*****************************************************************************
    static class Program
    {
        private static NotifyIcon notico;

        //==========================================================================
        public static void Main(string[] astrArg)
        {
            WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool hasAdministrativeRight = pricipal.IsInRole(WindowsBuiltInRole.Administrator);

            if (!hasAdministrativeRight)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.WorkingDirectory = Environment.CurrentDirectory;
                startInfo.FileName = Application.ExecutablePath;
                startInfo.Verb = "runas";
                try
                {
                    Process p = Process.Start(startInfo);
                    Application.Exit();
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                    MessageBox.Show("This utility requires elevated priviledges to complete correctly.", "Error: UAC Authorisation Required", MessageBoxButtons.OK);
                    //                    Debug.Print(ex.Message);
                    return;
                }
            } else
            {
                ContextMenu cm;
                MenuItem miCurr;
                int iIndex = 0;

                // Kontextmenü erzeugen
                cm = new ContextMenu();

                // Kontextmenüeinträge erzeugen
                miCurr = new MenuItem();
                miCurr.Index = iIndex++;
                miCurr.Text = "&Beenden";
                miCurr.Click += new System.EventHandler(ExitClick);
                cm.MenuItems.Add(miCurr);

                // NotifyIcon selbst erzeugen
                notico = new NotifyIcon();
                notico.Icon = new Icon("Tray.ico"); // Eigenes Icon einsetzen
                notico.Text = "Doppelklick mich!";   // Eigenen Text einsetzen
                notico.Visible = true;
                notico.ContextMenu = cm;
                notico.DoubleClick += new EventHandler(NotifyIconDoubleClick);

                var timer = new System.Timers.Timer();
                timer.Interval = 10000; // 10 seconds
                timer.Elapsed += new ElapsedEventHandler(OnTimer);
                timer.Start();
                OnTimer(null, null);

                // Ohne Appplication.Run geht es nicht
                Application.Run();

            }


        }

        //==========================================================================
        private static void ExitClick(Object sender, EventArgs e)
        {
            notico.Dispose();
            Application.Exit();
        }
        //==========================================================================
        private static void NotifyIconDoubleClick(Object sender, EventArgs e)
        {
            // Was immer du willst
        }

        public static void OnTimer(object sender, ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.
            bool isTabletMode = TabletModeDetector.IsTabletMode;
            if (isTabletMode)
            {
                KeyboardDisabler.BlockInput(true);
            }
            else
            {
                KeyboardDisabler.BlockInput(false);
            }
        }
    }
}