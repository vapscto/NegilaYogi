using DataAccessMsSqlServerProvider;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Services
{
    public class QualificationReportImpl:Interfaces.QualificationReportInterface
    {
        public HRMSContext _HRMSContext;
        public QualificationReportImpl(HRMSContext HRMSContext)
        {
            _HRMSContext = HRMSContext;
        }
        public MasterEmployeeDTO getalldetails(MasterEmployeeDTO data)
        {
            try
            {               
                data.castlist = (from a in _HRMSContext.MasterEmployee
                                 from b in _HRMSContext.Caste
                                 where (a.MI_Id == b.MI_Id && a.CasteId == b.IMC_Id && b.MI_Id == data.MI_Id)
                                 select new MasterEmployeeDTO
                                 {
                                     IMC_Id = b.IMC_Id,
                                     IMC_CasteName = b.IMC_CasteName,
                                 }).Distinct().ToArray();

                data.qualificationlist = (from a in _HRMSContext.MasterEmployee
                                          from b in _HRMSContext.Master_Employee_Qulaification
                                          where (a.MI_Id == b.MI_Id && a.HRME_Id == b.HRME_Id && b.MI_Id == data.MI_Id)
                                          select new MasterEmployeeDTO
                                          {
                                              HRME_QualificationName = b.HRME_QualificationName
                                          }).Distinct().ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;            
        }
        public async Task<MasterEmployeeDTO> getQualificationReport(MasterEmployeeDTO data)
        {
            if (data.Type == "quli") {
                data.employeedetailList = (from a in _HRMSContext.MasterEmployee
                                           from b in _HRMSContext.HR_Master_Department
                                           from c in _HRMSContext.HR_Master_Designation
                                           from d in _HRMSContext.Master_Employee_Qulaification
                                           where (a.MI_Id == b.MI_Id && a.HRMD_Id == b.HRMD_Id && a.HRME_ActiveFlag == true && a.MI_Id == data.MI_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRME_Id == d.HRME_Id && d.MI_Id == data.MI_Id && d.HRME_QualificationName == data.HRME_QualificationName)
                                           select new MasterEmployeeDTO
                                           {
                                               HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                               HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                               HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                               HRME_EmployeeCode = a.HRME_EmployeeCode,
                                               HRMD_Id = a.HRMD_Id,
                                               HRMD_DepartmentName = b.HRMD_DepartmentName,
                                               HRMDES_Id = a.HRMDES_Id,
                                               HRMDES_DesignationName = c.HRMDES_DesignationName,
                                           }).Distinct().ToArray();
            }
            else if(data.Type == "cas")
            {
                data.employeedetailList = (from a in _HRMSContext.MasterEmployee
                                           from b in _HRMSContext.HR_Master_Department
                                           from c in _HRMSContext.HR_Master_Designation
                                           from d in _HRMSContext.Caste
                                           where (a.MI_Id == b.MI_Id && a.HRMD_Id == b.HRMD_Id && a.HRME_ActiveFlag == true && a.MI_Id == data.MI_Id && a.HRMDES_Id == c.HRMDES_Id && d.MI_Id==a.MI_Id && d.IMC_Id == a.CasteId && d.IMC_CasteName==data.IMC_CasteName)
                                           select new MasterEmployeeDTO
                                           {
                                               HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                               HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                               HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                               HRME_EmployeeCode = a.HRME_EmployeeCode,
                                               HRMD_Id = a.HRMD_Id,
                                               HRMD_DepartmentName = b.HRMD_DepartmentName,
                                               HRMDES_Id = a.HRMDES_Id,
                                               HRMDES_DesignationName = c.HRMDES_DesignationName,
                                           }).Distinct().ToArray();
            }            
            return data;
        }
    }
}
