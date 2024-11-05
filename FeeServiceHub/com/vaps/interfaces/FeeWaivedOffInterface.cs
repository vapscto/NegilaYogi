using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeWaivedOffInterface
    {
        FeeStudentWaiveOffDTO getdata(FeeStudentWaiveOffDTO data);
        FeeStudentWaiveOffDTO getdatastudentdet(FeeStudentWaiveOffDTO data);        
        FeeStudentWaiveOffDTO getdatagroupdet(FeeStudentWaiveOffDTO data);
        FeeStudentWaiveOffDTO getdataheaddet(FeeStudentWaiveOffDTO data);
        FeeStudentWaiveOffDTO savedatadelegate(FeeStudentWaiveOffDTO data);
        FeeStudentWaiveOffDTO getpageedit(int id);
        FeeStudentWaiveOffDTO deleterec(int id);
        FeeStudentWaiveOffDTO searching(FeeStudentWaiveOffDTO data);
    }
}
