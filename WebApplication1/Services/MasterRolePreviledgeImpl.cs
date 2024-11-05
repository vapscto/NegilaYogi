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
using DomainModel.Model.com.vapstech.MobileApp;

namespace WebApplication1.Services
{
    public class MasterRolePreviledgeImpl : Interfaces.MasterRolePreviledgeInterface
    {
        private static ConcurrentDictionary<string, MasterRolePreviledgeDTO> _pgmod =
          new ConcurrentDictionary<string, MasterRolePreviledgeDTO>();

        private static ConcurrentDictionary<string, MasterPageDTO> _pgmodd =
         new ConcurrentDictionary<string, MasterPageDTO>();

        private static ConcurrentDictionary<string, MasterPageModuleMappingDTO> _pgmoddd =
        new ConcurrentDictionary<string, MasterPageModuleMappingDTO>();

        public MasterRolePreviledgesContext _MasterRolePreviledgesContext;
        public MasterPageContext _masterPageContext;
        public MasterPageModuleMappingContext _masterPageModuleMappingContext;
        public MasterRolePreviledgeImpl(MasterRolePreviledgesContext MasterRolePreviledgesContext, MasterPageContext masterPageContext, MasterPageModuleMappingContext masterPageModuleMappingContext)
        {
            _MasterRolePreviledgesContext = MasterRolePreviledgesContext;
            _masterPageContext = masterPageContext;
            _masterPageModuleMappingContext = masterPageModuleMappingContext;
        }

        public MasterRolePreviledgeDTO deleterec(int id)
        {
            bool returnresult = false;
            MasterRolePreviledgeDTO page = new MasterRolePreviledgeDTO();
            List<MasterRolePreviledgeDMO> lorg = new List<MasterRolePreviledgeDMO>();
            lorg = _MasterRolePreviledgesContext.masterRolePreviledgeDMO.Where(t => t.IVRMRP_Id.Equals(id)).ToList();

            try
            {
                if (lorg.Any())
                {
                    _MasterRolePreviledgesContext.Remove(lorg.ElementAt(0));

                    var contactExists = _MasterRolePreviledgesContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        returnresult = true;
                        page.returnvalDelete = returnresult;
                    }
                    else
                    {
                        returnresult = false;
                        page.returnvalDelete = returnresult;
                    }
                }

                //List<MasterRolePreviledgeDMO> rolemodulep = new List<MasterRolePreviledgeDMO>();
                //rolemodulep = _MasterRolePreviledgesContext.masterRolePreviledgeDMO.ToList();
                //page.thirdgriddata = rolemodulep.ToArray();

                page.thirdgriddata = (from a in _MasterRolePreviledgesContext.masterPageModuleMapping
                                      from b in _MasterRolePreviledgesContext.masterPage
                                      from c in _MasterRolePreviledgesContext.masterModule
                                      from d in _MasterRolePreviledgesContext.masterRolePreviledgeDMO
                                      from e in _MasterRolePreviledgesContext.masterRoleType
                                      where (d.IVRMMP_Id == a.IVRMMP_Id && a.IVRMM_Id == c.IVRMM_Id && a.IVRMP_Id == b.IVRMP_Id && e.IVRMRT_Id == d.IVRMRT_Id)
                                      select new MasterRolePreviledgeDTO
                                      {
                                          ivrmmP_PageName = b.IVRMMP_PageName,
                                          ivrmM_ModuleName = c.IVRMM_ModuleName,
                                          ivrmrT_Role = e.IVRMRT_Role,
                                          IVRMRP_Id = d.IVRMRP_Id
                                      }
      ).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public MasterRolePreviledgeDTO mobiledeletemodpages(MasterRolePreviledgeDTO dto )
        {
            bool returnresult = false;
            MasterRolePreviledgeDTO page = new MasterRolePreviledgeDTO();
            List<IVRM_Role_MobileApp_Privileges> lorg = new List<IVRM_Role_MobileApp_Privileges>();
            lorg = _MasterRolePreviledgesContext.IVRM_Role_MobileApp_Privileges.Where(t => t.IVRMRMAP_Id == dto.IVRMRMAP_Id).ToList();

            try
            {
                if (lorg.Any())
                {
                    _MasterRolePreviledgesContext.Remove(lorg.ElementAt(0));

                    var contactExists = _MasterRolePreviledgesContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        returnresult = true;
                        page.returnvalDelete = returnresult;
                    }
                    else
                    {
                        returnresult = false;
                        page.returnvalDelete = returnresult;
                    }
                }

                //List<MasterRolePreviledgeDMO> rolemodulep = new List<MasterRolePreviledgeDMO>();
                //rolemodulep = _MasterRolePreviledgesContext.masterRolePreviledgeDMO.ToList();
                //page.thirdgriddata = rolemodulep.ToArray();

                page.thirdgriddata = (from a in _MasterRolePreviledgesContext.IVRM_MobileApp_Page
                                      from b in _MasterRolePreviledgesContext.IVRM_Role_MobileApp_Privileges
                                      from c in _MasterRolePreviledgesContext.masterRoleType
                                      from d in _MasterRolePreviledgesContext.Institute
                                      where (a.IVRMMAP_Id == b.IVRMMAP_Id && b.IVRMRT_Id == c.IVRMRT_Id && b.MI_ID == d.MI_Id)
                                      select new MasterRolePreviledgeDTO
                                      {
                                          IVRMMAP_AppPageName = a.IVRMMAP_AppPageName,
                                          ivrmrT_Role = c.IVRMRT_Role,
                                          IVRMR_Id = c.IVRMR_Id,
                                          IVRMRP_Id = b.IVRMRMAP_Id,
                                          IVRMMAP_Id = a.IVRMMAP_Id,
                                          Institutename = d.MI_Name
                                      }
             ).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public MasterRolePreviledgeDTO getmoduledet(int id)
        {

            MasterRolePreviledgeDTO pgmod = new MasterRolePreviledgeDTO();
            try
            {
                List<MasterRoleType> allmodule = new List<MasterRoleType>();
                allmodule = _MasterRolePreviledgesContext.masterRoleType.ToList();
                pgmod.fillroletype = allmodule.ToArray();

                List<MasterPage> allpages = new List<MasterPage>();
                allpages = _masterPageContext.masterpage.Where(t => t.IVRMP_TemplateFlag == true).ToList();
                pgmod.allsaveddata = allpages.ToArray();


                List<MasterModule> alldata = new List<MasterModule>();
                alldata = _MasterRolePreviledgesContext.masterModule.Where(t => t.Module_ActiveFlag == 1).ToList();
                pgmod.fillmodulepagesdata = alldata.ToArray();

                //List<MasterRolePreviledgeDMO> rolemodulep = new List<MasterRolePreviledgeDMO>();
                //rolemodulep = _MasterRolePreviledgesContext.masterRolePreviledgeDMO.ToList();
                //pgmod.thirdgriddata = rolemodulep.ToArray();


                pgmod.thirdgriddata = (from a in _MasterRolePreviledgesContext.masterPageModuleMapping
                                       from b in _MasterRolePreviledgesContext.masterPage
                                       from c in _MasterRolePreviledgesContext.masterModule
                                       from d in _MasterRolePreviledgesContext.masterRolePreviledgeDMO
                                       from e in _MasterRolePreviledgesContext.masterRoleType
                                       where (d.IVRMMP_Id == a.IVRMMP_Id && a.IVRMM_Id == c.IVRMM_Id && a.IVRMP_Id == b.IVRMP_Id && e.IVRMRT_Id == d.IVRMRT_Id)
                                       select new MasterRolePreviledgeDTO
                                       {
                                           ivrmmP_PageName = b.IVRMMP_PageName,
                                           ivrmM_ModuleName = c.IVRMM_ModuleName,
                                           ivrmrT_Role = e.IVRMRT_Role,
                                           IVRMRP_Id = d.IVRMRP_Id
                                       }
             ).ToArray();



            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return pgmod;
        }

        public MasterRolePreviledgeDTO mobilegetmodulepages(MasterRolePreviledgeDTO id)
        {
            MasterRolePreviledgeDTO pgmod = new MasterRolePreviledgeDTO();
            try
            {
                List<long> pageist = new List<long>();
                pageist = (from a in _MasterRolePreviledgesContext.IVRM_MobileApp_Page
                           from b in _MasterRolePreviledgesContext.IVRM_Role_MobileApp_Privileges
                           from c in _MasterRolePreviledgesContext.masterRoleType
                           where (a.IVRMMAP_Id == b.IVRMMAP_Id && b.IVRMRT_Id == c.IVRMRT_Id && c.IVRMRT_Id == id.IVRMRT_Id && b.MI_ID == id.MI_Id)
                           select a).Distinct().Select(a => a.IVRMMAP_Id).ToList();

                pgmod.previousgrid = (from a in _MasterRolePreviledgesContext.IVRM_MobileApp_Page
                                      from b in _MasterRolePreviledgesContext.IVRM_Role_MobileApp_Privileges
                                      from c in _MasterRolePreviledgesContext.masterRoleType
                                      where (a.IVRMMAP_Id == b.IVRMMAP_Id && b.IVRMRT_Id == c.IVRMRT_Id && c.IVRMRT_Id == id.IVRMRT_Id && b.MI_ID == id.MI_Id)
                                      select new MasterRolePreviledgeDTO
                                      {
                                          IVRMMAP_AppPageName = a.IVRMMAP_AppPageName,
                                          ivrmrT_Role = c.IVRMRT_Role,
                                          IVRMR_Id = c.IVRMR_Id,
                                          IVRMRP_Id = b.IVRMRMAP_Id,
                                          IVRMMAP_Id = a.IVRMMAP_Id,
                                          IVRMMAP_AddFlg=b.IVRMMAP_AddFlg,
                                          IVRMMAP_UpdateFlg = b.IVRMMAP_UpdateFlg,
                                          IVRMMAP_DeleteFlg = b.IVRMMAP_DeleteFlg
                                      }
         ).ToArray();

                if (pageist.Count() > 0)
                {
                    List<IVRM_MobileApp_Page> allpages = new List<IVRM_MobileApp_Page>();
                    allpages = _masterPageContext.IVRM_MobileApp_Page.Where(t => t.IVRMMAP_ActiveFlg == true && !pageist.Contains(t.IVRMMAP_Id)).ToList();
                    pgmod.thirdgriddata = allpages.OrderBy(d => d.IVRMMAP_Id).ToArray();
                }
                else
                {
                    List<IVRM_MobileApp_Page> allpages = new List<IVRM_MobileApp_Page>();
                    allpages = _masterPageContext.IVRM_MobileApp_Page.Where(t => t.IVRMMAP_ActiveFlg == true).ToList();
                    pgmod.thirdgriddata = allpages.OrderBy(d => d.IVRMMAP_Id).ToArray();
                }
                pgmod.firstgrigdata = (from a in _MasterRolePreviledgesContext.IVRM_MobileApp_Page
                                       from b in _MasterRolePreviledgesContext.IVRM_Role_MobileApp_Privileges
                                       from c in _MasterRolePreviledgesContext.masterRoleType
                                       from d in _MasterRolePreviledgesContext.Institute
                                       where (a.IVRMMAP_Id == b.IVRMMAP_Id && b.IVRMRT_Id == c.IVRMRT_Id && b.MI_ID == d.MI_Id)
                                       select new MasterRolePreviledgeDTO
                                       {
                                           IVRMMAP_AppPageName = a.IVRMMAP_AppPageName,
                                           ivrmrT_Role = c.IVRMRT_Role,
                                           IVRMR_Id = c.IVRMR_Id,
                                           IVRMRP_Id = b.IVRMRMAP_Id,
                                           IVRMMAP_Id = a.IVRMMAP_Id,
                                           Institutename = d.MI_Name
                                       }
           ).ToArray();

                List<MasterRoleType> allmodule = new List<MasterRoleType>();
                allmodule = _MasterRolePreviledgesContext.masterRoleType.ToList();
                pgmod.fillroletype = allmodule.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return pgmod;
        }

        public MasterRolePreviledgeDTO mobilegetalldetails(int id)
        {

            MasterRolePreviledgeDTO pgmod = new MasterRolePreviledgeDTO();
            try
            {
                List<MasterRoleType> allmodule = new List<MasterRoleType>();
                allmodule = _MasterRolePreviledgesContext.masterRoleType.ToList();
                pgmod.fillroletype = allmodule.ToArray();

                List<Institution> lorgins = new List<Institution>();
                lorgins = _MasterRolePreviledgesContext.Institute.Where(t => t.MI_ActiveFlag == 1).ToList();
                pgmod.fillinstitution = lorgins.ToArray();

                List<IVRM_MobileApp_Page> allpages = new List<IVRM_MobileApp_Page>();
                allpages = _masterPageContext.IVRM_MobileApp_Page.Where(t => t.IVRMMAP_ActiveFlg == true).ToList();
                pgmod.allsaveddata = allpages.ToArray();


                pgmod.thirdgriddata = (from a in _MasterRolePreviledgesContext.IVRM_MobileApp_Page
                                       from b in _MasterRolePreviledgesContext.IVRM_Role_MobileApp_Privileges
                                       from c in _MasterRolePreviledgesContext.masterRoleType
                                       from d in _MasterRolePreviledgesContext.Institute
                                       where (a.IVRMMAP_Id == b.IVRMMAP_Id && b.IVRMRT_Id == c.IVRMRT_Id && b.MI_ID == d.MI_Id)
                                       select new MasterRolePreviledgeDTO
                                       {
                                           IVRMMAP_AppPageName = a.IVRMMAP_AppPageName,
                                           ivrmrT_Role = c.IVRMRT_Role,
                                           IVRMR_Id = c.IVRMR_Id,
                                           IVRMRP_Id = b.IVRMRMAP_Id,
                                           IVRMMAP_Id = a.IVRMMAP_Id,
                                           Institutename = d.MI_Name
                                       }
             ).OrderBy(d => d.IVRMRP_Id).ToArray();



            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return pgmod;
        }

        public MasterRolePreviledgeDTO saveorgdet(MasterRolePreviledgeDTO pgmod)
        {
            using (var transaction = _MasterRolePreviledgesContext.Database.BeginTransaction())
            {
                MasterRolePreviledgeDTO someObj = new MasterRolePreviledgeDTO();
                try
                {
                    // MasterRolePreviledgeDMO pgmodule = Mapper.Map<MasterRolePreviledgeDMO>(pgmod);
                    // MasterRolePreviledgeDMO pmm = null;
                    string returnresult = "";
                    if (pgmod.previoussavetmpdata != null)
                    {
                        if (pgmod.previoussavetmpdata.Count() > 0)
                        {
                            if (pgmod.savetmpdata != null)
                            {
                                int j = 0;
                                while (j < pgmod.savetmpdata.Count())
                                {
                                    MasterRolePreviledgeDMO pmm = new MasterRolePreviledgeDMO();
                                    // pmm=new MasterRolePreviledgeDMO();
                                    if (pgmod.IVRMRT_Id != 0 && pgmod.savetmpdata[j].IVRMMP_Id != 0)
                                    {
                                        pmm.IVRMRP_AddFlag = pgmod.savetmpdata[j].IVRMRP_AddFlag;
                                        pmm.IVRMRP_DeleteFlag = pgmod.savetmpdata[j].IVRMRP_DeleteFlag;
                                        pmm.IVRMRP_UpdateFlag = pgmod.savetmpdata[j].IVRMRP_UpdateFlag;
                                        pmm.IVRMRP_ProcessFlag = pgmod.savetmpdata[j].IVRMRP_ProcessFlag;
                                        pmm.IVRMRP_ReportFlag = pgmod.savetmpdata[j].IVRMRP_ReportFlag;
                                        pmm.IVRMRT_Id = pgmod.IVRMRT_Id;
                                        pmm.IVRMMP_Id = pgmod.savetmpdata[j].IVRMMP_Id;
                                        //added by 02/02/2017
                                        pmm.CreatedDate = DateTime.Now;
                                        pmm.UpdatedDate = DateTime.Now;
                                        _MasterRolePreviledgesContext.Add(pmm);

                                    }
                                    j++;
                                }

                                var contactExists = _MasterRolePreviledgesContext.SaveChanges();

                                if (contactExists == 1)
                                {
                                    returnresult = "Save";
                                    pgmod.returnval = returnresult;
                                }
                                else
                                {
                                    returnresult = "NotSave";
                                    pgmod.returnval = returnresult;
                                }
                            }

                            if (pgmod.previoussavetmpdata != null)
                            {
                                int p = 0;
                                while (p < pgmod.previoussavetmpdata.Count())
                                {
                                    MasterRolePreviledgeDMO pmm1 = new MasterRolePreviledgeDMO();
                                    // pmm=new MasterRolePreviledgeDMO();
                                    if (pgmod.IVRMRT_Id != 0 && pgmod.previoussavetmpdata[p].IVRMMP_Id != 0)
                                    {
                                        var result = _MasterRolePreviledgesContext.masterRolePreviledgeDMO.Single(t => t.IVRMRP_Id == pgmod.previoussavetmpdata[p].IVRMRP_Id);
                                        result.IVRMRP_AddFlag = pgmod.previoussavetmpdata[p].IVRMRP_AddFlag;
                                        result.IVRMRP_DeleteFlag = pgmod.previoussavetmpdata[p].IVRMRP_DeleteFlag;
                                        result.IVRMRP_UpdateFlag = pgmod.previoussavetmpdata[p].IVRMRP_UpdateFlag;
                                        result.IVRMRP_ProcessFlag = pgmod.previoussavetmpdata[p].IVRMRP_ProcessFlag;
                                        result.IVRMRP_ReportFlag = pgmod.previoussavetmpdata[p].IVRMRP_ReportFlag;
                                        result.IVRMRT_Id = pgmod.IVRMRT_Id;
                                        result.IVRMMP_Id = pgmod.previoussavetmpdata[p].IVRMMP_Id;
                                        //pmm1.IVRMRP_Id=pgmod.previoussavetmpdata[p].IVRMRP_Id;
                                        //added by 02/02/2017

                                        result.UpdatedDate = DateTime.Now;
                                        _MasterRolePreviledgesContext.Update(result);

                                    }
                                    p++;
                                }

                                var contactExists = _MasterRolePreviledgesContext.SaveChanges();

                                if (contactExists >= 1)
                                {
                                    returnresult = "Update";
                                    pgmod.returnval = returnresult;
                                }
                                else
                                {
                                    returnresult = "NotUpdate";
                                    pgmod.returnval = returnresult;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (pgmod.savetmpdata != null)
                        {

                            int j = 0;
                            while (j < pgmod.savetmpdata.Count())
                            {
                                MasterRolePreviledgeDMO pmm = new MasterRolePreviledgeDMO();
                                //pmm= new MasterRolePreviledgeDMO();
                                if (pgmod.IVRMRT_Id != 0 && pgmod.savetmpdata[j].IVRMMP_Id != 0)
                                {
                                    pmm.IVRMRP_AddFlag = pgmod.savetmpdata[j].IVRMRP_AddFlag;
                                    pmm.IVRMRP_DeleteFlag = pgmod.savetmpdata[j].IVRMRP_DeleteFlag;
                                    pmm.IVRMRP_UpdateFlag = pgmod.savetmpdata[j].IVRMRP_UpdateFlag;
                                    pmm.IVRMRP_ProcessFlag = pgmod.savetmpdata[j].IVRMRP_ProcessFlag;
                                    pmm.IVRMRP_ReportFlag = pgmod.savetmpdata[j].IVRMRP_ReportFlag;
                                    pmm.IVRMRT_Id = pgmod.IVRMRT_Id;
                                    pmm.IVRMMP_Id = pgmod.savetmpdata[j].IVRMMP_Id;
                                    //added by 02/02/2017
                                    pmm.CreatedDate = DateTime.Now;
                                    pmm.UpdatedDate = DateTime.Now;
                                    _MasterRolePreviledgesContext.Add(pmm);
                                }
                                j++;
                            }

                            var contactExists = _MasterRolePreviledgesContext.SaveChanges();

                            if (contactExists >= 1)
                            {
                                returnresult = "Save";
                                pgmod.returnval = returnresult;
                            }
                            else
                            {
                                returnresult = "NotSave";
                                pgmod.returnval = returnresult;
                            }

                        }
                    }

                    transaction.Commit();

                    //List<MasterRolePreviledgeDMO> rolemodulep = new List<MasterRolePreviledgeDMO>();
                    //rolemodulep = _MasterRolePreviledgesContext.masterRolePreviledgeDMO.ToList();
                    //pgmod.thirdgriddata = rolemodulep.ToArray();

                    //someObj.enq = (from a in _MasterRolePreviledgesContext.masterPageModuleMapping
                    //               from b in _MasterRolePreviledgesContext.masterPage
                    //               from c in _MasterRolePreviledgesContext.masterModule
                    //               from d in _MasterRolePreviledgesContext.masterRolePreviledgeDMO
                    //               where (d.IVRMMP_Id == a.IVRMMP_Id && a.IVRMM_Id==c.IVRMM_Id && a.IVRMP_Id==b.IVRMP_Id)
                    //               select new MasterRolePreviledgeDTO
                    //               {
                    //                   ivrmmP_PageName = b.IVRMMP_PageName,
                    //                   ivrmM_ModuleName = c.IVRMM_ModuleName
                    //               }
                    // ).ToArray();


                    pgmod.thirdgriddata = (from a in _MasterRolePreviledgesContext.masterPageModuleMapping
                                           from b in _MasterRolePreviledgesContext.masterPage
                                           from c in _MasterRolePreviledgesContext.masterModule
                                           from d in _MasterRolePreviledgesContext.masterRolePreviledgeDMO
                                           from e in _MasterRolePreviledgesContext.masterRoleType
                                           where (d.IVRMMP_Id == a.IVRMMP_Id && a.IVRMM_Id == c.IVRMM_Id && a.IVRMP_Id == b.IVRMP_Id && e.IVRMRT_Id == d.IVRMRT_Id)
                                           select new MasterRolePreviledgeDTO
                                           {
                                               ivrmmP_PageName = b.IVRMMP_PageName,
                                               ivrmM_ModuleName = c.IVRMM_ModuleName,
                                               ivrmrT_Role = e.IVRMRT_Role,
                                               IVRMRP_Id = d.IVRMRP_Id
                                           }
            ).ToArray();



                    //var statelist= _MasterPageModuleMappingContext.masterPage
                    //            .Join(_MasterPageModuleMappingContext.masterPageModuleMapping,
                    //                  maspge => maspge.IVRMP_Id,
                    //                  pgmodmap => pgmodmap.IVRMP_Id,
                    //                  (maspge, pgmodmap) => new
                    //                  {
                    //                      IVRMMP_PageName = maspge.IVRMMP_PageName,
                    //                      IVRMM_Id = pgmodmap.IVRMM_Id,
                    //                  }).Join(_MasterPageModuleMappingContext.masterModule,
                    //                   pgmodmap => pgmodmap.IVRMM_Id,
                    //                   pge => pge.IVRMM_Id,
                    //                  (pgmodmap, pge) => new
                    //                  {
                    //                      IVRMMP_PageName = pgmodmap.IVRMMP_PageName,
                    //                      IVRMM_ModuleName = pge.IVRMM_ModuleName
                    //                  });

                }

                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }

            return pgmod;
        }

        public MasterRolePreviledgeDTO mobilesaveorgdet(MasterRolePreviledgeDTO pgmod)
        {
            using (var transaction = _MasterRolePreviledgesContext.Database.BeginTransaction())
            {
                MasterRolePreviledgeDTO someObj = new MasterRolePreviledgeDTO();
                try
                {

                    string returnresult = "";


                    if (pgmod.savetmpdata != null)
                    {

                        int j = 0;
                        while (j < pgmod.savetmpdata.Count())
                        {
                            IVRM_Role_MobileApp_Privileges pmm = new IVRM_Role_MobileApp_Privileges();
                            //pmm= new MasterRolePreviledgeDMO();
                            if (pgmod.IVRMRT_Id != 0)
                            {
                                pmm.IVRMRT_Id = pgmod.IVRMRT_Id;
                                pmm.MI_ID = pgmod.MI_Id;
                                pmm.IVRMMAP_Id = pgmod.savetmpdata[j].IVRMMAP_Id;
                                pmm.IVRMRMAP_ActiveFlg = true;
                                pmm.IVRMMAP_AddFlg = pgmod.savetmpdata[j].IVRMMAP_AddFlg;
                                pmm.IVRMMAP_UpdateFlg = pgmod.savetmpdata[j].IVRMMAP_UpdateFlg;
                                pmm.IVRMMAP_DeleteFlg = pgmod.savetmpdata[j].IVRMMAP_DeleteFlg;
                                pmm.CreatedDate = DateTime.Now;
                                pmm.UpdatedDate = DateTime.Now;
                                _MasterRolePreviledgesContext.Add(pmm);
                            }
                            j++;
                        }

                        var contactExists = _MasterRolePreviledgesContext.SaveChanges();

                        if (contactExists >= 1)
                        {
                            returnresult = "Save";
                            pgmod.returnval = returnresult;
                        }
                        else
                        {
                            returnresult = "NotSave";
                            pgmod.returnval = returnresult;
                        }

                    }


                    transaction.Commit();



                    pgmod.thirdgriddata = (from a in _MasterRolePreviledgesContext.IVRM_MobileApp_Page
                                           from b in _MasterRolePreviledgesContext.IVRM_Role_MobileApp_Privileges
                                           from c in _MasterRolePreviledgesContext.masterRoleType
                                           from d in _MasterRolePreviledgesContext.Institute
                                           where (a.IVRMMAP_Id == b.IVRMMAP_Id && b.IVRMRT_Id == c.IVRMRT_Id && b.MI_ID == d.MI_Id)
                                           select new MasterRolePreviledgeDTO
                                           {
                                               IVRMMAP_AppPageName = a.IVRMMAP_AppPageName,
                                               ivrmrT_Role = c.IVRMRT_Role,
                                               IVRMR_Id = c.IVRMR_Id,
                                               IVRMRP_Id = b.IVRMRMAP_Id,
                                               IVRMMAP_Id = a.IVRMMAP_Id,
                                               Institutename = d.MI_Name
                                           }
                ).ToArray();



                }

                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }

            return pgmod;
        }

        public MasterRolePreviledgeDTO getmodulepagedata(MasterRolePreviledgeDTO someObj)
        {
            //MasterRolePreviledgeDTO someObj = new MasterRolePreviledgeDTO();
            try
            {
                //var savedpagelist = (from a in _MasterRolePreviledgesContext.masterPageModuleMapping
                //                     from b in _MasterRolePreviledgesContext.masterPage
                //                     from c in _MasterRolePreviledgesContext.masterRolePreviledgeDMO
                //                     where (a.IVRMP_Id == b.IVRMP_Id && a.IVRMMP_Id == c.IVRMMP_Id && a.IVRMM_Id == someObj.IVRMMP_Id && c.IVRMRT_Id == someObj.IVRMRT_Id)
                //                     select new MasterRolePreviledgeDTO
                //                     {
                //                         ivrmmP_PageName = b.IVRMMP_PageName,
                //                     }
                //     );

                List<long> deletegrid = new List<long>();

                for (int i = 0; i < someObj.savetmpdata.Count(); i++)
                {
                    deletegrid.Add(someObj.savetmpdata[i].IVRMM_Id);
                }

                someObj.previosgriddata = (from a in _MasterRolePreviledgesContext.masterPageModuleMapping
                                           from b in _MasterRolePreviledgesContext.masterPage
                                           from c in _MasterRolePreviledgesContext.masterRolePreviledgeDMO
                                           where (a.IVRMP_Id == b.IVRMP_Id && a.IVRMMP_Id == c.IVRMMP_Id && deletegrid.Contains(a.IVRMM_Id) && c.IVRMRT_Id == someObj.IVRMRT_Id)
                                           select new MasterRolePreviledgeDTO
                                           {
                                               IVRMRT_Id = c.IVRMRT_Id,
                                               ivrmmP_PageName = b.IVRMMP_PageName,
                                               IVRMRP_AddFlag = c.IVRMRP_AddFlag,
                                               IVRMRP_UpdateFlag = c.IVRMRP_UpdateFlag,
                                               IVRMRP_DeleteFlag = c.IVRMRP_DeleteFlag,
                                               IVRMRP_ReportFlag = c.IVRMRP_ReportFlag,
                                               IVRMRP_ProcessFlag = c.IVRMRP_ProcessFlag,
                                               IVRMMP_Id = a.IVRMMP_Id,
                                               IVRMRP_Id = c.IVRMRP_Id
                                           }
                   ).ToArray();


                List<long> notsaved = new List<long>();

                someObj.notsavetmpdata = (from a in _MasterRolePreviledgesContext.masterPageModuleMapping
                                          from b in _MasterRolePreviledgesContext.masterPage
                                          from c in _MasterRolePreviledgesContext.masterRolePreviledgeDMO
                                          where (a.IVRMP_Id == b.IVRMP_Id && a.IVRMMP_Id == c.IVRMMP_Id && deletegrid.Contains(a.IVRMM_Id) && c.IVRMRT_Id == someObj.IVRMRT_Id)
                                          select new MasterRolePreviledgeDTO
                                          {
                                              IVRMMP_Id = a.IVRMMP_Id,
                                              IVRMRP_Id = c.IVRMRP_Id
                                          }
                  ).ToArray();

                for (int i = 0; i < someObj.notsavetmpdata.Count(); i++)
                {
                    notsaved.Add(someObj.notsavetmpdata[i].IVRMMP_Id);
                }

                someObj.enq = (from a in _masterPageModuleMappingContext.masterPageModuleMapping
                               from b in _masterPageModuleMappingContext.masterPage
                               where (a.IVRMP_Id == b.IVRMP_Id && deletegrid.Contains(a.IVRMM_Id) && !notsaved.Contains(a.IVRMMP_Id)) /**/
                               select new MasterRolePreviledgeDTO
                               {
                                   ivrmmP_PageName = b.IVRMMP_PageName,
                                   IVRMMP_Id = a.IVRMMP_Id,
                               }
                       ).ToArray();



                //List<MasterPage> tempList = new List<MasterPage>();
                //int pageid = 0;
                //int j = 0;
                //while (j < allpages.Count())
                //{



                //    MasterPage singlepages = new MasterPage();

                //    pageid =Convert.ToInt32(allpages[j].IVRMP_Id);
                //    singlepages = (MasterPage) _masterPageContext.masterpage.Where(t => t.IVRMP_Id.Equals(pageid)).Distinct<MasterPage>();
                //    tempList.Add(singlepages);


                //     j++;
                //}
                //tempList.ToArray();


                //var statelist = _OrganisationContext.city
                //            .Join(_OrganisationContext.State,
                //                  c => c.IVRMMS_Id,
                //                  o => o.IVRMMS_Id,
                //                  (c, o) => new
                //                  {
                //                      IVRMMS_Id = c.IVRMMS_Id,
                //                      IVRMMS_Name = o.IVRMMS_Name,
                //                      IVRMMCT_Id = c.IVRMMCT_Id
                //                  }).Where(c => c.IVRMMCT_Id.Equals(id)).Distinct();

                //enq.stateDrpDown = statelist.ToArray();

                //enq.firstgrigdata = allpages.ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return someObj;
        }

        public MasterRolePreviledgeDTO getsearchdata(int id, MasterRolePreviledgeDTO org)
        {
            //string filetype = "All";
            //MasterRolePreviledgeDTO pagedata = new MasterRolePreviledgeDTO();
            try
            {
                List<MasterRolePreviledgeDMO> lorg = new List<MasterRolePreviledgeDMO>();
                if (org.searchname == "Role Name")
                {
                    // lorg = _MasterRolePreviledgesContext.masterRoleType.Where(t => t.IVRMRT_Role.Contains(org.IVRMRT_Role)).ToList();
                    org.thirdgriddata = (from a in _MasterRolePreviledgesContext.masterPageModuleMapping
                                         from b in _MasterRolePreviledgesContext.masterPage
                                         from c in _MasterRolePreviledgesContext.masterModule
                                         from d in _MasterRolePreviledgesContext.masterRolePreviledgeDMO
                                         from e in _MasterRolePreviledgesContext.masterRoleType
                                         from f in _MasterRolePreviledgesContext.MasterRole
                                         where (d.IVRMMP_Id == a.IVRMMP_Id && a.IVRMM_Id == c.IVRMM_Id && a.IVRMP_Id == b.IVRMP_Id && e.IVRMRT_Id == d.IVRMRT_Id && f.IVRMR_Id == e.IVRMR_Id && f.IVRMR_Role.Contains(org.prename))
                                         select new MasterRolePreviledgeDTO
                                         {
                                             ivrmmP_PageName = b.IVRMMP_PageName,
                                             ivrmM_ModuleName = c.IVRMM_ModuleName,
                                             ivrmrT_Role = e.IVRMRT_Role
                                         }
).ToArray();

                }
                if (org.searchname == "Module Page")
                {
                    //lorg = _MasterRolePreviledgesContext.masterRoleType.Where(t => t.IVRMRT_Role.Contains(org.IVRMRT_Role)).ToList();
                    org.thirdgriddata = (from a in _MasterRolePreviledgesContext.masterPageModuleMapping
                                         from b in _MasterRolePreviledgesContext.masterPage
                                         from c in _MasterRolePreviledgesContext.masterModule
                                         from d in _MasterRolePreviledgesContext.masterRolePreviledgeDMO
                                         from e in _MasterRolePreviledgesContext.masterRoleType
                                         from f in _MasterRolePreviledgesContext.MasterRole
                                         where (d.IVRMMP_Id == a.IVRMMP_Id && a.IVRMM_Id == c.IVRMM_Id && a.IVRMP_Id == b.IVRMP_Id && e.IVRMRT_Id == d.IVRMRT_Id && f.IVRMR_Id == e.IVRMR_Id && c.IVRMM_ModuleName.Contains(org.prename))
                                         select new MasterRolePreviledgeDTO
                                         {
                                             ivrmmP_PageName = b.IVRMMP_PageName,
                                             ivrmM_ModuleName = c.IVRMM_ModuleName,
                                             ivrmrT_Role = e.IVRMRT_Role
                                         }
).ToArray();

                }
                if (org.searchname == "Page Name")
                {
                    // lorg = _MasterRolePreviledgesContext.masterRoleType.Where(t => t.IVRMRT_Role.Contains(org.IVRMRT_Role)).ToList();
                    org.thirdgriddata = (from a in _MasterRolePreviledgesContext.masterPageModuleMapping
                                         from b in _MasterRolePreviledgesContext.masterPage
                                         from c in _MasterRolePreviledgesContext.masterModule
                                         from d in _MasterRolePreviledgesContext.masterRolePreviledgeDMO
                                         from e in _MasterRolePreviledgesContext.masterRoleType
                                         from f in _MasterRolePreviledgesContext.MasterRole
                                         where (d.IVRMMP_Id == a.IVRMMP_Id && a.IVRMM_Id == c.IVRMM_Id && a.IVRMP_Id == b.IVRMP_Id && e.IVRMRT_Id == d.IVRMRT_Id && f.IVRMR_Id == e.IVRMR_Id && b.IVRMMP_PageName.Contains(org.prename))
                                         select new MasterRolePreviledgeDTO
                                         {
                                             ivrmmP_PageName = b.IVRMMP_PageName,
                                             ivrmM_ModuleName = c.IVRMM_ModuleName,
                                             ivrmrT_Role = e.IVRMRT_Role
                                         }
).ToArray();

                }
                if (org.searchname == "All")
                {
                    //lorg = _MasterRolePreviledgesContext.masterRolePreviledgeDMO.ToList();
                    org.thirdgriddata = (from a in _MasterRolePreviledgesContext.masterPageModuleMapping
                                         from b in _MasterRolePreviledgesContext.masterPage
                                         from c in _MasterRolePreviledgesContext.masterModule
                                         from d in _MasterRolePreviledgesContext.masterRolePreviledgeDMO
                                         from e in _MasterRolePreviledgesContext.masterRoleType
                                         from f in _MasterRolePreviledgesContext.MasterRole
                                         where (d.IVRMMP_Id == a.IVRMMP_Id && a.IVRMM_Id == c.IVRMM_Id && a.IVRMP_Id == b.IVRMP_Id && e.IVRMRT_Id == d.IVRMRT_Id && f.IVRMR_Id == e.IVRMR_Id)
                                         select new MasterRolePreviledgeDTO
                                         {
                                             ivrmmP_PageName = b.IVRMMP_PageName,
                                             ivrmM_ModuleName = c.IVRMM_ModuleName,
                                             ivrmrT_Role = e.IVRMRT_Role
                                         }
).ToArray();
                }

                // org.thirdgriddata = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return org;
        }
    }
}
