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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using CommonLibrary;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.AspNetCore.Hosting;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel;
using DomainModel.Model.com.vapstech.MobileApp;

namespace WebApplication1.Services
{
    public class StaffLoginImpl : Interfaces.StaffLoginInterface
    {
        private static ConcurrentDictionary<string, StaffLoginDTO> _pgmod =
        new ConcurrentDictionary<string, StaffLoginDTO>();

        private readonly IHostingEnvironment _hostingEnvironment;
        public StaffLoginContext _StaffLoginContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public DomainModelMsSqlServerContext _context;
        //private readonly UserManager<ApplicationUserRole> _userManagerRole;
        public StaffLoginImpl(StaffLoginContext StaffLoginContext, UserManager<ApplicationUser> userManager, DomainModelMsSqlServerContext context, IHostingEnvironment hostingEnvironment)
        {
            _StaffLoginContext = StaffLoginContext;
            _userManager = userManager;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            // _userManagerRole = userManagerRole;
        }

        public StaffLoginDTO getmoduledet(StaffLoginDTO stafflogin)
        {
            // StaffLoginDTO stafflogin = new StaffLoginDTO();
            try
            {
                var rolelist = _StaffLoginContext.MasterRoleType.Where(t => t.IVRMRT_Id == stafflogin.roleId).ToList();

                if (rolelist[0].IVRMRT_Role.Equals("Super Admin", StringComparison.OrdinalIgnoreCase))
                {
                    List<Institution> allinstitution = new List<Institution>();
                    allinstitution = _StaffLoginContext.institution.Where(t => t.MI_ActiveFlag == 1).ToList();
                    stafflogin.fillinstitution = allinstitution.ToArray();

                    List<Organisation> organization = new List<Organisation>();
                    organization = _StaffLoginContext.Organisation.Where(t => t.MO_ActiveFlag == 1).ToList();
                    stafflogin.fillorganisation = organization.ToArray();

                    List<MasterRoleType> allroles = new List<MasterRoleType>();
                    allroles = _StaffLoginContext.masterRoleType.Where(t => t.flag != "N").ToList();
                    stafflogin.fillroletype = allroles.ToArray();
                }
                else if (rolelist[0].flag == "A" || rolelist[0].flag == "U")
                {
                    List<Institution> allinstitution = new List<Institution>();
                    allinstitution = _StaffLoginContext.institution.Where(t => t.MI_Id == stafflogin.MI_Id).ToList();
                    stafflogin.fillinstitution = allinstitution.ToArray();

                    List<MasterRoleType> allroles = new List<MasterRoleType>();
                    allroles = _StaffLoginContext.masterRoleType.Where(t => t.flag == "S" || t.flag == "U").ToList();
                    stafflogin.fillroletype = allroles.ToArray();

                }
                else if (rolelist[0].IVRMRT_Role == "Multi Admin")
                {
                    var Mo_id_list = _StaffLoginContext.institution.Where(d => d.MI_Id == stafflogin.MI_Id).Select(d => d.MO_Id).ToList();

                    List<Institution> allinstitution = new List<Institution>();
                    allinstitution = _StaffLoginContext.institution.Where(t => Mo_id_list.Contains(t.MO_Id)).ToList();
                    stafflogin.fillinstitution = allinstitution.ToArray();
                }

                List<MasterModule> allmodules = new List<MasterModule>();
                allmodules = _StaffLoginContext.masterModule.Where(m => m.Module_ActiveFlag == 1).ToList();
                stafflogin.fillmodule = allmodules.ToArray();

                List<MasterPage> allpages = new List<MasterPage>();
                allpages = _StaffLoginContext.masterPage.Where(p => p.IVRMP_TemplateFlag == true).ToList();
                stafflogin.showgrid1 = allpages.ToArray();



            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stafflogin;
        }

        //public StaffLoginDTO updateusername(StaffLoginDTO pgmod)
        //{

        //    return pgmod;
        //}

        public async Task<StaffLoginDTO> updateusername(StaffLoginDTO enqu)
        {
            enqu.returnMsg = "";
            ApplicationUser user = new ApplicationUser();
            ApplicationUser newuser = new ApplicationUser();
            try
            {
                if (enqu.newuser != "")
                {
                    user = await _userManager.FindByNameAsync(Convert.ToString(enqu.curuser));

                    if (user != null)
                    {
                        newuser = await _userManager.FindByNameAsync(Convert.ToString(enqu.newuser));
                        if (newuser == null)
                        {
                            _context.Database.ExecuteSqlCommand("Update_User_Name @p0,@p1", user.Id, enqu.newuser);
                            enqu.returnMsg = "Success";
                        }
                        else
                        {
                            enqu.returnMsg = "User already Exists!!";
                        }
                    }
                    else
                    {
                        enqu.returnMsg = "User Not Exists!!";
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

        public async Task<StaffLoginDTO> getpagedetails(StaffLoginDTO pgmod)
        {
            StaffLoginDTO staffdto = new StaffLoginDTO();
            try
            {
                //var savedgrid=new long;

                //var moduleiid  = new lon;

                List<long> moduleiid = new List<long>();

                List<long> savedgrid = new List<long>();

                List<long> institutionlist = new List<long>();

                for (int i = 0; i < pgmod.TempararyArrayList.Length; i++)
                {
                    moduleiid.Add(Convert.ToInt64(pgmod.TempararyArrayList[i].ivrmM_Id));
                }

                if (pgmod.multipleinsti != null && pgmod.multipleinsti.Length > 0)
                {
                    for (int i = 0; i < pgmod.multipleinsti.Length; i++)
                    {
                        institutionlist.Add(Convert.ToInt64(pgmod.multipleinsti[i].mi_id));
                    }
                }





                ApplicationUser user = new ApplicationUser();
                user = await _userManager.FindByNameAsync(pgmod.User_Name);

                if (user != null)
                {
                    if (pgmod.multipleinsti != null && pgmod.multipleinsti.Length > 0)
                    {
                        savedgrid = (from a in _StaffLoginContext.UserLoginPrivileges
                                     from b in _StaffLoginContext.institution_Module_Page
                                     from c in _StaffLoginContext.masterPage
                                     from d in _StaffLoginContext.institution_Module
                                     from e in _StaffLoginContext.masterModule
                                     where (a.IVRMIMP_Id == b.IVRMIMP_Id && b.IVRMIM_Id == d.IVRMIM_Id && a.IVRMIMP_Id == b.IVRMIMP_Id &&
                                     b.IVRMP_Id == c.IVRMP_Id && e.IVRMM_Id == d.IVRMM_Id && a.MI_Id == d.MI_Id &&
                                    institutionlist.Contains(d.MI_Id)
                                      && a.Id == user.Id && moduleiid.Contains(e.IVRMM_Id)
                                     )
                                     select new StaffLoginDTO
                                     {
                                         IVRMMP_PageName = c.IVRMMP_PageName,
                                         IVRMSTAUP_Id = a.IVRMULP_Id,
                                         IVRMRP_AddFlag = a.IVRMSTUUP_AddFlag,
                                         IVRMRP_DeleteFlag = a.IVRMSTUUP_DeleteFlag,
                                         IVRMRP_UpdateFlag = a.IVRMSTUUP_UpdateFlag,
                                         IVRMRP_ProcessFlag = a.IVRMSTUUP_ProcessFlag,
                                         IVRMRP_ReportFlag = a.IVRMSTUUP_ReportFlag,
                                         IVRMIMP_Id = b.IVRMIMP_Id,
                                         IVRMP_Id = c.IVRMP_Id
                                     }
              ).Select(m => m.IVRMIMP_Id).ToList();



                        staffdto.savedgrid = (from a in _StaffLoginContext.UserLoginPrivileges
                                              from b in _StaffLoginContext.institution_Module_Page
                                              from c in _StaffLoginContext.masterPage
                                              from d in _StaffLoginContext.institution_Module
                                              from e in _StaffLoginContext.masterModule
                                              where (a.IVRMIMP_Id == b.IVRMIMP_Id && b.IVRMIM_Id == d.IVRMIM_Id && a.IVRMIMP_Id == b.IVRMIMP_Id &&
                                              b.IVRMP_Id == c.IVRMP_Id && e.IVRMM_Id == d.IVRMM_Id &&
                                               institutionlist.Contains(a.MI_Id) && a.Id == user.Id &&
                                             moduleiid.Contains(e.IVRMM_Id))
                                              select new StaffLoginDTO
                                              {
                                                  IVRMMP_PageName = c.IVRMMP_PageName,
                                                  IVRMSTAUP_Id = a.IVRMULP_Id,
                                                  IVRMRP_AddFlag = a.IVRMSTUUP_AddFlag,
                                                  IVRMRP_DeleteFlag = a.IVRMSTUUP_DeleteFlag,
                                                  IVRMRP_UpdateFlag = a.IVRMSTUUP_UpdateFlag,
                                                  IVRMRP_ProcessFlag = a.IVRMSTUUP_ProcessFlag,
                                                  IVRMRP_ReportFlag = a.IVRMSTUUP_ReportFlag,
                                                  IVRMIMP_Id = b.IVRMIMP_Id,
                                                  IVRMP_Id = c.IVRMP_Id
                                              }
                        ).ToArray();
                    }
                    else
                    {
                        savedgrid = (from a in _StaffLoginContext.UserLoginPrivileges
                                     from b in _StaffLoginContext.institution_Module_Page
                                     from c in _StaffLoginContext.masterPage
                                     from d in _StaffLoginContext.institution_Module
                                     from e in _StaffLoginContext.masterModule
                                     where (a.IVRMIMP_Id == b.IVRMIMP_Id && b.IVRMIM_Id == d.IVRMIM_Id && a.IVRMIMP_Id == b.IVRMIMP_Id &&
                                     b.IVRMP_Id == c.IVRMP_Id && e.IVRMM_Id == d.IVRMM_Id && a.MI_Id == d.MI_Id &&
                                    d.MI_Id == pgmod.MI_Id
                                      && a.Id == user.Id && moduleiid.Contains(e.IVRMM_Id)
                                     )
                                     select new StaffLoginDTO
                                     {
                                         IVRMMP_PageName = c.IVRMMP_PageName,
                                         IVRMSTAUP_Id = a.IVRMULP_Id,
                                         IVRMRP_AddFlag = a.IVRMSTUUP_AddFlag,
                                         IVRMRP_DeleteFlag = a.IVRMSTUUP_DeleteFlag,
                                         IVRMRP_UpdateFlag = a.IVRMSTUUP_UpdateFlag,
                                         IVRMRP_ProcessFlag = a.IVRMSTUUP_ProcessFlag,
                                         IVRMRP_ReportFlag = a.IVRMSTUUP_ReportFlag,
                                         IVRMIMP_Id = b.IVRMIMP_Id,
                                         IVRMP_Id = c.IVRMP_Id
                                     }
              ).Select(m => m.IVRMIMP_Id).ToList();



                        staffdto.savedgrid = (from a in _StaffLoginContext.UserLoginPrivileges
                                              from b in _StaffLoginContext.institution_Module_Page
                                              from c in _StaffLoginContext.masterPage
                                              from d in _StaffLoginContext.institution_Module
                                              from e in _StaffLoginContext.masterModule
                                              where (a.IVRMIMP_Id == b.IVRMIMP_Id && b.IVRMIM_Id == d.IVRMIM_Id && a.IVRMIMP_Id == b.IVRMIMP_Id &&
                                              b.IVRMP_Id == c.IVRMP_Id && e.IVRMM_Id == d.IVRMM_Id &&
                                               a.MI_Id == pgmod.MI_Id && a.Id == user.Id &&
                                             moduleiid.Contains(e.IVRMM_Id))
                                              select new StaffLoginDTO
                                              {
                                                  IVRMMP_PageName = c.IVRMMP_PageName,
                                                  IVRMSTAUP_Id = a.IVRMULP_Id,
                                                  IVRMRP_AddFlag = a.IVRMSTUUP_AddFlag,
                                                  IVRMRP_DeleteFlag = a.IVRMSTUUP_DeleteFlag,
                                                  IVRMRP_UpdateFlag = a.IVRMSTUUP_UpdateFlag,
                                                  IVRMRP_ProcessFlag = a.IVRMSTUUP_ProcessFlag,
                                                  IVRMRP_ReportFlag = a.IVRMSTUUP_ReportFlag,
                                                  IVRMIMP_Id = b.IVRMIMP_Id,
                                                  IVRMP_Id = c.IVRMP_Id
                                              }
                        ).ToArray();
                    }


                }

                if (pgmod.multipleinsti != null && pgmod.multipleinsti.Length > 0)
                {


                    //                   staffdto.showgrid1 = (from a in _StaffLoginContext.institutionRolePrivileges
                    //                                         from b in _StaffLoginContext.institution_Module_Page
                    //                                         from c in _StaffLoginContext.masterRoleType
                    //                                         from d in _StaffLoginContext.masterPage
                    //                                         from e in _StaffLoginContext.institution_Module
                    //                                         from i in _StaffLoginContext.institution 
                    //                                         from g in _StaffLoginContext.masterModule
                    //                                         where (b.IVRMIMP_Id == a.IVRMIMP_Id && c.IVRMRT_Id == a.IVRMRT_Id &&
                    //                                         d.IVRMP_Id == b.IVRMP_Id && b.IVRMIM_Id == e.IVRMIM_Id && e.MI_Id==i.MI_Id && e.IVRMM_Id==g.IVRMM_Id &&
                    //                                         a.IVRMRT_Id == pgmod.IVRMRT_Id && moduleiid.Contains(e.IVRMM_Id) && institutionlist.Contains(e.MI_Id) 

                    //                                         )
                    //                                         select new StaffLoginDTO
                    //                                         {
                    //                                             IVRMMP_PageName = d.IVRMMP_PageName,
                    //                                             IVRMIMP_Id = a.IVRMIMP_Id,
                    //                                             IVRMRP_AddFlag = a.IVRMIRP_AddFlag,
                    //                                             IVRMRP_DeleteFlag = a.IVRMIRP_DeleteFlag,
                    //                                             IVRMRP_UpdateFlag = a.IVRMIRP_UpdateFlag,
                    //                                             IVRMRP_ProcessFlag = a.IVRMIRP_ProcessFlag,
                    //                                             IVRMRP_ReportFlag = a.IVRMIRP_ReportFlag,
                    //                                             IVRMP_Id = d.IVRMP_Id,
                    //                                             ivrmM_ModuleName=g.IVRMM_ModuleName,
                    //                                             intname=i.MI_Name

                    //                                         }
                    //).ToArray();
                    staffdto.showgrid1 = (from a in _StaffLoginContext.institutionRolePrivileges
                                          from b in _StaffLoginContext.institution_Module_Page
                                          from c in _StaffLoginContext.masterRoleType
                                          from d in _StaffLoginContext.masterPage
                                          from e in _StaffLoginContext.institution_Module
                                          from g in _StaffLoginContext.masterModule
                                          from i in _StaffLoginContext.institution
                                          where (b.IVRMIMP_Id == a.IVRMIMP_Id && c.IVRMRT_Id == a.IVRMRT_Id &&
                                          d.IVRMP_Id == b.IVRMP_Id && b.IVRMIM_Id == e.IVRMIM_Id && e.IVRMM_Id == g.IVRMM_Id && e.MI_Id == i.MI_Id &&
                                          a.IVRMRT_Id == pgmod.IVRMRT_Id && moduleiid.Contains(e.IVRMM_Id)
                                          && institutionlist.Contains(e.MI_Id) && !savedgrid.Contains(a.IVRMIMP_Id)
                                          )
                                          select new StaffLoginDTO
                                          {
                                              IVRMMP_PageName = d.IVRMMP_PageName,
                                              IVRMIMP_Id = a.IVRMIMP_Id,
                                              IVRMRP_AddFlag = a.IVRMIRP_AddFlag,
                                              IVRMRP_DeleteFlag = a.IVRMIRP_DeleteFlag,
                                              IVRMRP_UpdateFlag = a.IVRMIRP_UpdateFlag,
                                              IVRMRP_ProcessFlag = a.IVRMIRP_ProcessFlag,
                                              IVRMRP_ReportFlag = a.IVRMIRP_ReportFlag,
                                              IVRMP_Id = d.IVRMP_Id,
                                              ivrmM_ModuleName = g.IVRMM_ModuleName,
                                              intname = i.MI_Name
                                          }
           ).ToArray();
                }
                else
                {
                    //                   staffdto.showgrid1 = (from a in _StaffLoginContext.institutionRolePrivileges
                    //                                         from b in _StaffLoginContext.institution_Module_Page
                    //                                         from c in _StaffLoginContext.masterRoleType
                    //                                         from d in _StaffLoginContext.masterPage
                    //                                         from e in _StaffLoginContext.institution_Module
                    //                                         from i in _StaffLoginContext.institution
                    //                                         from g in _StaffLoginContext.masterModule
                    //                                         where (b.IVRMIMP_Id == a.IVRMIMP_Id && c.IVRMRT_Id == a.IVRMRT_Id &&
                    //                                         d.IVRMP_Id == b.IVRMP_Id && b.IVRMIM_Id == e.IVRMIM_Id && e.MI_Id == i.MI_Id && e.IVRMM_Id == g.IVRMM_Id &&
                    //                                         a.IVRMRT_Id == pgmod.IVRMRT_Id && moduleiid.Contains(e.IVRMM_Id)  && e.MI_Id == pgmod.MI_Id
                    //                                         )
                    //                                         select new StaffLoginDTO
                    //                                         {
                    //                                             IVRMMP_PageName = d.IVRMMP_PageName,
                    //                                             IVRMIMP_Id = a.IVRMIMP_Id,
                    //                                             IVRMRP_AddFlag = a.IVRMIRP_AddFlag,
                    //                                             IVRMRP_DeleteFlag = a.IVRMIRP_DeleteFlag,
                    //                                             IVRMRP_UpdateFlag = a.IVRMIRP_UpdateFlag,
                    //                                             IVRMRP_ProcessFlag = a.IVRMIRP_ProcessFlag,
                    //                                             IVRMRP_ReportFlag = a.IVRMIRP_ReportFlag,
                    //                                             IVRMP_Id = d.IVRMP_Id,
                    //                                             ivrmM_ModuleName = g.IVRMM_ModuleName,
                    //                                             intname = i.MI_Name
                    //                                         }
                    //).ToArray();
                    staffdto.showgrid1 = (from a in _StaffLoginContext.institutionRolePrivileges
                                          from b in _StaffLoginContext.institution_Module_Page
                                          from c in _StaffLoginContext.masterRoleType
                                          from d in _StaffLoginContext.masterPage
                                          from e in _StaffLoginContext.institution_Module
                                          from g in _StaffLoginContext.masterModule
                                          from i in _StaffLoginContext.institution
                                          where (b.IVRMIMP_Id == a.IVRMIMP_Id && c.IVRMRT_Id == a.IVRMRT_Id &&
                                          d.IVRMP_Id == b.IVRMP_Id && b.IVRMIM_Id == e.IVRMIM_Id && e.IVRMM_Id == g.IVRMM_Id && e.MI_Id == i.MI_Id &&
                                          a.IVRMRT_Id == pgmod.IVRMRT_Id && moduleiid.Contains(e.IVRMM_Id)
                                          && e.MI_Id == pgmod.MI_Id && !savedgrid.Contains(a.IVRMIMP_Id)
                                          )
                                          select new StaffLoginDTO
                                          {
                                              IVRMMP_PageName = d.IVRMMP_PageName,
                                              IVRMIMP_Id = a.IVRMIMP_Id,
                                              IVRMRP_AddFlag = a.IVRMIRP_AddFlag,
                                              IVRMRP_DeleteFlag = a.IVRMIRP_DeleteFlag,
                                              IVRMRP_UpdateFlag = a.IVRMIRP_UpdateFlag,
                                              IVRMRP_ProcessFlag = a.IVRMIRP_ProcessFlag,
                                              IVRMRP_ReportFlag = a.IVRMIRP_ReportFlag,
                                              IVRMP_Id = d.IVRMP_Id,
                                              ivrmM_ModuleName = g.IVRMM_ModuleName,
                                              intname = i.MI_Name
                                          }
).ToArray();
                }




                if (staffdto.showgrid1.Length == 0)
                {
                    staffdto.returnval = "Institution level Module and Page is not mapped";
                    //List<MasterPage> allpages = new List<MasterPage>();
                    //allpages = _StaffLoginContext.masterPage.ToList();
                    //staffdto.showgrid1 = allpages.ToArray();
                }

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return staffdto;
        }

        public StaffLoginDTO getstudata(StaffLoginDTO data)
        {
            try
            {
                List<long> moduleidsss = new List<long>();

                List<long> savedgrid = new List<long>();
                List<long> catids = new List<long>();

                List<ApplicationUser> username = new List<ApplicationUser>();

                List<MasterRoleType> userrolename = new List<MasterRoleType>();

                data.singleemployee = (from a in _StaffLoginContext.masterStaff
                                       from b in _StaffLoginContext.Staff_User_Login
                                       from c in _StaffLoginContext.ApplicationUser
                                       where (a.HRME_Id == b.Emp_Code && b.Id == c.Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_Id == data.IVRMSTAUL_Id
                                    )
                                       select new StaffLoginDTO
                                       {
                                           HRME_Id = a.HRME_Id,
                                           stafname = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                           HRME_EmployeeCode = a.HRME_EmployeeCode,
                                           User_Name = c.UserName,
                                       }
       ).Distinct().ToArray();



                if (data.singleemployee.Length > 0)
                {
                    username = (from a in _StaffLoginContext.masterStaff
                                from b in _StaffLoginContext.Staff_User_Login
                                from c in _StaffLoginContext.ApplicationUser
                                where (a.HRME_Id == b.Emp_Code && b.Id == c.Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_Id == data.IVRMSTAUL_Id
                             )
                                select c

).ToList();

                    userrolename = (from a in _StaffLoginContext.masterStaff
                                    from b in _StaffLoginContext.Staff_User_Login
                                    from c in _StaffLoginContext.ApplicationUser
                                    from d in _StaffLoginContext.masterRoleType
                                    from f in _StaffLoginContext.appUserRole
                                    where (a.HRME_Id == b.Emp_Code && b.Id == c.Id && c.Id == f.UserId && d.IVRMRT_Id == f.RoleTypeId && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_Id == data.IVRMSTAUL_Id
                                 )
                                    select d

).ToList();

                    data.User_Name = username.FirstOrDefault().UserName;
                    data.usrerolename = userrolename.FirstOrDefault().IVRMRT_Role;

                    //catids = _StaffLoginContext.staffLoginDMO.Where(t => t.IVRMSTAUL_Id == data.IVRMSTAUL_Id && t.User_Name == username.FirstOrDefault().UserName && t.MI_Id == data.MI_Id && t.IVRMRT_Id == data.IVRMRT_Id).Distinct().Select(a => a.amc_id).ToList();



                    moduleidsss = (from a in _StaffLoginContext.UserLoginPrivileges
                                   from b in _StaffLoginContext.institution_Module_Page
                                   from c in _StaffLoginContext.masterPage
                                   from d in _StaffLoginContext.institution_Module
                                   from e in _StaffLoginContext.masterModule
                                   where (a.IVRMIMP_Id == b.IVRMIMP_Id && b.IVRMIM_Id == d.IVRMIM_Id && a.IVRMIMP_Id == b.IVRMIMP_Id &&
                                   b.IVRMP_Id == c.IVRMP_Id && e.IVRMM_Id == d.IVRMM_Id && a.MI_Id == d.MI_Id &&
                                   a.MI_Id == data.MI_Id && a.Id == username.FirstOrDefault().Id
                                 )
                                   select e
                  ).Distinct().Select(a => a.IVRMM_Id).ToList();



                    List<MasterModule> allmodules = new List<MasterModule>();
                    allmodules = _StaffLoginContext.masterModule.Where(m => m.Module_ActiveFlag == 1 && moduleidsss.Contains(m.IVRMM_Id)).ToList();
                    data.moduleexistid = allmodules.ToArray();

                    List<MasterCategory> allcategory = new List<MasterCategory>();
                    allcategory = _StaffLoginContext.masterCategory.Where(t => t.MI_Id == data.MI_Id && catids.Contains(t.AMC_Id)).ToList();
                    data.categaryids = allcategory.ToArray();

                    savedgrid = (from a in _StaffLoginContext.UserLoginPrivileges
                                 from b in _StaffLoginContext.institution_Module_Page
                                 from c in _StaffLoginContext.masterPage
                                 from d in _StaffLoginContext.institution_Module
                                 from e in _StaffLoginContext.masterModule
                                 where (a.IVRMIMP_Id == b.IVRMIMP_Id && b.IVRMIM_Id == d.IVRMIM_Id && a.IVRMIMP_Id == b.IVRMIMP_Id &&
                                 b.IVRMP_Id == c.IVRMP_Id && e.IVRMM_Id == d.IVRMM_Id && a.MI_Id == d.MI_Id &&
                                 a.MI_Id == data.MI_Id && a.Id == username.FirstOrDefault().Id &&
                                 moduleidsss.Contains(e.IVRMM_Id))
                                 select new StaffLoginDTO
                                 {
                                     IVRMMP_PageName = c.IVRMMP_PageName,
                                     IVRMSTAUP_Id = a.IVRMULP_Id,
                                     IVRMRP_AddFlag = a.IVRMSTUUP_ActiveFlag,
                                     IVRMRP_DeleteFlag = a.IVRMSTUUP_DeleteFlag,
                                     IVRMRP_UpdateFlag = a.IVRMSTUUP_UpdateFlag,
                                     IVRMRP_ProcessFlag = a.IVRMSTUUP_ProcessFlag,
                                     IVRMRP_ReportFlag = a.IVRMSTUUP_ReportFlag,
                                     IVRMIMP_Id = b.IVRMIMP_Id
                                 }
                    ).Select(m => m.IVRMIMP_Id).ToList();

                    data.showgrid1 = (from a in _StaffLoginContext.institutionRolePrivileges
                                      from b in _StaffLoginContext.institution_Module_Page
                                      from c in _StaffLoginContext.masterRoleType
                                      from d in _StaffLoginContext.masterPage
                                      from e in _StaffLoginContext.institution_Module
                                      from i in _StaffLoginContext.institution
                                      from g in _StaffLoginContext.masterModule
                                      where (b.IVRMIMP_Id == a.IVRMIMP_Id && c.IVRMRT_Id == a.IVRMRT_Id &&
                                      d.IVRMP_Id == b.IVRMP_Id && b.IVRMIM_Id == e.IVRMIM_Id && i.MI_Id == e.MI_Id && e.IVRMM_Id == g.IVRMM_Id &&
                                      a.IVRMRT_Id == data.IVRMRT_Id && moduleidsss.Contains(e.IVRMM_Id)
                                      && e.MI_Id == data.MI_Id && !savedgrid.Contains(a.IVRMIMP_Id)

                                      )
                                      select new StaffLoginDTO
                                      {
                                          IVRMMP_PageName = d.IVRMMP_PageName,
                                          IVRMIMP_Id = a.IVRMIMP_Id,
                                          IVRMRP_AddFlag = a.IVRMIRP_AddFlag,
                                          IVRMRP_DeleteFlag = a.IVRMIRP_DeleteFlag,
                                          IVRMRP_UpdateFlag = a.IVRMIRP_UpdateFlag,
                                          IVRMRP_ProcessFlag = a.IVRMIRP_ProcessFlag,
                                          IVRMRP_ReportFlag = a.IVRMIRP_ReportFlag,
                                          ivrmM_ModuleName = g.IVRMM_ModuleName,
                                          intname = i.MI_Name
                                      }
                            ).ToArray();




                    data.savedgrid = (from a in _StaffLoginContext.UserLoginPrivileges
                                      from b in _StaffLoginContext.institution_Module_Page
                                      from c in _StaffLoginContext.masterPage
                                      from d in _StaffLoginContext.institution_Module
                                      from e in _StaffLoginContext.masterModule
                                      where (a.IVRMIMP_Id == b.IVRMIMP_Id && b.IVRMIM_Id == d.IVRMIM_Id && a.IVRMIMP_Id == b.IVRMIMP_Id &&
                                      b.IVRMP_Id == c.IVRMP_Id && e.IVRMM_Id == d.IVRMM_Id && a.MI_Id == d.MI_Id &&
                                      a.MI_Id == data.MI_Id && a.Id == username.FirstOrDefault().Id &&
                                     moduleidsss.Contains(e.IVRMM_Id)
                                     )
                                      select new StaffLoginDTO
                                      {
                                          IVRMMP_PageName = c.IVRMMP_PageName,
                                          IVRMSTAUP_Id = a.IVRMULP_Id,
                                          IVRMRP_AddFlag = a.IVRMSTUUP_AddFlag,
                                          IVRMRP_DeleteFlag = a.IVRMSTUUP_DeleteFlag,
                                          IVRMRP_UpdateFlag = a.IVRMSTUUP_UpdateFlag,
                                          IVRMRP_ProcessFlag = a.IVRMSTUUP_ProcessFlag,
                                          IVRMRP_ReportFlag = a.IVRMSTUUP_ReportFlag,
                                          IVRMIMP_Id = b.IVRMIMP_Id
                                      }
                        ).ToArray();



                    List<long> pageist = new List<long>();
                    pageist = (from a in _StaffLoginContext.IVRM_MobileApp_Page
                               from b in _StaffLoginContext.IVRM_Role_MobileApp_Privileges
                               from c in _StaffLoginContext.masterRoleType
                               from d in _StaffLoginContext.IVRM_User_MobileApp_Login_Privileges
                               where (a.IVRMMAP_Id == b.IVRMMAP_Id && b.IVRMRT_Id == c.IVRMRT_Id && d.IVRMMAP_Id == b.IVRMMAP_Id && c.IVRMRT_Id == data.IVRMRT_Id && d.IVRMUL_Id == username.FirstOrDefault().Id && b.MI_ID == data.MI_Id)
                               select a).Distinct().Select(a => a.IVRMMAP_Id).ToList();


                    data.previousgrid = (from a in _StaffLoginContext.IVRM_MobileApp_Page
                                         from b in _StaffLoginContext.IVRM_Role_MobileApp_Privileges
                                         from c in _StaffLoginContext.masterRoleType
                                         from d in _StaffLoginContext.IVRM_User_MobileApp_Login_Privileges
                                         where (a.IVRMMAP_Id == b.IVRMMAP_Id && b.IVRMRT_Id == c.IVRMRT_Id && d.IVRMMAP_Id == b.IVRMMAP_Id && c.IVRMRT_Id == data.IVRMRT_Id && d.IVRMUL_Id == username.FirstOrDefault().Id && b.MI_ID == data.MI_Id)
                                         select new MasterRolePreviledgeDTO
                                         {
                                             IVRMMAP_AppPageName = a.IVRMMAP_AppPageName,
                                             ivrmrT_Role = c.IVRMRT_Role,
                                             IVRMR_Id = c.IVRMR_Id,
                                             IVRMRP_Id = b.IVRMRMAP_Id,
                                             IVRMMAP_Id = a.IVRMMAP_Id,
                                             IVRMUMALP_Id = d.IVRMUMALP_Id,
                                             IVRMUMALP_AddFlg = d.IVRMUMALP_AddFlg,
                                             IVRMUMALP_UpdateFlg = d.IVRMUMALP_UpdateFlg,
                                             IVRMUMALP_DeleteFlg = d.IVRMUMALP_DeleteFlg
                                         }
             ).ToArray();

                    if (pageist.Count() > 0)
                    {
                        data.thirdgriddatamobile = (from a in _StaffLoginContext.IVRM_MobileApp_Page
                                                    from b in _StaffLoginContext.IVRM_Role_MobileApp_Privileges
                                                    from c in _StaffLoginContext.masterRoleType
                                                    where (a.IVRMMAP_Id == b.IVRMMAP_Id && b.IVRMRT_Id == c.IVRMRT_Id && !pageist.Contains(b.IVRMMAP_Id) && c.IVRMRT_Id == data.IVRMRT_Id && b.MI_ID == data.MI_Id)
                                                    select new MasterRolePreviledgeDTO
                                                    {
                                                        IVRMMAP_AppPageName = a.IVRMMAP_AppPageName,
                                                        ivrmrT_Role = c.IVRMRT_Role,
                                                        IVRMR_Id = c.IVRMR_Id,
                                                        IVRMRP_Id = b.IVRMRMAP_Id,
                                                        IVRMMAP_Id = a.IVRMMAP_Id,
                                                        IVRMMAP_AddFlg = b.IVRMMAP_AddFlg,
                                                        IVRMMAP_UpdateFlg = b.IVRMMAP_UpdateFlg,
                                                        IVRMMAP_DeleteFlg = b.IVRMMAP_DeleteFlg
                                                    }
         ).ToArray();

                    }
                    else
                    {
                        data.thirdgriddatamobile = (from a in _StaffLoginContext.IVRM_MobileApp_Page
                                                    from b in _StaffLoginContext.IVRM_Role_MobileApp_Privileges
                                                    from c in _StaffLoginContext.masterRoleType
                                                    where (a.IVRMMAP_Id == b.IVRMMAP_Id && b.IVRMRT_Id == c.IVRMRT_Id && c.IVRMRT_Id == data.IVRMRT_Id && b.MI_ID == data.MI_Id)
                                                    select new MasterRolePreviledgeDTO
                                                    {
                                                        IVRMMAP_AppPageName = a.IVRMMAP_AppPageName,
                                                        ivrmrT_Role = c.IVRMRT_Role,
                                                        IVRMR_Id = c.IVRMR_Id,
                                                        IVRMRP_Id = b.IVRMRMAP_Id,
                                                        IVRMMAP_Id = a.IVRMMAP_Id,
                                                        IVRMMAP_AddFlg = b.IVRMMAP_AddFlg,
                                                        IVRMMAP_UpdateFlg = b.IVRMMAP_UpdateFlg,
                                                        IVRMMAP_DeleteFlg = b.IVRMMAP_DeleteFlg
                                                    }
         ).ToArray();

                    }



                    if (data.showgrid1.Length == 0)
                    {
                        data.returnval = "Institution level Module and Page is not mapped";

                    }


                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<StaffLoginDTO> onchangeuser(StaffLoginDTO data)
        {
            try
            {
                List<long> moduleidsss = new List<long>();

                List<long> savedgrid = new List<long>();
                List<long> catids = new List<long>();
                List<long> miids = new List<long>();
                List<ApplicationUser> username = new List<ApplicationUser>();

                List<MasterRoleType> userrolename = new List<MasterRoleType>();

                ApplicationUser user = new ApplicationUser();
                user = await _userManager.FindByNameAsync(data.User_Name);

                if (data.multipleinsti != null && data.multipleinsti.Length > 1)
                {
                    for (int i = 0; i < data.multipleinsti.Length; i++)
                    {
                        miids.Add(data.multipleinsti[i].mi_id);
                    }
                }
                else
                {
                    miids.Add(data.MI_Id);
                }

                if (user != null)
                {
                    data.User_Name_exact = user.UserName;
                    //data.usrerolenameexact= data.rolenamess;


                    var d1 = (from a in _StaffLoginContext.appUserRole
                              from c in _StaffLoginContext.masterRoleType
                              where (a.RoleTypeId == c.IVRMRT_Id && a.UserId == user.Id
                              )
                              select c).ToArray();

                    data.usrerolenameexact = d1.FirstOrDefault().IVRMRT_Role;

                    if (miids.Count() > 0)
                    {
                        moduleidsss = (from a in _StaffLoginContext.UserLoginPrivileges
                                       from b in _StaffLoginContext.institution_Module_Page
                                       from c in _StaffLoginContext.masterPage
                                       from d in _StaffLoginContext.institution_Module
                                       from e in _StaffLoginContext.masterModule
                                       where (a.IVRMIMP_Id == b.IVRMIMP_Id && b.IVRMIM_Id == d.IVRMIM_Id && a.MI_Id == d.MI_Id &&
                                       b.IVRMP_Id == c.IVRMP_Id && e.IVRMM_Id == d.IVRMM_Id &&
                                      miids.Contains(a.MI_Id) && a.IVRMIMP_Id == b.IVRMIMP_Id &&
                                       a.Id == user.Id)
                                       select e
                                   ).Distinct().Select(a => a.IVRMM_Id).ToList();
                    }
                    //         else
                    //         {
                    //             moduleidsss = (from a in _StaffLoginContext.UserLoginPrivileges
                    //                            from b in _StaffLoginContext.institution_Module_Page
                    //                            from c in _StaffLoginContext.masterPage
                    //                            from d in _StaffLoginContext.institution_Module
                    //                            from e in _StaffLoginContext.masterModule
                    //                            where (a.IVRMIMP_Id == b.IVRMIMP_Id && b.IVRMIM_Id == d.IVRMIM_Id && a.MI_Id == d.MI_Id &&
                    //                            b.IVRMP_Id == c.IVRMP_Id && e.IVRMM_Id == d.IVRMM_Id &&
                    //                            a.MI_Id == data.MI_Id && a.IVRMIMP_Id == b.IVRMIMP_Id &&
                    //                            a.Id == user.Id)
                    //                            select e
                    //).Distinct().Select(a => a.IVRMM_Id).ToList();
                    //         }




                    List<MasterModule> allmodules = new List<MasterModule>();
                    allmodules = _StaffLoginContext.masterModule.Where(m => m.Module_ActiveFlag == 1 && moduleidsss.Contains(m.IVRMM_Id)).ToList();
                    data.moduleexistid = allmodules.ToArray();

                    List<MasterCategory> allcategory = new List<MasterCategory>();
                    allcategory = _StaffLoginContext.masterCategory.Where(t => miids.Contains(t.MI_Id) && catids.Contains(t.AMC_Id)).ToList();
                    data.categaryids = allcategory.ToArray();

                    savedgrid = (from a in _StaffLoginContext.UserLoginPrivileges
                                 from b in _StaffLoginContext.institution_Module_Page
                                 from c in _StaffLoginContext.masterPage
                                 from d in _StaffLoginContext.institution_Module
                                 from e in _StaffLoginContext.masterModule
                                 where (a.IVRMIMP_Id == b.IVRMIMP_Id && b.IVRMIM_Id == d.IVRMIM_Id &&
                                 b.IVRMP_Id == c.IVRMP_Id && e.IVRMM_Id == d.IVRMM_Id &&
                                 miids.Contains(d.MI_Id)
                                  && a.Id == user.Id && moduleidsss.Contains(e.IVRMM_Id)
                                 )
                                 select new StaffLoginDTO
                                 {
                                     IVRMMP_PageName = c.IVRMMP_PageName,
                                     IVRMSTAUP_Id = a.IVRMULP_Id,
                                     IVRMRP_AddFlag = a.IVRMSTUUP_AddFlag,
                                     IVRMRP_DeleteFlag = a.IVRMSTUUP_DeleteFlag,
                                     IVRMRP_UpdateFlag = a.IVRMSTUUP_UpdateFlag,
                                     IVRMRP_ProcessFlag = a.IVRMSTUUP_ProcessFlag,
                                     IVRMRP_ReportFlag = a.IVRMSTUUP_ReportFlag,
                                     IVRMIMP_Id = b.IVRMIMP_Id
                                 }
                ).Select(m => m.IVRMIMP_Id).ToList();





                    data.showgrid1 = (from a in _StaffLoginContext.institutionRolePrivileges
                                      from b in _StaffLoginContext.institution_Module_Page
                                      from c in _StaffLoginContext.masterRoleType
                                      from d in _StaffLoginContext.masterPage
                                      from e in _StaffLoginContext.institution_Module
                                      from g in _StaffLoginContext.masterModule
                                      from i in _StaffLoginContext.institution
                                      where (b.IVRMIMP_Id == a.IVRMIMP_Id && c.IVRMRT_Id == a.IVRMRT_Id &&
                                      d.IVRMP_Id == b.IVRMP_Id && b.IVRMIM_Id == e.IVRMIM_Id && e.IVRMM_Id == g.IVRMM_Id && e.MI_Id == i.MI_Id &&
                                      a.IVRMRT_Id == data.IVRMRT_Id && moduleidsss.Contains(e.IVRMM_Id)
                                      && miids.Contains(e.MI_Id) && !savedgrid.Contains(a.IVRMIMP_Id)
                                      )
                                      select new StaffLoginDTO
                                      {
                                          IVRMMP_PageName = d.IVRMMP_PageName,
                                          IVRMIMP_Id = a.IVRMIMP_Id,
                                          IVRMRP_AddFlag = a.IVRMIRP_AddFlag,
                                          IVRMRP_DeleteFlag = a.IVRMIRP_DeleteFlag,
                                          IVRMRP_UpdateFlag = a.IVRMIRP_UpdateFlag,
                                          IVRMRP_ProcessFlag = a.IVRMIRP_ProcessFlag,
                                          IVRMRP_ReportFlag = a.IVRMIRP_ReportFlag,
                                          IVRMP_Id = d.IVRMP_Id,
                                          ivrmM_ModuleName = g.IVRMM_ModuleName,
                                          intname = i.MI_Name
                                      }
                      ).ToArray();


                    List<Staff_User_Login> StaffloginCheck = new List<Staff_User_Login>();
                    StaffloginCheck = _StaffLoginContext.Staff_User_Login.Where(m => m.Id == user.Id).ToList();

                    if (StaffloginCheck.Count > 0)
                    {
                        data.savedgrid = (from a in _StaffLoginContext.UserLoginPrivileges
                                          from b in _StaffLoginContext.institution_Module_Page
                                          from c in _StaffLoginContext.masterPage
                                          from d in _StaffLoginContext.institution_Module
                                          from e in _StaffLoginContext.masterModule
                                          from f in _StaffLoginContext.Staff_User_Login
                                          from g in _StaffLoginContext.masterStaff
                                          where (a.IVRMIMP_Id == b.IVRMIMP_Id && b.IVRMIM_Id == d.IVRMIM_Id && a.IVRMIMP_Id == b.IVRMIMP_Id &&
                                          b.IVRMP_Id == c.IVRMP_Id && e.IVRMM_Id == d.IVRMM_Id && a.MI_Id == d.MI_Id &&
                                          miids.Contains(d.MI_Id) &&
                                          moduleidsss.Contains(e.IVRMM_Id) && a.Id == user.Id && f.Id == a.Id && f.Emp_Code == g.HRME_Id && miids.Contains(g.MI_Id)
                                          )
                                          select new StaffLoginDTO
                                          {
                                              IVRMMP_PageName = c.IVRMMP_PageName,
                                              IVRMSTAUP_Id = a.IVRMULP_Id,
                                              IVRMRP_AddFlag = a.IVRMSTUUP_AddFlag,
                                              IVRMRP_DeleteFlag = a.IVRMSTUUP_DeleteFlag,
                                              IVRMRP_UpdateFlag = a.IVRMSTUUP_UpdateFlag,
                                              IVRMRP_ProcessFlag = a.IVRMSTUUP_ProcessFlag,
                                              IVRMRP_ReportFlag = a.IVRMSTUUP_ReportFlag,
                                              IVRMIMP_Id = b.IVRMIMP_Id,
                                              IVRMP_Id = c.IVRMP_Id,
                                              stafname = ((g.HRME_EmployeeFirstName == null || g.HRME_EmployeeFirstName == "" ? "" : g.HRME_EmployeeFirstName) + (g.HRME_EmployeeMiddleName == null || g.HRME_EmployeeMiddleName == "" || g.HRME_EmployeeMiddleName == "0" ? "" : " " + g.HRME_EmployeeMiddleName) + (g.HRME_EmployeeLastName == null || g.HRME_EmployeeLastName == "" || g.HRME_EmployeeLastName == "0" ? "" : " " + g.HRME_EmployeeLastName)).Trim(),
                                              ivrmM_ModuleName = e.IVRMM_ModuleName
                                          }
                    ).ToArray();
                    }
                    else
                    {
                        data.savedgrid = (from a in _StaffLoginContext.UserLoginPrivileges
                                          from b in _StaffLoginContext.institution_Module_Page
                                          from c in _StaffLoginContext.masterPage
                                          from d in _StaffLoginContext.institution_Module
                                          from e in _StaffLoginContext.masterModule
                                          where (a.IVRMIMP_Id == b.IVRMIMP_Id && b.IVRMIM_Id == d.IVRMIM_Id && a.IVRMIMP_Id == b.IVRMIMP_Id &&
                                          b.IVRMP_Id == c.IVRMP_Id && e.IVRMM_Id == d.IVRMM_Id && a.MI_Id == d.MI_Id &&
                                          miids.Contains(d.MI_Id) &&
                                          moduleidsss.Contains(e.IVRMM_Id) && a.Id == user.Id && miids.Contains(d.MI_Id)
                                          )
                                          select new StaffLoginDTO
                                          {
                                              IVRMMP_PageName = c.IVRMMP_PageName,
                                              IVRMSTAUP_Id = a.IVRMULP_Id,
                                              IVRMRP_AddFlag = a.IVRMSTUUP_AddFlag,
                                              IVRMRP_DeleteFlag = a.IVRMSTUUP_DeleteFlag,
                                              IVRMRP_UpdateFlag = a.IVRMSTUUP_UpdateFlag,
                                              IVRMRP_ProcessFlag = a.IVRMSTUUP_ProcessFlag,
                                              IVRMRP_ReportFlag = a.IVRMSTUUP_ReportFlag,
                                              IVRMIMP_Id = b.IVRMIMP_Id,
                                              IVRMP_Id = c.IVRMP_Id,
                                              //stafname = ((g.HRME_EmployeeFirstName == null || g.HRME_EmployeeFirstName == "" ? "" : g.HRME_EmployeeFirstName) + (g.HRME_EmployeeMiddleName == null || g.HRME_EmployeeMiddleName == "" || g.HRME_EmployeeMiddleName == "0" ? "" : " " + g.HRME_EmployeeMiddleName) + (g.HRME_EmployeeLastName == null || g.HRME_EmployeeLastName == "" || g.HRME_EmployeeLastName == "0" ? "" : " " + g.HRME_EmployeeLastName)).Trim(),
                                              ivrmM_ModuleName = e.IVRMM_ModuleName
                                          }
                    ).ToArray();
                    }




                    List<long> pageist = new List<long>();
                    pageist = (from a in _StaffLoginContext.IVRM_MobileApp_Page
                               from b in _StaffLoginContext.IVRM_Role_MobileApp_Privileges
                               from c in _StaffLoginContext.masterRoleType
                               from d in _StaffLoginContext.IVRM_User_MobileApp_Login_Privileges
                               where (a.IVRMMAP_Id == b.IVRMMAP_Id && b.MI_ID == data.MI_Id && b.IVRMRT_Id == c.IVRMRT_Id && d.IVRMMAP_Id == b.IVRMMAP_Id && c.IVRMRT_Id == data.IVRMRT_Id && d.IVRMUL_Id == user.Id && d.MI_Id == data.MI_Id)
                               select a).Distinct().Select(a => a.IVRMMAP_Id).ToList();


                    data.previousgrid = (from a in _StaffLoginContext.IVRM_MobileApp_Page
                                         from b in _StaffLoginContext.IVRM_Role_MobileApp_Privileges
                                         from c in _StaffLoginContext.masterRoleType
                                         from d in _StaffLoginContext.IVRM_User_MobileApp_Login_Privileges
                                         where (a.IVRMMAP_Id == b.IVRMMAP_Id && b.MI_ID == d.MI_Id && b.IVRMRT_Id == c.IVRMRT_Id && d.IVRMMAP_Id == b.IVRMMAP_Id && c.IVRMRT_Id == data.IVRMRT_Id && d.IVRMUL_Id == user.Id && d.MI_Id == data.MI_Id)
                                         select new MasterRolePreviledgeDTO
                                         {
                                             IVRMMAP_AppPageName = a.IVRMMAP_AppPageName,
                                             ivrmrT_Role = c.IVRMRT_Role,
                                             IVRMR_Id = c.IVRMR_Id,
                                             IVRMRP_Id = b.IVRMRMAP_Id,
                                             IVRMMAP_Id = a.IVRMMAP_Id,
                                             IVRMUMALP_Id = d.IVRMUMALP_Id,
                                             IVRMUMALP_AddFlg = d.IVRMUMALP_AddFlg,
                                             IVRMUMALP_UpdateFlg = d.IVRMUMALP_UpdateFlg,
                                             IVRMUMALP_DeleteFlg = d.IVRMUMALP_DeleteFlg
                                         }
             ).ToArray();



                    data.thirdgriddatamobile = (from a in _StaffLoginContext.IVRM_MobileApp_Page
                                                from b in _StaffLoginContext.IVRM_Role_MobileApp_Privileges
                                                from c in _StaffLoginContext.masterRoleType
                                                where (a.IVRMMAP_Id == b.IVRMMAP_Id && b.IVRMRT_Id == c.IVRMRT_Id && !pageist.Contains(b.IVRMMAP_Id) && c.IVRMRT_Id == data.IVRMRT_Id && b.MI_ID == data.MI_Id)
                                                select new MasterRolePreviledgeDTO
                                                {
                                                    IVRMMAP_AppPageName = a.IVRMMAP_AppPageName,
                                                    ivrmrT_Role = c.IVRMRT_Role,
                                                    IVRMR_Id = c.IVRMR_Id,
                                                    IVRMRP_Id = b.IVRMRMAP_Id,
                                                    IVRMMAP_Id = a.IVRMMAP_Id,
                                                    IVRMMAP_AddFlg = b.IVRMMAP_AddFlg,
                                                    IVRMMAP_UpdateFlg = b.IVRMMAP_UpdateFlg,
                                                    IVRMMAP_DeleteFlg = b.IVRMMAP_DeleteFlg
                                                }
              ).ToArray();




                    if (data.showgrid1.Length == 0)
                    {
                        data.returnval = "Institution level Module and Page is not mapped";

                    }

                    data.NoUserName = true;
                }
                else
                {
                    data.showgrid1 = (from a in _StaffLoginContext.institutionRolePrivileges
                                      from b in _StaffLoginContext.institution_Module_Page
                                      from c in _StaffLoginContext.masterRoleType
                                      from d in _StaffLoginContext.masterPage
                                      from e in _StaffLoginContext.institution_Module
                                      from g in _StaffLoginContext.masterModule
                                      from i in _StaffLoginContext.institution
                                      where (b.IVRMIMP_Id == a.IVRMIMP_Id && c.IVRMRT_Id == a.IVRMRT_Id &&
                                      d.IVRMP_Id == b.IVRMP_Id && b.IVRMIM_Id == e.IVRMIM_Id && e.IVRMM_Id == g.IVRMM_Id && e.MI_Id == i.MI_Id &&
                                      a.IVRMRT_Id == data.IVRMRT_Id && moduleidsss.Contains(e.IVRMM_Id)
                                      && miids.Contains(e.MI_Id)
                                      )
                                      select new StaffLoginDTO
                                      {
                                          IVRMMP_PageName = d.IVRMMP_PageName,
                                          IVRMIMP_Id = a.IVRMIMP_Id,
                                          IVRMRP_AddFlag = a.IVRMIRP_AddFlag,
                                          IVRMRP_DeleteFlag = a.IVRMIRP_DeleteFlag,
                                          IVRMRP_UpdateFlag = a.IVRMIRP_UpdateFlag,
                                          IVRMRP_ProcessFlag = a.IVRMIRP_ProcessFlag,
                                          IVRMRP_ReportFlag = a.IVRMIRP_ReportFlag,
                                          IVRMP_Id = d.IVRMP_Id,
                                          ivrmM_ModuleName = g.IVRMM_ModuleName,
                                          intname = i.MI_Name
                                      }
                     ).ToArray();

                    data.NoUserName = false;
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<StaffLoginDTO> multionchangeuser(StaffLoginDTO data)
        {
            try
            {
                List<long> moduleidsss = new List<long>();
                List<long> savedgrid = new List<long>();
                List<long> ids = new List<long>();


                if (data.multiplestaff.Length > 0)
                {
                    for (int i = 0; i < data.multiplestaff.Length; i++)
                    {
                        ids.Add(data.multiplestaff[i].IVRMSTAUL_Id);
                    }
                    moduleidsss = (from a in _StaffLoginContext.UserLoginPrivileges
                                   from b in _StaffLoginContext.institution_Module_Page
                                   from c in _StaffLoginContext.masterPage
                                   from d in _StaffLoginContext.institution_Module
                                   from e in _StaffLoginContext.masterModule
                                   where (a.IVRMIMP_Id == b.IVRMIMP_Id && b.IVRMIM_Id == d.IVRMIM_Id && a.MI_Id == d.MI_Id &&
                                   b.IVRMP_Id == c.IVRMP_Id && e.IVRMM_Id == d.IVRMM_Id && a.MI_Id == data.MI_Id
                                  && a.IVRMIMP_Id == b.IVRMIMP_Id &&
                                   ids.Contains(a.Id))
                                   select e
                               ).Distinct().Select(a => a.IVRMM_Id).ToList();

                    data.savedgrid = (from a in _StaffLoginContext.UserLoginPrivileges
                                      from b in _StaffLoginContext.institution_Module_Page
                                      from c in _StaffLoginContext.masterPage
                                      from d in _StaffLoginContext.institution_Module
                                      from e in _StaffLoginContext.masterModule
                                      from f in _StaffLoginContext.Staff_User_Login
                                      from g in _StaffLoginContext.masterStaff
                                      where (a.IVRMIMP_Id == b.IVRMIMP_Id && b.IVRMIM_Id == d.IVRMIM_Id && a.IVRMIMP_Id == b.IVRMIMP_Id &&
                                      b.IVRMP_Id == c.IVRMP_Id && e.IVRMM_Id == d.IVRMM_Id && a.MI_Id == d.MI_Id &&
                                      d.MI_Id == data.MI_Id &&
                                      moduleidsss.Contains(e.IVRMM_Id) && ids.Contains(a.Id) && f.Id == a.Id && f.Emp_Code == g.HRME_Id && g.MI_Id == data.MI_Id)
                                      select new StaffLoginDTO
                                      {
                                          IVRMMP_PageName = c.IVRMMP_PageName,
                                          IVRMSTAUP_Id = a.IVRMULP_Id,
                                          IVRMRP_AddFlag = a.IVRMSTUUP_AddFlag,
                                          IVRMRP_DeleteFlag = a.IVRMSTUUP_DeleteFlag,
                                          IVRMRP_UpdateFlag = a.IVRMSTUUP_UpdateFlag,
                                          IVRMRP_ProcessFlag = a.IVRMSTUUP_ProcessFlag,
                                          IVRMRP_ReportFlag = a.IVRMSTUUP_ReportFlag,
                                          IVRMIMP_Id = b.IVRMIMP_Id,
                                          stafname = ((g.HRME_EmployeeFirstName == null || g.HRME_EmployeeFirstName == "" ? "" : g.HRME_EmployeeFirstName) + (g.HRME_EmployeeMiddleName == null || g.HRME_EmployeeMiddleName == "" || g.HRME_EmployeeMiddleName == "0" ? "" : " " + g.HRME_EmployeeMiddleName) + (g.HRME_EmployeeLastName == null || g.HRME_EmployeeLastName == "" || g.HRME_EmployeeLastName == "0" ? "" : " " + g.HRME_EmployeeLastName)).Trim(),
                                          ivrmM_ModuleName = e.IVRMM_ModuleName
                                      }
                      ).ToArray();
                    data.previousgrid = (from a in _StaffLoginContext.IVRM_MobileApp_Page
                                         from b in _StaffLoginContext.IVRM_Role_MobileApp_Privileges
                                         from c in _StaffLoginContext.masterRoleType
                                         from d in _StaffLoginContext.IVRM_User_MobileApp_Login_Privileges
                                         from f in _StaffLoginContext.Staff_User_Login
                                         from g in _StaffLoginContext.masterStaff
                                         where (a.IVRMMAP_Id == b.IVRMMAP_Id && b.MI_ID == d.MI_Id && b.IVRMRT_Id == c.IVRMRT_Id && d.IVRMMAP_Id == b.IVRMMAP_Id && c.IVRMRT_Id == data.IVRMRT_Id && ids.Contains(d.IVRMUL_Id) && b.MI_ID == data.MI_Id && d.MI_Id == data.MI_Id && f.Id == d.IVRMUL_Id && f.Emp_Code == g.HRME_Id && g.MI_Id == data.MI_Id)
                                         select new MasterRolePreviledgeDTO
                                         {
                                             IVRMMAP_AppPageName = a.IVRMMAP_AppPageName,
                                             ivrmrT_Role = c.IVRMRT_Role,
                                             IVRMR_Id = c.IVRMR_Id,
                                             IVRMRP_Id = b.IVRMRMAP_Id,
                                             IVRMMAP_Id = a.IVRMMAP_Id,
                                             IVRMUMALP_Id = d.IVRMUMALP_Id,
                                             stafname = ((g.HRME_EmployeeFirstName == null || g.HRME_EmployeeFirstName == "" ? "" : g.HRME_EmployeeFirstName) + (g.HRME_EmployeeMiddleName == null || g.HRME_EmployeeMiddleName == "" || g.HRME_EmployeeMiddleName == "0" ? "" : " " + g.HRME_EmployeeMiddleName) + (g.HRME_EmployeeLastName == null || g.HRME_EmployeeLastName == "" || g.HRME_EmployeeLastName == "0" ? "" : " " + g.HRME_EmployeeLastName)).Trim(),
                                             IVRMUMALP_AddFlg = d.IVRMUMALP_AddFlg,
                                             IVRMUMALP_UpdateFlg = d.IVRMUMALP_UpdateFlg,
                                             IVRMUMALP_DeleteFlg = d.IVRMUMALP_DeleteFlg
                                         }
             ).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<StaffLoginDTO> multiuserdeletpages(StaffLoginDTO data)
        {
            try
            {

                List<long> deletegrid = new List<long>();

                if (data.deletesaved.Length > 0)
                {

                    //List<long> deletegriduserprivileges = new List<long>();

                    for (int i = 0; i < data.deletesaved.Count(); i++)
                    {
                        deletegrid.Add(data.deletesaved[i].IVRMSTAUP_Id);
                    }


                    List<UserLoginPrivileges> lorguser = new List<UserLoginPrivileges>();
                    lorguser = _StaffLoginContext.UserLoginPrivileges.Where(t => deletegrid.Contains(t.IVRMULP_Id)).ToList();

                    try
                    {
                        if (lorguser.Any())
                        {
                            for (int i = 0; i < lorguser.Count(); i++)
                            {
                                _StaffLoginContext.Remove(lorguser.ElementAt(i));
                            }
                            var contactExists = _StaffLoginContext.SaveChanges();
                            if (contactExists > 0)
                            {
                                data.returnval = "true";
                            }
                            else
                            {
                                data.returnval = "false";
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }



                if (data.deletmobile != null)
                {
                    if (data.deletmobile.Length > 0)
                    {
                        List<long> deletemobilegrid = new List<long>();
                        //List<long> deletegriduserprivileges = new List<long>();

                        for (int i = 0; i < data.deletmobile.Count(); i++)
                        {

                            deletemobilegrid.Add(data.deletmobile[i].IVRMUMALP_Id);
                        }


                        List<IVRM_User_MobileApp_Login_Privileges> lorguser = new List<IVRM_User_MobileApp_Login_Privileges>();
                        lorguser = _StaffLoginContext.IVRM_User_MobileApp_Login_Privileges.Where(t => deletemobilegrid.Contains(t.IVRMUMALP_Id)).ToList();

                        try
                        {
                            if (lorguser.Any())
                            {
                                for (int i = 0; i < lorguser.Count(); i++)
                                {
                                    _StaffLoginContext.Remove(lorguser.ElementAt(i));
                                }
                                var contactExists = _StaffLoginContext.SaveChanges();
                                if (contactExists > 0)
                                {
                                    data.returnval = "true";
                                }
                                else
                                {
                                    data.returnval = "false";
                                }
                            }
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //public async Task<StaffLoginDTO> saveorgdet(StaffLoginDTO pgmod)
        //{
        //    using (var transaction = _StaffLoginContext.Database.BeginTransaction())
        //    {
        //        // DataAccessMsSqlServerProvider.ApplicationUserRole user = new DataAccessMsSqlServerProvider.ApplicationUserRole();
        //      //  AppUserRole user = new AppUserRole();
        //        StaffLoginDTO someObj = new StaffLoginDTO();
        //        try
        //        {
        //            ApplicationUser user = new ApplicationUser();
        //            user = await _userManager.FindByNameAsync(pgmod.User_Name);

        //            if (user != null)
        //            {
        //                var getuserinstidata = _context.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id && d.MI_Id == pgmod.MI_Id).Select(d => d.Id).ToList();
        //                if (getuserinstidata.Count() > 0)
        //                {
        //                    pgmod.returnval = "Username already exists for the Selected institution and staff";
        //                    return pgmod;
        //                }

        //            }

        //            var checkforduplicate = _StaffLoginContext.staffLoginDMO.Where(t => t.MI_Id == pgmod.MI_Id && t.IVRMSTAUL_Id==pgmod.IVRMSTAUL_Id);

        //            if(checkforduplicate.Count()>0)
        //            {
        //                pgmod.returnval = "Username already exists for the Selected institution and staff";
        //                return pgmod;
        //            }
        //            else
        //            {
        //                StaffLoginDMO pgmodule = Mapper.Map<StaffLoginDMO>(pgmod);
        //                int s = 0;

        //                if (pgmod.datasaved != null)
        //                {
        //                    while (s < pgmod.datasaved.Count())
        //                    {
        //                        var resultsaved = _StaffLoginContext.staffLoginDMO.Single(t => t.IVRMSTAUP_Id == pgmod.datasaved[s].IVRMSTAUP_Id);

        //                        resultsaved.IVRMSTAUP_AddFlag = pgmod.datasaved[s].IVRMRP_AddFlag;
        //                        resultsaved.IVRMSTAUP_DeleteFlag = pgmod.datasaved[s].IVRMRP_DeleteFlag;
        //                        resultsaved.IVRMSTAUP_ProcessFlag = pgmod.datasaved[s].IVRMRP_ProcessFlag;
        //                        resultsaved.IVRMSTAUP_UpdateFlag = pgmod.datasaved[s].IVRMRP_UpdateFlag;
        //                        resultsaved.IVRMSTAUP_ReportFlag = pgmod.datasaved[s].IVRMRP_ReportFlag;
        //                        //added by 02/02/2017

        //                        resultsaved.UpdatedDate = DateTime.Now;
        //                        _StaffLoginContext.Update(resultsaved);
        //                        var contactExists = _StaffLoginContext.SaveChanges();

        //                        s++;
        //                    }

        //                    if (pgmod.savetmpdata != null)
        //                    {
        //                        int j = 0;
        //                        while (j < pgmod.savetmpdata.Count())
        //                        {
        //                            StaffLoginDMO pmm = new StaffLoginDMO();

        //                            if (pgmod.IVRMRT_Id != 0)
        //                            {
        //                                pmm.IVRMSTAUP_AddFlag = pgmod.savetmpdata[j].IVRMRP_AddFlag;
        //                                pmm.IVRMSTAUP_DeleteFlag = pgmod.savetmpdata[j].IVRMRP_DeleteFlag;
        //                                pmm.IVRMSTAUP_UpdateFlag = pgmod.savetmpdata[j].IVRMRP_UpdateFlag;
        //                                pmm.IVRMSTAUP_ProcessFlag = pgmod.savetmpdata[j].IVRMRP_ProcessFlag;
        //                                pmm.IVRMSTAUP_ReportFlag = pgmod.savetmpdata[j].IVRMRP_ReportFlag;
        //                                pmm.IVRMRT_Id = pgmod.IVRMRT_Id;
        //                                pmm.MI_Id = pgmod.MI_Id;
        //                                pmm.IVRMSTAUL_Id = pgmod.IVRMSTAUL_Id;
        //                                pmm.IVRMIMP_Id = pgmod.savetmpdata[j].IVRMIMP_Id;
        //                                pmm.amc_id = pgmod.AMC_Id;
        //                                pmm.User_Name = pgmod.User_Name;
        //                                pmm.IVRMRT_Id = pgmod.IVRMRT_Id;
        //                                //added by 02/02/2017
        //                                pmm.CreatedDate = DateTime.Now;
        //                                pmm.UpdatedDate = DateTime.Now;
        //                                _StaffLoginContext.Add(pmm);
        //                                var contactExists = _StaffLoginContext.SaveChanges();

        //                                if (contactExists == 1)
        //                                {
        //                                    pgmod.returnval = "Record Saved/Updated Successfully.UserName & Password is sent to Email Id";
        //                                }
        //                                else
        //                                {
        //                                    pgmod.returnval = "Record not Saved/Updated ";
        //                                }
        //                            }
        //                            j++;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (pgmod.savetmpdata != null)
        //                    {
        //                        int j = 0;
        //                        while (j < pgmod.savetmpdata.Count())
        //                        {
        //                            StaffLoginDMO pmm = new StaffLoginDMO();

        //                            if (pgmod.IVRMRT_Id != 0)
        //                            {
        //                                pmm.IVRMSTAUP_AddFlag = pgmod.savetmpdata[j].IVRMRP_AddFlag;
        //                                pmm.IVRMSTAUP_DeleteFlag = pgmod.savetmpdata[j].IVRMRP_DeleteFlag;
        //                                pmm.IVRMSTAUP_UpdateFlag = pgmod.savetmpdata[j].IVRMRP_UpdateFlag;
        //                                pmm.IVRMSTAUP_ProcessFlag = pgmod.savetmpdata[j].IVRMRP_ProcessFlag;
        //                                pmm.IVRMSTAUP_ReportFlag = pgmod.savetmpdata[j].IVRMRP_ReportFlag;
        //                                pmm.IVRMRT_Id = pgmod.IVRMRT_Id;
        //                                pmm.MI_Id = pgmod.MI_Id;
        //                                pmm.IVRMSTAUL_Id = pgmod.IVRMSTAUL_Id;
        //                                pmm.IVRMIMP_Id = pgmod.savetmpdata[j].IVRMIMP_Id;
        //                                pmm.amc_id = pgmod.AMC_Id;
        //                                pmm.User_Name = pgmod.User_Name;
        //                                pmm.IVRMRT_Id = pgmod.IVRMRT_Id;


        //                                //added by 02/02/2017
        //                                pmm.CreatedDate = DateTime.Now;
        //                                pmm.UpdatedDate = DateTime.Now;
        //                                _StaffLoginContext.Add(pmm);
        //                                var contactExists = _StaffLoginContext.SaveChanges();

        //                                if (contactExists == 1)
        //                                {

        //                                    pgmod.returnval = "Record Saved/Updated Successfully.UserName & Password is sent to Email Id";
        //                                }
        //                                else
        //                                {

        //                                    pgmod.returnval = "Record not Saved/Updated";
        //                                }
        //                            }
        //                            j++;
        //                        }

        //                        string emailid = "", phoneno = "";
        //                        List<MasterStaff> staffdetails = new List<MasterStaff>();
        //                        staffdetails = _StaffLoginContext.masterStaff.Where(t => t.IVRMSTAUL_Id.Equals(pgmod.IVRMSTAUL_Id)).ToList();
        //                        if (staffdetails[0].email_id != null)
        //                        {
        //                            emailid = staffdetails[0].email_id;
        //                        }
        //                        if (staffdetails[0].phone_no != 0)
        //                        {
        //                            phoneno = staffdetails[0].phone_no.ToString();
        //                        }
        //                        //  ApplicationUser userr = new ApplicationUser();
        //                        user = new ApplicationUser { UserName = pgmod.User_Name, Email = emailid, PhoneNumber = phoneno, EmailConfirmed = true, UserImagePath = "D:\\IVRM 2.0 23.12\\corewebapi18072016\\src\\corewebapi18072016\\wwwroot\\images\\uploads\\Profile_Pics\\a_1c6e77cd-1ba4-45eb-959a-19b80a5c0dfc.jpg" };

        //                        var resul = await _userManager.CreateAsync(user, "Password@123");
        //                        if (resul.Succeeded)
        //                        {

        //                        }
        //                        else
        //                        {
        //                            user = await _userManager.FindByNameAsync(pgmod.User_Name);
        //                        }
        //                        SMS sms = new SMS(_context);
        //                        string smsdet = await sms.sendSms(pgmod.MI_Id, Convert.ToInt64(phoneno), "StaffUserCreation", user.Id);

        //                        Email Email = new Email(_context);

        //                        string m = Email.sendmail(pgmod.MI_Id, emailid, "StaffUserCreation", user.Id);

        //                        List<MasterRoleType> allmodules = new List<MasterRoleType>();
        //                        allmodules = _StaffLoginContext.masterRoleType.Where(t => t.IVRMRT_Id.Equals(pgmod.IVRMRT_Id)).ToList();
        //                        long roleid = allmodules[0].IVRMR_Id;
        //                        string roletypeflag = allmodules[0].IVRMRT_RoleFlag;

        //                        //inserting into applicationuserrole table
        //                        DataAccessMsSqlServerProvider.ApplicationUserRole rle = new DataAccessMsSqlServerProvider.ApplicationUserRole();
        //                        rle.RoleId = Convert.ToInt32(roleid);
        //                        rle.RoleTypeId = pgmod.IVRMRT_Id;
        //                        rle.UserId = user.Id;


        //                        //added by 02/02/2017
        //                        rle.CreatedDate = DateTime.Now;
        //                        rle.UpdatedDate = DateTime.Now;
        //                        _StaffLoginContext.Add(rle);
        //                        _StaffLoginContext.SaveChanges();
        //                        //inserting into applicationuserrole table

        //                        //inserting into IVRM_User_Login_Institutionwise table
        //                        UserRoleWithInstituteDMO mas = new UserRoleWithInstituteDMO();
        //                        mas.Id = user.Id;
        //                        mas.MI_Id = pgmod.MI_Id;   //added by 02/02/2017
        //                        mas.CreatedDate = DateTime.Now;
        //                        mas.UpdatedDate = DateTime.Now;
        //                        _StaffLoginContext.Add(mas);
        //                        _StaffLoginContext.SaveChanges();

        //                        //inserting into IVRM_Staff_User_Login table
        //                        Staff_User_Login stafflo = new Staff_User_Login();
        //                        stafflo.IVRMSTAUL_UserName = pgmod.User_Name;
        //                        stafflo.IVRMSTAUL_Password = "password@123";
        //                        stafflo.IVRMSTAUL_Id = 0;
        //                        stafflo.MI_Id = pgmod.MI_Id;
        //                        //added by 02/02/2017
        //                        stafflo.CreatedDate = DateTime.Now;
        //                        stafflo.UpdatedDate = DateTime.Now;
        //                        _StaffLoginContext.Add(stafflo);
        //                        _StaffLoginContext.SaveChanges();

        //                        // UserLogin loginusr = Mapper.Map<UserLogin>(pgmod);
        //                        UserLoginDTO usrlo = new UserLoginDTO();
        //                        UserLogin usrlodmo = new UserLogin();
        //                        usrlodmo.MI_Id = pgmod.MI_Id;
        //                        usrlodmo.IVRMRT_Id = pgmod.IVRMRT_Id;
        //                        usrlodmo.IVRMUL_UserName = pgmod.User_Name;
        //                        usrlodmo.IVRMUL_ActiveFlag = 0;
        //                        usrlodmo.IVRMUL_SuperAdminFlag = roletypeflag;
        //                        //added by 02/02/2017
        //                        usrlodmo.CreatedDate = DateTime.Now;
        //                        usrlodmo.UpdatedDate = DateTime.Now;
        //                        _StaffLoginContext.Add(usrlodmo);
        //                        var contactExistslogin = _StaffLoginContext.SaveChanges();

        //                        if (contactExistslogin == 1)
        //                        {

        //                            pgmod.returnval = "Record Saved/Updated Successfully.UserName & Password is sent to Email Id";
        //                        }
        //                        else
        //                        {

        //                            pgmod.returnval = "Record not Saved/Updated";
        //                        }

        //                        // UserLoginEmployeeDTO usrloemp = new UserLoginEmployeeDTO();
        //                        UserLoginEmployee usrloempdmo = new UserLoginEmployee();

        //                        usrloempdmo.MI_Id = pgmod.MI_Id;
        //                        usrloempdmo.IVRMUL_Id = usrlodmo.IVRMUL_Id;
        //                        usrloempdmo.Emp_Code = pgmod.IVRMSTAUL_Id;
        //                        //added by 02/02/2017
        //                        usrloempdmo.CreatedDate = DateTime.Now;
        //                        usrloempdmo.UpdatedDate = DateTime.Now;
        //                        _StaffLoginContext.Add(usrloempdmo);
        //                        var contactExistsloginemployee = _StaffLoginContext.SaveChanges();

        //                        if (contactExistslogin == 1)
        //                        {

        //                            pgmod.returnval = "Record Saved/Updated Successfully.UserName & Password is sent to Email Id";
        //                        }
        //                        else
        //                        {
        //                            pgmod.returnval = "Record not Saved/Updated";
        //                        }


        //                        //ApplicationUserRole userrole = new ApplicationUserRole();
        //                        ////userrole = new ApplicationUserRole { UserId = userr.Id, RoleId = 2, RoleTypeId = 12 };
        //                        //userrole = new ApplicationUserRole { UserId = userr.Id, RoleId = 2};
        //                        //var userroleresult = await _userManagerRole.CreateAsync(userrole);
        //                    }
        //                }

        //                //          pgmod.thirdgriddata = (from a in _StaffLoginContext.staffLoginDMO
        //                //                                      from b in _StaffLoginContext.institution
        //                //                                      from c in _StaffLoginContext.Staff_User_Login
        //                //                                 from d in _StaffLoginContext.institution_Module_Page
        //                //                                      from e in _StaffLoginContext.institution_Module
        //                //                                      from f in _StaffLoginContext.masterModule
        //                //                                      from g in _StaffLoginContext.masterPage
        //                //                                      where (a.MI_Id == b.MI_Id && c.IVRMSTAUL_Id == a.IVRMSTAUL_Id && d.IVRMIMP_Id == a.IVRMIMP_Id && e.IVRMIM_Id == d.IVRMIM_Id && e.IVRMM_Id == f.IVRMM_Id && d.IVRMP_Id == g.IVRMP_Id)
        //                //                                      select new StaffLoginDTO
        //                //                                      {
        //                //                                          User_Name = c.IVRMSTAUL_UserName,
        //                //                                          ivrmM_ModuleName = f.IVRMM_ModuleName,
        //                //                                          MI_Name = b.MI_Name,
        //                //                                          IVRMSTAUP_Id = a.IVRMSTAUP_Id,
        //                //                                          IVRMMP_PageName = g.IVRMMP_PageName
        //                //                                      }
        //                //).ToArray();

        //                transaction.Commit();

        //                var rolelist = _StaffLoginContext.MasterRoleType.Where(t => t.IVRMRT_Id == pgmod.roleId).ToList();

        //                if (rolelist[0].IVRMRT_Role == "Super Admin")
        //                {
        //                    pgmod.thirdgriddata = (from a in _StaffLoginContext.staffLoginDMO
        //                                           from b in _StaffLoginContext.institution
        //                                           from c in _StaffLoginContext.masterStaff
        //                                           from d in _StaffLoginContext.institution_Module_Page
        //                                           from e in _StaffLoginContext.institution_Module
        //                                           from f in _StaffLoginContext.masterModule
        //                                           from g in _StaffLoginContext.masterPage
        //                                           where (a.MI_Id == b.MI_Id && c.IVRMSTAUL_Id == a.IVRMSTAUL_Id && d.IVRMIMP_Id == a.IVRMIMP_Id && e.IVRMIM_Id == d.IVRMIM_Id && e.IVRMM_Id == f.IVRMM_Id && d.IVRMP_Id == g.IVRMP_Id)
        //                                           select new StaffLoginDTO
        //                                           {
        //                                               User_Name = c.IVRMSTAUL_Name,
        //                                               ivrmM_ModuleName = f.IVRMM_ModuleName,
        //                                               MI_Name = b.MI_Name,
        //                                               IVRMSTAUP_Id = a.IVRMSTAUP_Id,
        //                                               IVRMMP_PageName = g.IVRMMP_PageName,
        //                                               staffusername = a.User_Name
        //                                           }
        //    ).ToArray();
        //                }

        //                else
        //                {
        //                    pgmod.thirdgriddata = (from a in _StaffLoginContext.staffLoginDMO
        //                                           from b in _StaffLoginContext.institution
        //                                           from c in _StaffLoginContext.masterStaff
        //                                           from d in _StaffLoginContext.institution_Module_Page
        //                                           from e in _StaffLoginContext.institution_Module
        //                                           from f in _StaffLoginContext.masterModule
        //                                           from g in _StaffLoginContext.masterPage
        //                                           where (a.MI_Id == b.MI_Id && c.IVRMSTAUL_Id == a.IVRMSTAUL_Id && d.IVRMIMP_Id == a.IVRMIMP_Id && e.IVRMIM_Id == d.IVRMIM_Id && e.IVRMM_Id == f.IVRMM_Id && d.IVRMP_Id == g.IVRMP_Id && b.MI_Id == pgmod.MI_Id)
        //                                           select new StaffLoginDTO
        //                                           {
        //                                               User_Name = c.IVRMSTAUL_Name,
        //                                               ivrmM_ModuleName = f.IVRMM_ModuleName,
        //                                               MI_Name = b.MI_Name,
        //                                               IVRMSTAUP_Id = a.IVRMSTAUP_Id,
        //                                               IVRMMP_PageName = g.IVRMMP_PageName,
        //                                               staffusername = a.User_Name
        //                                           }
        //   ).ToArray();
        //                }
        //            }

        //        }

        //        catch (Exception ee)
        //        {
        //            Console.WriteLine(ee.Message);
        //            pgmod.returnval = "Record not Saved/Updated ";
        //        }
        //    }

        //    return pgmod;
        //}

        public async Task<StaffLoginDTO> saveorgdet(StaffLoginDTO pgmod)
        {
            int count = 0;
            using (var transaction = _StaffLoginContext.Database.BeginTransaction())
            {
                StaffLoginDTO someObj = new StaffLoginDTO();

                try
                {
                    List<MasterEmployee> staffdetails = new List<MasterEmployee>();

                    List<MasterEmployee> inststaffdetails = new List<MasterEmployee>();


                    List<Multiple_Mobile_DMO> staffdetailsmobile = new List<Multiple_Mobile_DMO>();
                    List<Multiple_Email_DMO> staffdetailsEmail = new List<Multiple_Email_DMO>();


                    if (pgmod.singlemulti == "Multiple")
                    {
                        pgmod = AddUpdateUserPrevilegesMultiuser(pgmod);
                    }
                    else
                    {
                        // staffdetails = _StaffLoginContext.masterStaff.Where(t => t.HRME_Id.Equals(pgmod.IVRMSTAUL_Id)).ToList();

                        staffdetails = (from a in _StaffLoginContext.masterStaff
                                        where a.MI_Id == pgmod.MI_Id && a.HRME_Id == pgmod.IVRMSTAUL_Id
                                        select new MasterEmployee
                                        {
                                            HRME_Id = a.HRME_Id,

                                            HRME_Photo = a.HRME_Photo
                                        }).ToList();


                        if (staffdetails.Count() > 0)
                        {
                            staffdetailsmobile = (from a in _StaffLoginContext.masterStaff
                                                  from b in _StaffLoginContext.Multiple_Mobile_DMO
                                                  where a.MI_Id == pgmod.MI_Id && a.HRME_Id == b.HRME_Id && a.HRME_Id == pgmod.IVRMSTAUL_Id
                                                  select new Multiple_Mobile_DMO
                                                  {
                                                      HRMEMNO_MobileNo = b.HRMEMNO_MobileNo

                                                  }).ToList();


                            staffdetailsEmail = (from a in _StaffLoginContext.masterStaff
                                                 from b in _StaffLoginContext.Multiple_Email_DMO
                                                 where a.MI_Id == pgmod.MI_Id && a.HRME_Id == b.HRME_Id && a.HRME_Id == pgmod.IVRMSTAUL_Id
                                                 select new Multiple_Email_DMO
                                                 {
                                                     HRMEM_EmailId = b.HRMEM_EmailId
                                                 }).ToList();

                            ApplicationUser usermobile = new ApplicationUser();
                            usermobile = await _userManager.FindByNameAsync(pgmod.User_Name.Trim());
                            if (usermobile == null)
                            {
                                if (staffdetailsmobile.Count() == 0 || staffdetailsEmail.Count() == 0)
                                {
                                    pgmod.returnval = "Mobile Number/Email Id Not Updated For This Staff..!! ";
                                    return pgmod;
                                }
                            }


                        }


                        ApplicationUser user = new ApplicationUser();
                        user = await _userManager.FindByNameAsync(pgmod.User_Name.Trim());



                        if (user == null)
                        {

                            string userphoto = "";
                            string webRootPath = _hostingEnvironment.WebRootPath;
                            var uploads1 = "images\\uploads\\Profile_Pics\\a_1c6e77cd-1ba4-45eb-959a-19b80a5c0dfc.jpg";
                            if (staffdetails.Count > 0)
                            {
                                if (staffdetails[0].HRME_Photo != "" || staffdetails[0].HRME_Photo != null)
                                {
                                    userphoto = staffdetails[0].HRME_Photo;
                                }
                                else
                                {
                                    userphoto = uploads1;
                                }

                                user = new ApplicationUser { UserName = pgmod.User_Name.Trim(), Email = staffdetailsEmail[0].HRMEM_EmailId, PhoneNumber = staffdetailsmobile[0].HRMEMNO_MobileNo.ToString(), EmailConfirmed = true, UserImagePath = userphoto };

                            }
                            else
                            {
                                inststaffdetails = (from a in _StaffLoginContext.institution
                                                    from b in _StaffLoginContext.institutemobile
                                                    from c in _StaffLoginContext.instituteemail
                                                    where a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == pgmod.MI_Id
                                                    select new MasterEmployee
                                                    {

                                                        HRME_EmailId = c.MIE_EmailId,
                                                        HRME_MobileNo = b.MIMN_MobileNo


                                                    }).ToList();

                                userphoto = uploads1;

                                if (inststaffdetails.Count() > 0)
                                {
                                    if (inststaffdetails[0].HRME_EmailId != null && inststaffdetails[0].HRME_MobileNo != 0)
                                    {
                                        user = new ApplicationUser { UserName = pgmod.User_Name.Trim(), Email = inststaffdetails[0].HRME_EmailId, PhoneNumber = inststaffdetails[0].HRME_MobileNo.ToString(), EmailConfirmed = true, UserImagePath = userphoto };
                                    }
                                    else
                                    {
                                        pgmod.returnval = "Mobile Number/Email Id Not Updated For This Institution..!! ";
                                        return pgmod;
                                    }
                                }
                                else
                                {
                                    pgmod.returnval = "Mobile Number/Email Id Not Updated For This Institution..!! ";
                                    return pgmod;
                                }



                            }

                            user.Entry_Date = DateTime.Now;
                            user.CreatedDate = DateTime.Now;
                            user.UpdatedDate = DateTime.Now;
                            user.RoleTypeFlag = "Staff";
                            user.Machine_Ip_Address = pgmod.Machine_Ip_Address;
                            user.Name = pgmod.User_Name.Trim();

                            var resul = await _userManager.CreateAsync(user, "Password@123");
                            if (resul.Succeeded)
                            {
                                pgmod.NewUserId = user.Id;
                                pgmod = InsertIntoApplicationUserRole(pgmod);
                                pgmod = InsertIntoUser_Login_Institutionwise(pgmod);
                            }
                            else
                            {
                                pgmod.returnval = "fail";
                                return pgmod;
                            }
                        }
                        else
                        {
                            if (user != null)
                            {
                                var userrole = _StaffLoginContext.appUserRole.Where(t => t.UserId == user.Id).ToList();
                                if (userrole != null)
                                {
                                    var roletype = _StaffLoginContext.MasterRoleType.Where(t => t.IVRMRT_Id == userrole.FirstOrDefault().RoleTypeId).ToList();
                                    if (roletype != null)
                                    {
                                        if (roletype.FirstOrDefault().flag.ToString() != "N")
                                        {
                                            pgmod.NewUserId = user.Id;
                                            pgmod = InsertIntoUser_Login_Institutionwise(pgmod);
                                            if (pgmod.returnval == "exists")
                                            {
                                                pgmod.returnval = "User already exist for this username!!!";
                                                return pgmod;
                                            }
                                        }
                                        else
                                        {
                                            pgmod.returnval = "User already exist for this username!!!";
                                            return pgmod;
                                        }
                                    }
                                }
                            }
                        }

                        var resultCount = _StaffLoginContext.UserLoginPrivileges.Where(t => t.Id == user.Id && t.IVRMULP_Id == pgmod.IVRMSTAUL_Id).Count();
                        if (resultCount == 0)
                        {
                            count++;
                            if (staffdetails.Count > 0)
                            {
                                pgmod = StaffUserCreationUpdation(pgmod);
                            }
                            // staffdetails = _StaffLoginContext.masterStaff.Where(t => t.HRME_Id.Equals(pgmod.IVRMSTAUL_Id)).ToList();
                            //if (staffdetails.Count() != 0)
                            //{
                            //    SMS sms = new SMS(_context);
                            //    string smsdet = await sms.sendSms(pgmod.MI_Id, Convert.ToInt64(staffdetailsmobile[0].HRMEMNO_MobileNo), "StaffUserCreation", pgmod.NewUserId);

                            //    Email Email = new Email(_context);

                            //    string m = Email.sendmail(pgmod.MI_Id, staffdetailsEmail[0].HRMEM_EmailId, "StaffUserCreation", pgmod.NewUserId);
                            //}
                            //else
                            //{
                            //    SMS sms = new SMS(_context);
                            //    string smsdet = await sms.sendSms(pgmod.MI_Id, Convert.ToInt64(inststaffdetails[0].HRME_MobileNo), "StaffUserCreation", pgmod.NewUserId);

                            //    Email Email = new Email(_context);

                            //    string m = Email.sendmail(pgmod.MI_Id, inststaffdetails[0].HRME_MobileNo.ToString(), "StaffUserCreation", pgmod.NewUserId);
                            //}

                        }

                        //if (staffdetails.Count > 0)
                        //{
                        //    pgmod = AddUpdateStaffUserPrevileges(pgmod);
                        //    count++;
                        //}


                        pgmod = AddUpdateUserPrevileges(pgmod);
                    }


                    if (count > 1)
                    {
                        pgmod.returnval = "Record Saved/Updated Successfully.UserName & Password is sent to Email Id";
                    }
                    else
                    {
                        pgmod.returnval = "Record Saved/Updated Successfully";
                    }
                    transaction.Commit();
                }

                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                    pgmod.returnval = "Record not Saved/Updated ";
                }
            }

            return pgmod;
        }

        public StaffLoginDTO StaffUserCreationUpdation(StaffLoginDTO pgmod)
        {
            try
            {
                var resultCount = _StaffLoginContext.Staff_User_Login.Where(t => t.IVRMSTAUL_UserName == pgmod.User_Name).Count();
                if (resultCount == 0)
                {
                    //inserting into IVRM_Staff_User_Login table
                    Staff_User_Login stafflo = new Staff_User_Login();
                    stafflo.IVRMSTAUL_UserName = pgmod.User_Name;
                    stafflo.IVRMSTAUL_Password = "password@123";
                    stafflo.MI_Id = pgmod.MI_Id;
                    stafflo.Id = pgmod.NewUserId;
                    stafflo.Emp_Code = pgmod.IVRMSTAUL_Id;
                    stafflo.IVRMSTAUL_ActiveFlag = 1;

                    //added by 02/02/2017
                    stafflo.CreatedDate = DateTime.Now;
                    stafflo.UpdatedDate = DateTime.Now;
                    _StaffLoginContext.Add(stafflo);
                    _StaffLoginContext.SaveChanges();
                }
            }

            catch (Exception e)
            {
                throw;
            }

            return pgmod;
        }

        public StaffLoginDTO InsertIntoApplicationUserRole(StaffLoginDTO pgmod)
        {
            try
            {
                List<MasterRoleType> allmodules = new List<MasterRoleType>();
                allmodules = _StaffLoginContext.masterRoleType.Where(t => t.IVRMRT_Id.Equals(pgmod.IVRMRT_Id)).ToList();
                long roleid = allmodules[0].IVRMR_Id;
                string roletypeflag = allmodules[0].IVRMRT_RoleFlag;


                var roleduplicate = _StaffLoginContext.appUserRole.Where(t => t.UserId == pgmod.NewUserId).ToList();
                if (roleduplicate.Count() == 0)
                {
                    //inserting into applicationuserrole table
                    DataAccessMsSqlServerProvider.ApplicationUserRole rle = new DataAccessMsSqlServerProvider.ApplicationUserRole();

                    //


                    rle.RoleId = Convert.ToInt32(roleid);
                    rle.RoleTypeId = pgmod.IVRMRT_Id;
                    rle.UserId = pgmod.NewUserId;

                    //added by 02/02/2017
                    rle.CreatedDate = DateTime.Now;
                    rle.UpdatedDate = DateTime.Now;
                    _StaffLoginContext.Add(rle);
                    _StaffLoginContext.SaveChanges();
                }

                var roleduplicateuser = _StaffLoginContext.userLogin.Where(t => t.IVRMUL_UserName.Equals(pgmod.User_Name, StringComparison.OrdinalIgnoreCase)).ToList();
                if (roleduplicateuser.Count() == 0)
                {
                    UserLogin usrlodmo = new UserLogin();
                    usrlodmo.MI_Id = pgmod.MI_Id;
                    usrlodmo.IVRMRT_Id = pgmod.IVRMRT_Id;
                    usrlodmo.IVRMUL_UserName = pgmod.User_Name;
                    usrlodmo.IVRMUL_ActiveFlag = 1;
                    usrlodmo.IVRMUL_SuperAdminFlag = roletypeflag;
                    //added by 02/02/2017
                    usrlodmo.CreatedDate = DateTime.Now;
                    usrlodmo.UpdatedDate = DateTime.Now;
                    _StaffLoginContext.Add(usrlodmo);
                    var contactExistslogin = _StaffLoginContext.SaveChanges();
                    if (contactExistslogin == 1)
                    {

                        pgmod.returnval = "Record Saved/Updated Successfully.UserName & Password is sent to Email Id";
                    }
                    else
                    {

                        pgmod.returnval = "Record not Saved/Updated";
                    }


                    List<MasterEmployee> staffdetails = new List<MasterEmployee>();

                    staffdetails = (from a in _StaffLoginContext.masterStaff
                                    where a.MI_Id == pgmod.MI_Id && a.HRME_Id == pgmod.IVRMSTAUL_Id
                                    select new MasterEmployee
                                    {
                                        HRME_Id = a.HRME_Id,
                                        //HRME_EmailId = a.HRME_EmailId,
                                        //HRME_MobileNo = a.HRME_MobileNo,
                                        HRME_Photo = a.HRME_Photo

                                    }).ToList();


                    if (staffdetails.Count != 0)
                    {
                        UserLoginEmployee usrloempdmo = new UserLoginEmployee();

                        usrloempdmo.MI_Id = pgmod.MI_Id;
                        usrloempdmo.IVRMUL_Id = usrlodmo.IVRMUL_Id;
                        usrloempdmo.Emp_Code = pgmod.IVRMSTAUL_Id;
                        //added by 02/02/2017
                        usrloempdmo.CreatedDate = DateTime.Now;
                        usrloempdmo.UpdatedDate = DateTime.Now;
                        _StaffLoginContext.Add(usrloempdmo);
                        var contactExistsloginemployee = _StaffLoginContext.SaveChanges();

                        if (contactExistslogin == 1)
                        {

                            pgmod.returnval = "Record Saved/Updated Successfully.UserName & Password is sent to Email Id";
                        }
                        else
                        {
                            pgmod.returnval = "Record not Saved/Updated";
                        }
                    }
                }



            }
            catch (Exception)
            {

                throw;
            }




            return pgmod;
        }

        public StaffLoginDTO InsertIntoUser_Login_Institutionwise(StaffLoginDTO pgmod)
        {
            try
            {

                if (pgmod.multipleinsti != null && pgmod.multipleinsti.Length > 0)
                {

                    for (int i = 0; i < pgmod.multipleinsti.Length; i++)
                    {
                        UserRoleWithInstituteDMO mas = new UserRoleWithInstituteDMO();
                        var Count = _StaffLoginContext.UserRoleWithInstituteDMO.Where(t => t.MI_Id == pgmod.multipleinsti[i].mi_id && t.Id == pgmod.NewUserId).ToList();
                        if (Count.Count() > 0)
                        {
                            var result = _StaffLoginContext.UserRoleWithInstituteDMO.Single(t => t.MI_Id == pgmod.multipleinsti[i].mi_id && t.Id == pgmod.NewUserId);

                            result.Id = pgmod.NewUserId;
                            result.MI_Id = pgmod.multipleinsti[i].mi_id;   //added by 02/02/2017
                            result.Activeflag = 1;
                            result.UpdatedDate = DateTime.Now;
                            _StaffLoginContext.Update(result);
                            _StaffLoginContext.SaveChanges();
                        }
                        else
                        {
                            mas.Id = pgmod.NewUserId;
                            mas.MI_Id = pgmod.multipleinsti[i].mi_id;
                            mas.Activeflag = 1;//added by 02/02/2017
                            mas.CreatedDate = DateTime.Now;
                            mas.UpdatedDate = DateTime.Now;
                            _StaffLoginContext.Add(mas);
                            _StaffLoginContext.SaveChanges();
                        }
                    }
                }
                else
                {
                    UserRoleWithInstituteDMO mas = new UserRoleWithInstituteDMO();
                    var Count = _StaffLoginContext.UserRoleWithInstituteDMO.Where(t => t.Id == pgmod.NewUserId).ToList();
                    if (Count.Count() == 0)
                    {
                        //var result = _StaffLoginContext.UserRoleWithInstituteDMO.Single(t => t.MI_Id == pgmod.MI_Id && t.Id == pgmod.NewUserId);

                        //result.Id = pgmod.NewUserId;
                        //result.MI_Id = pgmod.MI_Id;   //added by 02/02/2017
                        //result.Activeflag = 1;
                        //result.UpdatedDate = DateTime.Now;
                        //_StaffLoginContext.Add(result);
                        //_StaffLoginContext.SaveChanges();

                        mas.Id = pgmod.NewUserId;
                        mas.MI_Id = pgmod.MI_Id;   //added by 02/02/2017
                        mas.Activeflag = 1;
                        mas.CreatedDate = DateTime.Now;
                        mas.UpdatedDate = DateTime.Now;
                        _StaffLoginContext.Add(mas);
                        _StaffLoginContext.SaveChanges();
                    }
                    else if (Count.Count() == 1)
                    {
                        if (Count.FirstOrDefault().MI_Id != pgmod.MI_Id)
                        {
                            pgmod.returnval = "exists";
                            return pgmod;
                        }
                    }
                    //else
                    //{
                    //    if (Count.FirstOrDefault().MI_Id != pgmod.MI_Id)
                    //    {
                    //        pgmod.returnval = "exists";
                    //        return pgmod;
                    //    }
                    //}
                }
            }
            catch (Exception e)
            {

                throw;
            }

            return pgmod;
        }

        public StaffLoginDTO AddUpdateStaffUserPrevileges(StaffLoginDTO pgmod)
        {

            StaffLoginDMO pgmodule = Mapper.Map<StaffLoginDMO>(pgmod);
            int s = 0;
            int f = 0;

            if (pgmod.datasaved != null)
            {
                while (s < pgmod.datasaved.Count())
                {
                    var resultsaved = _StaffLoginContext.staffLoginDMO.Single(t => t.IVRMSTAUP_Id == pgmod.datasaved[s].IVRMSTAUP_Id);

                    resultsaved.IVRMSTAUP_AddFlag = Convert.ToBoolean(pgmod.datasaved[s].IVRMRP_AddFlag);
                    resultsaved.IVRMSTAUP_DeleteFlag = Convert.ToBoolean(pgmod.datasaved[s].IVRMRP_DeleteFlag);
                    resultsaved.IVRMSTAUP_ProcessFlag = Convert.ToBoolean(pgmod.datasaved[s].IVRMRP_ProcessFlag);
                    resultsaved.IVRMSTAUP_UpdateFlag = Convert.ToBoolean(pgmod.datasaved[s].IVRMRP_UpdateFlag);
                    resultsaved.IVRMSTAUP_ReportFlag = Convert.ToBoolean(pgmod.datasaved[s].IVRMRP_ReportFlag);
                    //added by 02/02/2017

                    resultsaved.UpdatedDate = DateTime.Now;
                    _StaffLoginContext.Update(resultsaved);
                    var contactExists = _StaffLoginContext.SaveChanges();

                    s++;
                }

                if (pgmod.savetmpdata != null)
                {
                    int j = 0;
                    while (j < pgmod.savetmpdata.Count())
                    {
                        StaffLoginDMO pmm = new StaffLoginDMO();

                        if (pgmod.IVRMRT_Id != 0)
                        {
                            pmm.IVRMSTAUP_AddFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_AddFlag);
                            pmm.IVRMSTAUP_DeleteFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_DeleteFlag);
                            pmm.IVRMSTAUP_UpdateFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_UpdateFlag);
                            pmm.IVRMSTAUP_ProcessFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_ProcessFlag);
                            pmm.IVRMSTAUP_ReportFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_ReportFlag);
                            pmm.IVRMRT_Id = pgmod.IVRMRT_Id;
                            pmm.MI_Id = pgmod.MI_Id;
                            pmm.IVRMSTAUL_Id = Convert.ToInt32(pgmod.IVRMSTAUL_Id);
                            pmm.IVRMIMP_Id = pgmod.savetmpdata[j].IVRMIMP_Id;
                            pmm.amc_id = pgmod.AMC_Id;
                            pmm.User_Name = pgmod.User_Name;
                            pmm.IVRMRT_Id = pgmod.IVRMRT_Id;
                            //added by 02/02/2017
                            pmm.CreatedDate = DateTime.Now;
                            pmm.UpdatedDate = DateTime.Now;
                            _StaffLoginContext.Add(pmm);
                            var contactExists = _StaffLoginContext.SaveChanges();

                            if (contactExists == 1)
                            {
                                pgmod.returnval = "Record Saved/Updated Successfully";
                            }
                            else
                            {
                                pgmod.returnval = "Record not Saved/Updated ";
                            }
                        }
                        j++;
                    }
                }

                //if (pgmod.deletesaved.Count()>0)
                //{
                //    while (f < pgmod.deletesaved.Count())
                //    {
                //        var deletesaved = _StaffLoginContext.staffLoginDMO.Single(t => t.IVRMSTAUP_Id == pgmod.deletesaved[f].IVRMSTAUP_Id);

                //        _StaffLoginContext.Remove(deletesaved);
                //        var contactExistsdelete = _StaffLoginContext.SaveChanges();

                //        f++;
                //    }
                //}

                List<long> deletegrid = new List<long>();

                for (int i = 0; i < pgmod.deletesaved.Count(); i++)
                {
                    deletegrid.Add(pgmod.deletesaved[i].IVRMSTAUP_Id);
                }

                List<StaffLoginDMO> lorg = new List<StaffLoginDMO>();
                lorg = _StaffLoginContext.staffLoginDMO.Where(t => deletegrid.Contains(t.IVRMSTAUP_Id)).ToList();

                try
                {
                    if (lorg.Any())
                    {
                        for (int i = 0; i < lorg.Count(); i++)
                        {
                            _StaffLoginContext.Remove(lorg.ElementAt(i));
                        }
                        var contactExists = _StaffLoginContext.SaveChanges();
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }
            else
            {
                if (pgmod.savetmpdata != null)
                {
                    int j = 0;
                    while (j < pgmod.savetmpdata.Count())
                    {
                        StaffLoginDMO pmm = new StaffLoginDMO();

                        if (pgmod.IVRMRT_Id != 0)
                        {
                            pmm.IVRMSTAUP_AddFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_AddFlag);
                            pmm.IVRMSTAUP_DeleteFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_DeleteFlag);
                            pmm.IVRMSTAUP_UpdateFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_UpdateFlag);
                            pmm.IVRMSTAUP_ProcessFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_ProcessFlag);
                            pmm.IVRMSTAUP_ReportFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_ReportFlag);
                            pmm.IVRMRT_Id = pgmod.IVRMRT_Id;
                            pmm.MI_Id = pgmod.MI_Id;
                            pmm.IVRMSTAUL_Id = Convert.ToInt32(pgmod.IVRMSTAUL_Id);
                            pmm.IVRMIMP_Id = pgmod.savetmpdata[j].IVRMIMP_Id;
                            pmm.amc_id = pgmod.AMC_Id;
                            pmm.User_Name = pgmod.User_Name;
                            pmm.IVRMRT_Id = pgmod.IVRMRT_Id;


                            //added by 02/02/2017
                            pmm.CreatedDate = DateTime.Now;
                            pmm.UpdatedDate = DateTime.Now;
                            _StaffLoginContext.Add(pmm);
                            var contactExists = _StaffLoginContext.SaveChanges();

                            if (contactExists == 1)
                            {

                                pgmod.returnval = "Record Saved/Updated Successfully";
                            }
                            else
                            {

                                pgmod.returnval = "Record not Saved/Updated";
                            }
                        }
                        j++;
                    }
                }

            }

            return pgmod;
        }

        public StaffLoginDTO AddUpdateUserPrevileges(StaffLoginDTO pgmod)
        {

            StaffLoginDMO pgmodule = Mapper.Map<StaffLoginDMO>(pgmod);
            int s = 0;
            int f = 0;

            List<long> modulelist = new List<long>();


            List<long> ivrmppages = new List<long>();
            if (pgmod.TempararyArrayList != null)
            {
                for (int i = 0; i < pgmod.TempararyArrayList.Length; i++)
                {
                    modulelist.Add(Convert.ToInt64(pgmod.TempararyArrayList[i].ivrmM_Id));
                }
            }


            if (pgmod.datasaved != null && pgmod.datasaved.Count() > 0)
            {

                //var Getempid = _StaffLoginContext.Staff_User_Login.Single(t => t.Emp_Code == pgmod.datasaved[0].IVRMSTAUL_Id);
                while (s < pgmod.datasaved.Count())
                {
                    //List<StaffLoginDMO> resultsaved = new List<StaffLoginDMO>();
                    //resultsaved = _StaffLoginContext.staffLoginDMO.Where(t => t.IVRMSTAUP_Id == pgmod.datasaved[s].IVRMSTAUP_Id).ToList();
                    //if(resultsaved.Count()>0)
                    //{
                    var updatetheprevileges = _StaffLoginContext.UserLoginPrivileges.Single(t => t.IVRMULP_Id == pgmod.datasaved[s].IVRMSTAUP_Id);

                    updatetheprevileges.IVRMSTUUP_AddFlag = Convert.ToBoolean(pgmod.datasaved[s].IVRMRP_AddFlag);
                    updatetheprevileges.IVRMSTUUP_DeleteFlag = Convert.ToBoolean(pgmod.datasaved[s].IVRMRP_DeleteFlag);
                    updatetheprevileges.IVRMSTUUP_ProcessFlag = Convert.ToBoolean(pgmod.datasaved[s].IVRMRP_ProcessFlag);
                    updatetheprevileges.IVRMSTUUP_UpdateFlag = Convert.ToBoolean(pgmod.datasaved[s].IVRMRP_UpdateFlag);
                    updatetheprevileges.IVRMSTUUP_ReportFlag = Convert.ToBoolean(pgmod.datasaved[s].IVRMRP_ReportFlag);
                    //updatetheprevileges.UpdatedDate = DateTime.Now;
                    _StaffLoginContext.Update(updatetheprevileges);
                    var contactExists = _StaffLoginContext.SaveChanges();
                    //}
                    s++;
                }

                if (pgmod.multipleinsti != null && pgmod.multipleinsti.Length > 0)
                {
                    for (int i = 0; i < pgmod.multipleinsti.Length; i++)
                    {

                        if (pgmod.savetmpdata != null)
                        {
                            int j = 0;
                            while (j < pgmod.savetmpdata.Count())
                            {
                                List<Institution_Module_Page> modulepage = new List<Institution_Module_Page>();

                                modulepage = (from a in _StaffLoginContext.institutionRolePrivileges
                                              from b in _StaffLoginContext.institution_Module_Page
                                              from c in _StaffLoginContext.masterRoleType
                                              from d in _StaffLoginContext.masterPage
                                              from e in _StaffLoginContext.institution_Module
                                              from h in _StaffLoginContext.institution
                                              from g in _StaffLoginContext.masterModule
                                              where (b.IVRMIMP_Id == a.IVRMIMP_Id && c.IVRMRT_Id == a.IVRMRT_Id &&
                                              d.IVRMP_Id == b.IVRMP_Id && b.IVRMIM_Id == e.IVRMIM_Id && e.MI_Id == h.MI_Id && e.IVRMM_Id == g.IVRMM_Id &&
                                              a.IVRMRT_Id == pgmod.IVRMRT_Id && modulelist.Contains(e.IVRMM_Id) && e.MI_Id == pgmod.multipleinsti[i].mi_id && d.IVRMP_Id == pgmod.savetmpdata[j].IVRMP_Id
                                              )
                                              select new Institution_Module_Page
                                              {
                                                  IVRMIMP_Id = a.IVRMIMP_Id,
                                                  IVRMP_Id = d.IVRMP_Id
                                              }
).ToList();
                                if (modulepage.Count() > 0)
                                {
                                    UserLoginPrivileges mas = new UserLoginPrivileges();
                                    var Count = _StaffLoginContext.UserLoginPrivileges.Where(t => t.MI_Id == pgmod.multipleinsti[i].mi_id && t.Id == pgmod.NewUserId && t.IVRMIMP_Id == modulepage.FirstOrDefault().IVRMIMP_Id).ToList();
                                    if (Count.Count() == 0)
                                    {
                                        UserLoginPrivileges pmm = new UserLoginPrivileges();

                                        if (pgmod.IVRMRT_Id != 0)
                                        {
                                            pmm.IVRMSTUUP_AddFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_AddFlag);
                                            pmm.IVRMSTUUP_DeleteFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_DeleteFlag);
                                            pmm.IVRMSTUUP_UpdateFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_UpdateFlag);
                                            pmm.IVRMSTUUP_ProcessFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_ProcessFlag);
                                            pmm.IVRMSTUUP_ReportFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_ReportFlag);
                                            pmm.Id = pgmod.NewUserId;
                                            pmm.MI_Id = pgmod.multipleinsti[i].mi_id;
                                            pmm.IVRMIMP_Id = pgmod.savetmpdata[j].IVRMIMP_Id;
                                            //pmm.CreatedDate = DateTime.Now;
                                            //pmm.UpdatedDate = DateTime.Now;
                                            _StaffLoginContext.Add(pmm);
                                            var contactExists = _StaffLoginContext.SaveChanges();

                                            if (contactExists == 1)
                                            {
                                                pgmod.returnval = "Record Saved/Updated Successfully";
                                            }
                                            else
                                            {
                                                pgmod.returnval = "Record not Saved/Updated ";
                                            }
                                        }

                                    }
                                }

                                j++;
                            }
                        }

                    }
                }
                List<long> deletegrid = new List<long>();
                //List<long> deletegriduserprivileges = new List<long>();

                for (int i = 0; i < pgmod.deletesaved.Count(); i++)
                {
                    deletegrid.Add(pgmod.deletesaved[i].IVRMSTAUP_Id);
                }
                List<UserLoginPrivileges> lorguser = new List<UserLoginPrivileges>();
                lorguser = _StaffLoginContext.UserLoginPrivileges.Where(t => deletegrid.Contains(t.IVRMULP_Id)).ToList();

                try
                {
                    if (lorguser.Any())
                    {
                        for (int i = 0; i < lorguser.Count(); i++)
                        {
                            _StaffLoginContext.Remove(lorguser.ElementAt(i));
                        }
                        var contactExists = _StaffLoginContext.SaveChanges();
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }


            if (pgmod.savetmpdata != null)
            {
                if (pgmod.savetmpdata != null && (pgmod.multipleinsti == null || pgmod.multipleinsti.Length == 0))
                {
                    int j = 0;
                    while (j < pgmod.savetmpdata.Count())
                    {
                        UserLoginPrivileges pmm = new UserLoginPrivileges();

                        if (pgmod.IVRMRT_Id != 0)
                        {
                            pmm.IVRMSTUUP_AddFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_AddFlag);
                            pmm.IVRMSTUUP_DeleteFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_DeleteFlag);
                            pmm.IVRMSTUUP_UpdateFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_UpdateFlag);
                            pmm.IVRMSTUUP_ProcessFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_ProcessFlag);
                            pmm.IVRMSTUUP_ReportFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_ReportFlag);
                            pmm.MI_Id = pgmod.MI_Id;
                            pmm.Id = pgmod.NewUserId;
                            pmm.IVRMIMP_Id = pgmod.savetmpdata[j].IVRMIMP_Id;
                            //pmm.CreatedDate = DateTime.Now;
                            //pmm.UpdatedDate = DateTime.Now;
                            _StaffLoginContext.Add(pmm);
                            var contactExists = _StaffLoginContext.SaveChanges();

                            if (contactExists == 1)
                            {

                                pgmod.returnval = "Record Saved/Updated Successfully";
                            }
                            else
                            {

                                pgmod.returnval = "Record not Saved/Updated";
                            }
                        }
                        j++;
                    }
                }


                if (pgmod.multipleinsti != null && pgmod.multipleinsti.Length > 0)
                {
                    for (int i = 0; i < pgmod.multipleinsti.Length; i++)
                    {

                        if (pgmod.savetmpdata != null)
                        {
                            int j = 0;
                            while (j < pgmod.savetmpdata.Count())
                            {
                                List<Institution_Module_Page> modulepage = new List<Institution_Module_Page>();

                                modulepage = (from a in _StaffLoginContext.institutionRolePrivileges
                                              from b in _StaffLoginContext.institution_Module_Page
                                              from c in _StaffLoginContext.masterRoleType
                                              from d in _StaffLoginContext.masterPage
                                              from e in _StaffLoginContext.institution_Module
                                              from h in _StaffLoginContext.institution
                                              from g in _StaffLoginContext.masterModule
                                              where (b.IVRMIMP_Id == a.IVRMIMP_Id && c.IVRMRT_Id == a.IVRMRT_Id &&
                                              d.IVRMP_Id == b.IVRMP_Id && b.IVRMIM_Id == e.IVRMIM_Id && e.MI_Id == h.MI_Id && e.IVRMM_Id == g.IVRMM_Id &&
                                              a.IVRMRT_Id == pgmod.IVRMRT_Id && modulelist.Contains(e.IVRMM_Id) && e.MI_Id == pgmod.multipleinsti[i].mi_id && b.IVRMIMP_Id == pgmod.savetmpdata[j].IVRMIMP_Id
                                              )
                                              select new Institution_Module_Page
                                              {
                                                  IVRMIMP_Id = a.IVRMIMP_Id,
                                                  IVRMP_Id = d.IVRMP_Id
                                              }
).ToList();
                                if (modulepage.Count() > 0)
                                {
                                    List<UserLoginPrivileges> mas = new List<UserLoginPrivileges>();
                                    mas = _StaffLoginContext.UserLoginPrivileges.Where(t => t.MI_Id == pgmod.multipleinsti[i].mi_id && t.Id == pgmod.NewUserId && t.IVRMIMP_Id == modulepage.FirstOrDefault().IVRMIMP_Id).ToList();
                                    if (mas.Count() == 0)
                                    {
                                        UserLoginPrivileges pmm = new UserLoginPrivileges();

                                        if (pgmod.IVRMRT_Id != 0)
                                        {
                                            pmm.IVRMSTUUP_AddFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_AddFlag);
                                            pmm.IVRMSTUUP_DeleteFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_DeleteFlag);
                                            pmm.IVRMSTUUP_UpdateFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_UpdateFlag);
                                            pmm.IVRMSTUUP_ProcessFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_ProcessFlag);
                                            pmm.IVRMSTUUP_ReportFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_ReportFlag);
                                            pmm.Id = pgmod.NewUserId;
                                            pmm.MI_Id = pgmod.multipleinsti[i].mi_id;
                                            pmm.IVRMIMP_Id = pgmod.savetmpdata[j].IVRMIMP_Id;
                                            //pmm.CreatedDate = DateTime.Now;
                                            //pmm.UpdatedDate = DateTime.Now;
                                            _StaffLoginContext.Add(pmm);
                                            var contactExists = _StaffLoginContext.SaveChanges();

                                            if (contactExists == 1)
                                            {
                                                pgmod.returnval = "Record Saved/Updated Successfully";
                                            }
                                            else
                                            {
                                                pgmod.returnval = "Record not Saved/Updated ";
                                            }
                                        }

                                    }
                                }

                                j++;
                            }
                        }

                    }
                }

            }

            if (pgmod.savetmpdatamobile != null)
            {
                if (pgmod.savetmpdatamobile != null)
                {
                    int j = 0;
                    while (j < pgmod.savetmpdatamobile.Count())
                    {
                        IVRM_User_MobileApp_Login_Privileges pmm = new IVRM_User_MobileApp_Login_Privileges();

                        if (pgmod.IVRMRT_Id != 0)
                        {
                            pmm.MI_Id = pgmod.MI_Id;
                            pmm.IVRMUL_Id = pgmod.NewUserId;
                            pmm.IVRMUMALP_ActiveFlg = true;
                            pmm.IVRMMAP_Id = Convert.ToInt64(pgmod.savetmpdatamobile[j].IVRMMAP_Id);
                            pmm.IVRMUMALP_AddFlg = pgmod.savetmpdatamobile[j].IVRMMAP_AddFlg;
                            pmm.IVRMUMALP_UpdateFlg = pgmod.savetmpdatamobile[j].IVRMMAP_UpdateFlg;
                            pmm.IVRMUMALP_DeleteFlg = pgmod.savetmpdatamobile[j].IVRMMAP_DeleteFlg;
                            pmm.CreatedDate = DateTime.Now;
                            pmm.UpdatedDate = DateTime.Now;
                            _StaffLoginContext.Add(pmm);
                            var contactExists = _StaffLoginContext.SaveChanges();

                            if (contactExists == 1)
                            {

                                pgmod.returnval = "Record Saved/Updated Successfully";
                            }
                            else
                            {

                                pgmod.returnval = "Record not Saved/Updated";
                            }
                        }
                        j++;
                    }
                }

            }

            if (pgmod.updatemobilepagespre != null)
            {
                if (pgmod.updatemobilepagespre.Length>0)
                {
                    int j = 0;
                    while (j < pgmod.updatemobilepagespre.Count())
                    {                     

                        var updatetheprevileges = _StaffLoginContext.IVRM_User_MobileApp_Login_Privileges.Single(t => t.IVRMUMALP_Id == pgmod.updatemobilepagespre[j].IVRMUMALP_Id);

                        updatetheprevileges.IVRMUMALP_AddFlg = pgmod.updatemobilepagespre[j].IVRMUMALP_AddFlg;
                        updatetheprevileges.IVRMUMALP_DeleteFlg = pgmod.updatemobilepagespre[j].IVRMUMALP_DeleteFlg;
                        updatetheprevileges.IVRMUMALP_UpdateFlg = pgmod.updatemobilepagespre[j].IVRMUMALP_UpdateFlg;                   
                        updatetheprevileges.UpdatedDate = DateTime.Now;
                        _StaffLoginContext.Update(updatetheprevileges);
                        var contactExists = _StaffLoginContext.SaveChanges();                     
                        j++;
                    }
                }

            }

            if (pgmod.deletmobile != null)
            {
                if (pgmod.deletmobile.Length > 0)
                {
                    List<long> deletemobilegrid = new List<long>();
                    //List<long> deletegriduserprivileges = new List<long>();

                    for (int i = 0; i < pgmod.deletmobile.Count(); i++)
                    {

                        deletemobilegrid.Add(pgmod.deletmobile[i].IVRMUMALP_Id);
                    }


                    List<IVRM_User_MobileApp_Login_Privileges> lorguser = new List<IVRM_User_MobileApp_Login_Privileges>();
                    lorguser = _StaffLoginContext.IVRM_User_MobileApp_Login_Privileges.Where(t => deletemobilegrid.Contains(t.IVRMUMALP_Id)).ToList();

                    try
                    {
                        if (lorguser.Any())
                        {
                            for (int i = 0; i < lorguser.Count(); i++)
                            {
                                _StaffLoginContext.Remove(lorguser.ElementAt(i));
                            }
                            var contactExists = _StaffLoginContext.SaveChanges();
                        }
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

            }



            return pgmod;
        }

        public StaffLoginDTO AddUpdateUserPrevilegesMultiuser(StaffLoginDTO pgmod)
        {

            StaffLoginDMO pgmodule = Mapper.Map<StaffLoginDMO>(pgmod);
            int s = 0;
            int f = 0;

            List<long> modulelist = new List<long>();


            List<long> ivrmppages = new List<long>();
            if (pgmod.TempararyArrayList != null)
            {
                for (int i = 0; i < pgmod.TempararyArrayList.Length; i++)
                {
                    modulelist.Add(Convert.ToInt64(pgmod.TempararyArrayList[i].ivrmM_Id));
                }
            }

            for (int i = 0; i < pgmod.multiplestaff.Length; i++)
            {
                if (pgmod.savetmpdata != null)
                {
                    if (pgmod.savetmpdata != null)
                    {
                        int j = 0;
                        while (j < pgmod.savetmpdata.Count())
                        {
                            UserLoginPrivileges pmm = new UserLoginPrivileges();

                            UserLoginPrivileges mas = new UserLoginPrivileges();
                            var Count = _StaffLoginContext.UserLoginPrivileges.Where(t => t.MI_Id == pgmod.MI_Id && t.IVRMIMP_Id == pgmod.savetmpdata[j].IVRMIMP_Id && t.Id == pgmod.multiplestaff[i].IVRMSTAUL_Id).ToList();
                            if (Count.Count() == 0)
                            {
                                pmm.IVRMSTUUP_AddFlag =Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_AddFlag);
                                pmm.IVRMSTUUP_DeleteFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_DeleteFlag);
                                pmm.IVRMSTUUP_UpdateFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_UpdateFlag);
                                pmm.IVRMSTUUP_ProcessFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_ProcessFlag);
                                pmm.IVRMSTUUP_ReportFlag = Convert.ToBoolean(pgmod.savetmpdata[j].IVRMRP_ReportFlag);
                                pmm.MI_Id = pgmod.MI_Id;
                                pmm.Id = pgmod.multiplestaff[i].IVRMSTAUL_Id;
                                pmm.IVRMIMP_Id = pgmod.savetmpdata[j].IVRMIMP_Id;
                                //pmm.CreatedDate = DateTime.Now;
                                //pmm.UpdatedDate = DateTime.Now;
                                _StaffLoginContext.Add(pmm);
                                var contactExists = _StaffLoginContext.SaveChanges();

                                if (contactExists == 1)
                                {
                                    pgmod.returnval = "Record Saved/Updated Successfully";
                                }
                                else
                                {
                                    pgmod.returnval = "Record not Saved/Updated";
                                }
                            }
                            j++;
                        }
                    }

                }

            }

            for (int i = 0; i < pgmod.multiplestaff.Length; i++)
            {
                if (pgmod.savetmpdatamobile != null)
                {
                    if (pgmod.savetmpdatamobile != null)
                    {
                        int j = 0;
                        while (j < pgmod.savetmpdatamobile.Count())
                        {
                            IVRM_User_MobileApp_Login_Privileges pmm = new IVRM_User_MobileApp_Login_Privileges();

                            IVRM_User_MobileApp_Login_Privileges mas = new IVRM_User_MobileApp_Login_Privileges();
                            var Count = _StaffLoginContext.IVRM_User_MobileApp_Login_Privileges.Where(t => t.MI_Id == pgmod.MI_Id && t.IVRMMAP_Id == pgmod.savetmpdatamobile[j].IVRMMAP_Id && t.IVRMUL_Id == pgmod.multiplestaff[i].IVRMSTAUL_Id).ToList();
                            if (Count.Count() == 0)
                            {
                                pmm.MI_Id = pgmod.MI_Id;
                                pmm.IVRMUL_Id = pgmod.multiplestaff[i].IVRMSTAUL_Id;
                                pmm.IVRMUMALP_ActiveFlg = true;
                                pmm.IVRMMAP_Id = Convert.ToInt64(pgmod.savetmpdatamobile[j].IVRMMAP_Id);
                                pmm.IVRMUMALP_AddFlg = pgmod.savetmpdatamobile[j].IVRMMAP_AddFlg;
                                pmm.IVRMUMALP_UpdateFlg = pgmod.savetmpdatamobile[j].IVRMMAP_UpdateFlg;
                                pmm.IVRMUMALP_DeleteFlg = pgmod.savetmpdatamobile[j].IVRMMAP_DeleteFlg;
                                pmm.CreatedDate = DateTime.Now;
                                pmm.UpdatedDate = DateTime.Now;
                                _StaffLoginContext.Add(pmm);
                                var contactExists = _StaffLoginContext.SaveChanges();

                                if (contactExists == 1)
                                {
                                    pgmod.returnval = "Record Saved/Updated Successfully";
                                }
                                else
                                {
                                    pgmod.returnval = "Record not Saved/Updated";
                                }
                            }
                            j++;
                        }
                    }

                }
            }
            return pgmod;
        }

        public StaffLoginDTO deleterec(StaffLoginDTO id)
        {
            List<long> deletegrid = new List<long>();

            for (int i = 0; i < id.TempararyArrayListdelete.Length; i++)
            {
                deletegrid.Add(id.TempararyArrayListdelete[i].ivrmstauP_Id);
            }


            StaffLoginDTO page = new StaffLoginDTO();
            List<UserLoginPrivileges> lorg = new List<UserLoginPrivileges>();
            lorg = _StaffLoginContext.UserLoginPrivileges.Where(t => deletegrid.Contains(t.IVRMULP_Id)).ToList();

            try
            {
                if (lorg.Any())
                {
                    for (int i = 0; i < lorg.Count(); i++)
                    {
                        //deletegrid.Add(Convert.ToInt64(id.TempararyArrayListdelete[i].ivrmstauP_Id));
                        _StaffLoginContext.Remove(lorg.ElementAt(i));
                    }


                    var contactExists = _StaffLoginContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        page.returnval = "Data Deleted Successfully";
                    }
                    else
                    {
                        page.returnval = "Data Not Deleted Successfully";
                    }
                }


                page.thirdgriddata = (from a in _StaffLoginContext.UserLoginPrivileges
                                      from app in _StaffLoginContext.ApplicationUser
                                      from b in _StaffLoginContext.institution
                                      from d in _StaffLoginContext.institution_Module_Page
                                      from e in _StaffLoginContext.institution_Module
                                      from f in _StaffLoginContext.masterModule
                                      from g in _StaffLoginContext.masterPage
                                      where (a.MI_Id == b.MI_Id && d.IVRMIMP_Id == a.IVRMIMP_Id &&
                                      d.IVRMIMP_Id == a.IVRMIMP_Id && e.IVRMIM_Id == d.IVRMIM_Id &&
                                      e.IVRMM_Id == f.IVRMM_Id && a.MI_Id == b.MI_Id && a.Id == app.Id &&
                                      d.IVRMP_Id == g.IVRMP_Id && b.MI_Id == id.MI_Id)
                                      select new StaffLoginDTO
                                      {
                                          User_Name = app.UserName,
                                          ivrmM_ModuleName = f.IVRMM_ModuleName,
                                          MI_Name = b.MI_Name,
                                          IVRMSTAUP_Id = a.IVRMULP_Id,
                                          IVRMMP_PageName = g.IVRMMP_PageName,
                                          staffusername = app.Name
                                      }
 ).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public StaffLoginDTO getmoduleroledetails(int id)
        {
            StaffLoginDTO stafflogin = new StaffLoginDTO();
            try
            {


                stafflogin.fillmodule = (from a in _StaffLoginContext.institution_Module
                                         from b in _StaffLoginContext.masterModule
                                         where (a.IVRMM_Id == b.IVRMM_Id && a.MI_Id == id)
                                         select new StaffLoginDTO
                                         {
                                             ivrmM_ModuleName = b.IVRMM_ModuleName,
                                             IVRMM_Id = b.IVRMM_Id,
                                         }
    ).ToArray();

                stafflogin.fillroletype = (from a in _StaffLoginContext.institutionRolePrivileges
                                           from b in _StaffLoginContext.masterRoleType
                                           from c in _StaffLoginContext.institution_Module_Page
                                           from d in _StaffLoginContext.institution_Module
                                           where (a.IVRMRT_Id == b.IVRMRT_Id && c.IVRMIMP_Id == a.IVRMIMP_Id && d.IVRMIM_Id == c.IVRMIM_Id && d.MI_Id == id)
                                           select b
    ).ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stafflogin;
        }

        public StaffLoginDTO getfilterdet(int filtype, StaffLoginDTO data)
        {
            string filetype = "All";
            StaffLoginDTO org = new StaffLoginDTO();
            try
            {
                List<StaffLoginDMO> lorg = new List<StaffLoginDMO>();
                if (data.ivrmM_ModuleName == "Module Name")
                {
                    org.thirdgriddata = (from a in _StaffLoginContext.staffLoginDMO
                                         from b in _StaffLoginContext.institution
                                         from c in _StaffLoginContext.masterStaff
                                         from d in _StaffLoginContext.institution_Module_Page
                                         from e in _StaffLoginContext.institution_Module
                                         from f in _StaffLoginContext.masterModule
                                         from g in _StaffLoginContext.masterPage
                                         where (a.MI_Id == b.MI_Id && c.HRME_Id == a.IVRMSTAUL_Id && d.IVRMIMP_Id == a.IVRMIMP_Id && e.IVRMIM_Id == d.IVRMIM_Id && e.IVRMM_Id == f.IVRMM_Id && d.IVRMP_Id == g.IVRMP_Id && f.IVRMM_ModuleName.Contains(data.IVRMMP_PageName))
                                         select new StaffLoginDTO
                                         {
                                             User_Name = c.HRME_EmployeeFirstName + ' ' + c.HRME_EmployeeMiddleName + ' ' + c.HRME_EmployeeLastName,
                                             ivrmM_ModuleName = f.IVRMM_ModuleName,
                                             MI_Name = b.MI_Name,
                                             IVRMSTAUP_Id = a.IVRMSTAUP_Id,
                                             IVRMMP_PageName = g.IVRMMP_PageName
                                         }
     ).ToArray();
                }
                if (data.ivrmM_ModuleName == "Staff Name")
                {
                    org.thirdgriddata = (from a in _StaffLoginContext.staffLoginDMO
                                         from b in _StaffLoginContext.institution
                                         from c in _StaffLoginContext.masterStaff
                                         from d in _StaffLoginContext.institution_Module_Page
                                         from e in _StaffLoginContext.institution_Module
                                         from f in _StaffLoginContext.masterModule
                                         from g in _StaffLoginContext.masterPage
                                         where (a.MI_Id == b.MI_Id && c.HRME_Id == a.IVRMSTAUL_Id && d.IVRMIMP_Id == a.IVRMIMP_Id && e.IVRMIM_Id == d.IVRMIM_Id && e.IVRMM_Id == f.IVRMM_Id && d.IVRMP_Id == g.IVRMP_Id && c.HRME_EmployeeFirstName.Contains(data.IVRMMP_PageName))
                                         select new StaffLoginDTO
                                         {
                                             User_Name = c.HRME_EmployeeFirstName + ' ' + c.HRME_EmployeeMiddleName + ' ' + c.HRME_EmployeeLastName,
                                             ivrmM_ModuleName = f.IVRMM_ModuleName,
                                             MI_Name = b.MI_Name,
                                             IVRMSTAUP_Id = a.IVRMSTAUP_Id,
                                             IVRMMP_PageName = g.IVRMMP_PageName
                                         }
      ).ToArray();
                }

                if (data.ivrmM_ModuleName == "Institution Name")
                {
                    org.thirdgriddata = (from a in _StaffLoginContext.staffLoginDMO
                                         from b in _StaffLoginContext.institution
                                         from c in _StaffLoginContext.masterStaff
                                         from d in _StaffLoginContext.institution_Module_Page
                                         from e in _StaffLoginContext.institution_Module
                                         from f in _StaffLoginContext.masterModule
                                         from g in _StaffLoginContext.masterPage
                                         where (a.MI_Id == b.MI_Id && c.HRME_Id == a.IVRMSTAUL_Id && d.IVRMIMP_Id == a.IVRMIMP_Id && e.IVRMIM_Id == d.IVRMIM_Id && e.IVRMM_Id == f.IVRMM_Id && d.IVRMP_Id == g.IVRMP_Id && b.MI_Name.Contains(data.IVRMMP_PageName))
                                         select new StaffLoginDTO
                                         {
                                             User_Name = c.HRME_EmployeeFirstName + ' ' + c.HRME_EmployeeMiddleName + ' ' + c.HRME_EmployeeLastName,
                                             ivrmM_ModuleName = f.IVRMM_ModuleName,
                                             MI_Name = b.MI_Name,
                                             IVRMSTAUP_Id = a.IVRMSTAUP_Id,
                                             IVRMMP_PageName = g.IVRMMP_PageName
                                         }
       ).ToArray();
                }
                if (data.ivrmM_ModuleName == "All")
                {
                    org.thirdgriddata = (from a in _StaffLoginContext.staffLoginDMO
                                         from b in _StaffLoginContext.institution
                                         from c in _StaffLoginContext.masterStaff
                                         from d in _StaffLoginContext.institution_Module_Page
                                         from e in _StaffLoginContext.institution_Module
                                         from f in _StaffLoginContext.masterModule
                                         from g in _StaffLoginContext.masterPage
                                         where (a.MI_Id == b.MI_Id && c.HRME_Id == a.IVRMSTAUL_Id && d.IVRMIMP_Id == a.IVRMIMP_Id && e.IVRMIM_Id == d.IVRMIM_Id && e.IVRMM_Id == f.IVRMM_Id && d.IVRMP_Id == g.IVRMP_Id)
                                         select new StaffLoginDTO
                                         {
                                             User_Name = c.HRME_EmployeeFirstName + ' ' + c.HRME_EmployeeMiddleName + ' ' + c.HRME_EmployeeLastName,
                                             ivrmM_ModuleName = f.IVRMM_ModuleName,
                                             MI_Name = b.MI_Name,
                                             IVRMSTAUP_Id = a.IVRMSTAUP_Id,
                                             IVRMMP_PageName = g.IVRMMP_PageName
                                         }
       ).ToArray();
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return org;
        }

        public StaffLoginDTO checkusernmedup(StaffLoginDTO orgdata)
        {
            try
            {
                List<StaffLoginDMO> usrnme = new List<StaffLoginDMO>();
                usrnme = _StaffLoginContext.staffLoginDMO.Where(t => t.User_Name.Contains(orgdata.User_Name) && t.MI_Id == orgdata.MI_Id).ToList();
                orgdata.checkusername = usrnme.ToArray();

                if (orgdata.checkusername.Length > 0)
                {
                    orgdata.returnval = "Already Username exists with the same Institution..Kindly choose any other username";
                }
                else
                {
                    orgdata.returnval = "";
                }
            }
            catch (Exception e)
            {

            }
            return orgdata;
        }

        public StaffLoginDTO changeinstitu(StaffLoginDTO data)
        {
            try
            {
                List<long> miids = new List<long>();

                if (data.multipleinsti.Length > 0)
                {
                    if (data.multipleinsti != null)
                    {
                        for (int i = 0; i < data.multipleinsti.Length; i++)
                        {
                            miids.Add(Convert.ToInt64(data.multipleinsti[i].mi_id));
                        }
                    }
                }
                else
                {
                    miids.Add(data.MI_Id);
                }

                var milist = data.fillmodule;
                data.fillmodule = (from a in _StaffLoginContext.institution_Module
                                   from b in _StaffLoginContext.masterModule
                                   where (a.IVRMM_Id == b.IVRMM_Id && miids.Contains(a.MI_Id))
                                   select new StaffLoginDTO
                                   {
                                       IVRMM_Id = a.IVRMM_Id,
                                       ivrmM_ModuleName = b.IVRMM_ModuleName,
                                   }
                ).Distinct().ToArray();


                data.thirdgriddata = (from a in _StaffLoginContext.UserLoginPrivileges
                                      from app in _StaffLoginContext.ApplicationUser
                                      from b in _StaffLoginContext.institution
                                      from d in _StaffLoginContext.institution_Module_Page
                                      from e in _StaffLoginContext.institution_Module
                                      from f in _StaffLoginContext.masterModule
                                      from g in _StaffLoginContext.masterPage
                                      where (a.MI_Id == b.MI_Id && d.IVRMIMP_Id == a.IVRMIMP_Id &&
                                      d.IVRMIMP_Id == a.IVRMIMP_Id && e.IVRMIM_Id == d.IVRMIM_Id &&
                                      e.IVRMM_Id == f.IVRMM_Id && a.MI_Id == b.MI_Id && a.Id == app.Id &&
                                      d.IVRMP_Id == g.IVRMP_Id && miids.Contains(b.MI_Id))
                                      select new StaffLoginDTO
                                      {
                                          User_Name = app.UserName,
                                          ivrmM_ModuleName = f.IVRMM_ModuleName,
                                          MI_Name = b.MI_Name,
                                          IVRMSTAUP_Id = a.IVRMULP_Id,
                                          IVRMMP_PageName = g.IVRMMP_PageName,
                                          staffusername = app.Name,
                                          IVRMP_Id = g.IVRMP_Id
                                      }
).ToArray();



                data.fillstaff = (from a in _StaffLoginContext.masterStaff
                                  where miids.Contains(a.MI_Id) && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false
                                  select new StaffLoginDTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      stafname = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim() + ":" + a.HRME_EmployeeCode,
                                  }).ToArray();

                data.fillstaffusers = (from a in _StaffLoginContext.masterStaff
                                       from b in _StaffLoginContext.Staff_User_Login
                                       where (a.HRME_Id == b.Emp_Code && miids.Contains(a.MI_Id) && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                       select new StaffLoginDTO
                                       {
                                           HRME_Id = a.HRME_Id,
                                           IVRMSTAUL_Id = Convert.ToInt32(b.Id),
                                           stafname = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim() + ":" + a.HRME_EmployeeCode,
                                       }).ToArray();

                List<MasterCategory> allcategory = new List<MasterCategory>();
                allcategory = _StaffLoginContext.masterCategory.Where(t => miids.Contains(t.MI_Id)).ToList();
                data.fillcategory = allcategory.ToArray();

                if (data.flag == "S")
                {

                    data.studentDetails = (from a in _StaffLoginContext.masterStaff
                                           from b in _StaffLoginContext.Staff_User_Login
                                           from c in _StaffLoginContext.ApplicationUser
                                           from f in _StaffLoginContext.appUserRole
                                           from d in _StaffLoginContext.masterRoleType
                                           where (a.HRME_Id == b.Emp_Code && b.Id == c.Id && miids.Contains(a.MI_Id) && c.Id == f.UserId && f.RoleTypeId == d.IVRMRT_Id && f.RoleTypeId == data.IVRMRT_Id
                                        )
                                           select new StaffLoginDTO
                                           {
                                               HRME_Id = a.HRME_Id,
                                               stafname = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                               HRME_EmployeeCode = a.HRME_EmployeeCode,
                                               User_Name = c.UserName,
                                           }
       ).Distinct().ToArray();
                }
                else if (data.flag == "A")
                {

                    List<long> savedgrid = new List<long>();

                    for (int i = 0; i < data.multipleinsti.Length; i++)
                    {
                        savedgrid.Add(Convert.ToInt64(data.multipleinsti[i].mi_id));
                    }


                    data.studentDetails = (from c in _StaffLoginContext.ApplicationUser
                                           from f in _StaffLoginContext.appUserRole
                                           from d in _StaffLoginContext.masterRoleType
                                           from a in _StaffLoginContext.UserRoleWithInstituteDMO
                                           from m in _StaffLoginContext.institution
                                           where (a.Id == f.UserId && f.RoleTypeId == d.IVRMRT_Id && a.Id == c.Id && a.MI_Id == m.MI_Id && c.Id == f.UserId && savedgrid.Contains(a.MI_Id) && d.IVRMRT_Role.Equals("Admin", StringComparison.OrdinalIgnoreCase)
                                        )
                                           select new StaffLoginDTO
                                           {
                                               rolenamess = d.IVRMRT_Role,
                                               intname = m.MI_Name,
                                               User_Name = c.UserName
                                           }
      ).Distinct().ToArray();
                }
                else
                {
                    data.studentDetails = (from c in _StaffLoginContext.ApplicationUser
                                           from f in _StaffLoginContext.appUserRole
                                           from d in _StaffLoginContext.masterRoleType
                                           from a in _StaffLoginContext.UserRoleWithInstituteDMO
                                           where (a.Id == f.UserId && f.RoleTypeId == d.IVRMRT_Id && a.Id == c.Id && c.Id == f.UserId && miids.Contains(a.MI_Id) && d.IVRMRT_Role.Equals(data.rolenamess, StringComparison.OrdinalIgnoreCase)
                                        )
                                           select new StaffLoginDTO
                                           {
                                               rolenamess = d.IVRMRT_Role,
                                               User_Name = c.UserName
                                           }
).Distinct().ToArray();


                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public StaffLoginDTO searchfilter(StaffLoginDTO data)
        {
            try
            {
                data.searchfilter = data.searchfilter.ToUpper();
                data.fillstaff = (from a in _StaffLoginContext.masterStaff
                                  where a.MI_Id == data.MI_Id && a.HRME_LeftFlag == false && a.HRME_ActiveFlag == true && ((a.HRME_EmployeeFirstName.Trim().ToUpper() + ' ' + a.HRME_EmployeeMiddleName.Trim().ToUpper() + ' ' + a.HRME_EmployeeLastName.Trim().ToUpper()).Contains(data.searchfilter) || (a.HRME_EmployeeFirstName.Trim().ToUpper() + a.HRME_EmployeeMiddleName.Trim().ToUpper() + ' ' + a.HRME_EmployeeLastName.Trim().ToUpper()).Contains(data.searchfilter) || a.HRME_EmployeeFirstName.ToUpper().Contains(data.searchfilter) || a.HRME_EmployeeMiddleName.ToUpper().Contains(data.searchfilter) || a.HRME_EmployeeLastName.ToUpper().Contains(data.searchfilter))
                                  select new StaffLoginDTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      stafname = ((a.HRME_EmployeeFirstName == null ? "" : a.HRME_EmployeeFirstName.ToUpper()) + " " + (a.HRME_EmployeeMiddleName == null ? "" : a.HRME_EmployeeMiddleName.ToUpper()) + " " + (a.HRME_EmployeeLastName == null ? "" : a.HRME_EmployeeLastName.ToUpper())).Trim(),
                                  }).ToArray();

                data.fillstaffusers = (from a in _StaffLoginContext.masterStaff
                                       from b in _StaffLoginContext.Staff_User_Login
                                       where (a.HRME_Id == b.Emp_Code && a.HRME_LeftFlag == false && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && ((a.HRME_EmployeeFirstName.Trim().ToUpper() + ' ' + a.HRME_EmployeeMiddleName.Trim().ToUpper() + ' ' + a.HRME_EmployeeLastName.Trim().ToUpper()).Contains(data.searchfilter) || (a.HRME_EmployeeFirstName.Trim().ToUpper() + a.HRME_EmployeeMiddleName.Trim().ToUpper() + ' ' + a.HRME_EmployeeLastName.Trim().ToUpper()).Contains(data.searchfilter) || a.HRME_EmployeeFirstName.ToUpper().Contains(data.searchfilter) || a.HRME_EmployeeMiddleName.ToUpper().Contains(data.searchfilter) || a.HRME_EmployeeLastName.ToUpper().Contains(data.searchfilter)))
                                       select new StaffLoginDTO
                                       {
                                           HRME_Id = a.HRME_Id,
                                           IVRMSTAUL_Id = Convert.ToInt32(b.Id),
                                           stafname = ((a.HRME_EmployeeFirstName == null ? "" : a.HRME_EmployeeFirstName.ToUpper()) + " " + (a.HRME_EmployeeMiddleName == null ? "" : a.HRME_EmployeeMiddleName.ToUpper()) + " " + (a.HRME_EmployeeLastName == null ? "" : a.HRME_EmployeeLastName.ToUpper())).Trim(),
                                       }).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public StaffLoginDTO checktrustfunction(StaffLoginDTO orgdata)
        {
            try
            {
                List<Institution> allinstitution = new List<Institution>();
                allinstitution = _StaffLoginContext.institution.Where(t => t.MI_ActiveFlag == 1 && t.MO_Id == orgdata.MO_Id).ToList();
                orgdata.fillinstitution = allinstitution.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return orgdata;
        }

        public StaffLoginDTO getstaffmobilepages(StaffLoginDTO orgdata)
        {
            try
            {
                orgdata.thirdgriddatamobileMulti = (from a in _StaffLoginContext.IVRM_MobileApp_Page
                                                    from b in _StaffLoginContext.IVRM_Role_MobileApp_Privileges
                                                    from c in _StaffLoginContext.masterRoleType
                                                    where (a.IVRMMAP_Id == b.IVRMMAP_Id && b.IVRMRT_Id == c.IVRMRT_Id && c.IVRMRT_Id == orgdata.IVRMRT_Id && b.MI_ID == orgdata.MI_Id)
                                                    select new MasterRolePreviledgeDTO
                                                    {
                                                        IVRMMAP_AppPageName = a.IVRMMAP_AppPageName,
                                                        ivrmrT_Role = c.IVRMRT_Role,
                                                        IVRMR_Id = c.IVRMR_Id,
                                                        IVRMRP_Id = b.IVRMRMAP_Id,
                                                        IVRMMAP_Id = a.IVRMMAP_Id,
                                                        IVRMMAP_AddFlg = b.IVRMMAP_AddFlg,
                                                        IVRMMAP_UpdateFlg = b.IVRMMAP_UpdateFlg,
                                                        IVRMMAP_DeleteFlg = b.IVRMMAP_DeleteFlg
                                                    }
           ).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return orgdata;
        }
    }
}
