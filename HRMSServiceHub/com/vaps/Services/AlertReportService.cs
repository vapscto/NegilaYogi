using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;


using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class AlertReportService : Interfaces.AlertReportInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public AlertReportService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public async Task<MasterEmployeeDTO> getAlertReport (MasterEmployeeDTO dto)
        {
            List<MasterEmployeeDTO> employeeDetails = new List<MasterEmployeeDTO>();
            //List<MasterEmployeeDTO> departmentlist = new List<MasterEmployeeDTO>();
            double dbldays = 0;
            double aleryday = 0;
            DateTime crntdate = DateTime.Today;
            var departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id == dto.MI_Id && t.HRMD_ActiveFlag == true).ToArray();
            var designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id == dto.MI_Id && t.HRMDES_ActiveFlag == true).ToArray();
            var alertdays = _HRMSContext.HR_Configuration.Where(t => t.MI_Id == dto.MI_Id).Select(t => t.HRC_AlertDay).FirstOrDefault();
            aleryday = Convert.ToDouble(alertdays);
            try
            {
                if (dto.Type.Equals("Retirement"))
                {
                    var empDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id == dto.MI_Id && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToArray();
                    for (int i = 0; i < empDetails.Length; i++)
                    {
                        dbldays = (DateTime.Today - Convert.ToDateTime(empDetails[i].HRME_DOB)).TotalDays;
                        if (dbldays < aleryday)
                        {
                            MasterEmployeeDTO ss = new MasterEmployeeDTO();
                            ss.HRME_EmployeeFirstName = empDetails[i].HRME_EmployeeFirstName;
                            ss.HRME_EmployeeMiddleName = empDetails[i].HRME_EmployeeMiddleName;
                            ss.HRME_EmployeeLastName = empDetails[i].HRME_EmployeeLastName;
                            ss.HRME_EmployeeCode = empDetails[i].HRME_EmployeeCode;
                            ss.HRME_DOB = empDetails[i].HRME_DOB;
                            ss.HRME_SalaryType = departmentlist.Where(t => t.HRMD_Id == empDetails[i].HRMD_Id).Select(t=>t.HRMD_DepartmentName).FirstOrDefault();
                            ss.HRME_UINumber = designationlist.Where(t => t.HRMDES_Id == empDetails[i].HRMDES_Id).Select(t => t.HRMDES_DesignationName).FirstOrDefault();
                            employeeDetails.Add(ss);
                        }
                    }
                    dto.employeedetailList = employeeDetails.ToArray();
                }
                else if (dto.Type.Equals("Increment"))
                {
                    var empDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id == dto.MI_Id && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToArray();
                    for (int i = 0; i < empDetails.Length; i++)
                    {
                        dbldays = (DateTime.Today - Convert.ToDateTime(empDetails[i].HRME_DOJ)).TotalDays;
                        if (dbldays < aleryday)
                        {
                            MasterEmployeeDTO ss = new MasterEmployeeDTO();
                            ss.HRME_EmployeeFirstName = empDetails[i].HRME_EmployeeFirstName;
                            ss.HRME_EmployeeMiddleName = empDetails[i].HRME_EmployeeMiddleName;
                            ss.HRME_EmployeeLastName = empDetails[i].HRME_EmployeeLastName;
                            ss.HRME_EmployeeCode = empDetails[i].HRME_EmployeeCode;
                            ss.HRME_DOB = empDetails[i].HRME_DOB;
                            ss.HRME_SalaryType = departmentlist.Where(t => t.HRMD_Id == empDetails[i].HRMD_Id).Select(t => t.HRMD_DepartmentName).FirstOrDefault();
                            ss.HRME_UINumber = designationlist.Where(t => t.HRMDES_Id == empDetails[i].HRMDES_Id).Select(t => t.HRMDES_DesignationName).FirstOrDefault();
                            employeeDetails.Add(ss);
                        }
                    }
                    dto.employeedetailList = employeeDetails.ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ex");
            }
            return dto;
        }
    }
}
