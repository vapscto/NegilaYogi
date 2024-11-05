using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class BranchChangeIMPL : Interface.BranchChangeInterface
    {
        private static ConcurrentDictionary<string, BranchChangeDTO> MsCadm =
               new ConcurrentDictionary<string, BranchChangeDTO>();
        public DomainModelMsSqlServerContext _db;
        public ClgAdmissionContext _branchange;
        public BranchChangeIMPL(ClgAdmissionContext mscadm, DomainModelMsSqlServerContext db)
        {
            _db = db;
            _branchange = mscadm;
        }
        public BranchChangeDTO getdetails(BranchChangeDTO data)
        {
            try
            {
                data.yearlist = _branchange.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToList().ToArray();
                data.courseslist = _branchange.MasterCourseDMO.Where(c => c.MI_Id == data.MI_Id && c.AMCO_ActiveFlag == true).ToList().Distinct().ToArray();
                data.branchlist = _branchange.ClgMasterBranchDMO.Where(c => c.MI_Id == data.MI_Id && c.AMB_ActiveFlag == true).ToList().Distinct().ToArray();
                data.semisterslist = _branchange.CLG_Adm_Master_SemesterDMO.Where(c => c.MI_Id == data.MI_Id && c.AMSE_ActiveFlg == true).ToList().Distinct().ToArray();
                data.sectionslist = _branchange.Adm_College_Master_SectionDMO.Where(t => t.MI_Id == data.MI_Id && t.ACMS_ActiveFlag == true).ToList().ToArray();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Admission_Branch_Changes";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToInt32(data.MI_Id) });

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
                        data.datalist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return data;
        }
        public BranchChangeDTO Studentdetails(BranchChangeDTO data)
        {
            try
            {
                data.studlist = (from a in _branchange.Adm_Master_College_StudentDMO
                                 from b in _branchange.Adm_College_Yearly_StudentDMO
                                 where (a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id
                                 && b.ASMAY_Id == data.ASMAY_Id && b.AMSE_Id == data.AMSE_Id && a.AMCST_ActiveFlag == true
                                 && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1 && b.ACMS_Id==data.ACMS_Id_Old)
                                 select new BranchChangeDTO
                                 {
                                     AMCST_Id = a.AMCST_Id,
                                     AMCST_Name = (((a.AMCST_FirstName == null || a.AMCST_FirstName.Trim() == "") ? "" : a.AMCST_FirstName.Trim()) + ((a.AMCST_MiddleName == null || a.AMCST_MiddleName.Trim() == "" || a.AMCST_MiddleName.Trim() == "0") ? "" : " " + a.AMCST_MiddleName.Trim()) + ((a.AMCST_LastName == null || a.AMCST_LastName.Trim() == "" || a.AMCST_LastName.Trim() == "0") ? "" : " " + a.AMCST_LastName.Trim())).Trim(),
                                     AMCST_RegistrationNo = a.AMCST_RegistrationNo

                                 }).Distinct().OrderBy(t => t.AMCST_RegistrationNo).ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            return data;
        }
        public BranchChangeDTO Savedetails(BranchChangeDTO data)
        {
            try
            {
                var result = (from t in _branchange.BranchChangeDMO
                              where (t.MI_Id == data.MI_Id && t.AMCO_Id == data.AMCO_Id && t.AMB_Id == data.AMB_Id
                              && t.ACSCOB_OldRegNo == data.ACSCOB_OldRegNo && t.ACSCOB_AMB_Id == data.ACSCOB_AMB_Id && t.ACSCOB_NewRegNo == data.ACSCOB_NewRegNo && t.AMCST_Id == data.AMCST_Id && t.ASMAY_Id == data.ASMAY_Id)
                              select new { }).Count();
                if (result > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {
                    var check_admission = _branchange.Adm_Master_College_StudentDMO.Single(a => a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id);
                    check_admission.AMCST_RegistrationNo = data.ACSCOB_NewRegNo;
                    check_admission.UpdatedDate = DateTime.Now;
                    _branchange.Update(check_admission);

                    var check_yearlystudent = _branchange.Adm_College_Yearly_StudentDMO.Single(a => a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id
                    && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id_Old && a.ACMS_Id == data.ACMS_Id_Old && a.ACYST_ActiveFlag==1);

                    check_yearlystudent.ACYST_ActiveFlag = 0;
                    check_yearlystudent.UpdatedDate = DateTime.Now;
                    _branchange.Update(check_yearlystudent);

                    Adm_College_Yearly_StudentDMO dmo = new Adm_College_Yearly_StudentDMO();

                    var resultnew = (from m in _branchange.Adm_Master_College_StudentDMO
                                     from n in _branchange.Adm_College_Yearly_StudentDMO
                                     where (m.AMCST_Id == n.AMCST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id
                                     && n.AMB_Id == data.ACSCOB_AMB_Id && n.AMCO_Id == data.AMCO_Id
                                     && n.AMSE_Id == data.AMSE_Id_New
                                     && n.ACMS_Id == data.ACMS_Id_New)
                                     select new ClgYearWiseStudentDTO
                                     {
                                         AMCST_Id = n.AMCST_Id,
                                         ACYST_RollNo = n.ACYST_RollNo
                                     }).ToList();

                    int i = 0;
                    i = resultnew.Count() + 1;

                    dmo.AMCST_Id = data.AMCST_Id;
                    dmo.ASMAY_Id = data.ASMAY_Id;
                    dmo.AMCO_Id = data.AMCO_Id;
                    dmo.AMB_Id = data.ACSCOB_AMB_Id;
                    dmo.AMSE_Id = data.AMSE_Id_New;
                    dmo.ACMS_Id = data.ACMS_Id_New;
                    dmo.ACYST_RollNo = i;
                    dmo.AYST_PassFailFlag = true;
                    dmo.LoginId = data.userid;
                    dmo.ACYST_DateTime = DateTime.Now;
                    dmo.ACYST_ActiveFlag = 1;
                    dmo.CreatedDate = DateTime.Now;
                    dmo.UpdatedDate = DateTime.Now;

                    _branchange.Add(dmo);

                    BranchChangeDMO obj_ccs = new BranchChangeDMO();
                    obj_ccs.ACSCOB_UpdatedBy = data.userid.ToString();
                    obj_ccs.ACSCOB_CreatedBy = data.userid.ToString();
                    obj_ccs.MI_Id = Convert.ToInt64(data.MI_Id);
                    obj_ccs.ACSCOB_OldRegNo = data.ACSCOB_OldRegNo;
                    obj_ccs.ACSCOB_NewRegNo = data.ACSCOB_NewRegNo;
                    obj_ccs.AMCO_Id = data.AMCO_Id;
                    obj_ccs.AMB_Id = data.AMB_Id;
                    obj_ccs.ASMAY_Id = data.ASMAY_Id;
                    obj_ccs.AMCST_Id = data.AMCST_Id;
                    obj_ccs.ACSCOB_AMB_Id = data.ACSCOB_AMB_Id;
                    obj_ccs.ACSCOB_AMSE_Id_New = data.AMSE_Id_New;
                    obj_ccs.ACSCOB_AMSE_Id_Old = data.AMSE_Id_Old;

                    obj_ccs.ACSCOB_COBFees = data.ACSCOB_COBFees;
                    obj_ccs.ACSCOB_Remarks = data.ACSCOB_Remarks;
                    obj_ccs.ACSCOB_ActiveFlag = true;
                    obj_ccs.CreatedDate = DateTime.Now;
                    obj_ccs.UpdatedDate = DateTime.Now;
                    obj_ccs.ACSCOB_COBDate = data.ACSCOB_COBDate;
                    _branchange.Add(obj_ccs);

                    var contactExists = _branchange.SaveChanges();
                    if (contactExists >0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public BranchChangeDTO deactive(BranchChangeDTO acd)
        {
            try
            {
                BranchChangeDMO pge = Mapper.Map<BranchChangeDMO>(acd);
                if (pge.ACSCOB_Id > 0)
                {
                    var result = _branchange.BranchChangeDMO.Single(t => t.ACSCOB_Id.Equals(pge.ACSCOB_Id));
                    if (result.ACSCOB_ActiveFlag.Equals(true))
                    {
                        result.ACSCOB_ActiveFlag = false;
                    }
                    else
                    {
                        result.ACSCOB_ActiveFlag = true;
                    }
                    _branchange.Update(result);
                    var flag = _branchange.SaveChanges();
                    if (flag.Equals(1))
                    {
                        acd.returnval = true;
                    }
                    else
                    {
                        acd.returnval = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }
    }
}
