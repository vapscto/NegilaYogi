using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using PreadmissionDTOs.com.vaps.Portals.IVRS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.IVRS.Services
{
    public class Interaction_Delete_Report_IMPL : Interfaces.Interaction_Delete_ReportInterface
    {
        public PortalContext _db;
        public Interaction_Delete_Report_IMPL(PortalContext db)
        {
            _db = db;
        }

        public Interaction_Delete_Report_DTO getreport(Interaction_Delete_Report_DTO dto)
        {
            try
            {
                //dto.HRME_Id = _db.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_InteractionDelete";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.DateTime)
                    { Value = dto.fromdate });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime)
                    { Value = dto.todate });



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
                        dto.deletemsglist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }



        public Interaction_Delete_Report_DTO loadreportdata(Interaction_Delete_Report_DTO data)
        {
            try
            {
                data.fillyear = _db.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(r => r.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

        public Interaction_Delete_Report_DTO getintreport(Interaction_Delete_Report_DTO data)
        {
            try
            {
                var rolet = _db.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    data.HRME_Id = 0;
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.AMST_Id = 0;
                    data.HRME_Id = _db.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;

                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Interaction_RoleWise_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                    { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@RoleType", SqlDbType.Char)
                    { Value = rolet.FirstOrDefault().IVRMRT_Role });

                    cmd.Parameters.Add(new SqlParameter("@StuORStaffId", SqlDbType.Char)
                    { Value = data.HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.Date)
                    { Value = data.fromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.Date)
                    { Value = data.todate });


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
                        data.deletemsglist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }


        public Interaction_Delete_Report_DTO mobload(Interaction_Delete_Report_DTO data)
        {
            try
            {
                data.fillyear = _db.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(r => r.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

        public Interaction_Delete_Report_DTO mobreport(Interaction_Delete_Report_DTO data)
        {
            try
            {

                if (data.optionflag == "Download")
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_MobApp_DownloadReport";
                        cmd.CommandTimeout = 90000000;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                        { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                        { Value = data.fromdate });
                        cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar)
                        { Value = data.todate });
                        cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.VarChar)
                        { Value = data.Active });
                        cmd.Parameters.Add(new SqlParameter("@DeActive", SqlDbType.VarChar)
                        { Value = data.DeActive });
                        cmd.Parameters.Add(new SqlParameter("@Left", SqlDbType.VarChar)
                        { Value = data.Left });
                        cmd.Parameters.Add(new SqlParameter("@Flg", SqlDbType.VarChar)
                        { Value = "Download" });



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
                            data.mobapplist = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_MobApp_DownloadReport";
                        cmd.CommandTimeout = 90000000;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                        { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                        { Value = data.fromdate });
                        cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar)
                        { Value = data.todate });
                        cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.VarChar)
                        { Value = data.Active });
                        cmd.Parameters.Add(new SqlParameter("@DeActive", SqlDbType.VarChar)
                        { Value = data.DeActive });
                        cmd.Parameters.Add(new SqlParameter("@Left", SqlDbType.VarChar)
                        { Value = data.Left });
                        cmd.Parameters.Add(new SqlParameter("@Flg", SqlDbType.VarChar)
                        { Value = "NotDownload" });


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
                            data.mobapplistnotcount = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_MobApp_DownloadReport";
                        cmd.CommandTimeout = 90000000;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                        { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                        { Value = data.fromdate });
                        cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar)
                        { Value = data.todate });
                        cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.VarChar)
                        { Value = data.Active });
                        cmd.Parameters.Add(new SqlParameter("@DeActive", SqlDbType.VarChar)
                        { Value = data.DeActive });
                        cmd.Parameters.Add(new SqlParameter("@Left", SqlDbType.VarChar)
                        { Value = data.Left });
                        cmd.Parameters.Add(new SqlParameter("@Flg", SqlDbType.VarChar)
                        { Value = "Total" });


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
                            data.mobapplisttotalcount = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }
                else
                {

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_MobApp_DownloadReport";
                        cmd.CommandTimeout = 90000000;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                        { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                        { Value = data.fromdate });
                        cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar)
                        { Value = data.todate });
                        cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.VarChar)
                        { Value = data.Active });
                        cmd.Parameters.Add(new SqlParameter("@DeActive", SqlDbType.VarChar)
                        { Value = data.DeActive });
                        cmd.Parameters.Add(new SqlParameter("@Left", SqlDbType.VarChar)
                        { Value = data.Left });
                        cmd.Parameters.Add(new SqlParameter("@Flg", SqlDbType.VarChar)
                        { Value = "NotDownload" });



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
                            data.mobapplist = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }




                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_MobApp_DownloadReport";
                        cmd.CommandTimeout = 90000000;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                        { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                        { Value = data.fromdate });
                        cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar)
                        { Value = data.todate });
                        cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.VarChar)
                        { Value = data.Active });
                        cmd.Parameters.Add(new SqlParameter("@DeActive", SqlDbType.VarChar)
                        { Value = data.DeActive });
                        cmd.Parameters.Add(new SqlParameter("@Left", SqlDbType.VarChar)
                        { Value = data.Left });
                        cmd.Parameters.Add(new SqlParameter("@Flg", SqlDbType.VarChar)
                        { Value = "Total" });


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
                            data.mobapplisttotalcount = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
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
