using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WeatherServices.Modles.Interfaces;

namespace WeatherServices.Modles.Entities
{
    /// <summary>
    /// 列表项
    /// </summary>
    [DataContract]
    public class Item : IItem
    {
        string code = string.Empty;
        string name = string.Empty;
        string description = string.Empty;
       

        /// <summary>
        /// 代码
        /// </summary>
        [DataMember]
        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        /// <summary>
        /// 名称 
        /// </summary>
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

    }
}