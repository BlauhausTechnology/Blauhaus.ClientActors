using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Containers;
using Blauhaus.ClientActors.Tests.Base;
using Blauhaus.ClientActors.Tests.Mocks;
using Blauhaus.ClientActors.Tests.Suts;
using Moq;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.Containers.ModelActorContainerTests
{
    public class SubscribeToActiveModelsAsyncTests : BaseActorTest<ModelActorContainer<ITestModelActor, Guid, ITestModel>>
    {
        private Guid _id;

        protected TestModelActorMockBuilder MockTestActor => AddMock<TestModelActorMockBuilder, ITestModelActor>().Invoke();

        public override void Setup()
        {
            base.Setup();

            _id = Guid.NewGuid();

            AddService(MockTestActor.Object);
        }

        [Test]
        public async Task WHEN_Actor_does_exist_SHOULD_subscribe_to_it()
        {
            //Arrange
            Func<ITestModel, Task> handler = model => Task.CompletedTask;
            await Sut.GetOneAsync(_id);

            //Act
            await Sut.SubscribeToActiveModelsAsync(handler);

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync(_id), Times.Once);
            MockTestActor.Mock.Verify(x => x.SubscribeAsync(handler, null), Times.Exactly(1));
        }

        [Test]
        public async Task SHOULD_publish_models_from_active_actors()
        {
            //Arrange
            await Sut.GetOneAsync(_id);
            ITestModel publishedModel = null;
            Func<ITestModel, Task> handler = model =>
            {
                publishedModel = model;
                return Task.CompletedTask;
            }; 

            //Act
            await Sut.SubscribeToActiveModelsAsync(handler);
            var newModelId = Guid.NewGuid();
            await MockTestActor.PublishMockSubscriptionAsync((ITestModel)new TestModel(newModelId));

            //Assert
            Assert.That(publishedModel, Is.Not.Null);
            Assert.That(publishedModel.Id, Is.EqualTo(newModelId));
        }

        [Test]
        public async Task WHEN_token_is_disposed_SHOULD_dispose_Subscriptions()
        {
            //Arrange
            Func<ITestModel, Task> handler = model => Task.CompletedTask;
            await Sut.GetOneAsync(_id);

            //Act
            var token = await Sut.SubscribeToActiveModelsAsync(handler);
            token.Dispose();

            //Assert
            MockTestActor.MockToken.Verify(x => x.Dispose());
        }

        [Test]
        public async Task WHEN_new_actor_is_cached_SHOULD_subscribe_to_it()
        {
            //Arrange
            Func<ITestModel, Task> handler = model => Task.CompletedTask;

            //Act
            await Sut.SubscribeToActiveModelsAsync(handler);
            await Sut.GetOneAsync(_id);

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync(_id), Times.Exactly(1));
            MockTestActor.Mock.Verify(x => x.SubscribeAsync(handler, null), Times.Exactly(1));
        }

        [Test]
        public async Task WHEN_new_actor_is_cached_SHOULD_publish_existing_and_updated_models()
        {
            //Arrange
            var modelOne = new TestModel(Guid.NewGuid());
            var modelTwo = new TestModel(Guid.NewGuid());
            MockTestActor.Where_GetModelAsync_returns(modelOne);
            List<ITestModel> publishedModels = new List<ITestModel>();
            Func<ITestModel, Task> handler = model =>
            {
                publishedModels.Add(model);
                return Task.CompletedTask;
            }; 

            //Act
            await Sut.SubscribeToActiveModelsAsync(handler);
            await Sut.GetOneAsync(_id);
            await MockTestActor.PublishMockSubscriptionAsync(modelTwo);

            //Assert
            Assert.That(publishedModels.Count, Is.EqualTo(2));
            Assert.That(publishedModels[0], Is.EqualTo(modelOne));
            Assert.That(publishedModels[1], Is.EqualTo(modelTwo));
        }

        [Test]
        public async Task WHEN_new_actor_is_cached_and_token_is_disposed_SHOULD_unsubscribe_from_it()
        {
            //Arrange
            Func<ITestModel, Task> handler = model => Task.CompletedTask;

            //Act
            var token = await Sut.SubscribeToActiveModelsAsync(handler);
            await Sut.GetOneAsync(_id);
            token.Dispose();
            ;
            //Assert
            MockTestActor.MockToken.Verify(x => x.Dispose());
        }
    }
}