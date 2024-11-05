using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DomainModel;
using PreadmissionDTOs;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DomainModel.Model.com.vapstech.admission;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Dynamic;
using System.Data.SqlClient;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class RFIDDashboardImpl : Interfaces.RFIDDashboardInterface
    {
        private static ConcurrentDictionary<string, RFIDDashboardDTO> _login =
            new ConcurrentDictionary<string, RFIDDashboardDTO>();

        public ActivateDeactivateContext _ActivateDeactivateContext;
        private readonly UserManager<ApplicationUser> _UserManager;
        public RFIDDashboardImpl(ActivateDeactivateContext ActivateDeactivateContext, UserManager<ApplicationUser> UserManager)
        {
            _ActivateDeactivateContext = ActivateDeactivateContext;
            _UserManager = UserManager;
        }
        
        public RFIDDashboardDTO Getdetails(RFIDDashboardDTO data)
        {
            try
            {

                using (var cmd = _ActivateDeactivateContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_RFID_STUDENT_TOTAL";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@DATE",
                      SqlDbType.Date)
                    {
                        Value = data.adate
                    });

                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                      SqlDbType.Char)
                    {
                        Value = "STDCNT"
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
                        data.totalstudentlist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                using (var cmd = _ActivateDeactivateContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_RFID_STUDENT_TOTAL";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@DATE",
                      SqlDbType.Date)
                    {
                        Value = data.adate
                    });

                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                      SqlDbType.Char)
                    {
                        Value = "PCNT"
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
                        data.presentstudentlist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


                using (var cmd = _ActivateDeactivateContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_RFID_STUDENT_LIST";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@DATE",
                      SqlDbType.Date)
                    {
                        Value = data.adate
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
                        data.studentlist = retObject.ToArray();
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

            return data;
        }
        public RFIDDashboardDTO showstudentGrid(RFIDDashboardDTO data)
        {
            try
            {
                using (var cmd = _ActivateDeactivateContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_RFID_IN_OUT_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@DATE",
                      SqlDbType.Date)
                    {
                        Value = data.IRFPU_DateTime
                    });
                    cmd.Parameters.Add(new SqlParameter("@CARDNO",
                      SqlDbType.VarChar)
                    {
                        Value = data.AMST_RFCardNo
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                      SqlDbType.VarChar)
                    {
                        Value = "IN"
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
                        data.inlist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }




                using (var cmd = _ActivateDeactivateContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_RFID_IN_OUT_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@DATE",
                      SqlDbType.Date)
                    {
                        Value = data.IRFPU_DateTime
                    });
                    cmd.Parameters.Add(new SqlParameter("@CARDNO",
                      SqlDbType.VarChar)
                    {
                        Value = data.AMST_RFCardNo
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                      SqlDbType.VarChar)
                    {
                        Value = "OUT"
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
                        data.outlist = retObject.ToArray();
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

            return data;
        }

        public RFIDDashboardDTO cleardata(RFIDDashboardDTO data)
        {
            try
            {
                using (var cmd = _ActivateDeactivateContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "DELETE_RFID_IN_OUT_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@DATE",
                      SqlDbType.Date)
                    {
                        Value = data.adate
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
                        data.inlist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


                var regstatus = _ActivateDeactivateContext.Database.ExecuteSqlCommand("DELETE_RFID_IN_OUT_DETAILS @p0,@p1,@p2", data.MI_Id, data.ASMAY_Id, data.adate );
                if (Convert.ToInt32(regstatus) > 0)
                {
                    data.returnval =true;
                }
                else
                {
                    data.returnval = true;
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
 
    }
}
