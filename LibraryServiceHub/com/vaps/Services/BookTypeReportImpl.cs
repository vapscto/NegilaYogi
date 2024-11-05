using DataAccessMsSqlServerProvider.com.vapstech.Library;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Services
{
    public class BookTypeReportImpl:Interfaces.BookTypeReportInterface
    {
        private LibraryContext _LibraryContext;
        public BookTypeReportImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }

        public BookTypeReportDTO get_report(BookTypeReportDTO data)
        {
           

            //try
            //{
            //    if (data.Type== "all")
            //    {
            //        data.getdata = (from a in _LibraryContext.BookRegisterDMO
            //                        from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                        from c in _LibraryContext.BookTransactionDMO
            //                        where (a.LMB_Id == b.LMB_Id && b.LMBANO_Id == c.LMBANO_Id  && a.MI_Id == data.MI_Id && c.Issue_Date>=data.Issue_Date && c.Issue_Date<=data.IssueToDate )
            //                        select new BookTypeReportDTO
            //                        {
            //                            LMB_Id = a.LMB_Id,
            //                            MI_Id = a.MI_Id,
            //                            LMB_BookTitle = a.LMB_BookTitle,
            //                            LMB_BookType = a.LMB_BookType,
            //                            LMB_ISBNNo = a.LMB_ISBNNo,
            //                            LMB_Price = a.LMB_Price,
            //                            LMBANO_Id = b.LMBANO_Id,
            //                            LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                            Book_Trans_Status = c.Book_Trans_Status,
            //                        }).Distinct().ToArray();
            //    }
            //    else
            //    {
            //        data.getdata = (from a in _LibraryContext.BookRegisterDMO
            //                        from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                        from c in _LibraryContext.BookTransactionDMO
            //                        where (a.LMB_Id == b.LMB_Id && b.LMBANO_Id == c.LMBANO_Id  && a.MI_Id == data.MI_Id && a.LMB_BookType==data.Type && c.Issue_Date >= data.Issue_Date && c.Issue_Date <= data.IssueToDate && c.Book_Trans_Status == "Issue")
            //                        select new BookTypeReportDTO
            //                        {
            //                            LMB_Id = a.LMB_Id,
            //                            MI_Id = a.MI_Id,
            //                            LMB_BookTitle = a.LMB_BookTitle,
            //                            LMB_BookType = a.LMB_BookType,
            //                            LMB_ISBNNo = a.LMB_ISBNNo,
            //                            LMB_Price = a.LMB_Price,
            //                            LMBANO_Id = b.LMBANO_Id,
            //                            LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                            Book_Trans_Status = c.Book_Trans_Status,
            //                        }).Distinct().ToArray();
            //    }

            try
            {
                if (data.Type == "all")
                {
                    data.getdata = (from a in _LibraryContext.BookRegisterDMO
                                    from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                    from c in _LibraryContext.BookTransactionDMO
                                    where (a.LMB_Id == b.LMB_Id && b.LMBANO_Id == c.LMBANO_Id && a.MI_Id == data.MI_Id && c.LBTR_IssuedDate >= data.Issue_Date && c.LBTR_IssuedDate <= data.IssueToDate)
                                    select new BookTypeReportDTO
                                    {
                                        LMB_Id = a.LMB_Id,
                                        MI_Id = a.MI_Id,
                                        LMB_BookTitle = a.LMB_BookTitle,
                                        LMB_BookType = a.LMB_BookType,
                                        LMB_ISBNNo = a.LMB_ISBNNo,
                                        LMB_Price = a.LMB_Price,
                                        LMBANO_Id = b.LMBANO_Id,
                                        LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                        Book_Trans_Status = c.LBTR_Status,
                                    }).Distinct().ToArray();
                }
                else
                {
                    data.getdata = (from a in _LibraryContext.BookRegisterDMO
                                    from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                    from c in _LibraryContext.BookTransactionDMO
                                    where (a.LMB_Id == b.LMB_Id && b.LMBANO_Id == c.LMBANO_Id && a.MI_Id == data.MI_Id && a.LMB_BookType == data.Type && c.LBTR_IssuedDate >= data.Issue_Date && c.LBTR_IssuedDate <= data.IssueToDate && c.LBTR_Status == "Issue")
                                    select new BookTypeReportDTO
                                    {
                                        LMB_Id = a.LMB_Id,
                                        MI_Id = a.MI_Id,
                                        LMB_BookTitle = a.LMB_BookTitle,
                                        LMB_BookType = a.LMB_BookType,
                                        LMB_ISBNNo = a.LMB_ISBNNo,
                                        LMB_Price = a.LMB_Price,
                                        LMBANO_Id = b.LMBANO_Id,
                                        LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                        Book_Trans_Status = c.LBTR_Status,
                                    }).Distinct().ToArray();
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
