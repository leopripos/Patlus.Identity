namespace Patlus.IdentityManagement.Rest.Features.Pools
{
    public class UpdateForm
    {
        private string? _name;
        private string? _description;

        public bool HasName { get; private set; }
        public string? Name
        {
            get { return _name; }
            set
            {
                _name = value;
                HasName = true;
            }
        }

        public bool HasDescription { get; private set; }
        public string? Description
        {
            get { return _description; }
            set
            {
                _description = value;
                HasDescription = true;
            }
        }
    }
}
