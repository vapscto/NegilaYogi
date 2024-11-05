using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class masterparameterDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Employee_AssesmentparameterDTO, HR_Employee_AssesmentparameterDTO> COMMM = new CommonDelegate<HR_Employee_AssesmentparameterDTO, HR_Employee_AssesmentparameterDTO>();

        public HR_Employee_AssesmentparameterDTO onloadgetdetails(HR_Employee_AssesmentparameterDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "masterparameterFacade/onloadgetdetails");
        }

        public HR_Employee_AssesmentparameterDTO savedetails(HR_Employee_AssesmentparameterDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "masterparameterFacade/");
        }
        public HR_Employee_AssesmentparameterDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "masterparameterFacade/getRecordById/");
        }
        public HR_Employee_AssesmentparameterDTO deleterec(HR_Employee_AssesmentparameterDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "masterparameterFacade/deactivateRecordById/");
        }

    }
}
