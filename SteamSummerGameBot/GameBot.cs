using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace SteamSummerGameBot
{
    public enum BotStatus
    {
        Started,
        Stopped
    }


    public static class Commands
    {
        public static string SwitchLane(string lane)
        {
            return "{\"ability\":2,\"new_lane\":%LANE%}".Replace("%LANE%", lane);
        }

        public static string Clicks(string num)
        {
            return "{\"ability\":1,\"num_clicks\":%NUM%}".Replace("%NUM%", num);
        }

        public static string SwitchTarget(string target)
        {
            return "{\"ability\":4,\"new_target\":%TARGET%}".Replace("%TARGET%", target);
        }

        public static string UseBadgePoints(string gameId, string abilities)
        {
            return "{\"gameid\":\"%GAMEID%\",\"ability_items\":[%ABILITIES%]}".Replace("%GAMEID%", gameId).Replace("%ABILITIES%", abilities);
        }

        public static string RequestedAbilities(string gameId, string abilities)
        {
            return "{\"requested_abilities\":[%ABILITIES%],\"gameid\":\"%GAMEID%\"}".Replace("%ABILITIES%", abilities).Replace("%GAMEID%", gameId);
        }

        public static string Respawn
        {
            get { return "{\"ability\":3}"; }
        }

        public static string AbilityItem(string id)
        {
            return "{\"ability\":%ID%}".Replace("%ID%", id);
        }

        public static string Upgrade(string gameId, string upgrades)
        {
            return "{\"gameid\":\"%GAMEID%\",\"upgrades\":[%UPGRADES%]}".Replace("%GAMEID%", gameId).Replace("%UPGRADES%", upgrades);
        }
    }




    public static class Items
    {
        //key = id, value - cost
        public static Dictionary<int, int> Cost = new Dictionary<int, int>()
        {
            {22, 2},
            {19, 1},
            {26, 100}
        };
        public static int GoldenChest = 22;
        public static int AddHealth = 19;
        /// <summary>
        /// Увеличение крита навсегда
        /// </summary>
        public static int Crit = 18;

        /// <summary>
        /// Золотой дождь
        /// </summary>
        public static int GoldenRain = 17;

        /// <summary>
        /// Изувечить чудовище
        /// </summary>
        public static int CrashMonster = 15;

        public static int WormHole = 26;
    }


    public static class Upgrades
    {
        //Легкая броня
        public static int LightArmor = 0;
        //Тяжелая броня
        public static int HeavyArmor = 8;
        //Силовой щит
        public static int ForceShield = 20;

        //Автопушка
        public static int AutoCannon = 1;

        //Улучшения клика
        public static int Bullet1 = 2;
        public static int Bullet2 = 10;
        public static int Bullet3 = 22;
        public static int Bullet4 = 25;
        public static int Bullet5 = 28;
        public static int Bullet6 = 31;
        public static int Bullet7 = 34;

        //Увеличение дамага крита
        public static int LuckyShot = 7;


    }

    public static class EnemyType
    {
        public static int Spawner = 0;
        public static int Creep = 1;
        public static int Boss = 2;
        public static int Miniboss = 3;
        public static int Treasure = 4;
    }


    //Deprecated
    public class PlayerData
    {
        public int Hp { get; set; }
    }


    internal class ApiRequest
    {
        private readonly string _apiCall;
        public string ApiCall { get { return _apiCall; }}

        private readonly string _inputJson;
        public string InputJson { get { return _inputJson; }}

        public ApiRequest(string apiCall, string inputJson)
        {
            _apiCall = apiCall;
            _inputJson = inputJson;
        }

        public static ApiRequest Make(string apiCall, string inputJson)
        {
            return new ApiRequest(apiCall, inputJson);
        }
    }

    public class GameBot
    {
        private Thread _thread;
        private Thread _consumerThread;

        private volatile int _sleepTime = 100;
        public int SleepTime { set { _sleepTime = value; } }

        private string _gameId;
        private string _clickCount = "125";

        private readonly RestClient _client;


        public event Action<GameBot, string> OnHp = delegate { };
        public event Action<GameBot, string> OnLog = delegate { };

        private bool _isRunning;

        private ConcurrentQueue<ApiRequest> _apiRequests = new ConcurrentQueue<ApiRequest>(); 

        public string GameId
        {
            get { return _gameId; }
            set { _gameId = value; }
        }

        public string Name
        {
            get { return _accountInfo.Name; }
        }

        public string SteamId
        {
            get { return _accountInfo.SteamId; }
        }

        public string AccessToken
        {
            get { return _accountInfo.AccessToken; }
        }

        public string SessionIdCookie
        {
            get { return _accountInfo.SessionIdCookieValue; }
        }

        public string SteamLoginCookie
        {
            get { return _accountInfo.SteamLoginCookieValue; }
        }

        public string SteamRememberLoginCookie
        {
            get { return _accountInfo.SteamRememberLogin; }
        }

        public BotStatus Status
        {
            get
            {
               return _isRunning ? BotStatus.Started : BotStatus.Stopped;
            }
        }

        private int _primaryElement = -1;
        private int _primaryElementUpgrade = -1;

        public GameBot()
        {
            _thread = new Thread(Work);
            _consumerThread = new Thread(ProcessApiRequests);
            _client = new RestClient("https://steamapi-a.akamaihd.net");
        }

        private void ProcessApiRequests()
        {
            while (_isRunning)
            {
                
                ApiRequest request;
                while (_apiRequests.TryDequeue(out request) && _isRunning)
                {
                    _postRequest.Resource = request.ApiCall;
                    _postRequest.Parameters[0].Value = request.InputJson;
                    try
                    {
                        _client.ExecuteAsync(_postRequest, (r, h) => OnLog(this, "Request done"));
                    }
                    catch
                    {
                        OnLog(this, "Fail on ProcessApiRequests");
                    }
                }
                Thread.Sleep(100);
            }
        }

        // Запрос для вызова API
        private RestRequest _postRequest;

        // Запрос для получения снапшота игры
        private RestRequest _getGameDataRequest;

        // Запрос для получения снапшота игрока
        private RestRequest _getPlayerDataRequest;

        // Игровые данные
        private dynamic _gameData;

        // Данные игрока
        private dynamic _playerData;

        //Список абилок для вызова API UseAbilities
        private readonly List<string> _abilitiesToUse = new List<string>();

        //key - APIFunc, value - input_json
        private readonly Dictionary<string, string> _requests = new Dictionary<string, string>();

        private AccountInfo _accountInfo = new AccountInfo();

        private readonly Random _random = new Random(DateTime.Now.Millisecond);


        //http://steamcommunity.com/minigame/ajaxleavegame/
        public bool LeaveGame()
        {
            try
            {
                var cl = new RestClient("http://steamcommunity.com");
                var req = new RestRequest("minigame/ajaxleavegame/", Method.POST);
                req.AddParameter("sessionid", _accountInfo.SessionIdCookieValue, ParameterType.Cookie);
                req.AddParameter("steamLogin", _accountInfo.SteamLoginCookieValue, ParameterType.Cookie);
                req.AddParameter("gameid", _gameId);
                req.AddParameter("sessionid", _accountInfo.SessionIdCookieValue);
                var response = cl.Execute(req);
                dynamic obj = JObject.Parse(response.Content);
                var success = obj.success;
                if (success == 1)
                {
                    OnLog(this, "LeaveGame successed");
                    return true;
                }
            }
            catch (Exception)
            {
                OnLog(this, "Except when LeaveGame");
                return false;
            }
            OnLog(this, "Cannot leave game");
            return false;
        }


        //Подключение к игре
        public bool JoinGame(string gameId)
        {
            try
            {
                var tryCount = 50;
                while (tryCount > 0)
                {
                    var cl = new RestClient("http://steamcommunity.com");
                    var req = new RestRequest("minigame/ajaxjoingame/", Method.POST);
                    req.AddParameter("sessionid", _accountInfo.SessionIdCookieValue, ParameterType.Cookie);
                    req.AddParameter("steamLogin", _accountInfo.SteamLoginCookieValue, ParameterType.Cookie);
                    req.AddParameter("steamRememberLogin", _accountInfo.SteamRememberLogin, ParameterType.Cookie);
                    req.AddParameter("gameid", gameId);
                    var response = cl.Execute(req);
                    dynamic obj = JObject.Parse(response.Content);
                    var successCode = obj.success;
                    if (successCode == 1) return true;
                    tryCount--;
                    Thread.Sleep(500);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }


        //Загрузить страничку, чтобы посмотреть Id игры
        //http://steamcommunity.com/minigame/
        public string RequestGameId()
        {
            var cl = new RestClient("http://steamcommunity.com");
            var req = new RestRequest("minigame/", Method.GET);
            req.AddParameter("sessionid", _accountInfo.SessionIdCookieValue, ParameterType.Cookie);
            req.AddParameter("steamLogin", _accountInfo.SteamLoginCookieValue, ParameterType.Cookie);
            req.AddParameter("steamRememberLogin", _accountInfo.SteamRememberLogin, ParameterType.Cookie);
            var response = cl.Execute(req);
            var index = response.Content.IndexOf("'gameid' : '", StringComparison.Ordinal);
            if (index != -1)
            {
                //Если эта подстрока есть, то нашли id игры
                var substr = response.Content.Substring(index + 12);
                var gameId = string.Concat(substr.TakeWhile((c) => c != '\''));
                OnLog(this, "Requested gameId: " + gameId);
                return gameId;
            }
            OnLog(this, "Requested gameId is empty");
            return string.Empty;
        }


        public string SaveToString()
        {
            var result = _accountInfo.SteamId + ";" + _accountInfo.AccessToken + ";" + _accountInfo.SessionIdCookieValue +
                         ";" + _accountInfo.SteamLoginCookieValue + ";" + _accountInfo.SteamRememberLogin + ";" + _accountInfo.Name + ";" +_clickCount;
            return result;
        }

        public void LoadFromString(string input)
        {
            var values = input.Split(';');
            if (values.Length != 7) return;
            _accountInfo = new AccountInfo
            {
                SteamId = values[0],
                AccessToken = values[1],
                SessionIdCookieValue = values[2],
                SteamLoginCookieValue = values[3],
                SteamRememberLogin = values[4],
                Name = values[5]
            };
            _clickCount = values[6];
        }

        private void PrepareRequests()
        {
            //Общий объект хапросов
            _postRequest = new RestRequest(Method.POST);
            _postRequest.AddParameter("input_json", "");
            _postRequest.AddParameter("access_token", _accountInfo.AccessToken);


            _getGameDataRequest = new RestRequest("ITowerAttackMiniGameService/GetGameData/v0001/", Method.GET);
            _getGameDataRequest.AddParameter("gameid", _gameId);
            _getGameDataRequest.AddParameter("include_stats", "0");

            _getPlayerDataRequest = new RestRequest("ITowerAttackMiniGameService/GetPlayerData/v0001/", Method.GET);
            _getPlayerDataRequest.AddParameter("gameid", _gameId);
            _getPlayerDataRequest.AddParameter("steamid", _accountInfo.SteamId);
            _getPlayerDataRequest.AddParameter("include_tech_tree", "1");

        }



        public void SetAccountInfo(string steamId, string accessToken, string sessionId, string steamLogin)
        {
            lock (_accountInfo)
            {
                _accountInfo.SessionIdCookieValue = sessionId;
                _accountInfo.SteamId = steamId;
                _accountInfo.AccessToken = accessToken;
                _accountInfo.SteamLoginCookieValue = steamLogin;
            }
        }

        public void SetAccountInfo(AccountInfo accountInfo)
        {
            lock (_accountInfo)
            {
                _accountInfo = accountInfo;
            }
        }

        public void Init()
        {
            PrepareRequests();
        }

        /// <summary>
        /// Обновление данных об игроке
        /// </summary>
        private void UpdatePlayerData()
        {
            var success = false;
            var tryCount = 5;
            while (!success && (tryCount > 0))
            {
                try
                {
                    var response = _client.Execute(_getPlayerDataRequest);
                    dynamic r = JObject.Parse(response.Content);
                    if (r != null && r.response != null && r.response.player_data != null)
                    {
                        success = true;
                        _playerData = r;
                        //return;
                    }
                }
                catch (Exception)
                {
                    success = false;
                }
                tryCount--;
            }
            if (!success) OnLog(this, "Bad UpdatePlayerData");
        }



        /// <summary>
        /// Обновление данных об игре
        /// </summary>
        private void UpdateGameData()
        {
            var success = false;
            var tryCount = 5;
            while (!success && (tryCount > 0))
            {
                try
                {
                    var response = _client.Execute(_getGameDataRequest);
                    dynamic r = JObject.Parse(response.Content);
                    if (r != null && r.response != null && r.response.game_data != null)
                    {
                        success = true;
                        _gameData = r;
                        //return;
                    }
                }
                catch (Exception)
                {
                    success = false;
                }
                tryCount--;
            }
            if (!success) OnLog(this, "Bad UpdateGameData");
        }

        private void Respawn()
        {
            _abilitiesToUse.Add(Commands.Respawn);
        }

        private void ProcessRequests()
        {               
            //Если ничего нет - выйдем
            if (_requests.Count == 0) return;
            try
            {
                foreach (var request in _requests)
                {
                    var apiCall = request.Key;
                    var inputJson = request.Value;
                    _postRequest.Resource = apiCall;
                    _postRequest.Parameters[0].Value = inputJson;
                    try
                    {
                        var response = _client.Execute(_postRequest);
                        
                    }
                    catch
                    {
                        
                    }
                }
            }
            catch (Exception)
            {
                
            }
            _requests.Clear();
        }


        private void ProcessAbilities()
        {
            if (_abilitiesToUse.Count == 0) return;
            var abilities = _abilitiesToUse.Aggregate((x, y) => x + "," + y);
            _apiRequests.Enqueue(ApiRequest.Make("ITowerAttackMiniGameService/UseAbilities/v0001/", Commands.RequestedAbilities(_gameId, abilities)));
            _abilitiesToUse.Clear();
        }


        private void UseAbilityItems()
        {
            //Заюзаем какую-нибудь абилку
            var abilityItems = _playerData.response.tech_tree.ability_items;
            var abilityUtemToUse = -1;
            foreach (var abilityItem in abilityItems)
            {
                //Используем пропуск уровня, если он кратен 100 
                var abilityId = (int) abilityItem.ability;
                if ( abilityId == Items.WormHole && ((int) _gameData.response.game_data.level)%100 == 0)
                {
                    abilityUtemToUse = abilityId;
                    break;
                }
                else
                {
                    //abilityUtemToUse = abilityId;
                }
            }
            _abilitiesToUse.Add(Commands.AbilityItem(abilityUtemToUse == -1 ? abilityItems[_random.Next(0, abilityItems.Count)].ability.ToString() : abilityUtemToUse.ToString()));
           // OnLog(this, "Use Ability Item");
        }

        
        /// <summary>
        /// Раскидка очков за бейджы
        /// Приоритет раскидки:
        /// 1. Кротовые норы
        /// 2. Золотой сундук
        /// 3. Рука-добавка ХП
        /// </summary>
        private void SpendBadgePoints()
        {
            try
            {
                var unusedBadgePoints = _playerData.response.tech_tree.badge_points;
                var badgesToSpend = new List<string>();
                while (unusedBadgePoints != 0)
                {
                    var cost = 0;
                    if (unusedBadgePoints >= Items.Cost[Items.WormHole])
                    {
                        cost = Items.Cost[Items.WormHole];
                        badgesToSpend.Add(Items.WormHole.ToString());
                    }
                    else if (unusedBadgePoints >= Items.Cost[Items.GoldenChest])
                    {
                        cost = Items.Cost[Items.GoldenChest];
                        badgesToSpend.Add(Items.GoldenChest.ToString());
                    }
                    else
                    {
                        cost = Items.Cost[Items.AddHealth];
                        badgesToSpend.Add(Items.AddHealth.ToString());
                    }
                    unusedBadgePoints -= cost;
                }
                var abilities = badgesToSpend.Aggregate((x, y) => x + "," + y);
                _apiRequests.Enqueue(ApiRequest.Make("ITowerAttackMiniGameService/UseBadgePoints/v0001/",
                    Commands.UseBadgePoints(_gameId, abilities)));
            }
            catch (Exception e)
            {
                OnLog(this, e.Message);
            }

        }



        //Выбор линии и цели огня
        private void ChooseLane()
        {
            //Выберем линию с хорошими мобами
            var laneId = 0;
            var currentTarget = (int)_playerData.response.player_data.target;
            var currentLine = (int)_playerData.response.player_data.current_lane;
            var candidateTargetType = EnemyType.Creep;
            var foundGood = false;

            foreach (var lane in _gameData.response.game_data.lanes)
            {
                if (foundGood) break;
                var targetId = 0;
                foreach (var enemy in lane.enemies)
                {
                    //Если на этой линии есть неубитый босс, переключим на неё
                    if (enemy.type == EnemyType.Boss && (enemy.hp > 0))
                    {
                        currentTarget = targetId;
                        currentLine = laneId;
                        foundGood = true;
                        break;
                    }
                    //Если это минибосс - запоним это и перейдем к другой линии
                    if (enemy.type == EnemyType.Miniboss && (enemy.hp > 0))
                    {
                        //Если уже усть минибосс, не переключаемся
                        if (candidateTargetType != EnemyType.Miniboss)
                        {
                            currentTarget = targetId;
                            currentLine = laneId;
                            candidateTargetType = EnemyType.Miniboss;
                        }
                        break;
                    }
                    if (enemy.type == EnemyType.Spawner && (enemy.hp > 0) )
                    {
                        currentTarget = targetId;
                        currentLine = laneId;
                        candidateTargetType = EnemyType.Spawner;
                        if (lane.type == _primaryElement)
                        {
                            break;
                        }
                    }
                    else if(candidateTargetType == EnemyType.Creep)
                    {
                   //     //Если это обычный крип, то запомним его, но не уходим пока с линии
                        currentTarget = targetId;
                    }
                    //А пока остаемся на той же линии
                    targetId++;
                }
                laneId++;
            }
            if (currentLine != (int)_playerData.response.player_data.current_lane)
            {
                //Добавим смену линии и таргета
                _abilitiesToUse.Add(Commands.SwitchLane(currentLine.ToString()));
                //OnLog(this, "Switch lane. Current: " + currentLine);
                if (currentTarget != (int)_playerData.response.player_data.target)
                {
                    _abilitiesToUse.Add(Commands.SwitchTarget(currentTarget.ToString()));
                    //OnLog(this, "Switch target. Current: " + currentTarget);
                }
            }
            else if (currentTarget != (int)_playerData.response.player_data.target)
            {
                //Сменим только таргет
                _abilitiesToUse.Add(Commands.SwitchTarget(currentTarget.ToString()));
                //OnLog(this, "Switch target. Current: " + currentTarget);
            }

            //Нужно запоминать тип текущего таргета
        }


        //Выбор ангрейдов. Не включены апгрейды элементов
        private void ChooseUpgrades()
        {
            var upgradesData = _playerData.response.tech_tree.upgrades;
            var choosenUpgrades = new List<string>();
            var currentGold = _playerData.response.player_data.gold;
            //Пройдем по-очереди все апгрейды
            if (upgradesData[Upgrades.LightArmor].level < 10)
            {
                if (upgradesData[Upgrades.LightArmor].cost_for_next_level < currentGold)
                {
                    choosenUpgrades.Add(Upgrades.LightArmor.ToString());
                    currentGold -= (double)upgradesData[Upgrades.LightArmor].cost_for_next_level;
                }
            }
            else if (upgradesData[Upgrades.HeavyArmor].level < 10)
            {
                if (upgradesData[Upgrades.HeavyArmor].cost_for_next_level < currentGold)
                {
                    choosenUpgrades.Add(Upgrades.HeavyArmor.ToString());
                    currentGold -= (double)upgradesData[Upgrades.HeavyArmor].cost_for_next_level;
                }
            }

            if (upgradesData[Upgrades.Bullet1].level < 10)
            {
                if (upgradesData[Upgrades.Bullet1].cost_for_next_level < currentGold)
                {
                    choosenUpgrades.Add(Upgrades.Bullet1.ToString());
                    currentGold -= (double)upgradesData[Upgrades.Bullet1].cost_for_next_level;
                }
            }
            else if (upgradesData[Upgrades.Bullet2].level < 10)
            {
                if (upgradesData[Upgrades.Bullet2].cost_for_next_level < currentGold)
                {
                    choosenUpgrades.Add(Upgrades.Bullet2.ToString());
                    currentGold -= (double)upgradesData[Upgrades.Bullet2].cost_for_next_level;
                }
            }
            else if (upgradesData[Upgrades.Bullet3].level < 10)
            {
                if (upgradesData[Upgrades.Bullet3].cost_for_next_level < currentGold)
                {
                    choosenUpgrades.Add(Upgrades.Bullet3.ToString());
                    currentGold -= (double)upgradesData[Upgrades.Bullet3].cost_for_next_level;
                }
            }
            else if (upgradesData[Upgrades.Bullet4].level < 10)
            {
                if (upgradesData[Upgrades.Bullet4].cost_for_next_level < currentGold)
                {
                    choosenUpgrades.Add(Upgrades.Bullet4.ToString());
                    currentGold -= (double) upgradesData[Upgrades.Bullet4].cost_for_next_level;
                }
            }
            else if (upgradesData[Upgrades.Bullet5].level < 10)
            {
                if (upgradesData[Upgrades.Bullet5].cost_for_next_level < currentGold)
                {
                    choosenUpgrades.Add(Upgrades.Bullet5.ToString());
                    currentGold -= (double)upgradesData[Upgrades.Bullet5].cost_for_next_level;
                }
            }
            else if (upgradesData[Upgrades.Bullet6].level < 10)
            {
                if (upgradesData[Upgrades.Bullet6].cost_for_next_level < currentGold)
                {
                    choosenUpgrades.Add(Upgrades.Bullet6.ToString());
                    currentGold -= (double)upgradesData[Upgrades.Bullet6].cost_for_next_level;
                }
            }
            if (upgradesData[Upgrades.Bullet7].cost_for_next_level < currentGold)
            {
                choosenUpgrades.Add(Upgrades.Bullet7.ToString());
                currentGold -= (double)upgradesData[Upgrades.Bullet7].cost_for_next_level;
            }

            if (upgradesData[Upgrades.ForceShield].cost_for_next_level < currentGold)
            {
                choosenUpgrades.Add(Upgrades.ForceShield.ToString());
                currentGold -= (double)upgradesData[Upgrades.ForceShield].cost_for_next_level;
            }


            //Апгрейд праймари элемента
            if (upgradesData[_primaryElementUpgrade].cost_for_next_level < currentGold)
            {
                choosenUpgrades.Add(_primaryElementUpgrade.ToString());
                currentGold -= (double)upgradesData[_primaryElementUpgrade].cost_for_next_level;
            }

            if(choosenUpgrades.Count == 0) return;
            var upgrades = choosenUpgrades.Aggregate((x, y) => x + "," + y);
            _apiRequests.Enqueue(ApiRequest.Make("ITowerAttackMiniGameService/ChooseUpgrade/v0001/", Commands.Upgrade(_gameId, upgrades)));
        }

        /// <summary>
        /// Кинем кость с заданной вероятностью
        /// </summary>
        /// <param name="probability">Вероятность выпадения</param>
        /// <returns>true - выпало, false - нет</returns>
        private bool RollDice(float probability)
        {
            var result = (_random.Next(0, 100))/100f;
            if (result < probability)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Количество кликов нужно раскинуть на несколько 
        /// запросов по 20 штук
        /// </summary>
        /// <param name="times"></param>
        private void Clicks(int times)
        {
            var numberOfAbilitiesToAdd = Math.Ceiling((double)times/20);
            var currentTimes = times;
            for (var i = 0; i < numberOfAbilitiesToAdd; i++, currentTimes -= 20)
            {
                _abilitiesToUse.Add(currentTimes < 20 ? Commands.Clicks(currentTimes.ToString()) : Commands.Clicks("20"));
            }
        }

        private void Work()
        {
            //Предварительная подготовка
            //Установим тип прокачиваемого урона

            try
            {
                UpdateGameData();
                UpdatePlayerData();
                var maxElementUpgradeLevel = 0;
                var maxUpgradedElement = 0;
                for (var i = 3; i < 7; i++)
                {
                    if ((int)_playerData.response.tech_tree.upgrades[i].level > maxElementUpgradeLevel)
                    {
                        maxElementUpgradeLevel = (int)_playerData.response.tech_tree.upgrades[i].level;
                        maxUpgradedElement = i;
                    } 
                }
                if (maxElementUpgradeLevel > 0)
                {
                    _primaryElement = maxUpgradedElement;
                    _primaryElementUpgrade = _primaryElement + 2;
                }
            }
            catch (Exception)
            {
                OnLog(this, "Exception during thread initialization");
            }
            if (_primaryElement == -1)
            {
                //Зарандомим элемент для прокачки
                _primaryElement = _random.Next(1, 4);
                _primaryElementUpgrade = _primaryElement + 2;
            }

            while (_isRunning)
            {
                try
                {
                    //Обновим PlayerData и GameData
                    UpdateGameData();
                    UpdatePlayerData();
                    if (_playerData != null)
                    {
                        //Теперь нужно посмотреть, нет ли у нас бэйджей, если есть, то нужно их распределить
                        if (_playerData.response.tech_tree.badge_points > 0)
                        {
                            //Распределяем поинты
                            SpendBadgePoints();
                        }
                        else
                        {
                            var hp = _playerData.response.player_data.hp;
                            //OnHp(Convert.ToString(hp));
                            //Все ок, можно все делать
                            if (_playerData.response.player_data.hp == 0)
                            {
                                Respawn();
                                //OnLog(this,"Respawn");
                            }
                            else
                            {
                                //Проапгрейдим
                                ChooseUpgrades();
                                //С вероятностью 50% заюзаем айтем
                                if((_playerData.response.tech_tree.ability_items != null) && RollDice(0.50f))
                                    UseAbilityItems();
                                ChooseLane();
                                Clicks(_random.Next(125,200));
                            }
                            //Обработаем все абилки
                            ProcessAbilities();
                        }

                        //Разошлем запросы
                        //ProcessRequests();
                    }
                    //OnLog("Tick");
                    Thread.Sleep(_sleepTime);
                }
                catch (Exception)
                {
                    OnLog(this, "Except");
                }
            }
        }

        public void Stop()
        {
            _isRunning = false;
            if (_thread != null && _thread.ThreadState == ThreadState.Running)
            {
                _thread.Join(100);
                _thread = null;
                OnLog(this, "Main thread stopped");
            }
            if (_consumerThread != null && _consumerThread.ThreadState == ThreadState.Running)
            {
                _consumerThread.Join(100);
                _consumerThread = null;
                OnLog(this, "Consumer thread stopped");
            }
        }

        public void Start()
        {
            _isRunning = true;
            if(_thread == null) _thread = new Thread(Work);
            else
            {
                if (_thread.ThreadState != ThreadState.Running)
                {
                    _thread.Start();
                    OnLog(this, "Started");
                }
            }
            if (_consumerThread == null) _consumerThread = new Thread(ProcessApiRequests);
            else
            {
                if (_consumerThread.ThreadState != ThreadState.Running)
                {
                    _consumerThread.Start();
                }
            }
        }
    }

}
