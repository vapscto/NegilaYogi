using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeRefundableInterface
    {
        FeeMasterRefundDTO getdata(FeeMasterRefundDTO id);
        FeeMasterRefundDTO getgroupheaddetails(FeeMasterRefundDTO data);
        FeeMasterRefundDTO getdatastuacad(FeeMasterRefundDTO id);
        FeeMasterRefundDTO getdatastuacadgrp(FeeMasterRefundDTO id);
        FeeMasterRefundDTO savedetails(FeeMasterRefundDTO data);
        FeeMasterRefundDTO geteditdet(FeeMasterRefundDTO id);
        FeeMasterRefundDTO deleterec(FeeMasterRefundDTO id);

        FeeMasterRefundDTO getdataclawisestude(FeeMasterRefundDTO clsid);

        FeeMasterRefundDTO GetStudentListByYear(FeeMasterRefundDTO id);

        FeeMasterRefundDTO GetSection(FeeMasterRefundDTO dto);

        FeeMasterRefundDTO onselectacademicyear(FeeMasterRefundDTO dto);
        FeeMasterRefundDTO GetStudent(FeeMasterRefundDTO dto);

        FeeMasterRefundDTO GetStudentListByamst(FeeMasterRefundDTO dto);
        FeeMasterRefundDTO getmodeofpaymentdata(FeeMasterRefundDTO data);
        FeeMasterRefundDTO searching(FeeMasterRefundDTO data);
        FeeMasterRefundDTO get_recepts(FeeMasterRefundDTO data);
    }
}
