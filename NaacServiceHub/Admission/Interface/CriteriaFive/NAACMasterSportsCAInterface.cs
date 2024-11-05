using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface NAACMasterSportsCAInterface
    {
        NAACMasterSportsCADTO loaddata(NAACMasterSportsCADTO data);
        NAACMasterSportsCADTO save(NAACMasterSportsCADTO data);
        NAACMasterSportsCADTO deactiveStudent(NAACMasterSportsCADTO data);
        NAACMasterSportsCADTO EditData(NAACMasterSportsCADTO obj);
        NAACMasterSportsCADTO deleteuploadfile(NAACMasterSportsCADTO obj);
        NAACMasterSportsCADTO viewuploadflies(NAACMasterSportsCADTO obj);
        NAACMasterSportsCADTO savemedicaldatawisecomments(NAACMasterSportsCADTO obj);
        NAACMasterSportsCADTO getcomment(NAACMasterSportsCADTO obj);
        NAACMasterSportsCADTO getfilecomment(NAACMasterSportsCADTO obj);
        NAACMasterSportsCADTO savefilewisecomments(NAACMasterSportsCADTO obj);
    }
}
