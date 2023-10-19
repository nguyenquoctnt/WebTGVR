using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
//using WebProject.SharedComponent.EntityInfos;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace VDMMutiline.Ultilities
{
    public class Utility
    {
        public static List<string> lstAirport = new List<string>() { "JQ", "3K", "BL", "GK" };
        public static string GetUserIPAddress()
        {
            var context = System.Web.HttpContext.Current;
            string ip = String.Empty;

            if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            else if (!String.IsNullOrWhiteSpace(context.Request.UserHostAddress))
                ip = context.Request.UserHostAddress;

            if (ip == "::1")
                ip = "127.0.0.1";

            return ip;
        }
        public static double Ceiling(double value, double significance)
        {
            //var cookie = HttpContext.Current.Request.Cookies["Language"];
            string culture = string.Empty;
            //if (cookie == null)
            //{
            //    culture = "Vi";
            //}
            //else
            //    culture = cookie.Value;
            //if (culture.ToLower().CompareTo("Vi") == 0 || string.IsNullOrEmpty(culture))
            //{
            //    culture = "Vi";
            //}
            //if (culture.ToLower().CompareTo("en-us") == 0)
            //{
            //    culture = "en-us";
            //}
            culture = "Vi";
            if ((value % significance) != 0)
            {
                return culture == "Vi" ? ((int)(value / significance) * significance) + significance : value;
            }
            return Convert.ToDouble(value);
        }
        public static double Floor(double value, double significance)
        {
            if ((value % significance) != 0)
            {
                return ((int)(value / significance) * significance);
            }

            return Convert.ToDouble(value);
        }
        private const string DATE_FORMAT = "dd/MM/yyyy";
        private const string DATETIME_FORMAT = "dd/MM/yyyy HH:mm";
        #region Text Utils
        /// <summary>
        /// Creates the tag.
        /// </summary>
        /// <param name="strInputs">The string inputs.</param>
        /// <returns></returns>
        public static string CreateTag(params string[] strInputs)
        {
            string result = string.Empty;
            foreach (string strInput in strInputs)
            {
                result += RemoveDigit(strInput) + ", ";
            }
            Regex reg = new Regex("\\s+");
            result = reg.Replace(result, " ");
            return result.Trim().ToLower();
        }
        /// <summary>
        /// Removes the digit.
        /// </summary>
        /// <param name="strInput">The string input.</param>
        /// <returns></returns>
        public static string RemoveDigit(string strInput)
        {
            if (string.IsNullOrEmpty(strInput))
            {
                return string.Empty;
            }
            else
            {
                string findText = "áàảãạâấầẩẫậăắằẳẵặđéèẽẻẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ";
                string replText = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";
                int index = -1, index2;
                while ((index = strInput.IndexOfAny(findText.ToCharArray())) != -1)
                {
                    index2 = findText.IndexOf(strInput[index]);
                    strInput = strInput.Replace(strInput[index], replText[index2]);
                }
                return strInput;
            }
        }
        #endregion
        #region Getting value methods
        /// <summary>
        /// Gets the date string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetDateString(object value)
        {
            DateTime? dateValue = value as DateTime?;
            return GetDateString(dateValue);
        }
        /// <summary>
        /// Gets the date string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetDateString(DateTime? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString(DATE_FORMAT);
        }
        public static string GetDateStringshos(DateTime? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString("dd/MM");
        }
        /// <summary>
        /// Gets the date minute string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetDateMinuteString(DateTime? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString("dd/MM/yyyy HH:mm");
        }
        public static string GetMinuteString(DateTime? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString("HH:mm");
        }
        /// <summary>
        /// Gets the date time string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetDateTimeString(DateTime? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString("dd/MM/yyyy hh:mm:ss");
        }
        /// <summary>
        /// Gets the month string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetMonthString(DateTime? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString("MM/yyyy");
        }
        /// <summary>
        /// Gets the int string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetIntString(int? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.ToString();
        }

        /// <summary>
        /// Gets the long string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetLongString(long? value)
        {
            if (value == null)
                return string.Empty;
            else
                return String.Format("{0:N0}", value.Value);
        }

        /// <summary>
        /// Gets the float string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetFloatString(float? value)
        {
            if (value == null)
                return string.Empty;
            else
                return String.Format("{0:N2}", value.Value);
        }

        /// <summary>
        /// Gets the decimal string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetDecimalString(decimal? value)
        {
            if (value == null)
                return string.Empty;
            else
                return String.Format("{0:N0}", value.Value);
        }

        #endregion
        public static DateTime GetDateFormat(string day, string format)
        {
            day = day.Replace("-", "/");
            DateTime date = DateTime.Now;
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime.TryParseExact(day, format, provider, DateTimeStyles.None, out date);
            return date;
        }
        public static DateTime GetDateFormat2(string day, string format)
        {
            if (!string.IsNullOrEmpty(day))
            {
                day = day.Replace("-", "/");
                var daysp = day.Split('/');
                return new DateTime(int.Parse(daysp[2]), int.Parse(daysp[1]), int.Parse(daysp[0]));
            }
            return new DateTime();
        }
        #region Conversion methods

        /// <summary>
        /// Converts to bool.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool? ConvertToBool(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            bool bValue = false;
            if (bool.TryParse(value, out bValue))
                return bValue;

            return null;
        }
        public static string CatChuoi(string choi, int lenth)
        {
            string value = "";
            if (!string.IsNullOrEmpty(choi))
            {
                var lenthchuoi = choi.Length;
                if (lenthchuoi > lenth)
                {
                    value = choi.Substring(0, lenth - 3);
                    value = value.Substring(0, value.LastIndexOf(" ")) + "...";
                }
                else
                {
                    value = choi;
                }
            }
            return value;
        }
        /// <summary>
        /// Converts to int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int? ConvertToInt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            int intValue = 0;
            if (int.TryParse(value, out intValue))
                return intValue;

            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ConvertToInt32(object value, int defaultValue)
        {
            try
            {
                return Convert.ToInt32(value.ToString().Trim());
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Converts to decimal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static decimal? ConvertToDecimal(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            decimal intValue = 0;
            if (decimal.TryParse(value, out intValue))
                return intValue;

            return null;
        }

        /// <summary>
        /// Converts to float.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static float? ConvertToFloat(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            float intValue = 0;
            if (float.TryParse(value, out intValue))
                return intValue;

            return null;
        }
        public static double? ConvertTodouble(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            double intValue = 0;
            if (double.TryParse(value, out intValue))
                return intValue;

            return null;
        }
        /// <summary>
        /// Converts to date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime? ConvertToDate(string value)
        {
            DateTime dt;
            bool isValid = DateTime.TryParseExact(value, DATE_FORMAT, null, System.Globalization.DateTimeStyles.None, out dt);
            if (!string.IsNullOrEmpty(value) && isValid)
            {
                return dt;
            }
            else
                return null;
        }
        public static DateTime? ConvertStringToDate(string Ngay, string Gio)
        {
            string giatringaybatdau = "";
            string giatrigiobatdau = "";
            var ngaybatdaulv = Ngay.Split('/');
            var giobatdauvl = Gio.Split(':');
            if (Ngay.Length >= 3)
            {
                giatringaybatdau = (ngaybatdaulv[0].Length == 1 ? ("0" + ngaybatdaulv[0]) : ngaybatdaulv[0]) + "/" + (ngaybatdaulv[1].Length == 1 ? ("0" + ngaybatdaulv[1]) : ngaybatdaulv[1]) + "/" + ngaybatdaulv[2];
            }
            else
                return null;
            if (giobatdauvl.Length >= 2)
            {
                giatrigiobatdau = (giobatdauvl[0].Length == 1 ? ("0" + giobatdauvl[0]) : giobatdauvl[0]) + ":" + (giobatdauvl[1].Length == 1 ? ("0" + giobatdauvl[1]) : giobatdauvl[1]);
            }
            else
                return null;
            return ConvertToDatetime(giatringaybatdau + " " + giatrigiobatdau);

        }
        public static DateTime? ConvertToDatetime(string value)
        {
            DateTime dt;
            bool isValid = DateTime.TryParseExact(value, DATETIME_FORMAT, null, System.Globalization.DateTimeStyles.None, out dt);
            if (!string.IsNullOrEmpty(value) && isValid)
            {
                return dt;
            }
            else
                return null;
        }
        /// <summary>
        /// Converts to date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="dateFormat">The date format.</param>
        /// <returns></returns>
        public static DateTime? ConvertToDate(string value, string dateFormat)
        {
            DateTime dt;
            bool isValid = DateTime.TryParseExact(value, dateFormat, null, System.Globalization.DateTimeStyles.None, out dt);
            if (!string.IsNullOrEmpty(value) && isValid)
            {
                return dt;
            }
            else
                return null;
        }

        #endregion

        #region Dictionary utils

        /// <summary>
        /// Gets the dictionary value.
        /// </summary>
        /// <param name="dicInput">The dic input.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetDictionaryValue(Dictionary<int, string> dicInput, int? key)
        {
            if (key.HasValue && dicInput.ContainsKey(key.Value))
                return dicInput[key.Value];

            return string.Empty;
        }
        public static int GetDictionarybyValue(Dictionary<int, string> dicInput, string value)
        {
            if (!string.IsNullOrEmpty(value) && dicInput.ContainsValue(value))
                return dicInput.FirstOrDefault(z => z.Value.ToUpper().Trim() == value.ToUpper().Trim()).Key;

            return 0;
        }
        public static string GetDictionaryValue(Dictionary<string, string> dicInput, string key)
        {
            if (!string.IsNullOrEmpty(key) && dicInput.ContainsKey(key))
                return dicInput[key];

            return string.Empty;
        }
        /// <summary>
        /// Gets the boolean dictionary value.
        /// </summary>
        /// <param name="dicInput">The dic input.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetBooleanDictionaryValue(Dictionary<bool, string> dicInput, bool? key)
        {
            if (key.HasValue && dicInput.ContainsKey(key.Value))
                return dicInput[key.Value];

            return string.Empty;
        }
        #endregion

        #region validate Email

        /// <summary>
        /// Validates the email address.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public static string ValidateEmailAddress(string email)
        {
            string re = string.Empty;
            bool isValid = System.Text.RegularExpressions.Regex.IsMatch(email,
              @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

            if (!isValid)
            {
                re = string.Format("Địa chỉ Email {0} của bạn không đúng. <br/>", email);
            }

            return re;
        }

        #endregion

        /// <summary>
        /// Normalizes the page URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static string NormalizePageUrl(string url)
        {
            Regex rg = new Regex(@"(/ui/.*?\.aspx\?type=\w+)|(/ui/.*?\.aspx)");

            if (rg.IsMatch(url))
            {
                url = rg.Match(url).Value;
                return url;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            var toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);


            var key = "DUONG@KEY";
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            var tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            var cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            var resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            //get the byte code of the string

            var toEncryptArray = Convert.FromBase64String(cipherString);

            //Get your key from config file to open the lock!
            var key = "DUONG@KEY";

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            var tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            var cTransform = tdes.CreateDecryptor();
            var resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        public static string ConvertToUnsign(string str)
        {
            if (str == null)
                return string.Empty;
            string[] signs = new string[] {
        "aAeEoOuUiIdDyY",
        "áàạảãâấầậẩẫăắằặẳẵ",
        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
        "éèẹẻẽêếềệểễ",
        "ÉÈẸẺẼÊẾỀỆỂỄ",
        "óòọỏõôốồộổỗơớờợởỡ",
        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
        "úùụủũưứừựửữ",
        "ÚÙỤỦŨƯỨỪỰỬỮ",
        "íìịỉĩ",
        "ÍÌỊỈĨ",
        "đ",
        "Đ",
        "ýỳỵỷỹ",
        "ÝỲỴỶỸ"
   };
            for (int i = 1; i < signs.Length; i++)
            {
                for (int j = 0; j < signs[i].Length; j++)
                {
                    str = str.Replace(signs[i][j], signs[0][i - 1]);
                }
            }
            str = str.Replace("à", "a").Replace("á", "a").Replace("ạ", "a").Replace("ã", "a").Replace("ả", "a").Replace("Ạ", "a");
            str = str.Replace("ắ", "a").Replace("ằ", "a").Replace("ặ", "a").Replace("ẵ", "a").Replace("ẳ", "a").Replace("ă", "a");
            str = str.Replace("ấ", "a").Replace("ầ", "a").Replace("ậ", "a").Replace("ẫ", "a").Replace("ẩ", "a").Replace("â", "a");

            // - Special with char 'D'
            str = str.Replace("đ", "d");

            // - Special with char 'E'
            str = str.Replace("è", "e").Replace("é", "e").Replace("ẹ", "e").Replace("ẽ", "e").Replace("ẻ", "e");
            str = str.Replace("ề", "e").Replace("ế", "e").Replace("ệ", "e").Replace("ễ", "e").Replace("ể", "e").Replace("ê", "e").Replace("Ẽ", "e").Replace("Ẽ", "e");

            // - Special with char 'O'
            str = str.Replace("ò", "o").Replace("ó", "o").Replace("ọ", "o").Replace("õ", "o").Replace("ỏ", "o").Replace("Ò", "o");
            str = str.Replace("ồ", "o").Replace("ố", "o").Replace("ộ", "o").Replace("ỗ", "o").Replace("ổ", "o").Replace("ô", "o");
            str = str.Replace("ờ", "o").Replace("ớ", "o").Replace("ợ", "o").Replace("ỡ", "o").Replace("ở", "o").Replace("ơ", "o");

            // - Special with char 'U'
            str = str.Replace("ù", "u").Replace("ú", "u").Replace("ụ", "u").Replace("ũ", "u").Replace("ủ", "u");
            str = str.Replace("ừ", "u").Replace("ứ", "u").Replace("ự", "u").Replace("ữ", "u").Replace("ử", "u").Replace("ư", "u");

            // - Special with char 'i'
            str = str.Replace("í", "i").Replace("ì", "i").Replace("ị", "i").Replace("ĩ", "i").Replace("ỉ", "i");

            // - Special with char 'y');
            str = str.Replace("ỳ", "y").Replace("ý", "y").Replace("ỵ", "y").Replace("ỹ", "y").Replace("ỷ", "i");

            return ConvertToUnSign2(str);
        }
        public static string ConvertToUnSign2(string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }
        public static void SetCookie(string key, string value, int expires)
        {
            var encodedCookie = new HttpCookie(key, value);
            if (HttpContext.Current.Request.Cookies[key] != null)
            {
                var cookieOld = HttpContext.Current.Request.Cookies[key];
                cookieOld.Expires = DateTime.Now.AddDays(expires);
                cookieOld.Value = encodedCookie.Value;
                HttpContext.Current.Response.Cookies.Add(cookieOld);
            }
            else
            {
                encodedCookie.Expires = DateTime.Now.AddDays(expires);
                HttpContext.Current.Response.Cookies.Add(encodedCookie);
            }
        }
        public static string GetCookie(string key)
        {
            string value = string.Empty;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];

            if (cookie != null)
            {

                HttpCookie decodedCookie = cookie;
                value = decodedCookie.Value;
            }
            return value;
        }
        public static string MonthName(int m)
        {
            string res;
            switch (m)
            {
                case 1:
                    res = "Jan";
                    break;
                case 2:
                    res = "Feb";
                    break;
                case 3:
                    res = "Mar";
                    break;
                case 4:
                    res = "Apr";
                    break;
                case 5:
                    res = "May";
                    break;
                case 6:
                    res = "Jun";
                    break;
                case 7:
                    res = "Jul";
                    break;
                case 8:
                    res = "Aug";
                    break;
                case 9:
                    res = "Sep";
                    break;
                case 10:
                    res = "Oct";
                    break;
                case 11:
                    res = "Nov";
                    break;
                case 12:
                    res = "Dec";
                    break;
                default:
                    res = "";
                    break;
            }
            return res.ToUpper();
        }
        public static bool Checkstringinput(string check)
        {
            string Dau =
              "À|Á|Â|Ã|È|É|Ê|Ì|Í|Ò|Ó|Ô|Õ|Ù|Ú|Ă|Đ|Ĩ|Ũ|Ơ|à|á|â|ã|è|é|ê|ì|í|ò|ó|ô|õ|ù|ú|ă|đ|ĩ|ũ|ơ|Ư|Ă|Ạ|Ả|Ấ|Ầ|Ẩ|Ẫ|Ậ|Ắ|Ằ|Ẳ|Ẵ|Ặ|Ẹ|Ẻ|Ẽ|Ề|Ề|Ể|ư|ă|ạ|ả|ấ|ầ|ẩ|ẫ|ậ|ắ|ằ|ẳ|ẵ|ặ|ẹ|ẻ|ẽ|ề|ề|ể|Ễ|Ệ|Ỉ|Ị|Ọ|Ỏ|Ố|Ồ|Ổ|Ỗ|Ộ|Ớ|Ờ|Ở|Ỡ|Ợ|Ụ|Ủ|Ứ|Ừ|ễ|ệ|ỉ|ị|ọ|ỏ|ố|ồ|ổ|ỗ|ộ|ớ|ờ|ở|ỡ|ợ|ụ|ủ|ứ|ừ|Ử|Ữ|Ự|Ỳ|Ỵ|Ý|Ỷ|Ỹ|ử|ữ|ự|ỳ|ỵ|ỷ|ỹ|";
            string kytudacbiet = "`|~|!|@|$|%|^|&|*|(|)|-|=|+|{|}|[|]|;|:|\"|'|<|>|,|.|?|/| |";
            foreach (var items in Dau)
            {
                if (check.Contains(items.ToString()))
                {
                    return false;
                    break;
                }
            }
            foreach (var items in kytudacbiet)
            {
                if (check.Contains(items.ToString()))
                {
                    return false;
                    break;
                }
            }
            return true;
        }
        public static void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }
        }
    }

}