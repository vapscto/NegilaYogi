using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAAC_Criteria_6_ReportDelegate
    {
        CommonDelegate<NAAC_Criteria_6_DTO, NAAC_Criteria_6_DTO> comm = new CommonDelegate<NAAC_Criteria_6_DTO, NAAC_Criteria_6_DTO>();
        public NAAC_Criteria_6_DTO loaddata(NAAC_Criteria_6_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_Criteria_6_ReportFacade/loaddata");
           
        }
        public NAAC_Criteria_6_DTO get_report(NAAC_Criteria_6_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_Criteria_6_ReportFacade/get_report");
        }
      

        
    }
}
