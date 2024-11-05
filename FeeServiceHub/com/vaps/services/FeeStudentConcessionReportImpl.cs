using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using FeeServiceHub.com.vaps.interfaces;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vaps.admission;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DomainModel.Model.com.vaps.Fee;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeStudentConcessionReportImpl : interfaces.FeeStudentConcessionReportInterface
    {


        public FeeGroupContext _FeeGroupContext;
        //   public AdmissionFormContext _AdmissionFormContext;
        public FeeStudentConcessionReportImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
            //   _AdmissionFormContext = _admc;
        }

        public StudentConcesstionDTO getdata123(StudentConcesstionDTO data)
        {

            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();

                year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();

                data.acayear = year.ToArray();

                //List<FeeGroupDMO> allpages = new List<FeeGroupDMO>();
                //allpages = _FeeGroupContext.feeGroup.Where(t => t.MI_Id == data.MI_Id).ToList();
                //data.fgrp = allpages.ToArray();

                List<School_M_Class> allclas = new List<School_M_Class>();
                allclas = _FeeGroupContext.admissioncls.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                data.classlist = allclas.ToArray();

                List<School_M_Section> allsetion = new List<School_M_Section>();
                allsetion = _FeeGroupContext.school_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToList();
                data.sectionlist = allsetion.ToArray();

                List<Fee_Master_ConcessionDMO> Concatergory = new List<Fee_Master_ConcessionDMO>();
                Concatergory = _FeeGroupContext.catergory.Where(t => t.MI_Id == data.MI_Id && t.FMCC_ActiveFlag == true).ToList();
                data.concategory = Concatergory.ToArray();


                if (data.reporttype.Equals("T"))
                {
                    data.fillmastergroup = (from a in _FeeGroupContext.feeMTH
                                            from b in _FeeGroupContext.feeTr
                                            where (a.FMT_Id == b.FMT_Id && a.MI_Id == data.MI_Id) /*&& a.fmg_id.Contains(data.fmg_id)*/
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
                                       where (a.FMGG_Id == b.FMGG_Id && c.FMG_ID == b.FMG_Id && c.User_Id == data.userid && a.FMGG_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                       select new FeeStudentTransactionDTO
                                       {
                                           FMGG_Id = a.FMGG_Id,
                                           fmg_groupname = a.FMGG_GroupName
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
                                      where (a.FMG_Id == b.FMG_Id && b.FMGG_Id == c.FMGG_Id && grpid.Contains(c.FMGG_Id) && a.MI_Id == data.MI_Id)
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
                                       where (a.FMGG_Id == b.FMGG_Id && c.FMG_ID == b.FMG_Id && c.User_Id == data.userid && a.FMGG_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                       select new FeeStudentTransactionDTO
                                       {
                                           FMGG_Id = a.FMGG_Id,
                                           fmg_groupname = a.FMGG_GroupName
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
                                      where (a.FMG_Id == b.FMG_Id && b.FMGG_Id == c.FMGG_Id && grpid.Contains(c.FMGG_Id) && a.MI_Id == data.MI_Id)
                                      select new FeeStudentTransactionDTO
                                      {
                                          FMG_Id = a.FMG_Id,
                                          FMG_GroupName = a.FMG_GroupName
                                      }
                                      ).Distinct().ToArray();

                }





            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public StudentConcesstionDTO getreport(StudentConcesstionDTO data)
        {
            string grpidsget = "";
            try
            {
                if (data.typeofrpt == "HeadWise")
                {
                    string fmccid = "0";
                    if (data.FMCC_Id == 0)
                    {
                        var Concatergory = _FeeGroupContext.catergory.Where(t => t.MI_Id == data.MI_Id && t.FMCC_ActiveFlag == true).ToList();
                        foreach (var item in Concatergory)
                        {
                            fmccid = fmccid + "," + item.FMCC_Id;
                        }
                        
                    }
                    else
                    {
                        fmccid = data.FMCC_Id.ToString();
                    }

                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HeadwiseConcession_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 300000;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.asmyid
                        });

                        cmd.Parameters.Add(new SqlParameter("@FMCC_Id",
                           SqlDbType.VarChar)
                        {
                            Value = fmccid
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
                            data.reportdatelist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    var fmg_ids = "";
                    foreach (var x in data.FMG_Ids)
                    {
                        fmg_ids += x + ",";
                    }
                    fmg_ids = fmg_ids.Substring(0, (fmg_ids.Length - 1));

                    var fmt_ids = "";
                    if (data.term_group == "T")
                    {
                        foreach (var x in data.FMT_Ids)
                        {
                            fmt_ids += x + ",";
                        }
                        fmt_ids = fmt_ids.Substring(0, (fmt_ids.Length - 1));
                    }

                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {



                        cmd.CommandText = "Fee_Student_Concession_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 300000;
                        cmd.Parameters.Add(new SqlParameter("@mid",
                            SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ayamYear",
                           SqlDbType.VarChar)
                        {
                            Value = data.asmyid
                        });

                        cmd.Parameters.Add(new SqlParameter("@groupids",
                           SqlDbType.VarChar)
                        {
                            Value = fmg_ids
                        });
                        cmd.Parameters.Add(new SqlParameter("@termids",
                         SqlDbType.VarChar)
                        {
                            Value = fmt_ids
                        });

                        cmd.Parameters.Add(new SqlParameter("@class", SqlDbType.VarChar)
                        {
                            Value = data.clsid
                        });
                        cmd.Parameters.Add(new SqlParameter("@section", SqlDbType.VarChar)
                        {
                            Value = data.setid
                        });
                        cmd.Parameters.Add(new SqlParameter("@conditionFlag",
                                       SqlDbType.VarChar)
                        {
                            Value = data.typeofrpt
                        });

                        cmd.Parameters.Add(new SqlParameter("@type",
                                      SqlDbType.VarChar)
                        {
                            Value = data.term_group
                        });


                        cmd.Parameters.Add(new SqlParameter("@concessiontype",
                                      SqlDbType.VarChar)
                        {
                            Value = data.FMCC_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@report",
                                     SqlDbType.VarChar)
                        {
                            Value = data.report
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        //var data = cmd.ExecuteNonQuery();
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
                            data.reportdatelist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }

                return data;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public FeeStudentTransactionDTO get_groups(FeeStudentTransactionDTO data)
        {
            try
            {
                if (data.reporttype.Equals("T"))
                {


                    data.grouplist = (from a in _FeeGroupContext.FeeGroupDMO
                                      from b in _FeeGroupContext.feeGGG
                                      from c in _FeeGroupContext.feegm
                                      where (a.FMG_Id == b.FMG_Id && b.FMGG_Id == c.FMGG_Id && data.FMGG_Ids.Contains(c.FMGG_Id) && a.MI_Id == data.MI_Id)
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
                                      where (a.FMG_Id == b.FMG_Id && b.FMGG_Id == c.FMGG_Id && data.FMGG_Ids.Contains(c.FMGG_Id) && a.MI_Id == data.MI_Id)
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
    }
}
