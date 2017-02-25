using Dynamic.Framework.Mvc;
using System.Collections.Generic;

namespace Dynamic.Framework.Generic
{
    public class JsonDataResult
    {
        public string Result { get; set; }

        public object Records { get; set; }

        public object Data { get; set; }

        public long TotalRecordCount { get; set; }

        public List<Error> ErrorMessages { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; }

        public JsonDataResult()
        {
            this.ErrorMessages = new List<Error>();
            this.StatusCode = 200;
        }
    }
}
