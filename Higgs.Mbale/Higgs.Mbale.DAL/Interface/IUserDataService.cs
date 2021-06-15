using System.Collections.Generic;
using Higgs.Mbale.EF.Models;
using Higgs.Mbale.DTO;

namespace Higgs.Mbale.DAL.Interface
{
  public  interface IUserDataService
    {

        AspNetUser GetLoggedInUser(string userId);
        bool UserExists(string finder);
        AspNetUser SaveUser(AspNetUserDTO user, string userId);
        bool MarkAsDeleted(string Id);
        AspNetUser GetAspNetUser(string Id);
        IEnumerable<AspNetRole> GetAllRoles();
        AspNetRole GetAspNetRole(string roleId);
        IEnumerable<AspNetUser> GetAspNetUsers();
        void CreateAspNetUserRolesRecord(string userId, string roleId);
        void SaveUserBranch(UserBranchDTO userBranchDTO);
        UserBranch GetUserBranch(string branchManagerId);
        bool IsAssignedBranch(string userId);
        void PurgeUserBranch(string userId, long branchId);
        long GetLoggedUserBranchId(string userId);
        IEnumerable<Region> GetAllRegions();
        IEnumerable<AspNetUser> GetAllCustomersForAParticularRegion(long regionId);
        Region GetRegion(long regionId);
        IEnumerable<UserBranch> GetAllUserBranches(string Id);
        IEnumerable<Location> GetAllLocationsForAParticularRegion(long regionId);
        IEnumerable<AspNetUserProduct> GetAspNetUserProducts(string userId);
        void SaveAspNetUserProduct(AspNetUserProductDTO aspNetUserProductDTO);
    }
}
