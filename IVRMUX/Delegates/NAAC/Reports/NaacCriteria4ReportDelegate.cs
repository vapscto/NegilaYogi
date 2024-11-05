using CommonLibrary;
using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Reports
{
    public class NaacCriteria4ReportDelegate
    {

        CommonDelegate<NaacCriteria4ReportDTO, NaacCriteria4ReportDTO> comm = new CommonDelegate<NaacCriteria4ReportDTO, NaacCriteria4ReportDTO>();
        public NaacCriteria4ReportDTO loaddata(NaacCriteria4ReportDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCriteria4ReportFacade/loaddata");
        }
        public NaacCriteria4ReportDTO Report(NaacCriteria4ReportDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCriteria4ReportFacade/Report");
        }
        public NaacCriteria4ReportDTO ReportCriteria4(NaacCriteria4ReportDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCriteria4ReportFacade/ReportCriteria4");
        }
       
        public NaacCriteria4ReportDTO ExpAcaReport(NaacCriteria4ReportDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCriteria4ReportFacade/ExpAcaReport");
        }
    }
}
