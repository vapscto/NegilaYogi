using CommonLibrary;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Fees
{
    public class FeeReceiptImportStthomasDelegate
    {
        CommonDelegate<FeeReceiptImportStthomasDTO, FeeReceiptImportStthomasDTO> _commnbranch = new CommonDelegate<FeeReceiptImportStthomasDTO, FeeReceiptImportStthomasDTO>();

        public FeeReceiptImportStthomasDTO Savedata(FeeReceiptImportStthomasDTO data)
        {
            return _commnbranch.POSTDatafee(data, "FeeReceiptImportStthomasFacade/Savedata/");
        }
        public FeeReceiptImportStthomasDTO getdetails(int id)
        {
            return _commnbranch.GETFees(id, "FeeReceiptImportStthomasFacade/getdetails/");
        }
        public FeeReceiptImportStthomasDTO deactiveY(FeeReceiptImportStthomasDTO data)
        {
            return _commnbranch.POSTDatafee(data, "FeeReceiptImportStthomasFacade/deactiveY/");
        }
        //Added By PraveenGouda
        public FeeReceiptImportStthomasDTO getreportdetails(FeeReceiptImportStthomasDTO data)
        {
            return _commnbranch.POSTDatafee(data, "FeeReceiptImportStthomasFacade/getreportdetails/");
        }

        public FeeReceiptImportStthomasDTO deletereceipt(FeeReceiptImportStthomasDTO data)
        {
            return _commnbranch.POSTDatafee(data, "FeeReceiptImportStthomasFacade/deletereceipt/");
        }
    }
}
