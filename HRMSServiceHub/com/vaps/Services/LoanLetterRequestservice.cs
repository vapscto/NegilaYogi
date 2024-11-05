using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace HRMSServicesHub.com.vaps.Services
{
    public class LoanLetterRequestservice : Interfaces.LoannonDeductLetterInterface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public LoanLetterRequestservice(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }
        public HR_Emp_Loan_TransactionDTO getBasicData(HR_Emp_Loan_TransactionDTO dto)
        {
            //dto.retrunMsg = "";

            List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_Master_LeaveYearDMO> leaveyear = new List<HR_Master_LeaveYearDMO>();
            List<Month> Monthlist = new List<Month>();
            List<HR_Emp_Loan_TransactionDMO> LoanTransaction = new List<HR_Emp_Loan_TransactionDMO>();
            try
            {



                //var employeedropdown = (from e in _HRMSContext.MasterEmployee
                //                        from f in _HRMSContext.HR_Emp_Loan


                //                        where (e.MI_Id == dto.MI_Id && e.HRME_ActiveFlag == true && e.HRME_LeftFlag == false)
                //                        select new HR_Emp_Loan_TransactionDTO
                //                        {
                //                            hrmE_EmployeeFirstName = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) + (e.HRME_EmployeeMiddleName == null ? " " : e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null ? " " : e.HRME_EmployeeLastName)).Trim(),
                //                            HRME_Id = e.HRME_Id,
                //                        }).Distinct().ToList();
                var  employeedropdown = (from a in _HRMSContext.MasterEmployee
                                        from b in _HRMSContext.HR_Emp_Loan
                                        where (a.MI_Id == dto.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && b.MI_Id == a.MI_Id && b.HRME_Id == a.HRME_Id && b.HREL_ActiveFlag == true)
                                        select new HR_Emp_Loan_TransactionDTO
                                        {
                                            hrmE_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                            HRME_Id = a.HRME_Id
                                        }).Distinct().ToList();

                dto.leaveyeardropdown = leaveyear.ToArray();

                dto.employeedropdown = employeedropdown.ToArray();

                if (employeedropdown.Count > 0)
                {
                    List<HRMasterLoan> loanname = new List<HRMasterLoan>();
                    loanname = _HRMSContext.HRMasterLoan.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLN_ActiveFlag == true).ToList();
                    dto.loandrop = loanname.ToArray();

                }

                leaveyear = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_ActiveFlag == true).ToList();
                dto.leaveyeardropdown = leaveyear.OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();

                Monthlist = _Context.month.Where(t => t.Is_Active == true).ToList();
                dto.monthdropdown = Monthlist.ToArray();

                var loanprocess = (from a in _HRMSContext.MasterEmployee
                                   from b in _HRMSContext.HR_Emp_Loan
                                   from c in _HRMSContext.HR_Emp_Loan_Transaction
                                   where (a.MI_Id == dto.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && b.MI_Id == a.MI_Id && b.HRME_Id == a.HRME_Id && c.HREL_Id == b.HREL_Id && c.HRME_Id == b.HRME_Id && a.MI_Id == c.MI_Id)
                                   select new HR_Emp_Loan_TransactionDTO
                                   {
                                       hrmE_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                       HRME_Id = a.HRME_Id,
                                       HRELT_Year=c.HRELT_Year,
                                       HRELT_Month=c.HRELT_Month,
                                       HRELT_Reason=c.HRELT_Reason
                                   }).Distinct().ToList();






              //  LoanTransaction = _HRMSContext.HR_Emp_Loan_Transaction.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
               dto.gridoption = loanprocess.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Emp_Loan_TransactionDTO SaveUpdate(HR_Emp_Loan_TransactionDTO dto)
        {
            try
            {
                if(dto.HRELT_Id==0)
                {
                    dto.retrunMsg = "";
                   
                        var loandetails = _HRMSContext.HR_Emp_Loan.Where(t => t.MI_Id == dto.MI_Id && t.HRME_Id == dto.HRME_Id && t.HREL_ActiveFlag == true && t.HRMLN_Id == dto.HRMLN_Id).Select(t => t.HREL_Id).Distinct().ToList();

                        foreach (var z in loandetails)
                        {
                        dto.HREL_Id = z;                           
                        //var alldata = _HRMSContext.HR_Emp_Loan_TransactionDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRME_Id == dto.HRME_Id);
                        var dupli_cnt = _HRMSContext.HR_Emp_Loan_Transaction.Where(t => t.MI_Id == dto.MI_Id && t.HRME_Id == dto.HRME_Id && t.HREL_Id == dto.HREL_Id).Count();
                        if(dupli_cnt==0)
                        {
                            HR_Emp_Loan_TransactionDMO dmoObj = Mapper.Map<HR_Emp_Loan_TransactionDMO>(dto);
                            dmoObj.HRELT_PaidFlag = false;
                            dmoObj.HRELT_LoanAmount = 0;
                            dmoObj.HRELT_PrincipalAmount = 0;
                            dmoObj.HRELT_InterestAmount = 0;
                            dmoObj.CreatedDate = DateTime.Now;
                            dmoObj.UpdatedDate = DateTime.Now;
                            _HRMSContext.Add(dmoObj);
                        }
                        else
                        {
                            dto.retrunMsg = "Duplicate";
                            return dto;
                        }

                        }
                    var exists = _HRMSContext.SaveChanges();
                    if(exists>=1)
                    {
                        dto.returnval = true;
                    }
                    else
                    {
                        dto.returnval = false;
                    }
                   
                }
                else if (dto.HRELT_Id > 0)
                {

                }
               
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";

            }

            return dto;
        }

        public HR_Emp_Loan_TransactionDTO get_loans(HR_Emp_Loan_TransactionDTO data)
        {
            try
            {


                data.loandrop = (from a in _HRMSContext.HR_Emp_Loan
                                 from b in _HRMSContext.HRMasterLoan
                                 where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && a.HREL_ActiveFlag == true && a.HRME_Id == data.HRME_Id && b.HRMLN_Id == a.HRMLN_Id && b.HRMLN_ActiveFlag == true)
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


            
            
        
    
