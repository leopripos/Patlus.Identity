using System;

namespace Patlus.Common.Presentation
{
    public interface IUserResolver
    {
        IUser Current { get; }
        void Initialize(IUser user);
    }

    public interface IUser
    {
        Guid Id { get; }
        Guid PoolId { get; }
    }
}
