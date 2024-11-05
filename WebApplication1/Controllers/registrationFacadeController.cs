using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Services;
using PreadmissionDTOs;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Alumni;
using Microsoft.AspNetCore.Identity;
using DataAccessMsSqlServerProvider;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using AutoMapper;
using CommonLibrary;
using DomainModel.Model.com.vaps.Fee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class registrationFacadeController : Controller
    {
        public registration _reg;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly DomainModelMsSqlServerContext _db;
        public ProspectusContext _ProspectusContext;
        public FeeGroupContext _feecontext;
        public registrationFacadeController(registration regi, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> UserManager, DomainModelMsSqlServerContext db ,FeeGroupContext feecontext, ProspectusContext ProspectusContext)
        {
            _reg = regi;
            _signInManager = signInManager;
            _UserManager = UserManager;
            _db = db;
            _feecontext = feecontext;
            _ProspectusContext = ProspectusContext;
        }
      
        // GET api/values/5
        [HttpGet]
        public async Task<bool> Get(string username)
        {
            bool val = await _reg.getregdata(username);
            return val;
            //return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<regis> Post([FromBody] regis reg) //
        {
           // string returnMsg = "";
            try
            {

                ApplicationUser user = new ApplicationUser();
                ApplicationUser user1 = new ApplicationUser();
                ApplicationUser user3 = new ApplicationUser();

                if(reg.Special!= "Special" && reg.Email_id!="" && reg.Email_id != null)
                {
                    user = await _UserManager.FindByEmailAsync(reg.Email_id);
                }
                
                user1 = await _UserManager.FindByNameAsync(reg.username.Trim());
               
                if (user == null || !(await _UserManager.IsEmailConfirmedAsync(user)))
                {
                    if (user1 == null)
                    {
                        user = new ApplicationUser { UserName = reg.username.Trim(), Email = reg.Email_id };
                        // Added on 11-11-2016 to store Institute, entry date and ip address
                        //user.MI_Id = reg.MI_Id;
                        
                        user.Machine_Ip_Address = reg.Machine_Ip_Address;
                        // Added on 11-11-2016 to store Institute, entry date and ip address
                        user.UserImagePath = reg.profile_pic_path;
                        //15/11/2016
                        user.PhoneNumber = reg.Mobileno;
                        if(reg.Alumni == "Alumni")
                        {
                            user.RoleTypeFlag = "Alumni";
                        }
                        else if (reg.Special == "Careers")
                        {
                            user.RoleTypeFlag = "Candidate";
                        }
                        else
                        {
                            user.RoleTypeFlag = "OnlinePreadmissionUser";
                        }
                        
                        user.EmailConfirmed = true;
                        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                        user.Entry_Date = indianTime;
                        user.CreatedDate = indianTime;
                        user.UpdatedDate = indianTime;
                       
                        user.Name = reg.Name;
                        // user.UserImagePath = "D:\\IVRM 2.0 23.12\\corewebapi18072016\\src\\corewebapi18072016\\wwwroot\\images\\uploads\\Profile_Pics\\a_1c6e77cd - 1ba4 - 45eb - 959a - 19b80a5c0dfc.jpg";



                        //  var fetchval = _UserManager.CreateAsync(user, reg.password).Result.Errors.ToArray();
                        var result = await _UserManager.CreateAsync(user, reg.password);

                        //  string abc = fetchval[0].Code;
                        if (result.Succeeded)
                        {
                            reg.returnMsg = "created";
                            //Date:15-12-2016 Send Confirmation email.
                            reg.User_Id = user.Id;

                            //string confirmationToken = _UserManager.
                            //                       GenerateEmailConfirmationTokenAsync(user).Result;
                            //reg.token = confirmationToken;

                            //try
                            //{
                            //    //SendEmail(user.UserName, user.Email, reg.MI_Id, "RegistrationEmailTemplate");
                            //    string s = await sendSms("Af7660f900c9df85ac5dfae4b055d75bd", "HHSJCP", Convert.ToInt64(reg.Mobileno), reg.MI_Id, user.UserName, "RegistrationEmailTemplate");
                            //}
                            //catch (Exception e)
                            //{
                            //    Console.WriteLine(e.Message);
                            //}

                            // Student Roles
                            string studentRole = "";
                            if (reg.Alumni == "Alumni")
                            {
                                studentRole = "Alumni";
                            }
                            else if(reg.Special == "Careers")
                            {
                                studentRole = "Candidate";
                            }
                            else
                            {
                                studentRole = "OnlinePreadmissionUser";
                            }
                               
                            var id = _db.applicationRole.Single(d => d.Name == studentRole);
                            //

                            // Student Role Type
                            string studentRoleType = "";
                            if (reg.Alumni == "Alumni")
                            {
                                studentRoleType = "Alumni";
                            }
                            else if (reg.Special == "Careers")
                            {
                                studentRoleType = "Candidate";
                            }
                            else
                            {
                                studentRoleType = "OnlinePreadmissionUser";
                            }
                            
                            var id2 = _db.MasterRoleType.Single(d => d.IVRMRT_Role == studentRoleType);
                            //

                            // Save role
                            var role = new DataAccessMsSqlServerProvider.ApplicationUserRole { RoleId = Convert.ToInt32(id.Id), UserId = user.Id, RoleTypeId = Convert.ToInt64(id2.IVRMRT_Id) };
                            _db.appUserRole.Add(role);
                            _db.SaveChanges();

                            UserRoleWithInstituteDTO master = new UserRoleWithInstituteDTO();
                            master.Id = user.Id;
                            master.MI_Id = reg.MI_Id;
                            master.Activeflag = 1;

                           

                            UserRoleWithInstituteDMO mas = Mapper.Map<UserRoleWithInstituteDMO>(master);
                            _db.Add(mas);
                            _db.SaveChanges();

                            if (reg.Special == "Special") 
                            {
                                Preadmission_Special_Registration special = new Preadmission_Special_Registration();
                                special.ID = user.Id;
                                _db.Add(special);
                                _db.SaveChanges();
                            }


                            if (reg.Alumni == "Alumni")
                            {
                                var concessiontype = _db.Institution.Single(t => t.MI_Id == reg.MI_Id).MI_SchoolCollegeFlag;
                                if (concessiontype=="S")
                                {
                                    AlumniUserRegistrationDMO Alumni = new AlumniUserRegistrationDMO();

                                    Alumni.MI_Id = reg.MI_Id;
                                    Alumni.ALSREG_Photo = reg.profile_pic_path;
                                    Alumni.ALSREG_AdmissionNo = reg.admission;
                                    Alumni.ALSREG_ApprovedFlag = false;
                                    Alumni.ALMST_Id = 0;
                                    Alumni.ALSREG_MemberName = reg.Name;
                                    Alumni.ALSREG_EmailId = reg.Email_id;
                                    Alumni.ALSREG_MobileNo = Convert.ToInt64(reg.Mobileno);
                                    Alumni.ALSREG_AdmittedYear = Convert.ToInt64(reg.admittedyer);
                                    Alumni.ALSREG_LeftYear = Convert.ToInt64(reg.leftyear);
                                    Alumni.ALSREG_LeftClass = Convert.ToInt64(reg.leftclassid);
                                    Alumni.ALSREG_AdmittedClass = Convert.ToInt64(reg.classidadmitted);
                                    Alumni.ALSREG_Date = DateTime.Now;
                                    Alumni.CreatedDate = DateTime.Now;
                                    Alumni.UpdatedDate = DateTime.Now;
                                    Alumni.ALSREG_CreatedBy = user.Id;
                                    Alumni.ALSREG_UpdatedBy = user.Id;
                                    Alumni.ALSREG_ActiveFlg = true;
                                    _db.Add(Alumni);
                                    _db.SaveChanges();


                                    Alumni_User_LoginDMO alumniluser = new Alumni_User_LoginDMO();
                                    alumniluser.ALSREG_Id = Alumni.ALSREG_Id;
                                    alumniluser.IVRMUL_Id = user.Id;
                                    _db.Add(alumniluser);
                                    _db.SaveChanges();
                                }
                                else if (concessiontype == "C")
                                {
                                    CLGAlumniUserRegistrationDMO Alumni = new CLGAlumniUserRegistrationDMO();

                                    Alumni.MI_Id = reg.MI_Id;
                                    Alumni.ALCSREG_Photo = reg.profile_pic_path;
                                    Alumni.ALCSREG_AdmissionNo = reg.admission;
                                    Alumni.ALCSREG_ApprovedFlag = false;
                                    Alumni.ALCSREG_MemberName = reg.Name;
                                    Alumni.ALCSREG_EmailId = reg.Email_id;
                                    Alumni.ALCSREG_Id = 0;
                                    Alumni.ALCSREG_MobileNo = Convert.ToInt64(reg.Mobileno);
                                    Alumni.ALCSREG_AdmittedYear = Convert.ToInt64(reg.admittedyer);
                                    Alumni.ALCSREG_LeftYear = Convert.ToInt64(reg.leftyear);
                                    Alumni.ALCSREG_LeftCourse = Convert.ToInt64(reg.courseleft);
                                    Alumni.ALCSREG_AdmittedCourse = Convert.ToInt64(reg.courseadmittted);
                                    Alumni.ALCSREG_AdmisstedBranch = Convert.ToInt64(reg.branchadmittted);
                                    Alumni.ALCSREG_LeftBranch = Convert.ToInt64(reg.branchleft);
                                    Alumni.ALCSREG_AdmittedSemester = Convert.ToInt64(reg.semadmittted);
                                    Alumni.ALCSREG_LeftSemester = Convert.ToInt64(reg.semleft);
                                    Alumni.ALCSREG_Date = DateTime.Now;
                                    Alumni.CreatedDate = DateTime.Now;
                                    Alumni.UpdatedDate = DateTime.Now;
                                    Alumni.ALCSREG_CreatedBy = user.Id;
                                    Alumni.ALCSREG_UpdatedBy = user.Id;
                                    Alumni.ALCSREG_ActiveFlg = true;
                                    _db.Add(Alumni);
                                    _db.SaveChanges();


                                    CLGAlumni_User_LoginDMO alumniluser = new CLGAlumni_User_LoginDMO();
                                    alumniluser.ALCSREG_Id = Alumni.ALCSREG_Id;
                                    alumniluser.IVRMUL_Id = user.Id;
                                    _db.Add(alumniluser);
                                    _db.SaveChanges();
                                }
                               
                            }



                        }
                        //else if (fetchval.FirstOrDefault().Code.Equals("DuplicateUserName"))
                        //{
                        //    reg.returnMsg = fetchval.FirstOrDefault().Description.ToString();
                        //}

                        else
                        {
                            reg.returnMsg = result.Errors.FirstOrDefault().Description.ToString();
                            reg.returnMsg = "Passwords must have at least 6 characters,one Special,one lowercase and one uppercase character.";
                        }
                    }
                    else
                    {
                        reg.returnMsg = "Username Already exist .. !!";
                    }
                }
                else
                {
                    reg.returnMsg = "Email id Already exist .. !!";
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
            return reg;
            
        }
       
        [Route("paymentPart")]
        public async Task<regis> paymentPart([FromBody] regis enq)
        {


            ApplicationUser user = new ApplicationUser();
            ApplicationUser user1 = new ApplicationUser();
           
            user =await  _UserManager.FindByEmailAsync(enq.Email_id);
            user1 = await _UserManager.FindByNameAsync(enq.username);

            if (user == null || !(await _UserManager.IsEmailConfirmedAsync(user)))
            {
                if (user1 == null)
                {

                    Payment pay = new Payment(_db);
                    ProspectusDTO data = new ProspectusDTO();
                    List<Prospepaymentamount> paymentdetails = new List<Prospepaymentamount>();
                    PaymentDetails PaymentDetailsDto = new PaymentDetails();
                    //enq.ASMAY_Id = 7;
                    try
                    {
                        paymentdetails = _ProspectusContext.Prospepaymentamount.Where(t => t.IVRMOP_MIID == enq.MI_Id).ToList();

                        //var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(enq.MI_Id) && Convert.ToDateTime(t.ASMAY_PreAdm_F_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_PreAdm_T_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                        var Acdemic_preadmission = _db.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == enq.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                        enq.ASMAY_Id = Acdemic_preadmission;

                        ProspectusDTO ProspectusDTO = new ProspectusDTO();
                        var FeeAmountresult = (from a in _feecontext.feeYCC

                                               from c in _feecontext.feeYCCC
                                               from d in _feecontext.FeeAmountEntryDMO

                                               from g in _feecontext.FeeHeadDMO
                                               where (d.FMH_Id == g.FMH_Id && d.FMCC_Id == a.FMCC_Id && a.ASMAY_Id == d.ASMAY_Id && a.FYCC_Id == c.FYCC_Id && d.ASMAY_Id == Acdemic_preadmission && d.MI_Id == enq.MI_Id && g.FMH_Flag == "A")
                                               select new FeeAmountEntryDMO
                                               {
                                                   FMA_Id = d.FMA_Id,
                                                   FMA_Amount = d.FMA_Amount
                                               }
                    ).FirstOrDefault();






                        List<Master_Numbering> masnum = new List<Master_Numbering>();
                        masnum = _db.Master_Numbering.Where(t => t.MI_Id == enq.MI_Id && t.IMN_Flag == "Online").ToList();


                        Master_NumberingDTO cc = new Master_NumberingDTO();

                        cc.MI_Id = masnum[0].MI_Id;
                        cc.IMN_Id = masnum[0].IMN_Id;

                        cc.IMN_AutoManualFlag = masnum[0].IMN_AutoManualFlag;
                        cc.IMN_DuplicatesFlag = masnum[0].IMN_DuplicatesFlag;
                        cc.IMN_StartingNo = masnum[0].IMN_StartingNo;
                        cc.IMN_Flag = masnum[0].IMN_Flag;
                        cc.IMN_Id = masnum[0].IMN_Id;
                        cc.IMN_PrefixAcadYearCode = masnum[0].IMN_PrefixAcadYearCode;
                        cc.IMN_PrefixCalYearCode = masnum[0].IMN_PrefixCalYearCode;
                        cc.IMN_PrefixFinYearCode = masnum[0].IMN_PrefixFinYearCode;
                        cc.IMN_PrefixParticular = masnum[0].IMN_PrefixParticular;
                        cc.IMN_RestartNumFlag = masnum[0].IMN_RestartNumFlag;

                        cc.IMN_SuffixAcadYearCode = masnum[0].IMN_SuffixAcadYearCode;
                        cc.IMN_SuffixCalYearCode = masnum[0].IMN_SuffixCalYearCode;
                        cc.IMN_SuffixFinYearCode = masnum[0].IMN_SuffixFinYearCode;
                        cc.IMN_SuffixParticular = masnum[0].IMN_SuffixParticular;
                        cc.IMN_WidthNumeric = masnum[0].IMN_WidthNumeric;
                        cc.IMN_ZeroPrefixFlag = masnum[0].IMN_ZeroPrefixFlag;




                        enq.transnumbconfigurationsettingsss = cc;




                        if (cc.IMN_AutoManualFlag == "Auto")
                        {
                            GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                            cc.MI_Id = enq.MI_Id;
                            cc.ASMAY_Id = Acdemic_preadmission;
                            PaymentDetailsDto.trans_id = a.GenerateNumber(cc);
                        }

                        if (paymentdetails.Count != 0)
                        {


                            string fmaid = Convert.ToString(FeeAmountresult.FMA_Id);

                            PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().IVRMOP_MERCHANT_KEY;
                            // PaymentDetailsDto.amount = paymentdetails.FirstOrDefault().IVRMOP_PROS_AMOUNT;
                            PaymentDetailsDto.amount = Convert.ToDecimal(paymentdetails.FirstOrDefault().IVRMOP_PROS_AMOUNT);
                            //PaymentDetailsDto.trans_id = "200";
                            PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";
                            PaymentDetailsDto.productinfo = "login";
                            PaymentDetailsDto.firstname = enq.Name;
                            PaymentDetailsDto.email = enq.Email_id;
                            PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().IVRMOP_SALT;
                            PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().IVRMOP_BASE_URL;
                            PaymentDetailsDto.phone = Convert.ToInt64(enq.Mobileno);
                            PaymentDetailsDto.udf1 = "Rs.";
                            PaymentDetailsDto.udf2 = enq.username;
                            PaymentDetailsDto.udf3 = enq.MI_Id.ToString();
                            PaymentDetailsDto.udf4 = enq.password;
                            if (enq.profile_pic_path != null)
                            {
                                PaymentDetailsDto.udf5 = enq.profile_pic_path;
                            }
                            else
                            {
                                PaymentDetailsDto.udf5 = "";
                            }
                            PaymentDetailsDto.udf6 = fmaid;
                            PaymentDetailsDto.transaction_response_url = "http://localhost:57606/api/" + PaymentDetailsDto.productinfo + "/paymentresponse/";
                            PaymentDetailsDto.status = "success";
                            PaymentDetailsDto.service_provider = "payu_paisa";

                            // PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);

                            enq.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);

                            FeePaymentDetailsDMO feepaydet = new FeePaymentDetailsDMO();
                            feepaydet.MI_Id = enq.MI_Id;
                            feepaydet.ASMAY_ID = enq.ASMAY_Id;

                            feepaydet.FTCU_Id = 1;
                            feepaydet.FYP_Receipt_No = PaymentDetailsDto.trans_id;
                            feepaydet.FYP_Bank_Name = "";
                            feepaydet.FYP_Bank_Or_Cash = "O";
                            feepaydet.FYP_DD_Cheque_No = "";
                            feepaydet.FYP_DD_Cheque_Date = DateTime.Now;
                            feepaydet.FYP_Date = DateTime.Now;
                            feepaydet.FYP_Tot_Amount = PaymentDetailsDto.amount;
                            feepaydet.FYP_Tot_Waived_Amt = 0;
                            feepaydet.FYP_Tot_Fine_Amt = 0;
                            feepaydet.FYP_Tot_Concession_Amt = 0;
                            feepaydet.FYP_Remarks = "User Registration";
                            feepaydet.FYP_Chq_Bounce = "CL";
                            feepaydet.DOE = DateTime.Now;
                            feepaydet.CreatedDate = DateTime.Now;
                            feepaydet.UpdatedDate = DateTime.Now;
                            feepaydet.user_id = 0;
                            feepaydet.fyp_transaction_id = PaymentDetailsDto.trans_id;
                            feepaydet.FYP_OnlineChallanStatusFlag = "Payment Initiated";
                            feepaydet.FYP_PaymentReference_Id = "";

                            _feecontext.FeePaymentDetailsDMO.Add(feepaydet);
                            _feecontext.SaveChanges();

                            PaymentDetailsDto.paymentdetails = "True";

                        }
                        else
                        {
                            PaymentDetailsDto.paymentdetails = "false";
                        }
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                }
                else
                {
                    enq.returnMsg = "Username Already exist .. !!";
                }
            }
            else
            {
                enq.returnMsg = "Email id Already exist .. !!";
            }

            return enq;

        }
        public async Task<string> sendSms(string workingKey, string sender, long mobileNo, string message)
        {
            System.Net.HttpWebRequest request = System.Net.WebRequest.Create("http://trans.kapsystem.com/api/web2sms.php?&workingkey=" + workingKey + "&sender=" + sender + "&to=" + mobileNo + "&message=" + message) as HttpWebRequest;
            //optional
            System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
            Stream stream = response.GetResponseStream();
            return "success";
        }
      
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
        public async Task<string> sendSms(string workingKey, string sender, long mobileNo, long MiId, string username, string smsTemplate)
        {
            try
            {
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MiId && e.ISES_Template_Name == smsTemplate && e.ISES_SMSActiveFlag == true).ToList();
                var institutionName = _db.Institution.Where(i => i.MI_Id == MiId).ToList();

                //By Sripad Joshi on 24-11-2016
                string sms = template.FirstOrDefault().ISES_SMSMessage;
                string result = "";
                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                for (int i = 0; i < variables.Count; i++)
                {
                    if (variables[i].ToString() == "[NAME]")
                    {
                        result = sms.Replace("[NAME]", username);
                        sms = result;
                    }
                    if (variables[i].ToString() == "[TITLE]")
                    {
                        result = sms.Replace("[TITLE]", "");
                        sms = result;
                    }
                    if (variables[i].ToString() == "[ISSUE_DATE]")
                    {
                        result = sms.Replace("[ISSUE_DATE]", "");
                        sms = result;
                    }
                    if (variables[i].ToString() == "[RETURN_DATE]")
                    {
                        result = sms.Replace("[RETURN_DATE]", "");
                        sms = result;
                    }
                    if (variables[i].ToString() == "[AMOUNT]")
                    {
                        result = sms.Replace("[AMOUNT]", "");
                        sms = result;
                    }
                    if (variables[i].ToString() == "[RECEIPT_NO]")
                    {
                        result = sms.Replace("[RECEIPT_NO]", "");
                        sms = result;
                    }
                    if (variables[i].ToString() == "[ADMNO]")
                    {
                        result = sms.Replace("[ADMNO]", "");
                        sms = result;
                    }
                    if (variables[i].ToString() == "[INSTITUTION_NAME]")
                    {
                        result = sms.Replace("[INSTITUTION_NAME]", institutionName.FirstOrDefault().MI_Name);
                        sms = result;
                    }
                    if (variables[i].ToString() == "[STUDENT_NAME]")
                    {
                        result = sms.Replace("[STUDENT_NAME]", username);
                        sms = result;
                    }
                    if (variables[i].ToString() == "[STAFF_NAME]")
                    {
                        result = sms.Replace("[STAFF_NAME]", "");
                        sms = result;
                    }
                    if (variables[i].ToString() == "[TOTAL]")
                    {
                        result = sms.Replace("[TOTAL]", "");
                        sms = result;
                    }
                    if (variables[i].ToString() == "[FROM_DATE]")
                    {
                        result = sms.Replace("[FROM_DATE]", "");
                        sms = result;
                    }
                    if (variables[i].ToString() == "[TO_DATE]")
                    {
                        result = sms.Replace("[TO_DATE]", "");
                        sms = result;
                    }
                }
                sms = result;
                //End.

                System.Net.HttpWebRequest request = System.Net.WebRequest.Create("http://trans.kapsystem.com/api/web2sms.php?&workingkey=" + workingKey + "&sender=" + sender + "&to=" + mobileNo + "&message=" + sms) as HttpWebRequest;
                //optional
                System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                Stream stream = response.GetResponseStream();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "success";
        }

        protected void SendEmail(string username, string emailId, long MIId, string templateType,string confirmationLink)
        {
            var template = _db.smsEmailSetting.Where(e => e.MI_Id == MIId && e.ISES_Template_Name == templateType && e.ISES_MailActiveFlag == true).ToList();
            var institutionName = _db.Institution.Where(i => i.MI_Id == MIId).ToList();
            string body = this.PopulateBody(username, institutionName.FirstOrDefault().MI_Name, MIId, templateType,confirmationLink);
            this.SendHtmlFormattedEmail(emailId, template.FirstOrDefault().ISES_MailSubject, body);
        }

        private string PopulateBody(string userName, string institutionName, long miId, string EMTYP_Id,string link)
        {
            var template = _db.smsEmailSetting.Where(e => e.MI_Id == miId && e.ISES_Template_Name == EMTYP_Id && e.ISES_MailActiveFlag == true).ToList();
            //Sripad Joshi on 24-11-2016
            string emailMsg = template.FirstOrDefault().ISES_Mail_Message;
            string result = "";
            List<Match> variables = new List<Match>();
            foreach (Match match in Regex.Matches(emailMsg, @"\[.*?\]"))
            {
                variables.Add(match);
            }
            for (int i = 0; i < variables.Count; i++)
            {
                if (variables[i].ToString() == "[NAME]")
                {
                    result = emailMsg.Replace("[NAME]", userName);
                    emailMsg = result;
                }
                if (variables[i].ToString() == "[TITLE]")
                {
                    result = emailMsg.Replace("[TITLE]", "");
                    emailMsg = result;
                }
                if (variables[i].ToString() == "[ISSUE_DATE]")
                {
                    result = emailMsg.Replace("[ISSUE_DATE]", "");
                    emailMsg = result;
                }
                if (variables[i].ToString() == "[RETURN_DATE]")
                {
                    result = emailMsg.Replace("[RETURN_DATE]", "");
                    emailMsg = result;
                }
                if (variables[i].ToString() == "[AMOUNT]")
                {
                    result = emailMsg.Replace("[AMOUNT]", "");
                    emailMsg = result;
                }
                if (variables[i].ToString() == "[RECEIPT_NO]")
                {
                    result = emailMsg.Replace("[RECEIPT_NO]", "");
                    emailMsg = result;
                }
                if (variables[i].ToString() == "[ADMNO]")
                {
                    result = emailMsg.Replace("[ADMNO]", "");
                    emailMsg = result;
                }
                if (variables[i].ToString() == "[INSTITUTION_NAME]")
                {
                    result = emailMsg.Replace("[INSTITUTION_NAME]", institutionName);
                    emailMsg = result;
                }
                if (variables[i].ToString() == "[STUDENT_NAME]")
                {
                    result = emailMsg.Replace("[STUDENT_NAME]", userName);
                    emailMsg = result;
                }
                if (variables[i].ToString() == "[STAFF_NAME]")
                {
                    result = emailMsg.Replace("[STAFF_NAME]", "");
                    emailMsg = result;
                }
                if (variables[i].ToString() == "[TOTAL]")
                {
                    result = emailMsg.Replace("[TOTAL]", "");
                    emailMsg = result;
                }
                if (variables[i].ToString() == "[FROM_DATE]")
                {
                    result = emailMsg.Replace("[FROM_DATE]", "");
                    emailMsg = result;
                }
                if (variables[i].ToString() == "[TO_DATE]")
                {
                    result = emailMsg.Replace("[TO_DATE]", "");
                    emailMsg = result;
                }
            }
            emailMsg = result;
            //End.
            string str = template.FirstOrDefault().ISES_MailHTMLTemplate;
            string body = string.Empty;
            byte[] byteArray = Encoding.UTF8.GetBytes(str);
            System.IO.MemoryStream stream = new MemoryStream(byteArray);

            using (StreamReader reader = new StreamReader(stream))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{MailBody}", template.FirstOrDefault().ISES_MailBody);
            body = body.Replace("{UserName}", userName);
            //  body = body.Replace("{message}", emailMsg + " " + Environment.NewLine + link);
            body = body.Replace("{message}", emailMsg + " " + Environment.NewLine + "<a href=\"" + link + "\">Please  Confirm your email by clicking here:</a>");
            body = body.Replace("{mailFooter}", template.FirstOrDefault().ISES_MailFooter);



            //  body = body.Replace("{InstitutionName}", institutionName);
            return body;
        }

        private void SendHtmlFormattedEmail(string recepientEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Vapstech", "vapstech123@gmail.com"));
            message.To.Add(new MailboxAddress("", recepientEmail));
            message.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;
            message.Body = bodyBuilder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note:for from email address turn on lesssecureapps.
                // for that go to the following url.
                //  http://www.google.com/settings/security/lesssecureapps

                // Note: only needed if the SMTP server requires authentication

                client.Authenticate("vapstech123@gmail.com", "vaps@123");
                client.Send(message);
                client.Disconnect(true);
            }
        }
        [Route("sendmail")]
        public bool sendmail([FromBody]regis reg)
        {
            try
            {
                SendEmail(reg.username, reg.Email_id, reg.MI_Id, "RegistrationEmailTemplate", reg.confirmationLink);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        [Route("confirm")]
        public bool confirm([FromBody] regis c)
        {
            ApplicationUser user = _UserManager.FindByIdAsync(c.User_Id.ToString()).Result;
            IdentityResult result = _UserManager.ConfirmEmailAsync(user, c.token).Result;
            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [Route("alu_regdata")]
        public async Task<FileDescriptionDTO> alu_regdata([FromBody] FileDescriptionDTO reg) //
        {
           
            try
            {

                ApplicationUser user = new ApplicationUser();
                ApplicationUser user1 = new ApplicationUser();
                ApplicationUser user3 = new ApplicationUser();
                if (reg.Special != "Special")
                {
                    user = await _UserManager.FindByEmailAsync(reg.Email_id);
                }

                user1 = await _UserManager.FindByNameAsync(reg.Username.Trim());

                if (user == null || !(await _UserManager.IsEmailConfirmedAsync(user)))
                {
                    if (user1 == null)
                    {
                        user = new ApplicationUser { UserName = reg.Username.Trim(), Email = reg.Email_id };
                      
                        user.PhoneNumber = reg.Mobileno;
                        if (reg.Alumni == "Alumni")
                       

                        user.EmailConfirmed = true;
                        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                        user.CreatedDate = indianTime;
                        user.UpdatedDate = indianTime;

                        user.Name = reg.Name;
                        
                        var result = await _UserManager.CreateAsync(user, reg.Password);

                        if (result.Succeeded)
                        {
                            reg.returnMsg = "created";
                           
                            reg.User_Id = user.Id;

                            string studentRole = "";
                            if (reg.Alumni == "Alumni")
                            {
                                studentRole = "Alumni";
                            }
                            else
                            {
                                studentRole = "OnlinePreadmissionUser";
                            }

                            var id = _db.applicationRole.Single(d => d.Name == studentRole);
                          
                        
                            string studentRoleType = "";
                            if (reg.Alumni == "Alumni")
                            {
                                studentRoleType = "Alumni";
                            }
                            else
                            {
                                studentRoleType = "OnlinePreadmissionUser";
                            }

                            var id2 = _db.MasterRoleType.Single(d => d.IVRMRT_Role == studentRoleType);
                           
                            var role = new DataAccessMsSqlServerProvider.ApplicationUserRole { RoleId = Convert.ToInt32(id.Id), UserId = user.Id, RoleTypeId = Convert.ToInt64(id2.IVRMRT_Id) };
                            _db.appUserRole.Add(role);
                            _db.SaveChanges();

                            UserRoleWithInstituteDTO master = new UserRoleWithInstituteDTO();
                            master.Id = user.Id;
                            master.MI_Id = reg.MI_Id;
                            master.Activeflag = 1;
                            
                            UserRoleWithInstituteDMO mas = Mapper.Map<UserRoleWithInstituteDMO>(master);
                            _db.Add(mas);
                            _db.SaveChanges();

                            if (reg.Special == "Special")
                            {
                                Preadmission_Special_Registration special = new Preadmission_Special_Registration();
                                special.ID = user.Id;
                                _db.Add(special);
                                _db.SaveChanges();
                            }


                            if (reg.Alumni == "Alumni")
                            {
                                

                                var concessiontype = _db.Institution.Single(t => t.MI_Id == reg.MI_Id).MI_SchoolCollegeFlag;
                                if (concessiontype == "S")
                                {
                                    AlumniUserRegistrationDMO Alumni = new AlumniUserRegistrationDMO();

                                    Alumni.MI_Id = reg.MI_Id;
                                    Alumni.ALSREG_Photo = reg.profile_pic_path;
                                    Alumni.ALSREG_AdmissionNo = reg.admission;
                                    Alumni.ALSREG_ApprovedFlag = false;
                                    Alumni.ALSREG_MemberName = reg.Name;
                                    Alumni.ALSREG_EmailId = reg.Email_id;
                                    Alumni.ALSREG_MobileNo = Convert.ToInt64(reg.Mobileno);
                                    Alumni.ALSREG_AdmittedYear = Convert.ToInt64(reg.admittedyer);
                                    Alumni.ALSREG_LeftYear = Convert.ToInt64(reg.leftyear);
                                    Alumni.ALSREG_LeftClass = Convert.ToInt64(reg.leftclassid);
                                    Alumni.ALSREG_AdmittedClass = Convert.ToInt64(reg.classidadmitted);
                                    Alumni.ALSREG_Date = DateTime.Now;
                                    Alumni.CreatedDate = DateTime.Now;
                                    Alumni.UpdatedDate = DateTime.Now;
                                    Alumni.ALSREG_CreatedBy = user.Id;
                                    Alumni.ALSREG_UpdatedBy = user.Id;
                                    Alumni.ALSREG_ActiveFlg = true;
                                    _db.Add(Alumni);
                                    _db.SaveChanges();


                                    Alumni_User_LoginDMO alumniluser = new Alumni_User_LoginDMO();
                                    alumniluser.ALSREG_Id = Alumni.ALSREG_Id;
                                    alumniluser.IVRMUL_Id = user.Id;
                                    _db.Add(alumniluser);
                                    _db.SaveChanges();
                            

                                    FeePaymentDetailsDMO FD = new FeePaymentDetailsDMO();
                                    FD.MI_Id = reg.MI_Id;
                                    FD.ASMAY_ID = reg.ASMAY_Id;
                                    FD.FTCU_Id = 1;
                                    FD.FYP_Receipt_No = reg.ReceiptNo;
                                    FD.FYP_Bank_Name = "";
                                    FD.FYP_Bank_Or_Cash = "O";
                                    FD.FYP_DD_Cheque_No = "";
                                    FD.FYP_DD_Cheque_Date = DateTime.Now;
                                    FD.FYP_Date = DateTime.Now;
                                    FD.FYP_Tot_Amount = reg.Donation_Amount;
                                    FD.FYP_Tot_Waived_Amt = 0;
                                    FD.FYP_Tot_Fine_Amt = 0;
                                    FD.FYP_Tot_Concession_Amt = 0;
                                    FD.FYP_Remarks = "Alumni Registration Fee";
                                    FD.FYP_Chq_Bounce = "CL";
                                    FD.DOE = DateTime.Now;
                                    FD.CreatedDate = DateTime.Now;
                                    FD.UpdatedDate = DateTime.Now;
                                    FD.user_id = reg.Id;
                                    FD.fyp_transaction_id = reg.orderId;
                                    FD.FYP_PaymentReference_Id = "";
                                    _db.Add(FD);
                                    _db.SaveChanges();
                                    var FYP_Id = FD.FYP_Id;

                                    Fee_Y_Payment_AlumniDMO pa = new Fee_Y_Payment_AlumniDMO();
                                    pa.FYP_Id = FYP_Id;
                                    pa.ALSREG_Id = Alumni.ALSREG_Id;
                                    pa.FYPALUM_ActiveFlg = true;
                                    pa.FYPALUM_CreatedBy = user.Id;
                                    pa.FYPALUM_UpdatedBy = user.Id;
                                    pa.FYPALUM_CreatedDate = DateTime.Today;
                                    pa.FYPALUM_UpdatedDate = DateTime.Today;
                                    _db.Add(pa);
                                    _db.SaveChanges();
                                }
                                else if (concessiontype == "C")
                                {
                                    CLGAlumniUserRegistrationDMO Alumni = new CLGAlumniUserRegistrationDMO();

                                    Alumni.MI_Id = reg.MI_Id;
                                    Alumni.ALCSREG_Photo = reg.profile_pic_path;
                                    Alumni.ALCSREG_AdmissionNo = reg.admission;
                                    Alumni.ALCSREG_ApprovedFlag = false;
                                    Alumni.ALCSREG_MemberName = reg.Name;
                                    Alumni.ALCSREG_EmailId = reg.Email_id;
                                    Alumni.ALCSREG_MobileNo = Convert.ToInt64(reg.Mobileno);
                                    Alumni.ALCSREG_AdmittedYear = Convert.ToInt64(reg.admittedyer);
                                    Alumni.ALCSREG_LeftYear = Convert.ToInt64(reg.leftyear);
                                    Alumni.ALCSREG_LeftCourse = Convert.ToInt64(reg.courseleft);
                                    Alumni.ALCSREG_AdmittedCourse = Convert.ToInt64(reg.courseadmittted);
                                    Alumni.ALCSREG_AdmisstedBranch = Convert.ToInt64(reg.branchadmittted);
                                    Alumni.ALCSREG_LeftBranch = Convert.ToInt64(reg.branchleft);
                                    Alumni.ALCSREG_AdmittedSemester = Convert.ToInt64(reg.semadmittted);
                                    Alumni.ALCSREG_LeftSemester = Convert.ToInt64(reg.semleft);
                                    Alumni.ALCSREG_Date = DateTime.Now;
                                    Alumni.CreatedDate = DateTime.Now;
                                    Alumni.UpdatedDate = DateTime.Now;
                                    Alumni.ALCSREG_CreatedBy = user.Id;
                                    Alumni.ALCSREG_UpdatedBy = user.Id;
                                    Alumni.ALCSREG_ActiveFlg = true;
                                    _db.Add(Alumni);
                                    _db.SaveChanges();
                                  

                                    CLGAlumni_User_LoginDMO alumniluser = new CLGAlumni_User_LoginDMO();
                                    alumniluser.ALCSREG_Id = Alumni.ALCSREG_Id;
                                    alumniluser.IVRMUL_Id = user.Id;
                                    _db.Add(alumniluser);
                                    _db.SaveChanges();
                                }


                               
                            }



                        }
                        //else if (fetchval.FirstOrDefault().Code.Equals("DuplicateUserName"))
                        //{
                        //    reg.returnMsg = fetchval.FirstOrDefault().Description.ToString();
                        //}

                        else
                        {
                            reg.returnMsg = result.Errors.FirstOrDefault().Description.ToString();
                            reg.returnMsg = "Passwords must have at least 6 characters,one Special,one lowercase and one uppercase character.";
                        }
                    }
                    else
                    {
                        reg.returnMsg = "Username Already exist .. !!";
                    }
                }
                else
                {
                    reg.returnMsg = "Email id Already exist .. !!";
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return reg;

        }
    }

}


