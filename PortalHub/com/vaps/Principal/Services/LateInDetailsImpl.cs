
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

namespace PortalHub.com.vaps.Principal.Services
{
    public class LateInDetailsImpl : Interfaces.LateInDetailsInterface
    {
        public FOContext _FOContext;

        private static ConcurrentDictionary<string, LateInDetailsDTO> _login =
         new ConcurrentDictionary<string, LateInDetailsDTO>();

        private readonly PortalContext _PrincipalDashboardContext;
        ILogger<LateInDetailsImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public LateInDetailsImpl(PortalContext cpContext, DomainModelMsSqlServerContext db, FOContext FOContext)
        {
            _PrincipalDashboardContext = cpContext;
            _db = db;
            _FOContext = FOContext;
        }

       


        public LateInDetailsDTO getalldetails(LateInDetailsDTO data) 
        {
            try
            {


                //    data.filldepartment = (from a in _FOContext.HR_Master_Employee_DMO
                //                           from b in _FOContext.HR_Master_Department_DMO
                //                           from c in _FOContext.EmployeeShiftMapping
                //                           from d in _FOContext.FO_Emp_Punch
                //                           from e in _FOContext.FO_Emp_Punch_Details


                //                           where (a.HRMD_Id == b.HRMD_Id && a.HRME_Id == c.HRME_Id && a.HRME_Id == d.HRME_Id && d.FOEP_Id == e.FOEP_Id && d.FOEP_PunchDate.Value.Date  == data.fromdate.Value.Date && a.MI_Id==data.MI_Id && e.FOEPD_InOutFlg == "I") 

                //                           select new LateInDetailsDTO
                //                           {
                //                               empFname = a.HRME_EmployeeFirstName,
                //                               empMname = a.HRME_EmployeeMiddleName,
                //                               empLname = a.HRME_EmployeeLastName,
                //                               HRMD_DepartmentName = b.HRMD_DepartmentName,
                //                               FOEST_IHalfLoginTime= c.FOEST_IHalfLoginTime,
                //                               FOEPD_PunchTime= e.FOEPD_PunchTime,
                //                               FOEP_PunchDate = d.FOEP_PunchDate,
                //                               MIID=a.MI_Id,
                //                               ts =Convert.ToDateTime(e.FOEPD_PunchTime).Subtract(Convert.ToDateTime(c.FOEST_IHalfLoginTime))

                //}
                //      ).Distinct().ToArray();
                //data.MI_Id = 5;
                //var a = "2017-06-20";
                //data.fromdate = Convert.ToDateTime(a);


                List<LateInDetailsDTO> result = new List<LateInDetailsDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 3000;
                    cmd.CommandText = "Principal_LateInDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@date",
                     SqlDbType.Date)
                    {
                        Value = data.fromdate
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
                                result.Add(new LateInDetailsDTO
                                {
                                    empFname = dataReader["ename"].ToString(),
                                    HRMD_DepartmentName = (dataReader["depname"].ToString()),
                                    FOEST_IHalfLoginTime = dataReader["actualtime"].ToString(),
                                    FOEP_PunchDate = Convert.ToDateTime(dataReader["punchdate"].ToString()),
                                    ts = dataReader["lateby"].ToString(),
                                    FOEPD_PunchTime = dataReader["intime"].ToString()

                                });
                                data.filldepartment = result.ToArray();
                            }
                        }
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
