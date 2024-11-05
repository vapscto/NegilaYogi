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
    public class HREmpChapterVIService : Interfaces.HREmpChapterVIInterface
    {

    public HRMSContext _HRMSContext;
    public DomainModelMsSqlServerContext _Context;
    public HREmpChapterVIService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
    {
      _HRMSContext = HRMSContext;
      _Context = Context;
    }

    public HR_Emp_ChapterVIDTO getBasicData(HR_Emp_ChapterVIDTO dto)
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

    public HR_Emp_ChapterVIDTO SaveUpdate(HR_Emp_ChapterVIDTO dto)
    {
      dto.retrunMsg = "";

           
            try
            {


                HR_Emp_ChapterVI dmoObj = Mapper.Map<HR_Emp_ChapterVI>(dto);

                var duplicatecountresult = _HRMSContext.HR_Employee_ChapterVI.Where(t => t.MI_Id == dto.MI_Id && t.HRMCVIA_Id == dto.HRMCVIA_Id && t.HRECVIA_Id==dto.HRECVIA_Id).Count();

                //  var duplicatecountresults = _HRMSContext.HR_Master_TDS.Where(t => t.MI_Id == dto.MI_Id ).Count();

                if (duplicatecountresult > 0)
                {
                    var result = _HRMSContext.HR_Employee_ChapterVI.Single(t => t.HRECVIA_Id == dmoObj.HRECVIA_Id);

                    dto.HRECVIA_UpdatedBy = dto.LogInUserId;
                    dto.HRECVIA_ActiveFlg = true;
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
                    var allowanceamt = _HRMSContext.HR_master_ChapterVI.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMCVIA_MaxLimit <= dto.HRECVIA_Amount && t.HRMCVIA_Id == dto.HRECVIA_Id).Count();

                    if (allowanceamt == 0)
                    {
                        dmoObj.HRECVIA_CreatedBy = dto.LogInUserId;
                        dmoObj.HRECVIA_UpdatedBy = dto.LogInUserId;

                        dmoObj.HRECVIA_ActiveFlg = true;
                        dmoObj.IMFY_Id = dto.IMFY_Id;
                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.HRMCVIA_Id = dto.HRMCVIA_Id;


                        dmoObj.HRECVIA_Amount = dto.HRECVIA_Amount;
                        dmoObj.HRME_Id = dto.HRME_Id;
                        dmoObj.HRMCVIA_Id = dto.HRMCVIA_Id;

                        // dmoObj.HREAL_Allowance = dto.HREAL_Allowance;
                        dmoObj.HRME_Id = dto.HRME_Id;
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

    public HR_Emp_ChapterVIDTO editData(int id)
    {

            HR_Emp_ChapterVIDTO dto = new HR_Emp_ChapterVIDTO();
      dto.retrunMsg = "";
      try
      {
       List<HR_Emp_ChapterVI> lorg = new List<HR_Emp_ChapterVI>();
        lorg = _HRMSContext.HR_Employee_ChapterVI.Where(t => t.HRECVIA_Id.Equals(id)).ToList();
        dto.emploanList = lorg.ToArray();

       dto = Mapper.Map<HR_Emp_ChapterVIDTO>(lorg.FirstOrDefault());
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

        public HR_Emp_ChapterVIDTO getDetailsByEmployee(HR_Emp_ChapterVIDTO dto)
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


        public HR_Emp_ChapterVIDTO deactivate(HR_Emp_ChapterVIDTO dto)
    {
      dto.retrunMsg = "";
      try
      {
                if (dto.HRECVIA_Id
 > 0)
                {
                    var result = _HRMSContext.HR_Employee_ChapterVI.Single(t => t.HRECVIA_Id
 == dto.HRECVIA_Id
);

                    if (result.HRECVIA_ActiveFlg
 == true)
                    {
                        result.HRECVIA_ActiveFlg
 = false;
                    }
                    else if (result.HRECVIA_ActiveFlg

 == false)
                    {
                        result.HRECVIA_ActiveFlg
 = true;
                    }
                    //result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRECVIA_ActiveFlg
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


    public HR_Emp_ChapterVIDTO GetAllDropdownAndDatatableDetails(HR_Emp_ChapterVIDTO dto)
    {
      List<HR_Emp_ChapterVI> datalist = new List<HR_Emp_ChapterVI>();

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

               // dto.leaveyeardropdown = _HRMSContext.IVRM_Master_FinancialYear.ToArray();
                if (employees.Count() > 0)
                {

                    var empIds = employees.Select(t => t.HRME_Id);

                    //datalist = _HRMSContext.HR_Employee_ChapterVI.Where(t => t.MI_Id.Equals(dto.MI_Id) && empIds.Contains(t.HRME_Id)).ToList();
                    //dto.emploanList = datalist.ToArray();



                    var datalists = (from emp in _HRMSContext.HR_Employee_ChapterVI
                                     from mm in _HRMSContext.MasterEmployee
                                     from kk in _HRMSContext.IVRM_Master_FinancialYear
                                     from qq in _HRMSContext.HR_master_ChapterVI
                                     where mm.MI_Id.Equals(dto.MI_Id)
                                     && empIds.Contains(mm.HRME_Id) && qq.HRMCVIA_Id == emp.HRMCVIA_Id

                                     && mm.HRME_ActiveFlag == true && emp.HRME_Id == mm.HRME_Id && kk.IMFY_Id == emp.IMFY_Id
                                     orderby mm.HRME_EmployeeOrder
                                     select new HR_Emp_ChapterVIDTO
                                     {
                                         hrmE_EmployeeFirstName = ((mm.HRME_EmployeeFirstName == null ? " " : mm.HRME_EmployeeFirstName) + (mm.HRME_EmployeeMiddleName == null ? " " : mm.HRME_EmployeeMiddleName) + (mm.HRME_EmployeeLastName == null ? " " : mm.HRME_EmployeeLastName)).Trim(),
                                         HRECVIA_Amount = emp.HRECVIA_Amount,
                                         HRECVIA_ActiveFlg = emp.HRECVIA_ActiveFlg,
                                         HRME_Id = emp.HRME_Id,
                                         IMFY_FinancialYear = kk.IMFY_FinancialYear,
                                         HRMCVIA_Id = qq.HRMCVIA_Id,
                                         HRMCVIA_SectionName = qq.HRMCVIA_SectionName,
                                         MI_Id = dto.MI_Id,
                                         IMFY_Id = kk.IMFY_Id,
                                         HRECVIA_Id = emp.HRECVIA_Id,

                                     }).Distinct().ToList();



                    dto.emploanList = datalists.ToArray();


                }



                dto.modeOfPaymentdropdown = _HRMSContext.HR_master_ChapterVI.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToArray();

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
