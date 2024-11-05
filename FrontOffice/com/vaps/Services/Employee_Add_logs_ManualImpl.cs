using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Birthday;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs.com.vaps.BirthDay;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using CommonLibrary;
using SendGrid.Helpers.Mail;
using SendGrid;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.HRMS;

namespace FrontOffice.com.vaps.Services
{
    public class Employee_Add_logs_ManualImpl : Interfaces.Employee_Add_logs_ManualInterface
    {
        int MI_ID = 0;
        private static ConcurrentDictionary<string, FO_Emp_PunchDTO> _login = new ConcurrentDictionary<string, FO_Emp_PunchDTO>();
        public DomainModelMsSqlServerContext _db;
        public FOContext _FOContext;


        public Employee_Add_logs_ManualImpl(FOContext FOContext, DomainModelMsSqlServerContext db)
        {
            _FOContext = FOContext;
            _db = db;
        }
        public FO_Emp_PunchDTO getdata(FO_Emp_PunchDTO data)
        {
            FO_Emp_PunchDTO TTMC = new FO_Emp_PunchDTO();
            try
            {

                List<HR_Master_GroupType> staf_types = new List<HR_Master_GroupType>();
                staf_types = _FOContext.HR_Master_GroupType_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMGT_ActiveFlag == true).ToList(); //
                TTMC.stf_types = staf_types.Distinct().ToArray();

                List<HR_Master_Department> Department_types = new List<HR_Master_Department>();
                foreach (HR_Master_GroupType dto in TTMC.stf_types)
                {
                    var deptlist = (from a in _FOContext.HR_Master_Employee_DMO
                                    from b in _FOContext.HR_Master_Department_DMO
                                    from c in _FOContext.HR_Master_GroupType_DMO
                                    where (a.HRMD_Id == b.HRMD_Id && a.HRMGT_Id == c.HRMGT_Id && c.HRMGT_ActiveFlag == true
                                         && b.HRMD_ActiveFlag == true && a.HRME_ActiveFlag == true && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id
                                         && dto.HRMGT_Id == c.HRMGT_Id && a.MI_Id == data.MI_Id)
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
                                        && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && dept.HRMD_Id == c.HRMD_Id && a.MI_Id == data.MI_Id)
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
                holiday_types = _FOContext.holidayWorkingDayType.Where(t => t.MI_Id == data.MI_Id && t.FOHWDT_ActiveFlg == true).ToList();
                TTMC.holiday_types = holiday_types.ToArray();

                List<MasterShiftsDMO> sfname = new List<MasterShiftsDMO>();
                sfname = _FOContext.masterShifts.Where(t => t.MI_Id == data.MI_Id && t.FOMS_ActiveFlg == true).ToList();
                TTMC.sfname = sfname.ToArray();

                TTMC.emplist = (from a in _FOContext.EmployeeShiftMapping
                                from b in _FOContext.HR_Master_Employee_DMO
                                from c in _FOContext.masterShifts
                                from e in _FOContext.holidayWorkingDayType
                                where (a.HRME_Id == b.HRME_Id && a.FOMS_Id == c.FOMS_Id && a.FOHWDT_Id == e.FOHWDT_Id && a.MI_Id == data.MI_Id)
                                //  group new { a,b,c,e}
                                //  by new {a.FOEST_Id } into g
                                select new EmployeeShiftMappingDTO
                                {
                                    FOEST_Id = a.FOEST_Id,
                                    HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName)
                                     + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                    FOHTWD_HolidayWDType = e.FOHTWD_HolidayWDType,
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
                           where m.MI_Id == data.MI_Id
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

        public FO_Emp_PunchDTO empname(FO_Emp_PunchDTO data)
        {
            FO_Emp_PunchDTO TTMC = new FO_Emp_PunchDTO();
            try
            {
                var que1 = _FOContext.FO_Emp_Punch.Where(a => a.MI_Id == data.MI_Id && a.FOEP_PunchDate.Value.Date == data.empdate.Date && a.HRME_Id == data.HRME_Id).Select(t => t.FOEP_Id).FirstOrDefault();

                if (que1 > 0)
                {
                    var que2 = _FOContext.FO_Emp_Punch_Details.Where(b => b.MI_Id == data.MI_Id && b.FOEP_Id == que1 && b.FOEPD_Flag=="1").ToList();
                    TTMC.employeelist = que2.ToArray();
                    TTMC.returnval = true;
                }
                else
                {
                    TTMC.returnval = false;
                }
            }
            catch (Exception ex)
            {
                TTMC.returnval = false;
            }
            return TTMC;
        }


        public FO_Emp_PunchDTO savedetail(FO_Emp_PunchDTO data)
        {

            try
            {
                if (data.FOEP_Id > 0)
                {
                    FO_Emp_Punch_DetailsDMO fopunch = new FO_Emp_Punch_DetailsDMO();
                    fopunch.MI_Id = data.MI_Id;
                    fopunch.FOEP_Id = data.FOEP_Id;
                    fopunch.FOEPD_PunchTime = data.FOEPD_PunchTime;
                    fopunch.FOEPD_InOutFlg = data.InOutFlg;
                    fopunch.FOEPD_Flag = "1";
                    fopunch.CreatedDate = DateTime.Now;
                    fopunch.UpdatedDate = DateTime.Now;
                    _FOContext.Add(fopunch);

                    var contactExists = _FOContext.SaveChanges();
                    if (contactExists > 0)
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
                    var query2 = _FOContext.FO_Emp_Punch.Where(d => d.HRME_Id == data.HRME_Id && d.FOEP_PunchDate.Value.Date == data.FOEP_PunchDate.Value.Date && d.MI_Id == data.MI_Id).ToList(); //query.FirstOrDefault().HRME_Id
                    if (query2.Count == 0)
                    {
                        FO_Emp_PunchDMO dmo = new FO_Emp_PunchDMO();
                        dmo.CreatedDate = DateTime.Now;
                        dmo.FOEP_Flag = true;
                        var query3 = (from m in _FOContext.holidaydate
                                      from n in _FOContext.holidayWorkingDayType
                                      where m.FOHWDT_Id == n.FOHWDT_Id && n.FOHTWD_HolidayFlag == true && data.FOEP_PunchDate.Value.Date >= m.FOMHWDD_FromDate.Value.Date && data.FOEP_PunchDate.Value.Date <= m.FOMHWDD_ToDate.Value.Date && n.MI_Id == data.MI_Id
                                      select m).ToList();
                        if (query3.Count > 0)
                        {
                            dmo.FOEP_HolidayPunchFlg = true;
                        }
                        else
                        {
                            dmo.FOEP_HolidayPunchFlg = false;
                        }

                        dmo.FOEP_PunchDate = data.FOEP_PunchDate;
                        dmo.HRME_Id = data.HRME_Id;
                        dmo.MI_Id = data.MI_Id;
                        dmo.UpdatedDate = DateTime.Now;
                        _FOContext.Add(dmo);

                        FO_Emp_Punch_DetailsDMO dmo2 = new FO_Emp_Punch_DetailsDMO();
                        dmo2.CreatedDate = DateTime.Now;
                        dmo2.MI_Id = data.MI_Id;
                        dmo2.FOEP_Id = dmo.FOEP_Id;
                        dmo2.FOEPD_Flag = "1";
                        dmo2.FOEPD_InOutFlg = "I";
                        dmo2.FOEPD_PunchTime = Convert.ToDateTime(data.FOEPD_PunchTime).ToString("HH:mm");
                        dmo2.UpdatedDate = DateTime.Now;
                        _FOContext.Add(dmo2);

                        var flag = _FOContext.SaveChanges();
                        if (flag > 0)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FO_Emp_PunchDTO deleterec(int id)
        {
            FO_Emp_PunchDTO delete = new FO_Emp_PunchDTO();
            try
            {

                var updaterecord = _FOContext.FO_Emp_Punch_Details.Single(t => t.FOEPD_Id.Equals(id));

                if (updaterecord.FOEPD_Flag == "1")
                {
                    updaterecord.FOEPD_Flag = "0";
                }
                else
                {
                    updaterecord.FOEPD_Flag = "1";
                }

                var contactExists = _FOContext.SaveChanges();
                if (contactExists == 1)
                {
                    delete.returnval = true;
                }
                else
                {
                    delete.returnval = false;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return delete;
        }





    }
}
