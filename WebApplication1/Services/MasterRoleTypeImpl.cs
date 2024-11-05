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
    public class MasterRoleTypeImpl : Interfaces.MasterRoleTypeInterface
    {
        private static ConcurrentDictionary<string, MasterRoleTypeDTO> _login =
         new ConcurrentDictionary<string, MasterRoleTypeDTO>();

        public MasterRoleTypeContext _MasterRoleTypeContext;
        public MasterRoleTypeImpl(MasterRoleTypeContext MasterRoleTypeContext)
        {
            _MasterRoleTypeContext = MasterRoleTypeContext;
        }

        public MasterRoleTypeDTO saveorgdet(MasterRoleTypeDTO page)
        {
            var returnresult = "false";
            try
            {
                MasterRoleType maspge = Mapper.Map<MasterRoleType>(page);

                if (page.IVRMRT_Id > 0)
                 {
                    List<MasterRoleType> Allname1 = new List<MasterRoleType>();
                    if (page.IVRMRT_Role == "Super Admin")
                    {
                        Array[] showdata1 = new Array[1];
                      
                        Allname1 = _MasterRoleTypeContext.masterRoleType.Where(t => t.IVRMRT_Role.ToLower() == maspge.IVRMRT_Role.ToLower() && t.IVRMR_Id == maspge.IVRMR_Id).ToList();
                        page.studentDetails = Allname1.ToArray();
                    }
                    else
                    {
                        Array[] showdata1 = new Array[1];
                      
                        Allname1 = _MasterRoleTypeContext.masterRoleType.Where(t => t.IVRMRT_Role.ToLower() == maspge.IVRMRT_Role.ToLower() && t.IVRMR_Id == maspge.IVRMR_Id && t.IVRMRT_Role!= "Super Admin").ToList();
                        page.studentDetails = Allname1.ToArray();
                    }
                    if (Allname1.Count > 0)
                    {
                        returnresult = "Duplicate";
                        page.returnval = returnresult;
                    }
                    else
                    {


                        var result = _MasterRoleTypeContext.masterRoleType.Single(t => t.IVRMRT_Id == maspge.IVRMRT_Id);
                        // var result = _OrganisationContext.Organisation.AsNoTracking().Single(t => t.MO_Id == enq.MO_Id);

                        result.IVRMRT_Role = maspge.IVRMRT_Role;
                        result.IVRMRT_RoleFlag= maspge.IVRMRT_Role;
                        result.IVRMR_Id = maspge.IVRMR_Id;
                        result.flag = maspge.flag;
                        //added by 02/02/2017

                        result.UpdatedDate = DateTime.Now;
                        _MasterRoleTypeContext.Update(result);
                        var contactExists = _MasterRoleTypeContext.SaveChanges();

                        if (contactExists == 1)
                        {
                            returnresult = "Update";
                            page.returnval = returnresult;
                        }
                        else
                        {
                            returnresult = "NotUpdate";
                            page.returnval = returnresult;
                        }

                        }
                    }
                else
                {

                //    var result = _MasterRoleTypeContext.masterRoleType.Where(t => t.IVRMRT_Role == maspge.IVRMRT_Role && t.IVRMR_Id=maspge.IVRMR_Id);

                    Array[] showdata1 = new Array[1];
                    List<MasterRoleType> Allname1 = new List<MasterRoleType>();
                    if (page.IVRMRT_Role == "Super Admin")
                    {
                        
                        Allname1 = _MasterRoleTypeContext.masterRoleType.Where(t => t.IVRMRT_Role.ToLower().Equals(maspge.IVRMRT_Role.ToLower()) && t.IVRMR_Id.Equals(maspge.IVRMR_Id)).ToList();
                        page.studentDetails = Allname1.ToArray();
                    }
                    else
                    {
                        Allname1 = _MasterRoleTypeContext.masterRoleType.Where(t => t.IVRMRT_Role.ToLower().Equals(maspge.IVRMRT_Role.ToLower()) && t.IVRMR_Id.Equals(maspge.IVRMR_Id) && t.IVRMRT_Role != "Super Admin").ToList();
                        page.studentDetails = Allname1.ToArray();
                    }
                    if (Allname1.Count > 0)
                    {
                        returnresult = "Duplicate";
                        page.returnval = returnresult;
                    }
                    else
                    {
                        //added by 02/02/2017
                        maspge.CreatedDate = DateTime.Now;
                        maspge.UpdatedDate = DateTime.Now;
                        _MasterRoleTypeContext.Add(maspge);
                        var contactExists = _MasterRoleTypeContext.SaveChanges();

                        if (contactExists == 1)
                        {
                            returnresult = "Save";
                            page.returnval = returnresult;
                        }
                        else
                        {
                            returnresult = "NotSave";
                            page.returnval = returnresult;
                        }
                    }
                }

                //List<MasterRoleType> allpages = new List<MasterRoleType>();
                //allpages = _MasterRoleTypeContext.masterRoleType.ToList();
                //page.pagesdata = allpages.ToArray();

                page.pagesdata = (from a in _MasterRoleTypeContext.ApplRole
                                 from b in _MasterRoleTypeContext.masterRoleType
                                 where (a.Id == b.IVRMR_Id)
                                 select new MasterRoleTypeDTO
                                 {
                                     IVRMRT_Role = b.IVRMRT_Role,
                                     Name = a.Name,
                                     Id = a.Id,
                                     IVRMRT_Id = b.IVRMRT_Id
                                 }
           ).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return page;
        }

        public MasterRoleTypeDTO deleterec(int id)
        {
            var returnresult = "false";
            MasterRoleTypeDTO page = new MasterRoleTypeDTO();
            List<MasterRoleType> lorg = new List<MasterRoleType>();
            lorg = _MasterRoleTypeContext.masterRoleType.Where(t => t.IVRMRT_Id.Equals(id)).ToList();

            try
            {
                if (lorg.Any())
                {
                    _MasterRoleTypeContext.Remove(lorg.ElementAt(0));

                    var contactExists = _MasterRoleTypeContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        returnresult = "true";
                        page.returnval = returnresult;
                    }
                    else
                    {
                        returnresult = "false";
                        page.returnval = returnresult;
                    }
                }

                //List<MasterRoleType> allpages = new List<MasterRoleType>();
                //allpages = _MasterRoleTypeContext.masterRoleType.ToList();
                //page.pagesdata = allpages.ToArray();

                page.pagesdata = (from a in _MasterRoleTypeContext.ApplRole
                                 from b in _MasterRoleTypeContext.masterRoleType
                                 where (a.Id == b.IVRMR_Id)
                                 select new MasterRoleTypeDTO
                                 {
                                     IVRMRT_Role = b.IVRMRT_Role,
                                     Name = a.Name,
                                     Id = a.Id,
                                     IVRMRT_Id = b.IVRMRT_Id
                                 }
           ).ToArray();
            }
            catch (Exception ee)
            {
                page.returnval = "false";
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public MasterRoleTypeDTO getdetails(int id)
        {
            MasterRoleTypeDTO org = new MasterRoleTypeDTO();
            try
            {
                
                List<ApplRole> roldta = new List<ApplRole>();
                roldta = _MasterRoleTypeContext.ApplRole.Where(t => t.ActiveFlag == 1).ToList();
                org.roledata = roldta.ToArray();

                org.pagesdata = (from a in _MasterRoleTypeContext.ApplRole
                                 from b in _MasterRoleTypeContext.masterRoleType
                                 where (a.Id==b.IVRMR_Id)
                                      select new MasterRoleTypeDTO
                                      {
                                          IVRMRT_Role = b.IVRMRT_Role,
                                          Name = a.Name,
                                          Id=a.Id,
                                          IVRMRT_Id = b.IVRMRT_Id
                                      }
             ).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return org;
        }

        public MasterRoleTypeDTO getpageedit(int id)
        {
            MasterRoleTypeDTO page = new MasterRoleTypeDTO();
            try
            {
                //List<MasterRoleType> lorg = new List<MasterRoleType>();
                //lorg = _MasterRoleTypeContext.masterRoleType.AsNoTracking().Where(t => t.IVRMRT_Id.Equals(id)).ToList();
                //page.pagesdata = lorg.ToArray();

                page.pagesdata = (from a in _MasterRoleTypeContext.ApplRole
                                 from b in _MasterRoleTypeContext.masterRoleType
                                 where (a.Id == b.IVRMR_Id && b.IVRMRT_Id==id)
                                 select new MasterRoleTypeDTO
                                 {
                                     IVRMRT_Role = b.IVRMRT_Role,
                                     Name = a.Name,
                                     Id=a.Id,
                                     IVRMRT_Id=b.IVRMRT_Id,
                                     flag = b.flag
                                 }).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public MasterRoleTypeDTO getsearchdata(int id, MasterRoleTypeDTO org)
        {
            //string filetype = "All";
            MasterRoleTypeDTO pagedata = new MasterRoleTypeDTO();
            try
            {
                List<MasterRoleType> lorg = new List<MasterRoleType>();
                if (org.IVRMRT_RoleFlag == "Page Name")
                {
                    lorg = _MasterRoleTypeContext.masterRoleType.Where(t => t.IVRMRT_Role.Contains(org.IVRMRT_Role)).ToList();

                }
                if (org.IVRMRT_RoleFlag == "All")
                {
                    lorg = _MasterRoleTypeContext.masterRoleType.ToList();
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
