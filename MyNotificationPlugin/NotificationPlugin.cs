using System;
using System.Collections.Generic;
using Resto.Front.Api;
using Resto.Front.Api.Attributes;

namespace MyNotificationPlugin
{
    [PluginLicenseModuleId(21016318)] // Замените на ваш ID лицензии
    public sealed class NotificationPlugin : IFrontPlugin
    {
        private readonly Stack<IDisposable> subscriptions = new Stack<IDisposable>();

        public NotificationPlugin()
        {
            PluginContext.Log.Info("Initializing NotificationPlugin");

            // Здесь будем добавлять подписки на уведомления
            subscriptions.Push(new NotificationHandler());

            PluginContext.Log.Info("NotificationPlugin started");
        }

        public void Dispose()
        {
            while (subscriptions.Count > 0)
            {
                var subscription = subscriptions.Pop();
                try
                {
                    subscription.Dispose();
                }
                catch (Exception ex)
                {
                    PluginContext.Log.Error($"Ошибка: {ex.Message}");
                }
            }

            PluginContext.Log.Info("NotificationPlugin stopped");
        }
    }
}