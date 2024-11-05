using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DataAccessMsSqlServerProvider;
using PreadmissionDTOs.com.vaps.Portals.Student;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.Portals.Employee;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DomainModel.Model.com.vapstech.HRMS;

namespace PortalHub.com.vaps.Employee.Services
{
    public class HREmpInvestmentSubsectionService : Interfaces.HREmpInvestmentSubsectionInterface
    {
        public FeeGroupContext _fees;

        public ExamContext _exm;

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public HREmpInvestmentSubsectionService(HRMSContext HRMSContext, FeeGroupContext fees, ExamContext exm, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
            _fees = fees;
            _exm = exm;
        }

        public EmployeeInvestmentSubsectionDTO getBasicData(EmployeeInvestmentSubsectionDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
             //   dto = GetAllDropdownAndDatatableDetails(dto);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public EmployeeInvestmentSubsectionDTO SaveUpdate(EmployeeInvestmentSubsectionDTO dto)
        {
            dto.retrunMsg = "";
            try
            {


                //HR_Employee_Subsection_Investment dmoObj = Mapper.Map<HR_Employee_Subsection_Investment>(dto);

                //var duplicatecountresult = _HRMSContext.HR_Employee_Subsection_Investment.Count();
                //if (duplicatecountresult == 0)
                //{

                //    if (dmoObj.HREID_Id > 0)
                //    {

                //        var duplicatecount = _HRMSContext.HR_Employee_Subsection_Investment.Count();
                //        if (duplicatecount == 0)
                //        {
                //            var result = _HRMSContext.HR_Employee_Subsection_Investment.Single(t => t.HREID_Id == dmoObj.HREID_Id);
                //            //dto.HRETDS_DepositedDate = DateTime.Now;
                //            //  dmoObj.HRETDS_UpdatedBy = dto.LogInUserId;
                //            // dmoObj.HRETDS_CreatedBy = dto.LogInUserId;
                //            Mapper.Map(dto, result);
                //            _HRMSContext.Update(result);
                //            var flag = _HRMSContext.SaveChanges();
                //            if (flag > 0)
                //            {
                //                dto.retrunMsg = "Update";
                //            }
                //            else
                //            {
                //                dto.retrunMsg = "false";
                //            }
                //        }
                //        else
                //        {
                //            dto.retrunMsg = "Duplicate";
                //        }
                //    }
                //    else
                //    {
                //        var duplicatecount = _HRMSContext.HR_Employee_Subsection_Investment.Count();
                //        if (duplicatecount == 0)
                //        {
                //            //dmoObj.HRETDS_UpdatedBy = dto.LogInUserId;
                //            //dmoObj.HRETDS_CreatedBy = dto.LogInUserId;
                //            //dmoObj.HRETDS_DepositedDate = DateTime.Now;
                //            //dmoObj.HRETDS_ActiveFlg = true;
                //            //dmoObj.IMFY_Id = dto.IMFY_Id;
                //            //dmoObj.MI_Id = dto.MI_Id;
                //            //  dmoObj.UpdatedDate = DateTime.Now;
                //            // dmoObj.CreatedDate = DateTime.Now;
                //            _HRMSContext.Add(dmoObj);
                //            var flag = _HRMSContext.SaveChanges();
                //            if (flag == 1)
                //            {
                //                dto.retrunMsg = "Add";
                //            }
                //            else
                //            {
                //                dto.retrunMsg = "false";
                //            }
                //        }
                //        else
                //        {
                //            dto.retrunMsg = "Duplicate";
                //        }
                //    }
              // }
              //  else
               // {
               //     dto.retrunMsg = "Duplicate";
               // }



              //  dto = GetAllDropdownAndDatatableDetails(dto);
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public EmployeeInvestmentSubsectionDTO editData(int id)
        {

           EmployeeInvestmentSubsectionDTO dto = new EmployeeInvestmentSubsectionDTO();
            dto.retrunMsg = "";
            try
           {
            //    List<HR_Employee_Subsection_Investment> lorg = new List<HR_Employee_Subsection_Investment>();
             //   lorg = _HRMSContext.HR_Employee_Subsection_Investment.Where(t => t.HREID_Id
        //.Equals(id)).ToList();
            //    dto.emploanList = lorg.ToArray();
            //
              //  dto = Mapper.Map<EmployeeInvestmentSubsectionDTO>(lorg.FirstOrDefault());
              //  dto.emploanList = lorg.ToArray();

                //var empearnDed = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                //                 from mas in _HRMSContext.HR_Master_EarningsDeductions
                //                 from empsalsry in _HRMSContext.HR_Employee_Salary

                //                 from salarydetails in _HRMSContext.HR_Employee_Salary_Details
                //                  where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == dto.HRME_Id && mas.HRMED_EarnDedFlag == "Deduction" && mas.HRMED_EDTypeFlag=="IT" && emp.HRME_Id==empsalsry.HRME_Id && salarydetails.HRES_Id==empsalsry.HRES_Id)
                //                   select new EmployeeInvestmentDTO
                //                   {

                //                               HRMED_Id = emp.HRMED_Id,
                //                               HRMED_Name = mas.HRMED_Name,
                //                               HRESD_Amount = emp.HREED_Amount,
                //                               HRMED_EarnDedFlag = mas.HRMED_EarnDedFlag
                //                   }).ToList();

                //         dto.empGrossSal = Convert.ToDecimal(empearnDed.Sum(t => t.HRESD_Amount));
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
               dto.retrunMsg = "Error occured";
           }

            return dto;
        }

        public EmployeeInvestmentSubsectionDTO getDetailsByEmployee(EmployeeInvestmentSubsectionDTO dto)
        {
            dto.empGrossSal = 0;
            // dto.totalAppliedAmount = 0;


            try
            {
                //  var empearnDed = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                //                    from mas in _HRMSContext.HR_Master_EarningsDeductions
                //                    where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == dto.HRME_Id && mas.HRMED_EarnDedFlag == "Earning")
                //                    select new HR_Employee_Salary_DetailsDTO
                //                        {

                //                        HRMED_Id = emp.HRMED_Id,
                //                        HRMED_Name = mas.HRMED_Name,
                //                        HRESD_Amount = emp.HREED_Amount,
                //                        HRMED_EarnDedFlag = mas.HRMED_EarnDedFlag
                //                        }).ToList();

                //  dto.empGrossSal = Convert.ToDecimal(empearnDed.Sum(t => t.HRESD_Amount));
                ////  GetTotalAppliedAmount(dto);




            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }


            return dto;
        }


        public EmployeeInvestmentSubsectionDTO deactivate(EmployeeInvestmentSubsectionDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
//                if (dto.HREIDSS_Id

// > 0)
//                {
//                    var result = _HRMSContext.HR_Employee_Subsection_Investment.Single(t => t.HREIDSS_Id

// == dto.HREIDSS_Id

//);

//                    if (result.HREIDSS_ActiveFlg

// == true)
//                    {
//                        result.HREIDSS_ActiveFlg

// = false;
//                    }
//                    else if (result.HREIDSS_ActiveFlg

// == false)
//                    {
//                        result.HREIDSS_ActiveFlg

// = true;
//                    }
//                    //result.UpdatedDate = DateTime.Now;

//                    _HRMSContext.Update(result);
//                    var flag = _HRMSContext.SaveChanges();
//                    if (flag > 0)
//                    {
//                        if (result.HREIDSS_ActiveFlg

// == true)
//                        {

//                            dto.retrunMsg = "Activated";
//                        }
//                        else
//                        {
//                            dto.retrunMsg = "Deactivated";
//                        }
//                    }
//                    else
//                    {
//                        dto.retrunMsg = "Record Not Activated/Deactivated";
//                    }

//                    dto = GetAllDropdownAndDatatableDetails(dto);
//                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }


        public EmployeeInvestmentSubsectionDTO GetAllDropdownAndDatatableDetails(EmployeeInvestmentSubsectionDTO dto)
        {
            List<HR_Employee_Investment> datalist = new List<HR_Employee_Investment>();

            List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HRMasterLoan> masteloan = new List<HRMasterLoan>();
            try
            {

                //var IVRM_ModeOfPayment = _HRMSContext.IVRM_ModeOfPayment.Where(t => t.IVRMMOD_ActiveFlag == true).ToList();
                //dto.modeOfPaymentdropdown = IVRM_ModeOfPayment.ToArray();

                //HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                //HR_ConfigurationDTO dmoObj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                //dto.configurationDetails = dmoObj;


                //var employees = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                //                 from mm in _HRMSContext.MasterEmployee
                //                 from med in _HRMSContext.HR_Master_EarningsDeductions
                //                 where mm.MI_Id.Equals(dto.MI_Id)
                //                 && emp.HRMED_Id == med.HRMED_Id

                //                 && mm.HRME_ActiveFlag == true && emp.HRME_Id == mm.HRME_Id
                //                 orderby mm.HRME_EmployeeOrder
                //                 select mm).Distinct().OrderBy(t => t.HRME_EmployeeOrder).ToList();



                //dto.employeedropdown = employees.ToArray();

                //dto.leaveyeardropdown = _HRMSContext.IVRM_Master_FinancialYear.ToArray();
                //if (employees.Count() > 0)
                //{

                //    var empIds = employees.Select(t => t.HRME_Id);

                //    //    datalist = _HRMSContext.HR_Master_TDS.Where(t => t.MI_Id.Equals(dto.MI_Id) && empIds.Contains(t.HRME_Id)).ToList();
                //    //  dto.emploanList = datalist.ToArray();
                //}



                ////masteloan = _HRMSContext.HRMasterLoan.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLN_ActiveFlag == true).ToList();
                ////dto.masterloandropdown = masteloan.ToArray();





            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }


    }
}
