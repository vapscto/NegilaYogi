using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class Hostel_Student_Gatepass_ProcessDelegate
    {


            private const String JsonContentType = "application/json; charset=utf-8";
            CommonDelegate<Hostel_Student_GatePassDTO, Hostel_Student_GatePassDTO> COMMM = new CommonDelegate<Hostel_Student_GatePassDTO, Hostel_Student_GatePassDTO>();

            public Hostel_Student_GatePassDTO onloadgetdetails(Hostel_Student_GatePassDTO dto)
            {
                return COMMM.Post_Hostel(dto, "Hostel_Student_Gatepass_ProcessFacade/onloadgetdetails");
            }
        public Hostel_Student_GatePassDTO empdetails(Hostel_Student_GatePassDTO dto)
        {
            return COMMM.Post_Hostel(dto, "Hostel_Student_Gatepass_ProcessFacade/empdetails");
        }

        public Hostel_Student_GatePassDTO approvedrecord(Hostel_Student_GatePassDTO dto)
        {
            return COMMM.Post_Hostel(dto, "Hostel_Student_Gatepass_ProcessFacade/approvedrecord");
        }

        ////------------------  Approval Report------------------------------
        public Hostel_Student_GatePassDTO Onload(Hostel_Student_GatePassDTO dto)
        {
            return COMMM.Post_Hostel(dto, "Hostel_Student_Gatepass_ProcessFacade/Onload");
        }
        public Hostel_Student_GatePassDTO getapprovalreport(Hostel_Student_GatePassDTO dto)
        {
            return COMMM.Post_Hostel(dto, "Hostel_Student_Gatepass_ProcessFacade/getapprovalreport");
        }

        //GatePass Admin Apply
        public Hostel_Student_GatePassDTO getGatePassAdminApplyOnload(Hostel_Student_GatePassDTO id)
        {
            return COMMM.Post_Hostel(id, "Hostel_Student_Gatepass_ProcessFacade/getGatePassAdminApplyOnload/");
        }
        public Hostel_Student_GatePassDTO SaveUpdate(Hostel_Student_GatePassDTO data)
        {
            return COMMM.Post_Hostel(data, "Hostel_Student_Gatepass_ProcessFacade/SaveUpdate/");
        }
        public Hostel_Student_GatePassDTO UpdateStatus(Hostel_Student_GatePassDTO data)
        {
            return COMMM.Post_Hostel(data, "Hostel_Student_Gatepass_ProcessFacade/UpdateStatus/");
        }
        public Hostel_Student_GatePassDTO deactivate(Hostel_Student_GatePassDTO data)
        {
            return COMMM.Post_Hostel(data, "Hostel_Student_Gatepass_ProcessFacade/deactivate/");
        }
        public Hostel_Student_GatePassDTO Edit(Hostel_Student_GatePassDTO data)
        {
            return COMMM.Post_Hostel(data, "Hostel_Student_Gatepass_ProcessFacade/Edit/");
        }



    }
}

