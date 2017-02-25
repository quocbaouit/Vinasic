namespace Dynamic.Framework.Security
{
    public interface IMembershipService
    {
        IUserService GetUserService(int userId);

        IPermissionService[] GetPermissionService(string featureName);
    }
}
