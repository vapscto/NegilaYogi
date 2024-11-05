using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface NAACPlacementInterface
    {
        NAACPlacementDTO loaddata(NAACPlacementDTO data);
        NAACPlacementDTO save(NAACPlacementDTO data);
        NAACPlacementDTO deactiveStudent(NAACPlacementDTO data);
        NAACPlacementDTO EditData(NAACPlacementDTO obj);
        NAACPlacementDTO deleteuploadfile(NAACPlacementDTO obj);
        NAACPlacementDTO viewuploadflies(NAACPlacementDTO obj);
        NAACPlacementDTO get_course(NAACPlacementDTO obj);
        NAACPlacementDTO get_branch(NAACPlacementDTO obj);
        NAACPlacementDTO savemedicaldatawisecomments(NAACPlacementDTO obj);
        NAACPlacementDTO getcomment(NAACPlacementDTO obj);
        NAACPlacementDTO getfilecomment(NAACPlacementDTO obj);
        NAACPlacementDTO savefilewisecomments(NAACPlacementDTO obj);
    }
}
