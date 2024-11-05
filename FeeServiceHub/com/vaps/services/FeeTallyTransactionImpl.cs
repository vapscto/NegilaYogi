using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Fee;
using PreadmissionDTOs.com.vaps.Fees.Tally;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model.com.vapstech.Fee.Tally;
using System.Dynamic;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeTallyTransactionImpl : interfaces.FeeTallyTransactionInterface
    {
        public FeeGroupContext _FeeGroupContext;
        public FeeTallyTransactionImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }


        public TallyMTransactionDTO deletedata(TallyMTransactionDTO pgmod)
        {
            try
            {
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fee_Tally_M_Insert_RV";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                    {
                        Value = pgmod.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = pgmod.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate",
                     SqlDbType.DateTime)
                    {
                        Value = pgmod.From_Date
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate",
               SqlDbType.DateTime)
                    {
                        Value = pgmod.To_Date
                    });

                    cmd.Parameters.Add(new SqlParameter("@userid",
              SqlDbType.BigInt)
                    {
                        Value = pgmod.TMT_VoucherNo
                    });

                    //if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();
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
                        pgmod.TempararyArrayList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }



                    if (pgmod.TempararyArrayList.Length >= 1)
                    {
                        pgmod.returnval = "true";

                    }
                    else
                    {
                        pgmod.returnval = "false";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return pgmod;
        }


        public TallyMTransactionDTO getstudentdetails(TallyMTransactionDTO data)
        {
            try
            {
                data.fillstudent = (from a in _FeeGroupContext.AdmissionStudentDMO
                                    from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                    where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && b.ASMCL_Id == data.ASMCL_Id)
                                    select new TallyMTransactionDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : a.AMST_LastName)).Trim(),
                                        AMST_AdmNo=a.AMST_AdmNo
                                    }
                                        ).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public TallyMTransactionDTO loaddata(TallyMTransactionDTO data)
        {
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.academicdrp = allyear.Distinct().ToArray();

                data.f_year = _FeeGroupContext.IVRM_Master_FinancialYear.OrderByDescending(a => a.IMFY_OrderBy).ToArray();
                data.f_year_1 = _FeeGroupContext.IVRM_Master_FinancialYear.OrderByDescending(a => a.IMFY_OrderBy).FirstOrDefault().IMFY_Id;

                data.f_year_Current_financial_year = _FeeGroupContext.IVRM_Master_FinancialYear.Where(t => Convert.ToDateTime(t.IMFY_FromDate) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.IMFY_ToDate) >= Convert.ToDateTime(System.DateTime.Today.Date)).ToArray();

                List<FeeMasterConfigurationDMO> config = new List<FeeMasterConfigurationDMO>();
                config = _FeeGroupContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.fillconfig = config.ToArray();

                List<School_M_Class> allclass = new List<School_M_Class>();
                allclass = _FeeGroupContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                data.allclsdata = allclass.ToArray();


                List<FeeTermDMO> allinstallment = new List<FeeTermDMO>();
                allinstallment = _FeeGroupContext.feeTr.Where(t => t.MI_Id == data.MI_Id).Distinct().ToList();
                data.allinsdata = allinstallment.ToArray();


                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Tally_Excel_Report_New";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                    {

                        Value = data.MI_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@Vouchertype",
                       SqlDbType.NVarChar)
                    {

                        Value = "JOURNALVOUCHER"
                    });

                    //cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    // SqlDbType.BigInt)
                    //{

                    //    Value = data.ASMAY_Id
                    //});

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
                        data.TempararyArrayList = retObject.ToArray();
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
            return data;
        }

        public TallyMTransactionDTO savedata(TallyMTransactionDTO pgmod)
        {
            try
            {
                int j = 0;


                var fmg_ids = "";

                var fmgg_ids = "";
                foreach (var x in pgmod.Amst_Ids)
                {
                    fmgg_ids += x + ",";
                }
                fmgg_ids = fmgg_ids.Substring(0, (fmgg_ids.Length - 1));

                //while (j < pgmod.savetmpdata.Count())
                //{

             
               var install = _FeeGroupContext.feemastersettings.Where(t => t.MI_Id == pgmod.MI_Id).FirstOrDefault().FMC_InstallmentwiseJVFlg;
                
                if (install == false)
                {
                    pgmod.ftiidss = "";
                }



                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fee_Tally_M_Insert_JV";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                    {
                      
                        Value = pgmod.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                       
                        Value = pgmod.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                   SqlDbType.BigInt)
                    {
                       
                        Value = pgmod.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                        SqlDbType.VarChar)
                    {
                       
                        Value = fmgg_ids
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMT_Id",
                     SqlDbType.VarChar)
                    {
                        Value = (pgmod.ftiidss == "") ? "0" : pgmod.ftiidss

                    });
                    //   cmd.Parameters.Add(new SqlParameter("@userid",
                    //SqlDbType.DateTime)
                    //   {
                    //       Value = pgmod.TMT_VoucherNo
                    //   });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    //var data = cmd.ExecuteNonQuery();

                    //if (data >= 1)
                    //{
                    //    pgmod.returnval = "true";

                    //}
                    //else
                    //{
                    //    pgmod.returnval = "false";
                    //}

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();
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
                        pgmod.TempararyArrayList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    
                    if (pgmod.TempararyArrayList.Length >= 1)
                    {
                        pgmod.returnval = "true";

                    }
                    else
                    {
                        pgmod.returnval = "false";
                    }



                }
                //    j++;
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return pgmod;
        }


        public TallyMTransactionDTO Concessiondata(TallyMTransactionDTO pgmod)
        {
            try
            {


                var fmgg_ids = "";
                foreach (var x in pgmod.Amst_Ids)
                {
                    fmgg_ids += x + ",";
                }
                fmgg_ids = fmgg_ids.Substring(0, (fmgg_ids.Length - 1));


                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fee_Tally_M_Insert_JV_Concession";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                    {

                        Value = pgmod.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {

                        Value = pgmod.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                   SqlDbType.BigInt)
                    {

                        Value = pgmod.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                        SqlDbType.VarChar)
                    {

                        Value = fmgg_ids
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMT_Id",
                     SqlDbType.VarChar)
                    {
                        Value = (pgmod.ftiidss == "") ? "0" : pgmod.ftiidss

                    });

                    cmd.Parameters.Add(new SqlParameter("@userid",
             SqlDbType.BigInt)
                    {
                        Value = pgmod.TMT_VoucherNo
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var data = cmd.ExecuteNonQuery();



                    if (data >= 1)
                    {
                        pgmod.returnval = "true";

                    }
                    else
                    {
                        pgmod.returnval = "false";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return pgmod;
        }


        public TallyMTransactionDTO Paymentdata(TallyMTransactionDTO pgmod)
        {
            try
            {
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fee_Tally_M_Insert_RV_Fee_Refund";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                    {
                        Value = pgmod.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = pgmod.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate",
                     SqlDbType.DateTime)
                    {
                        Value = pgmod.From_Date
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate",
               SqlDbType.DateTime)
                    {
                        Value = pgmod.To_Date
                    });

                    cmd.Parameters.Add(new SqlParameter("@userid",
             SqlDbType.BigInt)
                    {
                        Value = pgmod.TMT_VoucherNo
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    try
                    {
                        var data = cmd.ExecuteNonQuery();

                        if (data >= 1)
                        {
                            pgmod.returnval = "true";

                        }
                        else
                        {
                            pgmod.returnval = "false";
                        }
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
            return pgmod;
        }


        public TallyMTransactionDTO getalldetails(TallyMTransactionDTO data)
        {
            try
            {
                data.MI_Id = data.MI_Id;
                string description = "";
                long TMT_RefNo = 0;
                string url = "http://localhost:49540/api/FeeTallyTransactionFacade/getalldetails";

                //Dictionary<String, String> paramtr = new Dictionary<String, String>();
                //paramtr.Add("MI_Id", data.MI_Id.ToString());
                
                //var myContent = JsonConvert.SerializeObject(paramtr);
                
                //String postData = myContent;
                //WebRequest request1 = (HttpWebRequest)WebRequest.Create(url);
                //request1.ContentType = "application/json";
                //request1.Method = "POST";
                //var response = request1.GetResponseAsync().Result;
                //string contentType = response.ContentType;
                //Stream content = response.GetResponseStream();
                //if (content != null)
                //{
                //    StreamReader readStream = new StreamReader(content, Encoding.UTF8);
                //    string responseparameters = readStream.ReadToEnd();
                //    JObject joResponse = JObject.Parse(responseparameters);
                //    JArray array = (JArray)joResponse["values"];

                
                    foreach (var root in data.tallyoutput)
                    {
                        description = root.Tally_Status;
                        TMT_RefNo = root.Vaps_ID;

                        if ((description == "SUCCESS"))
                        {

                            var res1 = _FeeGroupContext.TallyMTransactionDMO.Where(t => t.TMT_Id== TMT_RefNo).ToList();
                            if (res1.Count > 0)
                            {
                            List<long> GrpId = new List<long>();
                            GrpId.Add(root.Vaps_ID);

                            var objpge1 = _FeeGroupContext.TallyMTransactionDMO.Single(t => t.TMT_Id == TMT_RefNo);
                                objpge1.TMT_ExportToTallyFlg = 1;
                                objpge1.TMT_TallyMasterId = root.Tally_MasterID;

                                _FeeGroupContext.Update(objpge1);
                                var contactExists = _FeeGroupContext.SaveChanges();
                        }
                        }
                    }
               // }

                    var exists = _FeeGroupContext.SaveChanges();
                    if (exists >= 1)
                    {
                        data.returnval = "Success";
                    }
                    else
                    {
                        data.returnval = "Failed";
                    }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public TallyMTransactionDTO getvouchertypedetails(TallyMTransactionDTO data)
        {
            try
            {
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Tally_Excel_Report_New";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                    {

                        Value = data.MI_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@Vouchertype",
                       SqlDbType.NVarChar)
                    {

                        Value =data.TMT_VoucherType
                    });

                    //cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    // SqlDbType.BigInt)
                    //{

                    //    Value = data.ASMAY_Id
                    //});

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
                        data.TempararyArrayList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //=============tally reports================== 
        public TallyMTransactionDTO get_tally_data(TallyMTransactionDTO dto)
        {
            try
            {
              
                    string fmt_id = "0";
                    List<long> fmt = new List<long>();
                    foreach (var item in dto.termsarray)
                    {
                        fmt.Add(item.FMT_Id);
                    }
                    for (int a = 0; a < fmt.Count; a++)
                    {
                        fmt_id = fmt_id + ',' + fmt[a].ToString();
                    }
               
              
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Tally_Journal_Voucher_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                    {

                        Value = dto.MI_Id
                    });
                     cmd.Parameters.Add(new SqlParameter("@FMT_Id",
                       SqlDbType.VarChar)
                    {

                        Value = fmt_id
                     });


                    cmd.Parameters.Add(new SqlParameter("@Vouchertype",
                       SqlDbType.VarChar)
                    {

                        Value = dto.TMT_VoucherType
                    });
                      cmd.Parameters.Add(new SqlParameter("@startdate",
                       SqlDbType.VarChar)
                    {

                        Value = dto.From_Date_new
                      });

                      cmd.Parameters.Add(new SqlParameter("@enddate",
                       SqlDbType.VarChar)
                    {

                        Value = dto.To_Date_new
                    });
                     cmd.Parameters.Add(new SqlParameter("@year_id",
                       SqlDbType.BigInt)
                    {

                        Value = dto.IMFY_Id
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
                        dto.tally_report_list = retObject.ToArray();
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


