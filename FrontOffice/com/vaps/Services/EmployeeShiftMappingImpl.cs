using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.TT;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Services
{
    public class EmployeeShiftMappingImpl:Interfaces.EmployeeShiftMappingInterface
    {
        //private static ConcurrentDictionary<string, TTMasterCategoryDTO> _login =
        // new ConcurrentDictionary<string, TTMasterCategoryDTO>();

        public FOContext _FOContext;

        public EmployeeShiftMappingImpl(FOContext ttcntx)

        {
            _FOContext = ttcntx;
        }


        public EmployeeShiftMappingDTO savedetail(EmployeeShiftMappingDTO _category)
        {
            var Duplicate_Count = 0;
            //   bool returnresult = false;
            try
            {
                if (_category.FOEST_Id > 0)
                {
                    for (int i = 0; i < _category.employee.Count(); i++)
                    {
                        var temp_usr = _category.employee[i].HRME_Id;
                        for(int j = 0; j < _category.SelectedDayType.Length; j++)
                        {
                            var dup = _FOContext.EmployeeShiftMapping.Where(t => t.FOHWDT_Id == _category.SelectedDayType[j].FOHWDT_Id && t.HRME_Id == temp_usr && t.MI_Id == _category.MI_Id && t.FOEST_Id!= _category.FOEST_Id && t.FOEST_Date == _category.FOEST_Date).Count();
                            if (dup > 0)
                            {
                                Duplicate_Count += 1;
                               _category.returnduplicatestatus = "Duplicate";
                            }
                        }
                       
                    }
                    if (Duplicate_Count == 0)
                    {
                        for (int i = 0; i < _category.employee.Count(); i++)
                        {

                            var result = _FOContext.EmployeeShiftMapping.Single(d => d.FOEST_Id == _category.FOEST_Id);
                            result.FOEST_BlockAttendance = _category.FOEST_BlockAttendance;
                            result.FOEST_Date = _category.FOEST_Date;
                            result.FOEST_DelayPerShiftHrMin = _category.FOEST_DelayPerShiftHrMin;
                            result.FOEST_EarlyPerShiftHrMin = _category.FOEST_EarlyPerShiftHrMin;
                            result.FOEST_FDWHrMin = _category.FOEST_FDWHrMin;
                            result.FOEST_FixTimings = _category.FOEST_FixTimings;
                            result.FOEST_HDWHrMin = _category.FOEST_HDWHrMin;
                            result.FOEST_IHalfLoginTime = _category.FOEST_IHalfLoginTime;
                            result.FOEST_IHalfLogoutTime = _category.FOEST_IHalfLogoutTime;
                            result.FOEST_IIHalfLoginTime = _category.FOEST_IIHalfLoginTime;
                            result.FOEST_IIHalfLogoutTime = _category.FOEST_IIHalfLogoutTime;
                            result.FOEST_LunchHoursDuration = _category.FOEST_LunchHoursDuration;
                            result.FOHWDT_Id = _category.SelectedDayType[0].FOHWDT_Id;
                            result.FOMS_Id = _category.FOMS_Id;
                            result.UpdatedDate = DateTime.Now;
                            result.FOEST_UpdatedBy = _category.Userid;
                            _FOContext.Update(result);
                            var contactExists = _FOContext.SaveChanges();
                            if (contactExists == 1)
                            {
                                _category.returnval = true;
                                _category.returnupdatestatus = "updated";

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
                    for (int i = 0; i < _category.employee.Count(); i++)
                    {
                        var temp_usr = _category.employee[i].HRME_Id;
                        for(int j = 0; j < _category.SelectedDayType.Length; j++)
                        {
                            var dup = _FOContext.EmployeeShiftMapping.Where(t => t.FOHWDT_Id == _category.SelectedDayType[j].FOHWDT_Id && t.FOMS_Id== _category.FOMS_Id && t.HRME_Id == temp_usr && t.MI_Id == _category.MI_Id && t.FOEST_Date== _category.FOEST_Date).Count();
                            if (dup > 0)
                            {
                                Duplicate_Count += 1;
                               _category.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                EmployeeShiftMappingDMO objpge = Mapper.Map<EmployeeShiftMappingDMO>(_category);

                                objpge.HRME_Id = temp_usr;
                                objpge.FOHWDT_Id = _category.SelectedDayType[j].FOHWDT_Id;
                                objpge.CreatedDate = DateTime.Now;
                                objpge.UpdatedDate = DateTime.Now;
                                objpge.FOEST_CreatedBy = _category.Userid;
                                objpge.FOEST_UpdatedBy = _category.Userid;

                                _FOContext.Add(objpge);
                                var contactExists = _FOContext.SaveChanges();
                                if (contactExists == 1)
                                {
                                    _category.returnval = true;
                                    _category.returnsavestatus = "saved";
                                }
                                else
                                {
                                    _category.returnval = false;
                                }
                            }
                        }
                       
                    }
                }
                //if (Duplicate_Count == 0)
                //{
                //    for (int i = 0; i < _category.employee.Count(); i++)
                //    {
                //        var temp_usr = _category.employee[i].HRME_Id;

                //        EmployeeShiftMappingDMO objpge = Mapper.Map<EmployeeShiftMappingDMO>(_category);

                //        objpge.HRME_Id = temp_usr;
                //        objpge.CreatedDate = DateTime.Now;
                //        objpge.UpdatedDate = DateTime.Now;

                //        _FOContext.Add(objpge);
                //        var contactExists = _FOContext.SaveChanges();
                //        if (contactExists == 1)
                //        {
                //            _category.returnval = true;
                //        }
                //        else
                //        {
                //            _category.returnval = false;
                //        }
                //    }
                //}

                //else
                //{
                //    _category.returnduplicatestatus = "Duplicate";
                //}
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public EmployeeShiftMappingDTO getdetails(int id)
        {
            EmployeeShiftMappingDTO TTMC = new EmployeeShiftMappingDTO();
            try
            {

                List<HR_Master_GroupType> staf_types = new List<HR_Master_GroupType>();
                staf_types = _FOContext.HR_Master_GroupType_DMO.Where(t => t.MI_Id == id && t.HRMGT_ActiveFlag == true).ToList(); 
                TTMC.stf_types = staf_types.Distinct().ToArray();

                List<HR_Master_Department> Department_types = new List<HR_Master_Department>();
                foreach (HR_Master_GroupType dto in TTMC.stf_types)
                {
                    var deptlist = (from a in _FOContext.HR_Master_Employee_DMO
                                    from b in _FOContext.HR_Master_Department_DMO
                                    from c in _FOContext.HR_Master_GroupType_DMO
                                    where (a.HRMD_Id == b.HRMD_Id && a.HRMGT_Id == c.HRMGT_Id && c.HRMGT_ActiveFlag == true
                                         && b.HRMD_ActiveFlag == true && a.HRME_ActiveFlag == true && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id
                                         && dto.HRMGT_Id == c.HRMGT_Id && a.MI_Id == id)
                                    select new HR_Master_Department
                                    {
                                        HRMD_Id = b.HRMD_Id,
                                        HRMD_DepartmentName = b.HRMD_DepartmentName,
                                    }
                                        ).Distinct().ToList();

                    for (int i = 0; i < deptlist.Count; i++)
                    {
                        Department_types.Add(deptlist[i]);
                    }
                }
                TTMC.Department_types = Department_types.Distinct().ToArray();
                //  List<HR_Master_Department> Department_types = new List<HR_Master_Department>();
                //  Department_types = _FOContext.HR_Master_Department_DMO.Where(t => t.MI_Id == id && t.HRMD_ActiveFlag == true).ToList(); //

                List<HR_Master_Designation> Designation_types = new List<HR_Master_Designation>();
                foreach (HR_Master_Department dept in TTMC.Department_types)
                {
                    var Designation = (from a in _FOContext.HR_Master_Employee_DMO
                                       from b in _FOContext.HR_Master_Designation_DMO
                                       from c in _FOContext.HR_Master_Department_DMO
                                       where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
                                       && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true
                                        && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && dept.HRMD_Id == c.HRMD_Id && a.MI_Id == id)
                                       select new HR_Master_Designation
                                       {
                                           HRMDES_Id = b.HRMDES_Id,
                                           HRMDES_DesignationName = b.HRMDES_DesignationName,
                                       }
                                       ).Distinct().ToList();
                    for (int i = 0; i < Designation.Count; i++)
                    {
                        Designation_types.Add(Designation[i]);
                    }
                }



                //List<HR_Master_Designation> Designation_types = new List<HR_Master_Designation>();
                //Designation_types = _FOContext.HR_Master_Designation_DMO.Where(t => t.MI_Id == id && t.HRMDES_DisplaySanctionedSeatsFlag == true).ToList(); //
                TTMC.Designation_types = Designation_types.Distinct().ToArray();

                List<HolidayWorkingDayTypeDMO> holiday_types = new List<HolidayWorkingDayTypeDMO>();
                holiday_types = _FOContext.holidayWorkingDayType.Where(t => t.MI_Id == id && t.FOHWDT_ActiveFlg == true).ToList();
                TTMC.holiday_types = holiday_types.ToArray();

                List<MasterShiftsDMO> sfname = new List<MasterShiftsDMO>();
                sfname = _FOContext.masterShifts.Where(t => t.MI_Id == id && t.FOMS_ActiveFlg == true).ToList();
                TTMC.sfname = sfname.ToArray();

                TTMC.emplist = (from a in _FOContext.EmployeeShiftMapping
                                from b in _FOContext.HR_Master_Employee_DMO
                                from c in _FOContext.masterShifts
                                from e in _FOContext.holidayWorkingDayType
                                where (a.HRME_Id == b.HRME_Id && a.FOMS_Id == c.FOMS_Id && a.FOHWDT_Id == e.FOHWDT_Id && a.MI_Id == id)
                                //  group new { a,b,c,e}
                                //  by new {a.FOEST_Id } into g
                                select new EmployeeShiftMappingDTO
                                {
                                    FOEST_Id = a.FOEST_Id,
                                    HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName)
                                     + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                    FOHTWD_HolidayWDType = e.FOHTWD_HolidayWDType,
                                    FOEST_Date = a.FOEST_Date,
                                    FOMS_ShiftName = c.FOMS_ShiftName,
                                    FOEST_FDWHrMin = a.FOEST_FDWHrMin,
                                    FOEST_HDWHrMin = a.FOEST_HDWHrMin,
                                    FOEST_IHalfLoginTime = a.FOEST_IHalfLoginTime,
                                    FOEST_IHalfLogoutTime = a.FOEST_IHalfLogoutTime,
                                    FOEST_IIHalfLoginTime = a.FOEST_IIHalfLoginTime,
                                    FOEST_IIHalfLogoutTime = a.FOEST_IIHalfLogoutTime,
                                    FOEST_DelayPerShiftHrMin = a.FOEST_DelayPerShiftHrMin,
                                    FOEST_EarlyPerShiftHrMin = a.FOEST_EarlyPerShiftHrMin,
                                    FOEST_LunchHoursDuration = a.FOEST_LunchHoursDuration
                                }).ToArray();

                //  List<HR_Master_Employee_DMO> emp = new List<HR_Master_Employee_DMO>();
                //  emp = _FOContext.HR_Master_Employee_DMO.Where(t => t.MI_Id == id).ToList();
                var emp = (from m in _FOContext.HR_Master_Employee_DMO
                           where m.MI_Id == id
                           select new HR_Master_Employee_DMO
                           {
                               HRME_Id = m.HRME_Id,
                               HRME_EmployeeFirstName = m.HRME_EmployeeFirstName
                           }).ToList();
                TTMC.employeelist = emp.ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }

        public EmployeeShiftMappingDTO Shiftname(EmployeeShiftMappingDTO data)
        {
            EmployeeShiftMappingDTO page = new EmployeeShiftMappingDTO();
            try
            {
                page.sflist = (from a in _FOContext.masterShifts
                               from b in _FOContext.masterShiftsTimings
                               where (a.FOMS_Id == data.FOMS_Id && a.FOMS_Id == b.FOMS_Id && a.MI_Id == data.MI_Id && a.FOMS_ActiveFlg == true)
                               select new EmployeeShiftMappingDTO
                               {
                                   FOMS_Id = a.FOMS_Id,
                                   FOMS_ShiftName = a.FOMS_ShiftName,
                                   FOMS_ActiveFlg = a.FOMS_ActiveFlg,
                                   FOMST_Id = b.FOMST_Id,
                                   FOHWDT_Id = b.FOHWDT_Id,
                                   FOMST_FDWHrMin = b.FOMST_FDWHrMin,
                                   FOMST_HDWHrMin = b.FOMST_HDWHrMin,
                                   FOMST_IHalfLoginTime = b.FOMST_IHalfLoginTime,
                                   FOMST_IHalfLogoutTime = b.FOMST_IHalfLogoutTime,
                                   FOMST_IIHalfLoginTime = b.FOMST_IIHalfLoginTime,
                                   FOMST_IIHalfLogoutTime = b.FOMST_IIHalfLogoutTime,
                                   FOMST_DelayPerShiftHrMin = b.FOMST_DelayPerShiftHrMin,
                                   FOMST_EarlyPerShiftHrMin = b.FOMST_EarlyPerShiftHrMin,
                                   FOMST_LunchHoursDuration = b.FOMST_LunchHoursDuration,
                                   // FOMST_BlockAttendance = b.FOMST_BlockAttendance,
                                   FOMST_FixTimings = b.FOMST_FixTimings

                               }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public EmployeeShiftMappingDTO editdetails(int id)
        {
            EmployeeShiftMappingDTO edit = new EmployeeShiftMappingDTO();
            try
            {
                List<EmployeeShiftMappingDMO> edit1 = new List<EmployeeShiftMappingDMO>();
                edit1 = _FOContext.EmployeeShiftMapping.Where(t => t.FOEST_Id == id).ToList();
                edit.editlist = edit1.ToArray();

               
                var  emp = _FOContext.HR_Master_Employee_DMO.Where(t => t.HRME_Id == edit1[0].HRME_Id).Select(d=> new EmployeeShiftMappingDTO {HRMGT_Id=d.HRMGT_Id,HRMD_Id=d.HRMD_Id,HRMDES_Id=d.HRMDES_Id }).ToList();
                edit.emplist1 = emp.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return edit;
        }

        public EmployeeShiftMappingDTO deleterec(EmployeeShiftMappingDTO data)
        {
           
            try
            {
                List<EmployeeShiftMappingDMO> lorg = new List<EmployeeShiftMappingDMO>();
                lorg = _FOContext.EmployeeShiftMapping.Where(t => t.FOEST_Id==data.FOEST_Id).ToList();
                if (lorg.Any())
                {
                    _FOContext.Remove(lorg.ElementAt(0));
                    var contactExists = _FOContext.SaveChanges();
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public EmployeeShiftMappingDTO get_departments(EmployeeShiftMappingDTO data)
        {
            List<long> selected_emp_types = new List<long>();

            foreach (var itm in data.emptypes)
            {
                selected_emp_types.Add(itm.HRMGT_Id);
            }

            data.Department_types = (from a in _FOContext.HRGroupDeptDessgDMO
                                     from b in _FOContext.HR_Master_Department_DMO                                 
                                     where (a.HRMD_Id == b.HRMD_Id 
                                          && b.HRMD_ActiveFlag == true && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id
                                          && selected_emp_types.Contains(a.HRMGT_Id))
                                     select new EmployeeShiftMappingDTO
                                     {
                                         HRMD_Id = b.HRMD_Id,
                                         HRMD_DepartmentName = b.HRMD_DepartmentName,
                                     }
                     ).Distinct().ToArray();

            return data;
        }


        public EmployeeShiftMappingDTO get_designation(EmployeeShiftMappingDTO data)
        {
            List<long> selected_desg_types = new List<long>();

            foreach (var itm in data.emptypes)
            {
                selected_desg_types.Add(itm.HRMD_Id);
            }

            data.Designation_types = (from a in _FOContext.HRGroupDeptDessgDMO
                                      from b in _FOContext.HR_Master_Designation_DMO                                   
                                      where (a.HRMDES_Id == b.HRMDES_Id 
                                      && b.HRMDES_ActiveFlag == true 
                                       && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && selected_desg_types.Contains(a.HRMD_Id))
                                      select new EmployeeShiftMappingDTO
                                      {
                                          HRMDES_Id = b.HRMDES_Id,
                                          HRMDES_DesignationName = b.HRMDES_DesignationName,
                                      }
                     ).Distinct().ToArray();

            return data;
        }

        public EmployeeShiftMappingDTO get_employee(EmployeeShiftMappingDTO data)
        {
            try
            {
                List<long> selected_typ = new List<long>();

                foreach (var itm in data.emptypes)
                {
                    selected_typ.Add(itm.HRMGT_Id);
                }
                List<long?> selected_dep = new List<long?>();

                foreach (var itm in data.empdept)
                {
                    selected_dep.Add(itm.HRMD_Id);
                }
                List<long> selected_des = new List<long>();

                foreach (var itm in data.empdesg)
                {
                    selected_des.Add(itm.HRMDES_Id);
                }

                data.get_emp = (from a in _FOContext.HR_Master_Employee_DMO
                                from b in _FOContext.HRGroupDeptDessgDMO                               
                                where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id==b.HRMD_Id && a.HRMGT_Id==b.HRMGT_Id && a.HRME_LeftFlag == false 
                                 && a.HRME_ActiveFlag == true 
                                 && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id  && selected_typ.Contains(b.HRMGT_Id) && selected_dep.Contains(b.HRMD_Id) && selected_des.Contains(b.HRMDES_Id))
                                select new EmployeeShiftMappingDTO
                                {
                                    HRME_Id = a.HRME_Id,
                                    HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName)
                                         + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim()
                                }
                         ).Distinct().OrderBy(t => t.HRME_EmployeeFirstName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return data;
        }
    }
}
