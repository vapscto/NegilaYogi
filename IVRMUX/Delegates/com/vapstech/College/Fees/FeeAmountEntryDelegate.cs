using System;
using PreadmissionDTOs.com.vaps.College.Fees;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class CLGFeeAmountEntrDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<CLGFeeAmountEntryDTO, CLGFeeAmountEntryDTO> COMMM = new CommonDelegate<CLGFeeAmountEntryDTO, CLGFeeAmountEntryDTO>();

        public CLGFeeAmountEntryDTO getdata(CLGFeeAmountEntryDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "CLGFeeAmountEntryFacade/getalldetails/");
        }

        public CLGFeeAmountEntryDTO getcourse(CLGFeeAmountEntryDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "CLGFeeAmountEntryFacade/getcoursedetails");
        }

        public CLGFeeAmountEntryDTO getbran(CLGFeeAmountEntryDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "CLGFeeAmountEntryFacade/getbranchdetails");
        }

        public CLGFeeAmountEntryDTO getcoubransem(CLGFeeAmountEntryDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "CLGFeeAmountEntryFacade/getsemesterdetails");
        }

        public CLGFeeAmountEntryDTO getgroupmapped(CLGFeeAmountEntryDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "CLGFeeAmountEntryFacade/getgroupmappedheads");
        }

        public CLGFeeAmountEntryDTO fillslabde(CLGFeeAmountEntryDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "CLGFeeAmountEntryFacade/fillslabdetails");
        }

        public CLGFeeAmountEntryDTO savedataa(CLGFeeAmountEntryDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "CLGFeeAmountEntryFacade/savedata");
        }


    }
}
