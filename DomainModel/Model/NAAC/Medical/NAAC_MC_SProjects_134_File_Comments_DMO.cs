using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_SProjects_134_File_Comments")]
    public class NAAC_MC_SProjects_134_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCSP134FC_Id { get; set; }
        public string NCMCSP134FC_Remarks { get; set; }
        public long NCMCSP134FC_RemarksBy { get; set; }
        public bool NCMCSP134FC_ActiveFlag { get; set; }
        public long? NCMCSP134FC_CreatedBy { get; set; }
        public DateTime? NCMCSP134FC_CreatedDate { get; set; }
        public long? NCMCSP134FC_UpdatedBy { get; set; }
        public DateTime? NCMCSP134FC_UpdatedDate { get; set; }
        public string NCMCSP134FC_StatusFlg { get; set; }
        public long NCMCSP134F_Id { get; set; }
    }
}
