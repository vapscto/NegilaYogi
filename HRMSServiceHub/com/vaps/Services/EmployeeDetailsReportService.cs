using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class EmployeeDetailsReportService : Interfaces.EmployeeDetailsReportInterface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public EmployeeDetailsReportService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public EmployeeReportsDTO getBasicData(EmployeeReportsDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }

        public EmployeeReportsDTO GetAllDropdownAndDatatableDetails(EmployeeReportsDTO dto)
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


           


            ////emptype
            dto.employeeTypedropdown = _HRMSContext.HR_Master_EmployeeType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMET_ActiveFlag == true).ToArray();
            }
            ////employee  
            //dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();

            //// employee grouptype
            //dto.groupTypedropdown = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToArray();

            ////departmentdropdown
            //dto.departmentdropdown = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToArray();

            ////designationdropdown 
            //dto.designationdropdown = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToArray();

            //  dto.headerdropdown= createHeaderArrayList();



            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public EmployeeReportsDTO FilterEmployeeData(EmployeeReportsDTO dto)
        {
            List<MasterEmployee> employeeDetails = new List<MasterEmployee>();
            try
            {


                if (dto.FormatType.Equals("Format1"))
                {

                    if (dto.DOBJL.Equals("DOB"))
                    {
                        employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && (t.HRME_DOB >= dto.FromDate && t.HRME_DOB <= dto.ToDate) && t.HRME_ActiveFlag == true).ToList();
                    }
                    else if (dto.DOBJL.Equals("DOJ"))
                    {
                        employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && (t.HRME_DOJ >= dto.FromDate && t.HRME_DOB <= dto.ToDate) && t.HRME_ActiveFlag == true).ToList();
                    }
                    else if (dto.DOBJL.Equals("DOL"))
                    {
                        employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && (t.HRME_DOL >= dto.FromDate && t.HRME_DOB <= dto.ToDate) && t.HRME_ActiveFlag == true).ToList();
                    }

                    if (dto.AllOrIndividual.Equals("Individual"))
                    {

                        if (dto.TypeOrEmployee.Equals("Type"))
                        {
                            employeeDetails = employeeDetails.Where(t => t.HRMET_Id.Equals(dto.HRMET_Id)).ToList();

                        }
                        else if (dto.TypeOrEmployee.Equals("Employee"))
                        {
                            employeeDetails = employeeDetails.Where(t => t.HRME_Id.Equals(dto.HRME_Id)).ToList();
                        }

                    }
                    else if (dto.AllOrIndividual.Equals("All"))
                    {

                    }



                    //Working , Left

                    if (dto.Left.Equals(true) && dto.Working.Equals(true))
                    {
                        dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).ToArray();
                    }
                    else if (dto.Left.Equals(true) && dto.Working.Equals(false))
                    {
                        dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_LeftFlag == true && t.HRME_ActiveFlag == true).ToArray();
                    }
                    else if (dto.Left.Equals(false) && dto.Working.Equals(true))
                    {
                        dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_LeftFlag != true && t.HRME_ActiveFlag == true).ToArray();
                    }
                    else if (dto.Left.Equals(false) && dto.Working.Equals(false))
                    {
                        dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).ToArray();
                    }
                }
                else
                {
                    dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).ToArray();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return dto;
        }


        public async Task<EmployeeReportsDTO> getEmployeedetailsBySelection(EmployeeReportsDTO dto)
        {
            List<MasterEmployee> employeeDetails = new List<MasterEmployee>();
            try
            {
                if (dto.FormatType.Equals("Format1"))
                {

                    if (dto.DOBJL.Equals("DOB"))
                    {
                        employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && (t.HRME_DOB >= dto.FromDate && t.HRME_DOB <= dto.ToDate) && t.HRME_ActiveFlag == true).ToList();
                    }
                    else if (dto.DOBJL.Equals("DOJ"))
                    {
                        employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && (t.HRME_DOJ >= dto.FromDate && t.HRME_DOJ <= dto.ToDate) && t.HRME_ActiveFlag == true).ToList();
                    }
                    else if (dto.DOBJL.Equals("DOL"))
                    {
                        employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && (t.HRME_DOL >= dto.FromDate && t.HRME_DOL <= dto.ToDate) && t.HRME_ActiveFlag == true).ToList();
                    }

                    if (dto.AllOrIndividual.Equals("Individual"))
                    {

                        if (dto.TypeOrEmployee.Equals("Type"))
                        {
                            employeeDetails = employeeDetails.Where(t => t.HRMET_Id.Equals(dto.HRMET_Id)).ToList();

                        }
                        else if (dto.TypeOrEmployee.Equals("Employee"))
                        {
                            employeeDetails = employeeDetails.Where(t => t.HRME_Id.Equals(dto.HRME_Id)).ToList();
                        }

                    }
                    else if (dto.AllOrIndividual.Equals("All"))
                    {

                    }

                }
                else if (dto.FormatType.Equals("Format2"))
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





                //Working , Left

                if (dto.Left.Equals(true) && dto.Working.Equals(true))
                {
                    employeeDetails = employeeDetails.ToList();
                }
                else if (dto.Left.Equals(true) && dto.Working.Equals(false))
                {
                    employeeDetails = employeeDetails.Where(t => t.HRME_LeftFlag == true).ToList();
                }
                else if (dto.Left.Equals(false) && dto.Working.Equals(true))
                {
                    employeeDetails = employeeDetails.Where(t => t.HRME_LeftFlag == false).ToList();
                }
                //else if (dto.Left.Equals(false) && dto.Working.Equals(false))
                //{
                //    employeeDetails = employeeDetails.Where(t => t.HRME_LeftFlag.Equals(null)).ToList();
                //}
                else if (dto.Left.Equals(false) && dto.Working.Equals(false))
                {
                    employeeDetails = employeeDetails.ToList();
                }
                if (employeeDetails.Count() > 0)
                {
                    var empIdList = employeeDetails.Select(t => t.HRME_Id);
                    dto.empIds = String.Join(",", empIdList);
                    //Column creation 
                    GetData(dto);
                    await GetEmployeeDetailsReoprt(dto);
                }

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

        public EmployeeReportsDTO GetData(EmployeeReportsDTO dto)
        {
            List<string> headId = new List<string>();
            string IVRM_CLM_coloumn = "";
            string name = "";

            try
            {
                for (int i = 0; i < dto.headerselected.Length; i++)
                {
                    string Id = dto.headerselected[i].columnID;
                    if (Id != null)
                    {
                        name = Id;
                        if (name.Equals("HRME_EmployeeFirstName"))
                        {
                            IVRM_CLM_coloumn += "(ISNULL(HRME_EmployeeFirstName, ' ') +' '+ ISNULL(HRME_EmployeeMiddleName, ' ') +' '+ ISNULL(HRME_EmployeeLastName, ' ')) as HRME_EmployeeFirstName,";
                        }
                        else if (name.Equals("HRME_PerStreet"))
                        {

                            IVRM_CLM_coloumn += "(ISNULL(HRME_PerStreet, ' ')+ ',' + ISNULL(HRME_PerArea, ' ') + ',' +ISNULL(HRME_PerCity, ' ')) as HRME_PerStreet,"; // + ',' + CAST(ISNULL(HRME_PerStateId, ' ') as varchar(max)) + ',' +CAST(ISNULL(HRME_PerCountryId, ' ') as varchar(max))
                        }

                        else if (name.Equals("HRME_LocStreet"))
                        {

                            IVRM_CLM_coloumn += "(ISNULL(HRME_LocStreet, ' ') +','+ISNULL(HRME_LocArea, ' ') + ',' + ISNULL(HRME_LocCity, ' ')) as HRME_LocStreet,"; // + ',' +CAST(ISNULL(HRME_LocStateId, ' ') as varchar(max)) + ',' + CAST(ISNULL(HRME_LocCountryId, ' ') as varchar(max))

                        }
                        else if (name.Equals("HRMET_Id"))
                        {
                            IVRM_CLM_coloumn += "(ISNULL(HRMET_EmployeeType, ' ')) as HRMET_Id,";
                        }

                        else if (name.Equals("HRMD_Id"))
                        {

                            IVRM_CLM_coloumn += "(ISNULL(HRMD_DepartmentName, ' ')) as HRMD_Id,";

                        }

                        else if (name.Equals("HRMGT_Id"))
                        {

                            IVRM_CLM_coloumn += "(ISNULL(HRMGT_EmployeeGroupType, ' ')) as HRMGT_Id,";

                        }

                        else if (name.Equals("HRMDES_Id"))
                        {

                            IVRM_CLM_coloumn += "(ISNULL(HRMDES_DesignationName, ' ')) as HRMDES_Id,";

                        }
                        else if (name.Equals("HRMG_Id"))
                        {

                            IVRM_CLM_coloumn += "(ISNULL(HRMG_GradeName, ' ')) as HRMG_Id,";

                        }
                        //added gautam
                        else if (name.Equals("IVRMMG_Id"))
                        {
                            IVRM_CLM_coloumn += "(ISNULL(IVRMMG_GenderName, ' ')) as IVRMMG_Id,";
                        }
                        else if (name.Equals("IVRMMMS_Id"))
                        {
                            IVRM_CLM_coloumn += "(ISNULL(IVRMMMS_MaritalStatus, ' ')) as IVRMMMS_Id,";
                        }
                        else if (name.Equals("ReligionId"))
                        {
                            IVRM_CLM_coloumn += "(ISNULL(IVRMMR_Name, ' ')) as ReligionId,";
                        }
                        else if (name.Equals("CasteId"))
                        {
                            IVRM_CLM_coloumn += "(ISNULL(IMC_CasteName, ' ')) as CasteId,";
                        }
                        else if (name.Equals("HRME_IdentificationMark"))
                        {
                            IVRM_CLM_coloumn += "ISNULL(HRME_IdentificationMark, ' ') as HRME_IdentificationMark,";
                        }
                        else if (name.Equals("HRME_SubjectsTaught"))
                        {
                            IVRM_CLM_coloumn += "ISNULL(HRME_SubjectsTaught, ' ') as HRME_SubjectsTaught,";
                        }
                        else if (name.Equals("HRMEB_AccountNo"))
                        {
                            IVRM_CLM_coloumn += "ISNULL(HRMEB_AccountNo, ' ') as HRMEB_AccountNo,";
                        }
                        else if (name.Equals("HREED_Amount"))
                        {
                            IVRM_CLM_coloumn += "CONVERT(varchar(10),HREED_Amount,105) as HREED_Amount,";
                        }

                        //added gautam
                        else if (name.Equals("HRME_DOB"))
                        {
                            //REPLACE(ISNULL(CONVERT(varchar(10), HRME_DOB, 105), ''), '', '01/01/1900') as HRME_DOB
                            IVRM_CLM_coloumn += "REPLACE(ISNULL(CONVERT(varchar(10), HRME_DOB, 105), ''), '', '01-01-1900') as HRME_DOB,";
                        }

                        else if (name.Equals("HRME_DOJ"))
                        {
                            IVRM_CLM_coloumn += "REPLACE(ISNULL(CONVERT(varchar(10), HRME_DOJ, 105), ''), '', '01-01-1900') as HRME_DOJ,";
                           // IVRM_CLM_coloumn += "REPLACE(ISNULL(CONVERT(DATE, HRME_DOJ), ''), '01-01-1900', '') as HRME_DOJ,";
                        }
                        else if (name.Equals("HRME_DOL"))
                        {
                            IVRM_CLM_coloumn += "REPLACE(ISNULL(CONVERT(varchar(10), HRME_DOL, 105), ''), '', '01-01-1900') as HRME_DOL,";
                            // IVRM_CLM_coloumn += "REPLACE(ISNULL(CONVERT(DATE, HRME_DOL), ''), '01-01-1900', '') as HRME_DOL,";
                        }

                        //else if (name.Equals("HRME_DOC"))
                        //{
                        //    IVRM_CLM_coloumn += "REPLACE(ISNULL(CONVERT(varchar(10), HRME_DOC, 105), ''), '', '01-01-1900') as HRME_DOC,";
                        //    // IVRM_CLM_coloumn += "REPLACE(ISNULL(CONVERT(DATE, HRME_DOL), ''), '01-01-1900', '') as HRME_DOL,";
                        //}


                        else if (name.Equals("HRME_ExpectedRetirementDate"))
                        {

                            IVRM_CLM_coloumn += "REPLACE(ISNULL(CONVERT(varchar(10), HRME_ExpectedRetirementDate,105), ''), '', '01-01-1900') as HRME_ExpectedRetirementDate,";
                        }
                        else if (name.Equals("HRME_EmailId"))
                        {

                            IVRM_CLM_coloumn += "(ISNULL(STUFF((SELECT ', ' + dbo.HR_Master_Employee_EmailId.HRMEM_EmailId FROM dbo.HR_Master_Employee_EmailId WHERE dbo.HR_Master_Employee_EmailId.HRME_Id = dbo.HR_Master_Employee.HRME_Id ORDER BY dbo.HR_Master_Employee_EmailId.HRMEM_EmailId FOR XML PATH('')), 1, 1, ''), ' ')) as HRME_EmailId,";

                        }

                        else if (name.Equals("HRME_MobileNo"))
                        {

                            IVRM_CLM_coloumn += "STUFF((SELECT ', ' + CAST(dbo.HR_Master_Employee_MobileNo.HRMEMNO_MobileNo AS VARCHAR(MAX)) FROM dbo.HR_Master_Employee_MobileNo WHERE dbo.HR_Master_Employee_MobileNo.HRME_Id = dbo.HR_Master_Employee.HRME_Id ORDER BY dbo.HR_Master_Employee_MobileNo.HRMEMNO_MobileNo FOR XML PATH('')), 1, 1, '') as HRME_MobileNo,";

                        }
                        else if (name.Equals("EmergencyContect"))
                        {

                            IVRM_CLM_coloumn += "STUFF((SELECT ', ' + CAST(dbo.HR_Master_Employee_MobileNo.HRMEMNO_MobileNo AS VARCHAR(MAX)) FROM dbo.HR_Master_Employee_MobileNo WHERE dbo.HR_Master_Employee_MobileNo.HRME_Id = dbo.HR_Master_Employee.HRME_Id and dbo.HR_Master_Employee_MobileNo.HRMEMNO_DeFaultFlag ='home'  ORDER BY dbo.HR_Master_Employee_MobileNo.HRMEMNO_MobileNo FOR XML PATH('')), 1, 1, '') as EmergencyContect,";

                        }
                        else if (name.Equals("DateofConfromation"))
                        {
                            IVRM_CLM_coloumn += "REPLACE(ISNULL(CONVERT(varchar(10), HRME_DOC, 105), ''), '', '01-01-1900') as DateofConfromation,";
                        }


                        else if (name.Equals("HRME_QualificationName"))
                        {

                            IVRM_CLM_coloumn += "STUFF((SELECT ', ' + CAST(dbo.HR_Master_Employee_Qualification.HRME_QualificationName AS VARCHAR(MAX)) FROM dbo.HR_Master_Employee_Qualification WHERE dbo.HR_Master_Employee.HRME_Id = dbo.HR_Master_Employee_Qualification.HRME_Id ORDER BY dbo.HR_Master_Employee_Qualification.HRMEQ_Id FOR XML PATH('')), 1, 1, '') as HRME_QualificationName,";

                        }
                        //else if (name.Equals("HRMEQ_CollegeName"))
                        // {
                        //     IVRM_CLM_coloumn += "STUFF((SELECT ', ' + CAST(dbo.HR_Master_Employee_Qualification.HRMEQ_CollegeName AS VARCHAR(MAX)) FROM dbo.HR_Master_Employee_Qualification WHERE dbo.HR_Master_Employee.HRME_Id = dbo.HR_Master_Employee_Qualification.HRME_Id ORDER BY dbo.HR_Master_Employee_Qualification.HRMEQ_CollegeName FOR XML PATH('')), 1, 1, '') as HRMEQ_CollegeName,";

                        // }
                        //else if (name.Equals("HRMC_QulaificationName"))
                        //{

                        //    string iddsss; iddsss = dto.empIds;
                        //    IVRM_CLM_coloumn += "stuff((select ', ' + isnull(C.HRMC_QulaificationName, '')  from HR_Master_Employee_Qualification Q INNER JOIN HR_Master_Course C ON  Q.HRMC_Id = C.HRMC_Id INNER jOIN HR_Master_Employee E ON E.MI_Id=C.MI_Id and Q.MI_Id=C.MI_Id  AND Q.HRME_Id=E.HRME_Id where Q.HRME_Id = iddsss and Q.MI_Id = E.MI_Id and Q.MI_Id=C.MI_Id and C.MI_Id=E.MI_Id  for xml path('')),1,1,'') as HRMC_QulaificationName,";


                        //}
                        //else if (name.Equals("HRMG_Id"))
                        //{

                        //  IVRM_CLM_coloumn += "(ISNULL(HRMG_GradeName, ' ')) as HRMG_Id,";

                        //}
                        //else if (name.Equals("HRMG_Id"))
                        //{

                        //        IVRM_CLM_coloumn += "(ISNULL(HRMG_GradeName, ' ')) as HRMG_Id,";

                        //}


                        else
                        {

                            if (IVRM_CLM_coloumn.Equals("") && dto.headerselected.Length == 1)
                            {
                                IVRM_CLM_coloumn = name;
                            }
                            else if (IVRM_CLM_coloumn.Equals("") && dto.headerselected.Length > 1)
                            {
                                IVRM_CLM_coloumn = name + "," + IVRM_CLM_coloumn;
                            }
                            else
                            {
                                IVRM_CLM_coloumn = name + "," + IVRM_CLM_coloumn;
                            }
                        }





                    }
                }
                string coloumns = "";
                if (IVRM_CLM_coloumn.EndsWith(","))
                {
                    coloumns = IVRM_CLM_coloumn.Remove(IVRM_CLM_coloumn.Length - 1);
                }
                else
                {
                    coloumns = IVRM_CLM_coloumn;
                }
                dto.coloumns = coloumns;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;

        }


        public async Task<EmployeeReportsDTO> GetEmployeeDetailsReoprt(EmployeeReportsDTO dto)
        {
            try
            {
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GetEmployeeDetailsReoprt";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@tableparam",SqlDbType.VarChar)
                    {
                        Value = dto.coloumns
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@empIds",SqlDbType.VarChar)
                    {
                        Value = dto.empIds
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        // var data = cmd.ExecuteNonQuery();

                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    var datatype = dataReader.GetFieldType(iFiled);
                                    if (datatype.Name == "DateTime")
                                    {
                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                        dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dateval  // use null instead of {}
                                    );
                                    }
                                    else
                                    {
                                        dataRow.Add(
                                      dataReader.GetName(iFiled),
                                      dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                    }
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.employeeDetailsfromDatabase = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }


            return dto;
        }

        public EmployeeReportsDTO get_depts(EmployeeReportsDTO data)
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



        public EmployeeReportsDTO get_desig(EmployeeReportsDTO data)
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