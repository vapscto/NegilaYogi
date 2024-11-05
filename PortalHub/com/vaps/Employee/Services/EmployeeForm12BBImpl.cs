using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PortalHub.com.vaps.Employee.Services
{
    public class EmployeeForm12BBImpl : Interfaces.EmployeeForm12BBInterface
    {
        public FeeGroupContext _fees;
        public HRMSContext _hrms;
        public ExamContext _exm;
        public EmployeeForm12BBImpl(HRMSContext hrms, FeeGroupContext fees, ExamContext exm)
        {
            _hrms = hrms;
            _fees = fees;
            _exm = exm;

        }

        public Employee12BBDTO getsalaryalldetails(Employee12BBDTO dto)
        {

            try
            {
                dto.finyear= _hrms.IVRM_Master_FinancialYear.Single(c => c.IMFY_Id == dto.IMFY_Id).IMFY_FinancialYear;
                dto.HRME_Id = _hrms.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                dto.investmentdetails = (from a in _hrms.HR_Employee_Subsection_Investment_other
                                             // from b in _hrms.HR_Employee_Investment
                                             // from c in _hrms.HR_master_ChapterVI
                                         from kk in _hrms.IVRM_Master_FinancialYear
                                         where a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id /*&& a.HRME_Id == b.HRME_Id && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && b.HRMCVIA_Id == c.HRMCVIA_Id */&& kk.IMFY_Id == a.IMFY_Id && kk.IMFY_Id==dto.IMFY_Id && a.HREIDO_ActiveFlg==true
                                         select new Employee12BBDTO
                                         {
                                             HREIDO_RentPaid = a.HREIDO_RentPaid,
                                             HREIDO_TravelConcession = a.HREIDO_TravelConcession,
                                             HREIDO_InterestPaid = a.HREIDO_InterestPaid,
                                             HREIDO_RentPaidEvidence = a.HREIDO_RentPaidEvidence,
                                             HREIDO_ConcessionEvidence = a.HREIDO_ConcessionEvidence,
                                             HREIDO_InterestEvidence = a.HREIDO_InterestEvidence,
                                             HREIDO_NameOfLandLord = a.HREIDO_NameOfLandLord,
                                             HREIDO_NameEvidence = a.HREIDO_NameEvidence,
                                             //  HREID_Amount = b.HREID_Amount,
                                             HREIDO_AddressEvidence = a.HREIDO_AddressEvidence,
                                             HREIDO_LenderAddress = a.HREIDO_LenderAddress,
                                             HREIDO_LAddressEvidence = a.HREIDO_LAddressEvidence,
                                             HREIDO_LenderPAN = a.HREIDO_LenderPAN,
                                             HREIDO_LPANEvidence = a.HREIDO_LPANEvidence,
                                             HREIDO_LandLordPAN = a.HREIDO_LandLordPAN,
                                             HREIDO_LandLordAddress = a.HREIDO_LandLordAddress,
                                             HREIDO_PANEvidence = a.HREIDO_PANEvidence,
                                             HREIDO_LenderName = a.HREIDO_LenderName,
                                             HREIDO_LNameEvidence = a.HREIDO_LNameEvidence,
                                             HREIDO_FinanceInst = a.HREIDO_FinanceInst,
                                             HREIDO_InstEvidence = a.HREIDO_InstEvidence,
                                             HREIDO_Employer = a.HREIDO_Employer,
                                             HREIDO_EmpEvidence = a.HREIDO_EmpEvidence,
                                             HREIDO_Others = a.HREIDO_Others,
                                             HREIDO_OthersEvidence = a.HREIDO_OthersEvidence
                                         }).ToArray();




                var chapter = (from ao in _hrms.HR_master_ChapterVI
                               from pa in _hrms.HR_Employee_Investment
                               from kk in _hrms.IVRM_Master_FinancialYear
                               where ao.HRMCVIA_Id == pa.HRMCVIA_Id && pa.HRME_Id == dto.HRME_Id && ao.MI_Id.Equals(dto.MI_Id) && ao.HRMCVIA_SectionCode == "80C" && pa.HREID_ActiveFlg==true && ao.HRMCVIA_ActiveFlg==true &&  kk.IMFY_Id == pa.IMFY_Id && kk.IMFY_Id == dto.IMFY_Id
                               select new Employee12BBDTO
                               {
                                   HRMCVIA_SectionName = ao.HRMCVIA_SectionName,

                                   HREID_Amount = pa.HREID_Amount

                               }).Distinct().ToList();

                dto.chapterlist = chapter.ToArray();

                var chapter1 = (from ao in _hrms.HR_master_ChapterVI
                               from pa in _hrms.HR_Employee_Investment
                                from kk in _hrms.IVRM_Master_FinancialYear
                                where ao.HRMCVIA_Id == pa.HRMCVIA_Id && pa.HRME_Id == dto.HRME_Id && ao.MI_Id.Equals(dto.MI_Id) && ao.HRMCVIA_SectionCode == "80E" && pa.HREID_ActiveFlg == true && ao.HRMCVIA_ActiveFlg == true && kk.IMFY_Id == pa.IMFY_Id && kk.IMFY_Id == dto.IMFY_Id
                                select new Employee12BBDTO
                               {
                                   HRMCVIA_SectionName = ao.HRMCVIA_SectionName,

                                   HREID_Amount = pa.HREID_Amount

                               }).Distinct().ToList();

                dto.chapterlist80E = chapter1.ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Employee12BBDTO getdata(Employee12BBDTO dto)
        {
        try
        {
            dto.HRME_Id = _hrms.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
            dto.leaveyeardropdown = _hrms.IVRM_Master_FinancialYear.ToArray();
            dto.empDetails = (from abc in _hrms.MasterEmployee
                                where abc.HRME_Id == dto.HRME_Id
                                select new Employee12BBDTO
                                {
                                    HRME_EmployeeFirstName = ((abc.HRME_EmployeeFirstName == null ? " " : abc.HRME_EmployeeFirstName) + (abc.HRME_EmployeeMiddleName == null ? " " : abc.HRME_EmployeeMiddleName) + (abc.HRME_EmployeeLastName == null ? " " : abc.HRME_EmployeeLastName)).Trim(),
                                    hrme_address = ((abc.HRME_PerStreet == null ? " " : abc.HRME_PerStreet) + (abc.HRME_PerArea == null ? " " : abc.HRME_PerArea) + (abc.HRME_PerCity == null ? " " : abc.HRME_PerCity)).Trim(),
                                    HRME_PFAccNo = (abc.HRME_PFAccNo == null ? " " : abc.HRME_PFAccNo),
                                    HRME_FatherName = (abc.HRME_FatherName == null ? " " : abc.HRME_FatherName),
                                    HRME_PerCity = (abc.HRME_PerCity == null ? " " : abc.HRME_PerCity)
                                }).ToArray();

            dto.designation = (from a in _hrms.MasterEmployee
                                from b in _hrms.HR_Master_Designation
                                where a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id && a.HRMDES_Id == b.HRMDES_Id
                                select new Employee12BBDTO
                                {
                                    HRMDES_DesignationName = b.HRMDES_DesignationName
                                }).ToArray();

                var chapter = (from ao in _hrms.HR_master_ChapterVI
                               from pa in _hrms.HR_Employee_ChapterVI
                               where ao.HRMCVIA_Id == pa.HRMCVIA_Id && pa.HRME_Id == dto.HRME_Id && ao.MI_Id.Equals(dto.MI_Id) && ao.HRMCVIA_SectionCode == "80C"
                               select new Employee12BBDTO
                               {
                                   HRMCVIA_SectionName = ao.HRMCVIA_SectionName,

                                   HRECVIA_Amount = pa.HRECVIA_Amount

                               }).Distinct().ToList();

                dto.chapterlist = chapter.ToArray();



                dto.investmentdetails = (from a in _hrms.HR_Employee_Subsection_Investment_other
                                    // from b in _hrms.HR_Employee_Investment
                                    // from c in _hrms.HR_master_ChapterVI
                                   from kk in _hrms.IVRM_Master_FinancialYear
                                         where a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id /*&& a.HRME_Id == b.HRME_Id && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && b.HRMCVIA_Id == c.HRMCVIA_Id */&& kk.IMFY_Id==a.IMFY_Id 
                                        select new Employee12BBDTO
                                        {
                                            HREIDO_RentPaid = a.HREIDO_RentPaid,
                                            HREIDO_TravelConcession = a.HREIDO_TravelConcession,
                                            HREIDO_InterestPaid = a.HREIDO_InterestPaid,
                                            HREIDO_RentPaidEvidence = a.HREIDO_RentPaidEvidence,
                                            HREIDO_ConcessionEvidence = a.HREIDO_ConcessionEvidence,
                                            HREIDO_InterestEvidence = a.HREIDO_InterestEvidence,
                                         //   HRMCVIA_Id = c.HRMCVIA_Id,
                                          //  HRMCVIA_SectionName = c.HRMCVIA_SectionName,
                                         //  HREID_Amount = b.HREID_Amount,
                                            HREIDO_AddressEvidence=a.HREIDO_AddressEvidence,
                                            HREIDO_LenderAddress=a.HREIDO_LenderAddress,
                                            HREIDO_LAddressEvidence=a.HREIDO_LAddressEvidence,
                                            HREIDO_LenderPAN=a.HREIDO_LenderPAN,
                                            HREIDO_LPANEvidence=a.HREIDO_LPANEvidence,
                                            HREIDO_LandLordPAN=a.HREIDO_LandLordPAN,
                                            HREIDO_LandLordAddress=a.HREIDO_LandLordAddress,
                                            HREIDO_PANEvidence = a.HREIDO_PANEvidence,
                                            HREIDO_LenderName = a.HREIDO_LenderName,
                                            HREIDO_LNameEvidence=a.HREIDO_LNameEvidence,
                                            HREIDO_FinanceInst = a.HREIDO_FinanceInst,
                                            HREIDO_InstEvidence=a.HREIDO_InstEvidence,
                                            HREIDO_Employer=a.HREIDO_Employer,
                                            HREIDO_EmpEvidence=a.HREIDO_EmpEvidence,
                                            HREIDO_Others=a.HREIDO_Others,
                                            HREIDO_OthersEvidence=a.HREIDO_OthersEvidence
                                        }).ToArray();

        }
        catch (Exception ee)
        {
            Console.WriteLine(ee.Message);
        }
        return dto;
        }
    }


   
}

