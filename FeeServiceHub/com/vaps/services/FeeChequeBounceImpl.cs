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
using System.Data;
using System.Data.SqlClient;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeChequeBounceImpl : interfaces.FeeChequeBounceInterface
    {
        private static ConcurrentDictionary<string, FeeChequeBounceDTO> _login =
      new ConcurrentDictionary<string, FeeChequeBounceDTO>();

        public FeeGroupContext _YearlyFeeGroupMappingContext;
        public FeeChequeBounceImpl(FeeGroupContext YearlyFeeGroupMappingContext)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
        }
        public FeeChequeBounceDTO getdata(FeeChequeBounceDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _YearlyFeeGroupMappingContext.AcademicYear.Where(y=>y.MI_Id==data.MI_ID && y.Is_Active==true).OrderByDescending(y=>y.ASMAY_Order).ToList();
                data.fillyear = year.Distinct().ToArray();

                List<School_M_Class> cls = new List<School_M_Class>();
                cls = _YearlyFeeGroupMappingContext.School_M_Class.Where(c => c.MI_Id == data.MI_ID && c.ASMCL_ActiveFlag == true).ToList();
                data.classlist = cls.Distinct().ToArray();

                data.alldata = (from a in _YearlyFeeGroupMappingContext.FeeChequeBounceDMO
                                    from b in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    from c in _YearlyFeeGroupMappingContext.AcademicYear
                                    from d in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                    from e in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    from f in _YearlyFeeGroupMappingContext.School_M_Class
                                    from g in _YearlyFeeGroupMappingContext.school_M_Section
                                    where ( b.AMST_Id==e.AMST_Id && a.ASMAY_ID==e.ASMAY_Id && a.AMST_Id == b.AMST_Id && a.ASMAY_ID == c.ASMAY_Id && a.FYP_ID == d.FYP_Id  && e.ASMCL_Id==f.ASMCL_Id && e.ASMS_Id==g.ASMS_Id && a.MI_ID == data.MI_ID && a.ASMAY_ID == data.ASMAY_ID &&  d.FYP_Bank_Or_Cash=="B" && b.AMST_SOL == "S" && b.AMST_ActiveFlag==1)
                                    select new FeeChequeBounceDTO
                                    {
                                        ASMAY_Year = c.ASMAY_Year,
                                        FCB_DATE = a.FCB_DATE,
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = b.AMST_FirstName,
                                        AMST_MiddleName = b.AMST_MiddleName,
                                        AMST_LastName = b.AMST_LastName,
                                        fyP_Receipt_No = d.FYP_Receipt_No,
                                        FCB_Id = a.FCB_Id,
                                        ASMCL_ClassName=f.ASMCL_ClassName,
                                        ASMC_SectionName=g.ASMC_SectionName
                                    }
             ).OrderByDescending(t=>t.FCB_Id).ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public FeeChequeBounceDTO getdatastuacad(FeeChequeBounceDTO data)
        {
            try
            {
                data.classlist = (from a in _YearlyFeeGroupMappingContext.School_M_Class
                                  from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                  where (a.MI_Id == data.MI_ID && b.ASMAY_Id==data.ASMAY_ID && b.AMAY_ActiveFlag == 1 && b.ASMCL_Id == a.ASMCL_Id && a.ASMCL_ActiveFlag == true)
                                  select a).Distinct().OrderBy(t=>t.ASMCL_Order).ToArray();

                //   data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                //                       from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                //                       where (a.AMST_Id == b.AMST_Id && b.ASMAY_Id == data.ASMAY_ID && a.MI_Id == data.MI_ID && a.AMST_SOL == "S" && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)//
                //                       select new FeeChequeBounceDTO
                //                       {
                //                           AMST_Id = a.AMST_Id,
                //                           AMST_FirstName = a.AMST_FirstName,
                //                           AMST_MiddleName = a.AMST_MiddleName,
                //                           AMST_LastName = a.AMST_LastName,
                //                       }
                //).Distinct().ToArray();

                data.alldata = (from a in _YearlyFeeGroupMappingContext.FeeChequeBounceDMO
                                from b in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                from c in _YearlyFeeGroupMappingContext.AcademicYear
                                from d in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                from e in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                from f in _YearlyFeeGroupMappingContext.School_M_Class
                                from g in _YearlyFeeGroupMappingContext.school_M_Section
                                where (b.AMST_Id == e.AMST_Id && a.ASMAY_ID == e.ASMAY_Id && a.AMST_Id == b.AMST_Id && a.ASMAY_ID == c.ASMAY_Id && a.FYP_ID == d.FYP_Id && e.ASMCL_Id == f.ASMCL_Id && e.ASMS_Id == g.ASMS_Id && a.MI_ID == data.MI_ID && a.ASMAY_ID == data.ASMAY_ID && d.FYP_Bank_Or_Cash == "B" && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && a.user_id == data.user_id)
                                select new FeeChequeBounceDTO
                                {
                                    ASMAY_Year = c.ASMAY_Year,
                                    FCB_DATE = a.FCB_DATE,
                                    AMST_Id = a.AMST_Id,
                                    AMST_FirstName = b.AMST_FirstName,
                                    AMST_MiddleName = b.AMST_MiddleName,
                                    AMST_LastName = b.AMST_LastName,
                                    fyP_Receipt_No = d.FYP_Receipt_No,
                                    FCB_Id = a.FCB_Id,
                                    ASMCL_ClassName = f.ASMCL_ClassName,
                                    ASMC_SectionName = g.ASMC_SectionName
                                }
             ).OrderByDescending(t => t.AMST_FirstName).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public FeeChequeBounceDTO getdatastuacadgrp(FeeChequeBounceDTO data)
        {
            try
            {
                data.fillreceipt = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                    where (a.FYP_Id==b.FYP_Id && a.ASMAY_ID==b.ASMAY_Id &&  a.MI_Id == data.MI_ID && b.AMST_Id == data.AMST_Id && a.ASMAY_ID==data.ASMAY_ID && a.FYP_Bank_Or_Cash == "B" && a.FYP_Chq_Bounce=="CL" && a.user_id==data.user_id)
                                    select new FeeChequeBounceDTO
                                    {
                                        FYP_ID = a.FYP_Id,
                                        fyP_Receipt_No = a.FYP_Receipt_No,
                                    }
               ).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public FeeChequeBounceDTO getstuddet(FeeChequeBounceDTO data)
        {
            throw new NotImplementedException();
        }

        public FeeChequeBounceDTO savedetails(FeeChequeBounceDTO data)
        {
            FeeChequeBounceDMO pgmodule = Mapper.Map<FeeChequeBounceDMO>(data);
            try
            {

                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "ChequeBounceInsert";
                    cmd.CommandTimeout = 30000;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (data.FCB_Id > 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@FCBId",
                        SqlDbType.BigInt)
                        {
                            Value = data.FCB_Id
                        });
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@FCBId",
                      SqlDbType.BigInt)
                        {
                            Value = 0
                        });
                    }
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                       SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FCB_DATE",
                SqlDbType.DateTime)
                    {
                        Value = data.FCB_DATE
                    });

                    cmd.Parameters.Add(new SqlParameter("@FCB_Remarks",
                      SqlDbType.VarChar)
                    {
                        Value = data.FCB_Remarks
                    });

                    cmd.Parameters.Add(new SqlParameter("@FYPId",
                      SqlDbType.BigInt)
                    {
                        Value = data.FYP_ID
                    });


                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });


                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.user_id
                    });



                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var count = cmd.ExecuteNonQuery();

                    if (count >= 1)
                    {
                        data.validationvalue = "Saved";

                    }
                    else
                    {

                        data.validationvalue = "not Saved";
                    }
                }





            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public FeeChequeBounceDTO deleterec(FeeChequeBounceDTO data)
        {
           // bool returnresult = false;
           // List<FeeChequeBounceDMO> lorg = new List<FeeChequeBounceDMO>();
          //  lorg = _YearlyFeeGroupMappingContext.FeeChequeBounceDMO.Where(t => t.FCB_Id.Equals(data.FCB_Id)).ToList();
            
            try
            {
                //if (lorg.Any())
                //{
                //    _YearlyFeeGroupMappingContext.Remove(lorg.ElementAt(0));
                //    var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();

                //    if (contactExists == 1)
                //    {
                //        returnresult = true;
                //        data.returnval = returnresult;
                //    }
                //    else
                //    {
                //        returnresult = false;
                //        data.returnval = returnresult;
                //    }
                //}
                var result = _YearlyFeeGroupMappingContext.FeeChequeBounceDMO.Single(t => t.FCB_Id == data.FCB_Id);
                _YearlyFeeGroupMappingContext.Remove(result);

                data.FYP_ID = result.FYP_ID;
                data.ASMAY_ID = result.ASMAY_ID;
                data.AMST_Id = result.AMST_Id;

                var result_y_pay = _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO.Single(t => t.FYP_Id == data.FYP_ID);
                result_y_pay.FYP_Chq_Bounce = "CL";
                result_y_pay.UpdatedDate = DateTime.Now;
                _YearlyFeeGroupMappingContext.Update(result_y_pay);

                var list_fma_ids = _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO.Where(t => t.FYP_Id == data.FYP_ID).ToList();
                foreach (var r in list_fma_ids)
                {
                    var result_stu_sta = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Single(t => t.MI_Id == data.MI_ID && t.ASMAY_Id == data.ASMAY_ID && t.AMST_Id == data.AMST_Id && t.FMA_Id == r.FMA_Id && t.User_Id==data.user_id);

                    result_stu_sta.FSS_PaidAmount = result_stu_sta.FSS_PaidAmount + Convert.ToInt64(r.FTP_Paid_Amt);
                    result_stu_sta.FSS_ToBePaid = result_stu_sta.FSS_ToBePaid - Convert.ToInt64(r.FTP_Paid_Amt);
                    result_stu_sta.FSS_ChequeBounceFlag = false;

                    _YearlyFeeGroupMappingContext.Update(result_stu_sta);
                }
                var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
                if (contactExists >= 1)
                {
                   
                    data.returnval = true;
                }
                else
                {
                   
                    data.returnval = false;
                }

             //   data.fillstudent = (from a in _YearlyFeeGroupMappingContext.FeeChequeBounceDMO
             //                       from b in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
             //                       from c in _YearlyFeeGroupMappingContext.AcademicYear
             //                       from d in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
             //                       where (a.MI_ID == data.MI_ID && a.ASMAY_ID == data.ASMAY_ID && a.AMST_Id == b.AMST_Id && a.ASMAY_ID == c.ASMAY_Id && a.FYP_ID == d.FYP_Id && b.AMST_SOL == "S")
             //                       select new FeeChequeBounceDTO
             //                       {
             //                           ASMAY_Year = c.ASMAY_Year,
             //                           FCB_DATE = a.FCB_DATE,

             //                           AMST_Id = a.AMST_Id,
             //                           AMST_FirstName = b.AMST_FirstName,
             //                           AMST_MiddleName = b.AMST_MiddleName,
             //                           AMST_LastName = b.AMST_LastName,
             //                           fyP_Receipt_No = d.FYP_Receipt_No
             //                       }
             //).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public FeeChequeBounceDTO geteditdet(FeeChequeBounceDTO data)
        {
            try
            {
                data.fillstudent = (from a in _YearlyFeeGroupMappingContext.FeeChequeBounceDMO
                                    from b in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    from c in _YearlyFeeGroupMappingContext.AcademicYear
                                    from d in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                    where (a.MI_ID == data.MI_ID && a.ASMAY_ID == data.ASMAY_ID && a.AMST_Id == b.AMST_Id && a.ASMAY_ID == c.ASMAY_Id && a.FYP_ID == d.FYP_Id && a.FCB_Id == data.FCB_Id && b.AMST_SOL == "S" && b.AMST_ActiveFlag==1 && a.user_id==data.user_id)
                                    select new FeeChequeBounceDTO
                                    {
                                        ASMAY_Year = c.ASMAY_Year,
                                        FCB_DATE = a.FCB_DATE,
                                        FYP_ID=d.FYP_Id,
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = b.AMST_FirstName,
                                        AMST_MiddleName = b.AMST_MiddleName,
                                        AMST_LastName = b.AMST_LastName,
                                        fyP_Receipt_No = d.FYP_Receipt_No,
                                        ASMAY_ID=a.ASMAY_ID,
                                        FCB_Id = a.FCB_Id,
                                        FCB_Remarks=a.FCB_Remarks
                                    }
            ).Distinct().OrderBy(t=>t.AMST_FirstName).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }
        public FeeChequeBounceDTO searching(FeeChequeBounceDTO data)
        {

            try
            {

                //switch (data.searchType)
                //{
                //    case "0":
                //        data.alldata = (from a in _YearlyFeeGroupMappingContext.FeeChequeBounceDMO
                //                        from b in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                //                        from c in _YearlyFeeGroupMappingContext.AcademicYear
                //                        from d in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                //                        where (a.MI_ID == data.MI_ID && a.ASMAY_ID == data.ASMAY_ID && a.AMST_Id == b.AMST_Id && a.ASMAY_ID == c.ASMAY_Id && a.FYP_ID == d.FYP_Id && d.FYP_Bank_Or_Cash == "B" && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1
                //                        && c.ASMAY_Year.Contains(data.searchtext) && a.user_id==data.user_id)
                //                        select new FeeChequeBounceDTO
                //                        {
                //                            ASMAY_Year = c.ASMAY_Year,
                //                            FCB_DATE = a.FCB_DATE,

                //                            AMST_Id = a.AMST_Id,
                //                            AMST_FirstName = b.AMST_FirstName,
                //                            AMST_MiddleName = b.AMST_MiddleName,
                //                            AMST_LastName = b.AMST_LastName,
                //                            fyP_Receipt_No = d.FYP_Receipt_No,

                //                            FCB_Id = a.FCB_Id
                //                        }).OrderByDescending(t => t.ASMAY_Year).ToList().ToArray();

                //        break;
                //    case "1":

                //        data.alldata = (from a in _YearlyFeeGroupMappingContext.FeeChequeBounceDMO
                //                        from b in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                //                        from c in _YearlyFeeGroupMappingContext.AcademicYear
                //                        from d in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                //                        where (a.MI_ID == data.MI_ID && a.ASMAY_ID == data.ASMAY_ID && a.AMST_Id == b.AMST_Id && a.ASMAY_ID == c.ASMAY_Id && a.FYP_ID == d.FYP_Id && d.FYP_Bank_Or_Cash == "B" && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1
                //                        && a.FCB_DATE.ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy") && a.user_id == data.user_id)
                //                        select new FeeChequeBounceDTO
                //                        {
                //                            ASMAY_Year = c.ASMAY_Year,
                //                            FCB_DATE = a.FCB_DATE,

                //                            AMST_Id = a.AMST_Id,
                //                            AMST_FirstName = b.AMST_FirstName,
                //                            AMST_MiddleName = b.AMST_MiddleName,
                //                            AMST_LastName = b.AMST_LastName,
                //                            fyP_Receipt_No = d.FYP_Receipt_No,

                //                            FCB_Id = a.FCB_Id
                //                        }).OrderByDescending(t => t.FCB_DATE).ToList().ToArray();
                //        break;
                //    case "2":
                //        string str = "";
                //        data.searchtext = data.searchtext.ToUpper();
                //        data.alldata = (from a in _YearlyFeeGroupMappingContext.FeeChequeBounceDMO
                //                        from b in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                //                        from c in _YearlyFeeGroupMappingContext.AcademicYear
                //                        from d in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                //                        where (a.MI_ID == data.MI_ID && a.ASMAY_ID == data.ASMAY_ID && a.AMST_Id == b.AMST_Id && a.ASMAY_ID == c.ASMAY_Id && a.FYP_ID == d.FYP_Id && d.FYP_Bank_Or_Cash == "B" && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && a.user_id == data.user_id
                //                        && (((b.AMST_FirstName.ToUpper().Trim() + ' ' + (string.IsNullOrEmpty(b.AMST_MiddleName.ToUpper().Trim()) == true ? str : b.AMST_MiddleName.ToUpper().Trim())).Trim() + ' ' + (string.IsNullOrEmpty(b.AMST_LastName.ToUpper().Trim()) == true ? str : b.AMST_LastName.ToUpper().Trim())).Trim().ToLower().Contains(data.searchtext.ToLower()) || b.AMST_FirstName.ToUpper().StartsWith(data.searchtext.ToLower()) || b.AMST_MiddleName.ToUpper().StartsWith(data.searchtext.ToUpper()) || b.AMST_LastName.ToUpper().StartsWith(data.searchtext.ToUpper())) )
                //                        select new FeeChequeBounceDTO
                //                        {
                //                            ASMAY_Year = c.ASMAY_Year,
                //                            FCB_DATE = a.FCB_DATE,

                //                            AMST_Id = a.AMST_Id,
                //                            AMST_FirstName = b.AMST_FirstName,
                //                            AMST_MiddleName = b.AMST_MiddleName,
                //                            AMST_LastName = b.AMST_LastName,
                //                            fyP_Receipt_No = d.FYP_Receipt_No,

                //                            FCB_Id = a.FCB_Id
                //                        }).OrderByDescending(t => t.AMST_FirstName).ToList().ToArray();
                //        break;
                //    case "3":
                //        data.alldata = (from a in _YearlyFeeGroupMappingContext.FeeChequeBounceDMO
                //                        from b in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                //                        from c in _YearlyFeeGroupMappingContext.AcademicYear
                //                        from d in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                //                        where (a.MI_ID == data.MI_ID && a.ASMAY_ID == data.ASMAY_ID && a.AMST_Id == b.AMST_Id && a.ASMAY_ID == c.ASMAY_Id && a.FYP_ID == d.FYP_Id && d.FYP_Bank_Or_Cash == "B" && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1
                //                        && d.FYP_Receipt_No.Contains(data.searchtext) && a.user_id == data.user_id)
                //                        select new FeeChequeBounceDTO
                //                        {
                //                            ASMAY_Year = c.ASMAY_Year,
                //                            FCB_DATE = a.FCB_DATE,

                //                            AMST_Id = a.AMST_Id,
                //                            AMST_FirstName = b.AMST_FirstName,
                //                            AMST_MiddleName = b.AMST_MiddleName,
                //                            AMST_LastName = b.AMST_LastName,
                //                            fyP_Receipt_No = d.FYP_Receipt_No,

                //                            FCB_Id = a.FCB_Id
                //                        }).OrderByDescending(t => t.fyP_Receipt_No).ToList().ToArray();
                //        break;                   
                //}
                if (data.searchtext == null)
                {
                    data.searchtext = "";
                }
                List<FeeChequeBounceDTO> result = new List<FeeChequeBounceDTO>();
                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Search_CheckBounce";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });

                    cmd.Parameters.Add(new SqlParameter("@searchtext",
                                 SqlDbType.VarChar)
                    {
                        Value = data.searchtext
                    });

                    cmd.Parameters.Add(new SqlParameter("@date",
                        SqlDbType.Date)
                    {
                        Value = data.searchdate
                    });

                    cmd.Parameters.Add(new SqlParameter("@type",
                        SqlDbType.VarChar)
                    {
                        Value = data.searchType
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_id",
                   SqlDbType.BigInt)
                    {
                        Value = data.user_id
                    });

                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
                 SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_ID
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
                                result.Add(new FeeChequeBounceDTO
                                {
                                    ASMAY_Year = dataReader["ASMAY_Year"].ToString(),
                                    FCB_DATE = Convert.ToDateTime(dataReader["FCB_DATE"].ToString()),
                                    AMST_Id = Convert.ToInt64(dataReader["AMST_Id"].ToString()),
                                    AMST_FirstName = dataReader["AMST_FirstName"].ToString(),
                                    AMST_MiddleName = dataReader["AMST_MiddleName"].ToString(),
                                    AMST_LastName = dataReader["AMST_LastName"].ToString(),
                                    fyP_Receipt_No = dataReader["fyP_Receipt_No"].ToString(),
                                    FCB_Id = Convert.ToInt64(dataReader["FCB_Id"].ToString()),

                                });
                            }
                        }
                        data.alldata = result.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public FeeChequeBounceDTO get_students(FeeChequeBounceDTO data)
        {
            try
            {

                //   data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                //                       from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                //                       where (a.AMST_Id == b.AMST_Id && b.ASMAY_Id == data.ASMAY_ID && a.MI_Id == data.MI_ID && a.AMST_SOL == "S" && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && b.ASMCL_Id==data.ASMCL_Id)//
                //                       select new FeeChequeBounceDTO
                //                       {
                //                           AMST_Id = a.AMST_Id,
                //                           AMST_FirstName = a.AMST_FirstName,
                //                           AMST_MiddleName = a.AMST_MiddleName,
                //                           AMST_LastName = a.AMST_LastName,
                //                       }
                //).Distinct().ToArray();


                data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    from c in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                    from  d in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                    where (a.AMST_Id == b.AMST_Id && b.ASMAY_Id == data.ASMAY_ID && a.MI_Id == data.MI_ID && a.AMST_SOL == "S" && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && b.ASMCL_Id == data.ASMCL_Id && c.MI_Id==data.MI_ID && c.ASMAY_ID==d.ASMAY_Id && c.ASMAY_ID==data.ASMAY_ID && d.FYP_Id==c.FYP_Id && d.AMST_Id==a.AMST_Id && c.FYP_Bank_Or_Cash=="B" && b.ASMS_Id==data.ASMS_Id)
                                    select new FeeChequeBounceDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_FirstName,
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                    }
            ).Distinct().OrderBy(t=>t.AMST_FirstName).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public FeeChequeBounceDTO get_section(FeeChequeBounceDTO data)
        {
            try
            {
                data.fillsection = (from a in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    from b in _YearlyFeeGroupMappingContext.school_M_Section
                                    where (a.ASMS_Id == b.ASMS_Id && b.MI_Id == data.MI_ID && a.ASMAY_Id == data.ASMAY_ID && a.ASMCL_Id==data.ASMCL_Id)
                                    select new FeeStudentAdjustmentDTO
                                    {
                                        ASMS_Id = a.ASMS_Id,
                                        ASMC_SectionName = b.ASMC_SectionName,
                                        ASMC_Order=b.ASMC_Order
                                    }).Distinct().OrderBy(t => t.ASMC_Order).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public FeeChequeBounceDTO get_receipts(FeeChequeBounceDTO data)
        {
            try
            {
                data.fillreceipt = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                    where (a.FYP_Id == b.FYP_Id && a.ASMAY_ID == b.ASMAY_Id && a.MI_Id == data.MI_ID && b.AMST_Id == data.AMST_Id && a.ASMAY_ID == data.ASMAY_ID && a.FYP_Bank_Or_Cash == "B" && a.FYP_Chq_Bounce == "CL" && a.user_id==data.user_id)
                                    select new FeeChequeBounceDTO
                                    {
                                        FYP_ID = a.FYP_Id,
                                        fyP_Receipt_No = a.FYP_Receipt_No,
                                    }
               ).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

    }
}

























