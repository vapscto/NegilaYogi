using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Interface
{
  public  interface NAAC_HSU_EvaluationRelated_253Interface
    {
        NAAC_HSU_EvaluationRelated_253_DTO loaddata(NAAC_HSU_EvaluationRelated_253_DTO data);
        NAAC_HSU_EvaluationRelated_253_DTO save(NAAC_HSU_EvaluationRelated_253_DTO data);
        NAAC_HSU_EvaluationRelated_253_DTO deactive(NAAC_HSU_EvaluationRelated_253_DTO data);
        NAAC_HSU_EvaluationRelated_253_DTO EditData(NAAC_HSU_EvaluationRelated_253_DTO data);
        NAAC_HSU_EvaluationRelated_253_DTO viewuploadflies(NAAC_HSU_EvaluationRelated_253_DTO data);
        NAAC_HSU_EvaluationRelated_253_DTO deleteuploadfile(NAAC_HSU_EvaluationRelated_253_DTO data);
    }
}
