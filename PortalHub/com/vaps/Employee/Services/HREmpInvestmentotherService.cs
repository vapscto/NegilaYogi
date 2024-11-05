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
    public class HREmpInvestmentotherService : Interfaces.HREmpInvestmentotherInterface
    {
        public FeeGroupContext _fees;
        public ExamContext _exm;
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
    public HREmpInvestmentotherService(HRMSContext HRMSContext, FeeGroupContext fees, ExamContext exm, DomainModelMsSqlServerContext Context)
    {
        _HRMSContext = HRMSContext;
        _Context = Context;
        _fees = fees;
        _exm = exm;
    }

    public EmployeeInvestmentothersDTO getBasicData(EmployeeInvestmentothersDTO dto)
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

    public EmployeeInvestmentothersDTO SaveUpdate(EmployeeInvestmentothersDTO dto)
    {
        dto.retrunMsg = "";
        

            try
            {


                HR_Employee_Subsection_Investment_other dmoObj = Mapper.Map<HR_Employee_Subsection_Investment_other>(dto);

                var duplicatecountresult = _HRMSContext.HR_Employee_Subsection_Investment_other.Where(t => t.MI_Id == dto.MI_Id && t.HREIDO_Id == dto.HREIDO_Id && t.IMFY_Id == dto.IMFY_Id && t.HRMCVIA_Id==dto.HRMCVIA_Id).Count();


                if (duplicatecountresult > 0)
                {
                    var result = _HRMSContext.HR_Employee_Subsection_Investment_other.Single(t => t.HREIDO_Id == dmoObj.HREIDO_Id);

                    dto.HREID_UpdatedBy = dto.LogInUserId;
                    dto.HREID_ActiveFlg = true;
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
                    var duplicatecount = _HRMSContext.HR_Employee_Subsection_Investment_other.Where(t => t.MI_Id == dto.MI_Id && t.HRME_Id == dto.HRME_Id && t.IMFY_Id == dto.IMFY_Id && t.HRMCVIA_Id==dto.HRMCVIA_Id).Count();
                    if (duplicatecount == 0)
                    {
                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.HRME_Id = dto.HRME_Id;
                        dmoObj.HRMCVIA_Id = dto.HRMCVIA_Id;
                        dmoObj.IMFY_Id = dto.IMFY_Id;
                        dmoObj.HREIDO_RentPaid = dto.HREIDO_RentPaid;
                        dmoObj.HREIDO_RentPaidEvidence = dto.HREIDO_RentPaidEvidence;
                        dmoObj.HREIDO_NameOfLandLord = dto.HREIDO_NameOfLandLord;
                        dmoObj.HREIDO_NameEvidence = dto.HREIDO_NameEvidence;
                        dmoObj.HREIDO_LandLordAddress = dto.HREIDO_LandLordAddress;
                        dmoObj.HREIDO_AddressEvidence = dto.HREIDO_AddressEvidence;
                        dmoObj.HREIDO_LandLordPAN = dto.HREIDO_LandLordPAN;
                        dmoObj.HREIDO_PANEvidence = dto.HREIDO_PANEvidence;
                        dmoObj.HREIDO_TravelConcession = dto.HREIDO_TravelConcession;
                        dmoObj.HREIDO_ConcessionEvidence = dto.HREIDO_ConcessionEvidence;
                        dmoObj.HREIDO_InterestPaid = dto.HREIDO_InterestPaid;
                        dmoObj.HREIDO_InterestEvidence = dto.HREIDO_InterestEvidence;
                        dmoObj.HREIDO_LenderName = dto.HREIDO_LenderName;
                        dmoObj.HREIDO_LNameEvidence = dto.HREIDO_LNameEvidence;
                        dmoObj.HREIDO_LenderAddress = dto.HREIDO_LenderAddress;
                        dmoObj.HREIDO_LAddressEvidence = dto.HREIDO_LAddressEvidence;
                        dmoObj.HREIDO_LenderPAN = dto.HREIDO_LenderPAN;
                        dmoObj.HREIDO_LPANEvidence = dto.HREIDO_LPANEvidence;
                        dmoObj.HREIDO_FinanceInst = dto.HREIDO_FinanceInst;
                        dmoObj.HREIDO_InstEvidence = dto.HREIDO_InstEvidence;
                        dmoObj.HREIDO_Employer = dto.HREIDO_Employer;
                        dmoObj.HREIDO_EmpEvidence = dto.HREIDO_EmpEvidence;
                        dmoObj.HREIDO_Others = dto.HREIDO_Others;
                        dmoObj.HREIDO_OthersEvidence = dto.HREIDO_OthersEvidence;
                        dmoObj.HREIDO_ActiveFlg = true;
                        dmoObj.HREIDO_UpdatedBy = dto.LogInUserId;
                        dmoObj.HREIDO_CreatedBy = dto.LogInUserId;
                        dmoObj.HREIDO_PANEvidence = dto.HREIDO_PANEvidence;
                        dmoObj.HREIDO_NameEvidence = dto.HREIDO_NameEvidence;
                        dmoObj.HREIDO_AddressEvidence = dto.HREIDO_AddressEvidence;
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

    public EmployeeInvestmentothersDTO editData(int id)
    {
        EmployeeInvestmentothersDTO dto = new EmployeeInvestmentothersDTO();
        dto.retrunMsg = "";
        try
        {
            List<HR_Employee_Subsection_Investment_other> lorg = new List<HR_Employee_Subsection_Investment_other>();
            lorg = _HRMSContext.HR_Employee_Subsection_Investment_other.Where(t => t.HREIDO_Id.Equals(id)).ToList();
            dto.emploanList = lorg.ToArray();

            dto = Mapper.Map<EmployeeInvestmentothersDTO>(lorg.FirstOrDefault());
            dto.emploanList = lorg.ToArray();       
        }
        catch (Exception ee)
        {
            Console.WriteLine(ee.Message);
            dto.retrunMsg = "Error occured";
        }
        return dto;
    }

    public EmployeeInvestmentothersDTO getDetailsByEmployee(EmployeeInvestmentothersDTO dto)
    {
        dto.empGrossSal = 0;
        try
        {
        }
        catch (Exception ee)
        {
        Console.WriteLine(ee.Message);
        }
        return dto;
    }

    public EmployeeInvestmentothersDTO deactivate(EmployeeInvestmentothersDTO dto)
    {
        dto.retrunMsg = "";
        try
        {
            if (dto.HREIDO_Id> 0)
            {
                var result = _HRMSContext.HR_Employee_Subsection_Investment_other.Single(t => t.HREIDO_Id == dto.HREIDO_Id);

                if (result.HREIDO_ActiveFlg== true)
                {
                    result.HREIDO_ActiveFlg= false;
                }
                else if (result.HREIDO_ActiveFlg== false)
                {
                    result.HREIDO_ActiveFlg= true;
                }

                _HRMSContext.Update(result);
                var flag = _HRMSContext.SaveChanges();
                if (flag > 0)
                {
                    if (result.HREIDO_ActiveFlg == true)
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


    public EmployeeInvestmentothersDTO GetAllDropdownAndDatatableDetails(EmployeeInvestmentothersDTO dto)
    {
        List<HR_Employee_Subsection_Investment_other> datalist = new List<HR_Employee_Subsection_Investment_other>();
        List<MasterEmployee> employe = new List<MasterEmployee>();
        List<HRMasterLoan> masteloan = new List<HRMasterLoan>();
        try
        {
            //var IVRM_ModeOfPayment = _HRMSContext.IVRM_ModeOfPayment.Where(t => t.IVRMMOD_ActiveFlag == true).ToList();
            //dto.modeOfPaymentdropdown = IVRM_ModeOfPayment.ToArray();

            //HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

            //HR_ConfigurationDTO dmoObj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
            //dto.configurationDetails = dmoObj;

            dto.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == dto.LogInUserId && c.MI_Id == dto.MI_Id).Emp_Code;

            var employees = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                from mm in _HRMSContext.MasterEmployee
                                from med in _HRMSContext.HR_Master_EarningsDeductions
                                where mm.MI_Id.Equals(dto.MI_Id)
                                && emp.HRMED_Id == med.HRMED_Id
                               
                                && mm.HRME_ActiveFlag == true && emp.HRME_Id == mm.HRME_Id && mm.HRME_Id == dto.HRME_Id
                                orderby mm.HRME_EmployeeOrder
                                select mm).Distinct().OrderBy(t => t.HRME_EmployeeOrder).ToList();

            dto.employeedropdown = employees.ToArray();

            dto.leaveyeardropdown = _HRMSContext.IVRM_Master_FinancialYear.ToArray();
            if (employees.Count() > 0)
            {
                var empIds = employees.Select(t => t.HRME_Id);

                var datalists = _HRMSContext.HR_master_ChapterVI.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                dto.emploanList = datalists.ToArray();

                //var datalists1 = _HRMSContext.HR_Employee_Subsection_Investment_other.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                //dto.emploangrid = datalists1.ToArray();

            }

            var datalists1 = (from emp in _HRMSContext.HR_Employee_Subsection_Investment_other
                                from hr in _HRMSContext.MasterEmployee
                                from ch in _HRMSContext.HR_master_ChapterVI
                                from kk in _HRMSContext.IVRM_Master_FinancialYear
                                where (emp.HRME_Id == hr.HRME_Id && emp.HRMCVIA_Id == ch.HRMCVIA_Id && hr.MI_Id == dto.MI_Id && kk.IMFY_Id==emp.IMFY_Id)
                                select new EmployeeInvestmentothersDTO
                                {
                                    HRMCVIA_Id = ch.HRMCVIA_Id,
                                    MI_Id=hr.MI_Id,
                                    IMFY_FinancialYear = kk.IMFY_FinancialYear,
                                    HRMCVIA_SectionName = ch.HRMCVIA_SectionName,
                                    HREIDO_RentPaid = emp.HREIDO_RentPaid,
                                    HRME_EmployeeFirstName = ((hr.HRME_EmployeeFirstName == null ? " " : hr.HRME_EmployeeFirstName) + (hr.HRME_EmployeeMiddleName == null ? " " : hr.HRME_EmployeeMiddleName) + (hr.HRME_EmployeeLastName == null ? " " : hr.HRME_EmployeeLastName)).Trim(),
                                    HREIDO_Id = emp.HREIDO_Id,
                                    HREIDO_ActiveFlg=emp.HREIDO_ActiveFlg,
                                }).Distinct().ToList();

            dto.emploanListgrid = datalists1.ToArray();
        }
        catch (Exception ee)
        {
            Console.WriteLine(ee.Message);
        }

        return dto;
    }
    }   
}
