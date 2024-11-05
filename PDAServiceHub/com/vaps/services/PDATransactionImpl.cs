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
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using CommonLibrary;

namespace PDAServiceHub.com.vaps.services
{
    public class PDATransactionImpl : interfaces.PDATransactionInterface
    {

        private static ConcurrentDictionary<string, PDATransactionDTO> _login =
     new ConcurrentDictionary<string, PDATransactionDTO>();

        public PDAContext _PdaheadContext;
        readonly ILogger<PDATransactionImpl> _logger;
        private readonly DomainModelMsSqlServerContext _db;


        public PDATransactionImpl(PDAContext frgContext, ILogger<PDATransactionImpl> log, DomainModelMsSqlServerContext db)
        {
            _logger = log;
            _PdaheadContext = frgContext;
            _db = db;

        }


        public PDATransactionDTO getdetails(PDATransactionDTO data)
        {
            try
            {



                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _PdaheadContext.AcademicYear.Where(t => t.MI_Id == data.MI_ID && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.fillyear = year.ToArray();

                List<Master_Numbering> masnum = new List<Master_Numbering>();
                masnum = _PdaheadContext.Master_Numbering.Where(t => t.MI_Id == data.MI_ID && t.IMN_Flag == "Transaction").ToList();
                data.transnumconfig = masnum.ToArray();

                var rolename = _PdaheadContext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == data.roleid).IVRMRT_Role;

                data.rolename = rolename;

                bool recsettingval = false;
                string maxval = "";

                List<School_M_Class> classes = new List<School_M_Class>();
                classes = _PdaheadContext.School_M_Class.Where(t => t.ASMCL_ActiveFlag == true && t.MI_Id == data.MI_ID).ToList();
                data.classlist = classes.ToArray();

                var fetchmaxfypid = _PdaheadContext.pdaexpenditure.Where(t => t.MI_Id == data.MI_ID).OrderByDescending(t => t.PDAE_Id).Take(10).Select(t => t.PDAE_Id).ToList();

                List<PDA_Master_HeadDMO> pdahead = new List<PDA_Master_HeadDMO>();
                pdahead = _PdaheadContext.pdahead.Where(t => t.MI_Id == data.MI_ID).OrderBy(t => t.PDAMH_Id).ToList();
                data.pdadata = pdahead.ToArray();


                data.receiparraydelete = (from a in _PdaheadContext.pdaexpenditure
                                          from c in _PdaheadContext.AdmissionStudentDMO
                                          from d in _PdaheadContext.School_Adm_Y_StudentDMO
                                          from e in _PdaheadContext.School_M_Class
                                          from f in _PdaheadContext.school_M_Section

                                          where (f.ASMS_Id == d.ASMS_Id && fetchmaxfypid.Contains(a.PDAE_Id) && a.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_ID && a.ASMAY_ID == data.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
                                          select new PDATransactionDTO
                                          {
                                              Amst_Id = c.AMST_Id,
                                              AMST_FirstName = c.AMST_FirstName,
                                              AMST_MiddleName = c.AMST_MiddleName,
                                              AMST_LastName = c.AMST_LastName,
                                              transactionno = a.PDAE_TransactionNo,
                                              PDAE_TotAmount = a.PDAE_TotAmount,
                                              classname = e.ASMCL_ClassName,
                                              sectionname = f.ASMC_SectionName,
                                              PDAE_ID = a.PDAE_Id,
                                              AMST_AdmNo = c.AMST_AdmNo,
                                              PDAE_Date = a.PDAE_Date,
                                              PDAR_ModeOfPayment = a.PDAE_ModeOfPayment
                                          }
 ).Distinct().OrderByDescending(t => t.PDAE_ID).ToArray();

            }
            catch (Exception ee)
            {
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
                    data.searchfilter = data.searchfilter.ToUpper();
                    data.fillstudent = (from a in _PdaheadContext.AdmissionStudentDMO
                                        from b in _PdaheadContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1 && ((a.AMST_FirstName.Trim() + ' ' + a.AMST_MiddleName.Trim() + ' ' + a.AMST_LastName.Trim()).StartsWith(data.searchfilter) || (a.AMST_FirstName.Trim() + a.AMST_MiddleName.Trim() + ' ' + a.AMST_LastName.Trim()).StartsWith(data.searchfilter) || a.AMST_FirstName.StartsWith(data.searchfilter) || a.AMST_MiddleName.StartsWith(data.searchfilter) || a.AMST_LastName.StartsWith(data.searchfilter)))
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
                    data.searchfilter = data.searchfilter.ToUpper();
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

                else if (data.filterinitialdata.Equals("regno"))
                {
                    data.fillstudent = (from a in _PdaheadContext.AdmissionStudentDMO
                                        from b in _PdaheadContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1 && a.AMST_RegistrationNo.StartsWith(data.searchfilter))
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
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1 && a.AMST_AdmNo.StartsWith(data.searchfilter))
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
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1 && a.AMST_AdmNo.StartsWith(data.searchfilter))
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
                    data.searchfilter = data.searchfilter.ToUpper();

                    data.fillstudent = (from a in _PdaheadContext.AdmissionStudentDMO
                                        from b in _PdaheadContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1 && a.AMST_FirstName.StartsWith(data.searchfilter))
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName + "-" + a.AMST_AdmNo,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName + "-" + a.AMST_AdmNo,
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("inactive"))
                {

                    data.fillstudent = (from a in _PdaheadContext.AdmissionStudentDMO
                                        from b in _PdaheadContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "D" && a.AMST_FirstName.ToUpper().StartsWith(data.searchfilter) && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : a.AMST_LastName)).Trim(),
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName,
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("left"))
                {
                    // data.searchfilter = data.searchfilter.ToUpper();
                    data.fillstudent = (from a in _PdaheadContext.AdmissionStudentDMO
                                        from b in _PdaheadContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "L" && a.AMST_FirstName.ToUpper().StartsWith(data.searchfilter) && a.AMST_ActiveFlag == 0)
                                        select new FeeStudentTransactionDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : a.AMST_LastName)).Trim(),
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName,
                                        }
             ).ToArray();
                }




                else if (data.filterinitialdata.Equals("NameRegNo"))
                {
                    data.searchfilter = data.searchfilter.ToUpper();
                    data.fillstudent = (from a in _PdaheadContext.AdmissionStudentDMO
                                        from b in _PdaheadContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1 && a.AMST_FirstName.StartsWith(data.searchfilter))
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
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1 && a.AMST_RegistrationNo.StartsWith(data.searchfilter))
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


        public PDATransactionDTO searching(PDATransactionDTO data)
        {
            try
            {

                switch (data.searchType)
                {

                    case "0":
                        string str = "";
                        data.searchtext = data.searchtext.ToUpper();
                        data.searcharray = (
                                            from b in _PdaheadContext.pdaexpenditure
                                            from c in _PdaheadContext.AdmissionStudentDMO
                                            from d in _PdaheadContext.School_Adm_Y_StudentDMO
                                            from e in _PdaheadContext.School_M_Class
                                            from f in _PdaheadContext.school_M_Section
                                            where (b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && b.MI_Id == data.MI_ID && d.ASMS_Id == f.ASMS_Id && b.ASMAY_ID == data.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id && (((c.AMST_FirstName.ToUpper().Trim() + ' ' + (string.IsNullOrEmpty(c.AMST_MiddleName.ToUpper().Trim()) == true ? str : c.AMST_MiddleName.ToUpper().Trim())).Trim() + ' ' + (string.IsNullOrEmpty(c.AMST_LastName.ToUpper().Trim()) == true ? str : c.AMST_LastName.ToUpper().Trim())).Trim().Contains(data.searchtext) || c.AMST_FirstName.ToUpper().ToUpper().StartsWith(data.searchtext) || c.AMST_MiddleName.ToUpper().ToUpper().StartsWith(data.searchtext) || c.AMST_LastName.ToUpper().StartsWith(data.searchtext)))
                                            select new PDATransactionDTO
                                            {
                                                Amst_Id = c.AMST_Id,
                                                AMST_FirstName = c.AMST_FirstName,
                                                AMST_MiddleName = c.AMST_MiddleName,
                                                AMST_LastName = c.AMST_LastName,
                                                transactionno = b.PDAE_TransactionNo,
                                                PDAE_TotAmount = b.PDAE_TotAmount,
                                                classname = e.ASMCL_ClassName,
                                                sectionname = f.ASMC_SectionName,
                                                PDAE_ID = b.PDAE_Id,
                                                AMST_AdmNo = c.AMST_AdmNo,
                                                PDAE_Date = b.PDAE_Date,
                                                PDAR_ModeOfPayment = b.PDAE_ModeOfPayment
                                            }
              ).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
                        break;
                    case "1":
                        data.searcharray = (
                                            from b in _PdaheadContext.pdaexpenditure
                                            from c in _PdaheadContext.AdmissionStudentDMO
                                            from d in _PdaheadContext.School_Adm_Y_StudentDMO
                                            from e in _PdaheadContext.School_M_Class
                                            from f in _PdaheadContext.school_M_Section
                                            where (b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && b.MI_Id == data.MI_ID && b.ASMAY_ID == data.ASMAY_Id && e.ASMCL_ClassName.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id)
                                            //&& c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1
                                            select new PDATransactionDTO
                                            {
                                                Amst_Id = c.AMST_Id,
                                                AMST_FirstName = c.AMST_FirstName,
                                                AMST_MiddleName = c.AMST_MiddleName,
                                                AMST_LastName = c.AMST_LastName,
                                                transactionno = b.PDAE_TransactionNo,
                                                PDAE_TotAmount = b.PDAE_TotAmount,
                                                classname = e.ASMCL_ClassName,
                                                sectionname = f.ASMC_SectionName,
                                                PDAE_ID = b.PDAE_Id,
                                                AMST_AdmNo = c.AMST_AdmNo,
                                                PDAE_Date = b.PDAE_Date,
                                                PDAR_ModeOfPayment = b.PDAE_ModeOfPayment
                                            }
              ).Distinct().OrderBy(t => t.classname).ToArray();
                        break;
                    case "2":
                        data.searcharray = (
                                            from b in _PdaheadContext.pdaexpenditure
                                            from c in _PdaheadContext.AdmissionStudentDMO
                                            from d in _PdaheadContext.School_Adm_Y_StudentDMO
                                            from e in _PdaheadContext.School_M_Class
                                            from f in _PdaheadContext.school_M_Section
                                            where (b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && b.MI_Id == data.MI_ID && b.ASMAY_ID == data.ASMAY_Id && c.AMST_AdmNo.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id)
                                            // && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1
                                            select new PDATransactionDTO
                                            {
                                                Amst_Id = c.AMST_Id,
                                                AMST_FirstName = c.AMST_FirstName,
                                                AMST_MiddleName = c.AMST_MiddleName,
                                                AMST_LastName = c.AMST_LastName,
                                                transactionno = b.PDAE_TransactionNo,
                                                PDAE_TotAmount = b.PDAE_TotAmount,
                                                classname = e.ASMCL_ClassName,
                                                sectionname = f.ASMC_SectionName,
                                                PDAE_ID = b.PDAE_Id,
                                                AMST_AdmNo = c.AMST_AdmNo,
                                                PDAE_Date = b.PDAE_Date,
                                                PDAR_ModeOfPayment = b.PDAE_ModeOfPayment
                                            }
              ).Distinct().OrderBy(t => t.AMST_AdmNo).ToArray();
                        break;
                    case "3":
                        data.searcharray = (
                                            from b in _PdaheadContext.pdaexpenditure
                                            from c in _PdaheadContext.AdmissionStudentDMO
                                            from d in _PdaheadContext.School_Adm_Y_StudentDMO
                                            from e in _PdaheadContext.School_M_Class
                                            from f in _PdaheadContext.school_M_Section
                                            where (b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && b.MI_Id == data.MI_ID && b.ASMAY_ID == data.ASMAY_Id && b.PDAE_TransactionNo.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id)
                                            // && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1
                                            select new PDATransactionDTO
                                            {
                                                Amst_Id = c.AMST_Id,
                                                AMST_FirstName = c.AMST_FirstName,
                                                AMST_MiddleName = c.AMST_MiddleName,
                                                AMST_LastName = c.AMST_LastName,
                                                transactionno = b.PDAE_TransactionNo,
                                                PDAE_TotAmount = b.PDAE_TotAmount,
                                                classname = e.ASMCL_ClassName,
                                                sectionname = f.ASMC_SectionName,
                                                PDAE_ID = b.PDAE_Id,
                                                AMST_AdmNo = c.AMST_AdmNo,
                                                PDAE_Date = b.PDAE_Date,
                                                PDAR_ModeOfPayment = b.PDAE_ModeOfPayment
                                            }
              ).Distinct().OrderBy(t => t.transactionno).ToArray();
                        break;
                    case "4":
                        var date_format = data.searchdate.ToString("dd/MM/yyyy");


                        data.searcharray = (
                                            from b in _PdaheadContext.pdaexpenditure
                                            from c in _PdaheadContext.AdmissionStudentDMO
                                            from d in _PdaheadContext.School_Adm_Y_StudentDMO
                                            from e in _PdaheadContext.School_M_Class
                                            from f in _PdaheadContext.school_M_Section
                                                // from g in list
                                            where (b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && b.MI_Id == data.MI_ID && b.ASMAY_ID == data.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id && b.PDAE_Date.ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy"))
                                            // && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1
                                            select new PDATransactionDTO
                                            {
                                                Amst_Id = c.AMST_Id,
                                                AMST_FirstName = c.AMST_FirstName,
                                                AMST_MiddleName = c.AMST_MiddleName,
                                                AMST_LastName = c.AMST_LastName,
                                                transactionno = b.PDAE_TransactionNo,
                                                PDAE_TotAmount = b.PDAE_TotAmount,
                                                classname = e.ASMCL_ClassName,
                                                sectionname = f.ASMC_SectionName,
                                                PDAE_ID = b.PDAE_Id,
                                                AMST_AdmNo = c.AMST_AdmNo,
                                                PDAE_Date = b.PDAE_Date,
                                                PDAR_ModeOfPayment = b.PDAE_ModeOfPayment
                                            }
           ).Distinct().ToArray();

                        break;
                    case "5":
                        data.searcharray = (
                                            from b in _PdaheadContext.pdaexpenditure
                                            from c in _PdaheadContext.AdmissionStudentDMO
                                            from d in _PdaheadContext.School_Adm_Y_StudentDMO
                                            from e in _PdaheadContext.School_M_Class
                                            from f in _PdaheadContext.school_M_Section
                                            where (b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && b.MI_Id == data.MI_ID && b.ASMAY_ID == data.ASMAY_Id && b.PDAE_TotAmount.ToString().Contains(data.searchnumber) && d.ASMAY_Id == data.ASMAY_Id)
                                            // && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1
                                            select new PDATransactionDTO
                                            {
                                                Amst_Id = c.AMST_Id,
                                                AMST_FirstName = c.AMST_FirstName,
                                                AMST_MiddleName = c.AMST_MiddleName,
                                                AMST_LastName = c.AMST_LastName,
                                                transactionno = b.PDAE_TransactionNo,
                                                PDAE_TotAmount = b.PDAE_TotAmount,
                                                classname = e.ASMCL_ClassName,
                                                sectionname = f.ASMC_SectionName,
                                                PDAE_ID = b.PDAE_Id,
                                                AMST_AdmNo = c.AMST_AdmNo,
                                                PDAE_Date = b.PDAE_Date,
                                                PDAR_ModeOfPayment = b.PDAE_ModeOfPayment
                                            }
              ).Distinct().OrderBy(t => t.PDAE_TotAmount).ToArray();


                        break;
                    case "6":
                        data.searcharray = (
                                            from b in _PdaheadContext.pdaexpenditure
                                            from c in _PdaheadContext.AdmissionStudentDMO
                                            from d in _PdaheadContext.School_Adm_Y_StudentDMO
                                            from e in _PdaheadContext.School_M_Class
                                            from f in _PdaheadContext.school_M_Section
                                            where (b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && b.MI_Id == data.MI_ID && b.ASMAY_ID == data.ASMAY_Id && f.ASMC_SectionName.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id)
                                            // && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1
                                            select new PDATransactionDTO
                                            {
                                                Amst_Id = c.AMST_Id,
                                                AMST_FirstName = c.AMST_FirstName,
                                                AMST_MiddleName = c.AMST_MiddleName,
                                                AMST_LastName = c.AMST_LastName,
                                                transactionno = b.PDAE_TransactionNo,
                                                PDAE_TotAmount = b.PDAE_TotAmount,
                                                classname = e.ASMCL_ClassName,
                                                sectionname = f.ASMC_SectionName,
                                                PDAE_ID = b.PDAE_Id,
                                                AMST_AdmNo = c.AMST_AdmNo,
                                                PDAE_Date = b.PDAE_Date,
                                                PDAR_ModeOfPayment = b.PDAE_ModeOfPayment
                                            }
              ).Distinct().OrderBy(t => t.sectionname).ToArray();
                        break;

                    case "7":
                        data.searcharray = (
                                            from b in _PdaheadContext.pdaexpenditure
                                            from c in _PdaheadContext.AdmissionStudentDMO
                                            from d in _PdaheadContext.School_Adm_Y_StudentDMO
                                            from e in _PdaheadContext.School_M_Class
                                            from f in _PdaheadContext.school_M_Section
                                            where (b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && b.MI_Id == data.MI_ID && b.ASMAY_ID == data.ASMAY_Id && b.PDAE_ModeOfPayment.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id)
                                            // && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1
                                            select new PDATransactionDTO
                                            {
                                                Amst_Id = c.AMST_Id,
                                                AMST_FirstName = c.AMST_FirstName,
                                                AMST_MiddleName = c.AMST_MiddleName,
                                                AMST_LastName = c.AMST_LastName,
                                                transactionno = b.PDAE_TransactionNo,
                                                PDAE_TotAmount = b.PDAE_TotAmount,
                                                classname = e.ASMCL_ClassName,
                                                sectionname = f.ASMC_SectionName,
                                                PDAE_ID = b.PDAE_Id,
                                                AMST_AdmNo = c.AMST_AdmNo,
                                                PDAE_Date = b.PDAE_Date,
                                                PDAR_ModeOfPayment = b.PDAE_ModeOfPayment
                                            }
              ).Distinct().OrderBy(t => t.PDAE_TotAmount).ToArray();


                        break;






                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public PDATransactionDTO Savedata(PDATransactionDTO data)
        {

            string recno = "";

            if (data.savetmpdata.Count() > 0)
            {
                bool typestatus;
                if (data.PDAR_ModeOfPayment == "Credit")
                {
                    typestatus = true;

                }

                else
                {
                    typestatus = false;
                }



                if (data.type != "multiplestd")

                {



                    try
                    {
                        if (data.transnumconfigsettings.IMN_AutoManualFlag == "Manual")
                        {
                            recno = data.transactionno;
                        }
                        else
                        {
                            //var recno1 = _PdaheadContext.pdaexpenditure.Where(t => t.MI_Id == data.MI_ID && t.ASMAY_ID == data.ASMAY_Id).OrderByDescending(s => s.PDAE_Id).Take(1).ToList();
                            //long recno2 = Convert.ToInt32(recno1[0].PDAE_TransactionNo);
                            //recno = (recno2 + 1).ToString();
                            GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                            data.transnumconfigsettings.MI_Id = data.MI_ID;
                            data.transnumconfigsettings.ASMAY_Id = data.ASMAY_Id;
                            recno = a.TransactionNumberingPDA(data.transnumconfigsettings);
                        }

                        PDA_ExpenditureDMO objpge1 = new PDA_ExpenditureDMO();
                        objpge1.AMST_Id = data.Amst_Id;
                        objpge1.MI_Id = data.MI_ID;
                        objpge1.PDAE_Date = data.PDAE_Date;
                        objpge1.PDAE_TransactionNo = recno;
                        objpge1.PDAE_TotAmount = data.amount;
                        objpge1.CreatedDate = DateTime.Now;
                        objpge1.UpdatedDate = DateTime.Now;
                        objpge1.ASMAY_ID = data.ASMAY_Id;
                        objpge1.PDAE_CreditFlg = typestatus;
                        objpge1.PDAE_ModeOfPayment = data.PDAR_ModeOfPayment;
                        _PdaheadContext.Add(objpge1);



                        foreach (var act1 in data.savetmpdata)
                        {
                            PDA_Expenditure_HeadsDMO objpge2 = new PDA_Expenditure_HeadsDMO();
                            objpge2.PDAE_Id = objpge1.PDAE_Id;
                            objpge2.PDAEH_Id = act1.PDAE_Id;
                            objpge2.PDAMH_Id = act1.PDAMH_Id;
                            objpge2.PDAEH_Amount = act1.PDAEH_Amount;
                            objpge2.PDAE_Remarks = act1.PDAE_Remarks;
                            objpge2.CreatedDate = DateTime.Now;
                            objpge2.UpdatedDate = DateTime.Now;
                            _PdaheadContext.Add(objpge2);
                        }
                        var contactExists = _PdaheadContext.SaveChanges();

                        if (contactExists > 0)
                        {

                            data.returnval = true;
                        }
                        else
                        {

                            data.returnval = false;
                        }


                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                }

                else
                {

                    try
                    {

                        foreach (var act1 in data.savetmpdata)
                        {
                            if (data.transnumconfigsettings.IMN_AutoManualFlag == "Manual")
                            {
                                recno = data.transactionno;
                            }
                            else
                            {
                                //var recno1 = _PdaheadContext.pdaexpenditure.Where(t => t.MI_Id == data.MI_ID && t.ASMAY_ID == data.ASMAY_Id).OrderByDescending(s => s.PDAE_Id).Take(1).ToList();
                                //long recno2 = Convert.ToInt32(recno1[0].PDAE_TransactionNo);
                                //recno = (recno2 + 1).ToString();
                                //GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                //data.transnumconfigsettings.MI_Id = data.MI_ID;
                                //data.transnumconfigsettings.ASMAY_Id = data.ASMAY_Id;
                                //recno = a.TransactionNumberingPDA(data.transnumconfigsettings);

                                try
                                {
                                    using (var cmd = _PdaheadContext.Database.GetDbConnection().CreateCommand())
                                    {

                                        cmd.CommandText = "pdareceiptnogeneration";

                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                                            SqlDbType.VarChar, 100)
                                        {
                                            Value = data.MI_ID
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@asmayid",
                                           SqlDbType.NVarChar, 100)
                                        {
                                            Value = data.ASMAY_Id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@receiptno",
                            SqlDbType.NVarChar, 500)
                                        {
                                            Direction = ParameterDirection.Output
                                        });

                                        if (cmd.Connection.State != ConnectionState.Open)
                                            cmd.Connection.Open();

                                        var data1 = cmd.ExecuteNonQuery();

                                        recno = cmd.Parameters["@receiptno"].Value.ToString();

                                    }

                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }



                            }
                            PDA_ExpenditureDMO objpge1 = new PDA_ExpenditureDMO();
                            objpge1.AMST_Id = act1.AMST_Id;
                            objpge1.MI_Id = data.MI_ID;
                            objpge1.PDAE_Date = data.PDAE_Date;
                            objpge1.PDAE_TransactionNo = recno;
                            objpge1.PDAE_TotAmount = act1.PDAEH_Amount;
                            objpge1.CreatedDate = DateTime.Now;
                            objpge1.UpdatedDate = DateTime.Now;
                            objpge1.ASMAY_ID = data.ASMAY_Id;
                            objpge1.PDAE_CreditFlg = typestatus;
                            objpge1.PDAE_ModeOfPayment = data.PDAR_ModeOfPayment;
                            _PdaheadContext.Add(objpge1);
                            PDA_Expenditure_HeadsDMO objpge2 = new PDA_Expenditure_HeadsDMO();
                            objpge2.PDAE_Id = objpge1.PDAE_Id;
                            objpge2.PDAEH_Id = act1.PDAE_Id;
                            objpge2.PDAMH_Id = act1.PDAMH_Id;
                            objpge2.PDAEH_Amount = act1.PDAEH_Amount;
                            objpge2.PDAE_Remarks = act1.PDAE_Remarks;
                            objpge2.CreatedDate = DateTime.Now;
                            objpge2.UpdatedDate = DateTime.Now;
                            _PdaheadContext.Add(objpge2);

                            var contactExists1 = _PdaheadContext.SaveChanges();

                            if (contactExists1 > 0)
                            {

                                data.returnval = true;
                            }
                            else
                            {

                                data.returnval = false;
                            }


                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    //}
                }

            }

            else
            {
                data.status = "Record Not Saved";
            }

            return data;
        }


        public PDATransactionDTO Deletedetails(PDATransactionDTO data)
        {
            try
            {
                //var lorg1 = _PdaheadContext.pdaexpenditure.Single(t => t.PDAE_Id == data.PDAE_ID);
                //var lorg2 = _PdaheadContext.pdaexpenditurehead.Where(t => t.PDAE_Id == data.PDAE_ID).ToList();

                //if (lorg2.Any())
                //{
                //    for (int i = 0; lorg2.Count > i; i++)
                //    {
                //        _PdaheadContext.Remove(lorg2.ElementAt(i));
                //    }
                //}

                //// _PdaheadContext.Remove(lorg2);
                //_PdaheadContext.Remove(lorg1);


                //var contactexisttransaction = 0;
                //using (var dbCtxTxn = _PdaheadContext.Database.BeginTransaction())
                //{
                //    try
                //    {
                //        contactexisttransaction = _PdaheadContext.SaveChanges();
                //        dbCtxTxn.Commit();
                //        data.returnval = true;
                //    }
                //    catch (Exception ex)
                //    {
                //        dbCtxTxn.Rollback();
                //        data.returnval = false;
                //    }
                //}

                _PdaheadContext.Database.ExecuteSqlCommand("PDA_Receipt_delete @p0", data.PDAE_ID);
                data.returnval = true;

            }
            catch (Exception ee)
            {
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public PDATransactionDTO getsection(PDATransactionDTO data)
        {

            try
            {

                data.sectionlist = (from a in _PdaheadContext.School_Adm_Y_StudentDMO
                                    from b in _PdaheadContext.school_M_Section
                                    where (a.ASMS_Id == b.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && b.MI_Id == data.MI_ID)
                                    select b
                          ).Distinct().OrderBy(t => t.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public PDATransactionDTO getstudent(PDATransactionDTO data)
        {

            try
            {
                //       data.admsudentslist = (from a in _PdaheadContext.AdmissionStudentDMO
                //                              from b in _PdaheadContext.School_Adm_Y_StudentDMO
                //                              where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && data.ASMCL_Ids.Contains(b.ASMS_Id) && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                //                              select new FeeArrearRegisterReportDTO
                //                              {
                //                                  Amst_Id = a.AMST_Id,
                //                                  Admno = a.AMST_AdmNo,
                //                                  AMST_FirstName = a.AMST_FirstName,
                //                                  AMST_MiddleName = a.AMST_MiddleName,
                //                                  AMST_LastName = a.AMST_LastName,
                //                              }
                //).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();

                data.admsudentslist = (from a in _PdaheadContext.AdmissionStudentDMO
                                       from b in _PdaheadContext.School_Adm_Y_StudentDMO
                                       from c in _PdaheadContext.School_M_Class
                                       from d in _PdaheadContext.school_M_Section
                                       where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && data.ASMCL_Ids.Contains(b.ASMS_Id)
                                       && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && b.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == b.ASMS_Id)
                                       select new FeeArrearRegisterReportDTO
                                       {
                                           Amst_Id = a.AMST_Id,
                                           Admno = a.AMST_AdmNo,
                                           AMST_FirstName = a.AMST_FirstName,
                                           AMST_MiddleName = a.AMST_MiddleName,
                                           AMST_LastName = a.AMST_LastName,
                                           asmc_sectionname = d.ASMC_SectionName,
                                           ASMCL_Classname = c.ASMCL_ClassName,
                                           trmR_Id=b.AMAY_RollNo
                                       }
).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
