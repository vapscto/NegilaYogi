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
    public class EmployeeContributionReportService : Interfaces.EmployeeContributionReportInterface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public EmployeeContributionReportService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public EmployeeContributionReportDTO getBasicData(EmployeeContributionReportDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }
        public EmployeeContributionReportDTO GetAllDropdownAndDatatableDetails(EmployeeContributionReportDTO dto)
        {
            List<MasterEmployee> EmployeeList = new List<MasterEmployee>();

            // List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            List<HR_Master_CourseDMO> Courselist = new List<HR_Master_CourseDMO>();
            List<HR_Master_Grade> Gradelist = new List<HR_Master_Grade>();
            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
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

                    EmployeeList = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedropdown = EmployeeList.ToArray();

                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id)) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdown = GroupTypelist.ToArray();

                    //Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdown = Departmentlist.ToArray();
                   // dto.departmentdropdown = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id == dto.MI_Id && t.HRMD_ActiveFlag == true).ToArray();

                    //Designationlist
                    //Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    //dto.designationdropdown = Designationlist.ToArray();

                    dto.designationdropdown = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id == dto.MI_Id && t.HRMDES_ActiveFlag == true).ToArray();

                }
                else
                {
                    dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();

                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdown = GroupTypelist.ToArray();

                    //Departmentlist
                    //Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    //dto.departmentdropdown = Departmentlist.ToArray();
                    dto.departmentdropdown = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id==dto.MI_Id && t.HRMD_ActiveFlag == true).ToArray();

                    //Designationlist
                    //Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    //dto.designationdropdown = Designationlist.ToArray();
                    dto.designationdropdown = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id == dto.MI_Id && t.HRMDES_ActiveFlag == true).ToArray();


                }




                //emptype
                //dto.employeeTypedropdown = _HRMSContext.HR_Master_EmployeeType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMET_ActiveFlag == true).ToArray();

                ////employee  
                //dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();

                //// employee grouptype
                //dto.groupTypedropdown = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToArray();

                ////departmentdropdown
                //dto.departmentdropdown = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToArray();

                ////designationdropdown 
                //dto.designationdropdown = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).ToArray();


                // earning , deduction details

                //Earning list
                dto.earningdropdown = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.HRMED_EarnDedFlag.Equals("Earning") && t.MI_Id.Equals(dto.MI_Id) && t.HRMED_ActiveFlag == true).ToArray();


                //Deduction List
                dto.detectiondropdown = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.HRMED_EarnDedFlag.Equals("Deduction") && t.MI_Id.Equals(dto.MI_Id) && t.HRMED_ActiveFlag == true).ToArray();

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



        public EmployeeContributionReportDTO FilterEmployeeData(EmployeeContributionReportDTO dto)
        {
            try
            {
                List<HR_Employee_Salary> employeeDetails = new List<HR_Employee_Salary>();
                if (dto.FormatType.Equals("Format1"))
                {
                    if (dto.MonthBetweenDates.Equals("Month"))
                    {
                        employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) &&
                        t.HRES_Month.Equals(dto.hreS_Month) && t.HRES_Year.Equals(dto.hreS_Year)).ToList();
                    }
                    else if (dto.MonthBetweenDates.Equals("BetweenDates"))
                    {
                        employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id)
                        && t.HRES_FromDate >= dto.FromDate && t.HRES_ToDate <= dto.ToDate).ToList();
                    }

                    if (employeeDetails.Count() > 0)
                    {
                        var empIdList = employeeDetails.Select(t => t.HRME_Id);

                        var employeedropdown = (from a in _HRMSContext.MasterEmployee
                                                from b in _HRMSContext.HR_Employee_Salary
                                                where a.HRME_Id.Equals(b.HRME_Id)
                                                && b.MI_Id.Equals(dto.MI_Id)
                                                && a.HRME_ActiveFlag == true && empIdList.Contains(a.HRME_Id)
                                                orderby a.HRME_EmployeeOrder
                                                select a).Distinct();
                        dto.employeedropdown = employeedropdown.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }




            return dto;
        }
        public EmployeeContributionReportDTO getEmployeedetailsBySelection(EmployeeContributionReportDTO dto)
        {
            List<EmployeeContributionReportDTO> cumDTOList = new List<EmployeeContributionReportDTO>();


            List<HR_Employee_Salary> employeeDetails = new List<HR_Employee_Salary>();
            try
            {
                if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() > 0)
                {
                    if (dto.MonthBetweenDates.Equals("Month"))
                    {
                        employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRES_Month.Equals(dto.hreS_Month) && t.HRES_Year.Equals(dto.hreS_Year)).ToList();
                        // employeeDetails = employeeDetails.Where(t => t.HRES_Month.Equals(dto.hreS_Month) && t.HRES_Year.Equals(dto.hreS_Year)).ToList();

                    }
                    else if (dto.MonthBetweenDates.Equals("BetweenDates"))
                    {
                        employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRES_FromDate >= dto.FromDate && t.HRES_ToDate <= dto.ToDate).ToList();
                        //  employeeDetails = employeeDetails.Where(t => t.HRES_FromDate >= dto.FromDate && t.HRES_ToDate <= dto.ToDate).ToList();
                    }

                    if (dto.FormatType.Equals("Format1"))
                    {
                        employeeDetails = employeeDetails.Where(t => t.HRME_Id == dto.HRME_Id).ToList();
                    }

                    if (employeeDetails.Count() > 0)
                    {
                        long? headId = 0;
                        if (dto.EarningDeduction.Equals("Earning"))
                        {
                            headId = dto.EarningHead;
                        }
                        else if (dto.EarningDeduction.Equals("Deduction"))
                        {
                            headId = dto.DeductionHead;
                        }

                        foreach (var emp in employeeDetails)
                        {
                            EmployeeContributionReportDTO employe = new EmployeeContributionReportDTO();
                            if (headId == 999)
                            {
                                var HRESD_Amount = (from a in _HRMSContext.HR_Employee_Salary_Details
                                                    from b in _HRMSContext.HR_Master_EarningsDeductions
                                                    where (a.HRMED_Id == b.HRMED_Id && b.HRMED_EarnDedFlag == dto.EarningDeduction && b.MI_Id == dto.MI_Id && b.HRMED_ActiveFlag == true && a.HRES_Id == emp.HRES_Id)
                                                    select new EmployeeContributionReportDTO
                                                    {
                                                        EarningDeduction = b.HRMED_Name,
                                                        selectedHeadAmount = a.HRESD_Amount
                                                    }).Distinct().ToArray();

                                var headerlist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.HRMED_EarnDedFlag == dto.EarningDeduction && t.MI_Id == dto.MI_Id && t.HRMED_ActiveFlag == true).ToList();
                                dto.headerlist = headerlist.ToArray();
                                employe = (from a in _HRMSContext.MasterEmployee
                                           from b in _HRMSContext.HR_Employee_Salary
                                           from d in _HRMSContext.HR_Master_EarningsDeductions
                                           from e in _HRMSContext.HR_Master_Department
                                           from f in _HRMSContext.HR_Master_Designation
                                           where (a.HRME_Id == b.HRME_Id &&
                                        b.MI_Id.Equals(dto.MI_Id) &&
                                        e.HRMD_Id == b.HRMD_Id &&
                                        f.HRMDES_Id == b.HRMDES_Id &&
                                        b.HRES_Id.Equals(emp.HRES_Id))
                                           select new EmployeeContributionReportDTO
                                           {
                                               EmployeeCode = a.HRME_EmployeeCode,
                                               HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                               HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                               HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                               departmentName = e.HRMD_DepartmentName,
                                               designationName = f.HRMDES_DesignationName,
                                               employeeContributionDetails = HRESD_Amount
                                           }).FirstOrDefault();
                            }
                            else
                            {
                                var HRESD_Amount = _HRMSContext.HR_Employee_Salary_Details.Where(t => t.HRES_Id == emp.HRES_Id && t.HRMED_Id == headId).ToList();
                                decimal? Amount = HRESD_Amount.Sum(t => t.HRESD_Amount);
                                //EmployeeContributionReportDTO employe = new EmployeeContributionReportDTO();
                                if (Amount > 0)
                                {
                                    employe = (from a in _HRMSContext.MasterEmployee
                                               from b in _HRMSContext.HR_Employee_Salary
                                               from d in _HRMSContext.HR_Master_EarningsDeductions
                                               from e in _HRMSContext.HR_Master_Department
                                               from f in _HRMSContext.HR_Master_Designation
                                               where (a.HRME_Id == b.HRME_Id &&
                                            b.MI_Id.Equals(dto.MI_Id) &&
                                            e.HRMD_Id == b.HRMD_Id &&
                                            f.HRMDES_Id == b.HRMDES_Id &&

                                            b.HRES_Id.Equals(emp.HRES_Id))
                                               select new EmployeeContributionReportDTO
                                               {
                                                   EmployeeCode = a.HRME_EmployeeCode,
                                                   HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                                   HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                                   HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                                   departmentName = e.HRMD_DepartmentName,
                                                   designationName = f.HRMDES_DesignationName,
                                                   selectedHeadAmount = Amount

                                               }).FirstOrDefault();
                                }
                            }
                            if (employe.EmployeeCode != null) { cumDTOList.Add(employe); }
                        }
                    }

                    dto.employeeDetails = cumDTOList.ToArray();

                    Institution institute = new Institution();
                    institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                    InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                    dto.institutionDetails = dmoObj;

                }
                //else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() == 0)
                //{
                    
                //    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id)).ToList();
                //}
                //else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() > 0)
                //{
                    
                //    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id)).ToList();
                //}
                //else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() > 0)
                //{
                //    //employee
                //    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id)).ToList();
                //}
                //else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() == 0)
                //{
                //    //employee
                //    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id)).ToList();
                //}
                //else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() == 0)
                //{
                //    //employee
                //    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.departmentselected.Contains(t.HRMD_Id)).ToList();
                //}

                //else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() > 0)
                //{
                //    //employee
                //    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id)).ToList();
                //}

               


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }


        public EmployeeContributionReportDTO get_depts(EmployeeContributionReportDTO data)
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



        public EmployeeContributionReportDTO get_desig(EmployeeContributionReportDTO data)
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
