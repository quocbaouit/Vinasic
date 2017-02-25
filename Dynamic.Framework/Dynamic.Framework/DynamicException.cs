using System;

namespace Dynamic.Framework
{
    public class DynamicException : Exception
    {
        public DynamicException()
        {
        }

        public DynamicException(string message)
            : base(message)
        {
        }
    }
}
