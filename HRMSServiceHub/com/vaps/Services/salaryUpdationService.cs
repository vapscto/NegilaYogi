 using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class salaryUpdationService : Interfaces.salaryUpdationInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public salaryUpdationService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public salaryUpdationDTO getBasicData(salaryUpdationDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }

        public salaryUpdationDTO GetAllDropdownAndDatatableDetails(salaryUpdationDTO dto)
        {
            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            List<HR_Master_CourseDMO> Courselist = new List<HR_Master_CourseDMO>();
            List<HR_Master_Grade> Gradelist = new List<HR_Master_Grade>();
            List<MasterEmployee> employe = new List<MasterEmployee>();
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


                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
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




                //emptype
                //dto.employeeTypedropdown = _HRMSContext.HR_Master_EmployeeType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMET_ActiveFlag == true).ToArray();

                //    //employee  
                //    dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();

                //    // employee grouptype
                //    dto.groupTypedropdown = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToArray();

                //    //departmentdropdown
                //    dto.departmentdropdown = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToArray();

                //    //designationdropdown 
                //    dto.designationdropdown = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).ToArray();



                //earning , deduction headdropdown
                // earning , deduction details

                dto.headdropdown = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_ActiveFlag == true).ToArray();



                //Earning list
                dto.earningdropdown = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.HRMED_EarnDedFlag.Equals("Earning") && t.MI_Id.Equals(dto.MI_Id) && t.HRMED_ActiveFlag == true).ToArray();


                //Deduction List
                dto.detectiondropdown = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.HRMED_EarnDedFlag.Equals("Deduction") && t.MI_Id.Equals(dto.MI_Id) && t.HRMED_ActiveFlag == true).ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public salaryUpdationDTO FilterEmployeeData(salaryUpdationDTO dto)
        {
            List<salaryUpdationDTO> cumDTOList = new List<salaryUpdationDTO>();


            List<MasterEmployee> employeeDetails = new List<MasterEmployee>();

            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
            //List<MasterEmployee> EmployeeList = new List<MasterEmployee>();
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

                    employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id)) && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeeDetails = employeeDetails.ToArray();

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

                    if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() > 0)
                    {


                        //employee
                        employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToList();

                    }
                    else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() == 0)
                    {

                        //employee
                        employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id) && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToList();
                    }
                    else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() > 0)
                    {

                        //employee
                        employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToList();
                    }
                    else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() > 0)
                    {

                        //employee
                        employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToList();
                    }
                    else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() == 0)
                    {


                        //employee
                        employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToList();
                    }
                    else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() == 0)
                    {

                        //employee
                        employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.departmentselected.Contains(t.HRMD_Id) && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToList();
                    }

                    else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() > 0)
                    {

                        //employee
                        employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToList();
                    }
                }

                var allhead = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.headselected.Contains(t.HRMED_Id) && t.HRMED_ActiveFlag == true).ToList();
                if (allhead.Count() > 0)
                {


                    if (employeeDetails.Count() > 0)
                    {

                        employeeDetails = (from HRES in _HRMSContext.HR_Employee_EarningsDeductions
                                           from HRME in _HRMSContext.MasterEmployee

                                           where (HRES.HRME_Id == HRME.HRME_Id && HRME.HRME_ActiveFlag == true
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
                                          HRMED_AmountPercentFlag = HRMED.HRMED_AmountPercentFlag
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
                            cumDTO.earningresult = alldata.Where(t => t.HRMED_EarnDedFlag.Equals("Earning")).ToArray();
                            cumDTO.deductionresult = alldata.Where(t => t.HRMED_EarnDedFlag.Equals("Deduction")).ToArray();

                            cumDTOList.Add(cumDTO);
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

        public salaryUpdationDTO getEmployeedetailsBySelection(salaryUpdationDTO dto)
        {
            int count = 0;
            try
            {
              
                
                    //if (dto.selectedEmpdetails.Count() > 0)
                    //{
                    //    for (int k = 0; k < dto.selectedEmpdetails.Count(); k++)
                    //    {
                    //        for (int j = 0; j < dto.selectedEmpdetails[k].earningresult.Count(); j++)
                    //        {
                    //            var hresdid = dto.selectedEmpdetails[k].earningresult[j].HRESD_Id;
                    //            var HRES_Id = dto.selectedEmpdetails[k].HRES_Id;

                    //            var checkresult = _HRMSContext.HR_Employee_Salary_Details.Where(t => t.HRESD_Id == dto.selectedEmpdetails[k].earningresult[j].HRESD_Id && t.HRES_Id == dto.selectedEmpdetails[k].HRES_Id);

                    //            if (checkresult.Count() > 0)
                    //            {
                    //                var result = _HRMSContext.HR_Employee_Salary_Details.Single(t => t.HRESD_Id == dto.selectedEmpdetails[k].earningresult[j].HRESD_Id && t.HRES_Id == dto.selectedEmpdetails[k].HRES_Id);
                    //                result.HRESD_Amount = dto.selectedEmpdetails[k].earningresult[j].HRESD_Amount;
                    //                _HRMSContext.Update(result);

                    //                var resultss = _HRMSContext.HR_Employee_Salary.Single(t => t.HRES_Id == dto.selectedEmpdetails[k].HRES_Id);
                    //                resultss.HRES_ApproveFlg = true;
                    //                _HRMSContext.Update(resultss);
                    //            }
                    //        }

                    //        HR_Emp_Salary_Approval abc = new HR_Emp_Salary_Approval();
                    //        abc.IVRMUL_Id = dto.IVRMSTAUL_Id;
                    //        abc.HRES_Id = dto.selectedEmpdetails[k].HRES_Id;
                    //        abc.HRESA_SanctionLevel = 1;
                    //        abc.HRESA_Date = DateTime.Now;
                    //        abc.HRESA_ActiveFlag = true;
                    //        abc.UpdatedDate = DateTime.Now;
                    //        abc.CreatedDate = DateTime.Now;
                    //        _HRMSContext.Add(abc);
                    //        var flag = _HRMSContext.SaveChanges();
                    //        if (flag > 1)
                    //        {
                    //            dto.retrunMsg = "Add";
                    //        }
                    //        else
                    //        {
                    //            dto.retrunMsg = "False";
                    //        }

                    //    }
                    //    count = _HRMSContext.SaveChanges();
                    //    dto.retrunMsg = "updated";
                    //}


                foreach (salaryUpdationDTO item in dto.selectedEmpdetails)
                {
                    foreach (HR_Employee_EarningsDeductionsDTO earningDTO in item.earningresult)
                    {
                        if (earningDTO.HREED_Id != 0)
                        {
                            var result = _HRMSContext.HR_Employee_EarningsDeductions.Single(t => t.HREED_Id == earningDTO.HREED_Id);
                            Mapper.Map(earningDTO, result);
                            _HRMSContext.Update(result);
                        }
                    }

                    foreach (HR_Employee_EarningsDeductionsDTO deductionDTO in item.deductionresult)
                    {
                        if (deductionDTO.HREED_Id != 0)
                        {
                            var result = _HRMSContext.HR_Employee_EarningsDeductions.Single(t => t.HREED_Id == deductionDTO.HREED_Id);
                            Mapper.Map(deductionDTO, result);
                            _HRMSContext.Update(result);
                        }
                    }
                    count = _HRMSContext.SaveChanges();
                }
                if (count > 0)
                {
                    dto.retrunMsg = "updated";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                dto.retrunMsg = "false";
            }
            return dto;
        }

        public salaryUpdationDTO get_depts(salaryUpdationDTO data)
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

        public salaryUpdationDTO get_desig(salaryUpdationDTO data)
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
