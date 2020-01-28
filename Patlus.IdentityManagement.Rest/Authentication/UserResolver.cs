using System;

namespace Patlus.IdentityManagement.Rest.Authentication
{
    public class UserResolver : IUserResolver
    {
        private IUser? user;

        public IUser Current
        {
            get
            {
                return user ?? throw new InvalidOperationException("Uninitialized property: " + nameof(user));
            }
        }

        public void Initialize(IUser user)
        {
            this.user = user;
        }
    }

    public class User : IUser
    {
        public Guid Id { get; private set; }

        public User(Guid id)
        {
            this.Id = id;
        }
    }
}
