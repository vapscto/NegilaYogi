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
    public class CLGAlumnistudentsearchipImpl : CLGAlumnistudentsearchInterface
    {
        private static ConcurrentDictionary<string, CLGAlumniStudentDTO> _login =
      new ConcurrentDictionary<string, CLGAlumniStudentDTO>();

        public AlumniContext _AlumniContext;
        private readonly DomainModelMsSqlServerContext _db;
        public CLGAlumnistudentsearchipImpl(AlumniContext AlumniContext, DomainModelMsSqlServerContext db)
        {
            _AlumniContext = AlumniContext;
            _db = db;
        }
        public CLGAlumniStudentDTO getData1(CLGAlumniStudentDTO stud)
        {
            string Where = "";
            int count = stud.condition.Count;
            for (int i = 0; i < stud.field.Count; i++)
            {

                if (stud.Operator[i].ToString() == "like")
                {
                    if (stud.stuStatus != "all")
                    {
                        if (count > i)
                        {
                            if (stud.condition[i] != null)
                            {
                                Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%'" + " '" + stud.stuStatus + "'" + " " + stud.condition[i].ToString();

                            }
                        }
                        else
                        {
                            Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%'" + " '" + stud.stuStatus + "'";

                        }
                    }
                    else
                    {
                        if (count > i)
                        {
                            if (stud.condition[i] != null)
                            {
                                Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%'" + " " + stud.condition[i].ToString();

                            }
                        }
                        else
                        {
                            Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%'";

                        }
                    }
                }
                else
                {
                    if (stud.stuStatus != "all")
                    {
                        if (count > i)
                        {
                            if (stud.condition[i] != null)
                            {
                                Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '" + stud.value[i].ToString() + "'" + " '" + stud.stuStatus + "'" + " " + stud.condition[i].ToString();

                            }
                        }
                        else
                        {
                            Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '" + stud.value[i].ToString() + "'" + " '" + stud.stuStatus + "'";

                        }
                    }
                    else
                    {
                        if (count > i)
                        {
                            if (stud.condition[i] != null)
                            {
                                Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '" + stud.value[i].ToString() + "'" + " " + stud.condition[i].ToString();

                            }
                        }
                        else
                        {
                            Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '" + stud.value[i].ToString() + "'";
                        }
                    }

                }

            }


            
            List<CLGAlumniStudentDTO> result = new List<CLGAlumniStudentDTO>();
            using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "CLGAlumniStudentSearch";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Where", SqlDbType.VarChar) { Value = Where });
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = stud.MI_Id });
                cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.VarChar) { Value = stud.ASMAY_Id });
                cmd.Parameters.Add(new SqlParameter("@amco_id", SqlDbType.VarChar) { Value = stud.AMCO_Id });
                cmd.Parameters.Add(new SqlParameter("@amb_id", SqlDbType.VarChar) { Value = stud.AMB_Id });
                cmd.Parameters.Add(new SqlParameter("@amse_id", SqlDbType.VarChar) { Value = stud.AMSE_Id });
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
                    stud.searchResult = retObject.ToArray();
                    if (stud.searchResult.Length > 0)
                    {
                        stud.count = stud.searchResult.Length;
                    }
                    else
                    {
                        stud.count = 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            return stud;
        }


        public CLGAlumniStudentDTO getData(CLGAlumniStudentDTO stud)
        {
            string Where = "";
            int count = stud.condition.Count;
            for (int i = 0; i < stud.field.Count; i++)
            {

                if (stud.Operator[i].ToString() == "like")
                {
                    if (stud.stuStatus != "all")
                    {
                        if (count > i)
                        {
                            if (stud.condition[i] != null)
                            {
                                if (stud.field[i].ToString() == "StudentName")
                                {
                                    Where += "  " + "(ALCMST_FirstName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                        "ALCMST_MiddleName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                        "ALCMST_LastName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' )"
                                          + " " + stud.condition[i].ToString();
                                }

                                else
                                {
                                    Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%'" + " " + stud.condition[i].ToString();
                                }
                            }
                        }
                        else
                        {
                            if (stud.field[i].ToString() == "StudentName")
                            {
                                Where += "  " + "(ALCMST_FirstName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                    "ALCMST_MiddleName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                    "ALCMST_LastName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' )"
                                     ;
                            }
                            else
                            {
                                Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%'";
                            }
                        }
                    }
                    else
                    {
                        if (count > i)
                        {
                            if (stud.condition[i] != null)
                            {
                                if (stud.field[i].ToString() == "StudentName")
                                {
                                    Where += "  " + "(ALCMST_FirstName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                    "ALCMST_MiddleName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                    "ALCMST_LastName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' )";
                                }
                                else
                                {
                                    Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%'" + " " + stud.condition[i].ToString();
                                }
                            }
                        }
                        else
                        {
                            if (stud.field[i].ToString() == "StudentName")
                            {
                                Where += "  " + "(ALCMST_FirstName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                "ALCMST_MiddleName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                "ALCMST_LastName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' )";
                            }
                            else
                            {
                                Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%'";
                            }
                        }
                    }
                }
                else
                {
                    if (stud.stuStatus != "all")
                    {
                        if (count > i)
                        {
                            if (stud.condition[i] != null)
                            {
                                if (stud.field[i].ToString() == "StudentName")
                                {
                                    Where += "  " + "(ALCMST_FirstName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or" +
                                       "ALCMST_MiddleName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                       "ALCMST_LastName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' )"
                                        + " " + stud.condition[i].ToString();
                                }
                                else
                                {
                                    Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '" + stud.value[i].ToString() + "'" + " and" + "'" + " " + stud.condition[i].ToString();
                                }
                            }
                        }
                        else
                        {
                            if (stud.field[i].ToString() == "StudentName")
                            {
                                Where += "  " + "(ALCMST_FirstName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or" +
                                   "ALCMST_MiddleName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or" +
                                   "ALCMST_LastName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' )"
                                    ;
                            }
                            else
                            {
                                Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '" + stud.value[i].ToString() + "'";
                            }
                        }
                    }
                    else
                    {
                        if (count > i)
                        {
                            if (stud.condition[i] != null)
                            {
                                if (stud.field[i].ToString() == "StudentName")
                                {
                                    Where += "  " + "(ALCMST_FirstName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                    "ALCMST_MiddleName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or" +
                                    "ALCMST_LastName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' ) " + stud.condition[i].ToString();
                                }
                                else
                                {
                                    Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '" + stud.value[i].ToString() + "'" + " " + stud.condition[i].ToString();
                                }
                            }
                        }
                        else
                        {
                            Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '" + stud.value[i].ToString() + "'";
                        }
                    }

                }

            }
            List<CLGAlumniStudentDTO> result = new List<CLGAlumniStudentDTO>();
            using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "CLGAlumniStudentSearch";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Where", SqlDbType.VarChar) { Value = Where });
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = stud.MI_Id });
                cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.VarChar) { Value = stud.ASMAY_Id });
                cmd.Parameters.Add(new SqlParameter("@amco_id", SqlDbType.VarChar) { Value = stud.AMCO_Id });
                cmd.Parameters.Add(new SqlParameter("@amb_id", SqlDbType.VarChar) { Value = stud.AMB_Id });
                cmd.Parameters.Add(new SqlParameter("@amse_id", SqlDbType.VarChar) { Value = stud.AMSE_Id });
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
                    stud.searchResult = retObject.ToArray();
                    if (stud.searchResult.Length > 0)
                    {
                        stud.count = stud.searchResult.Length;
                    }
                    else
                    {
                        stud.count = 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            return stud;
        }

        public CLGAlumniStudentDTO getsemdata(CLGAlumniStudentDTO data)
        {
            try
            {
                using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLGAlumniStudentReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@amco_id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@amb_id", SqlDbType.VarChar) { Value = data.AMB_Id });
                    cmd.Parameters.Add(new SqlParameter("@amse_id", SqlDbType.VarChar) { Value = data.AMSE_Id });
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
                        data.searchResult = retObject.ToArray();
                        if (data.searchResult.Length > 0)
                        {
                            data.count = data.searchResult.Length;
                        }
                        else
                        {
                            data.count = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}




