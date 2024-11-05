using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class HR_Training_Create_DTO
    {
        public string instlogo { get; set; }
        public long HRTCR_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRTCR_PrgogramName { get; set; }
    
        public long HRMD_Id { get; set; }
        public string HRTCR_ProgramDesc { get; set; }
        public bool HRTCR_CostFeeFlg { get; set; }
        public decimal HRTCR_Cost { get; set; }
        public long HRMB_Id { get; set; }
        public long HRMF_Id { get; set; }
        public long HRMR_Id { get; set; }
        public bool HRTCR_InternalORExternalFlg { get; set; }
        public bool HRTCR_ActiveFlag { get; set; }
        public long HRTCR_CreatedBy { get; set; }
        public long HRTCR_UpdatedBy { get; set; }
        public DateTime? HRTCR_StartDate { get; set; }
        public DateTime? HRTCR_EndDate { get; set; }
        public long HRTCR_StatusFlg { get; set; }
        //--------------------------------------------------
        public long userId { get; set; }
        public string Role_flag { get; set; }
        public long IVRMRT_Id { get; set; }
        public string HRMB_BuildingName { get; set; }
        public int Status { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_AppDownloadedDeviceId { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public long HRTCRD_Id { get; set; }
        public long? HRME_MobileNo { get; set; }
        public long n_id { get; set; }
        public bool selectiontype { get; set; }
        public string returnvalue { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMF_FloorName { get; set; }
        public string HRMR_RoomName { get; set; }
        public DateTime HRTCRINTTR_StartDate { get; set; }
        public string HRTCRD_StartTime { get; set; }
        public string HRTCRD_EndTime { get; set; }
        public string HRTCRD_Content { get; set; }
        public long HRMET_Id { get; set; }
        public string HRME_EmployeeName { get; set; }
        public long HRTCT_Id { get; set; }
        public long EMP_CODE { get; set; }
        public long sid { get; set; }
        public int checkadmin { get; set; }
        public long ALL { get; set; }
        public long OPEN { get; set; }
        public long RUNNING { get; set; }
        public long COMPLETE { get; set; }
        public DateTime START_DATE { get; set; }
        public DateTime END_DATE { get; set; }
        public string sound { get; set; }
        public string employeename { get; set; }
        public string department { get; set; }
        public string desgination { get; set; }
        public string emailid { get; set; }
        public string gradename { get; set; }
        public string message { get; set; }
        public bool CHECK_Notification { get; set; }
        public bool Notification { get; set; }
        //=========================
        public Array program_dd_list { get; set; }
        public Array program_dd_list_one { get; set; }
        public Array roletype { get; set; }
        public Array configflag { get; set; }

        public Array trinee_list { get; set; }
        public Array deptlist { get; set; }
        public Array buillist { get; set; }
        public Array training_details_list { get; set; }
        public Array induction_training_report { get; set; }
        public Array training_details_Check { get; set; }
        public Array training_creation_list { get; set; }
        public Array Training_create_Details { get; set; }
        public Array Training_create_Trainee_list { get; set; }
        public Array Training_create_Details_list { get; set; }
        public Array participates_Employee_list { get; set; }
        public Array evaluation_participants_list { get; set; }
        public Array evaluation_trainer_list { get; set; }
        public Array print_trainer_list { get; set; }
        public Array topiclist { get; set; }

        public Array tname_list { get; set; }
        public Array induction_view_list { get; set; }
        public Array induction_view_list_details { get; set; }
        public Array Training_Details_proc { get; set; }
        public Array getemployeedetails { get; set; }
        public Array deviceArray { get; set; }
        public miid_list1[] miid_list { get; set; }




        //------------------------------------------------
        public emplyee1[] emplyee { get; set; }
        public create_training_details[] trainingdetails { get; set; }
        //------------------------------------------
        public class emplyee1
        {
            public long HRME_Id { get; set; }
        }
         public class miid_list1
        {
            public long MI_Id { get; set; }
        }

        public class create_training_details
        {
            public long HRTCRD_Id { get; set; }
            public long HRTCR_Id { get; set; }
            public long HRME_Id { get; set; }
            public string HRME_EmployeeFirstName { get; set; }
            public string HRTCRD_Content { get; set; }
            public string HRTCRD_StartTime { get; set; }
            public string HRTCRD_EndTime { get; set; }

            public DateTime? HRTCRINTTR_StartDate { get; set; }
            public long HRTCRD_Rating { get; set; }
            public string HRTCRD_TrainerRemarks { get; set; }
            public long HRMET_Id { get; set; }
            public long HRMTT_Id { get; set; }
        }

    }
}
