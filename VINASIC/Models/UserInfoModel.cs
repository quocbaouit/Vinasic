using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VINASIC.Models
{
    public class UserInfoModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FisrtName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PassWord { get; set; }
        public string OldPassWord { get; set; }
        public int Status { get; set; }
        public string ImagePath { get; set; }
    }
}