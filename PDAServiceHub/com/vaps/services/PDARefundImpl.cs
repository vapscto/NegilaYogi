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
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.PDA;
using DataAccessMsSqlServerProvider.com.vapstech.PDA;
using DomainModel.Model.com.vapstech.PDA;
using DomainModel.Model.com.vaps.admission;

namespace PDAServiceHub.com.vaps.services
{
    public class PDARefundImpl : interfaces.PDARefundInterface
    {


        private static ConcurrentDictionary<string, PdaDTO> _login =
      new ConcurrentDictionary<string, PdaDTO>();

        public PDAContext _PdaheadContext;
        readonly ILogger<PDARefundImpl> _logger;
        public PDARefundImpl(PDAContext frgContext, ILogger<PDARefundImpl> log)
        {
            _logger = log;
            _PdaheadContext = frgContext;

        }

        public PDATransactionDTO getdetails(PDATransactionDTO data)
        {
            try
            {

                //var fetchmaxfypid = _PdaheadContext.PDA_RefundDMO.Where(t => t.MI_Id == data.mi_id).OrderByDescending(t => t.PDAE_Id).Take(5).Select(t => t.PDAE_Id).ToList();

                List<MasterAcademic> year = new List<MasterAcademic>();
                //year = _PdaheadContext.AcademicYear.Where(t => t.MI_Id == data.MI_ID && t.ASMAY_Id == data.ASMAY_Id).ToList();
                //data.fillyear = year.ToArray();

                year = _PdaheadContext.AcademicYear.Where(t => t.MI_Id == data.MI_ID && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.fillyear = year.Distinct().ToArray();

                List<PDA_Master_HeadDMO> pdahead = new List<PDA_Master_HeadDMO>();
                pdahead = _PdaheadContext.pdahead.Where(t => t.MI_Id == data.MI_ID).OrderBy(t => t.PDAMH_Id).ToList();
                data.pdadata = pdahead.ToArray();

                //data.receiparraydelete = (from b in _PdaheadContext.PDA_RefundDMO
                //                          from c in _PdaheadContext.AdmissionStudentDMO
                //                          from d in _PdaheadContext.School_Adm_Y_StudentDMO
                //                          from e in _PdaheadContext.School_M_Class
                //                          from f in _PdaheadContext.school_M_Section
                //                          where (b.AMST_Id==d.AMST_Id && f.ASMS_Id == d.ASMS_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && c.MI_Id == data.MI_ID && d.ASMAY_Id == data.ASMAY_Id)
                //                          select new PDATransactionDTO
                //                          {
                //                              Amst_Id = c.AMST_Id,
                //                              AMST_FirstName = c.AMST_FirstName,
                //                              AMST_MiddleName = c.AMST_MiddleName,
                //                              AMST_LastName = c.AMST_LastName,
                //                              classname = e.ASMCL_ClassName,
                //                              sectionname = f.ASMC_SectionName,
                //                              AMST_AdmNo = c.AMST_AdmNo,
                //                              transactionno=b.PDAR_RefundNo,
                //                              PDAE_TotAmount = b.PDAR_RefundAmount,
                //                              PDAE_Date=b.PDAR_Date,
                //                              PDAR_Id=b.PDAR_Id
                //                          }
                //        ).Distinct().OrderByDescending(t => t.PDAE_ID).ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return data;

        }

        public FeeStudentTransactionDTO getsearchfilter(FeeStudentTransactionDTO data)
        {
            try
            {

                if (data.filterinitialdata.Equals("regular"))
                {

                    data.fillstudent = (from a in _PdaheadContext.AdmissionStudentDMO
                                        from b in _PdaheadContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1 && ((a.AMST_FirstName.Trim() + ' ' + a.AMST_MiddleName.Trim() + ' ' + a.AMST_LastName.Trim()).StartsWith(data.searchfilter) || (a.AMST_FirstName.Trim() + a.AMST_MiddleName.Trim() + ' ' + a.AMST_LastName.Trim()).StartsWith(data.searchfilter) || a.AMST_FirstName.StartsWith(data.searchfilter) || a.AMST_MiddleName.StartsWith(data.searchfilter) || a.AMST_LastName.StartsWith(data.searchfilter)))         /*a.AMST_SOL == "S"*/
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName,
                                        }
             ).ToArray();


                }

                else if (data.filterinitialdata.Equals("InActive"))
                {
                    data.fillstudent = (from a in _PdaheadContext.AdmissionStudentDMO
                                        from b in _PdaheadContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "D" && a.AMST_FirstName.StartsWith(data.searchfilter))
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName,
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("left"))
                {
                    data.fillstudent = (from a in _PdaheadContext.AdmissionStudentDMO
                                        from b in _PdaheadContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "L" && a.AMST_AdmNo.StartsWith(data.searchfilter))
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName,
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("regno"))
                {
                    data.fillstudent = (from a in _PdaheadContext.AdmissionStudentDMO
                                        from b in _PdaheadContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && ((b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || (b.AMAY_ActiveFlag == 0 && a.AMST_ActiveFlag == 0)) && a.AMST_RegistrationNo.StartsWith(data.searchfilter))              /*  a.AMST_SOL == "S"*/
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_RegistrationNo,
                                            AMST_MiddleName = "",
                                            AMST_LastName = ""

                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("AdmNo"))
                {
                    data.fillstudent = (from a in _PdaheadContext.AdmissionStudentDMO
                                        from b in _PdaheadContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && ((b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || (b.AMAY_ActiveFlag == 0 && a.AMST_ActiveFlag == 0)) && a.AMST_AdmNo.StartsWith(data.searchfilter))            /*a.AMST_SOL == "S"*/
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_AdmNo,
                                            AMST_MiddleName = "",
                                            AMST_LastName = ""

                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("Admnoname"))
                {
                    data.fillstudent = (from a in _PdaheadContext.AdmissionStudentDMO
                                        from b in _PdaheadContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && ((b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || (b.AMAY_ActiveFlag == 0 && a.AMST_ActiveFlag == 0)) && a.AMST_AdmNo.StartsWith(data.searchfilter))         /* a.AMST_SOL == "S"*/
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_AdmNo + "-" + a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("NameAdmno"))
                {
                    data.fillstudent = (from a in _PdaheadContext.AdmissionStudentDMO
                                        from b in _PdaheadContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && ((b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || (b.AMAY_ActiveFlag == 0 && a.AMST_ActiveFlag == 0)) && a.AMST_FirstName.StartsWith(data.searchfilter))     /*a.AMST_SOL == "S"*/
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName + "-" + a.AMST_AdmNo,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName + "-" + a.AMST_AdmNo,
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("NameRegNo"))
                {
                    data.fillstudent = (from a in _PdaheadContext.AdmissionStudentDMO
                                        from b in _PdaheadContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && ((b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || (b.AMAY_ActiveFlag == 0 && a.AMST_ActiveFlag == 0)) && a.AMST_FirstName.StartsWith(data.searchfilter))          /*a.AMST_SOL == "S"*/
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("RegNoName"))
                {
                    data.fillstudent = (from a in _PdaheadContext.AdmissionStudentDMO
                                        from b in _PdaheadContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == data.AMST_SOL && ((b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1) || (b.AMAY_ActiveFlag == 0 && a.AMST_ActiveFlag == 0)) && a.AMST_RegistrationNo.StartsWith(data.searchfilter))     /*a.AMST_SOL == "S"*/
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_RegistrationNo + "-" + a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName,
                                        }
             ).ToArray();
                }
                else if (data.filterinitialdata.Equals("Preadmission"))
                {
                    data.fillstudent = (from a in _PdaheadContext.stuapp
                                        where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.pasr_id,
                                            AMST_FirstName = a.PASR_FirstName + ' ' + a.PASR_MiddleName + ' ' + a.PASR_LastName,
                                            AMST_MiddleName = a.PASR_MiddleName,
                                            AMST_LastName = a.PASR_LastName,
                                        }
             ).ToArray();
                }

            }
            catch (Exception e)
            {
                data.validationvalue = "Contact Administrator";
            }
            return data;
        }



        public PDATransactionDTO getstuddetails(PDATransactionDTO data)
        {
            try
            {
              
                //data.studentdata = (from a in _PdaheadContext.feehead
                //                          from b in _PdaheadContext.FeeStudentTransactionDMO
                //                          from c in _PdaheadContext.AdmissionStudentDMO
                //                          from d in _PdaheadContext.School_Adm_Y_StudentDMO
                //                          from e in _PdaheadContext.School_M_Class
                //                          from f in _PdaheadContext.school_M_Section
                //                          where (b.AMST_Id==d.AMST_Id && f.ASMS_Id == d.ASMS_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.FMH_Id==b.FMH_Id && c.MI_Id == data.MI_ID && d.ASMAY_Id == data.ASMAY_Id && a.FMH_RefundFlag==true && b.AMST_Id==data.Amst_Id)
                //                          select new PDATransactionDTO
                //                          {
                //                            FSS_CurrentYrCharges=b.FSS_CurrentYrCharges
                //                          }
                // ).Sum(t => t.FSS_CurrentYrCharges);

                data.studentdata = (from a in _PdaheadContext.PDA_StatusDMO
                                    where (a.MI_Id == data.MI_ID && a.ASMAY_Id == data.ASMAY_Id  && a.AMST_Id == data.Amst_Id)
                                    select new PDATransactionDTO
                                    {
                                        FSS_CurrentYrCharges = Convert.ToInt64(a.PDAS_CBExcessPaid)
                                    }
              ).Sum(t => t.FSS_CurrentYrCharges);



            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return data;

        }


        public PDATransactionDTO Savedata(PDATransactionDTO data)
        {
            bool returnresult = false;

            if (data.refundamt > 0)
            {
                try
                {
                    PDA_RefundDMO objpge1 = new PDA_RefundDMO();


                    objpge1.MI_Id = data.MI_ID;
                    objpge1.AMST_Id = data.Amst_Id;
                    objpge1.ASMAY_Id = data.ASMAY_Id;
                    objpge1.PDAMH_Id = data.PDAMH_Id;
                    objpge1.PDAR_Date = DateTime.Now;
                    objpge1.PDAR_RefundNo = data.transactionno;
                    objpge1.PDAR_RefundAmount = data.refundamt;
                    objpge1.PDAR_ModeOfPayment = data.PDAR_ModeOfPayment;
                    objpge1.PDAR_ChequeDDNo = data.PDAR_ChequeDDNo;
                    objpge1.PDAR_RefundRemarks = data.PDAR_RefundRemarks;
                    objpge1.PDAR_ChequeDDDate = data.PDAR_ChequeDDDate;
                    objpge1.PDAR_BankName = data.PDAR_BankName;
                    objpge1.PDAR_OPReferenceNo = "";
                    objpge1.PDAR_ActiveFlag = true;
                    objpge1.CreatedDate = DateTime.Now;
                    objpge1.UpdatedDate = DateTime.Now;
                    _PdaheadContext.Add(objpge1);
                    var contactExists = _PdaheadContext.SaveChanges();
                    PDA_StatusDMO feepge1 = Mapper.Map<PDA_StatusDMO>(data);

                    if (contactExists >= 1)
                    {
                        returnresult = true;
                        data.returnval = returnresult;
                    }
                    else
                    {
                        returnresult = false;
                        data.returnval = returnresult;
                    }


                    var studid = _PdaheadContext.PDA_StatusDMO.Where(t => t.AMST_Id == data.Amst_Id && t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_ID).ToList();


                    try
                    {
                        if (studid.Count > 0)
                        {
                            var result1 = _PdaheadContext.PDA_StatusDMO.Where(t => t.AMST_Id == data.Amst_Id && t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_ID).ToList();

                            var result = _PdaheadContext.PDA_StatusDMO.Single(t => t.PDAS_Id == result1.FirstOrDefault().PDAS_Id);

                            result.PDAS_CYRefundAmt = data.refundamt;
                            result.UpdatedDate = DateTime.Now;
                            _PdaheadContext.Update(result);
                            var Exists = _PdaheadContext.SaveChanges();


                            if (Exists >= 1)
                            {
                                returnresult = true;
                                data.returnval = returnresult;
                            }
                            else
                            {
                                returnresult = false;
                                data.returnval = returnresult;
                            }

                        }
                        else
                        {
                            feepge1.MI_Id = data.MI_ID;
                            feepge1.ASMAY_Id = data.ASMAY_Id;
                            feepge1.AMST_Id = data.Amst_Id;
                            feepge1.PDAS_OBStudentDue = 0;
                            feepge1.PDAS_OBExcessPaid = 0;
                            feepge1.PDAS_CYDeposit = 0;
                            feepge1.PDAS_CYExpenses = 0;
                            feepge1.PDAS_CYRefundAmt =data.refundamt;
                            feepge1.PDAS_CBStudentDue = 0;
                            feepge1.PDAS_CBExcessPaid = 0;
                            _PdaheadContext.Add(feepge1);
                            var Exists1 = _PdaheadContext.SaveChanges();

                            if (Exists1 >= 1)
                            {
                                returnresult = true;
                                data.returnval = returnresult;
                            }
                            else
                            {
                                returnresult = false;
                                data.returnval = returnresult;
                            }
                        }



                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                data.status = "Record Not Saved";
            }
            return data;

        }


        public PDATransactionDTO searching(PDATransactionDTO data)
        {
            try
            {

                switch (data.searchType)
                {

                    case "0":
                        string str = "";
                        data.searcharray = (
                                            from b in _PdaheadContext.PDA_RefundDMO
                                            from c in _PdaheadContext.AdmissionStudentDMO
                                            from d in _PdaheadContext.School_Adm_Y_StudentDMO
                                            from e in _PdaheadContext.School_M_Class
                                            from f in _PdaheadContext.school_M_Section
                                            where (b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && b.MI_Id == data.MI_ID && d.ASMS_Id == f.ASMS_Id && b.ASMAY_Id == data.ASMAY_Id && (((c.AMST_FirstName.Trim() + ' ' + (string.IsNullOrEmpty(c.AMST_MiddleName.Trim()) == true ? str : c.AMST_MiddleName.Trim())).Trim() + ' ' + (string.IsNullOrEmpty(c.AMST_LastName.Trim()) == true ? str : c.AMST_LastName.Trim())).Trim().Contains(data.searchtext) || c.AMST_FirstName.StartsWith(data.searchtext) || c.AMST_MiddleName.StartsWith(data.searchtext) || c.AMST_LastName.StartsWith(data.searchtext)) && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                                            select new PDATransactionDTO
                                            {
                                                Amst_Id = c.AMST_Id,
                                                AMST_FirstName = c.AMST_FirstName,
                                                AMST_MiddleName = c.AMST_MiddleName,
                                                AMST_LastName = c.AMST_LastName,
                                                transactionno = b.PDAR_RefundNo,
                                                PDAE_TotAmount = b.PDAR_RefundAmount,
                                                classname = e.ASMCL_ClassName,
                                                sectionname = f.ASMC_SectionName,
                                                PDAE_ID = b.PDAR_Id,
                                                AMST_AdmNo = c.AMST_AdmNo,
                                                PDAE_Date = b.PDAR_Date
                                            }
              ).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
                        break;
                    case "1":
                        data.searcharray = (
                                            from b in _PdaheadContext.PDA_RefundDMO
                                            from c in _PdaheadContext.AdmissionStudentDMO
                                            from d in _PdaheadContext.School_Adm_Y_StudentDMO
                                            from e in _PdaheadContext.School_M_Class
                                            from f in _PdaheadContext.school_M_Section
                                            where (b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && b.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id && e.ASMCL_ClassName.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                                            select new PDATransactionDTO
                                            {
                                                Amst_Id = c.AMST_Id,
                                                AMST_FirstName = c.AMST_FirstName,
                                                AMST_MiddleName = c.AMST_MiddleName,
                                                AMST_LastName = c.AMST_LastName,
                                                transactionno = b.PDAR_RefundNo,
                                                PDAE_TotAmount = b.PDAR_RefundAmount,
                                                classname = e.ASMCL_ClassName,
                                                sectionname = f.ASMC_SectionName,
                                                PDAE_ID = b.PDAR_Id,
                                                AMST_AdmNo = c.AMST_AdmNo,
                                                PDAE_Date = b.PDAR_Date
                                            }
              ).Distinct().OrderBy(t => t.classname).ToArray();
                        break;
                    case "2":
                        data.searcharray = (
                                            from b in _PdaheadContext.PDA_RefundDMO
                                            from c in _PdaheadContext.AdmissionStudentDMO
                                            from d in _PdaheadContext.School_Adm_Y_StudentDMO
                                            from e in _PdaheadContext.School_M_Class
                                            from f in _PdaheadContext.school_M_Section
                                            where (b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && b.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id && c.AMST_AdmNo.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                                            select new PDATransactionDTO
                                            {
                                                Amst_Id = c.AMST_Id,
                                                AMST_FirstName = c.AMST_FirstName,
                                                AMST_MiddleName = c.AMST_MiddleName,
                                                AMST_LastName = c.AMST_LastName,
                                                transactionno = b.PDAR_RefundNo,
                                                PDAE_TotAmount = b.PDAR_RefundAmount,
                                                classname = e.ASMCL_ClassName,
                                                sectionname = f.ASMC_SectionName,
                                                PDAE_ID = b.PDAR_Id,
                                                AMST_AdmNo = c.AMST_AdmNo,
                                                PDAE_Date = b.PDAR_Date
                                            }
              ).Distinct().OrderBy(t => t.AMST_AdmNo).ToArray();
                        break;
                    case "3":
                        data.searcharray = (
                                            from b in _PdaheadContext.PDA_RefundDMO
                                            from c in _PdaheadContext.AdmissionStudentDMO
                                            from d in _PdaheadContext.School_Adm_Y_StudentDMO
                                            from e in _PdaheadContext.School_M_Class
                                            from f in _PdaheadContext.school_M_Section
                                            where (b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && b.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id && b.PDAR_RefundNo.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                                            select new PDATransactionDTO
                                            {
                                                Amst_Id = c.AMST_Id,
                                                AMST_FirstName = c.AMST_FirstName,
                                                AMST_MiddleName = c.AMST_MiddleName,
                                                AMST_LastName = c.AMST_LastName,
                                                transactionno = b.PDAR_RefundNo,
                                                PDAE_TotAmount = b.PDAR_RefundAmount,
                                                classname = e.ASMCL_ClassName,
                                                sectionname = f.ASMC_SectionName,
                                                PDAE_ID = b.PDAR_Id,
                                                AMST_AdmNo = c.AMST_AdmNo,
                                                PDAE_Date = b.PDAR_Date
                                            }
              ).Distinct().OrderBy(t => t.transactionno).ToArray();
                        break;
                    case "4":
                        var date_format = data.searchdate.ToString("dd/MM/yyyy");


                        data.searcharray = (
                                            from b in _PdaheadContext.PDA_RefundDMO
                                            from c in _PdaheadContext.AdmissionStudentDMO
                                            from d in _PdaheadContext.School_Adm_Y_StudentDMO
                                            from e in _PdaheadContext.School_M_Class
                                            from f in _PdaheadContext.school_M_Section
                                                // from g in list
                                            where (b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && b.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && b.PDAR_Date.ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy"))
                                            select new PDATransactionDTO
                                            {
                                                Amst_Id = c.AMST_Id,
                                                AMST_FirstName = c.AMST_FirstName,
                                                AMST_MiddleName = c.AMST_MiddleName,
                                                AMST_LastName = c.AMST_LastName,
                                                transactionno = b.PDAR_RefundNo,
                                                PDAE_TotAmount = b.PDAR_RefundAmount,
                                                classname = e.ASMCL_ClassName,
                                                sectionname = f.ASMC_SectionName,
                                                PDAE_ID = b.PDAR_Id,
                                                AMST_AdmNo = c.AMST_AdmNo,
                                                PDAE_Date = b.PDAR_Date
                                            }
           ).Distinct().ToArray();

                        break;
                    case "5":
                        data.searcharray = (
                                            from b in _PdaheadContext.PDA_RefundDMO
                                            from c in _PdaheadContext.AdmissionStudentDMO
                                            from d in _PdaheadContext.School_Adm_Y_StudentDMO
                                            from e in _PdaheadContext.School_M_Class
                                            from f in _PdaheadContext.school_M_Section
                                            where (b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && b.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id && b.PDAR_RefundAmount.ToString().Contains(data.searchnumber) && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                                            select new PDATransactionDTO
                                            {
                                                Amst_Id = c.AMST_Id,
                                                AMST_FirstName = c.AMST_FirstName,
                                                AMST_MiddleName = c.AMST_MiddleName,
                                                AMST_LastName = c.AMST_LastName,
                                                transactionno = b.PDAR_RefundNo,
                                                PDAE_TotAmount = b.PDAR_RefundAmount,
                                                classname = e.ASMCL_ClassName,
                                                sectionname = f.ASMC_SectionName,
                                                PDAE_ID = b.PDAR_Id,
                                                AMST_AdmNo = c.AMST_AdmNo,
                                                PDAE_Date = b.PDAR_Date
                                            }
              ).Distinct().OrderBy(t => t.PDAE_TotAmount).ToArray();


                        break;
                    case "6":
                        data.searcharray = (
                                            from b in _PdaheadContext.PDA_RefundDMO
                                            from c in _PdaheadContext.AdmissionStudentDMO
                                            from d in _PdaheadContext.School_Adm_Y_StudentDMO
                                            from e in _PdaheadContext.School_M_Class
                                            from f in _PdaheadContext.school_M_Section
                                            where (b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && b.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id && f.ASMC_SectionName.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                                            select new PDATransactionDTO
                                            {
                                                Amst_Id = c.AMST_Id,
                                                AMST_FirstName = c.AMST_FirstName,
                                                AMST_MiddleName = c.AMST_MiddleName,
                                                AMST_LastName = c.AMST_LastName,
                                                transactionno = b.PDAR_RefundNo,
                                                PDAE_TotAmount = b.PDAR_RefundAmount,
                                                classname = e.ASMCL_ClassName,
                                                sectionname = f.ASMC_SectionName,
                                                PDAE_ID = b.PDAR_Id,
                                                AMST_AdmNo = c.AMST_AdmNo,
                                                PDAE_Date = b.PDAR_Date
                                            }
              ).Distinct().OrderBy(t => t.sectionname).ToArray();
                        break;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }



        public PDATransactionDTO Deletedetails(PDATransactionDTO data)
        {

            
            try
            {
                var lorg1 = _PdaheadContext.PDA_RefundDMO.Where(t => t.PDAR_Id == data.PDAR_Id).ToList();
                var result1 = _PdaheadContext.PDA_StatusDMO.Where(t => t.AMST_Id == data.Amst_Id && t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_ID).Select(t=>t.PDAS_CYRefundAmt).Sum();

                long  amt = Convert.ToInt64(result1) - Convert.ToInt64(data.PDAE_TotAmount);

                var result = _PdaheadContext.PDA_StatusDMO.Single(t => t.AMST_Id == data.Amst_Id && t.ASMAY_Id==data.ASMAY_Id && t.MI_Id==data.MI_ID);

                result.PDAS_CYRefundAmt = Math.Abs(amt);
                result.UpdatedDate = DateTime.Now;
                _PdaheadContext.Update(result);
                var Exists = _PdaheadContext.SaveChanges();
                
                if (lorg1.Any())
                {
                    for (int i = 0; lorg1.Count > i; i++)
                    {
                        _PdaheadContext.Remove(lorg1.ElementAt(i));
                    }
                }
                
            //    _PdaheadContext.Remove(lorg1);


                var contactexisttransaction = 0;
                using (var dbCtxTxn = _PdaheadContext.Database.BeginTransaction())
                {
                    try
                    {
                        contactexisttransaction = _PdaheadContext.SaveChanges();
                        dbCtxTxn.Commit();
                        data.returnval = true;
                    }
                    catch (Exception ex)
                    {
                        dbCtxTxn.Rollback();
                        data.returnval = false;
                    }
                }
                
            }
            catch (Exception ee)
            {
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public PDATransactionDTO getdatastuacad(PDATransactionDTO data)
        {
            try
            {
                data.receiparraydelete = (from b in _PdaheadContext.PDA_RefundDMO
                                          from c in _PdaheadContext.AdmissionStudentDMO
                                          from d in _PdaheadContext.School_Adm_Y_StudentDMO
                                          from e in _PdaheadContext.School_M_Class
                                          from f in _PdaheadContext.school_M_Section
                                          where (b.AMST_Id == d.AMST_Id && f.ASMS_Id == d.ASMS_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && c.MI_Id == data.MI_ID && d.ASMAY_Id == data.ASMAY_Id)
                                          select new PDATransactionDTO
                                          {
                                              Amst_Id = c.AMST_Id,
                                              AMST_FirstName = c.AMST_FirstName,
                                              AMST_MiddleName = c.AMST_MiddleName,
                                              AMST_LastName = c.AMST_LastName,
                                              classname = e.ASMCL_ClassName,
                                              sectionname = f.ASMC_SectionName,
                                              AMST_AdmNo = c.AMST_AdmNo,
                                              transactionno = b.PDAR_RefundNo,
                                              PDAE_TotAmount = b.PDAR_RefundAmount,
                                              PDAE_Date = b.PDAR_Date,
                                              PDAR_Id = b.PDAR_Id
                                          }
                         ).Distinct().OrderByDescending(t => t.PDAE_ID).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

    }
    
}
