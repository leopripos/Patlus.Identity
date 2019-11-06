namespace Patlus.IdentityManagement.Rest.Features.Pools
{
    public class UpdateForm
    {
        private string name;
        private string description;

        public bool HasName { get; private set; }
        public string Name
        {
            get { return name; }
            set
            {
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
