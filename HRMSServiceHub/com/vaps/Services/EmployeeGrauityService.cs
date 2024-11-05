using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;

namespace HRMSServicesHub.com.vaps.Services
{
    public class EmployeeGrauityService : Interfaces.EmployeeGrauityInterface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public EmployeeGrauityService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }
        public HR_Employee_SalaryDTO getBasicData(HR_Employee_SalaryDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }


        public HR_Employee_SalaryDTO GetAllDropdownAndDatatableDetails(HR_Employee_SalaryDTO dto)
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

              
                Monthlist = _Context.month.Where(t => t.Is_Active == true).ToList();
                dto.monthdropdown = Monthlist.ToArray();
               
             

              
                dto.leaveyeardropdown = _HRMSContext.IVRM_Master_FinancialYear.ToArray();
             
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


                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id)) && t.HRME_LeftFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedropdown = employe.ToArray();

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

                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id)  && t.HRME_LeftFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedropdown = employe.ToArray();


                }




            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public HR_Employee_SalaryDTO GetEmployeeDetailsByLeaveYearAndMonth(HR_Employee_SalaryDTO dto)
        {

            return dto;
        }



        public async Task<HR_Employee_SalaryDTO> GenerateEmployeeSalarySlip(HR_Employee_SalaryDTO dto)
        {
            try
            {

            
            Institution institute = new Institution();
            institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

            InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
            dto.institutionDetails = dmoObj;

            MasterEmployee employe = _HRMSContext.MasterEmployee.Single(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_Id.Equals(dto.HRME_Id) && t.HRME_LeftFlag==true);

            var DepartmentName = _HRMSContext.HR_Master_Department.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_Id.Equals(employe.HRMD_Id)).HRMD_DepartmentName;
            var DesignationName = _HRMSContext.HR_Master_Designation.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_Id.Equals(employe.HRMDES_Id)).HRMDES_DesignationName;

            //Employee Basic Details
            MasterEmployeeDTO employeObj = Mapper.Map<MasterEmployeeDTO>(employe);
            dto.currentemployeeDetails = employeObj;
        
            dto.DesignationName = DesignationName;
            dto.DepartmentName = DepartmentName;

                //Configuration details
                //HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));
                //HR_ConfigurationDTO HR_ConfigurationDTO = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                //dto.PayrollStandard = HR_ConfigurationDTO;

                //Employee Earning /Deduction heads

                //  dto = await getEmployeeSalarySlip(dto);


                //Double? Lopdays = 0;
                //decimal LopAmount = 0;
                ////LOP Calculation

                //var employeSalary = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month) && t.HRME_Id == dto.HRME_Id).FirstOrDefault();
                //if (employeSalary != null)
                //{
                //    HR_Employee_SalaryDTO employeSalObj = Mapper.Map<HR_Employee_SalaryDTO>(employeSalary);

                //    dto.empsaldetail = employeSalObj;

                //    var LOPcal = (from A in _HRMSContext.MasterEmployee
                //                  from B in _HRMSContext.HR_Master_Designation

                //                  select A
                //               ).ToList();
                //    if (LOPcal.Count() > 0)
                //    {
                //       // Lopdays = LOPcal.Sum(t => t.);

                //       // LopAmount = Convert.ToDecimal(Lopdays) * Convert.ToDecimal(employeSalary.HRES_DailyRates);
                //    }

                //    dto.empsaldetail.Lopdays = Lopdays;
                //    dto.empsaldetail.LopAmount = LopAmount;

                //    //Leave Details

                //    var LeayearId = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_LeaveYear.Equals(dto.HRES_Year)).FirstOrDefault().HRMLY_Id;

                //    if (LeayearId > 0)
                //    {
                //        var LeaveDetails = (from A in _HRMSContext.HR_Emp_Leave_StatusDMO
                //                            from B in _HRMSContext.HR_Master_Leave
                //                            where (B.HRML_Id == A.HRML_Id &&
                //                            A.MI_Id.Equals(dto.MI_Id) && A.HRME_Id == dto.HRME_Id &&
                //                            A.HRMLY_Id == LeayearId)
                //                            select new HR_Emp_Leave_StatusDTO
                //                            {
                //                                HRELS_Id = A.HRELS_Id,
                //                                HRML_LeaveName = B.HRML_LeaveName,
                //                                HRELS_TotalLeaves = A.HRELS_TotalLeaves,
                //                                HRELS_TransLeaves = A.HRELS_TransLeaves,
                //                                HRELS_CBLeaves = A.HRELS_CBLeaves

                //                            }).ToList();

                //        dto.employeeLeaveDetails = LeaveDetails.ToArray();
                //    }

                //}

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }


            return dto;
        }


        public async Task<HR_Employee_SalaryDTO> getEmployeeSalarySlip(HR_Employee_SalaryDTO dto)
        {

           

            return dto;
        }








    }
}