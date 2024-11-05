using CollegeFeeService.com.vaps.Interfaces;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class ColStudentConcessionReportImpl : ColStudentConcessionReportInterface
    {
        public CollFeeGroupContext _ClgAdmissionContext;
        readonly ILogger<ColStudentConcessionReportImpl> _logger;
        public ColStudentConcessionReportImpl(CollFeeGroupContext _ClgAdmissionCon, ILogger<ColStudentConcessionReportImpl> log)
        {
            _logger = log;
            _ClgAdmissionContext = _ClgAdmissionCon;

        }


        public CollegeConcessionDTO getalldetails(CollegeConcessionDTO dt)
        {
            // CollegeConcessionDTO data = new CollegeConcessionDTO();
            try
            {


                var year = _ClgAdmissionContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == dt.MI_Id).ToList();
                dt.yearlst = year.Distinct().GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();
                

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dt;
        }


    }
}
