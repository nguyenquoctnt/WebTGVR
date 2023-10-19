using System.Collections.Generic;
using System.Linq;

namespace VDMMutiline.Core.Helper
{
    public class EnumerableHelper
    {
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
        /// <summary>
        /// Gets the dictionary value.
        /// </summary>
        /// <param name="dicInput">The dic input.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetDictionaryValue(Dictionary<double, string> dicInput, double? key)
        {
            if (key.HasValue && dicInput.ContainsKey(key.Value))
                return dicInput[key.Value];

            return string.Empty;
        }
        /// <summary>
        /// Gets the dictionaryby value.
        /// </summary>
        /// <param name="dicInput">The dic input.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int GetDictionarybyValue(Dictionary<int, string> dicInput, string value)
        {
            if (!string.IsNullOrEmpty(value) && dicInput.ContainsValue(value))
                return dicInput.FirstOrDefault(z => z.Value.ToUpper().Trim() == value.ToUpper().Trim()).Key;

            return 0;
        }
        /// <summary>
        /// Gets the dictionary value.
        /// </summary>
        /// <param name="dicInput">The dic input.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
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
    }
}
