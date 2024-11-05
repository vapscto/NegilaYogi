using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using CommonLibrary;
using Microsoft.Extensions.Logging;
using System.Globalization;
using DomainModel.Model.com.vaps.Fee;
using PreadmissionDTOs.com.vaps.Fees;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Dynamic;

namespace WebApplication1.Services
{
    public class ProspectusImpl : Interfaces.prospectus
    {
        private static ConcurrentDictionary<string, ProspectusDTO> _login =
             new ConcurrentDictionary<string, ProspectusDTO>();

        public ProspectusContext _ProspectusContext;
        public DomainModelMsSqlServerContext _context;

        public FeeGroupContext _feecontext;

        readonly ILogger<ProspectusImpl> _logger;
        public ProspectusImpl(ProspectusContext prospectuscontext, DomainModelMsSqlServerContext context, ILogger<ProspectusImpl> log, FeeGroupContext feecontext)
        {
            _ProspectusContext = prospectuscontext;
            _context = context;
            _logger = log;
            _feecontext = feecontext;
        }

        public ProspectusDTO getfilePath(int miId)
        {
            ProspectusDTO dto = new ProspectusDTO();
            var filepath = _context.ProspectusFilePath.Where(d => d.MI_ID == miId).ToList();
            dto.prospectusfilePath = filepath.FirstOrDefault().IPPC_Path;
            return dto;
        }

        //public PaymentDetails payuresponse(PaymentDetails response)
        //{
        //    PaymentDetails dto = new PaymentDetails();
        //    if (response.status == "success")
        //    {
               
        //        var confirmstatus = _ProspectusContext.Database.ExecuteSqlCommand("Insert_fee_tables @p0,@p1,@p2,@p3,@p4,@p5,@p6", response.udf3, response.udf4, response.udf2, response.udf5, response.amount, response.txnid, response.mihpayid);


        //        if (confirmstatus > 0)
        //        {
        //            SMS sms = new SMS(_context);
        //            ProspectusDTO enq = new ProspectusDTO();

        //            enq.MI_ID = Convert.ToInt64(response.udf3);
        //            enq.PASP_MobileNo = response.phone;
        //            enq.PASP_Id = Convert.ToInt64(response.udf2);
        //            enq.PASP_EmailId = response.email;

        //            Email Email = new Email(_context);

        //            Email.sendmail(enq.MI_ID, enq.PASP_EmailId, "PROSPECTUS", enq.PASP_Id);


        //            sms.sendSms(enq.MI_ID, enq.PASP_MobileNo, "PROSPECTUS", enq.PASP_Id);

        //        }
        //    }
        //    else
        //    {
        //        dto.status = response.status;
        //    }

        //    return response;
        //}


        public PaymentDetails payuresponse(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            FeeStudentTransactionDTO data = new FeeStudentTransactionDTO();
            //   FeePaymentDetailsDMO feeypayment = Mapper.Map<FeePaymentDetailsDMO>(response);
            if (response.status == "success")
            {


                stu.MI_Id = Convert.ToInt64(response.udf3);
                stu.PASR_MobileNo = response.phone;
                stu.pasR_Id = Convert.ToInt64(response.udf2);
                stu.PASR_emailId = response.email;
                stu.ASMAY_Id = Convert.ToInt64(response.udf5);


                data.MI_Id = Convert.ToInt64(response.udf3);
                data.ASMCL_ID = Convert.ToInt64(response.udf4);
                data.ASMAY_Id = Convert.ToInt64(response.udf5);

                string recno = "";

                var confirmstatus = 0;

                if (recno != "0")
                {
                    confirmstatus = _ProspectusContext.Database.ExecuteSqlCommand("Preadmission_Application_online @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", response.udf3, response.udf4, response.udf2, response.udf5, response.amount, response.txnid, response.mihpayid, response.udf6, recno);
                }
                else
                {
                    recno = response.txnid;
                    confirmstatus = _ProspectusContext.Database.ExecuteSqlCommand("Preadmission_Application_online @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", response.udf3, response.udf4, response.udf2, response.udf5, response.amount, response.txnid, response.mihpayid, response.udf6, recno);
                }



                if (confirmstatus > 0)
                {

                    List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                    mstConfig = _context.mstConfig.Where(d => d.MI_Id.Equals(stu.MI_Id) && d.ASMAY_Id.Equals(stu.ASMAY_Id)).ToList();

                    //if (mstConfig.FirstOrDefault().ISPAC_ApplMailFlag == 1)
                    //{
                    //    Email Email = new Email(_context);

                    //    Email.sendmail(stu.MI_Id, stu.PASR_emailId, "STUDENT_REGISTRATION", stu.pasR_Id);
                    //}

                    //if (mstConfig.FirstOrDefault().ISPAC_ApplSMSFlag == 1)
                    //{
                    //    SMS sms = new SMS(_context);
                    //    sms.sendSms(stu.MI_Id, stu.PASR_MobileNo, "STUDENT_REGISTRATION", stu.pasR_Id);

                    //}
                }



            }
            else
            {
                dto.status = response.status;



            }

            return response;
        }


        public ProspectusDTO countrydrp(ProspectusDTO prosp)
        {
            try
            {

                var Acdemic_preadmission = _context.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == prosp.MI_ID).Select(d => d.ASMAY_Id).FirstOrDefault();


                prosp.ASMAY_Id = Acdemic_preadmission;

                var filepath = _context.ProspectusFilePath.Where(d => d.MI_ID == prosp.MI_ID).ToList();
                if(filepath.Count>0)
                {
                    prosp.prospectusfilePath = filepath.FirstOrDefault().IPPC_Path;
                }
                

                List<Country> allCountry = new List<Country>();
                allCountry = _ProspectusContext.country.ToList();
                prosp.countryDrpDown = allCountry.ToArray();

                //List<City> allcity = new List<City>();
                //allcity = _ProspectusContext.city.ToList();
                //prosp.cityDrpDown = allcity.ToArray();

                List<State> allstate = new List<State>();
                allstate = _ProspectusContext.State.ToList();
                prosp.stateDrpDown = allstate.ToArray();

                List<AdmissionClass> allclass = new List<AdmissionClass>();
                allclass = _ProspectusContext.AdmClass.Where(t =>  t.MI_Id==prosp.MI_ID && t.ASMCL_ActiveFlag==true  && t.ASMCL_PreadmFlag == 1).ToList();
                prosp.classdropDown = allclass.ToArray();

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _ProspectusContext.year.Where(t => t.ASMAY_Id==prosp.ASMAY_Id && t.MI_Id==prosp.MI_ID).ToList();
                prosp.yeardropDown = allyear.ToArray();

                List<MasterReference> allreference = new List<MasterReference>();
                allreference = _ProspectusContext.reference.ToList();
                prosp.referencedropDown = allreference.ToArray();

                List<MasterSource> allsource = new List<MasterSource>();
                allsource = _ProspectusContext.source.ToList();
                prosp.sourcedropDown = allsource.ToArray();

                //List<Prospectus> allprospectus = new List<Prospectus>();
                //allprospectus = _ProspectusContext.prospectus.Where(t => t.ASMAY_Id.Equals(prosp.ASMAY_Id) && t.MI_ID.Equals(prosp.MI_ID) && t.id.Equals(prosp.id)).ToList();
                //prosp.Prospectuslist = allprospectus.ToArray();

                var rolelist = _ProspectusContext.MasterRoleType.Where(t => t.IVRMRT_Id == prosp.roleId).ToList();

                if (rolelist[0].IVRMRT_Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase))
                {
                    List<Prospectus> allorganisation = new List<Prospectus>();
                    allorganisation = _ProspectusContext.prospectus.Where(t => t.ASMAY_Id==prosp.ASMAY_Id && t.MI_ID==prosp.MI_ID).ToList();
                    prosp.Prospectuslist = allorganisation.ToArray();
                }
                else
                {
                    List<Prospectus> allorganisation = new List<Prospectus>();
                    allorganisation = _ProspectusContext.prospectus.Where(t => t.ASMAY_Id==prosp.ASMAY_Id && t.MI_ID==prosp.MI_ID && t.id==prosp.id).ToList();
                    prosp.Prospectuslist = allorganisation.ToArray();
                }
                prosp.prospectusPaymentlist = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "P").ToArray();

                Array[] showdata3 = new Array[1];
                List<MasterConfiguration> Allname3 = new List<MasterConfiguration>();
                Allname3 = _ProspectusContext.MasterConfiguration.Where(t => t.ASMAY_Id==prosp.ASMAY_Id && t.MI_Id==prosp.MI_ID).ToList().ToList();
                prosp.MasterConfiguration = Allname3.ToArray();
                prosp.ISPAC_EnquiryApplFlag = Allname3[0].ISPAC_EnquiryApplFlag;

                //_logger.LogInformation(prosp.transnumbconfigurationsettingsss.IMN_AutoManualFlag);

                //if (prosp.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                //{
                //    prosp.GeneratedNumber = GenerateNumber(prosp.MI_ID, prosp.ASMAY_Id, prosp);
                //    _logger.LogInformation(prosp.GeneratedNumber);
                //}
                //else if (prosp.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Manual")
                //{
                //    prosp.GeneratedNumber = "";
                //}
            }

            catch (Exception e)
            {
                //Console.WriteLine(ee.Message);
                _logger.LogDebug(e.Message);
                _logger.LogError(e.Message);
                _context.Dispose();
                _ProspectusContext.Dispose();
            }

            return prosp;
        }

        public StateDTO enqdrpcountrydata(int id)
        {
            StateDTO enq = new StateDTO();
            try
            {
                Array[] drpall = new Array[3];
                List<State> allstate = new List<State>();
                allstate = _ProspectusContext.State.ToList();
                allstate = _ProspectusContext.State.Where(t => t.IVRMMC_Id.Equals(id)).ToList();
                enq.stateDrpDown = allstate.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return enq;
        }

        public CityDTO getcity(int id)
        {
            CityDTO enq = new CityDTO();
            try
            {
                Array[] drpall = new Array[3];
                List<City> allcity = new List<City>();
                allcity = _ProspectusContext.city.ToList();
                //allcity = _OrganisationContext.city.Where(t => t.IVRMMS_Id.Equals(stateid) && t.IVRMMC_Id.Equals(stateid)).ToList();
                allcity = _ProspectusContext.city.Where(t => t.IVRMMS_Id.Equals(id)).ToList();
                enq.cityDrpDown = allcity.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enq;
        }

        public ProspectusDTO getdetails(int id)
        {
            ProspectusDTO prosp = new ProspectusDTO();
            try
            {
                List<Prospectus> lorg = new List<Prospectus>();
                lorg = _ProspectusContext.prospectus.AsNoTracking().Where(t => t.PASP_Id.Equals(id)).ToList();
                //prosp.Prospectuslist = lorg.ToArray();

                //added New Things
                using (var cmd = _ProspectusContext.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "AttendanceReport_NEW";
                    cmd.CommandText = "Prospectus_Calkutta_Edit";
                    // AttendanceReport_NEW
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 90000000;
                    cmd.Parameters.Add(new SqlParameter("@PASP_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(id)
                    });
                 
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                   
                        using (var dataReader =cmd.ExecuteReader())
                        {
                            while ( dataReader.Read())
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
                        prosp.Prospectuslist = retObject.ToArray();
                   
                  
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return prosp;
        }
        public Enq getEnqdetails(searchEnquiryDTO serchinput)
        {
            Enq prosp = new Enq();
            try
            {
                List<Enquiry> lorg = new List<Enquiry>();
                if (serchinput.id == 1)
                {
                    lorg = _ProspectusContext.enquiry.Where(t => t.PASE_EnquiryNo == serchinput.enquiryChoice && t.MI_Id.Equals(serchinput.MI_ID) && t.Id.Equals(serchinput.userid)).ToList();
                }
                else
                {
                    var str = serchinput.enquiryChoice;
                    string[] namelist = str.Split(' ');
                    if (namelist.Length == 1)
                    {
                        lorg = _ProspectusContext.enquiry.Where(t => t.PASE_FirstName.Contains(namelist[0]) && t.MI_Id == serchinput.MI_ID && t.Id == serchinput.userid).ToList();
                        if (lorg.Count == 0)
                        {
                            lorg = _ProspectusContext.enquiry.Where(t => t.PASE_MiddleName.Contains(namelist[0]) && t.MI_Id == serchinput.MI_ID && t.Id == serchinput.userid).ToList();
                        }
                        if (lorg.Count == 0)
                        {
                            lorg = _ProspectusContext.enquiry.Where(t => t.PASE_LastName.Contains(namelist[0]) && t.MI_Id == serchinput.MI_ID && t.Id == serchinput.userid).ToList();
                        }
                    }
                    else if (namelist.Length == 3)
                    {
                        lorg = _ProspectusContext.enquiry.Where(t => t.PASE_FirstName.Contains(namelist[0]) || t.PASE_MiddleName.Contains(namelist[1]) || t.PASE_LastName.Contains(namelist[2]) && t.MI_Id == serchinput.MI_ID && t.Id == serchinput.userid).ToList();
                    }
                    // lorg = _ProspectusContext.enquiry.Where(t => t.PASE_FirstName==serchinput.enquiryChoice && t.MI_Id.Equals(serchinput.MI_ID) && t.Id.Equals(serchinput.userid)).ToList();

                }

                List<State> allstate = new List<State>();
                allstate = _ProspectusContext.State.ToList();
                prosp.stateDrpDown = allstate.ToArray();

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _ProspectusContext.year.Where(t => t.ASMAY_Id.Equals(lorg[0].ASMAY_Id) && t.MI_Id.Equals(serchinput.MI_ID)).ToList();
                prosp.yearDrpDwn = allyear.ToArray();

                prosp.enquiryList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return prosp;
        }
        public async Task<ProspectusDTO> saveProsdet(ProspectusDTO pros)
        {
            try
            {

                var Acdemic_preadmission = _context.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == pros.MI_ID).Select(d => d.ASMAY_Id).FirstOrDefault();


                pros.ASMAY_Id = Acdemic_preadmission;

                Prospectus enq = Mapper.Map<Prospectus>(pros);
                if (enq.PASP_Id > 0)
                {
                    var result = _ProspectusContext.prospectus.Single(t => t.PASP_Id == enq.PASP_Id);
                    result.IVRMMCT_Id = enq.IVRMMCT_Id;
                    result.IVRMMS_Id = enq.IVRMMS_Id;
                    result.IVRMMC_Id = enq.IVRMMC_Id;
                    result.ASMAY_Id = enq.ASMAY_Id;
                    result.ASMCL_Id = enq.ASMCL_Id;
                    result.PAMR_Id = enq.PAMR_Id;
                    result.PAMS_Id = enq.PAMS_Id;
                    result.PASP_Area = enq.PASP_Area;
                    result.PASP_Date = result.PASP_Date;
                    result.PASP_EmailId = enq.PASP_EmailId;
                    result.PASP_Enquiry = enq.PASP_Enquiry;
                    result.PASP_First_Name = enq.PASP_First_Name;
                    result.PASP_HouseNo = enq.PASP_HouseNo;
                    result.PASP_Last_Name = enq.PASP_Last_Name;
                    result.PASP_Middle_Name = enq.PASP_Middle_Name;
                    result.PASP_MobileNo = enq.PASP_MobileNo;
                    result.PASP_PhoneNo = enq.PASP_PhoneNo;
                    result.PASP_Pincode = enq.PASP_Pincode;
                    result.PASP_Street = enq.PASP_Street;

                    result.MI_ID = enq.MI_ID;
                    result.id = enq.id;


                    //added by 02/02/2017

                    result.UpdatedDate = DateTime.Now;
                    _ProspectusContext.Update(result);

                    var flag = _ProspectusContext.SaveChanges();
                    enq.PASP_Id = result.PASP_Id;

                    if (flag == 1)
                    {
                        //if (pros.configurationsettings.ISPAC_ProsptFeeApp == 1)
                        //{
                        //    pros.payementcheck = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "P" && t.PASA_Id == enq.PASP_Id).Count();

                        //    if (pros.payementcheck == 0)
                        //    {
                        //        pros.paydet = paymentPart(pros);
                        //    }
                        //}
                        //else
                        //{

                        //    SMS sms = new SMS(_context);

                        //    string s = await sms.sendSms(enq.MI_ID, result.PASP_MobileNo, "PROSPECTUS", enq.PASP_Id);

                        //    Email Email = new Email(_context);

                        //    string m = Email.sendmail(enq.MI_ID, result.PASP_EmailId, "PROSPECTUS", enq.PASP_Id);
                        //}
                        _ProspectusContext.Database.ExecuteSqlCommand("Insert_Prospectus_Calkutta @p0,@p1,@p2", pros.PASP_Date, pros.PASP_Id, pros.returnval);
                        pros.returnval = "true";

                    }
                    else
                    {
                        pros.returnval = "false";
                    }
                }
                else
                {
                    //duplication check
                    //var PASP_MobileNo = _ProspectusContext.prospectus.Where(t => t.PASP_MobileNo == enq.PASP_MobileNo && t.MI_ID == enq.MI_ID && t.ASMAY_Id == enq.ASMAY_Id).Count();
                    //if (PASP_MobileNo > 0)
                    //{
                    //    pros.returnval = "mobileDuplicate";
                    //    return pros;
                    //}

                    //var PASP_PhoneNo = _ProspectusContext.prospectus.Where(t => t.PASP_PhoneNo == enq.PASP_PhoneNo && t.MI_ID == enq.MI_ID && t.ASMAY_Id == enq.ASMAY_Id).Count();
                    //if (PASP_PhoneNo > 0)
                    //{
                    //    pros.returnval = "phoneDuplicate";
                    //    return pros;
                    //}

                    //var PASP_EmailId = _ProspectusContext.prospectus.Where(t => t.PASP_EmailId == enq.PASP_EmailId && t.MI_ID == enq.MI_ID && t.ASMAY_Id == enq.ASMAY_Id).Count();
                    //if (PASP_EmailId > 0)
                    //{
                    //    pros.returnval = "emailDuplicate";
                    //    return pros;
                    //}
                    //Get Autogenerated Prospectuse number

                    //MasterConfigurations
                    if (pros.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                    {
                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                        pros.transnumbconfigurationsettingsss.MI_Id = pros.MI_ID;
                        pros.transnumbconfigurationsettingsss.ASMAY_Id = pros.ASMAY_Id;
                        enq.PASP_ProspectusNo = a.GenerateNumber(pros.transnumbconfigurationsettingsss);
                        // enq.PASP_ProspectusNo = GenerateNumber(pros.MI_ID, pros.ASMAY_Id, pros);
                    }
                    else if (pros.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Manual")
                    {
                        enq.PASP_ProspectusNo = pros.PASP_ProspectusNo;

                        var count = _ProspectusContext.prospectus.Where(t => t.MI_ID == pros.MI_ID && t.PASP_ProspectusNo == pros.PASP_ProspectusNo).Count();
                        if (count > 0)
                        {
                            pros.returnval = "ProspectusNoDuplicate";
                            return pros;
                        }
                    }
                    else if (pros.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "serial")
                    {
                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                        pros.transnumbconfigurationsettingsss.MI_Id = pros.MI_ID;
                        pros.transnumbconfigurationsettingsss.ASMAY_Id = pros.ASMAY_Id;
                        enq.PASP_ProspectusNo = a.GenerateNumber(pros.transnumbconfigurationsettingsss);

                    }

                    /////////////////

                    enq.PASP_Date = System.DateTime.Today.Date;


                    //added by 02/02/2017
                    enq.CreatedDate = DateTime.Now;
                    enq.UpdatedDate = DateTime.Now;
                    _ProspectusContext.Add(enq);
                    var flag = _ProspectusContext.SaveChanges();
                    if (flag == 1)
                    {
                        //if (pros.configurationsettings.ISPAC_ProsptFeeApp == 1)
                        //{
                        //    pros.paydet = paymentPart(pros);
                        //}
                        //else
                        //{
                        //    SMS sms = new SMS(_context);

                        //    string s = await sms.sendSms(enq.MI_ID, enq.PASP_MobileNo, "PROSPECTUS", enq.PASP_Id);

                        //    Email Email = new Email(_context);

                        //    string m = Email.sendmail(enq.MI_ID, enq.PASP_EmailId, "PROSPECTUS", enq.PASP_Id);

                        //}
                        pros.PASP_Id = _ProspectusContext.prospectus.Where(R => R.MI_ID == pros.MI_ID).OrderByDescending(R => R.PASP_Id).FirstOrDefault().PASP_Id;

                        _ProspectusContext.Database.ExecuteSqlCommand("Insert_Prospectus_Calkutta @p0,@p1,@p2", pros.PASP_Date, pros.PASP_Id, pros.returnval);
                         pros.returnval = "true";
                    }
                    else
                    {
                        pros.returnval = "false";
                    }
                }


                var rolelist = await _ProspectusContext.MasterRoleType.Where(t => t.IVRMRT_Id == pros.roleId).ToListAsync();

                //List<Prospectus> allorganisation = new List<Prospectus>();
                //allorganisation = _ProspectusContext.prospectus.Where(t => t.ASMAY_Id.Equals(pros.ASMAY_Id) && t.MI_ID.Equals(pros.MI_ID) && t.id.Equals(pros.id)).ToList();
                //pros.Prospectuslist = allorganisation.ToArray();
                List<Prospectus> allprospectus = new List<Prospectus>();
                allprospectus = _ProspectusContext.prospectus.Where(t => t.ASMAY_Id.Equals(pros.ASMAY_Id) && t.MI_ID.Equals(pros.MI_ID) && t.id.Equals(pros.id)).ToList();

                pros.Prospectuslist = allprospectus.ToArray();


                if (rolelist[0].IVRMRT_Role == "ADMIN" || rolelist[0].IVRMRT_Role == "Admin")
                {
                    List<Prospectus> allorganisation = new List<Prospectus>();
                    allorganisation = _ProspectusContext.prospectus.Where(t => t.ASMAY_Id.Equals(pros.ASMAY_Id) && t.MI_ID.Equals(pros.MI_ID)).ToList();
                    pros.Prospectuslist = allorganisation.ToArray();
                }
                else
                {
                    List<Prospectus> allorganisation = new List<Prospectus>();
                    allorganisation = _ProspectusContext.prospectus.Where(t => t.ASMAY_Id.Equals(pros.ASMAY_Id) && t.MI_ID.Equals(pros.MI_ID) && t.id.Equals(pros.id)).ToList();
                    pros.Prospectuslist = allorganisation.ToArray();
                }

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return pros;
        }


        public static string GetCurrentFinancialYear()
        {
            int CurrentYear = DateTime.Today.Year;
            int PreviousYear = DateTime.Today.Year - 1;
            int NextYear = DateTime.Today.Year + 1;
            string PreYear = PreviousYear.ToString();
            string NexYear = NextYear.ToString();
            string CurYear = CurrentYear.ToString();
            string FinYear = null;

            if (DateTime.Today.Month > 3)
                FinYear = CurYear;
            else
                FinYear = PreYear;
            return FinYear.Trim();
        }

        public ProspectusDTO deleterec(ProspectusDTO org)
        {
            // ProspectusDTO org = new ProspectusDTO();
            List<Prospectus> lorg = new List<Prospectus>(); // Mapper.Map<Organisation>(org);


            try
            {
                lorg = _ProspectusContext.prospectus.Where(t => t.PASP_Id.Equals(org.PASP_Id)).ToList();

                if (lorg.Any())
                {
                    _ProspectusContext.Remove(lorg.ElementAt(0));

                    var flag = _ProspectusContext.SaveChanges();
                    if (flag == 1)
                    {
                        org.returnval = "true";
                    }
                    else
                    {
                        org.returnval = "false";
                    }
                }

               

                var rolelist = _ProspectusContext.MasterRoleType.Where(t => t.IVRMRT_Id == org.roleId).ToList();
                if (rolelist[0].IVRMRT_Role == "Admin" || rolelist[0].IVRMRT_Role == "ADMIN")
                {
                    List<Prospectus> allorganisation1 = new List<Prospectus>();
                    allorganisation1 = _ProspectusContext.prospectus.Where(t => t.ASMAY_Id.Equals(org.ASMAY_Id) && t.MI_ID.Equals(org.MI_ID)).ToList();
                    org.Prospectuslist = allorganisation1.ToArray();
                }
                else
                {
                    List<Prospectus> allorganisation2 = new List<Prospectus>();
                    allorganisation2 = _ProspectusContext.prospectus.Where(t => t.ASMAY_Id.Equals(org.ASMAY_Id) && t.MI_ID.Equals(org.MI_ID) && t.id.Equals(org.id)).ToList();
                    org.Prospectuslist = allorganisation2.ToArray();
                }




                List<Prospectus> allorganisation = new List<Prospectus>();
                allorganisation = _ProspectusContext.prospectus.Where(t => t.ASMAY_Id.Equals(org.ASMAY_Id) && t.MI_ID.Equals(org.MI_ID) && t.id.Equals(org.id)).ToList();
                org.Prospectuslist = allorganisation.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return org;
        }
        public ProspectusDTO searchByColumn(ProspectusDTO acd)
        {
            try
            {
                if (acd.SearchColumn == "1")
                {
                    List<Prospectus> allorganisation = new List<Prospectus>();
                    allorganisation = _ProspectusContext.prospectus.OrderByDescending(d => d.UpdatedDate).Where(w => w.PASP_First_Name == acd.EnteredData).ToList();
                    acd.Prospectuslist = allorganisation.ToArray();
                    if (allorganisation.Count > 0)
                    {
                        acd.count = allorganisation.Count;
                    }
                    else
                    {
                        acd.count = 0;
                    }
                }
                else if (acd.SearchColumn == "2")
                {
                    try
                    {
                        //DateTime date = DateTime.ParseExact(acd.EnteredData, "dd/MM/yyyy",
                        //            CultureInfo.InvariantCulture);
                        List<Prospectus> allorganisation = new List<Prospectus>();
                        allorganisation = _ProspectusContext.prospectus.OrderByDescending(d => d.UpdatedDate).Where(w => w.PASP_Middle_Name == acd.EnteredData).ToList();
                        acd.Prospectuslist = allorganisation.ToArray();
                        if (allorganisation.Count > 0)
                        {
                            acd.count = allorganisation.Count;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        //acd.message = "Please Enter date in dd/MM/yyyy format";
                        //List<MasterAcademic> allorganisation = new List<MasterAcademic>();
                        //allorganisation = _AcademicContext.Academic.OrderByDescending(d => d.UpdatedDate).ToList();
                        //acd.AcademicList = allorganisation.ToArray();
                        //if (allorganisation.Count > 0)
                        //{
                        //    acd.count = allorganisation.Count;
                        //}
                        //else
                        //{
                        //    acd.count = 0;
                        //}
                    }
                }
                else if (acd.SearchColumn == "3")
                {
                    try
                    {
                        //DateTime date = DateTime.ParseExact(acd.EnteredData, "dd/MM/yyyy",
                        //           CultureInfo.InvariantCulture);
                        List<Prospectus> allorganisation = new List<Prospectus>();
                        allorganisation = _ProspectusContext.prospectus.OrderByDescending(d => d.UpdatedDate).Where(w => w.PASP_Last_Name == acd.EnteredData).ToList();
                        acd.Prospectuslist = allorganisation.ToArray();
                        if (allorganisation.Count > 0)
                        {
                            acd.count = allorganisation.Count;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        //acd.message = "Please Enter date in dd/MM/yyyy format";
                        //List<MasterAcademic> allorganisation = new List<MasterAcademic>();
                        //allorganisation = _AcademicContext.Academic.OrderByDescending(d => d.UpdatedDate).ToList();
                        //acd.AcademicList = allorganisation.ToArray();
                        //if (allorganisation.Count > 0)
                        //{
                        //    acd.count = allorganisation.Count;
                        //}
                        //else
                        //{
                        //    acd.count = 0;
                        //}
                    }

                }
                else if (acd.SearchColumn == "4")
                {
                    try
                    {
                        DateTime date = DateTime.ParseExact(acd.EnteredData, "dd/MM/yyyy",
                                 CultureInfo.InvariantCulture);
                        List<Prospectus> allorganisation = new List<Prospectus>();
                        allorganisation = _ProspectusContext.prospectus.OrderByDescending(d => d.UpdatedDate).Where(w => w.PASP_Date.Value.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd"))).ToList();
                        acd.Prospectuslist = allorganisation.ToArray();
                        if (allorganisation.Count > 0)
                        {
                            acd.count = allorganisation.Count;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        acd.message = "Please Enter date in dd/MM/yyyy format";
                        List<Prospectus> allorganisation = new List<Prospectus>();
                        allorganisation = _ProspectusContext.prospectus.OrderByDescending(d => d.UpdatedDate).ToList();
                        acd.Prospectuslist = allorganisation.ToArray();
                        if (allorganisation.Count > 0)
                        {
                            acd.count = allorganisation.Count;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                    }

                }
                else if (acd.SearchColumn == "5")
                {
                    try
                    {
                        //DateTime date = DateTime.ParseExact(acd.EnteredData, "dd/MM/yyyy",
                        //         CultureInfo.InvariantCulture);
                        List<Prospectus> allorganisation = new List<Prospectus>();
                        allorganisation = _ProspectusContext.prospectus.OrderByDescending(d => d.UpdatedDate).Where(w => w.PASP_ProspectusNo == acd.EnteredData).ToList();
                        acd.Prospectuslist = allorganisation.ToArray();
                        if (allorganisation.Count > 0)
                        {
                            acd.count = allorganisation.Count;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        //acd.message = "Please Enter date in dd/MM/yyyy format";
                        //List<Prospectus> allorganisation = new List<Prospectus>();
                        //allorganisation = _ProspectusContext.prospectus.OrderByDescending(d => d.UpdatedDate).ToList();
                        //acd.Prospectuslist = allorganisation.ToArray();
                        //if (allorganisation.Count > 0)
                        //{
                        //    acd.count = allorganisation.Count;
                        //}
                        //else
                        //{
                        //    acd.count = 0;
                        //}
                    }

                }
                else if (acd.SearchColumn == "6")
                {
                    try
                    {
                        //DateTime date = DateTime.ParseExact(acd.EnteredData, "dd/MM/yyyy",
                        //         CultureInfo.InvariantCulture);
                        List<Prospectus> allorganisation = new List<Prospectus>();
                        allorganisation = _ProspectusContext.prospectus.OrderByDescending(d => d.UpdatedDate).Where(w => w.PASP_MobileNo.Equals(acd.EnteredData)).ToList();
                        acd.Prospectuslist = allorganisation.ToArray();
                        if (allorganisation.Count > 0)
                        {
                            acd.count = allorganisation.Count;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        //acd.message = "Please Enter date in dd/MM/yyyy format";
                        //List<Prospectus> allorganisation = new List<Prospectus>();
                        //allorganisation = _ProspectusContext.prospectus.OrderByDescending(d => d.UpdatedDate).ToList();
                        //acd.Prospectuslist = allorganisation.ToArray();
                        //if (allorganisation.Count > 0)
                        //{
                        //    acd.count = allorganisation.Count;
                        //}
                        //else
                        //{
                        //    acd.count = 0;
                        //}
                    }

                }
                else if (acd.SearchColumn == "7")
                {
                    try
                    {
                        //DateTime date = DateTime.ParseExact(acd.EnteredData, "dd/MM/yyyy",
                        //         CultureInfo.InvariantCulture);
                        List<Prospectus> allorganisation = new List<Prospectus>();
                        allorganisation = _ProspectusContext.prospectus.OrderByDescending(d => d.UpdatedDate).Where(w => w.PASP_EmailId.Equals(acd.EnteredData)).ToList();
                        acd.Prospectuslist = allorganisation.ToArray();
                        if (allorganisation.Count > 0)
                        {
                            acd.count = allorganisation.Count;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        //acd.message = "Please Enter date in dd/MM/yyyy format";
                        //List<Prospectus> allorganisation = new List<Prospectus>();
                        //allorganisation = _ProspectusContext.prospectus.OrderByDescending(d => d.UpdatedDate).ToList();
                        //acd.Prospectuslist = allorganisation.ToArray();
                        //if (allorganisation.Count > 0)
                        //{
                        //    acd.count = allorganisation.Count;
                        //}
                        //else
                        //{
                        //    acd.count = 0;
                        //}
                    }

                }
                else
                {
                    List<Prospectus> allorganisation = new List<Prospectus>();
                    allorganisation = _ProspectusContext.prospectus.OrderByDescending(d => d.UpdatedDate).ToList();
                    acd.Prospectuslist = allorganisation.ToArray();
                    if (allorganisation.Count > 0)
                    {
                        acd.count = allorganisation.Count;
                    }
                    else
                    {
                        acd.count = 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return acd;
        }


        //public Array paymentPart(Prospectus enq)
        //{
        //    Payment pay = new Payment(_context);
        //    ProspectusDTO data = new ProspectusDTO();

        //    List<Prospepaymentamount> paymentdetails = new List<Prospepaymentamount>();
        //    paymentdetails = _ProspectusContext.Prospepaymentamount.Where(t => t.IVRMOP_MIID == enq.MI_ID).ToList();
           


        //    PaymentDetails PaymentDetailsDto = new PaymentDetails();
        //    PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().IVRMOP_MERCHANT_KEY;
        //    //PaymentDetailsDto.amount = paymentdetails.FirstOrDefault().IVRMOP_PROS_AMOUNT;
        //   // PaymentDetailsDto.amount = Convert.ToString(FeeAmountresult.FMA_Amount);
        //    PaymentDetailsDto.trans_id = "201";
        //    PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";
        //    PaymentDetailsDto.productinfo = "Prospectus";
        //    PaymentDetailsDto.firstname = enq.PASP_First_Name;
        //    PaymentDetailsDto.email = enq.PASP_EmailId;
        //    PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().IVRMOP_SALT;
        //    PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().IVRMOP_BASE_URL;
        //    PaymentDetailsDto.phone = enq.PASP_MobileNo;
        //    PaymentDetailsDto.udf1 = "Rs.";
        //    PaymentDetailsDto.udf2 = enq.PASP_Id.ToString();
        //    PaymentDetailsDto.udf3 = enq.MI_ID.ToString();
        //    PaymentDetailsDto.udf4 = enq.ASMAY_Id.ToString();
        //    PaymentDetailsDto.udf5 = enq.ASMCL_Id.ToString();
        //    PaymentDetailsDto.udf6 = Convert.ToString(186);
        //    PaymentDetailsDto.transaction_response_url = "http://localhost:57606/api/" + PaymentDetailsDto.productinfo + "/paymentresponse/";
        //    PaymentDetailsDto.status = "success";
        //    PaymentDetailsDto.service_provider = "payu_paisa";

        //    PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);



        //    return PaymentDetailsDto.PaymentDetailsList;

        //}


        public Array paymentPart(ProspectusDTO enq)
        {
            Payment pay = new Payment(_context);
            ProspectusDTO data = new ProspectusDTO();
            List<Prospepaymentamount> paymentdetails = new List<Prospepaymentamount>();
            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            int autoinc = 1, totpayableamount = 0;

            List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
            //enq.ASMAY_Id = 7;
            try
            {
                paymentdetails = _ProspectusContext.Prospepaymentamount.Where(t => t.IVRMOP_MIID == enq.MI_ID).ToList();
                // ProspectusDTO ProspectusDTO = new ProspectusDTO();
                var FeeAmountresult = (from a in _feecontext.feeYCC

                                       from c in _feecontext.feeYCCC
                                       from d in _feecontext.FeeAmountEntryDMO

                                       from g in _feecontext.FeeHeadDMO
                                       where (d.FMH_Id == g.FMH_Id && d.FMCC_Id == a.FMCC_Id && a.ASMAY_Id == d.ASMAY_Id && a.FYCC_Id == c.FYCC_Id && d.ASMAY_Id == enq.ASMAY_Id && d.MI_Id == enq.MI_ID && g.FMH_Flag == "R" && c.ASMCL_Id == enq.ASMCL_Id)
                                       select new FeeAmountEntryDMO
                                       {
                                           FMA_Id = d.FMA_Id,
                                           FMA_Amount = d.FMA_Amount
                                       }
            ).FirstOrDefault();

                try
                {
                    // string ids = enq.ftiidss;

                    using (var cmd1 = _feecontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd1.CommandText = "Preadmission_Split_Payment_Registration";
                        cmd1.CommandType = CommandType.StoredProcedure;

                        cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                         SqlDbType.BigInt)
                        {
                            Value = enq.MI_ID
                        });

                        cmd1.Parameters.Add(new SqlParameter("@Asmay_Id",
                        SqlDbType.BigInt)
                        {
                            Value = enq.ASMAY_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@Amst_Id",
                        SqlDbType.VarChar)
                        {
                            Value = 7
                        });

                        //cmd1.Parameters.Add(new SqlParameter("@fmt_id",
                        // SqlDbType.VarChar)
                        //{
                        //    Value = enq.multiplegroups
                        //});

                        if (cmd1.Connection.State != ConnectionState.Open)
                            cmd1.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd1.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new FeeSlplitOnlinePayment
                                    {
                                        name = "splitId" + autoinc.ToString(),
                                        merchantId = dataReader["FPGD_MerchantId"].ToString(),
                                        value = dataReader["balance"].ToString(),
                                        commission = "0",
                                        description = "Online Payment",
                                    });

                                    autoinc = autoinc + 1;
                                }
                            }
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


                if (enq.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                    enq.transnumbconfigurationsettingsss.MI_Id = enq.MI_ID;
                    enq.transnumbconfigurationsettingsss.ASMAY_Id = enq.ASMAY_Id;
                    PaymentDetailsDto.trans_id = a.GenerateNumber(enq.transnumbconfigurationsettingsss);
                }

                if (FeeAmountresult != null)
                {


                    //string fmaid = Convert.ToString(FeeAmountresult.FMA_Id);

                    //PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().IVRMOP_MERCHANT_KEY;
                    //// PaymentDetailsDto.amount = paymentdetails.FirstOrDefault().IVRMOP_PROS_AMOUNT;
                    //PaymentDetailsDto.amount = FeeAmountresult.FMA_Amount;
                    ////PaymentDetailsDto.trans_id = "200";
                    //PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";
                    //PaymentDetailsDto.productinfo = "StudentApplication";
                    //PaymentDetailsDto.firstname = enq.PASR_FirstName;
                    //PaymentDetailsDto.email = enq.PASR_emailId;
                    //PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().IVRMOP_SALT;
                    //PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().IVRMOP_BASE_URL;
                    //PaymentDetailsDto.phone = enq.PASR_MobileNo;
                    //PaymentDetailsDto.udf1 = "Rs.";
                    //PaymentDetailsDto.udf2 = enq.pasR_Id.ToString();
                    //PaymentDetailsDto.udf3 = enq.MI_Id.ToString();
                    //PaymentDetailsDto.udf4 = enq.ASMAY_Id.ToString();
                    //PaymentDetailsDto.udf5 = enq.ASMCL_Id.ToString();
                    //PaymentDetailsDto.udf6 = fmaid;
                    //PaymentDetailsDto.transaction_response_url = "http://localhost:57606/api/" + PaymentDetailsDto.productinfo + "/paymentresponse/";
                    //PaymentDetailsDto.status = "success";
                    //PaymentDetailsDto.service_provider = "payu_paisa";

                    //PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);







                    PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

                    foreach (FeeSlplitOnlinePayment x in result)
                    {
                        totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                    }

                    var item = new
                    {
                        paymentParts = result
                    };

                    string payinfo = JsonConvert.SerializeObject(item);

                    PaymentDetailsDto.productinfo = payinfo;
                    PaymentDetailsDto.amount = Convert.ToDecimal(totpayableamount);
                    PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().IVRMOP_MERCHANT_KEY;
                    PaymentDetailsDto.firstname = enq.PASP_First_Name;


                    PaymentDetailsDto.email = enq.PASP_EmailId;

                    PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().IVRMOP_SALT;
                    PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().IVRMOP_BASE_URL;
                    PaymentDetailsDto.phone = enq.PASP_MobileNo;
                    PaymentDetailsDto.udf1 = Convert.ToString(enq.ASMAY_Id);
                    PaymentDetailsDto.udf2 = Convert.ToString(enq.PASP_Id);
                    PaymentDetailsDto.udf3 = enq.MI_ID.ToString();
                    PaymentDetailsDto.udf4 = enq.ASMCL_Id.ToString();
                    PaymentDetailsDto.udf5 = enq.ASMAY_Id.ToString();
                    PaymentDetailsDto.udf6 = enq.ASMCL_Id.ToString();
                    // PaymentDetailsDto.transaction_response_url = "";
                    PaymentDetailsDto.transaction_response_url = "http://localhost:57606/api/Prospectus/paymentresponse/";
                    PaymentDetailsDto.status = "success";
                    PaymentDetailsDto.service_provider = "payu_paisa";

                    PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);



                    FeePaymentDetailsDMO feepaydet = new FeePaymentDetailsDMO();
                    feepaydet.MI_Id = enq.MI_ID;
                    feepaydet.ASMAY_ID = enq.ASMAY_Id;

                    feepaydet.FTCU_Id = 1;
                    feepaydet.FYP_Receipt_No = PaymentDetailsDto.trans_id;
                    feepaydet.FYP_Bank_Name = "";
                    feepaydet.FYP_Bank_Or_Cash = "O";
                    feepaydet.FYP_DD_Cheque_No = "";
                    feepaydet.FYP_DD_Cheque_Date = DateTime.Now;
                    feepaydet.FYP_Date = DateTime.Now;
                    feepaydet.FYP_Tot_Amount = PaymentDetailsDto.amount;
                    feepaydet.FYP_Tot_Waived_Amt = 0;
                    feepaydet.FYP_Tot_Fine_Amt = 0;
                    feepaydet.FYP_Tot_Concession_Amt = 0;
                    feepaydet.FYP_Remarks = "Preadmission Prospectus Payment";
                    feepaydet.FYP_Chq_Bounce = "CL";
                    feepaydet.DOE = DateTime.Now;
                    feepaydet.CreatedDate = DateTime.Now;
                    feepaydet.UpdatedDate = DateTime.Now;
                    feepaydet.user_id = enq.id;
                    feepaydet.fyp_transaction_id = PaymentDetailsDto.trans_id;
                    feepaydet.FYP_OnlineChallanStatusFlag = "Payment Initiated";
                    feepaydet.FYP_PaymentReference_Id = "";

                    _feecontext.FeePaymentDetailsDMO.Add(feepaydet);
                    _feecontext.SaveChanges();

                    PaymentDetailsDto.paymentdetails = "True";

                }
                else
                {
                    PaymentDetailsDto.paymentdetails = "false";
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return PaymentDetailsDto.PaymentDetailsList;

        }
    }
}
