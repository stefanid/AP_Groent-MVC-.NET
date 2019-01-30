using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ASPAPMVC.Models
{
    public enum OrderStatus
    {
        [Description("Outgoing Orders")]
        Pending = 10,
        [Description("Received Orders")]
        Received = 20

    }

   public static class EnumDesc
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null) return null;
            var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
            return attribute.Description;
        }
    }

}