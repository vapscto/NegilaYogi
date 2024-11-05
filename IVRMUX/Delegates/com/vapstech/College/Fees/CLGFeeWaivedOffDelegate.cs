using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Fees
{
    public class CLGFeeWaivedOffDelegate
    {
        CommonDelegate<CLGFeeWaivedOffDTO, CLGFeeWaivedOffDTO> _commbranch = new CommonDelegate<CLGFeeWaivedOffDTO, CLGFeeWaivedOffDTO>();

        public CLGFeeWaivedOffDTO getalldetails(CLGFeeWaivedOffDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeWaivedOffFacade/getalldetails/");
        }
        public CLGFeeWaivedOffDTO get_students(CLGFeeWaivedOffDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeWaivedOffFacade/get_students/");
        }
        public CLGFeeWaivedOffDTO get_groups(CLGFeeWaivedOffDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeWaivedOffFacade/get_groups/");
        }
        public CLGFeeWaivedOffDTO get_heads(CLGFeeWaivedOffDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeWaivedOffFacade/get_heads/");
        }
        public CLGFeeWaivedOffDTO savedata(CLGFeeWaivedOffDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeWaivedOffFacade/savedata/");
        }
        public CLGFeeWaivedOffDTO EditDetails(CLGFeeWaivedOffDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeWaivedOffFacade/EditDetails/");
        }
        public CLGFeeWaivedOffDTO DeletRecord(CLGFeeWaivedOffDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeWaivedOffFacade/DeletRecord/");
        }
    }
}
