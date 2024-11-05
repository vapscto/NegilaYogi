using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Alumni
{
   public  class Alumni_School_Interactions_DTO
    {
        public List<Alumni_School_Interactions_DTO> devicelist12 { get; set; }
        public long ALSMINT_Id { get; set; }
        public long ALSTINT_Id { get; set; }
        public long MI_Id { get; set; }
        public long ALMST_Id { get; set; }
        public long HRME_Id { get; set; }
        public long? ALMST_MobileNo { get; set; }
        public long? HRME_MobileNo { get; set; }
        public long userhrmE_Id { get; set; }
        public string message { get; set; }
        public string studentName { get; set; }
        public string employeeName { get; set; }
        public string roleflg { get; set; }
        public string HRME_AppDownloadedDeviceId { get; set; }
        public string ALMST_AppDownloadedDeviceId { get; set; }
        public string AppDownloadedDeviceId { get; set; }
        public string trans_id { get; set; }
        public string ALSTINT_ComposedByFlg { get; set; }
        public string ALSTINT_ToFlg { get; set; }
        public long ALSTINT_ComposedById { get; set; }
        public bool returnval { get; set; }
        public string ALSTINT_Interaction { get; set; }
        public string ALSMINT_InteractionId { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public int ALSTINT_InteractionOrder { get; set; }
        public long ALSTINT_ToId { get; set; }
        public long useralmst { get; set; }
        public Array aludetails { get; set; }
        public Array getdetails { get; set; }
        public Array getinboxmsg { get; set; }
        public Array get_msgcreator { get; set; }
        public Array viewmessage { get; set; }
        public long IVRMRT_Id { get; set; }
        public string Role_flag { get; set; }
        public string userflag { get; set; }
        public string ALSMINT_ComposedByFlg { get; set; }
        public string ALSMINT_GroupOrIndFlg { get; set; }
        public long ALSMINT_ComposedById { get; set; }
        public string ALSMINT_Subject { get; set; }
        public DateTime? ALSMINT_DateTime { get; set; }
        public string ALSMINT_Interaction { get; set; }
        public string ALSMINT_Attachment { get; set; }
        public bool ALSMINT_ActiveFlag { get; set; }
        public DateTime? ALSMINT_CreatedDate { get; set; }
        public DateTime? ALSMINT_UpdatedDate { get; set; }
        public long ALSMINT_CreatedBy { get; set; }
        public long ALSMINT_UpdatedBy { get; set; }
        public string composeedto { get; set; }
        public string rolename { get; set; }
        public Array roletype { get; set; }
        public Array deviceids { get; set; }
        public Array deviceidGrp { get; set; }
        public Array typelistrole { get; set; }
        public string[] images_paths { get; set; }
        public arrayalumni1[] arrayalumni { get; set; }
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public class arrayalumni1
        {
            public long ALMST_Id { get; set; }
        }

    }
}
