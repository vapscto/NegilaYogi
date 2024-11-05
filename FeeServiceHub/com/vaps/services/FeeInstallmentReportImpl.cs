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
using DomainModel.Model.com.vapstech.Transport;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeInstallmentReportImpl : interfaces.FeeInstallmentReportInterface
    {
        public FeeGroupContext _FeeGroupContext;
        string coloumns1 = "";
        public FeeInstallmentReportImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }

        public FeeTransactionPaymentDTO getdetails(FeeTransactionPaymentDTO data)
        {

            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_ID).OrderByDescending(y => y.ASMAY_Order).ToList();
                data.adcyear = year.GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();



                List<FeeClassCategoryDMO> category = new List<FeeClassCategoryDMO>();
                category = _FeeGroupContext.feeCC.Where(t => t.FMCC_ActiveFlag == true && t.MI_Id == data.MI_ID).ToList();
                data.fillcategory = category.GroupBy(g => g.FMCC_ClassCategoryName).Select(c => c.First()).ToArray();

                List<MasterRouteDMO> installment = new List<MasterRouteDMO>();
                installment = _FeeGroupContext.MasterRouteDMO.Where(i => i.MI_Id == data.MI_ID).OrderBy(t => t.TRMR_order).ToList();
                data.groupalldata = installment.ToArray();


                data.fillinstallment = (from a in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                        from b in _FeeGroupContext.FeeInstallmentDMO
                                        from c in _FeeGroupContext.FeeAmountEntryDMO
                                        from d in _FeeGroupContext.FeeGroupDMO
                                        where (a.FMI_Id == b.FMI_Id && b.MI_Id == data.MI_ID && b.FMI_ActiceFlag == true && a.FTI_Id == c.FTI_Id && c.FMG_Id == d.FMG_Id && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_ID)

                                        select new FeeTransactionPaymentDTO
                                        {
                                            FTI_Id = a.FTI_Id,
                                            FTI_Name = a.FTI_Name,

                                        }
                    ).Distinct().OrderBy(t => t.FTI_Id).ToArray();



                List<School_M_Class> classes = new List<School_M_Class>();
                classes = _FeeGroupContext.School_M_Class.Where(t => t.ASMCL_ActiveFlag == true && t.MI_Id == data.MI_ID).ToList();
                data.fillclass = classes.GroupBy(c => c.ASMCL_ClassName).Select(c => c.First()).ToArray();



                data.fillmastergroup = (from a in _FeeGroupContext.feeGroup
                                        from b in _FeeGroupContext.Yearlygroups
                                        where (a.FMG_Id == b.FMG_Id && b.MI_Id == data.MI_ID && a.FMG_ActiceFlag == true)
                                        select new FeeTransactionPaymentDTO
                                        {
                                            FMG_Id = a.FMG_Id,
                                            fmg_groupname = a.FMG_GroupName,

                                        }
                   ).Distinct().OrderBy(t => t.FTI_Id).ToArray();


                var terms = _FeeGroupContext.feeTr.Where(a => a.MI_Id == data.MI_ID && a.FMT_ActiveFlag == true).OrderBy(a => a.FMT_Order).Distinct().ToArray();
                if (terms.Length > 0)
                {
                    data.termslist = terms;
                }

                data.fillgroup = (from a in _FeeGroupContext.feeGroup
                                  from b in _FeeGroupContext.Yearlygroups
                                  where (a.FMG_Id == b.FMG_Id && b.MI_Id == data.MI_ID && a.FMG_ActiceFlag == true && b.ASMAY_Id == data.ASMAY_Id)
                                  select new FeeTransactionPaymentDTO
                                  {
                                      FMG_Id = a.FMG_Id,
                                      fmg_groupname = a.FMG_GroupName,

                                  }
                 ).Distinct().OrderBy(t => t.FTI_Id).ToArray();



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }



        public FeeTransactionPaymentDTO getinstallmentid(FeeTransactionPaymentDTO data)
        {
            List<long> GrpId = new List<long>();
            try
            {

                //data.fillinstallment = (from a in _FeeGroupContext.FeeInstallmentsyearlyDMO
                //                        from b in _FeeGroupContext.FeeStudentTransactionDMO
                //                        where (a.FTI_Id == b.FTI_Id && b.ASMAY_Id == data.ASMAY_Id)

                //                        select new FeeTransactionPaymentDTO
                //                        {
                //                            FTI_Id = a.FTI_Id,
                //                            FTI_Name = a.FTI_Name,

                //                        }
                //).Distinct().OrderBy(t=>t.FTI_Id).ToArray();


                data.fillinstallment = (from a in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                        from b in _FeeGroupContext.FeeInstallmentDMO
                                        from c in _FeeGroupContext.FeeAmountEntryDMO
                                        from d in _FeeGroupContext.FeeGroupDMO
                                        where (a.FMI_Id == b.FMI_Id && b.MI_Id == data.MI_ID && b.FMI_ActiceFlag == true && a.FTI_Id == c.FTI_Id && c.FMG_Id == d.FMG_Id && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_ID)

                                        select new FeeTransactionPaymentDTO
                                        {
                                            FTI_Id = a.FTI_Id,
                                            FTI_Name = a.FTI_Name,

                                        }
                   ).Distinct().OrderBy(t => t.FTI_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeeTransactionPaymentDTO getinstallmentid1(FeeTransactionPaymentDTO data)
        {
            List<long> GrpId = new List<long>();
            try
            {
                foreach (var item in data.temparrayinst)
                {
                    GrpId.Add(item.FTI_Id);
                }

                //   data.fillinstallment = (from a in _FeeGroupContext.FeeInstallmentsyearlyDMO
                //                           from b in _FeeGroupContext.FeeStudentTransactionDMO
                //                           where (a.FTI_Id == b.FTI_Id && b.ASMAY_Id == data.ASMAY_Id && GrpId.Contains(a.FTI_Id))

                //                           select new FeeTransactionPaymentDTO
                //                           {
                //                               FTI_Id = a.FTI_Id,
                //                               FTI_Name = a.FTI_Name,
                //                           }
                //).Distinct().OrderBy(t=>t.FTI_Id).ToArray();

                data.fillinstallment = (from a in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                        from b in _FeeGroupContext.FeeInstallmentDMO
                                        from c in _FeeGroupContext.FeeAmountEntryDMO
                                        from d in _FeeGroupContext.FeeGroupDMO
                                        where (a.FMI_Id == b.FMI_Id && b.MI_Id == data.MI_ID && b.FMI_ActiceFlag == true && a.FTI_Id == c.FTI_Id && c.FMG_Id == d.FMG_Id && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_ID && GrpId.Contains(a.FTI_Id))

                                        select new FeeTransactionPaymentDTO
                                        {
                                            FTI_Id = a.FTI_Id,
                                            FTI_Name = a.FTI_Name,

                                        }
                   ).Distinct().OrderBy(t => t.FTI_Id).ToArray();




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeTransactionPaymentDTO classes(FeeTransactionPaymentDTO data)
        {
            List<long> GrpId = new List<long>();
            try
            {


                //   data.fillinstallment = (from a in _FeeGroupContext.feeMIY
                //                           from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                //                           from c in _FeeGroupContext.FeeStudentTransactionDMO
                //                           where (a.FTI_Id == c.FTI_Id && c.AMST_Id == b.AMST_Id && b.ASMCL_Id==data.ASMCL_Id && c.MI_Id == data.MI_ID && c.ASMAY_Id == data.ASMAY_Id)

                //                           select new FeeTransactionPaymentDTO
                //                           {
                //                               FTI_Id = a.FTI_Id,
                //                               FTI_Name = a.FTI_Name
                //                           }
                //).Distinct().OrderBy(t => t.FTI_Id).ToArray();



                data.fillinstallment = (from a in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                        from b in _FeeGroupContext.FeeInstallmentDMO
                                        from c in _FeeGroupContext.FeeAmountEntryDMO
                                        from d in _FeeGroupContext.FeeGroupDMO
                                        from e in _FeeGroupContext.feeYCC
                                        from f in _FeeGroupContext.feeYCCC
                                        where (a.FMI_Id == b.FMI_Id && b.MI_Id == data.MI_ID && b.FMI_ActiceFlag == true && a.FTI_Id == c.FTI_Id && c.FMG_Id == d.FMG_Id && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_ID && e.FMCC_Id == c.FMCC_Id && e.FYCC_Id == f.FYCC_Id)
                                        //&& f.ASMCL_Id == data.ASMCL_Id

                                        select new FeeTransactionPaymentDTO
                                        {
                                            FTI_Id = a.FTI_Id,
                                            FTI_Name = a.FTI_Name,

                                        }
                 ).Distinct().OrderBy(t => t.FTI_Id).ToArray();







                data.fillsection = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                    from b in _FeeGroupContext.school_M_Section
                                    where (a.ASMS_Id == b.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && b.MI_Id == data.MI_ID)
                                    select new FeeStudentGroupMappingDTO
                                    {
                                        AMSC_Id = b.ASMS_Id,
                                        asmc_sectionname = b.ASMC_SectionName,

                                    }
                         ).Distinct().OrderBy(t => t.AMSC_Id).ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeTransactionPaymentDTO group(FeeTransactionPaymentDTO data)
        {
            List<long> GrpId = new List<long>();
            try
            {


                //   data.fillinstallment = (from a in _FeeGroupContext.feeGroup
                //                           from b in _FeeGroupContext.feeMIY
                //                           from c in _FeeGroupContext.FeeStudentTransactionDMO
                //                           where (b.FTI_Id==c.FTI_Id && c.FMG_Id==a.FMG_Id && a.FMG_Id==data.FMG_Id && c.MI_Id==data.MI_ID && c.ASMAY_Id==data.ASMAY_Id)

                //                           select new FeeTransactionPaymentDTO
                //                           {
                //                               FTI_Id = b.FTI_Id,
                //                               FTI_Name=b.FTI_Name
                //                           }
                //).Distinct().ToArray();



                if (data.FMG_Id == 0)
                {

                    data.fillinstallment = (from a in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                            from b in _FeeGroupContext.FeeInstallmentDMO
                                            from c in _FeeGroupContext.FeeAmountEntryDMO
                                            from d in _FeeGroupContext.FeeGroupDMO
                                            where (a.FMI_Id == b.FMI_Id && b.MI_Id == data.MI_ID && b.FMI_ActiceFlag == true && a.FTI_Id == c.FTI_Id && c.FMG_Id == d.FMG_Id && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_ID)

                                            select new FeeTransactionPaymentDTO
                                            {
                                                FTI_Id = a.FTI_Id,
                                                FTI_Name = a.FTI_Name,

                                            }
                       ).Distinct().OrderBy(t => t.FTI_Id).ToArray();



                    data.fillmasterhead = (from c in _FeeGroupContext.FeeAmountEntryDMO
                                           from d in _FeeGroupContext.FeeGroupDMO
                                           from e in _FeeGroupContext.FeeHeadDMO
                                           where (c.MI_Id == data.MI_ID && e.FMH_ActiveFlag == true && c.FMG_Id == d.FMG_Id && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_ID && c.FMH_Id == e.FMH_Id)

                                           select new FeeTransactionPaymentDTO
                                           {
                                               FMH_Id = e.FMH_Id,
                                               FMH_FeeName = e.FMH_FeeName,

                                           }
                     ).Distinct().OrderBy(t => t.FMH_Id).ToArray();
                }
                else
                {
                    data.fillinstallment = (from a in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                            from b in _FeeGroupContext.FeeInstallmentDMO
                                            from c in _FeeGroupContext.FeeAmountEntryDMO
                                            from d in _FeeGroupContext.FeeGroupDMO
                                            where (a.FMI_Id == b.FMI_Id && b.MI_Id == data.MI_ID && b.FMI_ActiceFlag == true && a.FTI_Id == c.FTI_Id && c.FMG_Id == d.FMG_Id && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_ID && c.FMG_Id == data.FMG_Id)

                                            select new FeeTransactionPaymentDTO
                                            {
                                                FTI_Id = a.FTI_Id,
                                                FTI_Name = a.FTI_Name,

                                            }
                     ).Distinct().OrderBy(t => t.FTI_Id).ToArray();



                    data.fillmasterhead = (from c in _FeeGroupContext.FeeAmountEntryDMO
                                           from d in _FeeGroupContext.FeeGroupDMO
                                           from e in _FeeGroupContext.FeeHeadDMO
                                           where (c.MI_Id == data.MI_ID && e.FMH_ActiveFlag == true && c.FMG_Id == d.FMG_Id && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_ID && c.FMG_Id == data.FMG_Id && c.FMH_Id == e.FMH_Id)

                                           select new FeeTransactionPaymentDTO
                                           {
                                               FMH_Id = e.FMH_Id,
                                               FMH_FeeName = e.FMH_FeeName,

                                           }
                     ).Distinct().OrderBy(t => t.FMH_Id).ToArray();
                }
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<FeeTransactionPaymentDTO> radiobtndata(FeeTransactionPaymentDTO temp)
        {
            try
            {
                string fmtd = "0";
                string fmgd = "0";
                if (temp.termslistarray != null)
                {
                    foreach (var item in temp.termslistarray)
                    {
                        fmtd = fmtd + ',' + item.FMT_Id;
                    }
                }
                if (temp.fillgrouparray != null)
                {
                    foreach (var item1 in temp.fillgrouparray)
                    {
                        fmgd = fmgd + ',' + item1.FMG_Id;
                    }
                }
                if (temp.reporttype != "GroupWise")
                {
                    if (temp.termswise == 1)
                    {


                        using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Fee_termwise_statistics";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 300000;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                         SqlDbType.VarChar)
                            {
                                Value = temp.MI_ID
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                               SqlDbType.VarChar)
                            {
                                Value = temp.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                              SqlDbType.VarChar)
                            {
                                Value = temp.ASMCL_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                              SqlDbType.VarChar)
                            {
                                Value = temp.ASMS_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                        SqlDbType.VarChar)
                            {
                                Value = fmgd
                            });
                            cmd.Parameters.Add(new SqlParameter("@FMT_Id",
                       SqlDbType.VarChar)
                            {
                                Value = fmtd
                            });
                            cmd.Parameters.Add(new SqlParameter("@FMCC_Id",
                     SqlDbType.VarChar)
                            {
                                Value = temp.FMCC_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@user_id",
                        SqlDbType.VarChar)
                            {
                                Value = temp.userid
                            });
                            cmd.Parameters.Add(new SqlParameter("@active",
                        SqlDbType.VarChar)
                            {
                                Value = temp.active
                            });
                            cmd.Parameters.Add(new SqlParameter("@deactive",
                        SqlDbType.VarChar)
                            {
                                Value = temp.deactive
                            });
                            cmd.Parameters.Add(new SqlParameter("@left",
                       SqlDbType.VarChar)
                            {
                                Value = temp.left
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
                                temp.classwisedata = retObject.ToArray();

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    else if (temp.reporttype == "Consolidate")
                    {
                        string cls = "";
                        string sec = "";
                        if (temp.con == "Classwise")
                        {
                            cls = "0";
                            sec = "0";
                            foreach (var item in temp.classarray)
                            {
                                cls = cls + ',' + item.ASMCL_Id;
                            }
                            foreach (var item1 in temp.sectionarray)
                            {
                                sec = sec + ',' + item1.ASMS_Id;
                            }
                            temp.todate = "";
                            temp.fromdate = "";
                        }

                        using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Fee_TermWiseCollectedAmount_Details";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 300000;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                         SqlDbType.VarChar)
                            {
                                Value = temp.MI_ID
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                               SqlDbType.VarChar)
                            {
                                Value = temp.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                              SqlDbType.VarChar)
                            {
                                Value = cls
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                              SqlDbType.VarChar)
                            {
                                Value = sec
                            });

                            cmd.Parameters.Add(new SqlParameter("@FromDate",
                                SqlDbType.VarChar)
                            {
                                Value = temp.fromdate
                            });
                            cmd.Parameters.Add(new SqlParameter("@ToDate",
                             SqlDbType.VarChar)
                            {
                                Value = temp.todate
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
                                temp.classwisedata = retObject.ToArray();

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    else
                    {

                        int rdd = 0;
                        string IVRM_CLM_coloumn = "";
                        string name = "";
                        for (int i = 0; i < temp.temparrayinst1.Length; i++)
                        {
                            name = temp.temparrayinst1[i].columnID;
                            if (name != null)
                            {
                                if (rdd == 0)
                                {
                                    rdd = rdd + 1;

                                    IVRM_CLM_coloumn = name;
                                }
                                else
                                {
                                    IVRM_CLM_coloumn = IVRM_CLM_coloumn + "," + name;
                                }


                            }
                        }
                        var fmh_ids = "";
                        if (temp.reporttype == "transport")
                        {

                            foreach (var x in temp.FMGG_Ids)
                            {
                                fmh_ids += x + ",";
                            }
                            fmh_ids = fmh_ids.Substring(0, (fmh_ids.Length - 1));
                        }
                        coloumns1 = "";
                        //coloumns1 = IVRM_CLM_coloumn.Remove(IVRM_CLM_coloumn.Length - 1);
                        coloumns1 = IVRM_CLM_coloumn;

                        using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "get_installments_new";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 300000;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                         SqlDbType.VarChar)
                            {
                                Value = temp.MI_ID
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                               SqlDbType.VarChar)
                            {
                                Value = temp.ASMAY_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@FMCC_Id",
                     SqlDbType.VarChar)
                            {
                                Value = temp.FMCC_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@FTI_Id",
                            SqlDbType.VarChar)
                            {
                                Value = coloumns1
                            });

                            cmd.Parameters.Add(new SqlParameter("@asmcL_Id",
                           SqlDbType.VarChar)
                            {
                                Value = temp.ASMCL_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@amsC_Id",
                           SqlDbType.VarChar)
                            {
                                Value = temp.AMSC_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@type",
                        SqlDbType.VarChar)
                            {
                                Value = temp.reporttype
                            });

                            cmd.Parameters.Add(new SqlParameter("@fmg_id",
                          SqlDbType.VarChar)
                            {
                                Value = temp.FMG_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@trmr_Id",
                          SqlDbType.VarChar)
                            {
                                Value = temp.TRMR_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@user_id",
                        SqlDbType.VarChar)
                            {
                                Value = temp.userid
                            });
                            cmd.Parameters.Add(new SqlParameter("@fmh_ids",
                                        SqlDbType.VarChar)
                            {
                                Value = fmh_ids
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
                                temp.classwisedata = retObject.ToArray();

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "get_installments_new";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 300000;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.VarChar)
                        {
                            Value = temp.MI_ID
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                           SqlDbType.VarChar)
                        {
                            Value = temp.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@FMCC_Id",
                 SqlDbType.VarChar)
                        {
                            Value = temp.FMCC_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@FTI_Id",
                        SqlDbType.VarChar)
                        {
                            Value = coloumns1
                        });

                        cmd.Parameters.Add(new SqlParameter("@asmcL_Id",
                       SqlDbType.VarChar)
                        {
                            Value = temp.ASMCL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@amsC_Id",
                       SqlDbType.VarChar)
                        {
                            Value = temp.AMSC_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                    SqlDbType.VarChar)
                        {
                            Value = temp.reporttype
                        });

                        cmd.Parameters.Add(new SqlParameter("@fmg_id",
                      SqlDbType.VarChar)
                        {
                            Value = fmgd
                        });

                        cmd.Parameters.Add(new SqlParameter("@trmr_Id",
                      SqlDbType.VarChar)
                        {
                            Value = temp.TRMR_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@user_id",
                    SqlDbType.VarChar)
                        {
                            Value = temp.userid
                        });
                        cmd.Parameters.Add(new SqlParameter("@fmh_ids",
                                    SqlDbType.VarChar)
                        {
                            Value = ""
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
                            temp.classwisedata = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return temp;
            //List<long> ids = new List<long>();
            //foreach(var x in temp.temparrayinst1)
            //{
            //    ids.Add(Convert.ToInt64(x.columnID));
            //}

            //var list_final = (from a in _FeeGroupContext.FeeStudentTransactionDMO
            //                  from b in _FeeGroupContext.AdmissionStudentDMO
            //                  from c in _FeeGroupContext.School_Adm_Y_StudentDMO
            //                  from d in _FeeGroupContext.masterclasscategory
            //                  from e in _FeeGroupContext.School_M_Class
            //                  from f in _FeeGroupContext.school_M_Section
            //                  from g in _FeeGroupContext.FeeInstallmentsyearlyDMO
            //                  where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && d.ASMCL_Id == c.ASMCL_Id && d.AMC_Id == 4 && a.MI_Id == 5 && b.MI_Id == 5 && d.MI_Id == 5 && a.ASMAY_Id == 3 && c.ASMAY_Id == 3 && d.ASMAY_Id == 3 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && c.AMAY_ActiveFlag == 1 && e.MI_Id == 5 && e.ASMCL_Id == c.ASMCL_Id && e.ASMCL_ActiveFlag == true && f.MI_Id == 5 && f.ASMS_Id == c.ASMS_Id && f.ASMC_ActiveFlag == 1 && g.MI_ID == 5 && a.FTI_Id == g.FTI_Id && ids.Contains(a.FTI_Id))            
            //                  select new FeeTransactionPaymentDTO//&& a.FTI_Id in (23,24)
            //                  {
            //                      Amst_Id= a.AMST_Id,
            //                      AMST_FirstName = b.AMST_FirstName,
            //                      ASMCL_Id = c.ASMCL_Id,
            //                      classname = e.ASMCL_ClassName,
            //                      AMSC_Id = c.ASMS_Id,
            //                      sectionname = f.ASMC_SectionName,
            //                      FTI_Id = a.FTI_Id,
            //                      FTI_Name = g.FTI_Name,
            //                      ftp_tobepaid_amt = a.FSS_ToBePaid,
            //                      FTP_Paid_Amt = a.FSS_PaidAmount,
            //                      FTP_Concession_Amt = a.FSS_ConcessionAmount,
            //                      FTP_Waived_Amt = a.FSS_WaivedAmount,
            //                      FTP_Fine_Amt = a.FSS_FineAmount,
            //                      balance = a.FSS_NetAmount                                      
            //                  }).Distinct().ToList();

            //var final_list = (from i in list_final
            //                  group i by new { i.Amst_Id, i.AMST_FirstName, i.ASMCL_Id, i.classname, i.AMSC_Id, i.sectionname, i.FTI_Id, i.FTI_Name } into g
            //                  select new FeeTransactionPaymentDTO
            //                  {
            //                      Amst_Id = g.Key.Amst_Id,
            //                      AMST_FirstName = g.Key.AMST_FirstName,
            //                      ASMCL_Id = g.Key.ASMCL_Id,
            //                      classname = g.Key.sectionname,
            //                      AMSC_Id = g.Key.AMSC_Id,
            //                      sectionname = g.Key.sectionname,
            //                      FTI_Id = g.Key.FTI_Id,
            //                      FTI_Name = g.Key.FTI_Name,
            //                      ftp_tobepaid_amt = g.Sum(t => t.ftp_tobepaid_amt),
            //                      FTP_Paid_Amt = g.Sum(t => t.FTP_Paid_Amt),
            //                      FTP_Concession_Amt = g.Sum(t => t.FTP_Concession_Amt),
            //                      FTP_Waived_Amt = g.Sum(t => t.FTP_Waived_Amt),
            //                      FTP_Fine_Amt = g.Sum(t => t.FTP_Fine_Amt),
            //                      balance = g.Sum(t => t.balance)
            //                  }).Distinct().ToList();


            //Kiran
            //   int rdd = 0;
            //   List<long> GrpId = new List<long>();

            //   if (temp.reporttype == "Class")
            //   {
            //       temp.AMC_id = 0;
            //       temp.FTI_Id = 0;
            //       coloumns1 = "0";
            //       temp.FMG_Id = 0;


            //   }
            //   else  if (temp.reporttype == "Category")
            //   {
            //       temp.ASMCL_Id = 0;
            //       temp.AMSC_Id = 0;
            //       temp.FMG_Id = 0;
            //       string IVRM_CLM_coloumn = "";
            //       string name = "";
            //       for (int i = 0; i < temp.temparrayinst1.Length; i++)
            //       {
            //           name = temp.temparrayinst1[i].columnID;
            //           if (name != null)
            //           {
            //               if(rdd==0)
            //               {
            //                   rdd = rdd + 1;

            //                   IVRM_CLM_coloumn = name;
            //               }
            //               else
            //               {
            //                   IVRM_CLM_coloumn = IVRM_CLM_coloumn + "," + name;
            //               }


            //           }
            //       }
            //        coloumns1 = "";
            //       //coloumns1 = IVRM_CLM_coloumn.Remove(IVRM_CLM_coloumn.Length - 1);
            //       coloumns1 = IVRM_CLM_coloumn;
            //   }
            //   else
            //   {
            //       temp.AMC_id = 0;
            //       temp.FTI_Id = 0;
            //       temp.ASMCL_Id = 0;
            //       temp.AMSC_Id = 0;
            //       coloumns1 = "0";
            //       //string IVRM_CLM_coloumn = "";
            //       //string name = "";


            //       //for (int i = 0; i < temp.temparrayinst1.Length; i++)
            //       //{
            //       //    name = temp.groupid[i].columnID;
            //       //    if (name != null)
            //       //    {
            //       //        if (rdd == 0)
            //       //        {
            //       //            rdd = rdd + 1;

            //       //            IVRM_CLM_coloumn = name;
            //       //        }
            //       //        else
            //       //        {
            //       //            IVRM_CLM_coloumn = IVRM_CLM_coloumn + "," + name;
            //       //        }
            //       //    }
            //       //}
            //       //coloumns1 = "";
            //       ////coloumns1 = IVRM_CLM_coloumn.Remove(IVRM_CLM_coloumn.Length - 1);
            //       //coloumns1 = IVRM_CLM_coloumn;
            //   }

            //   using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
            //   {
            //       cmd.CommandText = "feeinstallments_report";
            //       cmd.CommandType = CommandType.StoredProcedure;
            //       cmd.CommandTimeout = 300000;

            //       cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
            //          SqlDbType.VarChar)
            //       {
            //           Value = temp.ASMAY_Id
            //       });

            //       cmd.Parameters.Add(new SqlParameter("@amc_id",
            //SqlDbType.VarChar)
            //       {
            //           Value = temp.AMC_id
            //       });

            //       cmd.Parameters.Add(new SqlParameter("@fti_id",
            //       SqlDbType.VarChar)
            //       {
            //           Value = coloumns1
            //       });
            //       cmd.Parameters.Add(new SqlParameter("@amscl_id",
            //     SqlDbType.VarChar)
            //       {
            //           Value = temp.ASMCL_Id
            //       });

            //       cmd.Parameters.Add(new SqlParameter("@amsc_id",
            //    SqlDbType.VarChar)
            //       {
            //           Value = temp.AMSC_Id
            //       });

            //       cmd.Parameters.Add(new SqlParameter("@type",
            // SqlDbType.VarChar)
            //       {
            //           Value = temp.reporttype
            //       });
            //       cmd.Parameters.Add(new SqlParameter("@groupid",
            // SqlDbType.VarChar)
            //       {
            //           Value = temp.FMG_Id

            //       });



            //       if (cmd.Connection.State != ConnectionState.Open)
            //           cmd.Connection.Open();

            //       var retObject = new List<dynamic>();

            //       try
            //       {

            //           using (var dataReader = await cmd.ExecuteReaderAsync())
            //           {
            //               while (await dataReader.ReadAsync())
            //               {
            //                   var dataRow = new ExpandoObject() as IDictionary<string, object>;

            //                   for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
            //                   {
            //                       dataRow.Add(
            //                           dataReader.GetName(iFiled),
            //                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
            //                       );
            //                   }

            //                   retObject.Add((ExpandoObject)dataRow);
            //               }
            //           }
            //           temp.classwisedata = retObject.ToArray();

            //       }
            //       catch (Exception ex)
            //       {
            //           Console.WriteLine(ex.Message);
            //       }
            //       return temp;
            //   }
            //kiran

        }



    }
}
