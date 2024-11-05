using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.College.Fees;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface CLGFeeRefundableInterface
    {
        CLGFeeRefundableDTO getdata(CLGFeeRefundableDTO id);
        CLGFeeRefundableDTO getgroupheaddetails(CLGFeeRefundableDTO data);
        CLGFeeRefundableDTO getdatastuacad(CLGFeeRefundableDTO id);
        CLGFeeRefundableDTO getdatastuacadgrp(CLGFeeRefundableDTO id);
        CLGFeeRefundableDTO savedetails(CLGFeeRefundableDTO data);
        CLGFeeRefundableDTO geteditdet(CLGFeeRefundableDTO id);
        CLGFeeRefundableDTO deleterec(CLGFeeRefundableDTO id);

        CLGFeeRefundableDTO getdataclawisestude(CLGFeeRefundableDTO clsid);

        CLGFeeRefundableDTO GetStudentListByYear(CLGFeeRefundableDTO id);

        CLGFeeRefundableDTO GetSection(CLGFeeRefundableDTO dto);
        CLGFeeRefundableDTO get_semisters(CLGFeeRefundableDTO dto);
        CLGFeeRefundableDTO GetStudent(CLGFeeRefundableDTO dto);

        CLGFeeRefundableDTO GetStudentListByamst(CLGFeeRefundableDTO dto);
        CLGFeeRefundableDTO getmodeofpaymentdata(CLGFeeRefundableDTO data);
        CLGFeeRefundableDTO searching(CLGFeeRefundableDTO data);
    }
}
