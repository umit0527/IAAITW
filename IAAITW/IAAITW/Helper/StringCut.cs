using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace IAAITW.Helper
{
    public static class StringCut
    {
        /// <summary>
        /// 字串截斷（中文算 2，英文算 1）
        /// </summary>
        public static string TruncateWithChinese(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;

            int len = 0;
            var sb = new StringBuilder();

            foreach (var c in value)
            {
                len += (c > 127) ? 2 : 1; // 中文算 2
                if (len > maxLength)
                {
                    sb.Append("…");
                    break;
                }
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}