using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Interface
{
   public interface NAAC_MC_EmpTrainedDevelopment244Interface
    {

        NAAC_MC_EmpTrainedDevelopment244_DTO loaddata(NAAC_MC_EmpTrainedDevelopment244_DTO data);
        NAAC_MC_EmpTrainedDevelopment244_DTO save(NAAC_MC_EmpTrainedDevelopment244_DTO data);
        NAAC_MC_EmpTrainedDevelopment244_DTO deactive(NAAC_MC_EmpTrainedDevelopment244_DTO data);
        NAAC_MC_EmpTrainedDevelopment244_DTO EditData(NAAC_MC_EmpTrainedDevelopment244_DTO data);
        NAAC_MC_EmpTrainedDevelopment244_DTO viewuploadflies(NAAC_MC_EmpTrainedDevelopment244_DTO data);
        NAAC_MC_EmpTrainedDevelopment244_DTO deleteuploadfile(NAAC_MC_EmpTrainedDevelopment244_DTO obj);

        NAAC_MC_EmpTrainedDevelopment244_DTO savemedicaldatawisecomments(NAAC_MC_EmpTrainedDevelopment244_DTO data);
        NAAC_MC_EmpTrainedDevelopment244_DTO savefilewisecomments(NAAC_MC_EmpTrainedDevelopment244_DTO data);
        NAAC_MC_EmpTrainedDevelopment244_DTO getcomment(NAAC_MC_EmpTrainedDevelopment244_DTO data);
        NAAC_MC_EmpTrainedDevelopment244_DTO getfilecomment(NAAC_MC_EmpTrainedDevelopment244_DTO data);

    }
}
