using System;
using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.College.Fees;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Fees;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeFeeService.com.vaps.Interfaces;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class MasterClgFeePrevilegeImpl: MasterClgFeePrevilegeInterface
    {
        private static ConcurrentDictionary<string, MasterClgFeePrevilegeDTO> _login =
        new ConcurrentDictionary<string, MasterClgFeePrevilegeDTO>();

        public CollFeeGroupContext _FeeGroupContext;
        readonly ILogger<MasterClgFeePrevilegeImpl> _logger;
        public MasterClgFeePrevilegeImpl(CollFeeGroupContext frgContext, ILogger<MasterClgFeePrevilegeImpl> log)
        {
            _logger = log;
            _FeeGroupContext = frgContext;
        }

        public MasterClgFeePrevilegeDTO getdetails(MasterClgFeePrevilegeDTO page)
        {

            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == page.MI_Id).ToList();
                page.adcyear = year.GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();

                List<FeeGroupClgDMO> group = new List<FeeGroupClgDMO>();
                group = _FeeGroupContext.FeeGroupClgDMO.Where(y => y.MI_Id == page.MI_Id && y.FMG_ActiceFlag == true).ToList();
                page.group = group.ToArray();

                List<FeeHeadClgDMO> head = new List<FeeHeadClgDMO>();
                head = _FeeGroupContext.FeeHeadClgDMO.Where(y => y.MI_Id == page.MI_Id && y.FMH_ActiveFlag == true).ToList();
                page.head = head.ToArray();

                List<MasterRoleType> role = new List<MasterRoleType>();
                role = _FeeGroupContext.IVRM_Role_Type.ToList();
                page.role = role.ToArray();

                page.prelist = (from a in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                from b in _FeeGroupContext.applicationUser
                                from c in _FeeGroupContext.FeeGroupClgDMO
                                from d in _FeeGroupContext.FeeHeadClgDMO
                                from e in _FeeGroupContext.ApplicationUserRole
                                from f in _FeeGroupContext.IVRM_Role_Type
                                where (a.User_Id == b.Id && a.FMG_ID == c.FMG_Id && a.FMH_Id == d.FMH_Id && a.MI_ID == page.MI_Id && a.User_Id == e.UserId
                                && e.RoleTypeId == f.IVRMRT_Id)
                                select new MasterClgFeePrevilegeDTO
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

        public MasterClgFeePrevilegeDTO getusername(MasterClgFeePrevilegeDTO name)
        {
            try
            {
                name.usnam = (from a in _FeeGroupContext.ApplicationUserRole
                              from b in _FeeGroupContext.IVRM_Role_Type
                              from c in _FeeGroupContext.applicationUser
                              from d in _FeeGroupContext.UserRoleWithInstituteDMO
                              where (c.Id == d.Id && a.RoleTypeId == b.IVRMRT_Id && b.IVRMRT_Id == name.IVRMRT_Id && a.UserId == c.Id && d.MI_Id == name.MI_Id)
                              select new MasterClgFeePrevilegeDTO
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

        public MasterClgFeePrevilegeDTO savedetail(MasterClgFeePrevilegeDTO _category)
        {
            bool returnresult = false;
            try
            {
                if (_category.FGL_Id > 0)
                {
                    var Duplicate_Count = 0;
                    for (int i = 0; i < _category.username.Count(); i++)
                    {
                        var temp_usr = _category.username[i].UserId;
                        for (int j = 0; j < _category.grouphead.Count(); j++)
                        {
                          
                            var temp_grphead = _category.grouphead[j].FMH_Id;
                            var dup = _FeeGroupContext.FEeGroupLoginPreviledgeDMO.Where(t => t.FMG_ID == _category.FMG_ID && t.FMH_Id == temp_grphead && t.User_Id == temp_usr && t.MI_ID == _category.MI_Id && t.FGL_Id != _category.FGL_Id).Count();
                            if (dup > 0)
                            {
                                Duplicate_Count += 1;
                            }
                        }
                    }
                    if (Duplicate_Count == 0)
                    {
                        MasterClgFeePrevilegeDTO list = delete(Convert.ToInt32(_category.FGL_Id));

                        if (list.returnval == true)
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
                          
                            var temp_grphead = _category.grouphead[j].FMH_Id;
                            var dup = _FeeGroupContext.FEeGroupLoginPreviledgeDMO.Where(t => t.FMG_ID == _category.FMG_ID && t.FMH_Id == temp_grphead && t.User_Id == temp_usr && t.MI_ID == _category.MI_Id).Count();
                            if (dup > 0)
                            {
                                Duplicate_Count += 1;
                            }
                        }
                    }
                    if (Duplicate_Count == 0)
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
                        _category.returnduplicatestatus = "Duplicate";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public MasterClgFeePrevilegeDTO delete(int id)
        {
            bool returnresult = false;
            MasterClgFeePrevilegeDTO del = new MasterClgFeePrevilegeDTO();
            try
            {

                var delete = _FeeGroupContext.FEeGroupLoginPreviledgeDMO.Where(t => t.FGL_Id == id).ToList();
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

        public MasterClgFeePrevilegeDTO edit(int id)
        {
            MasterClgFeePrevilegeDTO edit = new MasterClgFeePrevilegeDTO();
            try
            {
                List<FEeGroupLoginPreviledgeDMO> edit1 = new List<FEeGroupLoginPreviledgeDMO>();
                edit1 = _FeeGroupContext.FEeGroupLoginPreviledgeDMO.Where(t => t.FGL_Id == id).ToList();
                edit.editlist = edit1.ToArray();

                edit.usnam = (from a in _FeeGroupContext.ApplicationUserRole
                              from b in _FeeGroupContext.IVRM_Role_Type
                              from c in _FeeGroupContext.applicationUser
                              where (a.RoleTypeId == b.IVRMRT_Id && a.UserId == c.Id && a.UserId == edit1.FirstOrDefault().User_Id)
                              select new MasterClgFeePrevilegeDTO
                              {
                                  IVRMRT_Id = b.IVRMRT_Id,
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

    }
}
