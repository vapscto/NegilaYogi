using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface Naac_ICTInterface
    {
        Naac_ICT_DTO loaddata(Naac_ICT_DTO data);
        Naac_ICT_DTO savedata(Naac_ICT_DTO data);
        Naac_ICT_DTO editdata(Naac_ICT_DTO data);
        Naac_ICT_DTO savefilewisecomments(Naac_ICT_DTO data);
        Naac_ICT_DTO savemedicaldatawisecomments(Naac_ICT_DTO data);
        Naac_ICT_DTO getfilecomment(Naac_ICT_DTO data);
        Naac_ICT_DTO getcomment(Naac_ICT_DTO data);
        Naac_ICT_DTO deactivRow(Naac_ICT_DTO data);
        Naac_ICT_DTO viewuploadflies(Naac_ICT_DTO data);
        Naac_ICT_DTO deleteuploadfile(Naac_ICT_DTO data);
    }
}
