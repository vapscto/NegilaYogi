using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface SalesSMSEMAILInterface
    {
        SalesSMSEMAILDTO getBasicData(SalesSMSEMAILDTO dto);
        SalesSMSEMAILDTO sendsmsemail(SalesSMSEMAILDTO dto);
        SalesSMSEMAILDTO editData(int id);

        SalesSMSEMAILDTO get_state(SalesSMSEMAILDTO dto);
        SalesSMSEMAILDTO getrpt(SalesSMSEMAILDTO dto);
        SalesSMSEMAILDTO getrpt_lead(SalesSMSEMAILDTO dto);
        SalesSMSEMAILDTO loadtemplate(SalesSMSEMAILDTO dto);
        SalesSMSEMAILDTO viewtemplatedetails(SalesSMSEMAILDTO dto);
    }
}
