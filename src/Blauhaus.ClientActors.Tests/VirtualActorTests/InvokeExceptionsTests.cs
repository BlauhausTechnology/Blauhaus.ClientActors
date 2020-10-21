using System;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.ClientActors.Tests.VirtualActorTests._Base;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.VirtualActorTests
{
    [TestFixture]
    public class InvokeExceptionsTests : BaseVirtualActorTest<ExceptionActor>
    {
        public class Tasks : InvokeExceptionsTests
        {
            [Test]
            public void NoReturnValueNoMessage_SHOULD_invoke_Initializer_and_methods_in_given_sequence()
            {
                //Act
                 Assert.ThrowsAsync<Exception>(async () => await Sut.InvokeAsync(x => x.Task_NoReturnValueNoMessage));
            }

            [Test]
            public void WithReturnValueNoMessage_SHOULD_invoke_Initializer_and_methods_in_given_sequence()
            {
                //Act
                Assert.ThrowsAsync<Exception>(async () => await Sut.InvokeAsync<MyTestResult>(y => y.Task_WithReturnValueNoMessage)); 
            }

            [Test]
            public void NoReturnValueWithMessage_SHOULD_invoke_InitializeAsync_and_actor_method_in_given_sequence()
            {
                //Act 
                Assert.ThrowsAsync<Exception>(async () => await Sut.InvokeAsync(y => y.Task_NoReturnValueWithMessage, new MyTestMessage(1))); 
            }

            [Test]
            public void WithReturnValueWithMessage_SHOULD_invoke_Initializer_and_methods_in_given_sequence()
            {
                //Act  
                Assert.ThrowsAsync<Exception>(async () => await Sut.InvokeAsync<MyTestResult, MyTestMessage>(y => y.Task_WithReturnValueWithMessage, new MyTestMessage(1))); 
            }
        } 
          
        public class Actions: InvokeExceptionsTests
        {
            [Test]
            public void NoReturnValueNoMessage_SHOULD_invoke_Initializer_and_methods_in_given_sequence()
            {
                //Act 
                Assert.ThrowsAsync<Exception>(async () => await Sut.InvokeAsync(x => x.Action_NoReturnValueNoMessage)); 
            }

            [Test]
            public void WithReturnValueNoMessage_SHOULD_invoke_Initializer_and_methods_in_given_sequence()
            {
                //Act
                Assert.ThrowsAsync<Exception>(async () => await Sut.InvokeAsync<MyTestResult>(y => y.Action_WithReturnValueNoMessage)); 
                 
            }

            [Test]
            public void NoReturnValueWithMessage_SHOULD_invoke_InitializeAsync_and_actor_method_in_given_sequence()
            {
                //Act 
                Assert.ThrowsAsync<Exception>(async () => await Sut.InvokeAsync(y => y.Action_NoReturnValueWithMessage, new MyTestMessage(2))); 
            }

            [Test]
            public void WithReturnValueWithMessage_SHOULD_invoke_Initializer_and_methods_in_given_sequence()
            {
                //Act  
                Assert.ThrowsAsync<Exception>(async () => await Sut.InvokeAsync<MyTestResult, MyTestMessage>(y => y.Action_WithReturnValueWithMessage, new MyTestMessage(2)));  
            }
        } 
          
         
    }
}