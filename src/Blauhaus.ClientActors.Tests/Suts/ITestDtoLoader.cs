using System;
using Blauhaus.Domain.Abstractions.DtoCaches;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public interface ITestDtoLoader : IDtoLoader<TestDto, Guid>
    {
        
    }
}