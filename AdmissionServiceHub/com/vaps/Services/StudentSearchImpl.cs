using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.admission;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vaps.admission;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class StudentSearchImpl : Interfaces.StudentSearchInterface
    {
        private static ConcurrentDictionary<string, Adm_M_StudentDTO> _login =
          new ConcurrentDictionary<string, Adm_M_StudentDTO>();

        public DomainModelMsSqlServerContext _context;
        public StudentSearchImpl(DomainModelMsSqlServerContext context)
        {
            _context = context;
        }
        public Adm_M_StudentDTO getData1(Adm_M_StudentDTO stud)
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
                                Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%'" + " and" + " AMST_SOL=" + " '" + stud.stuStatus + "'" + " " + stud.condition[i].ToString();

                            }
                        }
                        else
                        {
                            Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%'" + " and" + " AMST_SOL=" + " '" + stud.stuStatus + "'";

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
                                Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '" + stud.value[i].ToString() + "'" + " and" + " AMST_SOL=" + " '" + stud.stuStatus + "'" + " " + stud.condition[i].ToString();

                            }
                        }
                        else
                        {
                            Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '" + stud.value[i].ToString() + "'" + " and" + " AMST_SOL=" + " '" + stud.stuStatus + "'";

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



            //if (dto.Operator == "like")
            //{
            //    if (stud.stuStatus != "all")
            //    {
            //        Where = " " + dto.field + " " + dto.Operator + " '%" + dto.value + "%'" + " and" + " AMST_SOL=" + " '" + stud.stuStatus + "'";
            //    }
            //    else
            //    {
            //        Where = " " + dto.field + " " + dto.Operator + " '%" + dto.value + "%'";
            //    }

            //}
            //else
            //{
            //    if (stud.stuStatus != "all")
            //    {
            //        Where = " " + dto.field + dto.Operator + " '" + dto.value + "'" + " and" + " AMST_SOL=" + " '" + stud.stuStatus + "'";
            //    }
            //    else
            //    {
            //        Where = " " + dto.field + dto.Operator + " '" + dto.value + "'";
            //    }
            //}
            List<Adm_M_StudentDTO> result = new List<Adm_M_StudentDTO>();

            //to get data according to search criteria.
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {

                cmd.CommandText = "getStudentSearchData1";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Where", SqlDbType.VarChar) { Value = Where });
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = stud.MI_Id });
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();
                try
                {
                    //using (var dataReader = cmd.ExecuteReader())
                    //{
                    //    while (dataReader.Read())
                    //    {
                    //        if(dataReader["AMST_BirthCertNO"].ToString()==null || dataReader["AMST_BirthCertNO"].ToString() == "")
                    //        {
                    //           stud.AMST_BirthCertNO = "N/A";
                    //        }
                    //        else
                    //        {
                    //            stud.AMST_BirthCertNO = dataReader["AMST_BirthCertNO"].ToString();
                    //        }
                    //        if (dataReader["AMST_BloodGroup"].ToString() == null || dataReader["AMST_BloodGroup"].ToString() == "")
                    //        {
                    //            stud.AMST_BloodGroup = "N/A";
                    //        }
                    //        else
                    //        {
                    //            stud.AMST_BloodGroup = dataReader["AMST_BloodGroup"].ToString();
                    //        }
                    //        if (dataReader["AMST_FatherName"].ToString() == null || dataReader["AMST_FatherName"].ToString() == "")
                    //        {
                    //            stud.AMST_FatherName = "N/A";
                    //        }
                    //        else
                    //        {
                    //            stud.AMST_FatherName = dataReader["AMST_FatherName"].ToString();
                    //        }
                    //        if (dataReader["AMST_FatherAadharNo"].ToString() == null || dataReader["AMST_FatherAadharNo"].ToString() == "")
                    //        {
                    //            stud.AMST_FatherAadharNo = 0;
                    //        }
                    //        else
                    //        {
                    //            stud.AMST_FatherAadharNo =Convert.ToInt64(dataReader["AMST_FatherAadharNo"]);
                    //        }
                    //        if (dataReader["AMST_FatherBankAccNo"].ToString() == null || dataReader["AMST_FatherBankAccNo"].ToString() == "")
                    //        {
                    //            stud.AMST_FatherBankAccNo = "N/A";
                    //        }
                    //        else
                    //        {
                    //            stud.AMST_FatherBankAccNo = dataReader["AMST_FatherBankAccNo"].ToString();
                    //        }
                    //        if (dataReader["AMST_MotherName"].ToString() == null || dataReader["AMST_MotherName"].ToString() == "")
                    //        {
                    //            stud.AMST_MotherName = "N/A";
                    //        }
                    //        else
                    //        {
                    //            stud.AMST_MotherName = dataReader["AMST_MotherName"].ToString();
                    //        }
                    //        if (dataReader["AMST_StuBankAccNo"].ToString() == null || dataReader["AMST_StuBankAccNo"].ToString() == "")
                    //        {
                    //            stud.AMST_StuBankAccNo = "N/A";
                    //        }
                    //        else
                    //        {
                    //            stud.AMST_StuBankAccNo = dataReader["AMST_StuBankAccNo"].ToString();
                    //        }

                    //        if (dataReader["AMST_RegistrationNo"].ToString() == null || dataReader["AMST_RegistrationNo"].ToString() == "")
                    //        {
                    //            stud.AMST_StuBankAccNo = "N/A";
                    //        }
                    //        else
                    //        {
                    //            stud.AMST_RegistrationNo = dataReader["AMST_RegistrationNo"].ToString();
                    //        }
                    //        if (dataReader["AMST_FirstName"].ToString() == null || dataReader["AMST_FirstName"].ToString() == "")
                    //        {
                    //            stud.AMST_FirstName = "N/A";
                    //        }
                    //        else
                    //        {
                    //            stud.AMST_FirstName = dataReader["AMST_FirstName"].ToString();
                    //        }

                    //        if (dataReader["AMST_AadharNo"].ToString() == null || dataReader["AMST_AadharNo"].ToString() == "")
                    //        {
                    //            stud.AMST_AadharNo = Convert.ToInt64("N/A");
                    //        }
                    //        else
                    //        {
                    //            stud.AMST_AadharNo = Convert.ToInt64(dataReader["AMST_AadharNo"].ToString());
                    //        }
                    //        if (dataReader["AMST_AdmNo"].ToString() == null || dataReader["AMST_AdmNo"].ToString() == "")
                    //        {
                    //            stud.AMST_AdmNo = "N/A";
                    //        }
                    //        else
                    //        {
                    //            stud.AMST_AdmNo = dataReader["AMST_AdmNo"].ToString();
                    //        }
                    //        result.Add(new Adm_M_StudentDTO
                    //        {
                    //            AMST_RegistrationNo = stud.AMST_RegistrationNo,
                    //            AMST_FirstName = stud.AMST_FirstName,
                    //            AMST_Date = Convert.ToDateTime(dataReader["AMST_Date"]),
                    //            AMST_DOB = Convert.ToDateTime(dataReader["AMST_DOB"]),
                    //            AMST_Sex = Convert.ToString(dataReader["AMST_Sex"]),
                    //            AMST_AadharNo = stud.AMST_AadharNo,
                    //            AMST_AdmNo = stud.AMST_AdmNo,
                    //            AMST_BirthCertNO = stud.AMST_BirthCertNO,
                    //            AMST_BloodGroup= stud.AMST_BloodGroup,
                    //            AMST_emailId=dataReader["AMST_emailId"].ToString(),
                    //            AMST_FatherName = stud.AMST_FatherName,
                    //            AMST_FatherAadharNo= stud.AMST_FatherAadharNo,
                    //            AMST_FatherBankAccNo = stud.AMST_FatherBankAccNo,
                    //            AMST_MotherName= stud.AMST_MotherName,
                    //            AMST_MobileNo=Convert.ToInt64(dataReader["AMST_MobileNo"]),
                    //            AMST_StuBankAccNo= stud.AMST_StuBankAccNo
                    //        });
                    //        stud.searchResult = result.ToArray();
                    //    }
                    //}

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
        public Adm_M_StudentDTO getData(Adm_M_StudentDTO stud)
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
                                    Where += "  " + "(AMST_FirstName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                        "AMST_MiddleName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                        "AMST_LastName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' )"
                                         + " and" + " AMST_SOL=" + " '" + stud.stuStatus + "'" + " " + stud.condition[i].ToString();
                                }

                                else
                                {
                                    Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%'" + " and" + " AMST_SOL=" + " '" + stud.stuStatus + "'" + " " + stud.condition[i].ToString();
                                }
                            }
                        }
                        else
                        {
                            if (stud.field[i].ToString() == "StudentName")
                            {
                                Where += "  " + "(AMST_FirstName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                    "AMST_MiddleName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                    "AMST_LastName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' )"
                                     + " and" + " AMST_SOL=" + " '" + stud.stuStatus + "'";
                            }
                            else if (stud.field[i].ToString() == "Address")
                            {
                                Where += "  " + "(IVRMMC_CountryName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                   "IVRMMS_Name " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                   "IVRMMD_Name " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' )"
                                    ;
                            }
                            else
                            {
                                Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%'" + " and" + " AMST_SOL=" + " '" + stud.stuStatus + "'";
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
                                    Where += "  " + "(AMST_FirstName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                    "AMST_MiddleName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                    "AMST_LastName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' )";
                                }
                                else if (stud.field[i].ToString() == "Address")
                                {
                                    Where += "  " + "(IVRMMC_CountryName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                       "IVRMMS_Name " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                       "IVRMMD_Name " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' )"
                                        ;
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
                                Where += "  " + "(AMST_FirstName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                "AMST_MiddleName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                "AMST_LastName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' )";
                            }
                            else if (stud.field[i].ToString() == "Address")
                            {
                                Where += "  " + "(IVRMMC_CountryName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                   "IVRMMS_Name " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                   "IVRMMD_Name " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' )"
                                    ;
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
                                    Where += "  " + "(AMST_FirstName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or" +
                                       "AMST_MiddleName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                       "AMST_LastName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' )"
                                        + " and" + " AMST_SOL=" + " '" + stud.stuStatus + "'" + " " + stud.condition[i].ToString();
                                }
                                else
                                {
                                    Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '" + stud.value[i].ToString() + "'" + " and" + " AMST_SOL=" + " '" + stud.stuStatus + "'" + " " + stud.condition[i].ToString();
                                }
                            }
                        }
                        else
                        {
                            if (stud.field[i].ToString() == "StudentName")
                            {
                                Where += "  " + "(AMST_FirstName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or" +
                                   "AMST_MiddleName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or" +
                                   "AMST_LastName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' )"
                                    + " and" + " AMST_SOL=" + " '" + stud.stuStatus + "'";
                            }
                            else if (stud.field[i].ToString() == "Address")
                            {
                                Where += "  " + "(IVRMMC_CountryName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                   "IVRMMS_Name " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                   "IVRMMD_Name " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' )"
                                    ;
                            }
                            else
                            {
                                Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '" + stud.value[i].ToString() + "'" + " and" + " AMST_SOL=" + " '" + stud.stuStatus + "'";
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
                                    Where += "  " + "(AMST_FirstName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                    "AMST_MiddleName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or" +
                                    "AMST_LastName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' ) " + stud.condition[i].ToString();
                                }
                                else if (stud.field[i].ToString() == "Address")
                                {
                                    Where += "  " + "(IVRMMC_CountryName " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                       "IVRMMS_Name " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' or " +
                                       "IVRMMD_Name " + stud.Operator[i].ToString() + " '%" + stud.value[i].ToString() + "%' )";
                                }
                                else
                                {
                                    Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '" + stud.value[i].ToString() + "'" + " " + stud.condition[i].ToString();
                                }
                            }
                        }
                        else
                        {
                            if (stud.field[i].ToString() == "Address")
                            {
                                Where += "  " + "(IVRMMC_CountryName " + stud.Operator[i].ToString() + " '" + stud.value[i].ToString() + "' or " +
                                       "IVRMMS_Name " + stud.Operator[i].ToString() + " '" + stud.value[i].ToString() + "' or " +
                                       "IVRMMD_Name " + stud.Operator[i].ToString() + " '" + stud.value[i].ToString() + "' )";
                            }
                            else
                            {
                                Where += " " + stud.field[i].ToString() + " " + stud.Operator[i].ToString() + " '" + stud.value[i].ToString() + "'";
                            }

                        }
                    }


                }

            }
            List<Adm_M_StudentDTO> result = new List<Adm_M_StudentDTO>();

            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {

                cmd.CommandText = "getStudentSearchData1";
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


            stud.getinstitution = _context.Institute.Where(a => a.MI_Id == stud.MI_Id && a.MI_ActiveFlag == 1).ToArray();

            return stud;
        }
    }
}
