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
    public class HeadwiseReportsService : Interfaces.HeadwiseReportsInterface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public HeadwiseReportsService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public HeaderwiseReportDTO getBasicData(HeaderwiseReportDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }
        public HeaderwiseReportDTO GetAllDropdownAndDatatableDetails(HeaderwiseReportDTO dto)
        {
           List<HR_Employee_Salary> SalaryCalculation = new List<HR_Employee_Salary>();
                //   List<MasterEmployee> employe = new List<MasterEmployee>();
                List<HR_Master_LeaveYearDMO> leaveyear = new List<HR_Master_LeaveYearDMO>();



                List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
                List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
                List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
                List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
                List<HR_Master_CourseDMO> Courselist = new List<HR_Master_CourseDMO>();
                List<HR_Master_Grade> Gradelist = new List<HR_Master_Grade>();
                List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
                List<MasterEmployee> emp = new List<MasterEmployee>();
                try
                {
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

                        //emp = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                        //dto.employeedropdown = emp.ToArray();

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
                        //dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();

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


                    //emptype
                    //dto.employeeTypedropdown = _HRMSContext.HR_Master_EmployeeType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMET_ActiveFlag == true).ToArray();


                    //// employee grouptype
                    //dto.groupTypedropdown = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToArray();

                    ////departmentdropdown
                    //dto.departmentdropdown = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToArray();

                    ////designationdropdown 
                    //dto.designationdropdown = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToArray();


                    // earning , deduction details

                    //leave year
                    dto.leaveyeardropdown = _HRMSContext.HR_MasterLeaveYear.Where(t => t.HRMLY_ActiveFlag == true && t.MI_Id.Equals(dto.MI_Id)).OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();


                dto.monthdropdown = _Context.month.Where(t => t.Is_Active == true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
        public async Task<HeaderwiseReportDTO> getEmployeedetailsBySelection(HeaderwiseReportDTO dto)
        {
            List<HR_Employee_Salary> employeeDetails = new List<HR_Employee_Salary>();
            List<HeaderwiseReportDTO> headerwiseReportDetails = new List<HeaderwiseReportDTO>();

            try
            {
                if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() > 0)
                {
                    //employee
                    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month)).ToList();

                }
                else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() == 0)
                {
                    //employee
                    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month)).ToList();
                }
                else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() > 0)
                {
                    //employee
                    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month)).ToList();
                }
                else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() > 0)
                {
                    //employee
                    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month)).ToList();
                }
                else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() == 0)
                {
                    //employee
                    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month)).ToList();
                }
                else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() == 0)
                {
                    //employee
                    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.departmentselected.Contains(t.HRMD_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month)).ToList();
                }

                else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() > 0)
                {
                    //employee
                    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month)).ToList();
                }

                if (employeeDetails.Count() > 0)
                {

                    foreach (var emp in employeeDetails)
                    {
                        // All heads
                        var empHeads = (from med in _HRMSContext.HR_Master_EarningsDeductions
                                        from eed in _HRMSContext.HR_Employee_EarningsDeductions
                                        where (eed.HRMED_Id == med.HRMED_Id && eed.HRME_Id == emp.HRME_Id)
                                        select med).ToList();

                        //Earning Heads
                        var empEarningHead = empHeads.Where(t => t.HRMED_EarnDedFlag.Equals("Earning")).Select(t => t.HRMED_Id).ToList();

                        //Deduction heads
                        var empDeductionHead = empHeads.Where(t => t.HRMED_EarnDedFlag.Equals("Deduction")).Select(t => t.HRMED_Id).ToList();

                        // Total Earning
                        decimal? TotalEarning = (from earn in _HRMSContext.HR_Employee_Salary_Details
                                                 where (earn.HRES_Id == emp.HRES_Id && empEarningHead.Contains(earn.HRMED_Id))
                                                 select earn.HRESD_Amount).Sum();

                        if (TotalEarning == null)
                        {
                            TotalEarning = 0;
                        }



                        // Total deduction
                        decimal? TotalDeduction = (from deduc in _HRMSContext.HR_Employee_Salary_Details
                                                   where (deduc.HRES_Id == emp.HRES_Id && empDeductionHead.Contains(deduc.HRMED_Id))
                                                   select deduc.HRESD_Amount).Sum();
                        if (TotalDeduction == null)
                        {
                            TotalDeduction = 0;
                        }


                        // net salary
                        decimal? NetSalary = TotalEarning - TotalDeduction;


                        HeaderwiseReportDTO headerwiseReportDTO = new HeaderwiseReportDTO();
                        headerwiseReportDTO = (from a in _HRMSContext.MasterEmployee
                                               from b in _HRMSContext.HR_Employee_Salary
                                               from c in _HRMSContext.HR_Master_Designation
                                               where (b.HRME_Id == a.HRME_Id &&
                                               b.HRMDES_Id == c.HRMDES_Id
                                               &&
                                                b.MI_Id.Equals(dto.MI_Id) &&
                                                b.HRES_Id == emp.HRES_Id)

                                               select new HeaderwiseReportDTO
                                               {
                                                   EmployeeCode = a.HRME_EmployeeCode,
                                                   EmployeeName = (a.HRME_EmployeeFirstName != null ? a.HRME_EmployeeFirstName : "") + " " + (a.HRME_EmployeeMiddleName != null ? a.HRME_EmployeeMiddleName : "") + " " + (a.HRME_EmployeeLastName != null ? a.HRME_EmployeeLastName : ""),
                                                   DesignationName = c.HRMDES_DesignationName,
                                                   TotalEarning = TotalEarning,
                                                   TotalDeduction = TotalDeduction,
                                                   NetSalary = NetSalary,
                                               }).FirstOrDefault();

                        headerwiseReportDetails.Add(headerwiseReportDTO);
                    }
                }

                dto.employeeDetails = headerwiseReportDetails.ToArray();
                Institution institute = new Institution();
                institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                dto.institutionDetails = dmoObj;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }


        public HeaderwiseReportDTO get_depts(HeaderwiseReportDTO data)
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



        public HeaderwiseReportDTO get_desig(HeaderwiseReportDTO data)
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
