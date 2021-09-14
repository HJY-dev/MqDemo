using EasyNetQ;
using EasyNetQ.Topology;
using Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IBus bus;

        public HomeController(IBus _bus)
        {
            bus = _bus;
        }

        /// <summary>
        /// 发送消息--登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("publish_msg")]
        public ApiResponseInfo apiResponseInfo() {

            var message = new Message<LoginRewardsEvent>(new LoginRewardsEvent
            {
                UserKeyId = Guid.NewGuid().ToString(),
                UserName = "Admin",
                IsFirstOperation = true
            });
            var ex = bus.Advanced.ExchangeDeclare("AccountSystem.Exchange", ExchangeType.Topic);
            bus.Advanced.PublishAsync(ex, "Login.Queue", false, message);

            return ApiResponseInfo.Response(null, ApiResponseInfo.ResponseCode.SUCCESS, "");
        }
        

    }
}
