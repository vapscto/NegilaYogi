
using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.Exam;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using Microsoft.Extensions.Logging;

namespace ExamServiceHub.com.vaps.Services
{
    public class ExamLoginPrivilagesImpl : Interfaces.ExamLoginPrivilagesInterface
    {
        private static ConcurrentDictionary<string, Exm_Login_PrivilegeDTO> _login =
         new ConcurrentDictionary<string, Exm_Login_PrivilegeDTO>();

        public ExamContext _studentmappingContext;
        ILogger<ExamLoginPrivilagesImpl> _acdimpl;
        public ExamLoginPrivilagesImpl(ExamContext studentmappingContext, ILogger<ExamLoginPrivilagesImpl> _acdim)
        {
            _studentmappingContext = studentmappingContext;
            _acdimpl = _acdim;
        }

        public Exm_Login_PrivilegeDTO Getdetails(Exm_Login_PrivilegeDTO data)//int IVRMM_Id
        {
            Exm_Login_PrivilegeDTO getdata = new Exm_Login_PrivilegeDTO();
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _studentmappingContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                getdata.yearlist = list.ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = _studentmappingContext.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).OrderBy(t => t.ASMC_Order).ToList();
                getdata.seclist = seclist.ToArray();

                List<AdmissionClass> admlist = new List<AdmissionClass>();
                admlist = _studentmappingContext.AdmissionClass.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).OrderBy(t => t.ASMCL_Order).ToList();
                getdata.classlist = admlist.ToArray();

                List<IVRM_School_Master_SubjectsDMO> mastersub = new List<IVRM_School_Master_SubjectsDMO>();
                mastersub = _studentmappingContext.IVRM_School_Master_SubjectsDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMS_ExamFlag == 1).OrderBy(t => t.ISMS_OrderFlag).ToList();
                getdata.subjlist = mastersub.ToArray();

                List<mastersubsubjectDMO> ssubj = new List<mastersubsubjectDMO>();
                ssubj = _studentmappingContext.mastersubsubject.Where(t => t.MI_Id == data.MI_Id && t.EMSS_ActiveFlag == true).OrderBy(t => t.EMSS_Order).ToList();
                getdata.subsubject = ssubj.ToArray();


                getdata.emplist = (from a in _studentmappingContext.Staff_User_Login
                                   from b in _studentmappingContext.HR_Master_Employee_DMO
                                   where (a.Emp_Code == b.HRME_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                   select new Exm_Login_PrivilegeDTO
                                   {
                                       HRME_Id = b.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName) + " : " + (b.HRME_EmployeeCode == null || b.HRME_EmployeeCode == "" ? "" : " " + b.HRME_EmployeeCode)).Trim(),
                                   }).Distinct().ToArray();

                getdata.userlist = (from a in _studentmappingContext.Staff_User_Login
                                    from b in _studentmappingContext.HR_Master_Employee_DMO
                                    where (a.Emp_Code == b.HRME_Id && a.MI_Id == data.MI_Id)
                                    select new Exm_Login_PrivilegeDTO
                                    {
                                        IVRMULF_Id = a.IVRMSTAUL_Id,
                                        HRME_EmployeeFirstName = ((a.IVRMSTAUL_UserName == null ? " " : a.IVRMSTAUL_UserName) + " : " + (b.HRME_EmployeeCode == null || b.HRME_EmployeeCode == "" ? " " : " " + b.HRME_EmployeeCode)),
                                        Emp_Code = a.Emp_Code
                                    }).Distinct().ToArray();


                getdata.pllist = (from a in _studentmappingContext.Exm_Login_PrivilegeDMO
                                  from b in _studentmappingContext.AcademicYear
                                  from c in _studentmappingContext.Staff_User_Login
                                  from d in _studentmappingContext.HR_Master_Employee_DMO
                                  from e in _studentmappingContext.Exm_Login_Privilege_SubjectsDMO
                                  from f in _studentmappingContext.AdmissionClass
                                  from g in _studentmappingContext.School_M_Section
                                  where (a.ASMAY_Id == b.ASMAY_Id && a.Login_Id == c.IVRMSTAUL_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && c.Emp_Code == d.HRME_Id && e.ELP_Id == a.ELP_Id && e.ASMCL_Id == f.ASMCL_Id && f.MI_Id == a.MI_Id && a.MI_Id == g.MI_Id && e.ASMS_Id == g.ASMS_Id)
                                  select new Exm_Login_PrivilegeDTO
                                  {
                                      ELP_Id = a.ELP_Id,
                                      ASMAY_Year = b.ASMAY_Year,
                                      IVRMULF_Id = c.IVRMSTAUL_Id,

                                      HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                      ELP_ActiveFlg = a.ELP_ActiveFlg,
                                      ASMCL_Id = f.ASMCL_Id,
                                      ASMCL_ClassName = f.ASMCL_ClassName,
                                      ASMS_Id = e.ASMS_Id,
                                      ASMC_SectionName = g.ASMC_SectionName,
                                      ASMAY_Order = b.ASMAY_Order

                                  }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();


                List<ClassTeacherMappingDMO> clstech = new List<ClassTeacherMappingDMO>();
                clstech = _studentmappingContext.ClassTeacherMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.IMCT_ActiveFlag == true).ToList();
                getdata.clastechlt = clstech.GroupBy(t => t.HRME_Id).Select(d => d.FirstOrDefault().HRME_Id).ToArray();


            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return getdata;

        }
        public Exm_Login_PrivilegeDTO getcategory(Exm_Login_PrivilegeDTO data)
        {
            Exm_Login_PrivilegeDTO getdata = new Exm_Login_PrivilegeDTO();
            try
            {
                //    getdata.ctlist = (from a in _studentmappingContext.Exm_Yearly_CategoryDMO
                //                      from b in _studentmappingContext.Exm_Master_CategoryDMO
                //                      where (a.EMCA_Id==b.EMCA_Id && a.MI_Id==b.MI_Id && a.MI_Id==data.MI_Id && a.ASMAY_Id==data.ASMAY_Id && a.EYC_ActiveFlg==true)
                //                        select new Exm_Login_PrivilegeDTO
                //                        {
                //                            EMCA_Id = b.EMCA_Id,
                //                            EMCA_CategoryName= b.EMCA_CategoryName

                //                        }).ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return getdata;

        }
        public Exm_Login_PrivilegeDTO getclassid(Exm_Login_PrivilegeDTO data)
        {
            Exm_Login_PrivilegeDTO getdata = new Exm_Login_PrivilegeDTO();
            try
            {
                //    getdata.classlist = (from a in _studentmappingContext.Exm_Category_ClassDMO
                //                         from b in _studentmappingContext.AdmissionClass
                //                         from c in _studentmappingContext.Exm_Master_CategoryDMO
                //                         where (a.EMCA_Id==c.EMCA_Id && a.EMCA_Id == data.EMCA_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id==c.MI_Id && b.MI_Id==data.MI_Id)
                //                         select new Exm_Login_PrivilegeDTO
                //                         {
                //                             ASMCL_Id = a.ASMCL_Id,
                //                             ASMCL_ClassName = b.ASMCL_ClassName

                //                         }).ToArray();

                //    getdata.grouplist= (from a in _studentmappingContext.Exm_Yearly_CategoryDMO
                //                        from b in _studentmappingContext.Exm_Yearly_Category_GroupDMO
                //                        from c in _studentmappingContext.Exm_Master_GroupDMO
                //                        where (a.EYC_Id==b.EYC_Id && a.EMCA_Id==data.EMCA_Id && b.EMG_Id==c.EMG_Id && a.MI_Id==c.MI_Id && c.EMG_ElectiveFlg==true)
                //                         select new Exm_Login_PrivilegeDTO
                //                         {
                //                           EMG_Id= c.EMG_Id,
                //                           EMG_GroupName= c.EMG_GroupName

                //                         }).ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return getdata;
        }
        public Exm_Login_PrivilegeDTO getclstechdetails(Exm_Login_PrivilegeDTO data)
        {
            Exm_Login_PrivilegeDTO getdata = new Exm_Login_PrivilegeDTO();
            try
            {
                List<ClassTeacherMappingDMO> clstech = new List<ClassTeacherMappingDMO>();
                clstech = _studentmappingContext.ClassTeacherMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == data.HRME_Id
                && t.IMCT_ActiveFlag == true && t.ASMAY_Id == data.ASMAY_Id).ToList();
                getdata.clastechlt = clstech.ToArray();              
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return getdata;
        }
        public Exm_Login_PrivilegeDTO Studentdetails(Exm_Login_PrivilegeDTO data)
        {
            Exm_Login_PrivilegeDTO getdata = new Exm_Login_PrivilegeDTO();
            try
            {

                //getdata.studlist = (from a in _studentmappingContext.School_Adm_Y_Student
                //                    from b in _studentmappingContext.AdmissionClass
                //                    from c in _studentmappingContext.School_M_Section
                //                    from d in _studentmappingContext.AcademicYear
                //                    from e in _studentmappingContext.Adm_M_Student
                //                    where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == c.ASMS_Id && a.ASMAY_Id == d.ASMAY_Id && a.AMST_Id == e.AMST_Id &&
                //                    a.ASMCL_Id ==data.ASMCL_Id && a.ASMS_Id ==data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id)
                //                    select new Exm_Login_PrivilegeDTO
                //                    {
                //                        AMST_Id = a.AMST_Id,
                //                        AMST_FirstName= e.AMST_FirstName
                //                    }).ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return getdata;
        }
        
        public Exm_Login_PrivilegeDTO savedetails(Exm_Login_PrivilegeDTO data)
        {
           // var Duplicate_Count = 0;

            try
            {

                if (data.selectedclass.Length > 0)
                {
                    for (int i = 0; i < data.selectedclass.Count(); i++)
                    {                    
                        var tempyrid = data.selectedclass[i].yearid;
                        var templogid = data.selectedclass[i].user_id;
                        var tempelpflg = data.selectedclass[i].elpflg;

                        var rlst = _studentmappingContext.Exm_Login_PrivilegeDMO.Where(t => t.ASMAY_Id == tempyrid && t.Login_Id == templogid && t.MI_Id == data.MI_Id && t.ELP_ActiveFlg == true && t.ELP_Flg == tempelpflg).ToList();

                        if (rlst.Count == 1)
                        {
                            List<Exm_Login_Privilege_SubjectsDMO> subrslt = new List<Exm_Login_Privilege_SubjectsDMO>();
                            subrslt = _studentmappingContext.Exm_Login_Privilege_SubjectsDMO.Where(t => t.ELP_Id == rlst[0].ELP_Id && t.ASMS_Id == data.selectedclass[i].secs.ASMS_Id && t.ASMCL_Id == data.selectedclass[i].clas.ASMCL_Id).ToList();

                            if (subrslt.Any())
                            {
                                if (data.action == "add")
                                {
                                    var rmvdup = data.selectedclass[i].sub.ToList();
                                    for (int z = 0; z < subrslt.Count; z++)
                                    {
                                        for (int cc = 0; cc < rmvdup.Count; cc++)
                                        {
                                            if (rmvdup[cc].ISMS_Id == subrslt[z].ISMS_Id)
                                            {
                                                subrslt[z].ELPs_ActiveFlg = false;
                                                subrslt[z].UpdatedDate = DateTime.Now;
                                                _studentmappingContext.Update(subrslt[z]);
                                                var subs = _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO.Where(t => t.ELPS_Id == subrslt[z].ELPS_Id).ToList();
                                                if (subs.Count > 0)
                                                {
                                                    for (int w = 0; w < subs.Count; w++)
                                                    {

                                                        subs[w].ELPSS_ActiveFlg = false;
                                                        subs[w].UpdatedDate = DateTime.Now;
                                                        _studentmappingContext.Update(subs[w]);

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (data.action == "edit")
                                {
                                    for (int z = 0; z < subrslt.Count; z++)
                                    {
                                        subrslt[z].ELPs_ActiveFlg = false;
                                        subrslt[z].UpdatedDate = DateTime.Now;
                                        _studentmappingContext.Update(subrslt[z]);

                                        var subs = _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO.Where(t => t.ELPS_Id == subrslt[z].ELPS_Id).ToList();
                                        if (subs.Count > 0)
                                        {
                                            for (int w = 0; w < subs.Count; w++)
                                            {

                                                subs[w].ELPSS_ActiveFlg = false;
                                                subs[w].UpdatedDate = DateTime.Now;
                                                _studentmappingContext.Update(subs[w]);

                                            }
                                        }
                                    }
                                }
                                var sss = data.selectedclass[i].sub.ToList();
                                for (var l = 0; l < sss.Count; l++)
                                {
                                    var al_sub_cnt = 0;
                                    for (int z = 0; z < subrslt.Count; z++)
                                    {
                                        if (sss[l].ISMS_Id == subrslt[z].ISMS_Id)
                                        {
                                            al_sub_cnt += 1;
                                            subrslt[z].ELPs_ActiveFlg = true;
                                            subrslt[z].UpdatedDate = DateTime.Now;
                                            _studentmappingContext.Update(subrslt[z]);
                                            //  _studentmappingContext.SaveChanges();
                                            var subsubect = _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO.Where(t => t.ELPS_Id == subrslt[z].ELPS_Id).ToList();

                                            if (subsubect.Any())
                                            {

                                                if (data.action == "add")
                                                {
                                                    var rmvdupsubsub = data.selectedclass[i].ssub.ToList();
                                                    for (int r = 0; r < subsubect.Count; r++)
                                                    {
                                                        for (int ssb = 0; ssb < rmvdupsubsub.Count; ssb++)
                                                        {

                                                            if (rmvdupsubsub[ssb].EMSS_Id == subsubect[r].EMSS_Id)
                                                            {
                                                                subsubect[r].ELPSS_ActiveFlg = false;
                                                                subsubect[r].UpdatedDate = DateTime.Now;
                                                                _studentmappingContext.Update(subsubect[r]);
                                                            }
                                                        }

                                                    }
                                                }


                                                if (data.action == "edit")
                                                {
                                                    for (int r = 0; r < subsubect.Count; r++)
                                                    {



                                                        subsubect[r].ELPSS_ActiveFlg = false;
                                                        subsubect[r].UpdatedDate = DateTime.Now;
                                                        _studentmappingContext.Update(subsubect[r]);


                                                    }
                                                }


                                                var editsubsub = data.selectedclass[i].ssub.ToList();
                                                if (editsubsub.Count > 0)
                                                {
                                                    for (int d = 0; d < editsubsub.Count; d++)
                                                    {
                                                        var subsub_cnt = 0;
                                                        for (int p = 0; p < subsubect.Count; p++)
                                                        {
                                                            if (editsubsub[d].EMSS_Id == subsubect[p].EMSS_Id)
                                                            {
                                                                subsub_cnt += 1;
                                                                subsubect[p].ELPSS_ActiveFlg = true;
                                                                subsubect[p].UpdatedDate = DateTime.Now;
                                                                _studentmappingContext.Update(subsubect[p]);
                                                                // _studentmappingContext.SaveChanges();
                                                            }

                                                        }
                                                        if (subsub_cnt == 0)
                                                        {
                                                            Exm_Login_Privilege_SubSubjectsDMO obj_ssub = new Exm_Login_Privilege_SubSubjectsDMO();
                                                            obj_ssub.ELPS_Id = subrslt[z].ELPS_Id;
                                                            obj_ssub.EMSS_Id = editsubsub[d].EMSS_Id;
                                                            obj_ssub.ELPSS_ActiveFlg = true;
                                                            obj_ssub.CreatedDate = DateTime.Now;
                                                            obj_ssub.UpdatedDate = DateTime.Now;
                                                            _studentmappingContext.Add(obj_ssub);
                                                            // _studentmappingContext.SaveChanges();
                                                        }

                                                    }
                                                }






                                                // }

                                            }
                                            else
                                            {
                                                var editsubsub_sa = data.selectedclass[i].ssub.ToList();
                                                if (editsubsub_sa.Count > 0)
                                                {
                                                    foreach (var t in editsubsub_sa)
                                                    {
                                                        Exm_Login_Privilege_SubSubjectsDMO obj_ssub = new Exm_Login_Privilege_SubSubjectsDMO();
                                                        obj_ssub.ELPS_Id = subrslt[z].ELPS_Id;
                                                        obj_ssub.EMSS_Id = t.EMSS_Id;
                                                        obj_ssub.ELPSS_ActiveFlg = true;
                                                        obj_ssub.CreatedDate = DateTime.Now;
                                                        obj_ssub.UpdatedDate = DateTime.Now;
                                                        _studentmappingContext.Add(obj_ssub);
                                                    }
                                                }


                                            }
                                        }
                                    }
                                    if (al_sub_cnt == 0)
                                    {
                                        Exm_Login_Privilege_SubjectsDMO obj_s11 = new Exm_Login_Privilege_SubjectsDMO();
                                        obj_s11.ELP_Id = rlst[0].ELP_Id;
                                        obj_s11.ASMCL_Id = data.selectedclass[i].clas.ASMCL_Id;
                                        obj_s11.ASMS_Id = data.selectedclass[i].secs.ASMS_Id;
                                        obj_s11.ISMS_Id = sss[l].ISMS_Id;
                                        obj_s11.ELPs_ActiveFlg = true;
                                        obj_s11.CreatedDate = DateTime.Now;
                                        obj_s11.UpdatedDate = DateTime.Now;
                                        _studentmappingContext.Add(obj_s11);
                                        // _studentmappingContext.SaveChanges();
                                        if (data.selectedclass[i].ssub.Length > 0)
                                        {
                                            foreach (var yy in data.selectedclass[i].ssub)
                                            {
                                                Exm_Login_Privilege_SubSubjectsDMO loginpss1 = new Exm_Login_Privilege_SubSubjectsDMO();
                                                loginpss1.ELPS_Id = obj_s11.ELPS_Id;
                                                loginpss1.EMSS_Id = yy.EMSS_Id;
                                                loginpss1.ELPSS_ActiveFlg = true;
                                                loginpss1.CreatedDate = DateTime.Now;
                                                loginpss1.UpdatedDate = DateTime.Now;
                                                _studentmappingContext.Add(loginpss1);
                                            }

                                        }

                                    }








                                    //_studentmappingContext.SaveChanges();
                                }
                                // }


                                //_studentmappingContext.SaveChanges();
                            }
                            else
                            {
                                //  var sss = data.selectedclass[i].sub.ToList();
                                foreach (var x in data.selectedclass[i].sub)
                                {
                                    Exm_Login_Privilege_SubjectsDMO loginps = new Exm_Login_Privilege_SubjectsDMO();
                                    loginps.ELP_Id = rlst[0].ELP_Id;
                                    loginps.ASMCL_Id = data.selectedclass[i].clas.ASMCL_Id;
                                    loginps.ASMS_Id = data.selectedclass[i].secs.ASMS_Id;
                                    loginps.ISMS_Id = x.ISMS_Id;
                                    loginps.ELPs_ActiveFlg = true;
                                    loginps.CreatedDate = DateTime.Now;
                                    loginps.UpdatedDate = DateTime.Now;
                                    _studentmappingContext.Add(loginps);
                                    if (data.selectedclass[i].ssub.Length > 0)
                                    {
                                        foreach (var y in data.selectedclass[i].ssub)
                                        {
                                            Exm_Login_Privilege_SubSubjectsDMO loginpss = new Exm_Login_Privilege_SubSubjectsDMO();
                                            loginpss.ELPS_Id = loginps.ELPS_Id;
                                            loginpss.EMSS_Id = y.EMSS_Id;
                                            loginpss.ELPSS_ActiveFlg = true;
                                            loginpss.CreatedDate = DateTime.Now;
                                            loginpss.UpdatedDate = DateTime.Now;
                                            _studentmappingContext.Add(loginpss);
                                        }

                                    }
                                }
                            }


                            //        var editsubsub= data.selectedclass[i].ssub.ToList();
                            //                if (editsubsub.Count>0)
                            //                {
                            //                    for (int p = 0; p < subsubect.Count; p++)
                            //                    {
                            //                        var subsub_cnt = 0;
                            //                        for (int d = 0; d < editsubsub.Count; d++)
                            //                        {
                            //                            if (editsubsub[d].EMSS_Id==subsubect[p].EMSS_Id)
                            //                            {
                            //                                subsub_cnt += 1;
                            //                                subsubect[p].ELPSS_ActiveFlg = true;
                            //                                subsubect[p].UpdatedDate = DateTime.Now;
                            //                                _studentmappingContext.Update(subsubect[p]);
                            //                               // _studentmappingContext.SaveChanges();
                            //                            }

                            //                        }
                            //                        if (subsub_cnt==0)
                            //                        {
                            //                            Exm_Login_Privilege_SubSubjectsDMO obj_ssub = new Exm_Login_Privilege_SubSubjectsDMO();
                            //                            obj_ssub.ELPS_Id = subrslt[z].ELPS_Id;
                            //                            obj_ssub.EMSS_Id = subsubect[p].EMSS_Id;
                            //                            obj_ssub.CreatedDate = DateTime.Now;
                            //                            obj_ssub.UpdatedDate = DateTime.Now;
                            //                            _studentmappingContext.Add(obj_ssub);
                            //                           // _studentmappingContext.SaveChanges();
                            //                        }

                            //                    }
                            //                }






                            //        }

                            //    }
                            //    if (al_sub_cnt == 0)
                            //    {
                            //        Exm_Login_Privilege_SubjectsDMO obj_s = new Exm_Login_Privilege_SubjectsDMO();
                            //        obj_s.ELP_Id = rlst[0].ELP_Id;
                            //        obj_s.ASMCL_Id = data.selectedclass[i].clas.ASMCL_Id;
                            //        obj_s.ASMS_Id = data.selectedclass[i].secs[0].ASMS_Id;
                            //        obj_s.ISMS_Id = sss[l].ISMS_Id;
                            //        obj_s.ELPs_ActiveFlg = true;
                            //        obj_s.CreatedDate = DateTime.Now;
                            //        obj_s.UpdatedDate = DateTime.Now;
                            //        _studentmappingContext.Add(obj_s);
                            //       // _studentmappingContext.SaveChanges();

                            //    }



                            //}

                            var contextexists = _studentmappingContext.SaveChanges();
                            if (contextexists >= 1)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }

                        }

                        if (rlst.Count == 0)
                        {
                            try
                            {
                                //var tempyrid = data.selectedclass[i].yearid;
                                //var templogid = data.selectedclass[i].user_id;
                                //var tempelpflg = data.selectedclass[i].elpflg;
                                Exm_Login_PrivilegeDMO loginp = new Exm_Login_PrivilegeDMO();
                                loginp.MI_Id = data.MI_Id;
                                loginp.ASMAY_Id = tempyrid;
                                loginp.Login_Id = templogid;
                                loginp.ELP_Flg = tempelpflg;
                                loginp.ELP_ActiveFlg = true;
                                loginp.CreatedDate = DateTime.Now;
                                loginp.UpdatedDate = DateTime.Now;
                                _studentmappingContext.Add(loginp);
                                for (int r = 0; r < data.selectedclass.Length; r++)
                                {
                                    foreach (var x in data.selectedclass[r].sub)
                                    {
                                        Exm_Login_Privilege_SubjectsDMO loginps = new Exm_Login_Privilege_SubjectsDMO();
                                        loginps.ELP_Id = loginp.ELP_Id;
                                        loginps.ASMCL_Id = data.selectedclass[r].clas.ASMCL_Id;
                                        loginps.ASMS_Id = data.selectedclass[r].secs.ASMS_Id;
                                        loginps.ISMS_Id = x.ISMS_Id;
                                        loginps.ELPs_ActiveFlg = true;
                                        loginps.CreatedDate = DateTime.Now;
                                        loginps.UpdatedDate = DateTime.Now;
                                        _studentmappingContext.Add(loginps);
                                        if (data.selectedclass[r].ssub.Length > 0)
                                        {
                                            foreach (var y in data.selectedclass[r].ssub)
                                            {
                                                Exm_Login_Privilege_SubSubjectsDMO loginpss = new Exm_Login_Privilege_SubSubjectsDMO();
                                                loginpss.ELPS_Id = loginps.ELPS_Id;
                                                loginpss.EMSS_Id = y.EMSS_Id;
                                                loginpss.ELPSS_ActiveFlg = true;
                                                loginpss.CreatedDate = DateTime.Now;
                                                loginpss.UpdatedDate = DateTime.Now;
                                                _studentmappingContext.Add(loginpss);
                                            }

                                        }
                                    }
                                }
                                var contextexists = _studentmappingContext.SaveChanges();
                                if (contextexists >= 1)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }
                                //}

                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }


                        //end pp
                        //var result1 = _studentmappingContext.Exm_Login_PrivilegeDMO.Where(t => t.ASMAY_Id == tempyrid && t.Login_Id == templogid && t.MI_Id == data.MI_Id && t.ELP_ActiveFlg == true).ToList();
                        //if (result1.Count > 0)
                        //{
                        //    Duplicate_Count += 1;

                        //    for (int jj = 0; jj < data.selectedclass[i].secs.Count(); jj++)
                        //    {
                        //        for (int kk = 0; kk < data.selectedclass[i].sub.Count(); kk++)
                        //        {
                        //        savedata.returnduplicatestatus = "";
                        //        var Duplicate_Count1 = 0;
                        //            //   tempSubDmo
                        //            Exm_Login_Privilege_SubjectsDMO tempSubDmo = new Exm_Login_Privilege_SubjectsDMO();
                        //            List<Exm_Login_Privilege_SubSubjectsDMO> tempSubSubList = new List<Exm_Login_Privilege_SubSubjectsDMO>();
                        //            tempSubDmo.ELP_Id = result1[0].ELP_Id;
                        //            tempSubDmo.ExamLoginPre = null;
                        //            var tempclass = data.selectedclass[i].clas.ASMCL_Id;
                        //            var tempsec = data.selectedclass[i].secs[jj].ASMS_Id;
                        //            var tempsub = data.selectedclass[i].sub[kk].ISMS_Id;
                        //            var result2 = _studentmappingContext.Exm_Login_Privilege_SubjectsDMO.Where(t => t.ASMCL_Id == tempclass && t.ASMS_Id == tempsec && t.ISMS_Id == tempsub && t.ELP_Id == tempSubDmo.ELP_Id && t.ELPs_ActiveFlg==true).ToList();

                        //            if (result2.Count > 0)
                        //            {
                        //            tempSubDmo = _studentmappingContext.Exm_Login_Privilege_SubjectsDMO.Single(t => t.ASMCL_Id == tempclass && t.ASMS_Id == tempsec && t.ISMS_Id == tempsub && t.ELP_Id == tempSubDmo.ELP_Id && t.ELPs_ActiveFlg == true);
                        //                Duplicate_Count1 += 1;
                        //                //  data.ELPS_Id = result2[0].ELPS_Id;

                        //                for (int ll1 = 0; ll1 < data.selectedclass[i].ssub.Count(); ll1++)
                        //                {
                        //                Exm_Login_Privilege_SubSubjectsDMO tempSSubDmo = new Exm_Login_Privilege_SubSubjectsDMO();
                        //                tempSSubDmo.ExamLoginPresub = null;

                        //                var Duplicate_Count3 = 0;
                        //                savedata.returnduplicatestatus = "";
                        //                    var tempssub = data.selectedclass[i].ssub[ll1].EMSS_Id;
                        //                    tempSSubDmo.ELPS_Id = result2[0].ELPS_Id;
                        //                    var result13 = _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO.Where(t => t.EMSS_Id == tempssub && t.ELPS_Id == tempSSubDmo.ELPS_Id && t.ELPSS_ActiveFlg==true).ToList();
                        //                    if (result13.Count > 0)
                        //                    {
                        //                        Duplicate_Count3 += 1;
                        //                        savedata.returnduplicatestatus = "Duplicate";
                        //                    }
                        //                    if (Duplicate_Count3 == 0)
                        //                    {
                        //                        tempSSubDmo.ELPS_Id = result2[0].ELPS_Id;
                        //                        tempSSubDmo.EMSS_Id = tempssub;
                        //                        tempSSubDmo.CreatedDate = DateTime.Now;
                        //                        tempSSubDmo.UpdatedDate = DateTime.Now;
                        //                    tempSSubDmo.ELPSS_ActiveFlg = true;
                        //                        tempSubSubList.Add(tempSSubDmo);
                        //                    }
                        //                }
                        //                if (savedata.returnduplicatestatus != "Duplicate")
                        //                {

                        //                    tempSubDmo.Exm_Login_Privilege_SubSubjects = tempSubSubList;
                        //                // tempSSubDmo.ExamLoginPresub = tempList321;
                        //                 // _studentmappingContext.Add(tempSubDmo);
                        //              _studentmappingContext.Update(tempSubDmo);

                        //                var contact1Exists = _studentmappingContext.SaveChanges();
                        //                    tempSubSubList = new List<Exm_Login_Privilege_SubSubjectsDMO>();
                        //                    if (contact1Exists >= 1)
                        //                    {
                        //                        savedata.returnval = true;
                        //                    }
                        //                    else
                        //                    {
                        //                        savedata.returnval = false;
                        //                    }
                        //                }
                        //            }
                        //            if (Duplicate_Count1 == 0)
                        //            {
                        //                tempSubDmo.ASMCL_Id = tempclass;
                        //                tempSubDmo.ASMS_Id = tempsec;
                        //                tempSubDmo.ISMS_Id = tempsub;
                        //                tempSubDmo.ELPs_ActiveFlg = true;
                        //                tempSubDmo.CreatedDate = DateTime.Now;
                        //                tempSubDmo.UpdatedDate = DateTime.Now;
                        //                tempSubDmo.ELP_Id = result1[0].ELP_Id;

                        //                for (int ll = 0; ll < data.selectedclass[i].ssub.Count(); ll++)
                        //                {
                        //                    var Duplicate_Count4 = 0;
                        //                savedata.returnduplicatestatus = "";
                        //                    var tempssub = data.selectedclass[i].ssub[ll].EMSS_Id;
                        //                    var result3 = _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO.Where(t => t.EMSS_Id == tempssub && t.ELPS_Id == data.ELPS_Id && t.ELPSS_ActiveFlg==true).ToList();
                        //                    if (result3.Count > 0)
                        //                    {
                        //                        Duplicate_Count4 += 1;
                        //                        savedata.returnduplicatestatus = "Duplicate";
                        //                    }
                        //                    if (Duplicate_Count4 == 0)
                        //                    {
                        //                        Exm_Login_Privilege_SubSubjectsDMO tbsubsub = new Exm_Login_Privilege_SubSubjectsDMO();
                        //                        tbsubsub.EMSS_Id = tempssub;
                        //                        tbsubsub.ELPS_Id = data.ELPS_Id;
                        //                        tbsubsub.CreatedDate = DateTime.Now;
                        //                        tbsubsub.UpdatedDate = DateTime.Now;
                        //                    tbsubsub.ELPSS_ActiveFlg = true;
                        //                    tempSubSubList.Add(tbsubsub);

                        //                    }

                        //                }
                        //                if (savedata.returnduplicatestatus != "Duplicate")
                        //                {
                        //                    tempSubDmo.Exm_Login_Privilege_SubSubjects = tempSubSubList;
                        //                    _studentmappingContext.Add(tempSubDmo);

                        //                    var contact2Exists = _studentmappingContext.SaveChanges();
                        //                    tempSubSubList = new List<Exm_Login_Privilege_SubSubjectsDMO>();
                        //                    if (contact2Exists >= 1)
                        //                    {
                        //                        savedata.returnval = true;
                        //                    }
                        //                    else
                        //                    {
                        //                        savedata.returnval = false;
                        //                    }
                        //                }
                        //            }
                        //        }

                        //    }

                        //}
                        //When no duplicates
                        //if (Duplicate_Count == 0)
                        //{
                        //    Exm_Login_PrivilegeDMO tb1Temp = new Exm_Login_PrivilegeDMO();
                        //    tb1Temp.ASMAY_Id = tempyrid;
                        //    tb1Temp.Login_Id = templogid;
                        //    tb1Temp.MI_Id = data.MI_Id;
                        //    tb1Temp.ELP_ActiveFlg = true;
                        //    tb1Temp.ELP_Flg = tempelpflg;
                        //    tb1Temp.CreatedDate = DateTime.Now;
                        //    tb1Temp.UpdatedDate = DateTime.Now;
                        //    data.ELP_Id = tb1Temp.ELP_Id;

                        //    _studentmappingContext.Add(tb1Temp);

                        //    var contact3Exists = _studentmappingContext.SaveChanges();
                        //    data.ELP_Id = tb1Temp.ELP_Id;

                        //    for (int j = 0; j < data.selectedclass[i].secs.Count(); j++)
                        //    {
                        //        for (int k = 0; k < data.selectedclass[i].sub.Count(); k++)
                        //        {
                        //            Exm_Login_Privilege_SubjectsDMO tempSubDmo = new Exm_Login_Privilege_SubjectsDMO();
                        //            List<Exm_Login_Privilege_SubSubjectsDMO> tempSubSubList = new List<Exm_Login_Privilege_SubSubjectsDMO>();
                        //            var Duplicate_Count4 = 0;
                        //            var tempclass = data.selectedclass[i].clas.ASMCL_Id;
                        //            var tempsec = data.selectedclass[i].secs[j].ASMS_Id;
                        //            var tempsub = data.selectedclass[i].sub[k].ISMS_Id;
                        //            var result2 = _studentmappingContext.Exm_Login_Privilege_SubjectsDMO.Where(t => t.ASMCL_Id == tempclass && t.ASMS_Id == tempsec && t.ISMS_Id == tempsub && t.ELP_Id == data.ELP_Id && t.ELPs_ActiveFlg==true).ToList();

                        //            if (result2.Count > 0)
                        //            {
                        //                Duplicate_Count4 += 1;
                        //                //  data.ELPS_Id = result2[0].ELPS_Id;
                        //                Exm_Login_Privilege_SubSubjectsDMO tempSSubDmo = new Exm_Login_Privilege_SubSubjectsDMO();
                        //                tempSSubDmo.ExamLoginPresub = null;
                        //                for (int ll1 = 0; ll1 < data.selectedclass[i].ssub.Count(); ll1++)
                        //                {
                        //                    var Duplicate_Count5 = 0;
                        //                savedata.returnduplicatestatus = "";
                        //                var tempssub = data.selectedclass[i].ssub[ll1].EMSS_Id;
                        //                    tempSSubDmo.ELPS_Id = result2[0].ELPS_Id;
                        //                    var result3 = _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO.Where(t => t.EMSS_Id == tempssub && t.ELPS_Id == tempSSubDmo.ELPS_Id && t.ELPSS_ActiveFlg==true).ToList();
                        //                    if (result3.Count > 0)
                        //                    {
                        //                        Duplicate_Count5 += 1;
                        //                        savedata.returnduplicatestatus = "Duplicate";
                        //                    }
                        //                    if (Duplicate_Count5 == 0)
                        //                    {
                        //                        tempSSubDmo.EMSS_Id = tempssub;
                        //                        tempSSubDmo.CreatedDate = DateTime.Now;
                        //                        tempSSubDmo.UpdatedDate = DateTime.Now;
                        //                    tempSSubDmo.ELPSS_ActiveFlg = true;
                        //                    tempSubSubList.Add(tempSSubDmo);
                        //                    }
                        //                }
                        //                if (savedata.returnduplicatestatus != "Duplicate")
                        //                {
                        //                    tempSubDmo.Exm_Login_Privilege_SubSubjects = tempSubSubList;
                        //                    // tempSSubDmo.ExamLoginPresub = tempList321;
                        //                    _studentmappingContext.Add(tempSubDmo);
                        //                    var contact4Exists = _studentmappingContext.SaveChanges();
                        //                    tempSubSubList = new List<Exm_Login_Privilege_SubSubjectsDMO>();
                        //                    if (contact4Exists >= 1)
                        //                    {
                        //                        savedata.returnval = true;
                        //                    }
                        //                    else
                        //                    {
                        //                        savedata.returnval = false;
                        //                    }
                        //                }
                        //            }
                        //            if (Duplicate_Count4 == 0)
                        //            {

                        //                tempSubDmo.ELP_Id = data.ELP_Id;
                        //                tempSubDmo.ASMCL_Id = tempclass;
                        //                tempSubDmo.ASMS_Id = tempsec;
                        //                tempSubDmo.ISMS_Id = tempsub;
                        //                tempSubDmo.ELPs_ActiveFlg = true;
                        //                tempSubDmo.CreatedDate = DateTime.Now;
                        //                tempSubDmo.UpdatedDate = DateTime.Now;
                        //                data.ELPS_Id = tempSubDmo.ELPS_Id;
                        //                // tempList123.Add(tb2);
                        //                //var tempcheck = tempList123;
                        //                for (int l = 0; l < data.selectedclass[i].ssub.Count(); l++)
                        //                {
                        //                    var Duplicate_Count6 = 0;
                        //                savedata.returnduplicatestatus = "";
                        //                Exm_Login_Privilege_SubSubjectsDMO tempSubSubWhenNoDup = new Exm_Login_Privilege_SubSubjectsDMO();
                        //                    var tempssub = data.selectedclass[i].ssub[l].EMSS_Id;
                        //                    var result3 = _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO.Where(t => t.EMSS_Id == tempssub && t.ELPS_Id == data.ELPS_Id && t.ELPSS_ActiveFlg==true).ToList();
                        //                    if (result3.Count > 0)
                        //                    {
                        //                        Duplicate_Count6 += 1;
                        //                        savedata.returnduplicatestatus = "Duplicate";
                        //                    }
                        //                    if (Duplicate_Count6 == 0)
                        //                    {
                        //                        tempSubSubWhenNoDup.EMSS_Id = tempssub;
                        //                        tempSubSubWhenNoDup.ELPS_Id = data.ELPS_Id;
                        //                        tempSubSubWhenNoDup.CreatedDate = DateTime.Now;
                        //                        tempSubSubWhenNoDup.UpdatedDate = DateTime.Now;
                        //                    tempSubSubWhenNoDup.ELPSS_ActiveFlg = true;
                        //                    //tempList321.Add(tb3);
                        //                    tempSubSubList.Add(tempSubSubWhenNoDup);
                        //                    }

                        //                }
                        //                if (savedata.returnduplicatestatus != "Duplicate")
                        //                {

                        //                    tempSubDmo.Exm_Login_Privilege_SubSubjects = tempSubSubList;


                        //                    _studentmappingContext.Add(tempSubDmo);

                        //                    var contact5Exists = _studentmappingContext.SaveChanges();
                        //                    tempSubSubList = new List<Exm_Login_Privilege_SubSubjectsDMO>();
                        //                    if (contact5Exists >= 1)
                        //                    {
                        //                        savedata.returnval = true;
                        //                    }
                        //                    else
                        //                    {
                        //                        savedata.returnval = false;
                        //                    }
                        //                }
                        //            }

                        //        }
                        //    }
                        //}
                    }
                }

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public string deleteRecord(Exm_Login_PrivilegeDTO data)
        {
            var Duplicate_Count = 0;

            Exm_Login_PrivilegeDTO savedata = new Exm_Login_PrivilegeDTO();

            try
            {

                var tempyrid = data.selectedclass[0].yearid;
                var templogid = data.selectedclass[0].user_id;
                var tempelpflg = data.selectedclass[0].elpflg;

                var result1 = _studentmappingContext.Exm_Login_PrivilegeDMO.Where(t => t.ASMAY_Id == tempyrid && t.Login_Id == templogid && t.MI_Id == data.MI_Id && t.ELP_ActiveFlg == true).ToList();
                if (result1.Count > 0)
                {

                    //deleteRecord class


                    for (int j = 0; j < data.selectedclass.Length; j++)
                    {
                        var cls_id = data.selectedclass[j].clas.ASMCL_Id;
                        var sec_id = data.selectedclass[j].secs.ASMS_Id;


                    }
                    var classidids = data.selectedclass.Select(t => t.clas.ASMCL_Id);
                    var resclassidids = _studentmappingContext.Exm_Login_Privilege_SubjectsDMO.Where(t => !classidids.Contains(t.ASMCL_Id) && t.ELP_Id == result1[0].ELP_Id && t.ELPs_ActiveFlg == true).Select(t => t.ELPS_Id).ToList();

                    if (resclassidids.Count > 0)
                    {
                        for (int delclass = 0; delclass < resclassidids.Count(); delclass++)
                        {

                            var resubsubjids = _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO.Where(t => t.ELPS_Id == resclassidids[delclass] && t.ELPSS_ActiveFlg == true).ToList();

                            if (resubsubjids.Count > 0)
                            {
                                for (int delsubsub = 0; delsubsub < resubsubjids.Count(); delsubsub++)
                                {
                                    var resultsubsubject = _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO.Single(t => t.ELPSS_Id == resubsubjids[delsubsub].ELPSS_Id);

                                    resultsubsubject.ELPSS_ActiveFlg = false;


                                    _studentmappingContext.Update(resultsubsubject);
                                    _studentmappingContext.SaveChanges();
                                }
                            }

                            var resultsubject = _studentmappingContext.Exm_Login_Privilege_SubjectsDMO.Single(t => t.ELPS_Id == resclassidids[delclass] && t.ELPs_ActiveFlg == true);

                            resultsubject.ELPs_ActiveFlg = false;

                            _studentmappingContext.Update(resultsubject);
                            _studentmappingContext.SaveChanges();

                        }
                    }


                    // class delete end

                    // secs delete 

                    for (int i = 0; i < data.selectedclass.Count(); i++)
                    {
                        //for (int jj = 0; jj < data.selectedclass[i].secs.Count(); jj++)
                        //{
                        var secidids = data.selectedclass[i].secs.ASMS_Id;

                        //var ressecidids = _studentmappingContext.Exm_Login_Privilege_SubjectsDMO.Where(t => t.ASMCL_Id == data.selectedclass[i].clas.ASMCL_Id && !secidids.Contains(t.ASMS_Id) && t.ELP_Id == result1[0].ELP_Id && t.ELPs_ActiveFlg == true).Select(t => t.ELPS_Id).ToList();

                        var ressecidids = _studentmappingContext.Exm_Login_Privilege_SubjectsDMO.Where(t => t.ASMCL_Id == data.selectedclass[i].clas.ASMCL_Id && !(t.ASMS_Id == secidids) && t.ELP_Id == result1[0].ELP_Id && t.ELPs_ActiveFlg == true).Select(t => t.ELPS_Id).ToList();

                        if (ressecidids.Count > 0)
                        {
                            for (int delsec = 0; delsec < ressecidids.Count(); delsec++)
                            {

                                var resubsubjids = _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO.Where(t => t.ELPS_Id == ressecidids[delsec] && t.ELPSS_ActiveFlg == true).ToList();

                                if (resubsubjids.Count > 0)
                                {
                                    for (int delsubsub = 0; delsubsub < resubsubjids.Count(); delsubsub++)
                                    {
                                        var resultsubsubject = _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO.Single(t => t.ELPSS_Id == resubsubjids[delsubsub].ELPSS_Id);

                                        resultsubsubject.ELPSS_ActiveFlg = false;


                                        _studentmappingContext.Update(resultsubsubject);
                                        _studentmappingContext.SaveChanges();
                                    }
                                }

                                var resultsubject = _studentmappingContext.Exm_Login_Privilege_SubjectsDMO.Single(t => t.ELPS_Id == ressecidids[delsec] && t.ELPs_ActiveFlg == true);

                                resultsubject.ELPs_ActiveFlg = false;


                                _studentmappingContext.Update(resultsubject);
                                _studentmappingContext.SaveChanges();
                            }
                        }
                        //}
                    }

                    //sec delete end

                    //subject delete
                    for (int i = 0; i < data.selectedclass.Count(); i++)
                    {
                        //for (int jj = 0; jj < data.selectedclass[i].secs.Count(); jj++)
                        //{

                        var subids = data.selectedclass[i].sub.Select(t => t.ISMS_Id);

                        //var resubjids = _studentmappingContext.Exm_Login_Privilege_SubjectsDMO.Where(t => t.ASMCL_Id == data.selectedclass[i].clas.ASMCL_Id && t.ASMS_Id == data.selectedclass[i].secs[jj].ASMS_Id && !subids.Contains(t.ISMS_Id) && t.ELP_Id == result1[0].ELP_Id && t.ELPs_ActiveFlg == true).ToList();
                        var resubjids = _studentmappingContext.Exm_Login_Privilege_SubjectsDMO.Where(t => t.ASMCL_Id == data.selectedclass[i].clas.ASMCL_Id && t.ASMS_Id == data.selectedclass[i].secs.ASMS_Id && !subids.Contains(t.ISMS_Id) && t.ELP_Id == result1[0].ELP_Id && t.ELPs_ActiveFlg == true).ToList();

                        if (resubjids.Count > 0)
                        {
                            for (int delsub = 0; delsub < resubjids.Count(); delsub++)
                            {

                                var resubsubjids = _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO.Where(t => t.ELPS_Id == resubjids[delsub].ELPS_Id && t.ELPSS_ActiveFlg == true).ToList();

                                if (resubsubjids.Count > 0)
                                {
                                    for (int delsubsub = 0; delsubsub < resubsubjids.Count(); delsubsub++)
                                    {
                                        var resultsubsubject = _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO.Single(t => t.ELPSS_Id == resubsubjids[delsubsub].ELPSS_Id);

                                        resultsubsubject.ELPSS_ActiveFlg = false;


                                        _studentmappingContext.Update(resultsubsubject);
                                        _studentmappingContext.SaveChanges();
                                    }
                                }

                                var resultsubject = _studentmappingContext.Exm_Login_Privilege_SubjectsDMO.Single(t => t.ELPS_Id == resubjids[delsub].ELPS_Id && t.ELPs_ActiveFlg == true);

                                resultsubject.ELPs_ActiveFlg = false;


                                _studentmappingContext.Update(resultsubject);
                                _studentmappingContext.SaveChanges();
                            }
                        }

                        //}
                    }

                    // subject delete end

                    //sub subject delete
                    for (int i = 0; i < data.selectedclass.Count(); i++)
                    {
                        //for (int jj = 0; jj < data.selectedclass[i].secs.Count(); jj++)
                        //{
                        for (int kk = 0; kk < data.selectedclass[i].sub.Count(); kk++)
                        {

                            var subids = data.selectedclass[i].ssub.Select(t => t.EMSS_Id);

                            //var resubjids = _studentmappingContext.Exm_Login_Privilege_SubjectsDMO.Where(t => t.ASMCL_Id == data.selectedclass[i].clas.ASMCL_Id && t.ASMS_Id == data.selectedclass[i].secs[jj].ASMS_Id && t.ISMS_Id == data.selectedclass[i].sub[kk].ISMS_Id && t.ELP_Id == result1[0].ELP_Id && t.ELPs_ActiveFlg == true).Select(t => t.ELPS_Id).ToList();

                            var resubjids = _studentmappingContext.Exm_Login_Privilege_SubjectsDMO.Where(t => t.ASMCL_Id == data.selectedclass[i].clas.ASMCL_Id && t.ASMS_Id == data.selectedclass[i].secs.ASMS_Id && t.ISMS_Id == data.selectedclass[i].sub[kk].ISMS_Id && t.ELP_Id == result1[0].ELP_Id && t.ELPs_ActiveFlg == true).Select(t => t.ELPS_Id).ToList();

                            var reselpsid = _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO.Where(t => resubjids.Contains(t.ELPS_Id) && !subids.Contains(t.EMSS_Id) && t.ELPSS_ActiveFlg == true).Select(t => t.ELPSS_Id).ToList();

                            if (reselpsid.Count > 0)
                            {
                                for (int delsubsub = 0; delsubsub < reselpsid.Count(); delsubsub++)
                                {
                                    var resultsubsubject = _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO.Single(t => t.ELPSS_Id == reselpsid[delsubsub]);

                                    resultsubsubject.ELPSS_ActiveFlg = false;


                                    _studentmappingContext.Update(resultsubsubject);
                                    _studentmappingContext.SaveChanges();
                                }
                            }

                        }
                        //}
                    }
                }
                //sub subject delete end


            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }

            return "";
        }
        public Exm_Login_PrivilegeDTO editdetails(Exm_Login_PrivilegeDTO editlt)
        {
            //Exm_Login_PrivilegeDTO editlt = new Exm_Login_PrivilegeDTO();
            try
            {
                // var hremid = Convert.ToInt64(ID);

                var ELPS_Ids = _studentmappingContext.Exm_Login_Privilege_SubjectsDMO.Where(s => s.ELP_Id == editlt.ELP_Id && s.ELPs_ActiveFlg == true && s.ASMCL_Id == editlt.ASMCL_Id && s.ASMS_Id == editlt.ASMS_Id).Select(s => s.ELPS_Id).ToList();
                var ELPSS_Ids = _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO.Where(ss => ELPS_Ids.Contains(ss.ELPS_Id) && ss.ELPSS_ActiveFlg == true).ToList();
                if (ELPSS_Ids.Count == 0)
                {
                    editlt.editlist = (from a in _studentmappingContext.Exm_Login_PrivilegeDMO
                                       from b in _studentmappingContext.Exm_Login_Privilege_SubjectsDMO
                                       from c in _studentmappingContext.Staff_User_Login
                                       from d in _studentmappingContext.HR_Master_Employee_DMO
                                           // from e in _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO
                                       where (a.ELP_Id == b.ELP_Id && a.ELP_Id == editlt.ELP_Id && c.Emp_Code == d.HRME_Id && a.Login_Id == c.IVRMSTAUL_Id && a.ELP_ActiveFlg == true && b.ASMCL_Id == editlt.ASMCL_Id && b.ASMS_Id == editlt.ASMS_Id && b.ELPs_ActiveFlg == true)
                                       //a.Login_Id == hremid && && b.ELPs_ActiveFlg == true
                                       select new Exm_Login_PrivilegeDTO
                                       {
                                           ASMAY_Id = a.ASMAY_Id,
                                           Login_Id = a.Login_Id,
                                           ELP_Flg = a.ELP_Flg,
                                           IVRMULF_Id = c.IVRMSTAUL_Id,
                                           ASMCL_Id = b.ASMCL_Id,
                                           ASMS_Id = b.ASMS_Id,
                                           ISMS_Id = b.ISMS_Id,
                                           Emp_Code = c.Emp_Code,
                                           HRME_Id = d.HRME_Id,
                                       }).ToArray();
                }
                else if (ELPSS_Ids.Count > 0)
                {
                    editlt.editlist = (from a in _studentmappingContext.Exm_Login_PrivilegeDMO
                                       from b in _studentmappingContext.Exm_Login_Privilege_SubjectsDMO
                                       from c in _studentmappingContext.Staff_User_Login
                                       from d in _studentmappingContext.HR_Master_Employee_DMO
                                       from e in _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO


                                       where (a.ELP_Id == b.ELP_Id && a.ELP_Id == editlt.ELP_Id && c.Emp_Code == d.HRME_Id && a.Login_Id == c.IVRMSTAUL_Id && b.ELPS_Id == e.ELPS_Id && a.ELP_ActiveFlg == true && b.ELPs_ActiveFlg == true && b.ASMCL_Id == editlt.ASMCL_Id && b.ASMS_Id == editlt.ASMS_Id)     //&& e.ELPSS_ActiveFlg == true                              
                                       select new Exm_Login_PrivilegeDTO
                                       {
                                           ASMAY_Id = a.ASMAY_Id,
                                           Login_Id = a.Login_Id,
                                           ELP_Flg = a.ELP_Flg,
                                           IVRMULF_Id = c.IVRMSTAUL_Id,
                                           ASMCL_Id = b.ASMCL_Id,
                                           ASMS_Id = b.ASMS_Id,
                                           ISMS_Id = b.ISMS_Id,
                                           Emp_Code = c.Emp_Code,
                                           HRME_Id = d.HRME_Id,
                                           EMSS_Id = e.EMSS_Id
                                       }).ToArray();
                }




            }
            catch (Exception ee)
            {
                _acdimpl.LogError(ee.Message);
                _acdimpl.LogDebug(ee.Message);
            }
            return editlt;
        }
        public Exm_Login_PrivilegeDTO getalldetailsviewrecords(Exm_Login_PrivilegeDTO data)
        {
            //Exm_Login_PrivilegeDTO detalt = new Exm_Login_PrivilegeDTO();
            try
            {

                //  List<Exm_Login_PrivilegeDMO> getlt = new List<Exm_Login_PrivilegeDMO>();
                // getlt = _studentmappingContext.Exm_Login_PrivilegeDMO.Where(t => t.Login_Id == ID).ToList();

                var ELPS_Ids = _studentmappingContext.Exm_Login_Privilege_SubjectsDMO.Where(s => s.ELP_Id == data.ELP_Id && s.ASMCL_Id == data.ASMCL_Id && s.ASMS_Id == data.ASMS_Id).Select(s => s.ELPS_Id).ToList();//&& s.ELPs_ActiveFlg == true
                List<Exm_Login_PrivilegeDTO> temp_list = new List<Exm_Login_PrivilegeDTO>();
                foreach (var y in ELPS_Ids)
                {
                    var ELPSS_Ids = _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO.Where(ss => ss.ELPS_Id == y).ToList();//&& ss.ELPSS_ActiveFlg == true
                    if (ELPSS_Ids.Count == 0)
                    {
                        var gtdetailsview = (from a in _studentmappingContext.Exm_Login_Privilege_SubjectsDMO
                                                 // from b in _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO
                                             from c in _studentmappingContext.IVRM_School_Master_SubjectsDMO                                             
                                             from e in _studentmappingContext.AdmissionClass
                                             from f in _studentmappingContext.School_M_Section
                                             where (a.ISMS_Id == c.ISMS_Id && a.ASMCL_Id == e.ASMCL_Id && a.ASMS_Id == f.ASMS_Id && a.ELP_Id == data.ELP_Id && f.ASMS_Id == data.ASMS_Id && e.ASMCL_Id == data.ASMCL_Id && a.ELPS_Id == y)//a.ELP_Id == getlt[0].ELP_Id && a.ELPs_ActiveFlg ==true
                                             select new Exm_Login_PrivilegeDTO
                                             {
                                                 ISMS_SubjectName = c.ISMS_SubjectName,
                                                 //  EMSS_SubSubjectName = d.EMSS_SubSubjectName,
                                                 ASMCL_ClassName = e.ASMCL_ClassName,
                                                 ASMC_SectionName = f.ASMC_SectionName,
                                                 ASMS_Id = f.ASMS_Id,
                                                 ASMCL_Id = e.ASMCL_Id,
                                                 ELPs_ActiveFlg = a.ELPs_ActiveFlg,
                                                 ELPS_Id = a.ELPS_Id,
                                                 ELP_Id = a.ELP_Id
                                             }).Distinct().ToArray();
                        foreach (var g in gtdetailsview)
                        {
                            temp_list.Add(g);
                        }

                    }
                    else if (ELPSS_Ids.Count > 0)
                    {
                        var gtdetailsview = (from a in _studentmappingContext.Exm_Login_Privilege_SubjectsDMO
                                             from b in _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO
                                             from c in _studentmappingContext.IVRM_School_Master_SubjectsDMO
                                             from d in _studentmappingContext.mastersubsubject
                                             from e in _studentmappingContext.AdmissionClass
                                             from f in _studentmappingContext.School_M_Section
                                             where (a.ELPS_Id == b.ELPS_Id && a.ISMS_Id == c.ISMS_Id && a.ASMCL_Id == e.ASMCL_Id && a.ASMS_Id == f.ASMS_Id && a.ELP_Id == data.ELP_Id && b.EMSS_Id == d.EMSS_Id && f.ASMS_Id == data.ASMS_Id && e.ASMCL_Id == data.ASMCL_Id && a.ELPS_Id == y)//a.ELP_Id == getlt[0].ELP_Id && a.ELPs_ActiveFlg == true && b.ELPSS_ActiveFlg == true
                                             select new Exm_Login_PrivilegeDTO
                                             {
                                                 ISMS_SubjectName = c.ISMS_SubjectName,
                                                 EMSS_SubSubjectName = d.EMSS_SubSubjectName,
                                                 ASMCL_Id = e.ASMCL_Id,
                                                 ASMCL_ClassName = e.ASMCL_ClassName,
                                                 ASMS_Id = f.ASMS_Id,
                                                 ASMC_SectionName = f.ASMC_SectionName,
                                                 ELPss_ActiveFlg = b.ELPSS_ActiveFlg,
                                                 ELPS_Id = a.ELPS_Id,
                                                 ELPSS_Id = b.ELPSS_Id,
                                                 ELP_Id = a.ELP_Id

                                             }).Distinct().ToArray();

                        foreach (var g in gtdetailsview)
                        {
                            temp_list.Add(g);
                        }
                    }

                }
                data.gtdetailsview = temp_list.ToArray();

                //var ELPSS_Ids = _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO.Where(ss => ELPS_Ids.Contains(ss.ELPS_Id) ).ToList();//&& ss.ELPSS_ActiveFlg == true
                //if (ELPSS_Ids.Count==0)
                //{
                //    detalt.gtdetailsview = (from a in _studentmappingContext.Exm_Login_Privilege_SubjectsDMO
                //                           // from b in _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO
                //                            from c in _studentmappingContext.IVRM_School_Master_SubjectsDMO
                //                            from d in _studentmappingContext.mastersubsubject
                //                            from e in _studentmappingContext.AdmissionClass
                //                            from f in _studentmappingContext.School_M_Section
                //                            where ( a.ISMS_Id == c.ISMS_Id && a.ASMCL_Id == e.ASMCL_Id && a.ASMS_Id == f.ASMS_Id && a.ELP_Id == data.ELP_Id  && f.ASMS_Id==data.ASMS_Id && e.ASMCL_Id==data.ASMCL_Id)//a.ELP_Id == getlt[0].ELP_Id && a.ELPs_ActiveFlg ==true
                //                            select new Exm_Login_PrivilegeDTO
                //                            {
                //                                ISMS_SubjectName = c.ISMS_SubjectName,
                //                              //  EMSS_SubSubjectName = d.EMSS_SubSubjectName,
                //                                ASMCL_ClassName = e.ASMCL_ClassName,
                //                                ASMC_SectionName = f.ASMC_SectionName,
                //                                 ASMS_Id = f.ASMS_Id,
                //                                ASMCL_Id = e.ASMCL_Id,
                //                                ELPs_ActiveFlg=a.ELPs_ActiveFlg,
                //                                ELPS_Id=a.ELPS_Id,
                //                                ELP_Id= a.ELP_Id
                //                            }).Distinct().ToArray();
                //}
                //else   if(ELPSS_Ids.Count>0)
                //{
                //    detalt.gtdetailsview = (from a in _studentmappingContext.Exm_Login_Privilege_SubjectsDMO
                //                            from b in _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO
                //                            from c in _studentmappingContext.IVRM_School_Master_SubjectsDMO
                //                            from d in _studentmappingContext.mastersubsubject
                //                            from e in _studentmappingContext.AdmissionClass
                //                            from f in _studentmappingContext.School_M_Section
                //                            where (a.ELPS_Id == b.ELPS_Id && a.ISMS_Id == c.ISMS_Id && a.ASMCL_Id == e.ASMCL_Id && a.ASMS_Id == f.ASMS_Id && a.ELP_Id == data.ELP_Id && b.EMSS_Id == d.EMSS_Id  && f.ASMS_Id == data.ASMS_Id && e.ASMCL_Id == data.ASMCL_Id)//a.ELP_Id == getlt[0].ELP_Id && a.ELPs_ActiveFlg == true && b.ELPSS_ActiveFlg == true
                //                            select new Exm_Login_PrivilegeDTO
                //                            {
                //                                ISMS_SubjectName = c.ISMS_SubjectName,
                //                                EMSS_SubSubjectName = d.EMSS_SubSubjectName,
                //                                ASMCL_Id = e.ASMCL_Id,
                //                                ASMCL_ClassName = e.ASMCL_ClassName,
                //                                ASMS_Id = f.ASMS_Id,
                //                                ASMC_SectionName = f.ASMC_SectionName,
                //                                ELPss_ActiveFlg = b.ELPSS_ActiveFlg,
                //                                ELPS_Id = a.ELPS_Id,
                //                                ELPSS_Id=b.ELPSS_Id,
                //                                ELP_Id = a.ELP_Id

                //                            }).Distinct().ToArray();
                //}



            }
            catch (Exception ee)
            {
                _acdimpl.LogError(ee.Message);
                _acdimpl.LogDebug(ee.Message);
            }
            return data;
        }
        public Exm_Login_PrivilegeDTO deactivate(Exm_Login_PrivilegeDTO data)
        {
            Exm_Login_PrivilegeDTO deact = new Exm_Login_PrivilegeDTO();
            try
            {
                // StudentMappingDMO master = Mapper.Map<StudentMappingDMO>(data);
                if (data.ELPS_Id > 0)
                {

                    //var ELPS_Ids = _studentmappingContext.Exm_Login_Privilege_SubjectsDMO.Where(s => s.ELPS_Id == data.ELPS_Id).Select(s => s.ELPS_Id).ToList();
                    //var ELPSS_Ids = _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO.Where(ss => ELPS_Ids.Contains(ss.ELPS_Id)).ToList();


                    if (data.ELPSS_Id == 0)
                    {
                        var result1 = _studentmappingContext.Exm_Login_Privilege_SubjectsDMO.Where(t => t.ELPS_Id == data.ELPS_Id).ToList();
                        for (var ii = 0; ii < result1.Count(); ii++)
                        {
                            var elcflag = result1[ii].ELPs_ActiveFlg;
                            if (elcflag == true)
                            {
                                result1[ii].ELPs_ActiveFlg = false;
                            }
                            else
                            {
                                result1[ii].ELPs_ActiveFlg = true;
                            }

                            _studentmappingContext.Update(result1[ii]);
                        }
                    }


                    if (data.ELPSS_Id > 0)
                    {
                        var result = _studentmappingContext.Exm_Login_Privilege_SubSubjectsDMO.Where(t => t.ELPSS_Id == data.ELPSS_Id).ToList();
                        for (var i = 0; i < result.Count(); i++)
                        {
                            var elcflag = result[i].ELPSS_ActiveFlg;
                            if (elcflag == true)
                            {
                                result[i].ELPSS_ActiveFlg = false;
                            }
                            else
                            {
                                result[i].ELPSS_ActiveFlg = true;
                            }

                            _studentmappingContext.Update(result[i]);
                        }
                    }



                    //var result = _studentmappingContext.Exm_Login_PrivilegeDMO.Where(t => t.ELP_Id == //data.ELP_Id).ToList();

                    var flag = _studentmappingContext.SaveChanges();
                    if (flag >= 1)
                    {
                        deact.returnval = true;
                    }
                    else
                    {
                        deact.returnval = false;
                    }
                }
            }
            catch (Exception ee)
            {
                _acdimpl.LogError(ee.Message);
                _acdimpl.LogDebug(ee.Message);
            }
            return deact;
        }
        public Exm_Login_PrivilegeDTO OnAcdyear(Exm_Login_PrivilegeDTO data)
        {
            Exm_Login_PrivilegeDTO getdata = new Exm_Login_PrivilegeDTO();
            try
            {
                if (data.ELP_Flg == "ct")
                {
                    List<ClassTeacherMappingDMO> clstech = new List<ClassTeacherMappingDMO>();
                    clstech = _studentmappingContext.ClassTeacherMappingDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id && t.IMCT_ActiveFlag == true).ToList();
                    getdata.clastechlt = clstech.GroupBy(t => t.HRME_Id).Select(d => d.FirstOrDefault().HRME_Id).ToArray();
                }
                else
                {
                    List<ClassTeacherMappingDMO> clstech = new List<ClassTeacherMappingDMO>();
                    clstech = _studentmappingContext.ClassTeacherMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.IMCT_ActiveFlag == true).ToList();
                    getdata.clastechlt = clstech.GroupBy(t => t.HRME_Id).Select(d => d.FirstOrDefault().HRME_Id).ToArray();
                }

                getdata.emplist = (from a in _studentmappingContext.Staff_User_Login
                                   from b in _studentmappingContext.HR_Master_Employee_DMO
                                   where (a.Emp_Code == b.HRME_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                   select new Exm_Login_PrivilegeDTO
                                   {
                                       HRME_Id = b.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName) + " : " + (b.HRME_EmployeeCode == null || b.HRME_EmployeeCode == "" ? "" : " " + b.HRME_EmployeeCode)).Trim(),
                                   }).Distinct().ToArray();

                getdata.userlist = (from a in _studentmappingContext.Staff_User_Login
                                    from b in _studentmappingContext.HR_Master_Employee_DMO
                                    where (a.Emp_Code == b.HRME_Id && a.MI_Id == data.MI_Id)
                                    select new Exm_Login_PrivilegeDTO
                                    {
                                        IVRMULF_Id = a.IVRMSTAUL_Id,
                                        HRME_EmployeeFirstName = ((a.IVRMSTAUL_UserName == null ? " " : a.IVRMSTAUL_UserName) + " : " + (b.HRME_EmployeeCode == null ? " " : " " + b.HRME_EmployeeCode)),
                                        Emp_Code = a.Emp_Code
                                    }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return getdata;
        }
    }
}
