using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_ELibraryLinks", Schema = "LIB")]
    public class LIB_ELibraryLinksDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long LELS_Id { get; set; }
        public string  LELS_Name { get; set; }
        public string LELS_BookType { get; set; }
        public string LELS_Genre { get; set; }
        public string LELS_PriceRange { get; set; }
        public string LELS_FilePath { get; set; }
        public string LELS_Url { get; set; }
        public bool LELS_ActiveFlag { get; set; }


    }
}
