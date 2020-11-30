using System;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Common.Utils.Contracts;
using Blauhaus.TestHelpers;

namespace Blauhaus.ClientActors.TestHelpers.Extensions
{
    public static class MockContainerExtensions
    {
        public static Func<ActorContainerMockBuilder<TActor, TId>> AddMockActorContainer<TActor, TId>(this MockContainer mocks) where TActor : class, IActor<TId>
        {
            return mocks.AddMock<ActorContainerMockBuilder<TActor, TId>, IActorContainer<TActor, TId>>();
        }

        public static Func<ModelActorContainerMockBuilder<TActor, TId, TModel>> AddMockModelActorContainer<TActor, TId, TModel>(this MockContainer mocks) where TActor : class, IModelActor<TId, TModel> where TModel : IHasId<TId>
        {
            return mocks.AddMock<ModelActorContainerMockBuilder<TActor, TId, TModel>, IModelActorContainer<TActor, TId, TModel>>();
        }
    }
}