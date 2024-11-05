using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.HOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.HOD
{
    public class HODExamTopperDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HODExamTopper_DTO, HODExamTopper_DTO> COMMM = new CommonDelegate<HODExamTopper_DTO, HODExamTopper_DTO>();
        public HODExamTopper_DTO Getdetails(HODExamTopper_DTO data)
        {
            return COMMM.POSTPORTALData(data, "HODExamTopperFacade/Getdetails/");
        }

        public HODExamTopper_DTO getcategory(HODExamTopper_DTO data)
        {
            return COMMM.POSTPORTALData(data, "HODExamTopperFacade/getcategory/");
        }
        public HODExamTopper_DTO getclassexam(HODExamTopper_DTO data)
        {
            return COMMM.POSTPORTALData(data, "HODExamTopperFacade/getclassexam/");
        }
        public HODExamTopper_DTO showreport(HODExamTopper_DTO data)
        {
            return COMMM.POSTPORTALData(data, "HODExamTopperFacade/showreport/");
        }

        public HODExamTopper_DTO getsection(HODExamTopper_DTO data)
        {
            return COMMM.POSTPORTALData(data, "HODExamTopperFacade/getsection/");
        }


    }
}
