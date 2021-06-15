
using System.Collections.Generic;
using System.Linq;
using Higgs.Mbale.DTO;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using Higgs.Mbale.Models.WebViewModel;
using log4net;

namespace Higgs.Mbale.BAL.Concrete
{
   public class UserService : IUserService
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(UserService));
        private IUserDataService _dataService;
       // private IProductService _productService;

        public UserService(IUserDataService dataService)
        {
            this._dataService = dataService;
           // this._productService = productService;
        }


        public AspNetUser GetLoggedInUser(string userId)
        {
            var result = this._dataService.GetLoggedInUser(userId);
            return MapEFToModel(result);
        }

        /// <summary>
        /// Converts an AspNetUser Models object to an AspNetUser DTO Object and passes the AspNetUser DTO
        /// along with the userId to the SaveUser Method in DAL for saving.
        /// </summary>
        /// <param name="user">AspNetUser Models object.</param>
        /// <param name="userId">UserId of the user saving the AspNetUser.</param>
        /// <returns>AspNetUser Models object.</returns>
        public AspNetUser SaveUser(AspNetUser user, string userId)
        {
            var aspNetUserDTO = new DTO.AspNetUserDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Mobile = user.Mobile,
                GenderId = user.GenderId,
                DateOfBirth = user.DateOfBirth,
                PasswordHash = user.PasswordHash,
                UserName = user.UserName,
                UniqueNumber = user.UniqueNumber,

            };
            var result = this._dataService.SaveUser(aspNetUserDTO, userId);

            return MapEFToModel(result);
        }


        /// <summary>
        /// checks whether the user with the specified email or userId exists,
        /// returns true if user exists or false.
        /// </summary>
        /// <param name="finder">EmailAddress or UserId of the user to check.</param>
        /// <returns>True or False</returns>
        public bool UserExists(string finder)
        {
            return this._dataService.UserExists(finder);
        }

        public void PurgeUserBranch(string userId, long branchId)
        {
            _dataService.PurgeUserBranch(userId, branchId);
        }

        /// <summary>
        /// Gets the full name of AspNetUser by concatenating its FirstName and lastName
        /// and returns the full name.
        /// </summary>
        /// <param name="aspNetUser">AspNetUser EF Models passed to obtain full Name from. </param>
        /// <returns>AspNetUser fullname which is a string resulting from the concatenation of first name and last name of the aspnet user.</returns>
        public string GetUserFullName(EF.Models.AspNetUser aspNetUser)
        {
            string fullName = string.Empty;
            if (aspNetUser != null)
            {
                fullName = string.Concat(aspNetUser.FirstName, " ", aspNetUser.LastName);
            }
            return fullName;
        }

        public AspNetUser GetAspNetUser(string Id)
        {
            var result = _dataService.GetAspNetUser(Id);
            if (result == null)
            {
                AspNetUser user = null;
                return user;
            }
            else
            {
                return MapEFToModel(result);
            }
        }

        public IEnumerable<AspNetUser> GetAllAspNetUsers()
        {
            var results = this._dataService.GetAspNetUsers();
            return MapEFToModel(results);
        }


        public IEnumerable<AspNetUser> GetAllAdmins()
        {
            var results = this._dataService.GetAspNetUsers();
            List<EF.Models.AspNetUser> admins = new List<EF.Models.AspNetUser>();
            if (results.Any())
            {
                foreach (var result in results)
                {
                    var roleName = string.Empty;
                    var roles = result.AspNetRoles.ToList();
                    foreach (var role in roles)
                    {
                        roleName = role.Name;
                    }
                    if (roleName == "admin")
                    {
                        admins.Add(result);
                    }

                }
                return MapEFToModel(admins);
            }
            return null;

        }

       public IEnumerable<UserBranch> GetAllUserBranches(string Id)
        {

            var results = this._dataService.GetAllUserBranches(Id);
            return MapEFUserBranchToModelUserBranch(results);
           
        }
        public IEnumerable<AspNetUserViewModel> GetAllCustomers()
        {
            var results = this._dataService.GetAspNetUsers();
            List<EF.Models.AspNetUser> customers = new List<EF.Models.AspNetUser>();
            if (results.Any())
            {
                foreach (var result in results)
                {
                    var roleName = string.Empty;
                    var roles = result.AspNetRoles.ToList();
                    foreach (var role in roles)
                    {
                        roleName = role.Name;
                    }
                    if (roleName == "customer")
                    {
                        customers.Add(result);
                    }

                }
                return MapEFAspNetUserToAspNetUserViewModel(customers);
            }
            return null;

        }

        public IEnumerable<AspNetUserViewModel> GetAllMechanics()
        {
            var results = this._dataService.GetAspNetUsers();
            List<EF.Models.AspNetUser> mechanics = new List<EF.Models.AspNetUser>();
            if (results.Any())
            {
                foreach (var result in results)
                {
                    var roleName = string.Empty;
                    var roles = result.AspNetRoles.ToList();
                    foreach (var role in roles)
                    {
                        roleName = role.Name;
                    }
                    if (roleName == "mechanic")
                    {
                        mechanics.Add(result);
                    }

                }
                return MapEFAspNetUserToAspNetUserViewModel(mechanics);
            }
            return null;

        }
        public IEnumerable<AspNetUserViewModel> GetAllOutSourcers()
        {
            var results = this._dataService.GetAspNetUsers();
            List<EF.Models.AspNetUser> outsourcers = new List<EF.Models.AspNetUser>();
            if (results.Any())
            {
                foreach (var result in results)
                {
                    var roleName = string.Empty;
                    var roles = result.AspNetRoles.ToList();
                    foreach (var role in roles)
                    {
                        roleName = role.Name;
                    }
                    if (roleName == "outsourcer")
                    {
                        outsourcers.Add(result);
                    }

                }
                return MapEFAspNetUserToAspNetUserViewModel(outsourcers);
            }
            return null;

        }

        public IEnumerable<AspNetUserViewModel> GetAllCustomersForAParticularRegion(long regionId)
        {
            var results = this._dataService.GetAllCustomersForAParticularRegion(regionId);
            List<EF.Models.AspNetUser> customers = new List<EF.Models.AspNetUser>();
            if (results.Any())
            {
                foreach (var result in results)
                {
                    var roleName = string.Empty;
                    var roles = result.AspNetRoles.ToList();
                    foreach (var role in roles)
                    {
                        roleName = role.Name;
                    }
                    if (roleName == "customer")
                    {
                        customers.Add(result);
                    }

                }
                return MapEFAspNetUserToAspNetUserViewModel(customers);
            }
            return null;

        }

        public IEnumerable<AspNetUserViewModel> GetAllSuppliers()
        {
            var results = this._dataService.GetAspNetUsers();
            List<EF.Models.AspNetUser> suppliers = new List<EF.Models.AspNetUser>();
            if (results.Any())
            {
                foreach (var result in results)
                {
                    var roleName = string.Empty;
                    var roles = result.AspNetRoles.ToList();
                    foreach (var role in roles)
                    {
                        roleName = role.Name;
                    }
                    if (roleName == "supplier")
                    {
                        suppliers.Add(result);
                    }

                }
                return MapEFAspNetUserToAspNetUserViewModel(suppliers);
            }
            return null;

        }
        public IEnumerable<AspNetUser> GetAllBranchManagers()
        {
            var results = this._dataService.GetAspNetUsers();
            List<EF.Models.AspNetUser> managers = new List<EF.Models.AspNetUser>();
            if (results.Any())
            {
                foreach (var result in results)
                {
                    var roleName = string.Empty;
                    var roles = result.AspNetRoles.ToList();
                    foreach (var role in roles)
                    {
                        roleName = role.Name;
                    }
                    if (roleName == "branchManager")
                    {
                        managers.Add(result);
                    }

                }
                return MapEFToModel(managers);
            }
            return null;

        }


        public IEnumerable<AspNetUser> GetAllBranchManagersSuperAdminsSuperUsersAndAdmins()
        {
            var results = this._dataService.GetAspNetUsers();
            List<EF.Models.AspNetUser> managers = new List<EF.Models.AspNetUser>();
            if (results.Any())
            {
                foreach (var result in results)
                {
                    List<string> listOfRoles = new List<string>() { "branchManager", "superadmin", "admin", "superuser" };
                    var roleName = string.Empty;
                    var roles = result.AspNetRoles.ToList();
                    foreach (var role in roles)
                    {
                        roleName = role.Name;
                    }
                    
                    if (listOfRoles.Contains(roleName))
                    {
                        managers.Add(result);
                    }

                }
                return MapEFToModel(managers);
            }
            return null;

        }
        public IEnumerable<AspNetRole> GetAllRoles()
        {
            var results = _dataService.GetAllRoles();
            return MapEFRoleToModelRole(results);
        }

        public AspNetRole GetAspNetRole(string roleId)
        {
            var result = _dataService.GetAspNetRole(roleId);
            return MapEFRoleToModelRole(result);
        }

        public IEnumerable<AspNetUserProduct> GetAspNetUserProducts(string userId)
        {
            var results = _dataService.GetAspNetUserProducts(userId);
            return MapEFAspNetUserProductToModelAspNetUserProduct(results);
        }

        public void SaveAspNetUserProduct(string userId,long productId)
        {
            var aspNetUserProductDTO = new AspNetUserProductDTO()
            {
                Id = userId,
                ProductId = productId,
            };
             _dataService.SaveAspNetUserProduct(aspNetUserProductDTO);
            
        }
        public IEnumerable<Region> GetAllRegions()
        {
            var results = _dataService.GetAllRegions();
            return MapEFRegionToModelRegion(results);
        }

        public IEnumerable<Location> GetAllLocationsForAParticularRegion(long regionId)
        {
            var results = _dataService.GetAllLocationsForAParticularRegion(regionId);
            return MapEFLocationToModelLocation(results);
        }

        #region Mapping Methods
        /// <summary>
        /// Maps An IEnumerable collection of EF AspNetUser objects to An IEnumerable collection of AspNetUser Models Objects.
        /// </summary>
        /// <param name="data">An IEnumerable collection of AspNetUser EF objects.</param>
        /// <returns>An IEnumerable collection of AspNetUser Models objects.</returns>
        private IEnumerable<AspNetUser> MapEFToModel(IEnumerable<EF.Models.AspNetUser> data)
        {
            var list = new List<AspNetUser>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }
        private IEnumerable<AspNetUserViewModel> MapEFAspNetUserToAspNetUserViewModel(IEnumerable<EF.Models.AspNetUser> data)
        {
            var list = new List<AspNetUserViewModel>();
            foreach (var result in data)
            {
                list.Add(MapEFAspNetUserToAspNetUserViewModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps AspNetUser EF Object to AspNetUser Models Object
        /// and returns AspNetUser Model object.
        /// </summary>
        /// <param name="data">AspNet EF Object.</param>
        /// <returns>AspNet Models Object</returns>
        private AspNetUser MapEFToModel(EF.Models.AspNetUser data)
        {
            if (data != null)
            {


                var user = new AspNetUser()
                {
                    Id = data.Id,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Email = data.Email,
                    UserName = data.UserName,
                    MiddleName = data.MiddleName,
                    UniqueNumber = data.UniqueNumber,
                    PhoneNumber = data.PhoneNumber,
                    Mobile = data.Mobile,
                    PasswordHash = data.PasswordHash,
                    GenderId = data.GenderId,
                    TimeStamp = data.TimeStamp,
                    DateOfBirth = data.DateOfBirth,
                    CreatedOn = data.CreatedOn,
                    RoleName = data.AspNetRoles.FirstOrDefault().Name,
                    RegionId = data.RegionId,
                    CreatedBy = GetUserFullName(data.AspNetUser1),
                    UpdatedBy = GetUserFullName(data.AspNetUser2),

                };

                if ( data.Region != null &&(user.RegionId != null || user.RegionId != 0))
                {
                    //var region = GetRegion(Convert.ToInt64(user.RegionId));
                    var region = data.Region;
                    if (region != null)
                    {
                        user.RegionName = region.Name;
                    }

                }
                List<string> attachedBranches = new List<string>();
                var userBranches = GetAllUserBranches(data.Id);
                List<string> userroles = new List<string>();

                var dbUserRoles = data.AspNetRoles;
                var numberOfRoles = dbUserRoles.Count;
                EF.Models.AspNetRole[] roles = new EF.Models.AspNetRole[numberOfRoles];
                if (dbUserRoles != null)
                {
                    dbUserRoles.CopyTo(roles, 0);
                    foreach (var role in roles)
                    {
                        userroles.Add(role.Name);
                    }
                }
                if(userBranches != null)
                {
                    foreach (var branch in userBranches)
                    {
                        attachedBranches.Add(branch.BranchName);
                    }
                    
                }
                user.UserRoles = userroles;
              
                return user;
            }
            return null;
        }


        private AspNetUserViewModel MapEFAspNetUserToAspNetUserViewModel(EF.Models.AspNetUser data)
        {
            if (data != null)
            {

                var user = new AspNetUserViewModel()
                {
                    Id = data.Id,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    UserName = data.UserName,
                    Email = data.Email,
                    UniqueNumber = data.UniqueNumber,
                    PhoneNumber = data.PhoneNumber,
                                      
                    RegionId = data.RegionId,
                   

                };

                         

                return user;
            }
            return null;
        }




        private IEnumerable<AspNetRole> MapEFRoleToModelRole(IEnumerable<EF.Models.AspNetRole> data)
        {
            var list = new List<AspNetRole>();
            foreach (var result in data)
            {
                list.Add(MapEFRoleToModelRole(result));
            }
            return list;
        }


        private AspNetRole MapEFRoleToModelRole(EF.Models.AspNetRole data)
        {
            var role = new AspNetRole()
            {
                Id = data.Id,
                Name = data.Name,
            };
            return role;
        }

        private IEnumerable<AspNetUserProduct> MapEFAspNetUserProductToModelAspNetUserProduct(IEnumerable<EF.Models.AspNetUserProduct> data)
        {
            var list = new List<AspNetUserProduct>();
            foreach (var result in data)
            {
                list.Add(MapEFAspNetUserProductToModelAspNetUserProduct(result));
            }
            return list;
        }


        private AspNetUserProduct MapEFAspNetUserProductToModelAspNetUserProduct(EF.Models.AspNetUserProduct data)
        {
            var aspNetUserProduct = new AspNetUserProduct()
            {
                Id = data.Id,
                ProductId = data.ProductId,
                AspNetUserName = data.AspNetUser != null ? data.AspNetUser.FirstName  : "",
                ProductName = data.Product != null ? data.Product.Name : "",
            };
            return aspNetUserProduct;
        }

        private IEnumerable<Region> MapEFRegionToModelRegion(IEnumerable<EF.Models.Region> data)
        {
            var list = new List<Region>();
            foreach (var result in data)
            {
                list.Add(MapEFRegionToModelRegion(result));
            }
            return list;
        }

        public Region GetRegion(long regionId)
        {
            var result = _dataService.GetRegion(regionId);
            return MapEFRegionToModelRegion(result);
        }

        private Region MapEFRegionToModelRegion(EF.Models.Region data)
        {
            if (data != null)
            {
                var district = new Region()
                {
                    RegionId = data.RegionId,
                    Name = data.Name,
                    Initials = data.Initials,
                };
                return district;
            }
            return null;
        }

        private Location MapEFLocationToModelLocation(EF.Models.Location data)
        {
            if (data != null)
            {
                var location = new Location()
                {
                    LocationId = data.LocationId,
                    Name = data.Name,
                    RegionId = data.RegionId,
                    Initials = data.Initials,
                    RegionName = data.Region != null ? data.Region.Name : "",
                };
                return location;
            }
            return null;
        }

        private IEnumerable<Location> MapEFLocationToModelLocation(IEnumerable<EF.Models.Location> data)
        {
            var list = new List<Location>();
            foreach (var result in data)
            {
                list.Add(MapEFLocationToModelLocation(result));
            }
            return list;
        }

        private IEnumerable<EF.Models.AspNetRole> MapModelRoleToEFRole(IEnumerable<AspNetRole> data)
        {
            var list = new List<EF.Models.AspNetRole>();
            foreach (var result in data)
            {
                list.Add(MapModelRoleToEFRole(result));
            }
            return list;
        }


        private EF.Models.AspNetRole MapModelRoleToEFRole(AspNetRole data)
        {
            var role = new EF.Models.AspNetRole()
            {
                Id = data.Id,
                Name = data.Name,
            };
            return role;
        }
        private IEnumerable<UserBranch> MapEFUserBranchToModelUserBranch(IEnumerable<EF.Models.UserBranch> data)
        {
            var list = new List<UserBranch>();
            foreach (var result in data)
            {
                list.Add(MapEFUserBranchToModelUserBranch(result));
            }
            return list;
        }
        public UserBranch MapEFUserBranchToModelUserBranch(EF.Models.UserBranch data)
        {

            var userBranch = new UserBranch()
            {
                BranchId = data.BranchId,
                UserId = data.UserId,
                BranchName = data.Branch != null ? data.Branch.Name : "",
               };
            return userBranch;
        }

        #endregion

         public void SaveUserBranch(string userId,long branchId)
         {
           var userBranchDTO = new UserBranchDTO()
           {
               UserId = userId,
               BranchId = branchId,
           };
       
       this._dataService.SaveUserBranch(userBranchDTO);

   }
    public UserBranch GetBranchManager(string branchManagerId)
       {
            
             var result = _dataService.GetUserBranch(branchManagerId);
             if (result == null)
             {
                 UserBranch userBranch = null;
                 return userBranch;
             }
             else
             {
                 return MapEFUserBranchToModelUserBranch(result);
             }
             

         }

     public bool MarkAsDeleted(string Id)
        {
            return _dataService.MarkAsDeleted(Id);
        }

     public bool IsAssignedBranch(string userId)
     {
         return _dataService.IsAssignedBranch(userId);
     }

     public long GetLoggedUserBranchId(string userId)
     {
         return _dataService.GetLoggedUserBranchId(userId);
     }

    }
}
