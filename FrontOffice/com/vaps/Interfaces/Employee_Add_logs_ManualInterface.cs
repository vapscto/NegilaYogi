using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOffice.com.vaps.Interfaces
{
    public interface Employee_Add_logs_ManualInterface
    {
        FO_Emp_PunchDTO getdata(FO_Emp_PunchDTO data);
        FO_Emp_PunchDTO empname(FO_Emp_PunchDTO data);
        FO_Emp_PunchDTO savedetail(FO_Emp_PunchDTO data);
        FO_Emp_PunchDTO deleterec(int id);

    }
}
