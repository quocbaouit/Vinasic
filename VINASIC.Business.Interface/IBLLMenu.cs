using System.Collections.Generic;
using System.Linq;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Business.Interface
{
    public interface IBLLMenu
    {
        IQueryable<ModelMenu> GetListMenuByCheckPermissionType(string position, List<string> listPermissionUrl);
    }
}
