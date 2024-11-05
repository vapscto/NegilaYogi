using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeDefaulterReportStthomasImpl : interfaces.FeeDefaulterReportStthomasInterface
    {

        public FeeGroupContext _FeeGroupContext;
        public FeeDefaulterReportStthomasImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }

        public FeeDefaulterReportStthomasDTO getreport(FeeDefaulterReportStthomasDTO data)
        {
            try
            {

                data.getfeedefaultertemplate = _FeeGroupContext.sMSEmailSetting.Where(t => t.ISES_Template_Name.Contains("DEFAULT") && t.MI_Id == data.MI_ID).ToArray();

                string asmcl_id = "0";
                string asmc_id = "0";
                if(data.fillclasflg==null || data.fillclasflg == 0)
                {
                    var classlist = _FeeGroupContext.School_M_Class.Where(t => t.MI_Id == data.MI_ID).ToList();
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
                    var sectionlist = _FeeGroupContext.AdmSection.Where(t => t.MI_Id == data.MI_ID).ToList();
                    foreach (var x in sectionlist)
                    {
                        asmc_id = asmc_id + ',' + x.ASMS_Id;
                    }
                }
                else
                {
                    asmc_id = asmc_id + ',' + data.fillseccls;
                }
                var fmt_ids = "";
                foreach (var x in data.FMT_Ids)
                {
                    fmt_ids += x + ",";
                }
                fmt_ids = fmt_ids.Substring(0, (fmt_ids.Length - 1));

                //data.termlist = _FeeGroupContext.feeTr.Where(a => a.MI_Id == data.MI_ID && a.FMT_ActiveFlag == true).ToArray();
               
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FeeDefaulterreportNewFormat";
                    cmd.CommandTimeout = 300000;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.MI_ID)
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
                    cmd.Parameters.Add(new SqlParameter("@Type",
                       SqlDbType.VarChar)
                    {
                        Value = data.typeofrpt
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.Amst_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FMT_Id",
                   SqlDbType.VarChar)
                    {
                        Value = fmt_ids
                    });



                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
               var retObject1 = new List<dynamic>();

                    try
                    {

                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while(  dataReader.Read())
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
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
