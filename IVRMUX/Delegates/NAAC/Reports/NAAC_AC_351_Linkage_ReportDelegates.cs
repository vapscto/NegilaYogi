using CommonLibrary;
using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Reports
{
    public class NAAC_AC_351_Linkage_ReportDelegates
    {
        CommonDelegate<NAAC_AC_351_Linkage_ReportDTO, NAAC_AC_351_Linkage_ReportDTO> comm = new CommonDelegate<NAAC_AC_351_Linkage_ReportDTO, NAAC_AC_351_Linkage_ReportDTO>();
        public NAAC_AC_351_Linkage_ReportDTO loaddata(NAAC_AC_351_Linkage_ReportDTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_351_Linkage_ReportFacade/loaddata");
        }
        public NAAC_AC_351_Linkage_ReportDTO report(NAAC_AC_351_Linkage_ReportDTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_351_Linkage_ReportFacade/report");
        }
        
    }
}
