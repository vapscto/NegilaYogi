using PreadmissionDTOs.com.vaps.College.Portals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vaps.admission;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DataAccessMsSqlServerProvider.com.vapstech.College.Portal;
using DomainModel.Model.com.vapstech.Portals.Employee;


namespace CollegePortals.com.Chairman.Services
{
    public class CLGCHStudentStrengthImpl : Interfaces.CLGCHStudentStrengthInterface
    {
        private static ConcurrentDictionary<string, CLGCHStudentStrengthDTO> _login =
           new ConcurrentDictionary<string, CLGCHStudentStrengthDTO>();
        private CollegeportalContext _ClgPortalContext;
        public CLGCHStudentStrengthImpl(CollegeportalContext ClgPortalContext)
        {
            _ClgPortalContext = ClgPortalContext;
        }
        public async Task<CLGCHStudentStrengthDTO> Getdetails(CLGCHStudentStrengthDTO data)
        {
            try
            {

                #region ACADEMIC YEAR
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _ClgPortalContext.academicYearDMO.Where(t => t.MI_Id == data.MI_Id  && t.Is_Active==true).Distinct().OrderBy(t=>t.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();
                #endregion

            


                #region STUDENT STRENGTH
                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_CHAIRMANPORTAL_TOTAL_STD_STRENGTH_1";
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
                    cmd.Parameters.Add(new SqlParameter("@TC",
                    SqlDbType.Bit)
                    {
                        Value = data.withtc
                    });
                    cmd.Parameters.Add(new SqlParameter("@DE",
                    SqlDbType.Bit)
                    {
                        Value = data.withdeactive
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
                        data.Fillstudentstrenth = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                #endregion

              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<CLGCHStudentStrengthDTO> Getdetails1(CLGCHStudentStrengthDTO data)
        {
            try
            {

                #region ACADEMIC YEAR
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _ClgPortalContext.academicYearDMO.Where(t => t.MI_Id == data.MI_Id  && t.Is_Active == true).Distinct().OrderBy(t => t.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();
                #endregion




                #region REGULAR STUDENT STRENGTH
                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_CHAIRMANPORTAL_TOTAL_STD_STRENGTH_1";
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
                    cmd.Parameters.Add(new SqlParameter("@TC",
                    SqlDbType.Bit)
                    {
                        Value = data.withtc
                    });
                    cmd.Parameters.Add(new SqlParameter("@DE",
                    SqlDbType.Bit)
                    {
                        Value = data.withdeactive
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
                        data.Fillstudentstrenth = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                #endregion
                #region NEW STUDENT STRENGTH
                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_CHAIRMANPORTAL_TOTAL_STD_STRENGTH_NEWSTD";
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
                    cmd.Parameters.Add(new SqlParameter("@TC",
                    SqlDbType.Bit)
                    {
                        Value = data.withtc
                    });
                    cmd.Parameters.Add(new SqlParameter("@DE",
                    SqlDbType.Bit)
                    {
                        Value = data.withdeactive
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
                        data.fillnewstd = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}