using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_441_ICTFacilities")]
   public class Naac_MC_IctFacility441_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]



      public long NCMCCTTF441_Id { get; set; }
 public long MI_Id { get; set; }
        public long NCMCCTTF441_Year { get; set; }
        public long NCMCCTTF441_NoOfClassroomsSeminarHallsLCD { get; set; }
        public long NCMCCTTF441_NoOfClassroomsSeminarHallsLCDLan { get; set; }
        public long NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLan { get; set; }
        public long NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLanAuVi { get; set; }
        public long NCMCCTTF441_TotalNoOfClassSeminarHalls { get; set; }
        public long NCMCCTTF441_CreatedBy { get; set; }
        public long NCMCCTTF441_UpdatedBy { get; set; }
        public DateTime NCMCCTTF441_CreateDate { get; set; }
        public DateTime NCMCCTTF441_UpdatedDate { get; set; }
        public List<Naac_MC_IctFacility441_filesDMO> Naac_MC_IctFacility441_filesDMO { get; set; }
    }
}
