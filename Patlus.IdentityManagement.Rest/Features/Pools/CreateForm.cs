using System;

namespace Patlus.IdentityManagement.Rest.Features.Pools
{
    public class CreateForm
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
