using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODELS
{
    [Serializable]
    public class SalePerson
    {
        public int SalesPersonId { get; set; }
        public string SPName { get; set; }
        public string LoginPwd { get; set; }
        public int LoginLogId { get; set; }//登录的日志Id用来记录退出事件
    }
}
