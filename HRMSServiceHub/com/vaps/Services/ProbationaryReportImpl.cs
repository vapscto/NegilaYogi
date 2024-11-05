using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Services
{
    public class ProbationaryReportImpl:Interfaces.ProbationaryReportInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public ProbationaryReportImpl(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;
        }
        public EmployeeProfileReportDTO getalldetails(EmployeeProfileReportDTO dto)
        {          
            try
            {
                List<HR_Master_GroupType> staf_types = new List<HR_Master_GroupType>();
                staf_types = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id == dto.MI_Id && t.HRMGT_ActiveFlag == true).ToList(); 
                dto.filltypes = staf_types.Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }    
        public async Task<EmployeeProfileReportDTO> getProbationaryReport(EmployeeProfileReportDTO dto)
        {
            try
            {
                if (dto.Type == "con")
                {
                    dto.employeedetailList = (from a in _HRMSContext.MasterEmployee
                                              from b in _HRMSContext.HR_Master_GroupType
                                              from c in _HRMSContext.HR_Master_Department
                                              from d in _HRMSContext.HR_Master_Designation
                                              where (a.MI_Id.Equals(dto.MI_Id) && a.HRMGT_Id == b.HRMGT_Id && a.HRMD_Id == c.HRMD_Id && a.HRMDES_Id == d.HRMDES_Id && a.HRME_DOC != null && a.MI_Id==b.MI_Id && a.MI_Id==c.MI_Id && a.MI_Id==d.MI_Id)
                                              select new EmployeeProfileReportDTO
                                              {
                                                  HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                                  HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                                  HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                                  HRME_EmployeeCode = a.HRME_EmployeeCode,
                                                  HRMGT_Id = b.HRMGT_Id,
                                                  HRMGT_EmployeeGroupType = b.HRMGT_EmployeeGroupType,
                                                  HRMD_Id = a.HRMD_Id,
                                                  HRMD_DepartmentName = c.HRMD_DepartmentName,
                                                  HRMDES_Id = a.HRMDES_Id,
                                                  HRMDES_DesignationName = d.HRMDES_DesignationName,
                                              }).Distinct().ToArray();
                }
                else if (dto.Type == "prob")
                {
                    dto.employeedetailList = (from a in _HRMSContext.MasterEmployee
                                              from b in _HRMSContext.HR_Master_GroupType
                                              from c in _HRMSContext.HR_Master_Department
                                              from d in _HRMSContext.HR_Master_Designation
                                              where (a.MI_Id.Equals(dto.MI_Id) && a.HRMGT_Id == b.HRMGT_Id && a.HRMD_Id == c.HRMD_Id && a.HRMDES_Id == d.HRMDES_Id && a.HRME_DOC ==null)
                                              select new EmployeeProfileReportDTO
                                              {
                                                  HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                                  HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                                  HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                                  HRME_EmployeeCode = a.HRME_EmployeeCode,
                                                 HRMGT_Id = b.HRMGT_Id,
                                                  HRMGT_EmployeeGroupType = b.HRMGT_EmployeeGroupType,
                                                  HRMD_Id = a.HRMD_Id,
                                                  HRMD_DepartmentName = c.HRMD_DepartmentName,
                                                  HRMDES_Id = a.HRMDES_Id,
                                                  HRMDES_DesignationName = d.HRMDES_DesignationName,
                                              }).Distinct().ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public EmployeeProfileReportDTO get_departments(EmployeeProfileReportDTO data)
        {
            var dd = data.multipletype.Split(',');
            List<long> list = new List<long>();
            for (int i = 0; i < dd.Length; i++)
            {
                list.Add(Convert.ToInt64(dd[i]));
            }
            data.departmentdropdown = (from a in _HRMSContext.MasterEmployee
                                           from b in _HRMSContext.HR_Master_Department
                                           from c in _HRMSContext.HR_Master_GroupType
                                           where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMD_Id == b.HRMD_Id &&  a.HRMGT_Id==c.HRMGT_Id && list.Contains(c.HRMGT_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMD_ActiveFlag == true && a.HRME_ActiveFlag==true && c.HRMGT_ActiveFlag==true && a.MI_Id==b.MI_Id && a.MI_Id==c.MI_Id )
                                       select new EmployeeProfileReportDTO
                                       {
                                           HRMD_Id = b.HRMD_Id,
                                           HRMD_DepartmentName = b.HRMD_DepartmentName,
                                       }).Distinct().ToArray();            
            return data;
        }
        public EmployeeProfileReportDTO get_designation(EmployeeProfileReportDTO data)
        {
            var dd = data.multipledep.Split(',');
            List<long> list = new List<long>();
            for (int i = 0; i < dd.Length; i++)
            {
                list.Add(Convert.ToInt64(dd[i]));
            }
            data.designationdropdown = (from a in _HRMSContext.MasterEmployee
                                            from b in _HRMSContext.HR_Master_Designation
                                            from c in _HRMSContext.HR_Master_Department
                                            where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id==c.HRMD_Id && list.Contains(c.HRMD_Id)  && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag==true && c.HRMD_ActiveFlag==true && a.MI_Id==b.MI_Id && a.MI_Id==c.MI_Id)
                                        select new EmployeeProfileReportDTO
                                        {
                                            HRMDES_Id = b.HRMDES_Id,
                                            HRMDES_DesignationName = b.HRMDES_DesignationName,
                                        }).Distinct().ToArray();            
            return data;
        }
    }
}
