using CommonLibrary;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Fees
{
    public class FeeReceiptImportDelegate
    {
        CommonDelegate<FeeReceiptImportDTO, FeeReceiptImportDTO> _commnbranch = new CommonDelegate<FeeReceiptImportDTO, FeeReceiptImportDTO>();

        public FeeReceiptImportDTO Savedata(FeeReceiptImportDTO data)
        {
            return _commnbranch.POSTDatafee(data, "FeeReceiptImportFacade/Savedata/");
        }
        public FeeReceiptImportDTO getdetails(int id)
        {
            return _commnbranch.GETFees(id, "FeeReceiptImportFacade/getdetails/");
        }
        public FeeReceiptImportDTO deactiveY(FeeReceiptImportDTO data)
        {
            return _commnbranch.POSTDatafee(data, "FeeReceiptImportFacade/deactiveY/");
        }
    }
}
