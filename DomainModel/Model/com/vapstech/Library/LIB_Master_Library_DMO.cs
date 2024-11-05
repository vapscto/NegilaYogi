using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_Library", Schema = "LIB")]
    public class LIB_Master_Library_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long LMAL_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMAL_LibraryName { get; set; }
        public bool LMAL_ActiveFlag { get; set; }



        public List<LIB_User_Library_DMO> LIB_User_Library_DMO { get; set; }
        public List<LIB_Library_Class_DMO> LIB_Library_Class_DMO { get; set; }
        public List<LIB_Master_NonBook_Library_DMO> LIB_Master_NonBook_Library_DMO { get; set; }

    }
}
