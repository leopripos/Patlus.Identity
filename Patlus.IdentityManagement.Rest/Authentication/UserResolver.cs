using System;

namespace Patlus.IdentityManagement.Rest.Authentication
{
    public class UserResolver : IUserResolver
    {
        private IUser? _user;

        public IUser Current
        {
            get
            {
                return _user ?? throw new InvalidOperationException("Uninitialized property: " + nameof(_user));
            }
        }

        public void Initialize(IUser user)
        {
            _user = user;
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
