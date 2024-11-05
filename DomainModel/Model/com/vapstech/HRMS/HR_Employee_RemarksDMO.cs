using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Employee_Remarks")]
    public class HR_Employee_RemarksDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HREREM_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HREREM_Subject { get; set; }
        public string HREREM_Remarks { get; set; }
        public DateTime? HREREM_Date { get; set; }
        public string HREREM_FileName { get; set; }
        public string HREREM_FilePath { get; set; }
        public long HREREM_RemarksBy { get; set; }
        public bool? HREREM_ActiveFlg { get; set; }
        public DateTime? HREREM_CreatedDate { get; set; }
        public long HREREM_CreatedBy { get; set; }
        public DateTime? HREREM_UpdatedDate { get; set; }
        public long HREREM_UpdatedBy { get; set; }
    }
}
