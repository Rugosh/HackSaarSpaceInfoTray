using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace MyTrayApp {
    public partial class SpaceInfoTray : Form {
        [STAThread]
        public static void Main() {
            Application.Run(new SpaceInfoTray());
        }

        private const string spaceApiStatus = "https://spaceapi.hacksaar.de/status.txt";

        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;

        private Icon spaceOpenIcon;
        private Icon spaceClosedIcon;
        private Icon spaceStateErrorIcon;

        private Timer spaceStateCheckTimer;

        public SpaceInfoTray() {
            spaceOpenIcon = new Icon("Images\\SpaceOpen.ico", 40, 40);
            spaceClosedIcon = new Icon("Images\\SpaceClosed.ico", 40, 40);
            spaceStateErrorIcon = new Icon(SystemIcons.Error, 40, 40);

            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);

            trayIcon = new NotifyIcon();
            trayIcon.Text = "Hack Saar Space State";
            trayIcon.Icon = spaceStateErrorIcon;

            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;

            spaceStateCheckTimer = new Timer();
            spaceStateCheckTimer.Interval = 10000;
            spaceStateCheckTimer.Tick += CheckSpaceState;
            spaceStateCheckTimer.Enabled = true;

            CheckSpaceState(null, null);
        }

        private void CheckSpaceState(object sender, EventArgs e) {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(spaceApiStatus);
            StreamReader reader = new StreamReader(stream);
            String content = reader.ReadToEnd();

            if (content.Equals("1")) {
                trayIcon.Icon = spaceOpenIcon;
            } else {
                trayIcon.Icon = spaceClosedIcon;
            }
        }

        protected override void OnLoad(EventArgs e) {
            this.Visible = false;
            this.ShowInTaskbar = false;

            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e) {
            spaceStateCheckTimer.Enabled = false;
            Application.Exit();
        }

    }
}