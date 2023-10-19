using System.Web;
namespace VDMMutiline.Dao
{
    public class StringFormat
    {
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
        public static string UrlHeper(string Name)
        {
            var strValue = string.Empty;
            if (!string.IsNullOrEmpty(Name))
            {
                Name = CatChuoi(Name, 75);
                var str = HttpUtility.HtmlDecode(Name);
                strValue = str.ToLower().Trim();
                strValue = strValue.Replace("à", "a").Replace("á", "a").Replace("ạ", "a").Replace("ã", "a").Replace("ả", "a");
                strValue = strValue.Replace("ắ", "a").Replace("ằ", "a").Replace("ặ", "a").Replace("ẵ", "a").Replace("ẳ", "a").Replace("ă", "a");
                strValue = strValue.Replace("ấ", "a").Replace("ầ", "a").Replace("ậ", "a").Replace("ẫ", "a").Replace("ẩ", "a").Replace("â", "a");
                strValue = strValue.Replace("đ", "d");
                strValue = strValue.Replace("è", "e").Replace("é", "e").Replace("ẹ", "e").Replace("ẽ", "e").Replace("ẻ", "e");
                strValue = strValue.Replace("ề", "e").Replace("ế", "e").Replace("ệ", "e").Replace("ễ", "e").Replace("ể", "e").Replace("ê", "e");
                strValue = strValue.Replace("ò", "o").Replace("ó", "o").Replace("ọ", "o").Replace("õ", "o").Replace("ỏ", "o");
                strValue = strValue.Replace("ồ", "o").Replace("ố", "o").Replace("ộ", "o").Replace("ỗ", "o").Replace("ổ", "o").Replace("ô", "o");
                strValue = strValue.Replace("ờ", "o").Replace("ớ", "o").Replace("ợ", "o").Replace("ỡ", "o").Replace("ở", "o").Replace("ơ", "o");
                strValue = strValue.Replace("ù", "u").Replace("ú", "u").Replace("ụ", "u").Replace("ũ", "u").Replace("ủ", "u");
                strValue = strValue.Replace("ừ", "u").Replace("ứ", "u").Replace("ự", "u").Replace("ữ", "u").Replace("ử", "u").Replace("ư", "u");
                strValue = strValue.Replace("í", "i").Replace("ì", "i").Replace("ị", "i").Replace("ĩ", "i").Replace("ỉ", "i");
                strValue = strValue.Replace("ỳ", "y").Replace("ý", "y").Replace("ỵ", "y").Replace("ỹ", "y").Replace("ỷ", "i");
                strValue = System.Text.RegularExpressions.Regex.Replace(strValue, "[^0-9a-zA-Z]+?", "-").Replace("--", "-");
            }
            return strValue.Replace(" ", "-");
        }

    }
}