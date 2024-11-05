using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Master_Room_Amenities")]
    public class HR_Master_Room_AmenitiesDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public long HRMRAM_Id { get; set; }
        public long HRMR_Id { get; set; }
        public long HRMAM_Id { get; set; }
        public decimal HRMRAM_RentPerDay { get; set; }
        public long HRMRAM_NoOfHrs { get; set; }
        public decimal HRMRAM_RentPerHour { get; set; }
        public bool HRMRAM_ActiveFlag { get; set; }
        public DateTime HRMRAM_CreatedDate { get; set; }
        public DateTime HRMRAM_UpdatedDate { get; set; }
        public long HRMRAM_CreatedBy { get; set; }
        public long HRMRAM_UpdatedBy { get; set; }

    }
}
