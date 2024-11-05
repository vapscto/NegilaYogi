using DataAccessMsSqlServerProvider;
using PreadmissionDTOs.com.vaps.College.Fee;
using FeeServiceHub.com.vaps.interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace FeeServiceHub.com.vaps.services
{
    public class ThirdPartyTransactionImpl : interfaces.ThirdPartyTransactionInterface
    {

        public FeeGroupContext _FeeGroupContext;

        public ThirdPartyTransactionImpl(FeeGroupContext feeGroupContext)
        {
            _FeeGroupContext = feeGroupContext;
        }


        public ThirdPartyTransactionDTO getdetails(ThirdPartyTransactionDTO data)
        {
            try
            {
                data.yearlist = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id).OrderByDescending(t => t.ASMAY_Order).Distinct().ToArray();

                data.alldata = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                from b in _FeeGroupContext.Fee_Y_Payment_ThirdPartyDMO
                                from c in _FeeGroupContext.FeeHeadDMO
                                from d in _FeeGroupContext.AcademicYear
                                where (a.ASMAY_ID == d.ASMAY_Id && a.MI_Id == d.MI_Id && a.FYP_Id == b.FYP_Id && c.FMH_Id == b.FMH_Id && d.MI_Id == data.MI_Id && c.FMH_ActiveFlag == true && d.ASMAY_ActiveFlag == 1)
                                select new ThirdPartyTransactionDTO
                                {
                                    //Student Name
                                    FYPTP_Id = b.FYPTP_Id,
                                    FYPTP_Name = b.FYPTP_Name,
                                    FMH_FeeName = c.FMH_FeeName,
                                    FMH_Id = c.FMH_Id,
                                    FYP_Receipt_No = a.FYP_Receipt_No,
                                    FYP_Bank_Name = a.FYP_Bank_Name,
                                    FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                    FYP_Tot_Amount = a.FYP_Tot_Amount,
                                    FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                    FYP_Id = a.FYP_Id,
                                    FYP_Date = a.FYP_Date,
                                    ASMAY_Year = d.ASMAY_Year,
                                    FYPTP_Towards = b.FYPTP_Towards
                                }
     ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();


                data.thirdparty_auto_receipt = _FeeGroupContext.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "ThirdParty").ToArray();

                data.grouplist = (from a in _FeeGroupContext.FeeHeadDMO
                                      // from b in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                  where (a.MI_Id == data.MI_Id && a.FMH_ActiveFlag == true
                                  )
                                  select new ThirdPartyTransactionDTO
                                  {
                                      FMH_Id = a.FMH_Id,
                                      FMH_FeeName = a.FMH_FeeName,
                                      FMH_Order = a.FMH_Order
                                  }
                               ).Distinct().OrderBy(t => t.FMH_Order).ToArray();


                data.feegrouplist = (from a in _FeeGroupContext.FeeGroupDMO
                                         // from b in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                     where (a.MI_Id == data.MI_Id && a.FMG_ActiceFlag == true
                                     )
                                     select new ThirdPartyTransactionDTO
                                     {
                                         FMG_Id = a.FMG_Id,
                                         FMG_GroupName = a.FMG_GroupName

                                     }
                             ).Distinct().OrderBy(t => t.FMG_Id).ToArray();


                data.stdetails = _FeeGroupContext.Fee_Master_BankDMO.Where(t => t.MI_Id == data.MI_Id && t.FMBANK_ActiveFlg == true).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ThirdPartyTransactionDTO printreceipt(ThirdPartyTransactionDTO data)
        {
            try
            {
                data.stdetails = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                  from b in _FeeGroupContext.Fee_Y_Payment_ThirdPartyDMO
                                  from c in _FeeGroupContext.FeeHeadDMO
                                  from d in _FeeGroupContext.AcademicYear
                                  where (a.ASMAY_ID == d.ASMAY_Id && a.MI_Id == d.MI_Id && a.FYP_Id == b.FYP_Id && c.FMH_Id == b.FMH_Id
                                  && d.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id && b.FYPTP_Id == data.FYPTP_Id && c.FMH_ActiveFlag == true && d.ASMAY_ActiveFlag == 1)
                                  select new ThirdPartyTransactionDTO
                                  {
                                      //Student Name
                                      FYPTP_Id = b.FYPTP_Id,
                                      FYPTP_Name = b.FYPTP_Name,
                                      FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                      FMH_FeeName = c.FMH_FeeName,
                                      FYP_Remarks = a.FYP_Remarks,
                                      FMH_Id = c.FMH_Id,
                                      FYP_Receipt_No = a.FYP_Receipt_No,
                                      FYP_Bank_Name = a.FYP_Bank_Name,
                                      FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                      FYP_Tot_Amount = a.FYP_Tot_Amount,
                                      FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                      FYP_Id = a.FYP_Id,
                                      FYP_Date = a.FYP_Date,
                                      ASMAY_Year = d.ASMAY_Year,
                                      FYPTP_Towards = b.FYPTP_Towards
                                  }
     ).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public ThirdPartyTransactionDTO getgrpdetails(ThirdPartyTransactionDTO data)
        {
            try
            {
                //      data.studntlist = (from a in _FeeGroupContext.AdmissionStudentDMO
                //                         from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                //                         where (a.AMST_Id == b.AMST_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1
                //                         && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id)//
                //                         select new ThirdPartyTransactionDTO
                //                         {
                //                             AMST_Id = a.AMST_Id,
                //                             // AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                //                             AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),

                //                             AMST_MiddleName = a.AMST_MiddleName,
                //                             AMST_LastName = a.AMST_LastName,
                //                         }
                //).Distinct().ToArray();

                string MI_SchoolCollegeFlag = "";

                 MI_SchoolCollegeFlag = _FeeGroupContext.Institution.Where(a => a.MI_Id == data.MI_Id).Select(a => a.MI_SchoolCollegeFlag).FirstOrDefault();

                if(MI_SchoolCollegeFlag == "S")
                {
                //    data.studntlist = (from a in _FeeGroupContext.AdmissionStudentDMO
                //                        from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                //                        from c in _FeeGroupContext.School_M_Class
                //                        from d in _FeeGroupContext.school_M_Section
                //                        where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1
                //                        && ((a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).StartsWith(data.searchfilter) || (a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || (a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || a.AMST_FirstName.ToUpper().Contains(data.searchfilter) || a.AMST_MiddleName.ToUpper().Contains(data.searchfilter) || a.AMST_LastName.ToUpper().Contains(data.searchfilter)))
                //                        select new ThirdPartyTransactionDTO
                //                        {
                //                            AMST_Id = a.AMST_Id,
                //                            AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                //                        (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                //                        (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim() + "-" + c.ASMCL_ClassName + "-" + d.ASMC_SectionName + "-" + a.AMST_AdmNo,
                //                            AMST_MiddleName = a.AMST_MiddleName,
                //                            AMST_LastName = a.AMST_LastName,
                //                        }
                //).OrderByDescending(g => (g.AMST_FirstName.Trim().ToUpper() + ' ' + g.AMST_MiddleName.Trim().ToUpper() + ' ' + g.AMST_LastName.Trim().ToUpper()).StartsWith(data.searchfilter)).ToArray();

                }
                else
                {
                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "getstudentfor_thirdpartytransaction";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt, 100)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.BigInt, 100)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@user_id", SqlDbType.BigInt, 100)
                        {
                            Value = data.user_id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject1 = new List<dynamic>();
                        try
                        {
                            using (var dataReader1 = cmd.ExecuteReader())
                            {
                                while (dataReader1.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader1.GetName(iFiled1),
                                            dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]
                                        );
                                    }

                                    retObject1.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.studntlist = retObject1.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }


                //data.grouplist = (from a in _FeeGroupContext.FeeGroupDMO
                //                  from b in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                //                  where (a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FMG_ActiceFlag == true && b.FYGHM_ActiveFlag == "1")
                //                  select new ThirdPartyTransactionDTO
                //                  {
                //                      FMG_Id = a.FMG_Id,
                //                      FMG_GroupName = a.FMG_GroupName
                //                  }
                //                   ).Distinct().ToArray();


                //   data.grouplist = (from a in _FeeGroupContext.FeeHeadDMO
                //                    // from b in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                //                     where (a.MI_Id == data.MI_Id && a.FMH_ActiveFlag == true 
                //                     )
                //                     select new ThirdPartyTransactionDTO
                //                     {
                //                         FMH_Id = a.FMH_Id,
                //                         FMH_FeeName = a.FMH_FeeName,
                //                         FMH_Order = a.FMH_Order
                //                     }
                //                   ).Distinct().OrderBy(t => t.FMH_Order).ToArray();


                //   data.feegrouplist = (from a in _FeeGroupContext.FeeGroupDMO
                //                            // from b in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                //                        where (a.MI_Id == data.MI_Id && a.FMG_ActiceFlag == true
                //                        )
                //                        select new ThirdPartyTransactionDTO
                //                        {
                //                            FMG_Id = a.FMG_Id,
                //                            FMG_GroupName = a.FMG_GroupName

                //                        }
                //).Distinct().OrderBy(t => t.FMG_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        //getStudtdetails
        public ThirdPartyTransactionDTO getStudtdetails(ThirdPartyTransactionDTO data)
        {
            try
            {

                data.studinfo = (from a in _FeeGroupContext.AdmissionStudentDMO
                                 from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                 from c in _FeeGroupContext.AcademicYear
                                 from d in _FeeGroupContext.School_M_Class
                                 from e in _FeeGroupContext.school_M_Section
                                 where (a.AMST_Id == b.AMST_Id && d.MI_Id == a.MI_Id && e.ASMS_Id == b.ASMS_Id
                                 && e.MI_Id == a.MI_Id && a.MI_Id == c.MI_Id && b.ASMAY_Id == c.ASMAY_Id
                                 && b.ASMCL_Id == d.ASMCL_Id && a.MI_Id == data.MI_Id
                                 && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id
                                 && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1
                                 && a.AMST_SOL == "S" && d.ASMCL_ActiveFlag == true
                                 && e.ASMC_ActiveFlag == 1) //a.AMST_SOL == "S" && &&  a.ASMAY_Id == b.ASMAY_Id
                                 select new ThirdPartyTransactionDTO
                                 {
                                     AMST_Id = a.AMST_Id,
                                     // AMST_FirstName = a.AMST_FirstName +" "+ a.AMST_MiddleName+" "+ a.AMST_LastName,
                                     //   AMST_FirstName= a.AMST_FirstName + (string.IsNullOrEmpty(a.AMST_MiddleName) ? "" : ' ' + a.AMST_MiddleName) + (string.IsNullOrEmpty(a.AMST_LastName) ? "" : ' ' + a.AMST_LastName),
                                     AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),

                                     //AMST_MiddleName = a.AMST_MiddleName,
                                     //AMST_LastName = a.AMST_LastName,
                                     AMST_AdmNo = a.AMST_AdmNo,
                                     AMST_RegistrationNo = a.AMST_RegistrationNo,
                                     ASMCL_ClassName = d.ASMCL_ClassName,
                                     AMAY_RollNo = b.AMAY_RollNo,
                                     ASMC_SectionName = e.ASMC_SectionName,
                                     ASMAY_Year = c.ASMAY_Year
                                 }
          ).OrderBy(t => t.AMAY_RollNo).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        //SaveOthersgroupdata
        public ThirdPartyTransactionDTO SaveStudentgroupdata(ThirdPartyTransactionDTO data)
        {
            try
            {
                if (data.FYP_Id > 0)
                {

                    var updatedata1 = _FeeGroupContext.FeePaymentDetailsDMO.Single(t => t.FYP_Id == data.FYP_Id);

                    updatedata1.ASMAY_ID = data.ASMAY_Id;
                    updatedata1.FTCU_Id = 1;
                    updatedata1.FYP_Bank_Name = data.FYP_Bank_Or_Cash == "C" ? "" : data.FYP_Bank_Name;
                    updatedata1.FYP_Bank_Or_Cash = data.FYP_Bank_Or_Cash;
                    updatedata1.FYP_DD_Cheque_No = data.FYP_Bank_Or_Cash == "C" ? "" : data.FYP_DD_Cheque_No;
                    updatedata1.FYP_DD_Cheque_Date = data.FYP_Bank_Or_Cash == "C" ? data.FYP_Date : data.FYP_DD_Cheque_Date;
                    updatedata1.FYP_Date = data.FYP_Date;
                    updatedata1.FYP_Tot_Amount = data.FYP_Tot_Amount;
                    updatedata1.FYP_Tot_Waived_Amt = 0;
                    updatedata1.FYP_Tot_Fine_Amt = 0;
                    updatedata1.FYP_Tot_Concession_Amt = 0;
                    updatedata1.FYP_Remarks = data.FYP_Remarks;
                    updatedata1.FYP_Chq_Bounce = "CL";
                    updatedata1.MI_Id = data.MI_Id;
                    updatedata1.DOE = DateTime.Now;
                    updatedata1.UpdatedDate = DateTime.Now;

                    _FeeGroupContext.Update(updatedata1);


                    var updatedata2 = _FeeGroupContext.Fee_Y_Payment_ThirdPartyDMO.Single(t => t.FYP_Id == data.FYP_Id);

                    updatedata2.FYP_Id = updatedata1.FYP_Id;
                    updatedata2.FYPTP_Name = data.FYPTP_Name;
                    updatedata2.FTP_TotalPaidAmount = data.FYP_Tot_Amount;
                    updatedata2.FYPTP_Towards = data.FYP_Remarks;
                    updatedata2.FMH_Id = data.FMH_Id;

                    _FeeGroupContext.Update(updatedata2);

                    var contactExists = _FeeGroupContext.SaveChanges();
                    if (contactExists >= 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else
                {
                    FeePaymentDetailsDMO obj1 = new FeePaymentDetailsDMO();


                    if (data.FYP_Receipt_No == "" || data.FYP_Receipt_No == null)
                    {

                        List<ThirdPartyTransactionDTO> list_all = new List<ThirdPartyTransactionDTO>();
                        List<ThirdPartyTransactionDTO> list_repts = new List<ThirdPartyTransactionDTO>();

                        list_all = (from b in _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO
                                    from c in _FeeGroupContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                    where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && c.FMG_Id == data.FMG_Id && b.FGAR_Id == c.FGAR_Id)

                                    select new ThirdPartyTransactionDTO
                                    {
                                        FMG_Id = c.FMG_Id
                                    }

                             ).Distinct().ToList();



                        if (list_all.Count() >= 1)
                        {
                            var masterinstitution = _FeeGroupContext.master_institution.Where(z => z.MI_ActiveFlag == 1 && z.MI_SchoolCollegeFlag == "S" && z.MI_Id == data.MI_Id).ToList();


                            using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                            {

                                if (masterinstitution.Count > 0)
                                {
                                    cmd.CommandText = "receiptnogeneration";
                                }
                                else
                                {
                                    cmd.CommandText = "receiptnogeneration_CLG";
                                }

                                //cmd.CommandText = "receiptnogeneration_CLG";
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
                                    Value = data.FMG_Id
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
                        }


                        else
                        {
                            using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Thirdpartyreceiptnogeneration";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                    SqlDbType.VarChar, 100)
                                {
                                    Value = data.MI_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@asmayid",
                                   SqlDbType.NVarChar, 100)
                                {
                                    Value = data.ASMAY_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@Receiptno",
                                 SqlDbType.NVarChar, 500)
                                {
                                    Direction = ParameterDirection.Output
                                });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var data1 = cmd.ExecuteNonQuery();

                                data.FYP_Receipt_No = cmd.Parameters["@Receiptno"].Value.ToString();
                            }
                        }
                    }

                    else
                    {
                        List<ThirdPartyTransactionDTO> list_all = new List<ThirdPartyTransactionDTO>();
                        List<ThirdPartyTransactionDTO> list_repts = new List<ThirdPartyTransactionDTO>();

                        list_all = (from b in _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO
                                    from c in _FeeGroupContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                    where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && c.FMG_Id == data.FMG_Id && b.FGAR_Id == c.FGAR_Id)

                                    select new ThirdPartyTransactionDTO
                                    {
                                        FMG_Id = c.FMG_Id
                                    }

                             ).Distinct().ToList();



                        if (list_all.Count() > 1)
                        {
                            var masterinstitution = _FeeGroupContext.master_institution.Where(z => z.MI_ActiveFlag == 1 && z.MI_SchoolCollegeFlag == "S" && z.MI_Id == data.MI_Id).ToList();

                            using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                            {
                                if (masterinstitution.Count > 0)
                                {
                                    cmd.CommandText = "receiptnogeneration";
                                }
                                else
                                {
                                    cmd.CommandText = "receiptnogeneration_CLG";
                                }
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
                                    Value = data.FMG_Id
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

                        }

                    }
                    var Duplicate = _FeeGroupContext.FeePaymentDetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_ID == data.ASMAY_Id && t.FYP_Receipt_No == data.FYP_Receipt_No).ToList();
                    if (Duplicate.Count() > 0)
                    {
                        data.dulicate = true;
                    }
                    else
                    {
                        if (data.FYP_Receipt_No != "" || data.FYP_Receipt_No != null)
                        {
                            obj1.ASMAY_ID = data.ASMAY_Id;
                            obj1.FTCU_Id = 1;

                            obj1.FYP_Receipt_No = data.FYP_Receipt_No;

                            obj1.FYP_Bank_Name = data.FYP_Bank_Or_Cash == "C" ? "" : data.FYP_Bank_Name;
                            obj1.FYP_Bank_Or_Cash = data.FYP_Bank_Or_Cash;
                            obj1.FYP_DD_Cheque_No = data.FYP_Bank_Or_Cash == "C" ? "" : data.FYP_DD_Cheque_No;
                            obj1.FYP_DD_Cheque_Date = data.FYP_Bank_Or_Cash == "C" ? data.FYP_Date : data.FYP_DD_Cheque_Date;
                            obj1.FYP_Date = data.FYP_Date;
                            obj1.FYP_Tot_Amount = data.FYP_Tot_Amount;
                            obj1.FYP_Tot_Waived_Amt = 0;
                            obj1.FYP_Tot_Fine_Amt = 0;
                            obj1.FYP_Tot_Concession_Amt = 0;
                            obj1.FYP_Remarks = data.FYP_Remarks;
                            obj1.FYP_Chq_Bounce = "CL";
                            obj1.MI_Id = data.MI_Id;
                            obj1.DOE = DateTime.Now;
                            obj1.CreatedDate = DateTime.Now;
                            obj1.UpdatedDate = DateTime.Now;
                            obj1.user_id = data.user_id;

                            _FeeGroupContext.Add(obj1);


                            Fee_Y_Payment_ThirdPartyDMO obj2 = new Fee_Y_Payment_ThirdPartyDMO();

                            obj2.FYP_Id = obj1.FYP_Id;
                            obj2.FYPTP_Name = data.FYPTP_Name;
                            obj2.FTP_TotalPaidAmount = data.FYP_Tot_Amount;
                            obj2.FYPTP_Towards = data.FYP_Remarks;
                            obj2.FMH_Id = data.FMH_Id;

                            _FeeGroupContext.Add(obj2);

                            var contactExists = _FeeGroupContext.SaveChanges();
                            if (contactExists >= 1)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //Ckeck_Receipt
        public ThirdPartyTransactionDTO Ckeck_Receipt(ThirdPartyTransactionDTO data)
        {
            try
            {
                var receipt_count = _FeeGroupContext.FeePaymentDetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_ID == data.ASMAY_Id && t.FYP_Receipt_No.Contains(data.FYP_Receipt_No)).ToList().Count;
                if (receipt_count == 0)
                {
                    data.returnval = false;
                }
                else if (receipt_count > 0)
                {
                    data.returnval = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //editOthtransaction
        public ThirdPartyTransactionDTO editOthtransaction(ThirdPartyTransactionDTO data)
        {
            try
            {
                data.EditOther = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                  from b in _FeeGroupContext.Fee_Y_Payment_ThirdPartyDMO
                                  where (a.FYP_Id == b.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Id == data.FYP_Id)
                                  select new ThirdPartyTransactionDTO
                                  {
                                      ASMAY_Id = a.ASMAY_ID,
                                      FYP_Receipt_No = a.FYP_Receipt_No,
                                      FYP_Bank_Name = a.FYP_Bank_Or_Cash == "B" ? a.FYP_Bank_Name : "",
                                      FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                      FYP_DD_Cheque_No = a.FYP_Bank_Or_Cash == "B" ? a.FYP_DD_Cheque_No : "",
                                      FYP_DD_Cheque_Date = a.FYP_Bank_Or_Cash == "B" ? a.FYP_DD_Cheque_Date : a.FYP_Date,
                                      FYP_Date = a.FYP_Date,
                                      FYP_Tot_Amount = a.FYP_Tot_Amount,
                                      FYPTP_Id = b.FYPTP_Id,
                                      FYP_Id = b.FYP_Id,
                                      FYPTP_Name = b.FYPTP_Name,
                                      FTP_TotalPaidAmount = b.FTP_TotalPaidAmount,
                                      FYPTP_Towards = b.FYPTP_Towards,
                                      FMH_Id = b.FMH_Id,
                                      FYP_Remarks = a.FYP_Remarks,
                                  }
                                ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ThirdPartyTransactionDTO DeletOthrRecord(ThirdPartyTransactionDTO data)
        {
            try
            {
                // FeePaymentDetailsDMO obj1 = new FeePaymentDetailsDMO();
                var remlve = _FeeGroupContext.FeePaymentDetailsDMO.Single(t => t.FYP_Id == data.FYP_Id);
                _FeeGroupContext.Remove(remlve);
                var remove2 = _FeeGroupContext.Fee_Y_Payment_ThirdPartyDMO.Single(t => t.FYP_Id == data.FYP_Id);
                _FeeGroupContext.Remove(remove2);

                var count = _FeeGroupContext.SaveChanges();
                if (count > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
