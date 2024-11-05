//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;



namespace DomainModel.Model
{
    public class ApplicationUser : IdentityUser<int>
    {
        // Added on 11-11-2016 for Organization, Institute & IP address
        //public long MI_Id { get; set; }
        public DateTime Entry_Date { get; set; }
        public string Machine_Ip_Address { get; set; }

        public string UserImagePath { get; set; }

        public string RoleTypeFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string Name  { get; set; }
        // Added on 11-11-2016 for Organization, Institute & IP address
    }


    public class ApplicationRole : IdentityRole<int>
    {
        //public ApplicationRole() : base() { }
        public string roleType { get; set; }
    }

    public class ApplicationUserRole : IdentityUserRole<int>
    {
        public ApplicationUserRole() : base() { }

        //[MaxLength(128)]
        //public override int RoleId { get; set; }

        //[MaxLength(128)]
        //public override int UserId { get; set; }
    }

    public class ApplicationRoleClaim : IdentityRoleClaim<int>
    {
        public ApplicationRoleClaim() : base() { }
        //[MaxLength(128)]
        //public override String RoleId { get; set; }
    }

    public class ApplicationUserClaims : IdentityUserClaim<int>
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id
        //{
        //    get;
        //    set;
        //}
    }

    public class ApplicationUserLogins : IdentityUser<int>
    {

       
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id
        //{
        //    get;
        //    set;
        //}

        public string test { get; set; }

    }


    public class ApplicationUserToken : IdentityUserToken<int>
    {
    }


//    public class CustomUserStore : UserStore<ApplicationUser, ApplicationRole, int,
//    ApplicationUserLogins, ApplicationUserRole, ApplicationUserClaims>
//    {
//        public CustomUserStore(ApplicationDbContext context)
//            : base(context)
//        {
//        }
//    }

//    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
//    {
//        public CustomRoleStore(ApplicationDbContext context)
//            : base(context)
//        {
//        }
//    }

}