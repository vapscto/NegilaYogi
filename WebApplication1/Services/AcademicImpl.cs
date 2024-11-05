using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebApplication1.Services
{
    public class AcademicImpl : Interfaces.AcademicInterface
    {
        private static ConcurrentDictionary<string, AcademicDTO> _login =
             new ConcurrentDictionary<string, AcademicDTO>();

        public AcademicContext _AcademicContext;
        ILogger<AcademicImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public AcademicImpl(AcademicContext academiccontext, ILogger<AcademicImpl> acdimpl, DomainModelMsSqlServerContext db)
        {
            _AcademicContext = academiccontext;
            _acdimpl = acdimpl;
            _db = db;
        }
        public AcademicDTO getallDetails(AcademicDTO acdmc)
        {
            try
            {
                var rolelist = _AcademicContext.MasterRoleType.Where(t => t.IVRMRT_Id == acdmc.roleId).ToList();

                var moId = _AcademicContext.institution.Where(m => m.MI_Id == acdmc.MI_Id).ToList();

                if (rolelist[0].IVRMRT_Role.Equals("Super Admin"))
                {
                    List<Institution> allInstitution = new List<Institution>();
                    allInstitution = _AcademicContext.institution.Where(d => d.MI_ActiveFlag == 1).ToList();
                    acdmc.institutionList = allInstitution.ToArray();

                    acdmc.AcademicList = (from m in _AcademicContext.Academic
                                          from n in _AcademicContext.institution
                                          where (m.MI_Id == n.MI_Id)
                                          select new AcademicDTO
                                          {
                                              ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                              ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                              ASMAY_From_Date = m.ASMAY_From_Date,
                                              ASMAY_Id = m.ASMAY_Id,
                                              ASMAY_Order = m.ASMAY_Order,
                                              ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                              ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                              ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                              ASMAY_To_Date = m.ASMAY_To_Date,
                                              CreatedDate = m.CreatedDate,
                                              Is_Active = m.Is_Active,
                                              MI_Id = m.MI_Id,
                                              UpdatedDate = m.UpdatedDate,
                                              ASMAY_Year = m.ASMAY_Year,
                                              MI_Name = n.MI_Name
                                          }).OrderByDescending(d => d.CreatedDate).ToArray();

                    if (acdmc.AcademicList.Length > 0)
                    {
                        acdmc.count = acdmc.AcademicList.Length;
                    }
                    else
                    {
                        acdmc.count = 0;
                    }

                }

                else if (rolelist[0].IVRMRT_Role.Equals("Admin"))
                {
                    List<Institution> allInstitution = new List<Institution>();
                    allInstitution = _AcademicContext.institution.Where(d => d.MI_ActiveFlag == 1 && d.MI_Id == acdmc.MI_Id).ToList();
                    acdmc.institutionList = allInstitution.ToArray();

                    acdmc.AcademicList = (from m in _AcademicContext.Academic
                                          from n in _AcademicContext.institution
                                          where (m.MI_Id == n.MI_Id && m.MI_Id == acdmc.MI_Id)
                                          select new AcademicDTO
                                          {
                                              ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                              ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                              ASMAY_From_Date = m.ASMAY_From_Date,
                                              ASMAY_Id = m.ASMAY_Id,
                                              ASMAY_Order = m.ASMAY_Order,
                                              ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                              ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                              ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                              ASMAY_To_Date = m.ASMAY_To_Date,
                                              CreatedDate = m.CreatedDate,
                                              Is_Active = m.Is_Active,
                                              MI_Id = m.MI_Id,
                                              UpdatedDate = m.UpdatedDate,
                                              ASMAY_Year = m.ASMAY_Year,
                                              MI_Name = n.MI_Name
                                          }).OrderByDescending(d => d.CreatedDate).ToArray();

                    if (acdmc.AcademicList.Length > 0)
                    {
                        acdmc.count = acdmc.AcademicList.Length;
                    }
                    else
                    {
                        acdmc.count = 0;
                    }
                }
                else
                {
                    List<Institution> allInstitution = new List<Institution>();
                    allInstitution = _AcademicContext.institution.Where(d => d.MI_ActiveFlag == 1 && d.MI_Id == acdmc.MI_Id).ToList();
                    acdmc.institutionList = allInstitution.ToArray();

                    acdmc.AcademicList = (from m in _AcademicContext.Academic
                                          from n in _AcademicContext.institution
                                          where (m.MI_Id == n.MI_Id && n.MI_Id == acdmc.MI_Id)
                                          select new AcademicDTO
                                          {
                                              ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                              ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                              ASMAY_From_Date = m.ASMAY_From_Date,
                                              ASMAY_Id = m.ASMAY_Id,
                                              ASMAY_Order = m.ASMAY_Order,
                                              ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                              ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                              ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                              ASMAY_To_Date = m.ASMAY_To_Date,
                                              CreatedDate = m.CreatedDate,
                                              Is_Active = m.Is_Active,
                                              MI_Id = m.MI_Id,
                                              UpdatedDate = m.UpdatedDate,
                                              ASMAY_Year = m.ASMAY_Year,
                                              MI_Name = n.MI_Name
                                          }).OrderByDescending(d => d.CreatedDate).ToArray();

                    if (acdmc.AcademicList.Length > 0)
                    {
                        acdmc.count = acdmc.AcademicList.Length;
                    }
                    else
                    {
                        acdmc.count = 0;
                    }
                }
                acdmc.getyear = _AcademicContext.Academic.Where(a => a.MI_Id == acdmc.MI_Id && a.ASMAY_Id == acdmc.ASMAY_Id).ToArray();
                acdmc.getallyear = _AcademicContext.Academic.Where(a => a.MI_Id == acdmc.MI_Id).OrderBy(b => b.ASMAY_Order).ToArray();
                acdmc.getallyearnew = _AcademicContext.Academic.Where(a => a.MI_Id == acdmc.MI_Id && a.Is_Active == true).OrderBy(b => b.ASMAY_Order).ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return acdmc;
        }

        public AcademicDTO getdetails(int id)
        {
            AcademicDTO acdmc = new AcademicDTO();
            try
            {
                List<MasterAcademic> lorg = new List<MasterAcademic>();
                lorg = _AcademicContext.Academic.AsNoTracking().Where(t => t.ASMAY_Id.Equals(id)).ToList();
                acdmc.AcademicList = lorg.ToArray();

                var getorder = lorg.FirstOrDefault().ASMAY_Order;

                var getmiid = lorg.FirstOrDefault().MI_Id;
                var lastyearorder = 0;
                if (getorder != 1)
                {
                    lastyearorder = getorder - 1;
                }
                else
                {
                    lastyearorder = getorder;
                }


                var getlastyear = _AcademicContext.Academic.Where(a => a.MI_Id == getmiid && a.ASMAY_Order == lastyearorder && a.Is_Active == true).ToArray();



                acdmc.geteditdetails = getlastyear;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return acdmc;
        }
        public AcademicDTO saveProsdet(AcademicDTO acd)
        {
            try
            {
                var check_accyear_assign = (from a in _AcademicContext.adm_m_student
                                            from b in _AcademicContext.Academic
                                            where (b.ASMAY_Id == a.ASMAY_Id && b.MI_Id == acd.MI_Id && a.ASMAY_Id == acd.ASMAY_Id && b.Is_Active == true)
                                            select new AcademicDTO
                                            {
                                                ASMAY_Id = b.ASMAY_Id
                                            }).ToList();

                var check_accyear_assign1 = (from a in _AcademicContext.adm_Y_student
                                             from b in _AcademicContext.Academic
                                             where (b.ASMAY_Id == a.ASMAY_Id && b.MI_Id == acd.MI_Id && a.ASMAY_Id == acd.ASMAY_Id && b.Is_Active == true)
                                             select new AcademicDTO
                                             {
                                                 ASMAY_Id = b.ASMAY_Id
                                             }).ToList();

                var check_accyear_assign2 = (from a in _AcademicContext.preadmission
                                             from b in _AcademicContext.Academic
                                             where (b.ASMAY_Id == a.ASMAY_Id && b.MI_Id == acd.MI_Id && a.ASMAY_Id == acd.ASMAY_Id)
                                             select new AcademicDTO
                                             {
                                                 ASMAY_Id = b.ASMAY_Id
                                             }).ToList();


                if (check_accyear_assign.Count > 0 || check_accyear_assign1.Count > 0 || check_accyear_assign2.Count > 0)
                {
                    MasterAcademic enqp = Mapper.Map<MasterAcademic>(acd);
                    if (enqp.ASMAY_Id > 0)
                    {
                        var checkorder = _AcademicContext.Academic.Where(d => d.MI_Id == acd.MI_Id && d.ASMAY_Id == acd.ASMAY_Id && d.ASMAY_Pre_ActiveFlag == 1).ToList();
                        if (checkorder.Count == 0)
                        {
                            acd.message = "Sorry...You Can't Edit This Record This Is Already Mapped With Student .";
                        }
                        else
                        {
                            var result = _AcademicContext.Academic.Single(t => t.ASMAY_Id == enqp.ASMAY_Id);
                            result.ASMAY_Cut_Of_Date = enqp.ASMAY_Cut_Of_Date;
                            result.ASMAY_PreAdm_F_Date = enqp.ASMAY_PreAdm_F_Date.Value.AddHours(acd.fhrors).AddMinutes(acd.fminutes).AddSeconds(acd.fsec);
                            result.ASMAY_PreAdm_T_Date = enqp.ASMAY_PreAdm_T_Date.Value.AddHours(acd.thrors).AddMinutes(acd.tminutes).AddSeconds(acd.tsec);
                            result.ASMAY_AcademicYearCode = enqp.ASMAY_AcademicYearCode;
                            result.ASMAY_NewAdmissionFlg = enqp.ASMAY_NewAdmissionFlg;
                            result.ASMAY_NewFlg = enqp.ASMAY_NewFlg;
                            result.ASMAY_ReggularFlg = enqp.ASMAY_ReggularFlg;
                            result.ASMAY_TransportEDate = enqp.ASMAY_TransportEDate;
                            result.ASMAY_TransportSDate = enqp.ASMAY_TransportSDate;
                            result.ASMAY_ReferenceDate = enqp.ASMAY_ReferenceDate;

                            result.ASMAY_RegularFeeFDate = enqp.ASMAY_RegularFeeFDate;
                            result.ASMAY_RegularFeeTDate = enqp.ASMAY_RegularFeeTDate;
                            result.ASMAY_AdvanceFeeDate = enqp.ASMAY_AdvanceFeeDate;
                            result.ASMAY_ArrearFeeDate = enqp.ASMAY_ArrearFeeDate;
                            result.ASMAY_From_Date = enqp.ASMAY_From_Date;
                            result.ASMAY_To_Date = enqp.ASMAY_To_Date;
                            result.CreatedDate = result.CreatedDate;
                            result.UpdatedDate = DateTime.Now;
                            result.MI_Id = enqp.MI_Id;
                            result.ASMAY_UpdatedBy = acd.userId;
                            _AcademicContext.Update(result);
                            var flag = _AcademicContext.SaveChanges();
                            if (flag == 1)
                            {
                                //acd.returnval = true;
                                //acd.messagesaveupdate = "Update";
                            }
                            else
                            {
                                //acd.returnval = false;
                                //acd.messagesaveupdate = "Update";
                            }
                        }
                    }


                    acd.message = "Sorry...You Can't Edit This Record This Is Already Mapped With Student .";
                }

                else
                {
                    _acdimpl.LogInformation("WebApplication1.Services/AcademicImpl/saveProsdet");
                    MasterAcademic enq = Mapper.Map<MasterAcademic>(acd);
                    if (enq.ASMAY_Id > 0)
                    {
                        var checkorder = _AcademicContext.Academic.Where(d => d.MI_Id == acd.MI_Id && d.ASMAY_Order == acd.ASMAY_Order && d.ASMAY_Id != acd.ASMAY_Id).ToList();

                        var checkordercode = _AcademicContext.Academic.Where(d => d.MI_Id == acd.MI_Id && d.ASMAY_AcademicYearCode == acd.ASMAY_AcademicYearCode && d.ASMAY_Id != acd.ASMAY_Id).ToList();

                        if (checkorder.Count > 0 || checkordercode.Count > 0)
                        {
                            acd.message = "Record Already Exist";
                        }
                        else
                        {
                            var result = _AcademicContext.Academic.Single(t => t.ASMAY_Id == enq.ASMAY_Id);
                            result.ASMAY_ActiveFlag = enq.ASMAY_ActiveFlag;
                            result.ASMAY_Cut_Of_Date = enq.ASMAY_Cut_Of_Date;
                            result.ASMAY_From_Date = enq.ASMAY_From_Date;
                            result.ASMAY_Order = enq.ASMAY_Order;
                            result.ASMAY_PreAdm_F_Date = enq.ASMAY_PreAdm_F_Date.Value.AddHours(acd.fhrors).AddMinutes(acd.fminutes).AddSeconds(acd.fsec);
                            result.ASMAY_PreAdm_T_Date = enq.ASMAY_PreAdm_T_Date.Value.AddHours(acd.thrors).AddMinutes(acd.tminutes).AddSeconds(acd.tsec);
                            result.ASMAY_Pre_ActiveFlag = enq.ASMAY_Pre_ActiveFlag;
                            result.ASMAY_To_Date = enq.ASMAY_To_Date;
                            result.CreatedDate = result.CreatedDate;
                            result.ASMAY_AcademicYearCode = enq.ASMAY_AcademicYearCode;
                            result.ASMAY_NewAdmissionFlg = enq.ASMAY_NewAdmissionFlg;
                            result.ASMAY_NewFlg = enq.ASMAY_NewFlg;
                            result.ASMAY_ReggularFlg = enq.ASMAY_ReggularFlg;
                            result.ASMAY_TransportEDate = enq.ASMAY_TransportEDate;
                            result.ASMAY_TransportSDate = enq.ASMAY_TransportSDate;
                            result.ASMAY_ReferenceDate = enq.ASMAY_ReferenceDate;
                            result.ASMAY_RegularFeeFDate = enq.ASMAY_RegularFeeFDate;
                            result.ASMAY_RegularFeeTDate = enq.ASMAY_RegularFeeTDate;
                            result.ASMAY_AdvanceFeeDate = enq.ASMAY_AdvanceFeeDate;
                            result.ASMAY_ArrearFeeDate = enq.ASMAY_ArrearFeeDate;
                            result.UpdatedDate = DateTime.Now;
                            var year = acd.ASMAY_From_Year + "-" + acd.ASMAY_To_Year;
                            result.ASMAY_Year = year;
                            result.MI_Id = enq.MI_Id;
                            result.ASMAY_UpdatedBy = acd.userId;
                            _AcademicContext.Update(result);
                            var flag = _AcademicContext.SaveChanges();
                            if (flag == 1)
                            {
                                acd.returnval = true;
                                acd.messagesaveupdate = "Update";
                            }
                            else
                            {
                                acd.returnval = false;
                                acd.messagesaveupdate = "Update";
                            }
                        }
                    }
                    else
                    {

                        var year = acd.ASMAY_From_Year + "-" + acd.ASMAY_To_Year;
                        var check = _AcademicContext.Academic.Where(y => y.ASMAY_Year == year && y.MI_Id == acd.MI_Id).ToList();
                        if (check.Count > 0)
                        {
                            acd.message = "Record Already Exist";
                        }
                        var checkorder = _AcademicContext.Academic.Where(d => d.MI_Id == acd.MI_Id && d.ASMAY_Order == acd.ASMAY_Order).ToList();
                        if (checkorder.Count > 0)
                        {
                            if (check.Count > 0)
                            {
                                acd.message = "Record Already Exist";
                            }
                            else
                            {
                                acd.message = "Record Already Exist";
                            }
                        }

                        var check1 = _AcademicContext.Academic.Where(y => y.ASMAY_AcademicYearCode == acd.ASMAY_AcademicYearCode && y.MI_Id == acd.MI_Id).ToList();
                        if (check1.Count > 0)
                        {
                            acd.message = "Record Already Exist";
                        }

                        if (check.Count == 0 && checkorder.Count == 0 && check1.Count == 0)
                        {
                            enq.ASMAY_Year = year;
                            enq.Is_Active = true;
                            enq.ASMAY_ActiveFlag = 1;
                            enq.ASMAY_PreAdm_F_Date = acd.ASMAY_PreAdm_F_Date.Value.AddHours(acd.fhrors).AddMinutes(acd.fminutes).AddSeconds(acd.fsec);
                            enq.ASMAY_PreAdm_T_Date = acd.ASMAY_PreAdm_T_Date.Value.AddHours(acd.thrors).AddMinutes(acd.tminutes).AddSeconds(acd.tsec);
                            enq.CreatedDate = DateTime.Now;
                            enq.UpdatedDate = DateTime.Now;
                            enq.ASMAY_CreatedBy = acd.userId;
                            enq.ASMAY_UpdatedBy = acd.userId;
                            _AcademicContext.Add(enq);
                            var flag = _AcademicContext.SaveChanges();
                            if (flag == 1)
                            {
                                acd.returnval = true;
                                acd.messagesaveupdate = "Save";
                            }
                            else
                            {
                                acd.returnval = false;
                                acd.messagesaveupdate = "Save";
                            }
                        }
                    }

                    var rolelist = _AcademicContext.MasterRoleType.Where(t => t.IVRMRT_Id == acd.roleId).ToList();

                    if (rolelist[0].IVRMRT_Role.Equals("Super Admin"))
                    {
                        acd.AcademicList = (from m in _AcademicContext.Academic
                                            from n in _AcademicContext.institution
                                            where (m.MI_Id == n.MI_Id)
                                            select new AcademicDTO
                                            {
                                                ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                ASMAY_From_Date = m.ASMAY_From_Date,
                                                ASMAY_Id = m.ASMAY_Id,
                                                ASMAY_Order = m.ASMAY_Order,
                                                ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                ASMAY_To_Date = m.ASMAY_To_Date,
                                                CreatedDate = m.CreatedDate,
                                                Is_Active = m.Is_Active,
                                                MI_Id = m.MI_Id,
                                                UpdatedDate = m.UpdatedDate,
                                                ASMAY_Year = m.ASMAY_Year,
                                                MI_Name = n.MI_Name
                                            }).OrderByDescending(d => d.CreatedDate).ToArray();

                        if (acd.AcademicList.Length > 0)
                        {
                            acd.count = acd.AcademicList.Length;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                    }
                    else
                    {
                        acd.AcademicList = (from m in _AcademicContext.Academic
                                            from n in _AcademicContext.institution
                                            where (m.MI_Id == n.MI_Id && m.MI_Id == acd.MI_Id)
                                            select new AcademicDTO
                                            {
                                                ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                ASMAY_From_Date = m.ASMAY_From_Date,
                                                ASMAY_Id = m.ASMAY_Id,
                                                ASMAY_Order = m.ASMAY_Order,
                                                ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                ASMAY_To_Date = m.ASMAY_To_Date,
                                                CreatedDate = m.CreatedDate,
                                                Is_Active = m.Is_Active,
                                                MI_Id = m.MI_Id,
                                                UpdatedDate = m.UpdatedDate,
                                                ASMAY_Year = m.ASMAY_Year,
                                                MI_Name = n.MI_Name
                                            }).OrderByDescending(d => d.CreatedDate).ToArray();

                        if (acd.AcademicList.Length > 0)
                        {
                            acd.count = acd.AcademicList.Length;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                _acdimpl.LogError(ee.Message);
                _acdimpl.LogDebug(ee.Message);
                Console.WriteLine(ee.Message);
            }

            return acd;
        }

        public AcademicDTO deleterec(AcademicDTO org)
        {
            List<MasterAcademic> lorg = new List<MasterAcademic>(); // Mapper.Map<Organisation>(org);            

            try
            {

                var check_accyear_assign = (from a in _AcademicContext.adm_m_student
                                            from b in _AcademicContext.Academic
                                            where (b.ASMAY_Id == a.ASMAY_Id && b.MI_Id == org.MI_Id && a.ASMAY_Id == org.ASMAY_Id && b.Is_Active == true)
                                            select new AcademicDTO
                                            {
                                                ASMAY_Id = b.ASMAY_Id
                                            }
                                                ).ToList();
                var check_accyear_assign1 = (from a in _AcademicContext.adm_Y_student
                                             from b in _AcademicContext.Academic
                                             where (b.ASMAY_Id == a.ASMAY_Id && b.MI_Id == org.MI_Id && a.ASMAY_Id == org.ASMAY_Id && b.Is_Active == true)
                                             select new AcademicDTO
                                             {
                                                 ASMAY_Id = b.ASMAY_Id
                                             }
                                        ).ToList();

                var check_accyear_assign2 = (from a in _AcademicContext.preadmission
                                             from b in _AcademicContext.Academic
                                             where (b.ASMAY_Id == a.ASMAY_Id && b.MI_Id == org.MI_Id && a.ASMAY_Id == org.ASMAY_Id)
                                             select new AcademicDTO
                                             {
                                                 ASMAY_Id = b.ASMAY_Id
                                             }
                                             ).ToList();
                var check_accyear_assign3 = (from a in _AcademicContext.Masterclasscategory
                                             from b in _AcademicContext.Academic
                                             where (b.ASMAY_Id == a.ASMAY_Id && b.MI_Id == org.MI_Id && a.ASMAY_Id == org.ASMAY_Id)
                                             select new AcademicDTO
                                             {
                                                 ASMAY_Id = b.ASMAY_Id
                                             }).ToList();

                if (check_accyear_assign.Count > 0 || check_accyear_assign1.Count > 0 || check_accyear_assign2.Count > 0 || check_accyear_assign3.Count > 0)
                {
                    org.message = "Sorry...You can not delete this record.Because this record is mapped with student or mapped with class category";
                }
                else
                {
                    lorg = _AcademicContext.Academic.Where(t => t.ASMAY_Id == org.ASMAY_Id).ToList();

                    if (lorg.Any())
                    {
                        _AcademicContext.Remove(lorg.ElementAt(0));
                        var flag = _AcademicContext.SaveChanges();
                        if (flag == 1)
                        {
                            org.returnval = true;
                        }
                        else
                        {
                            org.returnval = false;
                        }
                    }

                    var rolelist = _AcademicContext.MasterRoleType.Where(t => t.IVRMRT_Id == org.roleId).ToList();

                    if (rolelist[0].IVRMRT_Role.Equals("Super Admin"))
                    {
                        org.AcademicList = (from m in _AcademicContext.Academic
                                            from n in _AcademicContext.institution
                                            where (m.MI_Id == n.MI_Id)
                                            select new AcademicDTO
                                            {
                                                ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                ASMAY_From_Date = m.ASMAY_From_Date,
                                                ASMAY_Id = m.ASMAY_Id,
                                                ASMAY_Order = m.ASMAY_Order,
                                                ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                ASMAY_To_Date = m.ASMAY_To_Date,
                                                CreatedDate = m.CreatedDate,
                                                Is_Active = m.Is_Active,
                                                MI_Id = m.MI_Id,
                                                UpdatedDate = m.UpdatedDate,
                                                ASMAY_Year = m.ASMAY_Year,
                                                MI_Name = n.MI_Name
                                            }).OrderByDescending(d => d.CreatedDate).ToArray();

                        if (org.AcademicList.Length > 0)
                        {
                            org.count = org.AcademicList.Length;
                        }
                        else
                        {
                            org.count = 0;
                        }
                    }
                    else
                    {
                        org.AcademicList = (from m in _AcademicContext.Academic
                                            from n in _AcademicContext.institution
                                            where (m.MI_Id == n.MI_Id && m.MI_Id == lorg.FirstOrDefault().MI_Id)
                                            select new AcademicDTO
                                            {
                                                ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                ASMAY_From_Date = m.ASMAY_From_Date,
                                                ASMAY_Id = m.ASMAY_Id,
                                                ASMAY_Order = m.ASMAY_Order,
                                                ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                ASMAY_To_Date = m.ASMAY_To_Date,
                                                CreatedDate = m.CreatedDate,
                                                Is_Active = m.Is_Active,
                                                MI_Id = m.MI_Id,
                                                UpdatedDate = m.UpdatedDate,
                                                ASMAY_Year = m.ASMAY_Year,
                                                MI_Name = n.MI_Name
                                            }).OrderByDescending(d => d.CreatedDate).ToArray();

                        if (org.AcademicList.Length > 0)
                        {
                            org.count = org.AcademicList.Length;
                        }
                        else
                        {
                            org.count = 0;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                org.message = "Sorry...You can not delete this record.Because this record is mapped with student or mapped with class category";
                Console.WriteLine(ee.Message);
            }

            return org;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="acd"></param>
        /// <returns></returns>
        public AcademicDTO deactivate(AcademicDTO acd)
        {
            try
            {
                MasterAcademic enq = Mapper.Map<MasterAcademic>(acd);
                if (enq.ASMAY_Id > 0)
                {
                    var check_accyear_assign = (from a in _AcademicContext.adm_m_student
                                                from b in _AcademicContext.Academic
                                                where (b.ASMAY_Id == a.ASMAY_Id && b.MI_Id == acd.MI_Id && a.ASMAY_Id == acd.ASMAY_Id && b.Is_Active == true)
                                                select new AcademicDTO
                                                {
                                                    ASMAY_Id = b.ASMAY_Id
                                                }
                                               ).ToList();
                    var check_accyear_assign1 = (from a in _AcademicContext.adm_Y_student
                                                 from b in _AcademicContext.Academic
                                                 where (b.ASMAY_Id == a.ASMAY_Id && b.MI_Id == acd.MI_Id && a.ASMAY_Id == acd.ASMAY_Id && b.Is_Active == true)
                                                 select new AcademicDTO
                                                 {
                                                     ASMAY_Id = b.ASMAY_Id
                                                 }
                                            ).ToList();

                    if (check_accyear_assign.Count > 0 || check_accyear_assign1.Count > 0)
                    {
                        acd.message = "Sorry...You can not deactivate this record.Because this record is mapped with student";
                    }
                    else
                    {
                        var result = _AcademicContext.Academic.Single(t => t.ASMAY_Id == enq.ASMAY_Id);
                        if (result.Is_Active == true)
                        {
                            result.Is_Active = false;
                            result.ASMAY_ActiveFlag = 0;
                            result.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            result.Is_Active = true;
                            result.ASMAY_ActiveFlag = 1;
                            result.UpdatedDate = DateTime.Now;
                        }
                        _AcademicContext.Update(result);
                        var flag = _AcademicContext.SaveChanges();
                        if (flag == 1)
                        {
                            acd.returnval = true;
                        }
                        else
                        {
                            acd.returnval = false;
                        }

                        var rolelist = _AcademicContext.MasterRoleType.Where(t => t.IVRMRT_Id == acd.roleId).ToList();

                        if (rolelist[0].IVRMRT_Role.Equals("Super Admin"))
                        {
                            acd.AcademicList = (from m in _AcademicContext.Academic
                                                from n in _AcademicContext.institution
                                                where (m.MI_Id == n.MI_Id)
                                                select new AcademicDTO
                                                {
                                                    ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                    ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                    ASMAY_From_Date = m.ASMAY_From_Date,
                                                    ASMAY_Id = m.ASMAY_Id,
                                                    ASMAY_Order = m.ASMAY_Order,
                                                    ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                    ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                    ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                    ASMAY_To_Date = m.ASMAY_To_Date,
                                                    CreatedDate = m.CreatedDate,
                                                    Is_Active = m.Is_Active,
                                                    MI_Id = m.MI_Id,
                                                    UpdatedDate = m.UpdatedDate,
                                                    ASMAY_Year = m.ASMAY_Year,
                                                    MI_Name = n.MI_Name
                                                }).OrderByDescending(d => d.CreatedDate).ToArray();

                            if (acd.AcademicList.Length > 0)
                            {
                                acd.count = acd.AcademicList.Length;
                            }
                            else
                            {
                                acd.count = 0;
                            }
                        }
                        else
                        {
                            acd.AcademicList = (from m in _AcademicContext.Academic
                                                from n in _AcademicContext.institution
                                                where (m.MI_Id == n.MI_Id && m.MI_Id == result.MI_Id)
                                                select new AcademicDTO
                                                {
                                                    ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                    ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                    ASMAY_From_Date = m.ASMAY_From_Date,
                                                    ASMAY_Id = m.ASMAY_Id,
                                                    ASMAY_Order = m.ASMAY_Order,
                                                    ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                    ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                    ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                    ASMAY_To_Date = m.ASMAY_To_Date,
                                                    CreatedDate = m.CreatedDate,
                                                    Is_Active = m.Is_Active,
                                                    MI_Id = m.MI_Id,
                                                    UpdatedDate = m.UpdatedDate,
                                                    ASMAY_Year = m.ASMAY_Year,
                                                    MI_Name = n.MI_Name
                                                }).OrderByDescending(d => d.CreatedDate).ToArray();

                            if (acd.AcademicList.Length > 0)
                            {
                                acd.count = acd.AcademicList.Length;
                            }
                            else
                            {
                                acd.count = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }
        public AcademicDTO searchByColumn(AcademicDTO acd)
        {
            try
            {
                var rolelist = _AcademicContext.MasterRoleType.Where(t => t.IVRMRT_Id == acd.roleId).ToList();

                if (rolelist[0].IVRMRT_Role.Equals("Super Admin"))
                {
                    if (acd.roleId == 1)
                    {
                        acd.AcademicList = (from m in _AcademicContext.Academic
                                            from n in _AcademicContext.institution
                                            where (m.MI_Id == n.MI_Id && m.ASMAY_Year == acd.EnteredData)
                                            select new AcademicDTO
                                            {
                                                ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                ASMAY_From_Date = m.ASMAY_From_Date,
                                                ASMAY_Id = m.ASMAY_Id,
                                                ASMAY_Order = m.ASMAY_Order,
                                                ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                ASMAY_To_Date = m.ASMAY_To_Date,
                                                CreatedDate = m.CreatedDate,
                                                Is_Active = m.Is_Active,
                                                MI_Id = m.MI_Id,
                                                UpdatedDate = m.UpdatedDate,
                                                ASMAY_Year = m.ASMAY_Year,
                                                MI_Name = n.MI_Name
                                            }).OrderByDescending(d => d.CreatedDate).ToArray();

                        if (acd.AcademicList.Length > 0)
                        {
                            acd.count = acd.AcademicList.Length;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                    }
                    else
                    {
                        acd.AcademicList = (from m in _AcademicContext.Academic
                                            from n in _AcademicContext.institution
                                            where (m.MI_Id == n.MI_Id && m.MI_Id == acd.MI_Id && m.ASMAY_Year == acd.EnteredData)
                                            select new AcademicDTO
                                            {
                                                ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                ASMAY_From_Date = m.ASMAY_From_Date,
                                                ASMAY_Id = m.ASMAY_Id,
                                                ASMAY_Order = m.ASMAY_Order,
                                                ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                ASMAY_To_Date = m.ASMAY_To_Date,
                                                CreatedDate = m.CreatedDate,
                                                Is_Active = m.Is_Active,
                                                MI_Id = m.MI_Id,
                                                UpdatedDate = m.UpdatedDate,
                                                ASMAY_Year = m.ASMAY_Year,
                                                MI_Name = n.MI_Name
                                            }).OrderByDescending(d => d.CreatedDate).ToArray();

                        if (acd.AcademicList.Length > 0)
                        {
                            acd.count = acd.AcademicList.Length;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                    }
                }
                else if (acd.SearchColumn == "2")
                {
                    try
                    {
                        DateTime date = DateTime.ParseExact(acd.EnteredData, "dd/MM/yyyy",
                                    CultureInfo.InvariantCulture);

                        if (rolelist[0].IVRMRT_Role.Equals("Super Admin"))
                        {
                            acd.AcademicList = (from m in _AcademicContext.Academic
                                                from n in _AcademicContext.institution
                                                where (m.MI_Id == n.MI_Id && m.ASMAY_From_Date.Value.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd")))
                                                select new AcademicDTO
                                                {
                                                    ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                    ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                    ASMAY_From_Date = m.ASMAY_From_Date,
                                                    ASMAY_Id = m.ASMAY_Id,
                                                    ASMAY_Order = m.ASMAY_Order,
                                                    ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                    ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                    ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                    ASMAY_To_Date = m.ASMAY_To_Date,
                                                    CreatedDate = m.CreatedDate,
                                                    Is_Active = m.Is_Active,
                                                    MI_Id = m.MI_Id,
                                                    UpdatedDate = m.UpdatedDate,
                                                    ASMAY_Year = m.ASMAY_Year,
                                                    MI_Name = n.MI_Name
                                                }).OrderByDescending(d => d.CreatedDate).ToArray();

                            if (acd.AcademicList.Length > 0)
                            {
                                acd.count = acd.AcademicList.Length;
                            }
                            else
                            {
                                acd.count = 0;
                            }
                        }
                        else
                        {
                            acd.AcademicList = (from m in _AcademicContext.Academic
                                                from n in _AcademicContext.institution
                                                where (m.MI_Id == n.MI_Id && m.MI_Id == acd.MI_Id && m.ASMAY_From_Date.Value.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd")))
                                                select new AcademicDTO
                                                {
                                                    ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                    ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                    ASMAY_From_Date = m.ASMAY_From_Date,
                                                    ASMAY_Id = m.ASMAY_Id,
                                                    ASMAY_Order = m.ASMAY_Order,
                                                    ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                    ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                    ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                    ASMAY_To_Date = m.ASMAY_To_Date,
                                                    CreatedDate = m.CreatedDate,
                                                    Is_Active = m.Is_Active,
                                                    MI_Id = m.MI_Id,
                                                    UpdatedDate = m.UpdatedDate,
                                                    ASMAY_Year = m.ASMAY_Year,
                                                    MI_Name = n.MI_Name
                                                }).OrderByDescending(d => d.CreatedDate).ToArray();

                            if (acd.AcademicList.Length > 0)
                            {
                                acd.count = acd.AcademicList.Length;
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
                        acd.message = "Please Enter date in dd/MM/yyyy format";

                        if (rolelist[0].IVRMRT_Role.Equals("Super Admin"))
                        {
                            acd.AcademicList = (from m in _AcademicContext.Academic
                                                from n in _AcademicContext.institution
                                                where (m.MI_Id == n.MI_Id)
                                                select new AcademicDTO
                                                {
                                                    ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                    ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                    ASMAY_From_Date = m.ASMAY_From_Date,
                                                    ASMAY_Id = m.ASMAY_Id,
                                                    ASMAY_Order = m.ASMAY_Order,
                                                    ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                    ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                    ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                    ASMAY_To_Date = m.ASMAY_To_Date,
                                                    CreatedDate = m.CreatedDate,
                                                    Is_Active = m.Is_Active,
                                                    MI_Id = m.MI_Id,
                                                    UpdatedDate = m.UpdatedDate,
                                                    ASMAY_Year = m.ASMAY_Year,
                                                    MI_Name = n.MI_Name
                                                }).OrderByDescending(d => d.CreatedDate).ToArray();

                            if (acd.AcademicList.Length > 0)
                            {
                                acd.count = acd.AcademicList.Length;
                            }
                            else
                            {
                                acd.count = 0;
                            }
                        }
                        else
                        {
                            acd.AcademicList = (from m in _AcademicContext.Academic
                                                from n in _AcademicContext.institution
                                                where (m.MI_Id == n.MI_Id && m.MI_Id == acd.MI_Id)
                                                select new AcademicDTO
                                                {
                                                    ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                    ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                    ASMAY_From_Date = m.ASMAY_From_Date,
                                                    ASMAY_Id = m.ASMAY_Id,
                                                    ASMAY_Order = m.ASMAY_Order,
                                                    ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                    ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                    ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                    ASMAY_To_Date = m.ASMAY_To_Date,
                                                    CreatedDate = m.CreatedDate,
                                                    Is_Active = m.Is_Active,
                                                    MI_Id = m.MI_Id,
                                                    UpdatedDate = m.UpdatedDate,
                                                    ASMAY_Year = m.ASMAY_Year,
                                                    MI_Name = n.MI_Name
                                                }).OrderByDescending(d => d.CreatedDate).ToArray();

                            if (acd.AcademicList.Length > 0)
                            {
                                acd.count = acd.AcademicList.Length;
                            }
                            else
                            {
                                acd.count = 0;
                            }
                        }
                    }
                }
                else if (acd.SearchColumn == "3")
                {
                    try
                    {
                        DateTime date = DateTime.ParseExact(acd.EnteredData, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        if (rolelist[0].IVRMRT_Role.Equals("Super Admin"))
                        {
                            acd.AcademicList = (from m in _AcademicContext.Academic
                                                from n in _AcademicContext.institution
                                                where (m.MI_Id == n.MI_Id && m.ASMAY_To_Date.Value.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd")))
                                                select new AcademicDTO
                                                {
                                                    ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                    ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                    ASMAY_From_Date = m.ASMAY_From_Date,
                                                    ASMAY_Id = m.ASMAY_Id,
                                                    ASMAY_Order = m.ASMAY_Order,
                                                    ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                    ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                    ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                    ASMAY_To_Date = m.ASMAY_To_Date,
                                                    CreatedDate = m.CreatedDate,
                                                    Is_Active = m.Is_Active,
                                                    MI_Id = m.MI_Id,
                                                    UpdatedDate = m.UpdatedDate,
                                                    ASMAY_Year = m.ASMAY_Year,
                                                    MI_Name = n.MI_Name
                                                }).OrderByDescending(d => d.CreatedDate).ToArray();

                            if (acd.AcademicList.Length > 0)
                            {
                                acd.count = acd.AcademicList.Length;
                            }
                            else
                            {
                                acd.count = 0;
                            }
                        }
                        else
                        {
                            acd.AcademicList = (from m in _AcademicContext.Academic
                                                from n in _AcademicContext.institution
                                                where (m.MI_Id == n.MI_Id && m.MI_Id == acd.MI_Id && m.ASMAY_To_Date.Value.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd")))
                                                select new AcademicDTO
                                                {
                                                    ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                    ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                    ASMAY_From_Date = m.ASMAY_From_Date,
                                                    ASMAY_Id = m.ASMAY_Id,
                                                    ASMAY_Order = m.ASMAY_Order,
                                                    ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                    ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                    ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                    ASMAY_To_Date = m.ASMAY_To_Date,
                                                    CreatedDate = m.CreatedDate,
                                                    Is_Active = m.Is_Active,
                                                    MI_Id = m.MI_Id,
                                                    UpdatedDate = m.UpdatedDate,
                                                    ASMAY_Year = m.ASMAY_Year,
                                                    MI_Name = n.MI_Name
                                                }).OrderByDescending(d => d.CreatedDate).ToArray();

                            if (acd.AcademicList.Length > 0)
                            {
                                acd.count = acd.AcademicList.Length;
                            }
                            else
                            {
                                acd.count = 0;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        acd.message = "Please Enter date in dd/MM/yyyy format";

                        if (rolelist[0].IVRMRT_Role.Equals("Super Admin"))
                        {
                            acd.AcademicList = (from m in _AcademicContext.Academic
                                                from n in _AcademicContext.institution
                                                where (m.MI_Id == n.MI_Id)
                                                select new AcademicDTO
                                                {
                                                    ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                    ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                    ASMAY_From_Date = m.ASMAY_From_Date,
                                                    ASMAY_Id = m.ASMAY_Id,
                                                    ASMAY_Order = m.ASMAY_Order,
                                                    ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                    ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                    ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                    ASMAY_To_Date = m.ASMAY_To_Date,
                                                    CreatedDate = m.CreatedDate,
                                                    Is_Active = m.Is_Active,
                                                    MI_Id = m.MI_Id,
                                                    UpdatedDate = m.UpdatedDate,
                                                    ASMAY_Year = m.ASMAY_Year,
                                                    MI_Name = n.MI_Name
                                                }).OrderByDescending(d => d.CreatedDate).ToArray();

                            if (acd.AcademicList.Length > 0)
                            {
                                acd.count = acd.AcademicList.Length;
                            }
                            else
                            {
                                acd.count = 0;
                            }
                        }
                        else
                        {
                            acd.AcademicList = (from m in _AcademicContext.Academic
                                                from n in _AcademicContext.institution
                                                where (m.MI_Id == n.MI_Id && m.MI_Id == acd.MI_Id)
                                                select new AcademicDTO
                                                {
                                                    ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                    ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                    ASMAY_From_Date = m.ASMAY_From_Date,
                                                    ASMAY_Id = m.ASMAY_Id,
                                                    ASMAY_Order = m.ASMAY_Order,
                                                    ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                    ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                    ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                    ASMAY_To_Date = m.ASMAY_To_Date,
                                                    CreatedDate = m.CreatedDate,
                                                    Is_Active = m.Is_Active,
                                                    MI_Id = m.MI_Id,
                                                    UpdatedDate = m.UpdatedDate,
                                                    ASMAY_Year = m.ASMAY_Year,
                                                    MI_Name = n.MI_Name
                                                }).OrderByDescending(d => d.CreatedDate).ToArray();

                            if (acd.AcademicList.Length > 0)
                            {
                                acd.count = acd.AcademicList.Length;
                            }
                            else
                            {
                                acd.count = 0;
                            }
                        }
                    }
                }
                else if (acd.SearchColumn == "4")
                {
                    try
                    {
                        DateTime date = DateTime.ParseExact(acd.EnteredData, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        if (rolelist[0].IVRMRT_Role.Equals("Super Admin"))
                        {
                            acd.AcademicList = (from m in _AcademicContext.Academic
                                                from n in _AcademicContext.institution
                                                where (m.MI_Id == n.MI_Id && m.ASMAY_Cut_Of_Date.Value.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd")))
                                                select new AcademicDTO
                                                {
                                                    ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                    ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                    ASMAY_From_Date = m.ASMAY_From_Date,
                                                    ASMAY_Id = m.ASMAY_Id,
                                                    ASMAY_Order = m.ASMAY_Order,
                                                    ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                    ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                    ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                    ASMAY_To_Date = m.ASMAY_To_Date,
                                                    CreatedDate = m.CreatedDate,
                                                    Is_Active = m.Is_Active,
                                                    MI_Id = m.MI_Id,
                                                    UpdatedDate = m.UpdatedDate,
                                                    ASMAY_Year = m.ASMAY_Year,
                                                    MI_Name = n.MI_Name
                                                }).OrderByDescending(d => d.CreatedDate).ToArray();

                            if (acd.AcademicList.Length > 0)
                            {
                                acd.count = acd.AcademicList.Length;
                            }
                            else
                            {
                                acd.count = 0;
                            }
                        }
                        else
                        {
                            acd.AcademicList = (from m in _AcademicContext.Academic
                                                from n in _AcademicContext.institution
                                                where (m.MI_Id == n.MI_Id && m.MI_Id == acd.MI_Id && m.ASMAY_Cut_Of_Date.Value.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd")))
                                                select new AcademicDTO
                                                {
                                                    ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                    ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                    ASMAY_From_Date = m.ASMAY_From_Date,
                                                    ASMAY_Id = m.ASMAY_Id,
                                                    ASMAY_Order = m.ASMAY_Order,
                                                    ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                    ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                    ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                    ASMAY_To_Date = m.ASMAY_To_Date,
                                                    CreatedDate = m.CreatedDate,
                                                    Is_Active = m.Is_Active,
                                                    MI_Id = m.MI_Id,
                                                    UpdatedDate = m.UpdatedDate,
                                                    ASMAY_Year = m.ASMAY_Year,
                                                    MI_Name = n.MI_Name
                                                }).OrderByDescending(d => d.CreatedDate).ToArray();

                            if (acd.AcademicList.Length > 0)
                            {
                                acd.count = acd.AcademicList.Length;
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
                        acd.message = "Please Enter date in dd/MM/yyyy format";
                        if (rolelist[0].IVRMRT_Role.Equals("Super Admin"))
                        {
                            acd.AcademicList = (from m in _AcademicContext.Academic
                                                from n in _AcademicContext.institution
                                                where (m.MI_Id == n.MI_Id)
                                                select new AcademicDTO
                                                {
                                                    ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                    ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                    ASMAY_From_Date = m.ASMAY_From_Date,
                                                    ASMAY_Id = m.ASMAY_Id,
                                                    ASMAY_Order = m.ASMAY_Order,
                                                    ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                    ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                    ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                    ASMAY_To_Date = m.ASMAY_To_Date,
                                                    CreatedDate = m.CreatedDate,
                                                    Is_Active = m.Is_Active,
                                                    MI_Id = m.MI_Id,
                                                    UpdatedDate = m.UpdatedDate,
                                                    ASMAY_Year = m.ASMAY_Year,
                                                    MI_Name = n.MI_Name
                                                }).OrderByDescending(d => d.CreatedDate).ToArray();

                            if (acd.AcademicList.Length > 0)
                            {
                                acd.count = acd.AcademicList.Length;
                            }
                            else
                            {
                                acd.count = 0;
                            }
                        }
                        else
                        {
                            acd.AcademicList = (from m in _AcademicContext.Academic
                                                from n in _AcademicContext.institution
                                                where (m.MI_Id == n.MI_Id && m.MI_Id == acd.MI_Id)
                                                select new AcademicDTO
                                                {
                                                    ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                    ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                    ASMAY_From_Date = m.ASMAY_From_Date,
                                                    ASMAY_Id = m.ASMAY_Id,
                                                    ASMAY_Order = m.ASMAY_Order,
                                                    ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                    ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                    ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                    ASMAY_To_Date = m.ASMAY_To_Date,
                                                    CreatedDate = m.CreatedDate,
                                                    Is_Active = m.Is_Active,
                                                    MI_Id = m.MI_Id,
                                                    UpdatedDate = m.UpdatedDate,
                                                    ASMAY_Year = m.ASMAY_Year,
                                                    MI_Name = n.MI_Name
                                                }).OrderByDescending(d => d.CreatedDate).ToArray();

                            if (acd.AcademicList.Length > 0)
                            {
                                acd.count = acd.AcademicList.Length;
                            }
                            else
                            {
                                acd.count = 0;
                            }

                        }
                    }
                }
                else
                {
                    if (rolelist[0].IVRMRT_Role.Equals("Super Admin"))
                    {
                        acd.AcademicList = (from m in _AcademicContext.Academic
                                            from n in _AcademicContext.institution
                                            where (m.MI_Id == n.MI_Id)
                                            select new AcademicDTO
                                            {
                                                ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                ASMAY_From_Date = m.ASMAY_From_Date,
                                                ASMAY_Id = m.ASMAY_Id,
                                                ASMAY_Order = m.ASMAY_Order,
                                                ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                ASMAY_To_Date = m.ASMAY_To_Date,
                                                CreatedDate = m.CreatedDate,
                                                Is_Active = m.Is_Active,
                                                MI_Id = m.MI_Id,
                                                UpdatedDate = m.UpdatedDate,
                                                ASMAY_Year = m.ASMAY_Year,
                                                MI_Name = n.MI_Name
                                            }).OrderByDescending(d => d.CreatedDate).ToArray();

                        if (acd.AcademicList.Length > 0)
                        {
                            acd.count = acd.AcademicList.Length;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                    }
                    else
                    {
                        acd.AcademicList = (from m in _AcademicContext.Academic
                                            from n in _AcademicContext.institution
                                            where (m.MI_Id == n.MI_Id && m.MI_Id == acd.MI_Id)
                                            select new AcademicDTO
                                            {
                                                ASMAY_ActiveFlag = m.ASMAY_ActiveFlag,
                                                ASMAY_Cut_Of_Date = m.ASMAY_Cut_Of_Date,
                                                ASMAY_From_Date = m.ASMAY_From_Date,
                                                ASMAY_Id = m.ASMAY_Id,
                                                ASMAY_Order = m.ASMAY_Order,
                                                ASMAY_PreAdm_F_Date = m.ASMAY_PreAdm_F_Date,
                                                ASMAY_PreAdm_T_Date = m.ASMAY_PreAdm_T_Date,
                                                ASMAY_Pre_ActiveFlag = m.ASMAY_Pre_ActiveFlag,
                                                ASMAY_To_Date = m.ASMAY_To_Date,
                                                CreatedDate = m.CreatedDate,
                                                Is_Active = m.Is_Active,
                                                MI_Id = m.MI_Id,
                                                UpdatedDate = m.UpdatedDate,
                                                ASMAY_Year = m.ASMAY_Year,
                                                MI_Name = n.MI_Name
                                            }).OrderByDescending(d => d.CreatedDate).ToArray();

                        if (acd.AcademicList.Length > 0)
                        {
                            acd.count = acd.AcademicList.Length;
                        }
                        else
                        {
                            acd.count = 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return acd;
        }
        public AcademicDTO saveorder(AcademicDTO acd)
        {
            try
            {
                int id = 0;
                for (int i = 0; i < acd.yearorder.Count; i++)
                {
                    var reult = _AcademicContext.Academic.Single(t => t.MI_Id == acd.MI_Id && t.ASMAY_Id == acd.yearorder[i].ASMAY_Id);
                    id = id + 1;

                    if (i == 0)
                    {
                        reult.ASMAY_Order = id;
                    }
                    else
                    {
                        reult.ASMAY_Order = id;
                    }
                    reult.ASMAY_UpdatedBy = acd.userId;
                    reult.UpdatedDate = DateTime.UtcNow;
                    _db.Update(reult);
                    var flag = _db.SaveChanges();
                    if (flag > 0)
                    {
                        acd.returnval = true;
                    }
                    else
                    {
                        acd.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return acd;
        }

    }
}
