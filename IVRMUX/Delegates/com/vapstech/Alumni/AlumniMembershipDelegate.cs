using CommonLibrary;
using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Alumni
{
    public class AlumniMembershipDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<AlumniStudentDTO, AlumniStudentDTO> COMMM = new CommonDelegate<AlumniStudentDTO, AlumniStudentDTO>();
        public AlumniStudentDTO get_intial_data(AlumniStudentDTO AlumniStudentDTO)
        {
            return COMMM.POSTDataAlumni(AlumniStudentDTO, "AlumniMembershipFacade/Get_Intial_data/");
        }

        public AlumniStudentDTO Getstudentlist(AlumniStudentDTO sddto)
        {
            return COMMM.POSTDataAlumni(sddto, "AlumniMembershipFacade/Getstudentlist/");
        }

        public AlumniStudentDTO Getstudentlistapp(AlumniStudentDTO sddto)
        {
            return COMMM.POSTDataAlumni(sddto, "AlumniMembershipFacade/Getstudentlistapp/");
        }

        public AlumniStudentDTO checkstudent(AlumniStudentDTO sddto)
        {
            return COMMM.POSTDataAlumni(sddto, "AlumniMembershipFacade/checkstudent/");
        }
        public AlumniStudentDTO aproovedata(AlumniStudentDTO sddto)
        {
            return COMMM.POSTDataAlumni(sddto, "AlumniMembershipFacade/aproovedata/");
        }
        public AlumniStudentDTO searchfilter(AlumniStudentDTO sddto)
        {
            return COMMM.POSTDataAlumni(sddto, "AlumniMembershipFacade/searchfilter/");
        }

        public AlumniStudentDTO getstudata(AlumniStudentDTO sddto)
        {
            return COMMM.POSTDataAlumni(sddto, "AlumniMembershipFacade/getstudata/");
        }

        public AlumniStudentDTO svedata(AlumniStudentDTO sddto)
        {
            return COMMM.POSTDataAlumni(sddto, "AlumniMembershipFacade/savedata/");
        }

        public AlumniStudentDTO svedatanewalumni(AlumniStudentDTO sddto)
        {
            return COMMM.POSTDataAlumni(sddto, "AlumniMembershipFacade/svedatanewalumni/");
        }

        public AlumniStudentDTO onchangecountry(AlumniStudentDTO sddto)
        {
            return COMMM.POSTDataAlumni(sddto, "AlumniMembershipFacade/onchangecountry/");
        }
        public AlumniStudentDTO onchangedistrict(AlumniStudentDTO sddto)
        {
            return COMMM.POSTDataAlumni(sddto, "AlumniMembershipFacade/onchangedistrict/");
        }
        
         public AlumniStudentDTO viewData(AlumniStudentDTO sddto)
        {
            return COMMM.POSTDataAlumni(sddto, "AlumniMembershipFacade/viewData/");
        }
          public AlumniStudentDTO onchangestate(AlumniStudentDTO sddto)
        {
            return COMMM.POSTDataAlumni(sddto, "AlumniMembershipFacade/onchangestate/");
        }
         public AlumniStudentDTO deactive(AlumniStudentDTO sddto)
        {
            return COMMM.POSTDataAlumni(sddto, "AlumniMembershipFacade/deactive/");
        }

        //Akash
        public AlumniStudentDTO EditAlumniHomepages(AlumniStudentDTO data)
        {
            return COMMM.POSTDataAlumni(data, "AlumniMembershipFacade/EditAlumniHomepages");
        }
        public AlumniStudentDTO AlumniHomepageActiveDeactives(AlumniStudentDTO data)
        {
            return COMMM.POSTDataAlumni(data, "AlumniMembershipFacade/AlumniHomepageActiveDeactives");
        }

    }
}
