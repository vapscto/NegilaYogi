using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;


namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class Hostel_Student_GatePassDelegate
    {
            private const String JsonContentType = "application/json; charset=utf-8";
            CommonDelegate<Hostel_Student_GatePassDTO, Hostel_Student_GatePassDTO> COMMM = new CommonDelegate<Hostel_Student_GatePassDTO, Hostel_Student_GatePassDTO>();

            public Hostel_Student_GatePassDTO onloadgetdetails(Hostel_Student_GatePassDTO dto)
            {
                return COMMM.Post_Hostel(dto, "Hostel_Student_GatePassFacade/onloadgetdetails");
            }

            public Hostel_Student_GatePassDTO savedetails(Hostel_Student_GatePassDTO dto)
            {
                return COMMM.Post_Hostel(dto, "Hostel_Student_GatePassFacade/savedetails");
            }
        public Hostel_Student_GatePassDTO Edit(Hostel_Student_GatePassDTO maspage)
        {
            return COMMM.Post_Hostel(maspage, "Hostel_Student_GatePassFacade/Edit/");
        }
        public Hostel_Student_GatePassDTO deleterec(Hostel_Student_GatePassDTO maspage)
        {
            return COMMM.Post_Hostel(maspage, "Hostel_Student_GatePassFacade/deactivateRecordById/");
        }

    }
}
