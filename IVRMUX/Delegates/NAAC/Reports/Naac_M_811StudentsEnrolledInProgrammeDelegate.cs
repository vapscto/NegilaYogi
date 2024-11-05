using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission.Criteria8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Reports
{
    public class Naac_M_811StudentsEnrolledInProgrammeDelegate
    {
        CommonDelegate<NAAC_811MC_NEET_DTO, NAAC_811MC_NEET_DTO> _commnbranch = new CommonDelegate<NAAC_811MC_NEET_DTO, NAAC_811MC_NEET_DTO>();

        public NAAC_811MC_NEET_DTO getdata(NAAC_811MC_NEET_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Naac_M_811StudentsEnrolledInProgrammeFacade/getdata/");
        }
        public NAAC_811MC_NEET_DTO get_811M_report(NAAC_811MC_NEET_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Naac_M_811StudentsEnrolledInProgrammeFacade/get_811M_report/");
        }
        public NAAC_811MC_NEET_DTO get_813M_report(NAAC_811MC_NEET_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Naac_M_811StudentsEnrolledInProgrammeFacade/get_813M_report/");
        }
        public NAAC_811MC_NEET_DTO get_819M_report(NAAC_811MC_NEET_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Naac_M_811StudentsEnrolledInProgrammeFacade/get_819M_report/");
        }
        public NAAC_811MC_NEET_DTO get_8110M_report(NAAC_811MC_NEET_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Naac_M_811StudentsEnrolledInProgrammeFacade/get_8110M_report/");
        }
        public NAAC_811MC_NEET_DTO get_813D_report(NAAC_811MC_NEET_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Naac_M_811StudentsEnrolledInProgrammeFacade/get_813D_report/");
        }
        public NAAC_811MC_NEET_DTO get_815D_report(NAAC_811MC_NEET_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Naac_M_811StudentsEnrolledInProgrammeFacade/get_815D_report/");
        }
        public NAAC_811MC_NEET_DTO get_816D_report(NAAC_811MC_NEET_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Naac_M_811StudentsEnrolledInProgrammeFacade/get_816D_report/");
        }
        //public NAAC_811MC_NEET_DTO get_817D_report(NAAC_811MC_NEET_DTO data)
        //{
        //    return _commnbranch.naacdetailsbypost(data, "Naac_M_811StudentsEnrolledInProgrammeFacade/get_817D_report/");
        //}
        public NAAC_811MC_NEET_DTO get_8111D_report(NAAC_811MC_NEET_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Naac_M_811StudentsEnrolledInProgrammeFacade/get_8111D_report/");
        }
        public NAAC_811MC_NEET_DTO get_818N_report(NAAC_811MC_NEET_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Naac_M_811StudentsEnrolledInProgrammeFacade/get_818N_report/");
        }
    }
}
