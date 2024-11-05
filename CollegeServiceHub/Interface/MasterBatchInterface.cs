using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface MasterBatchInterface
    {
       // AdmCollegeMasterBatchDTO getalldetails(AdmCollegeMasterBatchDTO data);
        AdmCollegeMasterBatchDTO editbatch(AdmCollegeMasterBatchDTO data);
        AdmCollegeMasterBatchDTO getdata(AdmCollegeMasterBatchDTO data);
        AdmCollegeMasterBatchDTO activedeactivebatch(AdmCollegeMasterBatchDTO data);
        AdmCollegeMasterBatchDTO savebatch(AdmCollegeMasterBatchDTO data);
    }
}
