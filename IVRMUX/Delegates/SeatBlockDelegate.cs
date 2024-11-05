using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PreadmissionDTOs;
using CommonLibrary;
namespace corewebapi18072016.Delegates
{
    public class SeatBlockDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Preadmission_SeatBlocked_StudentDTO, Preadmission_SeatBlocked_StudentDTO> COMMM = new CommonDelegate<Preadmission_SeatBlocked_StudentDTO, Preadmission_SeatBlocked_StudentDTO>();

        public Preadmission_SeatBlocked_StudentDTO saveSeatBlockdetails(Preadmission_SeatBlocked_StudentDTO SeatBlocked)
        {
            return COMMM.POSTData(SeatBlocked, "SeatBlockFacade/");
        }
        public Preadmission_SeatBlocked_StudentDTO getSeatBlockdata(int Mi_id)
        {
            return COMMM.GetDataById(Mi_id, "SeatBlockFacade/");
        }
       

        //by id

        public Preadmission_SeatBlocked_StudentDTO getSeatBlockDetailsbySeatBlockId(int SeatBlockId)
        {
            return COMMM.GetDataById(SeatBlockId, "SeatBlockFacade/getdetailsById/");

        }

        //delete record
        public Preadmission_SeatBlocked_StudentDTO deleterec(int id)
        {
            return COMMM.DeleteDataById(id, "SeatBlockFacade/deletedetails/");
        }
        public Preadmission_SeatBlocked_StudentDTO getSeatConfirmedStud(Preadmission_SeatBlocked_StudentDTO stud)
        {
            return COMMM.POSTData(stud, "SeatBlockFacade/getSeatConfirmedStud/");
        }
    }
}
