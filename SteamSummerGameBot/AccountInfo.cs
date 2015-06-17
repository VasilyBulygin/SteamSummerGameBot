using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SteamSummerGameBot
{
    public class AccountInfo
    {
        //SteamId
        public string SteamId { get; set; }

        //Access Token
        public string AccessToken { get; set; }

        //Куки

        /// <summary>
        /// Имя куки - sessionid
        /// </summary>
        public string SessionIdCookieValue { get; set; }

        /// <summary>
        /// Имя куки - steamLogin
        /// </summary>
        public string SteamLoginCookieValue { get; set; }

        public string SteamRememberLogin { get; set; }

        /// <summary>
        /// Имя аккаунта
        /// </summary>
        public string Name { get; set; }
    }
}
