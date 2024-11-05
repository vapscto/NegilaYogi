using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.College.COE;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vapstech.College.COE;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.College.COE;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace COEServiceHub.com.vaps.Services
{
    public class ClgCOEMasterImpl : Interfaces.ClgCOEMasterInterface
    {
        private static ConcurrentDictionary<string, ClgMasterCOEDTO> _login =
       new ConcurrentDictionary<string, ClgMasterCOEDTO>();


        public ClgCOEContext _ClgCOEContext;
        public ClgCOEMasterImpl(ClgCOEContext coe)
        {
            _ClgCOEContext = coe;
        }

        public ClgMasterCOEDTO getdetails(ClgMasterCOEDTO data)
        {
            try
            {
                List<SMS_MAIL_PARAMETER_DMO> paramsall = new List<SMS_MAIL_PARAMETER_DMO>();
                paramsall = _ClgCOEContext.SMS_MAIL_PARAMETER_DMO.ToList();
                data.parameterlist = paramsall.Distinct().ToArray();

                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _ClgCOEContext.AcademicYear.Where(y => y.MI_Id == data.MI_Id && y.Is_Active == true).ToList();
                data.yearlist = year.Distinct().ToArray();

                List<COE_Master_EventsDMO> m_events = new List<COE_Master_EventsDMO>();
                m_events = _ClgCOEContext.COE_Master_EventsDMO.Where(e => e.MI_Id == data.MI_Id).ToList();
                data.master_eventlist = m_events.ToArray();

                List<COE_Master_EventsDMO> events = new List<COE_Master_EventsDMO>();
                m_events = _ClgCOEContext.COE_Master_EventsDMO.Where(e => e.MI_Id == data.MI_Id && e.COEME_ActiveFlag == true).OrderBy(e => e.COEME_EventName).ToList();
                data.eventlist_act = m_events.ToArray();

                List<HR_Master_Department> stf_types = new List<HR_Master_Department>();
                stf_types = _ClgCOEContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).ToList();
                data.stafftypelist = stf_types.Distinct().ToArray();

                data.mapped_eventlist = (from a in _ClgCOEContext.COE_Master_EventsDMO
                                         from b in _ClgCOEContext.COE_EventsDMO
                                         from c in _ClgCOEContext.AcademicYear
                                         where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.COEME_Id == b.COEME_Id && b.ASMAY_Id == c.ASMAY_Id && b.MI_Id == c.MI_Id)
                                         select new ClgMasterCOEDTO
                                         {
                                             ASMAY_Year = c.ASMAY_Year,
                                             COEE_Id = b.COEE_Id,
                                             COEME_Id = a.COEME_Id,
                                             COEME_EventName = a.COEME_EventName,
                                             COEE_EStartDate = b.COEE_EStartDate,
                                             COEE_EEndDate = b.COEE_EEndDate,
                                             COEE_ActiveFlag = b.COEE_ActiveFlag,
                                         }
                                  ).ToArray();
                data.course_list = (from a in _ClgCOEContext.CLG_Adm_College_AY_CourseDMO

                                    from c in _ClgCOEContext.AcademicYear
                                    from d in _ClgCOEContext.MasterCourseDMO
                                    where (a.MI_Id == c.MI_Id && a.AMCO_Id == d.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && d.AMCO_ActiveFlag == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                    select new ClgMasterCOEDTO
                                    {
                                        AMCO_Id = d.AMCO_Id,
                                        AMCO_CourseName = d.AMCO_CourseName,
                                        AMCO_CourseCode = d.AMCO_CourseCode,
                                        AMCO_CourseFlag = d.AMCO_CourseFlag,
                                        AMCO_ActiveFlag = d.AMCO_ActiveFlag,
                                        AMCO_Order = d.AMCO_Order
                                    }
                            ).Distinct().OrderBy(c => c.AMCO_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public ClgMasterCOEDTO courseselect(ClgMasterCOEDTO data)
        {
            try
            {
                List<long> cours_ids = new List<long>();
                foreach (var item in data.selected_courselist)
                {
                    cours_ids.Add(item.AMCO_Id);
                }

                data.branch_list = (from a in _ClgCOEContext.CLG_Adm_College_AY_CourseDMO
                                    from b in _ClgCOEContext.CLG_Adm_College_AY_Course_BranchDMO
                                    from c in _ClgCOEContext.AcademicYear
                                    from d in _ClgCOEContext.ClgMasterBranchDMO
                                    where (a.ACAYC_Id == b.ACAYC_Id && a.MI_Id == c.MI_Id && a.ASMAY_Id == c.ASMAY_Id && b.AMB_Id == d.AMB_Id && d.AMB_ActiveFlag == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && cours_ids.Contains(a.AMCO_Id))
                                    select new ClgMasterCOEDTO
                                    {
                                        AMB_Id = d.AMB_Id,
                                        AMB_BranchName = d.AMB_BranchName,
                                        AMB_BranchCode = d.AMB_BranchCode,
                                        AMB_ActiveFlag = d.AMB_ActiveFlag,
                                        AMB_Order = d.AMB_Order
                                    }
                       ).Distinct().OrderBy(c => c.AMB_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ClgMasterCOEDTO branchselect(ClgMasterCOEDTO data)
        {
            try
            {
                List<long> ids = new List<long>();
                if (data.brancharray != null)
                {
                    foreach (var c in data.brancharray)
                    {
                        ids.Add(c.AMB_Id);
                    }
                }
                data.sem_list = (from a in _ClgCOEContext.CLG_Adm_College_AY_CourseDMO
                                 from b in _ClgCOEContext.CLG_Adm_College_AY_Course_BranchDMO
                                 from c in _ClgCOEContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                 from d in _ClgCOEContext.AcademicYear
                                 from e in _ClgCOEContext.CLG_Adm_Master_SemesterDMO
                                 where (a.ACAYC_Id == b.ACAYC_Id && b.ACAYCB_Id == c.ACAYCB_Id && a.MI_Id == c.MI_Id && a.ASMAY_Id == d.ASMAY_Id && c.AMSE_Id == e.AMSE_Id && e.AMSE_ActiveFlg == true && c.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && ids.Contains(b.AMB_Id))
                                 select new ClgMasterCOEDTO
                                 {
                                     AMSE_Id = e.AMSE_Id,
                                     AMSE_SEMName = e.AMSE_SEMName,
                                     AMSE_SEMCode = e.AMSE_SEMCode,
                                     AMSE_SEMOrder = e.AMSE_SEMOrder
                                 }
                       ).Distinct().OrderBy(c => c.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ClgMasterCOEDTO savedetail1(ClgMasterCOEDTO _category)
        {
            COE_Master_EventsDMO objpge = Mapper.Map<COE_Master_EventsDMO>(_category);
            try
            {

                if (objpge.COEME_Id > 0)
                {

                    var resultCount = _ClgCOEContext.COE_Master_EventsDMO.Where(t => t.COEME_EventName == objpge.COEME_EventName && t.MI_Id == objpge.MI_Id && t.COEME_Id != objpge.COEME_Id).Count();

                    if (resultCount == 0)
                    {
                        var result = _ClgCOEContext.COE_Master_EventsDMO.Single(t => t.COEME_Id == objpge.COEME_Id && t.MI_Id == objpge.MI_Id);

                        result.COEME_EventName = objpge.COEME_EventName;
                        result.COEME_EventDesc = objpge.COEME_EventDesc;
                        result.COEME_SMSMessage = objpge.COEME_SMSMessage;
                        result.COEME_MailSubject = objpge.COEME_MailSubject;
                        result.COEME_MailHeader = objpge.COEME_MailHeader;
                        result.COEME_MailFooter = objpge.COEME_MailFooter;
                        result.COEME_Mail_Message = objpge.COEME_Mail_Message;
                        result.COEME_MailHTMLTemplate = objpge.COEME_MailHTMLTemplate;
                        result.COEME_ActiveFlag = true;


                        result.UpdatedDate = DateTime.Now;
                        _ClgCOEContext.Update(result);
                        var contactExists = _ClgCOEContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                    else
                    {
                        _category.returnduplicatestatus = "Duplicate";
                        return _category;
                    }
                }
                else
                {

                    var result = _ClgCOEContext.COE_Master_EventsDMO.Where(t => t.COEME_EventName == objpge.COEME_EventName && t.MI_Id == objpge.MI_Id).Count();

                    if (result > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else if (result == 0)
                    {
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        objpge.COEME_ActiveFlag = true;
                        _ClgCOEContext.Add(objpge);
                        var contactExists = _ClgCOEContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                }

                List<COE_Master_EventsDMO> m_events = new List<COE_Master_EventsDMO>();
                m_events = _ClgCOEContext.COE_Master_EventsDMO.Where(e => e.MI_Id == _category.MI_Id).ToList();
                _category.master_eventlist = m_events.ToArray();
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public ClgMasterCOEDTO savedetail2(ClgMasterCOEDTO _category)
        {
            COE_EventsDMO objpge = Mapper.Map<COE_EventsDMO>(_category);

            try
            {

                if (objpge.COEE_Id > 0)
                {
                    var resultCount = _ClgCOEContext.COE_EventsDMO.Where(t => t.COEME_Id == objpge.COEME_Id && t.MI_Id == objpge.MI_Id && t.ASMAY_Id == objpge.ASMAY_Id && t.COEE_Id != objpge.COEE_Id && t.COEE_ActiveFlag == true).Count();

                    if (resultCount == 0)
                    {
                        var result = _ClgCOEContext.COE_EventsDMO.Single(t => t.COEE_Id == objpge.COEE_Id && t.MI_Id == objpge.MI_Id);

                        result.ASMAY_Id = objpge.ASMAY_Id;
                        result.COEME_Id = objpge.COEME_Id;
                        result.COEE_EStartDate = objpge.COEE_EStartDate;
                        result.COEE_EEndDate = objpge.COEE_EEndDate;
                        result.COEE_EStartTime = objpge.COEE_EStartTime;
                        result.COEE_EEndTime = objpge.COEE_EEndTime;
                        result.COEE_SMSMessage = objpge.COEE_SMSMessage;
                        result.COEE_SMSActiveFlag = objpge.COEE_SMSActiveFlag;
                        result.COEE_MailSubject = objpge.COEE_MailSubject;
                        result.COEE_MailHeader = objpge.COEE_MailHeader;

                        result.COEE_MailFooter = objpge.COEE_MailFooter;
                        result.COEE_Mail_Message = objpge.COEE_Mail_Message;
                        result.COEE_MailHTMLTemplate = objpge.COEE_MailHTMLTemplate;
                        result.COEE_MailActiveFlag = objpge.COEE_MailActiveFlag;
                        result.COEE_ReminderDate = objpge.COEE_ReminderDate;
                        result.COEE_RepeatFlag = objpge.COEE_RepeatFlag;
                        result.COEE_ReminderSchedule = objpge.COEE_ReminderSchedule;
                        result.COEE_StudentFlag = objpge.COEE_StudentFlag;
                        result.COEE_AlumniFlag = objpge.COEE_AlumniFlag;
                        result.COEE_EmployeeFlag = objpge.COEE_EmployeeFlag;
                        result.COEE_ManagementFlag = objpge.COEE_ManagementFlag;
                        result.COEE_OtherFlag = objpge.COEE_OtherFlag;


                        result.UpdatedDate = DateTime.Now;
                        result.COEE_ActiveFlag = true;
                        result.COEE_HolidayFlag = false;
                        _ClgCOEContext.Update(result);

                        delete_event_classes(objpge.COEE_Id);

                        if (objpge.COEE_StudentFlag == true)
                        {

                            foreach (var act1 in _category.selected_courselist)
                            {
                                foreach (var act11 in _category.brancharray)
                                {
                                    foreach (var act12 in _category.semesterarray)
                                    {
                                        COE_Events_CourseBranchDMO objpge1 = Mapper.Map<COE_Events_CourseBranchDMO>(_category);

                                        objpge1.AMCO_Id = act1.AMCO_Id;
                                        objpge1.AMB_Id = act11.AMB_Id;
                                        objpge1.AMSE_Id = act12.AMSE_Id;
                                        objpge1.COEECB_ActiceFlg = true;
                                        objpge1.CreatedDate = DateTime.Now;
                                        objpge1.UpdatedDate = DateTime.Now;
                                        _ClgCOEContext.Add(objpge1);
                                    }

                                }
                            }
                        }

                        delete_event_emps(objpge.COEE_Id);

                        if (objpge.COEE_EmployeeFlag == true)
                        {
                            foreach (var act2 in _category.emp_type_list)
                            {
                                COE_Events_EmployeesDMO objpge2 = Mapper.Map<COE_Events_EmployeesDMO>(_category);
                                objpge2.HRMGT_Id = act2.HRMD_Id;
                                objpge2.CreatedDate = DateTime.Now;
                                objpge2.UpdatedDate = DateTime.Now;
                                _ClgCOEContext.Add(objpge2);

                            }
                        }

                        delete_event_othr_mobs(objpge.COEE_Id);

                        if (objpge.COEE_OtherFlag == true)
                        {
                            foreach (var act5 in _category.others_list)
                            {
                                COE_Events_OthersDMO objpge5 = Mapper.Map<COE_Events_OthersDMO>(_category);
                                objpge5.COEEO_MobileNo = act5.COEEO_MobileNo;
                                objpge5.CreatedDate = DateTime.Now;
                                objpge5.UpdatedDate = DateTime.Now;
                                objpge5.COEEO_Emailid = act5.COEEO_Emailid;
                                objpge5.COEEO_Name = act5.COEEO_Name;
                                _ClgCOEContext.Add(objpge5);

                            }
                        }


                        if (_category.images_list.Length > 0)
                        {
                            foreach (var act3 in _category.images_list)
                            {
                                COE_Events_ImagesDMO objpge3 = Mapper.Map<COE_Events_ImagesDMO>(_category);
                                objpge3.COEEI_Images = act3;
                                objpge3.CreatedDate = DateTime.Now;
                                objpge3.UpdatedDate = DateTime.Now;
                                _ClgCOEContext.Add(objpge3);

                            }
                        }


                        if (_category.videos_list.Length > 0)
                        {
                            foreach (var act4 in _category.videos_list)
                            {
                                COE_Events_VideosDMO objpge4 = Mapper.Map<COE_Events_VideosDMO>(_category);
                                objpge4.COEEV_Videos = act4;
                                objpge4.CreatedDate = DateTime.Now;
                                objpge4.UpdatedDate = DateTime.Now;
                                _ClgCOEContext.Add(objpge4);

                            }
                        }

                        var contactExists = _ClgCOEContext.SaveChanges();
                        if (contactExists >= 1)
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                    else
                    {
                        _category.returnduplicatestatus = "Duplicate";
                        return _category;
                    }
                }
                else
                {
                    var result = _ClgCOEContext.COE_EventsDMO.Where(t => t.COEME_Id == objpge.COEME_Id && t.MI_Id == objpge.MI_Id && t.ASMAY_Id == objpge.ASMAY_Id && t.COEE_ActiveFlag == true).Count();

                    if (result > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else if (result == 0)
                    {
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        objpge.COEE_ActiveFlag = true;
                        objpge.COEE_HolidayFlag = false;

                        _ClgCOEContext.Add(objpge);
                        _category.COEE_Id = objpge.COEE_Id;


                        if (objpge.COEE_StudentFlag == true)
                        {
                            foreach (var act1 in _category.selected_courselist)
                            {
                                foreach (var act11 in _category.brancharray)
                                {
                                    foreach (var act12 in _category.semesterarray)
                                    {
                                        COE_Events_CourseBranchDMO objpge1 = Mapper.Map<COE_Events_CourseBranchDMO>(_category);

                                        objpge1.AMCO_Id = act1.AMCO_Id;
                                        objpge1.AMB_Id = act11.AMB_Id;
                                        objpge1.AMSE_Id = act12.AMSE_Id;
                                        objpge1.COEECB_ActiceFlg = true;
                                        objpge1.CreatedDate = DateTime.Now;
                                        objpge1.UpdatedDate = DateTime.Now;
                                        _ClgCOEContext.Add(objpge1);
                                    }
                                }
                            }
                            //foreach (var act1 in _category.stu_class_list)
                            //{
                            //    COE_Events_ClassesDMO objpge1 = Mapper.Map<COE_Events_ClassesDMO>(_category);

                            //    objpge1.ASMCL_Id = act1.ASMCL_Id;
                            //    objpge1.CreatedDate = DateTime.Now;
                            //    objpge1.UpdatedDate = DateTime.Now;
                            //    _ClgCOEContext.Add(objpge1);

                            //}
                        }
                        //  }
                        if (objpge.COEE_EmployeeFlag == true)
                        {
                            foreach (var act2 in _category.emp_type_list)
                            {
                                COE_Events_EmployeesDMO objpge2 = Mapper.Map<COE_Events_EmployeesDMO>(_category);
                                objpge2.HRMGT_Id = act2.HRMD_Id;
                                objpge2.CreatedDate = DateTime.Now;
                                objpge2.UpdatedDate = DateTime.Now;
                                _ClgCOEContext.Add(objpge2);

                            }
                        }
                        if (objpge.COEE_OtherFlag == true)
                        {
                            foreach (var act5 in _category.others_list)
                            {
                                COE_Events_OthersDMO objpge5 = Mapper.Map<COE_Events_OthersDMO>(_category);
                                objpge5.COEEO_MobileNo = act5.COEEO_MobileNo;
                                objpge5.CreatedDate = DateTime.Now;
                                objpge5.UpdatedDate = DateTime.Now;
                                objpge5.COEEO_Emailid = act5.COEEO_Emailid;
                                objpge5.COEEO_Name = act5.COEEO_Name;
                                _ClgCOEContext.Add(objpge5);

                            }
                        }

                        if (_category.images_list.Length > 0)
                        {
                            foreach (var act3 in _category.images_list)
                            {
                                COE_Events_ImagesDMO objpge3 = Mapper.Map<COE_Events_ImagesDMO>(_category);
                                objpge3.COEEI_Images = act3;
                                objpge3.CreatedDate = DateTime.Now;
                                objpge3.UpdatedDate = DateTime.Now;
                                _ClgCOEContext.Add(objpge3);

                            }
                        }
                        if (_category.videos_list.Length > 0)
                        {
                            foreach (var act4 in _category.videos_list)
                            {
                                COE_Events_VideosDMO objpge4 = Mapper.Map<COE_Events_VideosDMO>(_category);
                                objpge4.COEEV_Videos = act4;
                                objpge4.CreatedDate = DateTime.Now;
                                objpge4.UpdatedDate = DateTime.Now;
                                _ClgCOEContext.Add(objpge4);

                            }
                        }

                        var contactExists = _ClgCOEContext.SaveChanges();
                        if (contactExists >= 1)
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                }


                _category.mapped_eventlist = (from a in _ClgCOEContext.COE_Master_EventsDMO
                                              from b in _ClgCOEContext.COE_EventsDMO
                                              from c in _ClgCOEContext.AcademicYear
                                              where (a.MI_Id == _category.MI_Id && a.MI_Id == b.MI_Id && a.COEME_Id == b.COEME_Id && b.ASMAY_Id == c.ASMAY_Id && b.MI_Id == c.MI_Id)
                                              select new ClgMasterCOEDTO
                                              {
                                                  ASMAY_Year = c.ASMAY_Year,
                                                  COEE_Id = b.COEE_Id,
                                                  COEME_Id = a.COEME_Id,
                                                  COEME_EventName = a.COEME_EventName,
                                                  COEE_EStartDate = b.COEE_EStartDate,
                                                  COEE_EEndDate = b.COEE_EEndDate,
                                                  COEE_ActiveFlag = b.COEE_ActiveFlag,
                                              }
                                  ).ToArray();
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return _category;
        }




        public ClgMasterCOEDTO geteventdetails(ClgMasterCOEDTO _category)
        {

            try
            {

                List<COE_Master_EventsDMO> s_m_events = new List<COE_Master_EventsDMO>();
                s_m_events = _ClgCOEContext.COE_Master_EventsDMO.Where(e => e.MI_Id == _category.MI_Id && e.COEME_Id == _category.COEME_Id).ToList();
                _category.selected_master_event = s_m_events.ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;

        }


        public ClgMasterCOEDTO getalldetailsviewrecords1(int id)
        {
            ClgMasterCOEDTO page = new ClgMasterCOEDTO();
            try
            {
                List<COE_Master_EventsDMO> events_m = new List<COE_Master_EventsDMO>();
                events_m = _ClgCOEContext.COE_Master_EventsDMO.Where(e => e.COEME_Id == id).ToList();
                page.edit_m_event = events_m.ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public ClgMasterCOEDTO getalldetailsviewrecords2(int id)
        {
            ClgMasterCOEDTO page = new ClgMasterCOEDTO();
            try
            {
                List<COE_EventsDMO> events_map = new List<COE_EventsDMO>();
                events_map = _ClgCOEContext.COE_EventsDMO.Where(e => e.COEE_Id == id).ToList();
                page.edit_map_event = events_map.ToArray();
                var m_id = events_map[0].COEME_Id;
                List<COE_Master_EventsDMO> events_m = new List<COE_Master_EventsDMO>();
                events_m = _ClgCOEContext.COE_Master_EventsDMO.Where(e => e.COEME_Id == m_id).ToList();
                page.edit_m_event = events_m.ToArray();


                //page.edit_stu_class_list = (from a in _ClgCOEContext.COE_Events_ClassesDMO
                //                            from b in _ClgCOEContext.AdmissionClass
                //                            where (a.COEE_Id == id && a.ASMCL_Id == b.ASMCL_Id)
                //                            select new ClgMasterCOEDTO
                //                            {
                //                                COEEC_Id = a.COEEC_Id,
                //                                COEE_Id = a.COEE_Id,
                //                                //    ASMCL_Id = a.ASMCL_Id,
                //                                // ASMCL_ClassName = b.ASMCL_ClassName,
                //                            }
                //                          ).Distinct().ToArray();

                page.edit_course_list = (from a in _ClgCOEContext.MasterCourseDMO
                                         from b in _ClgCOEContext.COE_Events_CourseBranchDMO
                                         where (a.AMCO_Id == b.AMCO_Id && a.MI_Id == id)
                                         select new ClgMasterCOEDTO
                                         {
                                             AMCO_Id = a.AMCO_Id,
                                             AMCO_CourseName = a.AMCO_CourseName
                                         }
                          ).Distinct().OrderBy(c => c.COEECB_Id).ToArray();

                page.edit_branchSem_list = (from a in _ClgCOEContext.MasterCourseDMO
                                            from b in _ClgCOEContext.ClgMasterBranchDMO
                                            from c in _ClgCOEContext.CLG_Adm_Master_SemesterDMO
                                            from d in _ClgCOEContext.COE_Events_CourseBranchDMO
                                            from e in _ClgCOEContext.COE_EventsDMO
                                            where (a.AMCO_Id == d.AMCO_Id && b.AMB_Id == d.AMB_Id && d.COEE_Id == e.COEE_Id && c.AMSE_Id == d.AMSE_Id && a.MI_Id == events_m.FirstOrDefault().MI_Id && e.COEE_Id == id)
                                            select new ClgMasterCOEDTO
                                            {
                                                COEECB_Id = d.COEECB_Id,
                                                COEE_Id = e.COEE_Id,
                                                AMCO_Id = a.AMCO_Id,
                                                AMB_Id = b.AMB_Id,
                                                AMSE_Id = c.AMSE_Id,
                                                AMCO_CourseName = a.AMCO_CourseName,
                                                AMB_BranchName = b.AMB_BranchName,
                                                AMSE_SEMName = c.AMSE_SEMName
                                            }
                       ).Distinct().OrderBy(c => c.COEECB_Id).ToArray();



                page.edit_emp_type_list = (from a in _ClgCOEContext.COE_Events_EmployeesDMO
                                           from b in _ClgCOEContext.HR_Master_Department
                                           where (a.COEE_Id == id && a.HRMGT_Id == b.HRMD_Id)
                                           select new ClgMasterCOEDTO
                                           {
                                               COEEE_Id = a.COEEE_Id,
                                               COEE_Id = a.COEE_Id,
                                               HRMD_Id = a.HRMGT_Id,
                                               HRMD_DepartmentName = b.HRMD_DepartmentName,

                                           }
                                        ).Distinct().ToArray();

                List<COE_Events_OthersDMO> others_map = new List<COE_Events_OthersDMO>();
                others_map = _ClgCOEContext.COE_Events_OthersDMO.Where(e => e.COEE_Id == id).ToList();
                page.edit_oth_mobilenos_list = others_map.ToArray();

                List<COE_Events_ImagesDMO> images_map = new List<COE_Events_ImagesDMO>();
                images_map = _ClgCOEContext.COE_Events_ImagesDMO.Where(e => e.COEE_Id == id).ToList();
                page.edit_images_list = images_map.ToArray();

                List<COE_Events_VideosDMO> videos_map = new List<COE_Events_VideosDMO>();
                videos_map = _ClgCOEContext.COE_Events_VideosDMO.Where(e => e.COEE_Id == id).ToList();
                page.edit_videos_list = videos_map.ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public ClgMasterCOEDTO getpageedit1(int id)
        {
            ClgMasterCOEDTO page = new ClgMasterCOEDTO();
            try
            {
                List<COE_Master_EventsDMO> events_m = new List<COE_Master_EventsDMO>();
                events_m = _ClgCOEContext.COE_Master_EventsDMO.Where(e => e.COEME_Id == id).ToList();
                page.edit_m_event = events_m.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public ClgMasterCOEDTO getpageedit2(int id)
        {
            ClgMasterCOEDTO page = new ClgMasterCOEDTO();
            try
            {
                List<COE_EventsDMO> events_map = new List<COE_EventsDMO>();
                events_map = _ClgCOEContext.COE_EventsDMO.Where(e => e.COEE_Id == id).ToList();
                page.edit_map_event = events_map.ToArray();
                var m_id = events_map[0].COEME_Id;
                List<COE_Master_EventsDMO> events_m = new List<COE_Master_EventsDMO>();
                events_m = _ClgCOEContext.COE_Master_EventsDMO.Where(e => e.COEME_Id == m_id).ToList();
                page.edit_m_event = events_m.ToArray();

                page.edit_stu_class_list = (from a in _ClgCOEContext.COE_Events_ClassesDMO
                                            from b in _ClgCOEContext.AdmissionClass
                                            where (a.COEE_Id == id && a.ASMCL_Id == b.ASMCL_Id)
                                            select new ClgMasterCOEDTO
                                            {
                                                COEEC_Id = a.COEEC_Id,
                                                COEE_Id = a.COEE_Id,
                                                //   ASMCL_Id = a.ASMCL_Id,
                                                //   ASMCL_ClassName = b.ASMCL_ClassName,
                                            }).Distinct().ToArray();

                var edit_asmy_id_mi_id = (from e in _ClgCOEContext.COE_EventsDMO
                                          where (e.COEE_Id == id)
                                          select new ClgMasterCOEDTO
                                          {
                                              MI_Id = e.MI_Id,
                                              ASMAY_Id = e.ASMAY_Id
                                          }).Distinct().ToList();
                List<long> mi_ids = new List<long>();
                List<long> asmay_ids = new List<long>();

                if (edit_asmy_id_mi_id.Count > 0)
                {
                    foreach (var item in edit_asmy_id_mi_id)
                    {
                        mi_ids.Add(item.MI_Id);
                        asmay_ids.Add(item.ASMAY_Id);
                    }
                }

                var course_list = (from a in _ClgCOEContext.COE_Events_CourseBranchDMO
                                   from d in _ClgCOEContext.MasterCourseDMO
                                   where (a.AMCO_Id == d.AMCO_Id && a.COEECB_ActiceFlg == true && d.AMCO_ActiveFlag == true && a.COEE_Id == id)
                                   select new ClgMasterCOEDTO
                                   {
                                       AMCO_Id = d.AMCO_Id,
                                   }).Distinct().ToList();
                page.courselist_select = course_list.Distinct().ToArray();

                List<long> cors_ids = new List<long>();
                if (course_list.Count > 0)
                {
                    foreach (var crsid in course_list)
                    {
                        cors_ids.Add(crsid.AMCO_Id);
                    }
                }

                var branch_list = (from a in _ClgCOEContext.CLG_Adm_College_AY_CourseDMO
                                   from b in _ClgCOEContext.CLG_Adm_College_AY_Course_BranchDMO
                                   from c in _ClgCOEContext.AcademicYear
                                   from d in _ClgCOEContext.ClgMasterBranchDMO
                                   where (a.ACAYC_Id == b.ACAYC_Id && a.ASMAY_Id == c.ASMAY_Id && a.MI_Id == c.MI_Id && b.AMB_Id == d.AMB_Id && d.AMB_ActiveFlag == true && mi_ids.Contains(a.MI_Id) && mi_ids.Contains(c.MI_Id) && asmay_ids.Contains(a.ASMAY_Id) && asmay_ids.Contains(c.ASMAY_Id) && cors_ids.Contains(a.AMCO_Id))
                                   select new ClgMasterCOEDTO
                                   {
                                       AMB_Id = d.AMB_Id,
                                       AMB_BranchName = d.AMB_BranchName,
                                       AMB_BranchCode = d.AMB_BranchCode,
                                       AMB_ActiveFlag = d.AMB_ActiveFlag,
                                       AMB_Order = d.AMB_Order
                                   }
                       ).Distinct().ToList();

                page.branch_list = branch_list.Distinct().ToArray();



                var brnchlist = (from a in _ClgCOEContext.COE_Events_CourseBranchDMO
                                 from d in _ClgCOEContext.ClgMasterBranchDMO
                                 where (a.AMB_Id == d.AMB_Id && a.COEECB_ActiceFlg == true && d.AMB_ActiveFlag == true && a.COEE_Id == id)
                                 select new ClgMasterCOEDTO
                                 {
                                     AMB_Id = d.AMB_Id,
                                 }).Distinct().ToList();
                page.branchlist_select = brnchlist.Distinct().ToArray();

                List<long> brnch_ids = new List<long>();
                if (brnchlist.Count > 0)
                {
                    foreach (var brnchids in brnchlist)
                    {
                        brnch_ids.Add(brnchids.AMB_Id);
                    }
                }

                var sem_list = (from a in _ClgCOEContext.CLG_Adm_College_AY_CourseDMO
                                from b in _ClgCOEContext.CLG_Adm_College_AY_Course_BranchDMO
                                from c in _ClgCOEContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                from d in _ClgCOEContext.AcademicYear
                                from e in _ClgCOEContext.CLG_Adm_Master_SemesterDMO
                                where (a.ACAYC_Id == b.ACAYC_Id && b.ACAYCB_Id == c.ACAYCB_Id && a.MI_Id == c.MI_Id && a.ASMAY_Id == d.ASMAY_Id && c.AMSE_Id == e.AMSE_Id && e.AMSE_ActiveFlg == true && mi_ids.Contains(a.MI_Id) && mi_ids.Contains(c.MI_Id) && asmay_ids.Contains(a.ASMAY_Id) && asmay_ids.Contains(d.ASMAY_Id) && cors_ids.Contains(a.AMCO_Id) && brnch_ids.Contains(b.AMB_Id))
                                select new ClgMasterCOEDTO
                                {
                                    AMSE_Id = e.AMSE_Id,
                                    AMSE_SEMName = e.AMSE_SEMName,
                                    AMSE_SEMCode = e.AMSE_SEMCode,
                                    AMSE_SEMOrder = e.AMSE_SEMOrder
                                }
                      ).Distinct().OrderBy(c => c.AMSE_SEMOrder).ToList();
                page.sem_list = sem_list.Distinct().ToArray();

                var semlist = (from a in _ClgCOEContext.COE_Events_CourseBranchDMO
                               from e in _ClgCOEContext.CLG_Adm_Master_SemesterDMO
                               where (a.AMSE_Id == e.AMSE_Id && a.COEECB_ActiceFlg == true && e.AMSE_ActiveFlg == true && a.COEE_Id == id)
                               select new ClgMasterCOEDTO
                               {
                                   AMSE_Id = e.AMSE_Id,
                               }).Distinct().ToList();

                page.semlist_select = semlist.Distinct().ToArray();


                page.edit_emp_type_list = (from a in _ClgCOEContext.COE_Events_EmployeesDMO
                                           from b in _ClgCOEContext.HR_Master_Department
                                           where (a.COEE_Id == id && a.HRMGT_Id == b.HRMD_Id)
                                           select new ClgMasterCOEDTO
                                           {
                                               COEEE_Id = a.COEEE_Id,
                                               COEE_Id = a.COEE_Id,
                                               HRMD_Id = a.HRMGT_Id,
                                               HRMD_DepartmentName = b.HRMD_DepartmentName,

                                           }
                                        ).Distinct().ToArray();

                List<COE_Events_OthersDMO> others_map = new List<COE_Events_OthersDMO>();
                others_map = _ClgCOEContext.COE_Events_OthersDMO.Where(e => e.COEE_Id == id).ToList();
                page.edit_oth_mobilenos_list = others_map.ToArray();

                List<COE_Events_ImagesDMO> images_map = new List<COE_Events_ImagesDMO>();
                images_map = _ClgCOEContext.COE_Events_ImagesDMO.Where(e => e.COEE_Id == id).ToList();
                page.edit_images_list = images_map.ToArray();

                List<COE_Events_VideosDMO> videos_map = new List<COE_Events_VideosDMO>();
                videos_map = _ClgCOEContext.COE_Events_VideosDMO.Where(e => e.COEE_Id == id).ToList();
                page.edit_videos_list = videos_map.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public ClgMasterCOEDTO deleterec(int id)
        {
            ClgMasterCOEDTO page = new ClgMasterCOEDTO();
            try
            {
                List<TT_Master_Staff_AbbreviationDMO> lorg = new List<TT_Master_Staff_AbbreviationDMO>();
                lorg = _ClgCOEContext.TT_Master_Staff_AbbreviationDMO.Where(t => t.TTMSAB_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    _ClgCOEContext.Remove(lorg.ElementAt(0));
                    var contactExists = _ClgCOEContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        page.returnval = true;
                    }
                    else
                    {
                        page.returnval = false;
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        //active deactive 
        public ClgMasterCOEDTO deactivate1(ClgMasterCOEDTO data)
        {
            COE_Master_EventsDMO pge = Mapper.Map<COE_Master_EventsDMO>(data);
            if (pge.COEME_Id > 0)
            {
                var result = _ClgCOEContext.COE_Master_EventsDMO.Single(t => t.COEME_Id == pge.COEME_Id);
                if (result.COEME_ActiveFlag == true)
                {
                    result.COEME_ActiveFlag = false;
                }
                else
                {
                    result.COEME_ActiveFlag = true;
                }
                // result.UpdatedDate = DateTime.Now;
                _ClgCOEContext.Update(result);
                var flag = _ClgCOEContext.SaveChanges();
                if (flag == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }


            return data;
        }
        public ClgMasterCOEDTO deactivate2(ClgMasterCOEDTO data)
        {
            COE_EventsDMO pge = Mapper.Map<COE_EventsDMO>(data);
            if (pge.COEME_Id > 0)
            {
                var result = _ClgCOEContext.COE_EventsDMO.Where(t => t.COEE_Id == pge.COEE_Id).Select(d => new COE_EventsDMO
                {
                    COEE_Id = d.COEE_Id,
                    COEE_ActiveFlag = d.COEE_ActiveFlag,
                    MI_Id = d.MI_Id,
                    ASMAY_Id = d.ASMAY_Id,
                    COEME_Id = d.COEME_Id
                }).ToList();
                if (result.FirstOrDefault().COEE_ActiveFlag == true)
                {
                    result.FirstOrDefault().COEE_ActiveFlag = false;
                }
                else
                {
                    result.FirstOrDefault().COEE_ActiveFlag = true;
                }
                // result.UpdatedDate = DateTime.Now;
                _ClgCOEContext.Update(result.ElementAt(0));
                var flag = _ClgCOEContext.SaveChanges();
                if (flag == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }


            return data;
        }
        public ClgMasterCOEDTO delete_event_classes(int id)
        {
            ClgMasterCOEDTO pagert = new ClgMasterCOEDTO();
            //TT_LABLIB_DetailsDTO page = new TT_LABLIB_DetailsDTO();
            try
            {

                List<COE_Events_CourseBranchDMO> lorg = new List<COE_Events_CourseBranchDMO>();
                lorg = _ClgCOEContext.COE_Events_CourseBranchDMO.Where(t => t.COEE_Id == id).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _ClgCOEContext.Remove(lorg.ElementAt(i));
                        var contactExists = _ClgCOEContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            pagert.returnval = true;
                        }
                        else
                        {
                            pagert.returnval = false;
                        }
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pagert;
        }

        public ClgMasterCOEDTO delete_event_emps(int id)
        {
            ClgMasterCOEDTO pagert = new ClgMasterCOEDTO();
            TT_LABLIB_DetailsDTO page = new TT_LABLIB_DetailsDTO();
            try
            {

                List<COE_Events_EmployeesDMO> lorg = new List<COE_Events_EmployeesDMO>();
                lorg = _ClgCOEContext.COE_Events_EmployeesDMO.Where(t => t.COEE_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _ClgCOEContext.Remove(lorg.ElementAt(i));
                        var contactExists = _ClgCOEContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            pagert.returnval = true;
                        }
                        else
                        {
                            pagert.returnval = false;
                        }
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pagert;
        }

        public ClgMasterCOEDTO delete_event_othr_mobs(int id)
        {
            ClgMasterCOEDTO pagert = new ClgMasterCOEDTO();
            TT_LABLIB_DetailsDTO page = new TT_LABLIB_DetailsDTO();
            try
            {

                List<COE_Events_OthersDMO> lorg = new List<COE_Events_OthersDMO>();
                lorg = _ClgCOEContext.COE_Events_OthersDMO.Where(t => t.COEE_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _ClgCOEContext.Remove(lorg.ElementAt(i));
                        var contactExists = _ClgCOEContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            pagert.returnval = true;
                        }
                        else
                        {
                            pagert.returnval = false;
                        }
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pagert;
        }

        public ClgMasterCOEDTO delete_event_images(int id)
        {
            ClgMasterCOEDTO pagert = new ClgMasterCOEDTO();
            TT_LABLIB_DetailsDTO page = new TT_LABLIB_DetailsDTO();
            try
            {

                List<COE_Events_ImagesDMO> lorg = new List<COE_Events_ImagesDMO>();
                lorg = _ClgCOEContext.COE_Events_ImagesDMO.Where(t => t.COEE_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _ClgCOEContext.Remove(lorg.ElementAt(i));
                        var contactExists = _ClgCOEContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            pagert.returnval = true;
                        }
                        else
                        {
                            pagert.returnval = false;
                        }
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pagert;
        }

        public ClgMasterCOEDTO delete_event_videos(int id)
        {
            ClgMasterCOEDTO pagert = new ClgMasterCOEDTO();
            TT_LABLIB_DetailsDTO page = new TT_LABLIB_DetailsDTO();
            try
            {

                List<COE_Events_VideosDMO> lorg = new List<COE_Events_VideosDMO>();
                lorg = _ClgCOEContext.COE_Events_VideosDMO.Where(t => t.COEE_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _ClgCOEContext.Remove(lorg.ElementAt(i));
                        var contactExists = _ClgCOEContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            pagert.returnval = true;
                        }
                        else
                        {
                            pagert.returnval = false;
                        }
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pagert;
        }
    }
}
