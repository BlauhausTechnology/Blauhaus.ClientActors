using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.ClientActors.Tests.VirtualActorTests._Base;
using Blauhaus.ClientActors.VirtualActors;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.VirtualActorTests
{
    [TestFixture]
    public class InvokeAsyncTests : BaseVirtualActorTest<TestVirtualActor>
    {
        public class Tasks : InvokeAsyncTests
        {
            [Test]
            public async Task NoReturnValueNoMessage_SHOULD_invoke_Initializer_and_methods_in_given_sequence()
            {
                //Act
                var sut = Sut; //must be constructed before the task loop
                var tasks = new List<Task>();
                for (var i = 0; i < 50; i++)
                {
                    tasks.Add(Task.Run(async () => await Sut.InvokeAsync(y => y.Task_NoReturnValueNoMessage)));
                }
                await Task.WhenAll(tasks);

                //Assert
                var numbers = await Sut.InvokeAsync<List<int>>(x => x.GetNumbers);
                Assert.That(numbers.Count, Is.EqualTo(51));
                Assert.That(numbers.SequenceEqual(Enumerable.Range(0, 51)));
            }

            //[Test]
            //public async Task WITH_Multiple_actors_NoReturnValueNoMessage_SHOULD_invoke_Initializer_and_methods_in_given_sequence()
            //{
            //    //Act
            //    var sut0 = Sut; // to get the ServiceLocator
            //    var sut1 = new VirtualActor<TestActor>(ServiceLocator, "1");
            //    var sut2 = new VirtualActor<TestActor>(ServiceLocator, "2");
            //    var tasks = new List<Task>();
            //    for (var i = 0; i < 50; i++)
            //    {
            //        if (i % 2f == 0)
            //        {
            //            tasks.Add(sut1.InvokeAsync(y => y.Task_NoReturnValueNoMessage));
            //        }
            //        else
            //        {
            //            tasks.Add(sut2.InvokeAsync(y => y.Task_NoReturnValueNoMessage));
            //        }
            //    }
            //    await Task.WhenAll(tasks);

            //    //Assert
            //    var numbers1 = await sut1.InvokeAsync<List<int>>(x => x.GetNumbers);
            //    var numbers2 = await sut2.InvokeAsync<List<int>>(x => x.GetNumbers);
            //    //Assert.That(numbers.Count, Is.EqualTo(51));
            //    //Assert.That(numbers.SequenceEqual(Enumerable.Range(0, 51)));
            //}

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