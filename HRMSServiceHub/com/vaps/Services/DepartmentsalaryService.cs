using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
namespace HRMSServicesHub.com.vaps.Services
{
    public class DepartmentsalaryService : Interfaces.Departmentsalaryinterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public DepartmentsalaryService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public HR_Department_SalaryDTO getBasicData(HR_Department_SalaryDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }


        public HR_Department_SalaryDTO GetAllDropdownAndDatatableDetails(HR_Department_SalaryDTO dto)
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

                //leave year
                leaveyear = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_ActiveFlag == true).ToList();
                dto.leaveyeardropdown = leaveyear.OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();



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






                HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                HR_ConfigurationDTO dmoObj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                dto.configurationDetails = dmoObj;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }


        public async Task<HR_Department_SalaryDTO> getEmployeedetailsBySelection(HR_Department_SalaryDTO dto)
         {
            //List<HR_Department_SalaryDTO> cumDTOList = new List<HR_Department_SalaryDTO>();
            //HR_Department_SalaryDTO cumDTO = new HR_Department_SalaryDTO();
            //List<HR_Employee_Salary> employeSalary = new List<HR_Employee_Salary>();
            List<CumulativeSalaryReportDTO> cumDTOList = new List<CumulativeSalaryReportDTO>();
            CumulativeSalaryReportDTO cumDTO = new CumulativeSalaryReportDTO();
            List<HR_Master_Department> employeSalary = new List<HR_Master_Department>();
            try
            {

                //  Inatitution Details

                long fmgg_id = 0;
                var HRMDD_IDS = "";

                foreach (var x in dto.hrmD_IdList)
                {
                    HRMDD_IDS += x + ",";
                }
                HRMDD_IDS = HRMDD_IDS.Substring(0, (HRMDD_IDS.Length - 1));
               

                Institution institute = new Institution();
                institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                dto.institutionDetails = dmoObj;


               

               employeSalary = (from a in _HRMSContext.HR_Master_Department
                                 from b in _HRMSContext.HR_Employee_Salary
                                 where (b.MI_Id.Equals(dto.MI_Id)) && dto.hrmD_IdList.Contains(a.HRMD_Id) && dto.hrmD_IdList.Contains(b.HRMD_Id)
                                 && b.HRES_Year.Equals(dto.HRES_Year) && b.HRES_Month.Equals(dto.HRES_Month) && dto.hrmdeS_IdList.Contains(b.HRMDES_Id) && dto.groupTypeIdList.Contains(b.HRMGT_Id) && a.HRMD_ActiveFlag ==true
                                 select a).Distinct().ToList();
                if (employeSalary.Count > 0)
                { 

                    if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                    {
                        //employee
                        employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id)  && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();

                    }
                    else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                    {
                        //employee
                      //  employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                    }
                    else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                    {
                        //employee
                      //  employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                    }
                    else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                    {
                        //employee
                     //   employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                    }
                    else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() == 0)
                    {
                        //employee
                     //   employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id)).ToList();
                    }
                    else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                    {
                        //employee
                       // employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                    }

                    else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                    {
                        //employee
                       // employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                    }

                    var hashSet = new HashSet<HR_Master_Department>(employeSalary);

                    for (int x = 0; x < hashSet.Count; x++)
                    {
                        var CurrentHRME_Id = hashSet.ElementAt(x);
                        Task tTemp = Task.Run(() =>
                        {

                            Double? Lopdays = 0;
                            decimal LopAmount = 0;


                            cumDTO = (from HRES in _HRMSContext.HR_Employee_Salary
                                     // from HRME in _HRMSContext.MasterEmployee
                                      from hrdes in _HRMSContext.HR_Master_Department
                                   
                                      where (
                                      
                                  hrdes.HRMD_Id==CurrentHRME_Id.HRMD_Id && HRES.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(HRES.HRMDES_Id)
                                      && HRES.HRES_Month.Equals(dto.HRES_Month) && HRES.HRES_Year.Equals(dto.HRES_Year)
                                  //checking condition
                                      )
                                      select new CumulativeSalaryReportDTO
                                      {
                                         HRES_Id = HRES.HRES_Id,
                                         HRMD_DepartmentName = hrdes.HRMD_DepartmentName,
                                          HRMD_Id = CurrentHRME_Id.HRMD_Id
                                      }).FirstOrDefault();





                            List<HR_Employee_Salary_DetailsDTO> alldata = new List<HR_Employee_Salary_DetailsDTO>();


                            if (cumDTO != null)
                            {
                        //Employee earning Deduction Details

                        var allhead = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_ActiveFlag == true).OrderBy(t => t.HRMED_Order).ToList();

                                if (allhead.Count() > 0)
                                {

                                    foreach (var head in allhead)
                                    {
                                        Task tTemp1 = Task.Run(() =>
                                        {
                                            HR_Employee_Salary_DetailsDTO ss = new HR_Employee_Salary_DetailsDTO();

                                            if (!head.HRMED_Name.Equals("") && head.HRMED_Name != null)
                                            {

                                                var currentdata = (from HRES in _HRMSContext.HR_Employee_Salary
                                                                   from HRESD in _HRMSContext.HR_Employee_Salary_Details
                                                                   from HRMED in _HRMSContext.HR_Master_EarningsDeductions
                                                                  // from dep in _HRMSContext.HR_Master_Department
                                                                   where (HRESD.HRES_Id == HRES.HRES_Id &&
                                                                            HRESD.HRMED_Id == HRMED.HRMED_Id 
                                                                         && HRES.HRMD_Id==CurrentHRME_Id.HRMD_Id &&
                                                                            HRESD.HRMED_Id == head.HRMED_Id &&
                                                                         //   && dto.hrmD_IdList.Contains(a.HRMD_Id)&&
                                                                              HRES.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(HRES.HRMDES_Id)
                                      && HRES.HRES_Month.Equals(dto.HRES_Month) && HRES.HRES_Year.Equals(dto.HRES_Year)
                                                                           //&& dep.HRMD_Id == HRES.HRMD_Id
                                                                            ) //checking condition
                                                           select new HR_Employee_Salary_DetailsDTO
                                                                   {

                                                                       HRESD_Id = HRESD.HRESD_Id,
                                                                       HRES_Id = HRES.HRES_Id,
                                                                       HRMED_Id = HRESD.HRMED_Id,
                                                                       HRMED_Name = HRMED.HRMED_Name,
                                                                       HRESD_Amount = HRESD.HRESD_Amount,
                                                                       HRMED_EarnDedFlag = HRMED.HRMED_EarnDedFlag
                                                                   }).ToList();

                                                if (currentdata.Count() > 0)
                                                {

                                                    decimal? HRESD_Amount = currentdata.Sum(t=>t.HRESD_Amount);

                                                    //ss.HRESD_Id = currentdata.FirstOrDefault().HRESD_Id;
                                                    ss.HRMED_Id = head.HRMED_Id;
                                                    ss.HRMED_Name = currentdata.FirstOrDefault().HRMED_Name;
                                                    ss.HRESD_Amount = Math.Round(Convert.ToDecimal(HRESD_Amount), 0);
                                                    ss.HRMED_EarnDedFlag = currentdata.FirstOrDefault().HRMED_EarnDedFlag;

                                                    alldata.Add(ss);

                                                }
                                                else
                                                {

                                                    ss.HRMED_Id = head.HRMED_Id;
                                                    ss.HRMED_Name = head.HRMED_Name;
                                                    ss.HRESD_Amount = 0;
                                                    ss.HRMED_EarnDedFlag = head.HRMED_EarnDedFlag;


                                                    alldata.Add(ss);

                                                }
                                            }

                                        });
                                        tTemp1.Wait();

                                    }


                                }

                                var earningresult = alldata.Where(t => t.HRMED_EarnDedFlag.Equals("Earning")).ToArray();
                                cumDTO.earningresult = earningresult;

                                cumDTO.grossEarning = 0;
                                foreach (var grsearn in cumDTO.earningresult)
                                {

                                    decimal? HRESD_Amount = 0;

                                    if (grsearn.HRESD_Amount != null)
                                    {
                                        HRESD_Amount = grsearn.HRESD_Amount;
                                    }
                                    else
                                    {
                                        HRESD_Amount = 0;
                                    }

                                    cumDTO.grossEarning = Math.Round(Convert.ToDecimal(cumDTO.grossEarning + HRESD_Amount), 0);


                                }
                                var deductionresult = alldata.Where(t => t.HRMED_EarnDedFlag.Equals("Deduction")).ToArray();
                                cumDTO.deductionresult = deductionresult;

                                cumDTO.grossDeduction = 0;
                                foreach (var grossDeduction in cumDTO.deductionresult)
                                {

                                    decimal? HRESD_Amount = 0;

                                    if (grossDeduction.HRESD_Amount != null)
                                    {
                                        HRESD_Amount = grossDeduction.HRESD_Amount;
                                    }
                                    else
                                    {
                                        HRESD_Amount = 0;
                                    }

                                    cumDTO.grossDeduction = Math.Round(Convert.ToDecimal(cumDTO.grossDeduction + HRESD_Amount), 0);
                                }



                                cumDTOList.Add(cumDTO);
                            }

                        });
                        tTemp.Wait();
                    }



                    dto.employeeSalaryslipDetails = cumDTOList.Distinct().ToArray();


                }
                else
                {
                    dto.employeeSalaryslipDetails = cumDTOList.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }






        public HR_Department_SalaryDTO get_depts(HR_Department_SalaryDTO data)
        { 
    
            try
            {
                //data.departmentdropdown = (from a in _HRMSContext.MasterEmployee
                //                           from b in _HRMSContext.HR_Master_Department
                //                           where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMD_Id == b.HRMD_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMD_ActiveFlag == true)
                //                           select b).Distinct().ToArray();

                data.departmentdropdown = (from a in _HRMSContext.HRGroupDeptDessgDMO
                                           from b in _HRMSContext.HR_Master_Department
                                           where (a.MI_Id == data.MI_Id && a.HRMD_Id == b.HRMD_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMD_ActiveFlag == true)
                                           select b).Distinct().ToArray();

                 

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public HR_Department_SalaryDTO get_desig(HR_Department_SalaryDTO data)
        {
            try
            {
                //data.designationdropdown = (from a in _HRMSContext.MasterEmployee
                //                            from b in _HRMSContext.HR_Master_Designation
                //                            where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && data.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                //                            select b).Distinct().ToArray();

                data.designationdropdown = (from a in _HRMSContext.HRGroupDeptDessgDMO
                                            from b in _HRMSContext.HR_Master_Designation
                                            where (a.MI_Id == data.MI_Id && a.HRMDES_Id == b.HRMDES_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && data.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
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
