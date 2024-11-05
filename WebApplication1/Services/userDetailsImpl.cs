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

namespace WebApplication1.Services
{
    public class userDetailsImpl : Interfaces.userdet
    {
        private static ConcurrentDictionary<string, usrdetails> _login =
       new ConcurrentDictionary<string, usrdetails>();

        private readonly userDetailscontext _userDetailscontext;

        //public registrationcontext _registrationcontext;

        public userDetailsImpl(userDetailscontext userDetailscontext)
        {
            _userDetailscontext = userDetailscontext;
        }
        public string userdata(usrdetails usd)
        {
            string username = "";
            userDetails usrdet = Mapper.Map<userDetails>(usd);

            _userDetailscontext.Add(usrdet);
            _userDetailscontext.SaveChanges();

            return username;
        }
    }
}
