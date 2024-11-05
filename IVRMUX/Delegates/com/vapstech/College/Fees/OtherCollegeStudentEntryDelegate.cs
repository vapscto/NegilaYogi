using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Fees
{
    public class OtherCollegeStudentEntryDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonLibrary.CommonDelegate<Fee_Master_College_OtherStudentsDTO, Fee_Master_College_OtherStudentsDTO> COMMM =
            new CommonDelegate<Fee_Master_College_OtherStudentsDTO, Fee_Master_College_OtherStudentsDTO>();
        public Fee_Master_College_OtherStudentsDTO getdata(int id)
        {
            return COMMM.GETClgFee(id, "OtherCollegeStudentEntryFacade/getdetails/");
         //   var response = client.GetAsync("api/OtherCollegeStudentEntryFacade/getdetails/" + id).Result;

               
        }
        public Fee_Master_College_OtherStudentsDTO save(Fee_Master_College_OtherStudentsDTO data)
        {
            return COMMM.POSTDataCollfee(data, "OtherCollegeStudentEntryFacade/save/");
           // var response = client.PostAsync("api/OtherCollegeStudentEntryFacade/save/", byteContent).Result;

              
        }
        public Fee_Master_College_OtherStudentsDTO edit(int id)
        {
            return COMMM.GETClgFee(id, "OtherCollegeStudentEntryFacade/edit/");
           // var response = client.GetAsync("api/OtherCollegeStudentEntryFacade/edit/" + id).Result;

               
        }
        public Fee_Master_College_OtherStudentsDTO delete(int id)
        {
            return COMMM.GETClgFee(id, "OtherCollegeStudentEntryFacade/delete/");
           // var response = client.GetAsync("api/OtherCollegeStudentEntryFacade/delete/" + id).Result;
            
        }
    }
}
