using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Problem4.Highway;

namespace Problem4.Generator
{
    internal static class Helper
    {
        public static int CarCapacity(CarType car)
        {
            switch (car)
            {
                case CarType.C1:
                    return 1;
                case CarType.C2:
                    return 2;
                case CarType.C3:
                    return 3;
                case CarType.C4:
                    return 4;
                case CarType.C40:
                    return 40;
            }


            return 1;
        }

        public static string GetDescription<T>(this object enummerationValue) where T : struct
        {
            Type type = enummerationValue.GetType();
            if (!type.IsEnum) throw new InvalidDataException("EnummerationValue must be of Enum Type");

            MemberInfo[] memberInfo = type.GetMember(enummerationValue.ToString());
            if (memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof (DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute) attrs[0]).Description;
                }
            }
            return enummerationValue.ToString();
        }
    }
}