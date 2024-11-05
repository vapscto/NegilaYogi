using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlumniHub.Com.Interface
{
    public interface AlumniMembershipInterface
    {
        AlumniStudentDTO Get_Intial_data (AlumniStudentDTO AlumniStudentDTO);

        Task<AlumniStudentDTO> Getstudentlist(AlumniStudentDTO sddto);

        AlumniStudentDTO checkstudent (AlumniStudentDTO sddto);

        AlumniStudentDTO aproovedata(AlumniStudentDTO sddto);

        Task<AlumniStudentDTO> Getstudentlistapp(AlumniStudentDTO sddto);

        AlumniStudentDTO searchfilter(AlumniStudentDTO data);
        AlumniStudentDTO viewData(AlumniStudentDTO data);
        AlumniStudentDTO onchangestate(AlumniStudentDTO data);
        AlumniStudentDTO deactive(AlumniStudentDTO data);

        AlumniStudentDTO getstudata(AlumniStudentDTO sddto);
        AlumniStudentDTO svedata(AlumniStudentDTO sddto);

        AlumniStudentDTO svedatanewalumni(AlumniStudentDTO sddto);

        AlumniStudentDTO onchangecountry(AlumniStudentDTO sddto);
        AlumniStudentDTO onchangedistrict(AlumniStudentDTO sddto);

        AlumniStudentDTO EditAlumniHomepages(AlumniStudentDTO sddto);
        AlumniStudentDTO AlumniHomepageActiveDeactives(AlumniStudentDTO sddto);

        void AlumniWedding(int stu1);

    }
}
