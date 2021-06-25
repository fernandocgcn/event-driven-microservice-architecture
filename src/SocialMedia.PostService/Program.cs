using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SocialMedia.PostService.Data;
using SocialMedia.PostService.Entities;
using System;
using System.Linq;
using System.Text;

namespace SocialMedia.PostService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ListenForIntegrationEvents();
            CreateHostBuilder(args).Build().Run();
        }

        private static void ListenForIntegrationEvents()
        {
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var contextOptions = new DbContextOptionsBuilder<PostServiceContext>()
                    .UseSqlite(@"Data Source=post.db")
                    .Options;
                var dbContext = new PostServiceContext(contextOptions);

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received {message}");

                var data = JObject.Parse(message);
                var type = ea.RoutingKey;
                if (type == "social-media.user.add")
                {
                    dbContext.User.Add(new User()
                    {
                        ID = data["id"].Value<int>(),
                        Name = data["name"].Value<string>()
                    });
                    dbContext.SaveChanges();
                }
                else if (type == "social-media.user.update")
                {
                    var user = dbContext.User.First(a => a.ID == data["id"].Value<int>());
                    user.Name = data["newname"].Value<string>();
                    dbContext.SaveChanges();
                }
            };
            channel.BasicConsume(queue: "social-media.user.postservice",
                                     autoAck: true,
                                     consumer: consumer);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
