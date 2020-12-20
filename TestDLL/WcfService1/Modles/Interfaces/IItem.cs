using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WeatherServices.Modles.Interfaces
{
    public interface IItem
    {
        /// <summary>
        /// 代码
        /// </summary>
        [DataMember]
        string Code
        {
            get;
            set;
        }

        /// <summary>
        /// 名称 
        /// </summary>
        [DataMember]
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        string Description
        {
            get;
            set;
        }

    }
}