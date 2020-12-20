using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WeatherServices.Modles.Interfaces
{
    /// <summary>
    /// 天气
    /// </summary> 
    public interface IWeather
    {
        /// <summary>
        /// 天气获取日期
        /// </summary>
        [DataMember]
        string Date
        {
            get;
            set;
        }

        /// <summary>
        /// 天气获取时间
        /// </summary>
        [DataMember]
        string Time
        {
            get;
            set;
        }
         
        /// <summary>
        /// 气温
        /// </summary>
        [DataMember]
        double Temperature
        {
            get;
            set;
        }

        /// <summary>
        /// 气压
        /// </summary>
        [DataMember]
        double Pressure
        {
            get;
            set;
        }

        /// <summary>
        /// 风力
        /// </summary>
        [DataMember]
        string Wind
        {
            get;
            set;
        }

        /// <summary>
        /// 湿度
        /// </summary>
        [DataMember]
        double Humidity
        {
            get;
            set;
        }

        /// <summary>
        /// 天气代码
        /// </summary>
        [DataMember]
        string WeatherCode
        {
            get;
            set;
        }

        /// <summary>
        /// 城市代码
        /// </summary>
        [DataMember]
        string CityCode
        {
            get;
            set;
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="myWeather"></param>
        void Copy(IWeather myWeather);

    }
}