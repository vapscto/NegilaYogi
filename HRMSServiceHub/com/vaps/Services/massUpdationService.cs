using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class massUpdationService : Interfaces.massUpdationInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public massUpdationService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public massUpdationDTO getBasicData(massUpdationDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }
        public massUpdationDTO GetAllDropdownAndDatatableDetails(massUpdationDTO dto)
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

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdown = Designationlist.ToArray();

                }
                else
                {
                    dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();

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


                //employee  
              
                    // employee grouptype
                    //dto.groupTypedropdown = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToArray();

                    ////departmentdropdown
                    //dto.departmentdropdown = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToArray();

                    ////designationdropdown 
                    //dto.designationdropdown = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).ToArray();

                    //earning , deduction headdropdown
                    // earning , deduction details

                    dto.headdropdown = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_ActiveFlag == true).ToArray();


                    // earning , deduction details
                    dto.eardettypelist = _HRMSContext.HR_Master_EarningsDeductions_Type.ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }



        public massUpdationDTO FilterEmployeeData(massUpdationDTO dto)
        {
            List<MasterEmployee> employeeDetails = new List<MasterEmployee>();
            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
            // List<MasterEmployee> EmployeeList = new List<MasterEmployee>();
            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            try
            {
                if (dto.Type.Equals("Head"))
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

                        employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
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

                        if (employeeDetails.Count() > 0)
                        {

                            employeeDetails = (from a in _HRMSContext.MasterEmployee
                                               from b in _HRMSContext.HR_Employee_EarningsDeductions
                                               where a.HRME_Id.Equals(b.HRME_Id)
                                               && b.MI_Id.Equals(dto.MI_Id)
                                               && a.HRME_ActiveFlag == true && b.HREED_ActiveFlag == true

                                               && b.HRMED_Id == dto.HRMED_Id
                                               && b.MI_Id == dto.MI_Id

                                               orderby a.HRME_Id

                                               select a).Distinct().ToList();



                            dto.Type = "";
                        }

                        else
                        {


                            if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() > 0)
                            {
                                //employee
                                employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();
                            }
                            else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() == 0)
                            {
                                //employee
                                employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id) && t.HRME_ActiveFlag == true).ToList();
                            }
                            else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() > 0)
                            {
                                //employee
                                employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();
                            }
                            else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() > 0)
                            {
                                //employee
                                employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();
                            }
                            else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() == 0)
                            {
                                //employee
                                employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && t.HRME_ActiveFlag == true).ToList();
                            }
                            else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() == 0)
                            {
                                //employee
                                employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.departmentselected.Contains(t.HRMD_Id) && t.HRME_ActiveFlag == true).ToList();
                            }

                            else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() > 0)
                            {
                                //employee
                                employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();
                            }



                        }
                    }
                    else
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
                                employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();
                            }
                            else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() == 0)
                            {
                                //employee
                                employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id) && t.HRME_ActiveFlag == true).ToList();
                            }
                            else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() > 0)
                            {
                                //employee
                                employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();
                            }
                            else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() > 0)
                            {
                                //employee
                                employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();
                            }
                            else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() == 0)
                            {
                                //employee
                                employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && t.HRME_ActiveFlag == true).ToList();
                            }
                            else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() == 0)
                            {
                                //employee
                                employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.departmentselected.Contains(t.HRMD_Id) && t.HRME_ActiveFlag == true).ToList();
                            }

                            else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() > 0)
                            {
                                //employee
                                employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();
                            }


                        }





                        //if (employeeDetails.Count() > 0)
                        //{
                        //    var empIdList = employeeDetails.Select(t => t.HRME_Id);

                        //    var employeedropdown = (from a in _HRMSContext.MasterEmployee
                        //                            from b in _HRMSContext.HR_Employee_Salary
                        //                            where a.HRME_Id.Equals(b.HRME_Id)
                        //                            && b.MI_Id.Equals(dto.MI_Id)
                        //                            && a.HRME_ActiveFlag == true && empIdList.Contains(a.HRME_Id)

                        //                            orderby a.HRME_EmployeeOrder

                        //                            select a).Distinct();

                        //    dto.employeedropdown = employeedropdown.ToArray();

                        //}
                        dto.employeedropdown = employeeDetails.ToArray();

                    }
                }
                else
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
                            employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();
                        }
                        else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() == 0)
                        {
                            //employee
                            employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id) && t.HRME_ActiveFlag == true).ToList();
                        }
                        else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() > 0)
                        {
                            //employee
                            employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();
                        }
                        else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() > 0)
                        {
                            //employee
                            employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();
                        }
                        else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() == 0)
                        {
                            //employee
                            employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && t.HRME_ActiveFlag == true).ToList();
                        }
                        else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() == 0)
                        {
                            //employee
                            employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.departmentselected.Contains(t.HRMD_Id) && t.HRME_ActiveFlag == true).ToList();
                        }

                        else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() > 0)
                        {
                            //employee
                            employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();
                        }



                    }






                    //if (employeeDetails.Count() > 0)
                    //{
                    //    var empIdList = employeeDetails.Select(t => t.HRME_Id);

                    //    var employeedropdown = (from a in _HRMSContext.MasterEmployee
                    //                            from b in _HRMSContext.HR_Employee_Salary
                    //                            where a.HRME_Id.Equals(b.HRME_Id)
                    //                            && b.MI_Id.Equals(dto.MI_Id)
                    //                            && a.HRME_ActiveFlag == true && empIdList.Contains(a.HRME_Id)

                    //                            orderby a.HRME_EmployeeOrder

                    //                            select a).Distinct();

                    //    dto.employeedropdown = employeedropdown.ToArray();

                    //}
                    dto.employeedropdown = employeeDetails.ToArray();

                }
                dto.employeedropdown = employeeDetails.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
        public massUpdationDTO getEmployeedetailsBySelection(massUpdationDTO dto)
        {
            int countAdd = 0;
            int countRemove = 0;
            try
            {
                if (dto.employeeselected.Count() > 0)
                {
                    HR_Employee_EarningsDeductionsDTO dtoObj =new HR_Employee_EarningsDeductionsDTO();

                    if (dto.AmountPercentage.Equals("Amount"))
                    {
                        dtoObj.HREED_Percentage = "0.00";
                        dtoObj.HREED_Amount = dto.HREED_Amount;
                    }
                    else if (dto.AmountPercentage.Equals("Percentage"))
                    {
                        dtoObj.HREED_Percentage = dto.HREED_Percentage;
                        dtoObj.HREED_Amount = 0;
                    }
                  
                    dtoObj.MI_Id = dto.MI_Id;
                    dtoObj.HRMED_Id = dto.HRMED_Id;


                    foreach (var HRME_Id in dto.employeeselected)
                    {
                        dtoObj.HRME_Id = HRME_Id.Value;

                        if (dto.AddRemove.Equals("Add"))
                        {
                            var resultCount = _HRMSContext.HR_Employee_EarningsDeductions.Where(t => t.MI_Id == dtoObj.MI_Id && t.HRME_Id == dtoObj.HRME_Id && t.HRMED_Id == dtoObj.HRMED_Id).ToList();
                            if (resultCount.Count() > 0)
                            {
                                var result = _HRMSContext.HR_Employee_EarningsDeductions.Single(t => t.MI_Id == dtoObj.MI_Id && t.HRME_Id == dtoObj.HRME_Id && t.HRMED_Id == dtoObj.HRMED_Id);

                                if (result.HREED_ActiveFlag == false)
                                {
                                    dtoObj.HREED_ActiveFlag = result.HREED_ActiveFlag = true;
                                }
                                else
                                {
                                    dtoObj.HREED_ActiveFlag = result.HREED_ActiveFlag;
                                }
                                dtoObj.HREED_Id = result.HREED_Id;

                                Mapper.Map(dtoObj, result);
                                _HRMSContext.Update(result);
                                var flag = _HRMSContext.SaveChanges();
                                dtoObj.HREED_Id = 0;
                            }
                            else
                            {
                                HR_Employee_EarningsDeductions dmoObj = Mapper.Map<HR_Employee_EarningsDeductions>(dtoObj);
                                dmoObj.HREED_ActiveFlag = true;
                                _HRMSContext.Add(dmoObj);
                                var flag = _HRMSContext.SaveChanges();
                            }
                            countAdd++;

                        }
                        else if (dto.AddRemove.Equals("Remove"))
                        {
                            var result = _HRMSContext.HR_Employee_EarningsDeductions.Where(t => t.MI_Id == dtoObj.MI_Id && t.HRME_Id == dtoObj.HRME_Id && t.HRMED_Id == dtoObj.HRMED_Id).FirstOrDefault();

                            if (result != null)
                            {
                                if (result.HREED_ActiveFlag == true)
                                {
                                    result.HREED_ActiveFlag = false;
                                }
                                _HRMSContext.Update(result);
                                 _HRMSContext.SaveChanges();
                                countRemove++;
                            }
                        }

                        
                    }

                    if (countAdd > 0)
                    {
                        dto.retrunMsg = "Add";
                    }
                    else if (countRemove > 0)
                    {
                        dto.retrunMsg = "Remove";
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

        public massUpdationDTO get_depts(massUpdationDTO data)
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



        public massUpdationDTO get_desig(massUpdationDTO data)
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
