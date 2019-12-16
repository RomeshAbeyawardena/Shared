using DotNetInsights.Shared.Contracts;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetInsights.Shared.Services.HostedServices
{
    public sealed class NotificationsHostedService : IHostedService, IDisposable
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _backgroundTaskTimer = new Timer(async(state) => await Work(state)
                .ConfigureAwait(false), null, interval, Timeout.Infinite);
        }

        private async Task Work(object state)
        {
                if(!_notificationSubscriberQueue.IsEmpty 
                    && _notificationSubscriberQueue.TryDequeue(out var queueitem))
                {
                    await queueitem.Item1
                        .OnChangeAsync(queueitem.Item2)
                        .ConfigureAwait(false);
                    
                    _backgroundTaskTimer.Change(_notificationSubscriberQueue.Count > 0 
                            ? processingInterval 
                            : interval, Timeout.Infinite);
                    return;
                }
            
                _backgroundTaskTimer.Change(interval, Timeout.Infinite);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            while(!_notificationSubscriberQueue.IsEmpty 
                    && _notificationSubscriberQueue.TryDequeue(out var queueitem))
                await queueitem.Item1
                        .OnChangeAsync(queueitem.Item2)
                        .ConfigureAwait(false);
        }

        public void Dispose()
        {
            _backgroundTaskTimer.Dispose();
        }

        public NotificationsHostedService(ConcurrentQueue<Tuple<INotificationSubscriber, object>> notificationSubscriberQueue)
        {
            _notificationSubscriberQueue = notificationSubscriberQueue;
            
        }

        private Timer _backgroundTaskTimer;
        private int interval = 60000;
        private int processingInterval = 60;
        private readonly ConcurrentQueue<Tuple<INotificationSubscriber, object>> _notificationSubscriberQueue;
    }
}