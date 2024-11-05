using CommonLibrary;
using DomainModel.Model.com.vapstech.Fee;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Fees
{
    public class ModeOfPaymentDelegate
    {
        CommonDelegate<ModeOfPaymentDTO, ModeOfPaymentDTO> comm = new CommonDelegate<ModeOfPaymentDTO, ModeOfPaymentDTO>();


        public ModeOfPaymentDTO loaddata(ModeOfPaymentDTO data)
        {
            return comm.POSTDatafee(data, "ModeofPaymentFacade/loaddata");
        }
        public ModeOfPaymentDTO savedata(ModeOfPaymentDTO data)
        {
            return comm.POSTDatafee(data, "ModeofPaymentFacade/savedata");
        }
        public ModeOfPaymentDTO deletedata(ModeOfPaymentDTO data)
        {
            return comm.POSTDatafee(data, "ModeofPaymentFacade/deletedata");
        }
        public ModeOfPaymentDTO paymentDecative(ModeOfPaymentDTO data)
        {
            return comm.POSTDatafee(data, "ModeofPaymentFacade/paymentDecative");
        }
    }
}
