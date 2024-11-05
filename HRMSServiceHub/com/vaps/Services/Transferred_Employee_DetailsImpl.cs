using DataAccessMsSqlServerProvider;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Services
{
    public class Transferred_Employee_DetailsImpl:Interfaces.Transferred_Employee_DetailsInterface
    {
        public HRMSContext _HRMSContext;
        public Transferred_Employee_DetailsImpl(HRMSContext HRMSContext)
        {
            _HRMSContext = HRMSContext;
        }

        public EmployeeReportsDTO getvalue(EmployeeReportsDTO data)
        {
            if (data.FormatType == "transfered")
            {
                data.employeeDetails = (from a in _HRMSContext.MasterEmployee
                                        from b in _HRMSContext.Institution
                                        from c in _HRMSContext.HR_Master_Department
                                        from d in _HRMSContext.HR_Master_Designation
                                        where (a.MI_Id == data.MI_Id && a.HRME_TransfferedTo != null && a.HRME_TransfferedTo == b.MI_Id && a.HRMD_Id == c.HRMD_Id && a.HRMDES_Id == d.HRMDES_Id)
                                        select new EmployeeProfileReportDTO
                                        {
                                            HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                            HRME_EmployeeCode = a.HRME_EmployeeCode,
                                            HRMD_DepartmentName = c.HRMD_DepartmentName,
                                            HRMDES_DesignationName = d.HRMDES_DesignationName,
                                            Institutionname = b.MI_Name
                                        }).ToArray();
            }
            else if (data.FormatType == "retired")
            {
                data.employeeDetails = (from a in _HRMSContext.MasterEmployee
                                        from c in _HRMSContext.HR_Master_Department
                                        from d in _HRMSContext.HR_Master_Designation
                                        from e in _HRMSContext.masterLeavingReasonDMO
                                        where (a.MI_Id == data.MI_Id && a.HRMD_Id == c.HRMD_Id && a.HRMDES_Id == d.HRMDES_Id && a.HRME_LeavingReason == e.HRMLREA_LeavingReason && e.HRMLREA_LeavingReason == "Retired")
                                        select new EmployeeProfileReportDTO
                                        {
                                            HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                            HRME_EmployeeCode = a.HRME_EmployeeCode,
                                            HRMD_DepartmentName = c.HRMD_DepartmentName,
                                            HRMDES_DesignationName = d.HRMDES_DesignationName
                                        }).ToArray();
            }

            return data;
        }
    }
}
