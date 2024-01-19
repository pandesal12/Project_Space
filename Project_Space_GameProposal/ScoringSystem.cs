using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Project_Space_GameProposal {
    internal abstract class LeaderboardManager {
        protected string FilePath { get; }

        protected LeaderboardManager(string filePath) {
            FilePath = filePath;
        }

        public abstract void WriteText(PlayerStats player);
        public abstract void DisplayLeaderboard();
        protected abstract List<Player> ReadLeaderboard();
        protected abstract void AddUpdate(string playerName, long playerScore);
    }

    internal class ScoringSystem : LeaderboardManager {
        public ScoringSystem() : base(Path.Combine(Environment.CurrentDirectory, "Project_Space.txt")) {
        }

        public override void WriteText(PlayerStats player) {
            AddUpdate(player.username, player.score);
        }

        public override void DisplayLeaderboard() {
            List<Player> players = ReadLeaderboard();

            string listOfPlayers = "";

            foreach (var player in players) {
                listOfPlayers += $"Name: {player.Name}, Highest Score: {player.Score}\n";
            }

            if (File.Exists(FilePath))
                MessageBox.Show(listOfPlayers, "LeaderBoards");
            else
                MessageBox.Show("Nothing to Show", "LeaderBoards");
        }

        protected override List<Player> ReadLeaderboard() {
            List<Player> players = new List<Player>();
            if (File.Exists(FilePath)) {
                string[] lines = File.ReadAllLines(FilePath);

                foreach (string line in lines) {
                    string[] arr = line.Split(':');
                    if (arr.Length == 2) {
                        string playerName = arr[0];
                        int playerScore;
                        if (int.TryParse(arr[1], out playerScore)) {
                            players.Add(new Player { Name = playerName, Score = playerScore });
                        }
                    }
                }
            }
            players.Sort((p1, p2) => p2.Score.CompareTo(p1.Score));

            return players;
        }

        protected override void AddUpdate(string playerName, long playerScore) {
            List<Player> players = ReadLeaderboard();

            Player existPlayer = null;
            foreach (var player in players) {
                if (player.Name.Equals(playerName)) {
                    existPlayer = player;
                    break;
                }
            }

            if (existPlayer != null) {
                existPlayer.Score = playerScore;
            } else {
                players.Add(new Player { Name = playerName, Score = playerScore });
            }
            players.Sort((p1, p2) => p2.Score.CompareTo(p1.Score));

            using (StreamWriter writer = new StreamWriter(FilePath)) {
                foreach (var player in players) {
                    writer.WriteLine(player.Name + ":" + player.Score);
                }
            }
        }
    }

    //Encapsulation Sample
    class Player {
        public string Name { get; set; }
        public long Score { get; set; }
    }
}