using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.VMS.Training;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class TrainingtypewisereportIMPL : Interfaces.TrainingtypewisereportInterface
    {
        public VMSContext _context;
        public TrainingtypewisereportIMPL(VMSContext _con)
        {
            _context = _con;
        }
        public TrainingtypewisereportDTO onloaddata(TrainingtypewisereportDTO data)
        {
            try
            {
                data.getloaddetails = _context.Master_External_TrainingTypeDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TrainingtypewisereportDTO getreport(TrainingtypewisereportDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                //data.trainingtypewisereport = _context.External_TrainingDMO.Where(a => a.HREXTTRN_ApprovedFlg == true && a.HRMETRTY_Id == data.HRMETRTY_Id).ToArray();

                if (data.HRMETRTY_Id > 0)
                {
                    data.trainingtypewisereport = (from a in _context.External_TrainingDMO
                                                   from d in _context.HR_Master_Employee_DMO                                          
                                                   from b in _context.Master_External_TrainingCentersDMO
                                                   from e in _context.Master_External_TrainingTypeDMO
                                                   where (a.HRME_Id == d.HRME_Id /*&& a.HREXTTRN_ApprovedFlg == "Approved"*/ && a.HRMETRTY_Id == data.HRMETRTY_Id && a.HRMETRCEN_Id == b.HRMETRCEN_Id && e.HRMETRTY_Id == a.HRMETRTY_Id && a.MI_Id==data.MI_Id)
                                                   select new TrainingtypewisereportDTO
                                                   {
                                                       EmplYoeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName),
                                                       //shedule= a.HREXTTRN_StartDate +(DateTime.IsNullOrEmpty(a.HREXTTRN_StartDate) ?)
                                                       HREXTTRN_TotalHrs = a.HREXTTRN_TotalHrs,
                                                       HRMETRCEN_CenterAddress = b.HRMETRCEN_CenterAddress,
                                                       HREXTTRN_CertificateFileName = a.HREXTTRN_CertificateFileName,
                                                       HREXTTRN_CertificateFilePath = a.HREXTTRN_CertificateFilePath,
                                                       HREXTTRN_StartDate = a.HREXTTRN_StartDate,
                                                       HREXTTRN_EndDate = a.HREXTTRN_EndDate,
                                                       HREXTTRN_StartTime = a.HREXTTRN_StartTime,
                                                       HREXTTRN_EndTime = a.HREXTTRN_EndTime,
                                                       HREXTTRN_ApprovedFlg=a.HREXTTRN_ApprovedFlg
                                                   }).Distinct().ToArray();

                }



            }
            catch (Exception ex)
            {
                //data.returnval = false;
                //data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }






    }
}
