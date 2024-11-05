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
    public class MasterSourceImpl : Interfaces.MasterSourceInterface
    {
        private static ConcurrentDictionary<string, MasterPageDTO> _login =
          new ConcurrentDictionary<string, MasterPageDTO>();

        public MasterSourceContext _MasterSourceContext;
        public AdmissionFormContext _admContext;
        public MasterSourceImpl(MasterSourceContext MasterSourceContext, AdmissionFormContext admContext)
        {
            _MasterSourceContext = MasterSourceContext;
            _admContext = admContext;
        }
        public MasterSourceDTO saveorgdet(MasterSourceDTO page)
        {
            bool returnresult = false;
            page.returnMsg = "";
            try
            {
                MasterSource maspge = Mapper.Map<MasterSource>(page);

                if (page.PAMS_Id > 0)
                {
                    var resultCount = _MasterSourceContext.MasterSource.Where(t => t.PAMS_SourceName == maspge.PAMS_SourceName && t.PAMS_Id!= page.PAMS_Id).Count();

                    if (resultCount == 0)
                    {

                        var result = _MasterSourceContext.MasterSource.Single(t => t.PAMS_Id == maspge.PAMS_Id);
                        // var result = _OrganisationContext.Organisation.AsNoTracking().Single(t => t.MO_Id == enq.MO_Id);

                        result.PAMS_SourceName = maspge.PAMS_SourceName;
                        result.PAMS_SourceDesc = maspge.PAMS_SourceDesc;


                        //added by 02/02/2017

                        result.UpdatedDate = DateTime.Now;
                        _MasterSourceContext.Update(result);
                        var contactExists = _MasterSourceContext.SaveChanges();

                        if (contactExists == 1)
                        {
                            returnresult = true;
                            page.returnval = returnresult;
                            page.returnMsg = "update";
                        }
                        else
                        {
                            returnresult = false;
                            page.returnval = returnresult;
                        }
                    }
                    else
                    {
                        page.returnMsg = "duplicate";
                        return page;
                    }
                }
                else
                {
                    var resultCount = _MasterSourceContext.MasterSource.Where(t => t.PAMS_SourceName == maspge.PAMS_SourceName).Count();

                    if(resultCount > 0)
                    {
                        page.returnMsg = "duplicate";
                        return page;
                    }
                    //added by 02/02/2017
                    maspge.CreatedDate = DateTime.Now;
                    maspge.UpdatedDate = DateTime.Now;
                    _MasterSourceContext.Add(maspge);
                    var contactExists = _MasterSourceContext.SaveChanges();

                    if (contactExists == 1)
                    {
                        returnresult = true;
                        page.returnval = returnresult;
                        page.returnMsg = "add";
                    }
                    else
                    {
                        returnresult = false;
                        page.returnval = returnresult;
                    }
                }

                List<MasterSource> allpages = new List<MasterSource>();
                allpages = _MasterSourceContext.MasterSource.ToList();
                page.pagesdata = allpages.OrderByDescending(c=>c.CreatedDate).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return page;
        }

        public MasterSourceDTO deleterec(int id)
        {
            bool returnresult = false;
            MasterSourceDTO page = new MasterSourceDTO();
            List<MasterSource> lorg = new List<MasterSource>();
            lorg = _MasterSourceContext.MasterSource.Where(t => t.PAMS_Id.Equals(id)).ToList();

            try
            {
                var check_id_used = _MasterSourceContext.StudentSourceDMO.Where(t => t.PAMS_Id == id).ToList();
               // var check_id_used1 = _MasterSourceContext.StudentApplication.Where(t => t.PAMS_Id == id).ToList();
                var check_id_user2 = _admContext.StudentSourceDMO.Where(d => d.PAMS_Id == id).ToList();

                if(check_id_used.Count==0 && check_id_user2.Count==0)
                {
                    if (lorg.Any())
                    {
                        _MasterSourceContext.Remove(lorg.ElementAt(0));

                        var contactExists = _MasterSourceContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            returnresult = true;
                            page.returnval = returnresult;
                        }
                        else
                        {
                            returnresult = false;
                            page.returnval = returnresult;
                        }
                    }
                }
                else
                {
                    page.returnMsg = "Delete";
                }
                List<MasterSource> allpages = new List<MasterSource>();
                allpages = _MasterSourceContext.MasterSource.ToList();
                page.pagesdata = allpages.OrderByDescending(c => c.CreatedDate).ToArray();
            }
            catch (Exception ee)
            {
                page.returnMsg = "Sorry You Can Not Delete This Record.Because It Is Mapped To Student.";
                Console.WriteLine(ee.Message);

            }
            return page;
        }

        public MasterSourceDTO getdetails(int id)
        {
            MasterSourceDTO org = new MasterSourceDTO();
            try
            {
                List<MasterSource> lorg = new List<MasterSource>();
                lorg = _MasterSourceContext.MasterSource.ToList();
                org.pagesdata = lorg.OrderByDescending(c => c.CreatedDate).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return org;
        }

        public MasterSourceDTO getpageedit(int id)
        {
            MasterSourceDTO page = new MasterSourceDTO();
            try
            {
                List<MasterSource> lorg = new List<MasterSource>();
                lorg = _MasterSourceContext.MasterSource.AsNoTracking().Where(t => t.PAMS_Id.Equals(id)).ToList();
                page.pagesdata = lorg.OrderByDescending(c => c.CreatedDate).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public MasterSourceDTO getsearchdata(int id, MasterSourceDTO org)
        {
            //string filetype = "All";
            MasterSourceDTO pagedata = new MasterSourceDTO();
            try
            {
                List<MasterSource> lorg = new List<MasterSource>();
                if (org.PAMS_SourceDesc == "Name")
                {
                    lorg = _MasterSourceContext.MasterSource.Where(t => t.PAMS_SourceName.Contains(org.PAMS_SourceName)).ToList();
                   
                }
                if (org.PAMS_SourceDesc == "All")
                {
                    lorg = _MasterSourceContext.MasterSource.ToList();
                }
                org.pagesdata = lorg.OrderByDescending(c => c.CreatedDate).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return org;
        }
    }
}
