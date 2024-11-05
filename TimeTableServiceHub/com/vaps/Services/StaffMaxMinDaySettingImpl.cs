using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.TT;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTableServiceHub.com.vaps.Interfaces;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class StaffMaxMinDaySettingImpl : StaffMaxMinDaySettingInterface
    {
        private readonly TTContext _ttcontext;

        public StaffMaxMinDaySettingImpl(TTContext obj)
        {
            _ttcontext = obj;
        }
        public StaffMaxMinDaySettingDTO getdetails(StaffMaxMinDaySettingDTO data)
        {
            try
            {
                List<AcademicYear> year = new List<AcademicYear>();
                year = _ttcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToList();
                data.Acdlist = year.Distinct().ToArray();

                List<TTMasterCategoryDMO> mcat = new List<TTMasterCategoryDMO>();
                mcat = _ttcontext.TTMasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToList();
                data.ctlist = mcat.Distinct().ToArray();

                data.stafflist = (from e in _ttcontext.HR_Master_Employee_DMO
                                  where (e.MI_Id == data.MI_Id && e.HRME_ActiveFlag.Equals(true))

                                  select new StaffMaxMinDaySettingDTO
                                  {
                                      HRME_Id = e.HRME_Id,
                                      FirstName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                      //MiddleName = HR_Master_Employee_DMO.HRME_EmployeeMiddleName,
                                      //LastName = HR_Master_Employee_DMO.HRME_EmployeeLastName
                                  }
                                  ).OrderBy(d => d.FirstName).ToArray();

                List<TT_Master_PeriodDMO> lorg9 = new List<TT_Master_PeriodDMO>();
                lorg9 = _ttcontext.TT_Master_PeriodDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMP_ActiveFlag == true).ToList();
                data.periodlist = lorg9.ToArray();

                data.daylistdetail = (from a in _ttcontext.StaffMaxMinDaySettingDMO
                                      from b in _ttcontext.AcademicYear
                                      from c in _ttcontext.TTMasterCategoryDMO
                                      from d in _ttcontext.TT_Master_PeriodDMO
                                      from e in _ttcontext.HR_Master_Employee_DMO
                                      where (a.ASMAY_Id == b.ASMAY_Id && a.TTMP_Id == d.TTMP_Id && a.MI_Id == data.MI_Id && a.HRME_Id == e.HRME_Id && a.TTMC_Id == c.TTMC_Id)
                                      select new StaffMaxMinDaySettingDTO
                                      {
                                          TTPMMD_Id = a.TTPMMD_Id,
                                          academicyr = b.ASMAY_Year,
                                          catgname = c.TTMC_CategoryName,
                                          stafname = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                          period = d.TTMP_PeriodName,
                                          maxday = a.TTPMMD_MaxDay,
                                          minday = a.TTPMMD_MinDay,
                                          TTPMMD_ActiveFlag = a.TTPMMD_ActiveFlag

                                      }

                                      ).ToArray();
                data.count = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMD_ActiveFlag == true).Count();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


            public StaffMaxMinDaySettingDTO savedetail(StaffMaxMinDaySettingDTO data)
            {
                StaffMaxMinDaySettingDMO objpge = Mapper.Map<StaffMaxMinDaySettingDMO>(data);
                try
                {
                    if (objpge.TTPMMD_Id > 0)
                    {
                        var res = _ttcontext.StaffMaxMinDaySettingDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id && t.TTMP_Id == data.TTMP_Id && t.HRME_Id == data.HRME_Id && t.TTMC_Id == data.TTMC_Id).ToList();
                        
                            var result = _ttcontext.StaffMaxMinDaySettingDMO.Single(t => t.TTPMMD_Id == data.TTPMMD_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.HRME_Id == data.HRME_Id && t.TTMC_Id == data.TTMC_Id);
                            result.TTMP_Id = data.TTMP_Id;
                            result.TTPMMD_MaxDay = data.TTPMMD_MaxDay;
                            result.TTPMMD_MinDay = data.TTPMMD_MinDay;
                            result.ASMAY_Id = data.ASMAY_Id;
                            result.HRME_Id = data.HRME_Id;
                            result.TTMC_Id = data.TTMC_Id;
                            result.TTPMMD_ActiveFlag = true;
                            result.UpdatedDate = DateTime.Now;
                            _ttcontext.Update(result);
                            var contactExists = _ttcontext.SaveChanges();
                            if (contactExists == 1)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                    }
                    else
                    {
                        var result = _ttcontext.StaffMaxMinDaySettingDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id && t.TTMP_Id == data.TTMP_Id && t.HRME_Id == data.HRME_Id && t.TTMC_Id == data.TTMC_Id).ToList();
                        if (result.Count() > 0)
                        {
                            data.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            objpge.CreatedDate = DateTime.Now;
                            objpge.UpdatedDate = DateTime.Now;
                            objpge.TTPMMD_ActiveFlag = true;
                            _ttcontext.Add(objpge);
                            var contactExists = _ttcontext.SaveChanges();
                            if (contactExists == 1)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
                return data;
            }

        public StaffMaxMinDaySettingDTO getdetail(int id)
        {
            StaffMaxMinDaySettingDTO page = new StaffMaxMinDaySettingDTO();
            try
            {
                page.detailedit = (from a in _ttcontext.StaffMaxMinDaySettingDMO
                                   where (a.TTPMMD_Id == id)
                                   select new StaffMaxMinDaySettingDTO
                                   {
                                       TTPMMD_Id = a.TTPMMD_Id,
                                       ASMAY_Id = a.ASMAY_Id,
                                       TTMC_Id = a.TTMC_Id,
                                       HRME_Id = a.HRME_Id,
                                       TTMP_Id =a.TTMP_Id,
                                       TTPMMD_MaxDay = a.TTPMMD_MaxDay,
                                       TTPMMD_MinDay = a.TTPMMD_MinDay,
                                       TTPMMD_ActiveFlag = a.TTPMMD_ActiveFlag
                                   }
                                  ).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public StaffMaxMinDaySettingDTO deactive(StaffMaxMinDaySettingDTO acd)
        {
            try
            {
                if (acd.TTPMMD_Id > 0)
                {
                    var result = _ttcontext.StaffMaxMinDaySettingDMO.Single(t => t.TTPMMD_Id.Equals(acd.TTPMMD_Id));
                    if (result.TTPMMD_ActiveFlag.Equals(false))
                    {
                        result.TTPMMD_ActiveFlag = true;
                    }
                    else
                    {
                        result.TTPMMD_ActiveFlag = false;
                    }
                    _ttcontext.Update(result);
                    var flag = _ttcontext.SaveChanges();
                    if (flag.Equals(1))
                    {
                        acd.returnval = true;
                    }
                    else
                    {
                        acd.returnval = false;
                    }
                }
            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return acd;
        }


    }
  
    }
