using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface NAACCompExamsInterface
    {
        NAACCompExamsDTO loaddata(NAACCompExamsDTO data);
        NAACCompExamsDTO save(NAACCompExamsDTO data);
        NAACCompExamsDTO deactiveStudent(NAACCompExamsDTO data);
        NAACCompExamsDTO EditData(NAACCompExamsDTO obj);
        NAACCompExamsDTO deleteuploadfile(NAACCompExamsDTO obj);
        NAACCompExamsDTO viewuploadflies(NAACCompExamsDTO obj);
        NAACCompExamsDTO savemedicaldatawisecomments(NAACCompExamsDTO obj);
        NAACCompExamsDTO getcomment(NAACCompExamsDTO obj);
        NAACCompExamsDTO getfilecomment(NAACCompExamsDTO obj);
        NAACCompExamsDTO savefilewisecomments(NAACCompExamsDTO obj);
    }
}
