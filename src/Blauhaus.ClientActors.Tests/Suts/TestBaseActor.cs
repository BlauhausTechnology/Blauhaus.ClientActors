using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Actors;
using Blauhaus.Domain.Abstractions.Actors;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public class TestBaseActor : BaseActor, IActor<string>
    {
        public  int Count;
        public List<int> Numbers = new();
        private readonly Random _random = new();
        public HashSet<int> UsedThreadIds = new();

        public List<long> StartTimeTicks = new();
         
        public Task InvokeAndBlockAsync(int callIndex)
        { 
            return InvokeLockedAsync(async () =>
            {
                await Execute(callIndex);
            });
        }
        public Task InvokeAsync(int callIndex)
        { 
            return InvokeAsync(async () =>
            {
                await Execute(callIndex);
            });
        }
         
        private async Task Execute(int callIndex)
        {
            await Task.Delay(1);
            StartTimeTicks.Add(DateTime.Now.Ticks);
            Thread.Sleep(_random.Next(0, 14));
            Numbers.Add(Count++);
            UsedThreadIds.Add(Thread.CurrentThread.ManagedThreadId);

            Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId}. Invoked: {Count}. Call index: {callIndex}");
        }

        public Task InitializeAsync(string id)
        {
            Id = id;
            return Task.CompletedTask;
        }

        public Task ReloadAsync()
        {
            return Task.CompletedTask;
        }

        public string Id { get; private set; } = null!;
    }
}