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

namespace WebApplication1.Services
{
    public class MasterMenuPageMappingInstitutionwisService : Interfaces.MasterMenuPageMappingInstitutionwisInterface
    {
        private static ConcurrentDictionary<string, IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO> _pgmod =
         new ConcurrentDictionary<string, IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO>();

        public DomainModelMsSqlServerContext _DomainModelMsSqlServerContext;
        public MasterMenuPageMappingInstitutionwisService(DomainModelMsSqlServerContext MasterPageModuleMappingContext)
        {
            _DomainModelMsSqlServerContext = MasterPageModuleMappingContext;
        }

        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO deletemasterdataa(IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO ID)
        {
            IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data = new IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO();
            //bool returnresult = false;

            //List<IVRM_Master_Menu_Page_MappingDMO> lorg = new List<IVRM_Master_Menu_Page_MappingDMO>();
            //lorg = _DomainModelMsSqlServerContext.IVRM_Master_Menu_Page_MappingDMO.Where(t => t.IVRMMMPM_Id.Equals(ID)).ToList();

            //try
            //{
            //    if (lorg.Any())
            //    {
            //        _DomainModelMsSqlServerContext.Remove(lorg.ElementAt(0));

            //        var contactExists = _DomainModelMsSqlServerContext.SaveChanges();
            //        if (contactExists == 1)
            //        {
            //            returnresult = true;
            //            data.returnval = returnresult;
            //        }
            //        else
            //        {
            //            returnresult = false;
            //            data.returnval = returnresult;
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}


            List<long> deletegrid = new List<long>();

            for (int i = 0; i < ID.privilagedata.Length; i++)
            {
                deletegrid.Add(ID.privilagedata[i].IVRMMMPMI_Id);
            }
            List<MasterMenuPageMappingInstituteWise> lorg = new List<MasterMenuPageMappingInstituteWise>();// Mapper.Map<Organisation>(org);
            lorg = _DomainModelMsSqlServerContext.IVRM_Master_Menu_Page_Mapping_InstitutionwiseDMO.Where(t => deletegrid.Contains(t.IVRMMMPMI_Id)).ToList();
        

            if (lorg.Any())
            {
                for (int i = 0; i < lorg.Count(); i++)
                {
                    //deletegrid.Add(Convert.ToInt64(id.TempararyArrayListdelete[i].ivrmstauP_Id));
                    _DomainModelMsSqlServerContext.Remove(lorg.ElementAt(i));
                }
                var contactExists = _DomainModelMsSqlServerContext.SaveChanges();
                if (contactExists > 0)
                {
                    data.returnval = "true";
                }
                else
                {
                    data.returnval = "false";
                }
            }

            return data;
        }

        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO institutionchan(int ID)
        {
            IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data = new IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO();
            try
            {
                //List<MasterMenuInstitutionWise> lorg1 = new List<MasterMenuInstitutionWise>();
                //lorg1 = _DomainModelMsSqlServerContext.MasterMenuInstitutionWise.Where(t => t.MI_Id == ID).ToList();
                //data.fillmodule = lorg1.ToArray();

                data.fillmodule = (from a in _DomainModelMsSqlServerContext.MasterMenuInstitutionWise
                                   from b in _DomainModelMsSqlServerContext.masterModule
                                   where (a.IVRMM_Id == b.IVRMM_Id && a.MI_Id == ID)
                                   select new IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO
                                   {
                                       modulename = b.IVRMM_ModuleName,
                                       IVRMM_Id = a.IVRMM_Id
                                   }
              ).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO loaddata(IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data)
        {
            try
            {
                //List<MasterMenu> lorg = new List<MasterMenu>();
                //lorg = _DomainModelMsSqlServerContext.Mastermenu.AsNoTracking().ToList();
                //data.mastermenuarray = lorg.ToArray();

                List<MasterMenu> lorg1 = new List<MasterMenu>();
                lorg1 = _DomainModelMsSqlServerContext.Mastermenu.AsNoTracking().Where(t => t.IVRMMM_ParentId == 0).ToList();
                data.mastersubmenuarray = lorg1.ToArray();

                List<MasterModule> lorg2 = new List<MasterModule>();
                lorg2 = _DomainModelMsSqlServerContext.masterModule.AsNoTracking().Where(t => t.Module_ActiveFlag.Equals(1)).ToList();
                data.mastermodule = lorg2.ToArray();

                var rolelist = _DomainModelMsSqlServerContext.MasterRoleType.Where(t => t.IVRMRT_Id == data.roleId).ToList();

                if (rolelist[0].IVRMRT_Role == "Super Admin")
                {
                    List<Institution> lorgins = new List<Institution>();
                    lorgins = _DomainModelMsSqlServerContext.Institution.Where(t=>t.MI_ActiveFlag==1).ToList();
                    data.fillinstitution = lorgins.ToArray();

                    data.fillgrid = (from a in _DomainModelMsSqlServerContext.IVRM_Master_Menu_Page_Mapping_InstitutionwiseDMO
                                     from e in _DomainModelMsSqlServerContext.MasterMenuInstitutionWise
                                     from b in _DomainModelMsSqlServerContext.Mastermenu
                                     from c in _DomainModelMsSqlServerContext.masterModule
                                     from d in _DomainModelMsSqlServerContext.masterpage
                                     from f in _DomainModelMsSqlServerContext.Institute
                                     where (a.IVRMMMI_Id == e.IVRMMMI_Id && e.IVRMM_Id == c.IVRMM_Id && b.IVRMMM_Id == e.IVRMMM_Id && e.IVRMM_Id == c.IVRMM_Id && a.IVRMP_Id == d.IVRMP_Id && f.MI_Id==e.MI_Id)
                                     select new IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO
                                     {
                                         menuname = b.IVRMMM_MenuName,
                                         submenuname = b.IVRMMM_MenuName,
                                         modulename = c.IVRMM_ModuleName,
                                         pagename = d.IVRMMP_PageName,
                                         IVRMMMPMI_Id = a.IVRMMMPMI_Id,
                                         MI_Name = f.MI_Name
                                     }
               ).ToArray();
                }
                else if (rolelist[0].IVRMRT_Role == "Multi Admin")
                {

                    //var Mo_id_list = _DomainModelMsSqlServerContext.Institute.Where(d => d.MI_Id == data.MI_Id).Select(d => d.MO_Id).ToList();

                    //var Mi_id_list = _DomainModelMsSqlServerContext.Institute.Where(d => d.MO_Id.Equals(Mo_id_list[0])).Select(d => d.MI_Id).ToList();


                    //List<Institution> lorgins = new List<Institution>();
                    //lorgins = _DomainModelMsSqlServerContext.Institution.Where(t => Mi_id_list.Contains(t.MI_Id)).ToList();
                    //data.fillinstitution = lorgins.ToArray();

                    var Mo_id_list = _DomainModelMsSqlServerContext.Institute.Where(d => d.MI_Id == data.MI_Id).Select(d => d.MO_Id).ToList();

                    var Mi_id_list = _DomainModelMsSqlServerContext.Institute.Where(d => d.MO_Id.Equals(Mo_id_list[0])).Select(d => d.MI_Id).ToList();



                    List<Institution> lorgins = new List<Institution>();
                    lorgins = _DomainModelMsSqlServerContext.Institute.Where(t => Mo_id_list.Contains(t.MO_Id) && t.MI_ActiveFlag == 1).ToList();
                    data.fillinstitution = lorgins.ToArray();



                    data.fillgrid = (from a in _DomainModelMsSqlServerContext.IVRM_Master_Menu_Page_Mapping_InstitutionwiseDMO
                                     from e in _DomainModelMsSqlServerContext.MasterMenuInstitutionWise
                                     from b in _DomainModelMsSqlServerContext.Mastermenu
                                     from c in _DomainModelMsSqlServerContext.masterModule
                                     from d in _DomainModelMsSqlServerContext.masterpage
                                     from f in _DomainModelMsSqlServerContext.Institute
                                     where (a.IVRMMMI_Id == e.IVRMMMI_Id && e.IVRMM_Id == c.IVRMM_Id && b.IVRMMM_Id == e.IVRMMM_Id && e.IVRMM_Id == c.IVRMM_Id && a.IVRMP_Id == d.IVRMP_Id && e.MI_Id == data.MI_Id && f.MI_Id == e.MI_Id)
                                     select new IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO
                                     {
                                         menuname = b.IVRMMM_MenuName,
                                         submenuname = b.IVRMMM_MenuName,
                                         modulename = c.IVRMM_ModuleName,
                                         pagename = d.IVRMMP_PageName,
                                         IVRMMMPMI_Id = a.IVRMMMPMI_Id,
                                         MI_Name = f.MI_Name
                                     }
              ).ToArray();
                }
                else if (rolelist[0].IVRMRT_Role == "Admin" || rolelist[0].IVRMRT_Role.Equals("COORDINATOR"))
                {

                    var Mo_id_list = _DomainModelMsSqlServerContext.Institute.Where(d => d.MI_Id == data.MI_Id).Select(d => d.MO_Id).ToList();

                    var Mi_id_list = _DomainModelMsSqlServerContext.Institute.Where(d => d.MO_Id.Equals(Mo_id_list[0])).Select(d => d.MI_Id).ToList();



                    List<Institution> lorgins = new List<Institution>();
                    lorgins = _DomainModelMsSqlServerContext.Institute.Where(t => t.MI_Id == data.MI_Id && t.MI_ActiveFlag == 1).ToList();
                    data.fillinstitution = lorgins.ToArray();



                    data.fillgrid = (from a in _DomainModelMsSqlServerContext.IVRM_Master_Menu_Page_Mapping_InstitutionwiseDMO
                                     from e in _DomainModelMsSqlServerContext.MasterMenuInstitutionWise
                                     from b in _DomainModelMsSqlServerContext.Mastermenu
                                     from c in _DomainModelMsSqlServerContext.masterModule
                                     from d in _DomainModelMsSqlServerContext.masterpage
                                     from f in _DomainModelMsSqlServerContext.Institute
                                     where (a.IVRMMMI_Id == e.IVRMMMI_Id && e.IVRMM_Id == c.IVRMM_Id && b.IVRMMM_Id == e.IVRMMM_Id && e.IVRMM_Id == c.IVRMM_Id && a.IVRMP_Id == d.IVRMP_Id && e.MI_Id == data.MI_Id && f.MI_Id == e.MI_Id)
                                     select new IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO
                                     {
                                         menuname = b.IVRMMM_MenuName,
                                         submenuname = b.IVRMMM_MenuName,
                                         modulename = c.IVRMM_ModuleName,
                                         pagename = d.IVRMMP_PageName,
                                         IVRMMMPMI_Id = a.IVRMMMPMI_Id,
                                         MI_Name = f.MI_Name
                                     }
              ).ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO mainmenuchangedata(IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data)
        {
            try
            {
                //List<MasterMenuInstitutionWise> lorg1 = new List<MasterMenuInstitutionWise>();
                //lorg1 = _DomainModelMsSqlServerContext.MasterMenuInstitutionWise.Where(t => t.IVRMMMI_ParentId == data.IVRMMMI_Id && t.MI_Id==data.MI_Id
                //).ToList();
                //data.mastersubmenuarray = lorg1.ToArray();

                data.mastersubmenuarray = (from a in _DomainModelMsSqlServerContext.MasterMenuInstitutionWise
                                           where (a.IVRMMMI_ParentId == data.IVRMMMI_Id && a.MI_Id == data.MI_Id)
                                           select new IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO
                                           {
                                               IVRMMMI_Id = a.IVRMMMI_Id,
                                               submenuname = a.IVRMMMI_MenuName
                                           }
              ).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO modchangedata(IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data)
        {
            try
            {
                List<MasterMenuInstitutionWise> lorg1 = new List<MasterMenuInstitutionWise>();
                lorg1 = _DomainModelMsSqlServerContext.MasterMenuInstitutionWise.Where(t => t.IVRMM_Id == data.IVRMM_Id && t.MI_Id==data.MI_Id && t.IVRMMMI_ParentId == 0 ).ToList();
                data.mastermenuarray = lorg1.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO savdata(IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data)
        {
            try
            {
                if (data.savetmpdata != null)
                {
                    int j = 0;
                    while (j < data.savetmpdata.Count())
                    {
                        MasterMenuPageMappingInstituteWise dta = new MasterMenuPageMappingInstituteWise();
                        if (data.IVRMMMI_Id > 0)
                        {
                            dta.IVRMMMI_Id = data.savetmpdata[j].IVRMMMI_Id;
                            // dta.IVRMMMI_Id = data.IVRMMMI_Id;
                            dta.IVRMP_Id = data.savetmpdata[j].IVRMP_Id;
                            dta.IVRMMMPMI_PageDisplayName = data.savetmpdata[j].Displayname;
                            //added by 02/02/2017
                            dta.CreatedDate = DateTime.Now;
                            dta.UpdatedDate = DateTime.Now;
                            _DomainModelMsSqlServerContext.Add(dta);
                        }
                        j++;
                    }
                }

                var contactExists = _DomainModelMsSqlServerContext.SaveChanges();

                if (contactExists >= 1)
                {
                    data.returnval = "Records Saved Successfully";
                }
                else
                {
                    data.returnval = "Records Not Saved Successfully";
                }
            }
            catch (Exception e)
            {
                data.returnval = "Contact Administrator";
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO submenuchangedata(IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data)
        {
            try
            {

                data.fillpages = (from a in _DomainModelMsSqlServerContext.masterpage
                                  from b in _DomainModelMsSqlServerContext.IVRM_Master_Menu_Page_MappingDMO
                                  from c in _DomainModelMsSqlServerContext.MasterMenuInstitutionWise
                                  where (a.IVRMP_Id == b.IVRMP_Id && c.IVRMMM_Id == b.IVRMMM_Id && c.IVRMM_Id == data.IVRMM_Id && c.MI_Id == data.MI_Id && c.IVRMMMI_ParentId != 0 && c.IVRMMMI_Id == data.IVRMMMI_Id)
                                  select new IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO
                                  {
                                      pagename = a.IVRMMP_PageName,
                                      IVRMP_Id = a.IVRMP_Id,
                                      IVRMMMI_Id = c.IVRMMMI_Id,
                                      Displayname=a.IVRMP_PageDisplayName
                                  }
                ).Distinct().ToArray();

                data.fillprioussavedgrid = (from a in _DomainModelMsSqlServerContext.IVRM_Master_Menu_Page_Mapping_InstitutionwiseDMO
                                            from b in _DomainModelMsSqlServerContext.MasterMenuInstitutionWise
                                            from c in _DomainModelMsSqlServerContext.masterModule
                                            from d in _DomainModelMsSqlServerContext.masterpage
                                            where (a.IVRMMMI_Id == b.IVRMMMI_Id && a.IVRMP_Id == d.IVRMP_Id && b.IVRMM_Id == c.IVRMM_Id && c.IVRMM_Id == data.IVRMM_Id && a.IVRMMMI_Id == data.IVRMMMI_Id) /*b.IVRMMMI_ParentId != data.IVRMMMI_Id*/
                                            select new IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO
                                            {
                                                menuname = b.IVRMMMI_MenuName,
                                                submenuname = b.IVRMMMI_MenuName,
                                                modulename = c.IVRMM_ModuleName,
                                                pagename = d.IVRMMP_PageName,
                                                IVRMP_Id = d.IVRMP_Id,
                                                IVRMMMI_Id = b.IVRMMMI_Id,
                                                Displayname=a.IVRMMMPMI_PageDisplayName
                                            }
              ).ToArray();




                data.GridDetails = (from a in _DomainModelMsSqlServerContext.IVRM_Master_Menu_Page_Mapping_InstitutionwiseDMO
                                            from b in _DomainModelMsSqlServerContext.MasterMenuInstitutionWise
                                            from c in _DomainModelMsSqlServerContext.masterModule
                                            from d in _DomainModelMsSqlServerContext.masterpage
                                            from e in  _DomainModelMsSqlServerContext.Institution_Module_Page
                                            from f in _DomainModelMsSqlServerContext.Institution_Module
                                            from g in _DomainModelMsSqlServerContext.Institute
                                            where (a.IVRMMMI_Id == b.IVRMMMI_Id && a.IVRMP_Id == d.IVRMP_Id && b.IVRMM_Id == c.IVRMM_Id
                                          && c.IVRMM_Id==f.IVRMM_Id && f.IVRMIM_Id==e.IVRMIM_Id && e.IVRMP_Id==d.IVRMP_Id && g.MI_Id==f.MI_Id
                                            && c.IVRMM_Id == data.IVRMM_Id && a.IVRMMMI_Id == data.IVRMMMI_Id && f.MI_Id==data.MI_Id) /*b.IVRMMMI_ParentId != data.IVRMMMI_Id*/
                                            select new IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO
                                            {
                                                menuname = b.IVRMMMI_MenuName,
                                                submenuname = b.IVRMMMI_MenuName,
                                                modulename = c.IVRMM_ModuleName,
                                                pagename = d.IVRMMP_PageName,
                                                IVRMP_Id = d.IVRMP_Id,
                                                IVRMMMI_Id = b.IVRMMMI_Id,
                                                IVRMIMP_Id=e.IVRMIMP_Id,
                                                pageorder =e.IVRMIMP_PageOrder,
                                                IVRMMMPMI_Id = a.IVRMMMPMI_Id,
                                                MI_Name =g.MI_Name,
                                                Displayname=a.IVRMMMPMI_PageDisplayName
                                            }).OrderBy(t => t.pageorder).Distinct().ToArray();



                data.GridDetails=(from a in _DomainModelMsSqlServerContext.masterModule
                                  from b in _DomainModelMsSqlServerContext.masterpage
                                  from c in _DomainModelMsSqlServerContext.masterPageModuleMapping
                                  from d in _DomainModelMsSqlServerContext.Institute
                                  from e in _DomainModelMsSqlServerContext.Institution_Module
                                  from f in _DomainModelMsSqlServerContext.Institution_Module_Page
                                  from g in _DomainModelMsSqlServerContext.MasterMenuInstitutionWise
                                  from h in _DomainModelMsSqlServerContext.IVRM_Master_Menu_Page_Mapping_InstitutionwiseDMO
                                  where (a.IVRMM_Id==c.IVRMM_Id && c.IVRMP_Id==c.IVRMP_Id && d.MI_Id==e.MI_Id && e.IVRMIM_Id==f.IVRMIM_Id && b.IVRMP_Id==f.IVRMP_Id && a.IVRMM_Id==e.IVRMM_Id && a.IVRMM_Id==g.IVRMM_Id && e.MI_Id==g.MI_Id && g.IVRMMMI_Id==h.IVRMMMI_Id && b.IVRMP_Id==h.IVRMP_Id && a.IVRMM_Id== data.IVRMM_Id && d.MI_Id==data.MI_Id && h.IVRMMMI_Id==data.IVRMMMI_Id)
                                  select new IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO
                                  {
                                      menuname = g.IVRMMMI_MenuName,
                                      submenuname = g.IVRMMMI_MenuName,
                                      modulename = a.IVRMM_ModuleName,
                                      pagename = b.IVRMMP_PageName,
                                      IVRMP_Id = b.IVRMP_Id,
                                      IVRMMMI_Id = g.IVRMMMI_Id,
                                      IVRMIMP_Id = f.IVRMIMP_Id,
                                      pageorder = f.IVRMIMP_PageOrder,
                                      IVRMMMPMI_Id = h.IVRMMMPMI_Id,
                                      MI_Name = d.MI_Name,
                                      Displayname = h.IVRMMMPMI_PageDisplayName
                                  }).OrderBy(t => t.pageorder).Distinct().ToArray();






                data.GridDetails = (from a in _DomainModelMsSqlServerContext.IVRM_Master_Menu_Page_Mapping_InstitutionwiseDMO
                                    from b in _DomainModelMsSqlServerContext.MasterMenuInstitutionWise
                                    from c in _DomainModelMsSqlServerContext.masterModule
                                    from d in _DomainModelMsSqlServerContext.masterpage
                                    from e in _DomainModelMsSqlServerContext.Institution_Module_Page
                                    from f in _DomainModelMsSqlServerContext.Institution_Module
                                    from g in _DomainModelMsSqlServerContext.Institute
                                    where (a.IVRMMMI_Id == b.IVRMMMI_Id && a.IVRMP_Id == d.IVRMP_Id && b.IVRMM_Id == c.IVRMM_Id
                                  && c.IVRMM_Id == f.IVRMM_Id && f.IVRMIM_Id == e.IVRMIM_Id && e.IVRMP_Id == d.IVRMP_Id && g.MI_Id == f.MI_Id
                                    && c.IVRMM_Id == data.IVRMM_Id && a.IVRMMMI_Id == data.IVRMMMI_Id && f.MI_Id == data.MI_Id) /*b.IVRMMMI_ParentId != data.IVRMMMI_Id*/
                                    select new IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO
                                    {
                                        menuname = b.IVRMMMI_MenuName,
                                        submenuname = b.IVRMMMI_MenuName,
                                        modulename = c.IVRMM_ModuleName,
                                        pagename = d.IVRMMP_PageName,
                                        IVRMP_Id = d.IVRMP_Id,
                                        IVRMMMI_Id = b.IVRMMMI_Id,
                                        IVRMIMP_Id = e.IVRMIMP_Id,
                                        pageorder = e.IVRMIMP_PageOrder,
                                        IVRMMMPMI_Id = a.IVRMMMPMI_Id,
                                        MI_Name = g.MI_Name,
                                        Displayname = a.IVRMMMPMI_PageDisplayName
                                    }).OrderBy(t => t.pageorder).Distinct().ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO changeorderData(IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.menuDTO.Count() > 0)
                {
                    foreach (IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO mob in dto.menuDTO)
                    {
                        if (mob.IVRMIMP_Id > 0)
                        {
                            var result = _DomainModelMsSqlServerContext.Institution_Module_Page.Single(t => t.IVRMIMP_Id.Equals(mob.IVRMIMP_Id));
                            // Mapper.Map(mob, result);
                         
                            result.IVRMIMP_PageOrder = mob.pageorder;
                            _DomainModelMsSqlServerContext.Update(result);
                            _DomainModelMsSqlServerContext.SaveChanges();
                        }
                    }

                    foreach (IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO mob in dto.menuDTO)
                    {
                        if (mob.IVRMIMP_Id > 0)
                        {
                            var result = _DomainModelMsSqlServerContext.IVRM_Master_Menu_Page_Mapping_InstitutionwiseDMO.Single(t => t.IVRMMMPMI_Id.Equals(mob.IVRMMMPMI_Id));
                            // Mapper.Map(mob, result);

                            result.IVRMMMPMI_PageDisplayName = mob.Displayname;
                            _DomainModelMsSqlServerContext.Update(result);
                            _DomainModelMsSqlServerContext.SaveChanges();
                        }
                    }

                    dto.retrunMsg = "Order Updated Successfully";

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

    }
}
