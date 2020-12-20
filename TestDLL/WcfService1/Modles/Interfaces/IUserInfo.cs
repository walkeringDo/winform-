using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WeatherServices.Modles.Interfaces
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public interface IUserInfo : ILogin
    {

        /// <summary>
        /// 订阅的城市列表
        /// </summary>
        [DataMember]
        string[] Cities
        {
            get;
            set;
        }

        /// <summary>
        /// 回调
        /// </summary>
        ICallback Callback
        {
            get;
            set;
        }
         
    }
}