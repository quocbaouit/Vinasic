using System.Collections.Generic;
using VINASIC.Business.Interface.Model;

namespace VINASIC.Business.Interface
{
    public interface IBLLMenuCategory
    {
        List<ModelMenuCategory> GetMenusAndMenuCategoriesByUserId(int userId, string position);
    }
}
