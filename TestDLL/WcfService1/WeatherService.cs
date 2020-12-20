using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WeatherServices.Modles.Entities;
using WeatherServices.Modles.Interfaces;

namespace WeatherServices
{
    public class ClientWeatherServer : IClientWeatherServer
    {
        
        #region 初始化
        public ClientWeatherServer()
        {
            //直到加载完成才初始化完成 
            SystemVars.IniData(); 
        }

        #endregion


        #region 对订阅用户公开的服务
        /// <summary>
        /// 订阅服务
        /// </summary>
        /// <param name="user">用户信息(包括订阅的城市列表)</param>
        /// <returns>返回订阅成功时的用户信息</returns>
        public UserInfo BookWeather(UserInfo user)
        {
            if (user == null) return null;

            //如果该用户存在，那么验证
            UserInfo myUser = SystemVars.lstUsers.Find(x
                => x.Code == user.Code
                && x.Password == user.Password);
            if (myUser == null)
            {
                myUser = new UserInfo();
                int i = SystemVars.lstUsers.Count + 1;
                while (SystemVars.lstUsers.Exists(x => x.Code == "user" + i.ToString()))
                {
                    i++;
                }
                //重新生成用户
                myUser.Code = "user" + i.ToString();
                myUser.Password = "666666";
                SystemVars.lstUsers.Add(myUser);
            }

            //更新城市
            if (user.Cities != null)
            {
                //删除不存在的城市 
                List<string> cities = new List<string>(user.Cities);
                cities.RemoveAll(x => !SystemVars.lstCities.Exists(c => c.Code == x));
                //更新为最新城市
                myUser.Cities = cities.ToArray();
            }

            //保存回调连接 
            ICallback callback = OperationContext.Current.GetCallbackChannel<ICallback>();
            myUser.Callback = callback;

            return myUser;
        }

       
        /// <summary>
        /// 取消服务
        /// </summary>
        /// <param name="login">用户信息(包括要取消的城市信息)</param>
        /// <returns>返回成功或者失败</returns> 
        public bool CancelWeather(Login login)
        {
            if (login == null) return false;
            SystemVars.lstUsers.RemoveAll(x => x.Code == login.Code && x.Password == login.Password);
            return true;
        }

        /// <summary>
        /// 获取天气
        /// </summary>
        /// <param name="login">用户</param>
        /// <returns>返回该用户订阅的所有城市天气</returns>
        public Weather[] GetWeather(Login login)
        {
            if (login == null) return null;

            //如果用户不存在，那么退出
            UserInfo myUser = SystemVars.lstUsers.Find(x => x.Code == login.Code && x.Password == login.Password);
            if (myUser == null) return null;

            //更新回调连接 
            ICallback callback = OperationContext.Current.GetCallbackChannel<ICallback>();
            myUser.Callback = callback;

            return SystemVars.lstCityWeathers.FindAll(x => myUser.Cities.Contains(x.CityCode)).ToArray();
        }

        /// <summary>
        /// 取得所有城市信息
        /// </summary>
        /// <param name="login">气象站</param>
        /// <returns>返回所有城市列表</returns> 
        public CityInfo[] GetCityEnum(Login login)
        {
            return SystemVars.GetCityEnum(login);
        }

        /// <summary>
        /// 取得天气代码映射关系
        /// </summary>
        /// <param name="login">气象站</param>
        /// <returns>返回所有天气代码映射关系</returns>
        public Item[] GetWeatherEnum(Login login)
        {
            return SystemVars.GetWeatherEnum(login);
        }

        /// <summary>
        /// 取得风力代码映射关系
        /// </summary>
        /// <param name="login">气象站</param>
        /// <returns>返回所有风力代码映射关系</returns>
        public Item[] GetWindEnum(Login login)
        {
            return SystemVars.GetWindEnum(login);
        }

        /// <summary>
        /// 更新会话信息
        /// </summary>
        /// <param name="login">用户信息</param>
        public void UpdateSession(Login login)
        {
            SystemVars.UpdateSession(login);
        }
        #endregion

    }

    public static class SystemVars
    {
        //是否加载完成
        public static bool IsLoading = false;

        //当前订阅服务的用户
        public static List<UserInfo> lstUsers = new List<UserInfo>();

        //当前气象站点
        public static List<Login> lstStations = new List<Login>();

        //当前能提供的天气服务的城市列表
        public static List<CityInfo> lstCities = new List<CityInfo>();

        //城市实时天气表
        public static List<Weather> lstCityWeathers = new List<Weather>();

        //天气映射
        public static List<Item> lstWeatherInfos = new List<Item>();

        //风力映射
        public static List<Item> lstWindInfos = new List<Item>();
         
        /// <summary>
        /// 加载变量
        /// </summary>
        public static void IniData()
        {
            if (IsLoading) return;
            IsLoading = true;

            lock (typeof(SystemVars))
            {
                //加载城市信息
                IniCities();

                //加载天气映射
                IniWeatherInfos();

                //加载风力
                IniWindInfos();

                //加载气象站信息
                IniStations();
            }
             
        }

        /// <summary>
        /// 加载城市信息
        /// </summary>
        static void IniCities()
        {
            lstCities.Clear();
            CityInfo city = new CityInfo();
            city.Code = "bj";
            city.Name = "北京";
            city.Description = "首都、故宫、长城";
            city.Longitude = "116.46";
            city.Latitude = "39.92";
            lstCities.Add(city);


            city = new CityInfo();
            city.Code = "nj";
            city.Name = "南京";
            city.Longitude = "126.45";
            city.Latitude = "30.92";
            city.Description = "江南名城、夫子庙";
            lstCities.Add(city);


            city = new CityInfo();
            city.Code = "tb";
            city.Name = "台北";
            city.Longitude = "121.40";
            city.Latitude = "22.92";
            city.Description = "台湾、澎湖列岛";
            lstCities.Add(city);

            city = new CityInfo();
            city.Code = "gz";
            city.Name = "广州";
            city.Longitude = "123.40";
            city.Latitude = "23.92";
            city.Description = "广州潮人";
            lstCities.Add(city);
        }

        /// <summary>
        /// 加载天气映射
        /// </summary>
        static void IniWeatherInfos()
        {
            lstWeatherInfos.Clear();

            Item weatherInfo = new Item();
            weatherInfo.Code = "0";
            weatherInfo.Name = "晴";
            lstWeatherInfos.Add(weatherInfo);

            weatherInfo = new Item();
            weatherInfo.Code = "1";
            weatherInfo.Name = "小雨";
            lstWeatherInfos.Add(weatherInfo);

            weatherInfo = new Item();
            weatherInfo.Code = "2";
            weatherInfo.Name = "中雨";
            lstWeatherInfos.Add(weatherInfo);

            weatherInfo = new Item();
            weatherInfo.Code = "3";
            weatherInfo.Name = "大雨";
            lstWeatherInfos.Add(weatherInfo);

            weatherInfo = new Item();
            weatherInfo.Code = "4";
            weatherInfo.Name = "雷雨";
            lstWeatherInfos.Add(weatherInfo);

            weatherInfo = new Item();
            weatherInfo.Code = "5";
            weatherInfo.Name = "小雪";
            lstWeatherInfos.Add(weatherInfo);

            weatherInfo = new Item();
            weatherInfo.Code = "6";
            weatherInfo.Name = "中雪";
            lstWeatherInfos.Add(weatherInfo);

            weatherInfo = new Item();
            weatherInfo.Code = "7";
            weatherInfo.Name = "大雪";
            lstWeatherInfos.Add(weatherInfo);

            weatherInfo = new Item();
            weatherInfo.Code = "8";
            weatherInfo.Name = "阴转多云";
            lstWeatherInfos.Add(weatherInfo);

            weatherInfo = new Item();
            weatherInfo.Code = "9";
            weatherInfo.Name = "多云转阴";
            lstWeatherInfos.Add(weatherInfo);

        }

        /// <summary>
        /// 加载风力映射
        /// </summary>
        static void IniWindInfos()
        {
            lstWindInfos.Clear();

            Item weatherInfo = new Item();
            weatherInfo.Code = "0";
            weatherInfo.Name = "无风";
            lstWindInfos.Add(weatherInfo);

            weatherInfo = new Item();
            weatherInfo.Code = "2";
            weatherInfo.Name = "微风";
            lstWindInfos.Add(weatherInfo);

            weatherInfo = new Item();
            weatherInfo.Code = "4";
            weatherInfo.Name = "轻风";
            lstWindInfos.Add(weatherInfo);

            weatherInfo = new Item();
            weatherInfo.Code = "6";
            weatherInfo.Name = "中风";
            lstWindInfos.Add(weatherInfo);

            weatherInfo = new Item();
            weatherInfo.Code = "8";
            weatherInfo.Name = "大风";
            lstWindInfos.Add(weatherInfo);

            weatherInfo = new Item();
            weatherInfo.Code = "10";
            weatherInfo.Name = "强风";
            lstWindInfos.Add(weatherInfo);

            weatherInfo = new Item();
            weatherInfo.Code = "12";
            weatherInfo.Name = "飓风";
            lstWindInfos.Add(weatherInfo);

        }

        /// <summary>
        /// 加载气象站信息
        /// </summary>
        static void IniStations()
        {
            lstStations.Clear();
            Login station = new Login();
            station.Code = "001";
            station.Name = "北京气象站";
            station.Password = "666666";
            lstStations.Add(station);

            station = new Login();
            station.Code = "002";
            station.Name = "南京气象站";
            station.Password = "666666";
            lstStations.Add(station);

            station = new Login();
            station.Code = "003";
            station.Name = "台北气象站";
            station.Password = "666666";
            lstStations.Add(station);
        }

        /// <summary>
        /// 取得所有城市信息
        /// </summary>
        /// <param name="login">气象站</param>
        /// <returns>返回所有城市列表</returns> 
        public static CityInfo[] GetCityEnum(Login login)
        {
            if (login == null) return null;

            //如果出现在了站点中
            if (SystemVars.lstStations.Exists(x
                => x.Code == login.Code
                && x.Password == login.Password))
            {
                return SystemVars.lstCities.ToArray();
            }

            //如果出现在了用户中
            if (SystemVars.lstUsers.Exists(x
                => x.Code == login.Code
                && x.Password == login.Password))
            {
                return SystemVars.lstCities.ToArray();
            }

            return null;
        }

        /// <summary>
        /// 取得天气代码映射关系
        /// </summary>
        /// <param name="login">气象站</param>
        /// <returns>返回所有天气代码映射关系</returns>
        public static Item[] GetWeatherEnum(Login login)
        {
            if (login == null) return null;

            //如果出现在了站点中
            if (SystemVars.lstStations.Exists(x
                => x.Code == login.Code
                && x.Password == login.Password))
            {
                return SystemVars.lstWeatherInfos.ToArray();
            }

            //如果出现在了用户中
            if (SystemVars.lstUsers.Exists(x
                => x.Code == login.Code
                && x.Password == login.Password))
            {
                return SystemVars.lstWeatherInfos.ToArray();
            }

            return null;

        }

        /// <summary>
        /// 取得风力代码映射关系
        /// </summary>
        /// <param name="login">气象站</param>
        /// <returns>返回所有风力代码映射关系</returns>
        public static Item[] GetWindEnum(Login login)
        {
            if (login == null) return null;

            //如果出现在了站点中
            if (SystemVars.lstStations.Exists(x
                => x.Code == login.Code
                && x.Password == login.Password))
            {
                return SystemVars.lstWindInfos.ToArray();
            }

            //如果出现在了用户中
            if (SystemVars.lstUsers.Exists(x
                => x.Code == login.Code
                && x.Password == login.Password))
            {
                return SystemVars.lstWindInfos.ToArray();
            }

            return null;
        }


        /// <summary>
        /// 更新用户会话
        /// </summary>
        /// <param name="login">用户信息</param> 
        public static void UpdateSession(Login login)
        {
            if (login == null) return;

            //找到用户
            UserInfo myUser = SystemVars.lstUsers.Find(x => x.Code == login.Code
                 && x.Password == login.Password);
            //如果不存在，那么退出
            if (myUser == null) return;

            //更新回调连接 
            ICallback callback = OperationContext.Current.GetCallbackChannel<ICallback>();
            myUser.Callback = callback;
        }

        /// <summary>
        /// 更新天气
        /// </summary>
        /// <param name="weather">天气</param>
        public static void UpdateWeather(Weather weather)
        {
            try
            {
                //找到那些订阅了该城市天气的用户
                List<UserInfo> users = SystemVars.lstUsers.FindAll(x => x.Cities.Contains(weather.CityCode));
                foreach (var user in users)
                {
                    if (user.Callback != null)
                    {
                        user.Callback.UpdateWeather(weather);
                    }
                }
            }
            catch
            { 
            
            }
        }

        /// <summary>
        /// 更新天气
        /// </summary>
        /// <param name="message">消息</param>
        public static void UpdateMessage(string message)
        {
            try
            {
                //通知所有用户
                List<UserInfo> users = SystemVars.lstUsers;
                foreach (var user in users)
                {
                    if (user.Callback != null)
                    {
                        user.Callback.UpdateMessage(message);
                    }
                }
            }
            catch
            { 
            
            }
        }
    }


    public class StationWeatherServer : IStationWeatherServer
    { 

        #region 初始化
        public StationWeatherServer()
        {
            //直到加载完成才初始化完成  
            SystemVars.IniData(); 
        }
        #endregion

        #region 对气象站公开服务
        /// <summary>
        /// 更新天气情况
        /// </summary>
        /// <param name="weather">天气信息</param>
        /// <param name="login">气象站</param>
        public void UpdateWeather(Weather weather, Login login)
        {
            if (login == null) return;

            //如果不存在该站点或者密码不匹配，那么丢弃
            Login myStation = SystemVars.lstStations.Find(x
                => x.Code == login.Code
                && x.Password == login.Password);
            if (myStation == null) return;

            //如果不存在该城市，那么丢弃
            if (!SystemVars.lstCities.Exists(x => x.Code == weather.CityCode)) return;

            //找到该时段的天气记录，如果到了那么修改，否则添加
            Weather myWeather = SystemVars.lstCityWeathers.Find(x
                => x.CityCode == weather.CityCode
                && x.Date == weather.Date
                && x.Time == weather.Time
                );
            if (myWeather == null)
            {
                myWeather = new Weather();
                SystemVars.lstCityWeathers.Add(myWeather);
            }

            //取得新数据
            weather.Copy(myWeather);

            //更新天气
            SystemVars.UpdateWeather(myWeather);

        }

        /// <summary>
        /// 更新消息
        /// </summary>
        /// <param name="message">消息信息</param>
        /// <param name="login">气象站</param>
        public void UpdateMessage(string message, Login login)
        {
            if (login == null) return ;
            if (string.IsNullOrWhiteSpace(message)) return;

            //如果用户不存在，那么退出
            if (!SystemVars.lstStations.Exists(x => x.Code == login.Code && x.Password == login.Password)) return;

            //更新天气
            SystemVars.UpdateMessage(message);
        
        }


        /// <summary>
        /// 获取天气
        /// </summary>
        /// <param name="login">气象站</param> 
        public Weather[] GetWeather(Login login)
        { 
            if (login == null) return null;

            //如果用户不存在，那么退出
            if (!SystemVars.lstStations.Exists(x => x.Code == login.Code && x.Password == login.Password)) return null;
           
            return SystemVars.lstCityWeathers.ToArray();
        }


        /// <summary>
        /// 取得所有城市信息
        /// </summary>
        /// <param name="login">气象站</param>
        /// <returns>返回所有城市列表</returns> 
        public CityInfo[] GetCityEnum(Login login)
        {
            return SystemVars.GetCityEnum(login);
        }

        /// <summary>
        /// 取得天气代码映射关系
        /// </summary>
        /// <param name="login">气象站</param>
        /// <returns>返回所有天气代码映射关系</returns>
        public Item[] GetWeatherEnum(Login login)
        {
            return SystemVars.GetWeatherEnum(login);
        }

        /// <summary>
        /// 取得风力代码映射关系
        /// </summary>
        /// <param name="login">气象站</param>
        /// <returns>返回所有风力代码映射关系</returns>
        public Item[] GetWindEnum(Login login)
        {
            return SystemVars.GetWindEnum(login);
        }

        #endregion

    }
}
