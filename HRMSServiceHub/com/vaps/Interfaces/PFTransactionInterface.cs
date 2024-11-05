using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface PFTransactionInterface
    {
        PFReportsDTO getBasicData(PFReportsDTO dto);

        //FilterEmployeeData

 
        PFReportsDTO SavePFData(PFReportsDTO dto);
        PFReportsDTO getReport(PFReportsDTO dto);
        PFReportsDTO getEmployeedetailsBySelectionStJames(PFReportsDTO dto);
        PFReportsDTO DeleteRecord(PFReportsDTO dto);
        PFReportsDTO editdata(PFReportsDTO dto);
        PFReportsDTO getloaddata(PFReportsDTO dto);
        PFReportsDTO savedetails(PFReportsDTO dto);
        PFReportsDTO deactive(PFReportsDTO dto);
        PFReportsDTO PFBlurcalculation(PFReportsDTO dto);
        PFReportsDTO EditSave(PFReportsDTO dto);
        PFReportsDTO finalverify(PFReportsDTO dto);
    }
}
