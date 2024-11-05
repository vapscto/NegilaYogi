using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;

namespace HRMSServicesHub.com.vaps.Services
{
    public class Form16Service : Interfaces.Form16Interface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public Form16Service(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }
        public Form16DTO getBasicData(Form16DTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }


        public Form16DTO GetAllDropdownAndDatatableDetails(Form16DTO dto)
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
                //employee

             

                //leave year
              //  leaveyear = _HRMSContext.IVRM_Master_FinancialYear.ToList();
                dto.leaveyeardropdown = _HRMSContext.IVRM_Master_FinancialYear.OrderBy(t=>t.IMFY_OrderBy).ToArray();
             
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


                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedropdown = employe.ToArray();

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

                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id)).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedropdown = employe.ToArray();


                }




            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public Form16DTO GetEmployeeDetailsByLeaveYearAndMonth(Form16DTO dto)
        {
           

            return dto;
        }



        public async Task<Form16DTO> GenerateEmployeeSalarySlip(Form16DTO dto)
        {
            try
            {

                dto.HRC_EducationCess = _HRMSContext.HR_Configuration.Where(t => t.MI_Id == dto.MI_Id).Select(t => t.HRC_EducationCess).FirstOrDefault();
                Institution institute = new Institution();
                institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                dto.institutionDetails = dmoObj;

                MasterEmployee employe = _HRMSContext.MasterEmployee.Single(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_Id.Equals(dto.HRME_Id));

                var DepartmentName = _HRMSContext.HR_Master_Department.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_Id.Equals(employe.HRMD_Id)).HRMD_DepartmentName;
                var DesignationName = _HRMSContext.HR_Master_Designation.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_Id.Equals(employe.HRMDES_Id)).HRMDES_DesignationName;

                //Employee Basic Details
                MasterEmployeeDTO employeObj = Mapper.Map<MasterEmployeeDTO>(employe);
                dto.currentemployeeDetails = employeObj;
                dto.birthyear = DateTime.Now.Year - dto.currentemployeeDetails.HRME_DOB.Value.Year;
                //hrmE_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),

                dto.DesignationName = DesignationName;
                dto.DepartmentName = DepartmentName;


                var quartername = _HRMSContext.HR_Master_quarter.Where(t =>t.MI_Id.Equals(dto.MI_Id) && t.HRMQ_ActiveFlg==true).ToArray();
                dto.quarterheads = quartername;

                var TDSname = _HRMSContext.HR_Master_TDS.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRETDS_ActiveFlg == true && t.HRME_Id==dto.HRME_Id && t.IMFY_Id==dto.IMFY_Id).ToArray();   
                dto.tdsheads = TDSname;

                //
                var receitno = (from ao in _HRMSContext.HR_Employee_TDS_Quarter
                                from pa in _HRMSContext.HR_Master_quarter
                                where ao.HRMQ_Id == pa.HRMQ_Id && ao.HRME_Id == dto.HRME_Id && ao.MI_Id.Equals(dto.MI_Id) && ao.HRETDSQ_ActiveFlg == true && pa.HRMQ_ActiveFlg == true && ao.IMFY_Id == dto.IMFY_Id && ao.MI_Id==pa.MI_Id
                                select new Form16DTO
                                {
                                    HRETDSR_ReceiptNo = ao.HRETDSR_ReceiptNo,

                                    HRMQ_QuarterName = pa.HRMQ_QuarterName,
                                    HRMQ_FromDay=pa.HRMQ_FromDay,
                                    HRMQ_ToDay=pa.HRMQ_ToDay,




                                 }).Distinct().ToList();

                dto.receit = receitno.ToArray();



                //var receitno = _HRMSContext.HR_Employee_TDS_Quarter.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRETDSQ_ActiveFlg == true && t.HRME_Id == dto.HRME_Id && t.IMFY_Id == dto.IMFY_Id).Select(t => t.HRETDSR_ReceiptNo);
                ////select new Form16DTO
                ////{
                ////    // HRMAL_AllowanceName = ao.,

                ////    HRETDSR_ReceiptNo = pa.HRETDSR_ReceiptNo




                ////}).Distinct().ToList();

                //dto.receit = receitno.ToArray();




                var allowed = _HRMSContext.HR_Master_Allowance.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMAL_ActiveFlg == true).ToArray();
                dto.masterallowance = allowed;

                var allowance = (from ao in _HRMSContext.HR_Master_Allowance
                                 from pa in _HRMSContext.HR_Master_Emp_Allowance
                                 where ao.HRMAL_Id == pa.HRMAL_Id && pa.HRME_Id == dto.HRME_Id && ao.MI_Id.Equals(dto.MI_Id) && ao.HRMAL_ActiveFlg==true && pa.HREAL_ActiveFlg==true && pa.IMFY_Id==dto.IMFY_Id
                                   select new Form16DTO
                                   {
                                       HRMAL_AllowanceName = ao.HRMAL_AllowanceName,

                                       HREAL_Allowance = pa.HREAL_Allowance




                                   }).Distinct().ToList();

                   dto.allowancelist = allowance.ToArray();

                var otherincomevalues = (from ao in _HRMSContext.HR_Master_OtherIncome
                                 from pa in _HRMSContext.HR_Master_Other_Income
                                 where ao.HRMOI_Id == pa.HRMOI_Id && pa.HRME_Id == dto.HRME_Id && ao.HRMOI_ActiveFlg==true  &&pa.HREOI_ActiveFlg==true && ao.MI_Id.Equals(dto.MI_Id) &&  pa.IMFY_Id == dto.IMFY_Id
                                         select new Form16DTO
                                 {
                                     HRMOI_OtherIncomeName = ao.HRMOI_OtherIncomeName,

                                     HREOI_OtherIncome = pa.HREOI_OtherIncome




                                 }).Distinct().ToList();

                dto.otherincomelist = otherincomevalues.ToArray();


                var empearnDed = (from emp in _HRMSContext.HR_Employee_Salary
                                  from hh in _HRMSContext.HR_Employee_Salary_Details
                                  from mas in _HRMSContext.HR_Master_EarningsDeductions
                                  where (hh.HRMED_Id == mas.HRMED_Id && mas.HRMED_ActiveFlag == true && emp.HRME_Id == dto.HRME_Id && mas.HRMED_EarnDedFlag == "Earning" && mas.MI_Id == dto.MI_Id && emp.HRES_FromDate >=dto.IMFY_FromDate && emp.HRES_ToDate<= dto.IMFY_ToDate && emp.HRES_Id==hh.HRES_Id)
                                  select new Form16DTO
                                  {
                                      HRESD_Amount = hh.HRESD_Amount

                                  }).ToList();

                dto.empGrossSal = Convert.ToDecimal(empearnDed.Sum(t => t.HRESD_Amount));

                var professionltax = (from emp in _HRMSContext.HR_Employee_Salary
                                      from hh in _HRMSContext.HR_Employee_Salary_Details
                                  from mas in _HRMSContext.HR_Master_EarningsDeductions
                                  where (hh.HRMED_Id == mas.HRMED_Id && emp.HRME_Id == dto.HRME_Id && mas.HRMED_EarnDedFlag == "Deduction" && mas.MI_Id == dto.MI_Id &&  mas.HRMED_EDTypeFlag == "PT" && emp.HRES_Id==hh.HRES_Id && emp.HRES_FromDate>=dto.IMFY_FromDate && emp.HRES_ToDate <= dto.IMFY_ToDate)
                                  select new Form16DTO
                                  {
                                  
                                      HRESD_Amount = hh.HRESD_Amount

                                  }).ToList();

                dto.professionaltaxamount = Convert.ToDecimal(professionltax.Sum(t => t.HRESD_Amount));



                //pf

                var pfvalue = (from emp in _HRMSContext.HR_Employee_Salary
                                      from hh in _HRMSContext.HR_Employee_Salary_Details
                                      from mas in _HRMSContext.HR_Master_EarningsDeductions
                                      where (hh.HRMED_Id == mas.HRMED_Id && emp.HRME_Id == dto.HRME_Id && mas.HRMED_EarnDedFlag == "Deduction" && mas.MI_Id == dto.MI_Id && mas.HRMED_EDTypeFlag == "PF" && emp.HRES_Id == hh.HRES_Id && emp.HRES_FromDate >= dto.IMFY_FromDate && emp.HRES_ToDate <= dto.IMFY_ToDate)
                                      select new Form16DTO
                                      {

                                          HRESD_Amount = hh.HRESD_Amount

                                      }).ToList();

                dto.pfvalue = Convert.ToDecimal(pfvalue.Sum(t => t.HRESD_Amount));


                var LICvalue = (from emp in _HRMSContext.HR_Employee_Salary
                               from hh in _HRMSContext.HR_Employee_Salary_Details
                               from mas in _HRMSContext.HR_Master_EarningsDeductions
                               where (hh.HRMED_Id == mas.HRMED_Id && emp.HRME_Id == dto.HRME_Id && mas.HRMED_EarnDedFlag == "Deduction" && mas.MI_Id == dto.MI_Id && mas.HRMED_Name == "LIC" && emp.HRES_Id == hh.HRES_Id && emp.HRES_FromDate >= dto.IMFY_FromDate && emp.HRES_ToDate <= dto.IMFY_ToDate)
                               select new Form16DTO
                               {

                                   HRESD_Amount = hh.HRESD_Amount

                               }).ToList();

                dto.licvalue = Convert.ToDecimal(LICvalue.Sum(t => t.HRESD_Amount));






                var chapter = (from ao in _HRMSContext.HR_master_ChapterVI
                                         from pa in _HRMSContext.HR_Employee_ChapterVI 
                                         where ao.HRMCVIA_Id == pa.HRMCVIA_Id && pa.HRME_Id == dto.HRME_Id && ao.MI_Id.Equals(dto.MI_Id) && ao.HRMCVIA_SectionCode=="80C" && ao.HRMCVIA_ActiveFlg==true && pa.HRECVIA_ActiveFlg==true &&  pa.IMFY_Id == dto.IMFY_Id
                               select new Form16DTO 
                                         {
                                             HRMCVIA_SectionName = ao.HRMCVIA_SectionName,

                                             HRECVIA_Amount = pa.HRECVIA_Amount,
                                             HRMCVIA_ORDER=ao.HRMCVIA_ORDER,
                                             HRMCIA_ROMANORDER=ao.HRMCIA_ROMANORDER

                               }).Distinct().ToList();

                dto.chapterlist = chapter.ToArray();

                var masterchapter=_HRMSContext.HR_master_ChapterVI.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMCVIA_ActiveFlg == true &&  t.HRMCVIA_SectionCode == "80C").ToArray();
                dto.masterch = masterchapter;

                var chapter80E = (from ao in _HRMSContext.HR_master_ChapterVI
                               from pa in _HRMSContext.HR_Employee_ChapterVI
                               where ao.HRMCVIA_Id == pa.HRMCVIA_Id && pa.HRME_Id == dto.HRME_Id && ao.MI_Id.Equals(dto.MI_Id) && ao.HRMCVIA_SectionCode == "80E" && ao.HRMCVIA_ActiveFlg == true && pa.HRECVIA_ActiveFlg == true && pa.IMFY_Id == dto.IMFY_Id
                                  select new Form16DTO
                               {
                                   HRMCVIA_SectionName = ao.HRMCVIA_SectionName,

                                   HRECVIA_Amount = pa.HRECVIA_Amount

                               }).Distinct().ToList();

                dto.chapterlist80E = chapter80E.ToArray();


                var chapter80EE = (from ao in _HRMSContext.HR_master_ChapterVI
                                  from pa in _HRMSContext.HR_Employee_ChapterVI
                                  where ao.HRMCVIA_Id == pa.HRMCVIA_Id && pa.HRME_Id == dto.HRME_Id && ao.MI_Id.Equals(dto.MI_Id) && ao.HRMCVIA_SectionCode == "80EE" && ao.HRMCVIA_ActiveFlg == true && pa.HRECVIA_ActiveFlg == true && pa.IMFY_Id == dto.IMFY_Id
                                   select new Form16DTO
                                  {
                                      HRMCVIA_SectionName = ao.HRMCVIA_SectionName,

                                      HRECVIA_Amount = pa.HRECVIA_Amount

                                  }).Distinct().ToList();

                dto.chapterlist80EE = chapter80EE.ToArray();


                var chapter80G = (from ao in _HRMSContext.HR_master_ChapterVI
                                   from pa in _HRMSContext.HR_Employee_ChapterVI
                                   where ao.HRMCVIA_Id == pa.HRMCVIA_Id && pa.HRME_Id == dto.HRME_Id && ao.MI_Id.Equals(dto.MI_Id) && ao.HRMCVIA_SectionCode == "80G" && ao.HRMCVIA_ActiveFlg == true && pa.HRECVIA_ActiveFlg == true && pa.IMFY_Id == dto.IMFY_Id
                                  select new Form16DTO
                                   {
                                       HRMCVIA_SectionName = ao.HRMCVIA_SectionName,

                                       HRECVIA_Amount = pa.HRECVIA_Amount

                                   }).Distinct().ToList();

                dto.chapterlist80G = chapter80G.ToArray();





                var chapter80GG = (from ao in _HRMSContext.HR_master_ChapterVI
                                  from pa in _HRMSContext.HR_Employee_ChapterVI
                                  where ao.HRMCVIA_Id == pa.HRMCVIA_Id && pa.HRME_Id == dto.HRME_Id && ao.MI_Id.Equals(dto.MI_Id) && ao.HRMCVIA_SectionCode == "80GG" && ao.HRMCVIA_ActiveFlg == true && pa.HRECVIA_ActiveFlg == true && pa.IMFY_Id == dto.IMFY_Id
                                   select new Form16DTO
                                  {
                                      HRMCVIA_SectionName = ao.HRMCVIA_SectionName,

                                      HRECVIA_Amount = pa.HRECVIA_Amount

                                  }).Distinct().ToList();

                dto.chapterlist80GG = chapter80GG.ToArray();



                var chapter80U = (from ao in _HRMSContext.HR_master_ChapterVI
                                   from pa in _HRMSContext.HR_Employee_ChapterVI
                                   where ao.HRMCVIA_Id == pa.HRMCVIA_Id && pa.HRME_Id == dto.HRME_Id && ao.MI_Id.Equals(dto.MI_Id) && ao.HRMCVIA_SectionCode == "80U" && ao.HRMCVIA_ActiveFlg == true && pa.HRECVIA_ActiveFlg == true && pa.IMFY_Id == dto.IMFY_Id
                                  select new Form16DTO
                                   {
                                       HRMCVIA_SectionName = ao.HRMCVIA_SectionName,

                                       HRECVIA_Amount = pa.HRECVIA_Amount

                                   }).Distinct().ToList();

                dto.chapterlist80U = chapter80U.ToArray();



                var chapter80DD = (from ao in _HRMSContext.HR_master_ChapterVI
                                  from pa in _HRMSContext.HR_Employee_ChapterVI
                                  where ao.HRMCVIA_Id == pa.HRMCVIA_Id && pa.HRME_Id == dto.HRME_Id && ao.MI_Id.Equals(dto.MI_Id) && ao.HRMCVIA_SectionCode == "80DD" && ao.HRMCVIA_ActiveFlg == true && pa.HRECVIA_ActiveFlg == true && pa.IMFY_Id == dto.IMFY_Id
                                   select new Form16DTO
                                  {
                                      HRMCVIA_SectionName = ao.HRMCVIA_SectionName,

                                      HRECVIA_Amount = pa.HRECVIA_Amount

                                  }).Distinct().ToList();

                dto.chapterlist80DD = chapter80DD.ToArray();


                var chapter80D = (from ao in _HRMSContext.HR_master_ChapterVI
                                   from pa in _HRMSContext.HR_Employee_ChapterVI
                                   where ao.HRMCVIA_Id == pa.HRMCVIA_Id && pa.HRME_Id == dto.HRME_Id && ao.MI_Id.Equals(dto.MI_Id) && ao.HRMCVIA_SectionCode == "80D" && ao.HRMCVIA_ActiveFlg == true && pa.HRECVIA_ActiveFlg == true && pa.IMFY_Id == dto.IMFY_Id
                                  select new Form16DTO
                                   {
                                       HRMCVIA_SectionName = ao.HRMCVIA_SectionName,

                                       HRECVIA_Amount = pa.HRECVIA_Amount

                                   }).Distinct().ToList();

                dto.chapterlist80D = chapter80D.ToArray( );


                var chapter80DDB = (from ao in _HRMSContext.HR_master_ChapterVI
                                  from pa in _HRMSContext.HR_Employee_ChapterVI
                                  where ao.HRMCVIA_Id == pa.HRMCVIA_Id && pa.HRME_Id == dto.HRME_Id && ao.MI_Id.Equals(dto.MI_Id) && ao.HRMCVIA_SectionCode == "80DDB" && ao.HRMCVIA_ActiveFlg == true && pa.HRECVIA_ActiveFlg == true && pa.IMFY_Id == dto.IMFY_Id
                                    select new Form16DTO
                                  {
                                      HRMCVIA_SectionName = ao.HRMCVIA_SectionName,   

                                      HRECVIA_Amount = pa.HRECVIA_Amount

                                  }).Distinct().ToList();

                dto.chapterlist80DDB = chapter80DDB.ToArray();

                //tax calculation


                var incomeTaxLists = (from ict in _HRMSContext.HR_MasterIncomeTax
                                      from gen in _HRMSContext.IVRM_Master_Gender
                                      from hr in _HRMSContext.MasterEmployee
                                      from xa in _HRMSContext.HR_MasterIncomeTaxDetails
                                      where ict.HRMIT_GenderFlag == gen.IVRMMG_Id
                                     && ict.IMFY_Id == dto.IMFY_Id
                                     && ict.MI_Id == dto.MI_Id
                                     && gen.IVRMMG_Id == hr.IVRMMG_Id
                                     && hr.HRME_Id == dto.HRME_Id && xa.HRMIT_Id == ict.HRMIT_Id
                                     && hr.HRME_ActiveFlag == true && gen.IVRMMG_ActiveFlag == true && ict.HRMIT_ActiveFlag == true && xa.HRMITD_ActiveFlag==true
                                      select new Form16DTO
                                      {
                                          HRMIT_GenderFlag = ict.HRMIT_GenderFlag,
                                          HRMIT_Id = ict.HRMIT_Id,
                                          MI_Id = ict.MI_Id,
                                          IMFY_Id = ict.IMFY_Id,
                                          IVRMMG_GenderName = gen.IVRMMG_GenderName,
                                          IVRMMG_Id = gen.IVRMMG_Id,
                                          HRMIT_AgeFlag = ict.HRMIT_AgeFlag,
                                          HRMIT_FromAge = ict.HRMIT_FromAge,
                                          HRMIT_ToAge = ict.HRMIT_ToAge,
                                          HRMITD_AmountFrom = xa.HRMITD_AmountFrom,
                                          HRMITD_AmountTo = xa.HRMITD_AmountTo,
                                          HRMITD_IncomeTax = xa.HRMITD_IncomeTax,
                                          HRME_DOB = DateTime.Now.Year- hr.HRME_DOB.Value.Year
                                         //  gendername = ict.gendername,
                                         // financilYear = ict.financilYear,
                                         //  CreatedDate = ict.CreatedDate,
                                         //   UpdatedDate = ict.UpdatedDate
                                     }).ToList();
                //var ca = DateTime.Now.Date - HRME_DOB.;
                dto.calculation = incomeTaxLists.ToArray();

                //r agefac = employeeDetails.Where(t => t.HRME_DOB.Value.Date.Year == DateTime.Now.Year);
                //var agefactor = _HRMSContext.HR_Configuration.Where(t => t.MI_Id == dto.MI_Id).Select(t => Convert.ToInt32(t.HRC_RetirementYrs));


                var days = from items in incomeTaxLists
                           select new
                           {
                               days = items.HRMIT_ToAge - items.HRME_DOB
                           };

                foreach (var day in days)
                {
                   // Console.WriteLine(day.days.TotalDays);
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return dto;
        }


        public async Task<Form16DTO> getEmployeeSalarySlip(Form16DTO dto)
        {

           

            return dto;
        }








    }
}