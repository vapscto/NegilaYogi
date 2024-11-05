using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface Naac_MOUInterface
    {
        Naac_MOU_DTO loaddata(Naac_MOU_DTO data);
        Naac_MOU_DTO save(Naac_MOU_DTO data);
        Naac_MOU_DTO deactiveStudent(Naac_MOU_DTO data);
        Naac_MOU_DTO getcomment(Naac_MOU_DTO data);
        Naac_MOU_DTO savemedicaldatawisecomments(Naac_MOU_DTO data);
        Naac_MOU_DTO savefilewisecomments(Naac_MOU_DTO data);
        Naac_MOU_DTO getfilecomment(Naac_MOU_DTO data);
        Naac_MOU_DTO viewuploadflies(Naac_MOU_DTO data);
        Naac_MOU_DTO deleteuploadfile(Naac_MOU_DTO obj);
        Naac_MOU_DTO EditData(Naac_MOU_DTO obj);
    }
}
