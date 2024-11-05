using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class QualificationReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterEmployeeDTO, MasterEmployeeDTO> COMMM = new CommonDelegate<MasterEmployeeDTO, MasterEmployeeDTO>();
        public MasterEmployeeDTO getalldetails(MasterEmployeeDTO data)
        {
            return COMMM.POSTDataHRMS(data, "QualificationReportFacade/getalldetails/");          
        }
        public MasterEmployeeDTO getQualificationReport(MasterEmployeeDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "QualificationReportFacade/getQualificationReport/");
        }
    }
}
