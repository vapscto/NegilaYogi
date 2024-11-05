using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Services
{
    public class TripImpl:Interfaces.TripInterface
    {
        TransportContext _context;
        DomainModelMsSqlServerContext _db;
        public TripImpl(TransportContext context, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _db = db;
        }
      public  TripDTO getdata(TripDTO obj)
        {
            try
            {
                var grouplist = _context.MasterHirerGroupDMO.Where(d => d.MI_Id == obj.MI_Id && d.TRHG_ActiveFlg == true).Select(d => new MasterHirer_Group_RateDTO { TRHG_Id = d.TRHG_Id, TRHG_HirerGroup = d.TRHG_HirerGroup }).ToList();
                if (grouplist.Count > 0)
                {
                    obj.hirerGroupList = grouplist.ToArray();
                }
                var vehicle = _context.Master_VehicleDMO.Where(d => d.MI_Id == obj.MI_Id && d.TRMV_ActiveFlag == true).Select(d=> new MasterVehicleDTO {TRMV_Id=d.TRMV_Id,TRMV_VehicleNo=d.TRMV_VehicleNo,TRMVT_Id=d.TRMVT_Id }).ToList();
                if(vehicle.Count > 0)
                {
                    obj.vehicleList = vehicle.ToArray();
                }
                var driver = _context.MasterDriverDMO.Where(d => d.MI_Id == obj.MI_Id && d.TRMD_ActiveFlg == true).Select(d => new MasterDriverDTO { TRMD_Id = d.TRMD_Id, TRMD_DriverName = d.TRMD_DriverName, TRMD_MobileNo = d.TRMD_MobileNo }).ToList();
                             
                             
                if(driver.Count > 0)
                {
                    obj.driverList = driver.ToArray();
                }
                var trip = _context.TripDMO.Where(d => d.MI_Id == obj.MI_Id && d.TRTP_ActiveFlg == true).Select(d=>d.TRVD_TripId).
                    ToList();
                if(trip.Count > 0)
                {
                    obj.tripDrpwn = trip.ToArray();
                }
                var trip1 = _context.TripDMO.Where(d => d.MI_Id == obj.MI_Id && d.TRTP_ActiveFlg == true && d.TRTP_BillGeneratedFlag==true).Select(d => d.TRVD_TripId).
                    ToList();
                if (trip1.Count > 0)
                {
                    obj.tripDrpwn1 = trip1.ToArray();
                }

                //Praveen
                var newtripdropdown = (from a in _context.TripDMO
                                       from b in _context.TripOnlineBookingDMO
                                       where a.MI_Id == b.MI_Id && a.TRTOB_Id == b.TRTOB_Id && a.TRTP_ActiveFlg == true && b.TRTOB_ActiveFlg == true && a.MI_Id== obj.MI_Id
                                       select new TripDTO
                                       {
                                           TRVD_TripId = a.TRVD_TripId,
                                           TRTOB_BookingId = b.TRTOB_BookingId,
                                           TRTOB_Id = b.TRTOB_Id,
                                           TRTP_Id = a.TRTP_Id
                                       }).Distinct().ToList();
                if (newtripdropdown.Count>0)
                {
                    obj.tripDrpwn = newtripdropdown.OrderByDescending(t=>t.TRTP_Id).ToArray();
                }

                var newtripdropdown1 = (from a in _context.TripDMO
                                       from b in _context.TripOnlineBookingDMO
                                       where a.MI_Id == b.MI_Id && a.TRTOB_Id == b.TRTOB_Id && a.TRTP_ActiveFlg == true && b.TRTOB_ActiveFlg == true && a.TRTP_BillGeneratedFlag == true && a.MI_Id == obj.MI_Id
                                        select new TripDTO
                                       {
                                           TRVD_TripId = a.TRVD_TripId,
                                           TRTOB_BookingId = b.TRTOB_BookingId,
                                           TRTOB_Id = b.TRTOB_Id,
                                           TRTP_Id = a.TRTP_Id
                                       }).Distinct().ToList();
                if (newtripdropdown1.Count > 0)
                {
                    obj.tripDrpwn1 = newtripdropdown1.OrderByDescending(t => t.TRTP_Id).ToArray();
                }



                var hirer = _context.MasterHirerDMO.Where(d => d.MI_Id == obj.MI_Id && d.TRMH_ActiveFlg == true).Select(d => new MasterHirerDMO {TRMH_Id=d.TRMH_Id, TRMH_HirerName=d.TRMH_HirerName }).ToList();

                if (hirer.Count > 0)
                {
                    obj.hirerDrpDwn = hirer.ToArray();
                }
                var paymentMode = _db.IVRM_ModeOfPaymentDMO.Where(d => d.MI_Id == obj.MI_Id && d.IVRMMOD_ActiveFlag == true).ToList();
                if(paymentMode.Count > 0)
                {
                    obj.modeOfPaymentList = paymentMode.ToArray();
                }
                var BookingIds = _db.TripOnlineBookingDMO.Where(d => d.MI_Id == obj.MI_Id && d.TRTOB_ActiveFlg == true).ToList();
                if(BookingIds.Count > 0)
                {
                    obj.bookingIdDrpDwn = BookingIds.Distinct().OrderByDescending(t=>t.TRTOB_Id).ToArray();
                }

                obj.vehicletypelist = _context.MasterVehicleTypeDMO.Where(t => t.MI_Id == obj.MI_Id && t.TRMVT_ActiveFlg == true).ToArray();


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public TripDTO getvahicle(TripDTO data)
        {
            try
            {
                
                List<long> aa = new List<long>();
                List<long> aa1 = new List<long>();

                if (data.allottedVehicleDriver.Length > 0)
                {
                    foreach (var item in data.allottedVehicleDriver)
                    {
                        aa.Add(item.TRMV_Id);
                    }
                }

                if (data.allottedVehicleDriver.Length > 0)
                {
                    foreach (var item in data.allottedVehicleDriver)
                    {
                        aa1.Add(item.TRVD_Id);
                    }
                }

                data.vehicleList = _context.Master_VehicleDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMVT_Id == data.TRMVT_Id && t.TRMV_ActiveFlag == true && !aa.Contains(t.TRMV_Id)).ToArray();

                var driverList = _context.MasterDriverDMO.Where(d => d.MI_Id == data.MI_Id && d.TRMD_ActiveFlg == true && ! aa1.Contains(d.TRMD_Id)).Select(d => new MasterDriverDTO { TRMD_Id = d.TRMD_Id, TRMD_DriverName = d.TRMD_DriverName, TRMD_MobileNo = d.TRMD_MobileNo }).ToList();
                if (driverList.Count > 0)
                {
                    data.driverList = driverList.ToArray();
                }


            }
            catch (Exception)
            {

                throw;
            }

            return data;
        }


      public  TripDTO SearchByBookingId(TripDTO data)
        {
            try
            {
                var finddata = (from n in _context.TripOnlineBookingDMO
                                from m in _context.MasterHirerGroupDMO
                                where m.MI_Id == n.MI_Id && n.MI_Id == data.MI_Id && n.TRTOB_ActiveFlg == true && n.TRTOB_BookingId.Equals(data.TRTOB_BookingId)
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
                                    TRTOB_BookingAmount=n.TRTOB_BookingAmount,
                                    TRTOB_NoOfTravellers=n.TRTOB_NoOfTravellers,
                                    TRTOB_DropLocation=n.TRTOB_DropLocation,
                                }
                                ).ToList();
                if(finddata.Count > 0)
                {
                    data.tripOnlineBookingDetails = finddata.ToArray();
                    data.count = finddata.Count;
                    var trtob_id = finddata.FirstOrDefault().TRTOB_Id;

                    var query = _context.TripDMO.Where(d => d.TRTOB_Id == trtob_id).ToList();

                    if(query.Count > 0)
                    {
                        data.TRTP_Id = query.FirstOrDefault().TRTP_Id;
                        var vhcleDvrAllottment = (from m in _context.TripOnlineBookingDMO
                                                  from n in _context.TripDMO
                                                  from o in _context.TRVehicleDriverAllottmentDMO
                                                  from p in _context.MasterDriverDMO
                                                  where m.TRTOB_Id == n.TRTOB_Id && n.TRTP_Id == o.TRTP_Id && m.TRTOB_ActiveFlg == true && n.TRTP_ActiveFlg == true && n.MI_Id == data.MI_Id && n.TRTOB_Id == trtob_id &&  p.MI_Id==m.MI_Id && o.TRVD_Id==p.TRMD_Id && p.TRMD_ActiveFlg==true
                                                  select new TripDTO
                                                  {
                                                      TRTP_Id = n.TRTP_Id,
                                                      TRMV_Id = o.TRMV_Id,
                                                      TRVD_Id = o.TRVD_Id,
                                                      TRTP_OpeningKM = Convert.ToInt64(o.TRTP_OpeningKM),
                                                      TRTP_ClosingKM = Convert.ToInt64(o.TRTP_ClosingKM),
                                                      TRMD_MobileNo=p.TRMD_MobileNo

                                                  }).ToList();
                        if (vhcleDvrAllottment.Count > 0)
                        {
                            data.vehicleDriverAllottmentList = vhcleDvrAllottment.ToArray();
                        }
                    }



                    data.tripList = (from m in _context.TripDMO
                                     from n in _context.MasterHirerGroupDMO
                                     from r in _context.TripOnlineBookingDMO
                                     where m.TRTOB_Id == r.TRTOB_Id && m.TRHG_Id == n.TRHG_Id  && m.MI_Id == data.MI_Id && m.TRTP_ActiveFlg == true && m.TRTOB_Id == trtob_id
                                     select new TripDTO
                                     {
                                         TRTOB_Id = m.TRTOB_Id,
                                         TRTP_Id = m.TRTP_Id,
                                         TRTP_BillGeneratedFlag = m.TRTP_BillGeneratedFlag,
                                         TRVD_TripId = m.TRVD_TripId,
                                         TRTP_HirerName = m.TRTP_HirerName,
                                         TRTP_TotalReceivable = m.TRTP_TotalReceivable,
                                         TRTP_TripAddress=m.TRTP_TripAddress,
                                         TRTP_NoOfTravellers=m.TRTP_NoOfTravellers
                                     }
                                   ).ToArray();
                }
                else
                {
                    data.count = 0;
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
      public  TripDTO save(TripDTO dto)
        {
            try
            {
               // string receiptNo = "";
                if (dto.TRTP_Id == 0)
                {

                    var updatedatelist = _context.TripOnlineBookingDMO.Where(t => t.MI_Id == dto.MI_Id && t.TRTOB_Id == dto.TRTOB_Id).ToList();
                    if (updatedatelist.Count > 0)
                    {
                        var updatedate = _context.TripOnlineBookingDMO.Single(t => t.MI_Id == dto.MI_Id && t.TRTOB_Id == dto.TRTOB_Id);
                        updatedate.TRTOB_BookingDate = dto.TRTP_BookingDate;
                        updatedate.TRTOB_TripFromDate = dto.TRTP_TripFromDate;
                        updatedate.TRTOB_TripToDate = dto.TRTP_TripToDate;
                        updatedate.TRTOB_FromTime = dto.TRTP_FromTime;
                        updatedate.TRTOB_ToTime = dto.TRTP_ToTime;
                        updatedate.TRTOB_NoOfTravellers= dto.TRTP_NoOfTravellers;
                        updatedate.TRTOB_ContactNo = dto.TRTP_HirerContactNo;
                        _context.Update(updatedate);
                        _context.SaveChanges();

                    }




                    var mapp = AutoMapper.Mapper.Map<TripDMO>(dto);
                    mapp.CreatedDate = DateTime.Now;
                    mapp.UpdatedDate = DateTime.Now;
                    mapp.TRTP_ActiveFlg = true;

                    //Trip Id No. generation code starts here.
                    var transnumconfigsettings = _db.Master_Numbering.Where(d => d.MI_Id == dto.MI_Id && d.IMN_Flag.Equals("TripNo")).ToList();
                    if (transnumconfigsettings.Count > 0)
                    {
                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                        Master_NumberingDTO num = new Master_NumberingDTO();
                        num.MI_Id = dto.MI_Id;
                        num.ASMAY_Id = dto.asmay_id;
                        num.IMN_AutoManualFlag = transnumconfigsettings.FirstOrDefault().IMN_AutoManualFlag;
                        num.IMN_DuplicatesFlag = transnumconfigsettings.FirstOrDefault().IMN_DuplicatesFlag;
                        num.IMN_StartingNo = transnumconfigsettings.FirstOrDefault().IMN_StartingNo;
                        num.IMN_WidthNumeric = transnumconfigsettings.FirstOrDefault().IMN_WidthNumeric;
                        num.IMN_ZeroPrefixFlag = transnumconfigsettings.FirstOrDefault().IMN_ZeroPrefixFlag;
                        num.IMN_PrefixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixAcadYearCode;
                        num.IMN_PrefixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixFinYearCode;
                        num.IMN_PrefixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixCalYearCode;
                        num.IMN_PrefixParticular = transnumconfigsettings.FirstOrDefault().IMN_PrefixParticular;
                        num.IMN_SuffixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixAcadYearCode;
                        num.IMN_SuffixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixFinYearCode;
                        num.IMN_SuffixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixCalYearCode;
                        num.IMN_SuffixParticular = transnumconfigsettings.FirstOrDefault().IMN_SuffixParticular;
                        num.IMN_RestartNumFlag = transnumconfigsettings.FirstOrDefault().IMN_RestartNumFlag;
                        num.IMN_Flag = "TripNo";
                        dto.TRVD_TripId = a.GenerateNumber(num);
                    }
                    else
                    {
                        dto.returnVal = "Nomapping";
                    }
                    //Trip Id No. generation code ends  here.
                    if (dto.TRVD_TripId != null)
                    {
                        mapp.TRVD_TripId = dto.TRVD_TripId;
                        _context.Add(mapp);
                        if (dto.allottedVehicleDriver.Length > 0)
                        {
                            foreach(TripDTO item in dto.allottedVehicleDriver)
                            {

                                TRVehicleDriverAllottmentDMO dmo = new TRVehicleDriverAllottmentDMO();
                                dmo.CreatedDate = DateTime.Now;
                                dmo.TRTP_Id = mapp.TRTP_Id;
                                dmo.TRMV_Id = item.TRMV_Id;
                                dmo.TRTP_ClosingKM = item.TRTP_ClosingKM;
                                dmo.TRTP_OpeningKM = item.TRTP_OpeningKM;
                                dmo.TRVD_Id = item.TRVD_Id;
                                dmo.UpdatedDate = DateTime.Now;
                                _context.Add(dmo);

                                //Update the driver mobile no
                                var drivermobile = _context.MasterDriverDMO.Single(t => t.MI_Id == dto.MI_Id && t.TRMD_Id == item.TRVD_Id && t.TRMD_ActiveFlg == true);
                                if (item.TRMD_MobileNo!=0 && item.TRMD_MobileNo !=null)
                                {
                                    drivermobile.TRMD_MobileNo = item.TRMD_MobileNo;
                                    _context.Update(drivermobile);
                                }
                                

                            }
                        }
                        var flag = _context.SaveChanges();
                        if (flag > 0)
                        {
                            string m = string.Empty;

                            string s = string.Empty;
                            string s1 = string.Empty;
                            string e1 = string.Empty;
                            SMS sms = new SMS(_db);
                            Email Email = new Email(_db);

                           

                            dto.returnVal = "saved";
                            var hirer_Det = _context.TripOnlineBookingDMO.Single(d => d.MI_Id == dto.MI_Id && d.TRTOB_Id == dto.TRTOB_Id);
                            //  hirer_Det.TRTOB_ContactNo = 9591081840;

                            if (hirer_Det.TRTOB_ContactNo>0)
                            {
                                s1 = sms.sendsmsforhirer(dto.MI_Id, hirer_Det.TRTOB_ContactNo, "TRIP_HIRER", 0, dto.TRTOB_Id, 0).Result;
                            }

                            if (hirer_Det.TRTOB_EmailId !=null && hirer_Det.TRTOB_EmailId !="")
                            {
                                e1 = Email.sendEmailforhirer(dto.MI_Id, hirer_Det.TRTOB_EmailId, "TRIP_HIRER", 0, dto.TRTOB_Id, 0);
                            }
                            //hirer_Det.TRTOB_EmailId = "praveenishwar@vapstech.com";
                           

                                
                            //    (dto.MI_Id, hirer_Det.TRTOB_ContactNo, "TRIP_HIRER", hirer_Det.TRTOB_Id, driverdetals).Result;

                          //  var institutionName = _db.Institution.Single(i => i.MI_Id == dto.MI_Id);

                          //  string driverdetals = "Dear" + " " + hirer_Det.TRTOB_HirerName +","+ Environment.NewLine;
                        //    string emaildriverdetals = "Dear" + " " + hirer_Det.TRTOB_HirerName + ",";

                         //   driverdetals = driverdetals + "The Bus Hire conformation details as per your request";
                       //     emaildriverdetals = emaildriverdetals + "<br />" + "The Bus Hire conformation details as per your request";


                            if (dto.allottedVehicleDriver.Length > 0)
                            {
                               
                               /// driverdetals = driverdetals + Environment.NewLine + "a." + "  " + "Trip Date: " + hirer_Det.TRTOB_TripFromDate;
                             //   emaildriverdetals = emaildriverdetals + "<br />" + "a." + "  " + "Trip Date: " + hirer_Det.TRTOB_TripFromDate;


                                //driverdetals = driverdetals + Environment.NewLine + "b." + "  " + "Place: " + hirer_Det.TRTOB_TripAddress;

                                //emaildriverdetals = emaildriverdetals + "<br />" + "b." + "  " + "Place: " + hirer_Det.TRTOB_TripAddress;

                                //driverdetals = driverdetals + Environment.NewLine + "c." + "  " + "Timing: " + hirer_Det.TRTOB_FromTime + Environment.NewLine;

                                //emaildriverdetals = emaildriverdetals + "<br />" + "c." + "  " + "Timing: " + hirer_Det.TRTOB_FromTime;
                                foreach (TripDTO item in dto.allottedVehicleDriver)
                                {
                                    //string drivermsg = string.Empty;
                                    //string driveremail = string.Empty;

                                   var drivename = _context.MasterDriverDMO.Single(t => t.MI_Id == dto.MI_Id && t.TRMD_Id == item.TRVD_Id);
                                    //var vahicleno = _context.Master_VehicleDMO.Single(t => t.MI_Id == dto.MI_Id && t.TRMV_Id == item.TRMV_Id);




                                    //driverdetals = driverdetals +"VEHICLE NO :"+ vahicleno.TRMV_VehicleNo +", " +"DRIVER NAME:"+ drivename.TRMD_DriverName + ", " + "DRIVER NO:" + item.TRMD_MobileNo + Environment.NewLine ;

                                    //emaildriverdetals = emaildriverdetals + "<br />" + "VEHICLE NO :" + vahicleno.TRMV_VehicleNo + ", " + "DRIVER NAME:" + drivename.TRMD_DriverName + ", " + "DRIVER NO:" + item.TRMD_MobileNo;

                                    ////msg to driver

                                    //drivermsg ="Dear" +"  "+ drivename.TRMD_DriverName +"," + Environment.NewLine;
                                    //driveremail = "Dear" + "  " + drivename.TRMD_DriverName + "," + "<br />";

                                    //drivermsg = drivermsg + "Trip Details" + Environment.NewLine;
                                    //driveremail = driveremail + "Trip Details";

                                    //drivermsg = drivermsg +  "Booked By :" +" " + hirer_Det.TRTOB_HirerName + Environment.NewLine;
                                    //driveremail = driveremail + "<br />" + "Booked By :" + " " + hirer_Det.TRTOB_HirerName ;


                                    //drivermsg = drivermsg + "Trip Date :" + " " + hirer_Det.TRTOB_TripFromDate + Environment.NewLine;
                                    //driveremail = driveremail + "<br />" + "Trip Date :" + " " + hirer_Det.TRTOB_TripFromDate; 


                                    //drivermsg = drivermsg + Environment.NewLine + "Place: " + hirer_Det.TRTOB_TripAddress;


                                    //driveremail = driveremail + "<br />" + "Place: " + hirer_Det.TRTOB_TripAddress;

                                    //drivermsg = drivermsg + Environment.NewLine +"Timing: " + hirer_Det.TRTOB_FromTime + Environment.NewLine;

                                    //driveremail = driveremail + "<br />" + "Timing: " + hirer_Det.TRTOB_FromTime ;

                                    //drivermsg = drivermsg + "Regards,";
                                    //driveremail = driveremail + "<br />" + "Regards,";


                                    //drivermsg = drivermsg + Environment.NewLine + institutionName.MI_Name;
                                    //driveremail = driveremail + "<br />" + institutionName.MI_Name;
                                    ////send msg to driver

                                    //drivename.TRMD_EmailId = "praveenishwar@vapstech.com";
                                    //item.TRMD_MobileNo = 9591081840;
                                    ////sms email
                                    //if (drivename.TRMD_EmailId !="" || drivename.TRMD_EmailId!=null)
                                    //{
                                    //    m = Email.bushiresendmail(dto.MI_Id, drivename.TRMD_EmailId, "TRIP_HIRER", hirer_Det.TRTOB_Id, driveremail);
                                    //}


                                    //if (item.TRMD_MobileNo > 0)
                                    //{
                                    //    long contno = Convert.ToInt64(item.TRMD_MobileNo);
                                    //    s = sms.bushiresendSms(dto.MI_Id,contno, "TRIP_HIRER", hirer_Det.TRTOB_Id, drivermsg).Result;
                                    //}
                                    if (item.TRMD_MobileNo !=0)
                                    {
                                        long contno = Convert.ToInt64(item.TRMD_MobileNo);
                                        s1 = sms.sendsmsforhirer(dto.MI_Id, contno, "TRIP_HIRER_DRIVER", 0, dto.TRTOB_Id, item.TRVD_Id).Result;
                                    }
                                    if (drivename.TRMD_EmailId !=null && drivename.TRMD_EmailId!="")
                                    {
                                        e1 = Email.sendEmailforhirer(dto.MI_Id, drivename.TRMD_EmailId, "TRIP_HIRER_DRIVER", 0, dto.TRTOB_Id, item.TRVD_Id);
                                    }

                                   // drivename.TRMD_EmailId = "praveenishwar@vapstech.com";
                                   

                                }

                               // driverdetals = driverdetals+ "Feel free to Contact No : 9686830071 " + Environment.NewLine;

                               // emaildriverdetals = emaildriverdetals+ "<br />" + "Feel free to Contact No : 9686830071 " ;
                                //driverdetals = driverdetals + "Regards,";
                                //emaildriverdetals = emaildriverdetals + "<br />" + "Regards,";

                                //driverdetals = driverdetals + Environment.NewLine + institutionName.MI_Name;
                                //emaildriverdetals = emaildriverdetals + "<br />" + institutionName.MI_Name;
                                
                            }

                           // hirer_Det.TRTOB_EmailId = "praveenishwar@vapstech.com";
                          //  hirer_Det.TRTOB_ContactNo = 9591081840;


                            //Comented for testing
                            // Email Email = new Email(_db);
                           // m = Email.bushiresendmail(dto.MI_Id, hirer_Det.TRTOB_EmailId, "TRIP_HIRER", hirer_Det.TRTOB_Id, emaildriverdetals);

                            
                            // Comented for testing
                          //  SMS sms = new SMS(_db);
        //   s = sms.bushiresendSms(dto.MI_Id, hirer_Det.TRTOB_ContactNo, "TRIP_HIRER", hirer_Det.TRTOB_Id, driverdetals).Result;


                            
                        }
                        }
                        else
                        {
                            dto.returnVal = "savingFailed";
                        }
                    }
                else if(dto.TRTP_Id > 0)
                {


                    if (dto.BtnClickVal.Equals("edit"))
                    {
                        var updatedatelist = _context.TripOnlineBookingDMO.Where(t => t.MI_Id == dto.MI_Id && t.TRTOB_Id == dto.TRTOB_Id).ToList();
                        if (updatedatelist.Count > 0)
                        {
                            var updatedate = _context.TripOnlineBookingDMO.Single(t => t.MI_Id == dto.MI_Id && t.TRTOB_Id == dto.TRTOB_Id);
                            updatedate.TRTOB_BookingDate = dto.TRTP_BookingDate;
                            updatedate.TRTOB_TripFromDate = dto.TRTP_TripFromDate;
                            updatedate.TRTOB_TripToDate = dto.TRTP_TripToDate;
                            updatedate.TRTOB_FromTime = dto.TRTP_FromTime;
                            updatedate.TRTOB_ToTime = dto.TRTP_ToTime;
                            updatedate.TRTOB_NoOfTravellers = dto.TRTP_NoOfTravellers;
                            updatedate.TRTOB_ContactNo = dto.TRTP_HirerContactNo;
                           

                            _context.Update(updatedate);
                            _context.SaveChanges();

                        }


                        var updatetriptable= _context.TripDMO.Single(t => t.MI_Id == dto.MI_Id && t.TRTP_Id == dto.TRTP_Id);

                        updatetriptable.TRTP_BookingDate = dto.TRTP_BookingDate;
                        updatetriptable.TRTP_TripFromDate = dto.TRTP_TripFromDate;
                        updatetriptable.TRTP_TripToDate = dto.TRTP_TripToDate;
                        updatetriptable.TRTP_FromTime = dto.TRTP_FromTime;
                        updatetriptable.TRTP_ToTime = dto.TRTP_ToTime;
                        updatetriptable.TRTP_NoOfTravellers = dto.TRTP_NoOfTravellers;
                        _context.Update(updatetriptable);
                        _context.SaveChanges();

                        if (dto.allottedVehicleDriver.Length > 0)
                        {

                            var upvhdrv = _context.TRVehicleDriverAllottmentDMO.Where(t => t.TRTP_Id == dto.TRTP_Id).ToList();
                            if (upvhdrv.Any())
                            {
                                foreach(var yy in upvhdrv)
                                {
                                    _context.Remove(yy);
                                }
                            }      

                            foreach (TripDTO item in dto.allottedVehicleDriver)
                            {

                                TRVehicleDriverAllottmentDMO dmo1 = new TRVehicleDriverAllottmentDMO();
                                dmo1.CreatedDate = DateTime.Now;
                                dmo1.TRTP_Id = dto.TRTP_Id;
                                dmo1.TRMV_Id = item.TRMV_Id;
                                dmo1.TRTP_ClosingKM = item.TRTP_ClosingKM;
                                dmo1.TRTP_OpeningKM = item.TRTP_OpeningKM;
                                dmo1.TRVD_Id = item.TRVD_Id;
                                dmo1.UpdatedDate = DateTime.Now;
                                _context.Add(dmo1);


                                //var update = _context.TRVehicleDriverAllottmentDMO.Single(d => d.TRVDA_Id == item.TRVDA_Id);
                                //update.TRTP_OpeningKM = item.TRTP_OpeningKM;
                                //update.TRVD_Id = item.TRVD_Id;
                                //update.TRMV_Id = item.TRMV_Id;
                                //update.UpdatedDate = DateTime.Now;
                                //_context.Update(update);
                            }
                            var status = _context.SaveChanges();
                            if (status > 0)
                            {
                                dto.returnVal = "updated";
                                string m = string.Empty;

                                string s = string.Empty;
                                string s1 = string.Empty;
                                string e1 = string.Empty;
                                SMS sms = new SMS(_db);
                                Email Email = new Email(_db);

                                dto.returnVal = "saved";
                                var hirer_Det = _context.TripOnlineBookingDMO.Single(d => d.MI_Id == dto.MI_Id && d.TRTOB_Id == dto.TRTOB_Id);

                                if (hirer_Det.TRTOB_ContactNo !=0)
                                {
                                    s1 = sms.sendsmsforhirer(dto.MI_Id, hirer_Det.TRTOB_ContactNo, "TRIP_HIRER", 0, dto.TRTOB_Id, 0).Result;
                                }

                                if (hirer_Det.TRTOB_EmailId !="" && hirer_Det.TRTOB_EmailId !=null)
                                {
                                    e1 = Email.sendEmailforhirer(dto.MI_Id, hirer_Det.TRTOB_EmailId, "TRIP_HIRER", 0, dto.TRTOB_Id, 0);
                                }
                                //hirer_Det.TRTOB_EmailId = "praveenishwar@vapstech.com";
                             
                                //var institutionName = _db.Institution.Single(i => i.MI_Id == dto.MI_Id);

                                //string driverdetals = "Dear" + " " + hirer_Det.TRTOB_HirerName + "," + Environment.NewLine;
                                //string emaildriverdetals = "Dear" + " " + hirer_Det.TRTOB_HirerName + ",";

                                //driverdetals = driverdetals + "The Bus Hire conformation details as per your request";
                                //emaildriverdetals = emaildriverdetals + "<br />" + "The Bus Hire conformation details as per your request";

                        
                               
                                if (dto.allottedVehicleDriver.Length > 0)
                                {

                                    //driverdetals = driverdetals + Environment.NewLine + "a." + "  " + "Trip Date: " + hirer_Det.TRTOB_TripFromDate;
                                    //emaildriverdetals = emaildriverdetals + "<br />" + "a." + "  " + "Trip Date: " + hirer_Det.TRTOB_TripFromDate;


                                    //driverdetals = driverdetals + Environment.NewLine + "b." + "  " + "Place: " + hirer_Det.TRTOB_TripAddress;

                                    //emaildriverdetals = emaildriverdetals + "<br />" + "b." + "  " + "Place: " + hirer_Det.TRTOB_TripAddress;

                                    //driverdetals = driverdetals + Environment.NewLine + "c." + "  " + "Timing: " + hirer_Det.TRTOB_FromTime + Environment.NewLine;

                                    //emaildriverdetals = emaildriverdetals + "<br />" + "c." + "  " + "Timing: " + hirer_Det.TRTOB_FromTime;
                                    foreach (TripDTO item in dto.allottedVehicleDriver)
                                    {
                                        string drivermsg = string.Empty;
                                        string driveremail = string.Empty;

                                        var drivename = _context.MasterDriverDMO.Single(t => t.MI_Id == dto.MI_Id && t.TRMD_Id == item.TRVD_Id);
                                        var vahicleno = _context.Master_VehicleDMO.Single(t => t.MI_Id == dto.MI_Id && t.TRMV_Id == item.TRMV_Id);




                                        //driverdetals = driverdetals + "VEHICLE NO :" + vahicleno.TRMV_VehicleNo + ", " + "DRIVER NAME:" + drivename.TRMD_DriverName + ", " + "DRIVER NO:" + item.TRMD_MobileNo + Environment.NewLine;

                                        //emaildriverdetals = emaildriverdetals + "<br />" + "VEHICLE NO :" + vahicleno.TRMV_VehicleNo + ", " + "DRIVER NAME:" + drivename.TRMD_DriverName + ", " + "DRIVER NO:" + item.TRMD_MobileNo;

                                        //msg to driver

                                        //drivermsg = "Dear" + "  " + drivename.TRMD_DriverName + "," + Environment.NewLine;
                                        //driveremail = "Dear" + "  " + drivename.TRMD_DriverName + "," + "<br />";

                                        //drivermsg = drivermsg + "Trip Details" + Environment.NewLine;
                                        //driveremail = driveremail + "Trip Details";

                                        //drivermsg = drivermsg + "Booked By :" + " " + hirer_Det.TRTOB_HirerName + Environment.NewLine;
                                        //driveremail = driveremail + "<br />" + "Booked By :" + " " + hirer_Det.TRTOB_HirerName;


                                        //drivermsg = drivermsg + "Trip Date :" + " " + hirer_Det.TRTOB_TripFromDate + Environment.NewLine;
                                        //driveremail = driveremail + "<br />" + "Trip Date :" + " " + hirer_Det.TRTOB_TripFromDate;


                                        //drivermsg = drivermsg + Environment.NewLine + "Place: " + hirer_Det.TRTOB_TripAddress;


                                        //driveremail = driveremail + "<br />" + "Place: " + hirer_Det.TRTOB_TripAddress;

                                        //drivermsg = drivermsg + Environment.NewLine + "Timing: " + hirer_Det.TRTOB_FromTime + Environment.NewLine;

                                        //driveremail = driveremail + "<br />" + "Timing: " + hirer_Det.TRTOB_FromTime;

                                        //drivermsg = drivermsg + "Regards,";
                                        //driveremail = driveremail + "<br />" + "Regards,";


                                        //drivermsg = drivermsg + Environment.NewLine + institutionName.MI_Name;
                                        //driveremail = driveremail + "<br />" + institutionName.MI_Name;
                                        //send msg to driver

                                        //drivename.TRMD_EmailId = "praveenishwar@vapstech.com";
                                        //item.TRMD_MobileNo = 9591081840;
                                        //sms email
                                        //if (drivename.TRMD_EmailId != "" || drivename.TRMD_EmailId != null)
                                        //{
                                        //    m = Email.bushiresendmail(dto.MI_Id, drivename.TRMD_EmailId, "TRIP_HIRER", hirer_Det.TRTOB_Id, driveremail);
                                        //}


                                        //if (item.TRMD_MobileNo > 0)
                                        //{
                                        //    long contno = Convert.ToInt64(item.TRMD_MobileNo);
                                        //    s = sms.bushiresendSms(dto.MI_Id, contno, "TRIP_HIRER", hirer_Det.TRTOB_Id, drivermsg).Result;
                                        //}

                                        if (item.TRMD_MobileNo !=0)
                                        {
                                            long contno = Convert.ToInt64(item.TRMD_MobileNo);
                                            s1 = sms.sendsmsforhirer(dto.MI_Id, contno, "TRIP_HIRER_DRIVER", 0, dto.TRTOB_Id, item.TRVD_Id).Result;
                                        }
                                        

                                        //drivename.TRMD_EmailId = "praveenishwar@vapstech.com";
                                        if (drivename.TRMD_EmailId !="" && drivename.TRMD_EmailId !=null)
                                        {
                                            e1 = Email.sendEmailforhirer(dto.MI_Id, drivename.TRMD_EmailId, "TRIP_HIRER_DRIVER", 0, dto.TRTOB_Id, item.TRVD_Id);
                                        }
                                       

                                    }

                                    // driverdetals = driverdetals+ "Feel free to Contact No : 9686830071 " + Environment.NewLine;

                                    // emaildriverdetals = emaildriverdetals+ "<br />" + "Feel free to Contact No : 9686830071 " ;
                                    //driverdetals = driverdetals + "Regards,";
                                    //emaildriverdetals = emaildriverdetals + "<br />" + "Regards,";

                                    //driverdetals = driverdetals + Environment.NewLine + institutionName.MI_Name;
                                    //emaildriverdetals = emaildriverdetals + "<br />" + institutionName.MI_Name;

                                   



                                }

                              //  hirer_Det.TRTOB_EmailId = "praveenishwar@vapstech.com";
//hirer_Det.TRTOB_ContactNo = 9591081840;


                                //Comented for testing
                                // Email Email = new Email(_db);
                                //m = Email.bushiresendmail(dto.MI_Id, hirer_Det.TRTOB_EmailId, "TRIP_HIRER", hirer_Det.TRTOB_Id, emaildriverdetals);


                                //// Comented for testing
                                ////  SMS sms = new SMS(_db);
                                //s = sms.bushiresendSms(dto.MI_Id, hirer_Det.TRTOB_ContactNo, "TRIP_HIRER", hirer_Det.TRTOB_Id, driverdetals).Result;



                            }
                            else
                            {
                                dto.returnVal = "failedUpdate";
                            }
                        }

                        //  var query = _context.TripDMO.Single(d => d.TRTP_Id == dto.TRTP_Id);
                        // query.UpdatedDate = DateTime.Now;
                        //  query.TRTP_ClosingKM = dto.TRTP_ClosingKM;
                        //  _context.Update(query);

                    }



                   else if (dto.BtnClickVal.Equals("Update"))
                    {
                        if (dto.allottedVehicleDriver.Length > 0)
                        {

                            var updatetriptbl = _context.TripDMO.Single(t => t.MI_Id == dto.MI_Id && t.TRTP_Id == dto.TRTP_Id);
                            updatetriptbl.TRTP_BookingDate = dto.TRTP_BookingDate;
                            updatetriptbl.TRTP_TripFromDate = dto.TRTP_TripFromDate;
                            updatetriptbl.TRTP_TripToDate = dto.TRTP_TripToDate;
                            updatetriptbl.UpdatedDate = DateTime.Now;
                            _context.Update(updatetriptbl);

                            var updatebookid = _context.TripDMO.Where(t => t.MI_Id == dto.MI_Id && t.TRTP_Id == dto.TRTP_Id).Select(e => e.TRTOB_Id).Single();
                            var updatebooktbl = _context.TripOnlineBookingDMO.Single(t => t.MI_Id == dto.MI_Id && t.TRTOB_Id == updatebookid);
                            updatebooktbl.TRTOB_BookingDate = dto.TRTP_BookingDate;
                            updatebooktbl.TRTOB_TripFromDate = dto.TRTP_TripFromDate;
                            updatebooktbl.TRTOB_TripToDate = dto.TRTP_TripToDate;
                            updatebooktbl.UpdatedDate = DateTime.Now;
                            _context.Update(updatebooktbl);


                            foreach (TripDTO item in dto.allottedVehicleDriver)
                            {
                                var update = _context.TRVehicleDriverAllottmentDMO.Single(d=>d.TRVDA_Id==item.TRVDA_Id);
                                update.TRTP_OpeningKM = item.TRTP_OpeningKM;
                                update.TRTP_ClosingKM = item.TRTP_ClosingKM;
                                update.UpdatedDate = DateTime.Now;
                                _context.Update(update);
                            }
                            var status = _context.SaveChanges();
                            if (status > 0)
                            {
                                dto.returnVal = "updated";
                            }
                            else
                            {
                                dto.returnVal = "failedUpdate";
                            }
                        }

                      //  var query = _context.TripDMO.Single(d => d.TRTP_Id == dto.TRTP_Id);
                       // query.UpdatedDate = DateTime.Now;
                      //  query.TRTP_ClosingKM = dto.TRTP_ClosingKM;
                      //  _context.Update(query);
                       
                    }
                    else if (dto.BtnClickVal.Equals("GenerateBill"))
                    {
                        var updatetriptbl = _context.TripDMO.Single(t => t.MI_Id == dto.MI_Id && t.TRTP_Id == dto.TRTP_Id);
                        updatetriptbl.TRTP_BookingDate = dto.TRTP_BookingDate;
                        updatetriptbl.TRTP_TripFromDate = dto.TRTP_TripFromDate;
                        updatetriptbl.TRTP_TripToDate = dto.TRTP_TripToDate;
                        updatetriptbl.UpdatedDate = DateTime.Now;
                        _context.Update(updatetriptbl);

                        var updatebookid = _context.TripDMO.Where(t => t.MI_Id == dto.MI_Id && t.TRTP_Id == dto.TRTP_Id).Select(e => e.TRTOB_Id).Single();
                        var updatebooktbl = _context.TripOnlineBookingDMO.Single(t => t.MI_Id == dto.MI_Id && t.TRTOB_Id == updatebookid);
                        updatebooktbl.TRTOB_BookingDate = dto.TRTP_BookingDate;
                        updatebooktbl.TRTOB_TripFromDate = dto.TRTP_TripFromDate;
                        updatebooktbl.TRTOB_TripToDate = dto.TRTP_TripToDate;
                        updatebooktbl.UpdatedDate = DateTime.Now;
                        _context.Update(updatebooktbl);



                        foreach (TripDTO item in dto.allottedVehicleDriver)
                        {
                            var update = _context.TRVehicleDriverAllottmentDMO.Single(d => d.TRVDA_Id == item.TRVDA_Id);
                            update.TRTP_OpeningKM = item.TRTP_OpeningKM;
                            update.TRTP_ClosingKM = item.TRTP_ClosingKM;
                            update.UpdatedDate = DateTime.Now;
                            _context.Update(update);
                        }

                        var query = _context.TripDMO.Single(d => d.TRTP_Id == dto.TRTP_Id);
                        query.UpdatedDate = DateTime.Now;
                       // query.TRTP_ClosingKM = dto.TRTP_ClosingKM;
                        query.TRTP_BillNo = dto.TRTP_BillNo;
                        query.TRTP_BillDate = dto.TRTP_BillDate;
                        query.TRTP_BillAmount = dto.TRTP_BillAmount;
                        query.TRTP_DiscountAmount = dto.TRTP_DiscountAmount;
                        query.TRTP_BillGeneratedFlag = true;
                        var total= (dto.TRTP_BillAmount) - (dto.TRTP_DiscountAmount);
                        query.TRTP_TotalReceivable = (dto.TRTP_BillAmount) - (dto.TRTP_DiscountAmount);
                        query.TRTP_BalanceAmount = (total) - (query.TRTP_PaidAmount);
                        _context.Update(query);
                        var status = _context.SaveChanges();
                        if (status > 0)
                        {
                            dto.returnVal = "updated";
                        }
                        else
                        {
                            dto.returnVal = "failedUpdate";
                        }
                    }
                    else
                    {
                        var updatedatelist = _context.TripOnlineBookingDMO.Where(t => t.MI_Id == dto.MI_Id && t.TRTOB_Id == dto.TRTOB_Id).ToList();
                        if (updatedatelist.Count > 0)
                        {
                            var updatedate = _context.TripOnlineBookingDMO.Single(t => t.MI_Id == dto.MI_Id && t.TRTOB_Id == dto.TRTOB_Id);
                            updatedate.TRTOB_BookingDate = dto.TRTP_BookingDate;
                            updatedate.TRTOB_TripFromDate = dto.TRTP_TripFromDate;
                            updatedate.TRTOB_TripToDate = dto.TRTP_TripToDate;
                            updatedate.TRTOB_FromTime = dto.TRTP_FromTime;
                            updatedate.TRTOB_ToTime = dto.TRTP_ToTime;
                            updatedate.TRTOB_NoOfTravellers = dto.TRTP_NoOfTravellers;
                            updatedate.TRTOB_ContactNo = dto.TRTP_HirerContactNo;
                            _context.Update(updatedate);
                            _context.SaveChanges();

                        }


                        var updatetriptable = _context.TripDMO.Single(t => t.MI_Id == dto.MI_Id && t.TRTP_Id == dto.TRTP_Id);

                        updatetriptable.TRTP_BookingDate = dto.TRTP_BookingDate;
                        updatetriptable.TRTP_TripFromDate = dto.TRTP_TripFromDate;
                        updatetriptable.TRTP_TripToDate = dto.TRTP_TripToDate;
                        updatetriptable.TRTP_FromTime = dto.TRTP_FromTime;
                        updatetriptable.TRTP_ToTime = dto.TRTP_ToTime;
                        updatetriptable.TRTP_NoOfTravellers = dto.TRTP_NoOfTravellers;
                        _context.Update(updatetriptable);
                        _context.SaveChanges();

                        if (dto.allottedVehicleDriver.Length > 0)
                        {

                            var upvhdrv = _context.TRVehicleDriverAllottmentDMO.Where(t => t.TRTP_Id == dto.TRTP_Id).ToList();
                            if (upvhdrv.Any())
                            {
                                foreach (var yy in upvhdrv)
                                {
                                    _context.Remove(yy);
                                }
                            }

                            foreach (TripDTO item in dto.allottedVehicleDriver)
                            {

                                TRVehicleDriverAllottmentDMO dmo1 = new TRVehicleDriverAllottmentDMO();
                                dmo1.CreatedDate = DateTime.Now;
                                dmo1.TRTP_Id = dto.TRTP_Id;
                                dmo1.TRMV_Id = item.TRMV_Id;
                                dmo1.TRTP_ClosingKM = item.TRTP_ClosingKM;
                                dmo1.TRTP_OpeningKM = item.TRTP_OpeningKM;
                                dmo1.TRVD_Id = item.TRVD_Id;
                                dmo1.UpdatedDate = DateTime.Now;
                                _context.Add(dmo1);


                                //var update = _context.TRVehicleDriverAllottmentDMO.Single(d => d.TRVDA_Id == item.TRVDA_Id);
                                //update.TRTP_OpeningKM = item.TRTP_OpeningKM;
                                //update.TRVD_Id = item.TRVD_Id;
                                //update.TRMV_Id = item.TRMV_Id;
                                //update.UpdatedDate = DateTime.Now;
                                //_context.Update(update);
                            }
                            var status = _context.SaveChanges();
                            if (status > 0)
                            {
                                dto.returnVal = "updated";

                                string m = string.Empty;

                                string s = string.Empty;
                                string s1 = string.Empty;
                                string e1 = string.Empty;
                                SMS sms = new SMS(_db);
                                Email Email = new Email(_db);

                                dto.returnVal = "saved";
                                var hirer_Det = _context.TripOnlineBookingDMO.Single(d => d.MI_Id == dto.MI_Id && d.TRTOB_Id == dto.TRTOB_Id);
                                //var institutionName = _db.Institution.Single(i => i.MI_Id == dto.MI_Id);

                                //string driverdetals = "Dear" + " " + hirer_Det.TRTOB_HirerName + "," + Environment.NewLine;
                                //string emaildriverdetals = "Dear" + " " + hirer_Det.TRTOB_HirerName + ",";

                                //driverdetals = driverdetals + "The Bus Hire conformation details as per your request";
                                //emaildriverdetals = emaildriverdetals + "<br />" + "The Bus Hire conformation details as per your request";


                                if (dto.allottedVehicleDriver.Length > 0)
                                {

                                    //driverdetals = driverdetals + Environment.NewLine + "a." + "  " + "Trip Date: " + hirer_Det.TRTOB_TripFromDate;
                                    //emaildriverdetals = emaildriverdetals + "<br />" + "a." + "  " + "Trip Date: " + hirer_Det.TRTOB_TripFromDate;


                                    //driverdetals = driverdetals + Environment.NewLine + "b." + "  " + "Place: " + hirer_Det.TRTOB_TripAddress;

                                    //emaildriverdetals = emaildriverdetals + "<br />" + "b." + "  " + "Place: " + hirer_Det.TRTOB_TripAddress;

                                    //driverdetals = driverdetals + Environment.NewLine + "c." + "  " + "Timing: " + hirer_Det.TRTOB_FromTime + Environment.NewLine;

                                    //emaildriverdetals = emaildriverdetals + "<br />" + "c." + "  " + "Timing: " + hirer_Det.TRTOB_FromTime;
                                    foreach (TripDTO item in dto.allottedVehicleDriver)
                                    {
                                        string drivermsg = string.Empty;
                                        string driveremail = string.Empty;

                                        var drivename = _context.MasterDriverDMO.Single(t => t.MI_Id == dto.MI_Id && t.TRMD_Id == item.TRVD_Id);
                                        var vahicleno = _context.Master_VehicleDMO.Single(t => t.MI_Id == dto.MI_Id && t.TRMV_Id == item.TRMV_Id);

                                        if (item.TRMD_MobileNo !=0)
                                        {
                                            long contno = Convert.ToInt64(item.TRMD_MobileNo);
                                            s1 = sms.sendsmsforhirer(dto.MI_Id, contno, "TRIP_HIRER_DRIVER", 0, dto.TRTOB_Id, item.TRVD_Id).Result;
                                        }

                                       

                                        //drivename.TRMD_EmailId = "praveenishwar@vapstech.com";

                                        if (drivename.TRMD_EmailId!="" && drivename.TRMD_EmailId!=null)
                                        {
                                            e1 = Email.sendEmailforhirer(dto.MI_Id, drivename.TRMD_EmailId, "TRIP_HIRER_DRIVER", 0, dto.TRTOB_Id, item.TRVD_Id);
                                        }
                               
                                        //driverdetals = driverdetals + "VEHICLE NO :" + vahicleno.TRMV_VehicleNo + ", " + "DRIVER NAME:" + drivename.TRMD_DriverName + ", " + "DRIVER NO:" + item.TRMD_MobileNo + Environment.NewLine;

                                        //emaildriverdetals = emaildriverdetals + "<br />" + "VEHICLE NO :" + vahicleno.TRMV_VehicleNo + ", " + "DRIVER NAME:" + drivename.TRMD_DriverName + ", " + "DRIVER NO:" + item.TRMD_MobileNo;

                                        ////msg to driver

                                        //drivermsg = "Dear" + "  " + drivename.TRMD_DriverName + "," + Environment.NewLine;
                                        //driveremail = "Dear" + "  " + drivename.TRMD_DriverName + "," + "<br />";

                                        //drivermsg = drivermsg + "Trip Details" + Environment.NewLine;
                                        //driveremail = driveremail + "Trip Details";

                                        //drivermsg = drivermsg + "Booked By :" + " " + hirer_Det.TRTOB_HirerName + Environment.NewLine;
                                        //driveremail = driveremail + "<br />" + "Booked By :" + " " + hirer_Det.TRTOB_HirerName;


                                        //drivermsg = drivermsg + "Trip Date :" + " " + hirer_Det.TRTOB_TripFromDate + Environment.NewLine;
                                        //driveremail = driveremail + "<br />" + "Trip Date :" + " " + hirer_Det.TRTOB_TripFromDate;


                                        //drivermsg = drivermsg + Environment.NewLine + "Place: " + hirer_Det.TRTOB_TripAddress;


                                        //driveremail = driveremail + "<br />" + "Place: " + hirer_Det.TRTOB_TripAddress;

                                        //drivermsg = drivermsg + Environment.NewLine + "Timing: " + hirer_Det.TRTOB_FromTime + Environment.NewLine;

                                        //driveremail = driveremail + "<br />" + "Timing: " + hirer_Det.TRTOB_FromTime;

                                        //drivermsg = drivermsg + "Regards,";
                                        //driveremail = driveremail + "<br />" + "Regards,";


                                        //drivermsg = drivermsg + Environment.NewLine + institutionName.MI_Name;
                                        //driveremail = driveremail + "<br />" + institutionName.MI_Name;
                                        ////send msg to driver

                                        //drivename.TRMD_EmailId = "praveenishwar@vapstech.com";
                                        //item.TRMD_MobileNo = 9591081840;
                                        ////sms email
                                        //if (drivename.TRMD_EmailId != "" || drivename.TRMD_EmailId != null)
                                        //{
                                        //    m = Email.bushiresendmail(dto.MI_Id, drivename.TRMD_EmailId, "TRIP_HIRER", hirer_Det.TRTOB_Id, driveremail);
                                        //}


                                        //if (item.TRMD_MobileNo > 0)
                                        //{
                                        //    long contno = Convert.ToInt64(item.TRMD_MobileNo);
                                        //    s = sms.bushiresendSms(dto.MI_Id, contno, "TRIP_HIRER", hirer_Det.TRTOB_Id, drivermsg).Result;
                                        //}


                                    }

                                    // driverdetals = driverdetals+ "Feel free to Contact No : 9686830071 " + Environment.NewLine;

                                    // emaildriverdetals = emaildriverdetals+ "<br />" + "Feel free to Contact No : 9686830071 " ;
                                    //driverdetals = driverdetals + "Regards,";
                                    //emaildriverdetals = emaildriverdetals + "<br />" + "Regards,";

                                    //driverdetals = driverdetals + Environment.NewLine + institutionName.MI_Name;
                                    //emaildriverdetals = emaildriverdetals + "<br />" + institutionName.MI_Name;





                                }

                               // hirer_Det.TRTOB_EmailId = "praveenishwar@vapstech.com";
                               // hirer_Det.TRTOB_ContactNo = 9591081840;


                                //Comented for testing
                                // Email Email = new Email(_db);
                                //m = Email.bushiresendmail(dto.MI_Id, hirer_Det.TRTOB_EmailId, "TRIP_HIRER", hirer_Det.TRTOB_Id, emaildriverdetals);


                                //// Comented for testing
                                ////  SMS sms = new SMS(_db);
                                //s = sms.bushiresendSms(dto.MI_Id, hirer_Det.TRTOB_ContactNo, "TRIP_HIRER", hirer_Det.TRTOB_Id, driverdetals).Result;
                            }
                            else
                            {
                                dto.returnVal = "failedUpdate";
                            }
                        }

                        //  var query = _context.TripDMO.Single(d => d.TRTP_Id == dto.TRTP_Id);
                        // query.UpdatedDate = DateTime.Now;
                        //  query.TRTP_ClosingKM = dto.TRTP_ClosingKM;
                        //  _context.Update(query);
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }


        public TripDTO duprecpcheck(TripDTO data) {
            try
            {
                var recdup = _context.TR_Trip_PaymentDMO.Where(t => t.MI_Id == data.MI_Id && t.TRTPP_ReceiptNo == data.TRTPP_ReceiptNo).Select(r => new TripDTO { TRTPP_ReceiptNo = r.TRTPP_ReceiptNo }).ToList();
                if (recdup.Count>0)
                {
                    data.recduparray = recdup.ToArray();
                }
                    
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }
      public  TripDTO SearchByTripId(TripDTO dto)
        {
            try
            {
                //Receipt No. generation code starts here.
                var transnumconfigsettings = _db.Receipt_Numbering.Where(d => d.MI_Id == dto.MI_Id).ToList();
                if(transnumconfigsettings.Count > 0)
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                    Master_NumberingDTO num = new Master_NumberingDTO();
                    num.MI_Id = dto.MI_Id;
                    num.ASMAY_Id = dto.asmay_id;
                    num.IMN_AutoManualFlag = transnumconfigsettings.FirstOrDefault().IRN_AutoManualFlag;
                    num.IMN_DuplicatesFlag = transnumconfigsettings.FirstOrDefault().IRN_DuplicatesFlag;
                    num.IMN_StartingNo = transnumconfigsettings.FirstOrDefault().IRN_StartingNo;
                    num.IMN_WidthNumeric = transnumconfigsettings.FirstOrDefault().IRN_WidthNumeric;
                    num.IMN_ZeroPrefixFlag = transnumconfigsettings.FirstOrDefault().IRN_ZeroPrefixFlag;
                    num.IMN_PrefixAcadYearCode = transnumconfigsettings.FirstOrDefault().IRN_PrefixAcadYearCode;
                    num.IMN_PrefixFinYearCode = transnumconfigsettings.FirstOrDefault().IRN_PrefixFinYearCode;
                    num.IMN_PrefixCalYearCode = transnumconfigsettings.FirstOrDefault().IRN_PrefixCalYearCode;
                    num.IMN_PrefixParticular = transnumconfigsettings.FirstOrDefault().IRN_PrefixParticular;
                    num.IMN_SuffixAcadYearCode = transnumconfigsettings.FirstOrDefault().IRN_SuffixAcadYearCode;
                    num.IMN_SuffixFinYearCode = transnumconfigsettings.FirstOrDefault().IRN_SuffixFinYearCode;
                    num.IMN_SuffixCalYearCode = transnumconfigsettings.FirstOrDefault().IRN_SuffixCalYearCode;
                    num.IMN_SuffixParticular = transnumconfigsettings.FirstOrDefault().IRN_SuffixParticular;
                    num.IMN_RestartNumFlag = transnumconfigsettings.FirstOrDefault().IRN_RestartNumFlag;
                    num.IMN_Flag = "TripReceiptNo";
                    dto.TRTPP_ReceiptNo = a.GenerateNumber(num);
                    //Receipt No. generation code ends  here.
                }
                else
                {
                    dto.returnVal = "noMapping";
                }
                if (dto.SearchBy.Equals("Trip"))
                {
                    var triplist = (from m in _context.TripDMO
                                    from n in _context.MasterHirerGroupDMO
                                    from r in _context.TripOnlineBookingDMO
                                    where m.TRTOB_Id == r.TRTOB_Id && m.TRHG_Id == n.TRHG_Id   
                                     && m.MI_Id == dto.MI_Id && m.TRTP_ActiveFlg == true && m.TRVD_TripId == dto.TRVD_TripId
                                    select new TripDTO
                                    {
                                        TRTOB_Id = m.TRTOB_Id,
                                        TRTP_Id = m.TRTP_Id,
                                        TRTP_BillGeneratedFlag = m.TRTP_BillGeneratedFlag,
                                        TRTP_AdvanceReceived = m.TRTP_AdvanceReceived,
                                        TRTP_AdvancePaid=m.TRTP_AdvancePaid,
                                        TRTP_TotalReceivable = m.TRTP_TotalReceivable,
                                        TRTP_BookingDate = m.TRTP_BookingDate,
                                        TRTP_TripAddress = m.TRTP_TripAddress,
                                        TRTP_TripFromDate = m.TRTP_TripFromDate,
                                        TRTP_TripToDate = m.TRTP_TripToDate,
                                        TRTP_HirerName = m.TRTP_HirerName,
                                        TRTP_HirerContactNo = m.TRTP_HirerContactNo,
                                        TRTP_BillNo = m.TRTP_BillNo,
                                        TRTP_BillDate = m.TRTP_BillDate,
                                        TRTP_BillAmount = m.TRTP_BillAmount,
                                        TRTP_DiscountAmount = m.TRTP_DiscountAmount,
                                        TRTP_PickUpLocation=m.TRTP_PickUpLocation,
                                        TRTP_PaidAmount=m.TRTP_PaidAmount,
                                        TRTP_BalanceAmount=m.TRTP_BalanceAmount,
                                        TRTP_NoOfTravellers=m.TRTP_NoOfTravellers
                                    }
                                ).ToList();
                    if (triplist.Count > 0)
                    {
                        dto.tripList = triplist.ToArray();
                        dto.TRTP_Id = triplist.FirstOrDefault().TRTP_Id;

                        var vhcleDvrAllottment = (from m in _context.TripOnlineBookingDMO
                                                  from n in _context.TripDMO
                                                  from o in _context.TRVehicleDriverAllottmentDMO
                                                  where m.TRTOB_Id == n.TRTOB_Id && n.TRTP_Id == o.TRTP_Id && m.TRTOB_ActiveFlg == true && n.TRTP_ActiveFlg == true && n.MI_Id == dto.MI_Id && n.TRTP_Id == triplist.FirstOrDefault().TRTP_Id
                                                  select new TripDTO
                                                  {
                                                      TRTP_Id = n.TRTP_Id,
                                                      TRMV_Id = o.TRMV_Id,
                                                      TRVD_Id = o.TRVD_Id,
                                                      TRTP_OpeningKM = Convert.ToInt64(o.TRTP_OpeningKM),
                                                      TRTP_ClosingKM = Convert.ToInt64(o.TRTP_ClosingKM),
                                                      TRVDA_Id=o.TRVDA_Id
                                                  }).ToList();
                        if (vhcleDvrAllottment.Count > 0)
                        {
                            dto.vehicleDriverAllottmentList = vhcleDvrAllottment.ToArray();
                        }
                    }

                    var receipts = (from m in _context.TripDMO
                                    from n in _context.MasterHirerGroupDMO
                                    from r in _context.TripOnlineBookingDMO
                                    from s in _context.TR_Trip_PaymentDMO
                                    from t in _context.TR_Trip_Payment_TripsDMO
                                    where m.TRTOB_Id == r.TRTOB_Id && m.TRHG_Id == n.TRHG_Id  
                                    && s.MI_Id==m.MI_Id && s.TRTPP_Id==t.TRTPP_Id && t.TRTP_Id==m.TRTP_Id  && m.MI_Id == dto.MI_Id && m.TRTP_ActiveFlg == true && m.TRVD_TripId == dto.TRVD_TripId && m.TRTP_BillGeneratedFlag == true 
                                    select new TripDTO
                                    {
                                        TRTPP_Id=s.TRTPP_Id,
                                        TRTOB_Id = m.TRTOB_Id,
                                        TRTP_Id = m.TRTP_Id,
                                        TRTP_BillGeneratedFlag = m.TRTP_BillGeneratedFlag,
                                        TRTP_AdvanceReceived = m.TRTP_AdvanceReceived,
                                        TRTP_AdvancePaid=m.TRTP_AdvancePaid,
                                        TRTP_TotalReceivable = m.TRTP_TotalReceivable,
                                        TRTP_BookingDate = m.TRTP_BookingDate,
                                        TRTP_TripDate = m.TRTP_TripDate,
                                        TRTP_TripAddress = m.TRTP_TripAddress,
                                        TRTP_TripFromDate = m.TRTP_TripFromDate,
                                        TRTP_TripToDate = m.TRTP_TripToDate,
                                        TRTP_HirerName = m.TRTP_HirerName,
                                        TRTP_HirerContactNo = m.TRTP_HirerContactNo,
                                        TRTP_BillNo = m.TRTP_BillNo,
                                        TRTP_BillDate = m.TRTP_BillDate,
                                        TRTP_BillAmount = m.TRTP_BillAmount,
                                        TRTP_DiscountAmount = m.TRTP_DiscountAmount,
                                        TRTP_PickUpLocation = m.TRTP_PickUpLocation,
                                        TRTPP_ReceiptDate = s.TRTPP_ReceiptDate,
                                        TRTPP_ReceiptNo = s.TRTPP_ReceiptNo,
                                        TRTPP_ReceiptReferenceNo = s.TRTPP_ReceiptReferenceNo,
                                        TRTPP_PaidAmount = s.TRTPP_PaidAmount,
                                        TRTP_PaidAmount = m.TRTP_PaidAmount,
                                        TRTP_BalanceAmount = m.TRTP_BalanceAmount
                                    }
                                ).ToList();
                    if (receipts.Count > 0)
                    {
                        dto.generatedReceiptsList = receipts.OrderByDescending(t=>t.TRTPP_Id).ToArray();
                    }
                }
                else if (dto.SearchBy.Equals("Hirer"))
                {
                    var hirer = (from m in _context.TripDMO
                                    from n in _context.MasterHirerGroupDMO
                                    from r in _context.TripOnlineBookingDMO
                                    from s in _context.MasterHirerDMO
                                    where m.TRTOB_Id == r.TRTOB_Id && m.TRHG_Id == n.TRHG_Id 
                                     && m.MI_Id==s.MI_Id  && m.MI_Id == dto.MI_Id && m.TRTP_ActiveFlg == true && m.TRTP_HirerName.Equals(dto.TRTP_HirerName) &&  m.TRTP_BillGeneratedFlag == true && m.TRTP_HirerName.Contains(s.TRMH_HirerName)
                                 group new {m,n,r,s } by new { m.TRTP_Id } into g
                                    select new TripDTO
                                    {
                                        TRTOB_Id = g.FirstOrDefault().m.TRTOB_Id,
                                        TRTP_Id = g.FirstOrDefault().m.TRTP_Id,
                                        TRTP_BillGeneratedFlag = g.FirstOrDefault().m.TRTP_BillGeneratedFlag,
                                        TRTP_AdvanceReceived = g.FirstOrDefault().m.TRTP_AdvanceReceived,
                                        TRTP_AdvancePaid=g.FirstOrDefault().m.TRTP_AdvancePaid,
                                        TRTP_TotalReceivable = g.FirstOrDefault().m.TRTP_TotalReceivable,
                                        TRTP_BookingDate = g.FirstOrDefault().m.TRTP_BookingDate,
                                        TRTP_TripDate = g.FirstOrDefault().m.TRTP_TripDate,
                                        TRTP_TripAddress = g.FirstOrDefault().m.TRTP_TripAddress,
                                        TRTP_TripFromDate = g.FirstOrDefault().m.TRTP_TripFromDate,
                                        TRTP_TripToDate = g.FirstOrDefault().m.TRTP_TripToDate,
                                        TRTP_HirerName = g.FirstOrDefault().m.TRTP_HirerName,
                                        TRTP_HirerContactNo = g.FirstOrDefault().m.TRTP_HirerContactNo,
                                        TRTP_BillNo = g.FirstOrDefault().m.TRTP_BillNo,
                                        TRTP_BillDate = g.FirstOrDefault().m.TRTP_BillDate,
                                        TRTP_BillAmount = g.FirstOrDefault().m.TRTP_BillAmount,
                                        TRTP_DiscountAmount = g.FirstOrDefault().m.TRTP_DiscountAmount,
                                        TRTP_PickUpLocation = g.FirstOrDefault().m.TRTP_PickUpLocation,
                                        TRMH_MobileNo= g.FirstOrDefault().s.TRMH_MobileNo,
                                        TRMH_EmailId= g.FirstOrDefault().s.TRMH_EmailId,
                                        TRMH_Id= g.FirstOrDefault().s.TRMH_Id,
                                        TRTP_PaidAmount=g.FirstOrDefault().m.TRTP_PaidAmount,
                                        TRTP_BalanceAmount=g.FirstOrDefault().m.TRTP_BalanceAmount
                                    }
                               ).ToList();
                    if (hirer.Count > 0)
                    {
                        dto.hirerList = hirer.ToArray();
                    }
                    var receipts = (from m in _context.TripDMO
                                    from n in _context.MasterHirerGroupDMO
                                    from r in _context.TripOnlineBookingDMO
                                    from s in _context.TR_Trip_PaymentDMO
                                    from t in _context.TR_Trip_Payment_TripsDMO
                                    where m.TRTOB_Id == r.TRTOB_Id && m.TRHG_Id == n.TRHG_Id  && s.MI_Id == m.MI_Id && s.TRTPP_Id == t.TRTPP_Id && t.TRTP_Id == m.TRTP_Id && m.MI_Id == dto.MI_Id && m.TRTP_ActiveFlg == true && m.TRTP_HirerName == dto.TRTP_HirerName && m.TRTP_BillGeneratedFlag == true
                                    select new TripDTO
                                    {
                                        TRTOB_Id = m.TRTOB_Id,
                                        TRTP_Id = m.TRTP_Id,
                                        TRTP_BillGeneratedFlag = m.TRTP_BillGeneratedFlag,
                                        TRTP_AdvanceReceived = m.TRTP_AdvanceReceived,
                                        TRTP_AdvancePaid=m.TRTP_AdvancePaid,
                                        TRTP_TotalReceivable = m.TRTP_TotalReceivable,
                                        TRTP_BookingDate = m.TRTP_BookingDate,
                                        TRTP_TripDate = m.TRTP_TripDate,
                                        TRTP_TripAddress = m.TRTP_TripAddress,
                                        TRTP_TripFromDate = m.TRTP_TripFromDate,
                                        TRTP_TripToDate = m.TRTP_TripToDate,
                                        TRTP_HirerName = m.TRTP_HirerName,
                                        TRTP_HirerContactNo = m.TRTP_HirerContactNo,
                                        TRTP_BillNo = m.TRTP_BillNo,
                                        TRTP_BillDate = m.TRTP_BillDate,
                                        TRTP_BillAmount = m.TRTP_BillAmount,
                                        TRTP_DiscountAmount = m.TRTP_DiscountAmount,
                                        TRTP_PickUpLocation = m.TRTP_PickUpLocation,
                                        TRTPP_ReceiptDate = s.TRTPP_ReceiptDate,
                                        TRTPP_ReceiptNo = s.TRTPP_ReceiptNo,
                                        TRTPP_ReceiptReferenceNo = s.TRTPP_ReceiptReferenceNo,
                                        TRTPP_PaidAmount = s.TRTPP_PaidAmount,
                                        TRTP_PaidAmount = m.TRTP_PaidAmount,
                                        TRTP_BalanceAmount = m.TRTP_BalanceAmount
                                    }
                                 ).ToList();
                    if (receipts.Count > 0)
                    {
                        dto.generatedReceiptsList = receipts.ToArray();
                    }
                }
              
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public TripDTO getbillNo(TripDTO dto)
        {
            try
            {
                var groupid = (from a in _context.TripOnlineBookingDMO
                               from b in _context.TripDMO
                               where a.MI_Id == b.MI_Id && a.TRTOB_Id == b.TRTOB_Id && b.TRTP_Id == dto.TRTP_Id
                               select new TripDTO
                               {
                                   TRHG_Id = a.TRHG_Id
                               }).Distinct().ToList();


                var query = _context.TripDMO.Where(d => d.MI_Id == dto.MI_Id && d.TRTP_Id == dto.TRTP_Id).Select(d =>new TripDMO { TRVD_TripId=d.TRVD_TripId}).ToList();
                if(dto.allottedVehicleDriver.Length > 0)
                {
                    decimal billamount = 0;

                    foreach (TripDTO items in dto.allottedVehicleDriver)
                    {
                        var query1 = _context.Master_VehicleDMO.Where(d => d.MI_Id == dto.MI_Id && d.TRMV_Id == items.TRMV_Id).ToList();
                        var query2 = _context.MasterHirerRateDMO.Where(d => d.MI_Id == dto.MI_Id && d.TRMVT_Id == query1.FirstOrDefault().TRMVT_Id && d.TRHG_Id== groupid.FirstOrDefault().TRHG_Id).ToList();
                        if (query2.Count>0)
                        {
                            var TotalKM = (items.TRTP_ClosingKM) - (Convert.ToInt64(items.TRTP_OpeningKM));
                            billamount = (billamount) + Convert.ToDecimal((TotalKM) * (query2.FirstOrDefault().TRHR_RatePerKM));
                        }
                        else
                        {
                            dto.returnVal = "rate"; 
                        }

                       
                       
                    }
                    dto.TRTP_BillAmount = Convert.ToDecimal(billamount);
                }
                //var query1 = _context.MasterHirerRateDMO.Where(d => d.MI_Id == dto.MI_Id).ToList();
               // var TotalKM = (dto.TRTP_ClosingKM) - (Convert.ToInt64(query.FirstOrDefault().TRTP_OpeningKM));
               // var billamount = (TotalKM) * (query1.FirstOrDefault().TRHR_RatePerKM);
               // dto.TRTP_BillAmount =Convert.ToDecimal(billamount);
                dto.TRVD_TripId = query.FirstOrDefault().TRVD_TripId;
                //Trip Bill No. generation code starts here.


                //commented because of Booking Id and Bill No should be Shame
                // var transnumconfigsettings = _db.Master_Numbering.Where(d => d.MI_Id == dto.MI_Id && d.IMN_Flag.Equals("TripBill")).ToList();
                //   if (transnumconfigsettings.Count > 0)
                //  {
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
                //    num.IMN_Flag = "TripBill";
                //    dto.TRTP_BillNo = a.GenerateNumber(num);
                //}
                //else
                //{
                //    dto.returnVal = "Nomapping";
                //}
                //Trip Bill No. generation code ends  here.

                //added booking id and bill id should be shame

                var billno = (from a in _context.TripOnlineBookingDMO
                               from b in _context.TripDMO
                               where a.MI_Id == b.MI_Id && a.TRTOB_Id == b.TRTOB_Id && b.TRTP_Id == dto.TRTP_Id
                               select new TripDTO
                               {
                                   TRTOB_BookingId = a.TRTOB_BookingId
                               }).Distinct().ToList();


                dto.TRTP_BillNo = billno.FirstOrDefault().TRTOB_BookingId;


            }
            catch (Exception e)
            {

            }
            return dto;
        }
        public TripDTO pay(TripDTO data)
        {
            try
            {
                if (data.SelectedRadioVal.Equals("trip"))
                {
                    var query = (from m in _db.TripDMO
                                 from n in _db.TR_Trip_PaymentDMO
                                 from o in _db.TR_Trip_Payment_TripsDMO
                                 where m.MI_Id == n.MI_Id && n.TRTPP_Id == o.TRTPP_Id && m.TRTP_Id == o.TRTP_Id && m.MI_Id == data.MI_Id && o.TRTP_Id == data.TRTP_Id && m.TRTOB_Id==n.TRTOB_Id
                                 select new TripDTO
                                 {
                                     TRTP_BillAmount = m.TRTP_BillAmount,
                                     TRTP_TotalReceivable = m.TRTP_TotalReceivable,
                                     TRTP_PaidAmount = m.TRTP_PaidAmount,
                                     TRTP_BalanceAmount = m.TRTP_BalanceAmount,
                                     TRTPP_PaidAmount = n.TRTPP_PaidAmount,
                                 }).ToList();

                  
                    if (query.Count > 0 && query.FirstOrDefault().TRTP_BalanceAmount == 0)
                    {
                        data.returnVal = "AmountPaid";
                        return data;
                    }
                    else
                    {
                        if (query.Count > 0)
                        {
                            if ((data.TRTPP_PaidAmount + data.TRTP_DiscountAmount) > query.FirstOrDefault().TRTP_BalanceAmount)
                            {
                                var excessamount = (data.TRTPP_PaidAmount + data.TRTP_DiscountAmount) - (query.FirstOrDefault().TRTP_BalanceAmount);
                                data.TRHOB_ExcessAmount = excessamount;
                            }
                        }
                        //To Get the Booking Id
                        var bookid = _db.TripDMO.Single(t => t.TRTP_Id == data.TRTP_Id && t.MI_Id == data.MI_Id &&t.TRTP_ActiveFlg==true).TRTOB_Id;
                        // Saving data in TR_Trip_PaymentDMO.
                        TR_Trip_PaymentDMO dmo = new TR_Trip_PaymentDMO();
                        dmo.CreatedDate = DateTime.Now;
                        dmo.TRTOB_Id = bookid;
                        dmo.MI_Id = data.MI_Id;
                        dmo.TRTPP_ActiveFlag = true;
                        dmo.TRTPP_ChequeDDDate = data.TRTPP_ChequeDDDate;
                        dmo.TRTPP_ChequeDDNo = data.TRTPP_ChequeDDNo;
                        dmo.TRTPP_PaidAmount = data.TRTPP_PaidAmount;
                        dmo.TRTPP_PaymentMode = data.TRTPP_PaymentMode;
                        dmo.TRTPP_ReceiptDate = data.TRTPP_ReceiptDate;
                        dmo.TRTPP_ReceiptNo = data.TRTPP_ReceiptNo;
                        dmo.TRTPP_ReceiptReferenceNo = data.TRTPP_ReceiptReferenceNo;
                        dmo.TRTPP_BankName = data.TRTPP_BankName;

                        dmo.UpdatedDate = DateTime.Now;
                        _db.Add(dmo);

                        // Saving data in TR_Trip_Payment_TripsDMO.
                        TR_Trip_Payment_TripsDMO trips = new TR_Trip_Payment_TripsDMO();
                        trips.CreatedDate = DateTime.Now;
                        trips.TRTPPT_ActiveFlag = true;
                        trips.TRTPP_Id = dmo.TRTPP_Id;
                        trips.TRTP_Id = data.TRTP_Id;
                        trips.UpdatedDate = DateTime.Now;
                        _db.Add(trips);
                        _db.SaveChanges();

                        //Updating table TripDMO.
                        if (dmo.TRTPP_Id > 0)
                        {
                            var update = _db.TripDMO.Single(d => d.MI_Id == data.MI_Id && d.TRTP_Id == data.TRTP_Id);
                            if (query.Count > 0)
                            {
                                update.TRTP_PaidAmount = (query.FirstOrDefault().TRTP_PaidAmount) + (data.TRTPP_PaidAmount);
                                var total = (update.TRTP_BillAmount) - (update.TRTP_DiscountAmount + data.TRTP_DiscountAmount);
                                update.TRTP_TotalReceivable = (update.TRTP_BillAmount) - (update.TRTP_DiscountAmount + data.TRTP_DiscountAmount);
                                update.TRTP_BalanceAmount = (total) - (update.TRTP_PaidAmount);

                               
                               update.TRTP_DiscountAmount= (update.TRTP_DiscountAmount) + (data.TRTP_DiscountAmount);
                            }
                            else
                            {

                                var total = (update.TRTP_BillAmount) - (update.TRTP_DiscountAmount + data.TRTP_DiscountAmount);
                                update.TRTP_TotalReceivable = (update.TRTP_BillAmount) - (update.TRTP_DiscountAmount + data.TRTP_DiscountAmount);
                                update.TRTP_BalanceAmount = (total) - (update.TRTP_PaidAmount);
                                update.TRTP_PaidAmount = data.TRTPP_PaidAmount;
                                update.TRTP_DiscountAmount = update.TRTP_DiscountAmount + data.TRTP_DiscountAmount;
                            }
                            var balance = (update.TRTP_TotalReceivable) - (update.TRTP_PaidAmount);
                            if (balance > 0)
                            {
                                update.TRTP_BalanceAmount = balance;
                            }
                            else
                            {
                                update.TRTP_BalanceAmount = 0;
                            }

                            _db.Update(update);
                            var flag = _db.SaveChanges();
                            if (flag > 0)
                            {
                                data.returnVal = "payed";
                                if (data.TRHOB_ExcessAmount > 0)
                                {
                                    //Saving data in TR_Hirer_OB_DMO.
                                    var query2 = _db.TripDMO.Single(d => d.MI_Id == data.MI_Id && d.TRTP_Id == data.TRTP_Id);
                                    var hirer = _db.MasterHirerDMO.Where(d => d.TRMH_HirerName.Equals(query2.TRTP_HirerName)).ToList();
                                    TR_Hirer_OB_DMO ob = new TR_Hirer_OB_DMO();
                                    ob.CreatedDate = DateTime.Now;
                                    ob.MI_Id = data.MI_Id;
                                    ob.TRHOB_DueAmount = update.TRTP_BalanceAmount;
                                    if (query.Count > 0)
                                    {
                                        if (data.TRTPP_PaidAmount > query.FirstOrDefault().TRTP_BalanceAmount)
                                        {
                                            var excessamt = (data.TRTPP_PaidAmount) - (query.FirstOrDefault().TRTP_BalanceAmount);
                                            ob.TRHOB_ExcessAmount = excessamt;
                                            ob.TRHOB_ClearedFlag = true;
                                        }
                                    }

                                    ob.TRMH_Id = hirer.FirstOrDefault().TRMH_Id;
                                    ob.UpdatedDate = DateTime.Now;
                                    _db.Add(ob);
                                    _db.SaveChanges();
                                }

                            }
                            else
                            {
                                data.returnVal = "failed";
                            }

                        }
                    }
                }
                else if (data.SelectedRadioVal.Equals("hirer"))
                {
                    try
                    {
                        if(data.selectedBills.Length > 0)
                        {
                            foreach(TripDTO item  in data.selectedBills)
                            {
                                var query = (from m in _db.TripDMO
                                             from n in _db.TR_Trip_PaymentDMO
                                             from o in _db.TR_Trip_Payment_TripsDMO
                                             where m.MI_Id == n.MI_Id && n.TRTPP_Id == o.TRTPP_Id && m.TRTP_Id == o.TRTP_Id && m.MI_Id == data.MI_Id && o.TRTP_Id == item.TRTP_Id
                                             select new TripDTO
                                             {
                                                 TRTP_BillAmount = m.TRTP_BillAmount,
                                                 TRTP_TotalReceivable = m.TRTP_TotalReceivable,
                                                 TRTP_PaidAmount = m.TRTP_PaidAmount,
                                                 TRTP_BalanceAmount = m.TRTP_BalanceAmount,
                                                 TRTPP_PaidAmount = n.TRTPP_PaidAmount,
                                             }).ToList();
                                if (query.Count > 0 && query.FirstOrDefault().TRTP_BalanceAmount == 0)
                                {
                                    data.returnVal = "AmountPaid";
                                }
                                else
                                {
                                    if (query.Count > 0)
                                    {
                                        if (item.TRTPP_PaidAmount > query.FirstOrDefault().TRTP_BalanceAmount)
                                        {
                                            var excessamount = (item.TRTPP_PaidAmount) - (query.FirstOrDefault().TRTP_BalanceAmount);
                                            data.TRHOB_ExcessAmount = excessamount;
                                        }
                                    }

                                    //getting booking id 
                                    var bookid1 = _db.TripDMO.Single(t => t.TRTP_Id == item.TRTP_Id && t.MI_Id == data.MI_Id && t.TRTP_ActiveFlg == true).TRTOB_Id;
                                    // Saving data in TR_Trip_PaymentDMO.

                                    TR_Trip_PaymentDMO dmo = new TR_Trip_PaymentDMO();
                                    dmo.CreatedDate = DateTime.Now;
                                    dmo.MI_Id = data.MI_Id;
                                    dmo.TRTOB_Id = bookid1;
                                    dmo.TRTPP_ActiveFlag = true;
                                    dmo.TRTPP_ChequeDDDate = data.TRTPP_ChequeDDDate;
                                    dmo.TRTPP_ChequeDDNo = data.TRTPP_ChequeDDNo;
                                    dmo.TRTPP_PaidAmount = item.TRTPP_PaidAmount;
                                    dmo.TRTPP_PaymentMode = data.TRTPP_PaymentMode;
                                    dmo.TRTPP_ReceiptDate = data.TRTPP_ReceiptDate;
                                    dmo.TRTPP_ReceiptNo = data.TRTPP_ReceiptNo;
                                    dmo.TRTPP_ReceiptReferenceNo = data.TRTPP_ReceiptReferenceNo;
                                    dmo.UpdatedDate = DateTime.Now;
                                    _db.Add(dmo);

                                    // Saving data in TR_Trip_Payment_TripsDMO.
                                    TR_Trip_Payment_TripsDMO trips = new TR_Trip_Payment_TripsDMO();
                                    trips.CreatedDate = DateTime.Now;
                                    trips.TRTPPT_ActiveFlag = true;
                                    trips.TRTPP_Id = dmo.TRTPP_Id;
                                    trips.TRTP_Id = item.TRTP_Id;
                                    trips.UpdatedDate = DateTime.Now;
                                    _db.Add(trips);
                                    _db.SaveChanges();

                                    //Updating table TripDMO.
                                    if (dmo.TRTPP_Id > 0)
                                    {
                                        var update = _db.TripDMO.Single(d => d.MI_Id == data.MI_Id && d.TRTP_Id == item.TRTP_Id);
                                        if (query.Count > 0)
                                        {
                                            //update.TRTP_PaidAmount = (query.FirstOrDefault().TRTP_PaidAmount) + (item.TRTPP_PaidAmount);


                                            update.TRTP_PaidAmount = (query.FirstOrDefault().TRTP_PaidAmount) + (item.TRTPP_PaidAmount);
                                            var total = (update.TRTP_BillAmount) - (update.TRTP_DiscountAmount + item.TRTP_DiscountAmount);
                                            update.TRTP_TotalReceivable = (update.TRTP_BillAmount) - (update.TRTP_DiscountAmount + item.TRTP_DiscountAmount);
                                            update.TRTP_BalanceAmount = (total) - (update.TRTP_PaidAmount);


                                            update.TRTP_DiscountAmount = (update.TRTP_DiscountAmount) + (item.TRTP_DiscountAmount);
                                        }
                                        else
                                        {
                                            //update.TRTP_PaidAmount = item.TRTPP_PaidAmount;

                                            var total = (update.TRTP_BillAmount) - (update.TRTP_DiscountAmount + item.TRTP_DiscountAmount);
                                            update.TRTP_TotalReceivable = (update.TRTP_BillAmount) - (update.TRTP_DiscountAmount + item.TRTP_DiscountAmount);
                                            update.TRTP_BalanceAmount = (total) - (update.TRTP_PaidAmount);
                                            update.TRTP_PaidAmount = item.TRTPP_PaidAmount;
                                            update.TRTP_DiscountAmount = update.TRTP_DiscountAmount + item.TRTP_DiscountAmount;
                                        }
                                        var balance = (update.TRTP_TotalReceivable) - (update.TRTP_PaidAmount);
                                        if (balance > 0)
                                        {
                                            update.TRTP_BalanceAmount = balance;
                                        }
                                        else
                                        {
                                            update.TRTP_BalanceAmount = 0;
                                        }

                                        _db.Update(update);
                                        var flag = _db.SaveChanges();
                                        if (flag > 0)
                                        {
                                            data.returnVal = "payed";
                                            if (data.TRHOB_ExcessAmount > 0)
                                            {
                                                //Saving data in TR_Hirer_OB_DMO.
                                                var query2 = _db.TripDMO.Single(d => d.MI_Id == data.MI_Id && d.TRTP_Id == item.TRTP_Id);
                                                var hirer = _db.MasterHirerDMO.Where(d => d.TRMH_HirerName.Equals(query2.TRTP_HirerName)).ToList();
                                                TR_Hirer_OB_DMO ob = new TR_Hirer_OB_DMO();
                                                ob.CreatedDate = DateTime.Now;
                                                ob.MI_Id = data.MI_Id;
                                                ob.TRHOB_DueAmount = update.TRTP_BalanceAmount;
                                                if (query.Count > 0)
                                                {
                                                    if (item.TRTPP_PaidAmount > query.FirstOrDefault().TRTP_BalanceAmount)
                                                    {
                                                        var excessamt = (item.TRTPP_PaidAmount) - (query.FirstOrDefault().TRTP_BalanceAmount);
                                                        ob.TRHOB_ExcessAmount = excessamt;
                                                        ob.TRHOB_ClearedFlag = true;
                                                    }
                                                }

                                                ob.TRMH_Id = hirer.FirstOrDefault().TRMH_Id;
                                                ob.UpdatedDate = DateTime.Now;
                                                _db.Add(ob);
                                                _db.SaveChanges();
                                            }

                                        }
                                        else
                                        {
                                            data.returnVal = "failed";
                                        }

                                    }
                                }
                            }
                          
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
       public TripDTO GetTripDetails(TripDTO dt)
        {
            try
            {
                var vehicle = _context.Master_VehicleDMO.Where(d => d.MI_Id == dt.MI_Id && d.TRMV_ActiveFlag == true).Select(d => new MasterVehicleDTO { TRMV_Id = d.TRMV_Id, TRMV_VehicleNo = d.TRMV_VehicleNo }).ToList();
                if (vehicle.Count > 0)
                {
                    dt.vehicleList = vehicle.ToArray();
                }
                var driver = _context.MasterDriverDMO.Where(d => d.MI_Id == dt.MI_Id && d.TRMD_ActiveFlg == true).Select(d => new MasterDriverDTO {TRMD_Id=d.TRMD_Id,TRMD_DriverName=d.TRMD_DriverName }).ToList();

                if (driver.Count > 0)
                {
                    dt.driverList = driver.ToArray();
                }

                var tripdet = (from m in _context.TripDMO
                               from n in _context.MasterHirerGroupDMO
                               from r in _context.TripOnlineBookingDMO
                               from s in _context.MasterHirerDMO
                               from t in _context.TR_Trip_PaymentDMO
                               from u in _context.TR_Trip_Payment_TripsDMO
                               where m.TRTOB_Id == r.TRTOB_Id && m.TRHG_Id == n.TRHG_Id  && t.MI_Id==m.MI_Id && t.TRTPP_Id==u.TRTPP_Id && m.TRTP_Id == u.TRTP_Id && m.MI_Id == s.MI_Id && m.MI_Id == dt.MI_Id && m.TRTP_ActiveFlg == true && m.TRTP_BillGeneratedFlag == true && r.TRTOB_TripStatus.Equals("Applied")
                              group new { m, n,r,s,t,u }
                              by new { m.TRTP_Id} into g
                          
                               select new TripDTO
                               {
                                   TRTOB_Id = g.FirstOrDefault().m.TRTOB_Id,
                                   TRTP_Id = g.FirstOrDefault().m.TRTP_Id,
                                   TRTP_BillGeneratedFlag = g.FirstOrDefault().m.TRTP_BillGeneratedFlag,
                                   TRTP_AdvanceReceived = g.FirstOrDefault().m.TRTP_AdvanceReceived,
                                   TRTP_AdvancePaid=g.FirstOrDefault().m.TRTP_AdvancePaid,
                                   TRTP_TotalReceivable = g.FirstOrDefault().m.TRTP_TotalReceivable,
                                   TRTP_BookingDate = g.FirstOrDefault().m.TRTP_BookingDate,
                                   TRTP_TripDate = g.FirstOrDefault().m.TRTP_TripDate,
                                   TRTP_TripAddress = g.FirstOrDefault().m.TRTP_TripAddress,
                                   TRTP_TripFromDate = g.FirstOrDefault().m.TRTP_TripFromDate,
                                   TRTP_TripToDate = g.FirstOrDefault().m.TRTP_TripToDate,
                                   TRTP_HirerName = g.FirstOrDefault().m.TRTP_HirerName,
                                   TRTP_HirerContactNo = g.FirstOrDefault().m.TRTP_HirerContactNo,
                                   TRTP_BillNo = g.FirstOrDefault().m.TRTP_BillNo,
                                   TRTP_BillDate = g.FirstOrDefault().m.TRTP_BillDate,
                                   TRTP_BillAmount = g.FirstOrDefault().m.TRTP_BillAmount,
                                   TRTP_DiscountAmount = g.FirstOrDefault().m.TRTP_DiscountAmount,
                                   TRTP_PickUpLocation = g.FirstOrDefault().m.TRTP_PickUpLocation,
                                   TRMH_MobileNo = g.FirstOrDefault().s.TRMH_MobileNo,
                                   TRMH_EmailId = g.FirstOrDefault().s.TRMH_EmailId,
                                   TRMH_Id = g.FirstOrDefault().s.TRMH_Id,
                                   TRTOB_BookingId = g.FirstOrDefault().r.TRTOB_BookingId,
                                   TRTP_PaidAmount = g.FirstOrDefault().m.TRTP_PaidAmount,
                                   TRTP_BalanceAmount = g.FirstOrDefault().m.TRTP_BalanceAmount,
                               }).ToList();
                if (tripdet.Count > 0)
                {
                    dt.tripDetails = tripdet.ToArray();
                    var vhcleDvrAllottment = (from m in _context.TripOnlineBookingDMO
                                              from n in _context.TripDMO
                                              from o in _context.TRVehicleDriverAllottmentDMO
                                              where m.TRTOB_Id == n.TRTOB_Id && n.TRTP_Id == o.TRTP_Id && m.TRTOB_ActiveFlg == true && n.TRTP_ActiveFlg == true && n.MI_Id == dt.MI_Id && n.TRTP_Id == tripdet.FirstOrDefault().TRTP_Id
                                              select new TripDTO
                                              {
                                                  TRTP_Id = n.TRTP_Id,
                                                  TRMV_Id = o.TRMV_Id,
                                                  TRVD_Id = o.TRVD_Id,
                                                  TRTP_OpeningKM = Convert.ToInt64(o.TRTP_OpeningKM),
                                                  TRTP_ClosingKM = Convert.ToInt64(o.TRTP_ClosingKM),
                                                  TRVDA_Id = o.TRVDA_Id
                                              }).ToList();
                    if (vhcleDvrAllottment.Count > 0)
                    {
                        dt.vehicleDriverAllottmentList = vhcleDvrAllottment.ToArray();
                    }
                }
                string[] str = { "Approved", "Rejected" };
                var approvedTrips = (from m in _context.TripDMO
                                     from n in _context.MasterHirerGroupDMO
                                     from r in _context.TripOnlineBookingDMO
                                     from s in _context.MasterHirerDMO
                                     from t in _context.TR_Trip_PaymentDMO
                                     from u in _context.TR_Trip_Payment_TripsDMO
                                     where m.TRTOB_Id == r.TRTOB_Id && m.TRHG_Id == n.TRHG_Id  &&t.MI_Id==m.MI_Id && t.TRTPP_Id==u.TRTPP_Id && m.TRTP_Id == u.TRTP_Id && m.MI_Id == s.MI_Id && m.MI_Id == dt.MI_Id && m.TRTP_ActiveFlg == true && m.TRTP_BillGeneratedFlag == true && str.Contains(r.TRTOB_TripStatus)
                                     group new { m, n,r, s, t, u }
                             by new { m.TRTP_Id } into g
                                     select new TripDTO
                                     {
                                         TRTOB_Id =g.FirstOrDefault().m.TRTOB_Id,
                                         TRTP_Id = g.FirstOrDefault().m.TRTP_Id,
                                         TRTP_BillGeneratedFlag = g.FirstOrDefault().m.TRTP_BillGeneratedFlag,
                                         TRTP_AdvanceReceived = g.FirstOrDefault().m.TRTP_AdvanceReceived,
                                         TRTP_AdvancePaid=g.FirstOrDefault().m.TRTP_AdvancePaid,
                                         TRTP_TotalReceivable = g.FirstOrDefault().m.TRTP_TotalReceivable,
                                         TRTP_BookingDate = g.FirstOrDefault().m.TRTP_BookingDate,
                                         TRTP_TripDate = g.FirstOrDefault().m.TRTP_TripDate,
                                         TRTP_TripAddress = g.FirstOrDefault().m.TRTP_TripAddress,
                                         TRTP_TripFromDate = g.FirstOrDefault().m.TRTP_TripFromDate,
                                         TRTP_TripToDate = g.FirstOrDefault().m.TRTP_TripToDate,
                                         TRTP_HirerName = g.FirstOrDefault().m.TRTP_HirerName,
                                         TRTP_HirerContactNo = g.FirstOrDefault().m.TRTP_HirerContactNo,
                                         TRTP_BillNo = g.FirstOrDefault().m.TRTP_BillNo,
                                         TRTP_BillDate = g.FirstOrDefault().m.TRTP_BillDate,
                                         TRTP_BillAmount = g.FirstOrDefault().m.TRTP_BillAmount,
                                         TRTP_DiscountAmount = g.FirstOrDefault().m.TRTP_DiscountAmount,
                                         TRTP_PickUpLocation = g.FirstOrDefault().m.TRTP_PickUpLocation,
                                         TRMH_MobileNo = g.FirstOrDefault().s.TRMH_MobileNo,
                                         TRMH_EmailId = g.FirstOrDefault().s.TRMH_EmailId,
                                         TRMH_Id = g.FirstOrDefault().s.TRMH_Id,
                                         TRTOB_BookingId = g.FirstOrDefault().r.TRTOB_BookingId,
                                         TRTP_PaidAmount = g.FirstOrDefault().m.TRTP_PaidAmount,
                                         TRTP_BalanceAmount = g.FirstOrDefault().m.TRTP_BalanceAmount,
                                         TRTOB_TripStatus = g.FirstOrDefault().r.TRTOB_TripStatus
                                     }).Distinct().ToList();
                if (approvedTrips.Count > 0)
                {
                    dt.approvedTripList = approvedTrips.ToArray();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dt;
        }
        public TripDTO approveTrip(TripDTO ds)
        {
            try
            {
                var update = _context.TripDMO.Where(d => d.TRTP_Id == ds.TRTP_Id).ToList();
                var trtobId = update.FirstOrDefault().TRTOB_Id;
                var updatetrip = _context.TripOnlineBookingDMO.Single(d => d.TRTOB_Id == trtobId);
                updatetrip.TRTOB_TripStatus = "Approved";
                _context.Update(updatetrip);
              var flag =_context.SaveChanges();
                if(flag > 0)
                {
                    ds.returnVal = "approved";
                }
                else
                {
                    ds.returnVal = "failed";
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return ds;
        }
        public TripDTO rejectTrip(TripDTO ds)
        {
            try
            {
                var update = _context.TripDMO.Where(d => d.TRTP_Id == ds.TRTP_Id).ToList();
                var trtobId = update.FirstOrDefault().TRTOB_Id;
                var updatetrip = _context.TripOnlineBookingDMO.Single(d => d.TRTOB_Id == trtobId);
                updatetrip.TRTOB_TripStatus = "Rejected";
                _context.Update(updatetrip);
                var flag = _context.SaveChanges();
                if (flag > 0)
                {
                    ds.returnVal = "rejected";
                }
                else
                {
                    ds.returnVal = "failed";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return ds;
        }
        public TripDTO viewDetails(TripDTO data)
        {
            try
            {
                var triplist = (from m in _context.TripDMO
                                from n in _context.MasterHirerGroupDMO
                                from r in _context.TripOnlineBookingDMO
                                where m.TRTOB_Id == r.TRTOB_Id && m.TRHG_Id == n.TRHG_Id && m.MI_Id == data.MI_Id && m.TRTP_ActiveFlg == true && m.TRTP_Id == data.TRTP_Id
                                select new TripDTO
                                {
                                    TRTOB_Id = m.TRTOB_Id,
                                    TRTP_Id = m.TRTP_Id,
                                    TRTP_BillGeneratedFlag = m.TRTP_BillGeneratedFlag,
                                    TRTP_AdvanceReceived = m.TRTP_AdvanceReceived,
                                    TRTP_AdvancePaid = m.TRTP_AdvancePaid,
                                    TRTP_TotalReceivable = m.TRTP_TotalReceivable,
                                    TRTP_BookingDate = m.TRTP_BookingDate,
                                    TRTP_TripDate = m.TRTP_TripDate,
                                    TRTP_TripAddress = m.TRTP_TripAddress,
                                    TRTP_TripFromDate = m.TRTP_TripFromDate,
                                    TRTP_TripToDate = m.TRTP_TripToDate,
                                    TRTP_HirerName = m.TRTP_HirerName,
                                    TRTP_HirerContactNo = m.TRTP_HirerContactNo,
                                    TRTP_BillNo = m.TRTP_BillNo,
                                    TRTP_BillDate = m.TRTP_BillDate,
                                    TRTP_BillAmount = m.TRTP_BillAmount,
                                    TRTP_DiscountAmount = m.TRTP_DiscountAmount,
                                    TRTP_PickUpLocation = m.TRTP_PickUpLocation,
                                    TRTP_PaidAmount = m.TRTP_PaidAmount,
                                    TRTP_BalanceAmount = m.TRTP_BalanceAmount
                                }
                                ).ToList();
                if (triplist.Count > 0)
                {
                    data.tripList = triplist.ToArray();
                    var vhcleDvrAllottment = (from m in _context.TripOnlineBookingDMO
                                              from n in _context.TripDMO
                                              from o in _context.TRVehicleDriverAllottmentDMO
                                              where m.TRTOB_Id == n.TRTOB_Id && n.TRTP_Id == o.TRTP_Id && m.TRTOB_ActiveFlg == true && n.TRTP_ActiveFlg == true && n.MI_Id == data.MI_Id && n.TRTP_Id == triplist.FirstOrDefault().TRTP_Id
                                              select new TripDTO
                                              {
                                                  TRTP_Id = n.TRTP_Id,
                                                  TRMV_Id = o.TRMV_Id,
                                                  TRVD_Id = o.TRVD_Id,
                                                  TRTP_OpeningKM = Convert.ToInt64(o.TRTP_OpeningKM),
                                                  TRTP_ClosingKM = Convert.ToInt64(o.TRTP_ClosingKM),
                                                  TRVDA_Id = o.TRVDA_Id
                                              }).ToList();
                    if (vhcleDvrAllottment.Count > 0)
                    {
                        data.vehicleDriverAllottmentList = vhcleDvrAllottment.ToArray();
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public TripDTO printrecept(TripDTO data)
        {

            data.instname = _context.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TRN_PRINT_BILL_RECEPT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TRVD_TripId",
                     SqlDbType.VarChar)
                    {
                        Value = data.TRVD_TripId
                    });
                    cmd.Parameters.Add(new SqlParameter("@TRTPP_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.TRTPP_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while ( dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }

                        }
                        data.receptprint = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex ;
            }
            return data;
        }

        public TripDTO printbill(TripDTO data)
        {

            data.instname = _context.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TRN_PRINT_BILL";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TRTP_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.TRTP_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TRTOB_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.TRTOB_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.printbilldata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

        
        public TripDTO printtripsheet(TripDTO data)
        {

            data.instname = _context.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TRN_TRIPSHEET_PRINT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                   
                    cmd.Parameters.Add(new SqlParameter("@TRTP_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.TRTP_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.tripsheetprint = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        
       public TripDTO deletetrip(TripDTO data)
        {
            try
            {
                var deletetrip= _context.TripDMO.Where(t => t.TRTP_Id == data.TRTP_Id).ToList();
                if (deletetrip.Any())
                {
                    foreach (var tt in deletetrip)
                    {
                        _context.Remove(tt);
                    }
                }

                var deletedriverallt = _context.TRVehicleDriverAllottmentDMO.Where(t => t.TRTP_Id == data.TRTP_Id).ToList();
                if (deletedriverallt.Any())
                {
                    foreach (var yy in deletedriverallt)
                    {
                        _context.Remove(yy);
                    }
                }
                var deletestatus = _context.SaveChanges();
                if (deletestatus > 0)
                {
                    data.returnVal = "success";
                }
                else
                {
                    data.returnVal = "fail";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
    }
}
