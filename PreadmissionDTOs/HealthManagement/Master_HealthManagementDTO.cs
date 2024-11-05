using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HealthManagement
{
    public class Master_HealthManagementDTO
    {
        public long HMMBEH_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string HMMBEH_BehaviourName { get; set; }
        public string HMMBEH_BehaviourDesc { get; set; }
        public bool HMMBEH_ActiveFlg { get; set; }
        public DateTime HMMBEH_CreatedDate { get; set; }
        public DateTime HMMBEH_UpdatedDate { get; set; }
        public long HMMBEH_CreatedBy { get; set; }
        public long HMMBEH_UpdatedBy { get; set; }
        public Array behaviourlist { get; set; }
        public Array behaviour_edit { get; set; }
        public string message { get; set; }

        public long HMMCLN_Id { get; set; }
        public string HMMCLN_CleannessName { get; set; }
        public string HMMCLN_CleannessDesc { get; set; }
        public bool HMMCLN_ActiveFlg { get; set; }
        public Array clennesslist { get; set; }
        public Array clenness_edit { get; set; }

        public long HMMDOC_Id { get; set; }
        public string HMMDOC_DoctorName { get; set; }
        public string HMMDOC_DoctorQualification { get; set; }
        public string HMMDOC_Specialisation { get; set; }
        public string HMMDOC_Address { get; set; }
        public string HMMDOC_Phoneno { get; set; }
        public string HMMDOC_EmailId { get; set; }
        public string HMMDOC_BloodGroup { get; set; }
        public bool HMMDOC_ActiveFlg { get; set; }
        public Array doctorlist { get; set; }
        public Array doctor_edit { get; set; }

        public long HMMEXM_Id { get; set; }
        public string HMMEXM_ExaminationName { get; set; }
        public string HMMEXM_ExamDesc { get; set; }
        public bool HMMEXM_ActiveFlg { get; set; }
        public Array examinationlist { get; set; }
        public Array examination_edit { get; set; }

        public long HMMOBS_Id { get; set; }
        public string HMMOBS_Observation { get; set; }
        public string HMMOBS_ObservationDesc { get; set; }
        public bool HMMOBS_ActiveFlg { get; set; }
        public DateTime? HMMOBS_CreatedDate { get; set; }
        public DateTime? HMMOBS_UpdatedDate { get; set; }
        public long HMMOBS_CreatedBy { get; set; }
        public long HMMOBS_UpdatedBy { get; set; }
        public Array observation_edit { get; set; }
        public Array observationlist { get; set; }

        // Master Illness
        public long HMMILL_Id { get; set; }      
        public string HMMILL_IllnessName { get; set; }
        public string HMMILL_IllnessDesc { get; set; }
        public bool HMMILL_ActiveFlg { get; set; }
        public bool Returnval { get; set; }
        public Array GetIllnessLoadDataList { get; set; }
        public Array EditIllnessDataList { get; set; }
    }
}
