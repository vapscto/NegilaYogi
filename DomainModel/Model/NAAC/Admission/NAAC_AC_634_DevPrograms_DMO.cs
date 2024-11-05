using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_634_DevPrograms")]
    public class NAAC_AC_634_DevPrograms_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC634DEVPRG_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC634DEVPRG_Year { get; set; }
        public long NCAC634DEVPRG_NoOfTeachersAttnd { get; set; }
        public DateTime NCAC634DEVPRG_FromDate { get; set; }
        public DateTime NCAC634DEVPRG_ToDate { get; set; }
        public string NCAC634DEVPRG_PDProgTitle { get; set; }
        public bool NCAC634DEVPRG_ActiveFlg { get; set; }
        public long NCAC634DEVPRG_CreatedBy { get; set; }
        public long NCAC634DEVPRG_UpdatedBy { get; set; }
        public DateTime NCAC634DEVPRG_CreatedDate { get; set; }
        public DateTime NCAC634DEVPRG_UpdatedDate { get; set; }
        public string NCAC634DEVPRG_NameOfTeachers { get; set; }
        public string NCAC634DEVPRG_StatusFlg { get; set; }
        public bool? NCAC634DEVPRG_ApprovedFlg { get; set; }
        public string NCAC634DEVPRG_Remarks { get; set; }

        public List<NAAC_AC_634_DevPrograms_files_DMO> NAAC_AC_634_DevPrograms_files_DMO { get; set; }
    }
}
