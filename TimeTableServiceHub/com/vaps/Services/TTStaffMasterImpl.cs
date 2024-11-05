using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class TTStaffMasterImpl : Interfaces.StaffMasterInterface
    {
        private static ConcurrentDictionary<string, TTStaffMasterDTO> _login =
       new ConcurrentDictionary<string, TTStaffMasterDTO>();


        public TTContext _ttcategorycontext;
        public TTStaffMasterImpl(TTContext ttcategory)
        {
            _ttcategorycontext = ttcategory;
        }

        public TTStaffMasterDTO savedetail(TTStaffMasterDTO _category)
        {
            TT_Master_Staff_AbbreviationDMO objpge = Mapper.Map<TT_Master_Staff_AbbreviationDMO>(_category);
            try
            {
                if (objpge.TTMSAB_Id > 0)
                {
                    var resultCount = _ttcategorycontext.TT_Master_Staff_AbbreviationDMO.Where(t => t.TTMSAB_Abbreviation.Trim().ToLower() == objpge.TTMSAB_Abbreviation.Trim().ToLower() && t.MI_Id == objpge.MI_Id && t.TTMSAB_Id != objpge.TTMSAB_Id).Count();
                    var result123 = _ttcategorycontext.TT_Master_Staff_AbbreviationDMO.Where(t => t.HRME_Id == objpge.HRME_Id && t.TTMSAB_Id != objpge.TTMSAB_Id).Count();
                    if (resultCount == 0 && result123 == 0)
                    {
                        var result = _ttcategorycontext.TT_Master_Staff_AbbreviationDMO.Single(t => t.TTMSAB_Id == objpge.TTMSAB_Id && t.MI_Id == objpge.MI_Id);
                        result.TTMSAB_Abbreviation = objpge.TTMSAB_Abbreviation;
                        result.HRME_Id = objpge.HRME_Id;
                        result.TTMSAB_PerDayMaxDeputation = objpge.TTMSAB_PerDayMaxDeputation;
                        result.TTMSAB_PerMonthMaxDeputation = objpge.TTMSAB_PerMonthMaxDeputation;
                        result.TTMSAB_PerWeekMaxDeputation = objpge.TTMSAB_PerWeekMaxDeputation;
                        result.TTMSAB_PerYearMaxDeputation = objpge.TTMSAB_PerYearMaxDeputation;
                        result.TTMSAB_ActiveFlag = true;
                        result.UpdatedDate = DateTime.Now;
                        _ttcategorycontext.Update(result);
                        var contactExists = _ttcategorycontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                    else
                    {
                        _category.returnduplicatestatus = "Duplicate";
                        return _category;
                    }
                }
                else
                {
                    var result = _ttcategorycontext.TT_Master_Staff_AbbreviationDMO.Where(t => t.TTMSAB_Abbreviation.Trim().ToLower() == objpge.TTMSAB_Abbreviation.Trim().ToLower() && t.MI_Id == objpge.MI_Id).Count();
                    var result123 = _ttcategorycontext.TT_Master_Staff_AbbreviationDMO.Where(t => t.HRME_Id == objpge.HRME_Id).Count();
                    if (result > 0 || result123 > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else if (result == 0 && result123 == 0)
                    {
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        objpge.TTMSAB_ActiveFlag = true;
                        _ttcategorycontext.Add(objpge);
                        var contactExists = _ttcategorycontext.SaveChanges();
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
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public TTStaffMasterDTO getdetails(int id)
        {
            TTStaffMasterDTO TTMC = new TTStaffMasterDTO();
            try
            {


                TTMC.stafflist = (from a in _ttcategorycontext.HR_Master_Employee_DMO
                                  where (a.MI_Id.Equals(id) && a.HRME_ActiveFlag.Equals(true))

                                  select new TTStaffMasterDTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      FirstName = a.HRME_EmployeeFirstName,
                                      MiddleName = a.HRME_EmployeeMiddleName,
                                      LastName = a.HRME_EmployeeLastName,
                                       staffName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == " " || a.HRME_EmployeeMiddleName == "0" ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == " " || a.HRME_EmployeeLastName == "0" ? " " : a.HRME_EmployeeLastName),
                                  }
                                  ).Distinct().OrderBy(d => d.staffName).ToArray();



                List<TT_Master_Staff_AbbreviationDMO> fmaster = new List<TT_Master_Staff_AbbreviationDMO>();


                TTMC.ttstafflist = (from a in _ttcategorycontext.HR_Master_Employee_DMO
                                    from b in _ttcategorycontext.TT_Master_Staff_AbbreviationDMO
                                    where (a.HRME_Id == b.HRME_Id && a.MI_Id == b.MI_Id && a.MI_Id == id)
                                    select new TTStaffMasterDTO
                                    {
                                        FirstName = a.HRME_EmployeeFirstName,
                                        MiddleName = a.HRME_EmployeeMiddleName,
                                        LastName = a.HRME_EmployeeLastName,
                                        TTMSAB_Id = b.TTMSAB_Id,
                                        TTMSAB_Abbreviation = b.TTMSAB_Abbreviation,
                                        TTMSAB_ActiveFlag = b.TTMSAB_ActiveFlag,
                                        staffName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == " " || a.HRME_EmployeeMiddleName == "0" ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == " " || a.HRME_EmployeeLastName == "0" ? " " : a.HRME_EmployeeLastName),
                                    }
                                       ).Distinct().OrderBy(e=>e.staffName).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }

        public TTStaffMasterDTO getpageedit(int id)
        {
            TTStaffMasterDTO page = new TTStaffMasterDTO();
            try
            {
             var   lorg =(from a in  _ttcategorycontext.TT_Master_Staff_AbbreviationDMO
                          from b in _ttcategorycontext.HR_Master_Employee_DMO
                          where a.TTMSAB_Id==id && a.MI_Id==b.MI_Id && a.HRME_Id==b.HRME_Id
                          select new TTStaffMasterDTO
                          {
                              TTMSAB_Id =a.TTMSAB_Id,
                              HRME_Id =a.HRME_Id,
                              TTMSAB_Abbreviation =a.TTMSAB_Abbreviation,
                             TTMSAB_PerDayMaxDeputation =a.TTMSAB_PerDayMaxDeputation,
                             TTMSAB_PerWeekMaxDeputation =a.TTMSAB_PerWeekMaxDeputation,
                              TTMSAB_PerMonthMaxDeputation=a.TTMSAB_PerMonthMaxDeputation,
                          TTMSAB_PerYearMaxDeputation=a.TTMSAB_PerYearMaxDeputation,
                          staffName = b.HRME_EmployeeFirstName + " " + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == " " || b.HRME_EmployeeMiddleName == "0" ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == " " || b.HRME_EmployeeLastName == "0" ? " " : b.HRME_EmployeeLastName),
                          }
                          ).Distinct().ToList();
                page.sujectslistedit = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public TTStaffMasterDTO deleterec(int id)
        {
            TTStaffMasterDTO page = new TTStaffMasterDTO();
            try
            {
                List<TT_Master_Staff_AbbreviationDMO> lorg = new List<TT_Master_Staff_AbbreviationDMO>();
                lorg = _ttcategorycontext.TT_Master_Staff_AbbreviationDMO.Where(t => t.TTMSAB_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    _ttcategorycontext.Remove(lorg.ElementAt(0));
                    var contactExists = _ttcategorycontext.SaveChanges();
                    if (contactExists == 1)
                    {
                        page.returnval = true;
                    }
                    else
                    {
                        page.returnval = false;
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        //active deactive 
        public TTStaffMasterDTO deactivate(TTStaffMasterDTO data)
        {
            TT_Master_Staff_AbbreviationDMO pge = Mapper.Map<TT_Master_Staff_AbbreviationDMO>(data);
            if (pge.TTMSAB_Id > 0)
            {
                var result = _ttcategorycontext.TT_Master_Staff_AbbreviationDMO.Single(t => t.TTMSAB_Id == pge.TTMSAB_Id);
                if (result.TTMSAB_ActiveFlag == true)
                {
                    result.TTMSAB_ActiveFlag = false;
                }
                else
                {
                    result.TTMSAB_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _ttcategorycontext.Update(result);
                var flag = _ttcategorycontext.SaveChanges();
                if (flag == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }


            return data;
        }



    }
}
