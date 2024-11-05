using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class MarksEntryReportImpl : Interfaces.MarksEntryReportInterface
    {
        public ExamContext _context;
        public MarksEntryReportImpl(ExamContext context)
        {
            _context = context;
        }
        public MarksEntryReportDTO Getdetails(MarksEntryReportDTO data)
        {
            try
            {
                data.getyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MarksEntryReportDTO get_class(MarksEntryReportDTO data)
        {
            try
            {
                data.getclass = (from a in _context.Exm_Category_ClassDMO
                                 from b in _context.AdmissionClass
                                 where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id && a.ECAC_ActiveFlag == true && b.ASMCL_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                 select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MarksEntryReportDTO get_section(MarksEntryReportDTO data)
        {
            try
            {
                data.getsection = (from a in _context.Exm_Category_ClassDMO
                                   from b in _context.AdmissionClass
                                   from c in _context.School_M_Section
                                   where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == c.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ECAC_ActiveFlag == true && b.ASMCL_ActiveFlag == true
                                   && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id)
                                   select c).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MarksEntryReportDTO get_exam(MarksEntryReportDTO data)
        {
            try
            {

                if(data.ASMS_Id >0)
                {
                    var catid = _context.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id
                    && t.MI_Id == data.MI_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).ToArray();

                    var eycid = _context.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && catid.Contains(t.EMCA_Id)
                    && t.EYC_ActiveFlg == true).Select(t => t.EYC_Id).ToArray();

                    var emeid = _context.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id) && t.EYCE_ActiveFlg == true).Select(t => t.EME_Id).ToArray();

                    data.getexam = _context.masterexam.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true && emeid.Contains(t.EME_Id)).ToArray();
                }
                else
                {
                    var catid = _context.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.MI_Id 
                    && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).ToArray();

                    var eycid = _context.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && catid.Contains(t.EMCA_Id)
                    && t.EYC_ActiveFlg == true).Select(t => t.EYC_Id).ToArray();

                    var emeid = _context.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id) && t.EYCE_ActiveFlg == true).Select(t => t.EME_Id).ToArray();

                    data.getexam = _context.masterexam.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true && emeid.Contains(t.EME_Id)).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MarksEntryReportDTO get_report(MarksEntryReportDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exm_Marks_Entry_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });                 

                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
                    {
                        Value = data.EME_Id
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
                        data.getreport = retObject.ToArray();
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
        public MarksEntryReportDTO get_markspublishreport(MarksEntryReportDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exm_Marks_Publish_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
                    {
                        Value = data.EME_Id
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
                        data.getreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
