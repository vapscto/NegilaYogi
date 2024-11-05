using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
   public class IVRTM_TrainingDTO
    {
        public long IVRMTMT_Id { get; set; }
        public long MI_Id { get; set; }
        public long Userid { get; set; }
        public long roleid { get; set; }
        public long HRME_Id { get; set; }
        public long IVRMTT_Id { get; set; }
        public long HRMETRTY_Id { get; set; }
        public long TrainerHRME_Id { get; set; }
        public DateTime? TrainingDate { get; set; }
        public DateTime? HREXTTRN_EndDate { get; set; }
        public string IVRMTT_TentetiveStartTime { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public long HRMEMNO_MobileNo { get; set; }
        public string IVRMTT_TentetiveEndTime { get; set; }
        public string IVRMTT_TrainingMode { get; set; }
        public string ClientURL { get; set; }
        public string HRMEM_EmailId { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public string IVRMTT_Remarks { get; set; }
        public string status { get; set; }
        public bool HREXTTRN_ActiveFlag { get; set; }
        public DateTime HREXTTRN_CreatedDate { get; set; }
        public DateTime HREXTTRN_UpdatedDate { get; set; }
        public long HREXTTRN_CreatedBy { get; set; }
        public long HREXTTRN_UpdatedBy { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public string institutionname { get; set; }
        public Array getloaddetails { get; set; }
        public Array trainingcentername { get; set; }
        public Array gettrainer { get; set; }
        public Array trainingdetails { get; set; }
        public Array participates_Employee_list { get; set; }
        public Array employeename { get; set; }
        public Array editdata { get; set; }
        public Array emp_deatils { get; set; }
        public Array aprovedlist { get; set; }
        public Array griddata { get; set; }        
        public string connectionstring { get; set; }
        public string IVRMTT_EmployeeName { get; set; }
        public string IVRMTT_EmployeeEmail { get; set; }
        public string IVRMTT_EmployeePhone { get; set; }
        public string HRMETRTY_ExternalTrainingType { get; set; }
        public string HREXTTRN_TrainingTopic { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }


        public long IVRMTMT_TrainerId { get; set; }
        public string IVRMTMT_TrainerName { get; set; }
        public string IVRMTMT_TrainerEmail { get; set; }
        public string IVRMTT_TrainerPhone { get; set; }
        public string IVRMTMT_Status { get; set; }

        
              public Trainerfeedback[] Trainerfeedback1 { get; set; }
        public emplyee1[] emplyee { get; set; }
        public bool vapstraining { get; set; }
        public class emplyee1
        {
            public long HRME_Id { get; set; }
        }
        public class Trainerfeedback
        {
            public long IVRMTMT_Id { get; set; }
            public string IVRMTMT_Feedback { get; set; }
        }

        public filedto[] FilePath_Array { get; set; }
    }

}

