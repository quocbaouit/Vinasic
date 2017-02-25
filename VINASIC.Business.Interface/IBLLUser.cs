using System.Collections.Generic;
using PagedList;
using VINASIC.Business.Interface.Model;
using VINASIC.Object;

namespace VINASIC.Business.Interface
{
    public interface IBLLUser
    {
        ResponseBase CreateUser(ModelUser user, int actionUserId);
        ResponseBase UpdateUser(ModelUser user, int actionUserId, bool IsOwner);
        ResponseBase UpdatePassword(int userId , int accountId, string password);
        ResponseBase UnLockTimeByAccountId(int actionUserId, int accountId);
        ResponseBase ChangeUserStateByAccountId(int actionUserId, int accountId);
        ResponseBase GetUserByUserNamePassword(string userName, string password, List<ModelCountLoginFail> listModelCountLoginFail, int timeLock, int loginCount, string systemCaptcha, string userCaptcha);
        ResponseBase ForgotPasswordRequest(string userName, string mailOrNote, int actionRequest);
        ResponseBase DeleteById(int accountId, int actionUserId);
        ResponseBase DeleteByListId(List<int> listId, int userId);
        List<ModelUser> GetListUserByUserId(int userId, int companyId, string sorting);
        PagedList<ModelUser> GetListUser(string keyWord, int startIndexRecord, int pageSize, string sorting, int userId);
        UserService GetUserService(int userId);
        ModelUser GetUserByCompanyId();
        ModelUser GetUserInfoByUserId(int userId);
        T_User GetUserInfoByUserIdAndCompanyId(int companyId ,int userId);
        ResponseBase UpdateUserInfo(ModelUser modelUser,string oldPassWord,int status);

        // service
        void DeleteUserbyEmail(string email, int companyId ,int acctionUserId);
        bool CheckExistsEmail(string email, int companyId);
        string AddUser(T_User user, int acctionUserId);
        List<T_User> GetListUserByCompanyId(int companyId);

        PagedList<T_User> GetListUserWithoutExistsInList(List<int> existsUserIds, string keyWord, int searchBy, int startIndexRecord, int pageSize, string sorting, int companyId);

        ResponseBase SaveUserGeneral(string firstName, string lastName, string email, string mobile, int userId);

        ResponseBase SaveUserName(string oldUser, string newUserName, string comfirmUserName, string password,
            int userId);

        ResponseBase SaveUserPassword(string oldPassword, string newPassword, string comfirmPassword, int userId);
    }
}
