﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests.Suts;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.ActorTests
{
    public class InvokeDoAndBlockAsyncTests
    {
        [Test] 
        public async Task HOULD_invoke_methods_in_given_sequence()
        {
            //Arrange
            var sut = new TestBaseActor();

            //Act
            var tasks = new List<Task>();
            for (var i = 0; i < 50; i++)
            {
                tasks.Add(Task.Run(async () => await sut.InvokeDoAndBlockAsync()));
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