using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOffice.com.vaps.Services
{
    public class StudentInOutReportImp : Interfaces.StudentInOutReportInterface
    {
        public FOContext _context;
        public StudentInOutReportImp(FOContext _cont)
        {
            _context = _cont;
        }
        public StudentInOutReportDTO loaddata(StudentInOutReportDTO data)
        {
            try
            {



                data.classlist = (from a in _context.Adm_School_Y_StudentDMO
                                  from b in _context.Adm_School_M_ClassDMO
                                  where (a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id)
                                  select new StudentInOutReportDTO
                                  {
                                      ASMCL_Id = a.ASMCL_Id,
                                      ASMCL_ClassName = b.ASMCL_ClassName
                                  }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //getsection
        public StudentInOutReportDTO getsection(StudentInOutReportDTO data)
        {
            try
            {



                List<long> Sectionid = new List<long>();
                foreach (var e in data.classlsttwo)
                {
                    Sectionid.Add(e.ASMCL_Id);
                }

                data.sectionlist = (from a in _context.Adm_School_Y_StudentDMO
                                    from b in _context.Adm_School_M_ClassDMO
                                    from c in _context.school_M_Section
                                    where (a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && Sectionid.Contains(b.ASMCL_Id) && c.MI_Id == data.MI_Id)
                                    select new StudentInOutReportDTO
                                    {
                                        ASMS_Id = c.ASMS_Id,
                                        ASMC_SectionName = c.ASMC_SectionName
                                    }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //getstudent
        public StudentInOutReportDTO getstudent(StudentInOutReportDTO data)
        {
            try
            {



                List<long> Sectionid = new List<long>();
                foreach (var e in data.classlsttwo)
                {
                    Sectionid.Add(e.ASMCL_Id);
                }

                List<long> Sectionids = new List<long>();
                foreach (var e in data.sectionlistarray)
                {
                    Sectionids.Add(e.ASMS_Id);
                }



                data.studentlist1 = (from a in _context.Adm_M_Student
                                     from b in _context.Adm_School_Y_StudentDMO
                                     from c in _context.Adm_School_M_ClassDMO
                                     from d in _context.school_M_Section
                                     where (c.ASMCL_Id == b.ASMCL_Id && d.ASMS_Id == b.ASMS_Id && a.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && Sectionids.Contains(d.ASMS_Id) && b.AMST_Id == a.AMST_Id && Sectionid.Contains(b.ASMCL_Id))
                                     select new StudentInOutReportDTO
                                     {
                                         AMST_Id = a.AMST_Id,
                                         AMST_FirstName = a.AMST_FirstName
                                     }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public StudentInOutReportDTO report(StudentInOutReportDTO data)
        {
            try
            {


                string AMSTIDD = "0";
                if (data.optionflag == "All")
                {
                    List<long> Sectionid = new List<long>();
                    foreach (var e in data.classlsttwo)
                    {
                        Sectionid.Add(e.ASMCL_Id);
                    }

                    List<long> Sectionids = new List<long>();
                    foreach (var e in data.sectionlistarray)
                    {
                        Sectionids.Add(e.ASMS_Id);
                    }


                    var studentlist = (from a in _context.Adm_School_Y_StudentDMO
                                       from b in _context.Adm_M_Student
                                       where(b.MI_Id==data.MI_Id && a.AMST_Id==b.AMST_Id && b.AMST_SOL=="S" && a.AMAY_ActiveFlag==1 && b.AMST_ActiveFlag==1 && Sectionids.Contains(a.ASMS_Id) && Sectionid.Contains(a.ASMCL_Id))
                                       select b).ToList();



                    if (studentlist != null && studentlist.Count() > 0)
                    {
                        foreach (var item in studentlist)
                        {

                            AMSTIDD = AMSTIDD + "," + item.AMST_Id;
                        }
                    }





                }
                else
                {
                    AMSTIDD = data.AMST_Id.ToString();
                }







                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ADM_Student_Punch_InoutDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@AMST_ID",
                    SqlDbType.VarChar)
                    {
                        Value = AMSTIDD
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
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                  SqlDbType.VarChar)
                    {
                        Value = data.inoutflag
                    });
                     cmd.Parameters.Add(new SqlParameter("@individual",
                  SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                     });




                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }
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
