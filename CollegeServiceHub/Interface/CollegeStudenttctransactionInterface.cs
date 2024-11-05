using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface CollegeStudenttctransactionInterface
    {
        CollegeStudenttctransactionDTO loaddata(CollegeStudenttctransactionDTO data);
        CollegeStudenttctransactionDTO onchangeyear(CollegeStudenttctransactionDTO data);
        CollegeStudenttctransactionDTO onchangecourse(CollegeStudenttctransactionDTO data);
        CollegeStudenttctransactionDTO onchangebranch(CollegeStudenttctransactionDTO data);
        CollegeStudenttctransactionDTO onchangesemester(CollegeStudenttctransactionDTO data);
        CollegeStudenttctransactionDTO onchangesection(CollegeStudenttctransactionDTO data);
        CollegeStudenttctransactionDTO searchfilter(CollegeStudenttctransactionDTO data);        
        CollegeStudenttctransactionDTO onchangestudent(CollegeStudenttctransactionDTO data);
        CollegeStudenttctransactionDTO chk_dup_tc(CollegeStudenttctransactionDTO data);
        CollegeStudenttctransactionDTO savetc(CollegeStudenttctransactionDTO data);
    }
    
}
