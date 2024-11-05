using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VidyaBharathi
{
    [Table("VBSC_Events_Category")]
    public class VBSC_Events_CategoryDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long VBSCECT_Id { get; set; }
        public long VBSCME_Id { get; set; }
        public long VBSCMCC_Id { get; set; }
        public long VBSCMSCC_Id { get; set; }
        public bool VBSCECT_GroupActivityFlg { get; set; }
        public long VBSCECT_MaxNoOfGroup { get; set; }
        public long VBSCECT_MaxNoOfStudents { get; set; }
        public String VBSCECT_Remarks { get; set; }
        public bool VBSCECT_ActiveFlag { get; set; }
        public DateTime? VBSCECT_CreatedDate { get; set; }
        public DateTime? VBSCECT_UpdatedDate { get; set; }
        public long VBSCECT_CreatedBy { get; set; }
        public long VBSCECT_UpdatedBy { get; set; }

    }
}
