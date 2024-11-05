using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_HSU_324_CampusStartUps")]
    public  class HSU_334_CampusStartUpsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCHSU324CS_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCHSU324CS_Year { get; set; }
        public string NCHSU324CS_StartUpName { get; set; }
        public string NCHSU324CS_NatureOfStartUp { get; set; }
        public string NCHSU324CS_Comments { get; set; }
        public string NCHSU324CS_Contactinfo { get; set; }
        public bool NCHSU324CS_ActiveFlag { get; set; }
        public long NCHSU324CS_CreatedBy { get; set; }
        public long NCHSU324CS_UpdatedBy { get; set; }
        public DateTime? NCHSU324CS_CreatedDate { get; set; }
        public DateTime? NCHSU324CS_UpdatedDate { get; set; }
        public List<HSU_334_CampusStartUps_FilesDMO> HSU_334_CampusStartUps_FilesDMO { get; set; }
    }
}
