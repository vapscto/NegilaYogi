using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.University
{
   public class MC_343_TechnologyTransferredDTO
    {
        public long NCMCTT343_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMCTT343F_Id { get; set; }
        public long UserId { get; set; }
        public long NCMCTT343_Year { get; set; }
        public string NCMCTT343_PatenterName { get; set; }
        public string NCMCTT343_Patent { get; set; }
        public string NCMCTT343_Title { get; set; }
        public long NCMCTT343_CreatedBy { get; set; }
        public long NCMCTT343_UpdatedBy { get; set; }
        public DateTime? NCMCTT343_CreatedDate { get; set; }
        public DateTime? NCMCTT343_UpdatedDate { get; set; }
        public bool NCMCTT343_ActiveFlag { get; set; }

        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public Array institutionlist { get; set; }
        public string ASMAY_Year { get; set; }
        public long asmaY_Id { get; set; }
        public bool duplicate { get; set; }
        public MC_343_TechnologyTransferredDTO[] filelist { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public string msg { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public long NCMC8110IMMF_Id { get; set; }

    }
}
