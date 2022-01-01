using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientFactoryDemo
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IHttpClientFactory httpClientFactory = GetHttpClientFactory();
            var baseAddress = new Uri("http://localhost:5030/");
            var requestUrl = "WeatherForecast";

            using (var client = httpClientFactory.CreateClient("SampleHttpClient"))
            {
                // 設定Time Out
                client.Timeout = TimeSpan.FromSeconds(5);
                client.BaseAddress = baseAddress;

                try
                {
                    var response = await client.GetAsync(requestUrl);
                    var contentStr = await response.Content.ReadAsStringAsync();

                    Console.WriteLine(contentStr);
                }
                catch (TaskCanceledException ex)
                {
                    Console.WriteLine("TaskCanceledException");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Call Second Api");

                    var postContent = new StringContent("");

                    var response2 = await client.PostAsync($"{requestUrl}?name=jyu", postContent);

                    var contentStr2 = await response2.Content.ReadAsStringAsync();

                    Console.WriteLine(contentStr2);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception");
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine("Done!!");
            Console.ReadLine();
        }

        /// <summary>
        /// 取得IHttpClientFactory 注入Service
        /// </summary>
        /// <returns></returns>
        public static IHttpClientFactory GetHttpClientFactory()
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddHttpClient();

            var serviceBuilder = serviceCollection.BuildServiceProvider();

            var httpClientFactory = serviceBuilder.GetService<IHttpClientFactory>();

            return httpClientFactory;
        }
    }
}
