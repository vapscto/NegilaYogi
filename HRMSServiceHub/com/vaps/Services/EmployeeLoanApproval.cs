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
    public class EmployeeLoanApproval : Interfaces.LoanApprovalInterface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;

        public EmployeeLoanApproval(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public HR_Emp_Loan_ApprovalDTO getBasicData(HR_Emp_Loan_ApprovalDTO dto)
        {

            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;

           
        }


        public HR_Emp_Loan_ApprovalDTO GetAllDropdownAndDatatableDetails(HR_Emp_Loan_ApprovalDTO dto)
        {

            try
            {
                List<HR_Emp_Loan> datalist = new List<HR_Emp_Loan>();


                List<HR_Process_Auth_OrderNo> processdetails = new List<HR_Process_Auth_OrderNo>();

                List<HR_PROCESSDMO> process = new List<HR_PROCESSDMO>();               

                var pro = (from a in _HRMSContext.HR_Process_Auth_OrderNoDMO
                           from b in _HRMSContext.HR_PROCESSDMO
                           where a.HRPA_Id == b.HRPA_Id && a.IVRMUL_Id == dto.IVRMUL_Id 
                           && b.HRPA_TypeFlag == "Loan"
                           select a).ToList();


                  

                if (pro.Count() > 0)
                {

                    if (pro.FirstOrDefault().HRPAON_FinalFlg)//final authority to saction or reject
                    {

                        var gridlist = (
                                                     from a in _HRMSContext.HR_Emp_Loan
                                                     from b in _HRMSContext.MasterEmployee
                                                     from c in _HRMSContext.HRMasterLoan
                                                     where a.HRME_Id == b.HRME_Id && a.HRMLN_Id == c.HRMLN_Id && a.MI_Id == dto.MI_Id && a.HREL_ActiveFlag == true && a.HREL_LoanStatus == "UnderProcess"
                                                     select new HR_Emp_LoanDTO
                                                     {

                                                         hrmE_EmployeeFirstName = (b.HRME_EmployeeFirstName != null ? b.HRME_EmployeeFirstName : "") + " " + (b.HRME_EmployeeMiddleName != null ? b.HRME_EmployeeMiddleName : "") + " " + (b.HRME_EmployeeLastName != null ? b.HRME_EmployeeLastName : ""),
                                                         HRME_Id = a.HRME_Id,
                                                         HREL_AppliedDate = a.HREL_AppliedDate,
                                                         HREL_LoanAmount = a.HREL_LoanAmount,
                                                         HRML_LoanType = c.HRML_LoanType
                                                     }).Distinct().ToList();

                        dto.griddisplay = gridlist.ToArray();

                    }
                    else//Except final authority to proceed further next level or reject
                    {
                        var gridlist = (
                                                     from a in _HRMSContext.HR_Emp_Loan
                                                     from b in _HRMSContext.MasterEmployee
                                                     from c in _HRMSContext.HRMasterLoan
                                                     where a.HRME_Id == b.HRME_Id && a.HRMLN_Id == c.HRMLN_Id && a.MI_Id == dto.MI_Id && a.HREL_ActiveFlag == true && a.HREL_LoanStatus == "Applied"
                                                     select new HR_Emp_LoanDTO
                                                     {

                                                         hrmE_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                                         HREL_AppliedDate = a.HREL_AppliedDate,
                                                         HREL_LoanAmount=a.HREL_LoanAmount,
                                                            HRML_LoanType = c.HRML_LoanType
                                                     }).Distinct().ToList();

                        dto.griddisplay = gridlist.ToArray();

                    }




                }else
                {
                    dto.returnMsg = "UserNotMapped";
                }

            }
            catch (Exception e)
            {

                dto.returnMsg = "Error";
            }

            return dto;

        }
    }
    
}
