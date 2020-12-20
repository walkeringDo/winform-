using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WeatherServices.Modles.Interfaces;

namespace WeatherServices.Modles.Entities
{
    [DataContract]
    public class Login : Item, ILogin
    {
        private string password;
        /// <summary>
        /// 密码
        /// </summary>
        [DataMember]
        public string Password
        {
            get { return password; }
            set { password = value; }
        } 
    }
}