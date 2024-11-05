using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
namespace PortalHub.com.vaps.Employee.Services
{
    public class EmployeeForm16Impl : Interfaces.EmployeeForm16Interface
    {
        public FeeGroupContext _fees;
        public HRMSContext _hrms;
        public ExamContext _exm;
        public DomainModelMsSqlServerContext _Context;
        public EmployeeForm16Impl(HRMSContext hrms, FeeGroupContext fees, ExamContext exm, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _hrms = hrms;
            _fees = fees;
            _exm = exm;
            _Context = MsSqlServerContext;

        }

        public Employee16DTO getdata(Employee16DTO dto)
        {
            try
            {
                dto.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                dto.leaveyeardropdown = _hrms.IVRM_Master_FinancialYear.ToArray();
                dto.empDetails = (from abc in _hrms.MasterEmployee
                                  where abc.HRME_Id == dto.HRME_Id
                                  select new Employee16DTO
                                  {
                                      HRME_EmployeeFirstName = ((abc.HRME_EmployeeFirstName == null ? " " : abc.HRME_EmployeeFirstName) + (abc.HRME_EmployeeMiddleName == null ? " " : abc.HRME_EmployeeMiddleName) + (abc.HRME_EmployeeLastName == null ? " " : abc.HRME_EmployeeLastName)).Trim(),
                                      hrme_address = ((abc.HRME_PerStreet == null ? " " : abc.HRME_PerStreet) + (abc.HRME_PerArea == null ? " " : abc.HRME_PerArea) + (abc.HRME_PerCity == null ? " " : abc.HRME_PerCity)).Trim(),
                                      HRME_PFAccNo = (abc.HRME_PFAccNo == null ? " " : abc.HRME_PFAccNo),
                                      HRME_FatherName = (abc.HRME_FatherName == null ? " " : abc.HRME_FatherName),
                                      HRME_PerCity = (abc.HRME_PerCity == null ? " " : abc.HRME_PerCity),
                                      HRME_PANCardNo=abc.HRME_PANCardNo,
                                      HRME_PerPincode = abc.HRME_PerPincode
                                  }).ToArray();

                dto.designation = (from a in _hrms.MasterEmployee
                                   from b in _hrms.HR_Master_Designation
                                   where a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id && a.HRMDES_Id == b.HRMDES_Id
                                   select new Employee16DTO
                                   {
                                       HRMDES_DesignationName = b.HRMDES_DesignationName
                                   }).ToArray();

                //dto..masterinstitution= (from xyz in _hrms.
                //                         where abc.HRME_Id == dto.HRME_Id
                //                         select new Employee16DTO
                //                  {



                Institution institute = new Institution();
                institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                dto.institutionDetails = dmoObj;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    }
}

