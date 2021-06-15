using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Higgs.Mbale.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Threading.Tasks;
using Higgs.Mbale.EF.Models;
using System.Net;
using PagedList;
using Higgs.Mbale.BAL.Interface;

namespace Higgs.Mbale.Web.Controllers
{
    public class UsersAdminController : Controller
    {       MbaleEntities context = new MbaleEntities();
        private const int PAGESIZE = 500;
        private IUserService _userService;
        private IProductService _productService;

        public UsersAdminController()
        {

        }

        public UsersAdminController(IUserService userService,IProductService productService)
        {
            this._userService = userService;
            this._productService = productService;
        }

        public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager, IUserService userService,IProductService productService)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            this._userService = userService;
            this._productService = productService;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }


        public ActionResult Index(string sortingOrder, string searchData, string Filter_Value, int? Page_No)
        {
            int Size_Of_Page = PAGESIZE;
            ViewBag.CurrentSortOrder = sortingOrder;
            ViewBag.SortingFirstName = String.IsNullOrEmpty(sortingOrder) ? "firstNameSorting" : "";
            ViewBag.SortingLastName = String.IsNullOrEmpty(sortingOrder) ? "lastNameSorting" : "";
            ViewBag.SortingUserName = String.IsNullOrEmpty(sortingOrder) ? "userNameSorting" : "";
            ViewBag.SortingPhoneNumber = String.IsNullOrEmpty(sortingOrder) ? "phoneNumberSorting" : "";
            ViewBag.SortingEmailAddress = String.IsNullOrEmpty(sortingOrder) ? "emailAddressSorting" : "";

            var searchText = string.Empty;
            if (!string.IsNullOrEmpty(searchData))
            {
                searchText = searchData.ToLower();
                Page_No = 1;
            }
            else
            {
                searchData = Filter_Value;
            }

            ViewBag.FilterValue = searchData;

            var usersx = UserManager.Users.ToList();
            var users = _userService.GetAllAdmins();
            if (users != null)
            {
                users = users.Where(u => u.FirstName.ToLower().Contains(searchText) ||
                u.LastName.ToLower().Contains(searchText)
                || u.Email.ToLower().Contains(searchText)
                || u.UserName.ToLower().Contains(searchText)).ToList();

            }

            switch (sortingOrder)
            {
                case "firstNameSorting":
                    users = users.OrderByDescending(u => u.FirstName).ToList();
                    break;
                case "lastNameSorting":
                    users = users.OrderByDescending(u => u.LastName).ToList();
                    break;
                case "userNameSorting":
                    users = users.OrderByDescending(u => u.UserName).ToList();
                    break;
                case "emailAddressSorting":
                    users = users.OrderByDescending(u => u.Email).ToList();
                    break;
                case "phoneNumberSorting":
                    users = users.OrderByDescending(u => u.PhoneNumber).ToList();
                    break;
                default:
                    users = users.OrderBy(u => u.FirstName).ToList(); ;
                    break;
            }


            int No_Of_Page = (Page_No ?? 1);
            return View(users.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        public ActionResult Suppliers(string sortingOrder, string searchData, string Filter_Value, int? Page_No, string submit = "")
        {
            int Size_Of_Page = PAGESIZE;
            ViewBag.CurrentSortOrder = sortingOrder;
            ViewBag.SortingFirstName = String.IsNullOrEmpty(sortingOrder) ? "firstNameSorting" : "";
            ViewBag.SortingLastName = String.IsNullOrEmpty(sortingOrder) ? "lastNameSorting" : "";
            ViewBag.SortingUserName = String.IsNullOrEmpty(sortingOrder) ? "userNameSorting" : "";
            ViewBag.SortingPhoneNumber = String.IsNullOrEmpty(sortingOrder) ? "phoneNumberSorting" : "";
            ViewBag.SortingEmailAddress = String.IsNullOrEmpty(sortingOrder) ? "emailAddressSorting" : "";

            var searchText = string.Empty;
            if (!string.IsNullOrEmpty(searchData))
            {
                searchText = searchData.ToLower();
                Page_No = 1;
            }
            else
            {
                searchData = Filter_Value;
            }

            ViewBag.FilterValue = searchData;

            var usersx = UserManager.Users.ToList();
            var users = _userService.GetAllSuppliers();
            if (users != null)
            {
                users = users.Where(u => u.FirstName.ToLower().Contains(searchText) ||
                u.LastName.ToLower().Contains(searchText)
                || u.Email.ToLower().Contains(searchText)
                || u.UserName.ToLower().Contains(searchText)).ToList();

            }

            switch (sortingOrder)
            {
                case "firstNameSorting":
                    users = users.OrderByDescending(u => u.FirstName).ToList();
                    break;
                case "lastNameSorting":
                    users = users.OrderByDescending(u => u.LastName).ToList();
                    break;
                case "userNameSorting":
                    users = users.OrderByDescending(u => u.UserName).ToList();
                    break;
                case "emailAddressSorting":
                    users = users.OrderByDescending(u => u.Email).ToList();
                    break;
                case "phoneNumberSorting":
                    users = users.OrderByDescending(u => u.PhoneNumber).ToList();
                    break;
                default:
                    users = users.OrderBy(u => u.FirstName).ToList(); ;
                    break;
            }


            int No_Of_Page = (Page_No ?? 1);
            return View(users.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        public ActionResult Customers(string sortingOrder, string searchData, string Filter_Value, int? Page_No, string submit = "")
        {
            int Size_Of_Page = PAGESIZE;
            ViewBag.CurrentSortOrder = sortingOrder;
            ViewBag.SortingFirstName = String.IsNullOrEmpty(sortingOrder) ? "firstNameSorting" : "";
            ViewBag.SortingLastName = String.IsNullOrEmpty(sortingOrder) ? "lastNameSorting" : "";
            ViewBag.SortingUserName = String.IsNullOrEmpty(sortingOrder) ? "userNameSorting" : "";
            ViewBag.SortingPhoneNumber = String.IsNullOrEmpty(sortingOrder) ? "phoneNumberSorting" : "";
            ViewBag.SortingEmailAddress = String.IsNullOrEmpty(sortingOrder) ? "emailAddressSorting" : "";

            var searchText = string.Empty;
            if (!string.IsNullOrEmpty(searchData))
            {
                searchText = searchData.ToLower();
                Page_No = 1;
            }
            else
            {
                searchData = Filter_Value;
            }

            ViewBag.FilterValue = searchData;

            var usersx = UserManager.Users.ToList();
            var users = _userService.GetAllCustomers();
            if (users != null)
            {
                users = users.Where(u => u.FirstName.ToLower().Contains(searchText) ||
                u.LastName.ToLower().Contains(searchText)
                || u.Email.ToLower().Contains(searchText)
                || u.UserName.ToLower().Contains(searchText)).ToList();

            }

            switch (sortingOrder)
            {
                case "firstNameSorting":
                    users = users.OrderByDescending(u => u.FirstName).ToList();
                    break;
                case "lastNameSorting":
                    users = users.OrderByDescending(u => u.LastName).ToList();
                    break;
                case "userNameSorting":
                    users = users.OrderByDescending(u => u.UserName).ToList();
                    break;
                case "emailAddressSorting":
                    users = users.OrderByDescending(u => u.Email).ToList();
                    break;
                case "phoneNumberSorting":
                    users = users.OrderByDescending(u => u.PhoneNumber).ToList();
                    break;
                default:
                    users = users.OrderBy(u => u.FirstName).ToList(); ;
                    break;
            }


            int No_Of_Page = (Page_No ?? 1);
            return View(users.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        public ActionResult BranchManagers(string sortingOrder, string searchData, string Filter_Value, int? Page_No, string submit = "")
        {
            int Size_Of_Page = PAGESIZE;
            ViewBag.CurrentSortOrder = sortingOrder;
            ViewBag.SortingFirstName = String.IsNullOrEmpty(sortingOrder) ? "firstNameSorting" : "";
            ViewBag.SortingLastName = String.IsNullOrEmpty(sortingOrder) ? "lastNameSorting" : "";
            ViewBag.SortingUserName = String.IsNullOrEmpty(sortingOrder) ? "userNameSorting" : "";
            ViewBag.SortingPhoneNumber = String.IsNullOrEmpty(sortingOrder) ? "phoneNumberSorting" : "";
            ViewBag.SortingEmailAddress = String.IsNullOrEmpty(sortingOrder) ? "emailAddressSorting" : "";

            var searchText = string.Empty;
            if (!string.IsNullOrEmpty(searchData))
            {
                searchText = searchData.ToLower();
                Page_No = 1;
            }
            else
            {
                searchData = Filter_Value;
            }

            ViewBag.FilterValue = searchData;

            var usersx = UserManager.Users.ToList();
            var users = _userService.GetAllBranchManagers();
            if (users != null)
            {
                users = users.Where(u => u.FirstName.ToLower().Contains(searchText) ||
                u.LastName.ToLower().Contains(searchText)
                || u.Email.ToLower().Contains(searchText)
                || u.UserName.ToLower().Contains(searchText)).ToList();

            }

            switch (sortingOrder)
            {
                case "firstNameSorting":
                    users = users.OrderByDescending(u => u.FirstName).ToList();
                    break;
                case "lastNameSorting":
                    users = users.OrderByDescending(u => u.LastName).ToList();
                    break;
                case "userNameSorting":
                    users = users.OrderByDescending(u => u.UserName).ToList();
                    break;
                case "emailAddressSorting":
                    users = users.OrderByDescending(u => u.Email).ToList();
                    break;
                case "phoneNumberSorting":
                    users = users.OrderByDescending(u => u.PhoneNumber).ToList();
                    break;
                default:
                    users = users.OrderBy(u => u.FirstName).ToList(); ;
                    break;
            }


            int No_Of_Page = (Page_No ?? 1);
            return View(users.ToPagedList(No_Of_Page, Size_Of_Page));
        }


        //
        // GET: /Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);

            return View(user);
        }

        //
        // GET: /Users/Create
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            
            //get the list of districts
            var districts = _userService.GetAllRegions();

            var districtList = districts.ToList();

            ViewBag.Name = new SelectList(districtList.Where(u => !u.Name.Contains("Admin"))
                                     .ToList(), "RegionId", "Name");

            //get the list of products
            var products = _productService.GetAllProducts().ToList();
          
            ViewBag.ProductName = new SelectList(products.Where(u => !u.Name.Contains("Name"))
                                    .ToList(),"Name", "Name");


           

            //get the list of districts
            //var locations = _userService.GetAllLocationsForAParticularRegion();

            //var districtList = districts.ToList();

            //ViewBag.Name = new SelectList(districts.Where(u => !u.Name.Contains("Admin"))
            //                         .ToList(), "RegionId", "Name");
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
       // public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = userViewModel.UserName,
                    Email = userViewModel.Email,
                    FirstName = userViewModel.FirstName,
                    LastName = userViewModel.LastName,
                    PhoneNumber = userViewModel.PhoneNumber,
                    UniqueNumber = userViewModel.UniqueNumber,
                    RegionId = userViewModel.RegionId,
                    //SelectedProducts = userViewModel.SelectedProducts,
                };
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                        if (selectedRoles.Contains("5ca87625-1e97-448d-b9a7-5619df8fb3e1"))
                        {
                            foreach (var item in user.SelectedProducts)
                            {
                                this._userService.SaveAspNetUserProduct(user.Id, item);;
                            }
                        }
                    }

                   
                }
                else
                {
                    //get the list of products
                    var products = _productService.GetAllProducts().ToList();
                    var districts = _userService.GetAllRegions().ToList();
                    ModelState.AddModelError("", adminresult.Errors.First());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                    //ViewBag.ProductName = new SelectList(products.Where(u => !u.Name.Contains("Name"))
                    //               .ToList(), "ProductId", "Name");
                    ViewBag.RegionName = new SelectList(districts.Where(u => !u.Name.Contains("Name"))
                                  .ToList(), "RegionId", "Name");

                    return View();

                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View();
        }

        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(string id)
        {

           string selected = string.Empty;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(user.Id);
            var userProducts = this._userService.GetAspNetUserProducts(user.Id).ToList();

            //get the list of products
            var products = _productService.GetAllProducts();
            //get the list of districts
            var districts = _userService.GetAllRegions();


            //selected = (from reg in context.Regions
            if (user.RegionId != null)
            {
                selected = (from reg in districts
                            where reg.RegionId == user.RegionId
                            select reg.Name).FirstOrDefault();
                ViewBag.Name = new SelectList(districts, "RegionId", "Name", selected);
            }
            else
            {
                ViewBag.Name = new SelectList(districts.Where(u => !u.Name.Contains("Admin"))
                                     .ToList(), "RegionId", "Name");

            }
           

            var userToEdit = new AdminViewModels.EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                UniqueNumber = user.UniqueNumber,
              SelectedRegionId = Convert.ToInt64(user.RegionId),

                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                }),

                ProductsList = products.ToList().Select(x => new SelectListItem()
                {
                    //Selected = userProducts,
                    Text = x.Name,
                    Value = Convert.ToString(x.ProductId)
                }),
                //DistrictsList = districts.ToList();

                DistrictsList = districts.ToList().Select(x => new SelectListItem()
                {
                    
                    Selected = true,
                    //Selected = districts.Contains(x.RegionId),
                    Text = x.Name,
                    Value =Convert.ToString(x.RegionId)
                }),


         

            };
        
          
                return View(userToEdit);
            
           
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Email,Id,LastName,FirstName,PhoneNumber,UserName,UniqueNumber,RegionId")] AdminViewModels.EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                var districts = _userService.GetAllRegions();
                //var districtList = new SelectList(new[] {districts});
                //ViewBag.ExemploList = districtList;

                
                var districtList = districts.ToList();
                //ViewBag.ExemploList = districtList;
                ViewBag.Name = new SelectList(districts.Where(u => !u.Name.Contains("Admin"))
                                         .ToList(), "RegionId", "Name");


                user.Email = editUser.Email;
                user.FirstName = editUser.FirstName;
                user.LastName = editUser.LastName;
                user.PhoneNumber = editUser.PhoneNumber;
                user.UserName = editUser.UserName;
                user.UniqueNumber = editUser.UniqueNumber;
                user.RegionId = editUser.RegionId;
              //  user.RegionId = editUser.SelectedRegionId;

                var userRoles = await UserManager.GetRolesAsync(user.Id);

                selectedRole = selectedRole ?? new string[] { };

                var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                }

                return RedirectToAction("Index");
            }
            else
            {
                // ModelState.AddModelError("", user.Errors.First());
            }

            return View();
        }

        //
        // GET: /Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                var result = _userService.MarkAsDeleted(user.Id);
                if (result == false)
                {
                    return View();
                }

                return RedirectToAction("Index");
            }
            return View();
        }
    }
}