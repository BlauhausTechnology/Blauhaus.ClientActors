using System;
using Blauhaus.ClientActors.Abstractions;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public interface ITestDtoLoader : IDtoLoader<TestDto, Guid>
    {
        
    }
}