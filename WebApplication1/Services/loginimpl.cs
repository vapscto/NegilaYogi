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
using System.Data;
using System.Collections.Concurrent;

namespace WebApplication1.Services
{
    public class loginimpl:logininterface
    {
        private static ConcurrentDictionary<string, regis> _login =
                new ConcurrentDictionary<string, regis>();

        private readonly registrationcontext _registrationcontext;
        public loginimpl(registrationcontext registrationcontext)
        {
            _registrationcontext = registrationcontext;
        }


        public string getregdata(string username)
        {
            //regis user;
            bool returnval;
            ////_login.TryGetValue(username.username, out user);
            //_login.TryGetValue(usernme, out user);
            //return user;

            // bool contactExists = _registrationcontext.Registration.Any(contact => contact.username.Equals(reg.username));
            //bool contactExists = _registrationcontext.Registration.Any(contact => contact.username.Equals(reg.username));

            var contactExists = _registrationcontext.Registration.Where(t => t.username.Contains(username)).Distinct();

            //Console.WriteLine($"Number of albums satisfied the condition: {contactExists.Count()}");

            //foreach (Registration item in contactExists)
            //{
            //    Console.WriteLine($"\t {item.username}");
            //}

            if (contactExists.Count() > 0)
            {
                returnval = true;
            }
            else
            {
                returnval = false;
            }
            return "";
        }

        public string checkusrnmepwd(regis reg)
        {
            Registration regi = new Registration();

            regi.Email_id = reg.Email_id;
            regi.Mobileno = reg.Mobileno;
            //string query = "select * from Adm_online_application_download_details where Email_id='"+ reg.Email_id + "' and '"+ reg.Mobileno + "'";
            _registrationcontext.Update(reg);
            //_registrationcontext.SaveChanges();
            return reg.username;
        }

        //public regis Find(regis id)
        //{
            
        //    //regis status;
        //    _login.TryGetValue(id.Email_id,out id);
        //    if(id.status==true)
        //    {
               
        //    }
        //    return id;
        //}

        //public IEnumerable<regis> GetAll()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
