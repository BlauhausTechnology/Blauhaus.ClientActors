using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.ClientActors.Tests.VirtualActorTests._Base;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.VirtualActorTests
{
    [TestFixture]
    public class InvokeAsyncTests : BaseVirtualActorTest<TestActor>
    {
        public class Tasks : InvokeAsyncTests
        {
            [Test]
            public async Task NoReturnValueNoMessage_SHOULD_invoke_Initializer_and_methods_in_given_sequence()
            {
                //Act
                var tasks = new List<Task>();
                for (var i = 0; i < 50; i++)
                {
                    tasks.Add(Sut.InvokeAsync(y => y.Task_NoReturnValueNoMessage));
                }
                await Task.WhenAll(tasks);

                //Assert
                var numbers = await Sut.InvokeAsync<List<int>>(x => x.GetNumbers);
                Assert.That(numbers.Count, Is.EqualTo(51));
                Assert.That(numbers.SequenceEqual(Enumerable.Range(0, 51)));
            }

            [Test]
            public async Task WithReturnValueNoMessage_SHOULD_invoke_Initializer_and_methods_in_given_sequence()
            {
                //Act
                var tasks = new List<Task<MyTestResult>>();
                for (var i = 0; i < 50; i++)
                {
                    tasks.Add(Sut.InvokeAsync<MyTestResult>(y => y.Task_WithReturnValueNoMessage));
                }
                await Task.WhenAll(tasks);

                //Assert
                var numbers = await Sut.InvokeAsync<List<int>>(x => x.GetNumbers);
                Assert.That(numbers.Count, Is.EqualTo(51));
                Assert.That(numbers.SequenceEqual(Enumerable.Range(0, 51)));
            }

            [Test]
            public async Task NoReturnValueWithMessage_SHOULD_invoke_InitializeAsync_and_actor_method_in_given_sequence()
            {
                //Act
                var tasks = new List<Task>();
                for (var i = 0; i < 50; i++)
                {
                    tasks.Add(Sut.InvokeAsync(y => y.Task_NoReturnValueWithMessage, new MyTestMessage(i)));
                }
                await Task.WhenAll(tasks);

                //Assert
                var numbers = await Sut.InvokeAsync<List<int>>(x => x.GetNumbers);
                Assert.That(numbers.Count, Is.EqualTo(51));
                Assert.That(numbers.SequenceEqual(Enumerable.Range(0, 51)));
            }

            [Test]
            public async Task WithReturnValueWithMessage_SHOULD_invoke_Initializer_and_methods_in_given_sequence()
            {
                //Act 
                var tasks = new List<Task<MyTestResult>>();
                for (var i = 0; i < 50; i++)
                {
                    tasks.Add(Sut.InvokeAsync<MyTestResult, MyTestMessage>(y => y.Task_WithReturnValueWithMessage, new MyTestMessage(i)));
                }
                await Task.WhenAll(tasks);

                //Assert
                var numbers = await Sut.InvokeAsync<List<int>>(x => x.GetNumbers);
                Assert.That(numbers.Count, Is.EqualTo(51));
                Assert.That(numbers.SequenceEqual(Enumerable.Range(0, 51))); 
            }
        } 
          
        public class Actions: InvokeAsyncTests
        {
            [Test]
            public async Task NoReturnValueNoMessage_SHOULD_invoke_Initializer_and_methods_in_given_sequence()
            {
    
                //Act
                var tasks = new List<Task>();
                for (var i = 0; i < 50; i++)
                {
                    tasks.Add(Sut.InvokeAsync(x => x.Action_NoReturnValueNoMessage));
                }
                await Task.WhenAll(tasks);

                //Assert
                var numbers = await Sut.InvokeAsync<List<int>>(x => x.GetNumbers);
                Assert.That(numbers.Count, Is.EqualTo(51));
                Assert.That(numbers.SequenceEqual(Enumerable.Range(0, 51)));
            }

            [Test]
            public async Task WithReturnValueNoMessage_SHOULD_invoke_Initializer_and_methods_in_given_sequence()
            {
                //Act
                var tasks = new List<Task<MyTestResult>>();
                for (var i = 0; i < 50; i++)
                {
                    tasks.Add(Sut.InvokeAsync<MyTestResult>(y => y.Action_WithReturnValueNoMessage));
                }
                await Task.WhenAll(tasks);

                //Assert
                var numbers = await Sut.InvokeAsync<List<int>>(x => x.GetNumbers);
                Assert.That(numbers.Count, Is.EqualTo(51));
                Assert.That(numbers.SequenceEqual(Enumerable.Range(0, 51)));
            }

            [Test]
            public async Task NoReturnValueWithMessage_SHOULD_invoke_InitializeAsync_and_actor_method_in_given_sequence()
            {
                //Act
                var tasks = new List<Task>();
                for (var i = 0; i < 50; i++)
                {
                    tasks.Add(Sut.InvokeAsync(y => y.Action_NoReturnValueWithMessage, new MyTestMessage(i)));
                }
                await Task.WhenAll(tasks);

                //Assert
                var numbers = await Sut.InvokeAsync<List<int>>(x => x.GetNumbers);
                Assert.That(numbers.Count, Is.EqualTo(51));
                Assert.That(numbers.SequenceEqual(Enumerable.Range(0, 51)));
            }

            [Test]
            public async Task WithReturnValueWithMessage_SHOULD_invoke_Initializer_and_methods_in_given_sequence()
            {
                //Act 
                var tasks = new List<Task<MyTestResult>>();
                for (var i = 0; i < 50; i++)
                {
                    tasks.Add(Sut.InvokeAsync<MyTestResult, MyTestMessage>(y => y.Action_WithReturnValueWithMessage, new MyTestMessage(i)));
                }
                await Task.WhenAll(tasks);

                //Assert
                var numbers = await Sut.InvokeAsync<List<int>>(x => x.GetNumbers);
                Assert.That(numbers.Count, Is.EqualTo(51));
                Assert.That(numbers.SequenceEqual(Enumerable.Range(0, 51))); 
            }
        } 
          
         
    }
}