using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.College.Admission;


namespace CollegeServiceHub.Impl
{
    public class CollegeDocumentReportImpl : Interface.CollegeDocumentReportInterface
    {
        public ClgAdmissionContext _clgadmctxt;
        public CollegeDocumentReportImpl(ClgAdmissionContext n)
        {
            _clgadmctxt = n;
        }
        public CollegeDocumentReportDTO getdetails(CollegeDocumentReportDTO data)
        {
            try
            {
                data.getyear = _clgadmctxt.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.getdocument = _clgadmctxt.MasterDocumentDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeDocumentReportDTO onchangeyear(CollegeDocumentReportDTO data)
        {
            try
            {
                data.getcourse = (from a in _clgadmctxt.MasterCourseDMO
                                  from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                  where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                  && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                  select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeDocumentReportDTO onchangecourse(CollegeDocumentReportDTO data)
        {
            try
            {
                data.getbranch = (from a in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                  from b in _clgadmctxt.MasterCourseDMO
                                  from c in _clgadmctxt.AcademicYear
                                  from d in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                  from e in _clgadmctxt.ClgMasterBranchDMO
                                  where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_Id == d.ACAYC_Id && d.AMB_Id == e.AMB_Id
                                  && a.ACAYC_ActiveFlag == true && a.MI_Id == data.MI_Id && b.AMCO_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id
                                  && d.ACAYCB_ActiveFlag == true && e.AMB_ActiveFlag == true && a.AMCO_Id == data.AMCO_Id)
                                  select e).Distinct().OrderBy(a => a.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeDocumentReportDTO onchangebranch(CollegeDocumentReportDTO data)
        {
            try
            {
                var semisterlist = (from a in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                    from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                    from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _clgadmctxt.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                    && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id
                                    && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id
                                    && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                    select a).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();

                data.getsemester = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeDocumentReportDTO onchangesemester(CollegeDocumentReportDTO data)
        {
            try
            {
                data.getsection = _clgadmctxt.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag == true).Distinct().OrderBy(t => t.ACMS_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeDocumentReportDTO onchangesection(CollegeDocumentReportDTO data)
        {
            try
            {
                data.getstudent = (from a in _clgadmctxt.Adm_College_Yearly_StudentDMO
                                   from b in _clgadmctxt.Adm_Master_College_StudentDMO
                                   from c in _clgadmctxt.MasterCourseDMO
                                   from d in _clgadmctxt.ClgMasterBranchDMO
                                   from e in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                   from f in _clgadmctxt.Adm_College_Master_SectionDMO
                                   where (a.AMCST_Id == b.AMCST_Id && a.AMCO_Id == c.AMCO_Id && a.AMB_Id == d.AMB_Id && a.AMSE_Id == e.AMSE_Id && a.ACMS_Id == f.ACMS_Id
                                   && b.AMCST_SOL == "S" && b.AMCST_ActiveFlag == true && a.ACYST_ActiveFlag == 1 && a.ASMAY_Id == data.ASMAY_Id
                                   && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACMS_Id == data.ACMS_Id)
                                   select new CollegeDocumentReportDTO
                                   {
                                       AMCST_Id = a.AMCST_Id,
                                       studentname = ((b.AMCST_FirstName == null ? "" : b.AMCST_FirstName) +
                                       (b.AMCST_MiddleName == null || b.AMCST_MiddleName == "" ? "" : " " + b.AMCST_MiddleName) +
                                       (b.AMCST_LastName == null || b.AMCST_LastName == "" ? "" : " " + b.AMCST_LastName) +
                                       (b.AMCST_AdmNo == null ? "" : " : " + b.AMCST_AdmNo)),

                                   }).Distinct().OrderBy(a => a.studentname).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeDocumentReportDTO getreportdetails(CollegeDocumentReportDTO data)
        {
            try
            {
                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Student_Document_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = data.AMB_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = data.AMSE_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = data.ACMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = data.STDORDOC });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar) { Value = data.AMCST_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMSMD_Id", SqlDbType.VarChar) { Value = data.AMSMD_Id });
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
                        data.getreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public CollegeDocumentReportDTO getdetails_view(CollegeDocumentReportDTO miid)
        {
            CollegeDocumentReportDTO ctdo = new CollegeDocumentReportDTO();
            try
            {
                ctdo.getyear = _clgadmctxt.AcademicYear.Where(t => t.MI_Id == miid.MI_Id && t.Is_Active == true).OrderByDescending(d => d.ASMAY_Order).ToArray();

                ctdo.getcourse = _clgadmctxt.MasterCourseDMO.Where(c => c.MI_Id == miid.MI_Id && c.AMCO_ActiveFlag == true).ToArray();

            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }


        public CollegeDocumentReportDTO getclgstudata_view(CollegeDocumentReportDTO miid)
        {
            CollegeDocumentReportDTO ctdo = new CollegeDocumentReportDTO();
            List<CollegeDocumentReportDTO> alldocList = new List<CollegeDocumentReportDTO>();
            List<CollegeDocumentReportDTO> alldocListmain = new List<CollegeDocumentReportDTO>();
            try
            {


                //
                List<long> amb_ids = new List<long>();

                if (miid.branchlisttwo.Length > 0)
                {
                    foreach (var ue in miid.branchlisttwo)
                    {
                        amb_ids.Add(ue.AMB_Id);
                    }

                }

                List<long> amco_ids = new List<long>();
                if (miid.courselsttwo.Length > 0)
                {
                    foreach (var ue in miid.courselsttwo)
                    {
                        amco_ids.Add(ue.AMCO_Id);
                    }

                }

                List<long> amse_ids = new List<long>();
                if (miid.semesterlisttwo.Length > 0)
                {
                    foreach (var ue in miid.semesterlisttwo)
                    {
                        amse_ids.Add(ue.AMSE_Id);
                    }

                }
                //

                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _clgadmctxt.AcademicYear.Where(t => t.MI_Id == miid.MI_Id && t.Is_Active == true).OrderByDescending(d => d.ASMAY_Order).ToList();
                ctdo.getyear = year.ToArray();

                ctdo.admissioncatdrp = _clgadmctxt.MasterCourseDMO.Where(c => c.MI_Id == miid.MI_Id && c.AMCO_ActiveFlag == true).ToArray();

                ctdo.admissioncatdrpall = _clgadmctxt.MasterCourseDMO.Where(c => c.MI_Id == miid.MI_Id && c.AMCO_ActiveFlag == true).ToArray();


                List<long> temparr = new List<long>();

                List<Adm_Master_College_StudentDMO> allRegStudentmain = new List<Adm_Master_College_StudentDMO>();

                //added
                if (amco_ids.Count == 0 && amb_ids.Count == 0 && amse_ids.Count == 0)
                {

                    allRegStudentmain = _clgadmctxt.Adm_Master_College_StudentDMO.Where(d => d.MI_Id.Equals(miid.MI_Id) && d.ASMAY_Id == miid.ASMAY_Id).ToList();
                }
                else
                {
                    allRegStudentmain = _clgadmctxt.Adm_Master_College_StudentDMO.Where(d => d.MI_Id.Equals(miid.MI_Id) && d.ASMAY_Id == miid.ASMAY_Id && amco_ids.Contains(d.AMCO_Id) && amb_ids.Contains(d.AMB_Id) && amse_ids.Contains(d.AMSE_Id)).ToList();
                }
                //
                for (int i = 0; i < allRegStudentmain.Count; i++)
                {
                    temparr.Add(allRegStudentmain[i].AMCST_Id);
                }

                ctdo.registrationList = allRegStudentmain.ToArray();
                // ctdo.prospectusPaymentlist = _feecontext.Fee_Y_Payment_PA_Application.Where(t => t.FYPPA_Type == "R" && temparr.Contains(t.PACA_Id)).ToArray();


                ctdo.ddoc = (from a in _clgadmctxt.AdmCollegeStudentDocumentsDMO
                             from b in _clgadmctxt.MasterDocumentDMO
                             from c in _clgadmctxt.Adm_Master_College_StudentDMO
                             where (a.ACSMD_Id == b.AMSMD_Id && a.AMCST_Id == c.AMCST_Id && c.ASMAY_Id == miid.ASMAY_Id && c.MI_Id == miid.MI_Id && temparr.Contains(c.AMCST_Id))
                             select new CollegeDocumentReportDTO
                             {
                                 AMCST_Id = c.AMCST_Id,
                                 ACSTD_Id = a.ACSTD_Id,
                                 ACSTD_Doc_Path = a.ACSTD_Doc_Path,
                                 AMSMD_DocumentName = b.AMSMD_DocumentName,
                                 ACSMD_Id = a.ACSMD_Id
                             }
                      ).ToArray();

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }
    }
}
