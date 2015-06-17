using System;
using System.Windows.Forms;

namespace SteamSummerGameBot
{
    public partial class BotSettingsForm : Form
    {
        public GameBot Bot { get; set; }

        public BotSettingsForm()
        {
            InitializeComponent();
        }

        private void btnAuth_Click(object sender, EventArgs e)
        {
            //Если нет бота, создадим
            if (Bot == null)
            {
                Bot = new GameBot();
            }
            //Откроем форму браузера
            var authForm = new BrowserForm(Bot);
            authForm.Show(this);
        }
    }
}
