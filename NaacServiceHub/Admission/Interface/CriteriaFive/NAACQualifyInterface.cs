using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface NAACQualifyInterface
    {
        NAACQualifyDTO loaddata(NAACQualifyDTO data);
        NAACQualifyDTO save1(NAACQualifyDTO data);
        NAACQualifyDTO deactiveStudent1(NAACQualifyDTO data);
        NAACQualifyDTO EditData1(NAACQualifyDTO obj);
        NAACQualifyDTO save(NAACQualifyDTO data);
        NAACQualifyDTO deactiveStudent(NAACQualifyDTO data);
        NAACQualifyDTO EditData(NAACQualifyDTO obj);
        NAACQualifyDTO viewuploadflies(NAACQualifyDTO obj);
        NAACQualifyDTO deleteuploadfile(NAACQualifyDTO obj);
        NAACQualifyDTO savemedicaldatawisecomments(NAACQualifyDTO obj);
        NAACQualifyDTO getcomment(NAACQualifyDTO obj);
        NAACQualifyDTO getfilecomment(NAACQualifyDTO obj);
        NAACQualifyDTO savefilewisecomments(NAACQualifyDTO obj);
    }
}
