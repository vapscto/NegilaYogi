using DataAccessMsSqlServerProvider;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Services
{
    public class SMSemailStaffReportIMPL : Interfaces.SMSemailStaffReportInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public SMSemailStaffReportIMPL(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public SMSemailStaffReportDTO getBasicData(SMSemailStaffReportDTO dto)
        {
            try
            {
                dto.getDepartment = _HRMSContext.HR_Master_Department.Where(R => R.MI_Id == dto.MI_Id && R.HRMD_ActiveFlag == true).Distinct().ToArray();
                dto.getdesination = (from a in _HRMSContext.HR_Master_Designation
                                     from b in _HRMSContext.MasterEmployee
                                     from c in _HRMSContext.Institution
                                     where (a.HRMDES_Id == b.HRMDES_Id && c.MI_Id == b.MI_Id && a.HRMDES_ActiveFlag == true && c.MI_Id == dto.MI_Id)
                                     select a
                                  ).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public SMSemailStaffReportDTO getreport(SMSemailStaffReportDTO dto)
        {
            try
            {
                List<long> HRMD_Id  = new List<long>();
               if(dto.departmentOne !=null)
                {
                    foreach(var item in dto.departmentOne)
                    {
                        HRMD_Id.Add(item.HRMD_Id);
                    }
                    
                }
               


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public SMSemailStaffReportDTO smsemail(SMSemailStaffReportDTO dto)
        {
            try
            {
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
        //Destination
        public SMSemailStaffReportDTO Destination(SMSemailStaffReportDTO dto)
        {
            try
            {

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
    }
}
