using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODELS
{
    /// <summary>
    /// 日志实体类
    /// </summary>
    [Serializable]
    public class LoginLog
    {
        public int LoginId { get; set; }
        public string SPName { get; set; }
        public string ServerName { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime ExitTime { get; set; }
    }
}
