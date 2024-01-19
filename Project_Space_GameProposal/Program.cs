using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Project_Space_GameProposal {

    internal class Program : Form {

        Random rnd = new Random();
        static private UIForms hud = new UIForms();

        //Last Key Pressed
        Keys lastInp = new Keys();
        bool moving;

        //Timer Declarations:
        private Timer windowTick;
        private Timer gameTick;
        private Timer moveTimer;
        private System.ComponentModel.IContainer components;

        //Picture Declarations:
        PictureBox playerShip;
        PictureBox[] stars = new PictureBox[20];
        PictureBox[] asteroids = new PictureBox[8];

        //Hitbox Triggers
        PictureBox[] collisions = new PictureBox[2];

        //Image
        Image[] shipDirection = {
            Properties.Resources.ShipL,
            Properties.Resources.ShipM_144x160,
            Properties.Resources.ShipR
        };

        Image background = Properties.Resources.BG_512x1536;

        //Player
        static public PlayerStats newPlayer = new PlayerStats();

        static void Main(string[] args) {
            new ConsoleMenu().ConsoleDevMenu();
            //Application.Run(new Program());
        }

        //global positions
        public int background_y = -1024;

        public Program() {

            InitializeComponent();
            LoadElements();
            Timers();

            //hud = new UIForms();
            hud.StartPosition = FormStartPosition.Manual;
            if (!hud.IsDisposed) hud.Show(this);
            else {
                hud = new UIForms();
                hud.Show(this);
            }
        }

        private void Timers() {

            components = new System.ComponentModel.Container();
            gameTick = new Timer(this.components);
            gameTick.Enabled = true;
            gameTick.Interval = 20;
            gameTick.Tick += new EventHandler(Game_Tick);

            moveTimer = new Timer(this.components);
            moveTimer.Interval = 20;
            moveTimer.Tick += new EventHandler(Move_Tick);

            windowTick = new Timer(this.components);
            windowTick.Interval = 20;
            windowTick.Tick += new EventHandler(Window_Tick);
            windowTick.Enabled = true;
        }


        private void LoadElements() {

            //Asteroids
            for (int i = 0; i < asteroids.Length; i++) {
                int size = i % 2 == 0 ? 144 / 4 : 144 / 6;
                asteroids[i] = new PictureBox {
                    Size = new Size(size, size),
                    Image = Properties.Resources.Asteroid_144x144,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Location = new Point(rnd.Next(0, 512), -rnd.Next(10, 304)),
                    BackColor = Color.Transparent
                };
                //this.Controls.Add(asteroids[i]);
            }

            //Ship
            playerShip = new PictureBox() {
                Size = new Size(144 / 2, 160 / 2),
                Image = Properties.Resources.ShipM_144x160,
                Location = new Point((this.Width / 2) - (144 / 2) / 2, this.Height / 2 + 100),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };

            //this.Controls.Add(playerShip);

            //Collision Boxs
            collisions[0] = new PictureBox() { //Vertical Hitbox of ship (body)
                Size = new Size(64 / 2, 160 / 2),
                Location = new Point((playerShip.Left + (playerShip.Width / 2)) - (64 / 2) / 2, playerShip.Top),
                BackColor = Color.Red
            };
            //this.Controls.Add(collisions[0]);

            collisions[1] = new PictureBox() { //Horizontal Hitbox of ship (wing)
                Size = new Size(144 / 2, 64 / 2),
                Location = new Point((playerShip.Left + (playerShip.Width / 2)) - (144 / 2) / 2, playerShip.Top + 48),
                BackColor = Color.Red
            };
            //this.Controls.Add(collisions[1]);


            //Stars
            for (int i = 0; i < stars.Length; i++) {
                int size = i % 2 == 0 ? 10 : 15;
                stars[i] = new PictureBox {
                    Size = new Size(size, size),
                    Image = Properties.Resources.Stars,
                    Location = new Point(rnd.Next(0, 512), -rnd.Next(10, 500)),
                    BackColor = Color.White
                };
                //this.Controls.Add(stars[i]);
            }

        }

        private void InitializeComponent() {
            this.SuspendLayout();
            // 
            // Program
            // 
            this.BackgroundImage = Properties.Resources.BG_512x1536;
            this.ClientSize = new System.Drawing.Size(512, 512);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Program";
            //this.Location = new Point(0, 200);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Project Space";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Program_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Program_KeyUp);
            this.ResumeLayout(false);

            this.Paint += new PaintEventHandler(FormFrameUpdateBG);

        }

        //Other Methods
        private void FormFrameUpdateBG(object sender, PaintEventArgs e) {
            e.Graphics.DrawImage(background, new Point(0, background_y));
            e.Graphics.DrawImage(playerShip.Image, new Rectangle(playerShip.Left, playerShip.Top, playerShip.Width, playerShip.Height));

            //Draw Asteroids
            for (int i = 0; i < asteroids.Length; i++) e.Graphics.DrawImage(asteroids[i].Image, new Rectangle(asteroids[i].Left, asteroids[i].Top, asteroids[i].Width, asteroids[i].Height));

            //Draw Stars
            for (int i = 0; i < stars.Length; i++) e.Graphics.DrawImage(stars[i].Image, new Rectangle(stars[i].Left, stars[i].Top, stars[i].Width, stars[i].Height));
        }

        private bool IsColliding(PictureBox A, PictureBox B) {
            return !(A.Left + A.Width < B.Left || B.Left + B.Width < A.Left ||
                    A.Top + A.Height < B.Top || B.Top + B.Height < A.Top);
        }

        private void ResetPosition(PictureBox A) {
            A.Top = -A.Height - rnd.Next(200, 400);
            A.Left = rnd.Next(0, 512);
        }

        public static void UpgradeBody_Click() {
            if (newPlayer.score > 501) {
                newPlayer.score -= 500;
                newPlayer.totalHp += 100;
                newPlayer.hp = newPlayer.totalHp;
                hud.UpdateBars();
            }
        }

        public static void UpgradeFuel_Click() {
            if (newPlayer.score > 501) {
                newPlayer.score -= 500;
                newPlayer.totalFuel += 100;
                newPlayer.fuel = newPlayer.totalFuel;
                hud.UpdateBars();
            }
        }

        public static void UpgradeWings_Click() {
            if (newPlayer.score > 501 && newPlayer.speed < 15) {
                newPlayer.score -= 500;
                newPlayer.speed += 1;
            }
        }

        //Time Methods:
        private void Game_Tick(object sender, EventArgs e) {
            //Stars
            for (int i = 0; i < stars.Length; i++) {
                stars[i].Top += (i % 2 == 0 ? 3 : 9);
                if (stars[i].Top >= 512) ResetPosition(stars[i]);
            }

            //Asteroids
            for (int i = 0; i < asteroids.Length; i++) {
                asteroids[i].Top += (i % 2 == 0 ? 6 : 12);
                if (IsColliding(collisions[0], asteroids[i]) || IsColliding(collisions[1], asteroids[i])) {
                    newPlayer.TakeDamage();
                    ResetPosition(asteroids[i]);
                }
                if (asteroids[i].Top >= 512) ResetPosition(asteroids[i]);
            }
            //Update Score
            if (newPlayer.score >= 0) newPlayer.score += 3;

            //Move Checker
            if (!moving) playerShip.Image = shipDirection[1];

            //Running Fuel
            newPlayer.fuel -= newPlayer.deducFuel;

            //Update Stats to other class
            hud.UpdateBars(newPlayer);

            //Check Stats
            if (newPlayer.fuel < 1 || newPlayer.hp < 1) GameOver();


            //Background
            this.Invalidate();

            if (background_y < 0) background_y += 8;
            else background_y = -1024;
        }

        private void Move_Tick(object sender, EventArgs e) {

            if (lastInp == Keys.A) {
                playerShip.Image = shipDirection[0];
                if (playerShip.Left > 0) {
                    playerShip.Left -= newPlayer.speed;
                }
            }

            if (lastInp == Keys.D) {
                playerShip.Image = shipDirection[2];
                if (playerShip.Left + playerShip.Width < 512) {
                    playerShip.Left += newPlayer.speed;
                }
            }
            collisions[0].Left = (playerShip.Left + (playerShip.Width / 2)) - (64 / 2) / 2;
            collisions[1].Left = playerShip.Left;
        }

        private void Window_Tick(object sender, EventArgs e) {
            //Relocate HUD
            hud.Left = this.Left + this.Width;
            hud.Top = this.Top;
        }

        private void Program_KeyDown(object sender, KeyEventArgs e) {
            lastInp = e.KeyData;
            if (lastInp == Keys.A || lastInp == Keys.D) {
                moving = true;
                moveTimer.Start();
            } else moving = false;
        }

        private void Program_KeyUp(object sender, KeyEventArgs e) {
            playerShip.Image = shipDirection[1];
            moveTimer.Stop();
        }

        private void GameOver() {
            gameTick.Stop();
            this.Close();
            new GameOverWindow().Show();
        }
        
        class GameOverWindow : Form {
            private PictureBox pictureBox1;
            private Panel panel2;
            private TextBox textBox1;
            private Label label1;
            private Label label2;
            private Panel panel1;
            private Button submit;

            public GameOverWindow() {
                LoadGameOver();
            }

            private void ReloadGame() {
                newPlayer.username = textBox1.Text;
                new ScoringSystem().WriteText(newPlayer);
                this.Close();
                newPlayer = new PlayerStats();

                new FormMenu().Show();
            }
            private void LoadGameOver() {
                this.submit = new Button();
                this.panel1 = new System.Windows.Forms.Panel();
                this.panel2 = new System.Windows.Forms.Panel();
                this.textBox1 = new System.Windows.Forms.TextBox();
                this.label1 = new System.Windows.Forms.Label();
                this.pictureBox1 = new System.Windows.Forms.PictureBox();
                this.label2 = new System.Windows.Forms.Label();
                this.panel1.SuspendLayout();
                this.panel2.SuspendLayout();
                this.SuspendLayout();

                this.panel1.BackColor = System.Drawing.Color.Black;
                this.panel1.Controls.Add(this.panel2);
                this.panel1.Controls.Add(this.pictureBox1);
                this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
                this.panel1.Location = new System.Drawing.Point(0, 0);
                this.panel1.Size = new System.Drawing.Size(996, 302);

                this.panel2.BackColor = System.Drawing.Color.White;
                this.panel2.Controls.Add(this.submit);
                this.panel2.Controls.Add(this.label2);
                this.panel2.Controls.Add(this.textBox1);
                this.panel2.Controls.Add(this.label1);
                this.panel2.Location = new System.Drawing.Point(356, 144);
                this.panel2.Size = new System.Drawing.Size(284, 216);

                this.submit.Size = new Size(90, 23);
                this.submit.Text = "Submit.";
                this.submit.Location = new Point((284/2)-45, 82+23);
                this.submit.Click += (sender, e) => ReloadGame();

                this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.textBox1.Location = new System.Drawing.Point(17, 45);
                this.textBox1.Size = new System.Drawing.Size(238, 34);
                this.textBox1.TabIndex = 1;

                this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.label1.Location = new System.Drawing.Point(12, 13);
                this.label1.Size = new System.Drawing.Size(243, 29);
                this.label1.TabIndex = 0;
                this.label1.Text = "Enter Player\'s Name: ";

                this.pictureBox1.Image = Properties.Resources.Game_Over_11_18_2023;
                this.pictureBox1.Location = new System.Drawing.Point(98, -40);
                this.pictureBox1.Size = new System.Drawing.Size(800, 211);

                this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.label2.Location = new System.Drawing.Point(12, 82);
                this.label2.Size = new System.Drawing.Size(400, 29);
                this.label2.Text = $"Score: {(newPlayer.score < 0 ? "Godly Score" : $"{newPlayer.score}")}";

                this.FormBorderStyle = FormBorderStyle.None;
                this.StartPosition = FormStartPosition.CenterScreen;
                this.ClientSize = new System.Drawing.Size(996, 302);
                this.Controls.Add(this.panel1);
                this.Name = "gamover";
                this.Text = "Game Over";
                this.TransparencyKey = System.Drawing.Color.Black;
                this.panel1.ResumeLayout(false);
                this.panel2.ResumeLayout(false);
                this.panel2.PerformLayout();
                this.ResumeLayout(false);

            }
        }
    }

    class PlayerStats {
        
        Random rnd = new Random();

        public int speed = 9;
        public double hp = 100;
        public double fuel = 100;

        public int totalHp = 100;
        public int totalFuel = 100;

        public string username;
        public long score = 0;

        public double deducFuel = 0.05;
        public void TakeDamage() {
            this.hp -= rnd.Next(10, Convert.ToInt32(Math.Ceiling(this.totalHp * 0.1))); //Take 10 percent of Base HP  as damage
        }
    }
}
