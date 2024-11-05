using CommonLibrary;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.University
{
    public class NAAC_HSU_EvaluationRelated_253Delegate
    {
        CommonDelegate<NAAC_HSU_EvaluationRelated_253_DTO, NAAC_HSU_EvaluationRelated_253_DTO> comm = new CommonDelegate<NAAC_HSU_EvaluationRelated_253_DTO, NAAC_HSU_EvaluationRelated_253_DTO>();

        public NAAC_HSU_EvaluationRelated_253_DTO loaddata(NAAC_HSU_EvaluationRelated_253_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_EvaluationRelated_253Facade/loaddata");
        }
        public NAAC_HSU_EvaluationRelated_253_DTO save(NAAC_HSU_EvaluationRelated_253_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_EvaluationRelated_253Facade/save");
        }
        public NAAC_HSU_EvaluationRelated_253_DTO deactive(NAAC_HSU_EvaluationRelated_253_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_EvaluationRelated_253Facade/deactive");
        }
        public NAAC_HSU_EvaluationRelated_253_DTO EditData(NAAC_HSU_EvaluationRelated_253_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_EvaluationRelated_253Facade/EditData");
        }
        public NAAC_HSU_EvaluationRelated_253_DTO viewuploadflies(NAAC_HSU_EvaluationRelated_253_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_EvaluationRelated_253Facade/viewuploadflies");
        }
        public NAAC_HSU_EvaluationRelated_253_DTO deleteuploadfile(NAAC_HSU_EvaluationRelated_253_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_EvaluationRelated_253Facade/deleteuploadfile");
        }
    }
}
