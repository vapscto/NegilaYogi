using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.College.Preadmission;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Fees.FinancialAccounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class GenerateTransactionNumbering
    {
        private readonly DomainModelMsSqlServerContext _db;
        private readonly ClgAdmissionContext _ClgContext;
        public GenerateTransactionNumbering(DomainModelMsSqlServerContext db)
        {
            _db = db;
        }
        public GenerateTransactionNumbering(ClgAdmissionContext ClgContext)
        {
            _ClgContext = ClgContext;
        }
        public string GenerateNumber(Master_NumberingDTO en)
        {
            string GeneratedNumber = "";

            if (en != null)
            {
                if (en.IMN_AutoManualFlag == "Manual")
                {
                    GeneratedNumber = "";
                }
                else if (en.IMN_AutoManualFlag == "Auto")
                {
                    var acyr = _db.AcademicYear.Where(t => t.ASMAY_Id.Equals(en.ASMAY_Id)).FirstOrDefault();
                    string AcadYear = acyr.ASMAY_Year;
                    //string AcadYear = acyr.ASMAY_AcademicYearCode;
                    // string[] a = AcadYear.Split('-');

                    // AcadYear = a.ElementAt(0);

                    if (en.IMN_PrefixAcadYearCode == true)
                    {
                        GeneratedNumber = AcadYear;
                    }

                    else
                    if (en.IMN_PrefixParticular != "" && en.IMN_PrefixParticular != null)
                    {
                        GeneratedNumber = en.IMN_PrefixParticular;
                    }

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            if (en.IMN_StartingNo != "" && en.IMN_StartingNo != null)
                            {
                                if (GeneratedNumber != "")
                                {
                                    GeneratedNumber = GeneratedNumber + "/" + en.IMN_StartingNo.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                                }
                                else
                                {
                                    GeneratedNumber = en.IMN_StartingNo.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                                }

                            }
                            else
                            {
                                en.IMN_StartingNo = "0";
                                if (GeneratedNumber != "")
                                {
                                    GeneratedNumber = GeneratedNumber + en.IMN_StartingNo.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                                }
                                else
                                {
                                    GeneratedNumber = en.IMN_StartingNo.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                                }
                            }
                        }
                        else if (en.IMN_ZeroPrefixFlag == "Numeric")
                        {
                            if (en.IMN_StartingNo != "" && en.IMN_StartingNo != null)
                            {
                                GeneratedNumber = GeneratedNumber + en.IMN_StartingNo.ToString();
                            }
                        }
                        else
                        {
                            if (en.IMN_StartingNo != "" && en.IMN_StartingNo != null)
                            {
                                if (GeneratedNumber != "")
                                {
                                    GeneratedNumber = GeneratedNumber + "/" + en.IMN_StartingNo.ToString();
                                }
                                else
                                {
                                    GeneratedNumber = en.IMN_StartingNo.ToString();
                                }
                            }
                        }


                    }
                    else
                    {
                        if (en.IMN_StartingNo != "" && en.IMN_StartingNo != null)
                        {
                            if (GeneratedNumber != "")
                            {
                                GeneratedNumber = GeneratedNumber + "/" + en.IMN_StartingNo.ToString();
                            }
                            else
                            {
                                GeneratedNumber = en.IMN_StartingNo.ToString();
                            }
                        }
                    }

                    if (en.IMN_SuffixAcadYearCode == true)
                    {
                        if (GeneratedNumber != "")
                        {
                            GeneratedNumber = GeneratedNumber + "/" + AcadYear;
                        }
                        else
                        {
                            GeneratedNumber = AcadYear;
                        }
                    }
                    else if (en.IMN_SuffixParticular != "" && en.IMN_SuffixParticular != null)
                    {
                        if (GeneratedNumber != "")
                        {
                            GeneratedNumber = GeneratedNumber + "/" + en.IMN_SuffixParticular;
                        }
                        else
                        {
                            GeneratedNumber = en.IMN_SuffixParticular;
                        }
                    }

                    GeneratedNumber = TransactionNumberingType(GeneratedNumber, en);
                }


                else if (en.IMN_AutoManualFlag == "serial")
                {

                    if (en.IMN_ZeroPrefixFlag == "Numeric")
                    {
                        if (en.IMN_StartingNo != "" && en.IMN_StartingNo != null)
                        {
                            if (GeneratedNumber != "")
                            {
                                GeneratedNumber = en.IMN_StartingNo.ToString();
                            }
                            else
                            {
                                GeneratedNumber = GeneratedNumber + en.IMN_StartingNo.ToString();
                            }

                        }
                    }
                    GeneratedNumber = TransactionNumberingType(GeneratedNumber, en);
                }
            }

            return GeneratedNumber;

        }

        public string TransactionNumberingType(string GeneratedNumber, Master_NumberingDTO en)
        {
            if (en.IMN_Flag == "Registration")
            {
                GeneratedNumber = checkDublicateandIncreamentForRegistrationNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "SMSEMAIL")
            {
                GeneratedNumber = checkDublicateandIncreamentForSMSEMAILNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "Form")
            {
                if (en.IMN_ZeroPrefixFlag == "Numeric")
                {
                    GeneratedNumber = checkDublicateandIncreamentForNumericFormNumber(GeneratedNumber, en);
                }
                else
                {
                    GeneratedNumber = checkDublicateandIncreamentForFormNumber(GeneratedNumber, en);
                }

            }
            else if (en.IMN_Flag == "FormC")
            {
                if (en.IMN_ZeroPrefixFlag == "Numeric")
                {
                    GeneratedNumber = checkDublicateandIncreamentForCNumericFormNumber(GeneratedNumber, en);
                }
                else
                {
                    GeneratedNumber = checkDublicateandIncreamentForFormCNumber(GeneratedNumber, en);
                }

            }
            else if (en.IMN_Flag == "BusForm")
            {
                GeneratedNumber = checkDublicateandIncreamentForBusFormNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "Prospectus")
            {
                GeneratedNumber = checkDublicateandIncreamentForProspectusNumber(GeneratedNumber, en);
            }

            else if (en.IMN_Flag == "Enquiry")
            {
                GeneratedNumber = checkDublicateandIncreamentForEnquiryNumber(GeneratedNumber, en);
            }



            else if (en.IMN_Flag == "PreRegistration")
            {
                GeneratedNumber = checkDublicateandIncreamentForFormNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "Admission")
            {
                GeneratedNumber = checkDublicateandIncreamentForAdmissionNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "InteractionStudent")
            {
                GeneratedNumber = checkDublicateandIncreamentForInteractionStudentNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "PushNotification")
            {
                GeneratedNumber = checkDublicateandIncreamentForPushNotificationNumber(GeneratedNumber, en);
            }

            else if (en.IMN_Flag == "INVPR")
            {
                GeneratedNumber = checkDublicateandIncreamentForInventoryPurchaseRequisitionNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "INVPI")
            {
                GeneratedNumber = checkDublicateandIncreamentForInventoryPurchaseIndentNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "INVQUOTATION")
            {
                GeneratedNumber = checkDublicateandIncreamentForInventoryQuotationNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "INVPO")
            {
                GeneratedNumber = checkDublicateandIncreamentForInventoryPurchaseOrderNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "INVGRN")
            {
                GeneratedNumber = checkDublicateandIncreamentForInventoryGRNNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "INVSALES")
            {

                var count4 = "";
                string GeneratedNumber1 = "";
                if (en.IMN_RestartNumFlag == "Never")
                {

                    var count1 = _db.INV_M_SalesDMO.Where(imp => imp.MI_Id == en.MI_Id).ToList();

                    if (count1.Count > 0)
                    {
                        count4 = _db.INV_M_SalesDMO.Where(imp => imp.MI_Id == en.MI_Id).LastOrDefault().INVMSL_SalesNo;

                    }
                    else
                    {
                        count4 = GeneratedNumber;
                        string[] lastRecordArray = count4.Split('/');
                        string firstfield = lastRecordArray.ElementAt(0);
                        int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                        string lastfield = lastRecordArray.ElementAt(2);
                        int staringNumberInc = staringNumber - 1;
                        count4 = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }
                else if (en.IMN_RestartNumFlag == "Yearly")
                {
                    var count1 = _db.INV_M_SalesDMO.Where(imp => imp.MI_Id == en.MI_Id).ToList();

                    if (count1.Count > 0)
                    {
                        count4 = _db.INV_M_SalesDMO.Where(imp => imp.MI_Id == en.MI_Id).LastOrDefault().INVMSL_SalesNo;

                    }
                    else
                    {
                        count4 = GeneratedNumber;
                        string[] lastRecordArray = count4.Split('/');
                        string firstfield = lastRecordArray.ElementAt(0);
                        int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                        string lastfield = lastRecordArray.ElementAt(2);
                        int staringNumberInc = staringNumber - 1;
                        count4 = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }
                if (count4 != null)
                {
                    string[] lastRecordArray = count4.Split('/');
                    if (lastRecordArray != null)
                    {
                        string firstfield = lastRecordArray.ElementAt(0);
                        int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                        string lastfield = lastRecordArray.ElementAt(2);
                        int staringNumberInc = staringNumber + 1;

                        if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                        {

                            if (en.IMN_ZeroPrefixFlag == "Yes")
                            {
                                GeneratedNumber1 = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                            }
                            else
                            {
                                GeneratedNumber1 = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                            }
                        }
                        else
                        {
                            GeneratedNumber1 = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }

                    GeneratedNumber = GeneratedNumber1;
                }

                // GeneratedNumber = checkDublicateandIncreamentForInventorySalesNumber(GeneratedNumber, en);
            }


            else if (en.IMN_Flag == "AdmissionReg")
            {
                GeneratedNumber = checkDublicateandIncreamentForAdmissionRegNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "tcno")
            {
                GeneratedNumber = checkDublicateandIncreamentFortcNumber(GeneratedNumber, en);
            }

            else if (en.IMN_Flag == "Online")
            {
                GeneratedNumber = checkDublicateandIncreamentForOnlineNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "OnlineC")
            {
                GeneratedNumber = checkDublicateandIncreamentForOnlineCNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "OnlineRegular")
            {
                GeneratedNumber = checkDublicateandIncreamentForOnlineRegularNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "PreOnlineFull")
            {
                GeneratedNumber = checkDublicateandIncreamentForPreadmissionOnlineFullpaymentNumber(GeneratedNumber, en);
            }

            else if (en.IMN_Flag == "Transaction")
            {
                GeneratedNumber = checkDublicateandIncreamentForTransactionNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "Refund")
            {
                GeneratedNumber = checkDublicateandIncreamentForRefundNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "Receipt")
            {
                GeneratedNumber = checkDublicateandIncreamentForReceiptNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "Loan")
            {
                GeneratedNumber = checkDublicateandIncreamentForLoan(GeneratedNumber, en);
            }
            //Bus Hire related numbering.
            else if (en.IMN_Flag == "TripOnlineBooking")
            {
                GeneratedNumber = checkDublicateandIncreamentForTripNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "TripNo")
            {
                GeneratedNumber = checkDublicateandIncreamentForTripNumberingNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "TripBill")
            {
                GeneratedNumber = checkDublicateandIncreamentForTripBillNumberingNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "TripReceiptNo")
            {
                GeneratedNumber = checkDublicateandIncreamentForTripReceiptNumberingNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "LeaveNo")
            {
                GeneratedNumber = checkDublicateandIncreamentForLeave(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "ClgAdmissionReg")
            {
                GeneratedNumber = checkDublicateandIncreamentForClgAdmissionRegNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "ClgAdmission")
            {
                GeneratedNumber = checkDublicateandIncreamentForClgAdmissionNumber(GeneratedNumber, en);

            }
            else if (en.IMN_Flag == "VisitorManagement")
            {
                GeneratedNumber = checkDublicateandIncreamentForVMInwardNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "VisitorManagement2")
            {
                GeneratedNumber = checkDublicateandIncreamentForVMOutwardNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "StudentGatePass")
            {
                GeneratedNumber = checkDublicateandIncreamentForStudentGatePassNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "StaffGatePass")
            {
                GeneratedNumber = checkDublicateandIncreamentForStaffGatePassNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "Subscription")
            {
                GeneratedNumber = checkDublicateandIncreamentForSubscriptionNumberInLIB(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "ServiceRefNo")
            {
                GeneratedNumber = checkDublicateandIncreamentForVehicleserviceNumberingNumber(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "studentSecredCode")
            {
                GeneratedNumber = checkDublicateandIncreamentForStudentGatePassSecredCode(GeneratedNumber, en);
            }
            else if (en.IMN_Flag == "PDA")
            {
                GeneratedNumber = checkDublicateandIncreamentForPaymentPDA(GeneratedNumber, en);
            }
            return GeneratedNumber;
        }

        public string checkDublicateandIncreamentForPaymentPDA(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;

            if (en.IMN_RestartNumFlag == "Never")
            {
                // count = _db.VMS_Payment_VoucherDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.VPAYVOU_VoucherNo == GeneratedNumber).Count();
                count = _db.PDA_ExpenditureDMO.Where(imp => imp.PDAE_TransactionNo == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                // count = _db.VMS_Payment_VoucherDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.VPAYVOU_VoucherNo == GeneratedNumber).Count();
                count = _db.PDA_ExpenditureDMO.Where(imp => imp.PDAE_TransactionNo == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_ZeroPrefixFlag == "Numeric")
                    {
                        GeneratedNumber = staringNumberInc.ToString();
                    }
                    else if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + lastfield + "/" + staringNumberInc.ToString();
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForPaymentPDA(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }


        //Transaction

        public string checkDublicateandIncreamentForTransactionNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.FeePaymentDetails.Where(imp => imp.MI_Id == en.MI_Id && imp.FYP_Receipt_No == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.FeePaymentDetails.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_ID == en.ASMAY_Id && imp.FYP_Receipt_No == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForTransactionNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }
        //refund
        public string checkDublicateandIncreamentForRefundNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.FeeMasterRefundDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.FR_RefundNo == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.FeeMasterRefundDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_ID == en.ASMAY_Id && imp.FR_RefundNo == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForRefundNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        //preadmission online full payment
        public string checkDublicateandIncreamentForPreadmissionOnlineFullpaymentNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.Fee_M_Online_TransactionDMO.Where(imp => imp.FMOT_Trans_Id == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.Fee_M_Online_TransactionDMO.Where(imp => imp.FMOT_Trans_Id == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;


                    var dat = _db.Fee_M_Online_TransactionDMO.Where(imp => imp.FMOT_Trans_Id.Contains(firstfield)
                       ).OrderByDescending(imp => imp.FMOT_Id).ToList();
                    if (dat.Count() > 0)
                    {
                        string generationnumber = dat[0].FMOT_Trans_Id;
                        string[] lastRecordArray1 = generationnumber.Split('/');
                        if (lastRecordArray1 != null)
                        {
                            int staringNumber1 = Convert.ToInt32(lastRecordArray1.ElementAt(1));
                            staringNumberInc = staringNumber1 + 1;
                        }
                    }


                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForPreadmissionOnlineFullpaymentNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        //online regular 

        public string checkDublicateandIncreamentForOnlineRegularNumber(string GeneratedNumber, Master_NumberingDTO en)
        {

            try
            {
                int count = 0;
                if (en.IMN_RestartNumFlag == "Never")
                {
                    count = _db.Fee_M_Online_TransactionDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.FMOT_Trans_Id == GeneratedNumber).Count();
                }
                else if (en.IMN_RestartNumFlag == "Yearly")
                {
                    count = _db.Fee_M_Online_TransactionDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_ID == en.ASMAY_Id && imp.FMOT_Trans_Id == GeneratedNumber).Count();
                }
                if (count > 0)
                {
                    string[] lastRecordArray = GeneratedNumber.Split('/');
                    if (lastRecordArray != null)
                    {
                        string firstfield = lastRecordArray.ElementAt(0);
                        int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                        string lastfield = lastRecordArray.ElementAt(2);
                        int staringNumberInc = staringNumber + 1;


                        var dat = _db.Fee_M_Online_TransactionDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_ID == en.ASMAY_Id && imp.FMOT_Trans_Id.Contains(firstfield)
                           ).OrderByDescending(imp => imp.FMOT_Id).ToList();
                        if (dat.Count() > 0)
                        {
                            string generationnumber = dat[0].FMOT_Trans_Id;
                            string[] lastRecordArray1 = generationnumber.Split('/');
                            if (lastRecordArray1 != null)
                            {
                                int staringNumber1 = Convert.ToInt32(lastRecordArray1.ElementAt(1));
                                staringNumberInc = staringNumber1 + 1;
                            }
                        }


                        if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                        {

                            if (en.IMN_ZeroPrefixFlag == "Yes")
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                            }
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }

                    GeneratedNumber = checkDublicateandIncreamentForOnlineRegularNumber(GeneratedNumber, en);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return GeneratedNumber;
        }

        //online

        public string checkDublicateandIncreamentForOnlineNumber(string GeneratedNumber, Master_NumberingDTO en)
        {

            try
            {


                int count = 0;
                if (en.IMN_RestartNumFlag == "Never")
                {
                    count = _db.FeePaymentDetails.Where(imp => imp.MI_Id == en.MI_Id && imp.fyp_transaction_id == GeneratedNumber).Count();
                }
                else if (en.IMN_RestartNumFlag == "Yearly")
                {
                    count = _db.FeePaymentDetails.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_ID == en.ASMAY_Id && imp.fyp_transaction_id == GeneratedNumber).Count();
                }
                if (count > 0)
                {
                    string[] lastRecordArray = GeneratedNumber.Split('/');
                    if (lastRecordArray != null)
                    {
                        string firstfield = lastRecordArray.ElementAt(0);
                        int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                        string lastfield = lastRecordArray.ElementAt(2);
                        int staringNumberInc = staringNumber + 1;


                        var dat = _db.FeePaymentDetails.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_ID == en.ASMAY_Id && imp.fyp_transaction_id.Contains(firstfield)
                           ).OrderByDescending(imp => imp.FYP_Id).ToList();
                        if (dat.Count() > 0)
                        {
                            string generationnumber = dat[0].fyp_transaction_id;
                            string[] lastRecordArray1 = generationnumber.Split('/');
                            if (lastRecordArray1 != null)
                            {
                                int staringNumber1 = Convert.ToInt32(lastRecordArray1.ElementAt(1));
                                staringNumberInc = staringNumber1 + 1;
                            }
                        }


                        if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                        {

                            if (en.IMN_ZeroPrefixFlag == "Yes")
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                            }
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }

                    GeneratedNumber = checkDublicateandIncreamentForOnlineNumber(GeneratedNumber, en);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return GeneratedNumber;
        }

        //onlinec

        public string checkDublicateandIncreamentForOnlineCNumber(string GeneratedNumber, Master_NumberingDTO en)
        {

            try
            {


                int count = 0;
                if (en.IMN_RestartNumFlag == "Never")
                {
                    count = _db.FeePaymentDetailsCollege.Where(imp => imp.MI_Id == en.MI_Id && imp.FYP_Transaction_Id == GeneratedNumber).Count();
                }
                else if (en.IMN_RestartNumFlag == "Yearly")
                {
                    count = _db.FeePaymentDetailsCollege.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id && imp.FYP_Transaction_Id == GeneratedNumber).Count();
                }
                if (count > 0)
                {
                    string[] lastRecordArray = GeneratedNumber.Split('/');
                    if (lastRecordArray != null)
                    {
                        string firstfield = lastRecordArray.ElementAt(0);
                        int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                        string lastfield = lastRecordArray.ElementAt(2);
                        int staringNumberInc = staringNumber + 1;


                        var dat = _db.FeePaymentDetailsCollege.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id && imp.FYP_Transaction_Id.Contains(firstfield)
                           ).OrderByDescending(imp => imp.FYP_Id).ToList();
                        if (dat.Count() > 0)
                        {
                            string generationnumber = dat[0].FYP_Transaction_Id;
                            string[] lastRecordArray1 = generationnumber.Split('/');
                            if (lastRecordArray1 != null)
                            {
                                int staringNumber1 = Convert.ToInt32(lastRecordArray1.ElementAt(1));
                                staringNumberInc = staringNumber1 + 1;
                            }
                        }


                        if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                        {

                            if (en.IMN_ZeroPrefixFlag == "Yes")
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                            }
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }

                    GeneratedNumber = checkDublicateandIncreamentForOnlineCNumber(GeneratedNumber, en);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return GeneratedNumber;
        }


        //Registration

        int cnt = 1;
        public string checkDublicateandIncreamentForRegistrationNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            //List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
            //mstConfig = _db.mstConfig.Where(d => d.MI_Id.Equals(en.MI_Id) && d.ASMAY_Id.Equals(en.ASMAY_Id)).ToList();
            long pasr_idr = _db.StudentApplication.Where(imp => imp.MI_Id == en.MI_Id).Select(s => s.pasr_id).Max();
            if (cnt == 1)
            {
                var dd = _db.StudentApplication.Where(imp => imp.MI_Id == en.MI_Id && imp.pasr_id == pasr_idr).Select(s => s.PASR_RegistrationNo).FirstOrDefault();
                if (dd != null)
                {
                    GeneratedNumber = dd;
                }

            }



            var mstConfig = _db.mstConfig.Where(d => d.MI_Id == en.MI_Id && d.ASMAY_Id == en.ASMAY_Id).ToList();

            int count = 0;
            int staringNumberInc = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.StudentApplication.Where(imp => imp.MI_Id == en.MI_Id && imp.PASR_RegistrationNo.Equals(GeneratedNumber)).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.StudentApplication.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id && imp.PASR_RegistrationNo.Equals(GeneratedNumber)).Count();
            }
            if (count > 0)
            {
                cnt += 1;
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string lastfield = "";
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
                    {
                        lastfield = "/" + lastRecordArray.ElementAt(2);
                    }

                    if (mstConfig.FirstOrDefault().ISPAC_ApplNoIncrementFlg == true)
                    {
                        staringNumberInc = staringNumber + (Convert.ToInt16(mstConfig.FirstOrDefault().ISPAC_ApplNoIncrementBy) + 1);
                    }
                    else
                    {
                        staringNumberInc = staringNumber + 1;
                    }


                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + lastfield;
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                            }

                        }
                        else
                        {
                            if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + lastfield;
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
                            }
                        }
                    }
                    else
                    {
                        if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
                        }
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForRegistrationNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }
        //public string checkDublicateandIncreamentForRegistrationNumber(string GeneratedNumber, Master_NumberingDTO en)
        //{
        //    List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
        //    mstConfig = _db.mstConfig.Where(d => d.MI_Id.Equals(en.MI_Id) && d.ASMAY_Id.Equals(en.ASMAY_Id)).ToList();

        //    int count = 0;
        //    int staringNumberInc = 0;
        //    if (en.IMN_RestartNumFlag == "Never")
        //    {
        //        count = _db.StudentApplication.Where(imp => imp.MI_Id == en.MI_Id && imp.PASR_RegistrationNo.Equals(GeneratedNumber)).Count();
        //    }
        //    else if (en.IMN_RestartNumFlag == "Yearly")
        //    {
        //        count = _db.StudentApplication.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id && imp.PASR_RegistrationNo.Equals(GeneratedNumber)).Count();
        //    }
        //    if (count > 0)
        //    {
        //        string[] lastRecordArray = GeneratedNumber.Split('/');
        //        if (lastRecordArray != null)
        //        {
        //            string lastfield = "";
        //            string firstfield = lastRecordArray.ElementAt(0);
        //            int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
        //            if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
        //            {
        //                lastfield = "/" + lastRecordArray.ElementAt(2);
        //            }

        //            if (mstConfig.FirstOrDefault().ISPAC_ApplNoIncrementFlg == true)
        //            {
        //                staringNumberInc = staringNumber + (Convert.ToInt16(mstConfig.FirstOrDefault().ISPAC_ApplNoIncrementBy) + 1);
        //            }
        //            else
        //            {
        //                staringNumberInc = staringNumber + 1;
        //            }


        //            if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
        //            {

        //                if (en.IMN_ZeroPrefixFlag == "Yes")
        //                {
        //                    if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
        //                    {
        //                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + lastfield;
        //                    }
        //                    else
        //                    {
        //                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
        //                    }

        //                }
        //                else
        //                {
        //                    if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
        //                    {
        //                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + lastfield;
        //                    }
        //                    else
        //                    {
        //                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
        //                {
        //                    GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + lastfield;
        //                }
        //                else
        //                {
        //                    GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
        //                }
        //            }
        //        }

        //        GeneratedNumber = checkDublicateandIncreamentForRegistrationNumber(GeneratedNumber, en);
        //    }

        //    return GeneratedNumber;
        //}

        //SMSEMAIL


        public string checkDublicateandIncreamentForSMSEMAILNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.SMS_Sent_Details.Where(imp => imp.MI_ID == en.MI_Id && imp.SSD_TransactionId.Equals(Convert.ToInt64(GeneratedNumber))).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.SMS_Sent_Details.Where(imp => imp.MI_ID == en.MI_Id && imp.SSD_TransactionId.Equals(Convert.ToInt64(GeneratedNumber))).Count();
            }
            if (count > 0)
            {
                string lastRecordArray = "";
                string lastRecordArray2 = "";
                string lastRecordArray3 = "";
                int staringNumber = 0;
                if (GeneratedNumber.Length > 5)
                {
                    lastRecordArray = GeneratedNumber.Substring(0, 9);
                    lastRecordArray2 = GeneratedNumber.Substring(10, 4);
                    lastRecordArray3 = GeneratedNumber.Substring(15, 3);
                }
                else
                {
                    lastRecordArray2 = GeneratedNumber;
                }

                if (lastRecordArray2 != null)
                {

                    if (GeneratedNumber.Length > 5)
                    {
                        string firstfield = lastRecordArray;
                        staringNumber = Convert.ToInt32(lastRecordArray2);
                        string lastfield = lastRecordArray3;
                    }
                    else
                    {
                        staringNumber = Convert.ToInt32(lastRecordArray2);
                    }
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                        }
                        else
                        {
                            GeneratedNumber = staringNumberInc.ToString();
                        }
                    }
                    else
                    {
                        GeneratedNumber = staringNumberInc.ToString();
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForSMSEMAILNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }




        //form no
        public string checkDublicateandIncreamentForFormNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.StudentApplication.Where(imp => imp.MI_Id == en.MI_Id && imp.PASR_RegistrationNo.Equals(GeneratedNumber)).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.StudentApplication.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id && imp.PASR_RegistrationNo.Equals(GeneratedNumber)).Count();
            }
            if (count > 0)
            {
                string lastRecordArray = "";
                string lastRecordArray2 = "";
                string lastRecordArray3 = "";
                int staringNumber = 0;
                if (GeneratedNumber.Length > 5)
                {
                    lastRecordArray = GeneratedNumber.Substring(0, 9);
                    lastRecordArray2 = GeneratedNumber.Substring(10, 4);
                    lastRecordArray3 = GeneratedNumber.Substring(15, 3);

                }
                else
                {
                    lastRecordArray2 = GeneratedNumber;
                }

                if (lastRecordArray2 != null)
                {

                    if (GeneratedNumber.Length > 5)
                    {
                        string firstfield = lastRecordArray;
                        staringNumber = Convert.ToInt32(lastRecordArray2);
                        string lastfield = lastRecordArray3;
                    }
                    else
                    {
                        staringNumber = Convert.ToInt32(lastRecordArray2);
                    }
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                        }
                        else
                        {
                            GeneratedNumber = staringNumberInc.ToString();
                        }
                    }
                    else
                    {
                        GeneratedNumber = staringNumberInc.ToString();
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForFormNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        public string checkDublicateandIncreamentForFormCNumber(string GeneratedNumber, Master_NumberingDTO en)
        {

            List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
            mstConfig = _db.mstConfig.Where(d => d.MI_Id.Equals(en.MI_Id) && d.ASMAY_Id.Equals(en.ASMAY_Id)).ToList();

            int count = 0;
            int staringNumberInc = 0;
            //int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.PA_College_Application.Where(imp => imp.MI_Id == en.MI_Id && imp.PACA_RegistrationNo == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.PA_College_Application.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id && imp.PACA_RegistrationNo == GeneratedNumber).Count();
            }
            if (count > 0)
            {


                //string lastRecordArray = "";
                //string lastRecordArray2 = "";
                //string lastRecordArray3 = "";
                //int staringNumber = 0;
                //if (GeneratedNumber.Length > 5)
                //{
                //    lastRecordArray = GeneratedNumber.Substring(0, 9);
                //    lastRecordArray2 = GeneratedNumber.Substring(10, 4);
                //    lastRecordArray3 = GeneratedNumber.Substring(15, 3);

                //}
                //else
                //{
                //    lastRecordArray2 = GeneratedNumber;
                //}

                //if (lastRecordArray2 != null)
                //{

                //    if (GeneratedNumber.Length > 5)
                //    {
                //        string firstfield = lastRecordArray;
                //        staringNumber = Convert.ToInt32(lastRecordArray2);
                //        string lastfield = lastRecordArray3;
                //    }
                //    else
                //    {
                //        staringNumber = Convert.ToInt32(lastRecordArray2);
                //    }
                //    int staringNumberInc = staringNumber + 1;

                //    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                //    {

                //        if (en.IMN_ZeroPrefixFlag == "Yes")
                //        {
                //            GeneratedNumber = staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                //        }
                //        else
                //        {
                //            GeneratedNumber = staringNumberInc.ToString();
                //        }
                //    }
                //    else
                //    {
                //        GeneratedNumber = staringNumberInc.ToString();
                //    }
                //}



                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string lastfield = "";
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    // int lastNumber = Convert.ToInt32(lastRecordArray.ElementAt(2));
                    if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
                    {
                        lastfield = "/" + lastRecordArray.ElementAt(2);
                    }

                    if (mstConfig.FirstOrDefault().ISPAC_ApplNoIncrementFlg == true)
                    {
                        staringNumberInc = staringNumber + (Convert.ToInt16(mstConfig.FirstOrDefault().ISPAC_ApplNoIncrementBy));
                    }
                    else
                    {
                        staringNumberInc = staringNumber + 1;
                    }


                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            //GeneratedNumber = staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                            if (lastfield != null && lastfield != "")
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                            }

                        }
                        else
                        {
                            GeneratedNumber = staringNumberInc.ToString();
                        }
                    }
                    else
                    {
                        GeneratedNumber = staringNumberInc.ToString();
                    }
                }

                //if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                //    {

                //        if (en.IMN_ZeroPrefixFlag == "Yes")
                //        {
                //            if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
                //            {
                //                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + lastfield;
                //            }
                //            else
                //            {
                //                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                //            }

                //        }
                //        else
                //        {
                //            if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
                //            {
                //                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + lastfield;
                //            }
                //            else
                //            {
                //                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
                //            }
                //        }
                //    }
                //    else
                //    {
                //        if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
                //        {
                //            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + lastfield;
                //        }
                //        else
                //        {
                //            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
                //        }
                //    }
                //}

                GeneratedNumber = checkDublicateandIncreamentForFormCNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        public string checkDublicateandIncreamentForNumericFormNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            List<StudentApplication> Reg = new List<StudentApplication>();
            if (en.IMN_RestartNumFlag == "Never")
            {
                Reg = _db.StudentApplication.Where(imp => imp.MI_Id == en.MI_Id).OrderByDescending(t => t.pasr_id).Take(1).ToList();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                Reg = _db.StudentApplication.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id).OrderByDescending(t => t.pasr_id).Take(1).ToList();
            }
            if (Reg.Count() > 0)
            {
                if (GeneratedNumber != "")
                {
                    GeneratedNumber = (Convert.ToInt32(Reg.FirstOrDefault().PASR_RegistrationNo) + 1).ToString();
                }
                //GeneratedNumber = checkDublicateandIncreamentForNumericFormNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        public string checkDublicateandIncreamentForCNumericFormNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            List<PA_College_Application> Reg = new List<PA_College_Application>();
            List<PA_College_Application> RegCheck = new List<PA_College_Application>();

            if (en.IMN_RestartNumFlag == "Never")
            {
                Reg = _db.PA_College_Application.Where(imp => imp.MI_Id == en.MI_Id).OrderByDescending(t => t.PACA_Id).Take(1).ToList();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                Reg = _db.PA_College_Application.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id).OrderByDescending(t => t.PACA_Id).Take(1).ToList();
            }
            if (Reg.Count() > 0)
            {
                if (GeneratedNumber != "")
                {
                    GeneratedNumber = (Convert.ToInt32(Reg.FirstOrDefault().PACA_RegistrationNo) + 1).ToString();
                }
                //GeneratedNumber = checkDublicateandIncreamentForNumericFormNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }


        //bus form
        public string checkDublicateandIncreamentForBusFormNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.Adm_Student_Transport_ApplicationDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.ASTA_ApplicationNo.Equals(GeneratedNumber)).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.Adm_Student_Transport_ApplicationDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.ASTA_FutureAY == en.ASMAY_Id && imp.ASTA_ApplicationNo.Equals(GeneratedNumber)).Count();
            }
            if (count > 0)
            {
                string lastRecordArray = "";
                string lastRecordArray2 = "";
                string lastRecordArray3 = "";
                int staringNumber = 0;
                if (GeneratedNumber.Length > 5)
                {
                    lastRecordArray = GeneratedNumber.Substring(0, 9);
                    lastRecordArray2 = GeneratedNumber.Substring(10, 4);
                    lastRecordArray3 = GeneratedNumber.Substring(15, 3);

                }
                else
                {
                    lastRecordArray2 = GeneratedNumber;
                }

                if (lastRecordArray2 != null)
                {

                    if (GeneratedNumber.Length > 5)
                    {
                        string firstfield = lastRecordArray;
                        staringNumber = Convert.ToInt32(lastRecordArray2);
                        string lastfield = lastRecordArray3;
                    }
                    else
                    {
                        staringNumber = Convert.ToInt32(lastRecordArray2);
                    }
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                        }
                        else
                        {
                            GeneratedNumber = staringNumberInc.ToString();
                        }
                    }
                    else
                    {
                        GeneratedNumber = staringNumberInc.ToString();
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForBusFormNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        //preregistration
        public string checkDublicateandIncreamentForpreregistrationNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.StudentApplication.Where(imp => imp.MI_Id == en.MI_Id && imp.PASR_RegistrationNo.Equals(GeneratedNumber)).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.StudentApplication.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id && imp.PASR_RegistrationNo.Equals(GeneratedNumber)).Count();
            }
            if (count > 0)
            {
                string lastRecordArray = "";
                string lastRecordArray2 = "";
                string lastRecordArray3 = "";
                int staringNumber = 0;
                if (GeneratedNumber.Length > 5)
                {
                    lastRecordArray = GeneratedNumber.Substring(0, 9);
                    lastRecordArray2 = GeneratedNumber.Substring(10, 4);
                    lastRecordArray3 = GeneratedNumber.Substring(15, 3);

                }
                else
                {
                    lastRecordArray2 = GeneratedNumber;
                }

                if (lastRecordArray2 != null)
                {

                    if (GeneratedNumber.Length > 5)
                    {
                        string firstfield = lastRecordArray;
                        staringNumber = Convert.ToInt32(lastRecordArray2);
                        string lastfield = lastRecordArray3;
                    }
                    else
                    {
                        staringNumber = Convert.ToInt32(lastRecordArray2);
                    }
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                        }
                        else
                        {
                            GeneratedNumber = staringNumberInc.ToString();
                        }
                    }
                    else
                    {
                        GeneratedNumber = staringNumberInc.ToString();
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForpreregistrationNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }
        //Enquiry

        public string checkDublicateandIncreamentForEnquiryNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.Enquiry.Where(imp => imp.MI_Id == en.MI_Id && imp.PASE_EnquiryNo == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.Enquiry.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id && imp.PASE_EnquiryNo == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForEnquiryNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        ////Prospectus

        //public string checkDublicateandIncreamentForProspectusNumber(string GeneratedNumber, Master_NumberingDTO en)
        //{
        //    int count = 0;
        //    if (en.IMN_RestartNumFlag == "Never")
        //    {
        //        count = _db.prospectus.Where(imp => imp.MI_ID == en.MI_Id && imp.PASP_ProspectusNo == GeneratedNumber).Count();
        //    }
        //    else if (en.IMN_RestartNumFlag == "Yearly")
        //    {
        //        count = _db.prospectus.Where(imp => imp.MI_ID == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id && imp.PASP_ProspectusNo == GeneratedNumber).Count();
        //    }
        //    if (count > 0)
        //    {
        //        string[] lastRecordArray = GeneratedNumber.Split('/');
        //        if (lastRecordArray != null)
        //        {
        //            string firstfield = lastRecordArray.ElementAt(0);
        //            int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
        //            string lastfield = lastRecordArray.ElementAt(2);
        //            int staringNumberInc = staringNumber + 1;

        //            if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
        //            {

        //                if (en.IMN_ZeroPrefixFlag == "Yes")
        //                {
        //                    GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
        //                }
        //                else
        //                {
        //                    GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
        //                }
        //            }
        //            else
        //            {
        //                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
        //            }
        //        }

        //        GeneratedNumber = checkDublicateandIncreamentForProspectusNumber(GeneratedNumber, en);
        //    }

        //    return GeneratedNumber;
        //}


        //Prospectus
        public string checkDublicateandIncreamentForProspectusNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            //int count = 0;
            //if (en.IMN_RestartNumFlag == "Never")
            //{
            //    count = _db.prospectus.Where(imp => imp.MI_ID == en.MI_Id && imp.PASP_ProspectusNo == GeneratedNumber).Count();
            //}
            //else if (en.IMN_RestartNumFlag == "Yearly")
            //{
            //    count = _db.prospectus.Where(imp => imp.MI_ID == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id && imp.PASP_ProspectusNo.Equals(GeneratedNumber)).Count();
            //}
            //if (count > 0)
            //{
            //    string[] lastRecordArray = GeneratedNumber.Split('/');
            //    if (lastRecordArray != null)
            //    {
            //        string lastfield = "";
            //        string firstfield = lastRecordArray.ElementAt(0);
            //        int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
            //        if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
            //        {
            //            lastfield = "/" + lastRecordArray.ElementAt(2);
            //        }

            //        int staringNumberInc = staringNumber + 1;

            //        if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
            //        {

            //            if (en.IMN_ZeroPrefixFlag == "Yes")
            //            {
            //                if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
            //                {
            //                    GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + lastfield;
            //                }
            //                else
            //                {
            //                    GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
            //                }

            //            }
            //            else
            //            {
            //                if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
            //                {
            //                    GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + lastfield;
            //                }
            //                else
            //                {
            //                    GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
            //                }
            //            }
            //        }
            //        else
            //        {
            //            if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
            //            {
            //                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + lastfield;
            //            }
            //            else
            //            {
            //                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
            //            }
            //        }
            //    }

            //    GeneratedNumber = checkDublicateandIncreamentForProspectusNumber(GeneratedNumber, en);
            //}




            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.prospectus.Where(imp => imp.MI_ID == en.MI_Id && imp.PASP_ProspectusNo == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.prospectus.Where(imp => imp.MI_ID == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id && imp.PASP_ProspectusNo.Equals(GeneratedNumber)).Count();
            }
            if (count > 0)
            {
                string lastRecordArray = "";
                string lastRecordArray2 = "";
                string lastRecordArray3 = "";
                int staringNumber = 0;
                if (GeneratedNumber.Length > 5)
                {
                    lastRecordArray = GeneratedNumber.Substring(0, 9);
                    lastRecordArray2 = GeneratedNumber.Substring(10, 4);
                    lastRecordArray3 = GeneratedNumber.Substring(15, 3);

                }
                else
                {
                    lastRecordArray2 = GeneratedNumber;
                }

                if (lastRecordArray2 != null)
                {

                    if (GeneratedNumber.Length > 5)
                    {
                        string firstfield = lastRecordArray;
                        staringNumber = Convert.ToInt32(lastRecordArray2);
                        string lastfield = lastRecordArray3;
                    }
                    else
                    {
                        staringNumber = Convert.ToInt32(lastRecordArray2);
                    }
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                        }
                        else
                        {
                            GeneratedNumber = staringNumberInc.ToString();
                        }
                    }
                    else
                    {
                        GeneratedNumber = staringNumberInc.ToString();
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForProspectusNumber(GeneratedNumber, en);
            }





            //int count = 0;
            //if (en.IMN_RestartNumFlag == "Never")
            //{
            //    count = _db.prospectus.Where(imp => imp.MI_ID == en.MI_Id && imp.PASP_ProspectusNo == GeneratedNumber).Count();
            //}
            //else if (en.IMN_RestartNumFlag == "Yearly")
            //{
            //    count = _db.StudentApplication.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id && imp.PASR_RegistrationNo.Equals(GeneratedNumber)).Count();
            //}
            //if (count > 0)
            //{
            //    string lastRecordArray = "";
            //    string lastRecordArray2 = "";
            //    string lastRecordArray3 = "";
            //    int staringNumber = 0;
            //    if (GeneratedNumber.Length > 5)
            //    {
            //        lastRecordArray = GeneratedNumber.Substring(0, 9);
            //        lastRecordArray2 = GeneratedNumber.Substring(10, 4);
            //        lastRecordArray3 = GeneratedNumber.Substring(15, 3);

            //    }
            //    else
            //    {
            //        lastRecordArray2 = GeneratedNumber;
            //    }

            //    if (lastRecordArray2 != null)
            //    {

            //        if (GeneratedNumber.Length > 5)
            //        {
            //            string firstfield = lastRecordArray;
            //            staringNumber = Convert.ToInt32(lastRecordArray2);
            //            string lastfield = lastRecordArray3;
            //        }
            //        else
            //        {
            //            staringNumber = Convert.ToInt32(lastRecordArray2);
            //        }
            //        int staringNumberInc = staringNumber + 1;

            //        if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
            //        {

            //            if (en.IMN_ZeroPrefixFlag == "Yes")
            //            {
            //                GeneratedNumber = staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
            //            }
            //            else
            //            {
            //                GeneratedNumber = staringNumberInc.ToString();
            //            }
            //        }
            //        else
            //        {
            //            GeneratedNumber = staringNumberInc.ToString();
            //        }
            //    }

            //    GeneratedNumber = checkDublicateandIncreamentForProspectusNumber(GeneratedNumber, en);
            //}

            return GeneratedNumber;
        }

        //admission adm_no
        public string checkDublicateandIncreamentForAdmissionNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.Adm_M_Student.Where(imp => imp.MI_Id == en.MI_Id && imp.AMST_AdmNo == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.Adm_M_Student.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id && imp.AMST_AdmNo == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForAdmissionNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        //Interaction Student
        public string checkDublicateandIncreamentForInteractionStudentNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.IVRM_School_Master_InteractionsDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.ISMINT_InteractionId == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.IVRM_School_Master_InteractionsDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id && imp.ISMINT_InteractionId == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForInteractionStudentNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        //====================================================Inventory Purchase Requisition 
        public string checkDublicateandIncreamentForInventoryPurchaseRequisitionNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.INV_M_PurchaseRequisitionDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.INVMPR_PRNo == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.INV_M_PurchaseRequisitionDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.INVMPR_PRNo == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForInventoryPurchaseRequisitionNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        //====================================================Inventory Purchase Indent  
        public string checkDublicateandIncreamentForInventoryPurchaseIndentNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.INV_M_PurchaseIndentDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.INVMPI_PINo == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.INV_M_PurchaseIndentDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.INVMPI_PINo == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForInventoryPurchaseIndentNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        //====================================================Inventory Purchase Quotation  
        public string checkDublicateandIncreamentForInventoryQuotationNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.INV_M_SupplierQuotationDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.INVMSQ_QuotationNo == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.INV_M_SupplierQuotationDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.INVMSQ_QuotationNo == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForInventoryQuotationNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        //====================================================Inventory Purchase Order  
        public string checkDublicateandIncreamentForInventoryPurchaseOrderNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.INV_M_PurchaseOrderDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.INVMPO_PONo == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.INV_M_PurchaseOrderDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.INVMPO_PONo == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForInventoryPurchaseOrderNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        //====================================================Inventory GRN 
        public string checkDublicateandIncreamentForInventoryGRNNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.INV_M_GRNDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.INVMGRN_GRNNo == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.INV_M_GRNDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.INVMGRN_GRNNo == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForInventoryGRNNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        //====================================================Inventory Sales 
        public string checkDublicateandIncreamentForInventorySalesNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.INV_M_SalesDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.INVMSL_SalesNo == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.INV_M_SalesDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.INVMSL_SalesNo == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForInventorySalesNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }
        //Push Notification

        public string checkDublicateandIncreamentForPushNotificationNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.IVRM_PushNotificationDMO.Where(ipn => ipn.MI_Id == en.MI_Id && ipn.IPN_No == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.IVRM_PushNotificationDMO.Where(ipn => ipn.MI_Id == en.MI_Id && ipn.ASMAY_Id == en.ASMAY_Id && ipn.IPN_No == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForPushNotificationNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }



        //admission adm_no
        public string checkDublicateandIncreamentForAdmissionRegNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.Adm_M_Student.Where(imp => imp.MI_Id == en.MI_Id && imp.AMST_RegistrationNo == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.Adm_M_Student.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id && imp.AMST_RegistrationNo == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForAdmissionRegNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        //-------------------tcno---------------------------
        public string checkDublicateandIncreamentFortcNumber(string GeneratedNumber, Master_NumberingDTO en)
        {

            var checkinstution = _db.Institute.Where(a => a.MI_Id == en.MI_Id).ToList();
            // TC NO FOR COLLEGE
            if (checkinstution.FirstOrDefault().MI_SchoolCollegeFlag == "C")
            {
                int count = 0;
                if (en.IMN_RestartNumFlag == "Never")
                {
                    count = _ClgContext.CollegeStudenttctransactionDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.ACSTC_TCNO == GeneratedNumber).Count();
                }
                else if (en.IMN_RestartNumFlag == "Yearly")
                {
                    count = _ClgContext.CollegeStudenttctransactionDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.ACSTC_TCNO == GeneratedNumber).Count();
                }
                if (count > 0)
                {
                    string[] lastRecordArray = GeneratedNumber.Split('/');
                    if (lastRecordArray != null)
                    {
                        string firstfield = lastRecordArray.ElementAt(0);
                        int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                        string lastfield = lastRecordArray.ElementAt(2);
                        int staringNumberInc = staringNumber + 1;

                        if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                        {

                            if (en.IMN_ZeroPrefixFlag == "Yes")
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                            }
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }

                    GeneratedNumber = checkDublicateandIncreamentFortcNumber(GeneratedNumber, en);
                }
            }
            // TC NO FOR SCHOOL
            else
            {
                int count = 0;
                if (en.IMN_RestartNumFlag == "Never")
                {
                    count = _db.Student_TC.Where(imp => imp.MI_Id == en.MI_Id && imp.ASTC_TCNO == GeneratedNumber).Count();
                }
                else if (en.IMN_RestartNumFlag == "Yearly")
                {
                    count = _db.Student_TC.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id && imp.ASTC_TCNO == GeneratedNumber).Count();
                }
                if (count > 0)
                {
                    string[] lastRecordArray = GeneratedNumber.Split('/');
                    if (lastRecordArray != null)
                    {
                        string firstfield = lastRecordArray.ElementAt(0);
                        int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                        string lastfield = lastRecordArray.ElementAt(2);
                        int staringNumberInc = staringNumber + 1;

                        if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                        {

                            if (en.IMN_ZeroPrefixFlag == "Yes")
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                            }
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }

                    GeneratedNumber = checkDublicateandIncreamentFortcNumber(GeneratedNumber, en);
                }
            }

            return GeneratedNumber;
        }
        //Receipt Number Generation
        public string checkDublicateandIncreamentForReceiptNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.FeePaymentDetails.Where(imp => imp.MI_Id == en.MI_Id && imp.FYP_Receipt_No == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.FeePaymentDetails.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_ID == en.ASMAY_Id && imp.FYP_Receipt_No == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForReceiptNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }


        //

        //Loan
        public string checkDublicateandIncreamentForLoan(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.StudentApplication.Where(imp => imp.MI_Id == en.MI_Id && imp.PASR_RegistrationNo.Equals(GeneratedNumber)).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.StudentApplication.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id && imp.PASR_RegistrationNo.Equals(GeneratedNumber)).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string lastfield = "";
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
                    {
                        lastfield = "/" + lastRecordArray.ElementAt(2);
                    }

                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + lastfield;
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                            }

                        }
                        else
                        {
                            if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + lastfield;
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
                            }
                        }
                    }
                    else
                    {
                        if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
                        }
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForLoan(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        //TripOnlineBooking Id Generation.


        public string checkDublicateandIncreamentForTripNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            string lastfield = "";
            int staringNumberInc = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.TripOnlineBookingDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.TRTOB_BookingId == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    if (lastRecordArray.Length > 2)
                    {
                        lastfield = lastRecordArray.ElementAt(2);
                        staringNumberInc = staringNumber + 1;
                    }
                    else
                    {
                        lastfield = "";
                        staringNumberInc = staringNumber + 1;
                    }
                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            if (lastfield == "")
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                            }

                        }
                        else
                        {
                            if (lastfield == "")
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                            }

                        }
                    }
                    else
                    {
                        if (lastfield == "")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }

                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForTripNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }



        //Vehicle Service Number
        public string checkDublicateandIncreamentForVehicleserviceNumberingNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            string lastfield = "";
            int staringNumberInc = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.TR_ServiceDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.TRSE_ServiceRefNo.Equals(GeneratedNumber)).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    if (lastRecordArray.Length > 2)
                    {
                        lastfield = lastRecordArray.ElementAt(2);
                        staringNumberInc = staringNumber + 1;
                    }
                    else
                    {
                        lastfield = "";
                        staringNumberInc = staringNumber + 1;
                    }
                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            if (lastfield == "")
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                            }

                        }
                        else
                        {
                            if (lastfield == "")
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                            }

                        }
                    }
                    else
                    {
                        if (lastfield == "")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }

                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForTripNumberingNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }



        //Trip Id Generation.
        public string checkDublicateandIncreamentForTripNumberingNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            string lastfield = "";
            int staringNumberInc = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.TripDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.TRVD_TripId.Equals(GeneratedNumber)).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    if (lastRecordArray.Length > 2)
                    {
                        lastfield = lastRecordArray.ElementAt(2);
                        staringNumberInc = staringNumber + 1;
                    }
                    else
                    {
                        lastfield = "";
                        staringNumberInc = staringNumber + 1;
                    }
                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            if (lastfield == "")
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                            }

                        }
                        else
                        {
                            if (lastfield == "")
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                            }

                        }
                    }
                    else
                    {
                        if (lastfield == "")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }

                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForTripNumberingNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        //Trip Bill No Generation.
        public string checkDublicateandIncreamentForTripBillNumberingNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            string lastfield = "";
            int staringNumberInc = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.TripDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.TRTP_BillNo.Equals(GeneratedNumber)).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    if (lastRecordArray.Length > 2)
                    {
                        lastfield = lastRecordArray.ElementAt(2);
                        staringNumberInc = staringNumber + 1;
                    }
                    else
                    {
                        lastfield = "";
                        staringNumberInc = staringNumber + 1;
                    }



                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            if (lastfield == "")
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                            }

                        }
                        else
                        {
                            if (lastfield == "")
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                            }

                        }
                    }
                    else
                    {
                        if (lastfield == "")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }

                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForTripBillNumberingNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        //Trip Receipt No Generation.
        public string checkDublicateandIncreamentForTripReceiptNumberingNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            string lastfield = "";
            int staringNumberInc = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.TR_Trip_PaymentDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.TRTPP_ReceiptNo.Equals(GeneratedNumber)).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    if (lastRecordArray.Length > 2)
                    {
                        lastfield = lastRecordArray.ElementAt(2);
                        staringNumberInc = staringNumber + 1;
                    }
                    else
                    {
                        lastfield = "";
                        staringNumberInc = staringNumber + 1;
                    }
                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            if (lastfield == "")
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                            }

                        }
                        else
                        {
                            if (lastfield == "")
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                            }

                        }
                    }
                    else
                    {
                        if (lastfield == "")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }

                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForTripReceiptNumberingNumber(GeneratedNumber, en);
            }
            return GeneratedNumber;
        }
        //Leave Numbering.
        public string checkDublicateandIncreamentForLeave(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.HR_Emp_Leave_ApplicationDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.HRELAP_ApplicationID == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForLeave(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }
        public string checkDublicateandIncreamentForClgAdmissionNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.Adm_Master_College_StudentDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.AMCST_AdmNo.Equals(GeneratedNumber)).Select(d => d.AMCST_Id).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.Adm_Master_College_StudentDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id && imp.AMCST_AdmNo.Equals(GeneratedNumber)).Select(d => d.AMCST_Id).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForClgAdmissionNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }
        public string checkDublicateandIncreamentForClgAdmissionRegNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.Adm_Master_College_StudentDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.AMCST_RegistrationNo.Equals(GeneratedNumber)).Select(s => s.AMCST_Id).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.Adm_Master_College_StudentDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id && imp.AMCST_RegistrationNo == GeneratedNumber).Select(s => s.AMCST_Id).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForClgAdmissionRegNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }



        //Visitor Management
        public string checkDublicateandIncreamentForVMInwardNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.FO_Inward_DMO.Where(ipn => ipn.MI_Id == en.MI_Id && ipn.FOIN_InwardNo == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.IVRM_PushNotificationDMO.Where(ipn => ipn.MI_Id == en.MI_Id && ipn.ASMAY_Id == en.ASMAY_Id && ipn.IPN_No == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForVMInwardNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        public string checkDublicateandIncreamentForVMOutwardNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.FO_Outward_DMO.Where(ipn => ipn.MI_Id == en.MI_Id && ipn.FOOUT_OutwardNo == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.IVRM_PushNotificationDMO.Where(ipn => ipn.MI_Id == en.MI_Id && ipn.ASMAY_Id == en.ASMAY_Id && ipn.IPN_No == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForVMOutwardNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        public string checkDublicateandIncreamentForStudentGatePassNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.Gate_Pass_Student_DMO.Where(vm => vm.MI_Id == en.MI_Id && vm.GPHS_GatePassNo == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.Gate_Pass_Student_DMO.Where(ipn => ipn.MI_Id == en.MI_Id && ipn.GPHS_GatePassNo == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForStudentGatePassNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        public string checkDublicateandIncreamentForStaffGatePassNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.Gate_Pass_Staff_DMO.Where(vm => vm.MI_Id == en.MI_Id && vm.GPHST_GatePassNo == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.Gate_Pass_Staff_DMO.Where(ipn => ipn.MI_Id == en.MI_Id && ipn.GPHST_GatePassNo == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForStaffGatePassNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }


        public string checkDublicateandIncreamentForSubscriptionNumberInLIB(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.LIB_Master_Subscription_DMO.Where(vm => vm.MI_Id == en.MI_Id && vm.LMSU_SubscriptionNo == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.IVRM_PushNotificationDMO.Where(ipn => ipn.MI_Id == en.MI_Id && ipn.ASMAY_Id == en.ASMAY_Id && ipn.IPN_No == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForSubscriptionNumberInLIB(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        //==================== For Student Secred Code Generation
        public string checkDublicateandIncreamentForStudentGatePassSecredCode(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _db.Adm_M_Student.Where(imp => imp.MI_Id == en.MI_Id && imp.AMST_SecretCode == GeneratedNumber).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _db.Adm_M_Student.Where(imp => imp.MI_Id == en.MI_Id && imp.ASMAY_Id == en.ASMAY_Id && imp.AMST_SecretCode == GeneratedNumber).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    string lastfield = lastRecordArray.ElementAt(2);
                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + "/" + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }
                    else
                    {
                        GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForStudentGatePassSecredCode(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        //PDA

        public string TransactionNumberingPDA(Master_NumberingDTO en)
        {

            string GeneratedNumber = "";

            if (en != null)
            {
                if (en.IMN_AutoManualFlag == "Manual")
                {
                    GeneratedNumber = "";
                }
                else if (en.IMN_AutoManualFlag == "Auto")
                {
                    var acyr = _db.AcademicYear.Where(t => t.ASMAY_Id.Equals(en.ASMAY_Id)).FirstOrDefault();
                    string AcadYear = acyr.ASMAY_Year;
                    //string AcadYear = acyr.ASMAY_AcademicYearCode;
                    // string[] a = AcadYear.Split('-');

                    // AcadYear = a.ElementAt(0);

                    if (en.IMN_PrefixAcadYearCode == true)
                    {
                        GeneratedNumber = AcadYear;
                    }

                    else
                    if (en.IMN_PrefixParticular != "" && en.IMN_PrefixParticular != null)
                    {
                        GeneratedNumber = en.IMN_PrefixParticular;
                    }

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            if (en.IMN_StartingNo != "" && en.IMN_StartingNo != null)
                            {
                                if (GeneratedNumber != "")
                                {
                                    GeneratedNumber = GeneratedNumber + "/" + en.IMN_StartingNo.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                                }
                                else
                                {
                                    GeneratedNumber = en.IMN_StartingNo.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                                }

                            }
                            else
                            {
                                en.IMN_StartingNo = "0";
                                if (GeneratedNumber != "")
                                {
                                    GeneratedNumber = GeneratedNumber + "/" + en.IMN_StartingNo.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                                }
                                else
                                {
                                    GeneratedNumber = en.IMN_StartingNo.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                                }
                            }
                        }
                        else if (en.IMN_ZeroPrefixFlag == "Numeric")
                        {
                            if (en.IMN_StartingNo != "" && en.IMN_StartingNo != null)
                            {
                                GeneratedNumber = GeneratedNumber + en.IMN_StartingNo.ToString();
                            }
                        }
                        else
                        {
                            if (en.IMN_StartingNo != "" && en.IMN_StartingNo != null)
                            {
                                if (GeneratedNumber != "")
                                {
                                    GeneratedNumber = GeneratedNumber + "/" + en.IMN_StartingNo.ToString();
                                }
                                else
                                {
                                    GeneratedNumber = en.IMN_StartingNo.ToString();
                                }
                            }
                        }


                    }
                    else
                    {
                        if (en.IMN_StartingNo != "" && en.IMN_StartingNo != null)
                        {
                            if (GeneratedNumber != "")
                            {
                                GeneratedNumber = GeneratedNumber + "/" + en.IMN_StartingNo.ToString();
                            }
                            else
                            {
                                GeneratedNumber = en.IMN_StartingNo.ToString();
                            }
                        }
                    }

                    if (en.IMN_SuffixAcadYearCode == true)
                    {
                        if (GeneratedNumber != "")
                        {
                            GeneratedNumber = GeneratedNumber + "/" + AcadYear;
                        }
                        else
                        {
                            GeneratedNumber = AcadYear;
                        }
                    }
                    else if (en.IMN_SuffixParticular != "" && en.IMN_SuffixParticular != null)
                    {
                        if (GeneratedNumber != "")
                        {
                            GeneratedNumber = GeneratedNumber + "/" + en.IMN_SuffixParticular;
                        }
                        else
                        {
                            GeneratedNumber = en.IMN_SuffixParticular;
                        }
                    }

                    GeneratedNumber = TransactionNumberingType(GeneratedNumber, en);
                }


                else if (en.IMN_AutoManualFlag == "serial")
                {

                    if (en.IMN_ZeroPrefixFlag == "Numeric")
                    {
                        if (en.IMN_StartingNo != "" && en.IMN_StartingNo != null)
                        {
                            if (GeneratedNumber != "")
                            {
                                GeneratedNumber = en.IMN_StartingNo.ToString();
                            }
                            else
                            {
                                GeneratedNumber = GeneratedNumber + en.IMN_StartingNo.ToString();
                            }

                        }
                    }
                    GeneratedNumber = TransactionNumberingType(GeneratedNumber, en);
                }
            }

            return GeneratedNumber;

        }

        public string TransactionNumberingFA(Voucher_NumberingDTO en, string AcadYear)
        {

            string GeneratedNumber = "";

            if (en != null)
            {
                if (en.IVN_AutoManualFlag == "Manual")
                {
                    GeneratedNumber = "";
                }
                else if (en.IVN_AutoManualFlag == "Auto")
                {



                    if (en.IVN_PrefixFinYearCode == false)
                    {
                        GeneratedNumber = AcadYear;
                    }

                    else
                    if (en.IVN_PrefixParticular != "" && en.IVN_PrefixParticular != null)
                    {
                        GeneratedNumber = en.IVN_PrefixParticular;
                    }

                    if (en.IVN_WidthNumeric != "" && en.IVN_WidthNumeric != null)
                    {

                        if (en.IVN_StartingNo == "Yes")
                        {
                            if (en.IVN_StartingNo != "" && en.IVN_StartingNo != null)
                            {
                                if (GeneratedNumber != "")
                                {
                                    GeneratedNumber = GeneratedNumber + "/" + en.IVN_StartingNo.ToString().PadLeft(Convert.ToInt32(en.IVN_WidthNumeric) - 0, '0');
                                }
                                else
                                {
                                    GeneratedNumber = en.IVN_StartingNo.ToString().PadLeft(Convert.ToInt32(en.IVN_WidthNumeric) - 0, '0');
                                }

                            }
                            else
                            {
                                en.IVN_StartingNo = "0";
                                if (GeneratedNumber != "")
                                {
                                    GeneratedNumber = GeneratedNumber + "/" + en.IVN_StartingNo.ToString().PadLeft(Convert.ToInt32(en.IVN_WidthNumeric) - 0, '0');
                                }
                                else
                                {
                                    GeneratedNumber = en.IVN_StartingNo.ToString().PadLeft(Convert.ToInt32(en.IVN_WidthNumeric) - 0, '0');
                                }
                            }
                        }
                        else if (en.IVN_StartingNo == "Numeric")
                        {
                            if (en.IVN_StartingNo != "" && en.IVN_StartingNo != null)
                            {
                                GeneratedNumber = GeneratedNumber + en.IVN_StartingNo.ToString();
                            }
                        }
                        else
                        {
                            if (en.IVN_StartingNo != "" && en.IVN_StartingNo != null)
                            {
                                if (GeneratedNumber != "")
                                {
                                    GeneratedNumber = GeneratedNumber + "/" + en.IVN_StartingNo.ToString();
                                }
                                else
                                {
                                    GeneratedNumber = en.IVN_StartingNo.ToString();
                                }
                            }
                        }


                    }
                    else
                    {
                        if (en.IVN_StartingNo != "" && en.IVN_StartingNo != null)
                        {
                            if (GeneratedNumber != "")
                            {
                                GeneratedNumber = GeneratedNumber + "/" + en.IVN_StartingNo.ToString();
                            }
                            else
                            {
                                GeneratedNumber = en.IVN_StartingNo.ToString();
                            }
                        }
                    }

                    if (en.IVN_SuffixFinYearCode == true)
                    {
                        if (GeneratedNumber != "")
                        {
                            GeneratedNumber = GeneratedNumber + "/" + AcadYear;
                        }
                        else
                        {
                            GeneratedNumber = AcadYear;
                        }
                    }
                    else if (en.IVN_SuffixParticular != "" && en.IVN_SuffixParticular != null)
                    {
                        if (GeneratedNumber != "")
                        {
                            GeneratedNumber = GeneratedNumber + "/" + en.IVN_SuffixParticular;
                        }
                        else
                        {
                            GeneratedNumber = en.IVN_SuffixParticular;
                        }
                    }

                    GeneratedNumber = TransactionNumberingTypeFA(GeneratedNumber, en);
                }


                else if (en.IVN_AutoManualFlag == "serial")
                {

                    if (en.IVN_ZeroPrefixFlag == "Numeric")
                    {
                        if (en.IVN_StartingNo != "" && en.IVN_StartingNo != null)
                        {
                            if (GeneratedNumber != "")
                            {
                                GeneratedNumber = en.IVN_StartingNo.ToString();
                            }
                            else
                            {
                                GeneratedNumber = GeneratedNumber + en.IVN_StartingNo.ToString();
                            }

                        }
                    }
                    GeneratedNumber = TransactionNumberingTypeFA(GeneratedNumber, en);
                }
            }

            return GeneratedNumber;

        }


        public string TransactionNumberingTypeFA(string GeneratedNumber, Voucher_NumberingDTO en)
        {

            if (en.IVN_VoucherName == "Payment Voucher")
            {
                GeneratedNumber = checkDublicateandIncreamentForFA(GeneratedNumber, en);
            }
            else if (en.IVN_VoucherName == "Journal Voucher")
            {
                GeneratedNumber = checkDublicateandIncreamentForFA(GeneratedNumber, en);
            }
            else if (en.IVN_VoucherName == "Receipt Voucher")
            {
                GeneratedNumber = checkDublicateandIncreamentForFA(GeneratedNumber, en);
            }
            else if (en.IVN_VoucherName == "Contra Voucher")
            {
                GeneratedNumber = checkDublicateandIncreamentForFA(GeneratedNumber, en);
            }
            return GeneratedNumber;
        }

        public string checkDublicateandIncreamentForFA(string GeneratedNumber, Voucher_NumberingDTO en)
        {

            try
            {
                int count = 0;
                if (en.IVN_RestartNumFlag == "Never")
                {
                    count = _db.FA_M_VoucherDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.FAMVOU_VoucherNo == GeneratedNumber && imp.FAMVOU_VoucherType == en.IVN_VoucherName).Count();
                }
                else if (en.IVN_RestartNumFlag == "Yearly")
                {
                    count = _db.FA_M_VoucherDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.FAMVOU_VoucherNo == GeneratedNumber && imp.FAMVOU_VoucherType == en.IVN_VoucherName).Count();
                }
                if (count > 0)
                {
                    string[] lastRecordArray = GeneratedNumber.Split('/');
                    if (lastRecordArray != null)
                    {
                        string firstfield = lastRecordArray.ElementAt(0);
                        int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                        string lastfield = lastRecordArray.ElementAt(2);
                        int staringNumberInc = staringNumber + 1;


                        var dat = _db.Fee_M_Online_TransactionDMO.Where(imp => imp.MI_Id == en.MI_Id && imp.FMOT_Trans_Id.Contains(firstfield)
                           ).OrderByDescending(imp => imp.FMOT_Id).ToList();
                        if (dat.Count() > 0)
                        {
                            string generationnumber = dat[0].FMOT_Trans_Id;
                            string[] lastRecordArray1 = generationnumber.Split('/');
                            if (lastRecordArray1 != null)
                            {
                                int staringNumber1 = Convert.ToInt32(lastRecordArray1.ElementAt(1));
                                staringNumberInc = staringNumber1 + 1;
                            }
                        }


                        if (en.IVN_WidthNumeric != "" && en.IVN_WidthNumeric != null)
                        {

                            if (en.IVN_ZeroPrefixFlag == "Yes")
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IVN_WidthNumeric) - 0, '0') + "/" + lastfield;
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                            }
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + "/" + lastfield;
                        }
                    }

                    GeneratedNumber = checkDublicateandIncreamentForFA(GeneratedNumber, en);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return GeneratedNumber;
        }



    }
}
