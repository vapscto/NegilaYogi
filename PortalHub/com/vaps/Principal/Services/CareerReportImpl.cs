
using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.Exam;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using PreadmissionDTOs.com.vaps.Portals.Employee;

namespace PortalHub.com.vaps.Principal.Services
{
    public class CareerReportImpl : Interfaces.CareerReportInterface
    {
        public FOContext _FOContext;

        private static ConcurrentDictionary<string, CareerReportDTO> _login =
         new ConcurrentDictionary<string, CareerReportDTO>();

        private readonly PortalContext _PrincipalDashboardContext;
        ILogger<LateInDetailsImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public CareerReportImpl(PortalContext cpContext, DomainModelMsSqlServerContext db, FOContext FOContext)
        {
            _PrincipalDashboardContext = cpContext;
            _db = db;
            _FOContext = FOContext;
        }

       


        public CareerReportDTO getalldetails(CareerReportDTO data) 
        {
            try
            {
              
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 90000;
                    cmd.CommandText = "PORTALCAREERREPORT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                     SqlDbType.Date)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                     SqlDbType.Date)
                    {
                        Value = data.todate
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
                        data.reportdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }

        //==========================home/class work upload
        public IVRM_Homework_DTO get_home_classwork(IVRM_Homework_DTO data)
        {
            try
            {

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 90000;
                    cmd.CommandText = "Home_classWork_report_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@upload_flg",
                      SqlDbType.VarChar)
                    {
                        Value = data.upload_flg
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
                        data.home_class_work_reports = retObject.ToArray();
                        data.upload_flg = data.upload_flg;
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }
    }
}
