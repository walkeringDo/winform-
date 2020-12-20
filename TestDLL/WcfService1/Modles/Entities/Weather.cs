using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;
using WeatherServices.Modles.Interfaces;

namespace WeatherServices.Modles.Entities
{
    /// <summary>
    /// 实时天气
    /// </summary>
    [DataContract]
    public class Weather:IWeather
    {
        double temperature = 0.0;
        double pressure = 0.0;
        double humidity = 0.0;
        string wind = string.Empty;
        string weatherCode = string.Empty;
        string cityCode = string.Empty;
        string date = string.Empty;
        string time = string.Empty;

        /// <summary>
        /// 天气获取日期
        /// </summary>
        [DataMember]
        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        /// <summary>
        /// 天气获取时段
        /// </summary>
        [DataMember]
        public string Time
        {
            get { return time; }
            set { time = value; }
        }

        /// <summary>
        /// 气温
        /// </summary>
        [DataMember]
        public double Temperature
        {
            get { return temperature; }
            set { temperature = value; }
        }

        /// <summary>
        /// 气压
        /// </summary>
        [DataMember]
        public double Pressure
        {
            get { return pressure; }
            set { pressure = value; }
        }

        /// <summary>
        /// 风力
        /// </summary>
        [DataMember]
        public string Wind 
        {
            get { return wind; }
            set { wind = value; }
        }

        /// <summary>
        /// 湿度
        /// </summary>
        [DataMember]
        public double Humidity
        {
            get { return humidity; }
            set { humidity = value; }
        }

        /// <summary>
        /// 天气代码
        /// </summary>
        [DataMember]
        public string WeatherCode
        {
            get { return weatherCode; }
            set { weatherCode = value; }
        }

        /// <summary>
        /// 城市代码
        /// </summary>
        [DataMember]
        public string CityCode
        {
            get { return cityCode; }
            set { cityCode = value; }
        }
        
        /// <summary>
        /// 复制项
        /// </summary>
        /// <param name="myWeather"></param>
        [OperationContract]
        public void Copy(IWeather myWeather)
        {
            myWeather.Temperature = this.Temperature;
            myWeather.Pressure = this.Pressure;
            myWeather.Humidity = this.Humidity;
            myWeather.Wind = this.Wind;
            myWeather.Date = this.Date;
            myWeather.Time = this.Time;
            myWeather.WeatherCode = this.WeatherCode;
            myWeather.CityCode = this.CityCode;
        }
    }

}