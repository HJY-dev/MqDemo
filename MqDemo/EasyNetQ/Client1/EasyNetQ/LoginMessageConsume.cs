using EasyNetQ;
using EasyNetQ.Topology;
using Messages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client1.EasyNetQ
{
    public class LoginMessageConsume
    {
        private readonly IBus bus;
        private ILogger logger;

        public LoginMessageConsume(IBus _bus, ILoggerFactory _loggerFactory)
        {
            bus = _bus;
            logger = _loggerFactory.CreateLogger("LoginMessageConsume");
        }

        /// <summary>
        /// 接收信号--登录
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="timeout"></param>
        /// <param name="cancellationToken"></param>
        public void Subscriber(string exchanges, string queueName, TimeSpan timeout, CancellationToken cancellationToken)
        {
            var ex = bus.Advanced.ExchangeDeclare(exchanges, ExchangeType.Topic);
            var qu = bus.Advanced.QueueDeclare(queueName);
            bus.Advanced.Bind(ex, qu, "Login.Queue");
            bus.Advanced.Consume(qu, (body, properties, info) => {
                var content = Encoding.UTF8.GetString(body);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    var res = JsonConvert.DeserializeObject<LoginRewardsEvent>(content);

                    logger.LogInformation($"操作时间:{DateTime.Now}:用户:{res.UserName}:登录:" + content);

                    //缓存拦截，防止多次重复性操作
                    ObjectCache oCache = MemoryCache.Default;
                    string messageCache = oCache["Login" + res.UserKeyId] as string;

                    if (messageCache != null)
                    {
                        //添加数据库日志
                        return;
                    }
                    else
                    {
                        CacheItemPolicy policy = new CacheItemPolicy();
                        policy.AbsoluteExpiration = DateTime.Now.AddSeconds(10);
                        oCache.Set("Login" + res.UserKeyId, res.UserKeyId, policy);
                    }

                    #region 登录逻辑处理


                    #endregion

                }

            });

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                cancellationToken.WaitHandle.WaitOne(timeout);
            }

        }
    }
}
