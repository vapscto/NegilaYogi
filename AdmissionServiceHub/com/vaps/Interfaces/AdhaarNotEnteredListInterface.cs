using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
  public  interface AdhaarNotEnteredListInterface
    {
        Task<AdhaarNotEnteredListDTO> getInitailData(AdhaarNotEnteredListDTO id);
        Task<AdhaarNotEnteredListDTO> getserdata(AdhaarNotEnteredListDTO data);
        Task<AdhaarNotEnteredListDTO> getsection(AdhaarNotEnteredListDTO id);
        Task<AdhaarNotEnteredListDTO> getclass(AdhaarNotEnteredListDTO id);

        //classchange

        Task<ClassChangeDTO> getInitailyear(ClassChangeDTO id);
        Task<ClassChangeDTO> searchdataclass(ClassChangeDTO data);

        //not promoted list report
        Task<AdhaarNotEnteredListDTO> getstudents(AdhaarNotEnteredListDTO data);

        //state and country wise student data
        Task<AdhaarNotEnteredListDTO> Getcountrystatedata(AdhaarNotEnteredListDTO data);
        Task<AdhaarNotEnteredListDTO> getsectionlist(AdhaarNotEnteredListDTO data);
        //section List for Attendance Not Done Report
        Task<AdhaarNotEnteredListDTO> getEntryType(AdhaarNotEnteredListDTO data);
        Task<AdhaarNotEnteredListDTO> getAttendencenotDoneReport(AdhaarNotEnteredListDTO data);

        Task<AdhaarNotEnteredListDTO> getClassEntryType(AdhaarNotEnteredListDTO data);
        AdhaarNotEnteredListDTO emailsend(AdhaarNotEnteredListDTO data);
    }
}
