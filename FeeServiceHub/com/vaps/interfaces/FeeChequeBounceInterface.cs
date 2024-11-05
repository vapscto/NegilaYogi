using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeChequeBounceInterface
    {
        FeeChequeBounceDTO getdata(FeeChequeBounceDTO data);
        FeeChequeBounceDTO getstuddet(FeeChequeBounceDTO data);
        FeeChequeBounceDTO getdatastuacad(FeeChequeBounceDTO data);
        FeeChequeBounceDTO getdatastuacadgrp(FeeChequeBounceDTO data);
        FeeChequeBounceDTO savedetails(FeeChequeBounceDTO data);
        FeeChequeBounceDTO geteditdet(FeeChequeBounceDTO data);
        FeeChequeBounceDTO deleterec(FeeChequeBounceDTO data);
        FeeChequeBounceDTO searching(FeeChequeBounceDTO data);
        FeeChequeBounceDTO get_students(FeeChequeBounceDTO data);
        FeeChequeBounceDTO get_section(FeeChequeBounceDTO data);
        FeeChequeBounceDTO get_receipts(FeeChequeBounceDTO data);
    }
}
