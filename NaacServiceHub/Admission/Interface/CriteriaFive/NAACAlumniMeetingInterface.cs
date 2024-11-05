using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface NAACAlumniMeetingInterface
    {
        NAACAlumniMeetingDTO loaddata(NAACAlumniMeetingDTO data);
        NAACAlumniMeetingDTO save(NAACAlumniMeetingDTO data);
        NAACAlumniMeetingDTO deactiveStudent(NAACAlumniMeetingDTO data);
        NAACAlumniMeetingDTO EditData(NAACAlumniMeetingDTO obj);
        NAACAlumniMeetingDTO deleteuploadfile(NAACAlumniMeetingDTO obj);
        NAACAlumniMeetingDTO viewuploadflies(NAACAlumniMeetingDTO obj);
        NAACAlumniMeetingDTO savemedicaldatawisecomments(NAACAlumniMeetingDTO obj);
        NAACAlumniMeetingDTO getcomment(NAACAlumniMeetingDTO obj);
        NAACAlumniMeetingDTO getfilecomment(NAACAlumniMeetingDTO obj);
        NAACAlumniMeetingDTO savefilewisecomments(NAACAlumniMeetingDTO obj);
    }
}
