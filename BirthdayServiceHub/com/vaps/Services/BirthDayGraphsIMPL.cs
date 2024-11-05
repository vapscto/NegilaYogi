using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.BirthDay;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace BirthdayServiceHub.com.vaps.Services
{
    public class BirthDayGraphsIMPL : Interfaces.BirthDayGraphsInterface
    {
        int MI_ID = 0;
        private static ConcurrentDictionary<string, BirthDayDTO> _login =
          new ConcurrentDictionary<string, BirthDayDTO>();
        public DomainModelMsSqlServerContext _db;       
        public BirthDayGraphsIMPL(DomainModelMsSqlServerContext db)
        {
            _db = db;
       

        }
        public BirthDayDTO getdata(int id)
        {
            BirthDayDTO dto = new BirthDayDTO();

            dto.accyear = _db.AcademicYear.Where(R => R.MI_Id == id).Distinct().ToArray();


            return dto;


        }
        public BirthDayDTO getlistthree(BirthDayDTO stu)
        {
            BirthDayDTO acdmc = new BirthDayDTO();
            try
            {
             
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stu;
        }
        public BirthDayDTO staflist(BirthDayDTO stu1)
        {
            try
            {
             
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stu1;
        }
        public BirthDayDTO staflist1(BirthDayDTO stu2)
        {
            BirthDayDTO acdmc = new BirthDayDTO();
            try
            {
                List<BirthDayDTO> stafflist1 = new List<BirthDayDTO>();
               
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stu2;
        }             
        public BirthDayDTO Sendmsg(BirthDayDTO msg) //Send SMS & Email from front end
        {         
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "MonthWiseSmsEmailCount";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = msg.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar)
                    {
                       
                        Value = msg.ASMAY_Id
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
                            msg.sms_mail_count = retObject.Distinct().ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }

                }

                //Emailcounts
            

                msg.sectionDrpDwn = _db.month.Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return msg;
        }
        public BirthDayDTO getReport(BirthDayDTO rpt)
        {
            try
            {
                List<long> ASMAY_Idd = new List<long>();
               
                
                string ASMAY_Id = "0";
                if (rpt.ASMAY_IdList != null)
                {
                    foreach (var g in rpt.ASMAY_IdList)
                    {
                        ASMAY_Id = ASMAY_Id + "," + g.ASMAY_Id;
                        ASMAY_Idd.Add(g.ASMAY_Id);
                    }
                }
                else
                {
                    ASMAY_Idd.Add(rpt.ASMAY_Id);
                }
                rpt.accyear = _db.AcademicYear.Where(R => R.MI_Id == rpt.MI_Id && ASMAY_Idd.Contains(R.ASMAY_Id)).Distinct().ToArray();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Yearwisebirthdaycount";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = rpt.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar)
                    {
                        // Value = ASMAY_Id
                        Value=rpt.ASMAY_Id
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
                            rpt.sms_mail_count = retObject.Distinct().ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }

                }
                rpt.sectionDrpDwn = _db.month.Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return rpt;
        }
        public BirthDayDTO getEmailSMSCount(BirthDayDTO rpt)
        {
            try
            {
               

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return rpt;
        }
        public BirthDayDTO SearchByColumn(BirthDayDTO search)
        {
            try
            {
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return search;
        }
        public BirthDayDTO getmonthreport(BirthDayDTO rpt)
        {
            try
            {
             
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return rpt;
        }
        public async Task<BirthDayDTO> getstaffdetails(BirthDayDTO data)
        {
            // id = 12;
            //return _dd.getdetails(data);

        
            return data;
        }
   

    }
}
