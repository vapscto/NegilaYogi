using System;
using System.Linq;
using DataAccessMsSqlServerProvider;
using System.Collections.Concurrent;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.Extensions.Logging;

namespace CommonServiceHub.com.vaps.services
{
    public class CommonServiceImpl : interfaces.CommonServiceInterface
    {
        private static ConcurrentDictionary<string, FeeStudentTransactionDTO> _login =
        new ConcurrentDictionary<string, FeeStudentTransactionDTO>();
        private static readonly Object obj = new Object();

        public FeeGroupContext _YearlyFeeGroupMappingContext;
        public DomainModelMsSqlServerContext _context;
        readonly ILogger<CommonServiceImpl> _logger;
        public CommonServiceImpl(FeeGroupContext YearlyFeeGroupMappingContext, DomainModelMsSqlServerContext context, ILogger<CommonServiceImpl> log)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _context = context;
            _logger = log;
        }



        public FeeStudentTransactionDTO getstuddet(FeeStudentTransactionDTO data)
        {
            try
            {
                if (data.filterinitialdata.Equals("regular"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1)




                                        select (new
                                        {
                                            a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                            middlename = a.AMST_MiddleName,
                                            lastname = a.AMST_LastName
                                        })
             //select new FeeStudentTransactionDTO
             //{
             //    Amst_Id = a.AMST_Id,
             //    AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
             //    AMST_MiddleName = a.AMST_MiddleName,
             //    AMST_LastName = a.AMST_LastName,
             //}

             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("YearLoss"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1)

                                        select (new
                                        {
                                            a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                            middlename = a.AMST_MiddleName,
                                            lastname = a.AMST_LastName
                                        })
                                        //select new FeeStudentTransactionDTO
                                        //{
                                        //    Amst_Id = a.AMST_Id,
                                        //    AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                        //    AMST_MiddleName = a.AMST_MiddleName,
                                        //    AMST_LastName = a.AMST_LastName,
                                        //}
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("InActive"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "D" && b.AMAY_ActiveFlag == 1)

                                        select (new
                                        {
                                            a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                            middlename = a.AMST_MiddleName,
                                            lastname = a.AMST_LastName
                                        })

             //select new FeeStudentTransactionDTO
             //{
             //    Amst_Id = a.AMST_Id,
             //    AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
             //    AMST_MiddleName = a.AMST_MiddleName,
             //    AMST_LastName = a.AMST_LastName,
             //}
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("regno"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1)

                                        select (new { a.AMST_Id, a.AMST_RegistrationNo })
                                        //select new FeeStudentTransactionDTO
                                        //{
                                        //    Amst_Id = a.AMST_Id,
                                        //    AMST_FirstName = a.AMST_RegistrationNo,
                                        //    AMST_MiddleName = "",
                                        //    AMST_LastName = ""

                                        //}
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("AdmNo"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1)

                                        select (new { a.AMST_Id, a.AMST_AdmNo })

                                        //select new FeeStudentTransactionDTO
                                        //{
                                        //    Amst_Id = a.AMST_Id,
                                        //    AMST_FirstName = a.AMST_AdmNo,
                                        //    AMST_MiddleName = "",
                                        //    AMST_LastName = ""

                                        //}
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("Admnoname"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1)


                                        select (new
                                        {
                                            a.AMST_Id,
                                            AMST_FirstName = a.AMST_AdmNo + "-" + a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                            middlename = a.AMST_MiddleName,
                                            lastname = a.AMST_LastName
                                        })

                                        //select new FeeStudentTransactionDTO
                                        //{
                                        //    Amst_Id = a.AMST_Id,
                                        //    AMST_FirstName = a.AMST_AdmNo + "-" + a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                        //    AMST_MiddleName = a.AMST_MiddleName,
                                        //    AMST_LastName = a.AMST_LastName
                                        //}
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("NameAdmno"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1)


                                        select (new
                                        {
                                            a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName + "-" + a.AMST_AdmNo,
                                            middlename = a.AMST_MiddleName,
                                            lastname = a.AMST_LastName + "-" + a.AMST_AdmNo
                                        })

                                        //select new FeeStudentTransactionDTO
                                        //{
                                        //    Amst_Id = a.AMST_Id,
                                        //    AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName + "-" + a.AMST_AdmNo,
                                        //    AMST_MiddleName = a.AMST_MiddleName,
                                        //    AMST_LastName = a.AMST_LastName + "-" + a.AMST_AdmNo,
                                        //}
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("NameRegNo"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1)

                                        select (new
                                        {
                                            a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                            middlename = a.AMST_MiddleName,
                                            lastname = a.AMST_LastName + "-" + a.AMST_RegistrationNo
                                        })

                                        //select new FeeStudentTransactionDTO
                                        //{
                                        //    Amst_Id = a.AMST_Id,
                                        //    AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                        //    AMST_MiddleName = a.AMST_MiddleName,
                                        //    AMST_LastName = a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                        //}
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("RegNoName"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1)


                                        select (new
                                        {
                                            a.AMST_Id,
                                            AMST_FirstName = a.AMST_RegistrationNo + "-" + a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                            middlename = a.AMST_MiddleName,
                                            lastname = a.AMST_LastName
                                        })

                                        //select new FeeStudentTransactionDTO
                                        //{
                                        //    Amst_Id = a.AMST_Id,
                                        //    AMST_FirstName = a.AMST_RegistrationNo + "-" + a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                        //    AMST_MiddleName = a.AMST_MiddleName,
                                        //    AMST_LastName = a.AMST_LastName,
                                        //}
             ).ToArray();
                }
                else if (data.filterinitialdata.Equals("Preadmission"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.stuapp
                                        where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)

                                        select (new
                                        {
                                            a.pasr_id,
                                            AMST_FirstName = a.PASR_FirstName + ' ' + a.PASR_MiddleName + ' ' + a.PASR_LastName,
                                            middlename = a.PASR_MiddleName,
                                            lastname = a.PASR_LastName
                                        })
                                        //select new FeeStudentTransactionDTO
                                        //{
                                        //    Amst_Id = a.pasr_id,
                                        //    AMST_FirstName = a.PASR_FirstName + ' ' + a.PASR_MiddleName + ' ' + a.PASR_LastName,
                                        //    AMST_MiddleName = a.PASR_MiddleName,
                                        //    AMST_LastName = a.PASR_LastName,
                                        //}
             ).ToArray();
                }


                //  if (data.filterinitialdata != "Preadmission")
                //      {
                //          _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Student_Fee_Det @p0,@p1,@p2,@p3", data.Amst_Id, data.ASMAY_Id, data.FYP_Date, data.MI_Id);
                //      }
                //      else if (data.filterinitialdata.Equals("Preadmission"))
                //      {
                //          _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Student_Fee_Det_Preadmission @p0,@p1,@p2,@p3", data.Amst_Id, data.ASMAY_Id, data.FYP_Date, data.MI_Id);
                //      }

                //      data.alldata = (from a in _YearlyFeeGroupMappingContext.V_StudentPendingDMO
                //                      from b in _YearlyFeeGroupMappingContext.FeeHeadDMO
                //                      from c in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                //                      from d in _YearlyFeeGroupMappingContext.FeeHeadDMO
                //                      from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                //                      where (a.fmh_id==d.FMH_Id && a.fti_id==e.FTI_Id &&  a.asmay_id == data.ASMAY_Id && a.FSS_ToBePaid > 0 && a.fmh_id==b.FMH_Id && a.fti_id==c.FTI_Id) /*&& a.fmg_id.Contains(data.fmg_id)*/
                //                      select new FeeStudentTransactionDTO
                //                      {
                //                          FMA_Id = a.fma_id,
                //                          FMH_FeeName = b.FMH_FeeName,
                //                          FTI_Name = c.FTI_Name,
                //                          FMH_Id=a.fmh_id,
                //                          FTI_Id=a.fti_id,
                //                          FSS_NetAmount = a.FSS_NetAmount,
                //                          FSS_ToBePaid = a.FSS_ToBePaid,
                //                          FSS_ConcessionAmount = a.FSS_ConcessionAmount,
                //                          FSS_FineAmount = a.FSS_FineAmount,
                //                      }
                //).ToArray();

                //      if(data.alldata.Length==0)
                //      {
                //          data.validationvalue = "Student has paid total Amount";
                //      }
                //      else if (data.alldata.Length > 0)
                //      {
                //          data.validationvalue = "Data Displayed Successfully";
                //      }

            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                data.validationvalue = "Contact Administrator";
            }
            return data;
        }



    }
}



