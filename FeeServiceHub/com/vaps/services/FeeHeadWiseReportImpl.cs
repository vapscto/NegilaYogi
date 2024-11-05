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
    public class FeeHeadWiseReportImpl :interfaces.FeeHeadWiseReportInterface
    {
        private static ConcurrentDictionary<string, FeeHeadWiseReportDTO> _login =
             new ConcurrentDictionary<string, FeeHeadWiseReportDTO>();

        public FeeGroupContext _db;

        public FeeHeadWiseReportImpl(FeeGroupContext db)
        {
            _db = db;
        }

        public FeeHeadWiseReportDTO getInitailData(FeeHeadWiseReportDTO data)
        {
            FeeHeadWiseReportDTO ctdo = new FeeHeadWiseReportDTO();
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _db.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_Id).OrderByDescending(y => y.ASMAY_Order).ToList();
                ctdo.YearList = allyear.Distinct().GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();

                //List<FeeClassCategoryDMO> allcategory = new List<FeeClassCategoryDMO>();
                //allcategory = _db.FeeClassCategoryDMO.Where(c => c.FMCC_ActiveFlag == true && c.MI_Id == mi_id).ToList();

                //ctdo.Class_Category_List = allcategory.ToArray();


                ctdo.Class_Category_List = (from a in _db.FeeClassCategoryDMO
                                            from b in _db.feeYCC
                                            from c in _db.feeYCCC
                                            where (a.FMCC_Id == b.FMCC_Id && a.MI_Id == data.MI_Id && b.FYCC_Id==c.FYCC_Id &&                  b.ASMAY_Id==data.ASMAY_Id)
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

        public FeeHeadWiseReportDTO SearchData(FeeHeadWiseReportDTO Clscatag)
        {

            try
            {
                List<FeeHeadWiseReportDTO> result = new List<FeeHeadWiseReportDTO>();

                //to get data according to search criteria.
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fee_Head_Wise_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = Clscatag.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@FMCC_Id", SqlDbType.BigInt) { Value = Clscatag.FMCC_Id});
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = Clscatag.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@user_id", SqlDbType.BigInt) { Value = Clscatag.user_id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new FeeHeadWiseReportDTO
                                {
                                    Fee_Group = dataReader["FMG_GroupName"].ToString(),
                                    Fee_Head = dataReader["FMH_FeeName"].ToString(),
                                    Active_Flag =Convert.ToBoolean(dataReader["FMG_ActiceFlag"]),
                                    Fine_Applicable =dataReader["FYGHM_FineApplicableFlag"].ToString(),
                                    Installment = dataReader["FTI_Name"].ToString(),
                                    Fine_Amount =Convert.ToDecimal(dataReader["amount"]),

                                });
                                Clscatag.FHWR_searchdatalist = result.ToArray();
                            }
                        }
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

        public FeeHeadWiseReportDTO getdata(FeeHeadWiseReportDTO data)
        {
           
            try
            {
                List<FeeClassCategoryDMO> allcategory = new List<FeeClassCategoryDMO>();
                data.Class_Category_List = (from a in _db.FeeClassCategoryDMO
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
            return data;
        }

        // Sudarshan 02-12-2023

        public FeeHeadWiseReportDTO getreport(FeeHeadWiseReportDTO data)
        {

            try
            {
                string asmcl_id = "0";
                string asmc_id = "0";
                if (data.fillclasflg == null || data.fillclasflg == 0)
                {
                    var classlist = _db.School_M_Class.Where(t => t.MI_Id == data.MI_Id).ToList();
                    foreach (var x in classlist)
                    {
                        asmcl_id = asmcl_id + ',' + x.ASMCL_Id;
                    }
                }
                else
                {
                    asmcl_id = asmcl_id + ',' + data.fillclasflg;
                }

                if (data.fillseccls == null || data.fillseccls == 0)
                {
                    var sectionlist = _db.AdmSection.Where(t => t.MI_Id == data.MI_Id).ToList();
                    foreach (var x in sectionlist)
                    {
                        asmc_id = asmc_id + ',' + x.ASMS_Id;
                    }
                }
                else
                {
                    asmc_id = asmc_id + ',' + data.fillseccls;
                }
                //var fmt_ids = "";
                //foreach (var x in data.FMT_Ids)
                //{
                //    fmt_ids += x + ",";
                //}
                //fmt_ids = fmt_ids.Substring(0, (fmt_ids.Length - 1));

                //data.termlist = _FeeGroupContext.feeTr.Where(a => a.MI_Id == data.MI_ID && a.FMT_ActiveFlag == true).ToArray();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 15000;
                    cmd.CommandText = "FeeStudentHeadWiseReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.ASMAY_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                   SqlDbType.VarChar)
                    {
                        Value = asmcl_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                   SqlDbType.VarChar)
                    {
                        Value = asmc_id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject1 = new List<dynamic>();

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

                                retObject1.Add((ExpandoObject)dataRow);
                            }

                        }

                        data.getreportdata = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                return data;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return data;
        }
    }
}
