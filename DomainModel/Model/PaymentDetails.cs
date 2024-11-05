using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class PaymentDetails
    {
        public string status { get; set; }
        public string udf1 { get; set; }

        public string hash { get; set; }
        public string trans_id { get; set; }
        public decimal amount { get; set; }

        public string MARCHANT_ID { get; set; }
        public string Seq { get; set; }
        public string productinfo { get; set; }
        public string firstname { get; set; }
        public string email { get; set; }
        public string AmountSymbol { get; set; }
        public string SaltKey { get; set; }

        public string payu_URL { get; set; }
        public string mobile { get; set; }
        public long phone { get; set; }

        public string msg { get; set; }

        public string surl { get; set; }

        public string furl { get; set; }
        public string service_provider { get; set; }
        public Array productinfoObj { get; set; }
        public string hash_string { get; set; }
        public long mihpayid { get; set; }
        public string mode { get; set; }

        public string txnid { get; set; }

        public string returnvalue { get; set; }

        public string IVRMOP_MERCHANT_KEY { get; set; }

        public string IVRMOP_MARCHANT_ID { get; set; }

        public string IVRMOP_SALT { get; set; }

        public string IVRMOP_BASE_URL { get; set; }

        public string IVRMOP_REG_AMOUNT { get; set; }

        public string IVRMOP_PROS_AMOUNT { get; set; }

        public long IVRMOP_MIID { get; set; }
        public string udf2 { get; set; }
        public string udf3 { get; set; }

        public string udf4 { get; set; }

        public string udf5 { get; set; }
        public string udf6 { get; set; }

        public string transaction_response_url { get; set; }


        public Array PaymentDetailsList { get; set; }

        public string paymentdetails { get; set; }

        public string responseupdate { get; set; }
        public string BANKTXNID { get; set; }

        public string razorpay_payment_id { get; set; }
        public string razorpay_order_id { get; set; }
        public string razorpay_signature { get; set; }

        public int FMA_Amount { get; set; }
        public Array razorpay { get; set; }
        public string splitpayinformation { get; set; }

        public string PASR_RegistrationNo { get; set; }

        public string institutioname { get; set; }
        public string institulogo { get; set; }
        public string stuaddress { get; set; }

        public long PASR_ID { get; set; }

        public long MI_Id { get; set; }
        public long date { get; set; }

        //added on 13-11-2021
        public long PACA_ID { get; set; }
        public string PACA_RegistrationNo { get; set; }

        public long UserId { get; set; }

        public string strForm { get; set; }
        public class EBSpayment
        {
            public int account_id { get; set; }
            public int channel { get; set; }
            public string currency { get; set; }
            public string ba_B25914CA4 { get; set; }
            public string ba_B259148DC { get; set; }
            public string split_profile_code { get; set; }

            public string reference_no { get; set; }
            public string description { get; set; }
            public string name { get; set; }
            public string address { get; set; }
            public string city { get; set; }

            public string state { get; set; }
            public string postal_code { get; set; }
            public string country { get; set; }
            public string return_url { get; set; }
            public string ship_name { get; set; }
            public string ship_address { get; set; }

            public string ship_city { get; set; }
            public string amount { get; set; }
            public string email { get; set; }
            public long phone { get; set; }
            public string mode { get; set; }
            public string ship_state { get; set; }
            public string ship_country { get; set; }
            public long ship_phone { get; set; }
            public string algo { get; set; }
            public string ship_postal_code { get; set; }
            public string name_on_card { get; set; }
            public long? card_number { get; set; }
            public long? card_expiry { get; set; }
            public long? card_cvv { get; set; }
            public string secure_hash { get; set; }
            public string V3URL { get; set; }

        }

        public class BillDeskPayment
        {
            public string MerchantID { get; set; }
            public string CustomerID { get; set; }
            public string TxnReferenceNo { get; set; }
            public string BankReferenceNo { get; set; }
            public string TxnAmount { get; set; }
            public string BankID { get; set; }
            public string BankMerchantID { get; set; }
            public string TxnType { get; set; }
            public string CurrencyName { get; set; }
            public string ItemCode { get; set; }
            public string SecurityType { get; set; }
            public string SecurityID { get; set; }
            public string SecurityPassword { get; set; }
            public string TxnDate { get; set; }
            public string AuthStatus { get; set; }
            public string SettlementType { get; set; }
            public string PhoneNo { get; set; }
            public string emailId { get; set; }
            public string Name { get; set; }
            public string ASMAY_Id { get; set; }
            public string AMST_Id { get; set; }
            public string MI_Id { get; set; }
            public string termid { get; set; }
            public string groupid { get; set; }
            public string ASMCL_ID { get; set; }
            public string ErrorStatus { get; set; }
            public string ErrorDescription { get; set; }
            public string CheckSum { get; set; }
            public string status { get; set; }

            public string responseparam { get; set; }
            public string responsechecksumparam { get; set; }


        }

        public class PAYTM
        {
            public string MID { get; set; }
            public string ORDER_ID { get; set; }
            public string CUST_ID { get; set; }
            public decimal TXN_AMOUNT { get; set; }
            public string CHANNEL_ID { get; set; }

            public string INDUSTRY_TYPE_ID { get; set; }
            public string WEBSITE { get; set; }
            public string CHECKSUMHASH { get; set; }
            public long? MOBILE_NO { get; set; }
            public string EMAIL { get; set; }//till this are mandatory
            public string CALLBACK_URL { get; set; }
            public string PAYMENT_MODE_ONLY { get; set; }
            public string AUTH_MODE { get; set; }
            public string PAYMENT_TYPE_ID { get; set; }
            public string CARD_TYPE { get; set; }
            public string BANK_CODE { get; set; }
            public string PROMO_CAMP_ID { get; set; }
            public string THEME { get; set; }

            public string REQUEST_TYPE { get; set; }
            public string payu_URL { get; set; }

            public string MERC_UNQ_REF { get; set; }

            public string status { get; set; }
            public string txnid { get; set; }

            public string BANKTXNID { get; set; }
            public long UserId { get; set; }

        }


        public class easybuzz
        {
            public string key { get; set; }
            public string txnid { get; set; }
            public string amount { get; set; }
            public string productinfo { get; set; }
            public string firstname { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
            public string UDF1 { get; set; }
            public string UDF2 { get; set; }
            public string UDF3 { get; set; }
            public string UDF4 { get; set; }
            public string UDF5 { get; set; }
            public string UDF6 { get; set; }
            public string UDF7 { get; set; }
            public string UDF8 { get; set; }
            public string UDF9 { get; set; }
            public string UDF10 { get; set; }
            public string status { get; set; }
            public string FYP_PaymentReference_Id { get; set; }

        }


        public class easybuzzmobile        {            public string result { get; set; }            public payment_response payment_response { get; set; }            public string status { get; set; }        }        public class payment_response        {            public string key { get; set; }            public string txnid { get; set; }            public string amount { get; set; }            public string productinfo { get; set; }            public string firstname { get; set; }            public string phone { get; set; }            public string email { get; set; }            public string udf1 { get; set; }            public string udf2 { get; set; }            public string udf3 { get; set; }            public string udf4 { get; set; }            public string udf5 { get; set; }            public string udf6 { get; set; }            public string udf7 { get; set; }            public string udf8 { get; set; }            public string udf9 { get; set; }            public string udf10 { get; set; }            public string status { get; set; }            public string FYP_PaymentReference_Id { get; set; }            public string merchant_logo { get; set; }            public string cardCategory { get; set; }            public decimal cash_back_percentage { get; set; }            public decimal deduction_percentage { get; set; }            public string error_Message { get; set; }            public string payment_source { get; set; }            public string error { get; set; }            public string addedon { get; set; }            public string mode { get; set; }            public string issuing_bank { get; set; }            public string bankcode { get; set; }            public long flag { get; set; }            public string unmappedstatus { get; set; }            public string easepayid { get; set; }            public string net_amount_debit { get; set; }            public string PG_TYPE { get; set; }            public string hash { get; set; }            public string name_on_card { get; set; }            public string bank_ref_num { get; set; }        }
        public class Airpay
        {
            public string TRANSACTIONID { get; set; }
            public string APTRANSACTIONID { get; set; }
            public decimal AMOUNT { get; set; }
            public string TRANSACTIONSTATUS { get; set; }
            public string MESSAGE { get; set; }
            public string ap_SecureHash { get; set; }
            public string CUSTOMVAR { get; set; }
            public string status { get; set; }
            //public string CUSTOMER { get; set; }
            
            //public long TRANSACTIONTYPE { get; set; }
            //public long CURRENCYCODE { get; set; }
            //public string ap_SecureHash { get; set; }
            //public string CHMOD { get; set; }
            //public long RISK { get; set; }
            //public string IPNID { get; set; }
            //public string TRANSACTIONPAYMENTSTATUS { get; set; }
            //public string buyerEmail { get; set; }
            //public string buyerFirstName { get; set; }
            //public string buyerLastName { get; set; }
            //public long buyerPhone { get; set; }
            //public string buyerAddress { get; set; }
            //public string buyerCity { get; set; }
            //public string buyerState { get; set; }
            //public string buyerCountry { get; set; }
            //public string buyerPinCode { get; set; }
            //public string customvar { get; set; }
            //public string checksum { get; set; }

            

        }
        public string order_id { get; set; }

        public Array notes { get; set; }
        public Array transfers { get; set; }

        public Array fetchuserid { get; set; }
        public class CCAvenue
        {
            public string order_id { get; set; }
            public int amount { get; set; }
            public string customer_id { get; set; }
            public string customer_email { get; set; }
            public string customer_phone { get; set; }
            public string payment_page_client_id { get; set; }
            public string return_url { get; set; }
            public string description { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string action { get; set; }
            public string status { get; set; }
            public string txn_id { get; set; }
            public string udf1 { get; set; }
            public string udf2 { get; set; }
            public string udf3 { get; set; }
            public string udf4 { get; set; }
            public string udf5 { get; set; }
            public string udf6 { get; set; }
            public string udf7 { get; set; }
            public string udf8 { get; set; }
            public string udf9 { get; set; }
            public string udf10 { get; set; }
        }


    }
    public class razorpaydetails
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class transferAPI
    {
        public string transfer_id { get; set; }
        public string entity { get; set; }
        public string source { get; set; }
        public string recipient { get; set; }
        public string amount { get; set; }
        public string created_at { get; set; }
    }

}
