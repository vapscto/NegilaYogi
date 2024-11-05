using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
    public interface MasterDayInterface
    {
        TT_Master_DayDTO savedetail(TT_Master_DayDTO objdata);
        TT_Master_DayDTO savedaydetail(TT_Master_DayDTO objdata);
        TT_Master_DayDTO getdetails(int id);
        TT_Master_DayDTO getpageedit(int id);
        TT_Master_DayDTO getdayedit(int id);
        TT_Master_DayDTO deleterec(int id);
        TT_Master_DayDTO deactivate(TT_Master_DayDTO id);
        TT_Master_DayDTO deactivate1(TT_Master_DayDTO id);
        TT_Master_DayDTO getorder(TT_Master_DayDTO id);
        TT_Master_DayDTO saveorder(TT_Master_DayDTO id);

        TT_Master_DayDTO getavdata(TT_Master_DayDTO data);
        TT_Master_DayDTO getPeriods(TT_Master_DayDTO data);
        TT_Master_DayDTO allocateperiod(TT_Master_DayDTO data);

    }
}
