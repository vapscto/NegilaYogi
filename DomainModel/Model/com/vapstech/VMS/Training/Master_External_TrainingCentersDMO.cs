using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Master_External_TrainingCenters")]
    public class Master_External_TrainingCentersDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMETRCEN_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMETRCEN_TrainingCenterName { get; set; }
        public string HRMETRCEN_ContactPersonName { get; set; }
        public string HRMETRCEN_ContactNo { get; set; }
        public string HRMETRCEN_ContactEmailId { get; set; }
        public string HRMETRCEN_CenterAddress { get; set; }
        public bool HRMETRCEN_ActiveFlag { get; set; }
        public DateTime HRMETRCEN_CreatedDate { get; set; }
        public DateTime HRMETRCEN_UpdatedDate { get; set; }
        public long HRMETRCEN_CreatedBy { get; set; }
        public long HRMETRCEN_UpdatedBy { get; set; }


    }
}
