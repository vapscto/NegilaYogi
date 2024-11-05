using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Adm_M_PreAdm_Online_Temporary")]
    public class userDetails : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AMPOT_Id { get; set; }
        //public int APD_Id { get; set; }
        //public string AMP_Reg_No { get; set; }
        //public string AMP_Adm_No { get; set; }
        //public DateTime AMP_Doa { get; set; }
        public string AMP_Name { get; set; }
        //public int AMCL_Id { get; set; }
        //public DateTime AMP_Dob { get; set; }
        //public string AMP_Dob_Words { get; set; }
        public int AMP_Age { get; set; }
        public string AMP_Sex { get; set; }
        //public string AMP_B_Village { get; set; }
        //public string AMP_B_Town { get; set; }
        //public string AMP_B_District { get; set; }
        //public string AMP_B_State { get; set; }
        //public string AMP_Religion { get; set; }
        //public string AMCA_Id { get; set; }
        //public string AMP_Nationality { get; set; }
        //public string AMP_Perm_Address { get; set; }
        //public string AMP_Perm_Phone { get; set; }
        //public string AMP_Pres_Address { get; set; }
        //public string AMP_Pres_Phone { get; set; }
        //public string AMP_Father_Name { get; set; }
        //public string AMP_Father_Edu { get; set; }
        //public string AMP_Father_Occupation { get; set; }
        //public string AMP_Mother_Name { get; set; }
        //public string AMP_Mother_Edu { get; set; }
        //public string AMP_Mother_Occupation { get; set; }
        //public string AMP_Father_Living { get; set; }
        //public string AMP_Mother_Living { get; set; }
        //public string AMP_Mother_Income { get; set; }
        //public string AMP_Total_Income { get; set; }
        //public string AMP_Tc_Produced { get; set; }
        //public string AMP_Tc_Date { get; set; }
        //public string AMP_Tc_No { get; set; }
        //public string AMP_Stud_Photo { get; set; }
        //public string AMP_Entry_Date { get; set; }
        //public string AMP_Leaving_Reason { get; set; }
        public string AMP_Email_Id { get; set; }
        public string AMP_Language { get; set; }
        public string AMP_Pres_Address_Door { get; set; }
        public string AMP_Pres_Address_village { get; set; }
        public string AMP_Pres_Address_Town { get; set; }
        public string AMP_Pres_Address_District { get; set; }
        public string AMP_Pres_Address_State { get; set; }
        public string AMP_Pres_Address_pin { get; set; }
        public string AMP_Mobile { get; set; }

    }
}
