using Newtonsoft.Json;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Saman.Backend.Share.shareClasses
{
    public class shareConvertor
    {
        public static string JsonSerialize(object obj)
        {
            try { return JsonConvert.SerializeObject(obj); }
            catch { }

            return "";
        }

        public static T? JsonDeserialize<T>(string? jsonString)
        {
            try { return JsonConvert.DeserializeObject<T>(jsonString ?? ""); }
            catch { }

            return default;
        }

        public static string strGUID()
        {
            var guid = Guid.NewGuid();

            // Convert the GUID to a byte array and encode it as Base64
            var bytes = guid.ToByteArray();
            var base64 = Convert.ToBase64String(bytes);

            // Remove any non-alphabetic characters from the string
            var result = "";
            foreach (var c in base64)
            {
                if (Char.IsLetter(c))
                {
                    result += c;
                }
            }

            return result;
        }

        public static void RefactorFarsiProperties<T>(T entity)
        {
            try
            {
                PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo p in properties)
                {
                    // Only work with strings
                    if (p.PropertyType != typeof(string)) { continue; }

                    // If not writable then cannot null it; if not readable then cannot check it's value
                    if (!p.CanWrite) { continue; }

                    // Ignore null value
                    if (p.GetValue(entity) == null) { continue; }

                    p.SetValue(entity, strFarsi(p.GetValue(entity)!));
                }
            }
            catch { }
        }

        public static string strFarsi(object p)
        {
            try
            {
                return str(p)
                    .Replace('ؤ', 'و')
                    .Replace('ك', 'ک')
                    .Replace('ي', 'ی')
                    .Replace('ۀ', 'ه')
                    .Replace('ي', 'ی')
                    //.Replace('ئ', 'ی')
                    .Replace('ى', 'ی')
                    .Replace('ى', 'ی')
                    .Replace('ي', 'ی')
                    //.Replace('ئ', 'ی')
                    .Replace('ء', 'ی')
                    .Replace('ة', 'ه')
                    .Replace('ؤ', 'و')
                    .Replace('إ', 'ا')
                    .Replace('ٱ', 'ا')
                    .Replace('أ', 'ا')
                    .Replace("ﻷ", "لا")
                    .Replace('۰', '0')
                    .Replace('۱', '1')
                    .Replace('۲', '2')
                    .Replace('۳', '3')
                    .Replace('۴', '4')
                    .Replace('۵', '5')
                    .Replace('۶', '6')
                    .Replace('۷', '7')
                    .Replace('۸', '8')
                    .Replace('۹', '9')
                    .Replace('٠', '0')
                    .Replace('١', '1')
                    .Replace('٢', '2')
                    .Replace('٣', '3')
                    .Replace('٤', '4')
                    .Replace('٥', '5')
                    .Replace('٦', '6')
                    .Replace('٧', '7')
                    .Replace('٨', '8')
                    .Replace('٩', '9');
            }
            catch
            {
                return "";
            }
        }

        public static byte[] strToByte(string value) { try { return System.Text.Encoding.UTF8.GetBytes(value); } catch { return null!; } }

        public static string byteToString(byte[] value) { try { return System.Text.Encoding.UTF8.GetString(value, 0, value.Length); } catch { return string.Empty; } }

        public static string str(object? p) { try { return (p == null) ? "" : p.ToString()!; } catch { return ""; } }

        /// <summary>
        /// p must be float or number
        /// </summary>
        public static string toRial(object p, bool showRial) { try { return float0(p).ToString("#,##0") + ((showRial) ? " ریال" : ""); } catch { return "0"; } }

        public static double double0(object p)
        {
            try
            {
                return (p == null) ? 0 : Convert.ToDouble(p);
            }
            catch
            {
                return 0;
            }
        }

        public static double double1(string p)
        {
            try
            {
                return (p == null) ? 1 : Convert.ToDouble(p);
            }
            catch
            {
                return 1;
            }
        }
        public static long long0(object? p)
        {
            try
            {
                return (p == null) ? 0 : Convert.ToInt64(p);
            }
            catch
            {
                return 0;
            }
        }

        public static short short0(object? p)
        {
            try
            {
                return (p == null) ? (short)0 : Convert.ToInt16(p);
            }
            catch
            {
                return 0;
            }
        }
        public static short short1(object p)
        {
            try
            {
                return (p == null) ? (short)1 : Convert.ToInt16(p);
            }
            catch
            {
                return 1;
            }
        }

        public static int int0(object? p)
        {
            try
            {
                return (p == null) ? 0 : Convert.ToInt32(p);
            }
            catch
            {
                return 0;
            }
        }
        public static int int1(object? p)
        {
            try
            {
                return (p == null) ? 1 : Convert.ToInt32(p);
            }
            catch
            {
                return 1;
            }
        }

        public static float float0(object p)
        {
            try
            {
                return (p == null) ? 0 : float.Parse(str(p));
            }
            catch
            {
                return 0;
            }
        }
        public static float float1(object p)
        {
            try
            {
                return (p == null) ? 1 : float.Parse(str(p));
            }
            catch
            {
                return 1;
            }
        }

        public static decimal decimal0(object p)
        {
            try
            {
                return Convert.ToDecimal(p);
            }
            catch
            {
                return 0;
            }
        }
        public static decimal decimal1(object p)
        {
            try
            {
                return Convert.ToDecimal(p);
            }
            catch
            {
                return 1;
            }
        }

        public static Boolean boolF(object p)
        {
            try
            {
                return Convert.ToBoolean(p);
            }
            catch
            {
                return false;
            }
        }
        public static Boolean boolT(object p)
        {
            try
            {
                return Convert.ToBoolean(p);
            }
            catch
            {
                return true;
            }
        }

        public static bool IntToBool(int p)
        {
            try
            {
                if (p == 1)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static int BoolToInt(bool p)
        {
            if (p)
                return 1;
            else
                return 0;
        }

        public static bool EmailIsValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }
    }
}
