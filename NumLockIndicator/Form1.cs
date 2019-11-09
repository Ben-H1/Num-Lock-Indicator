using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Management.Instrumentation;
using System.Collections.Specialized;
using System.Threading;

namespace NumLockIndicator
{
    public partial class Form1 : Form
    {

        NotifyIcon numLockIcon;
        Icon activeIcon;
        Icon idleIcon;
        Thread numLockWorker;

        public Form1()
        {
            InitializeComponent();

            //Load icons from files into objects
            activeIcon = new Icon("Num_Active.ico");
            idleIcon = new Icon("Num_Idle.ico");

            //Create notify icons and assign idle icon and show it
            numLockIcon = new NotifyIcon();
            numLockIcon.Icon = idleIcon;
            numLockIcon.Visible = true;

            //Create all context menu items and add them to the notification tray icon
            MenuItem progNameMenuItem = new MenuItem("Num Lock Indicator v1.1 by Ben Hawthorn");
            MenuItem quitMenuItem = new MenuItem("Quit");
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(progNameMenuItem);
            contextMenu.MenuItems.Add(quitMenuItem);
            numLockIcon.ContextMenu = contextMenu;

            //Wire up quit button to close application
            quitMenuItem.Click += QuitMenuItem_Click;

            //Hide the form because we don't need it, this is a notification tray application
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            //Start worker thread that pulls num lock status
            numLockWorker = new Thread(new ThreadStart(NumLockThread));
            numLockWorker.Start();

        }

        private void QuitMenuItem_Click(object sender, EventArgs e)
        {
            //Close the application on click of 'Quit' button on conext menu
            numLockIcon.Dispose();
            this.Close();
        }

        //This is the thread that pulls the num lock status and updates the notification icon
        public void NumLockThread()
        {
            try
            {
                //Main loop where all the magic happens
                while(true)
                {
                    if (Control.IsKeyLocked(Keys.NumLock))
                        numLockIcon.Icon = activeIcon;
                    else
                        numLockIcon.Icon = idleIcon;

                    Thread.Sleep(100);
                }
            } catch(ThreadAbortException tbe)
            {

            }
        }
    }
}
