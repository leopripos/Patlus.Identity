using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.Update
{
    public class UpdateCommand : ICommandFeature<Pool>
    {
        private string _name = null!;
        private string _description = null!;

        public Guid? Id { get; set; }
        public bool HasName { get; private set; }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                this.HasName = true;
            }
        }

        public bool HasDescription { get; private set; }
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                this.HasDescription = true;
            }
        }

        public Guid? RequestorId { get; set; }
    }
}
