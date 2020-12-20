using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WeatherServices.Modles.Interfaces
{
    public interface ILogin:IItem
    {
        /// <summary>
        /// 密码
        /// </summary>
        [DataMember]
        string Password
        {
            get;
            set;
        }

    }
}