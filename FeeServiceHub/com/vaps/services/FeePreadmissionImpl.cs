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
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using System.IO;
using System.Net;
using System.Text;
using MimeKit;
using MailKit.Net.Smtp;
using DomainModel.Model.com.vaps.admission;
using CommonLibrary;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Data;
using Microsoft.SqlServer.Server;
using System.Globalization;
using System.Dynamic;
using Microsoft.AspNetCore.Http.Features;
using DomainModel.Model.com.vapstech.Fee;

namespace FeeServiceHub.com.vaps.services
{
    public class FeePreadmissionImpl : interfaces.FeePreadmissionInterface
    {
        private static ConcurrentDictionary<string, FeeStaffOthersTransactionDTO> _login =
       new ConcurrentDictionary<string, FeeStaffOthersTransactionDTO>();

        private static readonly Object obj = new Object();

        public FeeGroupContext _YearlyFeeGroupMappingContext;
        public DomainModelMsSqlServerContext _context;
        readonly ILogger<FeePreadmissionImpl> _logger;
        public FeePreadmissionImpl(FeeGroupContext YearlyFeeGroupMappingContext, DomainModelMsSqlServerContext context, ILogger<FeePreadmissionImpl> log)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _context = context;
            _logger = log;
        }

        public FeeStudentTransactionDTO getdata(FeeStudentTransactionDTO data)
        {
            try
            {

                var Acdemic_preadmission = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();


                data.ASMAY_Id = Acdemic_preadmission;


                List<FeeMasterConfigurationDMO> feemasnum = new List<FeeMasterConfigurationDMO>();
                feemasnum = _YearlyFeeGroupMappingContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id && t.userid == data.userid ).ToList();
                data.feeconfiglist = feemasnum.ToArray();

                List<FeeGroupDMO> group = new List<FeeGroupDMO>();
                group = _YearlyFeeGroupMappingContext.FeeGroupDMO.Where(t => t.FMG_ActiceFlag == true && t.MI_Id == data.MI_Id).ToList();
                data.fillmastergroup = group.ToArray();

                List<Master_Numbering> masnum = new List<Master_Numbering>();
                masnum = _YearlyFeeGroupMappingContext.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "Transaction").ToList();
                data.transnumconfig = masnum.ToArray();

                //var rolename = _YearlyFeeGroupMappingContext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == data.roleid).IVRMRT_Role;

                //data.rolename = rolename;

                bool recsettingval = false;
                string maxval = "";
                var getcurrsett = _YearlyFeeGroupMappingContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id && t.userid == data.userid).ToList();
                foreach (var value in getcurrsett)
                {
                    if (value.FMC_AutoReceiptFeeGroupFlag == 1)
                    {
                        recsettingval = true;
                    }
                    else
                    {
                        recsettingval = false;
                    }

                }

                if(data.filterinitialdata=="Preadmission")
                {
                    data.preadmissionstudentlist = (from a in _YearlyFeeGroupMappingContext.stuapp
                                                    from b in _YearlyFeeGroupMappingContext.admissioncls
                                                    where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PASR_Adm_Confirm_Flag == false)
                                                    select new FeeStudentTransactionDTO
                                                    {
                                                        PASR_Id = a.pasr_id,
                                                        PASR_FirstName = ((a.PASR_FirstName == null || a.PASR_FirstName == "" ? "" : " " + a.PASR_FirstName) + (a.PASR_MiddleName == null || a.PASR_MiddleName == "" || a.PASR_MiddleName == "0" ? "" : " " + a.PASR_MiddleName) + (a.PASR_LastName == null || a.PASR_LastName == "" || a.PASR_LastName == "0" ? "" : " " + a.PASR_LastName)).Trim(),
                                                        PASR_MiddleName = a.PASR_MiddleName,
                                                        PASR_LastName = a.PASR_LastName,
                                                        classname = b.ASMCL_ClassName,
                                                    }
  ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();


                    //var fetchmaxfypid = _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ApplicationDMO.OrderByDescending(t => t.FYP_Id).Take(5).Select(t => t.FYP_Id).ToList();


                    var fetchmaxfypidabc = (from a in _YearlyFeeGroupMappingContext.stuapp
                                        from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                         where (a.pasr_id==b.PASA_Id && a.MI_Id==data.MI_Id && a.ASMAY_Id==data.ASMAY_Id)
                                                    select new FeeStudentTransactionDTO
                                                    {
                                                        FYP_Id=b.FYP_Id
                                                    }
 ).Distinct().OrderByDescending(t => t.FYP_Id).ToList();

                    List<long> fetchmaxfypid = new List<long>();
                    foreach (var item in fetchmaxfypidabc)
                    {
                        fetchmaxfypid.Add(item.FYP_Id);
                    }

                    data.receiparraydelete = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                              from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                              from c in _YearlyFeeGroupMappingContext.stuapp
                                              from d in _YearlyFeeGroupMappingContext.admissioncls
                                              where (a.FYP_Id == b.FYP_Id && b.PASA_Id == c.pasr_id && c.ASMCL_Id == d.ASMCL_Id && fetchmaxfypid.Contains(a.FYP_Id) && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id)
                                              select new FeeStudentTransactionDTO
                                              {
                                                  PASR_Id = c.pasr_id,
                                                  PASR_FirstName = ((c.PASR_FirstName == null || c.PASR_FirstName == "" ? "" : " " + c.PASR_FirstName) + (c.PASR_FirstName == null || c.PASR_MiddleName == "" || c.PASR_MiddleName == "0" ? "" : " " + c.PASR_MiddleName) + (c.PASR_LastName == null || c.PASR_LastName == "" || c.PASR_LastName == "0" ? "" : " " + c.PASR_LastName)).Trim(),
                                                  PASR_MiddleName = c.PASR_MiddleName,
                                                  PASR_LastName = c.PASR_LastName,
                                                  FYP_Receipt_No = a.FYP_Receipt_No,
                                                  FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                  FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                  classname = d.ASMCL_ClassName,
                                                  FYP_Id = a.FYP_Id,
                                                  pasR_MobileNo = c.PASR_MobileNo,
                                                  FYP_Date = a.FYP_Date,
                                                  PASR_RegistrationNo = c.PASR_Applicationno,
                                                  pasR_emailId = c.PASR_emailId,
                                                  userid=a.user_id
                                              }
         ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();
                }
                else if(data.filterinitialdata=="Prospectus")
                {
                    data.preadmissionstudentlist = (from a in _YearlyFeeGroupMappingContext.Prospectus
                                                    from b in _YearlyFeeGroupMappingContext.admissioncls
                                                    where (a.ASMCL_Id == b.ASMCL_Id && a.MI_ID == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                                    select new FeeStudentTransactionDTO
                                                    {
                                                        PASR_Id = a.PASP_Id,
                                                        PASR_FirstName = a.PASP_First_Name,
                                                        PASR_MiddleName = a.PASP_Middle_Name,
                                                        PASR_LastName = a.PASP_Last_Name,
                                                        classname = b.ASMCL_ClassName,
                                                    }
  ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();


                    var fetchmaxfypid = _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ProspectusDMO.OrderByDescending(t => t.FYP_Id).Take(5).Select(t => t.FYP_Id).ToList();

                    data.receiparraydelete = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                              from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ProspectusDMO
                                              from c in _YearlyFeeGroupMappingContext.Prospectus
                                              from d in _YearlyFeeGroupMappingContext.admissioncls
                                              where (a.FYP_Id == b.FYP_Id && b.PASP_Id == c.PASP_Id && c.ASMCL_Id == d.ASMCL_Id && fetchmaxfypid.Contains(a.FYP_Id) && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id)
                                              select new FeeStudentTransactionDTO
                                              {
                                                  PASR_Id = c.PASP_Id,
                                                  PASR_FirstName = ((c.PASP_First_Name == null || c.PASP_First_Name == "" ? "" : " " + c.PASP_First_Name) + (c.PASP_Middle_Name == null || c.PASP_Middle_Name == "" || c.PASP_Middle_Name == "0" ? "" : " " + c.PASP_Middle_Name) + (c.PASP_Last_Name == null || c.PASP_Last_Name == "" || c.PASP_Last_Name == "0" ? "" : " " + c.PASP_Last_Name)).Trim(),
                                                  PASR_MiddleName = c.PASP_Middle_Name,
                                                  PASR_LastName = c.PASP_Last_Name,
                                                  FYP_Receipt_No = a.FYP_Receipt_No,
                                                  FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                  FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                  classname = d.ASMCL_ClassName,
                                                  FYP_Id = a.FYP_Id,
                                                  pasR_MobileNo = c.PASP_MobileNo,
                                                  FYP_Date = a.FYP_Date,
                                                  PASR_RegistrationNo = c.PASP_ProspectusNo,
                                                  pasR_emailId = c.PASP_EmailId,
                                                  userid = a.user_id
                                              }
         ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();
                }
               

                data.academicyrlist = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).ToArray();

                data.specialheaddetails = (from a in _YearlyFeeGroupMappingContext.feespecialHead
                                           from b in _YearlyFeeGroupMappingContext.feeSGGG
                                           from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                           where (a.MI_Id == data.MI_Id && a.FMSFH_ActiceFlag == true && a.FMSFH_Id == b.FMSFH_Id && b.FMSFHFH_ActiceFlag == true && c.MI_Id == data.MI_Id && c.FMH_ActiveFlag == true && c.FMH_Id == b.FMH_Id)//&& a.IVRMSTAUL_Id==data.User_Id
                                           select new FeeSpecialFeeGroupDTO
                                           {
                                               FMSFH_Id = a.FMSFH_Id,
                                               FMSFH_Name = a.FMSFH_Name,
                                               FMSFHFH_Id = b.FMSFHFH_Id,
                                               FMH_ID = b.FMH_Id,
                                               FMH_Name = c.FMH_FeeName
                                           }).Distinct().ToArray();

                var specialheadlist = _YearlyFeeGroupMappingContext.feespecialHead.Where(t => t.MI_Id == data.MI_Id && t.FMSFH_ActiceFlag == true).Distinct().ToList();
                data.specialheadlist = specialheadlist.ToArray();

              

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public FeeStudentTransactionDTO selectstu(FeeStudentTransactionDTO data)
        {
            try
            {
                string alltrids = "0";
                var Acdemic_preadmission = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                data.ASMAY_Id = Acdemic_preadmission;

                if (data.Grp_Term_flg == "T")
                {
                    data.termlist = (from a in _YearlyFeeGroupMappingContext.feeTr                                     from b in _YearlyFeeGroupMappingContext.feeMTH                                     from c in _YearlyFeeGroupMappingContext.feehead                                     from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO                                     where (a.MI_Id == data.MI_Id && a.FMT_ActiveFlag == true && a.FMT_Id == b.FMT_Id && b.FMH_Id == c.FMH_Id && (c.FMH_Flag == "N" || c.FMH_Flag == "R") && a.FMT_ActiveFlag == true && b.FTI_Id == d.FTI_Id && d.FTI_Active == true)
                                     select new FeeStaffOthersTransactionDTO                                     {                                         FMT_Id = a.FMT_Id,                                         FMT_Name = a.FMT_Name,                                         FMT_Order = a.FMT_Order                                     }).Distinct().OrderBy(T => T.FMT_Order).ToList().ToArray();                    var readterms = (from a in _YearlyFeeGroupMappingContext.feeTr                                     from b in _YearlyFeeGroupMappingContext.feeMTH                                     from c in _YearlyFeeGroupMappingContext.feehead                                     from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO                                     where (a.MI_Id == data.MI_Id && a.FMT_ActiveFlag == true && a.FMT_Id == b.FMT_Id && b.FMH_Id == c.FMH_Id && (c.FMH_Flag == "N" || c.FMH_Flag == "R") && a.FMT_ActiveFlag == true && b.FTI_Id == d.FTI_Id && d.FTI_Active == true)                                     select new FeeStaffOthersTransactionDTO                                     {                                         FMT_Id = a.FMT_Id,                                         FMT_Name = a.FMT_Name,                                         FMT_Order = a.FMT_Order                                     }).Distinct().ToArray();

                    for (int s = 0; s < readterms.Count(); s++)
                    {
                        alltrids = alltrids + ',' + readterms[s].FMT_Id.ToString();
                    }

                }

                if (data.Grp_Term_flg == "G")
                {
                    data.termlist = (from a in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                     from b in _YearlyFeeGroupMappingContext.Yearlygroups
                                     where (a.MI_Id==data.MI_Id && a.FMG_Id==b.FMG_Id && b.ASMAY_Id==data.ASMAY_Id && a.FMG_ActiceFlag==true)
                                     select new FeeStaffOthersTransactionDTO
                                     {
                                         FMG_Id = a.FMG_Id,
                                         FMG_GroupName = a.FMG_GroupName,

                                     }).Distinct().ToList().ToArray();
                }

                if(data.filterinitialdata=="Preadmission")
                {
                    data.preadmissionstudentlist = (from a in _YearlyFeeGroupMappingContext.stuapp
                                                    from b in _YearlyFeeGroupMappingContext.admissioncls
                                                    where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PASR_Adm_Confirm_Flag == false && a.pasr_id == data.PASR_Id)
                                                    select new FeeStudentTransactionDTO
                                                    {
                                                        PASR_Id = a.pasr_id,
                                                        PASR_FirstName = a.PASR_FirstName,
                                                        PASR_MiddleName = a.PASR_MiddleName,
                                                        PASR_LastName = a.PASR_LastName,
                                                        classname = b.ASMCL_ClassName,
                                                        PASR_RegistrationNo = a.PASR_Applicationno,
                                                        pasR_MobileNo = a.PASR_MobileNo,
                                                        pasR_emailId = a.PASR_emailId,
                                                        fathername = (a.PASR_FatherName + ' ' + (a.PASR_FatherSurname == null || a.PASR_FatherSurname == "0" ? "" : a.PASR_FatherSurname))
                                                    }
 ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();

                }
                else if(data.filterinitialdata=="Prospectus")
                {
                    data.preadmissionstudentlist = (from a in _YearlyFeeGroupMappingContext.Prospectus
                                                    from b in _YearlyFeeGroupMappingContext.admissioncls
                                                    where (a.ASMCL_Id == b.ASMCL_Id && a.MI_ID == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id  && a.PASP_Id == data.PASR_Id)
                                                    select new FeeStudentTransactionDTO
                                                    {
                                                        PASR_Id = a.PASP_Id,
                                                        PASR_FirstName = a.PASP_First_Name,
                                                        PASR_MiddleName = a.PASP_Middle_Name,
                                                        PASR_LastName = a.PASP_Last_Name,
                                                        classname = b.ASMCL_ClassName,
                                                        PASR_RegistrationNo = a.PASP_ProspectusNo,
                                                        pasR_MobileNo = a.PASP_MobileNo,
                                                        pasR_emailId = a.PASP_EmailId
                                                    }
).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();
                }

                try
                {
                    using (var cmd1 = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd1.CommandText = "gettermsstatisticPreadmissiondetails";
                        cmd1.CommandType = CommandType.StoredProcedure;

                        cmd1.Parameters.Add(new SqlParameter("@Asmay_id",
                         SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@Mi_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@amst_id",
                        SqlDbType.VarChar)
                        {
                            Value = data.PASR_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@fmtids",
                         SqlDbType.VarChar)
                        {
                            Value = alltrids
                        });

                        if (cmd1.Connection.State != ConnectionState.Open)
                            cmd1.Connection.Open();

                        var retObject1 = new List<dynamic>();
                        try
                        {
                            using (var dataReader1 = cmd1.ExecuteReader())
                            {
                                while (dataReader1.Read())
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
                            data.disableterms = retObject1.ToArray();
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public FeeStudentTransactionDTO selectgrouppterm(FeeStudentTransactionDTO data)
        {
            try
            {
                var Acdemic_preadmission = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();


                data.ASMAY_Id = Acdemic_preadmission;

                if(data.filterinitialdata=="Preadmission")
                {
                    var saved_fma = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                     from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                     from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                     where (a.FYP_Id == b.FYP_Id && a.PASA_Id == data.PASR_Id && c.FMA_Id == b.FMA_Id && b.FTP_Paid_Amt >= c.FMA_Amount)
                                     select b.FMA_Id
).Distinct().ToList();


                    data.feepaiddetails = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                           from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                           where (a.FYP_Id == b.FYP_Id && a.PASA_Id == data.PASR_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               FMA_Id = b.FMA_Id,
                                               FTP_Paid_Amt = b.FTP_Paid_Amt
                                           }
    ).Distinct().ToArray();

                    var fetchclass = (from a in _YearlyFeeGroupMappingContext.stuapp
                                      where (a.MI_Id == data.MI_Id && a.pasr_id == data.PASR_Id && a.ASMAY_Id == data.ASMAY_Id)
                                      select new FeeStudentTransactionDTO
                                      {
                                          ASMCL_ID = a.ASMCL_Id,
                                          ASMAY_Id = a.ASMAY_Id,
                                          //pasl_id=a.PASL_ID
                                          ASMST_Id = a.ASMST_Id
                                      }
    ).Distinct().ToArray();

                    string classid = "0", academicyearid = "0",streamid="0";
                    for (int s = 0; s < fetchclass.Count(); s++)
                    {
                        classid = fetchclass[s].ASMCL_ID.ToString();
                        academicyearid = fetchclass[s].ASMAY_Id.ToString();
                        streamid = fetchclass[s].ASMST_Id.ToString();
                    }


                    int grpset = 0;
                    var feemasnum = _YearlyFeeGroupMappingContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id).ToArray();
                    for (int s = 0; s < feemasnum.Count(); s++)
                    {
                        grpset = Convert.ToInt16(feemasnum[s].fee_group_setting);
                        //grpset = 1;
                    }

                    List<long> groupids = new List<long>();
                    groupids = _YearlyFeeGroupMappingContext.Fee_Master_Stream_Group_MappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.PASL_ID == Convert.ToInt64(streamid) && t.ASMCL_ID == Convert.ToInt64(classid)).Select(t => t.FMG_Id).ToList();

                    if (data.Grp_Term_flg.Equals("G"))
                    {
                        data.mapped_hds_ins = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                               from b in _YearlyFeeGroupMappingContext.feeMIY
                                               from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                               from d in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                               where (a.MI_Id == b.MI_ID && a.FMH_Id == c.FMH_Id && a.FMH_ActiveFlag == true && b.FTI_Id == c.FTI_Id && c.MI_Id == b.MI_ID && b.MI_ID == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(c.FMG_Id) && d.MI_Id == b.MI_ID && d.FMG_Id == c.FMG_Id && d.FMG_ActiceFlag == true && !saved_fma.Contains(c.FMA_Id) && ((c.FMA_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")) && (d.FMG_PRE_FLAG == "1" || d.FMG_CompulsoryFlag == "N"))
                                               select new Head_Installments_DTO
                                               {
                                                   FMA_Id = c.FMA_Id,
                                                   FMH_FeeName = a.FMH_FeeName,
                                                   FTI_Name = b.FTI_Name,
                                                   FMH_Id = c.FMH_Id,
                                                   FTI_Id = c.FTI_Id,
                                                   CurrentYrCharges = Convert.ToInt64(c.FMA_Amount),
                                                   TotalCharges = Convert.ToInt64(c.FMA_Amount),
                                                   ConcessionAmount = 0,
                                                   FineAmount = 0,
                                                   ToBePaid = Convert.ToInt64(c.FMA_Amount),
                                                   NetAmount = Convert.ToDecimal(c.FMA_Amount),
                                                   FMG_Id = c.FMG_Id,
                                                   FMG_GroupName = d.FMG_GroupName,
                                               }).Distinct().ToList().ToArray();
                        data.instalspecial = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                              from b in _YearlyFeeGroupMappingContext.feeMIY
                                              from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                              from d in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                              where (a.MI_Id == b.MI_ID && a.FMH_Id == c.FMH_Id && a.FMH_ActiveFlag == true && b.FTI_Id == c.FTI_Id && c.MI_Id == b.MI_ID && b.MI_ID == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(c.FMG_Id) && d.MI_Id == b.MI_ID && d.FMG_Id == c.FMG_Id && d.FMG_ActiceFlag == true && !saved_fma.Contains(c.FMA_Id) && ((c.FMA_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")))
                                              select new Head_Installments_DTO
                                              {
                                                  FTI_Name = b.FTI_Name,
                                                  FTI_Id = c.FTI_Id
                                              }).Distinct().ToList().ToArray();
                    }
                    else if (data.Grp_Term_flg.Equals("T"))
                    {
                        if (grpset.Equals(0))
                        {
                            data.mapped_hds_ins = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                   from d in _YearlyFeeGroupMappingContext.feeMIY
                                                   from b in _YearlyFeeGroupMappingContext.feeMTH
                                                   from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                                   from e in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                                   from f in _YearlyFeeGroupMappingContext.feeYCCC
                                                   from g in _YearlyFeeGroupMappingContext.feeYCC
                                                   where (a.FMH_Id == c.FMH_Id && e.FMG_Id == c.FMG_Id && c.FTI_Id == b.FTI_Id && d.FTI_Id == c.FTI_Id && b.FMH_Id == a.FMH_Id && e.FMG_ActiceFlag == true && f.FYCC_Id == g.FYCC_Id && g.FMCC_Id == c.FMCC_Id && f.ASMCL_Id == Convert.ToInt16(classid) && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(b.FMT_Id) && !saved_fma.Contains(c.FMA_Id) && ((c.FMA_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")) && (e.FMG_PRE_FLAG == "1" || e.FMG_CompulsoryFlag == "N" || e.FMG_CompulsoryFlag == "R") && g.ASMAY_Id == c.ASMAY_Id)
                                                   select new Head_Installments_DTO
                                                   {
                                                       FMA_Id = c.FMA_Id,
                                                       FMH_FeeName = a.FMH_FeeName,
                                                       FTI_Name = d.FTI_Name,
                                                       FMH_Id = c.FMH_Id,
                                                       FTI_Id = c.FTI_Id,
                                                       CurrentYrCharges = Convert.ToInt64(c.FMA_Amount),
                                                       TotalCharges = Convert.ToInt64(c.FMA_Amount),
                                                       ConcessionAmount = 0,
                                                       FineAmount = 0,
                                                       ToBePaid = Convert.ToInt64(c.FMA_Amount),
                                                       NetAmount = Convert.ToDecimal(c.FMA_Amount),
                                                       FMG_Id = c.FMG_Id,
                                                       FMG_GroupName = e.FMG_GroupName,
                                                       FMH_Order = a.FMH_Order
                                                   }).Distinct().OrderBy(t => t.FMH_Order).ToList().ToArray();
                        }
                        else
                        {

                            List<long> groupidsreg = new List<long>();
                            groupidsreg = _YearlyFeeGroupMappingContext.FeeGroupDMO.Where(t => t.MI_Id == data.MI_Id && t.FMG_CompulsoryFlag=="R").Select(t => t.FMG_Id).ToList();


                            data.mapped_hds_ins = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                   from d in _YearlyFeeGroupMappingContext.feeMIY
                                                   from b in _YearlyFeeGroupMappingContext.feeMTH
                                                   from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                                   from e in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                                   from f in _YearlyFeeGroupMappingContext.feeYCCC
                                                   from g in _YearlyFeeGroupMappingContext.feeYCC
                                                   where (a.FMH_Id == c.FMH_Id && e.FMG_Id == c.FMG_Id && c.FTI_Id == b.FTI_Id && d.FTI_Id == c.FTI_Id && b.FMH_Id == a.FMH_Id && e.FMG_ActiceFlag == true && f.FYCC_Id == g.FYCC_Id && g.FMCC_Id == c.FMCC_Id && f.ASMCL_Id == Convert.ToInt16(classid) && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(b.FMT_Id) && !saved_fma.Contains(c.FMA_Id) && g.ASMAY_Id == c.ASMAY_Id && (groupids.Contains(c.FMG_Id) || groupidsreg.Contains(c.FMG_Id)) && ((c.FMA_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")) )
                                                   select new Head_Installments_DTO
                                                   {
                                                       FMA_Id = c.FMA_Id,
                                                       FMH_FeeName = a.FMH_FeeName,
                                                       FTI_Name = d.FTI_Name,
                                                       FMH_Id = c.FMH_Id,
                                                       FTI_Id = c.FTI_Id,
                                                       CurrentYrCharges = Convert.ToInt64(c.FMA_Amount),
                                                       TotalCharges = Convert.ToInt64(c.FMA_Amount),
                                                       ConcessionAmount = 0,
                                                       FineAmount = 0,
                                                       ToBePaid = Convert.ToInt64(c.FMA_Amount),
                                                       NetAmount = Convert.ToDecimal(c.FMA_Amount),
                                                       FMG_Id = c.FMG_Id,
                                                       FMG_GroupName = e.FMG_GroupName,
                                                       FMH_Order = a.FMH_Order
                                                   }).Distinct().OrderBy(t => t.FMH_Order).ToList().ToArray();
                        }
                           

                        data.instalspecial = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                              from d in _YearlyFeeGroupMappingContext.feeMIY
                                              from b in _YearlyFeeGroupMappingContext.feeMTH
                                              from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                              from e in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                              from f in _YearlyFeeGroupMappingContext.feeYCCC
                                              from g in _YearlyFeeGroupMappingContext.feeYCC
                                              where (a.FMH_Id == c.FMH_Id && e.FMG_Id == c.FMG_Id && c.FTI_Id == b.FTI_Id && d.FTI_Id == c.FTI_Id && b.FMH_Id == a.FMH_Id && e.FMG_ActiceFlag == true && f.FYCC_Id == g.FYCC_Id && g.FMCC_Id == c.FMCC_Id && f.ASMCL_Id == Convert.ToInt16(classid) && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(b.FMT_Id) && !saved_fma.Contains(c.FMA_Id) && ((c.FMA_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")) && g.ASMAY_Id==c.ASMAY_Id)
                                              select new Head_Installments_DTO
                                              {
                                                  FTI_Name = d.FTI_Name,
                                                  FTI_Id = c.FTI_Id
                                              }).Distinct().ToList().ToArray();
                    }

                    if (data.mapped_hds_ins.Length > 0)
                    {

                        var count_res = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                         from d in _YearlyFeeGroupMappingContext.feeMIY
                                         from b in _YearlyFeeGroupMappingContext.feeMTH
                                         from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                         from e in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                         from f in _YearlyFeeGroupMappingContext.feeYCCC
                                         from g in _YearlyFeeGroupMappingContext.feeYCC
                                         where (a.FMH_Id == c.FMH_Id && e.FMG_Id == c.FMG_Id && c.FTI_Id == b.FTI_Id && d.FTI_Id == c.FTI_Id && b.FMH_Id == a.FMH_Id && e.FMG_ActiceFlag == true && f.FYCC_Id == g.FYCC_Id && g.FMCC_Id == c.FMCC_Id && f.ASMCL_Id == Convert.ToInt16(classid) && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(b.FMT_Id) && !saved_fma.Contains(c.FMA_Id) && (e.FMG_CompulsoryFlag=="R" || e.FMG_PRE_FLAG=="1") && g.ASMAY_Id==c.ASMAY_Id)
                                         select new
                                         {
                                             FMG_Id = e.FMG_Id,
                                             FMH_Id = a.FMH_Id

                                         }).Distinct().GroupBy(id => id.FMG_Id).Select(g => new FeeStudentTransactionDTO { FMG_Id = g.Key, grp_count = g.Count() }).OrderByDescending(t => t.grp_count).ToList();

                        //var count_res1 = _YearlyFeeGroupMappingContext.V_StudentPendingDMO.Where(r => r.mi_id == data.MI_Id).Select(r => r.fmg_id).GroupBy(id => id).OrderByDescending(id => id.Count()).Select(g => new FeeStudentTransactionDTO { FMG_Id = g.Key, grp_count = g.Count() }).First();

                        //var count_res = _YearlyFeeGroupMappingContext.V_StudentPendingDMO.Where(r => r.mi_id == data.MI_Id).Select(r => r.fmg_id).GroupBy(id => id).OrderByDescending(id => id.Count()).Select(g => new FeeStudentTransactionDTO { FMG_Id = g.Key, grp_count = g.Count() }).First();

                        data.validationgroupid = count_res[0].FMG_Id;
                        data.validationgrougidcount = count_res[0].grp_count;
                    }
                }

                else if(data.filterinitialdata=="Prospectus")
                {
                    var saved_fma = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ProspectusDMO
                                     from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                     from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                     where (a.FYP_Id == b.FYP_Id && a.PASP_Id == data.PASR_Id && c.FMA_Id == b.FMA_Id && b.FTP_Paid_Amt >= c.FMA_Amount)
                                     select b.FMA_Id
).Distinct().ToList();


                    data.feepaiddetails = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ProspectusDMO
                                           from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                           where (a.FYP_Id == b.FYP_Id && a.PASP_Id == data.PASR_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               FMA_Id = b.FMA_Id,
                                               FTP_Paid_Amt = b.FTP_Paid_Amt
                                           }
    ).Distinct().ToArray();

                    var fetchclass = (from a in _YearlyFeeGroupMappingContext.Prospectus
                                      where (a.MI_ID == data.MI_Id && a.PASP_Id == data.PASR_Id && a.ASMAY_Id == data.ASMAY_Id)
                                      select new FeeStudentTransactionDTO
                                      {
                                          ASMCL_ID = a.ASMCL_Id,
                                          ASMAY_Id = a.ASMAY_Id
                                      }
    ).Distinct().ToArray();

                    string classid = "0", academicyearid = "0";
                    for (int s = 0; s < fetchclass.Count(); s++)
                    {
                        classid = fetchclass[s].ASMCL_ID.ToString();
                        academicyearid = fetchclass[s].ASMAY_Id.ToString();
                    }

                    if (data.Grp_Term_flg.Equals("G"))
                    {
                        data.mapped_hds_ins = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                               from b in _YearlyFeeGroupMappingContext.feeMIY
                                               from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                               from d in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                               where (a.MI_Id == b.MI_ID && a.FMH_Id == c.FMH_Id && a.FMH_ActiveFlag == true && b.FTI_Id == c.FTI_Id && c.MI_Id == b.MI_ID && b.MI_ID == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(c.FMG_Id) && d.MI_Id == b.MI_ID && d.FMG_Id == c.FMG_Id && d.FMG_ActiceFlag == true && !saved_fma.Contains(c.FMA_Id) && ((c.FMA_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")) && (d.FMG_PRE_FLAG == "1" || d.FMG_CompulsoryFlag == "N"))
                                               select new Head_Installments_DTO
                                               {
                                                   FMA_Id = c.FMA_Id,
                                                   FMH_FeeName = a.FMH_FeeName,
                                                   FTI_Name = b.FTI_Name,
                                                   FMH_Id = c.FMH_Id,
                                                   FTI_Id = c.FTI_Id,
                                                   CurrentYrCharges = Convert.ToInt64(c.FMA_Amount),
                                                   TotalCharges = Convert.ToInt64(c.FMA_Amount),
                                                   ConcessionAmount = 0,
                                                   FineAmount = 0,
                                                   ToBePaid = Convert.ToInt64(c.FMA_Amount),
                                                   NetAmount = Convert.ToDecimal(c.FMA_Amount),
                                                   FMG_Id = c.FMG_Id,
                                                   FMG_GroupName = d.FMG_GroupName,
                                               }).Distinct().ToList().ToArray();
                        data.instalspecial = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                              from b in _YearlyFeeGroupMappingContext.feeMIY
                                              from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                              from d in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                              where (a.MI_Id == b.MI_ID && a.FMH_Id == c.FMH_Id && a.FMH_ActiveFlag == true && b.FTI_Id == c.FTI_Id && c.MI_Id == b.MI_ID && b.MI_ID == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(c.FMG_Id) && d.MI_Id == b.MI_ID && d.FMG_Id == c.FMG_Id && d.FMG_ActiceFlag == true && !saved_fma.Contains(c.FMA_Id) && ((c.FMA_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")))
                                              select new Head_Installments_DTO
                                              {
                                                  FTI_Name = b.FTI_Name,
                                                  FTI_Id = c.FTI_Id
                                              }).Distinct().ToList().ToArray();
                    }
                    else if (data.Grp_Term_flg.Equals("T"))
                    {
                        data.mapped_hds_ins = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                               from d in _YearlyFeeGroupMappingContext.feeMIY
                                               from b in _YearlyFeeGroupMappingContext.feeMTH
                                               from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                               from e in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                               from f in _YearlyFeeGroupMappingContext.feeYCCC
                                               from g in _YearlyFeeGroupMappingContext.feeYCC
                                               where (a.FMH_Id == c.FMH_Id && e.FMG_Id == c.FMG_Id && c.FTI_Id == b.FTI_Id && d.FTI_Id == c.FTI_Id && b.FMH_Id == a.FMH_Id && e.FMG_ActiceFlag == true && f.FYCC_Id == g.FYCC_Id && g.FMCC_Id == c.FMCC_Id && f.ASMCL_Id == Convert.ToInt16(classid) && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(b.FMT_Id) && !saved_fma.Contains(c.FMA_Id) && ((c.FMA_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")) && (e.FMG_PRE_FLAG == "1" || e.FMG_CompulsoryFlag == "N" || e.FMG_CompulsoryFlag == "P") && g.ASMAY_Id==c.ASMAY_Id)
                                               select new Head_Installments_DTO
                                               {
                                                   FMA_Id = c.FMA_Id,
                                                   FMH_FeeName = a.FMH_FeeName,
                                                   FTI_Name = d.FTI_Name,
                                                   FMH_Id = c.FMH_Id,
                                                   FTI_Id = c.FTI_Id,
                                                   CurrentYrCharges = Convert.ToInt64(c.FMA_Amount),
                                                   TotalCharges = Convert.ToInt64(c.FMA_Amount),
                                                   ConcessionAmount = 0,
                                                   FineAmount = 0,
                                                   ToBePaid = Convert.ToInt64(c.FMA_Amount),
                                                   NetAmount = Convert.ToDecimal(c.FMA_Amount),
                                                   FMG_Id = c.FMG_Id,
                                                   FMG_GroupName = e.FMG_GroupName,
                                                   FMH_Order = a.FMH_Order
                                               }).Distinct().OrderBy(t => t.FMH_Order).ToList().ToArray();
                        data.instalspecial = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                              from d in _YearlyFeeGroupMappingContext.feeMIY
                                              from b in _YearlyFeeGroupMappingContext.feeMTH
                                              from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                              from e in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                              from f in _YearlyFeeGroupMappingContext.feeYCCC
                                              from g in _YearlyFeeGroupMappingContext.feeYCC
                                              where (a.FMH_Id == c.FMH_Id && e.FMG_Id == c.FMG_Id && c.FTI_Id == b.FTI_Id && d.FTI_Id == c.FTI_Id && b.FMH_Id == a.FMH_Id && e.FMG_ActiceFlag == true && f.FYCC_Id == g.FYCC_Id && g.FMCC_Id == c.FMCC_Id && f.ASMCL_Id == Convert.ToInt16(classid) && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(b.FMT_Id) && !saved_fma.Contains(c.FMA_Id) && ((c.FMA_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")) && g.ASMAY_Id==c.ASMAY_Id)
                                              select new Head_Installments_DTO
                                              {
                                                  FTI_Name = d.FTI_Name,
                                                  FTI_Id = c.FTI_Id
                                              }).Distinct().ToList().ToArray();
                    }

                    if (data.mapped_hds_ins.Length > 0)
                    {
                        var count_res = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                         from d in _YearlyFeeGroupMappingContext.feeMIY
                                         from b in _YearlyFeeGroupMappingContext.feeMTH
                                         from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                         from e in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                         from f in _YearlyFeeGroupMappingContext.feeYCCC
                                         from g in _YearlyFeeGroupMappingContext.feeYCC
                                         where (a.FMH_Id == c.FMH_Id && e.FMG_Id == c.FMG_Id && c.FTI_Id == b.FTI_Id && d.FTI_Id == c.FTI_Id && b.FMH_Id == a.FMH_Id && e.FMG_ActiceFlag == true && f.FYCC_Id == g.FYCC_Id && g.FMCC_Id == c.FMCC_Id && f.ASMCL_Id == Convert.ToInt16(classid) && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(b.FMT_Id) && !saved_fma.Contains(c.FMA_Id) && e.FMG_CompulsoryFlag=="P" && g.ASMAY_Id==c.ASMAY_Id)
                                         select new
                                         {
                                             FMG_Id = e.FMG_Id,
                                             FMH_Id = a.FMH_Id

                                         }).Distinct().GroupBy(id => id.FMG_Id).Select(g => new FeeStudentTransactionDTO { FMG_Id = g.Key, grp_count = g.Count() }).OrderByDescending(t => t.grp_count).ToList();

                        //var count_res = _YearlyFeeGroupMappingContext.V_StudentPendingDMO.Where(r => r.mi_id == data.MI_Id).Select(r => r.fmg_id).GroupBy(id => id).OrderByDescending(id => id.Count()).Select(g => new FeeStudentTransactionDTO { FMG_Id = g.Key, grp_count = g.Count() }).First();

                        data.validationgroupid = count_res[0].FMG_Id;
                        data.validationgrougidcount = count_res[0].grp_count;
                    }
                }

               
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeStudentTransactionDTO savedata(FeeStudentTransactionDTO pgmod)
        {
            try
            {
                string headflg = "";
                var Acdemic_preadmission = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == pgmod.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                pgmod.ASMAY_Id = Acdemic_preadmission;

                if(pgmod.filterinitialdata=="Preadmission")
                {
                    FeePaymentDetailsDMO obj_pay = new FeePaymentDetailsDMO();

                    pgmod.FYP_Chq_Bounce = "CL";

                    obj_pay.FYP_Bank_Name = pgmod.FYP_Bank_Name;
                    obj_pay.FYP_Bank_Or_Cash = pgmod.FYP_Bank_Or_Cash;
                    obj_pay.FYP_DD_Cheque_No = pgmod.FYP_DD_Cheque_No;
                    obj_pay.FYP_DD_Cheque_Date = pgmod.FYP_DD_Cheque_Date;
                    obj_pay.FYP_Date = pgmod.FYP_Date;
                    obj_pay.FYP_Tot_Amount = pgmod.FYP_Tot_Amount;
                    obj_pay.FYP_Tot_Waived_Amt = pgmod.FYP_Tot_Waived_Amt;
                    obj_pay.FYP_Tot_Fine_Amt = pgmod.FYP_Tot_Fine_Amt;
                    obj_pay.FYP_Tot_Concession_Amt = pgmod.FYP_Tot_Concession_Amt;
                    obj_pay.FYP_Chq_Bounce = pgmod.FYP_Chq_Bounce;
                    obj_pay.MI_Id = pgmod.MI_Id;
                    obj_pay.FYP_Remarks = pgmod.FYP_Remarks;
                    obj_pay.ASMAY_ID = pgmod.ASMAY_Id;
                    obj_pay.FTCU_Id = 1;
                    obj_pay.FYP_Tot_Waived_Amt = 0;
                    obj_pay.FYP_Chq_Bounce = "CL";
                    obj_pay.DOE = DateTime.Now;
                    obj_pay.CreatedDate = DateTime.Now;
                    obj_pay.UpdatedDate = DateTime.Now;
                    //obj_pay.user_id = pgmod.userid;
                    obj_pay.FYP_OnlineChallanStatusFlag = "Sucessfull";
                    obj_pay.FYP_PayModeType = "APP";
                    obj_pay.FYP_PayGatewayType = "";


                    //added on 02-07-2018

                    List<long> HeadId = new List<long>();
                    List<long> FMGID = new List<long>();
                    foreach (var item in pgmod.head_installments)
                    {
                        HeadId.Add(item.FMH_Id);
                        FMGID.Add(item.FMG_Id);
                    }

                    List<FeeStudentTransactionDTO> grps = new List<FeeStudentTransactionDTO>();
                    grps = (from b in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO

                            where (b.MI_Id == pgmod.MI_Id && b.ASMAY_Id == pgmod.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id) && FMGID.Contains(b.FMG_Id))

                            select new FeeStudentTransactionDTO
                            {
                                FMG_Id = b.FMG_Id
                            }
                           ).Distinct().ToList();

                    List<long> grpid = new List<long>();
                    string groupidss = "0";
                    foreach (var item in grps)
                    {
                        grpid.Add(item.FMG_Id);
                    }

                    var Euserid = (from a in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                   where (a.MI_Id == Convert.ToInt64(pgmod.MI_Id) && grpid.Contains(a.FMG_Id))
                                   select new FeeStudentTransactionDTO
                                   {
                                       enduserid = a.user_id,
                                   }
                       ).Distinct().Take(1).ToArray();
                    //added on 02-07-2018

                    obj_pay.user_id = Convert.ToInt64(Euserid[0].enduserid);

                    get_grp_reptno(pgmod);

                    obj_pay.FYP_Receipt_No = pgmod.FYP_Receipt_No;

                    _YearlyFeeGroupMappingContext.Add(obj_pay);

                    //multimode of payment
                    Fee_Y_Payment_PaymentModeSchool onlinemulti = new Fee_Y_Payment_PaymentModeSchool();
                    onlinemulti.FYP_Id = obj_pay.FYP_Id;
                    onlinemulti.FYP_TransactionTypeFlag = pgmod.FYP_Bank_Or_Cash;
                    onlinemulti.FYPPM_TotalPaidAmount = pgmod.FYP_Tot_Amount;
                    onlinemulti.FYPPM_LedgerId = 0;
                    onlinemulti.FYPPM_BankName = pgmod.FYP_Bank_Name;
                    onlinemulti.FYPPM_DDChequeNo = pgmod.FYP_DD_Cheque_No;
                    onlinemulti.FYPPM_DDChequeDate = pgmod.FYP_DD_Cheque_Date;
                    onlinemulti.FYPPM_TransactionId = "";
                    onlinemulti.FYPPM_PaymentReferenceId = "";
                    onlinemulti.FYPPM_ClearanceStatusFlag = "0";
                    onlinemulti.FYPPM_ClearanceDate = pgmod.FYP_DD_Cheque_Date;
                    _YearlyFeeGroupMappingContext.Add(onlinemulti);
                    //multimode of payment

                    if (pgmod.FYP_Receipt_No == "" || pgmod.FYP_Receipt_No == null)
                    {
                        pgmod.returnval = "Record Not Saved because Receipt No is not Generated Automatic.Settings are missing";
                        return pgmod;
                    }
                    else
                    {
                        Fee_Y_Payment_Preadmission_ApplicationDMO obj_pay_stf = new Fee_Y_Payment_Preadmission_ApplicationDMO();
                        var getdetails1 = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                           from b in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                           where (a.FMH_Id == b.FMH_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id && a.FMA_Id == pgmod.head_installments[0].FMA_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               headflag = b.FMH_Flag,
                                               headorder = b.FMH_Order
                                           }
          ).Distinct().OrderBy(t => t.headorder).ToList();

                        if (getdetails1.Count > 0)
                        {
                            headflg = getdetails1.FirstOrDefault().headflag;
                            if (headflg != "R")
                            {
                                headflg = "A";
                            }
                            else
                            {
                                headflg = "R";
                            }
                        }
                        else if (getdetails1.Count == 0)
                        {
                            pgmod.returnval = "Kindly contact Administrator!!";
                            return pgmod;
                        }

                        obj_pay_stf.FYP_Id = obj_pay.FYP_Id;
                        obj_pay_stf.PASA_Id = pgmod.PASR_Id;
                        obj_pay_stf.FYPPA_Type = headflg;
                        obj_pay_stf.FYPPA_TotalPaidAmount = Convert.ToInt64(obj_pay.FYP_Tot_Amount);

                        _YearlyFeeGroupMappingContext.Add(obj_pay_stf);

                        for (int l = 0; l < pgmod.head_installments.Length; l++)
                        {
                            if (pgmod.head_installments[l].ToBePaid > 0)
                            {
                                FeeTransactionPaymentDMO obj_trans_st = new FeeTransactionPaymentDMO();
                                obj_trans_st.FYP_Id = obj_pay.FYP_Id;
                                obj_trans_st.FMA_Id = pgmod.head_installments[l].FMA_Id;
                                obj_trans_st.FTP_Paid_Amt = pgmod.head_installments[l].ToBePaid;
                                obj_trans_st.FTP_Waived_Amt = 0;
                                obj_trans_st.FTP_Concession_Amt = pgmod.head_installments[l].ConcessionAmount;
                                obj_trans_st.FTP_Fine_Amt = pgmod.head_installments[l].FineAmount;
                                obj_trans_st.ftp_remarks = obj_pay.FYP_Remarks;

                                _YearlyFeeGroupMappingContext.Add(obj_trans_st);
                            }
                        }

                        string paystatusflag = "Pay";

                        var feepartialpaymentflag = _YearlyFeeGroupMappingContext.feemastersettings.Where(t=>t.MI_Id == pgmod.MI_Id).Select(d => d.FMC_Partial_Pre_Payment_flag).FirstOrDefault();

                        var feepartialpaymentflagamst = _YearlyFeeGroupMappingContext.stuapp.Where(t => t.pasr_id == pgmod.PASR_Id).Select(d => d.ASMCL_Id).FirstOrDefault();

                        if (headflg != "R" && feepartialpaymentflag!="P")
                        {
                            var resultupda = _YearlyFeeGroupMappingContext.stuapp.Where(t => t.pasr_id == Convert.ToInt64(pgmod.PASR_Id)).ToArray();

                            resultupda[0].PASR_FinalpaymentFlag = 1;
                            _YearlyFeeGroupMappingContext.Update(resultupda[0]);
                        }

                        var contactexisttransaction = 0;
                        using (var dbCtxTxn = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                        {
                            try
                            {
                                contactexisttransaction = _YearlyFeeGroupMappingContext.SaveChanges();
                                dbCtxTxn.Commit();
                                pgmod.returnval = "Save";

                                string MailId = "";
                                long mobileno = 0;
                                var getdetails = (from a in _YearlyFeeGroupMappingContext.stuapp
                                                  where (a.pasr_id == pgmod.PASR_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id)
                                                  select new FeeStudentTransactionDTO
                                                  {
                                                      amst_mobile = a.PASR_MobileNo,
                                                      amst_email_id = a.PASR_emailId
                                                  }
           ).ToList();


                                mobileno = getdetails.FirstOrDefault().amst_mobile;
                                MailId = getdetails.FirstOrDefault().amst_email_id;

                                if (mobileno != 0)
                                {
                                    SMS sms = new SMS(_context);
                                }

                                if (MailId != "" || MailId != null)
                                {
                                    Email Email = new Email(_context);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                dbCtxTxn.Rollback();
                                pgmod.returnval = "Cancel";
                            }
                        }

                        if (feepartialpaymentflag == "P")
                        {

                            var termidssss = _YearlyFeeGroupMappingContext.feeTr.Where(t => t.MI_Id == pgmod.MI_Id && t.FMT_Order == 1).Select(d => d.FMT_Id).FirstOrDefault();

                            var netamount = (from a in _YearlyFeeGroupMappingContext.Fee_OnlinePayment_Mapping
                                             from b in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                             from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                             from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                             from f in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                             from g in _YearlyFeeGroupMappingContext.feeYCC
                                             from h in _YearlyFeeGroupMappingContext.feeYCCC
                                             from i in _YearlyFeeGroupMappingContext.Yearlygroups
                                             where (b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == pgmod.MI_Id && b.ASMAY_Id == pgmod.ASMAY_Id && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(feepartialpaymentflagamst) && f.FMG_Id == i.FMG_Id && i.FMG_Id == a.fmg_id && b.ASMAY_Id == i.ASMAY_Id && a.fmt_id == termidssss && g.ASMAY_Id == b.ASMAY_Id)
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_NetAmount = Convert.ToDecimal(b.FMA_Amount),
                                             }
            ).Sum(t => t.FSS_NetAmount);

                            var paidamount = (from a in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                              from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                              where (a.FYP_Id == b.FYP_Id && b.PASA_Id == pgmod.PASR_Id)
                                              select new FeeStudentTransactionDTO
                                              {
                                                  FSS_PaidAmount = Convert.ToInt64(a.FTP_Paid_Amt),
                                              }
           ).Sum(t => t.FSS_PaidAmount);

                            if (Convert.ToInt64(paidamount) >= Convert.ToInt64(netamount))
                            {
                                paystatusflag = "Pay";
                            }
                            else
                            {
                                paystatusflag = "pending";
                            }


                            if (paystatusflag == "Pay")
                            {
                                var resultupda = _YearlyFeeGroupMappingContext.stuapp.Where(t => t.pasr_id == Convert.ToInt64(pgmod.PASR_Id)).ToArray();

                                resultupda[0].PASR_FinalpaymentFlag = 1;
                                _YearlyFeeGroupMappingContext.Update(resultupda[0]);
                                _YearlyFeeGroupMappingContext.SaveChanges();
                            }

                        }

                    }
                }

                else if(pgmod.filterinitialdata=="Prospectus")
                {
                     FeePaymentDetailsDMO obj_pay = new FeePaymentDetailsDMO();

                    pgmod.FYP_Chq_Bounce = "CL";

                    obj_pay.FYP_Bank_Name = pgmod.FYP_Bank_Name;
                    obj_pay.FYP_Bank_Or_Cash = pgmod.FYP_Bank_Or_Cash;
                    obj_pay.FYP_DD_Cheque_No = pgmod.FYP_DD_Cheque_No;
                    obj_pay.FYP_DD_Cheque_Date = pgmod.FYP_DD_Cheque_Date;
                    obj_pay.FYP_Date = pgmod.FYP_Date;
                    obj_pay.FYP_Tot_Amount = pgmod.FYP_Tot_Amount;
                    obj_pay.FYP_Tot_Waived_Amt = pgmod.FYP_Tot_Waived_Amt;
                    obj_pay.FYP_Tot_Fine_Amt = pgmod.FYP_Tot_Fine_Amt;
                    obj_pay.FYP_Tot_Concession_Amt = pgmod.FYP_Tot_Concession_Amt;
                    obj_pay.FYP_Chq_Bounce = pgmod.FYP_Chq_Bounce;
                    obj_pay.MI_Id = pgmod.MI_Id;
                    obj_pay.FYP_Remarks = pgmod.FYP_Remarks;
                    obj_pay.ASMAY_ID = pgmod.ASMAY_Id;
                    obj_pay.FTCU_Id = 1;
                    obj_pay.FYP_Tot_Waived_Amt = 0;
                    obj_pay.FYP_Chq_Bounce = "CL";
                    obj_pay.DOE = DateTime.Now;
                    obj_pay.CreatedDate = DateTime.Now;
                    obj_pay.UpdatedDate = DateTime.Now;
                    obj_pay.user_id = pgmod.userid;
                    obj_pay.FYP_PayModeType = "APP";
                    obj_pay.FYP_PayGatewayType = "";

                    get_grp_reptno(pgmod);

                    obj_pay.FYP_Receipt_No = pgmod.FYP_Receipt_No;

                    _YearlyFeeGroupMappingContext.Add(obj_pay);

                    //multimode of payment
                    Fee_Y_Payment_PaymentModeSchool onlinemulti = new Fee_Y_Payment_PaymentModeSchool();
                    onlinemulti.FYP_Id = obj_pay.FYP_Id;
                    onlinemulti.FYP_TransactionTypeFlag = pgmod.FYP_Bank_Or_Cash;
                    onlinemulti.FYPPM_TotalPaidAmount = pgmod.FYP_Tot_Amount;
                    onlinemulti.FYPPM_LedgerId = 0;
                    onlinemulti.FYPPM_BankName = pgmod.FYP_Bank_Name;
                    onlinemulti.FYPPM_DDChequeNo = pgmod.FYP_DD_Cheque_No;
                    onlinemulti.FYPPM_DDChequeDate = pgmod.FYP_DD_Cheque_Date;
                    onlinemulti.FYPPM_TransactionId = "";
                    onlinemulti.FYPPM_PaymentReferenceId = "";
                    onlinemulti.FYPPM_ClearanceStatusFlag = "0";
                    onlinemulti.FYPPM_ClearanceDate = pgmod.FYP_DD_Cheque_Date;
                    _YearlyFeeGroupMappingContext.Add(onlinemulti);
                    //multimode of payment

                    if (pgmod.FYP_Receipt_No == "" || pgmod.FYP_Receipt_No == null)
                    {
                        pgmod.returnval = "Record Not Saved because Receipt No is not Generated Automatic.Settings are missing";
                        return pgmod;
                    }
                    else
                    {
                    Payment_PA_Prospectus obj_pay_stf = new Payment_PA_Prospectus();
                    var getdetails1 = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                       from b in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                       where (a.FMH_Id == b.FMH_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id && a.FMA_Id== pgmod.head_installments[0].FMA_Id)
                                       select new FeeStudentTransactionDTO
                                       {
                                           headflag = b.FMH_Flag,
                                           headorder = b.FMH_Order
                                       }
      ).Distinct().OrderBy(t=>t.headorder).ToList();

                    if(getdetails1.Count>0)
                    {
                            headflg = "P";
                    }
                    else if (getdetails1.Count == 0)
                    {
                        pgmod.returnval = "Kindly contact Administrator!!";
                        return pgmod;
                    }
                  
                        obj_pay_stf.FYP_Id = obj_pay.FYP_Id;
                        obj_pay_stf.PASP_Id = pgmod.PASR_Id;
                        obj_pay_stf.FYPPP_ActiveFlag = true;
                        obj_pay_stf.FYPPP_TotalPaidAmount = Convert.ToInt64(obj_pay.FYP_Tot_Amount);

                        _YearlyFeeGroupMappingContext.Add(obj_pay_stf);

                        for (int l = 0; l < pgmod.head_installments.Length; l++)
                        {
                            if (pgmod.head_installments[l].ToBePaid > 0)
                            {
                                FeeTransactionPaymentDMO obj_trans_st = new FeeTransactionPaymentDMO();
                                obj_trans_st.FYP_Id = obj_pay.FYP_Id;
                                obj_trans_st.FMA_Id = pgmod.head_installments[l].FMA_Id;
                                obj_trans_st.FTP_Paid_Amt = pgmod.head_installments[l].ToBePaid;
                                obj_trans_st.FTP_Waived_Amt = 0;
                                obj_trans_st.FTP_Concession_Amt = pgmod.head_installments[l].ConcessionAmount;
                                obj_trans_st.FTP_Fine_Amt = pgmod.head_installments[l].FineAmount;
                                obj_trans_st.ftp_remarks = obj_pay.FYP_Remarks;

                                _YearlyFeeGroupMappingContext.Add(obj_trans_st);
                            }
                        }
                  
                    var contactexisttransaction = 0;
                        using (var dbCtxTxn = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                        {
                            try
                            {
                                contactexisttransaction = _YearlyFeeGroupMappingContext.SaveChanges();
                                dbCtxTxn.Commit();
                                pgmod.returnval = "Save";

                                string MailId = "";
                                long mobileno = 0;
                                var getdetails = (from a in _YearlyFeeGroupMappingContext.Prospectus
                                                  where (a.PASP_Id == pgmod.PASR_Id && a.MI_ID == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id)
                                                  select new FeeStudentTransactionDTO
                                                  {
                                                      amst_mobile = a.PASP_MobileNo,
                                                      amst_email_id = a.PASP_EmailId
                                                  }
           ).ToList();


                                mobileno = getdetails.FirstOrDefault().amst_mobile;
                                MailId = getdetails.FirstOrDefault().amst_email_id;

                                if (mobileno != 0)
                                {
                                    SMS sms = new SMS(_context);
                                }

                                if (MailId != "" || MailId != null)
                                {
                                    Email Email = new Email(_context);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                dbCtxTxn.Rollback();
                                pgmod.returnval = "Cancel";
                            }
                        }
                    
                }
                }
            }

            catch (Exception ee)
            {
                pgmod.returnval = "Error";
                Console.WriteLine(ee.Message);
                _logger.LogError(ee.Message);
            }

            return pgmod;
        }

        public FeeStudentTransactionDTO get_grp_reptno(FeeStudentTransactionDTO data)
        {
            try
            {
                if (data.auto_receipt_flag == 1)
                {
                    List<long> HeadId = new List<long>();
                    List<long> FMGID = new List<long>();
                    foreach (var item in data.head_installments)
                    {
                        HeadId.Add(item.FMH_Id);
                        FMGID.Add(item.FMG_Id);
                    }

                    List<FeeStudentTransactionDTO> grps = new List<FeeStudentTransactionDTO>();
                    grps = (from b in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO

                            where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id) && FMGID.Contains(b.FMG_Id))

                            select new FeeStudentTransactionDTO
                            {
                                FMG_Id = b.FMG_Id
                            }
                           ).Distinct().ToList();

                    List<long> grpid = new List<long>();
                    string groupidss = "0";
                    foreach (var item in grps)
                    {
                        grpid.Add(item.FMG_Id);
                    }

                    for (int r = 0; r < grpid.Count(); r++)
                    {
                        groupidss = groupidss + ',' + grpid[r];
                    }

                    var final_rept_no = "";
                    List<FeeStudentTransactionDTO> list_all = new List<FeeStudentTransactionDTO>();
                    List<FeeStudentTransactionDTO> list_repts = new List<FeeStudentTransactionDTO>();

                    list_all = (from b in _YearlyFeeGroupMappingContext.Fee_Groupwise_AutoReceiptDMO
                                from c in _YearlyFeeGroupMappingContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                                select new FeeStudentTransactionDTO
                                {
                                    FGAR_PrefixName = b.FGAR_PrefixName,
                                    FGAR_SuffixName = b.FGAR_SuffixName,
                                    //FGAR_Name = b.FGAR_Name,
                                    //FMG_Id = c.FMG_Id
                                }
                         ).Distinct().ToList();

                    data.grp_count = list_all.Count();

                    if (data.grp_count == 1)
                    {
                        //list_repts = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                        //              from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                        //              from c in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                        //              where (a.MI_Id == c.MI_Id && c.ASMAY_Id == a.ASMAY_ID && a.ASMAY_ID == b.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && grpid.Contains(c.FMG_Id) && a.FYP_Receipt_No.StartsWith(list_all[0].FGAR_PrefixName) && a.FYP_Receipt_No.EndsWith(list_all[0].FGAR_SuffixName))
                        //              select new FeeStudentTransactionDTO
                        //              {
                        //                  FYP_Receipt_No = a.FYP_Receipt_No
                        //              }
                        //           ).Distinct().ToList();

                        //var prefix = list_all[0].FGAR_PrefixName;
                        //var prefix_len = prefix.Length;
                        //var suffix = list_all[0].FGAR_SuffixName;
                        //var suffix_len = suffix.Length;

                        //var max_num = "";
                        //var rept_no = "";
                        //var rept_prefix = "";
                        //var rept_suffix = "";
                        //var max_num_int = 0;
                        //List<int> number_max = new List<int>();
                        //for (int z = 0; z < list_repts.Count(); z++)
                        //{
                        //    rept_no = list_repts[z].FYP_Receipt_No;
                        //    var num_1 = rept_no.Substring(0, 1);
                        //    if (num_1 != "0" && num_1 != "1" && num_1 != "2" && num_1 != "3" && num_1 != "4" && num_1 != "5" && num_1 != "6" && num_1 != "7" && num_1 != "8" && num_1 != "9")
                        //    {
                        //        rept_prefix = rept_no.Substring(0, prefix_len);
                        //        rept_suffix = rept_no.Substring((rept_no.Length - suffix_len), suffix_len);
                        //        max_num = rept_no.Substring(0, (rept_no.Length - suffix_len));
                        //        max_num = max_num.Substring(prefix_len, (max_num.Length - prefix_len));
                        //    }
                        //    if (max_num != "" && rept_prefix == prefix && rept_suffix == suffix)
                        //    {
                        //        number_max.Add(Convert.ToInt32(max_num));
                        //    }

                        //}
                        //if (number_max.Count() > 0)
                        //{
                        //    max_num_int = number_max.Max();
                        //}
                        //max_num_int += 1;

                        //final_rept_no = prefix + max_num_int.ToString().Trim() + suffix;


                        //var receiptno = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("receiptnogeneration @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, grpid, final_rept_no);



                        using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "receiptnogeneration";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@mi_id",
                                SqlDbType.VarChar, 100)
                            {
                                Value = data.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@asmayid",
                               SqlDbType.NVarChar, 100)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@fmgid",
                           SqlDbType.NVarChar, 100)
                            {
                                Value = groupidss
                            });

                            cmd.Parameters.Add(new SqlParameter("@receiptno",
                SqlDbType.NVarChar, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            data.FYP_Receipt_No = cmd.Parameters["@receiptno"].Value.ToString();

                        }

                        //data.auto_FYP_Receipt_No = final_rept_no;

                        //data.FYP_Receipt_No = final_rept_no;
                    }
                }

                else if (data.automanualreceiptno == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.FYP_Receipt_No = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public FeeStudentTransactionDTO Printterm(FeeStudentTransactionDTO data)
        {
            try
            {
                var Acdemic_preadmission = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();


                data.ASMAY_Id = Acdemic_preadmission;

                if(data.filterinitialdata=="Preadmission")
                {
                    data.receiptformathead = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                              from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                              from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                              from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                              from e in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                              from f in _YearlyFeeGroupMappingContext.feespecialHead
                                              from g in _YearlyFeeGroupMappingContext.feeSGGG
                                              where (g.FMH_Id == e.FMH_Id && f.FMSFH_Id == g.FMSFH_Id && a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && d.FMH_Id == e.FMH_Id && b.PASA_Id == data.PASR_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id && f.MI_Id == data.MI_Id)
                                              select new FeeStudentTransactionDTO
                                              {
                                                  FMH_FeeName = f.FMSFH_Name,
                                                  FMH_Id = e.FMH_Id,
                                                  FTP_Paid_Amt = c.FTP_Paid_Amt,
                                                  FTP_Concession_Amt = c.FTP_Concession_Amt,
                                                  FTP_Fine_Amt = c.FTP_Fine_Amt,
                                                  FTI_Id = d.FTI_Id,
                                                  totalcharges = d.FMA_Amount
                                              }
          ).ToArray();

                    data.fillstudentviewdetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                                   from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                                   from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                                   from e in _YearlyFeeGroupMappingContext.stuapp
                                                   from f in _YearlyFeeGroupMappingContext.School_M_Class
                                                   from h in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                   from i in _YearlyFeeGroupMappingContext.feeMIY
                                                   from j in _YearlyFeeGroupMappingContext.AcademicYear
                                                   where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && h.MI_Id == a.MI_Id && i.MI_ID == a.MI_Id && j.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && c.FYP_Id == a.FYP_Id && b.PASA_Id == e.pasr_id && f.ASMCL_Id == e.ASMCL_Id && c.FMA_Id == d.FMA_Id && d.FMH_Id == h.FMH_Id && d.FTI_Id == i.FTI_Id && a.ASMAY_ID == j.ASMAY_Id && d.ASMAY_Id == a.ASMAY_ID && j.ASMAY_Id == a.ASMAY_ID)
                                                   select new FeeStaffOthersTransactionDTO
                                                   {
                                                       FYP_Id = a.FYP_Id,
                                                       FYP_Receipt_No = a.FYP_Receipt_No,
                                                       FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                       FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                       FYP_Bank_Name = a.FYP_Bank_Name,
                                                       FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                                       FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                                       FYP_Remarks = a.FYP_Remarks,
                                                       FYP_Date = a.FYP_Date,
                                                       ToBePaid = Convert.ToInt64(d.FMA_Amount),
                                                       FTP_Paid_Amt = Convert.ToDecimal(c.FTP_Paid_Amt),
                                                       FTP_Concession_Amt = Convert.ToDecimal(c.FTP_Concession_Amt),
                                                       PASR_FirstName = ((e.PASR_FirstName == null ? " " : e.PASR_FirstName) + " " + (e.PASR_MiddleName == null ? " " : e.PASR_MiddleName) + " " + (e.PASR_LastName == null ? " " : e.PASR_LastName)).Trim(),
                                                       PASR_RegistrationNo = e.PASR_Applicationno,
                                                       classname = f.ASMCL_ClassName,
                                                       FMH_Id = d.FMH_Id,
                                                       FTI_Id = d.FTI_Id,
                                                       ASMAY_Id = a.ASMAY_ID,
                                                       PASR_Id = e.pasr_id,
                                                       FMH_FeeName = h.FMH_FeeName,
                                                       FTI_Name = i.FTI_Name,
                                                       ASMAY_Year = j.ASMAY_Year,
                                                       fathername= (e.PASR_FatherName + ' ' + (e.PASR_FatherSurname == null || e.PASR_FatherSurname == "0" ? "" : e.PASR_FatherSurname)),
                                                       mothername =e.PASR_MotherName
                                                   }).Distinct().ToArray();
                }
               else if(data.filterinitialdata=="Prospectus")
                {
                    data.receiptformathead = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                              from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ProspectusDMO
                                              from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                              from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                              from e in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                              from f in _YearlyFeeGroupMappingContext.feespecialHead
                                              from g in _YearlyFeeGroupMappingContext.feeSGGG
                                              where (g.FMH_Id == e.FMH_Id && f.FMSFH_Id == g.FMSFH_Id && a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && d.FMH_Id == e.FMH_Id && b.PASP_Id == data.PASR_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id && f.MI_Id == data.MI_Id)
                                              select new FeeStudentTransactionDTO
                                              {
                                                  FMH_FeeName = f.FMSFH_Name,
                                                  FMH_Id = e.FMH_Id,
                                                  FTP_Paid_Amt = c.FTP_Paid_Amt,
                                                  FTP_Concession_Amt = c.FTP_Concession_Amt,
                                                  FTP_Fine_Amt = c.FTP_Fine_Amt,
                                                  FTI_Id = d.FTI_Id,
                                                  totalcharges = d.FMA_Amount
                                              }
          ).ToArray();

                    data.fillstudentviewdetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ProspectusDMO
                                                   from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                                   from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                                   from e in _YearlyFeeGroupMappingContext.Prospectus
                                                   from f in _YearlyFeeGroupMappingContext.School_M_Class
                                                   from h in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                   from i in _YearlyFeeGroupMappingContext.feeMIY
                                                   from j in _YearlyFeeGroupMappingContext.AcademicYear
                                                   where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_ID == a.MI_Id && f.MI_Id == a.MI_Id && h.MI_Id == a.MI_Id && i.MI_ID == a.MI_Id && j.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && c.FYP_Id == a.FYP_Id && b.PASP_Id == e.PASP_Id && f.ASMCL_Id == e.ASMCL_Id && c.FMA_Id == d.FMA_Id && d.FMH_Id == h.FMH_Id && d.FTI_Id == i.FTI_Id && a.ASMAY_ID == j.ASMAY_Id && d.ASMAY_Id == a.ASMAY_ID && j.ASMAY_Id == a.ASMAY_ID)
                                                   select new FeeStaffOthersTransactionDTO
                                                   {
                                                       FYP_Id = a.FYP_Id,
                                                       FYP_Receipt_No = a.FYP_Receipt_No,
                                                       FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                       FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                       FYP_Bank_Name = a.FYP_Bank_Name,
                                                       FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                                       FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                                       FYP_Remarks = a.FYP_Remarks,
                                                       FYP_Date = a.FYP_Date,
                                                       ToBePaid = Convert.ToInt64(d.FMA_Amount),
                                                       FTP_Paid_Amt = Convert.ToDecimal(c.FTP_Paid_Amt),
                                                       FTP_Concession_Amt = Convert.ToDecimal(c.FTP_Concession_Amt),
                                                       PASR_FirstName = ((e.PASP_First_Name == null ? " " : e.PASP_First_Name) + " " + (e.PASP_Middle_Name == null ? " " : e.PASP_Middle_Name) + " " + (e.PASP_Last_Name == null ? " " : e.PASP_Last_Name)).Trim(),
                                                       PASR_RegistrationNo = e.PASP_ProspectusNo,
                                                       classname = f.ASMCL_ClassName,
                                                       FMH_Id = d.FMH_Id,
                                                       FTI_Id = d.FTI_Id,
                                                       ASMAY_Id = a.ASMAY_ID,
                                                       PASR_Id = e.PASP_Id,
                                                       FMH_FeeName = h.FMH_FeeName,
                                                       FTI_Name = i.FTI_Name,
                                                       ASMAY_Year = j.ASMAY_Year
                                                   }).Distinct().ToArray();
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeStudentTransactionDTO printrec(FeeStudentTransactionDTO data)
        {
            try
            {

                ReadTemplateFromAzure obj = new ReadTemplateFromAzure();
                string accountname = "", accesskey = "", html = "";
                var datatstu = _context.IVRM_Storage_path_Details.ToList();
                if (datatstu.Count() > 0)
                {
                    accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
                    accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
                }
                try
                {
                    html = obj.getHtmlContentFromAzure(accountname, accesskey, "feereceipt/" + data.MI_Id, "PreadmissionRec.html", 0);
                }
                catch (Exception ex)
                { html = ""; }

                data.htmldata = html;



                var Acdemic_preadmission = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();


                data.ASMAY_Id = Acdemic_preadmission;

                if (data.minstall == "0")
                {
                    Printterm(data);
                }
                else
                {
                    if(data.filterinitialdata=="Preadmission")
                    {
                        data.receiptdetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                               from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                               from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                               from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                               from e in _YearlyFeeGroupMappingContext.stuapp
                                               from f in _YearlyFeeGroupMappingContext.School_M_Class
                                               from h in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                               from i in _YearlyFeeGroupMappingContext.feeMIY
                                               from j in _YearlyFeeGroupMappingContext.AcademicYear
                                               where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && h.MI_Id == a.MI_Id && i.MI_ID == a.MI_Id && j.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && c.FYP_Id == a.FYP_Id && b.PASA_Id == e.pasr_id && f.ASMCL_Id == e.ASMCL_Id && c.FMA_Id == d.FMA_Id && d.FMH_Id == h.FMH_Id && d.FTI_Id == i.FTI_Id && a.ASMAY_ID == j.ASMAY_Id && d.ASMAY_Id == a.ASMAY_ID && j.ASMAY_Id == a.ASMAY_ID)
                                               select new FeeStaffOthersTransactionDTO
                                               {
                                                   FYP_Id = a.FYP_Id,
                                                   FYP_Receipt_No = a.FYP_Receipt_No,
                                                   FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                   FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                   FYP_Bank_Name = a.FYP_Bank_Name,
                                                   FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                                   FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                                   FYP_Remarks = a.FYP_Remarks,
                                                   FYP_Date = a.FYP_Date,
                                                   ToBePaid = Convert.ToInt64(d.FMA_Amount),
                                                   FTP_Paid_Amt = Convert.ToDecimal(c.FTP_Paid_Amt),
                                                   FTP_Concession_Amt = Convert.ToDecimal(c.FTP_Concession_Amt),
                                                   PASR_FirstName = ((e.PASR_FirstName == null ? " " : e.PASR_FirstName) + " " + (e.PASR_MiddleName == null ? " " : e.PASR_MiddleName) + " " + (e.PASR_LastName == null ? " " : e.PASR_LastName)).Trim(),
                                                   PASR_RegistrationNo = e.PASR_Applicationno,
                                                   classname = f.ASMCL_ClassName,
                                                   FMH_Id = d.FMH_Id,
                                                   FTI_Id = d.FTI_Id,
                                                   ASMAY_Id = a.ASMAY_ID,
                                                   PASR_Id = e.pasr_id,
                                                   FMH_FeeName = h.FMH_FeeName,
                                                   FTI_Name = i.FTI_Name,
                                                   ASMAY_Year = j.ASMAY_Year
                                               }).Distinct().ToArray();
                    }

                    else if(data.filterinitialdata=="Prospectus")
                    {
                        data.receiptdetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                               from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                               from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                               from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                               from e in _YearlyFeeGroupMappingContext.Prospectus
                                               from f in _YearlyFeeGroupMappingContext.School_M_Class
                                               from h in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                               from i in _YearlyFeeGroupMappingContext.feeMIY
                                               from j in _YearlyFeeGroupMappingContext.AcademicYear
                                               where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_ID == a.MI_Id && f.MI_Id == a.MI_Id && h.MI_Id == a.MI_Id && i.MI_ID == a.MI_Id && j.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && c.FYP_Id == a.FYP_Id && b.PASA_Id == e.PASP_Id && f.ASMCL_Id == e.ASMCL_Id && c.FMA_Id == d.FMA_Id && d.FMH_Id == h.FMH_Id && d.FTI_Id == i.FTI_Id && a.ASMAY_ID == j.ASMAY_Id && d.ASMAY_Id == a.ASMAY_ID && j.ASMAY_Id == a.ASMAY_ID)
                                               select new FeeStaffOthersTransactionDTO
                                               {
                                                   FYP_Id = a.FYP_Id,
                                                   FYP_Receipt_No = a.FYP_Receipt_No,
                                                   FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                   FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                   FYP_Bank_Name = a.FYP_Bank_Name,
                                                   FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                                   FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                                   FYP_Remarks = a.FYP_Remarks,
                                                   FYP_Date = a.FYP_Date,
                                                   ToBePaid = Convert.ToInt64(d.FMA_Amount),
                                                   FTP_Paid_Amt = Convert.ToDecimal(c.FTP_Paid_Amt),
                                                   FTP_Concession_Amt = Convert.ToDecimal(c.FTP_Concession_Amt),
                                                   PASR_FirstName = ((e.PASP_First_Name == null ? " " : e.PASP_First_Name) + " " + (e.PASP_Middle_Name == null ? " " : e.PASP_Middle_Name) + " " + (e.PASP_Last_Name == null ? " " : e.PASP_Last_Name)).Trim(),
                                                   PASR_RegistrationNo = e.PASP_ProspectusNo,
                                                   classname = f.ASMCL_ClassName,
                                                   FMH_Id = d.FMH_Id,
                                                   FTI_Id = d.FTI_Id,
                                                   ASMAY_Id = a.ASMAY_ID,
                                                   PASR_Id = e.PASP_Id,
                                                   FMH_FeeName = h.FMH_FeeName,
                                                   FTI_Name = i.FTI_Name,
                                                   ASMAY_Year = j.ASMAY_Year
                                               }).Distinct().ToArray();
                    }
                  
                }
              

                //var feeterm = (from a in _YearlyFeeGroupMappingContext.feeTr
                //               from b in _YearlyFeeGroupMappingContext.feeMTH
                //               from c in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                //               from d in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                //               from e in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                //               from f in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                //               from g in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                //               where (a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMT_ActiveFlag == true && a.FMT_Id == b.FMT_Id && c.FSSST_ActiveFlag == true && c.HRME_Id == d.HRME_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && c.ASMAY_Id == d.ASMAY_Id && e.MI_Id == a.MI_Id && d.ASMAY_Id == e.ASMAY_ID && d.FYP_Id == data.FYP_Id && e.FYP_Id == d.FYP_Id && f.FYP_Id == d.FYP_Id && g.FMAOST_Id == f.FMAOST_Id && g.FTI_Id == b.FTI_Id && g.FMAOST_OthStaffFlag == "S")

                //               select new FeeStaffOthersTransactionDTO
                //               {
                //                   FMT_Id = a.FMT_Id,
                //                   FMT_Order = a.FMT_Order,
                //                   FromMonth = a.FromMonth,
                //                   ToMonth = a.ToMonth
                //               }).Distinct().OrderByDescending(t => t.FMT_Order).ToArray();

             
                //long fmt_id_new = 0;
              

                //fmt_id_new = Convert.ToInt64(feeterm[0].FMT_Id) + 1;
                //var fmt_by_order = feeterm[0].FMT_Order + 1;
            
                //var term_ids = _YearlyFeeGroupMappingContext.feeTr.Where(t => t.MI_Id == data.MI_Id && t.FMT_Order <= fmt_by_order).Select(t => t.FMT_Id);


                //data.dueamount = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                //                  from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                //                  from e in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                //                  from f in _YearlyFeeGroupMappingContext.feeMTH
                //                  where (a.MI_Id == data.MI_Id && e.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && a.ASMAY_ID == b.ASMAY_Id && e.HRME_Id == b.HRME_Id && f.FMH_Id == e.FMH_Id && f.FTI_Id == e.FTI_Id && term_ids.Contains(f.FMT_Id))
                //                  select e.FSSST_ToBePaid).Sum();
                //var monthnameinitial = feeterm[(feeterm.Length - 1)].FromMonth.ToString();
                //var monthnameend = feeterm[0].ToMonth.ToString();

                //data.duration = monthnameinitial + '-' + monthnameend;// +'-' + data.year

                //var duedates_list = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                //                     from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                //                     from d in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                //                     from e in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                //                     from f in _YearlyFeeGroupMappingContext.feeMTH
                //                     from g in _YearlyFeeGroupMappingContext.Fee_T_Due_Date_OthStaffs
                //                     where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && a.ASMAY_ID == b.ASMAY_Id && d.ASMAY_Id == a.ASMAY_ID && e.HRME_Id == b.HRME_Id && e.FMG_Id == d.FMG_Id && e.FMH_Id == d.FMH_Id && e.FTI_Id == d.FTI_Id && f.FMH_Id == e.FMH_Id && f.FTI_Id == e.FTI_Id && term_ids.Contains(f.FMT_Id) && d.FMAOST_Id == g.FMAOST_Id && d.FMAOST_OthStaffFlag == "S")
                //                     select g).Distinct().ToList();

                //var year = duedates_list.Max(t => t.FTDD_Year);
                //var month = duedates_list.Where(t => t.FTDD_Year == year).Select(t => Convert.ToInt32(t.FTDD_Month)).Max();
                //var date = duedates_list.Where(t => Convert.ToInt32(t.FTDD_Month) == month && t.FTDD_Year == year).Select(t => Convert.ToInt32(t.FTDD_Day)).Max();

                //data.Due_Date = date + "/" + month + "/" + year;
             
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public FeeStudentTransactionDTO search(FeeStudentTransactionDTO data)
        {
           try
            {
                var Acdemic_preadmission = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();


                data.ASMAY_Id = Acdemic_preadmission;

                if(data.filterinitialdata== "Preadmission")
                {
                    data.searchfilter = data.searchfilter.ToUpper();
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.stuapp
                                        where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PASR_Adm_Confirm_Flag == false && ((a.PASR_FirstName.Trim().ToUpper() + ' ' + a.PASR_MiddleName.Trim().ToUpper() + ' ' + a.PASR_LastName.Trim().ToUpper()).StartsWith(data.searchfilter) || (a.PASR_FirstName.Trim().ToUpper() + a.PASR_MiddleName.Trim().ToUpper() + ' ' + a.PASR_LastName.Trim().ToUpper()).StartsWith(data.searchfilter) || a.PASR_FirstName.ToUpper().StartsWith(data.searchfilter) || a.PASR_MiddleName.ToUpper().StartsWith(data.searchfilter) || a.PASR_LastName.ToUpper().StartsWith(data.searchfilter)))
                                        select new FeeStudentTransactionDTO
                                        {
                                            PASR_Id = a.pasr_id,
                                            // PASR_FirstName = a.PASR_FirstName,
                                            PASR_MiddleName = a.PASR_MiddleName,
                                            PASR_LastName = a.PASR_LastName,

                                            PASR_FirstName = ((a.PASR_FirstName == null || a.PASR_FirstName == "" ? "" : " " + a.PASR_FirstName) + (a.PASR_MiddleName == null || a.PASR_MiddleName == "" || a.PASR_MiddleName == "0" ? "" : " " + a.PASR_MiddleName) + (a.PASR_LastName == null || a.PASR_LastName == "" || a.PASR_LastName == "0" ? "" : " " + a.PASR_LastName)).Trim(),
                                        }
           ).ToArray();
                }

                else if(data.filterinitialdata== "Prospectus")
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Prospectus
                                        where (a.MI_ID == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PASP_ProspectusNo.Contains(data.searchfilter))
                                        select new FeeStudentTransactionDTO
                                        {
                                            PASR_Id = a.PASP_Id,
                                            PASR_FirstName=a.PASP_ProspectusNo.Trim()
                                        }
         ).ToArray();
                }
            }
           catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeStudentTransactionDTO searchfilter(FeeStudentTransactionDTO data)
        {
            try
            {
                var Acdemic_preadmission = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();


                data.ASMAY_Id = Acdemic_preadmission;

                if(data.filterinitialdata== "Preadmission")
                {
                    switch (data.searchType)
                    {

                        case "0":
                            string str = "";
                            data.searchtext = data.searchtext.ToUpper();
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.stuapp
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                                from c in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                from d in _YearlyFeeGroupMappingContext.School_M_Class
                                                where (a.ASMCL_Id == d.ASMCL_Id && b.FYP_Id == c.FYP_Id && b.PASA_Id == a.pasr_id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && (((a.PASR_FirstName.Trim().ToUpper() + ' ' + (string.IsNullOrEmpty(a.PASR_MiddleName.Trim().ToUpper()) == true ? str : a.PASR_MiddleName.Trim().ToUpper())).Trim() + ' ' + (string.IsNullOrEmpty(a.PASR_LastName.Trim().ToUpper()) == true ? str : a.PASR_LastName.Trim().ToUpper())).Trim().Contains(data.searchtext) || a.PASR_FirstName.ToUpper().StartsWith(data.searchtext) || a.PASR_MiddleName.ToUpper().StartsWith(data.searchtext) || a.PASR_LastName.ToUpper().StartsWith(data.searchtext)))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    PASR_Id = a.pasr_id,
                                                    //PASR_FirstName = a.PASR_FirstName,
                                                    PASR_FirstName = ((a.PASR_FirstName == null || a.PASR_FirstName == "" ? "" : " " + a.PASR_FirstName) + (a.PASR_MiddleName == null || a.PASR_MiddleName == "" || a.PASR_MiddleName == "0" ? "" : " " + a.PASR_MiddleName) + (a.PASR_LastName == null || a.PASR_LastName == "" || a.PASR_LastName == "0" ? "" : " " + a.PASR_LastName)).Trim(),
                                                    PASR_MiddleName = a.PASR_MiddleName,
                                                    PASR_LastName = a.PASR_LastName,
                                                    FYP_Receipt_No = c.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = c.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = c.FYP_Tot_Amount,
                                                    classname = d.ASMCL_ClassName,
                                                    FYP_Id = c.FYP_Id,
                                                    PASR_RegistrationNo = a.PASR_RegistrationNo,
                                                    FYP_Date = c.FYP_Date,
                                                    pasR_MobileNo = a.PASR_MobileNo,
                                                    pasR_emailId = a.PASR_emailId
                                                }
                  ).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
                            break;

                        case "1":
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.stuapp
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                                from c in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                from d in _YearlyFeeGroupMappingContext.School_M_Class
                                                where (a.ASMCL_Id == d.ASMCL_Id && b.FYP_Id == c.FYP_Id && b.PASA_Id == a.pasr_id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id  && a.PASR_RegistrationNo.Contains(data.searchtext))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    PASR_Id = a.pasr_id,
                                                    // PASR_FirstName = a.PASR_FirstName,
                                                    PASR_FirstName = ((a.PASR_FirstName == null || a.PASR_FirstName == "" ? "" : " " + a.PASR_FirstName) + (a.PASR_MiddleName == null || a.PASR_MiddleName == "" || a.PASR_MiddleName == "0" ? "" : " " + a.PASR_MiddleName) + (a.PASR_LastName == null || a.PASR_LastName == "" || a.PASR_LastName == "0" ? "" : " " + a.PASR_LastName)).Trim(),
                                                    PASR_MiddleName = a.PASR_MiddleName,
                                                    PASR_LastName = a.PASR_LastName,
                                                    FYP_Receipt_No = c.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = c.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = c.FYP_Tot_Amount,
                                                    classname = d.ASMCL_ClassName,
                                                    FYP_Id = c.FYP_Id,
                                                    PASR_RegistrationNo = a.PASR_RegistrationNo,
                                                    FYP_Date = c.FYP_Date,
                                                    pasR_MobileNo = a.PASR_MobileNo,
                                                    pasR_emailId = a.PASR_emailId
                                                }
                ).Distinct().OrderBy(t => t.PASR_RegistrationNo).ToArray();
                            break;

                        case "2":
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.stuapp
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                                from c in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                from d in _YearlyFeeGroupMappingContext.School_M_Class
                                                where (a.ASMCL_Id == d.ASMCL_Id && b.FYP_Id == c.FYP_Id && b.PASA_Id == a.pasr_id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PASR_emailId.Contains(data.searchtext))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    PASR_Id = a.pasr_id,
                                                    //PASR_FirstName = a.PASR_FirstName,
                                                    PASR_FirstName = ((a.PASR_FirstName == null || a.PASR_FirstName == "" ? "" : " " + a.PASR_FirstName) + (a.PASR_MiddleName == null || a.PASR_MiddleName == "" || a.PASR_MiddleName == "0" ? "" : " " + a.PASR_MiddleName) + (a.PASR_LastName == null || a.PASR_LastName == "" || a.PASR_LastName == "0" ? "" : " " + a.PASR_LastName)).Trim(),
                                                    PASR_MiddleName = a.PASR_MiddleName,
                                                    PASR_LastName = a.PASR_LastName,
                                                    FYP_Receipt_No = c.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = c.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = c.FYP_Tot_Amount,
                                                    classname = d.ASMCL_ClassName,
                                                    FYP_Id = c.FYP_Id,
                                                    PASR_RegistrationNo = a.PASR_RegistrationNo,
                                                    FYP_Date = c.FYP_Date,
                                                    pasR_MobileNo = a.PASR_MobileNo,
                                                    pasR_emailId = a.PASR_emailId
                                                }
                ).Distinct().ToArray();
                            break;

                        case "3":
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.stuapp
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                                from c in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                from d in _YearlyFeeGroupMappingContext.School_M_Class
                                                where (a.ASMCL_Id == d.ASMCL_Id && b.FYP_Id == c.FYP_Id && b.PASA_Id == a.pasr_id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id  && c.FYP_Receipt_No.Contains(data.searchtext))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    PASR_Id = a.pasr_id,
                                                    //PASR_FirstName = a.PASR_FirstName,
                                                    PASR_FirstName = ((a.PASR_FirstName == null || a.PASR_FirstName == "" ? "" : " " + a.PASR_FirstName) + (a.PASR_MiddleName == null || a.PASR_MiddleName == "" || a.PASR_MiddleName == "0" ? "" : " " + a.PASR_MiddleName) + (a.PASR_LastName == null || a.PASR_LastName == "" || a.PASR_LastName == "0" ? "" : " " + a.PASR_LastName)).Trim(),
                                                    PASR_MiddleName = a.PASR_MiddleName,
                                                    PASR_LastName = a.PASR_LastName,
                                                    FYP_Receipt_No = c.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = c.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = c.FYP_Tot_Amount,
                                                    classname = d.ASMCL_ClassName,
                                                    FYP_Id = c.FYP_Id,
                                                    PASR_RegistrationNo = a.PASR_RegistrationNo,
                                                    FYP_Date = c.FYP_Date,
                                                    pasR_MobileNo = a.PASR_MobileNo,
                                                    pasR_emailId = a.PASR_emailId
                                                }
                ).Distinct().ToArray();
                            break;

                    }
                }
                else if(data.filterinitialdata== "Prospectus")
                {
                    switch (data.searchType)
                    {

                        case "0":
                            string str = "";
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.Prospectus
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ProspectusDMO
                                                from c in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                from d in _YearlyFeeGroupMappingContext.School_M_Class
                                                where (a.ASMCL_Id == d.ASMCL_Id && b.FYP_Id == c.FYP_Id && b.PASP_Id == a.PASP_Id && a.MI_ID == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && (((a.PASP_First_Name.Trim() + ' ' + (string.IsNullOrEmpty(a.PASP_Middle_Name.Trim()) == true ? str : a.PASP_Middle_Name.Trim())).Trim() + ' ' + (string.IsNullOrEmpty(a.PASP_Last_Name.Trim()) == true ? str : a.PASP_Last_Name.Trim())).Trim().Contains(data.searchtext) || a.PASP_First_Name.StartsWith(data.searchtext) || a.PASP_Middle_Name.StartsWith(data.searchtext) || a.PASP_Last_Name.StartsWith(data.searchtext)))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    PASR_Id = a.PASP_Id,
                                                    //PASR_FirstName = a.PASP_First_Name,
                                                    PASR_FirstName = ((a.PASP_First_Name == null || a.PASP_First_Name == "" ? "" : " " + a.PASP_First_Name) + (a.PASP_Middle_Name == null || a.PASP_Middle_Name == "" || a.PASP_Middle_Name == "0" ? "" : " " + a.PASP_Middle_Name) + (a.PASP_Last_Name == null || a.PASP_Last_Name == "" || a.PASP_Last_Name == "0" ? "" : " " + a.PASP_Last_Name)).Trim(),
                                                    PASR_MiddleName = a.PASP_Middle_Name,
                                                    PASR_LastName = a.PASP_Last_Name,
                                                    FYP_Receipt_No = c.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = c.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = c.FYP_Tot_Amount,
                                                    classname = d.ASMCL_ClassName,
                                                    FYP_Id = c.FYP_Id,
                                                    PASR_RegistrationNo = a.PASP_ProspectusNo,
                                                    FYP_Date = c.FYP_Date,
                                                    pasR_MobileNo = a.PASP_MobileNo,
                                                    pasR_emailId = a.PASP_EmailId
                                                }
                  ).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
                            break;

                        case "1":
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.Prospectus
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ProspectusDMO
                                                from c in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                from d in _YearlyFeeGroupMappingContext.School_M_Class
                                                where (a.ASMCL_Id == d.ASMCL_Id && b.FYP_Id == c.FYP_Id && b.PASP_Id == a.PASP_Id && a.MI_ID == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PASP_ProspectusNo.Contains(data.searchtext))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    PASR_Id = a.PASP_Id,
                                                    //PASR_FirstName = a.PASP_First_Name,
                                                    PASR_FirstName = ((a.PASP_First_Name == null || a.PASP_First_Name == "" ? "" : " " + a.PASP_First_Name) + (a.PASP_Middle_Name == null || a.PASP_Middle_Name == "" || a.PASP_Middle_Name == "0" ? "" : " " + a.PASP_Middle_Name) + (a.PASP_Last_Name == null || a.PASP_Last_Name == "" || a.PASP_Last_Name == "0" ? "" : " " + a.PASP_Last_Name)).Trim(),
                                                    PASR_MiddleName = a.PASP_Middle_Name,
                                                    PASR_LastName = a.PASP_Last_Name,
                                                    FYP_Receipt_No = c.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = c.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = c.FYP_Tot_Amount,
                                                    classname = d.ASMCL_ClassName,
                                                    FYP_Id = c.FYP_Id,
                                                    PASR_RegistrationNo = a.PASP_ProspectusNo,
                                                    FYP_Date = c.FYP_Date,
                                                    pasR_MobileNo = a.PASP_MobileNo,
                                                    pasR_emailId = a.PASP_EmailId
                                                }
                ).Distinct().OrderBy(t => t.PASR_RegistrationNo).ToArray();
                            break;

                        case "2":
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.Prospectus
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ProspectusDMO
                                                from c in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                from d in _YearlyFeeGroupMappingContext.School_M_Class
                                                where (a.ASMCL_Id == d.ASMCL_Id && b.FYP_Id == c.FYP_Id && b.PASP_Id == a.PASP_Id && a.MI_ID == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PASP_EmailId.Contains(data.searchtext))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    PASR_Id = a.PASP_Id,
                                                    //PASR_FirstName = a.PASP_First_Name,
                                                    PASR_FirstName = ((a.PASP_First_Name == null || a.PASP_First_Name == "" ? "" : " " + a.PASP_First_Name) + (a.PASP_Middle_Name == null || a.PASP_Middle_Name == "" || a.PASP_Middle_Name == "0" ? "" : " " + a.PASP_Middle_Name) + (a.PASP_Last_Name == null || a.PASP_Last_Name == "" || a.PASP_Last_Name == "0" ? "" : " " + a.PASP_Last_Name)).Trim(),
                                                    PASR_MiddleName = a.PASP_Middle_Name,
                                                    PASR_LastName = a.PASP_Last_Name,
                                                    FYP_Receipt_No = c.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = c.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = c.FYP_Tot_Amount,
                                                    classname = d.ASMCL_ClassName,
                                                    FYP_Id = c.FYP_Id,
                                                    PASR_RegistrationNo = a.PASP_ProspectusNo,
                                                    FYP_Date = c.FYP_Date,
                                                    pasR_MobileNo = a.PASP_MobileNo,
                                                    pasR_emailId = a.PASP_EmailId
                                                }
                ).Distinct().ToArray();
                            break;

                        case "3":
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.Prospectus
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ProspectusDMO
                                                from c in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                from d in _YearlyFeeGroupMappingContext.School_M_Class
                                                where (a.ASMCL_Id == d.ASMCL_Id && b.FYP_Id == c.FYP_Id && b.PASP_Id == a.PASP_Id && a.MI_ID == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.FYP_Receipt_No.Contains(data.searchtext))
                                                select new FeeStudentTransactionDTO
                                                {
                                                    PASR_Id = a.PASP_Id,
                                                    //PASR_FirstName = a.PASP_First_Name,
                                                    PASR_FirstName = ((a.PASP_First_Name == null || a.PASP_First_Name == "" ? "" : " " + a.PASP_First_Name) + (a.PASP_Middle_Name == null || a.PASP_Middle_Name == "" || a.PASP_Middle_Name == "0" ? "" : " " + a.PASP_Middle_Name) + (a.PASP_Last_Name == null || a.PASP_Last_Name == "" || a.PASP_Last_Name == "0" ? "" : " " + a.PASP_Last_Name)).Trim(),
                                                    PASR_MiddleName = a.PASP_Middle_Name,
                                                    PASR_LastName = a.PASP_Last_Name,
                                                    FYP_Receipt_No = c.FYP_Receipt_No,
                                                    FYP_Bank_Or_Cash = c.FYP_Bank_Or_Cash,
                                                    FYP_Tot_Amount = c.FYP_Tot_Amount,
                                                    classname = d.ASMCL_ClassName,
                                                    FYP_Id = c.FYP_Id,
                                                    PASR_RegistrationNo = a.PASP_ProspectusNo,
                                                    FYP_Date = c.FYP_Date,
                                                    pasR_MobileNo = a.PASP_MobileNo,
                                                    pasR_emailId = a.PASP_EmailId
                                                }
                ).Distinct().ToArray();
                            break;

                    }
                }

                
                }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        public FeeStudentTransactionDTO recnogen(FeeStudentTransactionDTO data)
        {
            try
            {
                get_grp_reptno(data);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeStudentTransactionDTO delrec(FeeStudentTransactionDTO data)
        {
            try
            {
                if(data.filterinitialdata=="Preadmission")
                {
                    var lorg1 = _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO.Where(t => t.FYP_Id == data.FYP_Id).ToList();

                    var lorg2 = _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYP_Id == data.FYP_Id).ToList();

                    var lorg3 = _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO.Where(t => t.FYP_Id == data.FYP_Id).ToList();

                    var lorg5 = _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeSchool.Where(t => t.FYP_Id == data.FYP_Id).ToList();

                    if (lorg1.Count() > 0 && lorg2.Count() > 0 && lorg3.Count() > 0)
                    {
                        _YearlyFeeGroupMappingContext.Remove(lorg2[0]);
                        _YearlyFeeGroupMappingContext.Remove(lorg1[0]);

                        for (int i = 0; i < lorg3.Count(); i++)
                        {
                            _YearlyFeeGroupMappingContext.Remove(lorg3[i]);
                        }

                        foreach (var c in lorg5)
                        {
                            var checkresult = _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeSchool.Single(a => a.FYPPM_Id == c.FYPPM_Id);
                            _YearlyFeeGroupMappingContext.Remove(checkresult);
                        }

                        var contactexisttransaction = 0;
                        using (var dbCtxTxn = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                        {
                            try
                            {
                                contactexisttransaction = _YearlyFeeGroupMappingContext.SaveChanges();
                                dbCtxTxn.Commit();
                                data.returnval = "Delete";
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                dbCtxTxn.Rollback();
                                data.returnval = "Cancel";
                            }
                        }
                    }

                    else
                    {
                        data.returnval = "Error";
                    }
                }
                else if(data.filterinitialdata=="Prospectus")
                {
                    var lorg1 = _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO.Where(t => t.FYP_Id == data.FYP_Id).ToList();

                    var lorg2 = _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ProspectusDMO.Where(t => t.FYP_Id == data.FYP_Id).ToList();

                    var lorg3 = _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO.Where(t => t.FYP_Id == data.FYP_Id).ToList();

                    if (lorg1.Count() > 0 && lorg2.Count() > 0 && lorg3.Count() > 0)
                    {
                        _YearlyFeeGroupMappingContext.Remove(lorg2[0]);
                        _YearlyFeeGroupMappingContext.Remove(lorg1[0]);

                        for (int i = 0; i < lorg3.Count(); i++)
                        {
                            _YearlyFeeGroupMappingContext.Remove(lorg3[i]);
                        }

                        var contactexisttransaction = 0;
                        using (var dbCtxTxn = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                        {
                            try
                            {
                                contactexisttransaction = _YearlyFeeGroupMappingContext.SaveChanges();
                                dbCtxTxn.Commit();
                                data.returnval = "Delete";
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                dbCtxTxn.Rollback();
                                data.returnval = "Cancel";
                            }
                        }
                    }

                    else
                    {
                        data.returnval = "Error";
                    }
                }
               
            }
            catch (Exception ee)
            {
                data.returnval = "Error";
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public FeeStudentTransactionDTO filstude(FeeStudentTransactionDTO data)
        {
            try
            {
                var Acdemic_preadmission = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();


                data.ASMAY_Id = Acdemic_preadmission;
                if(data.filterinitialdata== "Preadmission")
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.stuapp
                                        where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PASR_Adm_Confirm_Flag == false && ((a.PASR_FirstName.Trim() + ' ' + a.PASR_MiddleName.Trim() + ' ' + a.PASR_LastName.Trim()).StartsWith(data.searchfilter) || (a.PASR_FirstName.Trim() + a.PASR_MiddleName.Trim() + ' ' + a.PASR_LastName.Trim()).StartsWith(data.searchfilter) || a.PASR_FirstName.StartsWith(data.searchfilter) || a.PASR_MiddleName.StartsWith(data.searchfilter) || a.PASR_LastName.StartsWith(data.searchfilter)))
                                        select new FeeStudentTransactionDTO
                                        {
                                            PASR_Id = a.pasr_id,
                                            // PASR_FirstName = a.PASR_FirstName,
                                            PASR_MiddleName = a.PASR_MiddleName,
                                            PASR_LastName = a.PASR_LastName,

                                            PASR_FirstName = ((a.PASR_FirstName == null || a.PASR_FirstName == "" ? "" : " " + a.PASR_FirstName) + (a.PASR_MiddleName == null || a.PASR_MiddleName == "" || a.PASR_MiddleName == "0" ? "" : " " + a.PASR_MiddleName) + (a.PASR_LastName == null || a.PASR_LastName == "" || a.PASR_LastName == "0" ? "" : " " + a.PASR_LastName)).Trim(),
                                        }
           ).ToArray();

                    data.preadmissionstudentlist = (from a in _YearlyFeeGroupMappingContext.stuapp
                                                    from b in _YearlyFeeGroupMappingContext.admissioncls
                                                    where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PASR_Adm_Confirm_Flag == false)
                                                    select new FeeStudentTransactionDTO
                                                    {
                                                        PASR_Id = a.pasr_id,
                                                        PASR_FirstName = a.PASR_FirstName,
                                                        PASR_MiddleName = a.PASR_MiddleName,
                                                        PASR_LastName = a.PASR_LastName,
                                                        classname = b.ASMCL_ClassName,
                                                    }
  ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();


                    var fetchmaxfypid = _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ApplicationDMO.OrderByDescending(t => t.FYP_Id).Take(5).Select(t => t.FYP_Id).ToList();

                    data.receiparraydelete = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                              from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                              from c in _YearlyFeeGroupMappingContext.stuapp
                                              from d in _YearlyFeeGroupMappingContext.admissioncls
                                              where (a.FYP_Id == b.FYP_Id && b.PASA_Id == c.pasr_id && c.ASMCL_Id == d.ASMCL_Id && fetchmaxfypid.Contains(a.FYP_Id) && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id)
                                              select new FeeStudentTransactionDTO
                                              {
                                                  PASR_Id = c.pasr_id,
                                                  PASR_FirstName = c.PASR_FirstName,
                                                  PASR_MiddleName = c.PASR_MiddleName,
                                                  PASR_LastName = c.PASR_LastName,
                                                  FYP_Receipt_No = a.FYP_Receipt_No,
                                                  FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                  FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                  classname = d.ASMCL_ClassName,
                                                  FYP_Id = a.FYP_Id,
                                                  pasR_MobileNo = c.PASR_MobileNo,
                                                  FYP_Date = a.FYP_Date,
                                                  PASR_RegistrationNo = c.PASR_Applicationno,
                                                  pasR_emailId = c.PASR_emailId
                                              }
         ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();

                }

                else if (data.filterinitialdata== "Prospectus")
                {

          //          data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Prospectus
          //                              where (a.MI_ID == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id &&  ((a.PASP_First_Name.Trim() + ' ' + a.PASP_Middle_Name.Trim() + ' ' + a.PASP_Last_Name.Trim()).StartsWith(data.searchfilter) || (a.PASP_First_Name.Trim() + a.PASP_Middle_Name.Trim() + ' ' + a.PASP_Last_Name.Trim()).StartsWith(data.searchfilter) || a.PASP_First_Name.StartsWith(data.searchfilter) || a.PASP_Middle_Name.StartsWith(data.searchfilter) || a.PASP_Last_Name.StartsWith(data.searchfilter)))
          //                              select new FeeStudentTransactionDTO
          //                              {
          //                                  PASR_Id = a.PASP_Id,
          //                                  // PASR_FirstName = a.PASR_FirstName,
          //                                  PASR_MiddleName = a.PASP_Middle_Name,
          //                                  PASR_LastName = a.PASP_Last_Name,

          //                                  PASR_FirstName = ((a.PASP_First_Name == null || a.PASP_First_Name == "" ? "" : " " + a.PASP_First_Name) + (a.PASP_Middle_Name == null || a.PASP_Middle_Name == "" || a.PASP_Middle_Name == "0" ? "" : " " + a.PASP_Middle_Name) + (a.PASP_Last_Name == null || a.PASP_Last_Name == "" || a.PASP_Last_Name == "0" ? "" : " " + a.PASP_Last_Name)).Trim(),
          //                              }
          //).ToArray();

                    data.preadmissionstudentlist = (from a in _YearlyFeeGroupMappingContext.Prospectus
                                                    from b in _YearlyFeeGroupMappingContext.admissioncls
                                                    where (a.ASMCL_Id == b.ASMCL_Id && a.MI_ID == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                                    select new FeeStudentTransactionDTO
                                                    {
                                                        PASR_Id = a.PASP_Id,
                                                        PASR_FirstName = a.PASP_First_Name,
                                                        PASR_MiddleName = a.PASP_Middle_Name,
                                                        PASR_LastName = a.PASP_Last_Name,
                                                        classname = b.ASMCL_ClassName,
                                                    }
 ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();


                    var fetchmaxfypid = _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ProspectusDMO.OrderByDescending(t => t.FYP_Id).Take(5).Select(t => t.FYP_Id).ToList();

                    data.receiparraydelete = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                              from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ProspectusDMO
                                              from c in _YearlyFeeGroupMappingContext.Prospectus
                                              from d in _YearlyFeeGroupMappingContext.admissioncls
                                              where (a.FYP_Id == b.FYP_Id && b.PASP_Id == c.PASP_Id && c.ASMCL_Id == d.ASMCL_Id && fetchmaxfypid.Contains(a.FYP_Id) && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id)
                                              select new FeeStudentTransactionDTO
                                              {
                                                  PASR_Id = c.PASP_Id,
                                                  PASR_FirstName = c.PASP_First_Name,
                                                  PASR_MiddleName = c.PASP_Middle_Name,
                                                  PASR_LastName = c.PASP_Last_Name,
                                                  FYP_Receipt_No = a.FYP_Receipt_No,
                                                  FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                  FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                  classname = d.ASMCL_ClassName,
                                                  FYP_Id = a.FYP_Id,
                                                  pasR_MobileNo = c.PASP_MobileNo,
                                                  FYP_Date = a.FYP_Date,
                                                  PASR_RegistrationNo = c.PASP_ProspectusNo,
                                                  pasR_emailId = c.PASP_EmailId
                                              }
         ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();

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






















