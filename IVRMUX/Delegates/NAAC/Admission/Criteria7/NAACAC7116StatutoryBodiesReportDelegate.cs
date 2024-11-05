using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission.Criteria7
{
    public class NAACAC7116StatutoryBodiesReportDelegate
    {

        CommonDelegate<NAACAC7Report_DTO, NAACAC7Report_DTO> comm = new CommonDelegate<NAACAC7Report_DTO, NAACAC7Report_DTO>();
        public NAACAC7Report_DTO loaddata(NAACAC7Report_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC711GenderEquityReportFacade/loaddata");
        }
        public NAACAC7Report_DTO Report(NAACAC7Report_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC711GenderEquityReportFacade/Report");
        }
    }
}
