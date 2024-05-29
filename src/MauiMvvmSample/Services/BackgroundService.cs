using System;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiMvvmSample.Services;

public sealed class BackgroundService : IDisposable
{
    private CancellationTokenSource? cancellationTokenSource;

    private Task? backgroundTask;

    public void Start(TimeSpan interval)
    {
        if (this.cancellationTokenSource is not null)
        {
            return;
        }

        this.cancellationTokenSource = new CancellationTokenSource();
        this.backgroundTask = Task.Run(async () =>
        {
            var current = TimeSpan.Zero;
            using var timer = new PeriodicTimer(interval);

            while (true)
            {
                var message = new ValueChangedMessage<TimeSpan>(current);

                WeakReferenceMessenger.Default.Send(message);
                await timer.WaitForNextTickAsync(this.cancellationTokenSource.Token).ConfigureAwait(false);
                current += interval;
            }
        }, this.cancellationTokenSource.Token);
    }

    public void Stop()
    {
        if (this.cancellationTokenSource is null)
        {
            return;
        }

        this.cancellationTokenSource.Cancel();
        this.cancellationTokenSource.Dispose();
        this.cancellationTokenSource = null;
    }

    public void Dispose()
    {
        if (this.cancellationTokenSource is not null)
        {
            this.cancellationTokenSource.Cancel();
            this.cancellationTokenSource.Dispose();
            this.cancellationTokenSource = null;
        }
    }
}
