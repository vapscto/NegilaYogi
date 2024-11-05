using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlumniHub.Com.Interface
{
    public interface CLGAlumniApprovalInterface
    {
        CLGAlumniStudentDTO Get_Intial_data (CLGAlumniStudentDTO CLGAlumniStudentDTO);

        Task<CLGAlumniStudentDTO> Getstudentlist(CLGAlumniStudentDTO sddto);

        CLGAlumniStudentDTO checkstudent (CLGAlumniStudentDTO sddto);

        CLGAlumniStudentDTO aproovedata(CLGAlumniStudentDTO sddto);

        Task<CLGAlumniStudentDTO> Getstudentlistapp(CLGAlumniStudentDTO sddto);

        CLGAlumniStudentDTO searchfilter(CLGAlumniStudentDTO data);

        CLGAlumniStudentDTO getstudata(CLGAlumniStudentDTO sddto);

        CLGAlumniStudentDTO savedata(CLGAlumniStudentDTO sddto);


    }
}
