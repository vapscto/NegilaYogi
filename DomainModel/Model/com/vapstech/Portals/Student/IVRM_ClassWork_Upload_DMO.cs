using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DomainModel.Model.com.vapstech.Portals.Employee;

namespace DomainModel.Model.com.vapstech.Portals.Student
{
    [Table("IVRM_ClassWork_Upload")]
    public class IVRM_ClassWork_Upload_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ICWUPL_Id { get; set; }
        public long ICW_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime? ICWUPL_Date { get; set; }
        public string ICWUPL_Details { get; set; }
        public string ICWUPL_FileName { get; set; }
        public string ICWUPL_StaffUplaod { get; set; }
        public string ICWUPL_StaffRemarks { get; set; }
        public string ICWUPL_Attachment { get; set; }
        public decimal? ICWUPL_Marks { get; set; }
        public bool ICWUPL_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool? ICWUPL_ViewedFlg { get; set; }
        public DateTime? ICWUPL_ViewedDateTime { get; set; }
        public List<IVRM_ClassWork_Upload_Attatchment_DMO> IVRM_ClassWork_Upload_Attatchment_DMO { get; set; }
    }
}
