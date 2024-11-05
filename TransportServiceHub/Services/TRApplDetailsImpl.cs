using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace TransportServiceHub.Services
{
    public class TRApplDetailsImpl : Interfaces.TRApplDetailsInterface
    {
        private static ConcurrentDictionary<string, TRApplDetailsDTO> _login =
        new ConcurrentDictionary<string, TRApplDetailsDTO>();
        public DomainModelMsSqlServerContext _db;
        public TransportContext _context;
        ILogger<TRApplDetailsImpl> _areaimpl;
        public TRApplDetailsImpl(ILogger<TRApplDetailsImpl> areaimpl, TransportContext context, DomainModelMsSqlServerContext db)
        {

            _areaimpl = areaimpl;
            _context = context;
            _db = db;
        }

        public TRApplDetailsDTO getdata(int id)
        {
            TRApplDetailsDTO data = new TRApplDetailsDTO();
            data.MI_Id = id;
            try
            {
                List<AcademicYear> allyear = new List<AcademicYear>();
                allyear = _context.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == id).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.YearList = allyear.Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Transport Error Driver Char savedata" + ex.Message);
            }

            return data;
        }


        public TRApplDetailsDTO Getreportdetails(TRApplDetailsDTO data)

        {
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TR_GET_TRANSPORT_APPLIED_NOTAPPLIED";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                                SqlDbType.VarChar)
                    {
                        Value = data.TYPE
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
                        data.griddata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public TRApplDetailsDTO sendmsg(TRApplDetailsDTO data)
        {
            try
            {
                SMS sm = new SMS(_db);
                Email em = new Email(_db);

                foreach (var item in data.Temp_sms_List)
                {
                    if (data.smscheck==true)
                    {
                        if (item.AMST_MobileNo!=0 )
                        {
                           // item.AMST_MobileNo = 9591081840;
                            string s = sm.sendSms_TRNotApplied(data.MI_Id, item.AMST_MobileNo, "TRNOTAPPLIED", data.MSG, item.AMST_Id).Result;
                        }
                    }
                    if (data.emailcheck == true)
                    {
                        if (item.AMST_emailId != "" && item.AMST_emailId != null)
                        {
                           // item.AMST_emailId = "praveenishwar@vapstech.com";
                            string s = em.sendmail_TRNotApplied(data.MI_Id, item.AMST_emailId, "TRNOTAPPLIED", data.MSG, item.AMST_Id);
                        }
                        
                    }
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
            
        }
        }
}
