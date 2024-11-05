using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface Atten_Batch_MappingInterface
    {
        Atten_Batch_MappingDTO getalldetails(Atten_Batch_MappingDTO data);
        Atten_Batch_MappingDTO savedata1(Atten_Batch_MappingDTO data);
        Atten_Batch_MappingDTO get_courses(Atten_Batch_MappingDTO data);
        Atten_Batch_MappingDTO get_branches(Atten_Batch_MappingDTO data);
        Atten_Batch_MappingDTO get_semisters(Atten_Batch_MappingDTO data);
        Atten_Batch_MappingDTO get_students(Atten_Batch_MappingDTO data);
        Atten_Batch_MappingDTO savedata2(Atten_Batch_MappingDTO data);
        Atten_Batch_MappingDTO view_subjects(Atten_Batch_MappingDTO data);
        Atten_Batch_MappingDTO Deletedetails(Atten_Batch_MappingDTO data);
    }
}
