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
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Employee;

namespace PortalHub.com.vaps.Chairman.Services
{
    public class NewChairmanDashboardImpl : Interfaces.NewChairmanDashboardInterface
    {
        private static ConcurrentDictionary<string, ChairmanDashboardDTO> _login =
         new ConcurrentDictionary<string, ChairmanDashboardDTO>();

        private readonly PortalContext _ChairmanDashboardContext;
        ILogger<ChairmanDashboardImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public NewChairmanDashboardImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
        }

        //public NewChairmanDashboardDTO Getdetails(NewChairmanDashboardDTO data)
        //{
        //    try
        //    {

        //        var semid = _db.UserRoleWithInstituteDMO.Where(t=>t.Id==data.Userid).ToList();

        //        List<long> Mi_id = new List<long>();
        //        if (semid.Count > 0)
        //        {
        //            foreach (var i in semid)
        //            {
        //                Mi_id.Add(i.MI_Id);
        //            }
        //        }
        //        else
        //        {
        //            Mi_id.Add(0);
        //        }
        //        data.studentCount = _db.Adm_M_Student.Where(t => Mi_id.Contains(t.MI_Id)).ToArray();
        //        //data.BooksCount = _db.BookRegisterDMO.Where(t => Mi_id.Contains(t.MI_Id)).ToArray();
             

             
                


            
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}




        public NewChairmanDashboardDTO Getdetails(NewChairmanDashboardDTO data)
        {
            try

            {
                

                using (var cmd = _ChairmanDashboardContext.Database.GetDbConnection().CreateCommand())                {
                    cmd.CommandText = "Chairman_Dashboard_Count";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@id",                                SqlDbType.BigInt)                    {                        Value = data.Userid                    });

                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.get_levl = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public NewChairmanDashboardDTO ViewFiles(NewChairmanDashboardDTO data)
        {


            using (var cmd = _ChairmanDashboardContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Chairman_Dashboard_Modal";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@id",
                            SqlDbType.BigInt)
                {
                    Value = data.Userid
                });
                cmd.Parameters.Add(new SqlParameter("@Type",
                         SqlDbType.VarChar)
                {
                    Value = data.flag
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
                    data.getReport = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


            return data;
        }





    }
         

        }










