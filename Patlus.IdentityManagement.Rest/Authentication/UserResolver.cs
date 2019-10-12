using System;

namespace Patlus.IdentityManagement.Rest.Authentication
{
    public class UserResolver : IUserResolver
    {
        public IUserResolver.IUser Current { get; private set; }

        public void Initialize(Guid userId)
        {
            this.Current = new User(userId);
        }
    }

    public class User : IUserResolver.IUser
    {
        public Guid Id { get; private set; }

        public User(Guid id)
        {
            this.Id = id;
        }
    }
}
