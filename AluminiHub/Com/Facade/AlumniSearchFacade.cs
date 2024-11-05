using AlumniHub.Com.Interface;
using DataAccessMsSqlServerProvider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AlumniHub.Com.Facade
{
    [Route("api/[controller]")]
    public class AlumniSearchFacade : Controller
    {
        private readonly DomainModelMsSqlServerContext _db;
        AlumniSearchInterface _int;

        public AlumniSearchFacade(AlumniSearchInterface stu, DomainModelMsSqlServerContext db)
        {
            _int = stu;
            _db = db;
        }

        // GET api/values/5
        [HttpGet]
        public void Get(int id)
        {

        }

        // POST api/values
        [HttpPost]
        public AlumniStudentDTO Post([FromBody]AlumniStudentDTO value)
        {
            return _int.getData(value);
        }
        [Route("Getdetailsreport/")]
        public async Task<AlumniStudentDTO> Getdetailsreport([FromBody] AlumniStudentDTO stud)
        {

            //var classlist  = _db.School_M_Class.Where(a => a.MI_Id == stud.MI_Id && a.ASMCL_ActiveFlag == true).ToList();
            //var yearlist = _db.AcademicYear.Where(a => a.MI_Id == stud.MI_Id && a.Is_Active == true).ToList();
            //var occlist = _db.Alumni_Student_Profession_DMO_con.Where(a => a.MI_Id == stud.MI_Id && a.ALSPR_ActiveFlg == true).ToList();           
            //var contrylist = _db.country.Distinct().ToList();
            //var statelist = _db.State.Distinct().ToList();
            // var citylist = _db.Alumni_M_StudentDMO.Where(a => a.MI_Id == stud.MI_Id && a.ALMST_ActiveFlag == true).ToList();
            string classids = "0";
            string yearids = "0";
            string occids = "0";
            string cntryids = "0";
            string stateids = "0";
            string distids = "";

            if (stud.statelistarray.Length > 0)
            {
                foreach (var ue in stud.statelistarray)
                {
                    stateids = stateids + "," + ue.IVRMMS_Id;
                }
            }
            else
            {
                stateids = "";
            }

            if (stud.countrylistarray.Length > 0 && stud.countrylistarray != null)
            {
                foreach (var ue in stud.countrylistarray)
                {
                    cntryids = occids + "," + ue.IVRMMC_Id;
                }
            }
            else
            {
                cntryids = "";
            }
            if (stud.classlistnew.Length > 0 && stud.classlistnew != null)
            {
                foreach (var ue in stud.classlistnew)
                {
                    classids = classids + "," + ue.ASMCL_Id;
                }
            }
            else
            {
                classids = "";
            }


            if (stud.districtlistarray.Length > 0 && stud.districtlistarray != null)
            {
                foreach (var ue in stud.districtlistarray)
                {
                    if(distids !="")
                    {
                        distids = distids + "," + ue.IVRMMD_Id;
                    }
                    else
                    {
                        distids = ue.IVRMMD_Id.ToString();
                    }
                   
                }
            }
            else
            {
                distids = "";
            }


            if (stud.multipleBatchs != null && stud.multipleBatchs.Length > 0)
            {
                foreach (var d in stud.multipleBatchs)
                {
                    yearids = yearids + "," + d.ASMAY_Id;

                }
            }
            else
            {
                yearids = "";
            }

            //if (stud.ASMAY_Id == 0)
            //{
            //    yearids = "";

            //}
            //else
            //{
            //    yearids = stud.ASMAY_Id.ToString();
            //}

            if (stud.Occupation == null || stud.Occupation == "0")
            {
                occids = "";

            }
            else
            {
                occids = stud.Occupation;
            }



            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                //AlumnistudentsearchReport_new
                //AlumnistudentsearchReport
                //cmd.CommandText = "AlumnistudentsearchReport_srkvs_test";
                cmd.CommandText = "AlumnistudentsearchReport_srkvs";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = stud.MI_Id });
                cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar) { Value = yearids });
                cmd.Parameters.Add(new SqlParameter("@clas", SqlDbType.VarChar) { Value = classids });
                cmd.Parameters.Add(new SqlParameter("@Occupation", SqlDbType.VarChar) { Value = occids });
                cmd.Parameters.Add(new SqlParameter("@city", SqlDbType.VarChar) { Value = stud.city });
                cmd.Parameters.Add(new SqlParameter("@IVRMMC_Id", SqlDbType.VarChar) { Value = cntryids });
                cmd.Parameters.Add(new SqlParameter("@IVRMMS_Id", SqlDbType.VarChar) { Value = stateids });
                cmd.Parameters.Add(new SqlParameter("@district", SqlDbType.VarChar) { Value = distids });

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
                    stud.SearchstudentDetails = retObject.ToArray();
                    if (stud.SearchstudentDetails.Length > 0)
                    {
                        stud.count = stud.SearchstudentDetails.Length;
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

        [Route("getstate/")]
        public async Task<AlumniStudentDTO> getstate([FromBody] AlumniStudentDTO dto)
        {
            try
            {

                var country_ids = "0";
                if (dto.countrylistarray.Length > 0)
                {
                    foreach (var ue in dto.countrylistarray)
                    {
                        country_ids = country_ids + "," + ue.IVRMMC_Id;

                    }

                }


                //if (dto.IVRMMC_Id == 0)
                //{
                //    dto.statelist = _db.State.Distinct().ToArray();
                //}
                //else
                //{
                //    dto.statelist = _db.State.Where(a => a.IVRMMC_Id == dto.IVRMMC_Id && country_ids.Contains(a.IVRMMC_Id)).ToArray();
                //}




                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AlumniCountries";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@IVRMMC_Ids", SqlDbType.VarChar) { Value = country_ids });

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
                        dto.statelist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }



                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {

                    //  cmd.CommandText = "AlumniCityAndOccupation_test";
                    cmd.CommandText = "AlumniCityAndOccupation_test";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = "City" });
                    cmd.Parameters.Add(new SqlParameter("@IVRMMC_Ids", SqlDbType.BigInt) { Value = country_ids });
                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.BigInt) { Value = "Country" });

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
                        dto.citylist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    //  cmd.CommandText = "AlumniCityAndOccupation_test";
                    cmd.CommandText = "AlumniCityAndOccupation_test";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = "Occupation" });
                    cmd.Parameters.Add(new SqlParameter("@IVRMMC_Ids", SqlDbType.BigInt) { Value = country_ids });
                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.BigInt) { Value = "Country" });
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
                        dto.occupationlist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);

            }
            return dto;
        }
        [Route("getdistrict/")]
        public async Task<AlumniStudentDTO> getdistrict([FromBody] AlumniStudentDTO dto)
        {
            try
            {
                //if (dto.IVRMMS_Id == 0)
                //{

                //    dto.districtlist = _db.DistrictDMO.ToArray();

                //}
                //else
                //{
                //    dto.districtlist = _db.DistrictDMO.Where(a => a.IVRMMS_Id == dto.IVRMMS_Id).ToArray();
                // dto.citylist = _db.city.Where(a => a.IVRMMS_Id == dto.IVRMMS_Id).ToArray();
                //}

                var state_ids = "0";
                if (dto.statelistarray.Length > 0)
                {
                    foreach (var ue in dto.statelistarray)
                    {
                        state_ids = state_ids + "," + ue.IVRMMS_Id;

                    }

                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AlumniCountriesWiseDistrict";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@IVRMMS_Ids", SqlDbType.VarChar) { Value = state_ids });

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
                        dto.districtlist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }







            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);

            }


            return dto;
        }
        [HttpGet("{id}")]
        [Route("Getdetails")]
        public AlumniStudentDTO Getdetails([FromBody]AlumniStudentDTO castecategoryDTO)//int IVRMM_Id
        {
            //SchoolYearWiseStudentDTO castecategoryDTO = new SchoolYearWiseStudentDTO();
            //  castecategoryDTO.MI_Id = mi_id;
            return _int.GetddlDatabind(castecategoryDTO);

        }


    }
}
