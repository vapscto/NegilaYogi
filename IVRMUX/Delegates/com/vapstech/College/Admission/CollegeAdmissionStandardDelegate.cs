using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class CollegeAdmissionStandardDelegate
    {
        CommonDelegate<CollegeAdmissionStandardDTO, CollegeAdmissionStandardDTO> COMMM = new CommonDelegate<CollegeAdmissionStandardDTO, CollegeAdmissionStandardDTO>();


        public CollegeAdmissionStandardDTO getlisttwo(CollegeAdmissionStandardDTO student)
        {
            return COMMM.clgadmissionbypost(student, "CollegeAdmissionStandardFacade/savedata/");           
        }
        public CollegeAdmissionStandardDTO getlistdata(int id)
        {
            return COMMM.clgadmissionbyid(id, "CollegeAdmissionStandardFacade/loaddata/");         
        }
    }
}
