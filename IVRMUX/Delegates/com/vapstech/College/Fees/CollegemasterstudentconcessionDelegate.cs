using System;
using PreadmissionDTOs.com.vaps.College.Fees;
using CommonLibrary;

namespace corewebapi18072016.Delegates.com.vapstech.College.Fees.Masters
{
    public class CollegemasterstudentconcessionDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<CollegeConcessionDTO, CollegeConcessionDTO> COMMM = new CommonDelegate<CollegeConcessionDTO, CollegeConcessionDTO>();


        public CollegeConcessionDTO getdata(CollegeConcessionDTO data)
        {

            return COMMM.PostClgFee(data, "CollegemasterstudentconcessionFacade/getdata/");


     
        }


        public CollegeConcessionDTO get_courses(CollegeConcessionDTO data)
        {

            return COMMM.POSTDataCollfee(data, "CollegemasterstudentconcessionFacade/get_courses/");





        }


        public CollegeConcessionDTO get_branches(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegemasterstudentconcessionFacade/get_branches/");
            
          
        }



        public CollegeConcessionDTO get_semisters(CollegeConcessionDTO data)
        {


            return COMMM.POSTDataCollfee(data, "CollegemasterstudentconcessionFacade/get_semisters/");


        
        }


        public CollegeConcessionDTO get_student(CollegeConcessionDTO data)
        {


            return COMMM.POSTDataCollfee(data, "CollegemasterstudentconcessionFacade/get_student/");


        }



        public CollegeConcessionDTO fillheaddetailsss(CollegeConcessionDTO data)
        {

            return COMMM.POSTDataCollfee(data, "CollegemasterstudentconcessionFacade/getdata/");

         


        }


        public CollegeConcessionDTO fillamount(CollegeConcessionDTO data)
        {
            
            return COMMM.POSTDataCollfee(data, "CollegemasterstudentconcessionFacade/fillamount/");
            
       
        }
        
        public CollegeConcessionDTO savedata(CollegeConcessionDTO data)
        {



            return COMMM.POSTDataCollfee(data, "CollegemasterstudentconcessionFacade/savedata/");

      
        }

        public CollegeConcessionDTO DeletRecord(CollegeConcessionDTO data)
        {

            return COMMM.POSTDataCollfee(data, "CollegemasterstudentconcessionFacade/DeletRecord/");
            

        
        }
    }
}
