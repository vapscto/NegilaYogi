using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class DocumentViewDTO
    {
        public long asmay_id { get; set; }
        public long mi_id { get; set; }
        public Array fillyear { get; set; }
        public Array registrationList { get; set; }

        public DocumentViewDTO[] registrationListnew  { get; set; }
        public Array admissioncatdrp { get; set; }

        public Array admissioncatdrpall { get; set; }
        public Array prospectusPaymentlist { get; set; }
        public int payementcheck { get; set; }
        public int configurationsettings { get; set; }
        public Array doc_list { get; set; }
        public long PASRD_Id { get; set; }
        public string Document_Path { get; set; }
        public string AMSMD_DocumentName { get; set; }

        public long pasr_id { get; set; }

        public string PASR_FirstName { get; set; }
        public string PASR_MiddleName { get; set; }
        public string PASR_LastName { get; set; }
        public string pasR_RegistrationNo { get; set; }
        public string pasR_Student_Pic_Path { get; set; }

        public string ASMCL_ClassName  { get; set; }

        public DocumentViewDTO[] ddoc { get; set; }

        public long  AMSMD_Id { get; set; }

        public Array studentDetailsTEmp { get; set; }

        public Array studentDetails { get; set; }

        public Array studentDetailsHelth { get; set; }

        public long ASMCL_Id { get; set; }

        public Array status_array { get; set; }
        public long PAMST_Id { get; set; }
        //public long MI_Id { get; set; }
        public string PAMST_Status { get; set; }
        public string PAMST_StatusFlag { get; set; }
        public int active { get; set; }
        public string msg { get; set; }
        public bool returnVal_update { get; set; }
        public bool returnVal { get; set; }
        public Array GridviewDetails { get; set; }
    }
}
