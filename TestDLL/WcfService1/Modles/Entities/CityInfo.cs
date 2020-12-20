using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WeatherServices.Modles.Interfaces;

namespace WeatherServices.Modles.Entities
{
    /// <summary>
    /// 城市信息
    /// </summary>
    [DataContract]
    public class CityInfo : Item,ICityInfo
    {
        
        //Longitude 116° 19' 11" E 东经116度19分11秒
        //Latitude 39° 57' 0" N  北纬39度57分0秒
        string longitude = "0.0";
        string latitude = "0.0";
         
        /// <summary>
        /// 经度
        /// </summary>
        [DataMember]
        public string Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember]
        public string Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

    }
}