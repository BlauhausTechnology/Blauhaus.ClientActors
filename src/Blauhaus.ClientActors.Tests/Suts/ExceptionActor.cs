using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;

namespace Blauhaus.ClientActors.Tests.Suts
{ 

    public class ExceptionActor : IClientActor
    {

        private readonly List<int> _numbers = new List<int>();
        private int _count;
        private int _shutDownCount;
     
        public Task InitializeAsync(string id)
        {
            Thread.Sleep(1);
            _numbers.Add(_count++);
            Console.WriteLine("Initializes: " + _count);
            return Task.CompletedTask;
        }

        public Task ShutdownAsync()
        {
            _shutDownCount++;
            return Task.CompletedTask;
        }
         
        public Task Task_NoReturnValueNoMessage()
        {
            throw new Exception("Task_NoReturnValueNoMessage");
        }

        public Task<MyTestResult> Task_WithReturnValueNoMessage()
        {
            throw new Exception("Task_WithReturnValueNoMessage");
        }

        public Task Task_NoReturnValueWithMessage(MyTestMessage message)
        {
            throw new Exception("Task_NoReturnValueWithMessage");
        }

        public Task<MyTestResult> Task_WithReturnValueWithMessage(MyTestMessage message)
        {
            throw new Exception("Task_WithReturnValueWithMessage");
        }
         
        public void Action_NoReturnValueNoMessage()
        {
            throw new Exception("Action_NoReturnValueNoMessage");
        }

        public MyTestResult Action_WithReturnValueNoMessage()
        {
            throw new Exception("Action_WithReturnValueNoMessage");
        }

        public void Action_NoReturnValueWithMessage(MyTestMessage message)
        {
            throw new Exception("Action_NoReturnValueWithMessage");
        }

        public MyTestResult Action_WithReturnValueWithMessage(MyTestMessage message)
        {
            throw new Exception("Action_WithReturnValueWithMessage");
        }

        public Task<List<int>> GetNumbers()
        {
            return Task.FromResult(_numbers);
        }

        public int GetShutdownCount()
        {
            return _shutDownCount;
        }
         
    }
}