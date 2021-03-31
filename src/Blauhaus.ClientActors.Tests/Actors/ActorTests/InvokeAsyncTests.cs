using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests.Suts;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.Actors.ActorTests
{
    public class InvokeAsyncTests
    {
        [Test] 
        public async Task SHOULD_invoke_DoAsync_methods_in_given_sequence()
        {
            //Arrange
            var sut = new TestBaseActor();

            //Act
            var tasks = new List<Task>();
            for (var i = 0; i < 50; i++)
            {
                var call = i;
                tasks.Add(Task.Run(async () => await sut.InvokeAsync(call)));
            }
            await Task.WhenAll(tasks);

            //Assert 
            Assert.That(sut.UsedThreadIds.Count, Is.EqualTo(1));
            Assert.That(sut.Numbers.Count, Is.EqualTo(50));
            Assert.That(sut.Numbers.SequenceEqual(Enumerable.Range(0, 50)));
            
            for (var i = 1; i < 50; i++)
            {
                var previousStart = sut.StartTimeTicks[i - 1];
                var thisStartTime = sut.StartTimeTicks[i];
                Assert.That(thisStartTime > previousStart);
            }
            await sut.DisposeAsync();
        }
         
        [Test] 
        public async Task SHOULD_invoke_DoAndBlockAsync_methods_in_given_sequence()
        {
            //Arrange
            var sut = new TestBaseActor();

            //Act
            var tasks = new List<Task>();
            for (var i = 0; i < 50; i++)
            {
                var callIndex = i;
                tasks.Add(Task.Run(async () => await sut.InvokeAndBlockAsync(callIndex)));
            }
            await Task.WhenAll(tasks);

            //Assert 
            Assert.That(sut.Numbers.Count, Is.EqualTo(50));
            Assert.That(sut.Numbers.SequenceEqual(Enumerable.Range(0, 50)));

            for (var i = 1; i < 50; i++)
            {
                var previousStart = sut.StartTimeTicks[i - 1];
                var thisStartTime = sut.StartTimeTicks[i];
                Assert.That(thisStartTime > previousStart);
            }

            await sut.DisposeAsync();
        }

        [Test] 
        public async Task SHOULD_combine_Do_and_Block_and_Do()
        {
            //Arrange
            var sut = new TestBaseActor();

            //Act
            var tasks = new List<Task>();
            for (var i = 0; i < 50; i++)
            {
                var i1 = i;
                tasks.Add(i % 2 == 0 
                    ? Task.Run(async () => await sut.InvokeAndBlockAsync(i1)) 
                    : Task.Run(async () => await sut.InvokeAsync(i1)));
            }
            await Task.WhenAll(tasks);

            //Assert 
            Assert.That(sut.Numbers.Count, Is.EqualTo(50));
            Assert.That(sut.Numbers.SequenceEqual(Enumerable.Range(0, 50)));

            for (var i = 1; i < 50; i++)
            {
                var previousStart = sut.StartTimeTicks[i - 1];
                var thisStartTime = sut.StartTimeTicks[i];
                Assert.That(thisStartTime > previousStart);
            }

            await sut.DisposeAsync();
        }
    }
}