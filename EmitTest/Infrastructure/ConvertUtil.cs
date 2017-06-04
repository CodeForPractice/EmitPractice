using System;

namespace Infrastructure
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：ConvertUtil.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：转换工具类
    /// 创建标识：yjq 2017/6/3 15:30:03
    /// </summary>
    public static class ConvertUtil
    {
        public static byte ToByte(object value)
        {
            if (value is byte)
            {
                return (byte)value;
            }
            return Convert.ToByte(value);
        }

        public static sbyte ToSByte(object value)
        {
            if (value is sbyte)
            {
                return (sbyte)value;
            }
            return Convert.ToSByte(value);
        }

        public static short ToInt16(object value)
        {
            if (value is short)
            {
                return (short)value;
            }
            return Convert.ToInt16(value);
        }

        public static ushort ToUInt16(object value)
        {
            if (value is ushort)
            {
                return (ushort)value;
            }
            return Convert.ToUInt16(value);
        }

        public static int ToInt32(object value)
        {
            if (value is int)
            {
                return (int)value;
            }
            if (value is Enum)
            {
                return Convert.ToInt32(((Enum)value).ToString("d"));
            }
            return Convert.ToInt32(value);
        }

        public static uint ToUInt32(object value)
        {
            if (value is uint)
            {
                return (uint)value;
            }
            return Convert.ToUInt32(value);
        }

        public static long ToInt64(object value)
        {
            if (value is long)
            {
                return (long)value;
            }
            return Convert.ToInt64(value);
        }

        public static ulong ToUInt64(object value)
        {
            if (value is ulong)
            {
                return (ulong)value;
            }
            return Convert.ToUInt64(value);
        }

        public static bool ToBoolean(object value)
        {
            if (value == null)
            {
                return false;
            }
            if (value is bool)
            {
                return (bool)value;
            }
            if (value.Equals("1") || value.Equals("-1"))
            {
                value = "true";
            }
            else if (value.Equals("0"))
            {
                value = "false";
            }

            return Convert.ToBoolean(value);
        }

        public static decimal ToDecimal(object value)
        {
            if (value is decimal)
            {
                return (decimal)value;
            }

            return Convert.ToDecimal(value);
        }

        public static double ToDouble(object value)
        {
            if (value is double)
            {
                return (double)value;
            }

            return Convert.ToDouble(value);
        }

        public static float ToFloat(object value)
        {
            if (value is float)
            {
                return (float)value;
            }

            return Convert.ToSingle(value);
        }

        public static char ToChar(object value)
        {
            if (value is char)
            {
                return (char)value;
            }

            return Convert.ToChar(value);
        }

        public static Guid ToGuid(object value)
        {
            if (value is Guid)
            {
                return (Guid)value;
            }
            return Guid.Parse(value.ToString());
        }

        public static DateTime ToDateTime(object value)
        {
            if (value is DateTime)
            {
                return (DateTime)value;
            }
            return Convert.ToDateTime(value);
        }

        public static byte? ToNByte(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (value is byte?)
            {
                return (byte?)value;
            }
            return Convert.ToByte(value);
        }

        public static sbyte? ToNSByte(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (value is sbyte?)
            {
                return (sbyte?)value;
            }
            return Convert.ToSByte(value);
        }

        public static short? ToNInt16(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (value is short?)
            {
                return (short?)value;
            }
            return Convert.ToInt16(value);
        }

        public static ushort? ToNUInt16(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (value is ushort)
            {
                return (ushort)value;
            }

            return Convert.ToUInt16(value);
        }

        public static int? ToNInt32(object value)
        {
            if (value == null)
            {
                return null;
            }

            if (value is int)
            {
                return (int)value;
            }
            if (value is Enum)
            {
                return Convert.ToInt32(((Enum)value).ToString("d"));
            }
            return Convert.ToInt32(value);
        }

        public static uint? ToNUInt32(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (value is uint)
            {
                return (uint)value;
            }
            return Convert.ToUInt32(value);
        }

        public static long? ToNInt64(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (value is long)
            {
                return (long)value;
            }

            return Convert.ToInt64(value);
        }

        public static ulong? ToNUInt64(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (value is ulong?)
            {
                return (ulong?)value;
            }
            return Convert.ToUInt64(value);
        }

        public static bool? ToNBoolean(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (value is bool)
            {
                return (bool)value;
            }
            if (value.Equals("1") || value.Equals("-1"))
            {
                value = "true";
            }
            else if (value.Equals("0"))
            {
                value = "false";
            }

            return Convert.ToBoolean(value);
        }

        public static decimal? ToNDecimal(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (value is decimal)
            {
                return (decimal)value;
            }

            return Convert.ToDecimal(value);
        }

        public static double? ToNDouble(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (value is double)
            {
                return (double)value;
            }

            return Convert.ToDouble(value);
        }

        public static float? ToNFloat(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (value is float)
            {
                return (float)value;
            }

            return Convert.ToSingle(value);
        }

        public static char? ToNChar(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (value is char)
            {
                return (char)value;
            }

            return Convert.ToChar(value);
        }

        public static Guid? ToNGuid(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (value is Guid)
            {
                return (Guid)value;
            }
            return Guid.Parse(value.ToString());
        }

        public static DateTime? ToNDateTime(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (value is DateTime)
            {
                return (DateTime)value;
            }
            return Convert.ToDateTime(value);
        }
    }
}