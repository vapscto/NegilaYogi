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
    public class EmployeeStrengthReportService : Interfaces.EmployeeStrengthReportInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public EmployeeStrengthReportService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public EmployeeStrengthReportDTO getBasicData(EmployeeStrengthReportDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }
        public EmployeeStrengthReportDTO GetAllDropdownAndDatatableDetails(EmployeeStrengthReportDTO dto)
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




                //emptype
                //dto.employeeTypedropdown = _HRMSContext.HR_Master_EmployeeType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMET_ActiveFlag == true).ToArray();

                //// employee grouptype
                //dto.groupTypedropdown = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToArray();

                ////departmentdropdown
                //dto.departmentdropdown = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToArray();

                ////designationdropdown 
                //dto.designationdropdown = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public EmployeeStrengthReportDTO getEmployeedetailsBySelection(EmployeeStrengthReportDTO dto)
        {
            List<EmployeeStrengthReportDTO> empstrength = new List<EmployeeStrengthReportDTO>();
            List<MasterEmployee> employeeDetails = new List<MasterEmployee>();
            try
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



                if (dto.AllEmployee == false && dto.Departmentwise == false && dto.LeftEmployee == false)
                {
                    employeeDetails = employeeDetails.Where(t => t.HRME_DOJ >= dto.FromDate && t.HRME_DOJ <= dto.ToDate).ToList();
                }
                else if (dto.AllEmployee == false && dto.Departmentwise == true && dto.LeftEmployee == false)
                {
                    employeeDetails = employeeDetails.Where(t => t.HRME_DOJ >= dto.FromDate && t.HRME_DOJ <= dto.ToDate).ToList();
                }
                else if (dto.AllEmployee == false && dto.Departmentwise == true && dto.LeftEmployee == true)
                {
                    employeeDetails = employeeDetails.Where(t => t.HRME_DOL >= dto.FromDate && t.HRME_DOL <= dto.ToDate && t.HRME_LeftFlag == true).ToList();
                }
                else if (dto.AllEmployee == false && dto.Departmentwise == false && dto.LeftEmployee == true)
                {
                    employeeDetails = employeeDetails.Where(t => t.HRME_DOL >= dto.FromDate && t.HRME_DOL <= dto.ToDate && t.HRME_LeftFlag == true).ToList();
                }

                else if (dto.AllEmployee == true && dto.LeftEmployee == true)
                {
                    employeeDetails = employeeDetails.Where(t => t.HRME_LeftFlag == true).ToList();
                }



                if (employeeDetails.Count() > 0)
                {
                    List<long> empIds = employeeDetails.Select(c => c.HRME_Id).Distinct().ToList();

                    if (dto.Departmentwise == true || dto.LeftEmployee == true)
                    {
                        //Department wise
                        empstrength = (from a in _HRMSContext.MasterEmployee
                                       from b in _HRMSContext.HR_Master_GroupType
                                       from e in _HRMSContext.HR_Master_Department
                                       where (
                                                a.MI_Id.Equals(dto.MI_Id) &&
                                                b.HRMGT_Id == a.HRMGT_Id &&
                                                e.HRMD_Id == a.HRMD_Id &&
                                                empIds.Contains(a.HRME_Id) &&
                                                 a.HRME_ActiveFlag == true
                                             )
                                       group new { a, b, e } by new { b.HRMGT_EmployeeGroupType, e.HRMD_DepartmentName }
                                                                                            into g
                                       select new EmployeeStrengthReportDTO
                                       {
                                           grouptypeName = g.Key.HRMGT_EmployeeGroupType,
                                           departmentName = g.Key.HRMD_DepartmentName,
                                           totalEmployees = g.Count()
                                       }).ToList();

                        dto.totalWorkingEmployees = employeeDetails.Where(c => c.HRME_LeftFlag == false).Count();
                        dto.totalLeftEmployees = employeeDetails.Where(c => c.HRME_LeftFlag == true).Count();


                    }
                    else
                    { // by defalt designation wise
                        empstrength = (from a in _HRMSContext.MasterEmployee
                                       from b in _HRMSContext.HR_Master_GroupType
                                       from e in _HRMSContext.HR_Master_Department
                                       from f in _HRMSContext.HR_Master_Designation
                                       where (
                                                a.MI_Id.Equals(dto.MI_Id) &&
                                                b.HRMGT_Id == a.HRMGT_Id &&
                                                e.HRMD_Id == a.HRMD_Id &&
                                                f.HRMDES_Id == a.HRMDES_Id &&
                                               empIds.Contains(a.HRME_Id) &&
                                                 a.HRME_ActiveFlag == true
                                             )
                                       group new { a, b, e, f } by new { b.HRMGT_EmployeeGroupType, e.HRMD_DepartmentName, f.HRMDES_DesignationName }
                                                                    into g
                                       select new EmployeeStrengthReportDTO
                                       {
                                           grouptypeName = g.Key.HRMGT_EmployeeGroupType,
                                           departmentName = g.Key.HRMD_DepartmentName,
                                           designationName = g.Key.HRMDES_DesignationName,
                                           totalEmployees = g.Count()
                                       }).ToList();

                        dto.totalWorkingEmployees = employeeDetails.Where(c => c.HRME_LeftFlag == false).Count();
                        dto.totalLeftEmployees = employeeDetails.Where(c => c.HRME_LeftFlag == true).Count();
                    }



                }

                dto.employeeDetails = empstrength.ToArray();
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

        public EmployeeStrengthReportDTO get_depts(EmployeeStrengthReportDTO data)
        {
            try
            {
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



        public EmployeeStrengthReportDTO get_desig(EmployeeStrengthReportDTO data)
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