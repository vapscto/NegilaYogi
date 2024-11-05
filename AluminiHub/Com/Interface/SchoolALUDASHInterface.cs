using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlumniHub.Com.Interface
{
    public interface SchoolALUDASHInterface
    {
         AlumniStudentDTO getloaddata(AlumniStudentDTO obj);

        AlumniStudentDTO yearwiselist(AlumniStudentDTO data);

        AlumniStudentDTO classwisestudent(AlumniStudentDTO data);
        AlumniStudentDTO AluminiBirthday(AlumniStudentDTO data);
        AlumniStudentDTO getgallery(AlumniStudentDTO data);
        AlumniStudentDTO viewgallery(AlumniStudentDTO data);
        AlumniStudentDTO alumninotice(AlumniStudentDTO data);
        AlumniStudentDTO viewnotice(AlumniStudentDTO data);
        
    }
}
