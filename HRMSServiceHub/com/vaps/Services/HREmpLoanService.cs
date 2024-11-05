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
    public class HREmpLoanService:Interfaces.HREmpLoanInterface
    {

    public HRMSContext _HRMSContext;
    public DomainModelMsSqlServerContext _Context;
    public HREmpLoanService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
    {
      _HRMSContext = HRMSContext;
      _Context = Context;
    }

    public HR_Emp_LoanDTO getBasicData(HR_Emp_LoanDTO dto)
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

    public HR_Emp_LoanDTO SaveUpdate(HR_Emp_LoanDTO dto)
    {
      dto.retrunMsg = "";
      try
      {
                HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                HR_ConfigurationDTO conobj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);

                if (conobj.HRC_LoanApprovalFlg == true)
                    {
                    dto.HREL_LoanStatus = "Applied";
                    }
                else
                    {
                    dto.HREL_LoanStatus = "Sanctioned";
                    dto.HREL_SanctionedAmount = dto.HREL_LoanAmount;
                    dto.HREL_TotalPending = dto.HREL_LoanAmount;
                    }
                   

                HR_Emp_Loan dmoObj = Mapper.Map<HR_Emp_Loan>(dto);
               

                    if (dmoObj.HREL_Id > 0)
                    {
                          var result = _HRMSContext.HR_Emp_Loan.Single(t => t.HREL_Id == dmoObj.HREL_Id);
                          dto.UpdatedDate = DateTime.Now;
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
                          var duplicateaccno = _HRMSContext.HR_Emp_Loan.Where(t => t.MI_Id == dto.MI_Id && t.HRME_Id.Equals(dto.HRME_Id) && t.HREL_TotalPending > 0 && t.HREL_AppliedDate==dto.HREL_AppliedDate).Count();
                          if (duplicateaccno == 0)
                          {

                        var Master_Numbering = _Context.Master_Numbering.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.IMN_Flag == "Loan").ToList();
                        if (Master_Numbering.Count() > 0)
                            {
                            dto.transnumconfigsettings = Mapper.Map<Master_NumberingDTO>(Master_Numbering.FirstOrDefault());
                            }
                        else
                            {
                            dto.retrunMsg = "Numbering";
                            return dto;
                            }
                   

                        if (dto.transnumconfigsettings != null)
                            {
                            if (dto.transnumconfigsettings.IMN_AutoManualFlag == "Auto")
                                {

                                dto.transnumconfigsettings.MI_Id = dto.MI_Id;
                                dto.transnumconfigsettings.ASMAY_Id = dto.ASMAY_Id;
                                dmoObj.HREL_EmpLoanId = GenerateNumber(dto.transnumconfigsettings);

                                }
                            else
                                {
                                dmoObj.HREL_EmpLoanId = dto.HREL_EmpLoanId;
                                }

                            }
                        else
                            {
                            dto.retrunMsg = "Employee Loan Number is not Generated. kindly contact Admin";
                            return dto;
                            }


                            dmoObj.HREL_ActiveFlag = true;
                            dmoObj.UpdatedDate = DateTime.Now;
                            dmoObj.CreatedDate = DateTime.Now;
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
                           dto.retrunMsg = "Duplicate";
                           return dto;
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

    public HR_Emp_LoanDTO editData(int id)
    {

      HR_Emp_LoanDTO dto = new HR_Emp_LoanDTO();
      dto.retrunMsg = "";
      try
      {
        List<HR_Emp_Loan> lorg = new List<HR_Emp_Loan>();
        lorg = _HRMSContext.HR_Emp_Loan.AsNoTracking().Where(t => t.HREL_Id.Equals(id)).ToList();
        dto.emploanList = lorg.ToArray();

                dto = Mapper.Map<HR_Emp_LoanDTO>(lorg.FirstOrDefault());
                dto.emploanList = lorg.ToArray();

                var empearnDed = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                  from mas in _HRMSContext.HR_Master_EarningsDeductions
                                  where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == dto.HRME_Id && mas.HRMED_EarnDedFlag == "Earning")
                                  select new HR_Employee_Salary_DetailsDTO
                                      {

                                      HRMED_Id = emp.HRMED_Id,
                                      HRMED_Name = mas.HRMED_Name,
                                      HRESD_Amount = emp.HREED_Amount,
                                      HRMED_EarnDedFlag = mas.HRMED_EarnDedFlag
                                      }).ToList();

                dto.empGrossSal = Convert.ToDecimal(empearnDed.Sum(t => t.HRESD_Amount));
                }
      catch (Exception ee)
      {
        Console.WriteLine(ee.Message);
        dto.retrunMsg = "Error occured";
      }

      return dto;
    }

        public HR_Emp_LoanDTO getDetailsByEmployee(HR_Emp_LoanDTO dto)
            {
            dto.empGrossSal = 0;
           // dto.totalAppliedAmount = 0;


            try
                {
                var empearnDed = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                  from mas in _HRMSContext.HR_Master_EarningsDeductions
                                  where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == dto.HRME_Id && mas.HRMED_EarnDedFlag == "Earning")
                                  select new HR_Employee_Salary_DetailsDTO
                                      {

                                      HRMED_Id = emp.HRMED_Id,
                                      HRMED_Name = mas.HRMED_Name,
                                      HRESD_Amount = emp.HREED_Amount,
                                      HRMED_EarnDedFlag = mas.HRMED_EarnDedFlag
                                      }).ToList();

                dto.empGrossSal = Convert.ToDecimal(empearnDed.Sum(t => t.HRESD_Amount));
              //  GetTotalAppliedAmount(dto);




                }
            catch (Exception ee)
                {
                Console.WriteLine(ee.Message);
                }


            return dto;
            }


        public HR_Emp_LoanDTO deactivate(HR_Emp_LoanDTO dto)
    {
      dto.retrunMsg = "";
      try
      {
        if (dto.HREL_Id > 0)
        {
          var result = _HRMSContext.HR_Emp_Loan.Single(t => t.HREL_Id == dto.HREL_Id);

          if (result.HREL_ActiveFlag == true)
          {
            result.HREL_ActiveFlag = false;
          }
          else if (result.HREL_ActiveFlag == false)
          {
            result.HREL_ActiveFlag = true;
          }
          result.UpdatedDate = DateTime.Now;

          _HRMSContext.Update(result);
          var flag = _HRMSContext.SaveChanges();
          if (flag > 0)
          {
            if (result.HREL_ActiveFlag == true)
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


    public HR_Emp_LoanDTO GetAllDropdownAndDatatableDetails(HR_Emp_LoanDTO dto)
    {
      List<HR_Emp_Loan> datalist = new List<HR_Emp_Loan>();

      List<MasterEmployee> employe = new List<MasterEmployee>();
      List<HRMasterLoan> masteloan = new List<HRMasterLoan>();
            try
            {

                var IVRM_ModeOfPayment = _HRMSContext.IVRM_ModeOfPayment.Where(t => t.IVRMMOD_ActiveFlag == true && t.MI_Id.Equals(dto.MI_Id)).ToList();
                dto.modeOfPaymentdropdown = IVRM_ModeOfPayment.ToArray();

                HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                HR_ConfigurationDTO dmoObj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                dto.configurationDetails = dmoObj;
                  
             
                    var employees = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                     from mm in _HRMSContext.MasterEmployee
                                     from med in _HRMSContext.HR_Master_EarningsDeductions
                                     where mm.MI_Id.Equals(dto.MI_Id)
                                     && emp.HRMED_Id == med.HRMED_Id
                                     && med.HRMED_EDTypeFlag == "Loan" && emp.HREED_ActiveFlag == true
                                     && mm.HRME_ActiveFlag == true && emp.HRME_Id == mm.HRME_Id
                                     orderby mm.HRME_EmployeeOrder
                                     select mm).Distinct().OrderBy(t => t.HRME_EmployeeOrder).ToList();



                dto.employeedropdown = employees.ToArray();
                    if (employees.Count() > 0)
                    {

                        var empIds = employees.Select(t => t.HRME_Id).ToArray();

                        datalist = _HRMSContext.HR_Emp_Loan.Where(t => t.MI_Id.Equals(dto.MI_Id) && empIds.Contains(t.HRME_Id)).ToList();
                        dto.emploanList = datalist.ToArray();
                    }

              

                masteloan = _HRMSContext.HRMasterLoan.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLN_ActiveFlag == true).ToList();
                dto.masterloandropdown = masteloan.ToArray();

                

               

            }
      catch (Exception ee)
      {
        Console.WriteLine(ee.Message);
      }

      return dto;
    }


        public string GenerateNumber(Master_NumberingDTO en)
            {
            string GeneratedNumber = "";

            if (en != null)
                {
                if (en.IMN_AutoManualFlag == "Manual")
                    {
                    GeneratedNumber = "";
                    }
                else if (en.IMN_AutoManualFlag == "Auto")
                    {
                    var acyr = _Context.AcademicYear.Where(t => t.ASMAY_Id.Equals(en.ASMAY_Id)).FirstOrDefault();
                    string AcadYear = acyr.ASMAY_Year;
                    // string[] a = AcadYear.Split('-');

                    // AcadYear = a.ElementAt(0);

                    if (en.IMN_PrefixAcadYearCode == true)
                        {
                        GeneratedNumber = AcadYear;
                        }

                    else
                    if (en.IMN_PrefixParticular != "" && en.IMN_PrefixParticular != null)
                        {
                        GeneratedNumber = en.IMN_PrefixParticular;
                        }

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                        {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                            {
                            if (en.IMN_StartingNo != "" && en.IMN_StartingNo != null)
                                {
                                GeneratedNumber = GeneratedNumber + "/" + en.IMN_StartingNo.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                                }
                            else
                                {
                                en.IMN_StartingNo = "0";
                                GeneratedNumber = GeneratedNumber + "/" + en.IMN_StartingNo.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                                }
                            }
                        else
                            {
                            if (en.IMN_StartingNo != "" && en.IMN_StartingNo != null)
                                {
                                GeneratedNumber = GeneratedNumber + "/" + en.IMN_StartingNo.ToString();
                                }
                            }

                        }
                    else
                        {
                        if (en.IMN_StartingNo != "" && en.IMN_StartingNo != null)
                            {
                            GeneratedNumber = GeneratedNumber + "/" + en.IMN_StartingNo.ToString();
                            }
                        }

                    if (en.IMN_SuffixAcadYearCode == true)
                        {
                        GeneratedNumber = GeneratedNumber + "/" + AcadYear;
                        }
                    else if (en.IMN_SuffixParticular != "" && en.IMN_SuffixParticular != null)
                        {
                        GeneratedNumber = GeneratedNumber + "/" + en.IMN_SuffixParticular;
                        }

                    GeneratedNumber = TransactionNumberingType(GeneratedNumber, en);
                    }
                }

            return GeneratedNumber;

            }

        public string TransactionNumberingType(string GeneratedNumber, Master_NumberingDTO en)
            {
            if (en.IMN_Flag == "Loan")
                {
                GeneratedNumber = checkDublicateandIncreamentForLoan(GeneratedNumber, en);
                }
            return GeneratedNumber;
            }


        //Loan
        public string checkDublicateandIncreamentForLoan(string GeneratedNumber, Master_NumberingDTO en)
            {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
                {
                count = _HRMSContext.HR_Emp_Loan.Where(imp => imp.MI_Id == en.MI_Id && imp.HREL_EmpLoanId.Equals(GeneratedNumber)).Count();
                }
            else if (en.IMN_RestartNumFlag == "Yearly")
                {
                count = _HRMSContext.HR_Emp_Loan.Where(imp => imp.MI_Id == en.MI_Id && imp.HREL_EmpLoanId.Equals(GeneratedNumber)).Count();
                }
            if (count > 0)
                {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                    {
                    string lastfield = "";
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
                        {
                        lastfield = "/" + lastRecordArray.ElementAt(2);
                        }

                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                        {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                            {
                            if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
                                {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + lastfield;
                                }
                            else
                                {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                                }

                            }
                        else
                            {
                            if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
                                {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + lastfield;
                                }
                            else
                                {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
                                }
                            }
                        }
                    else
                        {
                        if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
                            {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + lastfield;
                            }
                        else
                            {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
                            }
                        }
                    }

                GeneratedNumber = checkDublicateandIncreamentForLoan(GeneratedNumber, en);
                }

            return GeneratedNumber;
            }





        }
    }
