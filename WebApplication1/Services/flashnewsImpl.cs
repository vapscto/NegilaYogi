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
using AutoMapper;
using System.Collections.Concurrent;
using CommonLibrary;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Services
{
    public class flashnewsImpl  : Interfaces.flashnewsInterface
    {
       
        private static ConcurrentDictionary<string, regis> _login =
               new ConcurrentDictionary<string, regis>();
        private readonly UserManager<ApplicationUser> _userManager;
        public Enquirycontext _Enquirycontext;
        public DomainModelMsSqlServerContext _context;
        //ILogger<EnqImpl> _logger;
        readonly ILogger<EnqImpl> _logger;

        public flashnewsImpl(UserManager<ApplicationUser> userManager, DomainModelMsSqlServerContext context, ILogger<EnqImpl> log)
        {
            _userManager = userManager;
            _context = context;
            _logger = log;
        }
      

        public async Task<regis> saveEnqdata(regis enqu)
        {
            enqu.returnMsg = "";
            ApplicationUser user = new ApplicationUser();
            try
            {
                var result = _context.Institution.Single(t => t.MI_Id == enqu.MI_Id);
                // var result = _OrganisationContext.Organisation.AsNoTracking().Single(t => t.MO_Id == enq.MO_Id);

                result.MI_HelpFile = enqu.password;

                //added by 02/02/2017

                result.UpdatedDate = DateTime.Now;
                _context.Update(result);
                var contactExists = _context.SaveChanges();

                if (contactExists == 1)
                {

                    enqu.returnMsg = "Success";
                }
                else
                {

                    enqu.returnMsg = "fail";
                }

            }
            catch (Exception ex)
            {
                enqu.returnMsg = "Error";
                Console.Write(ex.Message);
            }
            return enqu;
        }

       
    }
}
