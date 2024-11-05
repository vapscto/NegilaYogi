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
using System.Globalization;
using DomainModel.Model.com.vapstech.Transport;


namespace FeeServiceHub.com.vaps.services
{
    public class FeeArrearRegisterReportImpl : interfaces.FeeArrearRegisterReportInterface
    {


        public FeeGroupContext _FeeGroupContext;
        public FeeArrearRegisterReportImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }

        public FeeArrearRegisterReportDTO getdata123(FeeArrearRegisterReportDTO data)
        {

            try
            {

                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_ID).ToList();
                data.acayear = year.Distinct().GroupBy(y => y.ASMAY_Year).Select(y => y.First()).OrderByDescending(y=>y.ASMAY_Order).ToArray();

             
                List<School_M_Class> classes = new List<School_M_Class>();
                classes = _FeeGroupContext.School_M_Class.Where(t => t.ASMCL_ActiveFlag == true && t.MI_Id == data.MI_ID).ToList();
                data.classlist = classes.ToArray();


                List<MasterRouteDMO> installments = new List<MasterRouteDMO>();
                installments = _FeeGroupContext.MasterRouteDMO.Where(i => i.TRMR_ActiveFlg == true && i.MI_Id == data.MI_ID).ToList();
                data.fillroute = installments.ToArray();



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
                                       where (a.FMGG_Id == b.FMGG_Id && c.FMG_ID == b.FMG_Id  && a.FMGG_ActiveFlag == true && a.MI_Id == data.MI_ID && c.User_Id==data.userid)
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public FeeArrearRegisterReportDTO getsection(FeeArrearRegisterReportDTO data)
        {
            List<long> GrpId = new List<long>();
            try
            {
                List<School_M_Section> section = new List<School_M_Section>();
                data.sectionlist = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                    from b in _FeeGroupContext.school_M_Section
                                    where (a.ASMS_Id == b.ASMS_Id && a.ASMAY_Id == data.asmay_id && a.ASMCL_Id == data.ASMCL_Id && b.MI_Id == data.MI_ID)
                                    select new FeeArrearRegisterReportDTO
                                    {
                                        AMSC_Id = b.ASMS_Id,
                                        asmc_sectionname = b.ASMC_SectionName,
                                        ASMC_Order=b.ASMC_Order
                                    }
                          ).Distinct().OrderBy(b=>b.AMSC_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeArrearRegisterReportDTO getstudent(FeeArrearRegisterReportDTO data)
        {
            List<long> GrpId = new List<long>();
            try
            {
                List<School_M_Section> section = new List<School_M_Section>();


                //       data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                //                              from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                //                              where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.AMSC_Id && a.AMST_SOL == "S")
                //                              select new FeeArrearRegisterReportDTO
                //                              {
                //                                  Amst_Id = a.AMST_Id,
                //                                  AMST_FirstName = a.AMST_FirstName,
                //                                  AMST_MiddleName = a.AMST_MiddleName,
                //                                  AMST_LastName = a.AMST_LastName,
                //                              }
                //).Distinct().ToArray();


                if (data.AMSC_Id==0)
                {
                    data.admsudentslist = (from m in _FeeGroupContext.AdmissionStudentDMO
                                           from n in _FeeGroupContext.School_Adm_Y_StudentDMO
                                           where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_ID && n.ASMAY_Id == data.asmay_id && n.ASMCL_Id == data.ASMCL_Id 
                                           select new FeeTransactionPaymentDTO
                                           {
                                               Amst_Id = m.AMST_Id,
                                               ASMAY_Id = m.ASMAY_Id,
                                               AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + " " + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + " " + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),
                                               AMST_MiddleName = m.AMST_MiddleName,
                                               AMST_LastName = m.AMST_LastName,
                                               AMST_AdmNo = m.AMST_AdmNo
                                           }).Distinct().ToList().ToArray();
                }
                else
                {
                    data.admsudentslist = (from m in _FeeGroupContext.AdmissionStudentDMO
                                           from n in _FeeGroupContext.School_Adm_Y_StudentDMO
                                           where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_ID && n.ASMAY_Id == data.asmay_id && n.ASMCL_Id == data.ASMCL_Id && n.ASMS_Id == data.AMSC_Id
                                           select new FeeTransactionPaymentDTO
                                           {
                                               Amst_Id = m.AMST_Id,
                                               ASMAY_Id = m.ASMAY_Id,
                                               AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + " " + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + " " + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),
                                               AMST_MiddleName = m.AMST_MiddleName,
                                               AMST_LastName = m.AMST_LastName,
                                               AMST_AdmNo = m.AMST_AdmNo
                                           }).Distinct().ToList().ToArray();
                }
               
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeArrearRegisterReportDTO getstuddet(FeeArrearRegisterReportDTO data)
        {
            if (data.filterinitialdata == null)
            {
                data.filterinitialdata = "NameRegNo";
            }

            try
            {
                if (data.filterinitialdata == "NameRegNo")
                {
                    data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                           from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                           where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id && a.AMST_SOL == "S")
                                           select new FeeArrearRegisterReportDTO
                                           {
                                               Amst_Id = a.AMST_Id,
                                               AMST_FirstName = a.AMST_FirstName,
                                               AMST_MiddleName = a.AMST_MiddleName,
                                               AMST_LastName = a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                           }
                           ).Distinct().ToArray();


                }
                else if (data.filterinitialdata == "RegNoName")
                {
                    data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                           from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                           where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id && a.AMST_SOL == "S")
                                           select new FeeArrearRegisterReportDTO
                                           {
                                               Amst_Id = a.AMST_Id,
                                               AMST_FirstName = a.AMST_RegistrationNo + "-" + a.AMST_FirstName,
                                               AMST_MiddleName = a.AMST_MiddleName,
                                               AMST_LastName = a.AMST_LastName,
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

        public async Task<FeeArrearRegisterReportDTO> getreport([FromBody] FeeArrearRegisterReportDTO data)
        {
        
            int T = 0;
            string name = "";
            string name1 = "";
            List<long> GrpId = new List<long>();
            FeeArrearRegisterReportDTO pmm = new FeeArrearRegisterReportDTO();
            var retObject = new List<dynamic>();
            var retObject1 = new List<dynamic>();
            var retObject2 = new List<dynamic>();

            FeeStudentGroupInstallmentMappingDMO fsgim = new FeeStudentGroupInstallmentMappingDMO();


            long fmgg_id = 0;
            var fmgg_ids = "";
            foreach (var x in data.FMGG_Ids)
            {
                fmgg_ids += x + ",";
            }
            fmgg_ids = fmgg_ids.Substring(0, (fmgg_ids.Length - 1));
            //fmgg_id = Convert.ToInt32(fmgg_ids);

            var fmg_ids = "";
            foreach (var x in data.FMG_Ids)
            {
                fmg_ids += x + ",";
            }
            fmg_ids = fmg_ids.Substring(0, (fmg_ids.Length - 1));

            var fmt_ids = "";
            string maxcol = "";
            string mincol = "";
            string startmonth = "";
            string endmonth = "";
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
                maxcol = list.Max().ToString();
                mincol = list.Min().ToString();


                if (data.userid == 364)
                {
                    startmonth = _FeeGroupContext.feeTr.Where(d => d.FMT_Id == Convert.ToInt32(mincol) && d.MI_Id == data.MI_ID).ToList().FirstOrDefault().Transport_FromMonth;
                    endmonth = _FeeGroupContext.feeTr.Where(f => f.FMT_Id == Convert.ToInt32(maxcol) && f.MI_Id == data.MI_ID).ToList().FirstOrDefault().Transport_ToMonth;
                }
                else
                {
                    startmonth = _FeeGroupContext.feeTr.Where(d => d.FMT_Id == Convert.ToInt32(mincol) && d.MI_Id == data.MI_ID).ToList().FirstOrDefault().FromMonth;
                    endmonth = _FeeGroupContext.feeTr.Where(f => f.FMT_Id == Convert.ToInt32(maxcol) && f.MI_Id == data.MI_ID).ToList().FirstOrDefault().ToMonth;
                }

                data.period = startmonth + '-' + endmonth;

            }

            else
            {
                fmt_ids = "0";
            }
            

            var From_date = data.From_Date.ToString();
            var To_date = data.To_Date.ToString();
            

            data.studentdetails = (from a in _FeeGroupContext.feeGroup
                                   from b in _FeeGroupContext.FeeStudentTransactionDMO
                                   where (a.FMG_Id == b.FMG_Id && b.ASMAY_Id == data.asmay_id && b.MI_Id == data.MI_ID && b.AMST_Id == data.Amst_Id)

                                   select new FeeArrearRegisterReportDTO
                                   {
                                       fmg_groupname = a.FMG_GroupName
                                   }

              ).Distinct().ToArray();
            

            List<MasterAcademic> year = new List<MasterAcademic>();
            year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_ID && t.ASMAY_Id == data.asmay_id).ToList();

            //List<FeeStudentTransactionDTO> fordate = new List<FeeStudentTransactionDTO>();
            //List<FeeStudentTransactionDTO> fordateinfyp = new List<FeeStudentTransactionDTO>();

            //fordate = (from d in _FeeGroupContext.FeeStudentTransactionDMO
            //           from f in _FeeGroupContext.feeTDueDateRegularDMO
            //           from e in _FeeGroupContext.feegm
            //           from g in _FeeGroupContext.feeGGG
            //           where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.asmay_id && d.MI_Id == data.MI_ID && d.FMG_Id==g.FMG_Id && e.FMGG_Id==g.FMGG_Id && e.FMGG_Id==fmgg_id && (d.FSS_ToBePaid > 0))
            //           select new FeeStudentTransactionDTO
            //           {

            //               date = f.FTDD_Day,
            //               month = f.FTDD_Month
            //           }
            //          ).Distinct().ToList();

            //fordateinfyp = (from d in _FeeGroupContext.FeeStudentTransactionDMO
            //                from f in _FeeGroupContext.feeTDueDateRegularDMO
            //                where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.asmay_id && d.MI_Id == data.MI_ID )
            //                select new FeeStudentTransactionDTO
            //                {
            //                    month = f.FTDD_Month,
            //                }
            //         ).Distinct().ToList();

            //List<int> months = new List<int>();
            //List<int> dates = new List<int>();
            //List<int> months1 = new List<int>();
            //List<int> months2 = new List<int>();

            //List<int> startperiod = new List<int>();

            //foreach (FeeStudentTransactionDTO item in fordate)
            //{
            //    dates.Add(Convert.ToInt32(item.date));
            //    months.Add(Convert.ToInt32(item.month));
            //}

            //foreach (FeeStudentTransactionDTO itemperiod in fordateinfyp)
            //{
            //    startperiod.Add(Convert.ToInt32(itemperiod.month));
            //}

            //foreach (var item in months)
            //{
            //    if (Convert.ToInt32(item) <= 12 && Convert.ToInt32(item) > 3)
            //    {
            //        months1.Add(Convert.ToInt32(item));
            //        var curyear = DateTime.Now;
            //        data.year = curyear.Year.ToString();
            //    }
            //    else
            //    {
            //        months2.Add(Convert.ToInt32(item));
            //        var curyear = DateTime.Now;
            //        var nextyr = curyear.Year + 1;
            //        data.year = nextyr.ToString();
            //    }
            //}

            //var sMonth = DateTime.Now.Month;
            //var sdate = DateTime.Now.Day;
            //List<int> final_mnths = new List<int>();
            //foreach (var item in months1)
            //{
            //    if (item >= sMonth)
            //        {
            //        final_mnths.Add(item);

            //    }
            //}

            //string maxmonth = "", monthnameinitial = "", monthnameend = "";
            //if (months1.Count() > 0)
            //{

            //    data.month = final_mnths.Min().ToString();
            //    maxmonth = months1.Max().ToString();
            //    if (startperiod.Count >= 4)
            //    {
            //        monthnameinitial = startperiod.Min().ToString();
            //        maxmonth = startperiod.Max().ToString();
            //        monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(monthnameinitial));
            //        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
            //    }
            //    else
            //    {
            //        monthnameinitial = startperiod.Max().ToString();
            //        monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(monthnameinitial));
            //        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
            //    }

            //    data.period = monthnameinitial + '-' + monthnameend + '-' + data.year;
            //}
            //else if (months2.Count() > 0)
            //{
            //    data.month = months2.Min().ToString();
            //    maxmonth = months2.Max().ToString();

            //    monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(data.month));
            //    monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));


            //    data.period = monthnameinitial + '-' + monthnameend + '-' + data.year;

            //}
            //for (int i = 0; i < months.Count(); i++)
            //{
            //    if (Convert.ToInt32(data.month) == months[i])
            //    {
            //        data.date = dates[i].ToString();

            //    }
            //}

            //if (months.Count == 0)
            //{
            //    foreach (var item in startperiod)
            //    {
            //        if (Convert.ToInt32(item) <= 12 && Convert.ToInt32(item) > 3)
            //        {
            //            monthnameinitial = startperiod.Min().ToString();
            //            var curyear = DateTime.Now;
            //            data.year = curyear.Year.ToString();
            //        }
            //        else
            //        {
            //            maxmonth = startperiod.Max().ToString();
            //            var curyear = DateTime.Now;
            //            var nextyr = curyear.Year + 1;
            //            data.year = nextyr.ToString();
            //        }
            //    }
            //    if (monthnameinitial != "")
            //    {
            //        monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(monthnameinitial));
            //    }

            //    if (monthnameend != "")
            //    {
            //        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
            //    }
            //    else
            //    {
            //        maxmonth = startperiod.Max().ToString();
            //        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
            //    }
            //}

            //var termperiodlist = _FeeGroupContext.feeTr.Where(d => d.MI_Id == data.MI_ID && d.FMT_Id == fmt_id_new).ToArray();

            //monthnameinitial = termperiodlist[0].FromMonth.ToString();
            //monthnameend = termperiodlist[0].ToMonth.ToString();

            //data.period = monthnameinitial + '-' + monthnameend + '-' + data.year;



          //  data.date = duadatecollect(data.asmay_id, data.userid, data.fillclasflg);


           

            //List<long> fmtids = new List<long>();
            //foreach(FeeStudentTransactionDTO item in data.fillmastergroup)
            //{
            //    fmtids.Add(item.FMT_Id);
            //}


            if(data.trmR_Id==0)
            {
                data.reportdatelist = (from a in _FeeGroupContext.feeIDDD
                                       from b in _FeeGroupContext.feeMIY
                                       from c in _FeeGroupContext.feeMI
                                       from d in _FeeGroupContext.FeeAmountEntryDMO
                                       from e in _FeeGroupContext.FeeMasterTermHeadsDMO
                                       from f in _FeeGroupContext.feeYCC
                                       from g in _FeeGroupContext.feeYCCC
                                       where (a.FTI_Id == b.FTI_Id && b.FMI_Id == c.FMI_Id && b.FTI_Id == d.FTI_Id && d.FTI_Id == e.FTI_Id && e.FMT_Id == Convert.ToInt64(maxcol) && d.MI_Id == data.MI_ID && d.ASMAY_Id == data.asmay_id && a.MI_Id == data.MI_ID && a.ASMAY_Id == data.asmay_id && c.FMI_No_Of_Installments != 1 && data.FMG_Ids.Contains(d.FMG_Id) && f.FMCC_Id == d.FMCC_Id && f.FYCC_Id == g.FYCC_Id && g.ASMCL_Id == data.fillclasflg && f.ASMAY_Id == data.asmay_id)
                                       select new FeeStudentTransactionDTO
                                       {
                                           dated = a.FTIDD_DueDate
                                       }).Distinct().ToArray();
            }


            else
            {
                data.reportdatelist = (from a in _FeeGroupContext.feeIDDD
                                       from b in _FeeGroupContext.feeMIY
                                       from c in _FeeGroupContext.feeMI
                                       from d in _FeeGroupContext.FeeAmountEntryDMO
                                       from e in _FeeGroupContext.FeeMasterTermHeadsDMO
                                       from f in _FeeGroupContext.feeYCC
                                       from g in _FeeGroupContext.feeYCCC
                                       where (a.FTI_Id == b.FTI_Id && b.FMI_Id == c.FMI_Id && b.FTI_Id == d.FTI_Id && d.FTI_Id == e.FTI_Id && e.FMT_Id == Convert.ToInt64(maxcol) && d.MI_Id == data.MI_ID && d.ASMAY_Id == data.asmay_id && a.MI_Id == data.MI_ID && a.ASMAY_Id == data.asmay_id && c.FMI_No_Of_Installments != 1 && data.FMG_Ids.Contains(d.FMG_Id) && f.FMCC_Id == d.FMCC_Id && f.FYCC_Id == g.FYCC_Id && f.ASMAY_Id == data.asmay_id)
                                       select new FeeStudentTransactionDTO
                                       {
                                           dated = a.FTIDD_DueDate
                                       }).Distinct().ToArray();
            }


           



            using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fee_Arrear_Report_1";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@amay_id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.asmay_id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                       SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.fillclasflg)
                    });

                    cmd.Parameters.Add(new SqlParameter("@amcl_id",
                   SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.fillseccls)
                    });
                cmd.Parameters.Add(new SqlParameter("@amst_id",
                   SqlDbType.BigInt)
                {
                    Value = 1
                });
                cmd.Parameters.Add(new SqlParameter("@fmt_id",
                  SqlDbType.VarChar)
                    {
                        Value= fmt_ids
                    // Value = Convert.ToInt64(data.TempararyArrayhEADListnew[T].FMT_ID)
                });

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                  SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.MI_ID)
                    });

                cmd.Parameters.Add(new SqlParameter("@fmg_id",
               SqlDbType.VarChar)
                {
                    Value = fmg_ids

                });
                cmd.Parameters.Add(new SqlParameter("@userid",
             SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.userid)
                });

                cmd.Parameters.Add(new SqlParameter("@fmgg_id",
             SqlDbType.VarChar)
                {
                    Value = fmgg_ids
                });
               
                cmd.Parameters.Add(new SqlParameter("@trmr_id",
             SqlDbType.VarChar)
                {
                    Value = data.trmR_Id
                });
                cmd.Parameters.Add(new SqlParameter("@Amst_ids",
            SqlDbType.VarChar)
                {
                    Value = data.Amst_Id
                });

                if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    // var retObject = new List<dynamic>();

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

                                retObject1.Add((ExpandoObject)dataRow);
                            }

                        }

                        data.admsudentslist1 = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                   
            }

            using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Fee_Arrear_Report_1";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@amay_id",
                    SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.asmay_id)
                });
                cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                   SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.fillclasflg)
                });

                cmd.Parameters.Add(new SqlParameter("@amcl_id",
               SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.fillseccls)
                });
                cmd.Parameters.Add(new SqlParameter("@amst_id",
                   SqlDbType.BigInt)
                {
                    Value = 0
                });
                cmd.Parameters.Add(new SqlParameter("@fmt_id",
                  SqlDbType.VarChar)
                {
                    Value = fmt_ids
                   
                });

                cmd.Parameters.Add(new SqlParameter("@mi_id",
              SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.MI_ID)
                });

                cmd.Parameters.Add(new SqlParameter("@fmg_id",
               SqlDbType.VarChar)
                {
                    Value = fmgg_ids
                    
                });

                cmd.Parameters.Add(new SqlParameter("@userid",
              SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.userid)
                });

                cmd.Parameters.Add(new SqlParameter("@fmgg_id",
            SqlDbType.VarChar)
                {
                    Value = fmgg_ids
                });
                cmd.Parameters.Add(new SqlParameter("@trmr_id",
             SqlDbType.VarChar)
                {
                    Value = data.trmR_Id
                });

                cmd.Parameters.Add(new SqlParameter("@Amst_ids",
           SqlDbType.VarChar)
                {
                    Value = data.Amst_Id
                });

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                // var retObject = new List<dynamic>();

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

                            retObject2.Add((ExpandoObject)dataRow);
                        }

                    }

                    data.studentlist = retObject2.ToArray();
                    List<MasterAcademic> currentyr = new List<MasterAcademic>();
                    currentyr = _FeeGroupContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_ID && y.ASMAY_Id == data.asmay_id).ToList();
                    data.acayear = year.GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();


                    //var logodetail= _FeeGroupContext.feemastersettings.Where(d => d.MI_Id == data.MI_ID && d.userid == data.userid && d.ASMAY_ID==data.asmay_id).ToList().FirstOrDefault().MI_Logo;



                    // data.headlist = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                    //                  from b in _FeeGroupContext.FeePaymentDetailsDMO
                    //                  from c in _FeeGroupContext.feeMTH
                    //                  from d in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                    //                  from e in _FeeGroupContext.FeeAmountEntryDMO
                    //                  from f in _FeeGroupContext.FeeTransactionPaymentDMO
                    //                  from g in _FeeGroupContext.school_M_Section
                    //                  from h in _FeeGroupContext.School_M_Class
                    //                  where (a.AMST_Id == d.AMST_Id && b.FYP_Id == d.FYP_Id && d.FYP_Id == f.FYP_Id && f.FMA_Id == e.FMA_Id && e.FMH_Id == c.FMH_Id && e.FTI_Id == c.FTI_Id && a.ASMCL_Id == h.ASMCL_Id && a.ASMS_Id == g.ASMS_Id && b.MI_Id == data.MI_ID && b.ASMAY_ID == data.asmay_id && data.FMG_Ids.Contains(e.FMG_Id) && data.FMT_Ids.Contains(c.FMT_Id) && e.ASMAY_Id == data.asmay_id && a.ASMAY_Id == data.asmay_id && a.ASMCL_Id == data.fillclasflg && a.ASMS_Id == data.fillseccls && ((a.AMST_Id == 177) || (a.AMST_Id == 173)))
                    //                  select new FeeArrearRegisterReportDTO
                    //                  {
                    //                      Amst_Id = d.AMST_Id,
                    //                      customflag = b.FYP_Receipt_No,
                    //                      date = b.FYP_Date.ToString("dd/MM/yyyy"),
                    //                      FMT_ID = c.FMT_Id
                    //                  }
                    //).Distinct().OrderBy(t => t.Amst_Id).ThenBy(n => n.FMT_ID).ToArray();

                    var retObject5 = new List<dynamic>();
                    using (var cmd1 = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd1.CommandText = "Receipt_List";
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                        {
                            Value = Convert.ToInt64(data.MI_ID)
                        });
                        cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.VarChar)
                        {
                            Value = Convert.ToInt64(data.asmay_id)
                        });
                        cmd1.Parameters.Add(new SqlParameter("@fmg_ids",
                           SqlDbType.VarChar)
                        {
                            Value = fmg_ids
                        });
                        cmd1.Parameters.Add(new SqlParameter("@fmt_ids",
                           SqlDbType.VarChar)
                        {
                            Value = fmt_ids
                        });
                        cmd1.Parameters.Add(new SqlParameter("@fillclasflg",
                           SqlDbType.VarChar)
                        {
                            Value = Convert.ToInt64(data.fillclasflg)
                        });
                        cmd1.Parameters.Add(new SqlParameter("@fillseccls",
                          SqlDbType.VarChar)
                        {
                            Value = Convert.ToInt64(data.fillseccls)
                        });
                        if (cmd1.Connection.State != ConnectionState.Open)
                            cmd1.Connection.Open();

                        try
                        {

                            using (var dataReader = await cmd1.ExecuteReaderAsync())
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

                                    retObject5.Add((ExpandoObject)dataRow);
                                }

                            }

                            data.headlist = retObject5.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }

                    data.fillbusroutestudents = _FeeGroupContext.feemastersettings.Single(d => d.MI_Id == data.MI_ID && d.userid == data.userid).MI_Logo;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return data;
        }



        public string duadatecollect(long asmay_id, long userid, long fillclasflg)
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




        public FeeArrearRegisterReportDTO get_groups(FeeArrearRegisterReportDTO data)
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
                                      select new FeeArrearRegisterReportDTO
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
                                      select new FeeArrearRegisterReportDTO
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


  