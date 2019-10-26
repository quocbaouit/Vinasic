using PagedList;
using System.Collections.Generic;
using VINASIC.Business.Interface.Model;
namespace VINASIC.Business.Interface
{
    public interface IBllContent
    {
        ResponseBase Create(ModelContent obj);
        ResponseBase Update(ModelContent obj);
        ResponseBase DeleteById(int id, int userId);
        PagedList<ModelContent> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting);
        List<ModelSelectItem> GetListContent();
        ModelContent GetContentByType(int code); 
         ModelContent ReadEntries();
        ModelContent ReadEntries2();
    }
}
