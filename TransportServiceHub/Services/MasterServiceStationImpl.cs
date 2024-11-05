using PreadmissionDTOs.com.vaps.Transport;
using System;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DomainModel.Model.com.vapstech.Transport;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using CommonLibrary;
using PreadmissionDTOs;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace TransportServiceHub.Services
{
    public class MasterServiceStationImpl : Interfaces.MasterServiceStationInterface
    {
        public TransportContext _context;


        DomainModelMsSqlServerContext _db;

        public MasterServiceStationImpl(TransportContext context, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _db = db;
        }
        //Master Hirer Group.
        public ServiceStationDTO getrptparam(ServiceStationDTO obj)
        {
            try
            {
                obj.fillvahicletype = _context.MasterVehicleTypeDMO.Where(t => t.MI_Id == obj.MI_Id && t.TRMVT_ActiveFlg == true).ToArray();
                obj.servnamelist = _context.TR_Master_ServStationDMO.Where(t => t.MI_Id == obj.MI_Id && t.TRMSST_ActiveFlag == true).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public ServiceStationDTO Getreportdetails(ServiceStationDTO data)
        {

            try { 
       
                string vehicleids = "0";
                List<long> HeadId = new List<long>();


                foreach (var item in data.vhlid)
                {
                    HeadId.Add(item.TRMV_Id);
                }


                for (int i = 0; i < HeadId.Count; i++)
                {
                    vehicleids = vehicleids + "," + HeadId[i];
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TR_GET_SERVICEDETAILSREPOT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TRMSST_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.TRMSST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TRMV_Id",
                     SqlDbType.VarChar)
                    {
                        Value = vehicleids
                    });
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE",
                   SqlDbType.Date)
                    {
                        Value = data.FRMDATE
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE",
                   SqlDbType.Date)
                    {
                        Value = data.TODATE
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                   SqlDbType.Bit)
                    {
                        Value = data.statuscount
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
                        data.requisitionlistold = retObject.ToArray();
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


        public ServiceStationDTO Servicebillload(ServiceStationDTO data)
        {
            try
            {
                var vahiclelist = _context.Master_VehicleDMO.Where(d => d.MI_Id == data.MI_Id && d.TRMV_ActiveFlag == true).Select(d => new ServiceStationDTO { TRMV_Id = d.TRMV_Id, TRMV_VehicleNo = d.TRMV_VehicleNo }).ToList();
                if (vahiclelist.Count > 0)
                {
                    data.vehicaldata = vahiclelist.ToArray();
                }
                var driverlist = _context.MasterDriverDMO.Where(d => d.MI_Id == data.MI_Id && d.TRMD_ActiveFlg == true).Select(d => new ServiceStationDTO { TRMD_Id = d.TRMD_Id, TRMD_DriverName = d.TRMD_DriverName }).ToList();
                if (driverlist.Count > 0)
                {
                    data.driverdata = driverlist.ToArray();
                }

                var stationlist = _context.TR_Master_ServStationDMO.Where(d => d.MI_Id == data.MI_Id && d.TRMSST_ActiveFlag == true).Select(d => new ServiceStationDTO { TRMSST_Id = d.TRMSST_Id, TRMSST_ServiceStationName = d.TRMSST_ServiceStationName }).ToList();
                if (stationlist.Count > 0)
                {
                    data.servnamelist = stationlist.ToArray();
                }

                data.servicenolist = (from a in _context.TR_ServiceDMO
                                      where a.MI_Id == data.MI_Id && a.TRSE_ActiveFlag == true
                                      select new ServiceStationDTO
                                      {
                                          TRSE_Id = a.TRSE_Id,
                                          TRSE_ServiceRefNo = a.TRSE_ServiceRefNo,
                                          TRSE_ServiceDate = a.TRSE_ServiceDate
                                      }).Distinct().OrderByDescending(f => f.TRSE_ServiceDate).ToArray();

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;

        }


             public ServiceStationDTO duprecpcheck(ServiceStationDTO data)
        {
            try
            {
                var dupbill = _context.TR_ServiceDMO.Where(f => f.MI_Id == data.MI_Id && f.TRSE_Id != data.TRSE_Id && f.TRSE_BillNo.Trim() == data.TRSE_BillNo.Trim()).ToList();
                if (dupbill.Count>0)
                {
                    data.returnVal = "dup";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        public ServiceStationDTO findservice(ServiceStationDTO data)
        {
            try
            {
                if (data.TRMSST_Id == 0 && data.TRMV_Id==0)
                {
                    data.servicenolist = (from a in _context.TR_ServiceDMO
                                          where a.MI_Id == data.MI_Id && a.TRSE_ActiveFlag == true
                                          select new ServiceStationDTO
                                          {
                                              TRSE_Id = a.TRSE_Id,
                                              TRSE_ServiceRefNo = a.TRSE_ServiceRefNo,
                                              TRSE_ServiceDate = a.TRSE_ServiceDate
                                          }).Distinct().OrderByDescending(f => f.TRSE_ServiceDate).ToArray();
                }
                else if (data.TRMSST_Id == 0 && data.TRMV_Id > 0)
                {
                    data.servicenolist = (from a in _context.TR_ServiceDMO
                                          where a.MI_Id == data.MI_Id && a.TRSE_ActiveFlag == true && a.TRMV_Id==data.TRMV_Id
                                          select new ServiceStationDTO
                                          {
                                              TRSE_Id = a.TRSE_Id,
                                              TRSE_ServiceRefNo = a.TRSE_ServiceRefNo,
                                              TRSE_ServiceDate = a.TRSE_ServiceDate
                                          }).Distinct().OrderByDescending(f => f.TRSE_ServiceDate).ToArray();
                }
                else if (data.TRMSST_Id > 0 && data.TRMV_Id == 0)
                {
                    data.servicenolist = (from a in _context.TR_ServiceDMO
                                          where a.MI_Id == data.MI_Id && a.TRSE_ActiveFlag == true && a.TRMSST_Id == data.TRMSST_Id
                                          select new ServiceStationDTO
                                          {
                                              TRSE_Id = a.TRSE_Id,
                                              TRSE_ServiceRefNo = a.TRSE_ServiceRefNo,
                                              TRSE_ServiceDate = a.TRSE_ServiceDate
                                          }).Distinct().OrderByDescending(f => f.TRSE_ServiceDate).ToArray();
                }
                else if (data.TRMSST_Id > 0 && data.TRMV_Id > 0)
                {
                    data.servicenolist = (from a in _context.TR_ServiceDMO
                                          where a.MI_Id == data.MI_Id && a.TRSE_ActiveFlag == true && a.TRMSST_Id == data.TRMSST_Id && a.TRMV_Id == data.TRMV_Id
                                          select new ServiceStationDTO
                                          {
                                              TRSE_Id = a.TRSE_Id,
                                              TRSE_ServiceRefNo = a.TRSE_ServiceRefNo,
                                              TRSE_ServiceDate = a.TRSE_ServiceDate
                                          }).Distinct().OrderByDescending(f => f.TRSE_ServiceDate).ToArray();
                }



            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;

        }


        public ServiceStationDTO get_srvdetails(ServiceStationDTO data)
        {
            try
            {
                data.serivelist = (from a in _context.TR_ServiceDMO
                                   from b in _context.TR_Master_ServStationDMO
                                   from c in _context.Master_VehicleDMO
                                   from d in _context.MasterDriverDMO
                                   where a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && a.TRMSST_Id == b.TRMSST_Id && a.TRMV_Id == c.TRMV_Id && a.TRMD_Id == d.TRMD_Id && a.TRSE_Id == data.TRSE_Id
                                   select new ServiceStationDTO
                                   {
                                       TRSE_Id = a.TRSE_Id,
                                       TRMD_DriverName = d.TRMD_DriverName,
                                       TRMV_VehicleNo = c.TRMV_VehicleNo,
                                       TRMSST_ServiceStationName = b.TRMSST_ServiceStationName,
                                       TRSE_ServiceRefNo = a.TRSE_ServiceRefNo,
                                       TRSE_ProblemsListed = a.TRSE_ProblemsListed,
                                       TRSE_ServiceDetails = a.TRSE_ServiceDetails,
                                       TRSE_BillNo = a.TRSE_BillNo,
                                       TRSE_LabourCharges = a.TRSE_LabourCharges,
                                       TRSE_ItemsCost = a.TRSE_ItemsCost,
                                       TRSE_BillDate = a.TRSE_BillDate,
                                       TRSE_ServiceDate = a.TRSE_ServiceDate,
                                       TRSE_TotalBillValue = a.TRSE_TotalBillValue,
                                       TRSE_TotalDiscount = a.TRSE_TotalDiscount,
                                       TRSE_TaxValue = a.TRSE_TaxValue,
                                       TRSE_TDSValue = a.TRSE_TDSValue,
                                       TRSE_TotalPaid = a.TRSE_TotalPaid,
                                       TRSE_ActiveFlag = a.TRSE_ActiveFlag
                                   }).Distinct().ToArray();


                data.itemlist = _context.TR_Service_DetailsDMO.Where(e => e.TRSE_Id == data.TRSE_Id && e.TRSED_ActiveFlag == true).Distinct().ToArray();

                data.paymentlist = (from a in _context.TR_ServiceDMO
                                    from b in _context.TR_Service_PayementDMO
                                    where a.MI_Id == data.MI_Id && a.TRSE_Id == b.TRSE_Id && a.TRSE_Id == data.TRSE_Id && a.TRSE_ActiveFlag==true

                                    select new ServiceStationDTO
                                    {
                                        TRSE_Id = a.TRSE_Id,
                                        TRSEP_Id=b.TRSEP_Id,
                                        TRSEP_BankName=b.TRSEP_BankName,
                                        TRSEP_ModeOfPayment=b.TRSEP_ModeOfPayment,
                                        TRSEP_ActiveFlag=b.TRSEP_ActiveFlag,
                                        TRSEP_Amount=b.TRSEP_Amount,
                                        TRSEP_ChequeDDDate=b.TRSEP_ChequeDDDate,
                                        TRSEP_ChequeDDNo=b.TRSEP_ChequeDDNo,
                                        TRSE_BillDate=a.TRSE_BillDate,
                                        TRSEP_TransactionRefNo=b.TRSEP_TransactionRefNo,
                                        TRSE_BillNo=a.TRSE_BillNo,
                                        TRSEP_PaymentDate=b.TRSEP_PaymentDate

                                    }).Distinct().ToArray();



          


            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

        public ServiceStationDTO saveBilldata(ServiceStationDTO data)
        {
            try
            {
                var res = _context.TR_ServiceDMO.Single(z => z.MI_Id == data.MI_Id && z.TRSE_Id == data.TRSE_Id);


                res.TRSE_ProblemsListed = data.TRSE_ProblemsListed;
                res.TRSE_ServiceDetails = data.TRSE_ServiceDetails;
                res.TRSE_BillNo = data.TRSE_BillNo;
                res.TRSE_LabourCharges = data.TRSE_LabourCharges;
                res.TRSE_ItemsCost = data.TRSE_ItemsCost;
                res.TRSE_BillDate = data.TRSE_BillDate;
                res.TRSE_ServiceDate = data.TRSE_ServiceDate;
                res.TRSE_TotalBillValue = data.TRSE_TotalBillValue;
                res.TRSE_TotalDiscount = data.TRSE_TotalDiscount;
                res.TRSE_TaxValue = data.TRSE_TaxValue;
                res.TRSE_TDSValue = data.TRSE_TDSValue;
                res.TRSE_TotalPaid = data.TRSE_TotalPaid;
                res.TRSE_UpdatedBy = data.USER_Id;
                res.UpdatedDate = DateTime.Now;
                _context.Update(res);
                var removed = _context.TR_Service_DetailsDMO.Where(s => s.TRSE_Id == data.TRSE_Id).ToList();
                if (removed.Count>0)
                {
                    foreach (var item in removed)
                    {
                        _context.Remove(item);
                    }
                }

                if (data.allotteditems != null)
                {
                    if (data.allotteditems.Length > 0)
                    {


                        for (int i = 0; i < data.allotteditems.Length; i++)
                        {
                            TR_Service_DetailsDMO obj = new TR_Service_DetailsDMO();
                            obj.TRSE_Id = data.TRSE_Id;
                            obj.TRSED_ItemsName = data.allotteditems[i].TRSED_ItemsName;
                            obj.TRSED_Qty = data.allotteditems[i].TRSED_Qty;
                            obj.TRSED_Remarks = data.allotteditems[i].TRSED_Remarks;
                            obj.TRSED_ProblemsListed = data.allotteditems[i].TRSED_ProblemsListed;
                            obj.TRSED_ServiceDetails = data.allotteditems[i].TRSED_ServiceDetails;
                            obj.TRSED_Amount= data.allotteditems[i].TRSED_Amount;
                            obj.TRSED_TotalDiscount= data.allotteditems[i].TRSED_TotalDiscount;
                            obj.TRSED_TotalAmount= data.allotteditems[i].TRSED_TotalAmount;
  
                            obj.TRSED_ActiveFlag = true;
                            obj.TRSED_CreatedBy = data.USER_Id;
                            obj.TRSED_UpdatedBy = data.USER_Id;
                            obj.CreatedDate = DateTime.Now;
                            obj.UpdatedDate = DateTime.Now;
                            _context.Add(obj);
                        }


                    }

                }


                var update = _context.SaveChanges();
                if (update > 0)
                {
                    data.returnVal = "update";
                    data.retval = true;
                }
                else
                {
                    data.retval = false;
                    data.returnVal = "update";
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

        public ServiceStationDTO PayBill(ServiceStationDTO data)
        {
            try
            {
                data.payingdatalist = _context.TR_ServiceDMO.Where(t => t.MI_Id == data.MI_Id && t.TRSE_Id == data.TRSE_Id).ToArray();
                var paymentMode = _db.IVRM_ModeOfPaymentDMO.Where(d => d.MI_Id == data.MI_Id && d.IVRMMOD_ActiveFlag == true).ToList();
                if (paymentMode.Count > 0)
                {
                    data.modeOfPaymentList = paymentMode.ToArray();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        public ServiceStationDTO FinalPayBill(ServiceStationDTO data)
        {
            try
            {

                var result= _context.TR_ServiceDMO.Single(t => t.MI_Id == data.MI_Id && t.TRSE_Id == data.TRSE_Id);

               // result.TRSE_TotalDiscount = result.TRSE_TotalDiscount + data.TRSE_TotalDiscount;
                result.TRSE_TotalPaid = result.TRSE_TotalPaid + data.TRSEP_Amount;

                result.TRSE_UpdatedBy = data.USER_Id;
                result.UpdatedDate = DateTime.Now;
                _context.Update(result);

                TR_Service_PayementDMO sp = new TR_Service_PayementDMO();
                sp.TRSE_Id = data.TRSE_Id;
                sp.TRSEP_ModeOfPayment = data.TRSEP_ModeOfPayment;
                sp.TRSEP_TransactionRefNo = data.TRSEP_TransactionRefNo;
                sp.TRSEP_ChequeDDNo = data.TRSEP_ChequeDDNo;
                sp.TRSEP_ChequeDDDate = data.TRSEP_ChequeDDDate;
                sp.TRSEP_PaymentDate = data.TRSEP_PaymentDate;
                sp.TRSEP_Amount = data.TRSEP_Amount;
                sp.TRSEP_BankName = data.TRSEP_BankName;
                sp.TRSEP_ActiveFlag = true;
                sp.TRSEP_CreatedBy = data.USER_Id;
                sp.TRSEP_UpdatedBy = data.USER_Id;
                sp.UpdatedDate = DateTime.Now;
                sp.UpdatedDate = DateTime.Now;
                _context.Add(sp);
                var flag = _context.SaveChanges();
                if (flag > 0)
                {
                    data.returnVal = "Add";
                    data.retval = true;
                }
                else
                {
                    data.returnVal = "Add";
                    data.retval = false;
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        public ServiceStationDTO delete_rec(ServiceStationDTO data)
        {
            try
            {
                var pay_delete = _context.TR_Service_PayementDMO.Single(d => d.TRSEP_Id == data.TRSEP_Id && d.TRSE_Id == data.TRSE_Id);

                var result= _context.TR_ServiceDMO.Single(t => t.MI_Id == data.MI_Id && t.TRSE_Id == data.TRSE_Id);

               // result.TRSE_TotalDiscount = 0;
                result.TRSE_TotalPaid = result.TRSE_TotalPaid - pay_delete.TRSEP_Amount;

                result.TRSE_UpdatedBy = data.USER_Id;
                result.UpdatedDate = DateTime.Now;
                _context.Update(result);

               

               
                _context.Remove(pay_delete);
                var flag = _context.SaveChanges();
                if (flag > 0)
                {
                    data.returnVal = "del";
                    data.retval = true;
                }
                else
                {
                    data.returnVal = "n";
                    data.retval = false;
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

        //Master Parts
        public ServiceStationDTO getpartsdata(ServiceStationDTO obj)
        {
            try
            {
                var vahiclelist = _context.Master_VehicleDMO.Where(d => d.MI_Id == obj.MI_Id && d.TRMV_ActiveFlag == true).Select(d => new ServiceStationDTO { TRMV_Id = d.TRMV_Id, TRMV_VehicleNo = d.TRMV_VehicleNo }).ToList();
                if (vahiclelist.Count > 0)
                {
                    obj.vehicaldata = vahiclelist.ToArray();
                }
                var driverlist = _context.MasterDriverDMO.Where(d => d.MI_Id == obj.MI_Id && d.TRMD_ActiveFlg == true).Select(d => new ServiceStationDTO { TRMD_Id = d.TRMD_Id, TRMD_DriverName = d.TRMD_DriverName }).ToList();
                if (driverlist.Count > 0)
                {
                    obj.driverdata = driverlist.ToArray();
                }

                var stationlist = _context.TR_Master_ServStationDMO.Where(d => d.MI_Id == obj.MI_Id && d.TRMSST_ActiveFlag == true).Select(d => new ServiceStationDTO { TRMSST_Id = d.TRMSST_Id, TRMSST_ServiceStationName = d.TRMSST_ServiceStationName }).ToList();
                if (stationlist.Count > 0)
                {
                    obj.servnamelist = stationlist.ToArray();
                }

                var parttypelist = _context.TR_PartperticularTypeDMO.Where(d => d.MI_Id == obj.MI_Id && d.TRPAPT_ActiveFlag == true).Select(d => new ServiceStationDTO { TRPAPT_Id = d.TRPAPT_Id, TRPAPT_PType = d.TRPAPT_PType }).ToList();
                if (parttypelist.Count > 0)
                {
                    obj.parttypedropdown = parttypelist.ToArray();
                }

                var query = (from m in _context.TR_ServiceDMO
                             from n in _context.MasterDriverDMO
                             from o in _context.Master_VehicleDMO
                             from p in _context.TR_Master_ServStationDMO
                             where m.MI_Id == n.MI_Id && m.MI_Id == o.MI_Id && m.MI_Id == p.MI_Id && m.TRMD_Id == n.TRMD_Id && m.TRMV_Id == o.TRMV_Id && m.TRMSST_Id == p.TRMSST_Id
                             select new ServiceStationDTO
                             {
                                 TRSE_Id = m.TRSE_Id,
                                 TRMD_Id = m.TRMD_Id,
                                 TRMD_DriverName = n.TRMD_DriverName,
                                 TRMV_Id = m.TRMV_Id,
                                 TRMV_VehicleNo = o.TRMV_VehicleNo,
                                 TRSE_ServiceDate = m.TRSE_ServiceDate,
                                 TRSE_ProblemsListed = m.TRSE_ProblemsListed,
                                 TRSE_ServiceDetails = m.TRSE_ServiceDetails,
                                 TRSE_ServiceRefNo = m.TRSE_ServiceRefNo,
                                 TRSE_ActiveFlag = m.TRSE_ActiveFlag,
                                 TRSE_TotalPaid = m.TRSE_TotalPaid,
                                 TRMSST_ServiceStationName = p.TRMSST_ServiceStationName,
                                 //  TRPAPT_Id = q.TRPAPT_Id
                             }).OrderByDescending(w=>w.TRSE_ServiceDate).ToList();


                if (query.Count > 0)
                {
                    obj.partlist = query.Distinct().ToArray();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public ServiceStationDTO savepartsdata(ServiceStationDTO data)
        {
            try
            {
              //  data.ASMAY_Id = 11;
                if (data.TRSE_Id == 0)
                {

                    var transnumconfigsettings = _db.Master_Numbering.Where(d => d.MI_Id == data.MI_Id && d.IMN_Flag.Equals("ServiceRefNo1")).ToList();
                    if (transnumconfigsettings.Count > 0)
                    {
                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                        Master_NumberingDTO num = new Master_NumberingDTO();
                        num.MI_Id = data.MI_Id;
                        num.ASMAY_Id = data.ASMAY_Id;
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
                        num.IMN_Flag = "ServiceRefNo";
                        data.TRSE_ServiceRefNo = a.GenerateNumber(num);
                    }
                    else
                    {
                        var query = _context.TR_ServiceDMO.Where(d => d.MI_Id == data.MI_Id).ToList();
                        if (query.Count > 0)
                        {
                            long TRSE_Id = _context.TR_ServiceDMO.Where(d => d.MI_Id == data.MI_Id).Max(d => d.TRSE_Id);
                            var query1 = _context.TR_ServiceDMO.Where(d => d.MI_Id == data.MI_Id && d.TRSE_Id == TRSE_Id).Select(d => d.TRSE_ServiceRefNo).ToList();
                            string str = query1.FirstOrDefault();
                            string[] strarr = str.Split('/');
                            int number = Convert.ToInt32(strarr[0]);
                            data.TRSE_ServiceRefNo = (number + 1) + "/" + data.TRSE_ServiceDate.Date.ToString("dd-MM-yyyy");

                        }
                        else
                        {
                            data.TRSE_ServiceRefNo = 1 + "/" + data.TRSE_ServiceDate.Date.ToString("dd-MM-yyyy");
                        }
                    }


                    // data.TRSE_ServiceRefNo= data.TRSE_ServiceRefNo+"/"+data.TRSE_ServiceDate.Date.ToString("dd-MM-yyyy");

                    var duplicateCheck = _context.TR_ServiceDMO.Where(d => d.MI_Id == data.MI_Id && d.TRSE_ServiceRefNo.Trim() == data.TRSE_ServiceRefNo.Trim()).ToList();
                    if (duplicateCheck.Count > 0)
                    {
                        data.returnVal = "duplicate";
                    }
                    else
                    {
                        TR_ServiceDMO mapp2 = new TR_ServiceDMO();
                        mapp2.MI_Id = data.MI_Id;
                        mapp2.TRMSST_Id = data.TRMSST_Id;
                        mapp2.TRSE_ServiceStationName = data.TRSE_ServiceStationName;
                        mapp2.TRMV_Id = data.TRMV_Id;
                        mapp2.TRMD_Id = data.TRMD_Id;
                        mapp2.TRSE_ServiceRefNo = data.TRSE_ServiceRefNo.Trim();
                        mapp2.TRSE_ProblemsListed = data.TRSE_ProblemsListed;
                        mapp2.TRSE_ServiceDetails = data.TRSE_ServiceDetails;
                        mapp2.TRSE_ServiceDate = data.TRSE_ServiceDate;
                        mapp2.TRSE_ActiveFlag = true;
                        mapp2.TRSE_CreatedBy = data.USER_Id;
                        mapp2.TRSE_UpdatedBy = data.USER_Id;
                        mapp2.CreatedDate = DateTime.Now;
                        mapp2.UpdatedDate = DateTime.Now;

                        _context.Add(mapp2);
                        if (data.allotteditems != null)
                        {
                            if (data.allotteditems.Length > 0)
                            {


                                for (int i = 0; i < data.allotteditems.Length; i++)
                                {
                                    TR_Service_DetailsDMO obj = new TR_Service_DetailsDMO();
                                    obj.TRSE_Id = mapp2.TRSE_Id;
                                    obj.TRSED_ItemsName = data.allotteditems[i].TRSED_ItemsName;
                                    obj.TRSED_Qty = data.allotteditems[i].TRSED_Qty;
                                    obj.TRSED_Remarks = data.allotteditems[i].TRSED_Remarks;
                                    obj.TRSED_ProblemsListed = data.allotteditems[i].TRSED_ProblemsListed;
                                    obj.TRSED_ServiceDetails = data.allotteditems[i].TRSED_ServiceDetails;
                                    obj.TRSED_ActiveFlag = true;
                                    obj.TRSED_CreatedBy = data.USER_Id;
                                    obj.TRSED_UpdatedBy = data.USER_Id;
                                    obj.CreatedDate = DateTime.Now;
                                    obj.UpdatedDate = DateTime.Now;
                                    _context.Add(obj);
                                }


                            }

                        }


                        var flag = _context.SaveChanges();
                        if (flag > 0)
                        {
                            data.returnVal = "Add";
                            data.retval = true;
                        }
                        else
                        {
                            data.returnVal = "Add";
                            data.retval = false;
                        }

                    }
                }
                else
                {

                    var result = _context.TR_ServiceDMO.Single(d => d.TRSE_Id == data.TRSE_Id);
                    result.MI_Id = data.MI_Id;
                    result.TRMSST_Id = data.TRMSST_Id;
                    result.TRSE_ServiceStationName = data.TRSE_ServiceStationName;
                    result.TRMV_Id = data.TRMV_Id;
                    result.TRMD_Id = data.TRMD_Id;
                    result.TRSE_ProblemsListed = data.TRSE_ProblemsListed;
                    result.TRSE_ServiceDetails = data.TRSE_ServiceDetails;
                    result.TRSE_ServiceDate = data.TRSE_ServiceDate;
                    //    result.TRSE_ActiveFlag = true;
                    result.TRSE_UpdatedBy = data.USER_Id;
                    result.UpdatedDate = DateTime.Now;
                    _context.Update(result);

                    var deleteitems = _context.TR_Service_DetailsDMO.Where(d => d.TRSE_Id == data.TRSE_Id).ToList();
                    if (deleteitems.Count > 0)
                    {
                        foreach (var item in deleteitems)
                        {
                            _context.Remove(item);
                        }

                    }
                    if (data.allotteditems != null)
                    {
                        if (data.allotteditems.Length > 0)
                        {


                            for (int i = 0; i < data.allotteditems.Length; i++)
                            {
                                TR_Service_DetailsDMO obj = new TR_Service_DetailsDMO();
                                obj.TRSE_Id = data.TRSE_Id;
                                obj.TRSED_ItemsName = data.allotteditems[i].TRSED_ItemsName;
                                obj.TRSED_Qty = data.allotteditems[i].TRSED_Qty;
                                obj.TRSED_Remarks = data.allotteditems[i].TRSED_Remarks;
                                obj.TRSED_ProblemsListed = data.allotteditems[i].TRSED_ProblemsListed;
                                obj.TRSED_ServiceDetails = data.allotteditems[i].TRSED_ServiceDetails;
                                obj.TRSED_ActiveFlag = true;
                                obj.TRSED_CreatedBy = data.USER_Id;
                                obj.TRSED_UpdatedBy = data.USER_Id;
                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;
                                _context.Add(obj);
                            }


                        }

                    }


                    var update = _context.SaveChanges();
                    if (update > 0)
                    {
                        data.returnVal = "update";
                        data.retval = true;
                    }
                    else
                    {
                        data.retval = false;
                        data.returnVal = "update";
                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }
        public ServiceStationDTO editpartsdata(int id)
        {
            ServiceStationDTO obj = new ServiceStationDTO();
            try
            {
                /// obj.editparts = _context.TR_ServiceDMO.Where(d => d.TRSE_Id == id).ToArray();
                obj.editparts = (from a in _context.TR_ServiceDMO
                                 from b in _context.Master_VehicleDMO
                                 where a.TRSE_Id == id && a.TRMV_Id == b.TRMV_Id
                                 select new ServiceStationDTO
                                 {
                                     TRSE_Id = a.TRSE_Id,

                                     TRMSST_Id = a.TRMSST_Id,
                                     TRSE_ServiceStationName = a.TRSE_ServiceStationName,
                                     TRMV_Id = a.TRMV_Id,
                                     TRMD_Id = a.TRMD_Id,
                                     TRSE_ServiceRefNo = a.TRSE_ServiceRefNo,
                                     TRSE_ProblemsListed = a.TRSE_ProblemsListed,
                                     TRSE_ServiceDetails = a.TRSE_ServiceDetails,
                                     TRSE_BillNo = a.TRSE_BillNo,
                                     TRSE_LabourCharges = a.TRSE_LabourCharges,
                                     TRSE_ItemsCost = a.TRSE_ItemsCost,
                                     TRSE_BillDate = a.TRSE_BillDate,
                                     TRSE_ServiceDate = a.TRSE_ServiceDate,
                                     TRSE_TotalBillValue = a.TRSE_TotalBillValue,
                                     TRSE_TotalDiscount = a.TRSE_TotalDiscount,
                                     TRSE_TaxValue = a.TRSE_TaxValue,
                                     TRSE_TDSValue = a.TRSE_TDSValue,
                                     TRSE_TotalPaid = a.TRSE_TotalPaid,
                                     TRMV_VehicleNo = b.TRMV_VehicleNo
                                 }
                                 ).Distinct().ToArray();



                obj.itemlist = (from a in _context.TR_ServiceDMO
                                from b in _context.TR_Service_DetailsDMO
                                where a.TRSE_Id == b.TRSE_Id && a.TRSE_Id == id
                                select new ServiceStationDTO
                                {
                                    TRSE_Id = a.TRSE_Id,
                                    TRSED_Id = b.TRSED_Id,
                                    TRSED_ItemsName = b.TRSED_ItemsName,
                                    TRSED_Qty = b.TRSED_Qty,
                                    TRSED_ProblemsListed = b.TRSED_ProblemsListed,
                                    TRSED_ServiceDetails = b.TRSED_ServiceDetails,
                                    TRSED_Remarks = b.TRSED_Remarks,
                                    TRSE_ServiceRefNo = a.TRSE_ServiceRefNo,
                                    TRSED_ActiveFlag = b.TRSED_ActiveFlag,
                                    TRSE_ActiveFlag = a.TRSE_ActiveFlag,
                                    TRSE_ServiceDate = a.TRSE_ServiceDate,
                                    TRSE_ServiceDetails = a.TRSE_ServiceDetails,
                                    TRSE_ProblemsListed = a.TRSE_ProblemsListed,
                                    TRMD_Id = a.TRMD_Id,
                                    TRMV_Id = a.TRMV_Id,
                                    TRMSST_Id = a.TRMSST_Id
                                }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public ServiceStationDTO viewitems(int id)
        {
            ServiceStationDTO obj = new ServiceStationDTO();
            try
            {
                obj.itemlist = (from a in _context.TR_ServiceDMO
                                from b in _context.TR_Service_DetailsDMO
                                where a.TRSE_Id == b.TRSE_Id && a.TRSE_Id == id
                                select new ServiceStationDTO
                                {
                                    TRSE_Id = a.TRSE_Id,
                                    TRSED_Id = b.TRSED_Id,
                                    TRSED_ItemsName = b.TRSED_ItemsName,
                                    TRSED_Qty = b.TRSED_Qty,
                                    TRSED_ProblemsListed = b.TRSED_ProblemsListed,
                                    TRSED_ServiceDetails = b.TRSED_ServiceDetails,
                                    TRSED_Remarks = b.TRSED_Remarks,
                                    TRSE_ServiceRefNo = a.TRSE_ServiceRefNo,
                                    TRSED_ActiveFlag = b.TRSED_ActiveFlag
                                }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }

        public ServiceStationDTO activedeactiveparts(ServiceStationDTO dto)
        {

            try
            {
                var query = _context.TR_ServiceDMO.Single(d => d.TRSE_Id == dto.TRSE_Id);


                if (query.TRSE_ActiveFlag == true)
                {
                    query.TRSE_ActiveFlag = false;
                }
                else
                {
                    query.TRSE_ActiveFlag = true;
                }
                _context.Update(query);
                var exist = _context.SaveChanges();
                if (exist > 0)
                {
                    dto.retval = true;
                }
                else
                {
                    dto.retval = false;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            return dto;
        }


        public ServiceStationDTO getbillreport(ServiceStationDTO data)
        {
            try
            {

                string vehicleids = "0";
                List<long> HeadId = new List<long>();


                foreach (var item in data.vhlid)
                {
                    HeadId.Add(item.TRMV_Id);
                }


                for (int i = 0; i < HeadId.Count; i++)
                {
                    vehicleids = vehicleids + "," + HeadId[i];
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TR_GET_SERVICEBILLREPOT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TRMSST_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.TRMSST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TRMV_Id",
                     SqlDbType.VarChar)
                    {
                        Value = vehicleids
                    });
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE",
                   SqlDbType.Date)
                    {
                        Value = data.TRKMLB_FromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE",
                   SqlDbType.Date)
                    {
                        Value = data.TRKMLB_ToDate
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
                        data.requisitionlistold = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return data;

        }
        public ServiceStationDTO viewreq(ServiceStationDTO data)
        {
            try
            {
                data.instlist = _db.Institution.Where(d => d.MI_Id == data.MI_Id && d.MI_ActiveFlag==1).ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TR_GET_REQUISITIONFORM";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    
                    cmd.Parameters.Add(new SqlParameter("@TRSE_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.TRSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TRMV_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.TRMV_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                     SqlDbType.VarChar)
                    {
                        Value = "N"
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
                        data.requisitionlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TR_GET_REQUISITIONFORM";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@TRSE_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.TRSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TRMV_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.TRMV_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                     SqlDbType.VarChar)
                    {
                        Value = "O"
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
                        data.requisitionlistold = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            return data;
        }

        //Master service
        public ServiceStationDTO loadservicestation(ServiceStationDTO obj)
        {
            try
            {
                obj.servnamegrid = _context.TR_Master_ServStationDMO.Where(t => t.MI_Id == obj.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public ServiceStationDTO savestation(ServiceStationDTO data)
        {
            try
            {
                if (data.TRMSST_Id == 0)
                {
                    var duplicateCheck = _context.TR_Master_ServStationDMO.Where(d => d.MI_Id == data.MI_Id && d.TRMSST_Address == data.TRMSST_Address && d.TRMSST_Address == data.TRMSST_Address).ToList();
                    if (duplicateCheck.Count > 0)
                    {
                        data.returnVal = "duplicate";
                    }
                    else
                    {
                        TR_Master_ServStationDMO mapp = new TR_Master_ServStationDMO();

                        mapp.MI_Id = data.MI_Id;
                        mapp.TRMSST_ServiceStationName = data.TRMSST_ServiceStationName;
                        mapp.TRMSST_EmailId = data.TRMSST_EmailId;
                        mapp.TRMSST_ContactNo = data.TRMSST_ContactNo;
                        mapp.TRMSST_Address = data.TRMSST_Address.Trim(); ;
                        mapp.TRMSST_ActiveFlag = true;
                        mapp.TRMSST_CreatedBy = data.USER_Id;
                        mapp.TRMSST_UpdatedBy = data.USER_Id;
                        mapp.CreatedDate = DateTime.Now;
                        mapp.UpdatedDate = DateTime.Now;

                        _context.Add(mapp);
                        var flag = _context.SaveChanges();
                        if (flag > 0)
                        {
                            data.returnVal = "saved";
                        }
                        else
                        {
                            data.returnVal = "savingFailed";
                        }

                    }
                }
                else
                {
                    var duplicateCheck = _context.TR_Master_ServStationDMO.Where(d => d.MI_Id == data.MI_Id && d.TRMSST_Address == data.TRMSST_Address && d.TRMSST_ServiceStationName == data.TRMSST_ServiceStationName && d.TRMSST_Id != data.TRMSST_Id).ToList();
                    if (duplicateCheck.Count > 0)
                    {
                        data.returnVal = "duplicate";
                    }
                    else
                    {
                        var result = _context.TR_Master_ServStationDMO.Single(d => d.TRMSST_Id == data.TRMSST_Id);
                        result.TRMSST_ServiceStationName = data.TRMSST_ServiceStationName;
                        result.TRMSST_EmailId = data.TRMSST_EmailId;
                        result.TRMSST_ContactNo = data.TRMSST_ContactNo;
                        result.TRMSST_Address = data.TRMSST_Address;
                        result.TRMSST_ActiveFlag = true;
                        result.UpdatedDate = DateTime.Now;
                        result.TRMSST_UpdatedBy = data.USER_Id;
                        _context.Update(result);
                        var update = _context.SaveChanges();
                        if (update > 0)
                        {
                            data.returnVal = "updated";
                        }
                        else
                        {
                            data.returnVal = "failedUpdate";
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }
        public ServiceStationDTO Editstation(int id)
        {
            ServiceStationDTO obj = new ServiceStationDTO();
            try
            {
                obj.editDataList = _context.TR_Master_ServStationDMO.Where(d => d.TRMSST_Id == id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public ServiceStationDTO deactivatestation(ServiceStationDTO dto)
        {

            try
            {

                //var used1 = _context.TR_Part_PerticularsDMO.Where(x => x.TRMSES_Id == dto.TRMSST_Id && x.TRMSESP_ActiveFlag == true).ToList();
                //if (used1.Count == 0)
                //{
                var query = _context.TR_Master_ServStationDMO.Single(d => d.TRMSST_Id == dto.TRMSST_Id);


                if (query.TRMSST_ActiveFlag == true)
                {
                    query.TRMSST_ActiveFlag = false;
                }
                else
                {
                    query.TRMSST_ActiveFlag = true;
                }
                _context.Update(query);
                var exist = _context.SaveChanges();
                if (exist > 0)
                {
                    dto.retval = true;
                }
                else
                {
                    dto.retval = false;
                }
                //}
                //else
                //{
                //    dto.returnVal = "exist";
                //}


            }
            catch (Exception e)
            {
                throw e;
            }
            return dto;
        }

        //Master partstype
        public ServiceStationDTO loadparttype(ServiceStationDTO obj)
        {
            try
            {
                obj.parttypegrig = _context.TR_PartperticularTypeDMO.Where(t => t.MI_Id == obj.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public ServiceStationDTO saveparttype(ServiceStationDTO data)
        {
            try
            {
                if (data.TRPAPT_Id == 0)
                {
                    var duplicateCheck = _context.TR_PartperticularTypeDMO.Where(d => d.MI_Id == data.MI_Id && d.TRPAPT_PType == data.TRPAPT_PType).ToList();
                    if (duplicateCheck.Count > 0)
                    {
                        data.returnVal = "duplicate";
                    }
                    else
                    {
                        TR_PartperticularTypeDMO mapp = new TR_PartperticularTypeDMO();

                        mapp.MI_Id = data.MI_Id;
                        mapp.TRPAPT_PType = data.TRPAPT_PType;
                        mapp.TRPAPT_ActiveFlag = true;

                        mapp.CreatedDate = DateTime.Now;
                        mapp.UpdatedDate = DateTime.Now;

                        _context.Add(mapp);
                        var flag = _context.SaveChanges();
                        if (flag > 0)
                        {
                            data.returnVal = "saved";
                        }
                        else
                        {
                            data.returnVal = "savingFailed";
                        }

                    }
                }
                else
                {
                    var duplicateCheck = _context.TR_PartperticularTypeDMO.Where(d => d.MI_Id == data.MI_Id && d.TRPAPT_PType == data.TRPAPT_PType && d.TRPAPT_Id != data.TRPAPT_Id).ToList();
                    if (duplicateCheck.Count > 0)
                    {
                        data.returnVal = "duplicate";
                    }
                    else
                    {
                        var result = _context.TR_PartperticularTypeDMO.Single(d => d.TRPAPT_Id == data.TRPAPT_Id);
                        result.TRPAPT_PType = data.TRPAPT_PType;
                        result.TRPAPT_ActiveFlag = true;
                        result.UpdatedDate = DateTime.Now;
                        _context.Update(result);
                        var update = _context.SaveChanges();
                        if (update > 0)
                        {
                            data.returnVal = "updated";
                        }
                        else
                        {
                            data.returnVal = "failedUpdate";
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }
        public ServiceStationDTO Editparttype(int id)
        {
            ServiceStationDTO obj = new ServiceStationDTO();
            try
            {
                obj.editDataList = _context.TR_PartperticularTypeDMO.Where(d => d.TRPAPT_Id == id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public ServiceStationDTO deactivateparttype(ServiceStationDTO dto)
        {

            try
            {
                var used = _context.TR_Part_PerticularsDMO.Where(x => x.TRPAPT_Id == dto.TRPAPT_Id && x.TRMSESP_ActiveFlag == true).ToList();
                if (used.Count == 0)
                {
                    var query = _context.TR_PartperticularTypeDMO.Single(d => d.TRPAPT_Id == dto.TRPAPT_Id);


                    if (query.TRPAPT_ActiveFlag == true)
                    {
                        query.TRPAPT_ActiveFlag = false;
                    }
                    else
                    {
                        query.TRPAPT_ActiveFlag = true;
                    }
                    _context.Update(query);
                    var exist = _context.SaveChanges();
                    if (exist > 0)
                    {
                        dto.retval = true;
                    }
                    else
                    {
                        dto.retval = false;
                    }
                }
                else
                {
                    dto.returnVal = "exist";
                }


            }
            catch (Exception e)
            {
                throw e;
            }
            return dto;
        }

    }
}
