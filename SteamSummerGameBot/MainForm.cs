using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamSummerGameBot
{


    public partial class MainForm : Form
    {
        private List<GameBot> _bots = new List<GameBot>(); 


        public MainForm()
        {
            InitializeComponent();
        }

        private void BotOnOnLog(GameBot sender, string s)
        {
            Invoke((MethodInvoker)(() =>
            {
                lbLog.Items.Add(string.Format("[{0}] [{1}] : {2}", DateTime.Now, sender.Name, s));
                lbLog.SelectedIndex = lbLog.Items.Count - 1;
            }));
        }

        private void BotOnOnHp(GameBot sender, string s)
        {
            Invoke((MethodInvoker) (() =>lblHp.Text = s));
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var selectedBot = (GameBot) clbBots.SelectedItem;
            if (selectedBot.Status == BotStatus.Stopped)
            {
                selectedBot.Init();
                selectedBot.Start();
                btnStart.Text = "Stop";
            }
            else
            {
                selectedBot.Stop();
                btnStart.Text = "Start";
            }
        }

        private void fMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //_bot.Stop();
        }

        private void fMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach(var bot in _bots)
            {
                bot.OnLog -= BotOnOnLog;
                bot.OnHp -= BotOnOnHp;
                bot.Stop();
            }
            BotLoader.SaveBotsToFile(_bots);
        }


        private async void btnConnectToSpecificGame_Click(object sender, EventArgs e)
        {
            var gameIdToConnect = tbGameIdToConnect.Text;
            var selectedIndex = clbBots.SelectedIndex;
            btnConnectToSpecificGame.Enabled = false;
            slblStatus.Text = "Подключение к игре";
            await Task.Run(() =>
            {
                foreach (GameBot bot in clbBots.CheckedItems)
                {
                    var bot1 = bot;
                    BotOnOnLog(bot1, "Attempt to join game");
                    bot1.JoinGame(gameIdToConnect);
                }
            });
            slblStatus.Text = "Обновление GameId";
            //Обновим GameId
            await Task.Run(() =>
            {
                foreach (GameBot bot in clbBots.CheckedItems)
                {
                    var bot1 = bot;
                    BotOnOnLog(bot1, "Attempt to refresh gameId");
                    bot1.GameId = bot1.RequestGameId();
                }
            });
            clbBots.SelectedIndex = -1;
            clbBots.SelectedIndex = selectedIndex;
            slblStatus.Text = "Готово";
            btnConnectToSpecificGame.Enabled = true;
        }

        private void btnAddBot_Click(object sender, EventArgs e)
        {
            //Вызовем форму добавления бота
            using (var addBotForm = new BotSettingsForm())
            {
                if (addBotForm.ShowDialog(this) == DialogResult.OK)
                {
                    var newBot = addBotForm.Bot;
                    if (newBot != null)
                    {
                            _bots.Add(newBot);
                            newBot.GameId = newBot.RequestGameId();
                            newBot.OnLog += BotOnOnLog;
                            clbBots.Items.Add(newBot, true);
                    }
                }
            }
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            clbBots.DisplayMember = "Name";
            await Task.Run(() =>
            {
                //Загрузим ботов из файла
                _bots = BotLoader.LoadBotsFromFile();
                //Заполним список
                foreach (var bot in _bots)
                {
                    //Теперь загрузим для бота gameId
                    var bot1 = bot;
                    bot1.OnLog += BotOnOnLog;

                    bot1.GameId = bot1.RequestGameId();
                    Invoke((MethodInvoker) (() =>
                    {
                        clbBots.Items.Add(bot1, true);
                    }));
                }
            });
            if (clbBots.Items.Count > 0) clbBots.SelectedIndex = 0;
        }

        private void clbBots_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Сменили бота
            //Поменяем ину о нем
            if(clbBots.SelectedIndex == -1) return;
            var selectedBot = (GameBot)clbBots.SelectedItem;
            tbGameId.Text = selectedBot.GameId;
            tbAccessToken.Text = selectedBot.AccessToken;
            tbName.Text = selectedBot.Name;
            tbSteamId.Text = selectedBot.SteamId;
            if (selectedBot.Status != BotStatus.Started)
            {
                btnStart.Enabled = selectedBot.GameId != string.Empty;
                btnStart.Text = "Start";
            }
            else
            {
                btnStart.Enabled = true;
                btnStart.Text = "Stop";
            }
        }

        private async void btnLeaveGame_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("?", "Really?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var selectedIndex = clbBots.SelectedIndex;
                slblStatus.Text = "Выход из игры";
                var selectedBot = (GameBot) clbBots.SelectedItem;
                await Task.Run(() => selectedBot.LeaveGame());
                slblStatus.Text = "Обновление информации об игре";
                await Task.Run(() => selectedBot.GameId = selectedBot.RequestGameId());
                clbBots.SelectedIndex = -1;
                clbBots.SelectedIndex = selectedIndex;
                slblStatus.Text = "Готово";
            }
        }

        private void btnStartSelected_Click(object sender, EventArgs e)
        {
            var selectedIndex = clbBots.SelectedIndex;
            foreach (GameBot bot in clbBots.CheckedItems.Cast<GameBot>().Where(bot => bot.Status != BotStatus.Started))
            {
                bot.Init();
                bot.Start();
            }
            clbBots.SelectedIndex = -1;
            clbBots.SelectedIndex = selectedIndex;

        }

        private void btnRemoveBot_Click(object sender, EventArgs e)
        {
            var selectedBot = (GameBot)clbBots.SelectedItem;
            if (selectedBot.Status == BotStatus.Started)
            {
                selectedBot.OnLog -= BotOnOnLog;
                selectedBot.OnHp -= BotOnOnHp;
                selectedBot.Stop();
            }

            _bots.Remove(selectedBot);
            clbBots.Items.Remove(selectedBot);

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //Вызовем форму обновления бота
            using (var editBotForm = new BotSettingsForm())
            {
                editBotForm.Bot = (GameBot)clbBots.SelectedItem;
                if (editBotForm.ShowDialog(this) == DialogResult.OK)
                {
                    
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var selectedBot = (GameBot)clbBots.SelectedItem;
            selectedBot.GameId = tbGameId.Text;
        }

        private async void btnJoinGame_Click(object sender, EventArgs e)
        {
            //Подключение к указанной игре
            btnJoinGame.Enabled = false;
            slblStatus.Text = "Подключение к игре";
            var bot = (GameBot) clbBots.SelectedItem;
            var gameIdToConnect = tbGameId.Text;
            await Task.Run(() =>
            {
                var bot1 = bot;
                bot1.JoinGame(gameIdToConnect);
            });
            slblStatus.Text = "Обновление GameId";
            //Обновим GameId
            await Task.Run(() =>
            {
                var bot1 = bot;
                bot1.GameId = bot1.RequestGameId();
            });
            slblStatus.Text = "Готово";
            btnJoinGame.Enabled = true;
        }

        private async void btnRefreshGameId_Click(object sender, EventArgs e)
        {
            btnRefreshGameId.Enabled = false;
            var bot = (GameBot)clbBots.SelectedItem;
            slblStatus.Text = "Обновление GameId";
            //Обновим GameId
            await Task.Run(() =>
            {
                var bot1 = bot;
                bot1.GameId = bot1.RequestGameId();
            });
            slblStatus.Text = "Готово";
            btnRefreshGameId.Enabled = true;
        }

        private async void btnLeaveSelected_Click(object sender, EventArgs e)
        {
            foreach (GameBot bot in clbBots.CheckedItems)
            {
                await Task.Run(() =>
                {
                    var bot1 = bot;
                    bot1.LeaveGame();
                    bot1.GameId = bot1.RequestGameId();
                });
            }
        }
    }
}
