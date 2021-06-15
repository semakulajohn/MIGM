using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Higgs.Mbale.Web.Models
{
    public class AdminViewModels
    {
        public class RoleViewModel
        {
            public string Id { get; set; }
            [Required(AllowEmptyStrings = false)]
            [Display(Name = "RoleName")]
            public string Name { get; set; }

            public string Description { get; set; }

        }

        public class EditUserViewModel
        {
            public string Id { get; set; }

            [Required(AllowEmptyStrings = false)]
            [Display(Name = "Email Address")]
            [EmailAddress]
            public string Email { get; set; }

            [Required(AllowEmptyStrings = false)]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            public IEnumerable<SelectListItem> RolesList { get; set; }

            public IEnumerable<long> SelectedProducts { get; set; }
            public IEnumerable<SelectListItem> ProductsList { get; set; }


            [Display(Name = "Districts")]
            public long SelectedRegionId { get; set; }
            public IEnumerable<SelectListItem> DistrictsList { get; set; }

            public SelectList Regions { get; set; }

            [Display(Name = "Districts")]
            public long RegionId { get; set; }

            //[Display(Name = "Districts")]
            //public long SelectedRegionId { get; set; }
            //public IEnumerable<SelectListItem> DistrictsList { get; set; }

            //public SelectList Regions { get; set; }

            //[Display(Name = "Districts")]
            //public long RegionId { get; set; }

            //[Required]
            //[Display(Name = "State / Region")]
            //public string SelectedRegionCode { get; set; }
            //public IEnumerable<SelectListItem> RegionsNew { get; set; }

            [Required]
            [Display(Name = "First Name")]
            [StringLength(40, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
            public string FirstName { get; set; }

            [Required]
            [StringLength(40, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            public string PhoneNumber { get; set; }

            public string UniqueNumber { get; set; }


        }

        public class RegionViewModel
        {
            public long RegionId { get; set; }
            [Required(AllowEmptyStrings = false)]
            [Display(Name = "RoleName")]
            public string Name { get; set; }

        }
    }
}