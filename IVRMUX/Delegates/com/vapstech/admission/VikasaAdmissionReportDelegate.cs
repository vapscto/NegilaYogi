using System;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;

namespace corewebapi18072016.Delegates.com.vapstech.admission
{
    public class VikasaAdmissionReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<VikasaAdmissionreportDTO, VikasaAdmissionreportDTO> comml = new CommonDelegate<VikasaAdmissionreportDTO, VikasaAdmissionreportDTO>();



        public VikasaAdmissionreportDTO getdetails(int id)
        {
            return comml.GetDataByIdADM(id, "VikasaAdmissionReportFacade/getdata/");
        }


        public VikasaAdmissionreportDTO getStudDatabyclass(VikasaAdmissionreportDTO data)
        {
            return comml.POSTDataADM(data, "VikasaAdmissionReportFacade/getstudbyclass/");
        }


        public VikasaAdmissionreportDTO onacademicyearchange(VikasaAdmissionreportDTO data)
        {
            return comml.POSTDataADM(data, "VikasaAdmissionReportFacade/onacademicyearchange/");
        }


        public VikasaAdmissionreportDTO GetStudDataById(VikasaAdmissionreportDTO stuDTO)
        {
            return comml.POSTDataADM(stuDTO, "VikasaAdmissionReportFacade/getStudData/");
        }
        public VikasaAdmissionreportDTO searchfilter(VikasaAdmissionreportDTO stuDTO)
        {
            return comml.POSTDataADM(stuDTO, "VikasaAdmissionReportFacade/searchfilter/");
        }
        public VikasaAdmissionreportDTO ShowReport(VikasaAdmissionreportDTO data)
        {
            return comml.POSTDataADM(data, "VikasaAdmissionReportFacade/ShowReport/");
        }
        public VikasaAdmissionreportDTO ShowReport1(VikasaAdmissionreportDTO data)
        {
            return comml.POSTDataADM(data, "VikasaAdmissionReportFacade/ShowReport1/");
        }

        

    }
}
