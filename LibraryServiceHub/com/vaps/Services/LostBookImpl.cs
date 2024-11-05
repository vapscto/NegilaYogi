using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.HRMS;
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
    public class LostBookImpl : Interfaces.LostBookInterface
    {

        public LibraryContext _LibraryContext;

        public LostBookImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }

        public LostBook_DTO getdetails(LostBook_DTO data)
        {

            try
            {
                if (data.bookcat_type == "book")
                {
                    if (data.booktype == "issue")
                    {
                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                          from c in _LibraryContext.MasterCategoryDMO
                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && a.LMB_ActiveFlg == true && a.LMB_BookType == "Issue" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Book" && b.LMBANO_AvialableStatus != "lost")
                                          select new LostBook_DTO
                                          {

                                              LMB_Id = a.LMB_Id,
                                              MI_Id = a.MI_Id,
                                              LMB_BookTitle = a.LMB_BookTitle,
                                              LMC_Id = a.LMC_Id,
                                              LMBANO_Id = b.LMBANO_Id,
                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo,


                                          }).Distinct().OrderBy(t=>t.LMBANO_Id).Take(15).ToArray();
                    }
                    else
                    {
                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                          from c in _LibraryContext.MasterCategoryDMO
                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && a.LMB_ActiveFlg == true && a.LMB_BookType == "Reference" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Book" && b.LMBANO_AvialableStatus != "lost")
                                          select new LostBook_DTO
                                          {

                                              LMB_Id = a.LMB_Id,
                                              MI_Id = a.MI_Id,
                                              LMB_BookTitle = a.LMB_BookTitle,
                                              LMC_Id = a.LMC_Id,
                                              LMBANO_Id = b.LMBANO_Id,
                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo,


                                          }).Distinct().OrderBy(t => t.LMBANO_Id).Take(15).ToArray();
                    }
                }
                else if (data.bookcat_type == "nonbook")
                {
                    if (data.booktype == "issue")
                    {
                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                          from c in _LibraryContext.MasterCategoryDMO
                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && a.LMB_ActiveFlg == true && a.LMB_BookType == "Issue" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "nonbook" && b.LMBANO_AvialableStatus != "lost")
                                          select new LostBook_DTO
                                          {

                                              LMB_Id = a.LMB_Id,
                                              MI_Id = a.MI_Id,
                                              LMB_BookTitle = a.LMB_BookTitle,
                                              LMC_Id = a.LMC_Id,
                                              LMBANO_Id = b.LMBANO_Id,
                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo,


                                          }).Distinct().OrderBy(t => t.LMBANO_Id).Take(15).ToArray();
                    }
                    else
                    {
                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                          from c in _LibraryContext.MasterCategoryDMO
                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && a.LMB_ActiveFlg == true && a.LMB_BookType == "Reference" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "nonbook" && b.LMBANO_AvialableStatus != "lost")
                                          select new LostBook_DTO
                                          {

                                              LMB_Id = a.LMB_Id,
                                              MI_Id = a.MI_Id,
                                              LMB_BookTitle = a.LMB_BookTitle,
                                              LMC_Id = a.LMC_Id,
                                              LMBANO_Id = b.LMBANO_Id,
                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo,


                                          }).Distinct().OrderBy(t => t.LMBANO_Id).Take(15).ToArray();
                    }
                }



                //data.lostbooks = (from a in _LibraryContext.Lib_M_Book_Accn_DMO
                //                  from b in _LibraryContext.BookRegisterDMO
                //                  where (a.LMB_Id == b.LMB_Id && b.MI_Id == data.MI_Id && a.LMBANO_AvialableStatus=="lost")
                //                  select new LostBook_DTO
                //                  {
                //                      LMB_Id=b.LMB_Id,
                //                      LMB_BookTitle=b.LMB_BookTitle,
                //                      LMBANO_Id = a.LMBANO_Id,
                //                      LMBANO_AccessionNo=a.LMBANO_AccessionNo,
                //                      LMBANO_AvialableStatus=a.LMBANO_AvialableStatus,
                //                      LMBANO_LostDamagedDate=a.LMBANO_LostDamagedDate,
                //                      LMBANO_LostDamagedReason=a.LMBANO_LostDamagedReason,
                //                      LMBANO_AmountCollected=a.LMBANO_AmountCollected,
                //                      LMBANO_ModeOfPayment=a.LMBANO_ModeOfPayment,
                //                  }).Distinct().OrderBy(t => t.LMBANO_Id).ToArray();

                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_LostBook_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.lostbooks = retObject.ToArray();
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

        public LostBook_DTO searchfilter(LostBook_DTO data)
        {
            try
            {
                data.searchfilter = data.searchfilter.ToLower();

                if (data.bookcat_type == "book")
                {
                    if (data.booktype == "issue")
                    {
                       
                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                          from c in _LibraryContext.MasterCategoryDMO
                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id /*&& LMB_Id.Contains(a.LMB_Id)*/ && a.LMB_ActiveFlg == true && a.LMB_BookType == "Issue" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Book" && b.LMBANO_AvialableStatus != "lost" && ((a.LMB_BookTitle.ToLower().Contains(data.searchfilter)) || (b.LMBANO_AccessionNo.ToLower().Contains(data.searchfilter))))
                                          select new LostBook_DTO
                                          {

                                              LMB_Id = a.LMB_Id,
                                              MI_Id = a.MI_Id,
                                              LMB_BookTitle = a.LMB_BookTitle,
                                              LMC_Id = a.LMC_Id,
                                              LMBANO_Id = b.LMBANO_Id,
                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo
                                   


                                          }).Distinct().OrderBy(t => t.LMBANO_Id).ToArray();


                    }
                    else if (data.booktype == "ref")
                    {
                        
                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                          from c in _LibraryContext.MasterCategoryDMO
                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id /*&& LMB_Id.Contains(a.LMB_Id)*/ && a.LMB_ActiveFlg == true && a.LMB_BookType == "Reference" && b.LMBANO_AvialableStatus != "lost" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Book" && ((a.LMB_BookTitle.ToLower().Contains(data.searchfilter)) || (b.LMBANO_AccessionNo.ToLower().Contains(data.searchfilter))))
                                          select new LostBook_DTO
                                          {
                                              LMB_Id = a.LMB_Id,
                                              MI_Id = a.MI_Id,
                                              LMB_BookTitle = a.LMB_BookTitle,
                                              LMC_Id = a.LMC_Id,
                                              LMBANO_Id = b.LMBANO_Id,
                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo
                                              

                                          }).Distinct().OrderBy(t => t.LMBANO_Id).ToArray();
                    }

                }
                else
                {
                    if (data.booktype == "issue")
                    {
                     
                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                          from c in _LibraryContext.MasterCategoryDMO
                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id /*&& LMB_Id.Contains(a.LMB_Id)*/ && a.LMB_ActiveFlg == true && a.LMB_BookType == "Issue" && b.LMBANO_AvialableStatus != "lost" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Non-Book" && ((a.LMB_BookTitle.ToLower().Contains(data.searchfilter)) || (b.LMBANO_AccessionNo.ToLower().Contains(data.searchfilter))))
                                          select new LostBook_DTO
                                          {

                                              LMB_Id = a.LMB_Id,
                                              MI_Id = a.MI_Id,
                                              LMB_BookTitle = a.LMB_BookTitle,
                                       
                                              LMC_Id = a.LMC_Id,
                                              LMBANO_Id = b.LMBANO_Id,
                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo

                                          }).Distinct().OrderBy(t => t.LMBANO_Id).ToArray();


                    }
                    else if (data.booktype == "ref")
                    {
                      
                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                          from c in _LibraryContext.MasterCategoryDMO
                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id /*&& LMB_Id.Contains(a.LMB_Id)*/ && a.LMB_ActiveFlg == true && a.LMB_BookType == "Reference" && b.LMBANO_AvialableStatus != "lost" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Non-Book" && ((a.LMB_BookTitle.ToLower().Contains(data.searchfilter)) || (b.LMBANO_AccessionNo.ToLower().Contains(data.searchfilter))))
                                          select new LostBook_DTO
                                          {
                                              LMB_Id = a.LMB_Id,
                                              MI_Id = a.MI_Id,
                                              LMB_BookTitle = a.LMB_BookTitle,
                                              LMC_Id = a.LMC_Id,
                                              LMBANO_Id = b.LMBANO_Id,
                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo

                                          }).Distinct().OrderBy(t => t.LMBANO_Id).ToArray();
                    }
                
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

        public async Task<LostBook_DTO> get_authorNm(LostBook_DTO data)
        {
            try
            {
                try
                {
                    using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "LIB_Get_AuthorName";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@LMBANO_Id",
                        SqlDbType.VarChar)
                        {
                            Value = data.LMBANO_Id
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
                            data.authorlist = retObject.ToArray();
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


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return data;
        }

        public async Task<LostBook_DTO> get_radiochange(LostBook_DTO data)
        {
            try
            {
                #region
                //using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "Lib_GetBookName";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //    SqlDbType.BigInt)
                //    {
                //        Value = data.MI_Id
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@bookcat_type",
                //    SqlDbType.VarChar)
                //    {
                //        Value = data.bookcat_type
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@booktype",
                //    SqlDbType.VarChar)
                //    {
                //        Value = data.booktype
                //    });


                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = await cmd.ExecuteReaderAsync())
                //        {
                //            while (await dataReader.ReadAsync())
                //            {
                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                {
                //                    dataRow.Add(
                //                        dataReader.GetName(iFiled),
                //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        data.booktitle = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}
                #endregion

                #region
                if (data.bookcat_type == "book")
                {
                    if (data.booktype == "issue")
                    {
                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                          from c in _LibraryContext.MasterCategoryDMO
                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && a.LMB_ActiveFlg == true && a.LMB_BookType == "Issue" && b.LMBANO_AvialableStatus != "lost" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Book")
                                          select new LostBook_DTO
                                          {

                                              LMB_Id = a.LMB_Id,
                                              MI_Id = a.MI_Id,
                                              LMB_BookTitle = a.LMB_BookTitle,
                                              LMC_Id = a.LMC_Id,
                                              LMBANO_Id = b.LMBANO_Id,
                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo,


                                          }).Distinct().OrderBy(t => t.LMBANO_Id).Take(15).ToArray();
                    }
                    else
                    {
                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                          from c in _LibraryContext.MasterCategoryDMO
                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && a.LMB_ActiveFlg == true && a.LMB_BookType == "Reference" && b.LMBANO_AvialableStatus != "lost" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Book")
                                          select new LostBook_DTO
                                          {

                                              LMB_Id = a.LMB_Id,
                                              MI_Id = a.MI_Id,
                                              LMB_BookTitle = a.LMB_BookTitle,
                                              LMC_Id = a.LMC_Id,
                                              LMBANO_Id = b.LMBANO_Id,
                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo,


                                          }).Distinct().OrderBy(t => t.LMBANO_Id).Take(15).ToArray();
                    }
                }
                else if (data.bookcat_type == "nonbook")
                {
                    if (data.booktype == "issue")
                    {
                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                          from c in _LibraryContext.MasterCategoryDMO
                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && a.LMB_ActiveFlg == true && a.LMB_BookType == "Issue" && b.LMBANO_AvialableStatus != "lost" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Non-Book")
                                          select new LostBook_DTO
                                          {

                                              LMB_Id = a.LMB_Id,
                                              MI_Id = a.MI_Id,
                                              LMB_BookTitle = a.LMB_BookTitle,
                                              LMC_Id = a.LMC_Id,
                                              LMBANO_Id = b.LMBANO_Id,
                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo,


                                          }).Distinct().OrderBy(t => t.LMBANO_Id).Take(15).ToArray();
                    }
                    else
                    {
                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                          from c in _LibraryContext.MasterCategoryDMO
                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && a.LMB_ActiveFlg == true && a.LMB_BookType == "Reference" && b.LMBANO_AvialableStatus != "lost" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Non-Book")
                                          select new LostBook_DTO
                                          {

                                              LMB_Id = a.LMB_Id,
                                              MI_Id = a.MI_Id,
                                              LMB_BookTitle = a.LMB_BookTitle,
                                              LMC_Id = a.LMC_Id,
                                              LMBANO_Id = b.LMBANO_Id,
                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo,


                                          }).Distinct().OrderBy(t => t.LMBANO_Id).Take(15).ToArray();
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public LostBook_DTO saverecord(LostBook_DTO data)
        {
            try
            {
                if(data.LMBANO_Id>0)
                {
                    var update = _LibraryContext.Lib_M_Book_Accn_DMO.Single(t => t.LMBANO_Id == data.LMBANO_Id);

                    update.LMBANO_AvialableStatus = data.LMBANO_AvialableStatus;
                    update.LMBANO_LostDamagedDate = data.LMBANO_LostDamagedDate;
                    update.LMBANO_LostDamagedReason = data.LMBANO_LostDamagedReason;
                    update.LMBANO_LostDamagedFlg = true;
                    update.LMBANO_AmountCollected = data.LMBANO_AmountCollected;

                    update.LMBANO_ModeOfPayment = data.LMBANO_ModeOfPayment;
                    update.LMBANO_CreatedBy = data.Login_Id;

                  
                    _LibraryContext.Update(update);

                    int s = _LibraryContext.SaveChanges();
                    if(s>0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
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
