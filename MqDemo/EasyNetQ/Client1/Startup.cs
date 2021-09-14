using Client1.EasyNetQ;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            services.AddCors(options => options.AddPolicy("CorsPolicy",
                  builder =>
                  {
                      builder.SetIsOriginAllowed(c => true)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                  }));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Client1", Version = "v1" });
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                c.IncludeXmlComments(Path.Combine(basePath, "Project.xml"), true);
                c.IgnoreObsoleteActions();
                c.DocInclusionPredicate((docName, description) => true);
            });

            #region EasyNetQ

            // EasyNetQ ���Դ�ӡ��־
            //LogProvider.SetCurrentLogProvider(ConsoleLogProvider.Instance);

            services.AddSingleton(RabbitHutch.CreateBus(Configuration["EasyNetQ:Dev"]));

            services.AddSingleton<LoginMessageConsume>(); //��¼

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, LoginMessageConsume loginConsumer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Client1 v1"));

            #region EasyNetQ
            //EasyNetQ
            var ctsDeposit = new CancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                loginConsumer.Subscriber("AccountSystem.Exchange", "Login.Queue", TimeSpan.FromSeconds(1), ctsDeposit.Token);
            }, ctsDeposit.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);



            #endregion

            app.UseCors("CorsPolicy");
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
