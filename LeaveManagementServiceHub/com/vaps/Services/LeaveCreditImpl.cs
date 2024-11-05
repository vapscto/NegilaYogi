using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vapstech.LeaveManagement;
using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace LeaveManagementServiceHub.com.vaps.Services 
{
    public class LeaveCreditImpl : LeaveCreditInterface
    {
        private readonly DomainModelMsSqlServerContext _db;
        private readonly ILogger<LeaveCreditImpl> _log;

        public LMContext _lmContext;
        public LeaveCreditImpl(LMContext ttcategory)
        {
            _lmContext = ttcategory;
        }

        public LeaveCreditDTO getleave(LeaveCreditDTO data)
        {
            List<HR_Master_GroupType_DMO> staf_types = new List<HR_Master_GroupType_DMO>();
            staf_types = _lmContext.HR_Master_GroupType_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMGT_ActiveFlag == true).ToList();
            data.stf_types = staf_types.Distinct().ToArray();

            List<HR_Master_Department_DMO> Department_types = new List<HR_Master_Department_DMO>();
            Department_types = _lmContext.HR_Master_Department_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).ToList();
            data.Department_types = Department_types.Distinct().ToArray();


            List<HR_Master_Designation_DMO> Designation_types = new List<HR_Master_Designation_DMO>();
            Designation_types = _lmContext.HR_Master_Designation_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMDES_ActiveFlag == true).ToList();
            data.Designation_types = Designation_types.Distinct().ToArray();

            List<HR_Master_Leave_DMO> leave_name = new List<HR_Master_Leave_DMO>();
            leave_name = _lmContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRML_LeaveCreditFlg == true).ToList();
            data.leave_name = leave_name.Distinct().ToArray();

            List<HR_Master_Grade_DMO> gradename = new List<HR_Master_Grade_DMO>();
            gradename = _lmContext.HR_Master_Grade_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMG_ActiveFlag == true).ToList();
            data.grade_name = gradename.Distinct().ToArray();

            List<IVRM_Month_DMO> credit_month = new List<IVRM_Month_DMO>();
            credit_month = _lmContext.IVRM_Month_DMO.Where(t => t.Is_Active == true).ToList();
            data.credit_month = credit_month.Distinct().ToArray();

            List<HR_Master_EarningsDeductions_DMO> earnded = new List<HR_Master_EarningsDeductions_DMO>();
            earnded = _lmContext.HR_Master_EarningsDeductions_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMED_ActiveFlag == true).ToList();
            data.earnded = earnded.Distinct().ToArray();

            data.result = (from c in _lmContext.HR_Master_Leave_Details_DMO
                           from b in _lmContext.HR_Master_Leave_DMO
                           from d in _lmContext.HR_Master_Designation_DMO
                           from e in _lmContext.HR_Master_Department_DMO
                          where(c.MI_Id == data.MI_Id && c.HRMD_Id == e.HRMD_Id && d.HRMDES_Id == c.HRMDES_Id && b.MI_Id == data.MI_Id && b.HRML_Id == c.HRML_Id && c.HRMDES_Id == d.HRMDES_Id && d.MI_Id == data.MI_Id && e.MI_Id == data.MI_Id)
                           select new LeaveCreditDTO
                           {
                               HRMLD_Id = c.HRMLD_Id,
                               HRMLD_NoOfDays=c.HRMLD_NoOfDays,
                               HRMLD_MaxLeaveApplicable=c.HRMLD_MaxLeaveApplicable,
                               HRMDES_DesignationName=d.HRMDES_DesignationName,
                               HRMD_DepartmentName=e.HRMD_DepartmentName,
                               HRML_LeaveName = b.HRML_LeaveName
                           }
             ).Distinct().ToArray();           

            return data;
        }

        public LeaveCreditDTO get_leavecode(LeaveCreditDTO data)
        {
            data.leave_code = (from a in _lmContext.HR_Master_Leave_DMO
                               where (a.HRML_Id == data.leavecode)
                               select new LeaveCreditDTO
                               {
                                   HRML_Id = a.HRML_Id,
                                   HRML_LeaveCode = a.HRML_LeaveCode,
                               }
                     ).Distinct().ToArray();

            return data;
        }

        public LeaveCreditDTO get_departments(LeaveCreditDTO data)
        {
            List<long> selected_emp_types = new List<long>();

            foreach (var itm in data.emptypes)
            {
                selected_emp_types.Add(itm.HRMGT_Id);
            }

            data.Department_types = (from a in _lmContext.HR_Master_Employee_DMO
                                     from b in _lmContext.HR_Master_Department_DMO
                                     from c in _lmContext.HR_Master_GroupType_DMO
                                     where (a.HRMD_Id == b.HRMD_Id && a.HRMGT_Id == c.HRMGT_Id && c.HRMGT_ActiveFlag == true
                                          && b.HRMD_ActiveFlag == true && a.HRME_ActiveFlag == true && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id
                                          && selected_emp_types.Contains(c.HRMGT_Id))
                                     select new LeaveCreditDTO
                                     {
                                         HRMD_Id = b.HRMD_Id,
                                         HRMD_DepartmentName = b.HRMD_DepartmentName,
                                     }
                     ).Distinct().ToArray();

            return data;
        }

        public LeaveCreditDTO get_designation(LeaveCreditDTO data)
        {
            List<long> selected_desg_types = new List<long>();

            foreach (var itm in data.emptypes)
            {
                selected_desg_types.Add(itm.HRMD_Id);
            }

            data.Designation_types = (from a in _lmContext.HR_Master_Employee_DMO
                                      from b in _lmContext.HR_Master_Designation_DMO
                                      from c in _lmContext.HR_Master_Department_DMO

                                      where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
                                      && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true
                                       && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && selected_desg_types.Contains(c.HRMD_Id))
                                      select new LeaveCreditDTO
                                      {
                                          HRMDES_Id = b.HRMDES_Id,
                                          HRMDES_DesignationName = b.HRMDES_DesignationName,
                                      }
                     ).Distinct().ToArray();

            return data;
        }


        public LeaveCreditDTO get_grade(LeaveCreditDTO data)
        {
            List<long> selected_grade_types = new List<long>();

            foreach (var itm in data.emptypes)
            {
                selected_grade_types.Add(itm.HRMG_Id);
            }

            data.grade_name = (from a in _lmContext.HR_Master_Employee_DMO
                               from b in _lmContext.HR_Master_Designation_DMO
                               from c in _lmContext.HR_Master_Department_DMO
                               from d in _lmContext.HR_Master_Grade_DMO
                               where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && a.HRMG_Id == d.HRMG_Id && c.HRMD_ActiveFlag == true
                               && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true && d.HRMG_ActiveFlag == true
                                && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && selected_grade_types.Contains(d.HRMG_Id))
                               select new LeaveCreditDTO
                               {
                                   HRMG_Id = d.HRMG_Id,
                                   HRMG_GradeName = d.HRMG_GradeName,
                               }
                     ).Distinct().ToArray();

            return data;
        }


        public LeaveCreditDTO SaveData(LeaveCreditDTO data)

        {
            HR_Master_Leave_Details_DMO objpge = Mapper.Map<HR_Master_Leave_Details_DMO>(data);
            try
            {

                if (data.HRMLD_Id > 0)
                {

                    var result = _lmContext.HR_Master_Leave_Details_DMO.Single(t => t.MI_Id == data.MI_Id && t.HRML_Id == data.HRML_Id);
                    // data.HRMLD_Id = result.HRMLD_Id;
                    data.HRMLD_NoOfDays = result.HRMLD_NoOfDays;
                    data.HRMLD_MaxLeaveApplicable = result.HRMLD_MaxLeaveApplicable;
                    data.UpdatedDate = result.CreatedDate;
                    data.CreatedDate = result.CreatedDate;
                    Mapper.Map(data, result);
                    _lmContext.Update(result);
                    var flag = _lmContext.SaveChanges();
                    if (flag > 0)
                    {
                        data.Edit_flag = true;
                    }
                    else
                    {
                        data.Edit_flag = false;
                    }
                }
                else
                {

                    for (int i = 0; i < data.Emp_types.Length; i++)
                    {
                        for (int j = 0; j < data.dept_types.Length; j++)
                        {
                            for (int k = 0; k < data.desig_types.Length; k++)
                            {
                                for (int l = 0; l < data.grade_types.Length; l++)
                                {
                                    LeaveCreditDTO ttlldto = new LeaveCreditDTO();
                                    HR_Master_Leave_Details_DMO MM = Mapper.Map<HR_Master_Leave_Details_DMO>(ttlldto);
                                    MM.HRML_Id = data.HRML_Id;
                                    MM.MI_Id = data.MI_Id;
                                    MM.HRMLD_NoOfDays = data.HRMLD_NoOfDays;
                                    MM.HRMLD_MaxLeaveApplicable = data.HRMLD_MaxLeaveApplicable;
                                    MM.CreatedDate = DateTime.Now;
                                    MM.UpdatedDate = DateTime.Now;
                                    MM.HRMLD_CreatedBy = data.LoginId;
                                    MM.HRMLD_UpdatedBy = data.LoginId;
                                    MM.HRMGT_Id = Convert.ToInt64(data.Emp_types[i].HRMGT_Id);
                                    MM.HRMD_Id = Convert.ToInt64(data.dept_types[j].HRMD_Id);
                                    MM.HRMDES_Id = Convert.ToInt64(data.desig_types[k].HRMDES_Id);
                                    MM.HRMG_Id = Convert.ToInt64(data.grade_types[l].HRMG_Id);
                                    MM.HRMLD_CarryForFlg = data.HRMLD_CarryForFlg;
                                    MM.HRMLD_EncashFlg = data.HRMLD_EncashFlg;                                   
                                    _lmContext.Add(MM);
                                    long leavedetail = _lmContext.SaveChanges();
                                    if (leavedetail > 0)
                                    {
                                        if (data.HRMLD_CarryForFlg == true) {
                                            HR_Master_Leave_Details_CF_DMO obj = new HR_Master_Leave_Details_CF_DMO();
                                            obj.HRMLD_Id = MM.HRMLD_Id;
                                            obj.HRMLDCF_MaxLeaveAplFlg = data.HRMLDCF_MaxLeaveAplFlg;
                                            obj.HRMLDCF_MaxLeaveCF = data.HRMLDCF_MaxLeaveCF;
                                            obj.CreatedDate = DateTime.Now;
                                            obj.UpdatedDate = DateTime.Now;
                                            obj.HRMLDCF_UpdatedBy = data.LoginId;
                                            obj.HRMLDCF_CreatedBy = data.LoginId;
                                            _lmContext.Add(obj);
                                            long cfid = _lmContext.SaveChanges();
                                            for (int mo = 0; mo < data.selectedcarryMonthList.Length; mo++)
                                            {
                                                HR_Master_Leave_Details_CFMonth_DMO obj2 = new HR_Master_Leave_Details_CFMonth_DMO();
                                                obj2.HRMLDCF_Id = obj.HRMLDCF_Id;
                                                obj2.HRMLDCFM_MonthId = data.selectedcarryMonthList[mo];
                                                obj2.CreatedDate = DateTime.Now;
                                                obj2.UpdatedDate = DateTime.Now;
                                                obj2.HRMLDCFM_CreatedBy = data.LoginId;
                                                obj2.HRMLDCFM_UpdatedBy = data.LoginId;
                                                _lmContext.Add(obj2);
                                                _lmContext.SaveChanges();
                                            }
                                        }
                                        if (data.HRMLD_EncashFlg == true) {
                                            HR_Master_Leave_Details_EnCash_DMO obj3 = Mapper.Map<HR_Master_Leave_Details_EnCash_DMO>(data);
                                            obj3.HRMLD_Id = MM.HRMLD_Id;
                                            obj3.CreatedDate = DateTime.Now;
                                            obj3.UpdatedDate = DateTime.Now;
                                            obj3.HRMLDEC_CreatedBy = data.LoginId;
                                            obj3.HRMLDEC_UpdatedBy = data.LoginId;
                                            _lmContext.Add(obj3);
                                            long encid = _lmContext.SaveChanges();
                                        }
                                        for (int moo = 0; moo < data.selectedMonthList.Length; moo++)
                                        {
                                            HR_Master_Leave_Details_CreditMonth_DMO obj2 = new HR_Master_Leave_Details_CreditMonth_DMO();
                                            obj2.HRMLD_Id = MM.HRMLD_Id;
                                            obj2.HRMLDCM_LCMonthCode = Convert.ToString(data.selectedMonthList[moo]);
                                            obj2.CreatedDate = DateTime.Now;
                                            obj2.UpdatedDate = DateTime.Now;
                                            obj2.HRMLDCM_UpdatedBy = data.LoginId;
                                            obj2.HRMLDCM_CreatedBy = data.LoginId;
                                            _lmContext.Add(obj2);
                                            _lmContext.SaveChanges();
                                        }
                                        data.returnval = true;
                                    }
                                    else
                                    {
                                        data.returnval = false;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _log.LogInformation("Leave Details error");
                _log.LogDebug(ex.Message);
            }

            return data;
        }

        public LeaveCreditDTO LeaveMaster(LeaveCreditDTO data)
        {
            try
            {
                //add & update Activity details
                if (data.HRMLDCM_Id > 0)
                {
                    var result = _lmContext.HR_Master_Leave_DMO.Single(t => t.MI_Id == data.MI_Id && t.HRML_Id == data.HRMLDCM_Id);
                    data.HRML_Id = result.HRML_Id;
                    data.HRML_LeaveName = result.HRML_LeaveName;
                    data.HRML_LeaveCode = result.HRML_LeaveCode;
                    data.UpdatedDate = DateTime.Now;
                    data.CreatedDate = result.CreatedDate;


                    Mapper.Map(data, result);
                    _lmContext.Update(result);
                    var flag = _lmContext.SaveChanges();
                    if (flag > 0)
                    {
                        data.Edit_flag = true;
                    }
                    else
                    {
                        data.Edit_flag = false;
                    }
                }

                else
                {
                    HR_Master_Leave_DMO MM1 = new HR_Master_Leave_DMO();
                    MM1.MI_Id = data.MI_Id;
                    MM1.HRML_LeaveCode = data.HRML_LeaveCode;
                    MM1.CreatedDate = DateTime.Now;
                    MM1.UpdatedDate = DateTime.Now;

                    _lmContext.Add(MM1);

                    data.HRML_Id = MM1.HRML_Id;
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Leave details error");
                _log.LogDebug(e.Message);
            }

            return data;
        }
        public LeaveCreditDTO LeaveDetailsCreditMonth(LeaveCreditDTO data)
        {
            try
            {
                //add & update Activity details
                if (data.HRMLDCM_Id > 0)
                {

                    var result = _lmContext.HR_Master_Leave_Details_CreditMonth_DMO.Single(t => t.HRMLDCM_Id == data.HRMLDCM_Id);
                    data.HRMLDCM_Id = result.HRMLDCM_Id;
                    data.HRMLDCM_LCMonthCode = result.HRMLDCM_LCMonthCode;
                    data.UpdatedDate = DateTime.Now;
                    data.CreatedDate = result.CreatedDate;
                    data.HRMLDCM_Id = result.HRMLDCM_Id;

                    Mapper.Map(data, result);
                    _lmContext.Update(result);
                    var flag = _lmContext.SaveChanges();
                    if (flag > 0)
                    {
                        data.Edit_flag = true;
                    }
                    else
                    {
                        data.Edit_flag = false;
                    }
                }

                else
                {
                    HR_Master_Leave_Details_CreditMonth_DMO MM2 = new HR_Master_Leave_Details_CreditMonth_DMO();
                    //MM2.MI_Id = data.MI_Id;
                    MM2.HRMLDCM_Id = data.HRMLDCM_Id;
                    MM2.HRMLDCM_LCMonthCode = data.HRMLDCM_LCMonthCode;
                    MM2.CreatedDate = DateTime.Now;
                    MM2.UpdatedDate = DateTime.Now;

                    _lmContext.Add(MM2);

                    data.HRMLDCM_Id = MM2.HRMLDCM_Id;
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Leave details error");
                _log.LogDebug(e.Message);
            }

            return data;
        }
        public LeaveCreditDTO LeaveDetailsCF(LeaveCreditDTO data)
        {
            try
            {
                //add & update Activity details
                if (data.HRMLDCF_Id > 0)
                {

                    var result = _lmContext.HR_Master_Leave_Details_CF_DMO.Single(t => t.HRMLDCF_Id == data.HRMLDCF_Id);
                    data.HRMLDCF_Id = result.HRMLDCF_Id;
                    data.HRMLDCF_MaxLeaveAplFlg = result.HRMLDCF_MaxLeaveAplFlg;
                    data.UpdatedDate = DateTime.Now;
                    data.CreatedDate = result.CreatedDate;


                    Mapper.Map(data, result);
                    _lmContext.Update(result);
                    var flag = _lmContext.SaveChanges();
                    if (flag > 0)
                    {
                        data.Edit_flag = true;
                    }
                    else
                    {
                        data.Edit_flag = false;
                    }
                }

                else
                {
                    HR_Master_Leave_Details_CF_DMO MM3 = new HR_Master_Leave_Details_CF_DMO();
                    MM3.HRMLDCF_Id = data.HRMLDCF_Id;
                    MM3.HRMLDCF_MaxLeaveAplFlg = data.HRMLDCF_MaxLeaveAplFlg;
                    MM3.CreatedDate = DateTime.Now;
                    MM3.UpdatedDate = DateTime.Now;

                    _lmContext.Add(MM3);

                    data.HRMLDCF_Id = MM3.HRMLDCF_Id;
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Leave details error");
                _log.LogDebug(e.Message);
            }

            return data;
        }
        public LeaveCreditDTO LeaveDetailsCFmonth(LeaveCreditDTO data)
        {
            try
            {
                //add & update Activity details
                if (data.HRMLDCFM_Id > 0)
                {

                    var result = _lmContext.HR_Master_Leave_Details_CFMonth_DMO.Single(t => t.HRMLDCFM_Id == data.HRMLDCFM_Id);
                    data.HRMLDCF_Id = result.HRMLDCF_Id;

                    data.UpdatedDate = DateTime.Now;
                    data.CreatedDate = result.CreatedDate;


                    Mapper.Map(data, result);
                    _lmContext.Update(result);
                    var flag = _lmContext.SaveChanges();
                    if (flag > 0)
                    {
                        data.Edit_flag = true;
                    }
                    else
                    {
                        data.Edit_flag = false;
                    }
                }

                else
                {
                    HR_Master_Leave_Details_CFMonth_DMO MM4 = new HR_Master_Leave_Details_CFMonth_DMO();
                    MM4.HRMLDCFM_MonthId = data.HRMLDCFM_MonthId;
                    MM4.CreatedDate = DateTime.Now;
                    MM4.UpdatedDate = DateTime.Now;

                    _lmContext.Add(MM4);

                    data.HRMLDCFM_Id = MM4.HRMLDCFM_Id;
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Leave details error");
                _log.LogDebug(e.Message);
            }

            return data;
        }
        public LeaveCreditDTO LeaveDetailsEnCash(LeaveCreditDTO data)
        {
            try
            {
                //add & update Activity details
                if (data.HRMLDEC_Id > 0)
                {

                    var result = _lmContext.HR_Master_Leave_Details_EnCash_DMO.Single(t => t.HRMLDEC_Id == data.HRMLDEC_Id && t.HRMLDEC_ServiceAplFlg == data.HRMLDEC_ServiceAplFlg && t.HRMLDEC_MaxLeaveFlg == data.HRMLDEC_MaxLeaveFlg && t.HRMLDEC_MinLeaveFlg == data.HRMLDEC_MinLeaveFlg && t.HRMLDEC_ScheduleFlg == data.HRMLDEC_ScheduleFlg);
                    data.HRMLDEC_Id = result.HRMLDEC_Id;

                    data.HRMLDEC_ServiceAplFlg = result.HRMLDEC_ServiceAplFlg;
                    data.HRMLDEC_ServiceYear = result.HRMLDEC_ServiceYear;
                    data.HRMLDEC_ServiceMonth = result.HRMLDEC_ServiceMonth;
                    data.HRMLDEC_ServiceDays = result.HRMLDEC_ServiceDays;
                    data.HRMLDEC_MaxLeaveFlg = result.HRMLDEC_MaxLeaveFlg;
                    data.HRMLDEC_MaxLeaves = result.HRMLDEC_MaxLeaves;
                    data.HRMLDEC_MinLeaveFlg = result.HRMLDEC_MinLeaveFlg;
                    data.HRMLDEC_MinLeaves = result.HRMLDEC_MinLeaves;
                    data.HRMLDEC_ScheduleFlg = result.HRMLDEC_ScheduleFlg;
                    data.HRMLDEC_ScheduleYear = result.HRMLDEC_ScheduleYear;
                    data.HRMLDEC_ScheduleMonth = result.HRMLDEC_ScheduleMonth;

                    data.UpdatedDate = DateTime.Now;
                    data.CreatedDate = result.CreatedDate;

                    Mapper.Map(data, result);
                    _lmContext.Update(result);
                    var flag = _lmContext.SaveChanges();
                    if (flag > 0)
                    {
                        data.Edit_flag = true;
                    }
                    else
                    {
                        data.Edit_flag = false;
                    }
                }

                else
                {
                    HR_Master_Leave_Details_EnCash_DMO MM5 = new HR_Master_Leave_Details_EnCash_DMO();
                    MM5.HRMLDEC_ServiceAplFlg = data.HRMLDEC_ServiceAplFlg;
                    MM5.HRMLDEC_ServiceYear = data.HRMLDEC_ServiceYear;
                    MM5.HRMLDEC_ServiceMonth = data.HRMLDEC_ServiceMonth;
                    MM5.HRMLDEC_ServiceDays = data.HRMLDEC_ServiceDays;
                    MM5.HRMLDEC_MaxLeaveFlg = data.HRMLDEC_MaxLeaveFlg;
                    MM5.HRMLDEC_MaxLeaves = data.HRMLDEC_MaxLeaves;
                    MM5.HRMLDEC_MinLeaveFlg = data.HRMLDEC_MinLeaveFlg;
                    MM5.HRMLDEC_MinLeaves = data.HRMLDEC_MinLeaves;
                    MM5.HRMLDEC_ScheduleFlg = data.HRMLDEC_ScheduleFlg;
                    MM5.HRMLDEC_ScheduleYear = data.HRMLDEC_ScheduleYear;
                    MM5.HRMLDEC_ScheduleMonth = data.HRMLDEC_ScheduleMonth;
                    MM5.CreatedDate = DateTime.Now;
                    MM5.UpdatedDate = DateTime.Now;

                    _lmContext.Add(MM5);

                    data.HRMLDEC_Id = MM5.HRMLDEC_Id;
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Leave details error");
                _log.LogDebug(e.Message);
            }

            return data;
        }

    }
}
