using CommonLibrary;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Preadmission
{
    public class TotalCountClgDelegate
    {
        //preadmission status
        CommonDelegate<CommonDTO, CommonDTO> COMM = new CommonDelegate<CommonDTO, CommonDTO>();
        //
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CollegePreadmissionstudnetDto, CollegePreadmissionstudnetDto> COMMM = new CommonDelegate<CollegePreadmissionstudnetDto, CollegePreadmissionstudnetDto>();
        public CollegePreadmissionstudnetDto get_intial_data(CollegePreadmissionstudnetDto CollegePreadmissionstudnetDto)
        {
            return COMMM.CollegePOSTData(CollegePreadmissionstudnetDto, "TotalCountClgReportFacade/Get_Intial_data/");
        }

        public CollegePreadmissionstudnetDto Getdetails(CollegePreadmissionstudnetDto CollegePreadmissionstudnetDto)
        {
            return COMMM.CollegePOSTData(CollegePreadmissionstudnetDto, "TotalCountClgReportFacade/Getdetails/");
        }


        //preadmission status
     

        public CommonDTO getstatusdata(CommonDTO CollegePreadmissionstudnetDto)
        {
            return COMM.CollegePOSTData(CollegePreadmissionstudnetDto, "TotalCountClgReportFacade/getstatusdata/");
        }
        public CollegePreadmissionstudnetDto Clgapplicationstudocs( CollegePreadmissionstudnetDto cdto)
        {
           
            return COMMM.CollegePOSTData(cdto, "TotalCountClgReportFacade/Clgapplicationstudocs/");
        }
        public CollegePreadmissionstudnetDto Clgapplicationsturemarks(CollegePreadmissionstudnetDto cdto)
        {

            return COMMM.CollegePOSTData(cdto, "TotalCountClgReportFacade/Clgapplicationsturemarks/");
        }
    }
}
