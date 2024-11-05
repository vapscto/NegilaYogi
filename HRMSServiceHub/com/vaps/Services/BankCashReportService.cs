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
    public class BankCashReportService : Interfaces.BankCashReportInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public BankCashReportService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public BankCashReportDTO getBasicData(BankCashReportDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }
        public BankCashReportDTO GetAllDropdownAndDatatableDetails(BankCashReportDTO dto)
        {
            List<MasterEmployee> EmployeeList = new List<MasterEmployee>();
            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
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

                //leave year
                dto.leaveyeardropdown = _HRMSContext.HR_MasterLeaveYear.Where(t => t.HRMLY_ActiveFlag == true && t.MI_Id.Equals(dto.MI_Id)).OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();
                dto.bankdropdown = _HRMSContext.HR_Master_BankDeatils.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMBD_ActiveFlag == true).ToArray();

                dto.monthdropdown = _Context.month.Where(t => t.Is_Active == true).ToArray();

                if (PROCESSList.Count() > 0)
                {

                    List<long> groupTypeIdList = PROCESSList.Select(t => t.HRMGT_Id).Distinct().ToList();
                    List<long> hrmD_IdList = PROCESSList.Select(t => t.HRMD_Id).Distinct().ToList();
                    List<long> hrmdeS_IdList = PROCESSList.Select(t => t.HRMDES_Id).Distinct().ToList();

                    //EmployeeList = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    //dto.employeedetailList = EmployeeList.ToArray();

                    ////GroupTypelist
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


                    //emptype
                    dto.employeeTypedropdown = _HRMSContext.HR_Master_EmployeeType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMET_ActiveFlag == true).ToArray();


                    // employee grouptype
                    dto.groupTypedropdown = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToArray();

                    //departmentdropdown
                    dto.departmentdropdown = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToArray();

                    //designationdropdown 
                    dto.designationdropdown = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToArray();
                }
                

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
        public async Task<BankCashReportDTO> getEmployeedetailsBySelection(BankCashReportDTO dto)
        {
            List<HR_Employee_Salary> employeeDetails = new List<HR_Employee_Salary>();

            List<BankCashReportDTO> bankCashReportDetails = new List<BankCashReportDTO>();
            try
            {

                //employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month) && t.HRES_BankCashFlag.Equals(dto.BankCash)).ToList();

                var eemployeeDetails = (from a in _HRMSContext.HR_Employee_Salary
                                        from b in _HRMSContext.MasterEmployee
                                        where (a.MI_Id == dto.MI_Id && a.HRES_Year == dto.HRES_Year && a.HRES_Month == dto.HRES_Month && a.HRES_BankCashFlag == dto.BankCash && a.HRME_Id == b.HRME_Id && b.HRME_ActiveFlag == true && dto.departmentselected.Contains(a.HRMD_Id) && dto.groupTypeselected.Contains(a.HRMGT_Id) && dto.designationselected.Contains(a.HRMDES_Id))
                                        select new BankCashReportDTO
                                        {
                                            HRME_EmployeeOrder = b.HRME_EmployeeOrder,
                                            HRME_Id = b.HRME_Id,
                                            HRES_Id = a.HRES_Id,
                                            HRES_Year = a.HRES_Year,
                                            HRES_Month = a.HRES_Month,

                                        }).Distinct().OrderBy(b => b.HRME_EmployeeOrder).ToList();



                dto.employeeDetails = eemployeeDetails.ToList();



                if (dto.employeeDetails.Count > 0)
                {

                    foreach (var emp in dto.employeeDetails)
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



                        // Total deduction
                        decimal? TotalDeduction = (from deduc in _HRMSContext.HR_Employee_Salary_Details
                                                   where (deduc.HRES_Id == emp.HRES_Id && empDeductionHead.Contains(deduc.HRMED_Id))
                                                   select deduc.HRESD_Amount).Sum();

                        TotalDeduction = TotalDeduction == null ? 0 : TotalDeduction;

                        // net salary
                        decimal? NetSalary = TotalEarning-TotalDeduction;

                        if (dto.BankCash == "Cash")
                        {
                            BankCashReportDTO bankCashReportDTO = new BankCashReportDTO();
                            bankCashReportDTO = (from a in _HRMSContext.MasterEmployee
                                                 from b in _HRMSContext.HR_Employee_Salary
                                                 from c in _HRMSContext.HR_Master_Department
                                                 //from d in _HRMSContext.HR_Master_BankDeatils
                                                 //from e in _HRMSContext.HR_Master_Employee_Bank
                                                 where (a.HRME_Id == b.HRME_Id &&
                                                 a.HRMD_Id == c.HRMD_Id
                                                 &&
                                                  b.MI_Id.Equals(dto.MI_Id) &&
                                                  b.HRES_BankCashFlag.Equals(dto.BankCash) &&
                                                  b.HRES_Id == emp.HRES_Id /*&& d.MI_Id.Equals(dto.MI_Id)*/)

                                                 select new BankCashReportDTO
                                                 {
                                                     HRME_EmployeeOrder = a.HRME_EmployeeOrder,
                                                     EmployeeCode = a.HRME_EmployeeCode,
                                                     EmployeeName = (a.HRME_EmployeeFirstName != null ? a.HRME_EmployeeFirstName : "") + " " + (a.HRME_EmployeeMiddleName != null ? a.HRME_EmployeeMiddleName : "") + " " + (a.HRME_EmployeeLastName != null ? a.HRME_EmployeeLastName : ""),
                                                     //BankAcNumber = e.HRMEB_AccountNo.ToString(),
                                                     NetSalary = NetSalary,
                                                     //companyacc = d.HRMBD_BankAccountNo
                                                 }).FirstOrDefault();

                            bankCashReportDetails.Add(bankCashReportDTO);
                        }
                        else if (dto.BankCash == "Bank")
                        {
                            BankCashReportDTO bankCashReportDTO = new BankCashReportDTO();
                            bankCashReportDTO = (from a in _HRMSContext.MasterEmployee
                                                 from b in _HRMSContext.HR_Employee_Salary
                                                 from c in _HRMSContext.HR_Master_Department
                                                 from d in _HRMSContext.HR_Master_BankDeatils
                                                 from e in _HRMSContext.HR_Master_Employee_Bank
                                                 from f in _HRMSContext.HR_Master_BankDeatils
                                                 where (a.HRME_Id == b.HRME_Id && a.HRMD_Id == c.HRMD_Id && b.MI_Id.Equals(dto.MI_Id) &&
                                                  b.HRES_BankCashFlag.Equals(dto.BankCash) && b.HRES_Id == emp.HRES_Id && d.MI_Id.Equals(dto.MI_Id) && e.HRMBD_Id == d.HRMBD_Id && e.HRME_Id == a.HRME_Id && e.HRMBD_Id == f.HRMBD_Id)
                                                 select new BankCashReportDTO
                                                 {
                                                     HRME_EmployeeOrder = a.HRME_EmployeeOrder,
                                                     EmployeeCode = a.HRME_EmployeeCode,
                                                     EmployeeName = (a.HRME_EmployeeFirstName != null ? a.HRME_EmployeeFirstName : "") + " " + (a.HRME_EmployeeMiddleName != null ? a.HRME_EmployeeMiddleName : "") + " " + (a.HRME_EmployeeLastName != null ? a.HRME_EmployeeLastName : ""),
                                                     BankAcNumber = e.HRMEB_AccountNo.ToString(),
                                                     NetSalary = NetSalary,
                                                     companyacc = d.HRMBD_BankAccountNo,
                                                     HRMBD_IFSCCode = f.HRMBD_IFSCCode,
                                                     HRMBD_BankName = f.HRMBD_BankName,
                                                     HRMBD_BranchName = f.HRMBD_BranchName
                                                 }).FirstOrDefault();

                            bankCashReportDetails.Add(bankCashReportDTO);
                            //dto.bankCashReportDTO = bankCashReportDetails.OrderBy(t => t.HRME_EmployeeOrder).ToArray();
                        }

                    }
                }
                // dto.employeeDetails = bankCashReportDetails.OrderBy(t => t.HRME_EmployeeOrder).ToArray();

                dto.employeeDetails = bankCashReportDetails;

                Institution institute = new Institution();
                institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                dto.institutionDetails = dmoObj;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public BankCashReportDTO get_depts(BankCashReportDTO data)
        {
            try
            {
                data.departmentdropdown = (from a in _HRMSContext.HRGroupDeptDessgDMO
                                           from b in _HRMSContext.HR_Master_Department
                                           where (a.MI_Id == data.MI_Id  && a.HRMD_Id == b.HRMD_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMD_ActiveFlag == true)
                                           select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public BankCashReportDTO get_desig(BankCashReportDTO data)
        {
            try
            {
                data.designationdropdown = (from a in _HRMSContext.HRGroupDeptDessgDMO
                                            from b in _HRMSContext.HR_Master_Designation
                                            where (a.MI_Id == data.MI_Id  && a.HRMDES_Id == b.HRMDES_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && data.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
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
