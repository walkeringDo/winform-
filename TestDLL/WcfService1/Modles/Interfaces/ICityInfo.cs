using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WeatherServices.Modles.Interfaces
{
    /// <summary>
    /// 城市信息
    /// </summary> 
    public interface ICityInfo:IItem
    { 
       
        /// <summary>
        /// 经度
        /// </summary>
        [DataMember]
        string Longitude
        {
            get;
            set;
        }

        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember]
        string Latitude
        {
            get;
            set;
        }
         
    }
}