using CommonLibrary;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Fees
{
    public class BankDetailsDelegate
    {
        CommonDelegate<BankDetailsDTO, BankDetailsDTO> comm = new CommonDelegate<BankDetailsDTO, BankDetailsDTO>();
        public BankDetailsDTO getdata(BankDetailsDTO data)
        {
            return comm.POSTDatafee(data, "BankDetailsFacade/getdata");
        }
        public BankDetailsDTO getalldetails(BankDetailsDTO data)
        {
            return comm.POSTDatafee(data, "BankDetailsFacade/getalldetails");
        }
        public BankDetailsDTO edittab1(BankDetailsDTO data)
        {
            return comm.POSTDatafee(data, "BankDetailsFacade/edittab1");
        }
        public BankDetailsDTO deactive(BankDetailsDTO data)
        {
            return comm.POSTDatafee(data, "BankDetailsFacade/deactive");
        }
    }
}
