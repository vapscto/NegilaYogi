using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class Atten_Batch_MappingDelegate
    {
        CommonDelegate<Atten_Batch_MappingDTO, Atten_Batch_MappingDTO> _commbranch = new CommonDelegate<Atten_Batch_MappingDTO, Atten_Batch_MappingDTO>();

        public Atten_Batch_MappingDTO getalldetails(Atten_Batch_MappingDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Batch_MappingFacade/getalldetails/");
        }
        public Atten_Batch_MappingDTO savedata1(Atten_Batch_MappingDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Batch_MappingFacade/savedata1/");
        }
        public Atten_Batch_MappingDTO get_courses(Atten_Batch_MappingDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Batch_MappingFacade/get_courses/");
        }
        public Atten_Batch_MappingDTO get_branches(Atten_Batch_MappingDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Batch_MappingFacade/get_branches/");
        }
        public Atten_Batch_MappingDTO get_semisters(Atten_Batch_MappingDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Batch_MappingFacade/get_semisters/");
        }
        public Atten_Batch_MappingDTO get_students(Atten_Batch_MappingDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Batch_MappingFacade/get_students/");
        }
        public Atten_Batch_MappingDTO savedata2(Atten_Batch_MappingDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Batch_MappingFacade/savedata2/");
        }
        public Atten_Batch_MappingDTO view_subjects(Atten_Batch_MappingDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Batch_MappingFacade/view_subjects/");
        }
        public Atten_Batch_MappingDTO Deletedetails(Atten_Batch_MappingDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Batch_MappingFacade/Deletedetails/");
        }
    }
}
