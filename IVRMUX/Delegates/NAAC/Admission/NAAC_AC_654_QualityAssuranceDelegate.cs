using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAAC_AC_654_QualityAssuranceDelegate
    {
        CommonDelegate<NAAC_Criteria_6_DTO, NAAC_Criteria_6_DTO> COMMM = new CommonDelegate<NAAC_Criteria_6_DTO, NAAC_Criteria_6_DTO>();
        public NAAC_Criteria_6_DTO loaddata(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_654_QualityAssuranceFacade/loaddata");
           
        }
        public NAAC_Criteria_6_DTO save(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_654_QualityAssuranceFacade/save");
        }
        public NAAC_Criteria_6_DTO deactiveStudent(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_654_QualityAssuranceFacade/deactiveStudent");
        }
        public NAAC_Criteria_6_DTO EditData(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_654_QualityAssuranceFacade/EditData");
        }
        public NAAC_Criteria_6_DTO viewuploadflies(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_654_QualityAssuranceFacade/viewuploadflies");
        }
        public NAAC_Criteria_6_DTO deleteuploadfile(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_654_QualityAssuranceFacade/deleteuploadfile");
        }


        public NAAC_Criteria_6_DTO savemedicaldatawisecomments(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_654_QualityAssuranceFacade/savemedicaldatawisecomments");
        }
        public NAAC_Criteria_6_DTO savefilewisecomments(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_654_QualityAssuranceFacade/savefilewisecomments");
        }
        public NAAC_Criteria_6_DTO getcomment(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_654_QualityAssuranceFacade/getcomment");
        }
        public NAAC_Criteria_6_DTO getfilecomment(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_654_QualityAssuranceFacade/getfilecomment");
        }

    }
}
