using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CollegePreadmission.Interfaces;
using DataAccessMsSqlServerProvider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.College.Preadmission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegePreadmission.Controllers
{
    [Route("api/[controller]")]
    public class DocumentViewClgFacade : Controller
    {
        private readonly DomainModelMsSqlServerContext _db;
        public DocumentViewClgInterface _M_doc;
     
        public DocumentViewClgFacade(DocumentViewClgInterface _doc, DomainModelMsSqlServerContext db)
        {
            _M_doc = _doc;
            _db = db;
        }
        [Route("getdetails")]
        public CollegePreadmissionstudnetDto getInitialData([FromBody]CollegePreadmissionstudnetDto miid)
        {
            return _M_doc.getInitailData(miid);
        }
        [Route("getclgstudata")]
        public CollegePreadmissionstudnetDto getDpData([FromBody]CollegePreadmissionstudnetDto miid)
        {
            return _M_doc.getclgstudata(miid);
        }
        [Route("getdocksonly")]
        public CollegePreadmissionstudnetDto getdocksonly([FromBody]CollegePreadmissionstudnetDto miid)
        {
            return _M_doc.getdocksonly(miid);
        }

        [Route("getbranch")]
        public CollegePreadmissionstudnetDto getbranch([FromBody]CollegePreadmissionstudnetDto miid)
        {
            return _M_doc.getbranch(miid);
        }
        [Route("getsemester")]
        public CollegePreadmissionstudnetDto getsemester([FromBody]CollegePreadmissionstudnetDto miid)
        {
            return _M_doc.getsemester(miid);
        }
        //Admision Register Report
        [HttpPost]
        [Route("Getregdata")]
        public async Task<CollegePreadmissionstudnetDto> Getregdata([FromBody] CollegePreadmissionstudnetDto reg)
        {
            List<CollegePreadmissionstudnetDto> main_result = new List<CollegePreadmissionstudnetDto>();

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
                cmd.CommandText = "RegistrationReport";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@From_Date", SqlDbType.DateTime) { Value = reg.From_Date });
                cmd.Parameters.Add(new SqlParameter("@To_Date", SqlDbType.DateTime) { Value = reg.To_Date });
                cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.BigInt) { Value = reg.ASMAY_Id });
                cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.BigInt) { Value = reg.MI_Id });
                cmd.Parameters.Add(new SqlParameter("@psdate", SqlDbType.DateTime) { Value = reg.prestartdate });
                cmd.Parameters.Add(new SqlParameter("@prenddate", SqlDbType.DateTime) { Value = reg.presenddate });
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                var retObject = new List<dynamic>();

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                );
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
                    Console.WriteLine(ex.Message);
                }
                return reg;
            }
        }
    }
}
