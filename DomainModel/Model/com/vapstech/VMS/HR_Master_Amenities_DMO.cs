using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS
{
    [Table("HR_Master_Amenities")]
    public class HR_Master_Amenities_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMAM_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMAM_AmenitiesName { get; set; }
        public string HRMAM_AmenitiesDes { get; set; }
        public bool? HRMAM_PriceApplFlg { get; set; }
        public decimal? HRMAM_Price { get; set; }
        public bool? HRMAM_ActiveFlag { get; set; }
        public DateTime? HRMAM_CreatedDate { get; set; }
        public DateTime? HRMAM_UpdatedDate { get; set; }
        public long HRMAM_CreatedBy { get; set; }
        public long? HRMAM_UpdatedBy { get; set; }
    }
}
