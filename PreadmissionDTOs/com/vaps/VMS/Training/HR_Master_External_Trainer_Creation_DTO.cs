using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class HR_Master_External_Trainer_Creation_DTO
    {
        public long HRMETR_Id { get; set; }
        public string HRMETR_Name { get; set; }
        public long MI_Id { get; set; }
        public long HRMEQ_Id { get; set; }
        public bool HRMETR_ParttimeORFullTimeFlg { get; set; }
        public string HRMETR_Image_Name { get; set; }
        public string HRMETR_ImagePath { get; set; }
        public long IVRMMG_Id { get; set; }
        public DateTime HRMETR_DOB { get; set; }
        public string HRMETR_Address { get; set; }
        public string HRMETR_City { get; set; }
        public long HRMETR_Pincode { get; set; }
        public string HRMETR_EmailId { get; set; }
        public string HRMETR_ContactNo { get; set; }
        public decimal HRMETR_DomainExp { get; set; }
        public decimal HRMETR_TrainingExp { get; set; }
        public string HRMETR_Skills { get; set; }
        public string HRMETR_Remarks { get; set; }
        public bool HRMETR_ActiveFlag { get; set; }
        public long HRMETR_CreatedBy { get; set; }
        public long HRMETR_UpdatedBy { get; set; }
        public long userId { get; set; }
        public string HRME_QualificationName { get; set; }
        public string IVRMMG_GenderName { get; set; }
        //===================
        public long? HRME_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        //=====================
        public Array training_list { get; set; }
        public Array qualification_list { get; set; }
        public string returnvalue { get; set; }
        public Array gender_list { get; set; }
        public bool returnval { get; set; }
        public Array trinee_list { get; set; }
        public Array train_list { get; set; }
        public notice[] notice1 { get; set; }
        public class notice
        {
            public string imagepath { get; set; }
        }
    }
}
