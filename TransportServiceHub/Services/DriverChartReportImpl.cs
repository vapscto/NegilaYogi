using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class DriverChartReportImpl : Interfaces.DriverChartReportInterface
    {
        public TransportContext _context;
        public ILogger<DriverChartReportDTO> _log;

        public DriverChartReportImpl(ILogger<DriverChartReportDTO> log, TransportContext context)
        {
            _log = log;
            _context = context;
        }

        public DriverChartReportDTO getdata(int id)
        {
            DriverChartReportDTO data = new DriverChartReportDTO();
            try
            {


                data.fillvahicletype = _context.MasterVehicleTypeDMO.Where(t => t.MI_Id == id && t.TRMVT_ActiveFlg == true).ToArray();





            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Driver Char getdata" + ex.Message);
            }
            return data;
        }

        public  DriverChartReportDTO vehicletypechange(DriverChartReportDTO data)
        {
            try
            {
                if (data.TRMVT_Id==0)
                {
                    data.fillvahicleno = (from a in _context.MasterVehicleTypeDMO
                                          from b in _context.Master_VehicleDMO
                                          where a.MI_Id == b.MI_Id && a.TRMVT_Id == b.TRMVT_Id && a.TRMVT_ActiveFlg == true && b.TRMV_ActiveFlag == true && a.MI_Id==data.MI_Id
                                          select b).ToArray();
                }
                else
                {
                    data.fillvahicleno = (from a in _context.MasterVehicleTypeDMO
                                          from b in _context.Master_VehicleDMO
                                          where a.MI_Id == b.MI_Id && a.TRMVT_Id == b.TRMVT_Id && a.TRMVT_ActiveFlg == true && b.TRMV_ActiveFlag == true && a.TRMVT_Id==data.TRMVT_Id && a.MI_Id == data.MI_Id
                                          select b).ToArray();
                }
               
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        public DriverChartReportDTO Getreportdetails(DriverChartReportDTO data)
        {
            try
            {

                if (data.vhlid.Length>0)
                {
                    List<long> HeadId = new List<long>();


                    foreach (var item in data.vhlid)
                    {
                        HeadId.Add(item.TRMV_Id);

                    }

                    data.getloaddata = (from a in _context.DriverChartDMO
                                        from b in _context.MasterDriverDMO
                                        from c in _context.Master_VehicleDMO
                                        from d in _context.MasterFuelDMO
                                        from e in _context.MasterVehicleTypeDMO
                                        where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id == e.MI_Id && a.TRMD_Id == b.TRMD_Id && a.TRMV_Id == c.TRMV_Id && c.TRMVT_Id == e.TRMVT_Id && c.TRMFT_Id == d.TRMFT_Id && b.TRMD_ActiveFlg == true && c.TRMV_ActiveFlag == true && d.TRMFT_ActiveFlg == true && a.TRDC_Date >= data.FRMDATE && a.TRDC_Date <= data.TODATE && HeadId.Contains(c.TRMV_Id))
                                        select new DriverChartReportDTO
                                        {
                                            TRDC_Date = a.TRDC_Date,
                                            TRMD_DriverName = b.TRMD_DriverName,
                                            TRMD_Id = a.TRMD_Id,
                                            TRMFT_FuelType = d.TRMFT_FuelType,
                                            TRMV_VehicleNo = c.TRMV_VehicleNo,
                                            TRMV_Id = c.TRMV_Id,
                                            TRDC_NoofLtr = a.TRDC_NoofLtr,
                                            TRDC_FromKM = a.TRDC_FromKM,
                                            TRDC_ToKM = a.TRDC_ToKM,
                                            TRDC_TotalKM = a.TRDC_TotalKM,
                                            TRDC_TotalMileage = a.TRDC_TotalMileage,
                                            TRDC_AddtAmt = a.TRDC_AddtAmt,
                                            TRDC_RateKm = a.TRDC_RateKm,
                                            TRDC_Indent_BillNo = a.TRDC_Indent_BillNo,
                                            TRDC_Emission = a.TRDC_Emission,
                                            TRDC_GrossAmount = a.TRDC_GrossAmount,
                                            TRDC_Remarks = a.TRDC_Remarks,
                                            TRDC_TotalAmount = a.TRDC_TotalAmount

                                        }).ToArray();


                }



              
               
                //string headlist = "0";
               

                //for (int c = 0; c < HeadId.Count(); c++)
                //{

                //    if (c == 0)
                //    {
                //        headlist = HeadId[c].ToString();
                //    }
                //    else
                //    {
                //        headlist = headlist + ',' + HeadId[c];
                //    }

                //}

                


                //using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "CLG_Fee_BalRegister";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //  SqlDbType.BigInt)
                //    {
                //        Value = data.MI_Id
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                //SqlDbType.BigInt)
                //    {
                //        Value = data.AMCO_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                //SqlDbType.BigInt)
                //    {
                //        Value = data.AMB_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                //SqlDbType.BigInt)
                //    {
                //        Value = data.AMSE_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                //SqlDbType.BigInt)
                //    {
                //        Value = data.ACMS_Id
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                //      SqlDbType.NVarChar)
                //    {
                //        Value = groupidss
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@FMH_Id",
                //    SqlDbType.NVarChar)
                //    {
                //        Value = headlist
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                //   SqlDbType.NVarChar)
                //    {
                //        Value = stdlist
                //    });

                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                {
                //                    dataRow.Add(
                //                        dataReader.GetName(iFiled),
                //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        data.getloaddata = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}






















                //if (data.TRMVT_Id > 0)
                //{


                //    data.getloaddata = (from a in _context.DriverChartDMO
                //                        from b in _context.MasterDriverDMO
                //                        from c in _context.Master_VehicleDMO
                //                        from d in _context.MasterFuelDMO
                //                        from e in _context.MasterVehicleTypeDMO
                //                        where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id == e.MI_Id && a.TRMD_Id == b.TRMD_Id && a.TRMV_Id == c.TRMV_Id && c.TRMVT_Id == e.TRMVT_Id && c.TRMFT_Id == d.TRMFT_Id && b.TRMD_ActiveFlg == true && c.TRMV_ActiveFlag == true && d.TRMFT_ActiveFlg == true && e.TRMVT_Id == data.TRMVT_Id && a.TRDC_Date >= data.FRMDATE && a.TRDC_Date <= data.TODATE)
                //                        select new DriverChartReportDTO
                //                        {
                //                            TRDC_Date = a.TRDC_Date,
                //                            TRMD_DriverName = b.TRMD_DriverName,
                //                            TRMD_Id = a.TRMD_Id,
                //                            TRMFT_FuelType = d.TRMFT_FuelType,
                //                            TRMV_VehicleNo = c.TRMV_VehicleNo,
                //                            TRMV_Id = c.TRMV_Id,
                //                            TRDC_NoofLtr = a.TRDC_NoofLtr,
                //                            TRDC_FromKM = a.TRDC_FromKM,
                //                            TRDC_ToKM = a.TRDC_ToKM,
                //                            TRDC_TotalKM = a.TRDC_TotalKM,
                //                            TRDC_TotalMileage = a.TRDC_TotalMileage,
                //                            TRDC_AddtAmt = a.TRDC_AddtAmt,
                //                            TRDC_RateKm = a.TRDC_RateKm,
                //                            TRDC_Indent_BillNo = a.TRDC_Indent_BillNo,
                //                            TRDC_Emission = a.TRDC_Emission,
                //                            TRDC_GrossAmount = a.TRDC_GrossAmount,
                //                            TRDC_Remarks = a.TRDC_Remarks,
                //                            TRDC_TotalAmount=a.TRDC_TotalAmount

                //                        }).ToArray();
                //}
                //else
                //{
                    
                //}
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }






    }
}
