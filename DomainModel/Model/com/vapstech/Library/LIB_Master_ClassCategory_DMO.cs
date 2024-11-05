using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_ClassCategory", Schema = "LIB")]
    public class LIB_Master_ClassCategory_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long LMCC_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMCC_CategoryName { get; set; }
        public bool LMCC_ActiveFlag { get; set; }


        public List<LIB_User_ClassCategory_DMO> LIB_User_ClassCategory_DMO { get; set; }
    }
}
