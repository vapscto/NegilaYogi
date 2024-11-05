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
    public class ECRImpl : Interfaces.ECRInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public ECRImpl(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;
        }

        public ECRDTO getBasicData(ECRDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }

        public ECRDTO GetAllDropdownAndDatatableDetails(ECRDTO dto)
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
        public ECRDTO getEmployeedetailsBySelection(ECRDTO dto)
        {
            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
            try
            {
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
                    //dto.employeedropdown = employe.ToArray();

                    //  dto.employeedropdown = employeeDetails.ToArray();

                    //GroupTypelist
                    //GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id)) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    //dto.groupTypedropdownlist = GroupTypelist.ToArray();

                    ////Departmentlist
                    //Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    //dto.departmentdropdownlist = Departmentlist.ToArray();

                    //Designationlist
                    //Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    //dto.designationdropdownlist = Designationlist.ToArray();
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
        public ECRDTO get_depts(ECRDTO data)
        {

            //try
            //{
            //    data.departmentdropdown = (from a in _HRMSContext.MasterEmployee
            //                               from b in _HRMSContext.HR_Master_Department
            //                               where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMD_Id == b.HRMD_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMD_ActiveFlag == true)
            //                               select b).Distinct().ToArray();

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            return data;
        }
        public ECRDTO get_desig(ECRDTO data)
        {
            //try
            //{
            //    data.designationdropdown = (from a in _HRMSContext.MasterEmployee
            //                                from b in _HRMSContext.HR_Master_Designation
            //                                where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && data.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
            //                                select b).Distinct().ToArray();

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            return data;
        }
        public ECRDTO SaveData(ECRDTO data)
        {
            data.retrunMsg = "";
            try
            {
                HR_ECRDMO objHR_ECR = Mapper.Map<HR_ECRDMO>(data);

                var alldata = _HRMSContext.HR_ECRDMO.Where(t => t.MI_ID == data.MI_Id && t.Emp_code == data.Emp_code && t.Ecr_Arr_Epf_EE_Share == data.Ecr_Arr_Epf_EE_Share && t.ECR_EPF_Wages == data.ECR_EPF_Wages && t.Ecr_Arr_Epf_ER_Share == data.Ecr_Arr_Epf_ER_Share && t.Ecr_Eps_Wages == data.Ecr_Eps_Wages && t.Ecr_Arr_EPS == data.Ecr_Arr_EPS && t.ECr_Epf_Eps_Diff == data.ECr_Epf_Eps_Diff && t.Ecr_Epf_Eps_ReDif == data.Ecr_Epf_Eps_ReDif && t.Ecr_Epf_Contribution == data.Ecr_Epf_Contribution && t.ECR_GuardianName == data.ECR_GuardianName && t.Ecr_Guardian_Relation == data.Ecr_Guardian_Relation && t.Ecr_Ncp == data.Ecr_Ncp && t.Ecr_DOB == data.Ecr_DOB && t.Ecr_Adva_Ref == data.Ecr_Adva_Ref && t.Ecr_Gender == data.Ecr_Gender && t.Ecr_Join_DOEPF == data.Ecr_Join_DOEPF && t.ECR_Exit_DOEPF == data.ECR_Exit_DOEPF && t.ECR_Exit_DoEps == data.ECR_Exit_DoEps && t.Ecr_Leav_Reason == data.Ecr_Leav_Reason && t.Ecr_Epf_Cont_Remit == data.Ecr_Epf_Cont_Remit && t.Ecr_Arr_Epf == data.Ecr_Arr_Epf).Count();
                if (alldata == 0)
                {
                    if (objHR_ECR.Emp_code > 0)
                    {
                        var empaname = (from abc in _HRMSContext.MasterEmployee
                                          where abc.HRME_Id == data.Emp_code
                                          select new HR_ECRDMO
                                          {
                                              name = ((abc.HRME_EmployeeFirstName == null ? " " : abc.HRME_EmployeeFirstName) + (abc.HRME_EmployeeMiddleName == null ? " " : abc.HRME_EmployeeMiddleName) + (abc.HRME_EmployeeLastName == null ? " " : abc.HRME_EmployeeLastName)).Trim()
                                              
                    }).ToArray();


                        objHR_ECR.Emp_code = data.Emp_code;
                        objHR_ECR.name = empaname[0].name;
                        objHR_ECR.MI_ID = data.MI_Id;
                        objHR_ECR.Ecr_Arr_Epf_EE_Share = data.Ecr_Arr_Epf_EE_Share;
                        objHR_ECR.ECR_EPF_Wages = data.ECR_EPF_Wages;
                        objHR_ECR.Ecr_Arr_Epf_ER_Share = data.Ecr_Arr_Epf_ER_Share;
                        objHR_ECR.Ecr_Eps_Wages = data.Ecr_Eps_Wages;
                        objHR_ECR.Ecr_Arr_EPS = data.Ecr_Arr_EPS;
                        objHR_ECR.ECr_Epf_Eps_Diff = data.ECr_Epf_Eps_Diff;
                        objHR_ECR.Ecr_Epf_Eps_ReDif = data.Ecr_Epf_Eps_ReDif;
                        objHR_ECR.Ecr_Epf_Contribution = data.Ecr_Epf_Contribution;
                        objHR_ECR.ECR_GuardianName = data.ECR_GuardianName;
                        objHR_ECR.Ecr_Guardian_Relation = data.Ecr_Guardian_Relation;
                        objHR_ECR.Ecr_Ncp = data.Ecr_Ncp;
                        objHR_ECR.Ecr_DOB = data.Ecr_DOB;
                        objHR_ECR.Ecr_Adva_Ref = data.Ecr_Adva_Ref;
                        objHR_ECR.Ecr_Gender = data.Ecr_Gender;
                        objHR_ECR.Ecr_Join_DOEPF = data.Ecr_Join_DOEPF;
                        objHR_ECR.ECR_Exit_DOEPF = data.ECR_Exit_DOEPF;
                        objHR_ECR.ECR_Exit_DoEps = data.ECR_Exit_DoEps;
                        objHR_ECR.Ecr_Leav_Reason = data.Ecr_Leav_Reason;
                        objHR_ECR.Ecr_Epf_Cont_Remit = data.Ecr_Epf_Cont_Remit;
                        objHR_ECR.Ecr_Arr_Epf = data.Ecr_Arr_Epf;
                        _HRMSContext.Add(objHR_ECR);
                        var flag = _HRMSContext.SaveChanges();
                        if (flag == 1)
                        {
                            data.retrunMsg = "Add";
                        }
                        else
                        {
                            data.retrunMsg = "false";
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.retrunMsg = "Error occured";
            }
            return data;
        }
        public ECRDTO GetEmpDetails (ECRDTO data)
        {
            if (data.Emp_code > 0)
            {
                var employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(data.MI_Id) && t.HRME_Id == data.Emp_code).ToList();
                data.employeeDetails = employe.ToArray();

                var empGender = (from a in _HRMSContext.IVRM_Master_Gender
                                 from b in _HRMSContext.MasterEmployee
                                 where (a.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id && b.HRME_Id == data.Emp_code && a.IVRMMG_Id == b.IVRMMG_Id)
                                 select a.IVRMMG_GenderName).ToList();
                data.employeeGender = empGender.ToArray();
            }
            return data;
        }
    }
}