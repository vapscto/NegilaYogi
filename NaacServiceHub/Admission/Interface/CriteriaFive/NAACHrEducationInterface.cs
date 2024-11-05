using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface NAACHrEducationInterface
    {
        NAACHrEducationDTO loaddata(NAACHrEducationDTO data);
        NAACHrEducationDTO save(NAACHrEducationDTO data);
        NAACHrEducationDTO deactiveStudent(NAACHrEducationDTO data);
        NAACHrEducationDTO EditData(NAACHrEducationDTO obj);
        NAACHrEducationDTO deleteuploadfile(NAACHrEducationDTO obj);
        NAACHrEducationDTO viewuploadflies(NAACHrEducationDTO obj);
        NAACHrEducationDTO get_course(NAACHrEducationDTO obj);
        NAACHrEducationDTO get_branch(NAACHrEducationDTO obj);
        NAACHrEducationDTO savemedicaldatawisecomments(NAACHrEducationDTO obj);
        NAACHrEducationDTO getcomment(NAACHrEducationDTO obj);
        NAACHrEducationDTO getfilecomment(NAACHrEducationDTO obj);
        NAACHrEducationDTO savefilewisecomments(NAACHrEducationDTO obj);
    }
}
