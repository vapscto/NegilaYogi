using System;using System.Collections.Generic;using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;using System.Text;namespace DomainModel.Model.com.vapstech.VidyaBharathi{    [Table("VBSC_Master_SportsCCName_UOM")]    public class VBSC_Master_SportsCCName_UOMDMO    {        [Key]        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long VBSCMSCCUOM_Id { get; set; }
        public long VBSCMSCC_Id { get; set; }        public long VBCCMUOM_Id { get; set; }        public bool VBSCMSCCUOM_ActiveFlag { get; set; }        public DateTime? VBSCMSCCUOM_CreatedDate { get; set; }        public DateTime? VBSCMSCCUOM_UpdatedDate { get; set; }
        
    }}








