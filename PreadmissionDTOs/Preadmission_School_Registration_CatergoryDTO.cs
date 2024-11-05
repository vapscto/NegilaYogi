using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class Preadmission_School_Registration_CatergoryDTO : CommonParamDTO
    {
        public long PASRC_Id { get; set; }
        public long PASR_Id { get; set; }
        public long AMC_Id { get; set; }

        public long HRME_Id { get; set; }
        public long PASS_Id { get; set; }
        public long PASRS_Id { get; set; }

        public long FMCC_Id { get; set; }

        public string concessiontype { get; set; }
        public Array fillcategory { get; set; }
        public Array fillstudentlst { get; set; }
        public Array fillcheckedlst { get; set; }

        public Array fillstaff { get; set; }
        public long MI_ID { get; set; }
        public long userid { get; set; }
        public long ASMAY_Id { get; set; }

        public string confirmorrejectstatus { get; set; }
        public string PASRS_SiblingsAdmissionNo { get; set; }
        public string PASRS_SiblingsName { get; set; }
        public string PASRS_SiblingsClass { get; set; }
        public List<StudentSiblingDTO> studentdetails { get; set; }
        public List<StudentSiblingDTO> studentdetails1 { get; set; }
        public bool returnval { get; set; }
        public string verrejstatus { get; set; }

        public long AMST_Id { get; set; }

        public string studentphtoto { get; set; }

        public string fathername { get; set; }

        public MasterConfigurationDTO configurationsettings { get; set; }

        public Array concessionliststudent { get; set; }

        public Array concessionliststaff { get; set; }

    }
}
