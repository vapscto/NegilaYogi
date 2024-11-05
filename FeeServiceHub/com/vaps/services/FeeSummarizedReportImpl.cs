using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using System.Dynamic;
using System.IO;
using DomainModel.Model.com.vaps.admission;
using Microsoft.Extensions.Logging;

using DomainModel.Model.com.vaps.Fee;
using CommonLibrary;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeSummarizedReportImpl : interfaces.FeeSummarizedReportInterface
    {

        public FeeGroupContext _FeeGroupContext;


        public FeeSummarizedReportImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }

        public FeeTransactionPaymentDTO getdetails(FeeTransactionPaymentDTO data)
        {
            FeeTransactionPaymentDTO dt = new FeeTransactionPaymentDTO();
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_ID).OrderByDescending(y=>y.ASMAY_Order).ToList();
                data.adcyear = year.GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();

                //List<FeeGroupDMO> group = new List<FeeGroupDMO>();
                //group = _FeeGroupContext.FeeGroupDMO.Where(t => t.FMG_ActiceFlag == true).ToList();
                //data.fillmastergroup = group.GroupBy(g => g.FMG_GroupName).Select(g => g.First()).ToArray();
                List<School_M_Class> classes = new List<School_M_Class>();
                classes = _FeeGroupContext.School_M_Class.Where(t => t.ASMCL_ActiveFlag == true && t.MI_Id == data.MI_ID).ToList();
                data.fillclass = classes.ToArray();

                //List<School_M_Section> section = new List<School_M_Section>();
                //section = _FeeGroupContext.school_M_Section.Where(s=>s.ASMC_ActiveFlag==1 && s.MI_Id==data.MI_ID).ToList();
                //data.fillsection = section.ToArray();


                List<FeeGroupDMO> group = new List<FeeGroupDMO>();
                group = _FeeGroupContext.FeeGroupDMO.Where(t => t.FMG_ActiceFlag == true && t.MI_Id == data.MI_ID).ToList();
                data.fillmastergroup = group.ToArray();

                data.fillmasterhead = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                       from b in _FeeGroupContext.feeGroup
                                       from c in _FeeGroupContext.feehead
                                       where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_ID && a.ASMAY_Id == data.ASMAY_Id)
                                       select new FeeStudentGroupMappingDTO
                                       {
                                           FMG_Id = b.FMG_Id,
                                           FMH_Id = c.FMH_Id,
                                           FMH_FeeName = c.FMH_FeeName
                                       }
       ).Distinct().ToArray();

                data.fillinstallment = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                        from b in _FeeGroupContext.feeGroup
                                        from c in _FeeGroupContext.feehead
                                        from d in _FeeGroupContext.FeeInstallmentDMO
                                        from e in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                        where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.FMI_Id == d.FMI_Id && d.FMI_Id == e.FMI_Id && b.MI_Id == data.MI_ID && a.ASMAY_Id == data.ASMAY_Id)
                                        select new FeeStudentGroupMappingDTO
                                        {
                                            FMG_Id = b.FMG_Id,
                                            FMH_Id = c.FMH_Id,
                                            FTI_Id = e.FTI_Id,
                                            FTI_Name = e.FTI_Name
                                        }
      ).ToArray();

                //List<FeeTermDMO> terms = new List<FeeTermDMO>();
                //terms = _FeeGroupContext.feeTr.Where(t => t.FMT_ActiveFlag == true && t.MI_Id == data.MI_ID).ToList();
                //data.fillterms = terms.ToArray();



                if (data.reporttype.Equals("T"))
                {
                    data.fillmastergroup = (from a in _FeeGroupContext.feeMTH
                                            from b in _FeeGroupContext.feeTr
                                            from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                            where (a.FMH_Id == c.FMH_Id && a.FMT_Id == b.FMT_Id && a.MI_Id == data.MI_ID && c.User_Id == data.userid) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                            select new FeeStudentTransactionDTO
                                            {
                                                FMT_Name = b.FMT_Name,
                                                FMT_Id = a.FMT_Id,
                                            }
                         ).Distinct().ToArray();

                    //List<FeeTransactionPaymentDTO> customlist = new List<FeeTransactionPaymentDTO>();

                    data.customlist = (from a in _FeeGroupContext.feegm
                                       from b in _FeeGroupContext.feeGGG
                                       from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                       where (a.FMGG_Id == b.FMGG_Id && c.FMG_ID == b.FMG_Id  && a.FMGG_ActiveFlag == true && a.MI_Id == data.MI_ID && c.User_Id == data.userid)
                                       select new FeeStudentTransactionDTO
                                       {
                                           FMGG_Id = a.FMGG_Id,
                                           FMGG_GroupName = a.FMGG_GroupName
                                       }
                         ).Distinct().ToArray();


                    // data.customlist = customlist.ToArray();
                    List<long> grpid = new List<long>();

                    foreach (FeeStudentTransactionDTO item in data.customlist)
                    {
                        grpid.Add(item.FMGG_Id);
                    }

                    data.grouplist = (from a in _FeeGroupContext.FeeGroupDMO
                                      from b in _FeeGroupContext.feeGGG
                                      from c in _FeeGroupContext.feegm
                                          //where (a.FMG_Id==b.FMG_Id.Equals(data.customlist.Equals()))
                                      where (a.FMG_Id == b.FMG_Id && b.FMGG_Id == c.FMGG_Id && grpid.Contains(c.FMGG_Id) && a.MI_Id == data.MI_ID)
                                      select new FeeStudentTransactionDTO
                                      {
                                          FMG_Id = a.FMG_Id,
                                          FMG_GroupName = a.FMG_GroupName
                                      }
                                      ).Distinct().ToArray();


                }
                else
                {
                    data.customlist = (from a in _FeeGroupContext.feegm
                                       from b in _FeeGroupContext.feeGGG
                                       from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                       where (a.FMGG_Id == b.FMGG_Id && c.FMG_ID == b.FMG_Id  && a.FMGG_ActiveFlag == true && a.MI_Id == data.MI_ID && c.User_Id == data.userid)
                                       select new FeeStudentTransactionDTO
                                       {
                                           FMGG_Id = a.FMGG_Id,
                                           FMGG_GroupName = a.FMGG_GroupName
                                       }
                     ).Distinct().ToArray();


                    // data.customlist = customlist.ToArray();
                    List<long> grpid = new List<long>();

                    foreach (FeeStudentTransactionDTO item in data.customlist)
                    {
                        grpid.Add(item.FMGG_Id);
                    }

                    data.grouplist = (from a in _FeeGroupContext.FeeGroupDMO
                                      from b in _FeeGroupContext.feeGGG
                                      from c in _FeeGroupContext.feegm
                                          //where (a.FMG_Id==b.FMG_Id.Equals(data.customlist.Equals()))
                                      where (a.FMG_Id == b.FMG_Id && b.FMGG_Id == c.FMGG_Id && grpid.Contains(c.FMGG_Id) && a.MI_Id == data.MI_ID)
                                      select new FeeStudentTransactionDTO
                                      {
                                          FMG_Id = a.FMG_Id,
                                          FMG_GroupName = a.FMG_GroupName
                                      }
                                      ).Distinct().ToArray();

                }

                //           if (data.reporttype.Equals("T"))
                //           {
                //               data.fillterms = (from a in _FeeGroupContext.feeMTH
                //                                 from b in _FeeGroupContext.feeTr
                //                                 where (a.FMT_Id == b.FMT_Id && a.MI_Id == data.MI_ID) /*&& a.fmg_id.Contains(data.fmg_id)*/
                //                                 select new FeeStudentTransactionDTO
                //                                 {
                //                                     FMG_GroupName = b.FMT_Name,
                //                                     FMG_Id = a.FMT_Id,
                //                                 }
                //                    ).Distinct().ToArray();
                //           }
                //           else
                //           {
                //               data.fillterms = (from a in _FeeGroupContext.FeeGroupDMO
                //                                 from b in _FeeGroupContext.Yearlygroups
                //                                 where (a.FMG_Id == b.FMG_Id && a.FMG_ActiceFlag == true && b.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_ID) /*&& a.fmg_id.Contains(data.fmg_id)*/
                //                                 select new FeeStudentTransactionDTO
                //                                 {
                //                                     FMG_GroupName = a.FMG_GroupName,
                //                                     FMG_Id = a.FMG_Id,
                //                                 }
                //).Distinct().ToArray();
                //           }

                data.fillmasterhead = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                      from b in _FeeGroupContext.feeGroup
                                      from c in _FeeGroupContext.feehead
                                      from d in _FeeGroupContext.Yearlygroups
                                      where (a.FMH_Id == c.FMH_Id && a.FMG_Id == d.FMG_Id && a.MI_Id == data.MI_ID && a.ASMAY_Id == data.ASMAY_Id && c.FMH_ActiveFlag == true && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && d.FMG_Id==b.FMG_Id && d.ASMAY_Id==data.ASMAY_Id)
                                      select new FeeStudentGroupMappingDTO
                                      {
                                          FMG_Id = b.FMG_Id,
                                          FMH_Id = c.FMH_Id,
                                          FMH_FeeName = c.FMH_FeeName
                                      }).Distinct().ToArray();




            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public FeeTransactionPaymentDTO getsection(FeeTransactionPaymentDTO data)
        {
            List<long> GrpId = new List<long>();
            try
            {
                List<School_M_Section> section = new List<School_M_Section>();
                data.fillsection = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                    from b in _FeeGroupContext.school_M_Section
                                    where (a.ASMS_Id == b.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && b.MI_Id == data.MI_ID)
                                    select new FeeStudentGroupMappingDTO
                                    {
                                        AMSC_Id = b.ASMS_Id,
                                        asmc_sectionname = b.ASMC_SectionName
                                    }
                          ).Distinct().OrderBy(a=>a.AMSC_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public FeeTransactionPaymentDTO getstudent(FeeTransactionPaymentDTO data)
        {
            List<long> GrpId = new List<long>();
            try
            {
                List<School_M_Section> section = new List<School_M_Section>();


                data.fillstudent = (from a in _FeeGroupContext.AdmissionStudentDMO
                                    from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                    where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.AMSC_Id && a.AMST_SOL == "S")
                                    select new FeeTransactionPaymentDTO
                                    {
                                        Amst_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_FirstName,
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                    }
         ).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<FeeTransactionPaymentDTO> getradiofiltereddata([FromBody] FeeTransactionPaymentDTO data)
        {
            List<dynamic> arraylist = new List<dynamic>();
            List<long> GrpId = new List<long>();
            var retObject = new List<dynamic>();
            FeeStudentGroupInstallmentMappingDMO fsgim = new FeeStudentGroupInstallmentMappingDMO();
            try
            {
                string fmt = "0";

             //   long fmh_id = 0;
                var fmh_ids = "";
                foreach (var x in data.FMH_Ids)
                {
                    fmh_ids += x + ",";
                }
                fmh_ids = fmh_ids.Substring(0, (fmh_ids.Length - 1));
               // fmh_id = Convert.ToInt32(fmh_ids);
                if (data.FMT_Idss != null)
                {
                    for (int j = 0; j < data.FMT_Idss.Count; j++)
                    {
                        fmt += "," + data.FMT_Idss[j].FMT_ID;

                    }
                    
                   

                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fee_SummarizedReport";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@amay_id",
                            SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(data.ASMAY_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                        SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(data.ASMCL_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@asms_id",
                           SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(data.AMSC_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@amst_id",
                           SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(data.Amst_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                         SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(data.MI_ID)
                        });
                        cmd.Parameters.Add(new SqlParameter("@fmt_id",
                           SqlDbType.VarChar)
                        {
                            Value = fmt
                        });
                        cmd.Parameters.Add(new SqlParameter("@fmgg_id",
                         SqlDbType.BigInt)
                        {
                            Value = 0
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                         SqlDbType.VarChar)
                        {
                            Value = data.stype
                        });
                        cmd.Parameters.Add(new SqlParameter("@AllInd",
                           SqlDbType.VarChar)
                        {
                            Value = data.allindi
                        });
                        cmd.Parameters.Add(new SqlParameter("@fmh_id",
                           SqlDbType.VarChar)
                        {
                            Value = fmh_ids
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                try
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
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            data.classalldata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                    foreach (var e in data.classalldata)
                    {
                        arraylist.Add(e);
                    }
                }
                else
                {
                    data.FMG_Id = 0;
                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fee_SummarizedReport";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@amay_id",
                            SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(data.ASMAY_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                        SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(data.ASMCL_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@asms_id",
                           SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(data.AMSC_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@amst_id",
                           SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(data.Amst_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                         SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(data.MI_ID)
                        });
                        cmd.Parameters.Add(new SqlParameter("@fmt_id",
                           SqlDbType.BigInt)
                        {
                            Value = data.FMG_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@fmgg_id",
                            SqlDbType.BigInt)
                        {
                            Value = 0
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                           SqlDbType.VarChar)
                        {
                            Value = data.stype
                        });
                        cmd.Parameters.Add(new SqlParameter("@AllInd",
                           SqlDbType.VarChar)
                        {
                            Value = data.allindi
                        });
                        cmd.Parameters.Add(new SqlParameter("@fmh_id",
                          SqlDbType.VarChar)
                        {
                            Value = fmh_ids
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                try
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
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            data.classalldata = retObject.ToArray();

                            foreach (var e in data.classalldata)
                            {
                                arraylist.Add(e);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                // }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            fsgim.FMSGI_Id = 0;
            data.feesummlist = arraylist.ToArray();
            try
            {
                data.accountdetails = (from a in _FeeGroupContext.master_institution
                                       from b in _FeeGroupContext.FeeBankDetailsDMO

                                       where (a.MI_Id == b.MI_Id && b.MI_Id == data.MI_ID)
                                       select new FeeTransactionPaymentDTO
                                       {
                                           MI_Name = a.MI_Name,
                                           FBD_Id = b.FBD_ID,
                                           IFSC = b.IFSC,
                                           Acc_No = b.Acc_No,
                                           Bank_Name = b.Bank_Name,
                                       }
                    ).Distinct().ToArray();
                var storageDet = _FeeGroupContext.IVRM_Storage_path_Details.ToList();

                if (data.stype.Equals("Annual"))
                {

                    ReadTemplateFromAzure obj = new ReadTemplateFromAzure();
                    string accountname = "", accesskey = "", html = "";
                    var datatstu = _FeeGroupContext.IVRM_Storage_path_Details.ToList();
                    if (datatstu.Count() > 0)
                    {
                        accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
                        accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
                    }

                    html = obj.getHtmlContentFromAzure(accountname, accesskey, "files/" + data.MI_ID, "(Annual).html", 0);

                    //ReadTemplateFromAzure obj = new ReadTemplateFromAzure();
                    //string html = obj.getHtmlContentFromAzure(storageDet.FirstOrDefault().IVRM_SD_Access_Name, storageDet.FirstOrDefault().IVRM_SD_Access_Key, "files", data.MI_ID + "(Annual).html", data.MI_ID);
                    data.htmldata = html;
                    data.miid = data.MI_ID;
                }
                else if (data.stype.Equals("Others"))
                {
                    ReadTemplateFromAzure obj = new ReadTemplateFromAzure();
                    string html = obj.getHtmlContentFromAzure(storageDet.FirstOrDefault().IVRM_SD_Access_Name, storageDet.FirstOrDefault().IVRM_SD_Access_Key, "files", data.MI_ID + "(Groupwise).html", data.MI_ID);
                    data.htmldata = html;
                    data.miid = data.MI_ID;
                    // data.ftp_remarks = "Malda";
                }
                //else
                //{
                //    if (data.stype.Equals("Annual"))
                //    {
                //        ReadTemplateFromAzure obj = new ReadTemplateFromAzure();
                //        string html = obj.getHtmlContentFromAzure(storageDet.FirstOrDefault().IVRM_SD_Access_Name, storageDet.FirstOrDefault().IVRM_SD_Access_Key, "files", "5(Annual).html", data.MI_ID);
                //        data.htmldata = html;
                //        data.ftp_remarks = "Baldwins";
                //    }
                //    else
                //    {
                //        ReadTemplateFromAzure obj = new ReadTemplateFromAzure();

                //        string html = obj.getHtmlContentFromAzure(storageDet.FirstOrDefault().IVRM_SD_Access_Name, storageDet.FirstOrDefault().IVRM_SD_Access_Key, "files", "5(Groupwise).html", data.MI_ID);
                //        data.htmldata = html;
                //        data.ftp_remarks = "Baldwins";
                //    }

                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #region feesummrdreport
            //if (data.FMT_ID != null)
            //    {
            //    int T = 0;
            //    FeeTransactionPaymentDTO pmm = new FeeTransactionPaymentDTO();
            //    var retObject = new List<dynamic>();
            //    //  FeeStudentGroupMappingDMO pmm = new FeeStudentGroupMappingDMO();
            //    FeeStudentGroupInstallmentMappingDMO fsgim = new FeeStudentGroupInstallmentMappingDMO();
            //        while (T < data.temparrayinst1.Count())
            //       {                     
            //                pmm.FMG_Id = data.temparrayinst1[T].FMG_Id;
            //                                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
            //                                {
            //                                    cmd.CommandText = "Fee_Summarized_Report";
            //                                    cmd.CommandType = CommandType.StoredProcedure;
            //                                    cmd.Parameters.Add(new SqlParameter("@amay_id",
            //                                        SqlDbType.BigInt)
            //                                    {
            //                                        Value = Convert.ToInt64(data.ASMAY_Id)
            //                                    });
            //                                    cmd.Parameters.Add(new SqlParameter("@asmcl_id",
            //                                       SqlDbType.BigInt)
            //                                    {
            //                                        Value = Convert.ToInt64(data.ASMCL_Id)
            //                                    });

            //                                    cmd.Parameters.Add(new SqlParameter("@amcl_id",
            //                                   SqlDbType.BigInt)
            //                                    {
            //                                        Value = Convert.ToInt64(data.AMSC_Id)
            //                                    });
            //                                    cmd.Parameters.Add(new SqlParameter("@amst_id",
            //                                       SqlDbType.BigInt)
            //                                    {
            //                                        Value = Convert.ToInt64(data.Amst_Id)
            //                                    });
            //                                    cmd.Parameters.Add(new SqlParameter("@fmt_id",
            //                                  SqlDbType.BigInt)
            //                                    {
            //                                        Value = Convert.ToInt64(data.temparrayinst1[T].FMG_Id)
            //                                    });

            //                                    cmd.Parameters.Add(new SqlParameter("@mi_id",
            //                               SqlDbType.BigInt)
            //                                    {
            //                                        Value = Convert.ToInt64(data.MI_ID)
            //                                    });
            //            cmd.Parameters.Add(new SqlParameter("@User_Id",
            //                             SqlDbType.BigInt)
            //            {
            //                Value = Convert.ToInt64(data.userid)
            //            });

            //            if (cmd.Connection.State != ConnectionState.Open)
            //                                        cmd.Connection.Open();

            //                                   // var retObject = new List<dynamic>();

            //                                    try
            //                                    {

            //                                        using (var dataReader = await cmd.ExecuteReaderAsync())
            //                                        {
            //                                            while (await dataReader.ReadAsync())
            //                                            {
            //                                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
            //                                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
            //                                                {
            //                                                    dataRow.Add(
            //                                                        dataReader.GetName(iFiled),
            //                                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
            //                                                    );
            //                                                }

            //                                                retObject.Add((ExpandoObject)dataRow);
            //                                            }

            //                                        }

            //                                        data.classalldata = retObject.ToArray();
            //                                    }
            //                                    catch (Exception ex)
            //                                    {
            //                                        Console.WriteLine(ex.Message);
            //                                    }
            //                                    fsgim.FMSGI_Id = 0;
            //                                }                                                                                                                                                                                                
            //                T++;                        
            //        }

            //        try
            //        {
            //            data.accountdetails = (from a in _FeeGroupContext.master_institution
            //                                   from b in _FeeGroupContext.FeeBankDetailsDMO

            //                                   where (a.MI_Id == b.MI_Id && b.MI_Id == data.MI_ID)
            //                                   select new FeeTransactionPaymentDTO
            //                                   {
            //                                       MI_Name=a.MI_Name,
            //                                       FBD_Id = b.FBD_ID,
            //                                       IFSC = b.IFSC,
            //                                       Acc_No = b.Acc_No,
            //                                       Bank_Name = b.Bank_Name,
            //                                   }
            //                ).Distinct().ToArray();

            //        if (data.MI_ID == 15)
            //        {
            //            ReadTemplateFromAzure obj = new ReadTemplateFromAzure();
            //            string html = obj.getHtmlContentFromAzure("hutchingspreadmission", "so0eBdMVog1Gk7Av3+Df//ZqwM/HwSv7JE8Zh97UF6CmXw4op6OgUVZTI4lXdZHow6KGr1UTz/+ilLfSO0A+jw==", "files", "test.html", data.MI_ID);
            //            data.htmldata = html;
            //            data.ftp_remarks = "Malda";
            //        }
            //        else
            //        {
            //            ReadTemplateFromAzure obj = new ReadTemplateFromAzure();
            //            string html = obj.getHtmlContentFromAzure("bdcampusstrg", "2GbpRyxTMjVYZc0wnKLbpgCAPYRrdX3HPUE6kcYLmk19vkq8ErTC1eYIMl1oFMhzihqlq3j0eqWmiOGt1sfZ5w==", "files/BaldwinChallanTemplates", "baldwins.html", data.MI_ID);
            //            data.htmldata = html;
            //            data.ftp_remarks = "Baldwins";
            //        }
            //    }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine(ex.Message);
            //        }
            //    }

            //    else
            //    {
            //        Console.WriteLine("Please select all the fee details");
            //    }
            #endregion

            return data;
        }

        public FeeTransactionPaymentDTO get_groups(FeeTransactionPaymentDTO data)
        {
            try
            {
                //var fmggIds = data.FMGG_Ids.Select(d=>d.FMGG_Id);

                if (data.reporttype.Equals("T"))
                {
                    data.grouplist = (from a in _FeeGroupContext.FeeGroupDMO
                                      from b in _FeeGroupContext.feeGGG
                                      from c in _FeeGroupContext.feegm
                                          //where (a.FMG_Id==b.FMG_Id.Equals(data.customlist.Equals()))
                                      where (a.FMG_Id == b.FMG_Id && b.FMGG_Id == c.FMGG_Id && data.FMGG_Ids.Contains(c.FMGG_Id) && a.MI_Id == data.MI_ID)
                                      select new FeeStudentTransactionDTO
                                      {
                                          FMG_Id = a.FMG_Id,
                                          FMG_GroupName = a.FMG_GroupName
                                      }
                                      ).Distinct().ToArray();
                }
                else
                {
                    data.grouplist = (from a in _FeeGroupContext.FeeGroupDMO
                                      from b in _FeeGroupContext.feeGGG
                                      from c in _FeeGroupContext.feegm
                                          //where (a.FMG_Id==b.FMG_Id.Equals(data.customlist.Equals()))
                                      where (a.FMG_Id == b.FMG_Id && b.FMGG_Id == c.FMGG_Id && data.FMGG_Ids.Contains(c.FMGG_Id) && a.MI_Id == data.MI_ID)
                                      select new FeeStudentTransactionDTO
                                      {
                                          FMG_Id = a.FMG_Id,
                                          FMG_GroupName = a.FMG_GroupName
                                      }
                                      ).Distinct().ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        //public async Task<FeeTransactionPaymentDTO> getradiofiltereddata([FromBody] FeeTransactionPaymentDTO data)
        //{

        //    if (data.FMT_ID != null)
        //        {
        //        int T = 0;
        //        FeeTransactionPaymentDTO pmm = new FeeTransactionPaymentDTO();
        //        var retObject = new List<dynamic>();
        //        //  FeeStudentGroupMappingDMO pmm = new FeeStudentGroupMappingDMO();
        //        FeeStudentGroupInstallmentMappingDMO fsgim = new FeeStudentGroupInstallmentMappingDMO();
        //            while (T < data.temparrayinst1.Count())
        //            {                     
        //                    pmm.FMG_Id = data.temparrayinst1[T].FMG_Id;
        //                                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
        //                                    {
        //                                        cmd.CommandText = "Fee_Summarized_Report";
        //                                        cmd.CommandType = CommandType.StoredProcedure;
        //                                        cmd.Parameters.Add(new SqlParameter("@amay_id",
        //                                            SqlDbType.BigInt)
        //                                        {
        //                                            Value = Convert.ToInt64(data.ASMAY_Id)
        //                                        });
        //                                        cmd.Parameters.Add(new SqlParameter("@asmcl_id",
        //                                           SqlDbType.BigInt)
        //                                        {
        //                                            Value = Convert.ToInt64(data.ASMCL_Id)
        //                                        });

        //                                        cmd.Parameters.Add(new SqlParameter("@amcl_id",
        //                                       SqlDbType.BigInt)
        //                                        {
        //                                            Value = Convert.ToInt64(data.AMSC_Id)
        //                                        });
        //                                        cmd.Parameters.Add(new SqlParameter("@amst_id",
        //                                           SqlDbType.BigInt)
        //                                        {
        //                                            Value = Convert.ToInt64(data.Amst_Id)
        //                                        });
        //                                        cmd.Parameters.Add(new SqlParameter("@fmt_id",
        //                                      SqlDbType.BigInt)
        //                                        {
        //                                            Value = Convert.ToInt64(data.temparrayinst1[T].FMG_Id)
        //                                        });

        //                                        cmd.Parameters.Add(new SqlParameter("@mi_id",
        //                                   SqlDbType.BigInt)
        //                                        {
        //                                            Value = Convert.ToInt64(data.MI_ID)
        //                                        });
        //                cmd.Parameters.Add(new SqlParameter("@User_Id",
        //                                 SqlDbType.BigInt)
        //                {
        //                    Value = Convert.ToInt64(data.userid)
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                                            cmd.Connection.Open();

        //                                       // var retObject = new List<dynamic>();

        //                                        try
        //                                        {

        //                                            using (var dataReader = await cmd.ExecuteReaderAsync())
        //                                            {
        //                                                while (await dataReader.ReadAsync())
        //                                                {
        //                                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                                    {
        //                                                        dataRow.Add(
        //                                                            dataReader.GetName(iFiled),
        //                                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
        //                                                        );
        //                                                    }

        //                                                    retObject.Add((ExpandoObject)dataRow);
        //                                                }

        //                                            }

        //                                            data.classalldata = retObject.ToArray();
        //                                        }
        //                                        catch (Exception ex)
        //                                        {
        //                                            Console.WriteLine(ex.Message);
        //                                        }
        //                                        fsgim.FMSGI_Id = 0;
        //                                    }                                                                                                                                                                                                 
        //                    T++;                        
        //            }

        //            try
        //            {
        //                data.accountdetails = (from a in _FeeGroupContext.master_institution
        //                                       from b in _FeeGroupContext.FeeBankDetailsDMO

        //                                       where (a.MI_Id == b.MI_Id && b.MI_Id == data.MI_ID)
        //                                       select new FeeTransactionPaymentDTO
        //                                       {
        //                                           MI_Name=a.MI_Name,
        //                                           FBD_Id = b.FBD_ID,
        //                                           IFSC = b.IFSC,
        //                                           Acc_No = b.Acc_No,
        //                                           Bank_Name = b.Bank_Name,
        //                                       }
        //                    ).Distinct().ToArray();

        //            if (data.MI_ID == 15)
        //            {
        //                ReadTemplateFromAzure obj = new ReadTemplateFromAzure();
        //                string html = obj.getHtmlContentFromAzure("hutchingspreadmission", "so0eBdMVog1Gk7Av3+Df//ZqwM/HwSv7JE8Zh97UF6CmXw4op6OgUVZTI4lXdZHow6KGr1UTz/+ilLfSO0A+jw==", "files", "test.html", data.MI_ID);
        //                data.htmldata = html;
        //                data.ftp_remarks = "Malda";
        //            }
        //            else
        //            {
        //                ReadTemplateFromAzure obj = new ReadTemplateFromAzure();
        //                string html = obj.getHtmlContentFromAzure("bdcampusstrg", "2GbpRyxTMjVYZc0wnKLbpgCAPYRrdX3HPUE6kcYLmk19vkq8ErTC1eYIMl1oFMhzihqlq3j0eqWmiOGt1sfZ5w==", "files/BaldwinChallanTemplates", "baldwins.html", data.MI_ID);
        //                data.htmldata = html;
        //                data.ftp_remarks = "Baldwins";
        //            }
        //        }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //            }
        //        }

        //        else
        //        {
        //            Console.WriteLine("Please select all the fee details");
        //        }



        //    return data;
        //}
    }
}

