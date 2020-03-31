using System;

namespace Patlus.Common.UseCase.Services
{
    public interface IIdentifierService
    {
        Guid NewGuid();
    }
}
