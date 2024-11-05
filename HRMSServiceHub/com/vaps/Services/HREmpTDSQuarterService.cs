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
    public class HREmpTDSQuarterService : Interfaces.HREmpTDSQuarterInterface
    {

    public HRMSContext _HRMSContext;
    public DomainModelMsSqlServerContext _Context;
    public HREmpTDSQuarterService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
    {
      _HRMSContext = HRMSContext;
      _Context = Context;
    }

    public HR_Emp_TDS_QUARTERDTO getBasicData(HR_Emp_TDS_QUARTERDTO dto)
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

    public HR_Emp_TDS_QUARTERDTO SaveUpdate(HR_Emp_TDS_QUARTERDTO dto)
    {
      dto.retrunMsg = "";
      try
      {
                HR_Employee_TDS_Quarter dmoObj = Mapper.Map<HR_Employee_TDS_Quarter>(dto);

                var duplicatecountresult = _HRMSContext.HR_Employee_TDS_Quarter.Where(t=>t.MI_Id==dto.MI_Id && t.HRETDSQ_Id==dto.HRETDSQ_Id).Count();

                //  var duplicatecountresults = _HRMSContext.HR_Master_TDS.Where(t => t.MI_Id == dto.MI_Id ).Count();

                if (duplicatecountresult > 0)
                {
                    var result = _HRMSContext.HR_Employee_TDS_Quarter.Single(t => t.HRETDSQ_Id == dmoObj.HRETDSQ_Id);

                    dmoObj.HRETDSQ_UpdatedBy = dto.LogInUserId;
                    dto.HRETDSQ_ActiveFlg = true;
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
                    
                        dmoObj.HRETDSQ_UpdatedBy = dto.LogInUserId;
                        dmoObj.HRETDSQ_CreatedBy = dto.LogInUserId;
                        dmoObj.HRMQ_Id = dto.HRMQ_Id;
                        dmoObj.HRETDSQ_ActiveFlg = true;
                        dmoObj.IMFY_Id = dto.IMFY_Id;
                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.HRME_Id = dto.HRME_Id;
                        dmoObj.HRETDSR_ReceiptNo = dto.HRETDSR_ReceiptNo;
                        dmoObj.HRETDS_AmountPaid = dto.HRETDS_AmountPaid;
                    //   dmoObj.HRETDS_TaxDeposited = dto.HRETDS_TaxDeposited;
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

    public HR_Emp_TDS_QUARTERDTO editData(int id)
    {

            HR_Emp_TDS_QUARTERDTO dto = new HR_Emp_TDS_QUARTERDTO();
      dto.retrunMsg = "";
      try
      {
       List<HR_Employee_TDS_Quarter> lorg = new List<HR_Employee_TDS_Quarter>();
        //        lorg = (from a in _HRMSContext.HR_Employee_TDS_Quarter
        //                from c in _HRMSContext.HR_Master_quarter
        //                where (a.HRMQ_Id == c.HRMQ_Id && a.HRETDSQ_Id.Equals(id))).ToList();
               
        //dto.emploanList = lorg.ToArray();

     //  dto = Mapper.Map<HR_Emp_TDS_QUARTERDTO>(lorg.FirstOrDefault());
     //  dto.emploanList = lorg.ToArray();

                var empearnDed = (from emp in _HRMSContext.HR_Employee_TDS_Quarter
                                  from mas in _HRMSContext.HR_Master_quarter
                                  from fin in _HRMSContext.IVRM_Master_FinancialYear
                                  where (emp.HRMQ_Id == mas.HRMQ_Id && emp.HRETDSQ_ActiveFlg == true && emp.HRETDSQ_Id.Equals(id) && fin.IMFY_Id==emp.IMFY_Id)
                                  select new HR_Emp_TDS_QUARTERDTO
                                  {

                                      HRETDSR_ReceiptNo = emp.HRETDSR_ReceiptNo,
                                      HRETDS_AmountPaid = emp.HRETDS_AmountPaid,
                                      HRMQ_QuarterName = mas.HRMQ_QuarterName,
                                      HRMQ_Id = mas.HRMQ_Id,
                                      HRME_Id=emp.HRME_Id,
                                      IMFY_Id=emp.IMFY_Id,
                                  }).ToList();

                dto.emploanList = empearnDed.ToArray();
                ////  GetTotalAppliedAmount(dto);


            }
            catch (Exception ee)
      {
        Console.WriteLine(ee.Message);
        dto.retrunMsg = "Error occured";
      }

      return dto;
    }

        public HR_Emp_TDS_QUARTERDTO getDetailsByEmployee(HR_Emp_TDS_QUARTERDTO dto)
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


        public HR_Emp_TDS_QUARTERDTO deactivate(HR_Emp_TDS_QUARTERDTO dto)
    {
      dto.retrunMsg = "";
      try
      {
                if (dto.HRETDSQ_Id > 0)
                {
                    var result = _HRMSContext.HR_Employee_TDS_Quarter.Single(t => t.HRETDSQ_Id == dto.HRETDSQ_Id);

                    if (result.HRETDSQ_ActiveFlg == true)
                    {
                        result.HRETDSQ_ActiveFlg = false;
                    }
                    else if (result.HRETDSQ_ActiveFlg == false)
                    {
                        result.HRETDSQ_ActiveFlg = true;
                    }
                    //result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRETDSQ_ActiveFlg == true)
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


    public HR_Emp_TDS_QUARTERDTO GetAllDropdownAndDatatableDetails(HR_Emp_TDS_QUARTERDTO dto)
    {
      List<HR_Emp_TDS> datalist = new List<HR_Emp_TDS>();

      List<MasterEmployee> employe = new List<MasterEmployee>();
      List<HRMasterLoan> masteloan = new List<HRMasterLoan>();
            try
            {
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
                //dto.quarterdropdown = _HRMSContext.HR_Master_quarter.Where(t => t.HRMQ_ActiveFlg == true && t.MI_Id==dto.MI_Id).ToArray();
                var empIds = employees.Select(t => t.HRME_Id);

                    var datalists = (from emp in _HRMSContext.HR_Employee_TDS_Quarter
                                     from mm in _HRMSContext.MasterEmployee
                                     from kk in _HRMSContext.IVRM_Master_FinancialYear
                                     from qq in _HRMSContext.HR_Master_quarter
                                     where mm.MI_Id.Equals(dto.MI_Id)
                                     && empIds.Contains(mm.HRME_Id)

                                     && mm.HRME_ActiveFlag == true && emp.HRME_Id == mm.HRME_Id && kk.IMFY_Id==emp.IMFY_Id && qq.HRMQ_Id==emp.HRMQ_Id
                                     orderby mm.HRME_EmployeeOrder
                                     select new HR_Emp_TDS_QUARTERDTO
                                     {
                                         hrmE_EmployeeFirstName = ((mm.HRME_EmployeeFirstName == null ? " " : mm.HRME_EmployeeFirstName) + (mm.HRME_EmployeeMiddleName == null ? " " : mm.HRME_EmployeeMiddleName) + (mm.HRME_EmployeeLastName == null ? " " : mm.HRME_EmployeeLastName)).Trim(),
                                         HRMQ_Id = emp.HRMQ_Id,
                                         HRETDS_AmountPaid = emp.HRETDS_AmountPaid,
                                         HRETDSR_ReceiptNo = emp.HRETDSR_ReceiptNo,
                                         HRMQ_QuarterName = qq.HRMQ_QuarterName,
                                         IMFY_FinancialYear=kk.IMFY_FinancialYear,
                                         HRETDSQ_Id = emp.HRETDSQ_Id,
                                         HRME_Id=emp.HRME_Id,
                                        MI_Id=dto.MI_Id,IMFY_Id=kk.IMFY_Id,
                                         HRETDSQ_ActiveFlg=emp.HRETDSQ_ActiveFlg,
                                         //HRETDS_TaxDeposited=emp.HRETDS_TaxDeposited
                                     }).Distinct().ToList();

                    dto.emploanList = datalists.ToArray();
            }
      catch (Exception ee)
      {
        Console.WriteLine(ee.Message);
      }

      return dto;
    }

        public HR_Emp_TDS_QUARTERDTO getquarter(HR_Emp_TDS_QUARTERDTO dto)
        {
            dto.empGrossSal = 0;
            try
            {
                //var quarterdropdown = (from a in _HRMSContext.HR_Master_quarter
                //                       from b in _HRMSContext.IVRM_Master_FinancialYear
                //                       where (a.MI_Id == dto.MI_Id && b.IMFY_Id == dto.IMFY_Id && a.HRMQ_FromDay > b.IMFY_FromDate && a.HRMQ_ToDay < b.IMFY_ToDate && a.HRMQ_ActiveFlg == true && a.HRMQ_FromDay >= dto.IMFY_FromDate && a.HRMQ_ToDay < dto.IMFY_ToDate)
                //                       select new HR_Emp_TDS_QUARTERDTO
                //                       {
                //                           HRMQ_Id = a.HRMQ_Id,
                //                           HRMQ_QuarterName = a.HRMQ_QuarterName
                //                       }).ToArray();
                //dto.quarterdropdown = quarterdropdown;

                var quarterdropdown = (from a in _HRMSContext.HR_Master_quarter
                                       from b in _HRMSContext.IVRM_Master_FinancialYear
                                       where (a.MI_Id == dto.MI_Id && b.IMFY_Id == dto.IMFY_Id && a.HRMQ_FromDay >= dto.IMFY_FromDate && a.HRMQ_ToDay <= dto.IMFY_ToDate && a.HRMQ_ActiveFlg == true && a.HRMQ_FromDay >= dto.IMFY_FromDate && a.HRMQ_ToDay <= dto.IMFY_ToDate)
                                       select new HR_Emp_TDS_QUARTERDTO
                                       {
                                           HRMQ_Id = a.HRMQ_Id,
                                           HRMQ_QuarterName = a.HRMQ_QuarterName
                                       }).ToArray();
                dto.quarterdropdown = quarterdropdown;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

    }   
    }
