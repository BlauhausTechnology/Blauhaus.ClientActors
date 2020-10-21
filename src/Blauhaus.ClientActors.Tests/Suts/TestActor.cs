using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Domain.Abstractions.CommandHandlers;
using Blauhaus.Responses;

namespace Blauhaus.ClientActors.Tests.Suts
{ 

    public class TestActor : IClientActor,
        IVoidCommandHandler<MyVoidMessage>,
        ICommandHandler<MyTestResult, MyTestMessage>
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
            Thread.Sleep(1);
            _numbers.Add(_count++);
            Console.WriteLine("Invoked: " + _count);
            return Task.CompletedTask;
        }

        public Task<MyTestResult> Task_WithReturnValueNoMessage()
        {
            Thread.Sleep(1);
            _numbers.Add(_count++);
            Console.WriteLine("Invoked: " + _count);
            return Task.FromResult(new MyTestResult(_count));
        }

        public Task Task_NoReturnValueWithMessage(MyTestMessage message)
        {
            Thread.Sleep(1);
            _numbers.Add(_count++); 
            Console.WriteLine("Invoked: " + _count);
            return Task.CompletedTask;
        }

        public Task<MyTestResult> Task_WithReturnValueWithMessage(MyTestMessage message)
        {
            Thread.Sleep(1);
            _numbers.Add(_count++);
            Console.WriteLine("Invoked: " + _count);
            return Task.FromResult(new MyTestResult(_count));
        }
         
        public void Action_NoReturnValueNoMessage()
        {
            Thread.Sleep(1);
            _numbers.Add(_count++);
            Console.WriteLine("Invoked: " + _count);
        }

        public MyTestResult Action_WithReturnValueNoMessage()
        {
            Thread.Sleep(1);
            _numbers.Add(_count++);
            Console.WriteLine("Invoked: " + _count);
            return new MyTestResult(_count);
        }

        public void Action_NoReturnValueWithMessage(MyTestMessage message)
        {
            Thread.Sleep(1);
            _numbers.Add(_count++); 
            Console.WriteLine("Invoked: " + _count);
        }

        public MyTestResult Action_WithReturnValueWithMessage(MyTestMessage message)
        {
            Thread.Sleep(1);
            _numbers.Add(_count++);
            Console.WriteLine("Invoked: " + _count);
            return new MyTestResult(_count);
        }
        
        public async Task<Response> HandleAsync(MyVoidMessage command, CancellationToken token)
        {
            Thread.Sleep(1);
            _numbers.Add(_count++);
            Console.WriteLine("Invoked: " + _count);
            return Response.Success();
        }

        public async Task<Response<MyTestResult>> HandleAsync(MyTestMessage command, CancellationToken token)
        {
            Thread.Sleep(1);
            _numbers.Add(_count++);
            Console.WriteLine("Invoked: " + _count);
            return Response.Success(new MyTestResult(_count));
        }

        public Task<List<int>> GetNumbers()
        {
            return Task.FromResult(_numbers);
        }

      

    }
}