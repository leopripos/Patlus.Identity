using System;

namespace Patlus.IdentityManagement.Rest.Authentication
{
    public interface IUserResolver
    {
        IUser Current { get; }
        void Initialize(Guid userId);

        public interface IUser
        {
            public Guid Id { get; }
        }
    }
}
