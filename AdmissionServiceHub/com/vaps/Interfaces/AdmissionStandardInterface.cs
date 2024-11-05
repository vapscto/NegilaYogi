using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface AdmissionStandardInterface
    {
        AdmissionStandardDTO getlisttwo(AdmissionStandardDTO stu);
        AdmissionStandardDTO getlistdata(int id);

        // Admission Cancel Configuration
        AdmissionStandardDTO CancelConfigLoad(AdmissionStandardDTO stu);
        AdmissionStandardDTO SaveCancelConfigData(AdmissionStandardDTO stu);
        AdmissionStandardDTO EditCancelConfig(AdmissionStandardDTO stu);
        AdmissionStandardDTO ActiveDeactiveCancelConfig(AdmissionStandardDTO stu);
    }
}
