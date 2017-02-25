using System.ComponentModel;

namespace Dynamic.Framework.Generic
{
    public enum AllowAccess
    {
        Administrator,
        Company,
    }
    public enum CommandPermissionType
    {
        Or,
        And,
    }
    public enum eStatusCode
    {
        OK = 200,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        InternalServerError = 500,
    }
    public enum eUploadType
    {
        Image = 1,
        File = 2,
        Document = 3,
    }
    public enum PermissionType
    {
        Add = 1,
        Update = 2,
        Delete = 3,
        View = 4,
        Approve = 5,
        Other = 6,
        Restore = 7,
    }
    public enum EnumLogicals
    {
        [Description("And")]
        And = 0,
        [Description("Or")]
        Or = 1
    }

    public enum EnumOperators
    {
        [Description(" = ")]
        Is = 0,
        [Description(" != ")]
        IsNot = 1,
        [Description(" IS NULL ")]
        IsNull = 2,
        [Description(" IS NOT NULL ")]
        IsNotNull = 3
    }

}
