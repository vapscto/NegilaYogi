using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class MasterBatchDelegete
    {
        CommonDelegate<AdmCollegeMasterBatchDTO, AdmCollegeMasterBatchDTO> _commbatch = new CommonDelegate<AdmCollegeMasterBatchDTO, AdmCollegeMasterBatchDTO>();

        public AdmCollegeMasterBatchDTO savebatch(AdmCollegeMasterBatchDTO data)
        {
            return _commbatch.clgadmissionbypost(data, "MasterBatchFacade/savebatch/");
        }
        public AdmCollegeMasterBatchDTO editbatch(AdmCollegeMasterBatchDTO data)
        {
            return _commbatch.clgadmissionbypost(data, "MasterBatchFacade/editbatch/");
        }
        public AdmCollegeMasterBatchDTO activedeactivebatch(AdmCollegeMasterBatchDTO data)
        {
            return _commbatch.clgadmissionbypost(data, "MasterBatchFacade/activedeactivebatch/");
        }
        public AdmCollegeMasterBatchDTO getdata(AdmCollegeMasterBatchDTO data)
        {
            return _commbatch.clgadmissionbypost(data, "MasterBatchFacade/getdata/");
        }
    }
}
