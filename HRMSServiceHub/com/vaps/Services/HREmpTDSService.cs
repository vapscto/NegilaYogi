using AutoMapper;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class HREmpTDSService : Interfaces.HREmpTDSInterface
    {

    public HRMSContext _HRMSContext;
    public DomainModelMsSqlServerContext _Context;
    public HREmpTDSService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
    {
      _HRMSContext = HRMSContext;
      _Context = Context;
    }

    public HR_Emp_TDSDTO getBasicData(HR_Emp_TDSDTO dto)
    {
      dto.retrunMsg = "";
      try
      {
        dto = GetAllDropdownAndDatatableDetails(dto);
      }
      catch (Exception ee)
      {
        Console.WriteLine(ee.Message);
        dto.retrunMsg = "Error occured";
      }
      return dto;
    }

    public HR_Emp_TDSDTO SaveUpdate(HR_Emp_TDSDTO dto)
    {
      dto.retrunMsg = "";
      try
      {


                HR_Emp_TDS dmoObj = Mapper.Map<HR_Emp_TDS>(dto);

                var duplicatecountresult = _HRMSContext.HR_Master_TDS.Where(t=>t.MI_Id==dto.MI_Id && t.HRETDS_Id==dto.HRETDS_Id).Count();

                //  var duplicatecountresults = _HRMSContext.HR_Master_TDS.Where(t => t.MI_Id == dto.MI_Id ).Count();

                if (duplicatecountresult > 0)
                {
                    var result = _HRMSContext.HR_Master_TDS.Single(t => t.HRETDS_Id == dmoObj.HRETDS_Id);

                    dmoObj.HRETDS_UpdatedBy = dto.LogInUserId;
                    dto.HRETDS_ActiveFlg = true;
                    Mapper.Map(dto, result);
                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        dto.retrunMsg = "Update";
                    }
                    else
                    {
                        dto.retrunMsg = "false";
                    }
                }

                else
                {
                    
                        dmoObj.HRETDS_UpdatedBy = dto.LogInUserId;
                        dmoObj.HRETDS_CreatedBy = dto.LogInUserId;
                        dmoObj.HRETDS_DepositedDate = dto.HRETDS_DepositedDate;
                        dmoObj.HRETDS_ActiveFlg = true;
                        dmoObj.IMFY_Id = dto.IMFY_Id;
                        dmoObj.MI_Id = dto.MI_Id;
                    dmoObj.HRME_Id = dto.HRME_Id;
                        dmoObj.HRETDS_BSRCode = dto.HRETDS_BSRCode;
                        dmoObj.HRETDS_ChallanNo = dto.HRETDS_ChallanNo;
                        dmoObj.HRETDS_TaxDeposited = dto.HRETDS_TaxDeposited;
                        _HRMSContext.Add(dmoObj);
                        var flag = _HRMSContext.SaveChanges();
                        if (flag == 1)
                        {
                            dto.retrunMsg = "Add";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }

                    


                }
                
               
                
             


                dto = GetAllDropdownAndDatatableDetails(dto);
            }

      catch (Exception ee)
      {
        Console.WriteLine(ee.Message);
        dto.retrunMsg = "Error occured";
      }

      return dto;
    }

    public HR_Emp_TDSDTO editData(int id)
    {

            HR_Emp_TDSDTO dto = new HR_Emp_TDSDTO();
      dto.retrunMsg = "";
      try
      {
       List<HR_Emp_TDS> lorg = new List<HR_Emp_TDS>();
        lorg = _HRMSContext.HR_Master_TDS.Where(t => t.HRETDS_Id.Equals(id)).ToList();
        dto.emploanList = lorg.ToArray();

       dto = Mapper.Map<HR_Emp_TDSDTO>(lorg.FirstOrDefault());
       dto.emploanList = lorg.ToArray();

       //var empearnDed = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
       //                 from mas in _HRMSContext.HR_Master_EarningsDeductions
       //                 from empsalsry in _HRMSContext.HR_Employee_Salary

       //                 from salarydetails in _HRMSContext.HR_Employee_Salary_Details
       //                  where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == dto.HRME_Id && mas.HRMED_EarnDedFlag == "Deduction" && mas.HRMED_EDTypeFlag=="IT" && emp.HRME_Id==empsalsry.HRME_Id && salarydetails.HRES_Id==empsalsry.HRES_Id)
       //                   select new HR_Employee_Salary_DetailsDTO
       //                    {

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

        public HR_Emp_TDSDTO getDetailsByEmployee(HR_Emp_TDSDTO dto)
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


        public HR_Emp_TDSDTO deactivate(HR_Emp_TDSDTO dto)
    {
      dto.retrunMsg = "";
      try
      {
                if (dto.HRETDS_Id > 0)
                {
                    var result = _HRMSContext.HR_Master_TDS.Single(t => t.HRETDS_Id == dto.HRETDS_Id);

                    if (result.HRETDS_ActiveFlg == true)
                    {
                        result.HRETDS_ActiveFlg = false;
                    }
                    else if (result.HRETDS_ActiveFlg == false)
                    {
                        result.HRETDS_ActiveFlg = true;
                    }
                    //result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRETDS_ActiveFlg == true)
                        {

                            dto.retrunMsg = "Activated";
                        }
                        else
                        {
                            dto.retrunMsg = "Deactivated";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Record Not Activated/Deactivated";
                    }

                    dto = GetAllDropdownAndDatatableDetails(dto);
                }
            }
      catch (Exception e)
      {
        Console.WriteLine(e.InnerException);
        dto.retrunMsg = "Error occured";
      }

      return dto;
    }


    public HR_Emp_TDSDTO GetAllDropdownAndDatatableDetails(HR_Emp_TDSDTO dto)
    {
      List<HR_Emp_TDS> datalist = new List<HR_Emp_TDS>();

      List<MasterEmployee> employe = new List<MasterEmployee>();
      List<HRMasterLoan> masteloan = new List<HRMasterLoan>();
            try
            {

                //var IVRM_ModeOfPayment = _HRMSContext.IVRM_ModeOfPayment.Where(t => t.IVRMMOD_ActiveFlag == true).ToList();
                //dto.modeOfPaymentdropdown = IVRM_ModeOfPayment.ToArray();

                //HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                //HR_ConfigurationDTO dmoObj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                //dto.configurationDetails = dmoObj;


                var employees = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                 from mm in _HRMSContext.MasterEmployee
                                 from med in _HRMSContext.HR_Master_EarningsDeductions
                                 where mm.MI_Id.Equals(dto.MI_Id)
                                 && emp.HRMED_Id == med.HRMED_Id
                               
                                 && mm.HRME_ActiveFlag == true && emp.HRME_Id == mm.HRME_Id
                                 orderby mm.HRME_EmployeeOrder
                                 select mm).Distinct().OrderBy(t => t.HRME_EmployeeOrder).ToList();



                dto.employeedropdown = employees.ToArray();
                dto.leaveyeardropdown = _HRMSContext.IVRM_Master_FinancialYear.OrderBy(t => t.IMFY_OrderBy).ToArray();

                //  dto.leaveyeardropdown = _HRMSContext.IVRM_Master_FinancialYear.ToArray();
                //if (employees.Count() > 0)
                // {

                var empIds = employees.Select(t => t.HRME_Id);

                    //   datalist = _HRMSContext.HR_Master_TDS.Where(t => t.MI_Id.Equals(dto.MI_Id) && empIds.Contains(t.HRME_Id)).ToList();
                    //  dto.emploanList = datalist.ToArray();

                    var datalists = (from emp in _HRMSContext.HR_Master_TDS
                                     from mm in _HRMSContext.MasterEmployee
                                     from kk in _HRMSContext.IVRM_Master_FinancialYear
                                     where mm.MI_Id.Equals(dto.MI_Id)
                                     && empIds.Contains(mm.HRME_Id)

                                     && mm.HRME_ActiveFlag == true && emp.HRME_Id == mm.HRME_Id && kk.IMFY_Id==emp.IMFY_Id
                                     orderby mm.HRME_EmployeeOrder
                                     select new HR_Emp_TDSDTO
                                     {
                                         hrmE_EmployeeFirstName = ((mm.HRME_EmployeeFirstName == null ? " " : mm.HRME_EmployeeFirstName) + (mm.HRME_EmployeeMiddleName == null ? " " : mm.HRME_EmployeeMiddleName) + (mm.HRME_EmployeeLastName == null ? " " : mm.HRME_EmployeeLastName)).Trim(),
                                         HRETDS_DepositedDate =emp.HRETDS_DepositedDate,
                                         HRETDS_BSRCode=emp.HRETDS_BSRCode,
                                         HRETDS_ChallanNo=emp.HRETDS_ChallanNo,
                                         IMFY_FinancialYear=kk.IMFY_FinancialYear,
                                         HRETDS_Id = emp.HRETDS_Id,
                                         HRME_Id=emp.HRME_Id,
                                        MI_Id=dto.MI_Id,IMFY_Id=kk.IMFY_Id,
                                         HRETDS_ActiveFlg=emp.HRETDS_ActiveFlg,
                                         HRETDS_TaxDeposited=emp.HRETDS_TaxDeposited

                                     }).Distinct().ToList();



                    dto.emploanList = datalists.ToArray();



                //}



                //masteloan = _HRMSContext.HRMasterLoan.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLN_ActiveFlag == true).ToList();
                //dto.masterloandropdown = masteloan.ToArray();





            }
      catch (Exception ee)
      {
        Console.WriteLine(ee.Message);
      }

      return dto;
    }


        }   
    }
