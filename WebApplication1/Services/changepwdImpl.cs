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
    public class changepwdImpl : Interfaces.changepwdInterface
    {

        private static ConcurrentDictionary<string, regis> _login =
               new ConcurrentDictionary<string, regis>();
        private readonly UserManager<ApplicationUser> _userManager;
        public Enquirycontext _Enquirycontext;
        public DomainModelMsSqlServerContext _context;
        //ILogger<EnqImpl> _logger;
        readonly ILogger<EnqImpl> _logger;

        public changepwdImpl(UserManager<ApplicationUser> userManager, DomainModelMsSqlServerContext context, ILogger<EnqImpl> log)
        {
            _userManager = userManager;
            _context = context;
            _logger = log;
        }


        public async Task<regis> saveEnqdata(regis enqu)
        {
            enqu.returnMsg = "";
            ApplicationUser user = new ApplicationUser();
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            try
            {
                if (enqu.User_Id != 0)
                {
                    user = await _userManager.FindByIdAsync(Convert.ToString(enqu.User_Id));

                    var check = await _userManager.CheckPasswordAsync(user, enqu.password);

                    if (check != true)
                    {
                        enqu.returnMsg = "fail";
                        return enqu;
                    }
                    else
                    {
                        var result = await _userManager.ChangePasswordAsync(user, enqu.password, enqu.new_password);
                        if (result.Succeeded)
                        {
                            _context.Database.ExecuteSqlCommand("Update_User_Date @p0", user.Id);
                            enqu.returnMsg = "Success";

                            if (enqu.changepasswordtypeflag == "FirstTimeLogin")
                            {
                                try
                                {
                                    var Mi_id_list = _context.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id).ToList();

                                    if (Mi_id_list.Count > 0)
                                    {
                                        enqu.MI_Id = Mi_id_list[0].MI_Id;
                                    }

                                    _context.Database.ExecuteSqlCommand("insert_history @p0,@p1,@p2,@p3,@p4,@p5,@p6", enqu.MI_Id, user.Id, enqu.sMacAddress, enqu.myIP1, "0", enqu.netip, indianTime);

                                    if (enqu.Logintype != "" && enqu.Logintype != null)
                                    {
                                        _context.Database.ExecuteSqlCommand("logintype_history @p0,@p1,@p2,@p3", enqu.MI_Id, user.Id, indianTime, enqu.Logintype);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                        else
                        {
                            //enqu.returnMsg = result.Errors.FirstOrDefault().Description;
                            enqu.returnMsg = "Passwords must have at least 6 characters,one Special,one lowercase and one uppercase character.";
                            return enqu;
                        }
                    }
                }
                else
                {
                    user = await _userManager.FindByNameAsync(enqu.username);

                    var result = await _userManager.ChangePasswordAsync(user, enqu.password, enqu.new_password);

                    if (result.Succeeded)
                    {
                        _context.Database.ExecuteSqlCommand("Update_User_Date @p0", user.Id);
                        enqu.returnMsg = "Success";

                        if (enqu.changepasswordtypeflag == "FirstTimeLogin")
                        {
                            try
                            {
                                var Mi_id_list = _context.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id).ToList();

                                if (Mi_id_list.Count > 0)
                                {
                                    enqu.MI_Id = Mi_id_list[0].MI_Id;
                                }

                                _context.Database.ExecuteSqlCommand("insert_history @p0,@p1,@p2,@p3,@p4,@p5,@p6", enqu.MI_Id, user.Id, enqu.sMacAddress, enqu.myIP1, "0", enqu.netip, indianTime);

                                if (enqu.Logintype != "" && enqu.Logintype != null)
                                {
                                    _context.Database.ExecuteSqlCommand("logintype_history @p0,@p1,@p2,@p3", enqu.MI_Id, user.Id, indianTime, enqu.Logintype);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    else
                    {
                        //enqu.returnMsg = result.Errors.FirstOrDefault().Description;
                        enqu.returnMsg = "Passwords must have at least 6 characters,one Special,one lowercase and one uppercase character.";
                        return enqu;
                    }
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