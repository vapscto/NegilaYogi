using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class AttendanceRunImp : Interfaces.AttendanceRunInterface
    {
        public AdmissionFormContext _context;
        public AttendanceRunImp(AdmissionFormContext _con)
        {
            _context = _con;
        }
        public AttendanceRunDTO loaddata(AttendanceRunDTO data)
        {
            try
            {
                data.academic = _context.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

              
            }
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
            }
            return data;
        }



        public AttendanceRunDTO savedetails(AttendanceRunDTO data)
        {
            try
            {
               

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Student_Attendance_Insert";
                    cmd.CommandType = CommandType.StoredProcedure;

                   
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@dateparam", SqlDbType.Date) { Value = data.Date });



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
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
            }
            return data;
        }



        public AttendanceRunDTO griddetails(AttendanceRunDTO data)
        {
            try
            {
                
                string ASMS_Id = "0";
                string ASMCL_Id = "0";
                
                    if (data.studentlist != null && data.studentlist.Length > 0)
                    {
                        foreach (var item in data.studentlist)
                        {
                            ASMS_Id = ASMS_Id + "," + item.ASMS_Id;
                            ASMCL_Id = ASMCL_Id + "," + item.ASMCL_Id;
                        }
                    }
             




                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Student_Attendance_Insert_grid";
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.VarChar) { Value = data.Date });





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
                        data.griddto = retObject.ToArray();

                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }






            }
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
            }
            return data;
        }

    }
}
