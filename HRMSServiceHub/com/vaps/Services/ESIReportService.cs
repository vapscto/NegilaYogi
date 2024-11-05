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
    public class ESIReportService : Interfaces.ESIReportInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public ESIReportService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public ESIReportDTO getBasicData(ESIReportDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }
        public ESIReportDTO GetAllDropdownAndDatatableDetails(ESIReportDTO dto)
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
        public async Task<ESIReportDTO> getEmployeedetailsBySelection(ESIReportDTO dto)
        {
            List<HR_Employee_Salary> employeeDetails = new List<HR_Employee_Salary>();

            List<ESIReportDTO> esiReportDetails = new List<ESIReportDTO>();
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
                    // configuration data for Employer contribution
                    var HrConfiguration = _HRMSContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).FirstOrDefault();

                    //Master Earning & deduction details for Basic pay Header ID
                    //var BasicheadId = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_ActiveFlag == true && t.HRMED_EDTypeFlag.Equals("Basic Pay")).FirstOrDefault().HRMED_Id;
                    List<long> BasicheadId = new List<long>();
                    var multi_BasicheadId = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                             from mas in _HRMSContext.HR_Master_EarningsDeductions
                                             where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && mas.HRMED_EarnDedFlag == "Earning" && mas.HRMED_EDTypeFlag != "Other Allowance" && mas.MI_Id == dto.MI_Id)
                                             select new CumulativeSalaryReportDTO
                                             {
                                                 HRMD_Id = emp.HRMED_Id
                                             }).ToList();

                    for (int icount = 0; icount < multi_BasicheadId.Count; icount++)
                    {
                        BasicheadId.Add((long)multi_BasicheadId[icount].HRMD_Id);
                    }

                    //Master Earning & deduction details for ESI Header ID
                    var ESIheadId = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_ActiveFlag == true && t.HRMED_EDTypeFlag.Equals("ESI")).FirstOrDefault().HRMED_Id;


                    decimal? ESIEmplrContPer = HrConfiguration.HRC_ESIEmplrCont;
                    //decimal? ESIEmplrContPer = HrConfiguration.HRC_ESIMax;

                    foreach (var emp in employeeDetails)
                    {
                        var salaryDetails = _HRMSContext.HR_Employee_Salary_Details.Where(t => t.HRES_Id.Equals(emp.HRES_Id)).ToList();

                        decimal? basicAmount = salaryDetails.Where(t => BasicheadId.Contains(t.HRMED_Id)).Sum(t => t.HRESD_Amount);

                        decimal? EmployeeContibution = salaryDetails.Where(t => t.HRMED_Id == ESIheadId).Sum(t => t.HRESD_Amount);
                        // decimal? EmployeeContibution = (HrConfiguration.HRC_ESIMax * basicAmount) / 100;

                        if (EmployeeContibution >= 0)
                        {
                            decimal? EmployerContibution = (ESIEmplrContPer * basicAmount) / 100;

                            decimal? totalAmount = EmployeeContibution + EmployerContibution;

                            ESIReportDTO esiReportDTO = new ESIReportDTO();
                            esiReportDTO = (from a in _HRMSContext.MasterEmployee
                                            from b in _HRMSContext.HR_Employee_Salary
                                            from c in _HRMSContext.HR_Configuration
                                            from d in _HRMSContext.HR_Employee_Salary_Details
                                            from e in _HRMSContext.HR_Master_EarningsDeductions

                                            where (a.HRME_Id == b.HRME_Id &&
                                             b.MI_Id.Equals(dto.MI_Id) &&
                                             b.HRES_Id == emp.HRES_Id && b.MI_Id == c.MI_Id &&
                                             b.HRES_Id == emp.HRES_Id && d.HRMED_Id == e.HRMED_Id && e.HRMED_EDTypeFlag == "ESI" && a.HRME_ESIApplicableFlag == true)

                                            select new ESIReportDTO
                                            {
                                                HRME_EmployeeCode = a.HRME_EmployeeCode,
                                                HRME_ESIAccNo = a.HRME_ESIAccNo,
                                                EmployeeName = (a.HRME_EmployeeFirstName != null ? a.HRME_EmployeeFirstName : "") + " " + (a.HRME_EmployeeMiddleName != null ? a.HRME_EmployeeMiddleName : "") + " " + (a.HRME_EmployeeLastName != null ? a.HRME_EmployeeLastName : ""),// a.HRME_EmployeeFirstName + " " + a.HRME_EmployeeMiddleName + " " + a.HRME_EmployeeLastName,
                                                HRES_WorkingDays = b.HRES_WorkingDays,
                                                basicAmount = basicAmount,
                                                EmployeeContibution = EmployeeContibution,
                                                EmployerContibution = EmployerContibution,
                                                totalAmount = totalAmount,
                                                HRME_DOL = a.HRME_DOL,
                                                HRC_ECodePrefix = c.HRC_ECodePrefix,
                                            }).FirstOrDefault();

                            esiReportDetails.Add(esiReportDTO);
                        }
                    }
                }

                dto.employeeDetails = esiReportDetails.ToArray();
                //.OrderBy(t => t.HRME_ESIAccNo)
                Institution institute = new Institution();
                institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                dto.institutionDetails = dmoObj;
                var HrConfigurations = _HRMSContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).FirstOrDefault();
                HR_Configuration hit = new HR_Configuration();
                hit = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                HR_ConfigurationDTO dmoObjcc = Mapper.Map<HR_ConfigurationDTO>(hit);
                dto.configurationDetails = dmoObjcc;

            }
            catch (Exception ex)
            {
                Console.WriteLine("ex");
            }
            return dto;
        }


        public ESIReportDTO get_depts(ESIReportDTO data)
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



        public ESIReportDTO get_desig(ESIReportDTO data)
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
