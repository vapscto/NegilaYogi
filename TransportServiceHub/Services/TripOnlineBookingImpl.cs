using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DomainModel.Model.com.vapstech.Transport;

namespace TransportServiceHub.Services
{
    public class TripOnlineBookingImpl : Interfaces.TripOnlineBookingInterface
    {

        public DomainModelMsSqlServerContext _db;
        public TripOnlineBookingImpl(DomainModelMsSqlServerContext db)
        {
            _db = db;
        }
        public TripOnlineBookingDTO getdata(TripOnlineBookingDTO dto)
        {
            try
            {

                var grouplist = _db.MasterHirerGroupDMO.Where(d => d.MI_Id == dto.MI_Id && d.TRHG_ActiveFlg == true).Select(d => new MasterHirer_Group_RateDTO { TRHG_Id = d.TRHG_Id, TRHG_HirerGroup = d.TRHG_HirerGroup }).ToList();
                if (grouplist.Count > 0)
                {
                    dto.hirerGroupList = grouplist.ToArray();
                }
                var hirer = _db.MasterHirerDMO.Where(d => d.MI_Id == dto.MI_Id && d.TRMH_ActiveFlg == true).Select(d => new MasterHirerDMO { TRMH_Id = d.TRMH_Id, TRMH_HirerName = d.TRMH_HirerName }).ToList();

                if (hirer.Count > 0)
                {
                    dto.hirerDrpDwn = hirer.ToArray();
                }
                var paymentMode = _db.IVRM_ModeOfPaymentDMO.Where(d => d.MI_Id == dto.MI_Id && d.IVRMMOD_ActiveFlag == true).ToList();
                if (paymentMode.Count > 0)
                {
                    dto.modeOfPaymentList = paymentMode.ToArray();
                }
                var triplist = (from m in _db.MasterHirerGroupDMO
                                from n in _db.TripOnlineBookingDMO
                                where m.TRHG_Id == n.TRHG_Id && n.MI_Id == dto.MI_Id && n.TRTOB_ActiveFlg==true
                                select new TripOnlineBookingDTO
                                {
                                    trhG_HirerGroup = m.TRHG_HirerGroup,
                                    TRTOB_Id = n.TRTOB_Id,
                                    TRTOB_Date = n.TRTOB_Date,
                                    TRTOB_BookingId = n.TRTOB_BookingId,
                                    TRTOB_BookingDate = n.TRTOB_BookingDate,
                                    TRHG_Id = n.TRHG_Id,
                                    TRTOB_HirerName = n.TRTOB_HirerName,
                                    TRTOB_ConatctPerName = n.TRTOB_ConatctPerName,
                                    TRTOB_ContactPersonDesg = n.TRTOB_ContactPersonDesg,
                                    TRTOB_ContactNo = n.TRTOB_ContactNo,
                                    TRTOB_MobileNo = n.TRTOB_MobileNo,
                                    TRTOB_EmailId = n.TRTOB_EmailId,
                                    TRTOB_Address = n.TRTOB_Address,
                                    TRTOB_PickUpLocation = n.TRTOB_PickUpLocation,
                                    TRTOB_TripAddress = n.TRTOB_TripAddress,
                                    TRTOB_TripFromDate = n.TRTOB_TripFromDate,
                                    TRTOB_TripToDate = n.TRTOB_TripToDate,
                                    TRTOB_FromTime = n.TRTOB_FromTime,
                                    TRTOB_ToTime = n.TRTOB_ToTime,
                                    TRTOB_TripStatus = n.TRTOB_TripStatus,
                                    TRTOB_ActiveFlg = n.TRTOB_ActiveFlg,
                                    TRTOB_TripPurpose=n.TRTOB_TripPurpose,
                                    TRTOB_NoOfTravellers=n.TRTOB_NoOfTravellers,
                                    TRTOB_DropLocation = n.TRTOB_DropLocation,
                                }
                             ).ToList();

                if (triplist.Count > 0)
                {
                    dto.tripOnlineBookingList = triplist.OrderByDescending(t=>t.TRTOB_Id).ToArray();
                    dto.count = triplist.Count;
                }
                else
                {
                    dto.count = 0;
                }
                //Booking Id No. generation code starts here.
                //var transnumconfigsettings = _db.Master_Numbering.Where(d => d.MI_Id == dto.MI_Id && d.IMN_Flag.Equals("TripOnlineBooking")).ToList();
                //if (transnumconfigsettings.Count > 0)
                //{
                //    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                //    Master_NumberingDTO num = new Master_NumberingDTO();
                //    num.MI_Id = dto.MI_Id;
                //    num.ASMAY_Id = dto.asmay_id;
                //    num.IMN_AutoManualFlag = transnumconfigsettings.FirstOrDefault().IMN_AutoManualFlag;
                //    num.IMN_DuplicatesFlag = transnumconfigsettings.FirstOrDefault().IMN_DuplicatesFlag;
                //    num.IMN_StartingNo = transnumconfigsettings.FirstOrDefault().IMN_StartingNo;
                //    num.IMN_WidthNumeric = transnumconfigsettings.FirstOrDefault().IMN_WidthNumeric;
                //    num.IMN_ZeroPrefixFlag = transnumconfigsettings.FirstOrDefault().IMN_ZeroPrefixFlag;
                //    num.IMN_PrefixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixAcadYearCode;
                //    num.IMN_PrefixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixFinYearCode;
                //    num.IMN_PrefixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixCalYearCode;
                //    num.IMN_PrefixParticular = transnumconfigsettings.FirstOrDefault().IMN_PrefixParticular;
                //    num.IMN_SuffixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixAcadYearCode;
                //    num.IMN_SuffixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixFinYearCode;
                //    num.IMN_SuffixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixCalYearCode;
                //    num.IMN_SuffixParticular = transnumconfigsettings.FirstOrDefault().IMN_SuffixParticular;
                //    num.IMN_RestartNumFlag = transnumconfigsettings.FirstOrDefault().IMN_RestartNumFlag;
                //    num.IMN_Flag = "TripOnlineBooking";
                //    dto.TRTOB_BookingId = a.GenerateNumber(num);
                //}
                //else
                //{
                //    dto.returnVal = "Please Create TripOnlineBooking Mapping in Transaction Numbering Page";
                //}

                //Booking Id No. generation code ends  here.
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public TripOnlineBookingDTO getHirer(TripOnlineBookingDTO dtos)
        {
            try
            {
                var hirer = _db.MasterHirerDMO.Where(d => d.MI_Id == dtos.MI_Id && d.TRMH_ActiveFlg == true && d.TRHG_Id==dtos.TRHG_Id).Select(d => new MasterHirerDMO { TRMH_Id = d.TRMH_Id, TRMH_HirerName = d.TRMH_HirerName }).ToList();

                if (hirer.Count > 0)
                {
                    dtos.hirerDrpDwn = hirer.ToArray();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dtos;
        }
        public TripOnlineBookingDTO getHirerDetails(TripOnlineBookingDTO dtoss)
        {
            try
            {
                var hirer = _db.MasterHirerDMO.Where(d => d.MI_Id == dtoss.MI_Id && d.TRMH_ActiveFlg == true && d.TRMH_Id == dtoss.TRMH_Id).ToList();

                if (hirer.Count > 0)
                {
                    dtoss.hirerDetails = hirer.ToArray();
                }

                var query = _db.TripOnlineBookingDMO.Where(d => d.MI_Id == dtoss.MI_Id).ToList();
                if(query.Count > 0)
                {
                    long TRTOB_Id = _db.TripOnlineBookingDMO.Where(d => d.MI_Id == dtoss.MI_Id).Max(d => d.TRTOB_Id);
                    var query1 = _db.TripOnlineBookingDMO.Where(d => d.MI_Id == dtoss.MI_Id && d.TRTOB_Id== TRTOB_Id).Select(d=>d.TRTOB_BookingId).ToList();
                    string str = query1.FirstOrDefault();
                    string[] strarr = str.Split('/');
                    int number =Convert.ToInt32(strarr[1]);
                    dtoss.TRTOB_BookingId = hirer.FirstOrDefault().TRMH_HirerName.Substring(0, 3) + "/" + (number + 1) + "/" + dtoss.TRTOB_BookingDate.Value.Date.ToString("dd-MM-yyyy");

                }
                else
                {
                    dtoss.TRTOB_BookingId = hirer.FirstOrDefault().TRMH_HirerName.Substring(0, 3) + "/" + 1 + "/" + dtoss.TRTOB_BookingDate.Value.Date.ToString("dd-MM-yyyy");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dtoss;
        }
        public TripOnlineBookingDTO setsessionvalue(TripOnlineBookingDTO obj)
        {
            try
            {
                var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(obj.MI_Id) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();
                obj.asmay_id = acd_Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return obj;
        }

        public TripOnlineBookingDTO save(TripOnlineBookingDTO dto)
        {
            try
            {
               // string receiptNo = "";
                List<TripOnlineBookingDTO> query1 = new List<TripOnlineBookingDTO>();
                if (dto.TRTOB_Id == 0)
                {
                    if (dto.TRTOB_BookingAmount > 0)
                    {

                        //Checking whether hirer has any Due Amount or ExcessAmount.
                        var query = _db.MasterHirerDMO.Where(d => d.TRMH_Id == Convert.ToInt64(dto.TRTOB_HirerName) && d.MI_Id == dto.MI_Id).ToList();
                        if (query.Count > 0)
                        {
                            query1 = _db.TR_Hirer_OB_DMO.Where(d => d.TRMH_Id == query.FirstOrDefault().TRMH_Id).Select(d => new TripOnlineBookingDTO { dueamount = d.TRHOB_DueAmount, excessamount = d.TRHOB_ExcessAmount }).ToList();
                        }
                        else
                        {
                            //Saving Hirer Details in TR_Master_Hirer.
                            //MasterHirerDMO master = new MasterHirerDMO();
                            //master.CreatedDate = DateTime.Now;
                            //master.MI_Id = dto.MI_Id;
                            //master.TRHG_Id = dto.TRHG_Id;
                            //master.TRMH_ActiveFlg = true;
                            //master.TRMH_Address = dto.TRTOB_Address;
                            //master.TRMH_ConatctPerName = dto.TRTOB_ConatctPerName;
                            //master.TRMH_ContactNo = dto.TRTOB_ContactNo;
                            //master.TRMH_ContactPersonDesg = dto.TRTOB_ContactPersonDesg;
                            //master.TRMH_EmailId = dto.TRTOB_EmailId;
                            //master.TRMH_HirerName = dto.TRTOB_HirerName;
                            //master.TRMH_MobileNo = dto.TRTOB_MobileNo;
                            //master.UpdatedDate = DateTime.Now;
                            //_db.Add(master);
                            //_db.SaveChanges();
                        }

                        //Receipt No. generation code starts here.
                        // var transnumconfigsettings = _db.Receipt_Numbering.Where(d => d.MI_Id == dto.MI_Id).ToList();
                        //  if (transnumconfigsettings.Count > 0)
                        //  {
                        //GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                        //Master_NumberingDTO num = new Master_NumberingDTO();
                        //num.MI_Id = dto.MI_Id;
                        //num.ASMAY_Id = dto.asmay_id;
                        //num.IMN_AutoManualFlag = transnumconfigsettings.FirstOrDefault().IRN_AutoManualFlag;
                        //num.IMN_DuplicatesFlag = transnumconfigsettings.FirstOrDefault().IRN_DuplicatesFlag;
                        //num.IMN_StartingNo = transnumconfigsettings.FirstOrDefault().IRN_StartingNo;
                        //num.IMN_WidthNumeric = transnumconfigsettings.FirstOrDefault().IRN_WidthNumeric;
                        //num.IMN_ZeroPrefixFlag = transnumconfigsettings.FirstOrDefault().IRN_ZeroPrefixFlag;
                        //num.IMN_PrefixAcadYearCode = transnumconfigsettings.FirstOrDefault().IRN_PrefixAcadYearCode;
                        //num.IMN_PrefixFinYearCode = transnumconfigsettings.FirstOrDefault().IRN_PrefixFinYearCode;
                        //num.IMN_PrefixCalYearCode = transnumconfigsettings.FirstOrDefault().IRN_PrefixCalYearCode;
                        //num.IMN_PrefixParticular = transnumconfigsettings.FirstOrDefault().IRN_PrefixParticular;
                        //num.IMN_SuffixAcadYearCode = transnumconfigsettings.FirstOrDefault().IRN_SuffixAcadYearCode;
                        //num.IMN_SuffixFinYearCode = transnumconfigsettings.FirstOrDefault().IRN_SuffixFinYearCode;
                        //num.IMN_SuffixCalYearCode = transnumconfigsettings.FirstOrDefault().IRN_SuffixCalYearCode;
                        //num.IMN_SuffixParticular = transnumconfigsettings.FirstOrDefault().IRN_SuffixParticular;
                        //num.IMN_RestartNumFlag = transnumconfigsettings.FirstOrDefault().IRN_RestartNumFlag;
                        //num.IMN_Flag = "TripReceiptNo";
                        //receiptNo = a.GenerateNumber(num);
                        //Receipt No. generation code ends  here.
                        // if (receiptNo != null)
                        //{
                        var mapp = Mapper.Map<TripOnlineBookingDMO>(dto);
                        mapp.CreatedDate = DateTime.Now;
                        mapp.UpdatedDate = DateTime.Now;
                        mapp.TRTOB_HirerName = query.FirstOrDefault().TRMH_HirerName;
                        mapp.TRTOB_ActiveFlg = true;
                        mapp.TRTOB_TripStatus = "Applied";
                        _db.Add(mapp);
                        var flag = _db.SaveChanges();
                        if (flag > 0)
                        {
                            dto.returnVal = "saved";
                            //TR_Trip_PaymentDMO dmo = new TR_Trip_PaymentDMO();
                            //dmo.CreatedDate = DateTime.Now;
                            //dmo.MI_Id = dto.MI_Id;
                            //dmo.TRTOB_Id = mapp.TRTOB_Id;
                            //dmo.TRTPP_ActiveFlag = true;
                            //dmo.TRTPP_ChequeDDDate = dto.TRTPP_ChequeDDDate;
                            //dmo.TRTPP_ChequeDDNo = dto.TRTPP_ChequeDDNo;
                            //dmo.TRTPP_PaidAmount = dto.TRTOB_BookingAmount;
                            //dmo.TRTPP_PaymentMode = dto.TRTOB_ModeOfPayment;
                            //dmo.TRTPP_ReceiptDate = DateTime.Now;
                            //dmo.TRTPP_ReceiptNo = receiptNo;
                            //dmo.TRTPP_ReceiptReferenceNo = "refNo" + mapp.TRTOB_Id;
                            //dmo.UpdatedDate = DateTime.Now;
                            //_db.Add(dmo);
                            //_db.SaveChanges();

                        }
                        else
                        {
                            dto.returnVal = "savingFailed";
                        }
                    }
                    // }
                    //else
                    //{
                    //    dto.returnVal = "Sorry...Something Went Wrong";
                    //}

                    // }
                    //else
                    //{
                    //    dto.returnVal = "noMapping";
                    //}
                    //  }
                    else
                    {
                        //Checking whether hirer has any Due Amount or ExcessAmount.
                        var query = _db.MasterHirerDMO.Where(d => d.TRMH_Id == Convert.ToInt64(dto.TRTOB_HirerName) && d.MI_Id == dto.MI_Id).ToList();
                        if (query.Count > 0)
                        {
                            query1 = _db.TR_Hirer_OB_DMO.Where(d => d.TRMH_Id == query.FirstOrDefault().TRMH_Id).Select(d => new TripOnlineBookingDTO { dueamount = d.TRHOB_DueAmount, excessamount = d.TRHOB_ExcessAmount }).ToList();
                        }
                        else
                        {
                            //Saving Hirer Details in TR_Master_Hirer.
                            //MasterHirerDMO master = new MasterHirerDMO();
                            //master.CreatedDate = DateTime.Now;
                            //master.MI_Id = dto.MI_Id;
                            //master.TRHG_Id = dto.TRHG_Id;
                            //master.TRMH_ActiveFlg = true;
                            //master.TRMH_Address = dto.TRTOB_Address;
                            //master.TRMH_ConatctPerName = dto.TRTOB_ConatctPerName;
                            //master.TRMH_ContactNo = dto.TRTOB_ContactNo;
                            //master.TRMH_ContactPersonDesg = dto.TRTOB_ContactPersonDesg;
                            //master.TRMH_EmailId = dto.TRTOB_EmailId;
                            //master.TRMH_HirerName = dto.TRTOB_HirerName;
                            //master.TRMH_MobileNo = dto.TRTOB_MobileNo;
                            //master.UpdatedDate = DateTime.Now;
                            //_db.Add(master);
                            //_db.SaveChanges();
                        }
                        var mapp = Mapper.Map<TripOnlineBookingDMO>(dto);
                        mapp.CreatedDate = DateTime.Now;
                        mapp.UpdatedDate = DateTime.Now;
                        mapp.TRTOB_HirerName = query.FirstOrDefault().TRMH_HirerName;
                        mapp.TRTOB_ActiveFlg = true;
                        mapp.TRTOB_TripStatus = "Applied";
                        _db.Add(mapp);
                        var flag = _db.SaveChanges();
                        if (flag > 0)
                        {
                            dto.returnVal = "saved";
                        }
                        else
                        {
                            dto.returnVal = "savingFailed";
                        }
                    }
                }
                else if(dto.TRTOB_Id > 0)
                {
                    var query = _db.MasterHirerDMO.Where(d => d.TRMH_Id == Convert.ToInt64(dto.TRTOB_HirerName) && d.MI_Id == dto.MI_Id).ToList();
                    var update = _db.TripOnlineBookingDMO.Single(d => d.TRTOB_Id == dto.TRTOB_Id);
                    update.TRTOB_BookingDate = dto.TRTOB_BookingDate;
                    update.TRHG_Id = dto.TRHG_Id;
                    update.TRTOB_Address = dto.TRTOB_Address;
                    update.TRTOB_ConatctPerName = dto.TRTOB_ConatctPerName;
                    update.TRTOB_ContactNo = dto.TRTOB_ContactNo;
                    update.TRTOB_ContactPersonDesg = dto.TRTOB_ContactPersonDesg;
                    update.TRTOB_EmailId = dto.TRTOB_EmailId;
                    update.TRTOB_FromTime = dto.TRTOB_FromTime;
                    update.TRTOB_HirerName = query.FirstOrDefault().TRMH_HirerName;
                    update.TRTOB_MobileNo = dto.TRTOB_MobileNo;
                    update.TRTOB_PickUpLocation = dto.TRTOB_PickUpLocation;
                    update.TRTOB_ToTime = dto.TRTOB_ToTime;
                    update.TRTOB_TripAddress = dto.TRTOB_TripAddress;
                    update.TRTOB_TripFromDate = dto.TRTOB_TripFromDate;
                    update.TRTOB_TripToDate = dto.TRTOB_TripToDate;
                    update.TRTOB_TripPurpose = dto.TRTOB_TripPurpose;
                    update.TRTOB_NoOfTravellers = dto.TRTOB_NoOfTravellers;
                    update.TRTOB_DropLocation = dto.TRTOB_DropLocation;
                    update.UpdatedDate = DateTime.Now;
                    _db.Update(update);
                    var flag = _db.SaveChanges();
                    if (flag > 0)
                    {
                        var savedtriplist = _db.TripDMO.Where(t => t.TRTOB_Id == dto.TRTOB_Id).ToList();
                        if (savedtriplist.Count>0)
                        {
                            var updatetrp = _db.TripDMO.Single(t => t.TRTOB_Id == dto.TRTOB_Id);
                            updatetrp.TRTP_TripAddress=
                        
                            
                            updatetrp.TRTP_FromTime = dto.TRTOB_FromTime;
                            updatetrp.TRTP_PickUpLocation = dto.TRTOB_PickUpLocation;
                            updatetrp.TRTP_ToTime = dto.TRTOB_ToTime;
                            updatetrp.TRTP_TripAddress = dto.TRTOB_TripAddress;
                            updatetrp.TRTP_TripFromDate = dto.TRTOB_TripFromDate;
                            updatetrp.TRTP_TripToDate = dto.TRTOB_TripToDate;
                            updatetrp.TRTP_NoOfTravellers = dto.TRTOB_NoOfTravellers;
                            updatetrp.TRTP_DropLocation = dto.TRTOB_DropLocation;
                            _db.Update(updatetrp);
                            var flag1 = _db.SaveChanges();
                        }

                        dto.returnVal = "updated";
                    }
                    else
                    {
                        dto.returnVal = "failedUpdate";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public TripOnlineBookingDTO edit(int id)
        {
            TripOnlineBookingDTO dto = new TripOnlineBookingDTO();
            try
            {
                dto.editDataList = (from m in _db.MasterHirerGroupDMO
                                    from n in _db.TripOnlineBookingDMO
                                    where m.TRHG_Id == n.TRHG_Id && n.TRTOB_Id == id
                                    select new TripOnlineBookingDTO
                                    {
                                        trhG_HirerGroup = m.TRHG_HirerGroup,
                                        TRTOB_Date = n.TRTOB_Date,
                                        TRTOB_BookingId = n.TRTOB_BookingId,
                                        TRTOB_BookingDate = n.TRTOB_BookingDate,
                                        TRHG_Id = n.TRHG_Id,
                                        TRTOB_HirerName = n.TRTOB_HirerName,
                                        TRTOB_ConatctPerName = n.TRTOB_ConatctPerName,
                                        TRTOB_ContactPersonDesg = n.TRTOB_ContactPersonDesg,
                                        TRTOB_ContactNo = n.TRTOB_ContactNo,
                                        TRTOB_MobileNo = n.TRTOB_MobileNo,
                                        TRTOB_EmailId = n.TRTOB_EmailId,
                                        TRTOB_Address = n.TRTOB_Address,
                                        TRTOB_PickUpLocation = n.TRTOB_PickUpLocation,
                                        TRTOB_TripAddress = n.TRTOB_TripAddress,
                                        TRTOB_TripFromDate = n.TRTOB_TripFromDate,
                                        TRTOB_TripToDate = n.TRTOB_TripToDate,
                                        TRTOB_FromTime = n.TRTOB_FromTime,
                                        TRTOB_ToTime = n.TRTOB_ToTime,
                                        TRTOB_TripStatus = n.TRTOB_TripStatus,
                                        TRTOB_ActiveFlg = n.TRTOB_ActiveFlg,
                                        TRTOB_TripPurpose = n.TRTOB_TripPurpose,
                                        TRTOB_NoOfTravellers = n.TRTOB_NoOfTravellers,
                                        TRTOB_DropLocation = n.TRTOB_DropLocation,
                                }
                            ).ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public TripOnlineBookingDTO deactvate(TripOnlineBookingDTO dto)
        {
            try
            {
                var deactivatelist = (from a in _db.TripOnlineBookingDMO
                                      from b in _db.TripDMO
                                      where a.MI_Id == b.MI_Id && a.TRTOB_Id == b.TRTOB_Id && a.TRTOB_ActiveFlg == true && b.TRTP_BillGeneratedFlag==true && a.TRTOB_Id== dto.TRTOB_Id
                                      select new TripOnlineBookingDTO {
                                          TRTOB_BookingId=a.TRTOB_BookingId
                                      }).Distinct().ToList();
                if (deactivatelist.Count == 0)
                {
                    var query = _db.TripOnlineBookingDMO.Single(d => d.TRTOB_Id == dto.TRTOB_Id);
                    _db.Remove(query);
                    var deactive = _db.SaveChanges();
                    if (deactive > 0)
                    {
                        dto.returnVal = "Record Deleted Successfully";
                    }
                    else
                    {
                        dto.returnVal = "Failed to delete record";
                       
                    }
                }
                else
                {
                    dto.returnVal = "Already Bill Genarated For this Trip";
                }

                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
    }
}
