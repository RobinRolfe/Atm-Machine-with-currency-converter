﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;

namespace Atm11.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        [Display(Name = "First Name"), Required]
        public string firstName { get; set; }

        [Display(Name = "Middle Name"), Required]
        public string middleName { get; set; }

        [Display(Name = "Surname"), Required]
        public string surname { get; set; }

        [Display(Name = "Address 1"), Required]
        public string add1 { get; set; }

        [Display(Name = "Address 2")]
        public string add2 { get; set; }

        [Display(Name = "City"), Required]
        public string city { get; set; }

        [Display(Name = "Date of Birth"), Required]
        public DateTime DOB { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}