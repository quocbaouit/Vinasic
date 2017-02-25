namespace Dynamic.Framework
{
    public class DynamicEntityNotFound : DynamicException
    {
        public DynamicEntityNotFound()
            : base("Data was not found")
        {
        }

        public DynamicEntityNotFound(string entityName)
            : base(string.Format("The {0} was not found", (object)entityName))
        {
        }
    }
}
