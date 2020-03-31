using System;

namespace Patlus.Common.UseCase.Services
{
    public interface ITimeService
    {
        DateTimeOffset Now { get; }
        DateTime NowDateTime { get; }
    }
}
