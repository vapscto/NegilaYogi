using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.HOD
{
    public class HODStudentStrengthDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ADMClassSectionStrengthDTO, ADMClassSectionStrengthDTO> COMMM = new CommonDelegate<ADMClassSectionStrengthDTO, ADMClassSectionStrengthDTO>();
        public ADMClassSectionStrengthDTO Getdetails(ADMClassSectionStrengthDTO data)
        {
            return COMMM.POSTPORTALData(data, "HODStudentStrengthFacade/Getdetails/");
        }

        public ADMClassSectionStrengthDTO getclass(ADMClassSectionStrengthDTO data)
        {
            return COMMM.POSTPORTALData(data, "HODStudentStrengthFacade/getclass/");
        }
        public ADMClassSectionStrengthDTO Getsection(ADMClassSectionStrengthDTO data)
        {
            return COMMM.POSTPORTALData(data, "HODStudentStrengthFacade/Getsection/");
        }
        public ADMClassSectionStrengthDTO Getsectioncount(ADMClassSectionStrengthDTO data)
        {
            return COMMM.POSTPORTALData(data, "HODStudentStrengthFacade/Getsectioncount/");
        }
    }
}
