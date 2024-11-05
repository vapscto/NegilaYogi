using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface Atten_Subject_MaxPeriodInterface
    {
        Atten_Subject_MaxPeriodDTO getalldetails(Atten_Subject_MaxPeriodDTO data);       
        Atten_Subject_MaxPeriodDTO get_courses(Atten_Subject_MaxPeriodDTO data);
        Atten_Subject_MaxPeriodDTO get_branches(Atten_Subject_MaxPeriodDTO data);
        Atten_Subject_MaxPeriodDTO get_semisters(Atten_Subject_MaxPeriodDTO data);
        Atten_Subject_MaxPeriodDTO get_subjects(Atten_Subject_MaxPeriodDTO data);
        Atten_Subject_MaxPeriodDTO savedata(Atten_Subject_MaxPeriodDTO data);
        Atten_Subject_MaxPeriodDTO Deletedetails(Atten_Subject_MaxPeriodDTO data);
        Atten_Subject_MaxPeriodDTO showmodaldetails(Atten_Subject_MaxPeriodDTO data);
        Atten_Subject_MaxPeriodDTO deactivesem(Atten_Subject_MaxPeriodDTO data);
        
    }
}
