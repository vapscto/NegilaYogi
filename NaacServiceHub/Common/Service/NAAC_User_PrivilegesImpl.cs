using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.NAAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model.NAAC.Documents;

namespace NaacServiceHub.Common.Service
{
    public class NAAC_User_PrivilegesImpl : Interface.NAAC_User_PrivilegesInterface
    {
        public GeneralContext _context;
        ILogger<NAAC_User_PrivilegesImpl> _log;
        public NAAC_User_PrivilegesImpl(GeneralContext _cont, ILogger<NAAC_User_PrivilegesImpl> _logg)
        {
            _context = _cont;
            _log = _logg;
        }
        public NAAC_User_PrivilegesDTO Getdetails(NAAC_User_PrivilegesDTO data)
        {
            try
            {
                data.getemployee = (from a in _context.HR_Master_Employee_DMO
                                    from b in _context.HR_Master_Department
                                    from c in _context.HR_Master_Designation
                                    where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.MI_Id == data.MI_Id && a.HRME_LeftFlag == false
                                    && a.HRME_ActiveFlag == true)
                                    select new NAAC_User_PrivilegesDTO
                                    {
                                        HRME_Id = a.HRME_Id,
                                        HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                        + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                        + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)
                                        + (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : " : " + a.HRME_EmployeeCode)).Trim()
                                    }).Distinct().OrderBy(a => a.HRME_EmployeeFirstName).ToArray();


                data.getsavedata = (from a in _context.NAAC_User_PrivilegeDMO
                                    from c in _context.Staff_User_Login
                                    from d in _context.HR_Master_Employee_DMO
                                    where (a.IVRMUL_Id == c.Id && c.Emp_Code == d.HRME_Id && d.MI_Id == data.MI_Id)
                                    select new NAAC_User_PrivilegesDTO
                                    {
                                        HRME_Id = d.HRME_Id,
                                        HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null || d.HRME_EmployeeFirstName == "" ? "" : d.HRME_EmployeeFirstName)
                                        + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == "" ? "" : " " + d.HRME_EmployeeMiddleName)
                                        + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == "" ? "" : " " + d.HRME_EmployeeLastName)
                                        + (d.HRME_EmployeeCode == null || d.HRME_EmployeeCode == "" ? "" : " : " + d.HRME_EmployeeCode)).Trim(),
                                        NAACUPRI_AddFlg = a.NAACUPRI_AddFlg,
                                        NAACUPRI_UpdateFlg = a.NAACUPRI_UpdateFlg,
                                        NAACUPRI_DeleteFlg = a.NAACUPRI_DeleteFlg,
                                        NAACUPRI_IQACInchargeFlg = a.NAACUPRI_IQACInchargeFlg,
                                        NAACUPRI_ConsultantFlg = a.NAACUPRI_ConsultantFlg,
                                        NAACUPRI_TrustUserFlag = a.NAACUPRI_TrustUserFlag,
                                        NAACUPRI_ApproverFlg = a.NAACUPRI_ApproverFlg,
                                        NAACUPRI_Order = a.NAACUPRI_Order,
                                        NAACUPRI_Id = a.NAACUPRI_Id,
                                        NAACUPRI_ActiveFlag = a.NAACUPRI_ActiveFlag

                                    }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_User_PrivilegesDTO onchangeemployee(NAAC_User_PrivilegesDTO data)
        {
            try
            {
                string NAACSL_InstitutionTypeFlg = "";
                var masterinstitute = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();
                if (masterinstitute.Length > 0)
                {
                    NAACSL_InstitutionTypeFlg = masterinstitute.FirstOrDefault().MI_NAAC_InstitutionTypeFlg;
                }
                if (masterinstitute.FirstOrDefault().MI_NAAC_InstitutionTypeFlg.ToUpper() == "DEEMED")
                {
                    data.getcriteria = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_ParentId == 0
                    && a.NAACSL_InstitutionTypeFlg.ToUpper() == masterinstitute.FirstOrDefault().MI_NAAC_SubInstitutionTypeFlg.ToUpper()).OrderBy(a => a.NAACSL_SLNoOrder).ToArray();

                    data.getalldata = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_ActiveFlag == true && a.NAACSL_InstitutionTypeFlg.ToUpper() == NAACSL_InstitutionTypeFlg).OrderBy(a => a.NAACSL_SLNoOrder).ToArray();

                }
                else
                {
                    data.getcriteria = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_ParentId == 0 
                    && a.NAACSL_InstitutionTypeFlg == masterinstitute.FirstOrDefault().MI_NAAC_InstitutionTypeFlg).OrderBy(a => a.NAACSL_SLNoOrder).ToArray();

                    data.getalldata = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_ActiveFlag == true 
                    && a.NAACSL_InstitutionTypeFlg.ToUpper() == NAACSL_InstitutionTypeFlg).OrderBy(a => a.NAACSL_SLNoOrder).ToArray();
                }

                List<long> miids = new List<long>();

                var getuserid = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Emp_Code == data.HRME_Id && a.IVRMSTAUL_ActiveFlag == 1).ToList();

                if (getuserid.Count > 0)
                {
                    var getmiids = _context.UserRoleWithInstituteDMO.Where(a => a.Id == getuserid.FirstOrDefault().Id && a.Activeflag == 1).Distinct().ToList();

                    foreach (var c in getmiids)
                    {
                        miids.Add(c.MI_Id);
                    }

                    data.getinstitution = _context.Institution.Where(a => miids.Contains(a.MI_Id) && a.MI_ActiveFlag == 1).Distinct().ToArray();

                    data.getusersaveddetails = _context.NAAC_User_PrivilegeDMO.Where(a => a.IVRMUL_Id == getuserid.FirstOrDefault().Id).Distinct().ToArray();

                    //var getuserid = _context.Staff_User_Login.Where(a => a.Emp_Code == data.HRME_Id).ToList();

                    data.getsavedinstitution = (from a in _context.NAAC_User_PrivilegeDMO
                                                from c in _context.NAAC_User_Privilege_InstitutionDMO
                                                from d in _context.Institution
                                                where (a.NAACUPRI_Id == c.NAACUPRI_Id && c.MI_Id == d.MI_Id && a.IVRMUL_Id == getuserid.FirstOrDefault().Id)
                                                select d).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_User_PrivilegesDTO onchangecriteria(NAAC_User_PrivilegesDTO data)
        {
            try
            {
                var getuserid = _context.Staff_User_Login.Where(a => a.Emp_Code == data.HRME_Id).ToList();
                data.getsavedinstitution = (from a in _context.NAAC_User_PrivilegeDMO
                                            from b in _context.NAAC_User_Privilege_SLDMO
                                            from c in _context.NAAC_User_Privilege_InstitutionDMO
                                            from d in _context.Institution
                                            where (a.NAACUPRI_Id == b.NAACUPRI_Id && a.NAACUPRI_Id == c.NAACUPRI_Id && c.MI_Id == d.MI_Id
                                            && a.IVRMUL_Id == getuserid.FirstOrDefault().Id
                                            && b.NAACSL_Id == data.NAACSL_Id)
                                            select d).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_User_PrivilegesDTO savedata(NAAC_User_PrivilegesDTO data)
        {
            try
            {
                var getuserid = _context.Staff_User_Login.Where(a => a.Emp_Code == data.HRME_Id).ToList();

                var check_userexists = _context.NAAC_User_PrivilegeDMO.Where(a => a.IVRMUL_Id == getuserid.FirstOrDefault().Id).Distinct().ToList();

                if (check_userexists.Count > 0)
                {
                    data.NAACUPRI_Id = check_userexists.FirstOrDefault().NAACUPRI_Id;

                    var check_userresult = _context.NAAC_User_PrivilegeDMO.Single(a => a.IVRMUL_Id == getuserid.FirstOrDefault().Id);

                    check_userresult.NAACUPRI_AddFlg = data.NAACUPRI_AddFlg;
                    check_userresult.NAACUPRI_UpdateFlg = data.NAACUPRI_UpdateFlg;
                    check_userresult.NAACUPRI_DeleteFlg = data.NAACUPRI_DeleteFlg;
                    check_userresult.NAACUPRI_TrustUserFlag = data.NAACUPRI_TrustUserFlag;
                    check_userresult.NAACUPRI_IQACInchargeFlg = data.NAACUPRI_IQACInchargeFlg;
                    check_userresult.NAACUPRI_ConsultantFlg = data.NAACUPRI_ConsultantFlg;
                    check_userresult.NAACUPRI_ApproverFlg = data.NAACUPRI_ApproverFlg;
                    check_userresult.NAACUPRI_FinalFlg = data.NAACUPRI_FinalFlg;
                    check_userresult.NAACUPRI_Order = data.NAACUPRI_Order;
                    check_userresult.NAACUPRI_UpdatedBy = data.userid;
                    check_userresult.NAACUPRI_UpdatedDate = DateTime.Now;
                    _context.Update(check_userresult);

                    foreach (var head1 in data.mainheaderlist)
                    {
                        var check_criterialexistshead1 = _context.NAAC_User_Privilege_SLDMO.Where(a => a.NAACSL_Id == head1.NAACSL_Id
                        && a.NAACUPRI_Id == data.NAACUPRI_Id).Distinct().ToList();

                        if (check_criterialexistshead1.Count > 0)
                        {
                            var check_criterialresulthead1 = _context.NAAC_User_Privilege_SLDMO.Single(a => a.NAACSL_Id == head1.NAACSL_Id
                            && a.NAACUPRI_Id == data.NAACUPRI_Id);
                            check_criterialresulthead1.NAACUPRISL_UpdatedBy = data.userid;
                            check_criterialresulthead1.NAACUPRISL_UpdatedDate = DateTime.Now;
                            _context.Update(check_criterialresulthead1);
                        }
                        else
                        {
                            NAAC_User_Privilege_SLDMO dmosheadl = new NAAC_User_Privilege_SLDMO();
                            dmosheadl.NAACSL_Id = head1.NAACSL_Id;
                            dmosheadl.NAACUPRI_Id = data.NAACUPRI_Id;
                            dmosheadl.NAACUPRISL_ActiveFlag = true;
                            dmosheadl.NAACUPRISL_CreatedBy = data.userid;
                            dmosheadl.NAACUPRISL_UpdatedBy = data.userid;
                            dmosheadl.NAACUPRISL_CreatedDate = DateTime.Now;
                            dmosheadl.NAACUPRISL_UpdatedDate = DateTime.Now;
                            _context.Add(dmosheadl);
                        }
                    }

                    foreach (var head2 in data.headerlist)
                    {
                        var check_criterialexistshead2 = _context.NAAC_User_Privilege_SLDMO.Where(a => a.NAACSL_Id == head2.NAACSL_Id
                        && a.NAACUPRI_Id == data.NAACUPRI_Id).Distinct().ToList();

                        if (check_criterialexistshead2.Count > 0)
                        {
                            var check_criterialresulthead2 = _context.NAAC_User_Privilege_SLDMO.Single(a => a.NAACSL_Id == head2.NAACSL_Id
                            && a.NAACUPRI_Id == data.NAACUPRI_Id);
                            check_criterialresulthead2.NAACUPRISL_UpdatedBy = data.userid;
                            check_criterialresulthead2.NAACUPRISL_UpdatedDate = DateTime.Now;
                            _context.Update(check_criterialresulthead2);
                        }
                        else
                        {
                            NAAC_User_Privilege_SLDMO dmoslheade2 = new NAAC_User_Privilege_SLDMO();
                            dmoslheade2.NAACSL_Id = head2.NAACSL_Id;
                            dmoslheade2.NAACUPRI_Id = data.NAACUPRI_Id;
                            dmoslheade2.NAACUPRISL_ActiveFlag = true;
                            dmoslheade2.NAACUPRISL_CreatedBy = data.userid;
                            dmoslheade2.NAACUPRISL_UpdatedBy = data.userid;
                            dmoslheade2.NAACUPRISL_CreatedDate = DateTime.Now;
                            dmoslheade2.NAACUPRISL_UpdatedDate = DateTime.Now;
                            _context.Add(dmoslheade2);
                        }
                    }

                    foreach (var head3 in data.subheaderlist)
                    {
                        var check_criterialexistshead3 = _context.NAAC_User_Privilege_SLDMO.Where(a => a.NAACSL_Id == head3.NAACSL_Id
                          && a.NAACUPRI_Id == data.NAACUPRI_Id).Distinct().ToList();

                        if (check_criterialexistshead3.Count > 0)
                        {
                            var check_criterialresulthead3 = _context.NAAC_User_Privilege_SLDMO.Single(a => a.NAACSL_Id == head3.NAACSL_Id
                            && a.NAACUPRI_Id == data.NAACUPRI_Id);
                            check_criterialresulthead3.NAACUPRISL_UpdatedBy = data.userid;
                            check_criterialresulthead3.NAACUPRISL_UpdatedDate = DateTime.Now;
                            _context.Update(check_criterialresulthead3);
                        }
                        else
                        {
                            NAAC_User_Privilege_SLDMO dmoslhead3 = new NAAC_User_Privilege_SLDMO();
                            dmoslhead3.NAACSL_Id = head3.NAACSL_Id;
                            dmoslhead3.NAACUPRI_Id = data.NAACUPRI_Id;
                            dmoslhead3.NAACUPRISL_ActiveFlag = true;
                            dmoslhead3.NAACUPRISL_CreatedBy = data.userid;
                            dmoslhead3.NAACUPRISL_UpdatedBy = data.userid;
                            dmoslhead3.NAACUPRISL_CreatedDate = DateTime.Now;
                            dmoslhead3.NAACUPRISL_UpdatedDate = DateTime.Now;
                            _context.Add(dmoslhead3);
                        }
                    }

                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();

                    foreach (temp_miid ph in data.temp_miid)
                    {
                        temparr.Add(ph.MI_Id);
                    }

                    Array siblings_Noresultremove = _context.NAAC_User_Privilege_InstitutionDMO.Where(t => !temparr.Contains(t.MI_Id) && t.NAACUPRI_Id == data.NAACUPRI_Id).ToArray();

                    foreach (NAAC_User_Privilege_InstitutionDMO ph1 in siblings_Noresultremove)
                    {
                        _context.Remove(ph1);
                    }

                    for (int k = 0; k < data.temp_miid.Length; k++)
                    {
                        var check_institutionexists = _context.NAAC_User_Privilege_InstitutionDMO.Where(a => a.MI_Id == data.temp_miid[k].MI_Id
                        && a.NAACUPRI_Id == data.NAACUPRI_Id).Distinct().ToList();

                        if (check_institutionexists.Count > 0)
                        {
                            var check_institutionresult = _context.NAAC_User_Privilege_InstitutionDMO.Single(a => a.MI_Id == data.temp_miid[k].MI_Id
                            && a.NAACUPRI_Id == data.NAACUPRI_Id);
                            check_institutionresult.NAACUPRIIN_UpdatedBy = data.userid;
                            check_institutionresult.NAACUPRIIN_UpdatedDate = DateTime.Now;
                            _context.Update(check_institutionresult);
                        }
                        else
                        {
                            NAAC_User_Privilege_InstitutionDMO dmoinsti = new NAAC_User_Privilege_InstitutionDMO();
                            dmoinsti.MI_Id = data.temp_miid[k].MI_Id;
                            dmoinsti.NAACUPRIIN_CreatedBy = data.userid;
                            dmoinsti.NAACUPRIIN_ActiveFlag = true;
                            dmoinsti.NAACUPRI_Id = data.NAACUPRI_Id;
                            dmoinsti.NAACUPRIIN_CreatedDate = DateTime.Now;
                            dmoinsti.NAACUPRIIN_UpdatedBy = data.userid;
                            dmoinsti.NAACUPRIIN_UpdatedDate = DateTime.Now;
                            _context.Add(dmoinsti);
                        }
                    }

                    var i = _context.SaveChanges();
                    if (i > 0)
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
                    NAAC_User_PrivilegeDMO dmouser = new NAAC_User_PrivilegeDMO();
                    dmouser.IVRMUL_Id = getuserid.FirstOrDefault().Id;
                    dmouser.NAACUPRI_AddFlg = data.NAACUPRI_AddFlg;
                    dmouser.NAACUPRI_UpdateFlg = data.NAACUPRI_UpdateFlg;
                    dmouser.NAACUPRI_DeleteFlg = data.NAACUPRI_DeleteFlg;
                    dmouser.NAACUPRI_TrustUserFlag = data.NAACUPRI_TrustUserFlag;
                    dmouser.NAACUPRI_IQACInchargeFlg = data.NAACUPRI_IQACInchargeFlg;
                    dmouser.NAACUPRI_ConsultantFlg = data.NAACUPRI_ConsultantFlg;
                    dmouser.NAACUPRI_ApproverFlg = data.NAACUPRI_ApproverFlg;
                    dmouser.NAACUPRI_Order = data.NAACUPRI_Order;
                    dmouser.NAACUPRI_ActiveFlag = true;
                    dmouser.NAACUPRI_UpdatedBy = data.userid;
                    dmouser.NAACUPRI_UpdatedDate = DateTime.Now;
                    dmouser.NAACUPRI_CreatedBy = data.userid;
                    dmouser.NAACUPRI_CreatedDate = DateTime.Now;
                    _context.Add(dmouser);

                    foreach (var head1 in data.mainheaderlist)
                    {
                        NAAC_User_Privilege_SLDMO dmosheadl = new NAAC_User_Privilege_SLDMO();
                        dmosheadl.NAACSL_Id = head1.NAACSL_Id;
                        dmosheadl.NAACUPRI_Id = dmouser.NAACUPRI_Id;
                        dmosheadl.NAACUPRISL_ActiveFlag = true;
                        dmosheadl.NAACUPRISL_CreatedBy = data.userid;
                        dmosheadl.NAACUPRISL_UpdatedBy = data.userid;
                        dmosheadl.NAACUPRISL_CreatedDate = DateTime.Now;
                        dmosheadl.NAACUPRISL_UpdatedDate = DateTime.Now;
                        _context.Add(dmosheadl);
                    }

                    foreach (var head2 in data.headerlist)
                    {
                        NAAC_User_Privilege_SLDMO dmoslheade2 = new NAAC_User_Privilege_SLDMO();
                        dmoslheade2.NAACSL_Id = head2.NAACSL_Id;
                        dmoslheade2.NAACUPRI_Id = dmouser.NAACUPRI_Id;
                        dmoslheade2.NAACUPRISL_ActiveFlag = true;
                        dmoslheade2.NAACUPRISL_CreatedBy = data.userid;
                        dmoslheade2.NAACUPRISL_UpdatedBy = data.userid;
                        dmoslheade2.NAACUPRISL_CreatedDate = DateTime.Now;
                        dmoslheade2.NAACUPRISL_UpdatedDate = DateTime.Now;
                        _context.Add(dmoslheade2);
                    }

                    foreach (var head3 in data.subheaderlist)
                    {
                        NAAC_User_Privilege_SLDMO dmoslhead3 = new NAAC_User_Privilege_SLDMO();
                        dmoslhead3.NAACSL_Id = head3.NAACSL_Id;
                        dmoslhead3.NAACUPRI_Id = dmouser.NAACUPRI_Id;
                        dmoslhead3.NAACUPRISL_ActiveFlag = true;
                        dmoslhead3.NAACUPRISL_CreatedBy = data.userid;
                        dmoslhead3.NAACUPRISL_UpdatedBy = data.userid;
                        dmoslhead3.NAACUPRISL_CreatedDate = DateTime.Now;
                        dmoslhead3.NAACUPRISL_UpdatedDate = DateTime.Now;
                        _context.Add(dmoslhead3);
                    }

                    //NAAC_User_Privilege_SLDMO dmosl = new NAAC_User_Privilege_SLDMO();
                    //dmosl.NAACSL_Id = data.NAACSL_Id;
                    //dmosl.NAACUPRI_Id = dmouser.NAACUPRI_Id;
                    //dmosl.NAACUPRISL_ActiveFlag = true;
                    //dmosl.NAACUPRISL_CreatedBy = data.userid;
                    //dmosl.NAACUPRISL_UpdatedBy = data.userid;
                    //dmosl.NAACUPRISL_CreatedDate = DateTime.Now;
                    //dmosl.NAACUPRISL_UpdatedDate = DateTime.Now;
                    //_context.Add(dmosl);

                    for (int k = 0; k < data.temp_miid.Length; k++)
                    {
                        NAAC_User_Privilege_InstitutionDMO dmoinsti1 = new NAAC_User_Privilege_InstitutionDMO();
                        dmoinsti1.MI_Id = data.temp_miid[k].MI_Id;
                        dmoinsti1.NAACUPRIIN_CreatedBy = data.userid;
                        dmoinsti1.NAACUPRIIN_ActiveFlag = true;
                        dmoinsti1.NAACUPRI_Id = dmouser.NAACUPRI_Id;
                        dmoinsti1.NAACUPRIIN_CreatedDate = DateTime.Now;
                        dmoinsti1.NAACUPRIIN_UpdatedBy = data.userid;
                        dmoinsti1.NAACUPRIIN_UpdatedDate = DateTime.Now;
                        _context.Add(dmoinsti1);
                    }

                    var i = _context.SaveChanges();

                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.LogInformation("Error User Privileges : " + ex.Message);
            }
            return data;
        }
        public NAAC_User_PrivilegesDTO viewrecord(NAAC_User_PrivilegesDTO data)
        {
            try
            {
                if (data.flag == 1)
                {
                    data.getsavedcriteria = (from a in _context.NAAC_User_PrivilegeDMO
                                             from b in _context.NAAC_User_Privilege_SLDMO
                                             from c in _context.NaacDocumentUploadDMO
                                             where (a.NAACUPRI_Id == b.NAACUPRI_Id && b.NAACSL_Id == c.NAACSL_Id && a.NAACUPRI_Id == data.NAACUPRI_Id
                                             && b.NAACUPRI_Id == data.NAACUPRI_Id)
                                             select new NAAC_User_PrivilegesDTO
                                             {
                                                 NAACSL_Id = b.NAACSL_Id,
                                                 NAACSL_SLNoDescription = c.NAACSL_SLNo + " : " + c.NAACSL_SLNoDescription,
                                                 NAACUPRISL_Id = b.NAACUPRISL_Id,
                                                 NAACUPRISL_ActiveFlag = b.NAACUPRISL_ActiveFlag
                                             }).Distinct().ToArray();
                }
                else
                {
                    data.getsavedinstituiton = (from a in _context.NAAC_User_PrivilegeDMO
                                                from b in _context.NAAC_User_Privilege_InstitutionDMO
                                                from c in _context.Institution
                                                where (a.NAACUPRI_Id == b.NAACUPRI_Id && b.MI_Id == c.MI_Id && a.NAACUPRI_Id == data.NAACUPRI_Id
                                                && b.NAACUPRI_Id == data.NAACUPRI_Id)
                                                select new NAAC_User_PrivilegesDTO
                                                {
                                                    MI_Name = c.MI_Name,
                                                    NAACUPRIIN_Id = b.NAACUPRIIN_Id,
                                                    NAACUPRIIN_ActiveFlag = b.NAACUPRIIN_ActiveFlag
                                                }).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.LogInformation("Error User Privileges : " + ex.Message);
            }
            return data;
        }
        public NAAC_User_PrivilegesDTO deactivate(NAAC_User_PrivilegesDTO data)
        {
            try
            {
                if (data.flag == 1)
                {
                    var checkresult = _context.NAAC_User_PrivilegeDMO.Where(a => a.NAACUPRI_Id == data.NAACUPRI_Id).ToList();
                    if (checkresult.Count > 0)
                    {
                        var check_result = _context.NAAC_User_PrivilegeDMO.Single(a => a.NAACUPRI_Id == data.NAACUPRI_Id);

                        if (check_result.NAACUPRI_ActiveFlag == true)
                        {
                            check_result.NAACUPRI_ActiveFlag = false;
                        }
                        else
                        {
                            check_result.NAACUPRI_ActiveFlag = true;
                        }
                        check_result.NAACUPRI_UpdatedBy = data.userid;
                        check_result.NAACUPRI_UpdatedDate = DateTime.Now;
                        _context.Update(check_result);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.flag == 2)
                {
                    var checkresult = _context.NAAC_User_Privilege_SLDMO.Where(a => a.NAACUPRISL_Id == data.NAACUPRISL_Id).ToList();
                    if (checkresult.Count > 0)
                    {
                        var check_result = _context.NAAC_User_Privilege_SLDMO.Single(a => a.NAACUPRISL_Id == data.NAACUPRISL_Id);

                        if (check_result.NAACUPRISL_ActiveFlag == true)
                        {
                            check_result.NAACUPRISL_ActiveFlag = false;
                        }
                        else
                        {
                            check_result.NAACUPRISL_ActiveFlag = true;
                        }
                        check_result.NAACUPRISL_UpdatedBy = data.userid;
                        check_result.NAACUPRISL_UpdatedDate = DateTime.Now;
                        _context.Update(check_result);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.flag == 3)
                {
                    var checkresult = _context.NAAC_User_Privilege_InstitutionDMO.Where(a => a.NAACUPRIIN_Id == data.NAACUPRIIN_Id).ToList();
                    if (checkresult.Count > 0)
                    {
                        var check_result = _context.NAAC_User_Privilege_InstitutionDMO.Single(a => a.NAACUPRIIN_Id == data.NAACUPRIIN_Id);

                        if (check_result.NAACUPRIIN_ActiveFlag == true)
                        {
                            check_result.NAACUPRIIN_ActiveFlag = false;
                        }
                        else
                        {
                            check_result.NAACUPRIIN_ActiveFlag = true;
                        }
                        check_result.NAACUPRIIN_UpdatedBy = data.userid;
                        check_result.NAACUPRIIN_UpdatedDate = DateTime.Now;
                        _context.Update(check_result);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
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
                _log.LogInformation("Error User Privileges : " + ex.Message);
            }
            return data;
        }


        // Master Naac Criteria
        public NAAC_User_PrivilegesDTO OnChangeInstituionType(NAAC_User_PrivilegesDTO data)
        {
            try
            {
                if (data.TabFlag == "Tab1")
                {
                    data.GetZeroParentIdDetails = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_InstitutionTypeFlg == data.NAACSL_InstitutionTypeFlg
                    && a.NAACSL_ParentId == 0).ToArray();

                    data.GetZeroParentIdOrderDetails = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_InstitutionTypeFlg == data.NAACSL_InstitutionTypeFlg
                    && a.NAACSL_ParentId == 0).OrderBy(a => a.NAACSL_SLNoOrder).ToArray();
                }
                else if (data.TabFlag == "Tab2")
                {
                    data.GetSavedZeroPatentIdDetails = (from a in _context.NaacDocumentUploadDMO
                                                        where (a.NAACSL_InstitutionTypeFlg == data.NAACSL_InstitutionTypeFlg && a.NAACSL_ParentId == 0)
                                                        select new NAAC_User_PrivilegesDTO
                                                        {
                                                            NAACSL_Id = a.NAACSL_Id,
                                                            NAACSL_SLNoOrder = a.NAACSL_SLNoOrder,
                                                            criterianame = a.NAACSL_SLNo + ":" + a.NAACSL_SLNoDescription,
                                                        }).Distinct().OrderBy(a => a.NAACSL_SLNoOrder).ToArray();
                }
                else if (data.TabFlag == "Tab3")
                {
                    data.GetSavedZeroPatentIdDetails = (from a in _context.NaacDocumentUploadDMO
                                                        where (a.NAACSL_InstitutionTypeFlg == data.NAACSL_InstitutionTypeFlg && a.NAACSL_ParentId == 0)
                                                        select new NAAC_User_PrivilegesDTO
                                                        {
                                                            NAACSL_Id = a.NAACSL_Id,
                                                            NAACSL_SLNoOrder = a.NAACSL_SLNoOrder,
                                                            criterianame = a.NAACSL_SLNo + ":" + a.NAACSL_SLNoDescription,
                                                        }).Distinct().OrderBy(a => a.NAACSL_SLNoOrder).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_User_PrivilegesDTO SaveTab1(NAAC_User_PrivilegesDTO data)
        {
            try
            {
                data.returnval = false;
                data.returnmessage = "";
                if (data.NAACSL_Id > 0)
                {
                    var checkduplicate = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_SLNo == data.NAACSL_SLNo
                   && a.NAACSL_InstitutionTypeFlg == data.NAACSL_InstitutionTypeFlg && a.NAACSL_Id != data.NAACSL_Id).ToList();

                    if (checkduplicate.Count == 0)
                    {
                        SaveData(data);
                    }
                    else
                    {
                        data.returnmessage = "Duplicate";
                    }
                }
                else
                {
                    var checkduplicate = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_SLNo == data.NAACSL_SLNo
                    && a.NAACSL_InstitutionTypeFlg == data.NAACSL_InstitutionTypeFlg).ToList();

                    var checkorder = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_ParentId == 0
                    && a.NAACSL_InstitutionTypeFlg == data.NAACSL_InstitutionTypeFlg).Count();

                    if (checkduplicate.Count == 0)
                    {
                        data.checkorder = checkorder + 1;
                        SaveData(data);
                    }
                    else
                    {
                        data.returnmessage = "Duplicate";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.LogInformation("Error User Privileges : " + ex.Message);
            }
            return data;
        }
        public NAAC_User_PrivilegesDTO EditTab1(NAAC_User_PrivilegesDTO data)
        {
            try
            {
                data.GetEditTabOneDetails = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_Id == data.NAACSL_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.LogInformation("Error User Privileges : " + ex.Message);
            }
            return data;
        }
        public NAAC_User_PrivilegesDTO OnChangeCriteriaName(NAAC_User_PrivilegesDTO data)
        {
            try
            {
                if (data.TabFlag == "Tab2")
                {
                    data.GetTabTwoData = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_InstitutionTypeFlg == data.NAACSL_InstitutionTypeFlg
                     && a.NAACSL_ParentId == data.NAACSL_Id && a.NAACSL_ActiveFlag == true).Distinct().ToArray();


                    data.GetTabTwoDataOrder = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_InstitutionTypeFlg == data.NAACSL_InstitutionTypeFlg
                    && a.NAACSL_ParentId == data.NAACSL_Id && a.NAACSL_ActiveFlag == true).OrderBy(a => a.NAACSL_SLNoOrder).Distinct().ToArray();
                }

                if (data.TabFlag == "Tab3")
                {
                    data.GetTabThreeData = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_InstitutionTypeFlg == data.NAACSL_InstitutionTypeFlg
                     && a.NAACSL_ParentId == data.NAACSL_Id && a.NAACSL_ActiveFlag == true).Distinct().ToArray();


                    data.GetTabTwoDataOrder = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_InstitutionTypeFlg == data.NAACSL_InstitutionTypeFlg
                    && a.NAACSL_ParentId == data.NAACSL_Id && a.NAACSL_ActiveFlag == true).OrderBy(a => a.NAACSL_SLNoOrder).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_User_PrivilegesDTO SaveTab2(NAAC_User_PrivilegesDTO data)
        {
            try
            {
                data.returnmessage = "";
                data.returnval = false;

                if (data.NAACSL_Id > 0)
                {
                    var checkduplicate = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_SLNo == data.NAACSL_SLNo
                   && a.NAACSL_InstitutionTypeFlg == data.NAACSL_InstitutionTypeFlg && a.NAACSL_ParentId == data.ParentId
                   && a.NAACSL_Id != data.NAACSL_Id).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.returnmessage = "Duplicate";
                    }
                    else
                    {
                        SaveData(data);
                    }
                }
                else
                {
                    var checkduplicate = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_SLNo == data.NAACSL_SLNo
                    && a.NAACSL_InstitutionTypeFlg == data.NAACSL_InstitutionTypeFlg && a.NAACSL_ParentId == data.ParentId).ToList();

                    var checkorder = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_InstitutionTypeFlg == data.NAACSL_InstitutionTypeFlg
                    && a.NAACSL_ParentId == data.ParentId).Count();

                    if (checkduplicate.Count == 0)
                    {
                        data.checkorder = checkorder + 1;
                        SaveData(data);
                    }
                    else
                    {
                        data.returnmessage = "Duplicate";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.LogInformation("Error User Privileges : " + ex.Message);
            }
            return data;
        }
        public NAAC_User_PrivilegesDTO EditTab2(NAAC_User_PrivilegesDTO data)
        {
            try
            {
                data.GetEditTabTwoDetails = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_Id == data.NAACSL_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.LogInformation("Error User Privileges : " + ex.Message);
            }
            return data;
        }
        public NAAC_User_PrivilegesDTO SaveTab3(NAAC_User_PrivilegesDTO data)
        {
            try
            {
                data.returnmessage = "";
                data.returnval = false;

                if (data.NAACSL_Id > 0)
                {
                    var checkduplicate = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_SLNo == data.NAACSL_SLNo
                   && a.NAACSL_InstitutionTypeFlg == data.NAACSL_InstitutionTypeFlg && a.NAACSL_ParentId == data.ParentId
                   && a.NAACSL_Id != data.NAACSL_Id).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.returnmessage = "Duplicate";
                    }
                    else
                    {
                        SaveData(data);
                    }
                }
                else
                {
                    var checkduplicate = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_SLNo == data.NAACSL_SLNo
                    && a.NAACSL_InstitutionTypeFlg == data.NAACSL_InstitutionTypeFlg && a.NAACSL_ParentId == data.ParentId).ToList();

                    var checkorder = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_InstitutionTypeFlg == data.NAACSL_InstitutionTypeFlg
                    && a.NAACSL_ParentId == data.ParentId).Count();

                    if (checkduplicate.Count == 0)
                    {
                        data.checkorder = checkorder + 1;
                        SaveData(data);
                    }
                    else
                    {
                        data.returnmessage = "Duplicate";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.LogInformation("Error User Privileges : " + ex.Message);
            }
            return data;
        }
        public NAAC_User_PrivilegesDTO EditTab3(NAAC_User_PrivilegesDTO data)
        {
            try
            {
                data.GetEditTabThreeDetails = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_Id == data.NAACSL_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.LogInformation("Error User Privileges : " + ex.Message);
            }
            return data;
        }
        public NAAC_User_PrivilegesDTO OnChangeCriteriaNameLevelOne(NAAC_User_PrivilegesDTO data)
        {
            try
            {
                data.GetSavedPatentIdDetails = (from a in _context.NaacDocumentUploadDMO
                                                where (a.NAACSL_InstitutionTypeFlg == data.NAACSL_InstitutionTypeFlg && a.NAACSL_ParentId == data.NAACSL_Id)
                                                select new NAAC_User_PrivilegesDTO
                                                {
                                                    NAACSL_Id = a.NAACSL_Id,
                                                    NAACSL_SLNoOrder = a.NAACSL_SLNoOrder,
                                                    criterianame = a.NAACSL_SLNo + ":" + a.NAACSL_SLNoDescription,
                                                }).Distinct().OrderBy(a => a.NAACSL_SLNoOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.LogInformation("Error User Privileges : " + ex.Message);
            }
            return data;
        }
        public NAAC_User_PrivilegesDTO SaveData(NAAC_User_PrivilegesDTO data)
        {
            try
            {
                if (data.NAACSL_Id > 0)
                {
                    var result = _context.NaacDocumentUploadDMO.Single(a => a.NAACSL_InstitutionTypeFlg == data.NAACSL_InstitutionTypeFlg
                     && a.NAACSL_Id == data.NAACSL_Id);

                    result.NAACSL_SLNoDescription = data.NAACSL_SLNoDescription;
                    result.NAACSL_SLNo = data.NAACSL_SLNo;
                    result.NAACSL_SLNote = data.NAACSL_SLNote;
                    result.NAACSL_TextBoxFlg = data.NAACSL_TextBoxFlg;
                    result.NAACSL_UploadReq = data.NAACSL_UploadReq;
                    result.NAASCL_Template = data.NAASCL_Template;
                    result.NAACSL_Percentage = data.NAACSL_Percentage;
                    result.NAACSL_UpdatedDate = DateTime.Now;
                    result.NAACSL_UpdatedBy = data.userid;
                    _context.Update(result);
                }
                else
                {
                    NaacDocumentUploadDMO naacDocumentUploadDMO = new NaacDocumentUploadDMO
                    {
                        NAACSL_SLNo = data.NAACSL_SLNo,
                        NAACSL_SLNoDescription = data.NAACSL_SLNo,
                        NAACSL_ParentId = data.ParentId > 0 ? data.ParentId : 0,
                        NAACSL_SLNoOrder = data.checkorder,
                        NAACSL_SLNote = data.NAACSL_SLNote,
                        NAACSL_ActiveFlag = true,
                        NAACSL_CreatedBy = data.userid,
                        NAACSL_CreatedDate = DateTime.Now,
                        NAACSL_UpdatedBy = data.userid,
                        NAACSL_UpdatedDate = DateTime.Now,
                        NAASCL_Template = data.NAASCL_Template,
                        NAACSL_TextBoxFlg = data.NAACSL_TextBoxFlg,
                        NAACSL_Percentage = data.NAACSL_Percentage,
                        NAACSL_InstitutionTypeFlg = data.NAACSL_InstitutionTypeFlg,
                        NAACSL_UploadReq = data.NAACSL_UploadReq,
                    };

                    _context.Add(naacDocumentUploadDMO);
                }

                var i = _context.SaveChanges();
                if (i > 0)
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
    }
}
