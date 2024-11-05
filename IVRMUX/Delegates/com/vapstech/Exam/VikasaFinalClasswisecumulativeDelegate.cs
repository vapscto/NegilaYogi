using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;

namespace IVRMUX.Delegates.com.vapstech.Exam
{
    public class VikasaFinalClasswisecumulativeDelegate
    {
        CommonDelegate<VikasaSubjectwiseCumulativeReportDTO, VikasaSubjectwiseCumulativeReportDTO> COMMM = new CommonDelegate<VikasaSubjectwiseCumulativeReportDTO, VikasaSubjectwiseCumulativeReportDTO>();

        public VikasaSubjectwiseCumulativeReportDTO Getdetails(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTDataExam(data, "VikasaFinalClasswisecumulativeFacade/Getdetails/");
        }

        public VikasaSubjectwiseCumulativeReportDTO showdetails(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTDataExam(data, "VikasaFinalClasswisecumulativeFacade/showdetails/");
        }

        public VikasaSubjectwiseCumulativeReportDTO get_class(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTDataExam(data, "VikasaFinalClasswisecumulativeFacade/get_class/");
        }

        public VikasaSubjectwiseCumulativeReportDTO get_section(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTDataExam(data, "VikasaFinalClasswisecumulativeFacade/get_section/");
        }

        public VikasaSubjectwiseCumulativeReportDTO get_subject(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTDataExam(data, "VikasaFinalClasswisecumulativeFacade/get_subject/");
        }
        public VikasaSubjectwiseCumulativeReportDTO get_category(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTDataExam(data, "VikasaFinalClasswisecumulativeFacade/get_category/");
        }
        public VikasaSubjectwiseCumulativeReportDTO get_subject_group(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTDataExam(data, "VikasaFinalClasswisecumulativeFacade/get_subject_group/");
        }
        

    }
}
