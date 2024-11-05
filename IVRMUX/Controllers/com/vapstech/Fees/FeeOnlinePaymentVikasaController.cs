using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using Microsoft.Extensions.Options;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Fees;
using corewebapi18072016.Delegates.com.vapstech.Fees;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using IVRMUX.Delegates.com.vapstech.Fees;

namespace IVRMUX.Controllers.com.vapstech.Fees
{
    // [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeOnlinePaymentVikasaController : Controller
    {

        FeeOnlinePaymentVikasaDelegate clsdel = new FeeOnlinePaymentVikasaDelegate();
        private readonly IHostingEnvironment _hostingEnvironment;
        PaymentDetails od1 = new PaymentDetails();
        public FeeOnlinePaymentVikasaController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        [Route("getalldetails")]
        public FeeStudentTransactionDTO getInitialData([FromBody] FeeStudentTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.ASMAY_Id == 0)
            {
                data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            //data.Amst_Id = id;
            data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return clsdel.getInitailData(data);
        }

        [Route("getamountdetails")]
        public FeeStudentTransactionDTO getamountdetailss([FromBody] FeeStudentTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.ASMAY_Id == 0)
            {
                data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));

            return clsdel.getamountdet(data);
        }


        [Route("paymentresponse/")]
        public ActionResult paymentresponse(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            string querystring = "";
            try
            {
                dto = clsdel.getpaymentresponse(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/FeeOnlinePayment/13?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/FeeOnlinePayment/13?status=Networkfailure";
                }
            }
            catch (Exception e)
            {
                //  dto.returnvalue = "";
            }
            return Redirect(querystring);
        }

        [Route("BillDeskpaymentresponse/")]
        public ActionResult BillDeskpaymentresponse(string msg)
        {

            string returnmsg = "";
            string msgresponse = msg.Trim();

            string querystring = "";

            string responseparam = "";
            string responsechecksumparam = "";

            try
            {

                string[] res = msgresponse.Split('|');

                responseparam = res[0] + "|" + res[1] + "|" + res[2] + "|" + res[3] + "|" + res[4] + "|" + res[5] + "|" + res[6] + "|" + res[7] + "|" + res[8] + "|" + res[9] + "|" + res[10] + "|" + res[11] + "|" + res[12] + "|" + res[13] + "|" + res[14] + "|" + res[15] + "|" + res[16] + "|" + res[17] + "|" + res[18] + "|" + res[19] + "|" + res[20] + "|" + res[21] + "|" + res[22] + "|" + res[23] + "|" + res[24];
                responsechecksumparam = res[25];

                var json = new PaymentDetails.BillDeskPayment
                {
                    MerchantID = res[0],
                    CustomerID = res[1],
                    TxnReferenceNo = res[2],
                    BankReferenceNo = res[3],
                    TxnAmount = res[4],
                    BankID = res[5],
                    BankMerchantID = res[6],
                    TxnType = res[7],
                    CurrencyName = res[8],
                    ItemCode = res[9],
                    SecurityType = res[10],
                    SecurityID = res[11],
                    SecurityPassword = res[12],
                    TxnDate = res[13],
                    AuthStatus = res[14],
                    SettlementType = res[15],
                    PhoneNo = res[16],
                    emailId = res[17],
                    Name = res[18],
                    ASMAY_Id = res[19],
                    AMST_Id = res[20],
                    MI_Id = res[21],
                    termid = res[22],
                    //groupid = res[23],
                    //ASMCL_ID = res[24],
                    ErrorStatus = res[23],
                    ErrorDescription = res[24],
                    CheckSum = res[25]
                };
                if (json.AuthStatus == "0300")
                {
                    returnmsg = "success";
                }
                else if (json.AuthStatus == "0399")
                {
                    returnmsg = "Cancel Transaction";
                }
                else if (json.AuthStatus == "NA")
                {
                    returnmsg = "Cancel Transaction";
                }
                else if (json.AuthStatus == "0002")
                {
                    returnmsg = "Pending Transaction";
                }
                else if (json.AuthStatus == "0001")
                {
                    returnmsg = "Cancel Transaction";
                }
                else
                {
                    returnmsg = json.ErrorDescription;
                }

                json.responseparam = responseparam;
                json.responsechecksumparam = responsechecksumparam;

                PaymentDetails.BillDeskPayment bill = clsdel.getBillDeskpaymentresponse(json);

                if (returnmsg.Equals("success"))
                {
                    querystring = "http://localhost:57606/#/app/FeeOnlinePayment/13?status=" + returnmsg;
                }
                else if (returnmsg.Equals("Cancel Transaction"))
                {
                    querystring = "http://localhost:57606/#/app/FeeOnlinePayment/13?status=cancelled";
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/FeeOnlinePayment/13?status=Networkfailure";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Redirect(querystring);
        }


        [Route("getgrouportermdetails")]
        public FeeStudentTransactionDTO getgrouportermdet([FromBody] FeeStudentTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.ASMAY_Id == 0)
            {
                data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return clsdel.getgrouportermdeta(data);
        }

        [Route("generatehashsequence")]
        public FeeStudentTransactionDTO generatehash([FromBody] FeeStudentTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.ASMAY_Id == 0)
            {
                data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return clsdel.generatehashseq(data);
        }

        [Route("getcustomgroups")]
        public FeeStudentTransactionDTO getcusgrp([FromBody] FeeStudentTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.ASMAY_Id == 0)
            {
                data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return clsdel.getcusgrps(data);
        }


    }
}
