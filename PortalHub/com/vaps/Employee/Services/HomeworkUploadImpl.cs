using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Services
{
    public class HomeworkUploadImpl:Interfaces.HomeworkUploadInterface
    {
        public PortalContext _Context;

        public HomeworkUploadImpl(PortalContext context)
        {
            _Context = context;
        }
        //class work upload report
        public HomeWorkUploadDTO Getdata_class(HomeWorkUploadDTO dto)
        {
            try
            {
                var rolet = _Context.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.IVRMRT_Id).ToList();

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {

                    dto.HRME_Id = _Context.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                }
                else
                {
                    dto.HRME_Id = 0;
                }

                //var rolet = _Context.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.IVRMRT_Id).ToList();

                //dto.HRME_Id = _Context.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;

                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_ClassList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Role",
                SqlDbType.VarChar)
                    {
                        Value = rolet[0].IVRMRT_Role
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader =  cmd.ExecuteReader())
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
                        dto.classlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public HomeWorkUploadDTO getreport_class(HomeWorkUploadDTO dto)
        {
            try
            {
                string amclid = "0";
                if(dto.classarray.Length>0)
                {
                    foreach (var item in dto.classarray)
                    {
                        amclid = amclid + "," + item.ASMCL_Id;
                    }
                }

                var rolet = _Context.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.IVRMRT_Id).ToList();

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {

                    dto.HRME_Id = _Context.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                }
                else
                {
                    dto.HRME_Id = 0;
                }


                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_HomeworkUploadReport"; // Portal_HomeworkUploadRprt
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt){Value = dto.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar){Value = dto.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar){Value = amclid});
                    cmd.Parameters.Add(new SqlParameter("@fromdate",SqlDbType.VarChar){Value = dto.fromdate});
                    cmd.Parameters.Add(new SqlParameter("@todate",SqlDbType.VarChar){Value = dto.todate});                    
                    cmd.Parameters.Add(new SqlParameter("@Role",SqlDbType.VarChar){Value =1});
                    cmd.Parameters.Add(new SqlParameter("@Type",SqlDbType.VarChar){Value = "ClassWork"});
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = dto.HRME_Id });
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
                                    dataRow.Add(dataReader.GetName(iFiled),dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);}

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.classreportlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public HomeWorkUploadDTO getreport_home(HomeWorkUploadDTO dto)
        {
            try
            {
                string amclid = "0";
                if(dto.classarray.Length>0)
                {
                    foreach (var item in dto.classarray)
                    {
                        amclid = amclid + "," + item.ASMCL_Id;
                    }
                }

                var rolet = _Context.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.IVRMRT_Id).ToList();

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {

                    dto.HRME_Id = _Context.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                }
                else
                {
                    dto.HRME_Id = 0;
                }


                if (dto.flag == "Detailed")
                {
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_HomeworkUploadReport";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                        SqlDbType.VarChar)
                        {
                            Value = dto.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                       SqlDbType.VarChar)
                        {
                            Value = amclid
                        });

                        cmd.Parameters.Add(new SqlParameter("@fromdate",
                        SqlDbType.VarChar)
                        {
                            Value = dto.fromdate
                        });

                        cmd.Parameters.Add(new SqlParameter("@todate",
                        SqlDbType.VarChar)
                        {
                            Value = dto.todate
                        });

                        cmd.Parameters.Add(new SqlParameter("@Role",
                    SqlDbType.VarChar)
                        {
                            Value = 1
                        });
                        cmd.Parameters.Add(new SqlParameter("@Type",
                   SqlDbType.VarChar)
                        {
                            Value = dto.type
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
                            dto.classreportlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                if (dto.flag == "consolidated")
                {
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HWConsolidated_report";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@fromdate",
                        SqlDbType.VarChar)
                        {
                            Value = dto.fromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                       SqlDbType.VarChar)
                        {
                            Value = amclid
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate",
                        SqlDbType.VarChar)
                        {
                            Value = dto.todate
                        });

                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                      SqlDbType.VarChar)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@flag",
                    SqlDbType.VarChar)
                        {
                            Value = dto.type
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
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
                            dto.reportlist = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {

                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_HomeworkUploadReport"; // Portal_HomeworkUploadRprt
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = dto.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = amclid });
                        cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = dto.fromdate });
                        cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar) { Value = dto.todate });
                        cmd.Parameters.Add(new SqlParameter("@Role", SqlDbType.VarChar) { Value = 1 });
                        cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = "HomeWork" });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = dto.HRME_Id });
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }

                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            dto.classreportlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                return dto;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
         public HomeWorkUploadDTO getreport_notice(HomeWorkUploadDTO dto)
        {
            try
            {
                string amclid = "0";
                if(dto.classarray.Length>0)
                {
                    foreach (var item in dto.classarray)
                    {
                        amclid = amclid + "," + item.ASMCL_Id;
                    }
                }
                
                var rolet = _Context.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.IVRMRT_Id).ToList();
                

                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_HomeworkUploadReport";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                   SqlDbType.VarChar)
                    {
                        Value = amclid
                    });

                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                    SqlDbType.VarChar)
                    {
                        Value = dto.fromdate
                    });

                    cmd.Parameters.Add(new SqlParameter("@todate",
                    SqlDbType.VarChar)
                    {
                        Value = dto.todate
                    });
                    
                    cmd.Parameters.Add(new SqlParameter("@Role",
                SqlDbType.VarChar)
                    {
                        Value =1
                    });
                     cmd.Parameters.Add(new SqlParameter("@Type",
                SqlDbType.VarChar)
                    {
                        Value = "NoticeBoard"
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
                        dto.classreportlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public HomeWorkUploadDTO Getdataview(HomeWorkUploadDTO dto)
        {
            try

            {
                var rolet = _Context.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.IVRMRT_Id).ToList();
                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_HomeworkClassworkView";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                   SqlDbType.VarChar)
                    {
                        Value = dto.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Type",
               SqlDbType.VarChar)
                    {
                        Value = dto.flag
                    });

                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                    SqlDbType.VarChar)
                    {
                        Value = dto.fromdate
                    });

                    cmd.Parameters.Add(new SqlParameter("@todate",
                    SqlDbType.VarChar)
                    {
                        Value = dto.todate
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
                        dto.view_array = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                //   using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                //   {
                //       cmd.CommandText = "Portal_HomeworkUploadReport";
                //       cmd.CommandType = CommandType.StoredProcedure;

                //       cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //SqlDbType.BigInt)
                //       {
                //           Value = dto.MI_Id
                //       });
                //       cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                //       SqlDbType.VarChar)
                //       {
                //           Value = dto.ASMAY_Id
                //       });
                //       cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                //      SqlDbType.VarChar)
                //       {
                //           Value = dto.ASMCL_Id
                //       });

                //       cmd.Parameters.Add(new SqlParameter("@fromdate",
                //       SqlDbType.VarChar)
                //       {
                //           Value = dto.fromdate
                //       });

                //       cmd.Parameters.Add(new SqlParameter("@todate",
                //       SqlDbType.VarChar)
                //       {
                //           Value = dto.todate
                //       });

                //       cmd.Parameters.Add(new SqlParameter("@Role",
                //   SqlDbType.VarChar)
                //       {
                //           Value = 1
                //       });
                //       cmd.Parameters.Add(new SqlParameter("@Type",
                //  SqlDbType.VarChar)
                //       {
                //           Value = "Homework"
                //       });

                //       if (cmd.Connection.State != ConnectionState.Open)
                //           cmd.Connection.Open();

                //       var retObject = new List<dynamic>();
                //       try
                //       {
                //           using (var dataReader = cmd.ExecuteReader())
                //           {
                //               while (dataReader.Read())
                //               {
                //                   var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                   for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                   {
                //                       dataRow.Add(
                //                           dataReader.GetName(iFiled),
                //                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                       );
                //                   }

                //                   retObject.Add((ExpandoObject)dataRow);
                //               }
                //           }
                //           dto.view_array = retObject.ToArray();
                //       }
                //       catch (Exception ex)
                //       {
                //           Console.WriteLine(ex.Message);
                //       }
                //   }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }


        //seen and unseen report cw and hw
        public HomeWorkUploadDTO getsection(HomeWorkUploadDTO dto)
        {
            try
            {

                var rolet = _Context.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.IVRMRT_Id).ToList();

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {

                    dto.HRME_Id = _Context.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                }
                else
                {
                    dto.HRME_Id = 0;
                }

                var asmcl_ids = "0";
                if (dto.classarray.Length > 0)
                {
                    foreach (var ue in dto.classarray)
                    {
                        asmcl_ids = asmcl_ids + "," + ue.ASMCL_Id;

                    }

                }


                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_StaffwiseSectionStdata_new";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                SqlDbType.VarChar)
                    {
                        Value = asmcl_ids
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
                        dto.sectionlist = retObject.ToArray();
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

        public HomeWorkUploadDTO getseenreport(HomeWorkUploadDTO dto)
        {
            try
            {

                

                var asmcl_ids = "0";
                if (dto.classarray.Length > 0)
                {
                    foreach (var ue in dto.classarray)
                    {
                        asmcl_ids = asmcl_ids + "," + ue.ASMCL_Id;

                    }

                }

                var sec_ids = "0";
                if (dto.sectionarray.Length > 0)
                {
                    foreach (var ue in dto.sectionarray)
                    {
                        sec_ids = sec_ids + "," + ue.ASMS_Id;

                    }

                }


                var rolet = _Context.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.IVRMRT_Id).ToList();

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {

                    dto.HRME_Id = _Context.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                }
                else
                {
                    dto.HRME_Id = 0;
                }

                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_SeenReport";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                        SqlDbType.VarChar)
                    {
                        Value = dto.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                        SqlDbType.VarChar)
                    {
                        Value = dto.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });
                   
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                SqlDbType.VarChar)
                    {
                        Value = asmcl_ids
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
              SqlDbType.VarChar)
                    {
                        Value = sec_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
             SqlDbType.VarChar)
                    {
                        Value = dto.flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
            SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
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
                        dto.seen_unseenlist = retObject.ToArray();
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

        public HomeWorkUploadDTO Getdataview_seen(HomeWorkUploadDTO dto)
        {
            try

            {

                var rolet = _Context.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.IVRMRT_Id).ToList();

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {

                    dto.HRME_Id = _Context.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                }
                else
                {
                    dto.HRME_Id = 0;
                }
                if(dto.flag=="NoticeBoard")
                {
                    dto.HRME_Id = 0;
                }
                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_HomeworkClassworkseen_list";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                   SqlDbType.VarChar)
                    {
                        Value = dto.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                  SqlDbType.VarChar)
                    {
                        Value = dto.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
               SqlDbType.VarChar)
                    {
                        Value = dto.flag
                    });

                    cmd.Parameters.Add(new SqlParameter("@seen_unseen",
                    SqlDbType.VarChar)
                    {
                        Value = dto.seen_unseen
                    });

                    //cmd.Parameters.Add(new SqlParameter("@todate",
                    //SqlDbType.VarChar)
                    //{
                    //    Value = dto.todate
                    //});
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
           SqlDbType.VarChar)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@topic_Id",
         SqlDbType.VarChar)
                    {
                        Value = dto.seen_Topicid.ToString()
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
                        dto.view_array = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                //   using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                //   {
                //       cmd.CommandText = "Portal_HomeworkUploadReport";
                //       cmd.CommandType = CommandType.StoredProcedure;

                //       cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //SqlDbType.BigInt)
                //       {
                //           Value = dto.MI_Id
                //       });
                //       cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                //       SqlDbType.VarChar)
                //       {
                //           Value = dto.ASMAY_Id
                //       });
                //       cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                //      SqlDbType.VarChar)
                //       {
                //           Value = dto.ASMCL_Id
                //       });

                //       cmd.Parameters.Add(new SqlParameter("@fromdate",
                //       SqlDbType.VarChar)
                //       {
                //           Value = dto.fromdate
                //       });

                //       cmd.Parameters.Add(new SqlParameter("@todate",
                //       SqlDbType.VarChar)
                //       {
                //           Value = dto.todate
                //       });

                //       cmd.Parameters.Add(new SqlParameter("@Role",
                //   SqlDbType.VarChar)
                //       {
                //           Value = 1
                //       });
                //       cmd.Parameters.Add(new SqlParameter("@Type",
                //  SqlDbType.VarChar)
                //       {
                //           Value = "Homework"
                //       });

                //       if (cmd.Connection.State != ConnectionState.Open)
                //           cmd.Connection.Open();

                //       var retObject = new List<dynamic>();
                //       try
                //       {
                //           using (var dataReader = cmd.ExecuteReader())
                //           {
                //               while (dataReader.Read())
                //               {
                //                   var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                   for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                   {
                //                       dataRow.Add(
                //                           dataReader.GetName(iFiled),
                //                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                       );
                //                   }

                //                   retObject.Add((ExpandoObject)dataRow);
                //               }
                //           }
                //           dto.view_array = retObject.ToArray();
                //       }
                //       catch (Exception ex)
                //       {
                //           Console.WriteLine(ex.Message);
                //       }
                //   }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }


        //added by akash
        public HomeWorkUploadDTO viewData(HomeWorkUploadDTO dto)
        {
            try
            {



                if (dto.Temp == 1)
                {
                    //HomeWork
                    dto.attachementlist = (from a in _Context.IVRM_HomeWork_Attatchment_DMO_con
                                           from b in _Context.IVRM_Homework_DMO
                                           where a.IHW_Id == dto.IHW_Id && a.IHW_Id == b.IHW_Id
                                           select new HomeWorkUploadDTO
                                           {
                                               IHW_Id = b.IHW_Id,
                                               IHW_Attachment = b.IHW_Attachment,
                                               IHWATT_Attachment = a.IHWATT_Attachment,
                                               IHWATT_FileName = a.IHWATT_FileName
                                           }).ToArray();
                }
                else
                {
                    //classWork
                    dto.attachementlist = (from a in _Context.IVRM_ClassWork_Attatchment_DMO_con
                                           from b in _Context.IVRM_ClassWorkDMO
                                           where a.ICW_Id == dto.ICW_Id && a.ICW_Id == b.ICW_Id
                                           select new IVRM_ClassWorkDTO
                                           {
                                               ICW_Id = b.ICW_Id,
                                               ICW_Attachment = b.ICW_Attachment,
                                               ICWATT_Attachment = a.ICWATT_Attachment,
                                               ICWATT_FileName = a.ICWATT_FileName
                                           }).ToArray();
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
