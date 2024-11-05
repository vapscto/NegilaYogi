using DataAccessMsSqlServerProvider.com.vapstech.Hostel;
using DomainModel.Model.com.vapstech.Hostel;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Services
{
    public class Hostel_Student_InOutReportImple : Interface.Hostel_Student_InOutReportInterface
    {

        public HostelContext _context;
        public Hostel_Student_InOutReportImple(HostelContext _cont)
        {
            _context = _cont;
        }
        public Hostel_Student_InOutDTO loaddata(Hostel_Student_InOutDTO data)
        {
            try
            {
                data.studentlist1 = (from a in _context.Adm_Master_College_StudentDMO
                                  from b in _context.HL_Hostel_Student_BiometricDMO
                                  from c in _context.HL_Hostel_Student_Biometric_DetailsDMO
                                     where (a.AMCST_Id == b.AMCST_Id && b.HLHSTBIO_Id == c.HLHSTBIO_Id && b.MI_Id == data.MI_Id)
                                  select new Hostel_Student_InOutDTO
                                  {                                      
                                      AMCST_Id = a.AMCST_Id,                                      
                                      AMCST_FirstName = ((a.AMCST_FirstName == null || a.AMCST_FirstName == "" ? "" : " " + a.AMCST_FirstName)
                                     + (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "" || a.AMCST_MiddleName == "0" ? "" : " " + a.AMCST_MiddleName) + (a.AMCST_LastName == null || a.AMCST_LastName == "" || a.AMCST_LastName == "0" ? "" : " " + a.AMCST_LastName)).Trim(),
                                  }).Distinct().ToArray();  
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
       
        public Hostel_Student_InOutDTO empname(Hostel_Student_InOutDTO data)
        {
            Hostel_Student_InOutDTO TTMC = new Hostel_Student_InOutDTO();
            try
            {
                var que1 = _context.HL_Hostel_Student_BiometricDMO.Where(a => a.MI_Id == data.MI_Id && a.HLHSTBIO_PunchDate.Value.Date == data.empdate.Date && a.AMCST_Id == data.AMCST_Id).Select(t => t.HLHSTBIO_Id).FirstOrDefault();

                if (que1 > 0)
                {
                    var que2 = _context.HL_Hostel_Student_Biometric_DetailsDMO.Where( b=>b.HLHSTBIO_Id == que1).ToList();
                    TTMC.employeelist = que2.ToArray();
                    TTMC.returnval = true;
                }
                else
                {
                    TTMC.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TTMC.returnval = false;
            }
            return TTMC;
        }

        public Hostel_Student_InOutDTO savedetail(Hostel_Student_InOutDTO data)
        {

            try
            {
                if (data.HLHSTBIO_Id > 0)
                {
                    HL_Hostel_Student_Biometric_DetailsDMO fopunch = new HL_Hostel_Student_Biometric_DetailsDMO();             
                    fopunch.HLHSTBIO_Id = data.HLHSTBIO_Id;
                    fopunch.HLHSTBIOD_ActiveFlg = true;
                    fopunch.HLHSTBIOD_PunchTime = data.HLHSTBIOD_PunchTime;
                    fopunch.HLHSTBIOD_InOutFlg = data.HLHSTBIOD_InOutFlg;
                    fopunch.HLHSTBIOD_ManualEntryFlg = true;
                    fopunch.HLHSTBIOD_CreatedDate = DateTime.Now;
                    fopunch.HLHSTBIOD_UpdatedDate = DateTime.Now;
                    _context.Add(fopunch);

                    var contactExists = _context.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else
                {
                    var query2 = _context.HL_Hostel_Student_BiometricDMO.Where(d => d.AMCST_Id == data.AMCST_Id && d.HLHSTBIO_PunchDate.Value.Date == data.HLHSTBIO_PunchDate.Value.Date && d.MI_Id == data.MI_Id).ToList(); 
                    if (query2.Count == 0)
                    {
                        HL_Hostel_Student_BiometricDMO dmo = new HL_Hostel_Student_BiometricDMO();
                        dmo.HLHSTBIO_CreatedDate = DateTime.Now;
                        dmo.HLHSTBIO_ActiveFlg = true;   
                        dmo.HLHSTBIO_PunchDate = data.HLHSTBIO_PunchDate;
                        dmo.AMCST_Id = data.AMCST_Id;
                        dmo.MI_Id = data.MI_Id;
                        dmo.HLHSTBIO_UpdatedDate = DateTime.Now;
                        _context.Add(dmo);

                        HL_Hostel_Student_Biometric_DetailsDMO dmo2 = new HL_Hostel_Student_Biometric_DetailsDMO();
                        dmo2.HLHSTBIOD_CreatedDate = DateTime.Now;
                        dmo2.HLHSTBIOD_ActiveFlg = true;
                        dmo2.HLHSTBIO_Id = dmo.HLHSTBIO_Id;
                        dmo2.HLHSTBIOD_Id = dmo2.HLHSTBIOD_Id;
                        dmo2.HLHSTBIOD_ManualEntryFlg = true;
                        dmo2.HLHSTBIOD_InOutFlg = "I";
                        dmo2.HLHSTBIOD_PunchTime = Convert.ToDateTime(data.HLHSTBIOD_PunchTime).ToString("HH:mm");
                        dmo2.HLHSTBIOD_UpdatedDate = DateTime.Now;
                        _context.Add(dmo2);

                        var flag = _context.SaveChanges();
                        if (flag > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Hostel_Student_InOutDTO deleterec(Hostel_Student_InOutDTO data)
        {
            Hostel_Student_InOutDTO delete = new Hostel_Student_InOutDTO();
            try
            {
                List<HL_Hostel_Student_Biometric_DetailsDMO> lorg = new List<HL_Hostel_Student_Biometric_DetailsDMO>();
                lorg = _context.HL_Hostel_Student_Biometric_DetailsDMO.Where(t => t.HLHSTBIO_Id==data.HLHSTBIO_Id).ToList();
                if (lorg.Any())
                {
                    _context.Remove(lorg.ElementAt(0));
                    var contactExists = _context.SaveChanges();
                    if (contactExists == 1)
                    {
                        delete.returnval = true;
                    }
                    else
                    {
                        delete.returnval = false;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return delete;
        }

        // //// ///// REPORT
        public Hostel_Student_InOutDTO getloaddata(Hostel_Student_InOutDTO data)
        {
            try
            {
                data.studentlist = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.HL_Hostel_Student_BiometricDMO
                                     from c in _context.HL_Hostel_Student_Biometric_DetailsDMO
                                     where (a.AMCST_Id == b.AMCST_Id && b.HLHSTBIO_Id == c.HLHSTBIO_Id && b.MI_Id == data.MI_Id)
                                     select new Hostel_Student_InOutDTO
                                     {
                                         AMCST_Id = a.AMCST_Id,
                                         AMCST_FirstName = ((a.AMCST_FirstName == null || a.AMCST_FirstName == "" ? "" : " " + a.AMCST_FirstName)
                                        + (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "" || a.AMCST_MiddleName == "0" ? "" : " " + a.AMCST_MiddleName) + (a.AMCST_LastName == null || a.AMCST_LastName == "" || a.AMCST_LastName == "0" ? "" : " " + a.AMCST_LastName)).Trim(),
                                     }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Hostel_Student_InOutDTO report(Hostel_Student_InOutDTO data)
        {

            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Hl_Hostel_Student_InOut_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMCST_Id
                    });
                    cmd.Parameters.Add(value: new SqlParameter("@FROMDATE",
                  SqlDbType.VarChar)
                    {
                        Value = data.fromdate.Date.ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE",
                  SqlDbType.VarChar)
                    {
                        Value = data.todate.Date.ToString("yyyy-MM-dd")
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
                        data.viewlist = retObject.ToArray();

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

    }
}
