using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class CLGRegNoFormatDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CLGAdm_College_RegNo_FormatDTO, CLGAdm_College_RegNo_FormatDTO> COMMM = new CommonDelegate<CLGAdm_College_RegNo_FormatDTO, CLGAdm_College_RegNo_FormatDTO>();

       
        public CLGAdm_College_RegNo_FormatDTO Savedetails(CLGAdm_College_RegNo_FormatDTO dt)
        {
            return COMMM.clgadmissionbypost(dt, "CLGRegNoFormatFacade/Savedetails");
        }
        public CLGAdm_College_RegNo_FormatDTO getalldetails(int id)
        {
            return COMMM.clgadmissionbyid(id, "CLGRegNoFormatFacade/getalldetails/");
        }
        public CLGAdm_College_RegNo_FormatDTO Deletedetails(CLGAdm_College_RegNo_FormatDTO id)
        {
            return COMMM.clgadmissionbypost(id, "CLGRegNoFormatFacade/Deletedetails");
        }
       
    }
}
