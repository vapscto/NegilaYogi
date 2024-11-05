using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class CLGMasterSemisterDelegete
    {
        CommonDelegate<CLGMasterSemisterDTO, CLGMasterSemisterDTO> _commsem = new CommonDelegate<CLGMasterSemisterDTO, CLGMasterSemisterDTO>();

        public CLGMasterSemisterDTO savesem(CLGMasterSemisterDTO data)
        {
            return _commsem.clgadmissionbypost(data, "CLGMasterSemisterFacade/savesem/");
        }
        public CLGMasterSemisterDTO editsem(CLGMasterSemisterDTO data)
        {
            return _commsem.clgadmissionbypost(data, "CLGMasterSemisterFacade/editsem/");
        }
        public CLGMasterSemisterDTO activedeactivesem(CLGMasterSemisterDTO data)
        {
            return _commsem.clgadmissionbypost(data, "CLGMasterSemisterFacade/activedeactivesem/");
        }
        public CLGMasterSemisterDTO getdata(CLGMasterSemisterDTO data)
        {
            return _commsem.clgadmissionbypost(data, "CLGMasterSemisterFacade/getdata/");
        }
        public CLGMasterSemisterDTO getOrder(CLGMasterSemisterDTO data)
        {
            return _commsem.clgadmissionbypost(data, "CLGMasterSemisterFacade/getOrder/");
        }
        
    }
}
