using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WeatherServices.Modles.Interfaces;

namespace WeatherServices.Modles.Entities
{
    /// <summary>
    /// 用户
    /// </summary>
    [DataContract]
    public class UserInfo : Login, IUserInfo
    {
        string[] cities;
        ICallback callback;
         
        /// <summary>
        /// 订阅的城市列表
        /// </summary>
        [DataMember]
        public string[] Cities
        {
            get { return cities; }
            set { cities = value; }
        } 

        /// <summary>
        /// 回调连接
        /// </summary>
        public ICallback Callback
        {
            get { return callback; }
            set { callback = value; }
        }
         
    }
}