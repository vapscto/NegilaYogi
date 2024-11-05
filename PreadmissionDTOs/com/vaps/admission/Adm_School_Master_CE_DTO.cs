using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
   public class Adm_School_Master_CE_DTO
    {

        public long ASMCE_Id { get; set; }
        public long MI_Id { get; set; }
        public string ASMCE_CEName { get; set; }
        public string ASMCE_CECode { get; set; }
        public int ASMCE_Order { get; set; }
        public bool ASMCE_ActiveFlag { get; set; }


        public long ASSTCLCE_Id { get; set; }
        public long ASMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public bool ASSTCLCE_ActiveFlag { get; set; }
        public string ASMST_StreamName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public Array mastervehicle2 { get; set; }
        public Array mastervehicle { get; set; }
        public Array cexamlist { get; set; }
        public Array classlist { get; set; }
        public int order { get; set; }
        public Array streamlist { get; set; }
        public Array masterinsti { get; set; }
        public Array editdata { get; set; }
        public Array editdata2 { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
     
       
     

        public bool ASSTCLCE_CompulsoryFlg { get; set; }

    }
}
