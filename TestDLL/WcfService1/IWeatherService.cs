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

    [ServiceContract(CallbackContract = typeof(ICallback))]   
    public interface IClientWeatherServer
    {
        #region 对订阅用户公开的服务
        /// <summary>
        /// 订阅服务
        /// </summary>
        /// <param name="user">用户信息(包括订阅的城市列表)</param>
        /// <returns>返回订阅成功时的用户信息</returns>
        [OperationContract]
        UserInfo BookWeather(UserInfo user);
         
        /// <summary>
        /// 取消服务
        /// </summary>
        /// <param name="login">用户信息</param>
        /// <returns>返回成功或失败</returns>
        [OperationContract]
        bool CancelWeather(Login login);

        /// <summary>
        /// 获取天气
        /// </summary>
        /// <param name="login"></param>
        [OperationContract]
        Weather[] GetWeather(Login login);

        /// <summary>
        /// 取得所有城市信息
        /// </summary>
        /// <param name="login">用户信息</param>
        /// <returns>返回所有城市列表</returns>
        [OperationContract]
        CityInfo[] GetCityEnum(Login login);

        /// <summary>
        /// 取得天气代码映射关系
        /// </summary>
        /// <param name="login">用户信息</param>
        /// <returns>返回所有天气代码映射关系</returns>
        [OperationContract]
        Item[] GetWeatherEnum(Login login);

        /// <summary>
        /// 取得风力映射关系
        /// </summary>
        /// <param name="login">用户信息</param>
        /// <returns>返回所有风力代码映射关系</returns>
        [OperationContract]
        Item[] GetWindEnum(Login login);

        /// <summary>
        /// 更新登录信息
        /// </summary>
        /// <param name="login">用户信息</param>
        [OperationContract]
        void UpdateSession(Login login);
        #endregion  
    }

    [ServiceContract]
    public interface IStationWeatherServer
    {
        #region 对气象站公开服务
        /// <summary>
        /// 更新天气情况
        /// </summary>
        /// <param name="weather">天气信息</param>
        /// <param name="login">气象站</param>
        [OperationContract]
        void UpdateWeather(Weather weather, Login login);

        /// <summary>
        /// 更新消息
        /// </summary>
        /// <param name="message">消息信息</param>
        /// <param name="login">气象站</param>
        [OperationContract]
        void UpdateMessage(string message, Login login);


        /// <summary>
        /// 获取天气
        /// </summary>
        /// <param name="login">气象站</param>
        [OperationContract]
        Weather[] GetWeather(Login login);

        /// <summary>
        /// 取得所有城市信息
        /// </summary>
        /// <param name="login">气象站</param>
        /// <returns>返回所有城市列表</returns>
        [OperationContract]
        CityInfo[] GetCityEnum(Login login);

        /// <summary>
        /// 取得天气代码映射关系
        /// </summary>
        /// <param name="login">气象站</param>
        /// <returns>返回所有天气代码映射关系</returns>
        [OperationContract]
        Item[] GetWeatherEnum(Login login);

        /// <summary>
        /// 取得风力映射关系
        /// </summary>
        /// <param name="loginn">气象站</param>
        /// <returns>返回所有风力代码映射关系</returns>
        [OperationContract]
        Item[] GetWindEnum(Login login);
        #endregion
    }

    //用于回调的契约
    public interface ICallback
    {
        /// <summary>
        /// 更新天气
        /// </summary>
        /// <param name="weather">天气</param>
        [OperationContract(IsOneWay = true)]
        void UpdateWeather(Weather weather);

        /// <summary>
        /// 更新消息
        /// </summary>
        /// <param name="message">消息</param>
        [OperationContract(IsOneWay = true)]
        void UpdateMessage(string message);
         
    }
}
