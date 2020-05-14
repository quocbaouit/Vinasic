using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
using VINASIC.Object;

namespace VINASIC.Business.Interface
{
    public interface IBllSticker
    {
        ResponseBase Create(ModelSticker obj);
        ResponseBase Update(ModelSticker obj);
        ResponseBase DeleteById(int id, int userId);
        PagedList<ModelSticker> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSticker> GetListProduct();
        List<T_Sticker> GetAllSticker();
    }
}
