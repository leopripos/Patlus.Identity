using System;

namespace Patlus.IdentityManagement.Rest.Authentication
{
    public interface IUserResolver
    {
        IUser Current { get; }
        void Initialize(IUser user);
    }

    public interface IUser
    {
        Guid Id { get; }
    }
}
