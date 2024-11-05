using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VidyaBharathi
{
    [Table("VBSC_Master_SportsCCGroupName")]
    public class VBSC_Master_SportsCCGroupNameDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long VBSCMSCCG_Id { get; set; }
        public string VBSCMSCCG_SportsCCGroupName { get; set; }
        public string VBSCMSCCG_SportsCCGroupDesc { get; set; }
        public string VBSCMSCCG_SCCFlag { get; set; }
        public bool VBSCMSCCG_ActiveFlag { get; set; }
        public DateTime? VBSCMSCCG_CreatedDate { get; set; }
        public DateTime? VBSCMSCCG_UpdatedDate { get; set; }
        public long MT_Id { get; set; }


    }
}
