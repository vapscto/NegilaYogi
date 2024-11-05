using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PreadmissionDTOs
{
    public class FileDescriptionDTO : CommonParamDTO
    {
        public int Id { get; set; }
        public long MI_Id { get; set; }
        public long User_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string returnMsg { get; set; }
        public string folder { get; set; }
        public string Description { get; set; }
        public string admission { get; set; }
        public string profile_pic_path { get; set; }
        public string Name { get; set; }
        public Array docarray { get; set; }
        public ICollection<IFormFile> File { get; set; }
        public ICollection<IFormFile> Logo { get; set; }
        public string Email_id { get; set; }
        public DateTime Entry_Date { get; set; }
        public string Mobileno { get; set; }
        public string Machine_Ip_Address { get; set; }
        public string token { get; set; }
        public string confirmationLink { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Alumni { get; set; }
        public string ReceiptNo { get; set; }
        public string orderId { get; set; }
        public string paygtw { get; set; }
        public long? admittedyer { get; set; }
        public long? leftyear { get; set; }
        public long? classidadmitted { get; set; }
        public long? leftclassid { get; set; }
        public string OTPSTATUS { get; set; }
        public string Otpmobl { get; set; }
        public string Otpemail { get; set; }
        public string Special { get; set; }
        public string onlinepreadminfilepath { get; set; }
        public List<Master_NumberingDTO> transnumbconfigurationsettingsss { get; set; }
        public Array PaymentDetailsList { get; set; }
        public string Base64string { get; set; }
        public long? courseadmittted { get; set; }
        public long? courseleft { get; set; }
        public long? branchadmittted { get; set; }
        public long? branchleft { get; set; }
        public long? semadmittted { get; set; }
        public long? semleft { get; set; }
        public long? Intid { get; set; }
        public int vr_id { get; set; }       
        public long FPGD_Id { get; set; }
        public long MI_Idnew { get; set; }
        public string IMPG_PGFlag { get; set; }
        public string FPGD_Image { get; set; }
        public string FPGD_PGName { get; set; }
        public string ALUReg { get; set; }
        public Array fillpaymentgateway { get; set; }
        public Array paymentgateway { get; set; }
        public Array institution { get; set; }
        public Array gencon { get; set; }
        public string AlumniName { get; set; }
        public decimal Donation_Amount { get; set; }
        public long Mobile { get; set; }   
        public string returnval { get; set; }
        public decimal FMA_Amount { get; set; }
        public Array alumnifee { get; set; }
        public MergeFilesDTO[] MergeFilesDTO { get; set; }
        public string StudentName { get; set; }
        public string returnpath { get; set; }
        public string Foldername { get; set; }
        public string filepath { get; set; }
        public long AMST_Id { get; set; }
        public long LPMOEEX_Id { get; set; }
        public long LPSTUEX_Id { get; set; }
    }

    public class MergeFilesDTO
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int? FileOrder { get; set; }
        public string FileType { get; set; }       
    }
}
