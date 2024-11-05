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
    public class BookRegisterReportImpl : Interfaces.BookRegisterReportInterface
    {
        private LibraryContext _LibraryContext;

        public BookRegisterReportImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }

        public BookRegisterReportDTO getdetails(int id)
        {
            BookRegisterReportDTO data = new BookRegisterReportDTO();
            try
            {
                data.booktype = (from a in _LibraryContext.BookRegisterDMO.Where(a => a.MI_Id == id)
                                 select new BookRegisterReportDTO
                                 {
                                     LMB_BookType = a.LMB_BookType
                                 }).Distinct().ToArray();

                data.deptlist = _LibraryContext.MasterDepartmentDMO.Where(t => t.MI_Id == id).Distinct().ToArray();

                data.clsslist = _LibraryContext.Adm_School_M_ClassDMO.Where(t => t.MI_Id == id).Distinct().ToArray();
                //checklist

                data.Master_book = _LibraryContext.LIB_Master_Library_DMO.Where(R => R.LMAL_ActiveFlag == true && R.MI_Id == id).Distinct().ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public BookRegisterReportDTO get_report(BookRegisterReportDTO id)
        {
            try
            {
                if (id.Type == "all")
                {
                    if (id.LMD_Id > 0)
                    {

                        id.reportlist = (from a in _LibraryContext.BookRegisterDMO
                                         from b in _LibraryContext.MasterDepartmentDMO
                                         from c in _LibraryContext.MasterAuthorDMO
                                         from d in _LibraryContext.MasterPublisherDMO
                                         from e in _LibraryContext.MasterLanguageDMO
                                         from f in _LibraryContext.MasterSubject_DMO
                                         where (a.LMD_Id == b.LMD_Id && a.MI_Id == b.MI_Id && c.LMB_Id == a.LMB_Id && d.LMP_Id == a.LMP_Id && b.LMD_Id == id.LMD_Id && a.LML_Id == e.LML_Id && a.LMS_Id == f.LMS_Id && a.MI_Id == id.MI_Id)
                                         select new BookRegisterReportDTO
                                         {
                                             LMB_Id = a.LMB_Id,
                                             LMB_BookType = a.LMB_BookType,
                                             LMD_Id = b.LMD_Id,
                                             LMD_DepartmentName = b.LMD_DepartmentName,
                                             LMB_BookTitle = a.LMB_BookTitle,
                                             LMP_PublisherName = d.LMP_PublisherName,
                                             LMBA_AuthorFirstName = ((c.LMBA_AuthorFirstName == null ? " " : c.LMBA_AuthorFirstName) + " " + (c.LMBA_AuthorMiddleName == null ? " " : c.LMBA_AuthorLastName) + " " + (c.LMBA_AuthorLastName == null ? " " : c.LMBA_AuthorLastName)).Trim(),
                                             LML_LanguageName = e.LML_LanguageName,
                                             LMS_SubjectName = f.LMS_SubjectName,
                                             LMB_ClassNo = a.LMB_ClassNo,
                                             LMB_ISBNNo = a.LMB_ISBNNo,
                                             LMB_PurchaseDate = a.LMB_PurchaseDate,
                                             LMB_EntryDate = a.LMB_EntryDate,
                                             LMB_PurOrDonated = a.LMB_PurOrDonated,
                                             LMB_BillNo = a.LMB_BillNo,

                                         }).Distinct().ToArray();
                    }
                    else
                    {

                        id.reportlist = (from a in _LibraryContext.BookRegisterDMO
                                         from b in _LibraryContext.MasterDepartmentDMO
                                         from c in _LibraryContext.MasterAuthorDMO
                                         from d in _LibraryContext.MasterPublisherDMO
                                         from e in _LibraryContext.MasterLanguageDMO
                                         from f in _LibraryContext.MasterSubject_DMO
                                         where (a.LMD_Id == b.LMD_Id && a.MI_Id == b.MI_Id && c.LMB_Id == a.LMB_Id && d.LMP_Id == a.LMP_Id && a.LML_Id == e.LML_Id && a.LMS_Id == f.LMS_Id && a.MI_Id == id.MI_Id)
                                         select new BookRegisterReportDTO
                                         {
                                             LMB_Id = a.LMB_Id,
                                             LMB_BookType = a.LMB_BookType,
                                             LMD_Id = b.LMD_Id,
                                             LMD_DepartmentName = b.LMD_DepartmentName,
                                             LMB_BookTitle = a.LMB_BookTitle,
                                             LMP_PublisherName = d.LMP_PublisherName,
                                             LMBA_AuthorFirstName = ((c.LMBA_AuthorFirstName == null ? " " : c.LMBA_AuthorFirstName) + " " + (c.LMBA_AuthorMiddleName == null ? " " : c.LMBA_AuthorLastName) + " " + (c.LMBA_AuthorLastName == null ? " " : c.LMBA_AuthorLastName)).Trim(),
                                             LML_LanguageName = e.LML_LanguageName,
                                             LMS_SubjectName = f.LMS_SubjectName,
                                             LMB_ClassNo = a.LMB_ClassNo,
                                             LMB_ISBNNo = a.LMB_ISBNNo,
                                             LMB_PurchaseDate = a.LMB_PurchaseDate,
                                             LMB_EntryDate = a.LMB_EntryDate,
                                             LMB_PurOrDonated = a.LMB_PurOrDonated,
                                             LMB_BillNo = a.LMB_BillNo,

                                         }).Distinct().ToArray();
                    }

                }
                else
                {
                    if (id.LMD_Id > 0)
                    {

                        id.reportlist = (from a in _LibraryContext.BookRegisterDMO
                                         from b in _LibraryContext.MasterDepartmentDMO
                                         from c in _LibraryContext.MasterAuthorDMO
                                         from d in _LibraryContext.MasterPublisherDMO
                                         from e in _LibraryContext.MasterLanguageDMO
                                         from f in _LibraryContext.MasterSubject_DMO
                                         where (a.LMD_Id == b.LMD_Id && a.MI_Id == b.MI_Id && c.LMB_Id == a.LMB_Id && d.LMP_Id == a.LMP_Id && a.LML_Id == e.LML_Id && a.LMS_Id == f.LMS_Id && b.LMD_Id == id.LMD_Id && a.LMB_BookType == id.Type && a.MI_Id == id.MI_Id)
                                         select new BookRegisterReportDTO
                                         {
                                             LMB_Id = a.LMB_Id,
                                             LMB_BookType = a.LMB_BookType,
                                             LMD_Id = b.LMD_Id,
                                             LMD_DepartmentName = b.LMD_DepartmentName,
                                             LMB_BookTitle = a.LMB_BookTitle,
                                             LMP_PublisherName = d.LMP_PublisherName,
                                             LMBA_AuthorFirstName = ((c.LMBA_AuthorFirstName == null ? " " : c.LMBA_AuthorFirstName) + " " + (c.LMBA_AuthorMiddleName == null ? " " : c.LMBA_AuthorLastName) + " " + (c.LMBA_AuthorLastName == null ? " " : c.LMBA_AuthorLastName)).Trim(),
                                             LML_LanguageName = e.LML_LanguageName,
                                             LMS_SubjectName = f.LMS_SubjectName,
                                             LMB_ClassNo = a.LMB_ClassNo,
                                             LMB_ISBNNo = a.LMB_ISBNNo,
                                             LMB_PurchaseDate = a.LMB_PurchaseDate,
                                             LMB_EntryDate = a.LMB_EntryDate,
                                             LMB_PurOrDonated = a.LMB_PurOrDonated,
                                             LMB_BillNo = a.LMB_BillNo,

                                         }).Distinct().ToArray();
                    }
                    else
                    {

                        id.reportlist = (from a in _LibraryContext.BookRegisterDMO
                                         from b in _LibraryContext.MasterDepartmentDMO
                                         from c in _LibraryContext.MasterAuthorDMO
                                         from d in _LibraryContext.MasterPublisherDMO
                                         from e in _LibraryContext.MasterLanguageDMO
                                         from f in _LibraryContext.MasterSubject_DMO
                                         where (a.LMD_Id == b.LMD_Id && a.MI_Id == b.MI_Id && c.LMB_Id == a.LMB_Id && a.LML_Id == e.LML_Id && a.LMS_Id == f.LMS_Id && d.LMP_Id == a.LMP_Id && a.LMB_BookType == id.Type && a.MI_Id == id.MI_Id)
                                         select new BookRegisterReportDTO
                                         {
                                             LMB_Id = a.LMB_Id,
                                             LMB_BookType = a.LMB_BookType,
                                             LMD_Id = b.LMD_Id,
                                             LMD_DepartmentName = b.LMD_DepartmentName,
                                             LMB_BookTitle = a.LMB_BookTitle,
                                             LMP_PublisherName = d.LMP_PublisherName,
                                             LMBA_AuthorFirstName = ((c.LMBA_AuthorFirstName == null ? " " : c.LMBA_AuthorFirstName) + " " + (c.LMBA_AuthorMiddleName == null ? " " : c.LMBA_AuthorLastName) + " " + (c.LMBA_AuthorLastName == null ? " " : c.LMBA_AuthorLastName)).Trim(),
                                             LML_LanguageName = e.LML_LanguageName,
                                             LMS_SubjectName = f.LMS_SubjectName,
                                             LMB_ClassNo = a.LMB_ClassNo,
                                             LMB_ISBNNo = a.LMB_ISBNNo,
                                             LMB_PurchaseDate = a.LMB_PurchaseDate,
                                             LMB_EntryDate = a.LMB_EntryDate,
                                             LMB_PurOrDonated = a.LMB_PurOrDonated,
                                             LMB_BillNo = a.LMB_BillNo,

                                         }).Distinct().ToArray();
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return id;
        }
        //BarCode
        public BookRegisterReportDTO BarCode(BookRegisterReportDTO data)
        {
            try
            {

                string LMD_Id = "0";
                string LMB_Id = "0";
                if (data.DepartMent_list != null && data.DepartMent_list.Length > 0)
                {
                    foreach (var d in data.DepartMent_list)
                    {
                        LMD_Id = LMD_Id + ',' + d.LMD_Id;
                    }
                }

                if (data.Book_List != null && data.Book_List.Length > 0)
                {
                    foreach (var d in data.Book_List)
                    {
                        LMB_Id = LMB_Id + ',' + d.LMB_Id;
                    }
                }
                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_BOOK_REGISTER_MAIN_LOAD_BARCODE";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                 SqlDbType.VarChar)
                    {
                        Value = data.LMAL_Id
                    });
                    //LMB_ISBNNo
                    cmd.Parameters.Add(new SqlParameter("@LMB_ISBNNo",
                SqlDbType.VarChar)
                    {
                        Value = data.LMB_ISBNNo
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
               SqlDbType.VarChar)
                    {
                        Value = data.Type
                    });
                    //@LMD_Id
                    cmd.Parameters.Add(new SqlParameter("@LMD_Id",
              SqlDbType.VarChar)
                    {
                        Value = LMD_Id
                    });
                    //LMB_Id
                    cmd.Parameters.Add(new SqlParameter("@LMB_Id",
              SqlDbType.VarChar)
                    {
                        Value = LMB_Id
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
                        data.alldata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }

    }
}
