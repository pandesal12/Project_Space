using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Project_Space_GameProposal {
    internal class UIForms : Form {

        //Status
        PlayerStats uiPlayer = new PlayerStats();

        private Panel panel1;
        public PictureBox[] barUI = new PictureBox[2];
        public PictureBox[] barPlayer = new PictureBox[2];

        public Button upgradeBody = new Button();
        public Button upgradeFuel = new Button();
        public Button upgradeWings= new Button();

        Image[] barTypes = {
            Properties.Resources.HP_256x40,
            Properties.Resources.Fuel_256x40,
        };
        private Label statistics;
        Timer barTimer = new Timer();


        private void InitializeComponent() {
            this.panel1 = new System.Windows.Forms.Panel();
            this.statistics = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkKhaki;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(256, 364);
            this.panel1.TabIndex = 0;
            // 
            // UIForms
            // 
            this.ClientSize = new System.Drawing.Size(256, 364);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UIForms";
            this.TransparencyKey = System.Drawing.Color.DarkKhaki;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void LoadUI() {

            for (int i = 0; i < 2; i++) {
                barUI[i] = new PictureBox();
                barUI[i].Size = new Size(256, 40);
                barUI[i].Image = Properties.Resources.BarUI_256x40;
                barUI[i].SizeMode = PictureBoxSizeMode.Normal;
                barUI[i].Location = new Point(0, (i*48));
                panel1.Controls.Add(barUI[i]);

                barPlayer[i] = new PictureBox();
                barPlayer[i].Size = barUI[i].Size;
                barPlayer[i].BackColor = Color.Transparent;
                barPlayer[i].Image = barTypes[i];
                barPlayer[i].SizeMode = PictureBoxSizeMode.Normal;
                barUI[i].Controls.Add(barPlayer[i]);
            }

            statistics = new Label();
            statistics.Location = new Point(0, barUI[1].Top + barUI[1].Height + 16);
            statistics.BackColor = Color.White;
            statistics.Size = new Size(barUI[0].Width / 2, 50);
            statistics.Text = $"Score: {uiPlayer.score}";
            panel1.Controls.Add(statistics);

            upgradeBody = new Button();
            upgradeBody.BackColor = Color.White;
            upgradeBody.Location = new Point(0, statistics.Top + statistics.Height + 16);
            upgradeBody.Size = new Size(barUI[0].Width / 2, 50);
            upgradeBody.Text = "Upgrade Body\n[Cost]\n500 Score-points";
            panel1.Controls.Add(upgradeBody);

            upgradeFuel = new Button();
            upgradeFuel.BackColor = Color.White;
            upgradeFuel.Location = new Point(0, upgradeBody.Top + upgradeBody.Height + 16);
            upgradeFuel.Size = new Size(barUI[0].Width / 2, 50);
            upgradeFuel.Text = "Upgrade Fuel\n[Cost]\n500 Score-points";
            panel1.Controls.Add(upgradeFuel);

            upgradeWings = new Button();
            upgradeWings.BackColor = Color.White;
            upgradeWings.Location = new Point(0, upgradeFuel.Top + upgradeFuel.Height + 16);
            upgradeWings.Size = new Size(barUI[0].Width / 2, 50);
            upgradeWings.Text = "Upgrade Wings\n[Cost]\n500 Score-points";
            panel1.Controls.Add(upgradeWings);

            upgradeBody.Click += (sender, e) => Program.UpgradeBody_Click();
            upgradeFuel.Click += (sender, e) => Program.UpgradeFuel_Click();
            upgradeWings.Click += (sender, e) => Program.UpgradeWings_Click();

        }

        private void LoadTimers() {
            barTimer.Interval = 10;
            barTimer.Tick += new EventHandler(UpdateBars);
            barTimer.Enabled = true;
        }
        public UIForms() {
            InitializeComponent();
            LoadTimers();
            LoadUI();
        }

        //Polymorphism here:
        public void UpdateBars(object sender, EventArgs e) {

            if (uiPlayer.speed >= 15) upgradeWings.Text = "Wings - Maximum Level Reached";
            else upgradeWings.Text = "Upgrade Wings\n[Cost]\n500 Score-points";

            if (barPlayer[0].Width > 256 * (uiPlayer.hp/uiPlayer.totalHp)) {
                barPlayer[0].Width -= 4;
            }

            if (barPlayer[1].Width > 256 * (uiPlayer.fuel / uiPlayer.totalFuel)) {
                barPlayer[1].Width -= 4;
            }
            statistics.Text = $"Score: {(uiPlayer.score<0? "Godly Score" : $"{uiPlayer.score}")}\nHealth: {uiPlayer.hp}/{uiPlayer.totalHp}\nFuel: {Math.Floor(uiPlayer.fuel)}/{uiPlayer.totalFuel}";
            
        }

        public void UpdateBars() {
            for (int i = 0; i < 2; i++) barPlayer[i].Width = 256;      
        }

        public void UpdateBars(PlayerStats user) {
            uiPlayer = user;
        }        
    }
}
