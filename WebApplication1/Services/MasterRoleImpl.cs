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
    public class MasterRoleImpl :Interfaces.MasterRoleInterface
    {
        private static ConcurrentDictionary<string, MasterRoleDTO> _login =
          new ConcurrentDictionary<string, MasterRoleDTO>();

        public MasterRoleContext _MasterRoleContext;
        public MasterRoleImpl(MasterRoleContext MasterRoleContext)
        {
            _MasterRoleContext = MasterRoleContext;
        }

        public MasterRoleDTO saveorgdet(MasterRoleDTO page)
        {
            bool returnresult = false;
            try
            {
                string retval = "";
                MasterRole maspge = Mapper.Map<MasterRole>(page);
                ApplRole rol = new ApplRole();

                if (page.IVRMR_Id > 0)
                {
                   var result1 = _MasterRoleContext.ApplicationRole.Where(t => t.Name == page.Name && t.Id!= page.IVRMR_Id);
                    if (result1.Count() > 0)
                    {
                        retval = "Duplicate";
                        page.returnduplicatestatus = retval;
                    }
                    else
                    {

                        var result = _MasterRoleContext.ApplicationRole.Single(t => t.Id == page.IVRMR_Id);
                        // var result = _OrganisationContext.Organisation.AsNoTracking().Single(t => t.MO_Id == enq.MO_Id);

                        result.Name = page.Name;
                        result.NormalizedName = page.NormalizedName;
                        //added by 02/02/2017

                        result.UpdatedDate = DateTime.Now;
                        _MasterRoleContext.Update(result);
                        var contactExists = _MasterRoleContext.SaveChanges();

                        if (contactExists == 1)
                        {
                            returnresult = true;
                            page.returnval = "Update";
                        }
                        else
                        {
                            returnresult = false;
                            page.returnval = "NotUpdate";
                        }
                    }
                }
                else
                {
                    var result = _MasterRoleContext.ApplicationRole.Where(t => t.Name== page.Name);

                    if (result.Count() > 0)
                    {
                        retval = "Duplicate";
                        page.returnduplicatestatus = retval;
                    }
                    else
                    {

                        //added by 02/02/2017
                        //maspge.CreatedDate = DateTime.Now;
                        //maspge.UpdatedDate = DateTime.Now;
                        //maspge.IVRMR_ActiveFlag = 1;
                        //_MasterRoleContext.Add(maspge);
                        //var contactExists = _MasterRoleContext.SaveChanges();

                        rol.Name = page.Name;
                        rol.NormalizedName = page.NormalizedName;
                        rol.roleType = page.Name;
                        //added by 02/02/2017
                        rol.CreatedDate = DateTime.Now;
                        rol.UpdatedDate = DateTime.Now;
                        rol.ActiveFlag = 1;
                        //rol.
                        _MasterRoleContext.Add(rol);
                        var contactExists= _MasterRoleContext.SaveChanges();

                        if (contactExists == 1)
                        {
                           
                            page.returnval = "Save";
                        }
                        else
                        {
                          
                            page.returnval = "NotSave";
                        }
                    }
                }

                List<ApplRole> allpages = new List<ApplRole>();
                allpages = _MasterRoleContext.ApplicationRole.ToList();
                page.pagesdata = allpages.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return page;
        }

        public MasterRoleDTO deleterec(int id)
        {
           // bool returnresult = false;
            MasterRoleDTO page = new MasterRoleDTO();
           // List<ApplRole> lorg = new List<ApplRole>();
           // lorg = _MasterRoleContext.ApplicationRole.Where(t => t.Id.Equals(id)).ToList();

            try
            {
                var result = _MasterRoleContext.ApplicationRole.Single(t => t.Id == id);
                if (result.ActiveFlag == 1)
                {
                    result.ActiveFlag = 0;
                    result.UpdatedDate = DateTime.Now;
                    _MasterRoleContext.Update(result);
                    _MasterRoleContext.SaveChanges();
                    page.returnval = "true";
                }
                else
                {
                    result.ActiveFlag = 1;
                    result.UpdatedDate = DateTime.Now;
                    _MasterRoleContext.Update(result);
                    _MasterRoleContext.SaveChanges();
                    page.returnval = "false";
                }

                //if (lorg.Any())
                //{
                //    _MasterRoleContext.Remove(lorg.ElementAt(0));

                //    var contactExists = _MasterRoleContext.SaveChanges();
                //    if (contactExists == 1)
                //    {
                //        returnresult = true;
                //        page.returnval = returnresult;
                //    }
                //    else
                //    {
                //        returnresult = false;
                //        page.returnval = returnresult;
                //    }
                //}

                List<ApplRole> allpages = new List<ApplRole>();
                allpages = _MasterRoleContext.ApplicationRole.ToList();
                page.pagesdata = allpages.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public MasterRoleDTO getdetails(int id)
        {
            MasterRoleDTO org = new MasterRoleDTO();
            try
            {
                List<ApplRole> lorg = new List<ApplRole>();
                lorg = _MasterRoleContext.ApplicationRole.ToList();
                org.pagesdata = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return org;
        }

        public MasterRoleDTO getpageedit(int id)
        {
            MasterRoleDTO page = new MasterRoleDTO();
            try
            {
                List<ApplRole> lorg = new List<ApplRole>();
                lorg = _MasterRoleContext.ApplicationRole.AsNoTracking().Where(t => t.Id.Equals(id)).ToList();
                page.pagesdata = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public MasterRoleDTO getsearchdata(int id, MasterRoleDTO org)
        {
            //string filetype = "All";
            MasterRoleDTO pagedata = new MasterRoleDTO();
            try
            {
                List<ApplRole> lorg = new List<ApplRole>();
                if (org.IVRMR_Role_desc == "Name")
                {
                    lorg = _MasterRoleContext.ApplicationRole.Where(t => t.Name.Contains(org.IVRMR_Role)).ToList();
                }
                if (org.IVRMR_Role_desc == "All")
                {
                    lorg = _MasterRoleContext.ApplicationRole.ToList();
                }
                org.pagesdata = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return org;
        }
    }
}
