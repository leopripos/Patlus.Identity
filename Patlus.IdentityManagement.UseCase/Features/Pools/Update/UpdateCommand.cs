using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.Update
{
    public class UpdateCommand : ICommandFeature<Pool>
    {
        private string name;
        private string description;

        public Guid? Id { get; set; }
        public Guid? RequestorId { get; set; }

        public bool HasName { get; private set; }
        public string Name {
            get { return name; }
            set {
                name = Name;
                HasName = true;
            }
        }

        public bool HasDescription { get; private set; }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                HasDescription = true;
            }
        }
    }
}
