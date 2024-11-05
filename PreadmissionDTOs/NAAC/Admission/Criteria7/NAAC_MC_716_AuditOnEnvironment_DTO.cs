using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
   public class NAAC_MC_716_AuditOnEnvironment_DTO : CommonParamDTO
    {
        public long NCMC716AOE_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMC716AOE_Year { get; set; }
        public string NCMC716AOE_GreenauditFlag { get; set; }
        public string NCMC716AOE_EnergyAuditFlag { get; set; }
        public string NCMC716AOE_EnvironmentAuditFlag { get; set; }
        public string NCMC716AOE_CleanandgreenCampusRecognitionsFlag { get; set; }
        public bool NCMC716AOE_ActiveFlag { get; set; }
        public long NCMC716AOE_CreatedBy { get; set; }
        public long NCMC716AOE_UpdatedBy { get; set; }
        public DateTime NCMC716AOE_CreatedDate { get; set; }
        public DateTime NCMC716AOE_UpdatedDate { get; set; }
        public long NCMC716AOEF_Id { get; set; }
        public string NCMC716AOEF_FileName { get; set; }
        public string NCMC716AOEF_Filedesc { get; set; }
        public string NCMC716AOEF_FilePath { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public long UserId { get; set; }
        public string retrunMsg { get; set; }

        public Array allacademicyear { get; set; }
        public Array alldatalist { get; set; }
        public Array editlisttab1 { get; set; }
        public Array editfilelist { get; set; }
        public NAAC_MC_716_AuditOnEnvironment_DTO[] NAACMC716DTO { get; set; }
        public string ASMAY_Year { get; set; }
        public string MI_Name { get; set; }
        public Array institutionlist { get; set; }
    }
}
