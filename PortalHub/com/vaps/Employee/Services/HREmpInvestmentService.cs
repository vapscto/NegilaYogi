using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.HRMS;
using AutoMapper;

namespace PortalHub.com.vaps.Employee.Services
{
    public class HREmpInvestmentService : Interfaces.HREmpInvestmentInterface
    {
        public HRMSContext _hrms;
        public TTContext _tt;
        public ExamContext _exm;
        public FOContext _FOContext;
        public COEContext _COEContext;
        public PortalContext _PortalContext;
        public DomainModelMsSqlServerContext _Context;
        public HREmpInvestmentService(HRMSContext hrms, TTContext tt, FOContext fOContext, ExamContext exm, COEContext COEContext, PortalContext portalContext)
        {
            _hrms = hrms;
            _tt = tt;
            _COEContext = COEContext;
            _FOContext = fOContext;
            _exm = exm;
            _PortalContext = portalContext;
        }

        public EmployeeInvestmentDTO getBasicData(EmployeeInvestmentDTO dto)
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

    public EmployeeInvestmentDTO SaveUpdate(EmployeeInvestmentDTO dto)
    {
      dto.retrunMsg = "";
           
            

            try
            {


                HR_Employee_Investment dmoObj = Mapper.Map<HR_Employee_Investment>(dto);

                var duplicatecountresult = _hrms.HR_Employee_Investment.Where(t => t.MI_Id == dto.MI_Id && t.HREID_Id == dto.HREID_Id/* && t.HRMCVIA_Id == dto.HRMCVIA_Id*/).Count();

                
                if (duplicatecountresult > 0)
                {
                    var result = _hrms.HR_Employee_Investment.Single(t => t.HREID_Id == dmoObj.HREID_Id);

                    dto.HREID_UpdatedBy = dto.LogInUserId;
                    dto.HREID_ActiveFlg = true;
                    Mapper.Map(dto, result);
                    _hrms.Update(result);
                    var flag = _hrms.SaveChanges();
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
                    var duplicatecount = _hrms.HR_Employee_Investment.Where(t => t.MI_Id == dto.MI_Id && t.HREID_Id == dto.HREID_Id && t.IMFY_Id == dto.IMFY_Id && t.HRME_Id == dto.HRME_Id).Count();
                    if (duplicatecount == 0)
                    {
                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.HRME_Id = dto.HRME_Id;
                        dmoObj.HRMCVIA_Id = dto.HRMCVIA_Id;
                        dmoObj.IMFY_Id = dto.IMFY_Id;
                        dmoObj.HREID_Amount = dto.HREID_Amount == null ? 0 : Convert.ToDecimal(dto.HREID_Amount);
                        dmoObj.HREID_ActiveFlg = true;

                        _hrms.Add(dmoObj);
                        var flag = _hrms.SaveChanges();
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

    public EmployeeInvestmentDTO editData(int id)
    {

            EmployeeInvestmentDTO dto = new EmployeeInvestmentDTO();
      dto.retrunMsg = "";
      try
      {
       List<HR_Employee_Investment> lorg = new List<HR_Employee_Investment>();
        lorg = _hrms.HR_Employee_Investment.Where(t => t.HREID_Id
.Equals(id)).ToList();
        dto.emploanList = lorg.ToArray();

       dto = Mapper.Map<EmployeeInvestmentDTO>(lorg.FirstOrDefault());
       dto.emploanList = lorg.ToArray();

     
     }
      catch (Exception ee)
      {
        Console.WriteLine(ee.Message);
        dto.retrunMsg = "Error occured";
      }

      return dto;
    }

        public EmployeeInvestmentDTO getDetailsByEmployee(EmployeeInvestmentDTO dto)
            {
            dto.empGrossSal = 0;
           // dto.totalAppliedAmount = 0;


            try
                {
          



                }
            catch (Exception ee)
                {
                Console.WriteLine(ee.Message);
                }


            return dto;
            }


        public EmployeeInvestmentDTO deactivate(EmployeeInvestmentDTO dto)
    {
      dto.retrunMsg = "";
      try
      {
                if (dto.HREID_Id
 > 0)
                {
                    var result = _hrms.HR_Employee_Investment.Single(t => t.HREID_Id
 == dto.HREID_Id
);

                    if (result.HREID_ActiveFlg
 == true)
                    {
                        result.HREID_ActiveFlg
 = false;
                    }
                    else if (result.HREID_ActiveFlg
 == false)
                    {
                        result.HREID_ActiveFlg
 = true;
                    }
                    //result.UpdatedDate = DateTime.Now;

                    _hrms.Update(result);
                    var flag = _hrms.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HREID_ActiveFlg
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


        public EmployeeInvestmentDTO GetAllDropdownAndDatatableDetails(EmployeeInvestmentDTO dto)
        {
            try
            {
                dto.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == dto.LogInUserId && c.MI_Id == dto.MI_Id).Emp_Code;

                //dto.HRME_Id = _hrms.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                var employees = (from emp in _hrms.HR_Employee_EarningsDeductions
                                    from mm in _hrms.MasterEmployee
                                    from med in _hrms.HR_Master_EarningsDeductions
                                    where mm.MI_Id.Equals(dto.MI_Id)
                                    && emp.HRMED_Id == med.HRMED_Id
                               
                                    && mm.HRME_ActiveFlag == true && emp.HRME_Id == mm.HRME_Id && mm.HRME_Id == dto.HRME_Id
                                    orderby mm.HRME_EmployeeOrder
                                    select mm).Distinct().OrderBy(t => t.HRME_EmployeeOrder).ToList();

                dto.employeedropdown = employees.ToArray();
                dto.leaveyeardropdown = _hrms.IVRM_Master_FinancialYear.ToArray();
                if (employees.Count() > 0)
                {
                    var empIds = employees.Select(t => t.HRME_Id);
                    var datalists = (from emp in _hrms.HR_Employee_Investment
                                     from hr in _hrms.MasterEmployee
                                     from ch in _hrms.HR_master_ChapterVI
                                     from kk in _hrms.IVRM_Master_FinancialYear
                                     where (emp.HRME_Id == hr.HRME_Id && emp.HRMCVIA_Id == ch.HRMCVIA_Id && hr.MI_Id == dto.MI_Id && empIds.Contains(emp.HRME_Id) &&  kk.IMFY_Id == emp.IMFY_Id)
                                     select new EmployeeInvestmentDTO
                                     {
                                         HRMCVIA_Id = ch.HRMCVIA_Id,
                                         HRMCVIA_SectionName = ch.HRMCVIA_SectionName,
                                         HREID_Amount = emp.HREID_Amount,
                                         HRME_EmployeeFirstName = ((hr.HRME_EmployeeFirstName == null ? " " : hr.HRME_EmployeeFirstName) + (hr.HRME_EmployeeMiddleName == null ? " " : hr.HRME_EmployeeMiddleName) + (hr.HRME_EmployeeLastName == null ? " " : hr.HRME_EmployeeLastName)).Trim(),
                                         HREID_Id=emp.HREID_Id,
                                         IMFY_FinancialYear = kk.IMFY_FinancialYear,
                                         HREID_ActiveFlg = emp.HREID_ActiveFlg,
                                     }).Distinct().ToList();
                    dto.emploanList = datalists.ToArray();
                }

                dto.allowancedropdown = _hrms.HR_master_ChapterVI.Where(m => m.MI_Id == dto.MI_Id).ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
    }   
}
