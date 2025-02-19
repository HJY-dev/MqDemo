﻿using EasyNetQ;
using EasyNetQ.Topology;
using Messages;
using Microsoft.AspNetCore.Mvc;
using System;

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
            var queue = bus.Advanced.QueueDeclare("Login.Queue");


            //bus.Advanced.Bind(ex, queue,"Login.Queue");

            //bus.Advanced.PublishAsync(ex, "Login.Queue", false, message);

            // 5分钟后 发送消息
            bus.Scheduler.FuturePublishAsync(new OrdersEvent{ UserKeyId="admin",OrdersId=Guid.NewGuid().ToString("N"),CreateTime=DateTime.Now },TimeSpan.FromMinutes(5));
            
            return ApiResponseInfo.Response(null, ApiResponseInfo.ResponseCode.SUCCESS, "");
        }
        

    }
}
