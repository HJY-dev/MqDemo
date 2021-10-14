using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    /// <summary>
    /// 订单延时对象
    /// </summary>
    public class OrdersEvent
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserKeyId { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrdersId { get; set; }
        /// <summary>
        /// 订单创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
