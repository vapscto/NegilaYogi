using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOffice.com.vaps.Interfaces
{
  public  interface EmployeeLogImportInterface
    {
        EmployeeLogImportDTO Savedata(EmployeeLogImportDTO data);
    }
}
