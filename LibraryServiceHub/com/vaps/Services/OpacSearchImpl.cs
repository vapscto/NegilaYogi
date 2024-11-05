using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Library;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Services
{
    public class OpacSearchImpl : Interfaces.OpacSearchInterface
    {
        public DomainModelMsSqlServerContext _Context;
        public LibraryContext _LibraryContext;
        public OpacSearchImpl(DomainModelMsSqlServerContext Context, LibraryContext cont2)
        {
            _Context = Context;
            _LibraryContext = cont2;
        }
        public OpacSearchDTO getalldetails(OpacSearchDTO data)
        {
            try
            {
                data.subjectlist = (from a in _LibraryContext.MasterSubject_DMO
                                    where (a.MI_Id == data.MI_Id && a.LMS_ActiveFlg==true)
                                    select new OpacSearchDTO
                                    { 
                                        LMS_Id = a.LMS_Id,
                                        LMS_SubjectName = a.LMS_SubjectName
                                    }).Distinct().OrderBy(t => t.LMS_SubjectName).ToArray();
                data.publisherlist = (from b in _LibraryContext.MasterPublisherDMO
                                      where (b.MI_Id == data.MI_Id && b.LMP_ActiveFlg == true)
                                      select new OpacSearchDTO
                                      {
                                          LMP_Id = b.LMP_Id,
                                          LMP_PublisherName = b.LMP_PublisherName
                                      }).Distinct().OrderBy(t => t.LMP_PublisherName).ToArray();
                data.authorlist = (from b in _LibraryContext.LIB_Master_Author_DMO
                                   where (b.MI_Id == data.MI_Id && b.LMAU_ActiveFlg == true)
                                   select new OpacSearchDTO
                                   {
                                       LMAU_Id = b.LMAU_Id,
                                       authorname = b.LMAU_AuthorFirstName + (string.IsNullOrEmpty(b.LMAU_AuthorMiddleName) ? "" : ' ' + b.LMAU_AuthorMiddleName) + (string.IsNullOrEmpty(b.LMAU_AuthorLastName) ? "" : ' ' + b.LMAU_AuthorLastName),
                                   }).Distinct().OrderBy(t => t.authorname).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<OpacSearchDTO> report(OpacSearchDTO data)
        {         
            try
            {

                if(data.Title=="Calculation")
                {

                    using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "LIB_Weekly_Finecalculation";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_ID",
                        SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                     
                        cmd.Parameters.Add(new SqlParameter("@Returndate",
                        SqlDbType.VarChar)
                        {
                            Value = data.TYPE
                        });
                        cmd.Parameters.Add(new SqlParameter("@Duedate",
                               SqlDbType.VarChar)
                        {
                            Value = data.ClassNo
                        });
                        cmd.Parameters.Add(new SqlParameter("@Issuedate",
                               SqlDbType.VarChar)
                        {
                            Value = data.C1
                        });
                        cmd.Parameters.Add(new SqlParameter("@usertypeflag",
                             SqlDbType.VarChar)
                        {
                            Value = data.ISBNNO
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
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                else
                {

               
                if (data.TYPE==null)
                {
                    data.TYPE = "";
                }
                if (data.Title==null)
                {
                    data.Title = "";
                }
                if (data.AuthorId == null)
                {
                    data.AuthorId = "";
                }

                if (data.SubjectId == null)
                {
                    data.SubjectId = "";
                }
                if (data.PublisherId == null)
                {
                    data.PublisherId = "";
                }
                 if (data.AccessionNo == null)
                {
                    data.AccessionNo = "";
                }
                if (data.CallNo == null)
                {
                    data.CallNo = "";
                }
                if (data.ClassNo == null)
                {
                    data.ClassNo = "";
                }
                if (data.ISBNNO == null)
                {
                    data.ISBNNO = "";
                }
                if (data.C1 == null)
                {
                    data.C1 = "";
                }
                if (data.C2 == null)
                {
                    data.C2 = "";
                }
                if (data.C3 == null)
                {
                    data.C3 = "";
                }
                if (data.C4 == null)
                {
                    data.C4 = "";
                }

                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "OPACSEARCH_LIBRARY";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    //cmd.Parameters.Add(new SqlParameter("@ExactMatch",
                    //SqlDbType.BigInt)
                    //{
                    //    Value = data.ExactMatch
                    //});
                    
                    cmd.Parameters.Add(new SqlParameter("@Title",
                    SqlDbType.VarChar)
                    {
                        Value = data.Title
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
                    SqlDbType.VarChar)
                    {
                        Value = data.TYPE
                    });
                    cmd.Parameters.Add(new SqlParameter("@typesearch",
                    SqlDbType.VarChar)
                    {
                        Value = data.AuthorId
                    });
                    cmd.Parameters.Add(new SqlParameter("@SubjectId",
                    SqlDbType.VarChar)
                    {
                        Value = data.SubjectId
                    });
                    cmd.Parameters.Add(new SqlParameter("@PublisherId",
                    SqlDbType.VarChar)
                    {
                        Value = data.PublisherId
                    });
                    cmd.Parameters.Add(new SqlParameter("@AccessionNo",
                    SqlDbType.VarChar)
                    {
                        Value = data.AccessionNo
                    });
                    cmd.Parameters.Add(new SqlParameter("@CallNo",
                    SqlDbType.VarChar)
                    {
                        Value = data.CallNo
                    });
                    cmd.Parameters.Add(new SqlParameter("@ClassNo",
                    SqlDbType.VarChar)
                    {
                        Value = data.ClassNo
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISBNNO",
                    SqlDbType.VarChar)
                    {
                        Value = data.ISBNNO
                    });

                    cmd.Parameters.Add(new SqlParameter("@keyword",
                    SqlDbType.VarChar)
                    {
                        Value = data.ISBNNO
                    });
                    cmd.Parameters.Add(new SqlParameter("@C1",
                    SqlDbType.VarChar)
                    {
                        Value = data.C1
                    });
                    cmd.Parameters.Add(new SqlParameter("@C2",
                    SqlDbType.VarChar)
                    {
                        Value = data.C2
                    });
                    cmd.Parameters.Add(new SqlParameter("@C3",
                    SqlDbType.VarChar)
                    {
                        Value = data.C3
                    });
                    cmd.Parameters.Add(new SqlParameter("@C4",
                    SqlDbType.VarChar)
                    {
                        Value = data.C4
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
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
