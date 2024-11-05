using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;

using DomainModel.Model.com.vapstech.LeaveManagement;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;

using System.Collections.Generic;
using System.Linq;

namespace LeaveManagementServiceHub.com.vaps.Services
{
    public class LeaveOpeningBalanceImpl:Interfaces.LeaveOpeningBalanceInterface
    {
        LMContext _lmContext;
        DomainModelMsSqlServerContext _context;
        public LeaveOpeningBalanceImpl(LMContext ttcategory, DomainModelMsSqlServerContext context)
        {
            _lmContext = ttcategory;
            _context = context;
        }
        public LeaveCreditDTO getLeaveOB(LeaveCreditDTO data)
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

            data.result = (from c in _lmContext.HR_Emp_OB_Leave_DMO
                           from b in _lmContext.HR_Master_Leave_DMO
                           from a in _lmContext.HR_Master_Employee_DMO
                           where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && b.HRML_Id == c.HRML_Id && a.HRME_Id == c.HRME_Id)
                           select new LeaveCreditDTO
                           {
                               HREOBL_Id = c.HREOBL_Id,
                               HRME_Id = c.HRME_Id,
                               HRML_Id = c.HRML_Id,
                               HRML_LeaveName = b.HRML_LeaveName,
                               HREOBL_OBLeaves = c.HREOBL_OBLeaves,
                               HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim()

                           }).Distinct().ToArray();

            data.get_year = (from x in _lmContext.HR_Master_LeaveYear_DMO
                             where (x.MI_Id == data.MI_Id)
                             select new HR_Master_LeaveYear_DMO
                             {
                                 HRMLY_Id = x.HRMLY_Id,
                                 HRMLY_LeaveYearOrder=x.HRMLY_LeaveYearOrder
                             }).Distinct().OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();

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
                            && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRMGT_Id == d.HRMGT_Id && d.HRMGT_ActiveFlag == true && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id == d.MI_Id && selected_typ.Contains(d.HRMGT_Id) && selected_dep.Contains(c.HRMD_Id) && selected_des.Contains(b.HRMDES_Id))
                            select new LeaveCreditDTO
                            {
                                HRME_Id = a.HRME_Id,
                                HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim()
                            }).Distinct().ToArray();


            return data;
        }
        public LeaveCreditDTO save77(LeaveCreditDTO data)
        {
            HR_Emp_OB_Leave_DMO datadmo = Mapper.Map<HR_Emp_OB_Leave_DMO>(data);

            datadmo.MI_Id = data.MI_Id;
            datadmo.HRME_Id = data.HRME_Id;
            datadmo.HRML_Id = data.temp_table_data.FirstOrDefault().HRML_Id;
            datadmo.HREOBL_Date = DateTime.Now;
            datadmo.HREOBL_OBLeaves = data.temp_table_data.FirstOrDefault().HREOBL_OBLeaves;
            datadmo.HRMLY_Id = data.HRMLY_Id;
            datadmo.CreatedDate = DateTime.Now;
            datadmo.UpdatedDate = DateTime.Now;
            _context.Add(datadmo);
            var savecontent = _context.SaveChanges();
            if (savecontent > 0)
            {
                data.returnval = true;
                data.message = "Save";
            }
            else
            {
                data.returnval = false;
                data.message = "failed";
            }

            return data;
        }
        
        public LeaveCreditDTO save(LeaveCreditDTO data)
        {
            try
            {
                var yearid = (from x in _lmContext.HR_Master_LeaveYear_DMO
                              where (x.MI_Id == data.MI_Id && x.HRMLY_ActiveFlag == true)
                              select new LeaveCreditDTO
                              {
                                  HRMLY_Id = x.HRMLY_Id
                              }).OrderByDescending(d => d.HRMLY_Id).Distinct().ToList();

                //    HR_Emp_OB_Leave_DMO datadmo = Mapper.Map<HR_Emp_OB_Leave_DMO>(data);

                //    datadmo.MI_Id = data.MI_Id;
                //    datadmo.HRME_Id = data.HRME_Id;
                //    datadmo.HRML_Id = data.temp_table_data.FirstOrDefault().HRML_Id;
                //    datadmo.HREOBL_Date = DateTime.Now;
                //    datadmo.HREOBL_OBLeaves = data.temp_table_data.FirstOrDefault().HREOBL_OBLeaves;
                //    datadmo.HRMLY_Id = data.HRMLY_Id;
                //    datadmo.CreatedDate = DateTime.Now;
                //    datadmo.UpdatedDate = DateTime.Now;
                //    _context.Add(datadmo);
                //    var savecontent = _context.SaveChanges();
                //    if (savecontent > 0)
                //    {
                //        data.returnval = true;
                //        data.message = "Save";
                //    }
                //    else
                //    {
                //        data.returnval = false;
                //        data.message = "failed";
                //    }

                foreach (LeaveCreditDTO item in data.temp_table_data)
                {
                    var result = _lmContext.HR_Emp_OB_Leave_DMO.Where(t => t.MI_Id == item.MI_Id && t.HRMLY_Id == yearid.FirstOrDefault().HRMLY_Id && t.HRME_Id == data.HRME_Id && t.HRML_Id == item.HRML_Id).ToList();
                    if (result.Count > 0)
                    {
                        var resultData = _lmContext.HR_Emp_OB_Leave_DMO.Single(t => t.HREOBL_Id == result.FirstOrDefault().HREOBL_Id);
                        resultData.HREOBL_Date = DateTime.Now;
                        resultData.HREOBL_OBLeaves = item.HREOBL_OBLeaves;
                        resultData.UpdatedDate = DateTime.Now;
                        _lmContext.Update(resultData);
                        var savecontent = _lmContext.SaveChanges();
                        if (savecontent > 0)
                        {
                            data.returnval = true;
                            data.message = "Save";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Save";
                        }
                    }
                    else
                    {
                        HR_Emp_OB_Leave_DMO datadmo = new HR_Emp_OB_Leave_DMO();
                        datadmo.HRME_Id = data.HRME_Id;
                        datadmo.MI_Id = item.MI_Id;
                        datadmo.HRML_Id = item.HRML_Id;
                        datadmo.HREOBL_Date = DateTime.Now;
                        datadmo.HREOBL_OBLeaves = item.HREOBL_OBLeaves;
                        datadmo.HRMLY_Id = yearid.FirstOrDefault().HRMLY_Id;
                        datadmo.CreatedDate = DateTime.Now;
                        datadmo.UpdatedDate = DateTime.Now;
                        _lmContext.Add(datadmo);
                        var savecontent = _lmContext.SaveChanges();
                        if (savecontent > 0)
                        {
                            data.returnval = true;
                            data.message = "Save";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "failed";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.message = "Error";
            }
            return data;
        }

        public LeaveCreditDTO deletepages(int id)
        {
            LeaveCreditDTO page = new LeaveCreditDTO();
            try
            {
                List<HR_Emp_OB_Leave_DMO> lorg = new List<HR_Emp_OB_Leave_DMO>();
                lorg = _context.HR_Emp_OB_Leave_DMO.Where(t => t.HREOBL_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    _context.Remove(lorg.ElementAt(0));
                    var contactExists = _context.SaveChanges();
                    if (contactExists == 1)
                    {
                        page.returnval = true;
                    }
                    else
                    {
                        page.returnval = false;
                    }
                }

                //List<HR_Emp_OB_Leave_DMO> allpages = new List<HR_Emp_OB_Leave_DMO>();
                //allpages = _context.HR_Emp_OB_Leave_DMO.ToList();
                //page.edit_lvblnce = allpages.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public LeaveCreditDTO getpagedetails(int id)
        {
            LeaveCreditDTO page = new LeaveCreditDTO();
            try
            {
                List<HR_Emp_OB_Leave_DMO> events_m = new List<HR_Emp_OB_Leave_DMO>();
                events_m = _context.HR_Emp_OB_Leave_DMO.Where(e => e.HREOBL_Id == id).ToList();
                page.edit_lvblnce = events_m.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
    }
}
