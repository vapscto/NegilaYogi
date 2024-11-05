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
    public class EmployeeOfferAndExperienceReportService : Interfaces.EmployeeOfferAndExperienceReportInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public EmployeeOfferAndExperienceReportService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public EmployeeOfferAndExperienceReportDTO getBasicData(EmployeeOfferAndExperienceReportDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }
        public EmployeeOfferAndExperienceReportDTO GetAllDropdownAndDatatableDetails(EmployeeOfferAndExperienceReportDTO dto)
        {
            List<HR_Master_Designation> designation = new List<HR_Master_Designation>();
            List<HR_Master_EmployeeType> emptype = new List<HR_Master_EmployeeType>();
            List<HR_Master_Department> department = new List<HR_Master_Department>();
            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
            List<MasterEmployee> EmployeeList = new List<MasterEmployee>();
            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            List<HR_Master_CourseDMO> Courselist = new List<HR_Master_CourseDMO>();
            List<HR_Master_Grade> Gradelist = new List<HR_Master_Grade>();



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

                    EmployeeList = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id)  && t.HRME_ActiveFlag == true && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedropdown = EmployeeList.ToArray();
                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id)) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.emptypedropdown = GroupTypelist.ToArray();

                    //Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdown = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdown = Designationlist.ToArray();

                }
                else
                {

                    EmployeeList = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedropdown = EmployeeList.ToArray();

                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.emptypedropdown = GroupTypelist.ToArray();

                    //Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdown = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdown = Designationlist.ToArray();


                }



                //designation = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                //dto.designationdropdown = designation.ToArray();
                ////
                //emptype = _HRMSContext.HR_Master_EmployeeType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMET_ActiveFlag == true).ToList();
                //dto.emptypedropdown = emptype.ToArray();
                ////
                //department = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                //dto.departmentdropdown = department.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }


        public EmployeeOfferAndExperienceReportDTO FilterEmployeeData(EmployeeOfferAndExperienceReportDTO dto)
        {
            List<MasterEmployee> EmployeeList = new List<MasterEmployee>();
            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
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

                    //EmployeeList = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    //dto.employeedropdown = EmployeeList.ToArray();

                    //dto.employeedropdown = (from a in _HRMSContext.MasterEmployee
                    //                            from b in _HRMSContext.HR_Master_Designation
                    //                            from c in _HRMSContext.HR_Master_Department
                    //                            where (a.MI_Id == dto.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && dto.hrmgT_IdList.Contains(a.HRMGT_Id) && dto.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(dto.MI_Id) && b.HRMDES_ActiveFlag == true) 
                    //                            select a).Distinct().ToArray();
                    dto.employeedropdown = (from a in _HRMSContext.MasterEmployee
                                            from b in _HRMSContext.HR_Master_Designation
                                            from c in _HRMSContext.HR_Master_Department
                                            where (a.MI_Id == dto.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && a.HRMGT_Id == dto.HRMGT_Id && a.HRMD_Id == dto.HRMD_Id && a.HRMDES_Id == dto.HRMDES_Id && b.MI_Id.Equals(dto.MI_Id) && b.HRMDES_ActiveFlag == true  && a.HRME_ActiveFlag == true)
                                            select new MasterEmployee
                                            {
                                                HRME_Id = a.HRME_Id,
                                                HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                                HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                                HRME_EmployeeLastName = a.HRME_EmployeeLastName
                                            }).Distinct().ToArray();



                }

                else
                {

                    // dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).ToArray();

                    dto.employeedropdown = (from a in _HRMSContext.MasterEmployee
                                            from b in _HRMSContext.HR_Master_Designation
                                            from c in _HRMSContext.HR_Master_Department
                                            where (a.MI_Id == dto.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && a.HRMGT_Id == dto.HRMGT_Id && a.HRMD_Id == dto.HRMD_Id && a.HRMDES_Id == dto.HRMDES_Id && b.MI_Id.Equals(dto.MI_Id) && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true)
                                            select new MasterEmployee
                                            {
                                                HRME_Id = a.HRME_Id,
                                                HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                                HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                                HRME_EmployeeLastName = a.HRME_EmployeeLastName
                                            }).Distinct().ToArray();

                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return dto;
        }
        public async Task<EmployeeOfferAndExperienceReportDTO> getEmployeedetailsBySelection(EmployeeOfferAndExperienceReportDTO dto)
        {
            DateTime HRMER_Current_Date = DateTime.Now;

            try
            {
                List<EmployeeOfferAndExperienceReportDTO> emp = new List<EmployeeOfferAndExperienceReportDTO>();
                MasterEmployee employe = _HRMSContext.MasterEmployee.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_Id.Equals(dto.HRME_Id) && t.HRME_ActiveFlag==true);
                var DesignationName = _HRMSContext.HR_Master_Designation.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_Id.Equals(employe.HRMDES_Id)).HRMDES_DesignationName;
                MasterEmployeeDTO employeObj = Mapper.Map<MasterEmployeeDTO>(employe);
                dto.currentemployeeDetails = employeObj;
                dto.DesignationName = DesignationName;
                dto.HRMER_Current_Date = HRMER_Current_Date;

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
    }
}
