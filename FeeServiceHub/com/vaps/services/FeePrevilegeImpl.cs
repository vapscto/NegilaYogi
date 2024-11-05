using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using FeeServiceHub.com.vaps.interfaces;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.Extensions.Logging;

namespace FeeServiceHub.com.vaps.services
{
    public class FeePrevilegeImpl : interfaces.FeePrevilegeInterface
    {

        private static ConcurrentDictionary<string, FeePrevilegeDTO> _login =
         new ConcurrentDictionary<string, FeePrevilegeDTO>();

        public FeeGroupContext _FeeGroupContext;
        readonly ILogger<FeePrevilegeImpl> _logger;
        public FeePrevilegeImpl(FeeGroupContext frgContext, ILogger<FeePrevilegeImpl> log)
        {
            _logger = log;
            _FeeGroupContext = frgContext;
        }

        public FeePrevilegeDTO getdetails(FeePrevilegeDTO page)
        {

            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == page.MI_Id && y.Is_Active==true).OrderByDescending(f=>f.ASMAY_Order).ToList();
                page.adcyear = year.GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();

                List<FeeGroupDMO> group = new List<FeeGroupDMO>();
                group = _FeeGroupContext.feeGroup.Where(y => y.MI_Id == page.MI_Id && y.FMG_ActiceFlag==true).ToList();
                page.group = group.ToArray();

                List<FeeHeadDMO> head = new List<FeeHeadDMO>();
                head = _FeeGroupContext.feehead.Where(y => y.MI_Id == page.MI_Id && y.FMH_ActiveFlag == true).ToList();
                page.head = head.ToArray();

                List<MasterRoleType> role = new List<MasterRoleType>();
                role = _FeeGroupContext.IVRM_Role_Type.ToList();
                page.role = role.ToArray();

                page.prelist = (from a in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                from b in _FeeGroupContext.applicationUser
                                from c in _FeeGroupContext.feeGroup
                                from d in _FeeGroupContext.feehead
                                from e in _FeeGroupContext.ApplicationUserRole
                                from f in _FeeGroupContext.IVRM_Role_Type
                                where (a.User_Id == b.Id && a.FMG_ID == c.FMG_Id && a.FMH_Id == d.FMH_Id && a.MI_ID == page.MI_Id && a.User_Id == e.UserId
                                && e.RoleTypeId == f.IVRMRT_Id)
                                select new FeePrevilegeDTO
                                {
                                    FGL_Id = a.FGL_Id,
                                    User_Id = a.User_Id,
                                    NormalizedUserName = b.NormalizedUserName,
                                    FMG_GroupName = c.FMG_GroupName,
                                    FMH_FeeName = d.FMH_FeeName,
                                    IVRMRT_Role = f.IVRMRT_Role
                                }
                                ).ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public FeePrevilegeDTO getusername(FeePrevilegeDTO name)
        {
            try
            {
                name.usnam = (from a in _FeeGroupContext.ApplicationUserRole
                              from b in _FeeGroupContext.IVRM_Role_Type
                              from c in _FeeGroupContext.applicationUser
                              from d in _FeeGroupContext.UserRoleWithInstituteDMO
                              where (c.Id==d.Id && a.RoleTypeId == b.IVRMRT_Id && b.IVRMRT_Id == name.IVRMRT_Id && a.UserId == c.Id && d.MI_Id==name.MI_Id)
                              select new FeePrevilegeDTO
                              {
                                  UserId = a.UserId,
                                  NormalizedUserName = c.NormalizedUserName
                              }
                              ).ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return name;
        }

        public FeePrevilegeDTO savedetail(FeePrevilegeDTO _category)
        {
            bool returnresult = false;
            try
            {
                if(_category.FGL_Id > 0)
                {
                    var Duplicate_Count = 0;
                    for (int i = 0; i < _category.username.Count(); i++)
                    {
                        var temp_usr = _category.username[i].UserId;
                        for (int j = 0; j < _category.grouphead.Count(); j++)
                        {
                          //  FEeGroupLoginPreviledgeDMO objpge = Mapper.Map<FEeGroupLoginPreviledgeDMO>(_category);
                            var temp_grphead = _category.grouphead[j].FMH_Id;
                            var dup = _FeeGroupContext.FEeGroupLoginPreviledgeDMO.Where(t => t.FMG_ID == _category.FMG_ID && t.FMH_Id == temp_grphead && t.User_Id == temp_usr && t.MI_ID == _category.MI_Id && t.FGL_Id!=_category.FGL_Id).Count();
                            if (dup > 0)
                            {
                                Duplicate_Count += 1;
                            }
                        }
                    }
                    if (Duplicate_Count == 0)
                    {
                        FeePrevilegeDTO list = delete(Convert.ToInt32(_category.FGL_Id));
                       
                            if (list.returnval==true)
                            {
                                for (int i = 0; i < _category.username.Count(); i++)
                                {
                                    var temp_usr = _category.username[i].UserId;
                                    for (int j = 0; j < _category.grouphead.Count(); j++)
                                    {
                                       FEeGroupLoginPreviledgeDMO objpge = new FEeGroupLoginPreviledgeDMO();
                                     
                                    objpge.FMH_Id = _category.grouphead[j].FMH_Id;
                                    objpge.MI_ID = _category.MI_Id;
                                    objpge.FMG_ID = _category.FMG_ID;
                                    objpge.User_Id = temp_usr;
                                    objpge.CreatedDate = DateTime.Now;
                                    objpge.UpdatedDate = DateTime.Now;
                                     _FeeGroupContext.Add(objpge);
                                     var contactExists = _FeeGroupContext.SaveChanges();
                                        if (contactExists == 1)
                                        {
                                            _category.returnval = true;
                                        }
                                        else
                                        {
                                            _category.returnval = false;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                _category.returnval = false;
                            }
                        
                    }
                    else
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                }
                else
                {
                    var Duplicate_Count = 0;
                    for (int i = 0; i < _category.username.Count(); i++)
                    {
                        var temp_usr = _category.username[i].UserId;
                        for (int j = 0; j < _category.grouphead.Count(); j++)
                        {
                          //  FEeGroupLoginPreviledgeDMO objpge = Mapper.Map<FEeGroupLoginPreviledgeDMO>(_category);
                            var temp_grphead = _category.grouphead[j].FMH_Id;
                            var dup = _FeeGroupContext.FEeGroupLoginPreviledgeDMO.Where(t => t.FMG_ID == _category.FMG_ID && t.FMH_Id == temp_grphead && t.User_Id == temp_usr && t.MI_ID == _category.MI_Id).Count();
                            if(dup > 0)
                            {
                                Duplicate_Count += 1;
                            }
                        }
                    }
                    if(Duplicate_Count == 0)
                    {
                        for (int i = 0; i < _category.username.Count(); i++)
                        {
                            var temp_usr = _category.username[i].UserId;
                            for (int j = 0; j < _category.grouphead.Count(); j++)
                            {
                                FEeGroupLoginPreviledgeDMO objpge = Mapper.Map<FEeGroupLoginPreviledgeDMO>(_category);
                                var temp_grphead = _category.grouphead[j].FMH_Id;
                                objpge.FMH_Id = temp_grphead;
                                objpge.User_Id = temp_usr;
                                objpge.CreatedDate = DateTime.Now;
                                objpge.UpdatedDate = DateTime.Now;
                                _FeeGroupContext.Add(objpge);
                                var contactExists = _FeeGroupContext.SaveChanges();
                                if (contactExists == 1)
                                {
                                    returnresult = true;
                                    _category.returnval = returnresult;
                                }
                                else
                                {
                                    returnresult = false;
                                    _category.returnval = returnresult;
                                }
                            }
                        }
                    }
                    else
                    {
                        _category.returnduplicatestatus= "Duplicate";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public FeePrevilegeDTO delete(int id)
        {
            bool returnresult = false;
            FeePrevilegeDTO del = new FeePrevilegeDTO();
            try
            {
               
                var  delete = _FeeGroupContext.FEeGroupLoginPreviledgeDMO.Where(t => t.FGL_Id == id).ToList();
                try
                {
                    if (delete.Any())
                    {
                        _FeeGroupContext.Remove(delete.ElementAt(0));

                        var contactExists = _FeeGroupContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            returnresult = true;
                            del.returnval = returnresult;
                        }
                        else
                        {
                            returnresult = false;
                            del.returnval = returnresult;
                        }
                    }

                }
                catch (Exception ee)
                {
                    _logger.LogError(ee.Message);
                    Console.WriteLine(ee.Message);
                }
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return del;

        }

        public FeePrevilegeDTO edit(int id)
        {
            FeePrevilegeDTO edit = new FeePrevilegeDTO();
            try
            {
                List<FEeGroupLoginPreviledgeDMO> edit1 = new List<FEeGroupLoginPreviledgeDMO>();
                edit1 = _FeeGroupContext.FEeGroupLoginPreviledgeDMO.Where(t => t.FGL_Id == id).ToList();
                edit.editlist = edit1.ToArray();

                edit.usnam = (from a in _FeeGroupContext.ApplicationUserRole
                              from b in _FeeGroupContext.IVRM_Role_Type
                              from c in _FeeGroupContext.applicationUser
                              where (a.RoleTypeId == b.IVRMRT_Id  && a.UserId == c.Id && a.UserId == edit1.FirstOrDefault().User_Id)
                              select new FeePrevilegeDTO
                              {
                                 IVRMRT_Id=b.IVRMRT_Id,
                                  UserId = a.UserId,
                                  NormalizedUserName = c.NormalizedUserName
                              }
                              ).ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return edit;
        }

        public FeePrevilegeDTO fillheadsinterface(FeePrevilegeDTO data)
        {
            try
            {
                data.fillheads = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                              from b in _FeeGroupContext.FeeGroupDMO
                              from c in _FeeGroupContext.FeeHeadDMO
                              where (a.FMG_Id==b.FMG_Id && a.FMH_Id==c.FMH_Id && a.ASMAY_Id==data.ASMAY_Id && a.MI_Id==data.MI_Id && a.FMG_Id==data.FMG_ID)
                              select new FeePrevilegeDTO
                              {
                                  FMH_Id = a.FMH_Id,
                                  FMH_FeeName = c.FMH_FeeName
                              }
                            ).ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
