using System;
using System.Runtime.Remoting;
using Resto.Front.Api;
using Resto.Front.Api.Attributes.JetBrains;
using Resto.Front.Api.Data.Orders;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            // Правильная подписка на событие изменения заказа
            //subscription = PluginContext.Notifications.OrderChanged.Subscribe(x => OnOrderChanged(x.Entity));
            PluginContext.Log.Info($"Конструктор");
            NotificationProcessing();
        }

        private async void NotificationProcessing() 
        {
            PluginContext.Log.Info($"NotificationProcessing metod start");
            while (!_cts.Token.IsCancellationRequested) 
            {
                var responseGet = await _httpClient.GetAsync(ApiUrl);
                notificationList = await responseGet.Content.ReadFromJsonAsync<List<Notification>>();
                PluginContext.Log.Info($"HttpGet статус код:{responseGet.StatusCode.ToString()}");
                foreach (var notification in notificationList)
                {
                    await Task.Delay(5000);
                    PluginContext.Operations.AddNotificationMessage(notification.Message, "NotificationPlugin", TimeSpan.FromSeconds(5));
                    PluginContext.Log.Info($"Уведомление:{notification.Message}");
                    var responseDelete = await _httpClient.DeleteAsync(ApiUrl + $"/{notification.Id}");
                    PluginContext.Log.Info($"HttpGet статус код:{responseDelete.StatusCode.ToString()}");
                }
            }
        }

        public void Dispose()
        {
            try
            {
                _cts.Cancel();
                subscription?.Dispose();
            }
            catch (RemotingException)
            {
                // Обработка ошибок при отписке
            }
        }
    }
}