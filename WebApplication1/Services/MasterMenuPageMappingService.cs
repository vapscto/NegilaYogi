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
    public class MasterMenuPageMappingService : Interfaces.MasterMenuPageMappingInterface
    {
        private static ConcurrentDictionary<string, IVRM_Master_Menu_Page_MappingDTO> _pgmod =
           new ConcurrentDictionary<string, IVRM_Master_Menu_Page_MappingDTO>();

        public DomainModelMsSqlServerContext _DomainModelMsSqlServerContext;
        public MasterMenuPageMappingService(DomainModelMsSqlServerContext MasterPageModuleMappingContext)
        {
            _DomainModelMsSqlServerContext = MasterPageModuleMappingContext;
        }

        public IVRM_Master_Menu_Page_MappingDTO deletemasterdataa(int ID)
        {
            IVRM_Master_Menu_Page_MappingDTO data = new IVRM_Master_Menu_Page_MappingDTO();
            bool returnresult = false;
          
            List<IVRM_Master_Menu_Page_MappingDMO> lorg = new List<IVRM_Master_Menu_Page_MappingDMO>();
            lorg = _DomainModelMsSqlServerContext.IVRM_Master_Menu_Page_MappingDMO.Where(t => t.IVRMMMPM_Id.Equals(ID)).ToList();

            try
            {
                if (lorg.Any())
                {
                    _DomainModelMsSqlServerContext.Remove(lorg.ElementAt(0));

                    var contactExists = _DomainModelMsSqlServerContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        returnresult = true;
                        data.returnval = returnresult;
                    }
                    else
                    {
                        returnresult = false;
                        data.returnval = returnresult;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public IVRM_Master_Menu_Page_MappingDTO loaddata(int ID)
        {
            IVRM_Master_Menu_Page_MappingDTO data = new IVRM_Master_Menu_Page_MappingDTO();
            try
            {
                List<MasterMenu> lorg = new List<MasterMenu>();
                lorg = _DomainModelMsSqlServerContext.Mastermenu.AsNoTracking().ToList();
                data.mastermenuarray = lorg.ToArray();

                List<MasterMenu> lorg1 = new List<MasterMenu>();
                lorg1 = _DomainModelMsSqlServerContext.Mastermenu.AsNoTracking().Where(t => t.IVRMMM_ParentId!=0).ToList();
                data.mastersubmenuarray = lorg1.ToArray();

                List<MasterModule> lorg2 = new List<MasterModule>();
                lorg2 = _DomainModelMsSqlServerContext.masterModule.AsNoTracking().Where(t => t.Module_ActiveFlag.Equals(1)).ToList();
                data.mastermodule = lorg2.ToArray();


                data.fillgrid = (from a in _DomainModelMsSqlServerContext.IVRM_Master_Menu_Page_MappingDMO
                                 from b in _DomainModelMsSqlServerContext.Mastermenu
                                 from c in _DomainModelMsSqlServerContext.masterModule
                                 from d in _DomainModelMsSqlServerContext.masterpage
                                 where (a.IVRMMM_Id==b.IVRMMM_Id && a.IVRMP_Id==d.IVRMP_Id && b.IVRMM_Id==c.IVRMM_Id)
                                select new IVRM_Master_Menu_Page_MappingDTO
                                {
                                    menuname=b.IVRMMM_MenuName,
                                    submenuname=b.IVRMMM_MenuName,
                                    modulename=c.IVRMM_ModuleName,
                                    pagename=d.IVRMMP_PageName,
                                    IVRMMMPM_Id=a.IVRMMMPM_Id
                                }
               ).ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public IVRM_Master_Menu_Page_MappingDTO mainmenuchangedata(int ID)
        {
            IVRM_Master_Menu_Page_MappingDTO data = new IVRM_Master_Menu_Page_MappingDTO();
            try
            {
                //List<MasterMenu> lorg1 = new List<MasterMenu>();
                //lorg1 = _DomainModelMsSqlServerContext.Mastermenu.Where(t => t.IVRMMM_ParentId == ID).ToList();
                //data.mastersubmenuarray = lorg1.ToArray();

                data.mastersubmenuarray = (from a in _DomainModelMsSqlServerContext.Mastermenu
                                  where (a.IVRMMM_ParentId == ID)
                                  select new IVRM_Master_Menu_Page_MappingDTO
                                  {
                                      IVRMMM_Idsubmenu = a.IVRMMM_Id,
                                      submenuname = a.IVRMMM_MenuName
                                  }
               ).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public IVRM_Master_Menu_Page_MappingDTO modchangedata(int ID)
        {
            IVRM_Master_Menu_Page_MappingDTO data = new IVRM_Master_Menu_Page_MappingDTO();
            try
            {
                List<MasterMenu> lorg1 = new List<MasterMenu>();
                lorg1 = _DomainModelMsSqlServerContext.Mastermenu.Where(t => t.IVRMM_Id == ID && t.IVRMMM_ParentId==0).ToList();
                data.mastermenuarray = lorg1.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public IVRM_Master_Menu_Page_MappingDTO savdata(IVRM_Master_Menu_Page_MappingDTO data)
        {
            bool returnresult = false;
            try
            {
                    if (data.savetmpdata != null)
                    {
                        int j = 0;
                        while (j < data.savetmpdata.Count())
                        {
                            IVRM_Master_Menu_Page_MappingDMO dta = new IVRM_Master_Menu_Page_MappingDMO();
                            if(data.IVRMMM_Id>0)
                            {
                                dta.IVRMMM_Id = data.IVRMMM_Id;
                                dta.IVRMP_Id = data.savetmpdata[j].IVRMP_Id;

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
                        returnresult = true;
                        data.returnval = returnresult;
                    }
                    else
                    {
                        returnresult = false;
                        data.returnval = returnresult;
                    }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public IVRM_Master_Menu_Page_MappingDTO submenuchangedata(IVRM_Master_Menu_Page_MappingDTO data)
        {
            try
            {
                //List<MasterPage> lorg1 = new List<MasterPage>();
                //lorg1 = _DomainModelMsSqlServerContext.masterpage.ToList();
                //data.fillpages = lorg1.ToArray();

                data.fillpages = (from a in _DomainModelMsSqlServerContext.masterpage
                                            from b in _DomainModelMsSqlServerContext.masterPageModuleMapping
                                           // from c in _DomainModelMsSqlServerContext.Mastermenu
                                  where (a.IVRMP_Id==b.IVRMP_Id && b.IVRMM_Id==data.IVRMM_Id) /*&& c.IVRMM_Id == b.IVRMM_Id && c.IVRMMM_ParentId != 0*/
                                  select new IVRM_Master_Menu_Page_MappingDTO
                                            {
                                                pagename = a.IVRMMP_PageName,
                                                IVRMP_Id=a.IVRMP_Id
                                            }
                ).ToArray();

                data.fillprioussavedgrid = (from a in _DomainModelMsSqlServerContext.IVRM_Master_Menu_Page_MappingDMO
                                 from b in _DomainModelMsSqlServerContext.Mastermenu
                                 from c in _DomainModelMsSqlServerContext.masterModule  
                                 from d in _DomainModelMsSqlServerContext.masterpage
                                 where (a.IVRMMM_Id == b.IVRMMM_Id && a.IVRMP_Id == d.IVRMP_Id && b.IVRMM_Id == c.IVRMM_Id && c.IVRMM_Id== data.IVRMM_Id && a.IVRMMM_Id==data.IVRMMM_Id)/* b.IVRMMM_ParentId != data.IVRMMM_Id*/
                                 select new IVRM_Master_Menu_Page_MappingDTO
                                 {
                                     menuname = b.IVRMMM_MenuName,
                                     submenuname = b.IVRMMM_MenuName,
                                     modulename = c.IVRMM_ModuleName,
                                     pagename = d.IVRMMP_PageName,
                                     IVRMP_Id=d.IVRMP_Id
                                 }
              ).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

    }
}
