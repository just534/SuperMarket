using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SMProject
{
   public  class Validation
    {
        /// <summary>
        /// 正则表达式验证
        /// </summary>
        /// <param name="content">要验证的内容</param>
        /// <param name="regxExpress">正则表达式字符串集合</param>
        /// <returns>当符合要求时返回True,有一条不符合就返回False</returns>
        public static bool IsRegxMatch(string content,List<string> regxExpress)
        {
            bool Ismatched = true;
            foreach (string item in regxExpress)
            {
                Regex rex = new Regex(item,RegexOptions.IgnoreCase);
                if(! rex.IsMatch(content)) Ismatched=false;
            }
            return Ismatched;
        }
    }
}
