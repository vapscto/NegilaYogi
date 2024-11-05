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
    public class HREmpAllowanceService : Interfaces.HREmpAllowanceInterface
    {

    public HRMSContext _HRMSContext;
    public DomainModelMsSqlServerContext _Context;
    public HREmpAllowanceService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
    {
      _HRMSContext = HRMSContext;
      _Context = Context;
    }

    public HR_Emp_AllowanceDTO getBasicData(HR_Emp_AllowanceDTO dto)
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

    public HR_Emp_AllowanceDTO SaveUpdate(HR_Emp_AllowanceDTO dto)
    {
      dto.retrunMsg = "";



            //            try
            //            {


            //                HR_Emp_Allowance dmoObj = Mapper.Map<HR_Emp_Allowance>(dto);

            //                var duplicatecountresult = _HRMSContext.HR_Master_Emp_Allowance.Where(t => t.MI_Id == dto.MI_Id && t.HRME_Id == dto.HRME_Id && t.IMFY_Id == dto.IMFY_Id && t.HRMAL_Id==dto.HRMAL_Id).Count();
            //                if (duplicatecountresult == 0)
            //                {

            //                    if (dmoObj.HREAL_Id > 0)
            //                    {

            //                        var duplicatecount = _HRMSContext.HR_Master_Emp_Allowance.Where(t => t.MI_Id == dto.MI_Id && t.HREAL_Id
            // != dto.HREAL_Id
            //).Count();
            //                        if (duplicatecount == 0)
            //                        {
            //                            var result = _HRMSContext.HR_Master_Emp_Allowance.Single(t => t.HREAL_Id
            // == dmoObj.HREAL_Id
            //);

            //                            dmoObj.HREAL_UpdatedBy = dto.LogInUserId;
            //                            dto.HREAL_ActiveFlg = true;
            //                            Mapper.Map(dto, result);
            //                            _HRMSContext.Update(result);
            //                            var flag = _HRMSContext.SaveChanges();
            //                            if (flag > 0)
            //                            {
            //                                dto.retrunMsg = "Update";
            //                            }
            //                            else
            //                            {
            //                                dto.retrunMsg = "false";
            //                            }

            //                        }
            //                    }
            //                    else
            //                    {
            //                        var duplicatecounts = _HRMSContext.HR_Master_Emp_Allowance.Where(t => t.MI_Id == dto.MI_Id && t.HRME_Id == dto.HRME_Id && t.IMFY_Id == dto.IMFY_Id).Count();
            //                        if (duplicatecounts == 0)
            //                        {
            //                            var allowanceamt = _HRMSContext.HR_Master_Allowance.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMAL_MaxLimit <= dto.HREAL_Allowance && t.HRMAL_Id==dto.HRMAL_Id).Count();

            //                            if (allowanceamt == 0)
            //                            {
            //                                dmoObj.HREAL_UpdatedBy = dto.LogInUserId;
            //                                dmoObj.HREAL_CreatedBy = dto.LogInUserId;

            //                                dmoObj.HREAL_ActiveFlg = true;
            //                                dmoObj.IMFY_Id = dto.IMFY_Id;
            //                                dmoObj.MI_Id = dto.MI_Id;
            //                                dmoObj.HRMAL_Id = dto.HRMAL_Id;


            //                                dmoObj.HREAL_Allowance = dto.HREAL_Allowance;
            //                                dmoObj.HRME_Id = dto.HRME_Id;
            //                                dmoObj.HRMAL_Id = dto.HRMAL_Id;
            //                                _HRMSContext.Add(dmoObj);
            //                                var flag = _HRMSContext.SaveChanges();
            //                                if (flag == 1)
            //                                {
            //                                    dto.retrunMsg = "Add";
            //                                }
            //                                else
            //                                {
            //                                    dto.retrunMsg = "false";
            //                                }
            //                            }
            //                            else { dto.retrunMsg = "false"; }

            //                        }


            //                    }


            //                }

            //                dto = GetAllDropdownAndDatatableDetails(dto);
            //            }

            //            catch (Exception ee)
            //            {
            //                Console.WriteLine(ee.Message);
            //                dto.retrunMsg = "Error occured";
            //            }




            try
            {


                HR_Emp_Allowance dmoObj = Mapper.Map<HR_Emp_Allowance>(dto);

                var duplicatecountresult = _HRMSContext.HR_Master_Emp_Allowance.Where(t => t.MI_Id == dto.MI_Id && t.HRMAL_Id == dto.HRMAL_Id && t.HREAL_Id == dto.HREAL_Id).Count();

                //  var duplicatecountresults = _HRMSContext.HR_Master_TDS.Where(t => t.MI_Id == dto.MI_Id ).Count();

                if (duplicatecountresult > 0)
                {
                    var result = _HRMSContext.HR_Master_Emp_Allowance.Single(t => t.HREAL_Id == dmoObj.HREAL_Id);

                    dto.HREAL_UpdatedBy = dto.LogInUserId;
                     dto.HREAL_ActiveFlg = true;
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
                    var allowanceamt = _HRMSContext.HR_Master_Allowance.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMAL_MaxLimit <= dto.HREAL_Allowance && t.HRMAL_Id == dto.HRMAL_Id).Count();
                    if (allowanceamt == 0)
                    {
                        dmoObj.HREAL_UpdatedBy = dto.LogInUserId;
                        dmoObj.HREAL_CreatedBy = dto.LogInUserId;

                        dmoObj.HREAL_ActiveFlg = true;
                        dmoObj.IMFY_Id = dto.IMFY_Id;
                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.HRMAL_Id = dto.HRMAL_Id;


                        dmoObj.HREAL_Allowance = dto.HREAL_Allowance;
                        dmoObj.HRME_Id = dto.HRME_Id;
                        dmoObj.HRMAL_Id = dto.HRMAL_Id;
                        //dmoObj.HRMOI_Id = dto.HRMOI_Id;
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

    public HR_Emp_AllowanceDTO editData(int id)
    {

            HR_Emp_AllowanceDTO dto = new HR_Emp_AllowanceDTO();
      dto.retrunMsg = "";
      try
      {
       List<HR_Emp_Allowance> lorg = new List<HR_Emp_Allowance>();
        lorg = _HRMSContext.HR_Master_Emp_Allowance.Where(t => t.HREAL_Id
.Equals(id)).ToList();
        dto.emploanList = lorg.ToArray();

       dto = Mapper.Map<HR_Emp_AllowanceDTO>(lorg.FirstOrDefault());
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

        public HR_Emp_AllowanceDTO getDetailsByEmployee(HR_Emp_AllowanceDTO dto)
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


        public HR_Emp_AllowanceDTO deactivate(HR_Emp_AllowanceDTO dto)
    {
      dto.retrunMsg = "";
      try
      {
                if (dto.HREAL_Id
 > 0)
                {
                    var result = _HRMSContext.HR_Master_Emp_Allowance.Single(t => t.HREAL_Id
 == dto.HREAL_Id
);

                    if (result.HREAL_ActiveFlg
 == true)
                    {
                        result.HREAL_ActiveFlg
 = false;
                    }
                    else if (result.HREAL_ActiveFlg
 == false)
                    {
                        result.HREAL_ActiveFlg
 = true;
                    }
                    //result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HREAL_ActiveFlg
 == true)
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


    public HR_Emp_AllowanceDTO GetAllDropdownAndDatatableDetails(HR_Emp_AllowanceDTO dto)
    {
      List<HR_Emp_Allowance> datalist = new List<HR_Emp_Allowance>();

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
                dto.allowance = _HRMSContext.HR_Master_Allowance.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToArray();

                dto.leaveyeardropdown = _HRMSContext.IVRM_Master_FinancialYear.OrderBy(t => t.IMFY_OrderBy).ToArray();

                //dto.leaveyeardropdown = _HRMSContext.IVRM_Master_FinancialYear.ToArray();
                if (employees.Count() > 0)
                {

                    var empIds = employees.Select(t => t.HRME_Id);

                    // var master = _HRMSContext.HR_Master_Allowance.Where(t => t.MI_Id.Equals(dto.MI_Id));
                    // var mmmm = master.Select(t => t.HRMAL_Id);
                }
                    var datalists = (from emp in _HRMSContext.HR_Master_Emp_Allowance
                                     from mm in _HRMSContext.MasterEmployee
                                     from kk in _HRMSContext.IVRM_Master_FinancialYear
                                     from qq in _HRMSContext.HR_Master_Allowance
                                     where mm.MI_Id.Equals(dto.MI_Id)
                                      && qq.HRMAL_Id == emp.HRMAL_Id

                                     && mm.HRME_ActiveFlag == true && emp.HRME_Id == mm.HRME_Id && kk.IMFY_Id == emp.IMFY_Id
                                     orderby mm.HRME_EmployeeOrder
                                     select new HR_Emp_AllowanceDTO
                                     {
                                         hrmE_EmployeeFirstName = ((mm.HRME_EmployeeFirstName == null ? " " : mm.HRME_EmployeeFirstName) + (mm.HRME_EmployeeMiddleName == null ? " " : mm.HRME_EmployeeMiddleName) + (mm.HRME_EmployeeLastName == null ? " " : mm.HRME_EmployeeLastName)).Trim(),
                                         HREAL_Allowance = emp.HREAL_Allowance,
                                         HREAL_ActiveFlg = emp.HREAL_ActiveFlg,
                                         HRME_Id = emp.HRME_Id,
                                         IMFY_FinancialYear = kk.IMFY_FinancialYear,
                                         HRMAL_Id = qq.HRMAL_Id,
                                         HRMAL_AllowanceName = qq.HRMAL_AllowanceName,
                                         MI_Id = dto.MI_Id,
                                         IMFY_Id = kk.IMFY_Id,
                                         HREAL_Id = emp.HREAL_Id,

                                     }).Distinct().ToList();



                    dto.emploanList = datalists.ToArray();


                  

                

                




            }
      catch (Exception ee)
      {
        Console.WriteLine(ee.Message);
      }

      return dto;
    }


        }   
    }
