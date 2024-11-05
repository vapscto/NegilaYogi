using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
    public  interface PeriodAllocationInterface
    {
        TTPeriodAllocationDTO savedetail(TTPeriodAllocationDTO objperiod);
        TTPeriodAllocationDTO saveperiod(TTPeriodAllocationDTO period);
        TTPeriodAllocationDTO getdetails(TTPeriodAllocationDTO data);
        TTPeriodAllocationDTO getclasses(TTPeriodAllocationDTO data);
        TTPeriodAllocationDTO getcategories(TTPeriodAllocationDTO data);
        TTPeriodAllocationDTO getperiod_class(TTPeriodAllocationDTO data);
        TTPeriodAllocationDTO getpageedit(int id);
        TTPeriodAllocationDTO deleterec(int id);
        TTPeriodAllocationDTO deactivate(TTPeriodAllocationDTO id);
        TTPeriodAllocationDTO deactivate1(TTPeriodAllocationDTO id);

    }
}
