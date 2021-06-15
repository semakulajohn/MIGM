using System.Collections.Generic;
using Higgs.Mbale.Models;
using Higgs.Mbale.Models.WebViewModel;


namespace Higgs.Mbale.BAL.Interface
{
  public  interface IUserService
    {
        AspNetUser GetLoggedInUser(string userId);
        bool UserExists(string finder);
        AspNetUser SaveUser(AspNetUser user, string userId);
        string GetUserFullName(EF.Models.AspNetUser aspNetUser);
        
        bool MarkAsDeleted(string Id);
        AspNetUser GetAspNetUser(string Id);
        IEnumerable<AspNetRole> GetAllRoles();
        IEnumerable<AspNetUser> GetAllAspNetUsers();
        AspNetRole GetAspNetRole(string roleId);
        IEnumerable<AspNetUser> GetAllBranchManagers();
        IEnumerable<AspNetUser> GetAllAdmins();
        IEnumerable<AspNetUserViewModel> GetAllSuppliers();
        IEnumerable<AspNetUserViewModel> GetAllCustomers();

        IEnumerable<AspNetUserViewModel> GetAllOutSourcers();
        IEnumerable<AspNetUserViewModel> GetAllMechanics();
        void SaveUserBranch(string userId, long branchId);
        UserBranch GetBranchManager(string branchManagerId);
        bool IsAssignedBranch(string userId);
        IEnumerable<Region> GetAllRegions();
        long GetLoggedUserBranchId(string userId);
        void PurgeUserBranch(string userId, long branchId);
        IEnumerable<AspNetUserViewModel> GetAllCustomersForAParticularRegion(long regionId);
        Region GetRegion(long regionId);
        IEnumerable<AspNetUser> GetAllBranchManagersSuperAdminsSuperUsersAndAdmins();
        IEnumerable<Location> GetAllLocationsForAParticularRegion(long regionId);

        IEnumerable<AspNetUserProduct> GetAspNetUserProducts(string userId);
        void SaveAspNetUserProduct(string userId, long productId);

    }
}
