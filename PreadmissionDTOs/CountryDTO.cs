using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class CountryDTO : CommonParamDTO
    {
        public Array countryDrpDown { get; set; }
        public Array citydro { get; set; }
        public Array locationdrp { get; set; }
        public Array routedrp { get; set; }
        public Array categorydrp { get; set; }
        public Array admissioncatdrp { get; set; }
        public string returnval { get; set; }

        public Array electivegrouplist { get; set; }
        public long ASMCL_ID { get; set; }

        public DateTime ASMAY_PreAdm_F_Date { get; set; }

        public Array classcategoryList { get; set; }


        public Array admissioncatdrpall  { get; set; }
        public bool specialuser { get; set; }

        public Array meetinglist { get; set; }
        public Array academicdrp { get; set; }
        public Array castedrp { get; set; }

        public Array subcastedrp  { get; set; }
        public Array religiondrp { get; set; }
        public Array castecategorydrp { get; set; }
        public Array registrationList { get; set; }

        // 30-9-2016
        public int Organization { get; set; }
        public int Institute { get; set; }
        public Array mstConfig { get; set; }
        public string defaultcurrency { get; set; } //added on 11/11/2016

        // 30-9-2016


        //---------//

        public long MI_Id { get; set; }

        public long ASMAY_Id { get; set; }

        public long Id { get; set; }

        public Array sectiondropdown { get; set; }
        public Array DocumentList { get; set; }

        public Array syllabuslist { get; set; } // 20/12/2016 16:25

        public Array syllabuslistoth { get; set; } // 20/12/2016 16:25
        public Array prospectusPaymentlist { get; set; }
        
        //cutofdate
        public long roleId { get; set; }
        public DateTime cutoffdate { get; set; }

        public string precutdate  { get; set; }
        public Array caste_doc_maplist { get; set; }

        public Array statuslist { get; set; }

        public Array vaccines { get; set; }

        public string htmldata { get; set; }

        public string dashboardpage { get; set; }

        public string prospectusfilePath { get; set; }

        public Array registrationListhealth  { get; set; }

        public bool multipleapplications { get; set; }
        public bool multipleapplicationsc { get; set; }

        public Array totalcountDetails { get; set; }

        public bool countrole { get; set; }

        public string roleName { get; set; }
        public Array prospectuslist { get; set; }

        public Array studentDetailsTEmp { get; set; }

        public Array areaList { get; set; }

        public Array studentDetailsss { get; set; }

        public string PASR_Languagespeaking { get; set; }

        public string PASR_FatherPanno { get; set; }

        public string PASR_MotherPanno { get; set; }

        public long pasr_id { get; set; }

        public int PASR_UnderAge { get; set; }

        public int PASR_OverAge { get; set; }

        public Array sourcedropDown { get; set; }

        public Array fillpaymentgateway { get; set; }

        public long FPGD_Id { get; set; }
        public string FPGD_PGName { get; set; }
        public string FPGD_Image { get; set; }

        public string username { get; set; }

        public string useremail { get; set; }
        public string mobilenumber { get; set; }
        public string stuusername { get; set; } 

        public string useimagepath { get; set; }

        public Array userdetails { get; set; }

        public Array StudentReferenceDetails { get; set; }
        public Array StudentSourceDetails { get; set; }

        public Array admissioncongigurationList { get; set; }
    }
}
