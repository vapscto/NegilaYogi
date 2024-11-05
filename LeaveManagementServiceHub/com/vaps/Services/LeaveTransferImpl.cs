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
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementServiceHub.com.vaps.Services
{
    public class LeaveTransferImpl : LeaveTransferInterface
    {


        public LMContext _lmContext;
        public LeaveTransferImpl(LMContext ttcategory)
        {
            _lmContext = ttcategory;
        }

        public LeaveCreditDTO getBasicData(LeaveCreditDTO dto)
        {
            List<HR_Master_Leave_DMO> leaveyeardropdown = new List<HR_Master_Leave_DMO>();
            leaveyeardropdown = _lmContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == dto.MI_Id && t.HRML_LeaveCreditFlg == true).ToList(); //
            dto.leave_name = leaveyeardropdown.Distinct().ToArray();

            return dto;
            
        }

        public LeaveCreditDTO getLeaveOB(LeaveCreditDTO data)
        {
            List<HR_Master_GroupType_DMO> staf_types = new List<HR_Master_GroupType_DMO>();
            staf_types = _lmContext.HR_Master_GroupType_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMGT_ActiveFlag == true).ToList(); //
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

            List<HR_Master_LeaveYear_DMO> leavearrayyear = new List<HR_Master_LeaveYear_DMO>();
            leavearrayyear = _lmContext.HR_Master_LeaveYear_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMLY_ActiveFlag == true).OrderBy(t=>t.HRMLY_LeaveYearOrder).ToList();
            data.leavearrayyear = leavearrayyear.Distinct().ToArray();

            //data.HRML_Id = leave_name.

            //List<HR_Master_Employee_DMO> get_emp = new List<HR_Master_Employee_DMO>();
            //get_emp = _lmContext.HR_Master_Employee_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_ActiveFlag == true).ToList(); //
            //data.get_emp = get_emp.Distinct().ToArray();



            //data.get_emp = (from a in _lmContext.HR_Master_Employee_DMO
            //                where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true)
            //                select new HR_Master_Employee_DMO
            //                {
            //                    HRME_Id = a.HRME_Id,
            //                    HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim()
            //                }
            //        ).Distinct().ToArray();


            data.result = (from c in _lmContext.HR_Emp_Leave_StatusDMO
                           from b in _lmContext.HR_Master_Leave_DMO
                           from a in _lmContext.HR_Master_Employee_DMO
                           from x in _lmContext.HR_Master_LeaveYear_DMO
                           where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && b.HRML_Id == c.HRML_Id && a.HRME_Id == c.HRME_Id && x.MI_Id == data.MI_Id && x.HRMLY_ActiveFlag == true && c.HRMLY_Id == x.HRMLY_Id)
                           select new LeaveCreditDTO
                           {
                               HRMLY_Id = c.HRMLY_Id,
                               HRMLY_LeaveYear = x.HRMLY_LeaveYear,
                               HRELS_Id = c.HRELS_Id,
                               HRME_Id = c.HRME_Id,
                               HRML_Id = c.HRML_Id,
                               HRML_LeaveName = b.HRML_LeaveName,
                               HRELS_CBLeaves=c.HRELS_CBLeaves,
                               HRELS_TotalLeaves = c.HRELS_TotalLeaves,
                               HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim()
                           }).Distinct().ToArray();

            data.leave_year_id = (from x in _lmContext.HR_Master_LeaveYear_DMO
                                  where (x.MI_Id == data.MI_Id)
                                  select new HR_Master_LeaveYear_DMO
                                  {
                                      HRMLY_Id = x.HRMLY_Id
                                  }).Distinct().ToArray();
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
    
        public LeaveCreditDTO get_ob_Details(LeaveCreditDTO data)
        {


            return data;
        }
        public LeaveCreditDTO get_Employe_ob(LeaveCreditDTO data)
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
                            && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true && a.HRMGT_Id == d.HRMGT_Id && d.HRMGT_ActiveFlag == true
                             && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id == d.MI_Id && selected_typ.Contains(d.HRMGT_Id) && selected_dep.Contains(c.HRMD_Id) && selected_des.Contains(b.HRMDES_Id))
                            select new LeaveCreditDTO
                            {
                                HRME_Id = a.HRME_Id,
                                HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim()

                            }
                     ).Distinct().ToArray();


            return data;
        }

        //public LeaveCreditDTO SaveDetails(LeaveCreditDTO _category)
        //{
        //    HR_Emp_Leave_StatusDMO objpge = Mapper.Map<HR_Emp_Leave_StatusDMO>(_category);
        //    //  HR_Emp_OB_Leave_DMO objpge = Mapper.Map<HR_Emp_OB_Leave_DMO>(data);
        //    try
        //    {

        //        if (objpge.HRELS_Id > 0)
        //        {
        //            var resultCount = _lmContext.HR_Emp_Leave_StatusDMO.Where(t => t.HRELS_OBLeaves == objpge.HRELS_OBLeaves && t.HRELS_CreditedLeaves == objpge.HRELS_CreditedLeaves && t.HRELS_TotalLeaves == objpge.HRELS_TotalLeaves && t.HRELS_TransLeaves == objpge.HRELS_TransLeaves && t.MI_Id == objpge.MI_Id && t.HRML_Id == objpge.HRML_Id  && t.HRME_Id== objpge.HRME_Id && t.HRMLY_Id== objpge.HRMLY_Id && t.HRELS_EncashedLeaves==objpge.HRELS_EncashedLeaves && t.HRELS_CBLeaves== objpge.HRELS_CBLeaves).Count();
        //           // var resultCount = 0;
        //            if (resultCount == 0)
        //            {
        //                var result = _lmContext.HR_Emp_Leave_StatusDMO.Single(t => t.HRML_Id == objpge.HRML_Id && t.MI_Id == objpge.MI_Id  && t.HRME_Id==objpge.HRME_Id);
        //                result.HRELS_OBLeaves = objpge.HRELS_OBLeaves;
        //                result.HRELS_CreditedLeaves = objpge.HRELS_CreditedLeaves;
        //                result.HRELS_TotalLeaves = objpge.HRELS_TotalLeaves;
        //                result.HRELS_TransLeaves = objpge.HRELS_TransLeaves;
        //                result.MI_Id = objpge.MI_Id;
        //                result.HRMLY_Id = objpge.HRMLY_Id;
        //                result.HRML_Id = objpge.HRML_Id;
        //                result.HRELS_EncashedLeaves = result.HRELS_EncashedLeaves;
        //                result.HRELS_CBLeaves = result.HRELS_CBLeaves;

        //                result.UpdatedDate = DateTime.Now;

        //                _lmContext.Update(result);
        //                var contactExists = _lmContext.SaveChanges();
        //                if (contactExists == 1)
        //                {
        //                    _category.returnval = true;
        //                }
        //                else
        //                {
        //                    _category.returnval = false;
        //                }
        //            }
        //            else
        //            {
        //                _category.returnduplicatestatus = "Duplicate";
        //                return _category;
        //            }
        //        }
        //        else
        //        {
        //            //foreach (var itm1 in _category.temp_table_data)
        //            //{
        //            for(int i=0;i<_category.emplist.Length;i++)
        //            {
        //                HR_Emp_Leave_StatusDMO objpge1 = Mapper.Map<HR_Emp_Leave_StatusDMO>(_category);
        //                var result = _lmContext.HR_Emp_Leave_StatusDMO.Where(t => t.HRELS_CreditedLeaves == _category.emplist[i].HRELS_CreditedLeaves   && t.MI_Id == objpge1.MI_Id && t.HRML_Id == objpge1.HRML_Id && t.HRME_Id == _category.emplist[i].HRME_Id && t.HRML_Id== objpge1.HRML_Id && t.HRMLY_Id== objpge1.HRMLY_Id).Count();
        //                if (result > 0)
        //                {
        //                    _category.returnduplicatestatus = "Duplicate";
        //                }
        //                else if (result == 0)
        //                {
        //                    objpge1.MI_Id = _category.MI_Id;
        //                    objpge1.HRME_Id = _category.emplist[i].HRME_Id;
        //                    objpge1.HRML_Id = _category.HRML_Id;
        //                    objpge1.HRMLY_Id = _category.HRMLY_Id;
        //                    objpge1.HRELS_OBLeaves = 0;
        //                    objpge1.HRELS_CreditedLeaves = _category.emplist[i].HRELS_CreditedLeaves;
        //                    objpge1.HRELS_TotalLeaves = objpge1.HRELS_OBLeaves + objpge1.HRELS_CreditedLeaves;
        //                    objpge1.HRELS_TransLeaves =0;
        //                    objpge1.HRELS_EncashedLeaves = 0;
        //                    objpge1.HRELS_CBLeaves = objpge1.HRELS_TotalLeaves;

        //                    objpge1.CreatedDate = DateTime.Now;
        //                    objpge1.UpdatedDate = DateTime.Now;

        //                    _lmContext.Add(objpge1);


        //                    // _lmContext.Add(objpge);
        //                }

        //            }
        //            var contactExists = _lmContext.SaveChanges();
        //            if (contactExists >= 1)
        //            {
        //                _category.returnval = true;
        //            }
        //            else
        //            {
        //                _category.returnval = false;
        //            }

        //        }
        //        List<HR_Emp_Leave_StatusDMO> m_events = new List<HR_Emp_Leave_StatusDMO>();
        //            m_events = _lmContext.HR_Emp_Leave_StatusDMO.Where(e => e.MI_Id == _category.MI_Id).ToList();
        //            _category.master_eventlist = m_events.ToArray();



        //      //  }
        //    }
        //    catch (Exception ee)
        //    {

        //        Console.WriteLine(ee.Message);
        //    }
        //    return _category;
        //}


        public LeaveCreditDTO SaveDetails(LeaveCreditDTO _category)
        {
            //HR_Emp_Leave_StatusDMO objpge = Mapper.Map<HR_Emp_Leave_StatusDMO>(_category);          
            try
            {
                if (_category.HRELS_Id > 0)
                {
                    var resultCount = _lmContext.HR_Emp_Leave_StatusDMO.Where(t => t.HRELS_OBLeaves == _category.HRELS_OBLeaves && t.HRELS_CreditedLeaves == _category.HRELS_CreditedLeaves && t.HRELS_TotalLeaves == _category.HRELS_TotalLeaves && t.HRELS_TransLeaves == _category.HRELS_TransLeaves && t.MI_Id == _category.MI_Id && t.HRML_Id == _category.HRML_Id && t.HRME_Id == _category.HRME_Id && t.HRMLY_Id == _category.HRMLY_Id && t.HRELS_EncashedLeaves == _category.HRELS_EncashedLeaves && t.HRELS_CBLeaves == _category.HRELS_CBLeaves).Count();
                    if (resultCount > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                        return _category;
                    }
                    else
                    {
                        if (resultCount == 0)
                        {
                            var result = _lmContext.HR_Emp_Leave_StatusDMO.Single(t => t.HRELS_Id == _category.HRELS_Id);
                            result.HRELS_CBLeaves = _category.HRELS_CBLeaves;
                            result.MI_Id = _category.MI_Id;
                            result.UpdatedDate = DateTime.Now;
                            result.HRELS_UpdatedBy = _category.UserId;
                            _lmContext.Update(result);
                        }
                        var contactExists = _lmContext.SaveChanges();
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
                else
                {
                    //foreach (var itm1 in _category.temp_table_data)
                    //{
                    for (int i = 0; i < _category.emplist.Length; i++)
                    {
                        HR_Emp_Leave_StatusDMO objpge1 = Mapper.Map<HR_Emp_Leave_StatusDMO>(_category);
                        var result = _lmContext.HR_Emp_Leave_StatusDMO.Where(t => t.HRELS_CreditedLeaves == _category.emplist[i].HRELS_CreditedLeaves && t.MI_Id == objpge1.MI_Id && t.HRML_Id == objpge1.HRML_Id && t.HRME_Id == _category.emplist[i].HRME_Id && t.HRML_Id == objpge1.HRML_Id && t.HRMLY_Id == objpge1.HRMLY_Id).Count();
                        if (result > 0)
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }
                        else if (result == 0)
                        {
                            objpge1.MI_Id = _category.MI_Id;
                            objpge1.HRME_Id = _category.emplist[i].HRME_Id;
                            objpge1.HRML_Id = _category.HRML_Id;
                            objpge1.HRMLY_Id = _category.HRMLY_Id;
                            objpge1.HRELS_OBLeaves = 0;
                            objpge1.HRELS_CreditedLeaves = _category.emplist[i].HRELS_CreditedLeaves;
                            objpge1.HRELS_TotalLeaves = objpge1.HRELS_CreditedLeaves;
                            objpge1.HRELS_TransLeaves = 0;
                            objpge1.HRELS_EncashedLeaves = 0;
                            objpge1.HRELS_CBLeaves = _category.emplist[i].HRELS_CreditedLeaves;

                            objpge1.CreatedDate = DateTime.Now;
                            objpge1.UpdatedDate = DateTime.Now;
                            objpge1.HRELS_UpdatedBy = _category.UserId;
                            objpge1.HRELS_CreatedBy = _category.UserId;
                            _lmContext.Add(objpge1);
                            // _lmContext.Add(objpge);
                        }

                    }
                    var contactExists = _lmContext.SaveChanges();
                    if (contactExists >= 1)
                    {
                        _category.returnval = true;
                    }
                    else
                    {
                        _category.returnval = false;
                    }

                }
                List<HR_Emp_Leave_StatusDMO> m_events = new List<HR_Emp_Leave_StatusDMO>();
                m_events = _lmContext.HR_Emp_Leave_StatusDMO.Where(e => e.MI_Id == _category.MI_Id).ToList();
                _category.master_eventlist = m_events.ToArray();
                //  }
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public LeaveCreditDTO SaveDetails11(LeaveCreditDTO data)
        {
            try
            {
                for (int i = 0; i < data.emplist.Length; i++)
                {
                    var result = _lmContext.HR_Emp_Leave_StatusDMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == data.emplist[i].HRME_Id && t.HRMLY_Id == data.HRMLY_Id && t.HRML_Id == data.HRML_Id).ToList();
                    if(result.Count > 0)
                    {
                        var resultwo = _lmContext.HR_Emp_Leave_StatusDMO.Single(t => t.MI_Id == data.MI_Id && t.HRME_Id == data.emplist[i].HRME_Id && t.HRMLY_Id == data.HRMLY_Id && t.HRML_Id == data.HRML_Id);
                        resultwo.HRELS_CreditedLeaves = resultwo.HRELS_CreditedLeaves + data.emplist[i].HRELS_CreditedLeaves;
                        resultwo.HRELS_TotalLeaves = resultwo.HRELS_TotalLeaves + data.emplist[i].HRELS_CreditedLeaves;
                        resultwo.HRELS_CBLeaves = resultwo.HRELS_CBLeaves + data.emplist[i].HRELS_CreditedLeaves;
                        resultwo.UpdatedDate = DateTime.Now;
                        resultwo.HRELS_UpdatedBy = data.UserId;
                        _lmContext.Update(resultwo);
                    }

                   
                }
                var contactExists = _lmContext.SaveChanges();
                if (contactExists >= 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public LeaveCreditDTO leavecarryforward(LeaveCreditDTO dto)
        {
            try
            {
                for (int i = 0; i < dto.emplist.Length; i++)
                {
                    var contactExistsP = _lmContext.Database.ExecuteSqlCommand("HR_EMPLEAVE_CARRYFORWARD_DETAILS @p0,@p1", dto.MI_Id, dto.emplist[i].HRME_Id);
                if (contactExistsP > 0)
                {
                    dto.retrunMsg = "Add";
                }

                else
                {
                    dto.retrunMsg = "notUpdated";
                }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public LeaveCreditDTO deletepages(int id)
        {
            LeaveCreditDTO page = new LeaveCreditDTO();
            try
            {
                List<HR_Emp_Leave_StatusDMO> lorg = new List<HR_Emp_Leave_StatusDMO>();
                lorg = _lmContext.HR_Emp_Leave_StatusDMO.Where(t => t.HRELS_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    _lmContext.Remove(lorg.ElementAt(0));
                    var contactExists = _lmContext.SaveChanges();
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
    }
}

