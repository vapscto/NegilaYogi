using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Library
{
  
    [Table("LIB_Master_Book_Files", Schema = "LIB")]
    public class Master_Book_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public  long LMBFILE_Id { get; set; }
        public long LMB_Id { get; set; }

        public string LMBFILE_FileName { get; set; }
        public string LMBFILE_FilePath { get; set; }

        public  bool LMBFILE_ActiveFlg { get; set; }
        public long LMBFILE_CreatedBy { get; set; }
        public long LMBFILE_UpdatedBy { get; set; }
        public DateTime? LMBFILE_CreatedDate { get; set; }
        public DateTime? LMBFILE_UpdatedDate { get; set; }
    }
}
