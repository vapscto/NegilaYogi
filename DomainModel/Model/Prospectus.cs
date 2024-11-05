using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_School_Prospectus")]
    public class Prospectus : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PASP_Id { get; set; }

        public string IVRMMCT_Id { get; set; }
        [ForeignKey("IVRMMC_Id")]
        public long IVRMMC_Id { get; set; }
        [ForeignKey("IVRMMS_Id")]
        public long IVRMMS_Id { get; set; }
        [ForeignKey("ASMAY_Id")]
        public long ASMAY_Id { get; set; }
        [ForeignKey("ASMCL_Id")]
        public long ASMCL_Id { get; set; }
        [ForeignKey("PAMR_Id")]
        public long PAMR_Id { get; set; }
        [ForeignKey("PAMS_Id")]
        public long PAMS_Id { get; set; }
        public string PASP_First_Name { get; set; }
        public string PASP_Middle_Name { get; set; }
        public string PASP_Last_Name { get; set; }
        public string PASP_Pincode { get; set; }
        public DateTime? PASP_Date { get; set; }
        public long PASP_MobileNo { get; set; }
        public long PASP_PhoneNo { get; set; }
        public string PASP_EmailId { get; set; }
        public string PASP_HouseNo { get; set; }
        public string PASP_Street { get; set; }
        public string PASP_Area { get; set; }
        public string PASP_Enquiry { get; set; }
        public long MI_ID { get; set; }

        public string PASP_ProspectusNo { get; set; }
        public long id { get; set; }
    }
}
