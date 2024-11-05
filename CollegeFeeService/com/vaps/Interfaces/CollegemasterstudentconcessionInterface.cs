using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.College.Fees;

namespace CollegeFeeService.com.vaps.Interfaces
{
  public  interface CollegemasterstudentconcessionInterface
    {
        CollegeConcessionDTO getdata(CollegeConcessionDTO data); 
        CollegeConcessionDTO get_courses(CollegeConcessionDTO data);
        CollegeConcessionDTO get_branches(CollegeConcessionDTO data);
        CollegeConcessionDTO get_semisters(CollegeConcessionDTO data);
        CollegeConcessionDTO get_student(CollegeConcessionDTO data);

        CollegeConcessionDTO fillamount(CollegeConcessionDTO data);
        CollegeConcessionDTO fillheaddetailsss(CollegeConcessionDTO data);
        CollegeConcessionDTO savedata(CollegeConcessionDTO data);
        CollegeConcessionDTO DeletRecord(CollegeConcessionDTO data);
    }
    
}

