using CommonLibrary;
using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Alumni
{
    public class CLGAlumniMembershipDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CLGAlumniStudentDTO, CLGAlumniStudentDTO> COMMM = new CommonDelegate<CLGAlumniStudentDTO, CLGAlumniStudentDTO>();
        public CLGAlumniStudentDTO get_intial_data(CLGAlumniStudentDTO CLGAlumniStudentDTO)
        {
            return COMMM.POSTDataAlumni(CLGAlumniStudentDTO, "CLGAlumniMembershipFacade/Get_Intial_data/");
        }

        public CLGAlumniStudentDTO Getstudentlist(CLGAlumniStudentDTO sddto)
        {
            return COMMM.POSTDataAlumni(sddto, "CLGAlumniMembershipFacade/Getstudentlist/");
        }

        public CLGAlumniStudentDTO Getstudentlistapp(CLGAlumniStudentDTO sddto)
        {
            return COMMM.POSTDataAlumni(sddto, "CLGAlumniMembershipFacade/Getstudentlistapp/");
        }

        public CLGAlumniStudentDTO checkstudent(CLGAlumniStudentDTO sddto)
        {
            return COMMM.POSTDataAlumni(sddto, "CLGAlumniMembershipFacade/checkstudent/");
        }
        public CLGAlumniStudentDTO aproovedata(CLGAlumniStudentDTO sddto)
        {
            return COMMM.POSTDataAlumni(sddto, "CLGAlumniMembershipFacade/aproovedata/");
        }
        public CLGAlumniStudentDTO searchfilter(CLGAlumniStudentDTO sddto)
        {
            return COMMM.POSTDataAlumni(sddto, "CLGAlumniMembershipFacade/searchfilter/");
        }

        public CLGAlumniStudentDTO getstudata(CLGAlumniStudentDTO sddto)
        {
            return COMMM.POSTDataAlumni(sddto, "CLGAlumniMembershipFacade/getstudata/");
        }

        public CLGAlumniStudentDTO savedata(CLGAlumniStudentDTO sddto)
        {
            return COMMM.POSTDataAlumni(sddto, "CLGAlumniMembershipFacade/savedata/");
        }

        public CLGAlumniStudentDTO onchangecountry(CLGAlumniStudentDTO sddto)
        {
            return COMMM.POSTDataAlumni(sddto, "CLGAlumniMembershipFacade/onchangecountry/");
        }

    }
}
