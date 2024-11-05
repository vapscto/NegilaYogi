using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface Staff_BookTranasctionInterface
    {
        Staff_BookTranasctionDTO getdetails(Staff_BookTranasctionDTO data);
        Staff_BookTranasctionDTO get_Staffdetails(Staff_BookTranasctionDTO data);
        Staff_BookTranasctionDTO get_bookdetails(Staff_BookTranasctionDTO data);
        Staff_BookTranasctionDTO Savedata(Staff_BookTranasctionDTO data);
        Staff_BookTranasctionDTO renewaldata(Staff_BookTranasctionDTO data);
        Staff_BookTranasctionDTO Editdata(Staff_BookTranasctionDTO data);
        Staff_BookTranasctionDTO returndata(Staff_BookTranasctionDTO data);
    }
}
