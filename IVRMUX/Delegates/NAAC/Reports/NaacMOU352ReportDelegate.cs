using CommonLibrary;
using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Reports
{
    public class NaacMOU352ReportDelegate
    {

        CommonDelegate<NaacMOU352ReportDTO, NaacMOU352ReportDTO> comm = new CommonDelegate<NaacMOU352ReportDTO, NaacMOU352ReportDTO>();
        public NaacMOU352ReportDTO getdata(NaacMOU352ReportDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacMOU352ReportFacade/getdata");
        }
        public NaacMOU352ReportDTO get_report(NaacMOU352ReportDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacMOU352ReportFacade/get_report");
        }


    }
}
