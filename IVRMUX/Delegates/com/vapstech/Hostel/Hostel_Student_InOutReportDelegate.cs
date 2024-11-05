using CommonLibrary;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class Hostel_Student_InOutReportDelegate 
    {
        CommonDelegate<Hostel_Student_InOutDTO, Hostel_Student_InOutDTO> _comm = new CommonDelegate<Hostel_Student_InOutDTO, Hostel_Student_InOutDTO>();
        public Hostel_Student_InOutDTO getdetails(Hostel_Student_InOutDTO data)
        {
            return _comm.Post_Hostel(data, "Hostel_Student_InOutReportFacade/loaddata/");
        }
        public Hostel_Student_InOutDTO empname(Hostel_Student_InOutDTO data)
        {
            return _comm.Post_Hostel(data, "Hostel_Student_InOutReportFacade/empname/");
        }
        public Hostel_Student_InOutDTO savedetail(Hostel_Student_InOutDTO data)
        {
            return _comm.Post_Hostel(data, "Hostel_Student_InOutReportFacade/savedetail/");
        }
        public Hostel_Student_InOutDTO deleterec(Hostel_Student_InOutDTO data)
        {
            return _comm.Post_Hostel(data, "Hostel_Student_InOutReportFacade/deletedetails/");
        }

        //report
        public Hostel_Student_InOutDTO loaddata(Hostel_Student_InOutDTO data)
        {
            return _comm.Post_Hostel(data, "Hostel_Student_InOutReportFacade/getloaddata/");
        }

        public Hostel_Student_InOutDTO report(Hostel_Student_InOutDTO data)
        {
            return _comm.Post_Hostel(data, "Hostel_Student_InOutReportFacade/report/");
        }

    }
}
