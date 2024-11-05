using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
using MailKit.Net.Smtp;
using MimeKit;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Net;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class countstatusreportFacadeController : Controller
    {
        public countstatusreportInterface _IStatus;
        private readonly DomainModelMsSqlServerContext _db;
        private readonly ApplicationDBContext _ApplicationDBContext;
        public countstatusreportFacadeController(countstatusreportInterface IStatus, DomainModelMsSqlServerContext db, ApplicationDBContext ApplicationDBContext)
        {
            _IStatus = IStatus;
            _db = db;
            _ApplicationDBContext = ApplicationDBContext;
        }

        // load initial dropdown
        [Route("getinitialdata/{mi_id:int}")]
        public Task<CommonDTO> getInitialData(int mi_id)
        {

            return _IStatus.getInitailData(mi_id);
        }

        //public countstatusreportFacadeController(DomainModelMsSqlServerContext db, ApplicationDBContext ApplicationDBContext)
        //{

        //    _db = db;
        //    _ApplicationDBContext = ApplicationDBContext;
        //}
        //// get student on search filters

        [Route("Getdetails/")]
        public async Task<CommonDTO> Getdetails([FromBody] CommonDTO reg)
        {
            //string studentRole = "OnlinePreadmissionUser";
            //  var id = _db.applicationRole.Single(d => d.Name == studentRole);

           


            if(reg.ASMAY_Id==0)
            {
                var Acdemic_preadmission = _db.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == reg.IVRM_MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();
                reg.ASMAY_Id = Acdemic_preadmission;
            }

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {

                cmd.CommandText = "getStatusCountreport";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Year_id", SqlDbType.BigInt)

                {
                    Value = Convert.ToInt32(reg.ASMAY_Id)
                }
                );
                cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.BigInt)

                {
                    Value = Convert.ToInt32(reg.IVRM_MI_Id)
                }
               );
              
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();
                var retobject1 = new List<dynamic>();
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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                );
                            }
                            retObject.Add((ExpandoObject)dataRow);
                        }
                        if (dataReader.NextResult())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retobject1.Add((ExpandoObject)dataRow1);
                            }
                        }

                    }
                    reg.countreportstatus = retObject.ToArray();
                    reg.applicationstatus = retobject1.ToArray();
                }
                catch (Exception ex)
                {

                }

            }

            //reg.applicationstatus = (from a in _db.AdmissionStatus
            //                         from b in _db.StudentApplication
            //                         where a.PAMST_Id == b.PAMS_Id && a.MI_Id == reg.IVRM_MI_Id
            //                         select new CommonDTO
            //                         {
            //                             PASRAPS_ID = b.PASRAPS_ID
            //                         })
            //                          .GroupBy(t => t.PASRAPS_ID, (key, values) => new { sId = key, Count = values.Count() }).ToArray();

            //var res = _db.StudentApplication.Where(a => a.MI_Id == reg.IVRM_MI_Id).GroupBy(t => t.PASRAPS_ID, (key, values) => new { sId = key, Count = values.Count() }).ToArray();
            //ar groups = userInfoList
            //.GroupBy(n => n.metric)
            //.Select(n => new
            //{
            //    MetricName = n.Key,
            //    MetricCount = n.Count()
            //}
            //)
            //.OrderBy(n => n.MetricName);




            return reg;
        }


    }
}
