using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class EmployeeSalaryIncreementProcessDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EmployeeSalaryIncreementProcessDTO, EmployeeSalaryIncreementProcessDTO> COMMM = new CommonDelegate<EmployeeSalaryIncreementProcessDTO, EmployeeSalaryIncreementProcessDTO>();

        public EmployeeSalaryIncreementProcessDTO onloadgetdetails(EmployeeSalaryIncreementProcessDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeSalaryIncreementProcessFacade/onloadgetdetails");
        }

        public EmployeeSalaryIncreementProcessDTO getReport(EmployeeSalaryIncreementProcessDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeSalaryIncreementProcessFacade/getReport");
        }
        public EmployeeSalaryIncreementProcessDTO Empdetails(EmployeeSalaryIncreementProcessDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeSalaryIncreementProcessFacade/Empdetails");
        }


        public EmployeeSalaryIncreementProcessDTO savedetails(EmployeeSalaryIncreementProcessDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeSalaryIncreementProcessFacade/");
        }
        public EmployeeSalaryIncreementProcessDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "EmployeeSalaryIncreementProcessFacade/getRecordById/");
        }
        public EmployeeSalaryIncreementProcessDTO deleterec(EmployeeSalaryIncreementProcessDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeSalaryIncreementProcessFacade/deactivateRecordById/");
        }





    }
}
