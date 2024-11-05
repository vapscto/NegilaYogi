using System;
using DataAccessMsSqlServerProvider.com.vapstech.Library;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Library;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using LibraryServiceHub.com.vaps.Interfaces;

namespace LibraryServiceHub.com.vaps.Services
{
    public class BookCirculationReportImpl:Interfaces.BookCirculationReportInterface
    {

        private static ConcurrentDictionary<string, BookCirculationReportDTO> _login =
         new ConcurrentDictionary<string, BookCirculationReportDTO>();

        private LibraryContext _LibraryContext;
        public BookCirculationReportImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }

        public BookCirculationReportDTO getstuddetails(BookCirculationReportDTO data)
        {

            //data.alldata = (from a in _LibraryContext.BookRegisterDMO
            //                from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                from c in _LibraryContext.BookTransactionDMO
            //                from d in _LibraryContext.Adm_M_Student
            //                where (a.LMB_Id == b.LMB_Id && b.LMBANO_Id == c.LMBANO_Id && d.AMST_Id==c.AMST_Id && a.MI_Id==data.MI_Id && a.LMB_BookType==data.LMB_BookType) select new BookCirculationReportDTO
            //                {
            //                    Book_Trans_Id=c.Book_Trans_Id,
            //                    AMST_Id=d.AMST_Id,
            //                    LMB_BookTitle = a.LMB_BookTitle,
            //                    Book_Trans_Status = c.Book_Trans_Status,
            //                    LMBANO_AccessionNo=b.LMBANO_AccessionNo,
            //                    AMST_FirstName = ((d.AMST_FirstName == null ? " " : d.AMST_FirstName) + " " + (d.AMST_MiddleName == null ? " " : d.AMST_MiddleName) + " " + (d.AMST_LastName == null ? " " : d.AMST_LastName)).Trim(),
            //                    AMST_MiddleName = d.AMST_MiddleName,
            //                    AMST_LastName = d.AMST_LastName
            //                }).Distinct().ToArray();
           
            

            return data;
        }

        public BookCirculationReportDTO getdetails(BookCirculationReportDTO id)
        {
            //BookCirculationReportDTO data = new BookCirculationReportDTO();
            try
            {
                id.booktype = (from a in _LibraryContext.BookRegisterDMO.Where(a => a.MI_Id == id.MI_Id) select new BookCirculationReportDTO
                {
                    LMB_BookType = a.LMB_BookType,
                   // LMB_Id = a.LMB_Id

                }).Distinct().ToArray();
                id.lib_list = (from a in _LibraryContext.LIB_Master_Library_DMO
                               from b in _LibraryContext.LIB_User_Library_DMO
                                   // from c in _LibraryContext.LIB_Library_Class_DMO
                               where a.MI_Id == b.MI_Id && a.MI_Id == id.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == id.IVRMUL_Id
                               select a).ToArray();
                var lib_list1 = (from a in _LibraryContext.LIB_Master_Library_DMO
                                 from b in _LibraryContext.LIB_User_Library_DMO
                                     // from c in _LibraryContext.LIB_Library_Class_DMO
                                 where a.MI_Id == b.MI_Id && a.MI_Id == id.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == id.IVRMUL_Id
                                 select a).ToList();

                if (lib_list1.Count>0)
                {
                    id.classList = (from a in _LibraryContext.School_M_Class
                                    from c in _LibraryContext.LIB_Library_Class_DMO
                                    where (a.MI_Id == c.MI_Id && a.ASMCL_Id == c.ASMCL_Id && c.MI_Id == id.MI_Id && a.ASMCL_ActiveFlag == true && c.LLC_ActiveFlg==true && c.LMAL_Id==lib_list1.FirstOrDefault().LMAL_Id)select a).Distinct().ToArray();
                    id.LMAL_Id = lib_list1.FirstOrDefault().LMAL_Id;
                }

                //id.classList = _LibraryContext.School_M_Class.Where(d => d.MI_Id == id.MI_Id && d.ASMCL_ActiveFlag == true).Select(d => new BookCirculationReportDTO { ASMCL_Id = d.ASMCL_Id, className = d.ASMCL_ClassName }).ToArray();

                id.sectionList = _LibraryContext.school_M_Section.Where(d => d.MI_Id == id.MI_Id && d.ASMC_ActiveFlag == 1).Select(d => new BookCirculationReportDTO { ASMS_Id = d.ASMS_Id, sectionName = d.ASMC_SectionName }).ToArray();

                id.booktitle = (from t in  _LibraryContext.BookRegisterDMO.Where(t => t.MI_Id == id.MI_Id) select new BookCirculationReportDTO { LMB_Id=t.LMB_Id,LMB_BookTitle=t.LMB_BookTitle }).Distinct().ToArray();

                id.parentsubjectlist = _LibraryContext.IVRM_Master_Subjects_DMO.Where(t => t.MI_Id == id.MI_Id).Distinct().ToArray();

                id.subsubjectlist = _LibraryContext.MasterSubject_DMO.Where(t => t.MI_Id == id.MI_Id).Distinct().ToArray();

               
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return id;
        }
        
        public async Task<BookCirculationReportDTO> get_report(BookCirculationReportDTO data)
        {
            try
            {
                string classs_ids = "0";
                string section_idss = "0";


                List<long> clss_ids = new List<long>();
                List<long> section_ids = new List<long>();


                if (data.AGType == "Student")
                {
                    foreach (var item in data.selectedClasslist)
                    {
                        clss_ids.Add(item.ASMCL_Id);
                    }
                    for (int s = 0; s < clss_ids.Count(); s++)
                    {
                        classs_ids = classs_ids + ',' + clss_ids[s].ToString();

                    }
                    foreach (var item in data.selectedSectionlist)
                    {
                        section_ids.Add(item.ASMS_Id);
                    }
                    for (int s = 0; s < section_ids.Count(); s++)
                    {
                        section_idss = section_idss + ',' + section_ids[s].ToString();
                    }

                }
                if(data.BookSummary==true)
                {
                    string LMS_Id = "0";
                    if (data.BookSummaryCircular != null && data.BookSummaryCircular.Length > 0)
                    {
                       foreach(var d in data.BookSummaryCircular)
                        {
                            LMS_Id = LMS_Id + ',' + d.LMS_Id;
                        }
                    }
                    using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Lib_Transaction_Stthomos";
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@Type",
                         SqlDbType.VarChar)
                        {
                            Value = data.Type
                        });
                        cmd.Parameters.Add(new SqlParameter("@AGType",
                       SqlDbType.VarChar)
                        {
                            Value = data.AGType
                        });
                        cmd.Parameters.Add(new SqlParameter("@TrnType",
                       SqlDbType.VarChar)
                        {
                            Value = data.TrnType
                        });

                        cmd.Parameters.Add(new SqlParameter("@IssueFromDate",
                   SqlDbType.VarChar)
                        {
                            Value = data.IssueFromDate
                        });
                        cmd.Parameters.Add(new SqlParameter("@IssueToDate",
                   SqlDbType.VarChar)
                        {
                            Value = data.IssueToDate
                        });
                        cmd.Parameters.Add(new SqlParameter("@DueFromdate",
                SqlDbType.VarChar)
                        {
                            Value = data.DueFromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@DueTodate",
                   SqlDbType.VarChar)
                        {
                            Value = data.DueTodate
                        });
                        cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                SqlDbType.BigInt)
                        {
                            Value = data.LMAL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
             SqlDbType.VarChar)
                        {
                            Value = classs_ids
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                SqlDbType.VarChar)
                        {
                            Value = section_idss
                        });
                        cmd.Parameters.Add(new SqlParameter("@LMS_Id",
                SqlDbType.VarChar)
                        {
                            Value = LMS_Id
                        });
                        //@ISMS_Id

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
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
                            }
                            data.reportlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Lib_Transaction_NewOne";
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@Type",
                         SqlDbType.VarChar)
                        {
                            Value = data.Type
                        });
                        cmd.Parameters.Add(new SqlParameter("@AGType",
                       SqlDbType.VarChar)
                        {
                            Value = data.AGType
                        });
                        cmd.Parameters.Add(new SqlParameter("@TrnType",
                       SqlDbType.VarChar)
                        {
                            Value = data.TrnType
                        });

                        cmd.Parameters.Add(new SqlParameter("@IssueFromDate",
                   SqlDbType.VarChar)
                        {
                            Value = data.IssueFromDate
                        });
                        cmd.Parameters.Add(new SqlParameter("@IssueToDate",
                   SqlDbType.VarChar)
                        {
                            Value = data.IssueToDate
                        });
                        cmd.Parameters.Add(new SqlParameter("@DueFromdate",
                SqlDbType.VarChar)
                        {
                            Value = data.DueFromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@DueTodate",
                   SqlDbType.VarChar)
                        {
                            Value = data.DueTodate
                        });
                        cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                SqlDbType.BigInt)
                        {
                            Value = data.LMAL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
             SqlDbType.VarChar)
                        {
                            Value = classs_ids
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                SqlDbType.VarChar)
                        {
                            Value = section_idss
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
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
                            }
                            data.reportlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
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
        
    }
}
