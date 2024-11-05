using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Fees
{
    public class CLGFeeOpeningBalanceDelegate
    {
        CommonDelegate<CLGFeeOpeningBalanceDTO, CLGFeeOpeningBalanceDTO> _commbranch = new CommonDelegate<CLGFeeOpeningBalanceDTO, CLGFeeOpeningBalanceDTO>();

        public CLGFeeOpeningBalanceDTO getalldetails(CLGFeeOpeningBalanceDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeOpeningBalanceFacade/getalldetails/");
        }
        public CLGFeeOpeningBalanceDTO get_courses(CLGFeeOpeningBalanceDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeOpeningBalanceFacade/get_courses/");
        }
        public CLGFeeOpeningBalanceDTO get_branches(CLGFeeOpeningBalanceDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeOpeningBalanceFacade/get_branches/");
        }
        public CLGFeeOpeningBalanceDTO get_semisters(CLGFeeOpeningBalanceDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeOpeningBalanceFacade/get_semisters/");
        }
        public CLGFeeOpeningBalanceDTO get_groups(CLGFeeOpeningBalanceDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeOpeningBalanceFacade/get_groups/");
        }
        public CLGFeeOpeningBalanceDTO get_heads(CLGFeeOpeningBalanceDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeOpeningBalanceFacade/get_heads/");
        }
        public CLGFeeOpeningBalanceDTO get_installments(CLGFeeOpeningBalanceDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeOpeningBalanceFacade/get_installments/");
        }
        public CLGFeeOpeningBalanceDTO get_students(CLGFeeOpeningBalanceDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeOpeningBalanceFacade/get_students/");
        }
        public CLGFeeOpeningBalanceDTO savedata(CLGFeeOpeningBalanceDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeOpeningBalanceFacade/savedata/");
        }
        public CLGFeeOpeningBalanceDTO Deletedetails(CLGFeeOpeningBalanceDTO data)
        {
            return _commbranch.PostClgFee(data, "CLGFeeOpeningBalanceFacade/Deletedetails/");
        }
    }
}
