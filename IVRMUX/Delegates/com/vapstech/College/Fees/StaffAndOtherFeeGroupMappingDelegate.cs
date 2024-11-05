using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates
{
    public class StaffAndOtherFeeGroupMappingDelegate
    {
        CommonDelegate<Clg_StudentFeeGroupMapping_DTO, Clg_StudentFeeGroupMapping_DTO> COMMM = new CommonDelegate<Clg_StudentFeeGroupMapping_DTO, Clg_StudentFeeGroupMapping_DTO>();
        public Clg_StudentFeeGroupMapping_DTO getdata(Clg_StudentFeeGroupMapping_DTO data)
        {
           
               

            return COMMM.PostClgFee(data, "StaffAndOtherFeeGroupMappingFacade/getalldetails/");
        }

        public Clg_StudentFeeGroupMapping_DTO getstucls(Clg_StudentFeeGroupMapping_DTO data)
        {
       
               
            return COMMM.PostClgFee(data, "StaffAndOtherFeeGroupMappingFacade/getgroupmappedheads/");
        }


        public Clg_StudentFeeGroupMapping_DTO fillstudentsroute(Clg_StudentFeeGroupMapping_DTO data)
        {

            return COMMM.PostClgFee(data, "StaffAndOtherFeeGroupMappingFacade/fillstudentsroute/");
         
            
        }

        public Clg_StudentFeeGroupMapping_DTO EditDetails(int id)
        {
            //  return COMMM.PostClgFee(id, "StaffAndOtherFeeGroupMappingFacade/Editdetails/");
            // return COMMM.PostClgFee(id, "StaffAndOtherFeeGroupMappingFacade/Editdetails/");
            //  var response = client.GetAsync("api/StudentFeeGroupMappingFacade/Editdetails/" + id).Result;
            return COMMM.GETClgFee(id, "StaffAndOtherFeeGroupMappingFacade/Editdetails/");


        }

        public Clg_StudentFeeGroupMapping_DTO savedetails(Clg_StudentFeeGroupMapping_DTO pgmod)
        {
            return COMMM.PostClgFee(pgmod, "StaffAndOtherFeeGroupMappingFacade/");
           
        }
        public Clg_StudentFeeGroupMapping_DTO savedetails_s(Clg_StudentFeeGroupMapping_DTO pgmod)
        {
            return COMMM.PostClgFee(pgmod, "StaffAndOtherFeeGroupMappingFacade/savedetails_s/");
            //var response = client.PostAsync("api/StudentFeeGroupMappingFacade/savedetails_s", byteContent).Result;
            
        }
        public Clg_StudentFeeGroupMapping_DTO savedetails_o(Clg_StudentFeeGroupMapping_DTO pgmod)
        {
             // var response = client.PostAsync("api/StudentFeeGroupMappingFacade/savedetails_o", byteContent).Result;
            return COMMM.PostClgFee(pgmod, "StaffAndOtherFeeGroupMappingFacade/savedetails_o/");
        }


        public Clg_StudentFeeGroupMapping_DTO deleterec(Clg_StudentFeeGroupMapping_DTO enqdto)
        {
               // var response = client.PostAsync("api/StudentFeeGroupMappingFacade/deletemodpages/", byteContent).Result;
            return COMMM.PostClgFee(enqdto, "StaffAndOtherFeeGroupMappingFacade/deletemodpages/");
        }
        public Clg_StudentFeeGroupMapping_DTO deleterec_s(Clg_StudentFeeGroupMapping_DTO enqdto)
        {
            
           //     var response = client.PostAsync("api/StudentFeeGroupMappingFacade/deletemodpages_s/", byteContent).Result;
            return COMMM.PostClgFee(enqdto, "StaffAndOtherFeeGroupMappingFacade/deletemodpages_s/");
        }


        public Clg_StudentFeeGroupMapping_DTO deleterec_o(Clg_StudentFeeGroupMapping_DTO enqdto)
        {
            
               
            return COMMM.PostClgFee(enqdto, "StaffAndOtherFeeGroupMappingFacade/deletemodpages_o/");
        }
        public Clg_StudentFeeGroupMapping_DTO getsearchdata(int data, Clg_StudentFeeGroupMapping_DTO dataa)
        {
            return COMMM.PostClgFee(dataa, "StaffAndOtherFeeGroupMappingFacade/1/");
           // var response = client.PostAsync("api/StudentFeeGroupMappingFacade/1/", byteContent).Result;
            
        }

        public Clg_StudentFeeGroupMapping_DTO getradiofiltereddata(Clg_StudentFeeGroupMapping_DTO pgmod)
        {
            return COMMM.PostClgFee(pgmod, "StaffAndOtherFeeGroupMappingFacade/radiobtndata");
           // var response = client.PostAsync("api/StudentFeeGroupMappingFacade/radiobtndata", byteContent).Result;
            
        }

        public Clg_StudentFeeGroupMapping_DTO studentsavedgroupcon(Clg_StudentFeeGroupMapping_DTO pgmod)
        {
            return COMMM.PostClgFee(pgmod, "StaffAndOtherFeeGroupMappingFacade/studentsavedgroup/");
           // var response = client.PostAsync("api/StudentFeeGroupMappingFacade/studentsavedgroup", byteContent).Result;
            
        }


        public Clg_StudentFeeGroupMapping_DTO getdataaspercategory(Clg_StudentFeeGroupMapping_DTO pgmod)
        {
            return COMMM.PostClgFee(pgmod, "StaffAndOtherFeeGroupMappingFacade/getclassoncatselect");
         //   var response = client.PostAsync("api/StudentFeeGroupMappingFacade/getclassoncatselect", byteContent).Result;
            
        }

        public Clg_StudentFeeGroupMapping_DTO searching(Clg_StudentFeeGroupMapping_DTO enqdto)
        {
            return COMMM.PostClgFee(enqdto, "StaffAndOtherFeeGroupMappingFacade/searching");

         //   var response = client.PostAsync("api/StudentFeeGroupMappingFacade/searching", byteContent).Result;

                
        }
        public Clg_StudentFeeGroupMapping_DTO searching_s(Clg_StudentFeeGroupMapping_DTO enqdto)
        {
            return COMMM.PostClgFee(enqdto, "StaffAndOtherFeeGroupMappingFacade/searching_s");

           // var response = client.PostAsync("api/StudentFeeGroupMappingFacade/searching_s", byteContent).Result;
            
        }
        public Clg_StudentFeeGroupMapping_DTO searching_o(Clg_StudentFeeGroupMapping_DTO enqdto)
        {

            return COMMM.PostClgFee(enqdto, "StaffAndOtherFeeGroupMappingFacade/searching_o");

            //var response = client.PostAsync("api/StudentFeeGroupMappingFacade/searching_o", byteContent).Result;

              
        }
        public Clg_StudentFeeGroupMapping_DTO saveeditdata(Clg_StudentFeeGroupMapping_DTO pgmod)
        {

            //   var response = client.PostAsync("api/StudentFeeGroupMappingFacade/saveeditdata", byteContent).Result;

            return COMMM.PostClgFee(pgmod, "StaffAndOtherFeeGroupMappingFacade/saveeditdata");

        }


        public Clg_StudentFeeGroupMapping_DTO searchingstu(Clg_StudentFeeGroupMapping_DTO enqdto)
        {
            return COMMM.PostClgFee(enqdto, "StaffAndOtherFeeGroupMappingFacade/searchingstud");

            //var response = client.PostAsync("api/StudentFeeGroupMappingFacade/searchingstud", byteContent).Result;

             
        }
        public Clg_StudentFeeGroupMapping_DTO searchingstaff(Clg_StudentFeeGroupMapping_DTO enqdto)
        {
            return COMMM.PostClgFee(enqdto, "StaffAndOtherFeeGroupMappingFacade/searchingstaff");

            //var response = client.PostAsync("api/StudentFeeGroupMappingFacade/searchingstaff", byteContent).Result;
            
        }


        public Clg_StudentFeeGroupMapping_DTO searchingothers(Clg_StudentFeeGroupMapping_DTO enqdto)
        {
            return COMMM.PostClgFee(enqdto, "StaffAndOtherFeeGroupMappingFacade/searchingothers");

        //    var response = client.PostAsync("api/StudentFeeGroupMappingFacade/searchingothers", byteContent).Result;

              
        }

        public Clg_StudentFeeGroupMapping_DTO studentdataedit(Clg_StudentFeeGroupMapping_DTO enqdto)
        {
            return COMMM.PostClgFee(enqdto, "StaffAndOtherFeeGroupMappingFacade/geteditdata");

         //   var response = client.PostAsync("api/StudentFeeGroupMappingFacade/geteditdata", byteContent).Result;
            
        }
        public Clg_StudentFeeGroupMapping_DTO getacademicye(Clg_StudentFeeGroupMapping_DTO enqdto)
        {
            return COMMM.PostClgFee(enqdto, "StaffAndOtherFeeGroupMappingFacade/getacademicyear");

            //var response = client.PostAsync("api/StudentFeeGroupMappingFacade/getacademicyear", byteContent).Result;

            
        }


        public Clg_StudentFeeGroupMapping_DTO geteditdatastaffothers(Clg_StudentFeeGroupMapping_DTO enqdto)
        {
         
               // var response = client.PostAsync("api/StudentFeeGroupMappingFacade/geteditdatastaffothers", byteContent).Result;
            return COMMM.PostClgFee(enqdto, "StaffAndOtherFeeGroupMappingFacade/geteditdatastaffothers");


        }


        public Clg_StudentFeeGroupMapping_DTO saveeditdatastaff(Clg_StudentFeeGroupMapping_DTO enqdto)
        {
            
           //     var response = client.PostAsync("api/StudentFeeGroupMappingFacade/geteditdatastaffothers", byteContent).Result;
            return COMMM.PostClgFee(enqdto, "StaffAndOtherFeeGroupMappingFacade/geteditdatastaffothers");


        }


        public Clg_StudentFeeGroupMapping_DTO saveeditdataothers(Clg_StudentFeeGroupMapping_DTO enqdto)
        {
            return COMMM.PostClgFee(enqdto, "StaffAndOtherFeeGroupMappingFacade/saveeditdataothers");

            //var response = client.PostAsync("api/StudentFeeGroupMappingFacade/saveeditdataothers", byteContent).Result;
            
        }

    }
}
