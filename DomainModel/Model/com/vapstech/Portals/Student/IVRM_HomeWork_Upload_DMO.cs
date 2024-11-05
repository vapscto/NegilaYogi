using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DomainModel.Model.com.vapstech.Portals.Employee;

namespace DomainModel.Model.com.vapstech.Portals.Student
{
    [Table("IVRM_HomeWork_Upload")]

    public class IVRM_HomeWork_Upload_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IHWUPL_Id { get; set; }
        public long IHW_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime? IHWUPL_Date { get; set; }
        public string IHWUPL_Details { get; set; }
        public string IHWUPL_FileName { get; set; }
        public string IHWUPL_Attachment { get; set; }
        public decimal? IHWUPL_Marks { get; set; }
        public string IHWUPL_StaffRemarks { get; set; }
        public string IHWUPL_StaffUpload { get; set; }
        public bool IHWUPL_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool IHWUPL_ViewedFlg { get; set; }
        public DateTime? IHWUPL_ViewedDateTime { get; set; }
        public List<IVRM_HomeWork_Upload_Attatchment_DMO> IVRM_HomeWork_Upload_Attatchment_DMO { get; set; }
    }
}
