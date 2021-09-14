using System;

namespace Messages
{
    /// <summary>
    /// 积分奖励 登录 EventDto
    /// </summary>
    public class LoginRewardsEvent
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserKeyId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public System.String UserName { get; set; }

        /// <summary>
        /// 是否首次操作 true 首次操作，false 非首次操作
        /// </summary>
        public bool IsFirstOperation { get; set; }
    }
}
