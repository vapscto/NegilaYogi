using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAAC_AC_633_AdmTrainingDelegate
    {
        CommonDelegate<NAAC_Criteria_6_DTO, NAAC_Criteria_6_DTO> comm = new CommonDelegate<NAAC_Criteria_6_DTO, NAAC_Criteria_6_DTO>();
        public NAAC_Criteria_6_DTO loaddata(NAAC_Criteria_6_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_633_AdmTrainingFacade/loaddata");

        }
        public NAAC_Criteria_6_DTO save(NAAC_Criteria_6_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_633_AdmTrainingFacade/save");
        }
        public NAAC_Criteria_6_DTO deactiveStudent(NAAC_Criteria_6_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_633_AdmTrainingFacade/deactiveStudent");
        }
        public NAAC_Criteria_6_DTO EditData(NAAC_Criteria_6_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_633_AdmTrainingFacade/EditData");
        }
        public NAAC_Criteria_6_DTO viewuploadflies(NAAC_Criteria_6_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_633_AdmTrainingFacade/viewuploadflies");
        }

        public NAAC_Criteria_6_DTO deleteuploadfile(NAAC_Criteria_6_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_633_AdmTrainingFacade/deleteuploadfile");
        }



        public NAAC_Criteria_6_DTO savemedicaldatawisecomments(NAAC_Criteria_6_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_633_AdmTrainingFacade/savemedicaldatawisecomments");
        }
        public NAAC_Criteria_6_DTO savefilewisecomments(NAAC_Criteria_6_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_633_AdmTrainingFacade/savefilewisecomments");
        }
        public NAAC_Criteria_6_DTO getcomment(NAAC_Criteria_6_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_633_AdmTrainingFacade/getcomment");
        }
        public NAAC_Criteria_6_DTO getfilecomment(NAAC_Criteria_6_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_633_AdmTrainingFacade/getfilecomment");
        }

    }
}
