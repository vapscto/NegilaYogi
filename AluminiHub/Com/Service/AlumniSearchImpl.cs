using DataAccessMsSqlServerProvider;
using DomainModel.Model;
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
    public class AlumniSearchImpl : Interface.AlumniSearchInterface
    {
        private static ConcurrentDictionary<string, AlumniStudentDTO> _login =
          new ConcurrentDictionary<string, AlumniStudentDTO>();

        public DomainModelMsSqlServerContext _context;
        public AlumniSearchImpl(DomainModelMsSqlServerContext context)
        {
            _context = context;
        }
        public AlumniStudentDTO getData1(AlumniStudentDTO stud)
        {
            string Where = "";
            int count = stud.condition.Count;
            for (int i = 0; i < stud.field.Count; i++)
            {

                if (stud.Operator[i].ToString() == "like")
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
            List<AlumniStudentDTO> result = new List<AlumniStudentDTO>();
            //to get data according to search criteria.
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Alumnistudentsearch";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Where", SqlDbType.VarChar) { Value = Where });
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = stud.MI_Id });
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


        public AlumniStudentDTO getData(AlumniStudentDTO stud)
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
                                    Where += "  " + "(ALMST_FirstName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or  " +
                                        "ALMST_MiddleName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                        "ALMST_LastName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' )"
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
                                Where += "  " + "(ALMST_FirstName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or  " +
                                    "ALMST_MiddleName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                    "ALMST_LastName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' )";
                            }
                            else
                            {
                                Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%'"  + stud.stuStatus ;
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
                                    Where += "  " + "(ALMST_FirstName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                       "ALMST_MiddleName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                       "ALMST_LastName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' )"
                                       + " " + stud.condition[i].ToString();
                                }
                                else
                                {
                                    Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '" + stud.value[i].ToString() + "'" + stud.stuStatus + "'" + stud.condition[i].ToString();
                                }
                            }
                        }
                        else
                        {
                            if (stud.field[i].ToString() == "StudentName")
                            {
                                Where += "  " + "(ALMST_FirstName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                   "ALMST_MiddleName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or" +
                                   "ALMST_LastName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' )"
                                   ;
                            }
                            else
                            {
                                Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '" + stud.value[i].ToString() + "'";
                            }
                        }
                    }
                }

            }
            List<AlumniStudentDTO> result = new List<AlumniStudentDTO>();
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Alumnistudentsearch";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Where", SqlDbType.VarChar) { Value = Where });
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = stud.MI_Id });
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

        public AlumniStudentDTO GetddlDatabind(AlumniStudentDTO mas)
        {
            try
            {

         

            List<MasterAcademic> aya = new List<MasterAcademic>();
            aya = _context.AcademicYear.Where(d => d.MI_Id == mas.MI_Id && d.Is_Active == true).ToList();
            mas.YearList = aya.OrderByDescending(a => a.ASMAY_Order).ToArray();

            List<School_M_Class> aya1 = new List<School_M_Class>();
            aya1 = _context.School_M_Class.Where(d => d.MI_Id == mas.MI_Id && d.ASMCL_ActiveFlag == true).ToList();
            mas.classList = aya1.OrderBy(c => c.ASMCL_Order).ToArray();
            char[] trim = { ',', '@', '*', '"' };
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "AlumniCityAndOccupation";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = mas.MI_Id });
                cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = "City" });
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
                    mas.citylist = retObject.ToArray();

                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }

            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "AlumniCityAndOccupation";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = mas.MI_Id });
                cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = "Occupation" });
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
                    mas.occupationlist = retObject.ToArray();

                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }

            //mas.citylist = (from a in _context.Alumni_M_StudentDMO
            //                where a.MI_Id == mas.MI_Id && a.ALMST_ActiveFlag == true && a.ALMST_ConCity != null && a.ALMST_ConCity != ""
            //                select new AlumniStudentDTO
            //                {
            //                    ALMST_ConCity = a.ALMST_ConCity.Trim(trim)
            //                }).Distinct().ToArray();

            //mas.occupationlist = (from a in _context.Alumni_Student_Profession_DMO_con
            //                      where a.MI_Id == mas.MI_Id && a.ALSPR_ActiveFlg == true
            //                      select new AlumniStudentDTO
            //                      {
            //                          Designation = a.ALSPR_Designation
            //                      }).Distinct().ToArray();

            mas.countrylist = _context.country.ToArray();

            mas.statelistall = _context.State.Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return mas;
        }
    }
}
