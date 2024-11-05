using CommonLibrary;
using PreadmissionDTOs.NAAC.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Documents
{
    public class NaacMarksSlabDelegate
    {
        CommonDelegate<NAAC_AC_Criteria_MarksSlab_DTO, NAAC_AC_Criteria_MarksSlab_DTO> comm = new CommonDelegate<NAAC_AC_Criteria_MarksSlab_DTO, NAAC_AC_Criteria_MarksSlab_DTO>();

        public NAAC_AC_Criteria_MarksSlab_DTO Getdetails(NAAC_AC_Criteria_MarksSlab_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacMarksSlabFacade/Getdetails");
        }
        public NAAC_AC_Criteria_MarksSlab_DTO savedata(NAAC_AC_Criteria_MarksSlab_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacMarksSlabFacade/savedata");
        }
        public NAAC_AC_Criteria_MarksSlab_DTO editdata(NAAC_AC_Criteria_MarksSlab_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacMarksSlabFacade/editdata");
        }
        public NAAC_AC_Criteria_MarksSlab_DTO deactive(NAAC_AC_Criteria_MarksSlab_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacMarksSlabFacade/deactive");
        }
        
    }
}
