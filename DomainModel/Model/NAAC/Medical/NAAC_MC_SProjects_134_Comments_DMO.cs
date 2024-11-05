using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_SProjects_134_Comments")]
    public class NAAC_MC_SProjects_134_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCSP134C_Id { get; set; }
        public string NCMCSP134C_Remarks { get; set; }
        public long NCMCSP134C_RemarksBy { get; set; }
        public string NCMCSP134C_StatusFlg { get; set; }
        public bool NCMCSP134C_ActiveFlag { get; set; }
        public long NCMCSP134C_CreatedBy { get; set; }
        public DateTime? NCMCSP134C_CreatedDate { get; set; }
        public long NCMCSP134C_UpdatedBy { get; set; }
        public DateTime? NCMCSP134C_UpdatedDate { get; set; }
        public long NCMCSP134_Id { get; set; }
    }
}
