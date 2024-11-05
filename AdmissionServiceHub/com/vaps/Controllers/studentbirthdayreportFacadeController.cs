using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
//using MailKit.Net.Smtp;
//using MimeKit;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Net;

using PreadmissionDTOs.com.vaps.admission;



namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class studentbirthdayreportFacadeController
    {

        private readonly DomainModelMsSqlServerContext _db;
        private readonly ApplicationDBContext _ApplicationDBContext;


        public studentbirthdayreportFacadeController(DomainModelMsSqlServerContext db, ApplicationDBContext ApplicationDBContext)
        {

            _db = db;
            _ApplicationDBContext = ApplicationDBContext;
        }



        //[HttpPost]
        // [Route("getdetails")]
        // //[Route("getenquirycontroller")]
        // public studentbirthdayreportDTO getdetails([FromBody] studentbirthdayreportDTO data)
        // {
        //     // id = 12;
        //     return _dd.getdetails(data);

        // }
        [Route("getYear/{id:int}")]
        public studentbirthdayreportDTO getYear(int id)
        {
            studentbirthdayreportDTO dto = new studentbirthdayreportDTO();
            dto.accyear = _db.AcademicYear.Where(d => d.MI_Id == id && d.Is_Active == true).OrderByDescending(d => d.ASMAY_Order).ToArray();
            return dto;

        }


        [HttpPost]
        [Route("getdetails")]
        //[Route("getenquirycontroller")]
        public async Task<studentbirthdayreportDTO> getdetails([FromBody] studentbirthdayreportDTO data)
        {
            // id = 12;
            //return _dd.getdetails(data);

            DateTime fromdatecon = DateTime.Now;
            string confromdate = "";
            fromdatecon = Convert.ToDateTime(data.Fromdate.Value.Date.ToString("yyyy-MM-dd"));
            confromdate = fromdatecon.ToString("yyyy-MM-dd");

            DateTime todatecon = DateTime.Now;
            string contodate = "";
            todatecon = Convert.ToDateTime(data.Todate.Value.Date.ToString("yyyy-MM-dd"));
            contodate = todatecon.ToString("yyyy-MM-dd");

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Adm_Student_Birthday_Report_New";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@month",
                    SqlDbType.VarChar)
                {
                    Value = Convert.ToString(data.months)
                });

                cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                {


                    Value = confromdate
                }

                );

                cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                {


                    Value = contodate
                }

               );
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                {


                    Value = Convert.ToInt64(data.MI_ID)
                }

             );

                cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.Text)
                {


                    Value = Convert.ToString(data.flag)
                }

               );


                cmd.Parameters.Add(new SqlParameter("@all1", SqlDbType.Text)
                {


                    Value = Convert.ToString(data.all1)
                }

               );




                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();
                //var data = cmd.ExecuteNonQuery();

                try
                {
                    // var data = cmd.ExecuteNonQuery();

                    using (var dataReader = await cmd.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                            {
                                dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? "N/A" : dataReader[iFiled] // use null instead of {}
                                );
                            }

                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    data.studentDetails = retObject.ToArray();
                    if (data.studentDetails.Length > 0)
                    {
                        data.count = data.studentDetails.Length;
                    }
                    else
                    {
                        data.count = 0;
                    }
                    data.schooldetails = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_ID).ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return data;


        }



        [Route("radiobtndata")]
        //[Route("getenquirycontroller")]
        public async Task<studentbirthdayreportDTO> radiobtndata([FromBody] studentbirthdayreportDTO data)
        {
            // id = 12;
            //return _dd.getdetails(data);

            DateTime fromdatecon = DateTime.Now;
            string confromdate = "";
            fromdatecon = Convert.ToDateTime(data.Fromdate.Value.Date.ToString("yyyy-MM-dd"));
            confromdate = fromdatecon.ToString("yyyy-MM-dd");

            DateTime todatecon = DateTime.Now;
            string contodate = "";
            todatecon = Convert.ToDateTime(data.Todate.Value.Date.ToString("yyyy-MM-dd"));
            contodate = todatecon.ToString("yyyy-MM-dd");

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Adm_Student_Birthday_Report_New";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@month",
                    SqlDbType.VarChar)
                {
                    Value = Convert.ToString(data.months)
                });

                cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                {
                    Value = confromdate
                });

                cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                {
                    Value = contodate
                });
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(data.MI_ID)
                });

                cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.Text)
                {
                    Value = Convert.ToString(data.flag)
                });


                cmd.Parameters.Add(new SqlParameter("@all1", SqlDbType.Text)
                {
                    Value = Convert.ToString(data.all1)
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
                                dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? "N/A" : dataReader[iFiled] // use null instead of {}
                                );
                            }

                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    data.studentDetails = retObject.ToArray();
                    if (data.studentDetails.Length > 0)
                    {
                        data.count = data.studentDetails.Length;
                    }
                    else
                    {
                        data.count = 0;
                    }
                    data.schooldetails = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_ID).ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return data;


        }


        [Route("admcntdata")]
        //[Route("getenquirycontroller")]
        public async Task<studentbirthdayreportDTO> admcntdata([FromBody] studentbirthdayreportDTO data)
        {
            // id = 12;
            //return _dd.getdetails(data);

            //DateTime fromdatecon = DateTime.Now;
            //string confromdate = "";
            //fromdatecon = Convert.ToDateTime(data.Fromdate.Value.Date.ToString("yyyy-MM-dd"));
            //confromdate = fromdatecon.ToString("yyyy-MM-dd");

            //DateTime todatecon = DateTime.Now;
            //string contodate = "";
            //todatecon = Convert.ToDateTime(data.Todate.Value.Date.ToString("yyyy-MM-dd"));
            //contodate = todatecon.ToString("yyyy-MM-dd");

            string asmcl_ids = "0";
            if (data.classlsttwo != null && data.classlsttwo.Length > 0)
            {
                if (data.classlsttwo.Length > 0)
                {
                    foreach (var ue in data.classlsttwo)
                    {
                        asmcl_ids = asmcl_ids + "," + ue.ASMCL_Id;
                        // asmsid = asmsid + "," + ue.ASMS_Id;
                    }

                }
            }

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Adm_Yearwiseadmissioncount1";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                {
                    Value = data.ASMAY_Id
                });

               
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(data.MI_ID)
                });
                cmd.Parameters.Add(new SqlParameter("@ASMCL_ID", SqlDbType.VarChar)
                {
                    Value = asmcl_ids
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
                                dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? "N/A" : dataReader[iFiled] // use null instead of {}
                                );
                            }

                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    data.studentDetails = retObject.ToArray();
                    if (data.studentDetails.Length > 0)
                    {
                        data.count = data.studentDetails.Length;
                    }
                    else
                    {
                        data.count = 0;
                    }
                    data.schooldetails = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_ID).ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Adm_Yearwiseadmissioncount";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                {
                    Value = data.ASMAY_Id
                });


                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(data.MI_ID)
                });
                cmd.Parameters.Add(new SqlParameter("@ASMCL_ID", SqlDbType.VarChar)
                {
                    Value = asmcl_ids
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
                                dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? "N/A" : dataReader[iFiled] // use null instead of {}
                                );
                            }

                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    data.schooltabledetails = retObject.ToArray();
                    if (data.schooltabledetails.Length > 0)
                    {
                        data.count = data.schooltabledetails.Length;
                    }
                    else
                    {
                        data.count = 0;
                    }
                    //data.schooltabledetails = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_ID).ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return data;


        }

     
        [Route("getalladmdetails")]
        //[Route("getenquirycontroller")]
        public async Task<studentbirthdayreportDTO> getalladmdetails([FromBody] studentbirthdayreportDTO data)
        {
            // id = 12;
            //return _dd.getdetails(data);
            studentbirthdayreportDTO dto = new studentbirthdayreportDTO();
            data.accyear = _db.AcademicYear.Where(d => d.MI_Id == data.MI_ID && d.Is_Active == true).OrderByDescending(d => d.ASMAY_Order).ToArray();

            data.classlist = _db.School_M_Class.Where(d => d.MI_Id == data.MI_ID && d.ASMCL_ActiveFlag == true).OrderByDescending(d => d.ASMCL_Order).ToArray();





            return data;


        }

        [Route("ExportToExcle/")]

        public string ExportToExcle([FromBody] studentbirthdayreportDTO reg)
        {
            string str = "";



            return str;
        }


    }


}
