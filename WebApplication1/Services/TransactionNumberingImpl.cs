using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class TransactionNumberingImpl : Interfaces.TransactionNumberingInterface
    {
        private static ConcurrentDictionary<string, Master_NumberingDTO> _login =
           new ConcurrentDictionary<string, Master_NumberingDTO>();

        public DomainModelMsSqlServerContext _Context;

        public TransactionNumberingImpl(DomainModelMsSqlServerContext OrganisationContext)
        {
            _Context = OrganisationContext;
        }
        public Master_NumberingDTO saveMaster_Numbering(Master_NumberingDTO TransDTO)
        {
            switch (TransDTO.IMN_Flag)
            {
                case "Receipt":
                    TransDTO = AddUppdateReceiptNumbering(TransDTO);
                    break;
               
                case "Voucher":
                    TransDTO = AddUppdateVoucherNumbering(TransDTO);
                    break;
                case "RollNumber":
                    TransDTO = AddUppdateRollNoNumbering(TransDTO);
                    break;
                default:
                    TransDTO = AddUppdateMasterNumbering(TransDTO);
                    break;
            }

            return TransDTO;
        }

        // ENQUIRY ,PROSPECTUS,REGISTRATION,PREADMISSION REISTRATION,ADMISSION,ADMISSION REGISTRATION,Transaction,Application,Loan
        public Master_NumberingDTO AddUppdateMasterNumbering(Master_NumberingDTO TransDTO)
        {
            List<Master_Numbering> allMaster_Numbering = new List<Master_Numbering>();

            Master_Numbering enq = Mapper.Map<Master_Numbering>(TransDTO);

            try
            {
                if (enq.IMN_Id > 0)
                {
                    var result = _Context.Master_Numbering.Single(t => t.IMN_Id == enq.IMN_Id);


                    //added by 02/02/2017
                    result.IMN_AutoManualFlag = enq.IMN_AutoManualFlag;
                    TransDTO.UpdatedDate = DateTime.Now;
                    TransDTO.CreatedDate = result.CreatedDate;
                    Mapper.Map(TransDTO, result);
                    _Context.SaveChanges();
                }
                else
                {

                    //added by 02/02/2017
                    enq.CreatedDate = DateTime.Now;
                    enq.UpdatedDate = DateTime.Now;
                    _Context.Add(enq);
                    _Context.SaveChanges();
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return TransDTO;
        }

        public Master_NumberingDTO AddUppdateReceiptNumbering(Master_NumberingDTO TransDTO)
        {

            //Receipt_Numbering enq = Mapper.Map<Receipt_Numbering>(TransDTO);

            Receipt_Numbering enq = new Receipt_Numbering();

            enq.IRN_Id = TransDTO.IMN_Id;

            enq.MI_Id = TransDTO.MI_Id;
            enq.IRN_TransactionName = TransDTO.IRN_TransactionName;
            enq.IRN_AutoManualFlag = TransDTO.IMN_AutoManualFlag;
            enq.IRN_DuplicatesFlag = TransDTO.IMN_DuplicatesFlag;
            enq.IRN_StartingNo = TransDTO.IMN_StartingNo;
            enq.IRN_WidthNumeric = TransDTO.IMN_WidthNumeric;
            enq.IRN_ZeroPrefixFlag = TransDTO.IMN_ZeroPrefixFlag;
            enq.IRN_PrefixAcadYearCode = TransDTO.IMN_PrefixAcadYearCode;
            enq.IRN_PrefixFinYearCode = TransDTO.IMN_PrefixFinYearCode;
            enq.IRN_PrefixCalYearCode = TransDTO.IMN_PrefixCalYearCode;
            enq.IRN_PrefixParticular = TransDTO.IMN_PrefixParticular;
            enq.IRN_SuffixAcadYearCode = TransDTO.IMN_SuffixAcadYearCode;
            enq.IRN_SuffixFinYearCode = TransDTO.IMN_SuffixFinYearCode;
            enq.IRN_SuffixCalYearCode = TransDTO.IMN_SuffixCalYearCode;
            enq.IRN_SuffixParticular = TransDTO.IMN_SuffixParticular;
            enq.IRN_RestartNumFlag = TransDTO.IMN_RestartNumFlag;
            enq.IRN_RestartAcadYear = TransDTO.IRN_RestartAcadYear;
            enq.IRN_RestartFinYear = TransDTO.IRN_RestartFinYear;
            enq.IRN_RestartcalendYear = TransDTO.IRN_RestartcalendYear;


            try
            {
                if (enq.IRN_Id > 0)
                {
                    var result = _Context.Receipt_Numbering.Single(t => t.IRN_Id == enq.IRN_Id);

                    result.MI_Id = enq.MI_Id;
                    result.IRN_TransactionName = enq.IRN_TransactionName;
                    result.IRN_AutoManualFlag = enq.IRN_AutoManualFlag;
                    result.IRN_DuplicatesFlag = enq.IRN_DuplicatesFlag;
                    result.IRN_StartingNo = enq.IRN_StartingNo;
                    result.IRN_WidthNumeric = enq.IRN_WidthNumeric;
                    result.IRN_ZeroPrefixFlag = enq.IRN_ZeroPrefixFlag;
                    result.IRN_PrefixAcadYearCode = enq.IRN_PrefixAcadYearCode;
                    result.IRN_PrefixFinYearCode = enq.IRN_PrefixFinYearCode;
                    result.IRN_PrefixCalYearCode = enq.IRN_PrefixCalYearCode;
                    result.IRN_PrefixParticular = enq.IRN_PrefixParticular;
                    result.IRN_SuffixAcadYearCode = enq.IRN_SuffixAcadYearCode;
                    result.IRN_SuffixFinYearCode = enq.IRN_SuffixFinYearCode;
                    result.IRN_SuffixCalYearCode = enq.IRN_SuffixCalYearCode;
                    result.IRN_SuffixParticular = enq.IRN_SuffixParticular;
                    result.IRN_RestartNumFlag = enq.IRN_RestartNumFlag;
                    result.IRN_RestartAcadYear = enq.IRN_RestartAcadYear;
                    result.IRN_RestartFinYear = enq.IRN_RestartFinYear;
                    result.IRN_RestartcalendYear = enq.IRN_RestartcalendYear;
                    //added by 02/02/2017

                    result.UpdatedDate = DateTime.Now;
                    _Context.Update(result);
                    _Context.SaveChanges();
                }
                else
                {

                    //added by 02/02/2017
                    enq.CreatedDate = DateTime.Now;
                    enq.UpdatedDate = DateTime.Now;
                    _Context.Add(enq);
                    _Context.SaveChanges();
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return TransDTO;
        }

        //Voucher
        public Master_NumberingDTO AddUppdateVoucherNumbering(Master_NumberingDTO TransDTO)
        {
            Voucher_Numbering enq = new Voucher_Numbering();

            enq.IVN_Id = TransDTO.IMN_Id;

            enq.MI_Id = TransDTO.MI_Id;
            enq.IVN_VoucherName = TransDTO.VoucherName;
            enq.IVN_VoucherID = TransDTO.VoucherType;
            enq.IVN_AutoManualFlag = TransDTO.IMN_AutoManualFlag;
            enq.IVN_DuplicatesFlag = TransDTO.IMN_DuplicatesFlag;
            enq.IVN_StartingNo = TransDTO.IMN_StartingNo;
            enq.IVN_WidthNumeric = TransDTO.IMN_WidthNumeric;
            enq.IVN_ZeroPrefixFlag = TransDTO.IMN_ZeroPrefixFlag;
           // enq.IVN_PrefixAcadYearCode = TransDTO.IMN_PrefixAcadYearCode;
            enq.IVN_PrefixFinYearCode = TransDTO.IMN_PrefixFinYearCode;
          //  enq.IVN_PrefixCalYearCode = TransDTO.IMN_PrefixCalYearCode;
            enq.IVN_PrefixParticular = TransDTO.IMN_PrefixParticular;
          //  enq.IVN_SuffixAcadYearCode = TransDTO.IMN_SuffixAcadYearCode;
            enq.IVN_SuffixFinYearCode = TransDTO.IMN_SuffixFinYearCode;
           // enq.IVN_SuffixCalYearCode = TransDTO.IMN_SuffixCalYearCode;
            enq.IVN_SuffixParticular = TransDTO.IMN_SuffixParticular;
            enq.IVN_RestartNumFlag = TransDTO.IMN_RestartNumFlag;
           


            try
            {
                if (enq.IVN_Id > 0)
                {
                    var result = _Context.Voucher_Numbering.Single(t => t.IVN_Id == enq.IVN_Id);

                    result.MI_Id = enq.MI_Id;
                    result.IVN_VoucherName = enq.IVN_VoucherName;
                    result.IVN_VoucherID = enq.IVN_VoucherID;
                    result.IVN_AutoManualFlag = enq.IVN_AutoManualFlag;
                    result.IVN_DuplicatesFlag = enq.IVN_DuplicatesFlag;
                    result.IVN_StartingNo = enq.IVN_StartingNo;
                    result.IVN_WidthNumeric = enq.IVN_WidthNumeric;
                    result.IVN_ZeroPrefixFlag = enq.IVN_ZeroPrefixFlag;
                   // result.IVN_PrefixAcadYearCode = enq.IVN_PrefixAcadYearCode;
                    result.IVN_PrefixFinYearCode = enq.IVN_PrefixFinYearCode;
                   // result.IVN_PrefixCalYearCode = enq.IVN_PrefixCalYearCode;
                    result.IVN_PrefixParticular = enq.IVN_PrefixParticular;
                   // result.IVN_SuffixAcadYearCode = enq.IVN_SuffixAcadYearCode;
                    result.IVN_SuffixFinYearCode = enq.IVN_SuffixFinYearCode;
                  //  result.IVN_SuffixCalYearCode = enq.IVN_SuffixCalYearCode;
                    result.IVN_SuffixParticular = enq.IVN_SuffixParticular;
                    result.IVN_RestartNumFlag = enq.IVN_RestartNumFlag;
                   
                    //added by 02/02/2017

                    result.UpdatedDate = DateTime.Now;
                    _Context.Update(result);
                    _Context.SaveChanges();
                }
                else
                {

                    //added by 02/02/2017
                    enq.CreatedDate = DateTime.Now;
                    enq.UpdatedDate = DateTime.Now;
                    _Context.Add(enq);
                    _Context.SaveChanges();
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return TransDTO;
        }

        //roll no


        public Master_NumberingDTO AddUppdateRollNoNumbering(Master_NumberingDTO TransDTO)
        {
            List<Master_Numbering> allMaster_Numbering = new List<Master_Numbering>();

            Master_Numbering enq = Mapper.Map<Master_Numbering>(TransDTO);

            try
            {
                if (enq.IMN_Id > 0)
                {
                    var result = _Context.Master_Numbering.Single(t => t.IMN_Id == enq.IMN_Id);


                    //added by 02/02/2017
                    result.IMN_AutoManualFlag = enq.IMN_AutoManualFlag;
                    TransDTO.UpdatedDate = DateTime.Now;
                    TransDTO.CreatedDate = result.CreatedDate;
                    Mapper.Map(TransDTO, result);
                    _Context.SaveChanges();
                }
                else
                {

                    //added by 02/02/2017
                    enq.CreatedDate = DateTime.Now;
                    enq.UpdatedDate = DateTime.Now;
                    _Context.Add(enq);
                    
                }

                if (TransDTO.RollNumberingconfig.Length > 0)
                {

                    foreach (var roll in TransDTO.RollNumberingconfig)
                    {

                        IVRM_Auto_RollNo_Configuration rollno = new IVRM_Auto_RollNo_Configuration();
                        if (roll.IVRMARNC_Id>0)
                        {
                            var resultobj = _Context.IVRM_Auto_RollNo_Configuration.Single(t => t.IVRMARNC_Id.Equals(roll.IVRMARNC_Id));
                            resultobj.MI_Id = TransDTO.MI_Id;
                            resultobj.IVRMARNC_Field = roll.IVRMARNC_Field;
                            resultobj.IVRMARNC_AscDscOrder = roll.IVRMARNC_AscDscOrder;
                            resultobj.IVRMARNC_Order = roll.IVRMARNC_Order;
                            resultobj.IVRMARNC_UpdatedDate = DateTime.Today;
                            resultobj.IVRMARNC_UpdatedBy = TransDTO.UserId;
                            _Context.Update(resultobj);
                            _Context.SaveChanges();
                        }
                        else
                        {
                            rollno.MI_Id = TransDTO.MI_Id;
                            rollno.IVRMARNC_Field = roll.IVRMARNC_Field;
                            rollno.IVRMARNC_AscDscOrder = roll.IVRMARNC_AscDscOrder;
                            rollno.IVRMARNC_Order = roll.IVRMARNC_Order;
                            rollno.IVRMARNC_ActiveFlag = true;
                            rollno.IVRMARNC_CreatedDate = DateTime.Today;
                            rollno.IVRMARNC_UpdatedDate = DateTime.Today;
                            rollno.IVRMARNC_CreatedBy = TransDTO.UserId;
                            rollno.IVRMARNC_UpdatedBy = TransDTO.UserId;
                            _Context.Add(rollno);
                            _Context.SaveChanges();
                        }
                        
                        
                    }
                    




                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return TransDTO;
        }


        public Master_NumberingDTO getdetails(MandatoryFieldsDTO id)
        {
            Master_NumberingDTO org = new Master_NumberingDTO();
            try
            {
                List<Master_Numbering> EnquiryNumbering = new List<Master_Numbering>();
                EnquiryNumbering = _Context.Master_Numbering.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id) && t.IMN_Flag == "Enquiry").ToList();
                org.EnquiryNumberingArraylist = EnquiryNumbering.ToArray();

                List<Master_Numbering> ProspectusNumbering = new List<Master_Numbering>();
                ProspectusNumbering = _Context.Master_Numbering.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id) && t.IMN_Flag == "Prospectus").ToList();
                org.ProspectusNumberingArraylist = ProspectusNumbering.ToArray();

                List<Master_Numbering> RegistrationNumbering = new List<Master_Numbering>();
                RegistrationNumbering = _Context.Master_Numbering.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id) && t.IMN_Flag == "Registration").ToList();
                org.RegistrationNumberingArraylist = RegistrationNumbering.ToArray();


                List<Master_Numbering> PreRegistrationNumbering = new List<Master_Numbering>();
                PreRegistrationNumbering = _Context.Master_Numbering.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id) && t.IMN_Flag == "PreRegistration").ToList();
                org.PreRegistrationNumberingArraylist = PreRegistrationNumbering.ToArray();

                List<Master_Numbering> AdmissionNumbering = new List<Master_Numbering>();
                AdmissionNumbering = _Context.Master_Numbering.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id) && t.IMN_Flag == "Admission").ToList();
                org.AdmissionNumberingArraylist = AdmissionNumbering.ToArray();


                List<Master_Numbering> AdmissionRegNumbering = new List<Master_Numbering>();
                AdmissionRegNumbering = _Context.Master_Numbering.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id) && t.IMN_Flag == "AdmissionReg").ToList();
                org.AdmissionRegNumberingArraylist = AdmissionRegNumbering.ToArray();

                List<Master_Numbering> ApplicationNumbering = new List<Master_Numbering>();
                ApplicationNumbering = _Context.Master_Numbering.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id) && t.IMN_Flag == "Application").ToList();
                org.ApplicationNumberingArraylist = ApplicationNumbering.ToArray();

                List<Master_Numbering> TransactionNumbering = new List<Master_Numbering>();
                TransactionNumbering = _Context.Master_Numbering.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id) && t.IMN_Flag == "Transaction").ToList();
                org.TransactionNumberingArraylist = TransactionNumbering.ToArray();


                List<Receipt_Numbering> ReceiptNumbering = new List<Receipt_Numbering>();
                ReceiptNumbering = _Context.Receipt_Numbering.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id)).ToList();
                org.ReceiptNumberingArraylist = ReceiptNumbering.ToArray();


                List<Voucher_Numbering> VoucherNumbering = new List<Voucher_Numbering>();
                VoucherNumbering = _Context.Voucher_Numbering.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id)).ToList();
                org.VoucherNumberingArraylist = VoucherNumbering.ToArray();

                //---------------------tc----------

                List<Master_Numbering> tcNumbering = new List<Master_Numbering>();
                tcNumbering = _Context.Master_Numbering.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id) && t.IMN_Flag == "tcno").ToList();
                org.tcNumberingArraylist = tcNumbering.ToArray();

                //-------------------------

                //---------------------Loan----------

                List<Master_Numbering> LoanNumbering = new List<Master_Numbering>();
                LoanNumbering = _Context.Master_Numbering.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id) && t.IMN_Flag == "Loan").ToList();
                org.loanNumberingArraylist = LoanNumbering.ToArray();

                //-------------------------

                //TripOnlineBooking
                List < Master_Numbering > onlineBooking = new List<Master_Numbering>();
                onlineBooking = _Context.Master_Numbering.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id) && t.IMN_Flag.Equals("TripOnlineBooking")).ToList();
                org.onlineBookingNumberingArraylist = onlineBooking.ToArray();
                
                //Trip
                List < Master_Numbering > trip = new List<Master_Numbering>();
                trip = _Context.Master_Numbering.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id) && t.IMN_Flag.Equals("TripNo")).ToList();
               org.tripNumberingArraylist = trip.ToArray();
                
                //TripBill
                List < Master_Numbering > tripBill = new List<Master_Numbering>();
                tripBill = _Context.Master_Numbering.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id) && t.IMN_Flag.Equals("TripBill")).ToList();
                org.tripBillNumberingArraylist = tripBill.ToArray();

                //Leave Numbering.
                List<Master_Numbering> LeaveNumbering = new List<Master_Numbering>();
                LeaveNumbering = _Context.Master_Numbering.AsNoTracking().Where(t => t.MI_Id == id.MI_Id && t.IMN_Flag.Equals("LeaveNo")).ToList();
                org.leaveNumberingArraylist = LeaveNumbering.ToArray();


                List<Master_Numbering> RolenoNumbering = new List<Master_Numbering>();
                RolenoNumbering = _Context.Master_Numbering.AsNoTracking().Where(t => t.MI_Id == id.MI_Id && t.IMN_Flag.Equals("RollNumber")).ToList();
                org.RolenoNumbering = RolenoNumbering.ToArray();

                if(org.RolenoNumbering!=null && org.RolenoNumbering.Length>0)
                {
                    if(RolenoNumbering.FirstOrDefault().IMN_AutoManualFlag=="Auto")
                    {
                        org.RolenoNumberingConfig = _Context.IVRM_Auto_RollNo_Configuration.Where(s => s.MI_Id == id.MI_Id).ToArray();
                    }
                    
                }
                
                //org.FieldArray = _Context.IVRM_COLOUMN.ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return org;
        }

        public Master_NumberingDTO deleteRollnoconfig(Master_NumberingDTO id)
        {
            List<IVRM_Auto_RollNo_Configuration> st = _Context.IVRM_Auto_RollNo_Configuration.Where(rg => rg.IVRMARNC_Id == id.IVRMARNC_Id).ToList();

            if (st.Count() > 0)
            {
                for (int i = 0; i < st.Count(); i++)
                {
                    _Context.Remove(st.ElementAt(i));
                    _Context.SaveChanges();
                    id.message = "Success";
                }
            }
            return id;
        }
        
    }
}
