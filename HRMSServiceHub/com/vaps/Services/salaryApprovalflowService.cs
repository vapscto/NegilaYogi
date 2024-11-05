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
    public class salaryApprovalflowService : Interfaces.salaryApprovalflowInterface 
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public salaryApprovalflowService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public salaryApprovalFlowDTO getBasicData(salaryApprovalFlowDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }
        public salaryApprovalFlowDTO GetAllDropdownAndDatatableDetails(salaryApprovalFlowDTO dto)
        {
            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            List<HR_Master_CourseDMO> Courselist = new List<HR_Master_CourseDMO>();
            List<HR_Master_Grade> Gradelist = new List<HR_Master_Grade>();
            List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
            List<HR_Master_LeaveYearDMO> leaveyear = new List<HR_Master_LeaveYearDMO>();

            List<Month> Monthlist = new List<Month>();
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


                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id)  && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedropdown = employe.ToArray();

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
                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedropdown = employe.ToArray();

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



                    dto.headdropdown = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_ActiveFlag == true).ToArray();



                    //Earning list
                    dto.earningdropdown = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.HRMED_EarnDedFlag.Equals("Earning") && t.MI_Id.Equals(dto.MI_Id) && t.HRMED_ActiveFlag == true).ToArray();


                    //Deduction List
                    dto.detectiondropdown = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.HRMED_EarnDedFlag.Equals("Deduction") && t.MI_Id.Equals(dto.MI_Id) && t.HRMED_ActiveFlag == true).ToArray();
                Monthlist = _Context.month.Where(t => t.Is_Active == true).ToList();
                dto.monthdropdown = Monthlist.ToArray();

                //leave year
                leaveyear = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_ActiveFlag == true).ToList();
                dto.leaveyeardropdown = leaveyear.OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }


        
            public salaryApprovalFlowDTO FilterEmployeeData(salaryApprovalFlowDTO dto)
        {
            List<salaryUpdationDTO> cumDTOList = new List<salaryUpdationDTO>();
           

            List<MasterEmployee> employeeDetails = new List<MasterEmployee>();

            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
            // List<MasterEmployee> EmployeeList = new List<MasterEmployee>();
            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
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

                    employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedropdown = employeeDetails.ToArray();
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
                    employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedropdown = employeeDetails.ToArray();

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

                var allhead = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.headselected.Contains(t.HRMED_Id) && t.HRMED_ActiveFlag == true).ToList();
                if (allhead.Count() > 0)
                {


                    if (employeeDetails.Count() > 0)
                    {

                        employeeDetails = (from HRES in _HRMSContext.HR_Employee_EarningsDeductions
                                           from HRME in _HRMSContext.MasterEmployee

                                           where (HRES.HRME_Id== HRME.HRME_Id && HRME.HRME_ActiveFlag == true
                                           && HRES.MI_Id.Equals(dto.MI_Id)
                                           && employeeDetails.Contains(HRME) //checking condition
                                           )
                                           select HRME).Distinct().ToList();


                        foreach (MasterEmployee employee in employeeDetails)
                        {

                            salaryUpdationDTO cumDTO = new salaryUpdationDTO();

                            cumDTO.HRME_EmployeeFirstName = employee.HRME_EmployeeFirstName;
                            cumDTO.HRME_EmployeeMiddleName = employee.HRME_EmployeeMiddleName;
                            cumDTO.HRME_EmployeeLastName = employee.HRME_EmployeeLastName;
                            cumDTO.HRME_EmployeeCode = employee.HRME_EmployeeCode;

                            List<HR_Employee_EarningsDeductionsDTO> alldata = new List<HR_Employee_EarningsDeductionsDTO>();



                            foreach (var head in allhead)
                            {
                                HR_Employee_EarningsDeductionsDTO ss = new HR_Employee_EarningsDeductionsDTO();


                                ss = (from HREED in _HRMSContext.HR_Employee_EarningsDeductions
                                      from HRMED in _HRMSContext.HR_Master_EarningsDeductions
                                      where (HREED.HRMED_Id == HRMED.HRMED_Id
                                      && HREED.HRME_Id == employee.HRME_Id
                                        && HREED.MI_Id.Equals(dto.MI_Id)
                                        && HREED.HREED_ActiveFlag == true
                                      && HREED.HRMED_Id == head.HRMED_Id)//checking condition

                                      select new HR_Employee_EarningsDeductionsDTO
                                      {
                                          HRME_Id = employee.HRME_Id,
                                          MI_Id = dto.MI_Id,
                                          HREED_ActiveFlag = HREED.HREED_ActiveFlag,
                                          HREED_Id = HREED.HREED_Id,
                                          HRMED_Id = HRMED.HRMED_Id,
                                          HRMED_Name = HRMED.HRMED_Name,
                                          HREED_Amount = HREED.HREED_Amount,
                                          HREED_Percentage = HREED.HREED_Percentage,
                                          HRMED_EarnDedFlag = HRMED.HRMED_EarnDedFlag,
                                          HRMED_AmountPercentFlag=HRMED.HRMED_AmountPercentFlag
                                      }).FirstOrDefault();

                                if (ss != null)
                                {

                                    alldata.Add(ss);

                                }
                                else
                                {
                                    HR_Employee_EarningsDeductionsDTO ss1 = new HR_Employee_EarningsDeductionsDTO();

                                    ss1.HRME_Id = employee.HRME_Id;
                                    ss1.MI_Id = dto.MI_Id;

                                    ss1.HREED_Id = 0;
                                    ss1.HRMED_Id = head.HRMED_Id;
                                    ss1.HRMED_Name = head.HRMED_Name;
                                    ss1.HREED_Amount = 0;
                                    ss1.HREED_Percentage = "0";
                                    ss1.HRMED_EarnDedFlag = head.HRMED_EarnDedFlag;
                                    ss1.HRMED_AmountPercentFlag = head.HRMED_AmountPercentFlag;

                                    alldata.Add(ss1);

                                }

                            }
                           // cumDTO.earningresult = alldata.Where(t => t.HRMED_EarnDedFlag.Equals("Earning")).ToArray();
                            //cumDTO.deductionresult = alldata.Where(t => t.HRMED_EarnDedFlag.Equals("Deduction")).ToArray();

                           // cumDTOList.Add(cumDTO);
                        }

                        dto.employeeDetails = cumDTOList.ToArray();
                    }
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
        public salaryApprovalFlowDTO getEmployeedetailsBySelection(salaryApprovalFlowDTO dto)
        {
            int count = 0;
            try
            {

             
                if (dto.selectedEmpdetails.Count() > 0)
                {
                    for (int k = 0; k < dto.selectedEmpdetails.Count(); k++)
                    {
                        for (int j = 0; j < dto.selectedEmpdetails[k].earningresult.Count(); j++)
                        {
                            var hresdid = dto.selectedEmpdetails[k].earningresult[j].HRESD_Id;
                            var HRES_Id = dto.selectedEmpdetails[k].HRES_Id;

                            var checkresult = _HRMSContext.HR_Employee_Salary_Details.Where(t => t.HRESD_Id == dto.selectedEmpdetails[k].earningresult[j].HRESD_Id && t.HRES_Id == dto.selectedEmpdetails[k].HRES_Id);

                            if (checkresult.Count() > 0)
                            {
                                var result = _HRMSContext.HR_Employee_Salary_Details.Single(t => t.HRESD_Id == dto.selectedEmpdetails[k].earningresult[j].HRESD_Id && t.HRES_Id == dto.selectedEmpdetails[k].HRES_Id);
                                result.HRESD_Amount = dto.selectedEmpdetails[k].earningresult[j].HRESD_Amount;
                                _HRMSContext.Update(result);
                            }
                        }

                    


                    }
                    count = _HRMSContext.SaveChanges();
                    if (count > 0)
                    {
                        HR_Emp_Salary_Approval abc = new HR_Emp_Salary_Approval();
                        abc.IVRMUL_Id = dto.IVRMSTAUL_Id;
                        abc.HRES_Id = dto.HRES_Id;
                        abc.HRESA_SanctionLevel = 1;
                        abc.HRESA_Date = DateTime.Now;
                        abc.HRESA_ActiveFlag = true;
                        

                        dto.retrunMsg = "updated";
                    }
                    else
                    {
                        dto.retrunMsg = "false";
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                dto.retrunMsg = "false";
            }
            return dto;
        }


        public salaryApprovalFlowDTO get_depts(salaryApprovalFlowDTO data)
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



        public salaryApprovalFlowDTO get_desig(salaryApprovalFlowDTO data)
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
