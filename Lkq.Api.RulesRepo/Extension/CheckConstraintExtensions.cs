using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;

namespace Lkq.Api.RulesRepo.Extension
{
    /// <summary>
    /// CheckConstraintExtensions
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class CheckConstraintExtensions
    {
        /// <summary>
        /// Check the given int less than or Equal to given value
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="givenInt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool CheckLessThanOrEqual(this int obj, int givenInt, [OptionalAttribute] string msg)
        {
            if (obj <= givenInt)
            {
                throw new ArgumentOutOfRangeException("",msg);
            }
            return true;
        }
        /// <summary>
        /// Check the given int greater than given value
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="givenInt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool CheckGreaterThan(this int obj, int givenInt, [OptionalAttribute] string msg)
        {
            if (givenInt > obj)
            {
                throw new ArgumentOutOfRangeException("", msg);
            }
            return true;
        }

        /// <summary>
        /// Check the given String is Null or Empty
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool CheckNullOrEmpty(this string obj, [OptionalAttribute] string msg)
        {

            if (string.IsNullOrEmpty(obj))
            {
                throw new ArgumentNullException(obj, msg);
            }
            return true;
        }
        /// <summary>
        /// Check the Length of the string equals to given value
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool CheckLengthNotEquals(this string obj, int value, [OptionalAttribute] string msg)
        {
            if (obj.Length != value)
            {
                throw new ArgumentOutOfRangeException("", msg);
            }
            return true;
        }

        /// <summary>
        /// Check for valid enum value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool IsValidEnum<T>(this T value, string msg)
        {
            if (!Enum.IsDefined(value.GetType(), value))
            {
                throw new InvalidEnumArgumentException(msg);
            }
            return true;
        }

        /// <summary>
        /// Check for valid enum value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="invalidValue"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool IsValidLobEnum<T>(this T value, T invalidValue, string msg)
        {
            if (value.Equals(null) || !Enum.IsDefined(value.GetType(), value) ||  value.Equals(invalidValue))
            {
                throw new InvalidEnumArgumentException(msg);
            }
            return true;
        }

        /// <summary>
        /// Check the given object is Null
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool CheckObjNull(this object obj, [OptionalAttribute] string msg)
        {

            if (obj == null)
            {
                throw new ArgumentNullException("",msg);
            }
            return true;
        }

        /// <summary>
        /// Check the given value in between the range
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool CheckValueInRange(this int obj,int from, int to ,[OptionalAttribute] string msg)
        { 
            if (!Enumerable.Range(from, to+1).Contains(obj))
            {
                throw new ArgumentOutOfRangeException("",msg);
            }
            return true;
        }

        /// <summary>
        /// Check the given int list less than or Equal to given value
        /// </summary>
        ///<param name="obj"></param>
        /// <param name="givenInt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool CheckValueLessThanOrEqual(this List<string> obj, int givenInt, [OptionalAttribute] string msg)
        {
            foreach (var objVal in obj)
            {
                if(objVal.ToUpper() != "ALL")
                if (int.Parse(objVal) <= givenInt)
                {
                    throw new ArgumentOutOfRangeException("", msg);
                }

            }

            return true;
        }

    }
}
