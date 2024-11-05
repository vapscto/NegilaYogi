using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using PreadmissionDTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class MasterInstitutionRoleandModuleMappingImpl : Interfaces.MasterInstitutionRoleandModuleMappingInterface
    {

        private static ConcurrentDictionary<string, Institution_Module_PageDTO> _login =
          new ConcurrentDictionary<string, Institution_Module_PageDTO>();

        public DomainModelMsSqlServerContext _Context;

        public MasterInstitutionRoleandModuleMappingImpl(DomainModelMsSqlServerContext MasterContext)
        {
            _Context = MasterContext;
        }

        public InstitutionRolePrivilegesDTO GetDropdownList(InstitutionRolePrivilegesDTO IMPDTO)
        {
            try
            {
                var rolelist = _Context.MasterRoleType.Where(t => t.IVRMRT_Id == IMPDTO.roleId).ToList();

                if (rolelist[0].IVRMRT_Role.Equals("Super Admin"))
                {
                    List<Institution> lorgins = new List<Institution>();
                    lorgins = _Context.Institute.Where(t => t.MI_ActiveFlag == 1).ToList();
                    IMPDTO.InstitutionDropDown = lorgins.ToArray();

                    List<MasterRoleType> allRoleType = new List<MasterRoleType>();
                    allRoleType = _Context.MasterRoleType.ToList();
                    IMPDTO.RoleDropDown = allRoleType.ToArray();

                }
                else
                {
                    List<Institution> lorgins = new List<Institution>();
                    lorgins = _Context.Institute.Where(t => t.MI_Id == IMPDTO.MI_Id && t.MI_ActiveFlag==1).ToList();
                    IMPDTO.InstitutionDropDown = lorgins.ToArray();

                    List<MasterRoleType> allRoleType = new List<MasterRoleType>();
                    allRoleType = _Context.MasterRoleType.Where(t => t.IVRMRT_Role != "Super Admin").ToList();
                    IMPDTO.RoleDropDown = allRoleType.ToArray();

                }

                //List<Institution> allInstitution = new List<Institution>();
                //allInstitution = _Context.Institution.ToList();
                //IMPDTO.InstitutionDropDown = allInstitution.ToArray();


                List<MasterModule> allmasterModule = new List<MasterModule>();
                allmasterModule = _Context.masterModule.Where(t => t.Module_ActiveFlag == 1).ToList();
                IMPDTO.ModuleDropDown = allmasterModule.ToArray();

                //List<MasterPage> allpages = new List<MasterPage>();
                //allpages = _Context.masterpage.Where(t => t.IVRMP_TemplateFlag == true).ToList();
                //IMPDTO.PageDropDown = allpages.ToArray();


                IMPDTO.InstitutionRolePrivilegesList = (from irp in _Context.InstitutionRolePrivileges
                                                        from rt in _Context.MasterRoleType
                                                        from imp in _Context.Institution_Module_Page
                                                        from im in _Context.Institution_Module
                                                        from i in _Context.Institution
                                                        from m in _Context.masterModule
                                                        from p in _Context.masterPage
                                                        where (irp.IVRMIMP_Id == imp.IVRMIMP_Id && imp.IVRMIM_Id == im.IVRMIM_Id && im.IVRMM_Id == m.IVRMM_Id && imp.IVRMP_Id == p.IVRMP_Id &&
                                                        irp.IVRMRT_Id == rt.IVRMRT_Id && im.MI_Id ==  i.MI_Id)
                                                        select new InstitutionRolePrivilegesDTO
                                                        {
                                                            IVRMIRP_Id = irp.IVRMIRP_Id,
                                                            MI_Id = i.MI_Id,
                                                            MI_Name = i.MI_Name,
                                                            IVRMM_Id= m.IVRMM_Id,
                                                            IVRMM_ModuleName=  m.IVRMM_ModuleName,
                                                            IVRMP_Id = p.IVRMP_Id,
                                                            IVRMMP_PageName = p.IVRMMP_PageName,
                                                            IVRMRT_Id = irp.IVRMRT_Id,
                                                            IVRMRT_Role = rt.IVRMRT_Role,
                                                            IVRMIRP_AddFlag= irp.IVRMIRP_AddFlag,
                                                            IVRMIRP_UpdateFlag = irp.IVRMIRP_UpdateFlag,
                                                            IVRMIRP_DeleteFlag = irp.IVRMIRP_DeleteFlag,
                                                            IVRMIRP_ReportFlag = irp.IVRMIRP_ReportFlag,
                                                            IVRMIRP_SearchFlag  =irp.IVRMIRP_SearchFlag,
                                                            IVRMIRP_ProcessFlag  =irp.IVRMIRP_ProcessFlag

                                                        }).ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return IMPDTO;
        }

        public InstitutionRolePrivilegesDTO GetModuleDropdownList(int id)
        {
            InstitutionRolePrivilegesDTO IMPDTO = new InstitutionRolePrivilegesDTO();
            try
            {
                List<MasterModule> allmasterModule = new List<MasterModule>();
                allmasterModule = _Context.masterModule.Where(t => t.Module_ActiveFlag == 1).ToList();
                IMPDTO.ModuleDropDown = allmasterModule.ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return IMPDTO;
        }


        public MasterRolePreviledgeDTO getPagedetailsByModuleId(int moduleId)
        {
            MasterRolePreviledgeDTO rolDTO = new MasterRolePreviledgeDTO();
            try
            {

                List<MasterRoleType> allRoleType = new List<MasterRoleType>();
                allRoleType = _Context.MasterRoleType.ToList();
                rolDTO.RoleList = allRoleType.ToArray();



                rolDTO.thirdgriddata = (from a in _Context.masterPageModuleMapping
                                       from b in _Context.masterPage
                                       from c in _Context.masterModule
                                       where (a.IVRMM_Id == c.IVRMM_Id && a.IVRMP_Id == b.IVRMP_Id && c.IVRMM_Id == moduleId)
                                       select new MasterPageModuleMappingDTO
                                       {
                                           ivrmmP_PageName = b.IVRMMP_PageName,
                                           ivrmM_ModuleName = c.IVRMM_ModuleName,
                                           IVRMMP_Id = a.IVRMMP_Id,
                                           IVRMP_Id=b.IVRMP_Id
                                       }
             ).OrderBy(a => a.ivrmM_ModuleName).ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return rolDTO;
        }


        //by role type

        public MasterRolePreviledgeDTO getPagedetailsByRoleTypeId(MasterRolePreviledgeDTO rolDTO)
        {
           // MasterRolePreviledgeDTO rolDTO = new MasterRolePreviledgeDTO();
            try
            {

                rolDTO.privilegesList = (from rp in _Context.Role_Privileges
                                         from mmp in _Context.masterPageModuleMapping
                                         from p in _Context.masterPage
                                         where (rp.IVRMMP_Id == mmp.IVRMMP_Id && mmp.IVRMP_Id == p.IVRMP_Id && rp.IVRMRT_Id == rolDTO.IVRMRT_Id && mmp.IVRMM_Id== rolDTO.IVRMM_Id)
                                         select new MasterRolePreviledgeDTO
                                         {
                                             IVRMMP_Id = rp.IVRMMP_Id,
                                             ivrmP_Id = p.IVRMP_Id,
                                             ivrmmP_PageName = p.IVRMMP_PageName,
                                             IVRMRP_AddFlag = rp.IVRMRP_AddFlag,
                                             IVRMRP_DeleteFlag = rp.IVRMRP_DeleteFlag,
                                             IVRMRP_UpdateFlag = rp.IVRMRP_UpdateFlag,
                                             IVRMRP_ReportFlag = rp.IVRMRP_ReportFlag,
                                             IVRMRP_Id = rp.IVRMRP_Id,
                                             IVRMRT_Id = rp.IVRMRT_Id,

                                         }).ToArray();
                InstitutionRolePrivilegesDTO ff = new InstitutionRolePrivilegesDTO();
                rolDTO.InstitutionRolePrivilegesDTO = ff;

                rolDTO.InstitutionRolePrivilegesDTO.InstitutionRolePrivilegesList = (from irp in _Context.InstitutionRolePrivileges
                                             from rt in _Context.MasterRoleType
                                             from imp in _Context.Institution_Module_Page
                                             from im in _Context.Institution_Module
                                             from i in _Context.Institution
                                             from m in _Context.masterModule
                                             from p in _Context.masterPage
                                             from mmp in _Context.masterPageModuleMapping
                                             where (irp.IVRMIMP_Id == imp.IVRMIMP_Id && imp.IVRMIM_Id == im.IVRMIM_Id && 
                                             im.IVRMM_Id == m.IVRMM_Id && imp.IVRMP_Id == p.IVRMP_Id && mmp.IVRMM_Id== m.IVRMM_Id && mmp.IVRMP_Id==p.IVRMP_Id &&
                                             irp.IVRMRT_Id == rt.IVRMRT_Id && im.MI_Id == i.MI_Id && i.MI_Id== rolDTO.MI_Id && rt.IVRMRT_Id==rolDTO.IVRMRT_Id && 
                                             im.IVRMM_Id==rolDTO.IVRMM_Id)
                                             select new InstitutionRolePrivilegesDTO
                                             {
                                                 ivrmmP_Id = mmp.IVRMMP_Id,
                                                 IVRMIRP_Id = irp.IVRMIRP_Id,
                                                 MI_Id = i.MI_Id,
                                                 MI_Name = i.MI_Name,
                                                 IVRMM_Id = m.IVRMM_Id,
                                                 IVRMM_ModuleName = m.IVRMM_ModuleName,
                                                 IVRMP_Id = p.IVRMP_Id,
                                                 IVRMMP_PageName = p.IVRMMP_PageName,
                                                 IVRMRT_Id = irp.IVRMRT_Id,
                                                 IVRMRT_Role = rt.IVRMRT_Role,
                                                 ivrmrP_AddFlag = irp.IVRMIRP_AddFlag,
                                                 ivrmrP_UpdateFlag = irp.IVRMIRP_UpdateFlag,
                                                 ivrmrP_DeleteFlag = irp.IVRMIRP_DeleteFlag,
                                                 ivrmrP_ReportFlag = irp.IVRMIRP_ReportFlag,
                                                 ivrmrP_SearchFlag = irp.IVRMIRP_SearchFlag,
                                                 ivrmrP_ProcessFlag = irp.IVRMIRP_ProcessFlag

                                             }).ToArray();

               // rolDTO.previoussavetmpdata = previoussavetmpdata.ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return rolDTO;
        }



        public InstitutionRolePrivilegesDTO SaveInstitution_Module_Page(InstitutionRolePrivilegesDTO IMPDTO)
        {
            InstitutionRolePrivileges enq = Mapper.Map<InstitutionRolePrivileges>(IMPDTO);

            Institution_ModuleDTO imdto = new Institution_ModuleDTO();
            imdto.IVRMIM_Id = IMPDTO.IVRMIM_Id;
            imdto.MI_Id = IMPDTO.MI_Id;
            imdto.IVRMM_Id = IMPDTO.IVRMM_Id;

            Institution_Module im = Mapper.Map<Institution_Module>(imdto);
            int count = 0;
            int impCount = 0;
            int irolpCount = 0;

            try
            {
                List<Institution_Module> list = _Context.Institution_Module.Where(t => t.MI_Id == im.MI_Id && t.IVRMM_Id == im.IVRMM_Id).ToList();
                if (list.Count() > 0)
                {

                    var IMresult = _Context.Institution_Module.Single(t => t.IVRMIM_Id == list.FirstOrDefault().IVRMIM_Id);

                    IMresult.MI_Id = im.MI_Id;
                    IMresult.IVRMM_Id = im.IVRMM_Id;
                    //added by 02/02/2017
              
                    IMresult.UpdatedDate = DateTime.Now;
                    _Context.Update(IMresult);
                    count = _Context.SaveChanges();
                    im.IVRMIM_Id = IMresult.IVRMIM_Id;
                }
                else
                { //added by 02/02/2017
                    im.CreatedDate = DateTime.Now;
                    im.UpdatedDate = DateTime.Now;
                    im.IVRMIM_Flag = 1;
                    _Context.Add(im);
                    count = _Context.SaveChanges();
                }
                if(count > 0)
                {
                    Institution_Module_PageDTO impto = new Institution_Module_PageDTO();
                    impto.IVRMIMP_Id = enq.IVRMIMP_Id;
                    impto.IVRMIM_Id = im.IVRMIM_Id;


                    //if (IMPDTO.privilagedata.Count() > 0)
                    //{
                    //    foreach (InstitutionRolePrivilegesDTO inRPdto in IMPDTO.privilagedata)
                    //    {

                    //        var IVRMP_Id = _Context.masterPageModuleMapping.Single(t => t.IVRMMP_Id == inRPdto.ivrmmP_Id);
                    //        impto.IVRMP_Id = IVRMP_Id.IVRMP_Id;
                            
                    //        Institution_Module_Page imp = Mapper.Map<Institution_Module_Page>(impto);

                    //        List<Institution_Module_Page> list2 = _Context.Institution_Module_Page.Where(t => t.IVRMIM_Id == imp.IVRMIM_Id && t.IVRMP_Id == imp.IVRMP_Id).ToList();

                    //        if (list2.Count() > 0)
                    //        {

                    //            var IMPresult = _Context.Institution_Module_Page.Single(t => t.IVRMIMP_Id == list2.FirstOrDefault().IVRMIMP_Id);
                    //            //added by 02/02/2017
                         
                    //            IMPresult.UpdatedDate = DateTime.Now;
                    //            _Context.Update(IMPresult);
                    //            impCount = _Context.SaveChanges();
                    //            imp.IVRMIMP_Id = IMPresult.IVRMIMP_Id;
                    //        }
                    //        else
                    //        { //added by 02/02/2017
                    //            imp.CreatedDate = DateTime.Now;
                    //            imp.UpdatedDate = DateTime.Now;
                    //            imp.IVRMIMP_Flag = 1;
                    //            _Context.Add(imp);
                    //            impCount = _Context.SaveChanges();
                    //        }

                    //        if (impCount > 0)
                    //        {

                    //            inRPdto.IVRMIMP_Id = imp.IVRMIMP_Id;
                    //            inRPdto.IVRMRT_Id = IMPDTO.IVRMRT_Id;

                    //            inRPdto.IVRMIRP_AddFlag = inRPdto.ivrmrP_AddFlag;
                    //            inRPdto.IVRMIRP_DeleteFlag = inRPdto.ivrmrP_DeleteFlag;
                    //            inRPdto.IVRMIRP_UpdateFlag = inRPdto.ivrmrP_UpdateFlag;
                    //            inRPdto.IVRMIRP_ReportFlag = inRPdto.ivrmrP_ReportFlag;
                    //            inRPdto.IVRMIRP_ProcessFlag = inRPdto.ivrmrP_ProcessFlag;
                    //            inRPdto.IVRMIRP_SearchFlag = inRPdto.ivrmrP_SearchFlag;


                    //            InstitutionRolePrivileges inRP = Mapper.Map<InstitutionRolePrivileges>(inRPdto);

                    //            List<InstitutionRolePrivileges> list3 = _Context.InstitutionRolePrivileges.Where(t => t.IVRMRT_Id == inRP.IVRMRT_Id && t.IVRMIMP_Id == inRPdto.IVRMIMP_Id).ToList();

                    //            if (list3.Count() > 0)
                    //            {

                    //                var IRPresult = _Context.InstitutionRolePrivileges.Single(t => t.IVRMIRP_Id == list3.FirstOrDefault().IVRMIRP_Id);
                    //                IRPresult.IVRMIRP_AddFlag = inRP.IVRMIRP_AddFlag;
                    //                IRPresult.IVRMIRP_DeleteFlag = inRP.IVRMIRP_DeleteFlag;
                    //                IRPresult.IVRMIRP_ProcessFlag = inRP.IVRMIRP_ProcessFlag;
                    //                IRPresult.IVRMIRP_ReportFlag = inRP.IVRMIRP_ReportFlag;
                    //                IRPresult.IVRMIRP_SearchFlag = inRP.IVRMIRP_SearchFlag;
                    //                IRPresult.IVRMIRP_UpdateFlag = inRP.IVRMIRP_UpdateFlag;
                    //                //added by 02/02/2017
                             
                    //                IRPresult.UpdatedDate = DateTime.Now;
                    //                _Context.Update(IRPresult);
                    //              var  contactExists = _Context.SaveChanges();
                    //                //imp.IVRMIMP_Id = IRPresult.IVRMIMP_Id;
                    //                if (contactExists >= 1)
                    //                {

                    //                    IMPDTO.returnval = "Update";
                    //                }
                    //                else
                    //                {

                    //                    IMPDTO.returnval = "NotUpdate";
                    //                }
                    //            }
                    //            else
                    //            { //added by 02/02/2017
                    //                inRP.CreatedDate = DateTime.Now;
                    //                inRP.UpdatedDate = DateTime.Now;
                    //                _Context.Add(inRP);
                    //                var contactExists = _Context.SaveChanges();
                    //                if (contactExists >= 1)
                    //                {

                    //                    IMPDTO.returnval = "Save";
                    //                }
                    //                else
                    //                {

                    //                    IMPDTO.returnval = "NotSave";
                    //                }
                    //            }

                    //        }
                    //    }
                    //}

                    // save tempdata  


                    if (IMPDTO.savetmpdata.Count() > 0)
                    {
                        foreach (InstitutionRolePrivilegesDTO inRPdto in IMPDTO.savetmpdata)
                        {
                             impto.IVRMP_Id = inRPdto.IVRMP_Id;

                            //var masterpageid = _Context.masterPageModuleMapping.Single(t => t.IVRMMP_Id == inRPdto.ivrmmP_Id).IVRMP_Id;
                            //impto.IVRMP_Id = masterpageid;

                            Institution_Module_Page imp = Mapper.Map<Institution_Module_Page>(impto);

                            List<Institution_Module_Page> list2 = _Context.Institution_Module_Page.Where(t => t.IVRMIM_Id == imp.IVRMIM_Id && t.IVRMP_Id == imp.IVRMP_Id).ToList();

                            if (list2.Count() > 0)
                            {

                                var IMPresult = _Context.Institution_Module_Page.Single(t => t.IVRMIMP_Id == list2.FirstOrDefault().IVRMIMP_Id);

                                //added by 02/02/2017
                                IMPresult.UpdatedDate = DateTime.Now;
                                _Context.Update(IMPresult);
                                impCount = _Context.SaveChanges();
                                imp.IVRMIMP_Id = IMPresult.IVRMIMP_Id;
                            }
                            else
                            {//added by 02/02/2017
                                imp.CreatedDate = DateTime.Now;
                                imp.UpdatedDate = DateTime.Now;
                                imp.IVRMIMP_Flag = 1;
                                _Context.Add(imp);
                                impCount = _Context.SaveChanges();

                            }

                            if (impCount > 0)
                            {

                                inRPdto.IVRMIMP_Id = imp.IVRMIMP_Id;
                                inRPdto.IVRMRT_Id = IMPDTO.IVRMRT_Id;

                                inRPdto.IVRMIRP_AddFlag = inRPdto.ivrmrP_AddFlag;
                                inRPdto.IVRMIRP_DeleteFlag = inRPdto.ivrmrP_DeleteFlag;
                                inRPdto.IVRMIRP_UpdateFlag = inRPdto.ivrmrP_UpdateFlag;
                                inRPdto.IVRMIRP_ReportFlag = inRPdto.ivrmrP_ReportFlag;
                                inRPdto.IVRMIRP_ProcessFlag = inRPdto.ivrmrP_ProcessFlag;
                                 inRPdto.IVRMIRP_SearchFlag = inRPdto.ivrmrP_SearchFlag;


                                InstitutionRolePrivileges inRP = Mapper.Map<InstitutionRolePrivileges>(inRPdto);

                                List<InstitutionRolePrivileges> list3 = _Context.InstitutionRolePrivileges.Where(t => t.IVRMRT_Id == inRP.IVRMRT_Id && t.IVRMIMP_Id == inRPdto.IVRMIMP_Id).ToList();

                                if (list3.Count() > 0)
                                {

                                    var IRPresult = _Context.InstitutionRolePrivileges.Single(t => t.IVRMIRP_Id == list3.FirstOrDefault().IVRMIRP_Id);
                                    IRPresult.IVRMIRP_AddFlag = inRP.IVRMIRP_AddFlag;
                                    IRPresult.IVRMIRP_DeleteFlag = inRP.IVRMIRP_DeleteFlag;
                                    IRPresult.IVRMIRP_ProcessFlag = inRP.IVRMIRP_ProcessFlag;
                                    IRPresult.IVRMIRP_ReportFlag = inRP.IVRMIRP_ReportFlag;
                                    IRPresult.IVRMIRP_SearchFlag = inRP.IVRMIRP_SearchFlag;
                                    IRPresult.IVRMIRP_UpdateFlag = inRP.IVRMIRP_UpdateFlag;
                                    //added by 02/02/2017
                                    IRPresult.UpdatedDate = DateTime.Now;
                                    _Context.Update(IRPresult);
                                    var contactExists  = _Context.SaveChanges();
                                   // irolpCount = _Context.SaveChanges();
                                    //imp.IVRMIMP_Id = IRPresult.IVRMIMP_Id;

                                    if (contactExists >= 1)
                                    {

                                        IMPDTO.returnval = "Update";
                                    }
                                    else
                                    {
                                      
                                        IMPDTO.returnval = "NotUpdate";
                                    }
                                }
                                else
                                { //added by 02/02/2017
                                    inRP.CreatedDate = DateTime.Now;
                                    inRP.UpdatedDate = DateTime.Now;
                                    _Context.Add(inRP);
                                    var contactExists = _Context.SaveChanges();

                                    if (contactExists >= 1)
                                    {

                                        IMPDTO.returnval = "Save";
                                    }
                                    else
                                    {

                                        IMPDTO.returnval = "NotSave";
                                    }
                                }

                            }
                        }

                    }

                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return IMPDTO;
        }



        public InstitutionRolePrivilegesDTO deleterec(InstitutionRolePrivilegesDTO id)
        {
            InstitutionRolePrivilegesDTO IMPDTO = new InstitutionRolePrivilegesDTO();

            //List<Institution_Module_Page> lorgModpage = new List<Institution_Module_Page>();
            //List<Institution_Module> lorgMod = new List<Institution_Module>();
            //try
            //{

            //    lorg = _Context.InstitutionRolePrivileges.Where(t => t.IVRMIRP_Id.Equals(id)).ToList();

            //    long instModpage = lorg.FirstOrDefault().IVRMIMP_Id;

            //    if (lorg.Any())
            //    {
            //        _Context.Remove(lorg.ElementAt(0));
            //        var contactExists = _Context.SaveChanges();

            //    }
            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}

            List<long> deletegrid = new List<long>();

            for (int i = 0; i < id.privilagedata.Length; i++)
            {
                deletegrid.Add(id.privilagedata[i].IVRMIRP_Id);
            }
            List<InstitutionRolePrivileges> lorg = new List<InstitutionRolePrivileges>();// Mapper.Map<Organisation>(org);
            lorg = _Context.InstitutionRolePrivileges.Where(t => deletegrid.Contains(t.IVRMIRP_Id)).ToList();

         
                if (lorg.Any())
                {
                    for (int i = 0; i < lorg.Count(); i++)
                    {
                    //deletegrid.Add(Convert.ToInt64(id.TempararyArrayListdelete[i].ivrmstauP_Id));
                    _Context.Remove(lorg.ElementAt(i));
                    }
                    var contactExists = _Context.SaveChanges();
                    if (contactExists > 0)
                    {
                    IMPDTO.returnval = "Data Deleted Successfully";
                    }
                    else
                    {
                    IMPDTO.returnval = "Data Not Deleted Successfully";
                    }
                }

                return IMPDTO;
        }
    }
}
