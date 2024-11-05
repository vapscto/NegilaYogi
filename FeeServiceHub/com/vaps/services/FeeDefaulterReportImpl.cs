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
using CommonLibrary;
using System.Net.Mail;
using SendGrid;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using DomainModel.Model.com.vapstech.Transport;
using SendGrid.Helpers.Mail;
using DomainModel.Model.com.vapstech.Fee;
using Newtonsoft.Json.Linq;
using DomainModel.Model.com.vapstech.Portals.Employee;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
//using PreadmissionDTOs.com.vaps.Fees.FeeReport;
//using DataAccessMsSqlServerProvider.com.vapstech.Fees.FeeReports;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeDefaulterReportImpl : interfaces.FeeDefaulterReportInterface
    {
        public FeeGroupContext _FeeGroupContext;
        public DomainModelMsSqlServerContext _db;
        public PortalContext _PortalContext;

        public FeeDefaulterReportImpl(FeeGroupContext frgContext, DomainModelMsSqlServerContext db , PortalContext pContext)
        {
            _FeeGroupContext = frgContext;
            _db = db;
            _PortalContext = pContext;
        }

        // public FeeTransactionPaymentDTO mas = new FeeTransactionPaymentDTO();
        public FeeTransactionPaymentDTO getdetails(FeeTransactionPaymentDTO data)
        {
            FeeTransactionPaymentDTO dt = new FeeTransactionPaymentDTO();
            try
            {

                List<FeeMasterConfigurationDMO> feemasnum = new List<FeeMasterConfigurationDMO>();
                feemasnum = _db.FeeMasterConfigurationDMO.Where(t => t.MI_Id == data.MI_ID).ToList();
                data.accountdetails = feemasnum.ToArray();


                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_ID).OrderByDescending(y => y.ASMAY_Order).ToList();
                data.adcyear = year.GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();

                //List<FeeGroupDMO> group = new List<FeeGroupDMO>();
                //group = _FeeGroupContext.FeeGroupDMO.Where(t => t.FMG_ActiceFlag == true && t.MI_Id==Mi_Id).ToList();
                //data.fillmastergroup = group.GroupBy(g => g.FMG_GroupName).Select(g => g.First()).ToArray();

                List<FeeTermDMO> terms = new List<FeeTermDMO>();
                terms = _FeeGroupContext.feeTr.Where(t => t.FMT_ActiveFlag == true && t.MI_Id == data.MI_ID).ToList();
                data.fillterms = terms.ToArray();
                //List<FeeClassCategoryDMO> classlist=new List<FeeClassCategoryDMO>();
                //classlist = _FeeGroupContext.feeCC.Where(c =>c.FMCC_ActiveFlag == true && c.MI_Id==Mi_Id).ToList();
                //data.fillclass = classlist.GroupBy(c=>c.FMCC_ClassCategoryName).Select(c=>c.First()).ToArray();


                List<School_M_Class> allclas = new List<School_M_Class>();
                allclas = _FeeGroupContext.admissioncls.Where(t => t.MI_Id == data.MI_ID && t.ASMCL_ActiveFlag == true).ToList();
                data.fillclass = allclas.ToArray();

                List<MasterRouteDMO> installments = new List<MasterRouteDMO>();
                installments = _FeeGroupContext.MasterRouteDMO.Where(i => i.TRMR_ActiveFlg == true && i.MI_Id == data.MI_ID).ToList();
                data.fillinstallment = installments.ToArray();

                data.getfeedefaultertemplate = _FeeGroupContext.sMSEmailSetting.Where(t => t.ISES_Template_Name.Contains("DEFAULT") && t.MI_Id==data.MI_ID).ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public FeeTransactionPaymentDTO getgrpterms(FeeTransactionPaymentDTO data)
        {
            try
            {
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
                                       where (a.FMGG_Id == b.FMGG_Id && c.FMG_ID == b.FMG_Id && a.FMGG_ActiveFlag == true && a.MI_Id == data.MI_ID
                                       && c.User_Id == data.userid)
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
                                       where (a.FMGG_Id == b.FMGG_Id && c.FMG_ID == b.FMG_Id && a.FMGG_ActiveFlag == true && a.MI_Id == data.MI_ID && c.User_Id == data.userid)
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
                                      where (a.FMG_Id == b.FMG_Id && b.FMGG_Id == c.FMGG_Id && grpid.Contains(c.FMGG_Id) && a.MI_Id == data.MI_ID)
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



        //FeeTransactionPaymentDTO FeeDefaulterReportInterface.getradiofiltereddata(FeeTransactionPaymentDTO temp)
        //{
        //    throw new NotImplementedException();
        //}


        public async Task<FeeTransactionPaymentDTO> getradiofiltereddata([FromBody] FeeTransactionPaymentDTO temp)
        {
            List<long> GrpId = new List<long>();

            int T = 0;
            string name = "";
            //FeeArrearRegisterReportDTO pmm = new FeeArrearRegisterReportDTO();
            //FeeStudentGroupInstallmentMappingDMO fsgim = new FeeStudentGroupInstallmentMappingDMO();
            //while (T < temp.TempararyArrayhEADListnew.Count())
            //{
            //    GrpId.Add(temp.TempararyArrayhEADListnew[T].FMT_ID);
            //    pmm.FMT_ID = temp.TempararyArrayhEADListnew[T].FMT_ID;
            //    T++;
            //}

            ////temp.headlist = GrpId.ToArray();
            //string IVRM_CLM_coloumn = "";
            //for (int i = 0; i < temp.TempararyArrayhEADListnew.Length; i++)
            //{
            //    name = temp.TempararyArrayhEADListnew[i].columnID;
            //    if (name != null)
            //    {
            //        IVRM_CLM_coloumn = name + "," + IVRM_CLM_coloumn;
            //    }
            //}

            //string coloumns = "";
            //coloumns = IVRM_CLM_coloumn.Remove(IVRM_CLM_coloumn.Length - 1);
            //List<int> list = new List<int>();
            //string[] s = coloumns.Split(',');
            //for(int i = 0; i < s.Length; i++)
            //{
            //    var n =Convert.ToInt16(s[i]);
            //    list.Add(n);
            //}
            //var maxcol = list.Max();
            //temp.FMT_ID = maxcol;

            var fmg_ids = "";
            foreach (var x in temp.FMG_Ids)
            {
                fmg_ids += x + ",";
            }
            fmg_ids = fmg_ids.Substring(0, (fmg_ids.Length - 1));


            string maxcol1 = "";
            string mincol1 = "";
            var fmt_ids = "";
            var trmrids = "0,";
            if (temp.term_group == "T")
            {
                foreach (var x in temp.FMT_Ids)
                {
                    fmt_ids += x + ",";
                }
                fmt_ids = fmt_ids.Substring(0, (fmt_ids.Length - 1));

                List<int> list1 = new List<int>();

                string[] d = fmt_ids.Split(',');
                for (int i = 0; i < d.Length; i++)
                {
                    var n = Convert.ToInt16(d[i]);
                    list1.Add(n);
                }
                maxcol1 = list1.Max().ToString();
                mincol1 = list1.Min().ToString();

                string startmonth = "";
                string endmonth = "";

                startmonth = _FeeGroupContext.feeTr.Where(a => a.FMT_Id == Convert.ToInt32(mincol1) && a.MI_Id == temp.MI_ID).ToList().FirstOrDefault().FromMonth;
                endmonth = _FeeGroupContext.feeTr.Where(f => f.FMT_Id == Convert.ToInt32(maxcol1) && f.MI_Id == temp.MI_ID).ToList().FirstOrDefault().ToMonth;

                temp.month = startmonth + '-' + endmonth;

                List<int> list = new List<int>();
                string[] s = fmt_ids.Split(',');
                for (int i = 0; i < s.Length; i++)
                {
                    var n = Convert.ToInt16(s[i]);
                    list.Add(n);
                }
                var maxcol = list.Max();
                temp.FMT_ID = maxcol;
            }
            if (temp.TRMR_Ids!=null)
            {
                foreach (var x in temp.TRMR_Ids)
                {
                    trmrids += x + ",";
                }
            }

            trmrids = trmrids.Substring(0, (trmrids.Length - 1));


            if (trmrids == "0")
            {
                temp.yearid = 0;
            }
            else
            {
                temp.yearid = 1;
            }
            var From_date = temp.From_Date.ToString();
            var To_date = temp.To_Date.ToString();


            if (temp.reporttype == "year")
            {
                temp.From_Date = Convert.ToDateTime("01 / 01 / 2017");
                //temp.To_Date = Convert.ToDateTime("01 / 01 / 2017");
                temp.duedate = "null";
            }

            temp.FBD_Id = _FeeGroupContext.AcademicYear.Single(a => a.MI_Id == temp.MI_ID && a.ASMAY_From_Date <= DateTime.Now && a.ASMAY_To_Date >= DateTime.Now).ASMAY_Id;

            if ((temp.FBD_Id == temp.ASMAY_Id) && temp.ASMCL_Id != 0)
            {
                if (temp.TRMR_Id == 0)
                {
                    using (var cmd1 = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd1.CommandText = "ClassWiseTermDueDate";
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.CommandTimeout = 90000000;
                        cmd1.Parameters.Add(new SqlParameter("@TermIds",
                         SqlDbType.VarChar)
                        {
                            Value = fmt_ids
                        });

                        cmd1.Parameters.Add(new SqlParameter("@Classids",
                        SqlDbType.VarChar)
                        {
                            Value = temp.ASMCL_Id.ToString()
                        });

                        cmd1.Parameters.Add(new SqlParameter("@groupids",
                         SqlDbType.VarChar)
                        {
                            Value = fmg_ids
                        });
                        cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                         SqlDbType.VarChar)
                        {
                            Value = temp.MI_ID.ToString()
                        });
                        cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
                        SqlDbType.VarChar)
                        {
                            Value = temp.ASMAY_Id.ToString()

                        });

                        if (cmd1.Connection.State != ConnectionState.Open)
                            cmd1.Connection.Open();

                        var retObject1 = new List<dynamic>();
                        try
                        {
                            using (var dataReader1 = await cmd1.ExecuteReaderAsync())
                            {
                                while (await dataReader1.ReadAsync())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader1.GetName(iFiled1),
                                            dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                        );
                                    }

                                    retObject1.Add((ExpandoObject)dataRow1);
                                }
                            }
                            temp.feesummlist = retObject1.ToArray();
                        }


                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {

                }
            }

            if (temp.Balance_1 != 1)
            {

                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "defaulters_report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;
                    cmd.Parameters.Add(new SqlParameter("@fmg_id",
                        SqlDbType.VarChar)
                    {
                        // Value = coloumns
                        Value = fmt_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(temp.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@amsc_id",
                   SqlDbType.BigInt)
                    {
                        Value = temp.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
                   SqlDbType.VarChar)
                    {
                        Value = temp.reporttype
                    });
                    cmd.Parameters.Add(new SqlParameter("@option",
                       SqlDbType.VarChar)
                    {
                        Value = temp.radioval
                    });
                    cmd.Parameters.Add(new SqlParameter("@date1",
               SqlDbType.DateTime)
                    {
                        Value = temp.From_Date
                    });
                    cmd.Parameters.Add(new SqlParameter("@due",
               SqlDbType.VarChar)
                    {
                        Value = temp.duedate
                    });


                    cmd.Parameters.Add(new SqlParameter("@section",
              SqlDbType.BigInt)
                    {
                        Value = temp.AMSC_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@userid",
             SqlDbType.BigInt)
                    {
                        Value = temp.userid
                    });

                    cmd.Parameters.Add(new SqlParameter("@grpid",
                      SqlDbType.VarChar)
                    {
                        Value = fmg_ids
                    });

                    cmd.Parameters.Add(new SqlParameter("@trmr_id",
                     SqlDbType.VarChar)
                    {
                        Value = trmrids
                    });

                    cmd.Parameters.Add(new SqlParameter("@active",
                 SqlDbType.BigInt)
                    {
                        Value = temp.active
                    });
                    cmd.Parameters.Add(new SqlParameter("@deactive",
                  SqlDbType.BigInt)
                    {
                        Value = temp.deactive
                    });
                    cmd.Parameters.Add(new SqlParameter("@left",
                  SqlDbType.BigInt)
                    {
                        Value = temp.left
                    });
                    cmd.Parameters.Add(new SqlParameter("@StdType",
                  SqlDbType.VarChar)
                    {
                        Value = temp.Select_Button
                    });
                    cmd.Parameters.Add(new SqlParameter("@busroute",
                 SqlDbType.VarChar)
                    {
                        Value = temp.yearid
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        // var data = cmd.ExecuteNonQuery();

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
                        if ((temp.radioval == "FSW") || (temp.radioval == "FIW"))
                        {
                            temp.studentalldata = retObject.ToArray();
                        }
                        else if (temp.radioval == "FGW")
                        {
                            temp.groupalldata = retObject.ToArray();
                        }
                        else if (temp.radioval == "FHW")
                        {
                            temp.headalldata = retObject.ToArray();
                        }
                        else if (temp.radioval == "FCW")
                        {
                            temp.classalldata = retObject.ToArray();
                        }
                        else if (temp.radioval == "FPW")
                        {
                            temp.studentalldata = retObject.ToArray();
                        }
                        List<SMSEmailSetting> smsemailset = new List<SMSEmailSetting>();
                        smsemailset = _FeeGroupContext.sMSEmailSetting.Where(y => y.MI_Id == temp.MI_ID && y.ISES_Template_Name == "FEEDEFAULTREPORT").ToList();
                        temp.smsemailsettings = smsemailset.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return temp;
                }
            }
            //======================== balance
            else
            {
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "YearlyFeeStudent_21Jan2020";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        // Value = coloumns
                        Value = temp.MI_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(temp.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                   SqlDbType.BigInt)
                    {
                        Value = temp.ASMCL_Id
                    });



                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
              SqlDbType.BigInt)
                    {
                        Value = temp.AMSC_Id
                    });



                    cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                      SqlDbType.VarChar)
                    {
                        Value = fmg_ids
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMT_Id",
                     SqlDbType.VarChar)
                    {
                        Value = fmt_ids
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
                        temp.student_balance_list = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return temp;
                }
            }

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
                          ).Distinct().OrderBy(t => t.AMSC_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<FeetransactionSMS> sendsms(FeetransactionSMS data)
        {
            try
            {

                var templatecnt = _db.smsEmailSetting.Where(e => e.MI_Id == data.MI_ID && e.ISES_Template_Name == "FEEDEFAULTREPORT" && e.ISES_MailActiveFlag == true && e.ISES_MailHTMLTemplate != null).ToList();

                if (templatecnt.Count > 0)
                {
                    var query = _FeeGroupContext.feeTr.Where(d => d.FMT_Id == data.FMT_ID).ToList();

                    foreach (FeetransactionSMS y in data.selectedStudentList)
                    {
                        string PHNO = y.AMST_MobileNo.ToString();
                        //string PHNO = "7899248169";
                        List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                        alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_ID)).ToList();

                        if (alldetails.Count > 0)
                        {

                            try
                            {

                                Dictionary<string, string> val = new Dictionary<string, string>();

                                var template = _db.smsEmailSetting.Where(e => e.MI_Id == data.MI_ID && e.ISES_Template_Name == "FEEDEFAULTREPORT" && e.ISES_SMSActiveFlag == true).ToList();

                                if (template.Count == 0)
                                {

                                }


                                var institutionName = _db.Institution.Where(i => i.MI_Id == data.MI_ID).ToList();

                                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_ID && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(i => i.ISMP_ID).ToList();

                                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                                string sms = template.FirstOrDefault().ISES_SMSMessage;

                                string result = sms;

                                List<Match> variables = new List<Match>();

                                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                                {
                                    variables.Add(match);
                                }
                                //dua date calculation 

                                List<long> GrpId = new List<long>();

                                var fmg_ids = "";


                                var fmt_ids = "";
                                var fmt_name = "";
                                string maxcol1 = "";
                                if (data.term_group == "T")
                                {
                                    foreach (var x in data.FMT_Ids)
                                    {
                                        fmt_ids += x + ",";
                                    }
                                    fmt_ids = fmt_ids.Substring(0, (fmt_ids.Length - 1));
                                    List<int> list = new List<int>();
                                    string[] s = fmt_ids.Split(',');
                                    for (int i = 0; i < s.Length; i++)
                                    {
                                        var n = Convert.ToInt16(s[i]);
                                        list.Add(n);
                                    }
                                    var maxcol = list.Max();
                                    maxcol1 = list.Max().ToString();
                                    data.FMT_ID = maxcol;
                                }
                                else
                                {
                                    foreach (var x in data.FMG_Ids)
                                    {
                                        fmg_ids += x + ",";
                                    }
                                    fmg_ids = fmg_ids.Substring(0, (fmg_ids.Length - 1));

                                    List<int> list = new List<int>();
                                    string[] s = fmg_ids.Split(',');
                                    for (int i = 0; i < s.Length; i++)
                                    {
                                        var n = Convert.ToInt16(s[i]);
                                        list.Add(n);
                                    }
                                    var maxcol = list.Max();
                                    maxcol1 = list.Max().ToString();
                                    data.FMT_ID = maxcol;
                                }


                                var fordateinfyp = (from d in _FeeGroupContext.FeeStudentTransactionDMO
                                                    from f in _FeeGroupContext.feeMTH
                                                    where (d.FMH_Id == f.FMH_Id && d.FTI_Id == f.FTI_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_ID && d.AMST_Id == y.AMST_ID && data.FMT_Ids.Contains(f.FMT_Id) && d.FSS_ToBePaid > 0)
                                                    select new FeeStudentTransactionDTO
                                                    {
                                                        fmt_id = f.FMT_Id,
                                                    }
                                ).Distinct().ToList();

                                List<int> fmt_list = new List<int>();
                                foreach (FeeStudentTransactionDTO item in fordateinfyp)
                                {
                                    fmt_list.Add(Convert.ToInt32(item.fmt_id));
                                }

                                foreach (var x in fmt_list)
                                {
                                    var query1 = _FeeGroupContext.feeTr.Where(d => d.FMT_Id == x).ToList().FirstOrDefault().FMT_Name;
                                    fmt_name += query1 + ",";
                                }
                                fmt_name = fmt_name.Substring(0, (fmt_name.Length - 1));


                                //var query = _FeeGroupContext.feeTr.Where(d => d.FMT_Id == fmt_ids.Contains(d.FMT_Id)).ToList();


                                data.date = duadatecollect(data.ASMAY_Id, data.FMT_ID, data.ASMCL_Id, data.term_group);



                                var Due_Date = data.date;
                                if (Due_Date == null)
                                {
                                    Due_Date = DateTime.Now.ToString();
                                }

                                //end duadatec calculation
                                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "SMSMAILPARAMETER_TERMS";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@template",
                                       SqlDbType.VarChar)
                                    {
                                        Value = "FEEDEFAULTREPORT"
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@termName",
                                          SqlDbType.VarChar)
                                    {
                                        Value = fmt_name
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@duedate",
                                          SqlDbType.VarChar)
                                    {
                                        Value = Due_Date
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@amount",
                                         SqlDbType.VarChar)
                                    {
                                        Value = y.totalbalance
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@insname",
                                       SqlDbType.VarChar)
                                    {
                                        Value = institutionName[0].MI_Name
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@email",
                                       SqlDbType.VarChar)
                                    {
                                        Value = (y.paid == null || y.paid == "") ? "0" : y.paid
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@type",
                                 SqlDbType.VarChar)
                                    {
                                        Value = "SMS"
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@AMST_ID",
                                 SqlDbType.VarChar)
                                    {
                                        Value = y.AMST_ID
                                    });

                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();

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
                                                    var datatype = dataReader.GetFieldType(iFiled);
                                                    if (datatype.Name == "DateTime")
                                                    {
                                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                                    }
                                                    else
                                                    {
                                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }

                                }




                                for (int j = 0; j < ParamaetersName.Count; j++)
                                {
                                    for (int p = 0; p < val.Count; p++)
                                    {
                                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                                        {

                                            //result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                            result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                            sms = result;
                                        }
                                    }
                                }


                                sms = result;



                                List<SMS_DETAILS_DMO> alldetails123 = new List<SMS_DETAILS_DMO>();
                                alldetails123 = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_ID)).ToList();

                                List<Institution> insdeta = new List<Institution>();
                                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(data.MI_ID)).ToList();

                                if (alldetails123.Count > 0)
                                {
                                    string url = alldetails123[0].IVRMSD_URL.ToString();

                                    url = url.Replace("PHNO", PHNO);

                                    url = url.Replace("MESSAGE", sms);

                                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);

                                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                                    Stream stream = response.GetResponseStream();

                                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                                    string responseparameters = readStream.ReadToEnd();

                                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                    {
                                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == data.MI_ID && e.ISES_Template_Name == "FEEDEFAULTREPORT" && e.ISES_SMSActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

                                        cmd.CommandText = "IVRM_SMS_Outgoing";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                                            SqlDbType.NVarChar)
                                        {
                                            Value = PHNO
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@Message",
                                           SqlDbType.NVarChar)
                                        {
                                            Value = sms
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@module",
                                        SqlDbType.VarChar)
                                        {
                                            Value = "FEES"
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                       SqlDbType.BigInt)
                                        {
                                            Value = data.MI_ID
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@status",
                                   SqlDbType.VarChar)
                                        {
                                            Value = "Delivered"
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                                SqlDbType.VarChar)
                                        {
                                            Value = responseparameters
                                        });


                                        if (cmd.Connection.State != ConnectionState.Open)
                                            cmd.Connection.Open();

                                        try
                                        {
                                            using (var dataReader = cmd.ExecuteReader())
                                            {
                                            }
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

                        }
                    }
                    data.msg = "successSMS";
                }
                else
                {
                    data.msg = "template";
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.msg = "failedSMS";
            }
            return data;
        }

        public string duadatecollect(long asmay_id, long userid, long fillclasflg, string type)
        {
            string date = "";
            string v = "";
            List<FeetransactionSMS> Due_Date_array = new List<FeetransactionSMS>();
            List<FeetransactionSMS> result_duadate = new List<FeetransactionSMS>();
            using (var cmdnew = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
            {
                cmdnew.CommandText = "DUE_DATE_CALCULATION_test";
                cmdnew.CommandType = CommandType.StoredProcedure;
                cmdnew.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = asmay_id });
                cmdnew.Parameters.Add(new SqlParameter("@FMT_Id", SqlDbType.VarChar) { Value = userid });
                cmdnew.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = fillclasflg });
                cmdnew.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar) { Value = type });
                if (cmdnew.Connection.State != ConnectionState.Open)
                    cmdnew.Connection.Open();

                var retObject = new List<dynamic>();
                try
                {
                    using (var dataReadernew = cmdnew.ExecuteReader())
                    {
                        while (dataReadernew.Read())
                        {
                            result_duadate.Add(new FeetransactionSMS
                            {
                                Due_Date = dataReadernew["duedate"].ToString(),
                            });
                            Due_Date_array = result_duadate.ToList();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            if (Due_Date_array.Count == 0)
            {
                v = "";

            }
            else
            {
                date = Due_Date_array[0].Due_Date;
                v = date.Substring(0, 10);

            }

            return v;
        }
        public FeeTransactionPaymentDTO sendemail(FeeTransactionPaymentDTO data)
        {

            try
            {
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == data.MI_ID && e.ISES_Template_Name == data.TemplateName && e.ISES_MailActiveFlag == true && e.ISES_MailHTMLTemplate != null ).ToList();

                if(data.category == true)
                {
                    if (template.Count > 0)
                    {
                        foreach (FeeTransactionPaymentDTO d in data.selectedStudentListforemail)
                        {
                            IVRM_NoticeBoardDMO obj = new IVRM_NoticeBoardDMO();

                            string Mailmsg = template[0].ISES_MailBody;

                            string pattern = "[AMOUNT]";

                            string INTB_Description = Mailmsg.Replace(pattern, d.totalbalance);

                            obj.MI_Id = data.MI_ID;
                            obj.INTB_Title = template[0].ISES_MailSubject;
                            obj.INTB_Description = INTB_Description;
                            obj.INTB_StartDate = DateTime.Today;
                            obj.INTB_EndDate = DateTime.Today;
                            obj.INTB_DisplayDate = DateTime.Today;
                            obj.INTB_Attachment = "";
                            obj.INTB_FilePath = "";
                            obj.NTB_TTSylabusFlg = "O";
                            obj.INTB_DispalyDisableFlg = true;
                            obj.INTB_ToStaffFlg = false;
                            obj.INTB_ToStudentFlg = true;
                            obj.INTB_ActiveFlag = true;

                            obj.CreatedDate = DateTime.Now;
                            _PortalContext.Add(obj);
                           var return_noticedata = _PortalContext.SaveChanges();
                            data.FBD_Id = obj.INTB_Id;

                            if(return_noticedata > 0)
                            {
                                var studentclass = (from a in _db.Adm_M_Student
                                                  from b in _db.School_Adm_Y_StudentDMO
                                                  where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == d.Amst_Id)
                                                  select new FeeTransactionPaymentDTO
                                                  {
                                                      ASMCL_Id = b.ASMCL_Id
                                                  }).Distinct().ToList();

                                IVRM_NoticeBoard_Class_DMO stnc = new IVRM_NoticeBoard_Class_DMO();
                                stnc.INTB_Id = data.FBD_Id;
                                stnc.ASMCL_Id = studentclass[0].ASMCL_Id;
                                stnc.INTBC_ActiveFlag = true;
                                stnc.CreatedDate = DateTime.Today;
                                stnc.UpdatedDate = DateTime.Today;
                                _PortalContext.Add(stnc);

                                _PortalContext.SaveChanges();

                            }


                            if(return_noticedata > 0)
                            {
                                IVRM_NoticeBoard_Student_DMO st = new IVRM_NoticeBoard_Student_DMO();
                                st.AMST_Id = d.Amst_Id;
                                st.INTB_Id = data.FBD_Id;
                                st.INTBCSTD_ActiveFlag = true;
                                st.CreatedDate = DateTime.Today;
                                st.INTBCSTD_CreatedBy = data.userid;
                                _PortalContext.Add(st);

                                var return_noticestudata = _PortalContext.SaveChanges();

                                if(return_noticestudata > 0)
                                {
                                    var devicelist = (from a in _db.Adm_M_Student
                                                      from b in _db.School_Adm_Y_StudentDMO
                                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == d.Amst_Id)
                                                      select new FeeTransactionPaymentDTO
                                                      {
                                                          AMST_MobileNo =Convert.ToString(a.AMST_MobileNo),
                                                          AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                                      }).Distinct().ToList();

                                    long revieveduserid = 0;

                                    if (devicelist.Count > 0)
                                    {
                                        revieveduserid = (from a in _db.StudentUserLoginDMO
                                                          from b in _db.ApplicationUser
                                                          where (a.IVRMSTUUL_UserName == b.UserName && a.AMST_Id == d.Amst_Id)
                                                          select b).Select(t => t.Id).FirstOrDefault();

                                        PushNotification push_noti = new PushNotification(_PortalContext);
                                        push_noti.Call_PushNotificationGeneral(devicelist[0].AMST_AppDownloadedDeviceId, data.MI_ID, data.userid, revieveduserid, data.FBD_Id, template[0].ISES_MailSubject, "NoticeBoard", "NoticeBoard");

                                    }

                                }

                            }




                        }

                        data.msg = "success";
                    }

                }
                else
                {
                    if (template.Count > 0)
                    {
                        foreach (FeeTransactionPaymentDTO d in data.selectedStudentListforemail)
                        {

                            string textBody = " ";
                            d.miid = data.MI_ID;
                            string m = sendmail(d.miid, d.AMST_emailId, data.TemplateName, d.Amst_Id, d.totalbalance, data.ASMAY_Id);
                        }
                        data.msg = "success";
                    }
                    else
                    {

                        data.msg = "template";
                    }
                }

            



               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public FeeTransactionPaymentDTO pushnotification(FeeTransactionPaymentDTO data)
        {

            try
            {
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == data.MI_ID && e.ISES_Template_Name == "FEEDEFAULTREPORT" && e.ISES_PNActiveFlg == true && e.ISES_PNMessage != null).ToList();

                if (template.Count > 0)
                {
                    foreach (FeeTransactionPaymentDTO d in data.selectedStudentListforemail)
                    {

                        string textBody = " ";
                        d.miid = data.MI_ID;
                        var deviceid = _FeeGroupContext.Adm_M_Student.Where(t => t.AMST_Id == d.Amst_Id && t.AMST_ActiveFlag == 1).ToList();
                        string m = callnotification(d.miid, deviceid[0].AMST_AppDownloadedDeviceId, "FEEDEFAULTREPORT", d.Amst_Id, d.totalbalance, data,"Fee Defaulter",d.mobileno);
                    }
                    data.msg = "success";
                }
                else
                {

                    data.msg = "template";
                }




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeTransactionPaymentDTO get_groups(FeeTransactionPaymentDTO data)
        {
            try
            {
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

        //===============================================Staff Portal
        public async Task<FeeTransactionPaymentDTO> getstaffwiseclass(FeeTransactionPaymentDTO data)
        {
            try
            {
                var hrmeid = _FeeGroupContext.Staff_User_Login.Single(c => c.Id == data.userid && c.MI_Id == data.MI_ID).Emp_Code;

                //var s_flag = (from a in _FeeGroupContext.Exm_Login_Privilege

                //              from c in _FeeGroupContext.Staff_User_Login
                //              from d in _FeeGroupContext.MasterEmployee
                //              where (c.IVRMSTAUL_Id == a.Login_Id && d.HRME_Id == c.Emp_Code && a.ELP_ActiveFlg == true && a.MI_Id == data.MI_ID
                //             && a.ASMAY_Id == data.ASMAY_Id && d.HRME_Id == hrmeid)
                //              select new FeeTransactionPaymentDTO
                //              {
                //                  ELP_Flg = a.ELP_Flg,
                //              }
                //                ).ToList();

                //var s_flag = _FeeGroupContext.ClassTeacherMappingDMO.Where(c => c.MI_Id == data.MI_ID && c.ASMAY_Id == data.ASMAY_Id && c.HRME_Id == hrmeid).ToList();

                //data.staff_flag = s_flag.ToArray();
                //if (data.staff_flag.Length > 0)
                //{                    
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_StaffwiseClasslist";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
              SqlDbType.BigInt)
                    {
                        Value = hrmeid
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
                        data.fillclass = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //}
                //else
                //{
                //    data.retmsg = "Not Mapped";
                //    //data.fillclass = (from d in _FeeGroupContext.AcademicYear
                //    //                  from a in _FeeGroupContext.School_M_Class
                //    //                  from b in _FeeGroupContext.school_M_Section
                //    //                  from c in _FeeGroupContext.School_Adm_Y_StudentDMO
                //    //                  where (a.MI_Id == b.MI_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id &&
                //    //                  a.MI_Id == data.MI_ID && c.ASMAY_Id == data.ASMAY_Id)
                //    //                  select new FeeTransactionPaymentDTO
                //    //                  {
                //    //                      ASMCL_Id = c.ASMCL_Id,
                //    //                      asmcL_ClassName = a.ASMCL_ClassName,
                //    //                  }).Distinct().ToArray();
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public FeeTransactionPaymentDTO getStaffterms(FeeTransactionPaymentDTO data)
        {
            try
            {
                if (data.reporttype.Equals("T"))
                {
                    data.fillmastergroup = (from a in _FeeGroupContext.feeMTH
                                            from b in _FeeGroupContext.feeTr
                                            from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                            where (a.FMH_Id == c.FMH_Id && a.FMT_Id == b.FMT_Id && a.MI_Id == data.MI_ID) /*&& c.User_Id == data.userid*/ /*&& a.fmg_id.Contains(data.fmg_id)*/
                                            select new FeeStudentTransactionDTO
                                            {
                                                FMT_Name = b.FMT_Name,
                                                FMT_Id = a.FMT_Id,
                                            }
                         ).Distinct().ToArray();


                    data.customlist = (from a in _FeeGroupContext.feegm
                                       from b in _FeeGroupContext.feeGGG
                                       from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                       where (a.FMGG_Id == b.FMGG_Id && c.FMG_ID == b.FMG_Id && a.FMGG_ActiveFlag == true && a.MI_Id == data.MI_ID) /*&& c.User_Id == data.userid*/
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
                                       where (a.FMGG_Id == b.FMGG_Id && c.FMG_ID == b.FMG_Id && a.FMGG_ActiveFlag == true && a.MI_Id == data.MI_ID) /*&& c.User_Id == data.userid*/
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
                                      where (a.FMG_Id == b.FMG_Id && b.FMGG_Id == c.FMGG_Id && grpid.Contains(c.FMGG_Id) && a.MI_Id == data.MI_ID)
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

        public FeeTransactionPaymentDTO saveremark(FeeTransactionPaymentDTO data)
        {
            try
            {
                if (data.remarkarray.Length > 0)
                {
                    foreach (var item in data.remarkarray)
                    {
                        Fee_Student_Defaulter_Remarks_DMO sd = new Fee_Student_Defaulter_Remarks_DMO();
                        sd.MI_Id = data.MI_ID;
                        sd.AMST_Id = item.Amst_Id;
                        sd.ASMAY_Id = data.ASMAY_Id;
                        sd.FMG_Id = item.FMG_Id;
                        sd.FMT_Id = item.FMT_ID;
                        sd.User_Id = data.userid;
                        sd.FSDREM_Remarks = item.FSDREM_Remarks;
                        sd.FSDREM_RemarksDate = DateTime.Today;
                        sd.FSDREM_ActiveFlag = true;
                        sd.FSDREM_CreatedDate = DateTime.Today;
                        sd.FSDREM_CreatedBy = data.userid;
                        sd.FSDREM_UpdatedDate = DateTime.Today;
                        sd.FSDREM_UpdatedBy = data.userid;
                        _FeeGroupContext.Add(sd);
                    }
                    var cnt = _FeeGroupContext.SaveChanges();
                    if (cnt > 0)
                    {
                        data.message = "Add";
                    }
                    else
                    {
                        data.message = "Error";
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public FeeTransactionPaymentDTO feeremarkreport(FeeTransactionPaymentDTO data)
        {
            try
            {
                string secid = "0";
                if (data.ASMS_Id == 0)
                {
                    if (data.sectionarray.Length > 0)
                    {
                        foreach (var item in data.sectionarray)
                        {
                            secid = secid + "," + item.ASMS_Id;
                        }

                    }
                }
                else
                {
                    secid = secid + "," + data.ASMS_Id;
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FeeDefaulterRemarkReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    { Value = data.MI_ID });
                    cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar)
                    { Value = data.fromdate });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar)
                    { Value = data.todate });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt)
                    { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    { Value = secid });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                    { Value = data.ASMAY_Id });





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
                        data.feedefaulterreport = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public FeeTransactionPaymentDTO feeremarkload(FeeTransactionPaymentDTO data)
        {
            try
            {
                var cls = _FeeGroupContext.School_M_Class.Where(a => a.MI_Id == data.MI_ID && a.ASMCL_ActiveFlag == true).ToList();
                if (cls.Count > 0)
                {
                    data.fillclass = cls.ToArray();
                }

                var year = _FeeGroupContext.AcademicYear.Where(a => a.MI_Id == data.MI_ID && a.ASMAY_ActiveFlag == 1).ToList();
                if (cls.Count > 0)
                {
                    data.adcyear = year.OrderByDescending(a => a.ASMAY_Order).ToArray();
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public FeeTransactionPaymentDTO feeremarksection(FeeTransactionPaymentDTO data)
        {
            try
            {
                var edit = (from a in _db.School_Adm_Y_StudentDMO
                            from b in _db.School_M_Section
                            where (a.ASMS_Id == b.ASMS_Id && b.MI_Id == data.MI_ID && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id)
                            select new FeeTransactionPaymentDTO
                            {
                                ASMS_Id = a.ASMS_Id,
                                ASMC_SectionName = b.ASMC_SectionName

                            }).Distinct().OrderBy(t => t.ASMC_SectionName).ToArray();
                if (edit.Length > 0)
                {
                    data.fillsection = edit.ToArray();
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
       
        public string sendmail(long MI_Id, string Email, string Template, long UserID,string totbal,long asmay_id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    Mailmsg = result;
                    Mailcontent = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "SMSMAILPARAMETER_FEE_DEFAULTER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });
                        cmd.Parameters.Add(new SqlParameter("@totbalance",
                          SqlDbType.VarChar)
                        {
                            Value = totbal
                        });
                        cmd.Parameters.Add(new SqlParameter("@asmay_id",
                       SqlDbType.VarChar)
                        {
                            Value = asmay_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                      SqlDbType.VarChar)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }


                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                //result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                Mailmsg = result;
                            }
                        }
                    }
                    Mailmsg = result;

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                //Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                Mailcontent = Resultsms;
                            }
                        }
                    }
                    Mailcontent = Resultsms;
                }

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        string mailcc = "";
                        string mailbcc = "";
                        if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        {
                            string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                            mailcc = ccmail[0].ToString();

                            if (ccmail.Length > 1)
                            {
                                if (ccmail[1] != null || ccmail[1] != "")
                                {
                                    mailbcc = ccmail[1].ToString();
                                }
                            }

                        }
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        //Sending mail using SendGrid API.
                        //Date:07-02-2017.

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);

                        if (Attechement.Equals("1"))
                        {
                            var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                            if (img.Count > 0)
                            {
                                for (int i = 0; i < img.Count; i++)
                                {
                                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                    Stream stream = response.GetResponseStream();
                                    message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                                }
                            }
                        }

                        if (mailcc != null && mailcc != "")
                        {
                            message.AddCc(mailcc);
                        }
                        if (mailbcc != null && mailbcc != "")
                        {
                            message.AddBcc(mailbcc);
                        }

                        message.HtmlContent = Mailmsg;
                        // var client = new Web("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");

                        //if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                        //{
                        //    message.AddAttachment(template.FirstOrDefault().ISES_MailHTMLTemplate);

                        //}
                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {


                        string mailcc = "";
                        string mailbcc = "";
                        if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        {
                            string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                            mailcc = ccmail[0].ToString();

                            if (ccmail.Length > 1)
                            {
                                if (ccmail[1] != null || ccmail[1] != "")
                                {
                                    mailbcc = ccmail[1].ToString();
                                }
                            }

                        }
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };

                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;

                            using (var emailMessage = new MailMessage())
                            {


                                emailMessage.To.Add(new MailAddress(Email));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {

                                                //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));

                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }


                                            //var attachment = new MimePart("image", "gif")
                                            //{
                                            //    ContentObject = new ContentObject(File.OpenRead(img[i].IVRM_Att_Path), ContentEncoding.Default),
                                            //    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                                            //    ContentTransferEncoding = ContentEncoding.Base64,
                                            //    FileName = Path.GetFileName(img[i].IVRM_Att_Path)
                                            //};
                                        }
                                    }
                                }


                                if (mailcc != null && mailcc != "")
                                {
                                    emailMessage.CC.Add(mailcc);
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    emailMessage.Bcc.Add(mailbcc);
                                }
                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailcontent
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public string callnotification(long mi_id, string devicenew, string templatename, long amstid, string totalbalance,  FeeTransactionPaymentDTO dto, string header_flg,long? mobileno)
        {

            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == mi_id && e.ISES_Template_Name == templatename && e.ISES_PNActiveFlg == true).ToList();

                if (template.Count == 0)
                {
                    return "";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == mi_id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == mi_id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

              
                string Mailmsg = template.FirstOrDefault().ISES_PNMessage;
                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

               
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "SMSMAILPARAMETER_FEE_DEFAULTER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = dto.userid
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = templatename
                        });
                        cmd.Parameters.Add(new SqlParameter("@totbalance",
                          SqlDbType.VarChar)
                        {
                            Value = totalbalance
                        });
                        cmd.Parameters.Add(new SqlParameter("@asmay_id",
                       SqlDbType.VarChar)
                        {
                            Value = dto.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                      SqlDbType.VarChar)
                        {
                            Value = mi_id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }


                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                //result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                Mailmsg = result;
                            }
                        }
                    }
                    Mailmsg = result;








                    var key = _db.MobileApplAuthenticationDMO.Single(a => a.MI_Id == mi_id).MAAN_AuthenticationKey;
                FeeTransactionPaymentDTO data = new FeeTransactionPaymentDTO();


              //  var homework = _db.IVRM_Homework_DMO.Where(h => h.MI_Id == mi_id && h.IHW_ActiveFlag == true && h.IHW_Id == ihw_Id).Distinct().ToList();
                string url = "";
                string utrrno = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";

                string sound = "default";
                string notId = "3";
                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                 "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + "Homework" + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + Mailmsg + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                notificationparams.Add(daata.ToString());

                // var mycontent = JsonConvert.SerializeObject(notificationparams);
                var mycontent = notificationparams[0];
                string postdata = mycontent.ToString();
                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                connection.Method = "post";
                connection.Headers["authorization"] = "key=" + key;

                using (StreamWriter requestwriter = new StreamWriter(connection.GetRequestStream()))
                {
                    requestwriter.Write(postdata);
                }
                string responsedata = string.Empty;

                using (StreamReader responsereader = new StreamReader(connection.GetResponse().GetResponseStream()))
                {
                    responsedata = responsereader.ReadToEnd();
                    JObject joresponse1 = JObject.Parse(responsedata);
                }
                PushNotification push_noti = new PushNotification(_db);
              //  public string Insert_PushNotificationFeeDefaulter(long MI_Id, string Header_Name, string Message, long? MobileNo, string HRME_AppDownloadedDeviceId, long AMST_Id)


               // push_noti.Insert_PushNotificationFeeDefaulter(mi_id,header_flg,Mailmsg,mobileno, devicenew, amstid );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";

        }

        public FeeTransactionPaymentDTO feedefaultersmstriggering(FeeTransactionPaymentDTO data)        {            List<FeeTransactionPaymentDTO> devicelist = new List<FeeTransactionPaymentDTO>();            List<FeeTransactionPaymentDTO> deviceidsnew = new List<FeeTransactionPaymentDTO>();            try            {
                // List<MasterAcademic> year = new List<MasterAcademic>();
                var year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_ID && t.Is_Active == true).OrderByDescending(l => l.ASMAY_Order).FirstOrDefault().ASMAY_Id;


                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "FeeDefaulterEmailTrigger";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@MI_Id",                  SqlDbType.BigInt)                    {                        Value = data.MI_ID                    });                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",               SqlDbType.BigInt)                    {                        Value = year                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                devicelist.Add(new FeeTransactionPaymentDTO                                {
                                    //MI_ID = Convert.ToInt64(dataReader["MI_Id"]),
                                    AMST_FirstName = Convert.ToString(dataReader["StudentName"]),                                    AMST_emailId = Convert.ToString(dataReader["AMST_emailId"]),                                    FMT_Name = Convert.ToString(dataReader["FMT_Name"]),                                    Due_Date = Convert.ToString(dataReader["FMTFHDD_DueDate"]),                                    ftp_tobepaid_amt = Convert.ToDecimal(dataReader["Amount"]),                                });                            }                        }                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }                int k = 0;                foreach (var deviceid in devicelist)                {                    string m = sendmailToDefaulter(data.MI_ID, deviceid.AMST_FirstName, data.TemplateName, deviceid.AMST_emailId, deviceid.FMT_Name, deviceid.Due_Date, deviceid.ftp_tobepaid_amt);
                }            }            catch (Exception e)            {                Console.WriteLine(e.Message);            }            return data;        }

        public string sendmailToDefaulter(long mi_id, string Username, string Template, string email, string termname, string duedate,decimal ftp_tobepaid_amt)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == mi_id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == mi_id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                Mailmsg = Mailmsg.Replace("[NAME]", Username);
                //  Mailmsg = Mailmsg.Replace("[EMAIL]", email);
                Mailmsg = Mailmsg.Replace("[TERM]", termname);
                Mailmsg = Mailmsg.Replace("[DUEDATE]", duedate);
                Mailmsg = Mailmsg.Replace("[AMOUNT]", ftp_tobepaid_amt.ToString());
                Mailcontent = Template;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(mi_id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(mi_id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "SMTP")
                    {
                        string mailcc = "";
                        string mailbcc = "";

                        if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        {
                            string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                            mailcc = ccmail[0].ToString();

                            if (ccmail.Length > 1)
                            {
                                if (ccmail[1] != null || ccmail[1] != "")
                                {
                                    mailbcc = ccmail[1].ToString();
                                }
                            }

                        }
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(email)

    ;


                        if (Attechement.Equals("1"))
                        {
                            var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                            if (img.Count > 0)
                            {
                                for (int i = 0; i < img.Count; i++)
                                {
                                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                    Stream stream = response.GetResponseStream();
                                    message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                                }
                            }
                        }
                        //***************** EMAIL CC DETAILS **************//

                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        //***************** EMAIL BCC DETAILS **************//

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailBCCId.Split(',');

                            foreach (var d in ccmaildetails)
                            {
                                message.AddCc(d);
                            }
                        }




                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        try
                        {
                            client.SendEmailAsync(message).Wait();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(email)

    ;
                        //***************** EMAIL CC DETAILS **************//

                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        //***************** EMAIL BCC DETAILS **************//

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailBCCId.Split(',');

                            foreach (var d in ccmaildetails)
                            {
                                message.AddCc(d);
                            }
                        }

                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        try
                        {
                            client.SendEmailAsync(message).Wait();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }




                    }


                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == mi_id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailcontent
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = (mi_id.ToString())

                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
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
            return "success";


        }
    }
}












