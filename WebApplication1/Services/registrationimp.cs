using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using WebApplication1.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Services
{
    public class registrationimp :Interfaces.registration
    {
        private static ConcurrentDictionary<string, regis> _login =
        new ConcurrentDictionary<string, regis>();

        private readonly registrationcontext _registrationcontext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        //public registrationcontext _registrationcontext;

        public registrationimp(registrationcontext registrationcontext, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _registrationcontext = registrationcontext;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        //public string getregdata(string username)
        //{
        //    string name = "";
        //    regis status;
        //  // _login.TryGetValue(reg.Email_id,out reg);
        //    string emailid = "radha@vapstech.com";
        //    _login.TryGetValue(emailid, out reg);
        //    return name;
        //}

        public async Task<bool> getregdata(string username)
        {


            //var user1 = new ApplicationUser { UserName = "radha@vapstech.com", Email = "radha@vapstech.com" };

            //var result1 = await _userManager.CreateAsync(user1, "radha");

            var user = new ApplicationUser { UserName = username};

            var result = await _signInManager.PasswordSignInAsync(user, "radha" ,true, false);
            if (result.Succeeded)
            {
                return true;
            }


            //var contactExists = _registrationcontext.Registration.Where(t => t.username.Contains(username)).Distinct();


            //if (contactExists.Count() > 0)
            //{
            //    returnval = true;
            //}
            //else
            //{
            //    returnval = false;
            //}
            
            return false;
        }

        //public AM_Login_User Find(string ALU_ID)
        //{
        //    AM_Login_User user;
        //    _user.TryGetValue(ALU_ID, out user);
        //    return user;
        //}

        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return '';
        //    }

        //    var mov = await _registrationcontext.Registration.SingleOrDefaultAsync(m => m.Email_id == email_id);
        //    if (mov == null)
        //    {

        //    }
        //}

        public async Task<string> regdata(regis reg)
        {
            string username = "";

            //Registration regi = new Registration();
            //regi.student_name = reg.student_name;
            //regi.AMCL_ID = reg.AMCL_ID;
            //regi.Email_id = reg.Email_id;
            //regi.Mobileno = reg.Mobileno;
            //// regi.download_date = reg.download_date;
            //regi.username = reg.username;
            //regi.password = reg.password;
            //regi.old_password = reg.old_password;
            //regi.new_password = reg.new_password;
            //regi.user_ip_address = reg.user_ip_address;

             Registration regi = Mapper.Map<Registration>(reg);

            var user = new ApplicationUser { UserName = reg.username, Email = reg.Email_id };

            var result = await _userManager.CreateAsync(user, reg.password);

            if (result.Succeeded)
            {
                return username;
            }

           // _registrationcontext.Add(regi);
           // _registrationcontext.SaveChanges();

            return "failed";
        }
    }
}
