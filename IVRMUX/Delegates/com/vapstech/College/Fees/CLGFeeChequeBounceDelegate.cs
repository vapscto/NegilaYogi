using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Fees
{
    public class CLGFeeChequeBounceDelegate
    {
        CommonDelegate<CLGFeeChequeBounceDTO, CLGFeeChequeBounceDTO> _commbranch = new CommonDelegate<CLGFeeChequeBounceDTO, CLGFeeChequeBounceDTO>();

        public CLGFeeChequeBounceDTO getalldetails(CLGFeeChequeBounceDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeChequeBounceFacade/getalldetails/");
        }
        public CLGFeeChequeBounceDTO get_students(CLGFeeChequeBounceDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeChequeBounceFacade/get_students/");
        }
        public CLGFeeChequeBounceDTO get_receipts(CLGFeeChequeBounceDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeChequeBounceFacade/get_receipts/");
        }
        public CLGFeeChequeBounceDTO savedata(CLGFeeChequeBounceDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeChequeBounceFacade/savedata/");
        }
        public CLGFeeChequeBounceDTO DeletRecord(CLGFeeChequeBounceDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeChequeBounceFacade/DeletRecord/");
        }
    }
}
