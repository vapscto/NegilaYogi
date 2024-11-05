using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System.Dynamic;
using CommonLibrary;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.Data;

namespace HRMSServicesHub.com.vaps.Services
{
    public class EmployeeSalarySlipServiceModified: Interfaces.EmployeeSalarySlipInterfaceModified
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public EmployeeSalarySlipServiceModified(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }
        public HR_Employee_SalaryModifiedDTO getBasicData(HR_Employee_SalaryModifiedDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }

        public HR_Employee_SalaryModifiedDTO GetAllDropdownAndDatatableDetails(HR_Employee_SalaryModifiedDTO dto)
        {
            List<HR_Employee_Salary> SalaryCalculation = new List<HR_Employee_Salary>();
            List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_Master_LeaveYearDMO> leaveyear = new List<HR_Master_LeaveYearDMO>();

            List<Month> Monthlist = new List<Month>();

            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            List<HR_Master_CourseDMO> Courselist = new List<HR_Master_CourseDMO>();
            List<HR_Master_Grade> Gradelist = new List<HR_Master_Grade>();
            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();

            try
            {

                //leave year
                Monthlist = _Context.month.Where(t => t.Is_Active == true).ToList();
                dto.monthdropdown = Monthlist.ToArray();
                //employee

                employe = (from a in _HRMSContext.MasterEmployee
                           from b in _HRMSContext.HR_Employee_Salary
                           where (a.HRME_Id == b.HRME_Id && a.MI_Id.Equals(dto.MI_Id) && a.HRME_ActiveFlag == true)
                           select a).ToList();


                //  employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).ToList();
                dto.employeedropdown = employe.ToArray();

                //leave year
                leaveyear = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_ActiveFlag == true).OrderBy(t=>t.HRMLY_LeaveYearOrder).ToList();
                dto.leaveyeardropdown = leaveyear.ToArray();

                PROCESSList = (from ao in _HRMSContext.HR_Process_Auth_OrderNoDMO
                               from pa in _HRMSContext.HR_PROCESSDMO
                               from cc in _HRMSContext.Staff_User_Login
                               where (pa.HRPA_Id == ao.HRPA_Id && ao.IVRMUL_Id == cc.IVRMSTAUL_Id && cc.Id == dto.LogInUserId)


                               select pa
                      ).ToList();

                if (PROCESSList.Count() > 0)
                {

                    List<long> groupTypeIdList = PROCESSList.Select(t => t.HRMGT_Id).Distinct().ToList();
                    List<long> hrmD_IdList = PROCESSList.Select(t => t.HRMD_Id).Distinct().ToList();
                    List<long> hrmdeS_IdList = PROCESSList.Select(t => t.HRMDES_Id).Distinct().ToList();


                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id)) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdown = GroupTypelist.ToArray();

                    //Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdown = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdown = Designationlist.ToArray();

                }
                else
                {


                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdown = GroupTypelist.ToArray();

                    //Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdown = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdown = Designationlist.ToArray();


                }


                //dto.groupTypedropdown = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToArray();
                ////employee  
                ////employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) ).ToList();
                ////dto.employeedropdown = employe.ToArray();

                ////departmentdropdown
                //dto.departmentdropdown = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToArray();

                ////designationdropdown 
                //dto.designationdropdown = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public HR_Employee_SalaryModifiedDTO GetEmployeeDetailsByLeaveYearAndMultiMonth(HR_Employee_SalaryModifiedDTO dto)
        {
            for (int intmonth = 0; intmonth < dto.MonthList.Length; intmonth++)
            {
                dto.HRES_Month = dto.MonthList[intmonth].Trim().ToString();
                List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
                List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
                List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
                List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
                List<MasterEmployee> employe = new List<MasterEmployee>();
                List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
                try
                {
                    //employee

                    //employee list
                    PROCESSList = (from ao in _HRMSContext.HR_Process_Auth_OrderNoDMO
                                   from pa in _HRMSContext.HR_PROCESSDMO
                                   from cc in _HRMSContext.Staff_User_Login
                                   where (pa.HRPA_Id == ao.HRPA_Id && ao.IVRMUL_Id == cc.IVRMSTAUL_Id && cc.Id == dto.LogInUserId)
                                   select pa
                     ).ToList();

                    if (PROCESSList.Count() > 0)
                    {

                        List<long> groupTypeIdList = PROCESSList.Select(t => t.HRMGT_Id).Distinct().ToList();
                        List<long> hrmD_IdList = PROCESSList.Select(t => t.HRMD_Id).Distinct().ToList();
                        List<long> hrmdeS_IdList = PROCESSList.Select(t => t.HRMDES_Id).Distinct().ToList();

                        employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                        dto.employeeDetails = employe.ToArray();
                        dto.employeedropdown = employe.ToArray();

                        //  dto.employeedropdown = employeeDetails.ToArray();

                        //GroupTypelist
                        GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id)) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                        dto.groupTypedropdownlist = GroupTypelist.ToArray();

                        ////Departmentlist
                        Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                        dto.departmentdropdownlist = Departmentlist.ToArray();

                        //Designationlist
                        Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                        dto.designationdropdownlist = Designationlist.ToArray();
                    }
                    else
                    {
                        //var abc = (from a in _HRMSContext.HR_Employee_Salary where (a.HRES_Month.Contains("January,February")) select a).ToList();

                        employe = (from a in _HRMSContext.MasterEmployee
                                   from b in _HRMSContext.HR_Employee_Salary
                                   where (b.HRME_Id == a.HRME_Id && b.MI_Id.Equals(dto.MI_Id))
                                   //&& b.HRES_Year.Equals(dto.HRES_Year) && b.HRES_Month.Contains(dto.HRES_Month) && a.HRME_ActiveFlag == true
                                   && b.HRES_Year.Equals(dto.HRES_Year) && b.HRES_Month.Equals(dto.HRES_Month) && a.HRME_ActiveFlag == true
                                   select a).Distinct().ToList();
                        if (employe.Count > 0)
                        {
                            // employe = employe.Where(a => a.HRME_LeftFlag == false && Convert.ToDateTime(a.HRME_DOJ) <= Convert.ToDateTime(selecteddate)).ToList();


                            if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                            {
                                //employee
                                employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();

                            }
                            else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                            {
                                //employee
                                employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                            }
                            else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                            {
                                //employee
                                employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                            }
                            else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                            {
                                //employee
                                employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                            }
                            else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() == 0)
                            {
                                //employee
                                employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id)).ToList();
                            }
                            else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                            {
                                //employee
                                employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                            }

                            else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                            {
                                //employee
                                employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                            }

                            if (Convert.ToInt32(dto.HRES_Year) > 0 && dto.HRES_Month != "")
                            {
                                //get month id by month name
                                //var Month = _Context.month.Where(t => t.Is_Active == true && t.IVRM_Month_Name.Equals(dto.HRES_Month)).ToList();
                                var Month = _Context.month.Where(t => t.Is_Active == true && t.IVRM_Month_Name.Contains(dto.HRES_Month)).ToList();
                                var config = _HRMSContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).FirstOrDefault();
                                int IVRM_Month_Id = 0;
                                if (Month.Count > 0)
                                {
                                    if (config.HRC_SalaryFromDay > 1 && Convert.ToInt32(Month.FirstOrDefault().IVRM_Month_Id) < 12)
                                    {

                                        IVRM_Month_Id = Convert.ToInt32(Month.FirstOrDefault().IVRM_Month_Id) + 1;
                                    }
                                    else if (config.HRC_SalaryFromDay > 1 && Convert.ToInt32(Month.FirstOrDefault().IVRM_Month_Id) == 12)
                                    {
                                        IVRM_Month_Id = 01;
                                        dto.HRES_Year = (Convert.ToInt32(dto.HRES_Year) + 1).ToString();
                                    }
                                    else
                                    {
                                        IVRM_Month_Id = Convert.ToInt32(Month.FirstOrDefault().IVRM_Month_Id);
                                        var days = DateTime.DaysInMonth(Convert.ToInt32(dto.HRES_Year), IVRM_Month_Id);

                                        config.HRC_SalaryToDay = days;
                                    }

                                    //employee list
                                    DateTime selectedFromdate = new DateTime(Convert.ToInt32(dto.HRES_Year), Convert.ToInt32(IVRM_Month_Id), config.HRC_SalaryFromDay, 0, 0, 0, 0);

                                    // string selectedTodate = "" + config.HRC_SalaryToDay + "-" + IVRM_Month_Id + "-" + Convert.ToInt32(dto.HRES_Year) + "";
                                    DateTime selectedTodate = new DateTime(Convert.ToInt32(dto.HRES_Year), Convert.ToInt32(IVRM_Month_Id), config.HRC_SalaryToDay, 0, 0, 0, 0);


                                    var leftEmp = employe.Where(t => t.HRME_LeftFlag == true && Convert.ToDateTime(t.HRME_DOL) < Convert.ToDateTime(selectedFromdate)).Select(t => t.HRME_Id);
                                    if (leftEmp.Count() > 0)
                                    {
                                        employe = employe.Where(t => Convert.ToDateTime(t.HRME_DOJ) <= Convert.ToDateTime(selectedTodate) && !leftEmp.Contains(t.HRME_Id) == true).ToList();
                                    }
                                    else
                                    {
                                        employe = employe.Where(t => Convert.ToDateTime(t.HRME_DOJ) <= Convert.ToDateTime(selectedTodate) && t.HRME_ActiveFlag == true).ToList();
                                    }

                                }

                            }

                        }
                    }
                    dto.employeedropdown = employe.ToArray();

                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }

            }
                return dto;            
        }

        public HR_Employee_SalaryModifiedDTO GetEmployeeDetailsByLeaveYearAndMonth(HR_Employee_SalaryModifiedDTO dto)
        {
            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
            try
            {
                //employee

                //employee list
                PROCESSList = (from ao in _HRMSContext.HR_Process_Auth_OrderNoDMO
                               from pa in _HRMSContext.HR_PROCESSDMO
                               from cc in _HRMSContext.Staff_User_Login
                               where (pa.HRPA_Id == ao.HRPA_Id && ao.IVRMUL_Id == cc.IVRMSTAUL_Id && cc.Id == dto.LogInUserId)
                               select pa
                 ).ToList();

                if (PROCESSList.Count() > 0)
                {

                    List<long> groupTypeIdList = PROCESSList.Select(t => t.HRMGT_Id).Distinct().ToList();
                    List<long> hrmD_IdList = PROCESSList.Select(t => t.HRMD_Id).Distinct().ToList();
                    List<long> hrmdeS_IdList = PROCESSList.Select(t => t.HRMDES_Id).Distinct().ToList();

                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeeDetails = employe.ToArray();
                    dto.employeedropdown = employe.ToArray();

                    //  dto.employeedropdown = employeeDetails.ToArray();

                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id)) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdownlist = GroupTypelist.ToArray();

                    ////Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdownlist = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdownlist = Designationlist.ToArray();

                }
                else
                {

                    employe = (from a in _HRMSContext.MasterEmployee
                               from b in _HRMSContext.HR_Employee_Salary
                               where (b.HRME_Id == a.HRME_Id && b.MI_Id.Equals(dto.MI_Id))
                               && b.HRES_Year.Equals(dto.HRES_Year) && b.HRES_Month.Equals(dto.HRES_Month) && a.HRME_ActiveFlag == true
                               select a).Distinct().ToList();
                    if (employe.Count > 0)
                    {
                        // employe = employe.Where(a => a.HRME_LeftFlag == false && Convert.ToDateTime(a.HRME_DOJ) <= Convert.ToDateTime(selecteddate)).ToList();


                        if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();

                        }
                        else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                        {
                            //employee
                            employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() == 0)
                        {
                            //employee
                            employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                        {
                            //employee
                            employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                        }

                        else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                        }

                        if (Convert.ToInt32(dto.HRES_Year) > 0 && dto.HRES_Month != "")
                        {
                            //get month id by month name
                            var Month = _Context.month.Where(t => t.Is_Active == true && t.IVRM_Month_Name.Equals(dto.HRES_Month)).ToList();
                            var config = _HRMSContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).FirstOrDefault();
                            int IVRM_Month_Id = 0;
                            if (Month.Count > 0)
                            {
                                if (config.HRC_SalaryFromDay > 1 && Convert.ToInt32(Month.FirstOrDefault().IVRM_Month_Id) < 12)
                                {

                                    IVRM_Month_Id = Convert.ToInt32(Month.FirstOrDefault().IVRM_Month_Id) + 1;
                                }
                                else if (config.HRC_SalaryFromDay > 1 && Convert.ToInt32(Month.FirstOrDefault().IVRM_Month_Id) == 12)
                                {
                                    IVRM_Month_Id = 01;
                                    dto.HRES_Year = (Convert.ToInt32(dto.HRES_Year) + 1).ToString();
                                }
                                else
                                {
                                    IVRM_Month_Id = Convert.ToInt32(Month.FirstOrDefault().IVRM_Month_Id);
                                    var days = DateTime.DaysInMonth(Convert.ToInt32(dto.HRES_Year), IVRM_Month_Id);

                                    config.HRC_SalaryToDay = days;
                                }

                                //employee list
                                DateTime selectedFromdate = new DateTime(Convert.ToInt32(dto.HRES_Year), Convert.ToInt32(IVRM_Month_Id), config.HRC_SalaryFromDay, 0, 0, 0, 0);

                                // string selectedTodate = "" + config.HRC_SalaryToDay + "-" + IVRM_Month_Id + "-" + Convert.ToInt32(dto.HRES_Year) + "";
                                DateTime selectedTodate = new DateTime(Convert.ToInt32(dto.HRES_Year), Convert.ToInt32(IVRM_Month_Id), config.HRC_SalaryToDay, 0, 0, 0, 0);


                                var leftEmp = employe.Where(t => t.HRME_LeftFlag == true && Convert.ToDateTime(t.HRME_DOL) < Convert.ToDateTime(selectedFromdate)).Select(t => t.HRME_Id);
                                if (leftEmp.Count() > 0)
                                {
                                    employe = employe.Where(t => Convert.ToDateTime(t.HRME_DOJ) <= Convert.ToDateTime(selectedTodate) && !leftEmp.Contains(t.HRME_Id) == true).ToList();
                                }
                                else
                                {
                                    employe = employe.Where(t => Convert.ToDateTime(t.HRME_DOJ) <= Convert.ToDateTime(selectedTodate) && t.HRME_ActiveFlag == true).ToList();
                                }

                            }

                        }

                    }
                }
                dto.employeedropdown = employe.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }


        public async Task<HR_Employee_SalaryModifiedDTO> GenerateEmployeeSalarySlipMultiMonth(HR_Employee_SalaryModifiedDTO dto)
        {
            try
            {
                List<HR_Employee_SalaryModifiedDTO> main_list = new List<HR_Employee_SalaryModifiedDTO>();
                for (int j = 0; j < dto.empid.Count(); j++)
                { 
                    for (int i = 0; i < dto.MonthList.Length; i++)
                //if(dto.MonthList!="")
                {
                    HR_Employee_SalaryModifiedDTO obj = new HR_Employee_SalaryModifiedDTO();
                    obj.HRME_Id = dto.empid[j];                    
                    obj.MI_Id = dto.MI_Id;
                    obj.HRES_Year = dto.HRES_Year;
                    obj.HRES_Month = dto.MonthList[i].Trim().ToString();
                    //obj.HRME_Id = dto.HRME_Id;
                    dto.HRES_Month = dto.MonthList[i].Trim().ToString();

                    Institution institute = new Institution();
                    institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                    InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                    obj.institutionDetails = dmoObj;

                    MasterEmployee employe = _HRMSContext.MasterEmployee.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_Id.Equals(obj.HRME_Id));

                    var DepartmentName = _HRMSContext.HR_Master_Department.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_Id.Equals(employe.HRMD_Id)).HRMD_DepartmentName;
                    var DesignationName = _HRMSContext.HR_Master_Designation.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_Id.Equals(employe.HRMDES_Id)).HRMDES_DesignationName;

                    //Employee Basic Details
                    MasterEmployeeDTO employeObj = Mapper.Map<MasterEmployeeDTO>(employe);
                    obj.currentemployeeDetails = employeObj;

                    obj.DesignationName = DesignationName;
                    obj.DepartmentName = DepartmentName;


                    //Configuration details
                    HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));
                    HR_ConfigurationDTO HR_ConfigurationDTO = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                    obj.PayrollStandard = HR_ConfigurationDTO;

                    //Employee Earning /Deduction heads

                    obj = await getEmployeeSalarySlip(obj);
                    decimal Lopdays = 0;
                    decimal LopAmount = 0;
                    //LOP Calculation

                    var employeSalary = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month) && t.HRME_Id == obj.HRME_Id).FirstOrDefault();
                    if (employeSalary != null)
                    {
                        HR_Employee_SalaryModifiedDTO employeSalObj = Mapper.Map<HR_Employee_SalaryModifiedDTO>(employeSalary);

                        obj.empsaldetail = employeSalObj;

                        var LOPcal = (from A in _HRMSContext.HR_Emp_Leave_Trans
                                      from c in _HRMSContext.HR_Emp_Leave_Trans_Details
                                      from B in _HRMSContext.HR_Master_Leave
                                      where (B.HRML_Id == A.HRELT_LeaveId && A.HRELT_Id==c.HRELT_Id &&
                                      A.MI_Id.Equals(dto.MI_Id) && A.HRME_Id == obj.HRME_Id  && A.HRELT_ActiveFlag == true && c.HRELTD_LWPFlag==true 
                                    && ((A.HRELT_FromDate >= employeSalary.HRES_FromDate && A.HRELT_FromDate <= employeSalary.HRES_ToDate)
                                        || (A.HRELT_ToDate >= employeSalary.HRES_FromDate && A.HRELT_ToDate <= employeSalary.HRES_ToDate))
                                      )
                                      select A
                                   ).ToList();
                        if (LOPcal.Count() > 0)
                        {
                            Lopdays = LOPcal.Sum(t => t.HRELT_TotDays);

                            LopAmount = Convert.ToDecimal(Lopdays) * Convert.ToDecimal(employeSalary.HRES_DailyRates);
                        }

                        obj.empsaldetail.Lopdays = Lopdays;
                        obj.empsaldetail.LopAmount = LopAmount;

                        //Leave Details

                        var LeayearId = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_LeaveYear.Equals(dto.HRES_Year)).FirstOrDefault().HRMLY_Id;

                        if (LeayearId > 0)
                        {
                            var LeaveDetails = (from A in _HRMSContext.HR_Emp_Leave_StatusDMO
                                                from B in _HRMSContext.HR_Master_Leave
                                                where (B.HRML_Id == A.HRML_Id &&
                                                A.MI_Id.Equals(dto.MI_Id) && A.HRME_Id == obj.HRME_Id &&
                                                A.HRMLY_Id == LeayearId)
                                                select new HR_Emp_Leave_StatusDTO
                                                {
                                                    HRELS_Id = A.HRELS_Id,
                                                    HRML_LeaveName = B.HRML_LeaveName,
                                                    HRELS_TotalLeaves = A.HRELS_TotalLeaves,
                                                    HRELS_TransLeaves = A.HRELS_TransLeaves,
                                                    HRELS_CBLeaves = A.HRELS_CBLeaves
                                                }).ToList();

                            obj.employeeLeaveDetails = LeaveDetails.ToArray();
                        }
                    }
                    if (obj.employeeSalaryslipDetails.Length != 0) {
                        main_list.Add(obj);   
                    }
                    //main_list.Add(obj);
                }
                }
                //for (int i = 0; i < dto.MonthList.Length; i++)
                //{
                //    HR_Employee_SalaryModifiedDTO obj = new HR_Employee_SalaryModifiedDTO();
                //    //obj.HRME_Id = dto.empid[i];                    
                //    obj.MI_Id = dto.MI_Id;
                //    obj.HRES_Year = dto.HRES_Year;
                //    obj.HRES_Month = dto.MonthList[i];
                //    obj.HRME_Id = dto.HRME_Id;

                //    Institution institute = new Institution();
                //    institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                //    InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                //    obj.institutionDetails = dmoObj;

                //    MasterEmployee employe = _HRMSContext.MasterEmployee.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_Id.Equals(obj.HRME_Id));

                //    var DepartmentName = _HRMSContext.HR_Master_Department.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_Id.Equals(employe.HRMD_Id)).HRMD_DepartmentName;
                //    var DesignationName = _HRMSContext.HR_Master_Designation.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_Id.Equals(employe.HRMDES_Id)).HRMDES_DesignationName;

                //    //Employee Basic Details
                //    MasterEmployeeDTO employeObj = Mapper.Map<MasterEmployeeDTO>(employe);
                //    obj.currentemployeeDetails = employeObj;

                //    obj.DesignationName = DesignationName;
                //    obj.DepartmentName = DepartmentName;


                //    //Configuration details
                //    HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));
                //    HR_ConfigurationDTO HR_ConfigurationDTO = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                //    obj.PayrollStandard = HR_ConfigurationDTO;

                //    //Employee Earning /Deduction heads

                //    obj = await getEmployeeSalarySlip(obj);


                //    Double? Lopdays = 0;
                //    decimal LopAmount = 0;
                //    //LOP Calculation

                //    var employeSalary = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month) && t.HRME_Id == obj.HRME_Id).FirstOrDefault();
                //    if (employeSalary != null)
                //    {
                //        HR_Employee_SalaryModifiedDTO employeSalObj = Mapper.Map<HR_Employee_SalaryModifiedDTO>(employeSalary);

                //        obj.empsaldetail = employeSalObj;

                //        var LOPcal = (from A in _HRMSContext.HR_Emp_Leave_Trans
                //                      from B in _HRMSContext.HR_Master_Leave
                //                      where (B.HRML_Id == A.HRELT_LeaveId &&
                //                      A.MI_Id.Equals(dto.MI_Id) && A.HRME_Id == obj.HRME_Id &&
                //                      B.HRML_LeaveType.Equals("LWP") && A.HRELT_ActiveFlag == true
                //                    && ((A.HRELT_FromDate >= employeSalary.HRES_FromDate && A.HRELT_FromDate <= employeSalary.HRES_ToDate)
                //                        || (A.HRELT_ToDate >= employeSalary.HRES_FromDate && A.HRELT_ToDate <= employeSalary.HRES_ToDate))
                //                      )
                //                      select A
                //                   ).ToList();
                //        if (LOPcal.Count() > 0)
                //        {
                //            Lopdays = LOPcal.Sum(t => t.HRELT_TotDays);

                //            LopAmount = Convert.ToDecimal(Lopdays) * Convert.ToDecimal(employeSalary.HRES_DailyRates);
                //        }

                //        obj.empsaldetail.Lopdays = Lopdays;
                //        obj.empsaldetail.LopAmount = LopAmount;

                //        //Leave Details

                //        var LeayearId = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_LeaveYear.Equals(dto.HRES_Year)).FirstOrDefault().HRMLY_Id;

                //        if (LeayearId > 0)
                //        {
                //            var LeaveDetails = (from A in _HRMSContext.HR_Emp_Leave_StatusDMO
                //                                from B in _HRMSContext.HR_Master_Leave
                //                                where (B.HRML_Id == A.HRML_Id &&
                //                                A.MI_Id.Equals(dto.MI_Id) && A.HRME_Id == dto.HRME_Id &&
                //                                A.HRMLY_Id == LeayearId)
                //                                select new HR_Emp_Leave_StatusDTO
                //                                {
                //                                    HRELS_Id = A.HRELS_Id,
                //                                    HRML_LeaveName = B.HRML_LeaveName,
                //                                    HRELS_TotalLeaves = A.HRELS_TotalLeaves,
                //                                    HRELS_TransLeaves = A.HRELS_TransLeaves,
                //                                    HRELS_CBLeaves = A.HRELS_CBLeaves

                //                                }).ToList();

                //            obj.employeeLeaveDetails = LeaveDetails.ToArray();
                //        }



                //    }
                //    main_list.Add(obj);
                //    ////---To Delete The Generated File Start ---

                //    ///////////
                //    //////var Empdetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == obj.HRME_Id && t.HRME_ActiveFlag == true).ToList();
                //    //////string slaryslip = "Salary Slip Of Employee " + Empdetails.FirstOrDefault().HRME_EmployeeCode + ".pdf";
                //    //////string GetDownloadFolderPath =
                //    //////      Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();

                //    //////IFileProvider provider = new PhysicalFileProvider(GetDownloadFolderPath);
                //    //////IFileInfo fileInfo = provider.GetFileInfo(slaryslip);

                //    //////bool fileexists = File.Exists(fileInfo.PhysicalPath);
                //    //////if (fileexists == true)
                //    //////{
                //    //////    File.Delete(fileInfo.PhysicalPath);
                //    //////}
                //    //---To Delete The Generated File Start ---
                //    ///////////////////////////
                //}
                dto.main_list = main_list.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public async Task<HR_Employee_SalaryModifiedDTO> GenerateEmployeeSalarySlip(HR_Employee_SalaryModifiedDTO dto)
        {
            try
            {
                List<HR_Employee_SalaryModifiedDTO> main_list = new List<HR_Employee_SalaryModifiedDTO>();
                for (int i = 0; i < dto.empid.Count(); i++)
                //for (int i = 0; i < dto.MonthList.Length; i++)
                //if(dto.MonthList!="")
                {
                    HR_Employee_SalaryModifiedDTO obj = new HR_Employee_SalaryModifiedDTO();
                    obj.HRME_Id = dto.empid[i];                    
                    obj.MI_Id = dto.MI_Id;
                    obj.HRES_Year = dto.HRES_Year;
                    obj.HRES_Month = dto.MonthList[i];
                    //obj.HRME_Id = dto.HRME_Id;

                    Institution institute = new Institution();
                    institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                    InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                    obj.institutionDetails = dmoObj;

                    MasterEmployee employe = _HRMSContext.MasterEmployee.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_Id.Equals(obj.HRME_Id));

                    var DepartmentName = _HRMSContext.HR_Master_Department.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_Id.Equals(employe.HRMD_Id)).HRMD_DepartmentName;
                    var DesignationName = _HRMSContext.HR_Master_Designation.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_Id.Equals(employe.HRMDES_Id)).HRMDES_DesignationName;

                    //Employee Basic Details
                    MasterEmployeeDTO employeObj = Mapper.Map<MasterEmployeeDTO>(employe);
                    obj.currentemployeeDetails = employeObj;
                    obj.DesignationName = DesignationName;
                    obj.DepartmentName = DepartmentName;


                    //Configuration details
                    HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));
                    HR_ConfigurationDTO HR_ConfigurationDTO = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                    obj.PayrollStandard = HR_ConfigurationDTO;

                    //Employee Earning /Deduction heads

                    obj = await getEmployeeSalarySlip(obj);


                    decimal Lopdays = 0;
                    decimal LopAmount = 0;
                    //LOP Calculation

                    var employeSalary = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month) && t.HRME_Id == obj.HRME_Id).FirstOrDefault();
                    if (employeSalary != null)
                    {
                        HR_Employee_SalaryModifiedDTO employeSalObj = Mapper.Map<HR_Employee_SalaryModifiedDTO>(employeSalary);

                        obj.empsaldetail = employeSalObj;

                        var LOPcal = (from A in _HRMSContext.HR_Emp_Leave_Trans
                                      from B in _HRMSContext.HR_Master_Leave
                                      where (B.HRML_Id == A.HRELT_LeaveId &&
                                      A.MI_Id.Equals(dto.MI_Id) && A.HRME_Id == obj.HRME_Id &&
                                      B.HRML_LeaveType.Equals("LWP") && A.HRELT_ActiveFlag == true
                                    && ((A.HRELT_FromDate >= employeSalary.HRES_FromDate && A.HRELT_FromDate <= employeSalary.HRES_ToDate)
                                        || (A.HRELT_ToDate >= employeSalary.HRES_FromDate && A.HRELT_ToDate <= employeSalary.HRES_ToDate))
                                      )
                                      select A
                                   ).ToList();
                        if (LOPcal.Count() > 0)
                        {
                            Lopdays = LOPcal.Sum(t => t.HRELT_TotDays);

                            LopAmount = Convert.ToDecimal(Lopdays) * Convert.ToDecimal(employeSalary.HRES_DailyRates);
                        }

                        obj.empsaldetail.Lopdays = Lopdays;
                        obj.empsaldetail.LopAmount = LopAmount;

                        //Leave Details

                        var LeayearId = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_LeaveYear.Equals(dto.HRES_Year)).FirstOrDefault().HRMLY_Id;

                        if (LeayearId > 0)
                        {
                            var LeaveDetails = (from A in _HRMSContext.HR_Emp_Leave_StatusDMO
                                                from B in _HRMSContext.HR_Master_Leave
                                                where (B.HRML_Id == A.HRML_Id &&
                                                A.MI_Id.Equals(dto.MI_Id) && A.HRME_Id == dto.HRME_Id &&
                                                A.HRMLY_Id == LeayearId)
                                                select new HR_Emp_Leave_StatusDTO
                                                {
                                                    HRELS_Id = A.HRELS_Id,
                                                    HRML_LeaveName = B.HRML_LeaveName,
                                                    HRELS_TotalLeaves = A.HRELS_TotalLeaves,
                                                    HRELS_TransLeaves = A.HRELS_TransLeaves,
                                                    HRELS_CBLeaves = A.HRELS_CBLeaves

                                                }).ToList();

                            obj.employeeLeaveDetails = LeaveDetails.ToArray();
                        }



                    }
                    main_list.Add(obj);


                }




                //for (int i = 0; i < dto.MonthList.Length; i++)
                //{
                //    HR_Employee_SalaryModifiedDTO obj = new HR_Employee_SalaryModifiedDTO();
                //    //obj.HRME_Id = dto.empid[i];                    
                //    obj.MI_Id = dto.MI_Id;
                //    obj.HRES_Year = dto.HRES_Year;
                //    obj.HRES_Month = dto.MonthList[i];
                //    obj.HRME_Id = dto.HRME_Id;

                //    Institution institute = new Institution();
                //    institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                //    InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                //    obj.institutionDetails = dmoObj;

                //    MasterEmployee employe = _HRMSContext.MasterEmployee.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_Id.Equals(obj.HRME_Id));

                //    var DepartmentName = _HRMSContext.HR_Master_Department.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_Id.Equals(employe.HRMD_Id)).HRMD_DepartmentName;
                //    var DesignationName = _HRMSContext.HR_Master_Designation.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_Id.Equals(employe.HRMDES_Id)).HRMDES_DesignationName;

                //    //Employee Basic Details
                //    MasterEmployeeDTO employeObj = Mapper.Map<MasterEmployeeDTO>(employe);
                //    obj.currentemployeeDetails = employeObj;

                //    obj.DesignationName = DesignationName;
                //    obj.DepartmentName = DepartmentName;


                //    //Configuration details
                //    HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));
                //    HR_ConfigurationDTO HR_ConfigurationDTO = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                //    obj.PayrollStandard = HR_ConfigurationDTO;

                //    //Employee Earning /Deduction heads

                //    obj = await getEmployeeSalarySlip(obj);


                //    Double? Lopdays = 0;
                //    decimal LopAmount = 0;
                //    //LOP Calculation

                //    var employeSalary = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month) && t.HRME_Id == obj.HRME_Id).FirstOrDefault();
                //    if (employeSalary != null)
                //    {
                //        HR_Employee_SalaryModifiedDTO employeSalObj = Mapper.Map<HR_Employee_SalaryModifiedDTO>(employeSalary);

                //        obj.empsaldetail = employeSalObj;

                //        var LOPcal = (from A in _HRMSContext.HR_Emp_Leave_Trans
                //                      from B in _HRMSContext.HR_Master_Leave
                //                      where (B.HRML_Id == A.HRELT_LeaveId &&
                //                      A.MI_Id.Equals(dto.MI_Id) && A.HRME_Id == obj.HRME_Id &&
                //                      B.HRML_LeaveType.Equals("LWP") && A.HRELT_ActiveFlag == true
                //                    && ((A.HRELT_FromDate >= employeSalary.HRES_FromDate && A.HRELT_FromDate <= employeSalary.HRES_ToDate)
                //                        || (A.HRELT_ToDate >= employeSalary.HRES_FromDate && A.HRELT_ToDate <= employeSalary.HRES_ToDate))
                //                      )
                //                      select A
                //                   ).ToList();
                //        if (LOPcal.Count() > 0)
                //        {
                //            Lopdays = LOPcal.Sum(t => t.HRELT_TotDays);

                //            LopAmount = Convert.ToDecimal(Lopdays) * Convert.ToDecimal(employeSalary.HRES_DailyRates);
                //        }

                //        obj.empsaldetail.Lopdays = Lopdays;
                //        obj.empsaldetail.LopAmount = LopAmount;

                //        //Leave Details

                //        var LeayearId = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_LeaveYear.Equals(dto.HRES_Year)).FirstOrDefault().HRMLY_Id;

                //        if (LeayearId > 0)
                //        {
                //            var LeaveDetails = (from A in _HRMSContext.HR_Emp_Leave_StatusDMO
                //                                from B in _HRMSContext.HR_Master_Leave
                //                                where (B.HRML_Id == A.HRML_Id &&
                //                                A.MI_Id.Equals(dto.MI_Id) && A.HRME_Id == dto.HRME_Id &&
                //                                A.HRMLY_Id == LeayearId)
                //                                select new HR_Emp_Leave_StatusDTO
                //                                {
                //                                    HRELS_Id = A.HRELS_Id,
                //                                    HRML_LeaveName = B.HRML_LeaveName,
                //                                    HRELS_TotalLeaves = A.HRELS_TotalLeaves,
                //                                    HRELS_TransLeaves = A.HRELS_TransLeaves,
                //                                    HRELS_CBLeaves = A.HRELS_CBLeaves

                //                                }).ToList();

                //            obj.employeeLeaveDetails = LeaveDetails.ToArray();
                //        }



                //    }
                //    main_list.Add(obj);
                //    ////---To Delete The Generated File Start ---

                //    ///////////
                //    //////var Empdetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == obj.HRME_Id && t.HRME_ActiveFlag == true).ToList();
                //    //////string slaryslip = "Salary Slip Of Employee " + Empdetails.FirstOrDefault().HRME_EmployeeCode + ".pdf";
                //    //////string GetDownloadFolderPath =
                //    //////      Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();

                //    //////IFileProvider provider = new PhysicalFileProvider(GetDownloadFolderPath);
                //    //////IFileInfo fileInfo = provider.GetFileInfo(slaryslip);

                //    //////bool fileexists = File.Exists(fileInfo.PhysicalPath);
                //    //////if (fileexists == true)
                //    //////{
                //    //////    File.Delete(fileInfo.PhysicalPath);
                //    //////}
                //    //---To Delete The Generated File Start ---
                //    ///////////////////////////

                //}
                dto.main_list = main_list.ToArray();


            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }

            return dto;
        }


        public async Task<HR_Employee_SalaryModifiedDTO> getEmployeeSalarySlip(HR_Employee_SalaryModifiedDTO dto)
        {

            try
            {
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EmployeeSalarySlipGeneration";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                        SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.HRME_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMLY_LeaveYear",
                  SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.HRES_Year)
                    });

                    cmd.Parameters.Add(new SqlParameter("@IVRM_Month_Name",
                  SqlDbType.VarChar)
                    {
                        Value = Convert.ToString(dto.HRES_Month)
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        // var data = cmd.ExecuteNonQuery();

                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.employeeSalaryslipDetails = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }




            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }


            return dto;
        }


        public static string NumberToWords(long number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public async Task<HR_Employee_SalaryModifiedDTO> SendEmailSMS(HR_Employee_SalaryModifiedDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                for (int i = 0; i < dto.empid.Count(); i++)
                {
                    HR_Employee_SalaryModifiedDTO obj = new HR_Employee_SalaryModifiedDTO();
                    obj.HRME_Id = dto.empid[i];
                    var employeedetails = _HRMSContext.MasterEmployee.Single(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_Id == obj.HRME_Id);

                    if (dto.EmailSMS == "Email")
                    {
                        Email Email = new Email(_Context);


                        var email_list = _HRMSContext.Emp_Email_Id.Where(t => t.HRME_Id == obj.HRME_Id && t.HRMEM_DeFaultFlag.Equals("default")).ToList();
                        var Empdetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == obj.HRME_Id && t.HRME_ActiveFlag == true).ToList();
                        string slaryslip = "Salary Slip Of Employee " + Empdetails.FirstOrDefault().HRME_EmployeeCode + ".pdf";

                        if (email_list.Count > 0)
                        {
                            string Subject = "Salary slip";
                            //string m = Email.sendmailWithoutTemplate(dto.MI_Id, email_list.FirstOrDefault().HRMEM_EmailId, dto.Template, Subject);
                            string m = Email.sendmailforslaryslip(dto.MI_Id, email_list.FirstOrDefault().HRMEM_EmailId, dto.Template, Subject, slaryslip);


                            dto.retrunMsg = m;

                            Console.WriteLine(m);
                        }
                        else
                        {
                            dto.retrunMsg = "notFound";
                        }


                    }
                    else if (dto.EmailSMS == "SMS")
                    {

                        var mobile_list = _HRMSContext.Emp_MobileNo.Where(t => t.HRME_Id == obj.HRME_Id && t.HRMEMNO_DeFaultFlag.Equals("default")).ToList();

                        if (mobile_list.Count > 0)
                        {
                            SMS sms = new SMS(_Context);

                            string smsdet = await sms.sendSms(dto.MI_Id, Convert.ToInt64(mobile_list.FirstOrDefault().HRMEMNO_MobileNo), "EMPLOYEE_SALARY_SLIP", obj.HRME_Id);
                            dto.retrunMsg = smsdet;
                            Console.WriteLine(smsdet);
                        }
                        else
                        {
                            dto.retrunMsg = "notFound";
                        }
                    }
                }
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error";
            }

            return dto;

        }

        public HR_Employee_SalaryModifiedDTO get_depts(HR_Employee_SalaryModifiedDTO data)
        {
            try
            {
                data.departmentdropdown = (from a in _HRMSContext.MasterEmployee
                                           from b in _HRMSContext.HR_Master_Department
                                           where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMD_Id == b.HRMD_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMD_ActiveFlag == true)
                                           select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HR_Employee_SalaryModifiedDTO get_desig(HR_Employee_SalaryModifiedDTO data)
        {
            try
            {
                data.designationdropdown = (from a in _HRMSContext.MasterEmployee
                                            from b in _HRMSContext.HR_Master_Designation
                                            where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && data.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                            select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
