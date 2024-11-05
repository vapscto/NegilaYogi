using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface NAACEncDevSchemeInterface
    {
        NAACEncDevSchemeDTO loaddata(NAACEncDevSchemeDTO data);
        NAACEncDevSchemeDTO save(NAACEncDevSchemeDTO data);
        NAACEncDevSchemeDTO deactiveStudent(NAACEncDevSchemeDTO data);
        NAACEncDevSchemeDTO EditData(NAACEncDevSchemeDTO obj);
        NAACEncDevSchemeDTO deleteuploadfile(NAACEncDevSchemeDTO obj);
        NAACEncDevSchemeDTO viewuploadflies(NAACEncDevSchemeDTO obj);
        NAACEncDevSchemeDTO savemedicaldatawisecomments(NAACEncDevSchemeDTO obj);
        NAACEncDevSchemeDTO getcomment(NAACEncDevSchemeDTO obj);
        NAACEncDevSchemeDTO getfilecomment(NAACEncDevSchemeDTO obj);
        NAACEncDevSchemeDTO savefilewisecomments(NAACEncDevSchemeDTO obj);
    }
}
