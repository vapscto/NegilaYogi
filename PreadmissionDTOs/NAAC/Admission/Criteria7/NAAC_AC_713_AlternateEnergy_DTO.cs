using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
   public class NAAC_AC_713_AlternateEnergy_DTO : CommonParamDTO
    {
        public long NCAC713ALTENE_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC713ALTENE_Year { get; set; }
        public string NCAC713ALTENE_PowerRequirements { get; set; }
        public long NCAC713ALTENE_TotalPowerReq { get; set; }
        public string NCAC713ALTENE_EnergySource { get; set; }
        public string NCAC713ALTENE_EnergyUsed { get; set; }
        public string NCAC713ALTENE_EnergySupplied { get; set; }
        public string NCAC713ALTENEF_Filedesc { get; set; }
        public string NCAC713ALTENEF_FileName { get; set; }
        public string NCAC713ALTENEF_FilePath { get; set; }
        public bool NCAC713ALTENE_ActiveFlg { get; set; }
        public long NCAC713ALTENE_CreatedBy { get; set; }
        public long NCAC713ALTENE_UpdatedBy { get; set; }
        public long NCAC713ALTENE_CreatedDate { get; set; }
        public long NCAC713ALTENE_UpdatedDate { get; set; }
        public long NCAC713ALTENEF_Id { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public long UserId { get; set; }
        public string retrunMsg { get; set; }

        public Array allacademicyear { get; set; }
        public Array alldatalist { get; set; }
        public Array editlisttab1 { get; set; }
        public Array editfilelist { get; set; }
        public NAAC_AC_713_AlternateEnergy_DTO[] NAACAC7DTO { get; set; }
        public string ASMAY_Year { get; set; }
        public string MI_Name { get; set; }
        public Array institutionlist { get; set; }

        //MC
        public long NCMC713ALTENE_Id { get; set; }
        public string NCMC713ALTENE_SolarenergyFlag { get; set; }
        public string NCMC713ALTENE_WindenergyFlag { get; set; }
        public string NCMC713ALTENE_SensorbasedEnergyFlag { get; set; }
        public string NCMC713ALTENE_BiogasPlantFlag { get; set; }
        public string NCMC713ALTENE_LEDbulbsORPowerefficEquipFlag { get; set; }
        public long NCMC713ALTENE_CreatedBy { get; set; }
        public long NCMC713ALTENE_UpdatedBy { get; set; }
        public long NCMC713ALTENE_CreatedDate { get; set; }
        public long NCMC713ALTENE_UpdatedDate { get; set; }
        public long NCMC713ALTENE_Year { get; set; }
        public bool NCMC713ALTENE_ActiveFlg { get; set; }
        //MC
    }
}
