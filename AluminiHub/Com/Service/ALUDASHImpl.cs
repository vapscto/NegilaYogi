using AlumniHub.Com.Interface;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Alumni;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Alumni;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AlumniHub.Com.Service
{
    public class ALUDASHImpl : ALUDASHInterface
    {
        private static ConcurrentDictionary<string, CLGAlumniStudentDTO> _login =
     new ConcurrentDictionary<string, CLGAlumniStudentDTO>();

        public AlumniContext _AlumniContext;
        private readonly DomainModelMsSqlServerContext _db;
        public ALUDASHImpl(AlumniContext AlumniContext, DomainModelMsSqlServerContext db)
        {
            _AlumniContext = AlumniContext;
            _db = db;
        }
        public CLGAlumniStudentDTO getloaddata(CLGAlumniStudentDTO dto)
        {
            try
            {
                string rolename = _AlumniContext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == dto.roleid).IVRMRT_Role;

                dto.rolename = rolename;

                #region  CALENDER 
                try
                {
                    int month = DateTime.Now.Month;
                    dto.calenderlist = (from m in _AlumniContext.COE_Master_EventsDMO
                                        from n in _AlumniContext.COE_EventsDMO
                                        where (m.COEME_Id == n.COEME_Id && n.MI_Id == dto.MI_Id && n.ASMAY_Id==dto.ASMAY_Id && n.COEE_AlumniFlag==true && n.COEE_EStartDate.Value.Month == month)
                                        select new CLGAlumniStudentDTO
                                        {
                                            COEME_EventName = m.COEME_EventName,
                                            COEME_EventDesc = m.COEME_EventDesc,
                                            COEE_EStartDate = n.COEE_EStartDate,
                                            COEE_EEndDate = n.COEE_EEndDate,
                                            COEE_ReminderDate = n.COEE_ReminderDate
                                        }).OrderByDescending(c => c.COEE_EStartDate).Distinct().ToArray();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                #endregion

                using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLGAlumniDashboard1";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = dto.MI_Id });
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
                        dto.batch = retObject.ToArray();
                        if (dto.batch.Length > 0)
                        {
                            dto.count = dto.batch.Length;
                        }
                        else
                        {
                            dto.count = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLGAlumniDashboard2";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = dto.MI_Id });
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
                        dto.alumnilist = retObject.ToArray();
                        if (dto.alumnilist.Length > 0)
                        {
                            dto.count = dto.alumnilist.Length;
                        }
                        else
                        {
                            dto.count = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLGALUMNI_BirthdayList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar){ Value = dto.MI_Id });
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
                        dto.alumnibirthday = retObject.ToArray();
                        if (dto.alumnibirthday.Length > 0)
                        {
                            dto.count = dto.alumnibirthday.Length;
                        }
                        else
                        {
                            dto.count = 0;
                        }
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
    }
}
