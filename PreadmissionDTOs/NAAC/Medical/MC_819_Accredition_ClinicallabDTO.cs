using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Medical
{
   public class MC_819_Accredition_ClinicallabDTO
    {
        public long NCMCCL819_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public long NCMCCL819_Year { get; set; }
        public string ASMAY_Year { get; set; }
        public bool NCMCCL819_NABHAccnTechHoslFlg { get; set; }
        public bool NCMCCL819_NABHAccnTechlabslFlg { get; set; }
        public bool NCMCCL819_CertificationDeptlFlg { get; set; }
        public bool NCMCCL819_OtherRecAccCertificationFlg { get; set; }
        public bool NCMCCL819_ActiveFlag { get; set; }
        public long NCMCCL819_CreatedBy { get; set; }
        public long NCMCCL819_UpdatedBy { get; set; }
        public DateTime? NCMCCL819_CreatedDate { get; set; }
        public DateTime? NCMCCL819_UpdatedDate { get; set; }
        public Array institutionlist { get; set; }
        public Array yearlist { get; set; }
        public Array alldata819MC { get; set; }
        public Array editdata { get; set; }
        public Array commentlist { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public MC_819_Accredition_ClinicallabDTO[] filelist { get; set; }
        public long NCMCCL819C_RemarksBy { get; set; }
        public string NCMCCL819C_Remarks { get; set; }
        public string NCMCCL819C_StatusFlg { get; set; }
        public string UserName { get; set; }
        public bool NCMCCL819C_ActiveFlag { get; set; }
        public long NCMCCL819C_CreatedBy { get; set; }
        public long NCMCCL819C_UpdatedBy { get; set; }
        public long NCMCCL819C_Id { get; set; }
        public long filefkid { get; set; }
        public DateTime? NCMCCL819C_CreatedDate { get; set; }
        public DateTime? NCMCCL819C_UpdatedDate { get; set; }
        public string Remarks { get; set; }

        //Dental 813
        public long NCDCCL813_Id { get; set; }      
        public long NCDCCL813_Year { get; set; }      
        public bool NCDCCL813_CentralSterileSuppliesDepartmentFlag { get; set; }
        public bool NCDCCL813_ProvidesPersonalProtectiveEquipmentFlag { get; set; }
        public bool NCDCCL813_PatientSafetyCurriculumFlag { get; set; }
        public bool NCDCCL813_PeriodicFumigationClinicalAreasFlag { get; set; }
        public bool NCDCCL813_ImmunizationOfAllTheCaregiversFlag { get; set; }
        public bool NCDCCL813_NeedleStickInjuryRegisterFlag { get; set; }
        public bool NCDCCL813_ActiveFlag { get; set; }
        public long NCDCCL813_CreatedBy { get; set; }
        public long NCDCCL813_UpdatedBy { get; set; }
        public DateTime? NCDCCL813_CreatedDate { get; set; }
        public DateTime? NCDCCL813_UpdatedDate { get; set; }
        public Array alldata813DC { get; set; }

        //Dental 815
        public long NCDCEQT815_Id { get; set; }      
        public long NCDCEQT815_Year { get; set; }      
        public bool NCDCEQT815_ConeBeamComputedTomogramFlag { get; set; }
        public bool NCDCEQT815_CAMFacilityFlag { get; set; }
        public bool NCDCEQT815_ImagingMorphomEtricSoftwaresFlag { get; set; }
        public bool NCDCEQT815_DentalLASERUnitFlag { get; set; }
        public bool NCDCEQT815_ExtendedApplicationLightBasedMicroscopyFlag { get; set; }
        public bool NCDCEQT815_ActiveFlag { get; set; }
        public long NCDCEQT815_CreatedBy { get; set; }
        public long NCDCEQT815_UpdatedBy { get; set; }
        public DateTime? NCDCEQT815_CreatedDate { get; set; }
        public DateTime? NCDCEQT815_UpdatedDate { get; set; }
        public Array alldata815DC { get; set; }

        //Dental 816
        public long NCDCSC816_Id { get; set; }      
        public long NCDCSC816_Year { get; set; }      
        public bool NCDCSC816_ComprehensiveclinicFlag { get; set; }
        public bool NCDCSC816_ImplantClinicFlag { get; set; }
        public bool NCDCSC816_GeriatricClinicFlag { get; set; }
        public bool NCDCSC816_SpecialHealthCareNeedsClinicFlag { get; set; }
        public bool NCDCSC816_TobaccoCessationClinicFlag { get; set; }
        public bool NCDCSC816_EstheticClinicFlag { get; set; }
        public bool NCDCSC816_ActiveFlag { get; set; }
        public long NCDCSC816_CreatedBy { get; set; }
        public long NCDCSC816_UpdatedBy { get; set; }
        public DateTime? NCDCSC816_CreatedDate { get; set; }
        public DateTime? NCDCSC816_UpdatedDate { get; set; }
        public Array alldata816DC { get; set; }
        public string NCMCCL819_StatusFlg { get; set; }
    }
}
