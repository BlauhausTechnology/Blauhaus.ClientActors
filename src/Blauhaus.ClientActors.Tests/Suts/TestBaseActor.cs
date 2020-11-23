using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public class TestBaseActor : BaseActor, IActor
    {
        public  int Count;
        public List<int> Numbers = new List<int>();
        private readonly Random _random = new Random();
        public HashSet<int> UsedThreadIds = new HashSet<int>();

        public List<long> StartTimeTicks = new List<long>();
         
        public Task InvokeDoAsync(int callIndex)
        { 
            return DoAsync(async () =>
            {
                await Execute(callIndex);
            });
        }

        public Task InvokeDoAndBlockAsync(int callIndex)
        { 
            return DoAndBlockAsync(async () =>
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
            return Task.CompletedTask;
        }

        public Task ReloadAsync()
        {
            return Task.CompletedTask;
        }
    }
}