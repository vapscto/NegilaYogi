using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class Atten_Subject_MaxPeriodDelegate
    {
        CommonDelegate<Atten_Subject_MaxPeriodDTO, Atten_Subject_MaxPeriodDTO> _commbranch = new CommonDelegate<Atten_Subject_MaxPeriodDTO, Atten_Subject_MaxPeriodDTO>();

        public Atten_Subject_MaxPeriodDTO getalldetails(Atten_Subject_MaxPeriodDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Subject_MaxPeriodFacade/getalldetails/");
        }
        public Atten_Subject_MaxPeriodDTO get_courses(Atten_Subject_MaxPeriodDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Subject_MaxPeriodFacade/get_courses/");
        }
        public Atten_Subject_MaxPeriodDTO get_branches(Atten_Subject_MaxPeriodDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Subject_MaxPeriodFacade/get_branches/");
        }
        public Atten_Subject_MaxPeriodDTO get_semisters(Atten_Subject_MaxPeriodDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Subject_MaxPeriodFacade/get_semisters/");
        }      
        public Atten_Subject_MaxPeriodDTO get_subjects(Atten_Subject_MaxPeriodDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Subject_MaxPeriodFacade/get_subjects/");
        }
        public Atten_Subject_MaxPeriodDTO savedata(Atten_Subject_MaxPeriodDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Subject_MaxPeriodFacade/savedata/");
        }
        public Atten_Subject_MaxPeriodDTO Deletedetails(Atten_Subject_MaxPeriodDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Subject_MaxPeriodFacade/Deletedetails/");
        }
        public Atten_Subject_MaxPeriodDTO showmodaldetails(Atten_Subject_MaxPeriodDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Subject_MaxPeriodFacade/showmodaldetails/");
        }
        public Atten_Subject_MaxPeriodDTO deactivesem(Atten_Subject_MaxPeriodDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Subject_MaxPeriodFacade/deactivesem/");
        }
        
    }
}
