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
    public class MasterPageModuleMappingImpl : Interfaces.MasterPageModuleMappingInterface
    {
        private static ConcurrentDictionary<string, MasterPageModuleMappingDTO> _pgmod =
           new ConcurrentDictionary<string, MasterPageModuleMappingDTO>();

        public MasterPageModuleMappingContext _MasterPageModuleMappingContext;
        public MasterPageModuleMappingImpl(MasterPageModuleMappingContext MasterPageModuleMappingContext)
        {
            _MasterPageModuleMappingContext = MasterPageModuleMappingContext;
        }

        public MasterPageModuleMappingDTO deleterec(int id)
        {
            MasterPageModuleMappingDTO page = new MasterPageModuleMappingDTO();
            try
            {

                List<MasterPageModuleMapping> lorg = new List<MasterPageModuleMapping>();
                lorg = _MasterPageModuleMappingContext.masterPageModuleMapping.Where(t => t.IVRMMP_Id.Equals(id)).ToList();

                var data = _MasterPageModuleMappingContext.MasterRolePreviledgeDMO.Where(t => t.IVRMMP_Id == id).Count();

                if(data<=0)
                {
                    if (lorg.Any())
                    {
                        _MasterPageModuleMappingContext.Remove(lorg.ElementAt(0));

                        var contactExists = _MasterPageModuleMappingContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            page.returnval = "Record Deleted Successfully";
                        }
                        else
                        {
                            page.returnval = "Record Not Deleted";
                        }
                    }
                }
                else
                {
                    page.returnval = "Kindly Contact Administrator";
                }
                
                List<MasterPageModuleMapping> allmodpages = new List<MasterPageModuleMapping>();
                allmodpages = _MasterPageModuleMappingContext.masterPageModuleMapping.OrderByDescending(a=>a.CreatedDate).ToList();
                page.thirdgriddata = allmodpages.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public MasterPageModuleMappingDTO getmoduledet(int id)
        {
            MasterPageModuleMappingDTO pgmod = new MasterPageModuleMappingDTO();
            try
            {
                List<MasterModule> allmodule = new List<MasterModule>();
                allmodule = _MasterPageModuleMappingContext.masterModule.Where(t => t.Module_ActiveFlag == 1).ToList();
                pgmod.fillmodule = allmodule.ToArray();

                List<MasterPage> allpages = new List<MasterPage>();
                allpages = _MasterPageModuleMappingContext.masterPage.Where(t => t.IVRMP_TemplateFlag == true).ToList();
                pgmod.fillpagesdata = allpages.ToArray();


                //List<MasterPageModuleMapping> alldata = new List<MasterPageModuleMapping>();
                //alldata = _MasterPageModuleMappingContext.masterPageModuleMapping.ToList();
                //pgmod.thirdgriddata = alldata.ToArray();

                pgmod.thirdgriddata = (from a in _MasterPageModuleMappingContext.masterPageModuleMapping
                                         from b in _MasterPageModuleMappingContext.masterPage
                                         from c in _MasterPageModuleMappingContext.masterModule
                                         where (a.IVRMM_Id==c.IVRMM_Id && a.IVRMP_Id==b.IVRMP_Id)
                                         select new MasterPageModuleMappingDTO
                                         {
                                             ivrmmP_PageName = b.IVRMMP_PageName,
                                             ivrmM_ModuleName = c.IVRMM_ModuleName,
                                             IVRMMP_Id = a.IVRMMP_Id
                                         }
               ).OrderByDescending(a=>a.CreatedDate).ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return pgmod;
        }
        public MasterPageModuleMappingDTO saveorgdet(MasterPageModuleMappingDTO pgmod)
        {
            try
            {
                MasterPageModuleMapping pgmodule = Mapper.Map<MasterPageModuleMapping>(pgmod);

                bool returnresult = false;
                if (pgmodule.IVRMMP_Id > 0)
                {
                    var result = _MasterPageModuleMappingContext.masterPageModuleMapping.Single(t => t.IVRMMP_Id == pgmod.IVRMMP_Id);
                    // var result = _OrganisationContext.Organisation.AsNoTracking().Single(t => t.MO_Id == enq.MO_Id);

                    result.IVRMM_Id = pgmod.IVRMM_Id;


                    //added by 02/02/2017
                  //  result.CreatedDate = DateTime.Now;
                    result.UpdatedDate = DateTime.Now;
                    _MasterPageModuleMappingContext.Update(result);
                    var contactExists = _MasterPageModuleMappingContext.SaveChanges();

                    if (contactExists == 1)
                    {
                        pgmod.returnval = "Record Updated Successfully";
                    }
                    else
                    {
                        pgmod.returnval = "Record Not Updated";
                    }
                }
                else
                {
                    if (pgmod.savetmpdata != null)
                    {
                        int j = 0;
                        while (j < pgmod.savetmpdata.Count())
                        {
                            MasterPageModuleMapping pmm = new MasterPageModuleMapping();
                            if(pgmod.savetmpdata[j].IVRMP_Id!=0)
                            {
                                pmm.IVRMP_Id = pgmod.savetmpdata[j].IVRMP_Id;
                                pmm.IVRMM_Id = pgmod.IVRMM_Id;

                                //added by 02/02/2017
                                pmm.CreatedDate = DateTime.Now;
                                pmm.UpdatedDate = DateTime.Now;
                                _MasterPageModuleMappingContext.Add(pmm);
                                var contactExists=_MasterPageModuleMappingContext.SaveChanges();

                                if (contactExists == 1)
                                {
                                    pgmod.returnval = "Record Saved Successfully";
                                }
                                else
                                {
                                    pgmod.returnval = "Record Not Saved";
                                }

                            }
                            j++;
                        }
                    }
                }

               // List<MasterPageModuleMapping> alldata = new List<MasterPageModuleMapping>();
                //alldata = _MasterPageModuleMappingContext.masterPageModuleMapping.ToList();

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

                //pgmod.fillpagesdata =  statelist.ToArray();
               // pgmod.fillpagesdata = alldata.ToArray();

                pgmod.fillpagesdata = (from a in _MasterPageModuleMappingContext.masterPageModuleMapping
                                       from b in _MasterPageModuleMappingContext.masterPage
                                       from c in _MasterPageModuleMappingContext.masterModule
                                       where (a.IVRMM_Id == c.IVRMM_Id && a.IVRMP_Id == b.IVRMP_Id)
                                       select new MasterPageModuleMappingDTO
                                       {
                                           ivrmmP_PageName = b.IVRMMP_PageName,
                                           ivrmM_ModuleName = c.IVRMM_ModuleName,
                                           IVRMMP_Id = a.IVRMMP_Id
                                       }
             ).OrderByDescending(a=>a.CreatedDate).ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return pgmod;
        }
        public MasterPageModuleMappingDTO getsaveddata(int id)
        {
            MasterPageModuleMappingDTO data = new MasterPageModuleMappingDTO();
            try
            {
                var masterpageslist = _MasterPageModuleMappingContext.masterPageModuleMapping.Where(t => t.IVRMM_Id.Equals(id)).Select(d => d.IVRMP_Id).ToArray().ToArray();

                List<MasterPage> allpages = new List<MasterPage>();
                allpages = _MasterPageModuleMappingContext.masterPage.Where( t=>t.IVRMP_TemplateFlag==true && !masterpageslist.Contains(t.IVRMP_Id)).ToList();
                data.fillpagesdata = allpages.ToArray();

                data.fillmodule = (from a in _MasterPageModuleMappingContext.masterPageModuleMapping
                                       from b in _MasterPageModuleMappingContext.masterPage
                                       where (a.IVRMP_Id == b.IVRMP_Id && a.IVRMM_Id==id)
                                       select new MasterPageModuleMappingDTO
                                       {
                                           ivrmmP_PageName = b.IVRMMP_PageName,
                                           IVRMMP_Id = a.IVRMMP_Id,
                                           IVRMP_Id=a.IVRMP_Id
                                       }
           ).ToArray();

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

    }
}
