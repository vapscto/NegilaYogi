using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;


namespace FeeServiceHub.com.vaps.interfaces
{
   public interface FeeMasterTermsInterface
    {

        FeeTermDTO SaveGroupData(FeeTermDTO org);
        FeeTermDTO EditgroupDetails(int id);
        FeeTermDTO getdetails(FeeTermDTO id);
        FeeTermDTO GetGroupSearchData(FeeTermDTO mas);
        FeeTermDTO getpageedit(int id);
        FeeTermDTO deleterec(int id);
        FeeTermDTO deactivate(FeeTermDTO id);
        FeeMasterTermHeadsDTO savedetailfourth(FeeMasterTermHeadsDTO org);
        FeeMasterTermHeadsDTO SaveYearlyGroupData(FeeMasterTermHeadsDTO org);
        FeeMasterTermHeadsDTO getdetailsY(int id);
        FeeMasterTermHeadsDTO deactivateY(FeeMasterTermHeadsDTO id);
        FeeMasterTermHeadsDTO getpageeditY(int id);
        
        FeeMasterTermHeadsDTO deleterecY(FeeMasterTermHeadsDTO mas);
        FeeMasterTermHeadsDTO[] Getduedates(FeeMasterTermHeadsDTO mas);
        FeeMasterTermHeadsDTO savedetailDDD(FeeMasterTermHeadsDTO org);
        FeeMasterTermFeeHeadsDueDateDTO getdetailsDY(int id);
        FeeMasterTermFeeHeadsDueDateDTO deletepagesthird(int id);
        ///materperiod
        FeeMasterTermFeeHeadsDueDateDTO getdetailsDYfourth(int id);
        FeeMasterTermHeadsDTO DeleteYss(FeeMasterTermHeadsDTO mas);
    }
}
