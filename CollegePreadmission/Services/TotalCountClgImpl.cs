using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Preadmission;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Text;
using DomainModel.Model.com.vapstech.College.Preadmission;
using DomainModel.Model.com.vapstech.College.Admission;

namespace CollegePreadmission.Services
{
    public class TotalCountClgImpl : Interfaces.TotalCountClgReportInterface
    {
        CollegepreadmissionContext _precontext;
        ClgAdmissionContext _context;

        private readonly DomainModelMsSqlServerContext _db;


        public TotalCountClgImpl(ClgAdmissionContext context, CollegepreadmissionContext precontext, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _precontext = precontext;
            _db = db;

        }

        public async Task<CollegePreadmissionstudnetDto> Get_Intial_data(CollegePreadmissionstudnetDto data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(d => d.ASMAY_Order).ToList();
                data.fillyear = year.ToArray();


                //List<School_M_Class> classname = new List<School_M_Class>();
                //classname = _db.admissioncls.ToList();
                //data.fillclass = classname.Where(t => t.MI_Id == data.MI_Id).ToArray();

                data.fillcourse = _precontext.MasterCourseDMO.Where(c => c.MI_Id == data.MI_Id && c.AMCO_ActiveFlag == true).ToArray();


                var Acdemic_preadmission = _db.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();
                data.ASMAY_Id = Acdemic_preadmission;

                DateTime startdate = Convert.ToDateTime(_db.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_PreAdm_F_Date).FirstOrDefault());
                data.prestartdate = startdate;
                // Student Roles
                string studentRole = "OnlinePreadmissionUser";
                var id = _db.applicationRole.Single(d => d.Name == studentRole);
                //

                // Student Role Type
                string studentRoleType = "OnlinePreadmissionUser";
                var id2 = _db.MasterRoleType.Single(d => d.IVRMRT_Role == studentRoleType);
                //


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "totalcountReport_Clg";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@RoleId",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(id.Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@RoleTypeId",
                       SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt64(id2.IVRMRT_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@year",
                SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@miid",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@predate",
         SqlDbType.DateTime)
                    {
                        Value = data.prestartdate
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    var datatype = dataReader.GetFieldType(iFiled);
                                    if (datatype.Name == "DateTime")
                                    {
                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                        dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? " " : dateval  // use null instead of {}
                                    );
                                    }
                                    else
                                    {
                                        dataRow.Add(
                                       dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? " " : dataReader[iFiled] // use null instead of {}
                                   );
                                    }
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.totalcountDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {

                    }

                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<CollegePreadmissionstudnetDto> Getdetails(CollegePreadmissionstudnetDto reg)
        {
            DateTime enddate = DateTime.Now;
            if (reg.ASMAY_Id != 0)
            {
                DateTime startdate = Convert.ToDateTime(_db.AcademicYear.Where(t => t.ASMAY_Id == reg.ASMAY_Id && t.MI_Id == reg.MI_Id).Select(d => d.ASMAY_PreAdm_F_Date).FirstOrDefault());
                reg.prestartdate = startdate;
                var ASMAY_orderid = (from a in _db.AcademicYear
                                     where (a.ASMAY_Id == reg.ASMAY_Id && a.MI_Id == reg.MI_Id)
                                     select new CollegePreadmissionstudnetDto
                                     {
                                         academicorder = a.ASMAY_Order + 1
                                     }
          ).ToList();
                var academicorderid = (from a in _db.AcademicYear
                                       where (a.ASMAY_Order == ASMAY_orderid.FirstOrDefault().academicorder && a.MI_Id == reg.MI_Id)
                                       select new CollegePreadmissionstudnetDto
                                       {
                                           academicyearstratdate = a.ASMAY_PreAdm_F_Date,
                                           acedemicyear = a.ASMAY_Id

                                       }
          ).ToList();


                if (academicorderid.Count > 0)
                {
                    enddate = Convert.ToDateTime(_db.AcademicYear.Where(t => t.ASMAY_Id == academicorderid.FirstOrDefault().acedemicyear && t.MI_Id == reg.MI_Id).Select(d => d.ASMAY_PreAdm_F_Date).FirstOrDefault());
                    reg.presenddate = enddate.AddDays(-1);
                }
                else
                {
                    reg.presenddate = enddate;
                }

            }
            else
            {
                reg.prestartdate = enddate;
                reg.presenddate = enddate;
            }



            // Student Roles
            string studentRole = "OnlinePreadmissionUser";
            var id = _db.applicationRole.Single(d => d.Name == studentRole);
            //

            // Student Role Type
            string studentRoleType = "OnlinePreadmissionUser";
            var id2 = _db.MasterRoleType.Single(d => d.IVRMRT_Role == studentRoleType);
            //

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "AllInOneReport_Clg";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@RoleId",
                    SqlDbType.BigInt)
                {
                    Value = Convert.ToInt32(id.Id)
                });
                cmd.Parameters.Add(new SqlParameter("@RoleTypeId",
                   SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(id2.IVRMRT_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@From_Date",
               SqlDbType.DateTime)
                {
                    Value = reg.From_Date
                });
                cmd.Parameters.Add(new SqlParameter("@To_Date",
               SqlDbType.DateTime)
                {
                    Value = reg.To_Date
                });
                cmd.Parameters.Add(new SqlParameter("@option",
              SqlDbType.BigInt)
                {
                    Value = reg.ReportType
                });
                cmd.Parameters.Add(new SqlParameter("@year",
            SqlDbType.BigInt)
                {
                    Value = reg.ASMAY_Id
                });
                cmd.Parameters.Add(new SqlParameter("@type",
          SqlDbType.VarChar)
                {
                    Value = reg.type
                });
                cmd.Parameters.Add(new SqlParameter("@miid",
         SqlDbType.VarChar)
                {
                    Value = reg.MI_Id
                });
                cmd.Parameters.Add(new SqlParameter("@predate",
         SqlDbType.DateTime)
                {
                    Value = reg.prestartdate
                });
                cmd.Parameters.Add(new SqlParameter("@prenddate",
          SqlDbType.DateTime)
                {
                    Value = reg.presenddate
                });
                cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
          SqlDbType.BigInt)
                {
                    Value = reg.AMCO_Id
                });

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();


                try
                {

                    using (var dataReader = await cmd.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {

                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                            {
                                var datatype = dataReader.GetFieldType(iFiled);
                                if (datatype.Name == "DateTime")
                                {
                                    var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? " " : dateval  // use null instead of {}
                                     );
                                }
                                else
                                {
                                    dataRow.Add(
                                   dataReader.GetName(iFiled),
                                   dataReader.IsDBNull(iFiled) ? " " : dataReader[iFiled] // use null instead of {}
                               );
                                }
                            }
                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    reg.SearchstudentDetails = retObject.ToArray();
                    if (reg.SearchstudentDetails.Length > 0)
                    {
                        reg.count = reg.SearchstudentDetails.Length;
                    }
                    else
                    {
                        reg.count = 0;
                    }
                }
                catch (Exception ex)
                {

                }

            }
            return reg;
        }



        //preadmission status
        public async Task<CommonDTO> getstatusdata(int mi_id)
        {
            CommonDTO ctdo = new CommonDTO();
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = (from a in _db.AcademicYear
                           where (a.MI_Id == mi_id && a.ASMAY_Pre_ActiveFlag == 1 && a.Is_Active == true && a.MI_Id == mi_id)
                           select new MasterAcademic
                           {
                               ASMAY_Id = a.ASMAY_Id,
                               ASMAY_Year = a.ASMAY_Year
                           }
                      ).ToList();
                ctdo.AcademicList = allyear.ToArray();
                //List<School_M_Class> allclass = new List<School_M_Class>();
                //allclass = _db.School_M_Class.Where(t => t.MI_Id == mi_id && t.ASMCL_ActiveFlag == true).ToList();
                //ctdo.classlist = allclass.ToArray();

                ctdo.courselist = _db.MasterCourseDMO.Where(c => c.MI_Id == mi_id && c.AMCO_ActiveFlag == true).ToArray();

                List<AdmissionStatus> status = new List<AdmissionStatus>();
                status = _db.status.Where(t => t.MI_Id == mi_id).ToList();
                ctdo.statuslist = status.ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }

        public CommonDTO getdataonsearchfilter(CommonDTO cdto)
        {
            List<CollegePreadmissionstudnetDto> stulist = new List<CollegePreadmissionstudnetDto>();
            try
            {
                List<CollegePreadmissionstudnetDto> result = new List<CollegePreadmissionstudnetDto>();
                //to get data according to search criteria.
                if (cdto.status_type == "Appsts")
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Get_Student_Status_clg";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Ids", SqlDbType.Int) { Value = cdto.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMCO_Ids", SqlDbType.Int) { Value = cdto.AMCO_Id });
                        cmd.Parameters.Add(new SqlParameter("@PAMST_Ids", SqlDbType.Int) { Value = cdto.PAMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int) { Value = cdto.IVRM_MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@type_", SqlDbType.VarChar) { Value = cdto.status_type });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new CollegePreadmissionstudnetDto
                                    {

                                        //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                        //for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                        //{
                                        //    dataRow.Add(
                                        //        dataReader.GetName(iFiled),
                                        //       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        //    );
                                        //}
                                        //retObject.Add((ExpandoObject)dataRow);



                                        PACA_Id = Convert.ToInt64(dataReader["PACA_Id"]),
                                        AMCO_CourseName = Convert.ToString(dataReader["AMCO_CourseName"]),
                                        // PACA_AdmStatus = Convert.ToInt64(dataReader["PACA_AdmStatus"]),
                                        PACA_Statusremark = (dataReader["PACA_Statusremark"]).ToString(),
                                        PACA_ApplStatus = Convert.IsDBNull((dataReader["PACA_ApplStatus"])).ToString(),
                                        //fee_status = Convert.IsDBNull(dataReader["fee_status"]).ToString(),
                                        //  remark = (dataReader["Remark"]).ToString(),
                                        PACA_FirstName = (dataReader["PACA_FirstName"]).ToString(),
                                        PACA_MiddleName = Convert.IsDBNull((dataReader["PACA_MiddleName"])).ToString(),
                                        PACA_LastName = Convert.IsDBNull(Convert.ToString(dataReader["PACA_LastName"])).ToString(),
                                        AMCO_Id = Convert.ToInt64(dataReader["AMCO_Id"]),
                                        courseName = dataReader["AMCO_CourseName"].ToString(),
                                        //statusName = dataReader["PAMST_Status"].ToString(),
                                        //statusFlag = dataReader["PAMST_StatusFlag"].ToString(),
                                        PACA_Sex = dataReader["PACA_Sex"].ToString(),
                                        PACA_RegistrationNo = dataReader["PACA_RegistrationNo"].ToString(),
                                        PACA_emailId = dataReader["PACA_emailId"].ToString(),
                                        PACA_MobileNo = Convert.ToInt64(dataReader["PACA_MobileNo"]),
                                        PACA_FatherName = Convert.IsDBNull(dataReader["PACA_FatherName"]).ToString() + ' ' + Convert.IsDBNull(dataReader["PACA_FatherSurname"]).ToString(),
                                        PACA_DOB_inwords = dataReader["PACA_DOB_inwords"].ToString(),
                                        PACA_ConCity = Convert.IsDBNull(dataReader["PACA_ConCity"]).ToString(),
                                        PACA_ConStreet = Convert.IsDBNull(dataReader["PACA_ConStreet"]).ToString(),
                                        PACA_ConArea = Convert.IsDBNull(dataReader["PACA_ConArea"]).ToString(),
                                        PACA_ConPincode = Convert.ToInt32(dataReader["PACA_ConPincode"]),
                                        // Repeat_Class_Id = Convert.ToInt64(dataReader["Repeat_Class_Id"])
                                        PACA_FatherPhoto = Convert.IsDBNull(dataReader["PACA_FatherPhoto"]).ToString(),
                                        PACA_MotherPhoto = Convert.IsDBNull(dataReader["PACA_MotherPhoto"]).ToString(),
                                        remarkcount = Convert.ToInt64(Convert.IsDBNull(dataReader["remarkcount"]))



                                    });
                                    cdto.studentlist = result.ToArray();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }



                    //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    //{
                    //    cmd.CommandText = "Get_Student_Status_clg";
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Ids", SqlDbType.Int) { Value = cdto.ASMAY_Id });
                    //    cmd.Parameters.Add(new SqlParameter("@AMCO_Ids", SqlDbType.Int) { Value = cdto.AMCO_Id });
                    //    cmd.Parameters.Add(new SqlParameter("@PAMST_Ids", SqlDbType.Int) { Value = cdto.PAMST_Id });
                    //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int) { Value = cdto.IVRM_MI_Id });
                    //    cmd.Parameters.Add(new SqlParameter("@type_", SqlDbType.VarChar) { Value = cdto.status_type });

                    //    if (cmd.Connection.State != ConnectionState.Open)
                    //        cmd.Connection.Open();

                    //    var retObject = new List<dynamic>();
                    //    try
                    //    {
                    //        using (var dataReader = cmd.ExecuteReader())
                    //        {
                    //            while (dataReader.Read())
                    //            {
                    //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                    //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                    //                {
                    //                    dataRow.Add(
                    //                        dataReader.GetName(iFiled),
                    //                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                    //                    );
                    //                }
                    //                retObject.Add((ExpandoObject)dataRow);
                    //            }
                    //        }
                    //        cdto.studentlist = retObject.ToArray();
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(ex.Message);
                    //    }
                    //}

                    if (cdto.status_all != null)
                    {
                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "GetStudentStatusOverallReport_clg";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Ids", SqlDbType.Int) { Value = cdto.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@AMCO_Ids", SqlDbType.Int) { Value = cdto.AMCO_Id });
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int) { Value = cdto.IVRM_MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@type_", SqlDbType.VarChar) { Value = cdto.status_type });
                            cmd.Parameters.Add(new SqlParameter("@PAMST_Ids", SqlDbType.Int) { Value = cdto.PAMST_Id });
                            cmd.Parameters.Add(new SqlParameter("@all", SqlDbType.VarChar) { Value = cdto.status_all });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        result.Add(new CollegePreadmissionstudnetDto
                                        {
                                            //PACA_Id = Convert.ToInt64(dataReader["PACA_Id"]),
                                            //AMCO_CourseName = Convert.ToString(dataReader["AMCO_CourseName"]),
                                            //// PACA_AdmStatus = Convert.ToInt64(dataReader["PACA_AdmStatus"]),
                                            //PACA_ApplStatus = Convert.ToString(dataReader["PACA_ApplStatus"]),
                                            //fee_status = Convert.ToString(dataReader["fee_status"]),
                                            ////  remark = (dataReader["Remark"]).ToString(),
                                            //PACA_FirstName = (dataReader["PACA_FirstName"]).ToString(),
                                            //PACA_MiddleName = (dataReader["PACA_MiddleName"]).ToString(),
                                            //PACA_LastName = Convert.ToString(dataReader["PACA_LastName"]),
                                            //AMCO_Id = Convert.ToInt64(dataReader["AMCO_Id"]),
                                            //courseName = dataReader["AMCO_CourseName"].ToString(),
                                            ////statusName = dataReader["PAMST_Status"].ToString(),
                                            ////statusFlag = dataReader["PAMST_StatusFlag"].ToString(),
                                            //PACA_Sex = dataReader["PACA_Sex"].ToString(),
                                            //PACA_RegistrationNo = dataReader["PACA_RegistrationNo"].ToString(),
                                            //PACA_emailId = dataReader["PACA_emailId"].ToString(),
                                            //PACA_MobileNo = Convert.ToInt64(dataReader["PACA_MobileNo"]),
                                            //PACA_FatherName = dataReader["PACA_FatherName"].ToString() + ' ' + dataReader["PACA_FatherSurname"].ToString(),
                                            //PACA_DOB_inwords = dataReader["PACA_DOB_inwords"].ToString(),
                                            //PACA_ConCity = dataReader["PACA_ConCity"].ToString(),
                                            //PACA_ConStreet = dataReader["PACA_ConStreet"].ToString(),
                                            //PACA_ConArea = dataReader["PACA_ConArea"].ToString(),
                                            //PACA_ConPincode = Convert.ToInt32(dataReader["PACA_ConPincode"]),
                                            //// Repeat_Class_Id = Convert.ToInt64(dataReader["Repeat_Class_Id"])
                                            //PACA_StudentPhoto = Convert.ToString(dataReader["PACA_StudentPhoto"]),
                                            //PACA_FatherPhoto = Convert.ToString(dataReader["PACA_FatherPhoto"]),
                                            //PACA_MotherPhoto = Convert.ToString(dataReader["PACA_MotherPhoto"]),



                                            PACA_Id = Convert.ToInt64(dataReader["PACA_Id"]),
                                            AMCO_CourseName = Convert.ToString(dataReader["AMCO_CourseName"]),
                                            // PACA_AdmStatus = Convert.ToInt64(dataReader["PACA_AdmStatus"]),
                                            PACA_ApplStatus = Convert.ToString(dataReader["PACA_ApplStatus"]),
                                            fee_status = Convert.IsDBNull(dataReader["fee_status"]).ToString(),
                                            //  remark = (dataReader["Remark"]).ToString(),
                                            PACA_FirstName = (dataReader["PACA_FirstName"]).ToString(),
                                            PACA_MiddleName = Convert.IsDBNull(dataReader["PACA_MiddleName"]).ToString(),
                                            PACA_LastName = Convert.IsDBNull(dataReader["PACA_LastName"]).ToString(),
                                            AMCO_Id = Convert.ToInt64(dataReader["AMCO_Id"]),
                                            courseName = dataReader["AMCO_CourseName"].ToString(),
                                            //statusName = dataReader["PAMST_Status"].ToString(),
                                            //statusFlag = dataReader["PAMST_StatusFlag"].ToString(),
                                            PACA_Sex = dataReader["PACA_Sex"].ToString(),
                                            PACA_RegistrationNo = dataReader["PACA_RegistrationNo"].ToString(),
                                            PACA_emailId = dataReader["PACA_emailId"].ToString(),
                                            PACA_MobileNo = Convert.ToInt64(dataReader["PACA_MobileNo"]),
                                            PACA_FatherName = Convert.IsDBNull(dataReader["PACA_FatherName"]).ToString() + ' ' + Convert.IsDBNull(dataReader["PACA_FatherSurname"]).ToString(),
                                            PACA_DOB_inwords = Convert.IsDBNull(dataReader["PACA_DOB_inwords"]).ToString(),
                                            PACA_ConCity = Convert.IsDBNull(dataReader["PACA_ConCity"]).ToString(),
                                            PACA_ConStreet = Convert.IsDBNull(dataReader["PACA_ConStreet"]).ToString(),
                                            PACA_ConArea = Convert.IsDBNull(dataReader["PACA_ConArea"]).ToString(),
                                            PACA_ConPincode = Convert.ToInt32(dataReader["PACA_ConPincode"]),
                                            // Repeat_Class_Id = Convert.ToInt64(dataReader["Repeat_Class_Id"])
                                            PACA_StudentPhoto = Convert.IsDBNull(dataReader["PACA_StudentPhoto"]).ToString(),
                                            PACA_FatherPhoto = Convert.IsDBNull(dataReader["PACA_FatherPhoto"]).ToString(),
                                            PACA_MotherPhoto = Convert.IsDBNull(dataReader["PACA_MotherPhoto"]).ToString(),


                                        });
                                        cdto.studentlist = result.ToArray();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }
                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "GetStudentStatusOverallCountReport_clg";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Ids", SqlDbType.Int) { Value = cdto.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@AMCO_Ids", SqlDbType.Int) { Value = cdto.AMCO_Id });
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int) { Value = cdto.IVRM_MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@type_", SqlDbType.VarChar) { Value = cdto.status_type });
                            cmd.Parameters.Add(new SqlParameter("@all", SqlDbType.VarChar) { Value = cdto.status_all });
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
                                cdto.studentlist = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }

                    }
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Get_Student_Status_clg";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Ids", SqlDbType.Int) { Value = cdto.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMCO_Ids", SqlDbType.Int) { Value = cdto.AMCO_Id });
                        cmd.Parameters.Add(new SqlParameter("@PAMST_Ids", SqlDbType.Int) { Value = cdto.PAMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int) { Value = cdto.IVRM_MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@type_", SqlDbType.VarChar) { Value = cdto.status_type });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new CollegePreadmissionstudnetDto
                                    {
                                        PACA_Id = Convert.ToInt64(dataReader["PACA_Id"]),

                                        PACA_AdmStatus = Convert.ToInt64(dataReader["PACA_AdmStatus"]),
                                        AMCO_CourseName = Convert.ToString(dataReader["AMCO_CourseName"]),
                                        // PACA_ApplStatus = Convert.ToString(dataReader["PACA_ApplStatus"]),
                                        PACA_Statusremark = (dataReader["PACA_Statusremark"]).ToString(),
                                        fee_status = Convert.ToString(dataReader["fee_status"]),
                                        PACA_FirstName = (dataReader["PACA_FirstName"]).ToString(),
                                        PACA_MiddleName = (dataReader["PACA_MiddleName"]).ToString(),
                                        PACA_LastName = Convert.ToString(dataReader["PACA_LastName"]),
                                        AMCO_Id = Convert.ToInt64(dataReader["AMCO_Id"]),
                                        courseName = dataReader["AMCO_CourseName"].ToString(),
                                        statusName = dataReader["Statusadm"].ToString(),
                                        //statusFlag = dataReader["PAMST_StatusFlag"].ToString(),
                                        PACA_Sex = dataReader["PACA_Sex"].ToString(),
                                        PACA_RegistrationNo = dataReader["PACA_RegistrationNo"].ToString(),
                                        PACA_emailId = dataReader["PACA_emailId"].ToString(),
                                        PACA_MobileNo = Convert.ToInt64(dataReader["PACA_MobileNo"]),
                                        PACA_FatherName = dataReader["PACA_FatherName"].ToString() + ' ' + dataReader["PACA_FatherSurname"].ToString(),
                                        PACA_DOB_inwords = dataReader["PACA_DOB_inwords"].ToString(),
                                        PACA_ConCity = dataReader["PACA_ConCity"].ToString(),
                                        PACA_ConStreet = dataReader["PACA_ConStreet"].ToString(),
                                        PACA_ConArea = dataReader["PACA_ConArea"].ToString(),
                                        PACA_ConPincode = Convert.ToInt32(dataReader["PACA_ConPincode"]),
                                        // Repeat_Class_Id = Convert.ToInt64(dataReader["Repeat_Class_Id"])
                                        PACA_StudentPhoto = Convert.ToString(dataReader["PACA_StudentPhoto"]),
                                        PACA_FatherPhoto = Convert.ToString(dataReader["PACA_FatherPhoto"]),
                                        PACA_MotherPhoto = Convert.ToString(dataReader["PACA_MotherPhoto"]),
                                        remarkcount = Convert.ToInt64(dataReader["remarkcount"])
                                    });

                                    cdto.studentlist = result.ToArray();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                    if (cdto.status_all != null)
                    {
                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "GetStudentStatusOverallReport_clg";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Ids", SqlDbType.Int) { Value = cdto.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@AMCO_Ids", SqlDbType.Int) { Value = cdto.AMCO_Id });
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int) { Value = cdto.IVRM_MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@type_", SqlDbType.VarChar) { Value = cdto.status_type });
                            cmd.Parameters.Add(new SqlParameter("@PAMST_Ids", SqlDbType.Int) { Value = cdto.PAMST_Id });
                            cmd.Parameters.Add(new SqlParameter("@all", SqlDbType.VarChar) { Value = cdto.status_all });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        result.Add(new CollegePreadmissionstudnetDto
                                        {
                                            PACA_Id = Convert.ToInt64(dataReader["PACA_Id"]),
                                            AMCO_CourseName = Convert.ToString(dataReader["AMCO_CourseName"]),
                                            // PACA_AdmStatus = Convert.ToInt64(dataReader["PACA_AdmStatus"]),
                                            //PACA_ApplStatus = Convert.ToString(dataReader["PACA_ApplStatus"]),
                                            fee_status = Convert.ToString(dataReader["fee_status"]),
                                            //  remark = (dataReader["Remark"]).ToString(),
                                            PACA_FirstName = (dataReader["PACA_FirstName"]).ToString(),
                                            PACA_MiddleName = (dataReader["PACA_MiddleName"]).ToString(),
                                            PACA_LastName = Convert.ToString(dataReader["PACA_LastName"]),
                                            AMCO_Id = Convert.ToInt64(dataReader["AMCO_Id"]),
                                            courseName = dataReader["AMCO_CourseName"].ToString(),
                                            //statusName = dataReader["PAMST_Status"].ToString(),
                                            //statusFlag = dataReader["PAMST_StatusFlag"].ToString(),
                                            PACA_Sex = dataReader["PACA_Sex"].ToString(),
                                            PACA_RegistrationNo = dataReader["PACA_RegistrationNo"].ToString(),
                                            PACA_emailId = dataReader["PACA_emailId"].ToString(),
                                            PACA_MobileNo = Convert.ToInt64(dataReader["PACA_MobileNo"]),
                                            PACA_FatherName = dataReader["PACA_FatherName"].ToString() + ' ' + dataReader["PACA_FatherSurname"].ToString(),
                                            PACA_DOB_inwords = dataReader["PACA_DOB_inwords"].ToString(),
                                            PACA_ConCity = dataReader["PACA_ConCity"].ToString(),
                                            PACA_ConStreet = dataReader["PACA_ConStreet"].ToString(),
                                            PACA_ConArea = dataReader["PACA_ConArea"].ToString(),
                                            PACA_ConPincode = Convert.ToInt32(dataReader["PACA_ConPincode"]),
                                            // Repeat_Class_Id = Convert.ToInt64(dataReader["Repeat_Class_Id"])
                                            PACA_StudentPhoto = Convert.ToString(dataReader["PACA_StudentPhoto"]),
                                            PACA_FatherPhoto = Convert.ToString(dataReader["PACA_FatherPhoto"]),
                                            PACA_MotherPhoto = Convert.ToString(dataReader["PACA_MotherPhoto"])
                                        });
                                        cdto.studentlist = result.ToArray();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }

                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "GetStudentStatusOverallCountReport_clg";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Ids", SqlDbType.Int) { Value = cdto.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@AMCO_Ids", SqlDbType.Int) { Value = cdto.AMCO_Id });
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int) { Value = cdto.IVRM_MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@type_", SqlDbType.VarChar) { Value = cdto.status_type });
                            cmd.Parameters.Add(new SqlParameter("@all", SqlDbType.VarChar) { Value = cdto.status_all });
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
                                cdto.studentcountreport = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }
                    }
                }
                //view application 
                List<Adm_Master_College_StudentDMO> allRegStudentmain_adm = new List<Adm_Master_College_StudentDMO>();
                List<PA_College_Application> allRegStudentmain_appl = new List<PA_College_Application>();
                //if(cdto.)
                //    allRegStudentmain = _context.Adm_Master_College_StudentDMO.Where(d => d.MI_Id.Equals(cdto.mi_id) && d.ASMAY_Id == cdto.ASMAY_Id).ToList();
                List<long> temparr = new List<long>();
                List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                mstConfig = _precontext.masterConfig.Where(d => d.MI_Id.Equals(cdto.IVRM_MI_Id) && d.ASMAY_Id.Equals(cdto.ASMAY_Id)).ToList();

                string paca_Ids = "0";
                allRegStudentmain_appl = _precontext.PA_College_Application.Where(d => d.MI_Id.Equals(cdto.IVRM_MI_Id) && d.ASMAY_Id == cdto.ASMAY_Id).ToList();
                for (int i = 0; i < allRegStudentmain_appl.Count; i++)
                {
                    temparr.Add(allRegStudentmain_appl[i].PACA_Id);
                    paca_Ids = paca_Ids + "," + allRegStudentmain_appl[i].PACA_Id;
                }
                //download//
                if (cdto.status_type == "Appsts")
                {
                    var appl_status = cdto.PAMST_Id.ToString();
                    //cdto.ddoc = (from a in _precontext.PA_College_Student_Documents
                    //             from b in _precontext.MasterDocumentDMO
                    //             from c in _precontext.PA_College_Application
                    //             where (a.AMSMD_Id == b.AMSMD_Id && a.PACA_Id == c.PACA_Id && c.ASMAY_Id == cdto.ASMAY_Id && c.MI_Id == cdto.IVRM_MI_Id && temparr.Contains(c.PACA_Id) && c.PACA_ApplStatus == appl_status)
                    //             select new CollegePreadmissionstudnetDto
                    //             {
                    //                 PACA_Id = c.PACA_Id,
                    //                 PACSTD_Id = a.PACSTD_Id,
                    //                 ACSTD_Doc_Path = a.ACSTD_Doc_Path,
                    //                 AMSMD_DocumentName = b.AMSMD_DocumentName,
                    //                 AMSMD_Id = a.AMSMD_Id
                    //             }
                    //  ).ToArray();

                    using (var cmd = _precontext.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "College_preadmission_documentlist";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                        {
                            Value = cdto.IVRM_MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@PACA_Id",
                     SqlDbType.VarChar)
                        {
                            Value = paca_Ids
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                         SqlDbType.VarChar)
                        {
                            Value = cdto.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@status",
                    SqlDbType.VarChar)
                        {
                            Value = appl_status
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
                            cdto.ddoc = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    var appl_status = cdto.PAMST_Id.ToString();
                    //cdto.ddoc = (from a in _precontext.PA_College_Student_Documents
                    //             from b in _precontext.MasterDocumentDMO
                    //             from c in _precontext.PA_College_Application
                    //             where (a.AMSMD_Id == b.AMSMD_Id && a.PACA_Id == c.PACA_Id && c.ASMAY_Id == cdto.ASMAY_Id && c.MI_Id == cdto.IVRM_MI_Id && temparr.Contains(c.PACA_Id) && c.PACA_AdmStatus == cdto.PAMST_Id)
                    //             select new CollegePreadmissionstudnetDto
                    //             {
                    //                 PACA_Id = c.PACA_Id,
                    //                 PACSTD_Id = a.PACSTD_Id,
                    //                 ACSTD_Doc_Path = a.ACSTD_Doc_Path,
                    //                 AMSMD_DocumentName = b.AMSMD_DocumentName,
                    //                 AMSMD_Id = a.AMSMD_Id
                    //             }
                    //).ToArray();

                    using (var cmd = _precontext.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "College_preadmission_documentlist";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                        {
                            Value = cdto.IVRM_MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@PACA_Id",
                     SqlDbType.VarChar)
                        {
                            Value = paca_Ids
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                         SqlDbType.VarChar)
                        {
                            Value = cdto.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@status",
                    SqlDbType.VarChar)
                        {
                            Value = appl_status
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
                            cdto.ddoc = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                //else 
                //{

                //    allRegStudentmain_adm = _context.Adm_Master_College_StudentDMO.Where(d => d.MI_Id.Equals(cdto.IVRM_MI_Id) && d.ASMAY_Id == cdto.ASMAY_Id).ToList();
                //    for (int i = 0; i < allRegStudentmain_adm.Count; i++)
                //    {
                //        temparr.Add(allRegStudentmain_adm[i].AMCST_Id);
                //    }

                //    //download//
                //    cdto.ddoc = (from a in _precontext.PA_College_Student_Documents
                //                 from b in _precontext.MasterDocumentDMO
                //                 from c in _precontext.PA_College_Application
                //                 where (a.AMSMD_Id == b.AMSMD_Id && a.PACA_Id == c.PACA_Id && c.ASMAY_Id == cdto.ASMAY_Id && c.MI_Id == cdto.IVRM_MI_Id && temparr.Contains(c.PACA_Id))
                //                 select new CollegePreadmissionstudnetDto
                //                 {
                //                     PACA_Id = c.PACA_Id,
                //                     PACSTD_Id = a.PACSTD_Id,
                //                     ACSTD_Doc_Path = a.ACSTD_Doc_Path,
                //                     AMSMD_DocumentName = b.AMSMD_DocumentName,
                //                     AMSMD_Id = a.AMSMD_Id
                //                 }
                //      ).ToArray();
                //    //
                //}



                if (mstConfig.FirstOrDefault().ISPAC_ApplFeeFlag == 1)
                {
                    cdto.prospectusPaymentlist = _precontext.Fee_Y_Payment_PA_Application.Where(t => t.FYPPA_Type == "R" && temparr.Contains(t.PACA_Id)).ToArray();
                }
                else
                {
                    cdto.prospectusPaymentlist = _precontext.Fee_Y_Payment_PA_Application.Where(t => t.FYPPA_Type == "R" && temparr.Contains(t.PACA_Id)).ToArray();
                }
                //view application 




            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return cdto;
        }


        public CommonDTO saveData(CommonDTO cdto)
        {
            try
            {
                Dictionary<string, string> smsemail = new Dictionary<string, string>();
                smsemail.Add("header", cdto.header);
                smsemail.Add("Subject", cdto.Subject);
                smsemail.Add("Message", cdto.Message);
                smsemail.Add("Footer", cdto.Footer);
                cdto.smsemailarry = smsemail.ToArray();
                long trnsno = 0;
                long senderid = cdto.userId;

                if (cdto.data_arrayc.Count() > 0)
                {
                    if (cdto._type == "Appsts")
                    {
                        for (int i = 0; i < cdto.data_arrayc.Count(); i++)
                        {
                            try
                            {

                                var changedStudentData = _precontext.PA_College_Application.Single(d => d.PACA_Id == cdto.data_arrayc[i].PACA_Id);
                                if (cdto.data_arrayc[i].PACA_Statusremark != null)
                                {
                                    if (cdto.data_arrayc[i].PACA_Statusremark.ToString() != "")
                                    {
                                        changedStudentData.PACA_Statusremark = cdto.data_arrayc[i].PACA_Statusremark.ToString();
                                    }
                                }

                                long adm_status = 0;
                                if (cdto.data_arrayc[i].PAMST_Id.ToString() == "787928")
                                {
                                    adm_status = _precontext.AdmissionStatus.Where(s => s.MI_Id == cdto.mi_id && s.PAMST_StatusFlag == "INP").Select(m => m.PAMST_Id).FirstOrDefault();
                                    changedStudentData.PACA_AdmStatus = adm_status;
                                   // changedStudentData.PACA_ApplStatus = cdto.data_arrayc[i].PAMST_Id.ToString();
                                }
                            

                                changedStudentData.PACA_ApplStatus = cdto.data_arrayc[i].PAMST_Id.ToString();
                               
                                //_precontext.PA_College_Application.Update(changedStudentData);
                                if (changedStudentData.PACA_Statusremark != null && changedStudentData.PACA_Statusremark != "")
                                {
                                    stud_reamrk_history(cdto.data_arrayc[i].PACA_Id, changedStudentData.PACA_Statusremark);
                                }
                                _precontext.PA_College_Application.Update(changedStudentData);
                                int cnt = _precontext.SaveChanges();

                                if (cnt == 1)
                                {


                                    if (cdto.defaultsmsemail == false)
                                    {
                                        if (cdto.emailcheck == true)
                                        {
                                            try
                                            {
                                                Email Email = new Email(_db);
                                                Email.sendmailschedule(changedStudentData.MI_Id, "DEFAULT", smsemail, changedStudentData.PACA_emailId, "Preadmission App Status");
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }

                                        if (cdto.smscheck == true)
                                        {

                                            try
                                            {

                                                Dictionary<string, string> val = new Dictionary<string, string>();

                                                var institutionName = _db.Institution.Where(g => g.MI_Id == changedStudentData.MI_Id).ToList();


                                                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                                                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(changedStudentData.MI_Id)).ToList();

                                                List<Institution> insdeta = new List<Institution>();
                                                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(changedStudentData.MI_Id)).ToList();

                                                var template = _db.smsEmailSetting.Where(e => e.MI_Id == changedStudentData.MI_Id && e.ISES_Template_Name == "DEFAULT" && e.ISES_SMSActiveFlag == true).ToList();

                                                if (alldetails.Count > 0)
                                                {
                                                    string url = alldetails[0].IVRMSD_URL.ToString();

                                                    string PHNO = changedStudentData.PACA_MobileNo.ToString();

                                                    url = url.Replace("PHNO", PHNO);

                                                    url = url.Replace("MESSAGE", cdto.smscontent);
                                                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                                                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);


                                                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                                                    System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse;
                                                    Stream stream = response.GetResponseStream();

                                                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                                                    string responseparameters = readStream.ReadToEnd();

                                                    //List<SMSParameters> list = JsonConvert.DeserializeObject<List<SMSParameters>>(responseparameters);

                                                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                                    {

                                                        // var modulename = "InstitutionCreation";
                                                        cmd.CommandText = "IVRM_SMS_Outgoing";
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                                                            SqlDbType.NVarChar)
                                                        {
                                                            Value = PHNO
                                                        });
                                                        cmd.Parameters.Add(new SqlParameter("@Message",
                                                           SqlDbType.NVarChar)
                                                        {
                                                            Value = cdto.smscontent
                                                        });
                                                        cmd.Parameters.Add(new SqlParameter("@module",
                                                        SqlDbType.VarChar)
                                                        {
                                                            Value = "Preadmission Status"
                                                        });
                                                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                                       SqlDbType.BigInt)
                                                        {
                                                            Value = changedStudentData.MI_Id
                                                        });

                                                        cmd.Parameters.Add(new SqlParameter("@status",
                                                   SqlDbType.VarChar)
                                                        {
                                                            Value = responseparameters
                                                        });

                                                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                                                SqlDbType.VarChar)
                                                        {
                                                            Value = 0
                                                        });

                                                        if (cmd.Connection.State != ConnectionState.Open)
                                                            cmd.Connection.Open();

                                                        try
                                                        {
                                                            using (var dataReader = cmd.ExecuteReader())
                                                            {
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            //return ex.Message;
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                                //return ex.Message;
                                            }
                                            //}
                                            //catch (Exception ex)
                                            //{

                                            //}
                                        }
                                    }
                                    else
                                    {
                                        Email Email = new Email(_db);
                                        string m = Email.sendmail(changedStudentData.MI_Id, changedStudentData.PACA_emailId, "STATUS_CONFIRM", changedStudentData.PACA_Id);

                                        try
                                        {

                                            Dictionary<string, string> val = new Dictionary<string, string>();

                                            var template = _db.smsEmailSetting.Where(e => e.MI_Id == changedStudentData.MI_Id && e.ISES_Template_Name == "STATUS_CONFIRM" && e.ISES_SMSActiveFlag == true).ToList();


                                            var institutionName = _db.Institution.Where(j => j.MI_Id == changedStudentData.MI_Id).ToList();

                                            var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(j => j.MI_Id == changedStudentData.MI_Id && j.ISES_Id == template[0].ISES_Id && j.Flag == "S").Select(d => d.ISMP_ID).ToList();

                                            var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(j => Paramaeters.Contains(j.ISMP_ID)).ToList();

                                            string sms = template.FirstOrDefault().ISES_SMSMessage;

                                            string result = sms;

                                            List<Match> variables = new List<Match>();

                                            foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                                            {
                                                variables.Add(match);
                                            }
                                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                            {


                                                cmd.CommandText = "SMSMAILPARAMETER";
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add(new SqlParameter("@UserID",
                                                    SqlDbType.BigInt)
                                                {
                                                    Value = changedStudentData.PACA_Id
                                                });
                                                cmd.Parameters.Add(new SqlParameter("@template",
                                                   SqlDbType.VarChar)
                                                {
                                                    Value = "STATUS_CONFIRM"
                                                });


                                                if (cmd.Connection.State != ConnectionState.Open)
                                                    cmd.Connection.Open();

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
                                                                var datatype = dataReader.GetFieldType(iFiled);
                                                                if (datatype.Name == "DateTime")
                                                                {
                                                                    var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                                                    val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                                                }
                                                                else
                                                                {
                                                                    val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                                                }
                                                            }
                                                        }

                                                    }
                                                }
                                                catch (Exception ex)
                                                {

                                                }

                                            }

                                            for (int j = 0; j < ParamaetersName.Count; j++)
                                            {
                                                for (int p = 0; p < val.Count; p++)
                                                {
                                                    if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                                                    {
                                                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                                        sms = result;
                                                    }
                                                }
                                            }

                                            sms = result;



                                            List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                                            alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(changedStudentData.MI_Id)).ToList();

                                            List<Institution> insdeta = new List<Institution>();
                                            insdeta = _db.Institution.Where(t => t.MI_Id.Equals(changedStudentData.MI_Id)).ToList();

                                            if (alldetails.Count > 0)
                                            {
                                                string url = alldetails[0].IVRMSD_URL.ToString();

                                                string PHNO = changedStudentData.PACA_MobileNo.ToString();

                                                url = url.Replace("PHNO", PHNO);

                                                url = url.Replace("MESSAGE", sms);
                                                url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());
                                                url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);

                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();

                                                StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                                                string responseparameters = readStream.ReadToEnd();
                                                var myContent = JsonConvert.SerializeObject(responseparameters);

                                                dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                                                string messageid = responsedata;


                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);

                                        }
                                    }



                                }
                                else
                                {
                                    cdto.count = 0;
                                }
                                //  save student status change to history table
                                //StudentStatusHistory ssh = new StudentStatusHistory();
                                //ssh.PASR_Id = changedStudentData.PACA_Id;
                                //ssh.PASSH_Status = Convert.ToString(changedStudentData.PACA_ApplStatus);
                                //ssh.PASSH_Date = DateTime.Now;


                                //added by 02/02/2017
                                //ssh.CreatedDate = DateTime.Now;
                                //ssh.UpdatedDate = DateTime.Now;
                                //_db.studentstatushistory.Add(ssh);
                                //_db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < cdto.data_arrayc.Count(); i++)
                        {
                            try
                            {
                                string _status_id = cdto.data_arrayc[i].PAMST_Id.ToString();
                                if (cdto.data_arrayc[i].PACA_Statusremark != null)
                                {
                                    string _remarks = cdto.data_arrayc[i].PACA_Statusremark.ToString();
                                }
                                var changedStudentData = _precontext.PA_College_Application.Single(d => d.PACA_Id == cdto.data_arrayc[i].PACA_Id);
                                if (cdto.data_arrayc[i].PACA_Statusremark != null)
                                {
                                    if (cdto.data_arrayc[i].PACA_Statusremark.ToString() != "")
                                    {
                                        changedStudentData.PACA_Statusremark = cdto.data_arrayc[i].PACA_Statusremark.ToString();
                                    }
                                }
                                changedStudentData.PACA_AdmStatus = cdto.data_arrayc[i].PAMST_Id;
                                //_precontext.Update(changedStudentData);
                                if (changedStudentData.PACA_Statusremark != null && changedStudentData.PACA_Statusremark != "")
                                {
                                    stud_reamrk_history(cdto.data_arrayc[i].PACA_Id, changedStudentData.PACA_Statusremark);
                                }
                                _precontext.PA_College_Application.Update(changedStudentData);

                                int cnt = _precontext.SaveChanges();
                                if (cnt == 1)
                                {
                                    cdto.count = 1;
                                    if (cdto.defaultsmsemail == false)
                                    {
                                        if (cdto.emailcheck == true)
                                        {
                                            Email Email = new Email(_db);
                                            Email.sendmailschedule(changedStudentData.MI_Id, "DEFAULT", smsemail, changedStudentData.PACA_emailId, "Preadmission Admission Status");
                                        }
                                        if (cdto.smscheck == true)
                                        {
                                            try
                                            {
                                                Dictionary<string, string> val = new Dictionary<string, string>();
                                                var institutionName = _db.Institution.Where(g => g.MI_Id == changedStudentData.MI_Id).ToList();
                                                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                                                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(changedStudentData.MI_Id)).ToList();

                                                List<Institution> insdeta = new List<Institution>();
                                                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(changedStudentData.MI_Id)).ToList();

                                                var template = _db.smsEmailSetting.Where(e => e.MI_Id == changedStudentData.MI_Id && e.ISES_Template_Name == "DEFAULT" && e.ISES_SMSActiveFlag == true).ToList();
                                                if (alldetails.Count > 0)
                                                {
                                                    string url = alldetails[0].IVRMSD_URL.ToString();
                                                    string PHNO = changedStudentData.PACA_MobileNo.ToString();
                                                    url = url.Replace("PHNO", PHNO);
                                                    url = url.Replace("MESSAGE", cdto.smscontent);
                                                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());
                                                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);

                                                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                                                    System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse;
                                                    Stream stream = response.GetResponseStream();
                                                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                                                    string responseparameters = readStream.ReadToEnd();
                                                    //List<SMSParameters> list = JsonConvert.DeserializeObject<List<SMSParameters>>(responseparameters);
                                                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                                    {
                                                        // var modulename = "InstitutionCreation";
                                                        cmd.CommandText = "IVRM_SMS_Outgoing";
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                                                            SqlDbType.NVarChar)
                                                        {
                                                            Value = PHNO
                                                        });
                                                        cmd.Parameters.Add(new SqlParameter("@Message",
                                                           SqlDbType.NVarChar)
                                                        {
                                                            Value = cdto.smscontent
                                                        });
                                                        cmd.Parameters.Add(new SqlParameter("@module",
                                                        SqlDbType.VarChar)
                                                        {
                                                            Value = "Preadmission Status"
                                                        });
                                                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                                       SqlDbType.BigInt)
                                                        {
                                                            Value = changedStudentData.MI_Id
                                                        });

                                                        cmd.Parameters.Add(new SqlParameter("@status",
                                                   SqlDbType.VarChar)
                                                        {
                                                            Value = responseparameters
                                                        });

                                                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                                                SqlDbType.VarChar)
                                                        {
                                                            Value = 0
                                                        });

                                                        if (cmd.Connection.State != ConnectionState.Open)
                                                            cmd.Connection.Open();
                                                        try
                                                        {
                                                            using (var dataReader = cmd.ExecuteReader())
                                                            {
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            //return ex.Message;
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                                //return ex.Message;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var statusflag = _db.status.Single(d => d.PAMST_Id == cdto.data_array[i].PAMST_Id).PAMST_StatusFlag;
                                        if (statusflag == "CNF")
                                        {
                                            Email Email = new Email(_db);
                                            string m = Email.sendmail(changedStudentData.MI_Id, changedStudentData.PACA_emailId, "STATUS_CONFIRM", changedStudentData.PACA_Id);
                                            try
                                            {
                                                Dictionary<string, string> val = new Dictionary<string, string>();
                                                var template = _db.smsEmailSetting.Where(e => e.MI_Id == changedStudentData.MI_Id && e.ISES_Template_Name == "STATUS_CONFIRM" && e.ISES_SMSActiveFlag == true).ToList();
                                                var institutionName = _db.Institution.Where(j => j.MI_Id == changedStudentData.MI_Id).ToList();
                                                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(j => j.MI_Id == changedStudentData.MI_Id && j.ISES_Id == template[0].ISES_Id && j.Flag == "S").Select(d => d.ISMP_ID).ToList();
                                                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(j => Paramaeters.Contains(j.ISMP_ID)).ToList();
                                                string sms = template.FirstOrDefault().ISES_SMSMessage;
                                                string result = sms;
                                                List<Match> variables = new List<Match>();
                                                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                                                {
                                                    variables.Add(match);
                                                }
                                                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                                {
                                                    cmd.CommandText = "SMSMAILPARAMETER";
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add(new SqlParameter("@UserID",
                                                        SqlDbType.BigInt)
                                                    {
                                                        Value = changedStudentData.PACA_Id
                                                    });
                                                    cmd.Parameters.Add(new SqlParameter("@template",
                                                       SqlDbType.VarChar)
                                                    {
                                                        Value = "STATUS_CONFIRM"
                                                    });
                                                    if (cmd.Connection.State != ConnectionState.Open)
                                                        cmd.Connection.Open();
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
                                                                    var datatype = dataReader.GetFieldType(iFiled);
                                                                    if (datatype.Name == "DateTime")
                                                                    {
                                                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                                                    }
                                                                    else
                                                                    {
                                                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }
                                                for (int j = 0; j < ParamaetersName.Count; j++)
                                                {
                                                    for (int p = 0; p < val.Count; p++)
                                                    {
                                                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                                                        {
                                                            result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                                            sms = result;
                                                        }
                                                    }
                                                }
                                                sms = result;
                                                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                                                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(changedStudentData.MI_Id)).ToList();

                                                List<Institution> insdeta = new List<Institution>();
                                                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(changedStudentData.MI_Id)).ToList();
                                                if (alldetails.Count > 0)
                                                {
                                                    string url = alldetails[0].IVRMSD_URL.ToString();
                                                    string PHNO = changedStudentData.PACA_MobileNo.ToString();
                                                    url = url.Replace("PHNO", PHNO);
                                                    url = url.Replace("MESSAGE", sms);
                                                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());
                                                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);
                                                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                                                    System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse;
                                                    Stream stream = response.GetResponseStream();
                                                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                                                    string responseparameters = readStream.ReadToEnd();
                                                    var myContent = JsonConvert.SerializeObject(responseparameters);
                                                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                                                    string messageid = responsedata;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    cdto.count = 0;
                                }
                                //  save student status change to history table
                                //StudentStatusHistory ssh = new StudentStatusHistory();
                                //ssh.PASR_Id = changedStudentData.PACA_Id;
                                //ssh.PASSH_Status = Convert.ToString(changedStudentData.PACA_AdmStatus);
                                //ssh.PASSH_Date = DateTime.Now;
                                ////added by 02/02/2017
                                //ssh.CreatedDate = DateTime.Now;
                                //ssh.UpdatedDate = DateTime.Now;
                                //_db.studentstatushistory.Add(ssh);
                                //_db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return cdto;
        }


        public CollegePreadmissionstudnetDto Clgapplicationstudocs(CollegePreadmissionstudnetDto data)
        {
            try
            {
                //miid.ddoc = (from a in _precontext.PA_College_Student_Documents
                //             from b in _precontext.MasterDocumentDMO
                //             from c in _precontext.PA_College_Application
                //             where (a.AMSMD_Id == b.AMSMD_Id && a.PACA_Id == c.PACA_Id && c.PACA_Id == miid.PACA_Id && a.PACA_Id == miid.PACA_Id )
                //             select new CollegePreadmissionstudnetDto
                //             {

                //                 PACA_Id = c.PACA_Id,
                //                 PACSTD_Id = a.PACSTD_Id,
                //                 ACSTD_Doc_Path = a.ACSTD_Doc_Path,
                //                 AMSMD_DocumentName = b.AMSMD_DocumentName,
                //                 AMSMD_Id = a.AMSMD_Id,
                //                 PACA_StudentPhoto = c.PACA_StudentPhoto,
                //                 PACA_FirstName = c.PACA_FirstName,
                //                 PACA_MiddleName = c.PACA_MiddleName,
                //                 PACA_LastName = c.PACA_LastName
                //             }
                //      ).ToArray();

                try
                {
                    using (var cmd = _precontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "College_Student_DocumentsPre";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = data.status_type });
                        cmd.Parameters.Add(new SqlParameter("@PACA_Id", SqlDbType.VarChar) { Value = data.PACA_Id });


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
                            data.doclist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return data;

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return data;
        }


        public CollegePreadmissionstudnetDto stud_reamrk_history(long PACA_Id, string PACA_Statusremark)
        {
            CollegePreadmissionstudnetDto dto = new CollegePreadmissionstudnetDto();
            try
            {
                if (PACA_Statusremark != null && PACA_Statusremark != "")
                {

                    var remarkNoresult = _precontext.PA_Student_Status_History_College.Where(t => t.PACA_Id == PACA_Id).ToList();
                    if (remarkNoresult.Count > 0)
                    {
                        //var update = _precontext.PA_Student_Status_History_College.Where(t => t.PACA_Id == PACA_Id).FirstOrDefault();
                        //// var remarkNoresultt = _precontext.PA_Student_Status_History_College.Single(t => t.PACA_Id == PACA_Id);
                        //update.PACA_Id = PACA_Id;
                        //update.PASSHC_Status = PACA_Statusremark;
                        //update.PASSHC_Date = DateTime.Now;
                        //    _precontext.Update(update);
                        //}
                        //else
                        //{
                        PA_Student_Status_History_College remarks = new PA_Student_Status_History_College();
                        remarks.PACA_Id = PACA_Id;
                        remarks.PASSHC_Status = PACA_Statusremark;
                        remarks.PASSHC_Date = DateTime.Now;
                        _precontext.Add(remarks);
                    }
                }
                _precontext.SaveChanges();
            
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _precontext.Database.RollbackTransaction();
            }
            return dto;
        }

public CollegePreadmissionstudnetDto Clgapplicationsturemarks(CollegePreadmissionstudnetDto data)
{
    try
    {


        try
        {
            using (var cmd = _precontext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "College_Student_RemarksHistoryPre";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = data.status_type });
                cmd.Parameters.Add(new SqlParameter("@PACA_Id", SqlDbType.VarChar) { Value = data.PACA_Id });


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
                    data.remarkslist = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return data;

    }
    catch (Exception ex)
    {
        Console.Write(ex.Message);
    }
    return data;
}

    }
}
