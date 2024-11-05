using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using FeeServiceHub.com.vaps.interfaces;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeDueDateReportImpl :interfaces.FeeDueDateReportInterface
    {
        private static ConcurrentDictionary<string, FeeDueDateReportDTO> _login =
             new ConcurrentDictionary<string, FeeDueDateReportDTO>();

        public FeeGroupContext _db;

       // DateTime? toDate = null;
        public FeeDueDateReportImpl(FeeGroupContext db)
        {
            _db = db;
        }
       
        public FeeDueDateReportDTO getInitailData(FeeDueDateReportDTO data)
        {
            FeeDueDateReportDTO ctdo = new FeeDueDateReportDTO();
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _db.AcademicYear.Where(y=>y.Is_Active==true && y.MI_Id==data.MI_Id).OrderByDescending(y => y.ASMAY_Order).ToList();
                ctdo.YearList = allyear.Distinct().GroupBy(y=>y.ASMAY_Year).Select(y=>y.First()).ToArray();

                //List<FeeClassCategoryDMO> allcategory = new List<FeeClassCategoryDMO>();
                //allcategory = _db.FeeClassCategoryDMO.Where(c=>c.FMCC_ActiveFlag==true && c.MI_Id==mi_id).ToList();
                //ctdo.Class_Category_List = allcategory.ToArray();
                ctdo.Class_Category_List = (from a in _db.FeeClassCategoryDMO
                                            from b in _db.feeYCC
                                            from c in _db.feeYCCC
                                            where (a.FMCC_Id == b.FMCC_Id && a.MI_Id == data.MI_Id && b.FYCC_Id == c.FYCC_Id && b.ASMAY_Id == data.ASMAY_Id)
                                            select new FeeHeadWiseReportDTO
                                            {
                                                FMCC_Id = a.FMCC_Id,
                                                FHWR_ClassCategoryName = a.FMCC_ClassCategoryName,

                                            }
                                    ).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }

        public async Task<FeeDueDateReportDTO> SearchData(FeeDueDateReportDTO Clscatag)
        {
            try
            {
                List<FeeDueDateReportDTO> result = new List<FeeDueDateReportDTO>();

                //to get data according to search criteria.
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FEE_DUE_DATE_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@asmay_Id", SqlDbType.BigInt) { Value = Clscatag.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = Clscatag.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@fmcc_id", SqlDbType.BigInt) { Value = Clscatag.FMCC_Id });
                    cmd.Parameters.Add(new SqlParameter("@user_id", SqlDbType.BigInt) { Value = Clscatag.user_id });
                    //if (cmd.Connection.State != ConnectionState.Open)
                    //    cmd.Connection.Open();

                    //var retObject = new List<dynamic>();
                    //try
                    //{

                    //    using (var dataReader = cmd.ExecuteReader())
                    //    {
                    //        while (dataReader.Read())
                    //        {

                    //            if (dataReader["FTIDD_DueDate"] != System.DBNull.Value)
                    //            {
                    //                toDate = Convert.ToDateTime(dataReader["FTIDD_DueDate"]);
                    //            }
                    //            else
                    //            {
                    //                toDate = null;
                    //            }

                    //            result.Add(new FeeDueDateReportDTO
                    //            {
                    //                Fee_Group = dataReader["FMG_GroupName"].ToString(),
                    //                Fee_Head = dataReader["FMH_FeeName"].ToString(),
                    //                Master_Installment = dataReader["FMI_Name"].ToString(),
                    //                Tran_Installment = dataReader["FTI_Name"].ToString(),
                    //                Due_Date = toDate,
                    //                Fine_Slab = dataReader["FMFS_FineType"].ToString(),
                    //                Fine_Type = dataReader["FTFS_FineType"].ToString(),
                    //                Fine_Amount = Convert.ToDecimal(dataReader["FMA_Amount"].ToString()),

                    //            });
                    //            Clscatag.FHWR_searchdatalist = result.ToArray();
                    //        }
                    //    }
                    //}
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        // var data = cmd.ExecuteNonQuery();

                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        Clscatag.FHWR_searchdatalist = retObject.ToArray();
                        
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return Clscatag;
        }

        public FeeDueDateReportDTO getdata(FeeDueDateReportDTO data)
        {

            try
            {
                List<FeeDueDateReportDTO> allcategory = new List<FeeDueDateReportDTO>();
                data.Class_Category_List = (from a in _db.FeeClassCategoryDMO
                                            from b in _db.feeYCC
                                            from c in _db.feeYCCC
                                            where (a.FMCC_Id == b.FMCC_Id && a.MI_Id == data.MI_Id && b.FYCC_Id == c.FYCC_Id && b.ASMAY_Id == data.ASMAY_Id)
                                            select new FeeDueDateReportDTO
                                            {
                                                FMCC_Id = a.FMCC_Id,
                                                FHWR_ClassCategoryName = a.FMCC_ClassCategoryName,

                                            }
                                       ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return data;
        }

        public async Task<FeeDueDateReportDTO> getreport(FeeDueDateReportDTO dto)
        {
            DateTime fdate = Convert.ToDateTime(dto.Fromdate);
            string frmdate = fdate.ToString("yyyy-MM-dd");
            DateTime tdate = Convert.ToDateTime(dto.Todate);
            string todate = tdate.ToString("yyyy-MM-dd");

            try
            {
                using (var cmd1 = _db.Database.GetDbConnection().CreateCommand())
                {
                    
                    cmd1.CommandText = "usp_s_DailyIncomeforVidya";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.CommandTimeout = 90000000;
                    //cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    // SqlDbType.BigInt)
                    //{
                    //    Value = dto.ASMAY_Id
                    //});

                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@DateSelection",
                     SqlDbType.VarChar)
                    {
                        Value = frmdate
                    });
                    //cmd1.Parameters.Add(new SqlParameter("@ToDate",
                    // SqlDbType.DateTime)
                    //{
                    //    Value = dto.Todate
                    //});

                    if (cmd1.Connection.State != ConnectionState.Open)
                        cmd1.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = await cmd1.ExecuteReaderAsync())
                        {
                            while (await dataReader1.ReadAsync())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        dto.incomereport = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd1 = _db.Database.GetDbConnection().CreateCommand())
                {

                    cmd1.CommandText = "usp_s_DailyExpenseforVidya";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.CommandTimeout = 90000000;
                    //cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    // SqlDbType.BigInt)
                    //{
                    //    Value = dto.ASMAY_Id
                    //});

                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@DateSelection",
                     SqlDbType.DateTime)
                    {
                        Value = dto.Fromdate
                    });
                    //cmd1.Parameters.Add(new SqlParameter("@ToDate",
                    // SqlDbType.DateTime)
                    //{
                    //    Value = dto.Todate
                    //});

                    if (cmd1.Connection.State != ConnectionState.Open)
                        cmd1.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = await cmd1.ExecuteReaderAsync())
                        {
                            while (await dataReader1.ReadAsync())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        dto.expensereport = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
    }
}
