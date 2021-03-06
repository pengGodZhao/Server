﻿using AlarmAndPlan.Services;
using AllInOneContext;
using DasMulli.Win32.ServiceUtils;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using PAPS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskServer;

namespace MainServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            if (args.Length > 0 && args[0].Equals("Console"))
            {
                //数据迁移
               // MyMigration.Migrate();

                //启动报警上传检测
                ForwardAlarmLogTask.Instance.Run();

                //启动预案任务
                PlanTaskScheduler.Instance.Start();

                //启动查勤包生成线程
                DutyCheckPackageRunner.Instance.Start();

                Console.WriteLine("start Web host ..");
                var host = new WebHostBuilder().UseKestrel()
                    .UseUrls("http://*:5001")
                    .UseStartup<Startup>()
                    .Build();
                                host.Start();
                Console.WriteLine("Web host is runing..");
                Console.ReadLine();
            }
            else
            {
                var allInOneService = new AllInOneService();
                var serviceHost = new Win32ServiceHost(allInOneService);
                serviceHost.Run();
            }
        }


    }
}
