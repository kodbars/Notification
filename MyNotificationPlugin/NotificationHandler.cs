using System;
using System.Runtime.Remoting;
using Resto.Front.Api;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace MyNotificationPlugin
{
    public class NotificationHandler : IDisposable
    {
        private readonly IDisposable subscription;
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "http://localhost:5264/api/notifications";
        private List<Notification> notificationList;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public NotificationHandler()
        {
            PluginContext.Log.Info($"Конструктор");
            NotificationProcessing();
        }

        private async void NotificationProcessing() 
        {
            try
            {
                PluginContext.Log.Info($"NotificationProcessing metod start");

                while (!_cts.Token.IsCancellationRequested)
                {
                    await Task.Delay(5000);

                    var responseGet = await _httpClient.GetAsync(ApiUrl);

                    if (!responseGet.IsSuccessStatusCode)
                    {
                        PluginContext.Log.Info($"Ошибка HttpGet StatusCode: {responseGet.StatusCode}");
                        continue;
                    }
                    PluginContext.Log.Info($"HttpGet StatusCode:{responseGet.StatusCode}");

                    notificationList = await responseGet.Content.ReadFromJsonAsync<List<Notification>>();
                    if (notificationList == null || notificationList.Count == 0)
                    {
                        PluginContext.Log.Info($"Нет новых уведомлений");
                        await Task.Delay(5000);
                        continue;
                    }

                    foreach (var notification in notificationList)
                    {

                        PluginContext.Operations.AddNotificationMessage(notification.Message, "NotificationPlugin", TimeSpan.FromSeconds(15));
                        PluginContext.Log.Info($"Уведомление:{notification.Message}");
                        var responseDelete = await _httpClient.DeleteAsync(ApiUrl + $"/{notification.Id}");
                        if (!responseDelete.IsSuccessStatusCode)
                        {
                            PluginContext.Log.Info($"Ошибка HttpGet StatusCode: {responseDelete.StatusCode}");
                            continue;
                        }
                        PluginContext.Log.Info($"HttpDelete StatusCode:{responseDelete.StatusCode}");
                        await Task.Delay(5000);
                    }
                }
            }
            catch (Exception ex)
            {
                PluginContext.Log.Error($"Ошибка: {ex.Message}");
                await Task.Delay(30000);
            }
            
        }

        public void Dispose()
        {
            try
            {
                _cts.Cancel();
                subscription?.Dispose();
            }
            catch (RemotingException ex)
            {
                PluginContext.Log.Error($"Ошибка: {ex.Message}");
            }
        }
    }
}