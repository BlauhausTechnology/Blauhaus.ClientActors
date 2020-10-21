using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.ClientActors.Tests.VirtualActorTests._Base;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.VirtualActorTests
{
    [TestFixture]
    public class HandleAsyncTests : BaseVirtualActorTest<TestActor>
    {
        public class WithResponse : InvokeAsyncTests
        {
            [Test]
            public async Task SHOULD_invoke_Initializer_and_methods_in_given_sequence()
            {
                //Act
                var tasks = new List<Task>();
                for (var i = 0; i < 50; i++)
                {
                    tasks.Add(Sut.HandleVoidAsync(new MyVoidMessage(i), CancelToken));
                }
                await Task.WhenAll(tasks);

                //Assert
                var numbers = await Sut.InvokeAsync<List<int>>(x => x.GetNumbers);
                Assert.That(numbers.Count, Is.EqualTo(51));
                Assert.That(numbers.SequenceEqual(Enumerable.Range(0, 51)));
            }
             
        } 
          
        public class NoResponse: InvokeAsyncTests
        {
            [Test]
            public async Task SHOULD_invoke_Initializer_and_methods_in_given_sequence()
            {
    
                //Act
                var tasks = new List<Task>();
                for (var i = 0; i < 50; i++)
                {
                    tasks.Add(Sut.HandleAsync<MyTestResult, MyTestMessage>(new MyTestMessage(i), CancelToken));
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