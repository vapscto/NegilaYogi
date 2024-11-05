using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.TT;
using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LeaveManagementServiceHub.com.vaps.Services
{
    public class LeaveTransactionManualImpl : LeaveTransactionManualInterface
    {
        public LMContext _lmContext;
        public LeaveTransactionManualImpl(LMContext ttcategory)
        {
            _lmContext = ttcategory;
        }

        public LeaveCreditDTO getLeavetransm(LeaveCreditDTO data)
        {
            List<HR_Master_GroupType_DMO> staf_types = new List<HR_Master_GroupType_DMO>();
            staf_types = _lmContext.HR_Master_GroupType_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMGT_ActiveFlag == true).ToList(); //
            data.stf_types = staf_types.Distinct().ToArray();

            List<HR_Master_Department_DMO> Department_types = new List<HR_Master_Department_DMO>();
            Department_types = _lmContext.HR_Master_Department_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).ToList(); //
            data.Department_types = Department_types.Distinct().ToArray();


            List<HR_Master_Designation_DMO> Designation_types = new List<HR_Master_Designation_DMO>();
            Designation_types = _lmContext.HR_Master_Designation_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMDES_ActiveFlag == true).ToList(); //
            data.Designation_types = Designation_types.Distinct().ToArray();


            List<HR_Master_LeaveYear_DMO> leavearrayyear = new List<HR_Master_LeaveYear_DMO>();
            leavearrayyear = _lmContext.HR_Master_LeaveYear_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMLY_ActiveFlag == true).OrderBy(t => t.HRMLY_LeaveYearOrder).ToList();
            data.get_year = leavearrayyear.Distinct().ToArray();

            data.get_emp = (from x in _lmContext.HR_Master_Employee_DMO
                            where (x.MI_Id == data.MI_Id && x.HRME_ActiveFlag == true)
                            select new HR_Master_Employee_DMO
                            {
                                HRME_Id = x.HRME_Id,
                                HRME_EmployeeFirstName = ((x.HRME_EmployeeFirstName == null ? "" : x.HRME_EmployeeFirstName) + (x.HRME_EmployeeMiddleName == null ? "" : " " + x.HRME_EmployeeMiddleName) + (x.HRME_EmployeeLastName == null ? "" : " " + x.HRME_EmployeeLastName)).Trim()
                            }
                   ).Distinct().ToArray();

            data.master_loplist = _lmContext.HR_Emp_Leave_Trans_Details_DMO.Where(d => d.MI_Id == data.MI_Id).ToArray();

            data.get_emp_lop = (from a in _lmContext.HR_Emp_Leave_Trans_Details_DMO
                                from b in _lmContext.HR_Master_Leave_DMO
                                from c in _lmContext.HR_Master_Employee_DMO
                                from d in _lmContext.HR_Emp_Leave_Trans_DMO
                                where (a.HRELT_Id == d.HRELT_Id &&
                                a.HRML_Id == b.HRML_Id && a.MI_Id == data.MI_Id && a.HRME_Id == c.HRME_Id)

                                select new LeaveCreditDTO
                                {
                                    HRELTD_Id = a.HRELTD_Id,
                                    HRME_EmployeeFirstName = c.HRME_EmployeeFirstName + (string.IsNullOrEmpty(c.HRME_EmployeeMiddleName) ? "" : ' ' + c.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(c.HRME_EmployeeLastName) ? "" : ' ' + c.HRME_EmployeeLastName),
                                    HRML_LeaveName = b.HRML_LeaveName,
                                    HRELTD_FromDate = a.HRELTD_FromDate,
                                    HRELTD_ToDate = a.HRELTD_ToDate,
                                    HRELTD_TotDays = a.HRELTD_TotDays,
                                    HRELT_Id = d.HRELT_Id,
                                    HRELT_ActiveFlag = d.HRELT_ActiveFlag,
                                    HRME_Id= a.HRME_Id,

                                }
                      ).Distinct().OrderByDescending(t=>t.HRELTD_Id).ToArray();


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

        public LeaveCreditDTO get_employee(LeaveCreditDTO data)
        {

            List<long> selected_typ = new List<long>();

            foreach (var itm in data.emptypes)
            {
                selected_typ.Add(itm.HRMGT_Id);
            }
            List<long> selected_dep = new List<long>();

            foreach (var itm in data.empdept)
            {
                selected_dep.Add(itm.HRMD_Id);
            }
            List<long> selected_des = new List<long>();

            foreach (var itm in data.empdesg)
            {
                selected_des.Add(itm.HRMDES_Id);
            }

            data.get_emp = (from a in _lmContext.HR_Master_Employee_DMO
                            from b in _lmContext.HR_Master_Designation_DMO
                            from c in _lmContext.HR_Master_Department_DMO
                            from d in _lmContext.HR_Master_GroupType_DMO
                            where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
                            && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true && a.HRMGT_Id == d.HRMGT_Id && d.HRMGT_ActiveFlag == true && a.HRME_LeftFlag == false
                             && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id == d.MI_Id && selected_typ.Contains(d.HRMGT_Id) && selected_dep.Contains(c.HRMD_Id) && selected_des.Contains(b.HRMDES_Id))
                            select new LeaveCreditDTO
                            {
                                HRME_Id = a.HRME_Id,
                                // HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? "" : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "" : " " + a.HRME_EmployeeLastName) + " :" + a.HRME_EmployeeCode).Trim()
                            }
                     ).Distinct().ToArray();
            return data;
        }

        public LeaveCreditDTO get_Emp_lop(LeaveCreditDTO data)
        {
            try
            {
                List<long> selected_emp_lop = new List<long>();
                foreach (var itm in data.emptypes)
                {
                    selected_emp_lop.Add(itm.HRME_Id);
                }

                data.get_emp_lop = (from a in _lmContext.HR_Emp_Leave_Trans_Details_DMO
                                    from b in _lmContext.HR_Master_Leave_DMO
                                    from c in _lmContext.HR_Master_Employee_DMO
                                    from d in _lmContext.HR_Emp_Leave_Trans_DMO
                                    where (a.HRELT_Id == d.HRELT_Id &&
                                    a.HRML_Id == b.HRML_Id && a.MI_Id == data.MI_Id && a.HRME_Id == c.HRME_Id && selected_emp_lop.Contains(a.HRME_Id))

                                    select new LeaveCreditDTO
                                    {
                                        HRELTD_Id = a.HRELTD_Id,
                                        HRME_EmployeeFirstName = c.HRME_EmployeeFirstName + (string.IsNullOrEmpty(c.HRME_EmployeeMiddleName) ? "" : ' ' + c.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(c.HRME_EmployeeLastName) ? "" : ' ' + c.HRME_EmployeeLastName),
                                        HRML_LeaveName = b.HRML_LeaveName,
                                        HRELTD_FromDate = a.HRELTD_FromDate,
                                        HRELTD_ToDate = a.HRELTD_ToDate,
                                        HRELTD_TotDays = a.HRELTD_TotDays,
                                        HRELT_Id = d.HRELT_Id,
                                        HRELT_ActiveFlag = d.HRELT_ActiveFlag,
                                        HRME_Id=d.HRME_Id,

                                    }
                         ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }




        public LeaveCreditDTO saveDATA(LeaveCreditDTO data)
        {
            int flag = 0;
            try
            {
                var checkfromtodate = (from a in _lmContext.HR_Emp_Leave_Trans_DMO
                                       from b in _lmContext.HR_Emp_Leave_Trans_Details_DMO
                                       where (a.HRELT_Id == b.HRELT_Id && a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id
                                       && a.HRELT_FromDate == data.HRELT_FromDate && a.HRELT_ToDate == data.HRELT_ToDate && a.HRELT_ActiveFlag==true)
                                       select new LeaveCreditDTO
                                       {
                                           HRELT_FromDate = a.HRELT_FromDate,
                                           HRELT_ToDate = a.HRELT_ToDate
                                       }).ToList();

                if (checkfromtodate.Count() > 0)
                {
                    // data.message = "duplicate";
                    data.returnduplicatestatus = "false";
                }
                else
                {
                    HR_Emp_Leave_Trans_DMO empltrans = Mapper.Map<HR_Emp_Leave_Trans_DMO>(data);
                    if (empltrans.HRELT_Id > 0)
                    {
                        var resultempltrans = _lmContext.HR_Emp_Leave_Trans_DMO.Single(t => t.HRELT_Id == empltrans.HRELT_Id);
                        data.HRELT_TotDays = data.HRELT_TotDays;
                        data.UpdatedDate = DateTime.Now;
                        Mapper.Map(data, resultempltrans);
                        _lmContext.Update(resultempltrans);
                        flag = _lmContext.SaveChanges();
                        if (flag > 0)
                        {
                            data.returnduplicatestatus = "Update";
                        }
                        else
                        {
                            data.returnduplicatestatus = "false";
                        }
                    }
                    else
                    {
                        var datalwp = _lmContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRML_LeaveType.Equals(data.HRML_LeaveType)).ToList();

                        if (datalwp.Count == 0)
                        {
                            data.returnduplicatestatus = "LWPNotSet";
                        }
                        else
                        {
                            empltrans.HRELT_LeaveId = datalwp.FirstOrDefault().HRML_Id;
                            empltrans.HRELT_TotDays = data.HRELT_TotDays;
                            empltrans.HRELT_LeaveReason = "LWP";
                            empltrans.HRELT_Status = "Approved";
                            empltrans.CreatedDate = DateTime.Now;
                            empltrans.UpdatedDate = DateTime.Now;
                            empltrans.HRELT_ActiveFlag = true;
                            _lmContext.Add(empltrans);
                            flag = _lmContext.SaveChanges();
                            if (flag > 0)
                            {
                                data.returnduplicatestatus = "Add";
                                data.HRELT_Id = empltrans.HRELT_Id;
                            }
                        }


                    }


                    if (empltrans.HRELT_Id > 0)
                    {
                        HR_Emp_Leave_Trans_Details_DMO objpge = Mapper.Map<HR_Emp_Leave_Trans_Details_DMO>(data);
                        if (objpge.HRELTD_Id > 0)
                        {
                            var resultempltransDetails = _lmContext.HR_Emp_Leave_Trans_Details_DMO.Single(t => t.HRELTD_Id == objpge.HRELTD_Id);
                            data.HRELTD_TotDays = data.HRELT_TotDays;
                            data.UpdatedDate = DateTime.Now;
                            Mapper.Map(data, resultempltransDetails);
                            _lmContext.Update(resultempltransDetails);
                            flag = _lmContext.SaveChanges();
                            if (flag > 0)
                            {
                                data.returnduplicatestatus = "Update";
                            }
                            else
                            {
                                data.returnduplicatestatus = "false";
                            }
                        }
                        else
                        {
                            var datalwp = _lmContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRML_LeaveType.Equals(data.HRML_LeaveType)).ToList();
                            if (datalwp.Count == 0)
                            {
                                data.returnduplicatestatus = "LWPNotSet";
                            }
                            else
                            {


                                objpge.HRELTD_TotDays = data.HRELT_TotDays;

                                objpge.HRELTD_FromDate = data.HRELT_FromDate;
                                objpge.HRELTD_ToDate = data.HRELT_ToDate;

                                objpge.HRML_Id = datalwp.FirstOrDefault().HRML_Id;
                                objpge.CreatedDate = DateTime.Now;
                                objpge.UpdatedDate = DateTime.Now;

                                _lmContext.Add(objpge);
                                flag = _lmContext.SaveChanges();
                                if (flag > 0)
                                {
                                    data.returnduplicatestatus = "Add";

                                }
                            }
                        }
                    }
                }

                data.master_loplist = _lmContext.HR_Emp_Leave_Trans_Details_DMO.Where(d => d.MI_Id == data.MI_Id).ToArray();

            }
            catch (Exception ee)
            {
                data.returnduplicatestatus = "error";
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public LeaveCreditDTO Deletedetails(LeaveCreditDTO data)
        {
            //LeaveCreditDTO data = new LeaveCreditDTO();

            if (data.HRELT_Id > 0)
            {
                var result = _lmContext.HR_Emp_Leave_Trans_DMO.Single(t => t.HRELT_Id == data.HRELT_Id);
                if (result.HRELT_ActiveFlag == true)
                {
                    result.HRELT_ActiveFlag = false;
                }
                else
                {
                    result.HRELT_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _lmContext.Update(result);
                var flag = _lmContext.SaveChanges();
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