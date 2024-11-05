using CommonLibrary;
using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Reports
{
    public class NaacAward342ReportDelegate
    {
        CommonDelegate<NaacAward342ReportDTO, NaacAward342ReportDTO> comm = new CommonDelegate<NaacAward342ReportDTO, NaacAward342ReportDTO>();
        public NaacAward342ReportDTO getdata(NaacAward342ReportDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacAward342ReportFacade/getdata");
        }
        public NaacAward342ReportDTO get_report(NaacAward342ReportDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacAward342ReportFacade/get_report");
        }
    }
}
