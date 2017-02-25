using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamic.Framework.Mvc;

namespace VINASIC.Business.Interface.Model
{
    public class ResponseBase
    {
        public bool IsSuccess { get; set; }
        public List<Error> Errors { get; set; }
        public dynamic Data { get; set; }
        public ResponseBase()
        {
            Errors = new List<Error>();
        }
    }
}
