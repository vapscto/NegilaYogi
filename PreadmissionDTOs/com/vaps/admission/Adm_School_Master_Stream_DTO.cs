using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class Adm_School_Master_Stream_DTO
    {

        public long ASMST_Id { get; set; }
        public long MI_Id { get; set; }
        public string ASMST_StreamName { get; set; }
        public string ASMST_StreamCode { get; set; }
        public int ASMST_Order { get; set; }
        public bool ASMST_ActiveFlag { get; set; }
        public long ASSTCL_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public Array streamlist { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array masterinsti { get; set; }
        public Array mastervehicle { get; set; }
        public Array mastervehicle2 { get; set; }
        public Array editdata { get; set; }
        public Array streamdetails { get; set; }
        public Array sectionlistedit { get; set; }
        public int order { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public bool retrunval2 { get; set; }
        public bool ASSTCL_ActiveFlag { get; set; }
        public bool editedit { get; set; }
        public string flag { get; set; }
        public long secid { get; set; }
        public bool del { get; set; }
        public Adm_School_Master_Stream_DTO[] selectedsectionlist {get;set;}

    }
}
