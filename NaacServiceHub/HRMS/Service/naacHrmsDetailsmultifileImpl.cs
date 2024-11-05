
using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.HRMS;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.HRMS;
using DomainModel.Model.NAAC.Admission;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.HRMS.Services
{
    public class naacHrmsDetailsmultifileImpl : Interface.naacHrmsDetailsmultifileInterface
    {
        public NaacHRMSContext _context;
        public HRMSContext _HRMSContext;

        public naacHrmsDetailsmultifileImpl(NaacHRMSContext context, HRMSContext HRMSContext)
        {
            _context = context;
            _HRMSContext = HRMSContext;
        }
        public HRMS_NAAC_DTO getdetails(HRMS_NAAC_DTO data)
        {
            try
            {
                data = GetAllDropdownAndDatatableDetails(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HRMS_NAAC_DTO GetAllDropdownAndDatatableDetails(HRMS_NAAC_DTO dto)
        {
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            try
            {
                //GroupTypelist
                GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                dto.groupTypedropdown = GroupTypelist.ToArray();

                //Departmentlist
                Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                dto.departmentdropdown = Departmentlist.ToArray();

                //Designationlist
                Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                dto.designationdropdown = Designationlist.ToArray();

                dto.academicyearlist = _HRMSContext.AcademicYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.ASMAY_ActiveFlag == 1).OrderBy(t => t.ASMAY_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public HRMS_NAAC_DTO get_depts(HRMS_NAAC_DTO data)
        {
            try
            {
                data.departmentdropdown = (from a in _HRMSContext.MasterEmployee
                                           from b in _HRMSContext.HR_Master_Department
                                           where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMD_Id == b.HRMD_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMD_ActiveFlag == true)
                                           select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HRMS_NAAC_DTO get_desig(HRMS_NAAC_DTO data)
        {
            try
            {
                data.designationdropdown = (from a in _HRMSContext.MasterEmployee
                                            from b in _HRMSContext.HR_Master_Designation
                                            where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && data.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                            select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HRMS_NAAC_DTO get_Employe_ob(HRMS_NAAC_DTO data)
        {
            List<long> selected_typ = new List<long>();

            foreach (var itm in data.emptypes)
            {
                selected_typ.Add(itm.HRMGT_Id);
            }
            List<long> selected_dep = new List<long>();

            foreach (var itm in data.empdept)
            {
                selected_dep.Add(itm.HRMD_Id);
            }
            List<long> selected_des = new List<long>();

            foreach (var itm in data.empdesg)
            {
                selected_des.Add(itm.HRMDES_Id);
            }

            data.get_emp = (from a in _HRMSContext.MasterEmployee
                            from b in _HRMSContext.HR_Master_Designation
                            from c in _HRMSContext.HR_Master_Department
                            from d in _HRMSContext.HR_Master_GroupType
                            where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
                            && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true && a.HRMGT_Id == d.HRMGT_Id && d.HRMGT_ActiveFlag == true
                             && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id == d.MI_Id && selected_typ.Contains(d.HRMGT_Id) && selected_dep.Contains(c.HRMD_Id) && selected_des.Contains(b.HRMDES_Id))
                            select new HRMS_NAAC_DTO
                            {
                                HRME_Id = a.HRME_Id,
                                HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim()
                            }
                     ).Distinct().ToArray();

            return data;
        }

        public HRMS_NAAC_DTO viewfileremark(HRMS_NAAC_DTO data)
        {
            data.documentCommentlist = (from a in _context.HR_Employee_BOSBOE_CommentsDMO
                                        from b in _context.Staff_User_Login
                                        from c in _context.MasterEmployee
                                        where (a.NCHRBOSC_RemarksBy == b.Id && b.Emp_Code == c.HRME_Id && a.HREBOS_Id == data.HREBOS_Id)
                                        select new HR_Employee_BOSBOE_CommentsDTO
                                        {
                                            NCHRBOSC_Id = a.NCHRBOSC_Id,
                                            HREBOS_Id = a.HREBOS_Id,
                                            NCHRBOSC_Remarks = a.NCHRBOSC_Remarks,
                                            NCHRBOSC_RemarksBy = a.NCHRBOSC_RemarksBy,
                                            RemarkPersonname = c.HRME_EmployeeFirstName + " " + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == " " || c.HRME_EmployeeMiddleName == "0" ? " " : c.HRME_EmployeeMiddleName) + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == " " || c.HRME_EmployeeLastName == "0" ? " " : c.HRME_EmployeeLastName),
                                            NCHRBOSC_StatusFlg = a.NCHRBOSC_StatusFlg,
                                            NCHRBOSC_ActiveFlag = a.NCHRBOSC_ActiveFlag
                                        }).ToArray();
            return data;
        }

        public HRMS_NAAC_DTO viewsubfileremark(HRMS_NAAC_DTO data)
        {
            data.documentsubCommentlist = (from a in _context.HR_Employee_BOSBOE_File_CommentsDMO
                                        from b in _context.Staff_User_Login
                                        from c in _context.MasterEmployee
                                        where (a.NCHRBOSFC_RemarksBy == b.Id && b.Emp_Code == c.HRME_Id && a.NCHREBOSF_Id == data.NCHREBOSF_Id)
                                        select new HR_Employee_BOSBOE_File_CommentsDTO
                                        {
                                            NCHRBOSFC_Id = a.NCHRBOSFC_Id,
                                            NCHREBOSF_Id = a.NCHREBOSF_Id,
                                            NCHRBOSFC_Remarks = a.NCHRBOSFC_Remarks,
                                            NCHRBOSFC_RemarksBy = a.NCHRBOSFC_RemarksBy,
                                            RemarkPersonname = c.HRME_EmployeeFirstName + " " + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == " " || c.HRME_EmployeeMiddleName == "0" ? " " : c.HRME_EmployeeMiddleName) + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == " " || c.HRME_EmployeeLastName == "0" ? " " : c.HRME_EmployeeLastName),
                                            NCHRBOSFC_StatusFlg = a.NCHRBOSFC_StatusFlg,
                                            NCHRBOSFC_ActiveFlag = a.NCHRBOSFC_ActiveFlag
                                        }).ToArray();
            return data;
        }

        public HRMS_NAAC_DTO SaveData(HRMS_NAAC_DTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.TabName.Equals("OrientationTab"))
                {
                    AddUpdateEmployeeOrientationDetails(dto);
                }
                else if (dto.TabName.Equals("ExamintionTab"))
                {
                    AddUpdateExaminationActivityDetails(dto);
                }
                else if (dto.TabName.Equals("StudentActivityTab"))
                {
                    AddUpdateStidentActivityDetails(dto);
                }
                else if (dto.TabName.Equals("ProfessionalActivityTab"))
                {
                    AddUpdateProfessionalActivityDetails(dto);
                }
                else if (dto.TabName.Equals("ResearchProjectTab"))
                {
                    AddUpdateResearchProjectDetails(dto);
                }
                else if (dto.TabName.Equals("ResearchGuidanceTab"))
                {
                    AddUpdateResearchGuidanceDetails(dto);
                }
                else if (dto.TabName.Equals("BOSBOETab"))
                {
                    AddUpdateBOSBOEDetails(dto);
                }
                else if (dto.TabName.Equals("JournalTab"))
                {
                    AddUpdateJournalDetails(dto);
                }
                else if (dto.TabName.Equals("ConferenceTab"))
                {
                    AddUpdateConferenceDetails(dto);
                }
                else if (dto.TabName.Equals("BookTab"))
                {
                    AddUpdateBookDetails(dto);
                }
                else if (dto.TabName.Equals("BookChapterTab"))
                {
                    AddUpdateBookChapterDetails(dto);
                }
                else if (dto.TabName.Equals("CommetteeTab"))
                {
                    AddUpdateCommetteeDetails(dto);
                }
                else if (dto.TabName.Equals("OtherDetailTab"))
                {
                    AddUpdateOtherDetails(dto);
                }
                else if (dto.TabName.Equals("GroupADetailTab"))
                {
                    AddUpdateGroupADetails(dto);
                }
                else if (dto.TabName.Equals("GroupBDetailTab"))
                {
                    AddUpdateGroupBDetails(dto);
                }
                
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error";
            }
            return dto;
        }

        public HRMS_NAAC_DTO AddUpdateExaminationActivityDetails(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.Type != "All")
                {
                    dto.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == dto.HRME_Id && a.MI_Id == dto.MI_Id).Select(a => a.Emp_Code).FirstOrDefault();
                }

                if (dto.HR_Employee_ExamDutyDetailsArrayDTO.Count() > 0)
                {
                    foreach (HR_Employee_ExamDutyDTO DocumentsDTO in dto.HR_Employee_ExamDutyDetailsArrayDTO)
                    {
                        DocumentsDTO.MI_Id = dto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.HRME_Id;
                        HR_Employee_ExamDutyDMO Documents = Mapper.Map<HR_Employee_ExamDutyDMO>(DocumentsDTO);

                        if (Documents.HREEXDT_Id > 0)
                        {
                            var Documentsresult = _context.HR_Employee_ExamDutyDMO.Single(t => t.HREEXDT_Id == DocumentsDTO.HREEXDT_Id);
                            Documentsresult.UpdatedDate = DateTime.Now;
                            Documents.HREEXDT_UpdatedBy = dto.LogInUserId;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _context.Update(Documentsresult);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            Documents.CreatedDate = DateTime.Now;
                            Documents.UpdatedDate = DateTime.Now;
                            Documents.HREEXDT_ActiveFlg = true;
                            Documents.HREEXDT_CreatedBy = dto.LogInUserId;
                            Documents.HREEXDT_UpdatedBy = dto.LogInUserId;
                            _context.Add(Documents);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        //Document details
        public HRMS_NAAC_DTO AddUpdateEmployeeOrientationDetails(HRMS_NAAC_DTO dto)
        {
            //add/update Documents details
            try
            {
                if (dto.Type != "All")
                {
                    dto.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == dto.HRME_Id && a.MI_Id == dto.MI_Id).Select(a => a.Emp_Code).FirstOrDefault();
                }

                if (dto.HR_Employee_OrientationCourseArrayDTO.Count() > 0)
                {
                    foreach (HR_Employee_OrientationCourseDTO DocumentsDTO in dto.HR_Employee_OrientationCourseArrayDTO)
                    {
                        DocumentsDTO.MI_Id = dto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.HRME_Id;
                        HR_Employee_OrientationCourseDMO Documents = Mapper.Map<HR_Employee_OrientationCourseDMO>(DocumentsDTO);

                        if (Documents.HREORCO_Id > 0)
                        {
                            var Documentsresult = _context.HR_Employee_OrientationCourseDMO.Single(t => t.HREORCO_Id == DocumentsDTO.HREORCO_Id);
                            Documentsresult.UpdatedDate = DateTime.Now;
                            Documents.HREORCO_UpdatedBy = dto.LogInUserId;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _context.Update(Documentsresult);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            Documents.CreatedDate = DateTime.Now;
                            Documents.UpdatedDate = DateTime.Now;
                            Documents.HREORCO_ActiveFlg = true;
                            Documents.HREORCO_CreatedBy = dto.LogInUserId;
                            Documents.HREORCO_UpdatedBy = dto.LogInUserId;
                            _context.Add(Documents);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HRMS_NAAC_DTO AddUpdateStidentActivityDetails(HRMS_NAAC_DTO dto)
        {
            //add/update Documents details
            try
            {
                if (dto.Type != "All")
                {
                    dto.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == dto.HRME_Id && a.MI_Id == dto.MI_Id).Select(a => a.Emp_Code).FirstOrDefault();
                }

                if (dto.HR_Employee_StudentActivitiesArrayDTO.Count() > 0)
                {
                    foreach (HR_Employee_StudentActivitiesDTO DocumentsDTO in dto.HR_Employee_StudentActivitiesArrayDTO)
                    {
                        DocumentsDTO.MI_Id = dto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.HRME_Id;
                        HR_Employee_StudentActivitiesDMO Documents = Mapper.Map<HR_Employee_StudentActivitiesDMO>(DocumentsDTO);

                        if (Documents.HRESACT_Id > 0)
                        {
                            var Documentsresult = _context.HR_Employee_StudentActivitiesDMO.Single(t => t.HRESACT_Id == DocumentsDTO.HRESACT_Id);
                            Documentsresult.UpdatedDate = DateTime.Now;
                            Documents.HRESACT_UpdatedBy = dto.LogInUserId;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _context.Update(Documentsresult);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            Documents.CreatedDate = DateTime.Now;
                            Documents.UpdatedDate = DateTime.Now;
                            Documents.HRESACT_ActiveFlg = true;
                            Documents.HRESACT_CreatedBy = dto.LogInUserId;
                            Documents.HRESACT_UpdatedBy = dto.LogInUserId;
                            _context.Add(Documents);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HRMS_NAAC_DTO AddUpdateProfessionalActivityDetails(HRMS_NAAC_DTO dto)
        {
            //add/update Documents details
            try
            {
                if (dto.Type != "All")
                {
                    dto.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == dto.HRME_Id && a.MI_Id == dto.MI_Id).Select(a => a.Emp_Code).FirstOrDefault();
                }

                if (dto.HR_Employee_DevActivitiesArrayDTO.Count() > 0)
                {
                    foreach (HR_Employee_DevActivitiesDTO DocumentsDTO in dto.HR_Employee_DevActivitiesArrayDTO)
                    {
                        DocumentsDTO.MI_Id = dto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.HRME_Id;
                        HR_Employee_DevActivitiesDMO Documents = Mapper.Map<HR_Employee_DevActivitiesDMO>(DocumentsDTO);

                        if (Documents.HREDACT_Id > 0)
                        {
                            var Documentsresult = _context.HR_Employee_DevActivitiesDMO.Single(t => t.HREDACT_Id == DocumentsDTO.HREDACT_Id);
                            Documentsresult.UpdatedDate = DateTime.Now;
                            Documents.HREDACT_UpdatedBy = dto.LogInUserId;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _context.Update(Documentsresult);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            Documents.CreatedDate = DateTime.Now;
                            Documents.UpdatedDate = DateTime.Now;
                            Documents.HREDACT_ActiveFlg = true;
                            Documents.HREDACT_CreatedBy = dto.LogInUserId;
                            Documents.HREDACT_UpdatedBy = dto.LogInUserId;
                            _context.Add(Documents);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HRMS_NAAC_DTO AddUpdateResearchProjectDetails(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.Type != "All")
                {
                    dto.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == dto.HRME_Id && a.MI_Id == dto.MI_Id).Select(a => a.Emp_Code).FirstOrDefault();
                }

                if (dto.HR_Employee_ResearchProjectsArrayDTO.Count() > 0)
                {
                    foreach (HR_Employee_ResearchProjectsDTO DocumentsDTO in dto.HR_Employee_ResearchProjectsArrayDTO)
                    {
                        DocumentsDTO.MI_Id = dto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.HRME_Id;
                        HR_Employee_ResearchProjectsDMO Documents = Mapper.Map<HR_Employee_ResearchProjectsDMO>(DocumentsDTO);

                        if (Documents.HREREPR_Id > 0)
                        {
                            var Documentsresult = _context.HR_Employee_ResearchProjectsDMO.Single(t => t.HREREPR_Id == DocumentsDTO.HREREPR_Id);
                            Documentsresult.UpdatedDate = DateTime.Now;
                            Documents.HREREPR_UpdatedBy = dto.LogInUserId;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _context.Update(Documentsresult);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            Documents.CreatedDate = DateTime.Now;
                            Documents.UpdatedDate = DateTime.Now;
                            Documents.HREREPR_ActiveFlg = true;
                            Documents.HREREPR_CreatedBy = dto.LogInUserId;
                            Documents.HREREPR_UpdatedBy = dto.LogInUserId;
                            _context.Add(Documents);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HRMS_NAAC_DTO AddUpdateResearchGuidanceDetails(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.Type != "All")
                {
                    dto.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == dto.HRME_Id && a.MI_Id == dto.MI_Id).Select(a => a.Emp_Code).FirstOrDefault();
                }

                if (dto.HR_Employee_ResearchGuidanceArrayDTO.Count() > 0)
                {
                    foreach (HR_Employee_ResearchGuidanceDTO DocumentsDTO in dto.HR_Employee_ResearchGuidanceArrayDTO)
                    {
                        DocumentsDTO.MI_Id = dto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.HRME_Id;
                        HR_Employee_ResearchGuidanceDMO Documents = Mapper.Map<HR_Employee_ResearchGuidanceDMO>(DocumentsDTO);

                        if (Documents.HREREGU_Id > 0)
                        {
                            var Documentsresult = _context.HR_Employee_ResearchGuidanceDMO.Single(t => t.HREREGU_Id == DocumentsDTO.HREREGU_Id);
                            Documentsresult.UpdatedDate = DateTime.Now;
                            Documents.HREREGU_UpdatedBy = dto.LogInUserId;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _context.Update(Documentsresult);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            Documents.CreatedDate = DateTime.Now;
                            Documents.UpdatedDate = DateTime.Now;
                            Documents.HREREGU_ActiveFlg = true;
                            Documents.HREREGU_CreatedBy = dto.LogInUserId;
                            Documents.HREREGU_UpdatedBy = dto.LogInUserId;
                            _context.Add(Documents);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HRMS_NAAC_DTO AddUpdateBOSBOEDetails(HRMS_NAAC_DTO dto)
        {
            long s = 0;
            try
            {
                if (dto.Type != "All")
                {
                    dto.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == dto.HRME_Id && a.MI_Id == dto.MI_Id).Select(a => a.Emp_Code).FirstOrDefault();
                }

                HR_Employee_BOSBOEDMO Documents = Mapper.Map<HR_Employee_BOSBOEDMO>(dto.HR_Employee_BOSBOEDTO);
                Documents.MI_Id = dto.MI_Id;
                Documents.HRME_Id = dto.HRME_Id;

                if (Documents.HREBOS_Id > 0)
                {
                    var Documentsresult = _context.HR_Employee_BOSBOEDMO.Single(t => t.HREBOS_Id == Documents.HREBOS_Id);
                    if (Documentsresult.HREBOS_ApprovedFlg == true && Documentsresult.HREBOS_StatusFlg == "Approved")
                    {
                        dto.retrunMsg = "Approved";
                    }
                    else
                    {
                        Documentsresult.UpdatedDate = DateTime.Now;
                        Documents.HREBOS_UpdatedBy = dto.LogInUserId;
                        Mapper.Map(Documents, Documentsresult);
                        _context.Update(Documentsresult);
                        _context.SaveChanges();
                        s = Documentsresult.HREBOS_Id;
                        if (dto.HR_Employee_BOSBOE_FilesArrayDTO.Count() > 0)
                        {
                            foreach (HR_Employee_BOSBOE_FilesDTO DocumentsDTO in dto.HR_Employee_BOSBOE_FilesArrayDTO)
                            {
                                if (DocumentsDTO.NCHREBOSF_Id > 0)
                                {
                                    var filesdata = _context.HR_Employee_BOSBOE_FilesDMO.Where(t => t.NCHREBOSF_Id == DocumentsDTO.NCHREBOSF_Id).FirstOrDefault();
                                    filesdata.NCHREBOSF_FileName = DocumentsDTO.NCHREBOSF_FileName;
                                    filesdata.NCHREBOSF_Filedesc = DocumentsDTO.NCHREBOSF_Filedesc;
                                    filesdata.NCHREBOSF_FilePath = DocumentsDTO.NCHREBOSF_FilePath;
                                    _context.Update(filesdata);
                                    int flag = _context.SaveChanges();
                                    if (flag > 0)
                                    {
                                        dto.retrunMsg = "Add";
                                    }
                                    else
                                    {
                                        dto.retrunMsg = "false";
                                    }
                                }
                                else
                                {
                                    HR_Employee_BOSBOE_FilesDMO obj2 = new HR_Employee_BOSBOE_FilesDMO();
                                    obj2.NCHREBOSF_FileName = DocumentsDTO.NCHREBOSF_FileName;
                                    obj2.NCHREBOSF_Filedesc = DocumentsDTO.NCHREBOSF_Filedesc;
                                    obj2.NCHREBOSF_FilePath = DocumentsDTO.NCHREBOSF_FilePath;
                                    obj2.HREBOS_Id = s;
                                    obj2.NCHREBOSF_ApprovedFlg = false;
                                    obj2.NCHREBOSF_ActiveFlag = true;
                                    _context.Add(obj2);
                                    int flag = _context.SaveChanges();
                                    if (flag > 0)
                                    {
                                        dto.retrunMsg = "Add";
                                    }
                                    else
                                    {
                                        dto.retrunMsg = "false";
                                    }
                                }
                            }
                        }
                        else if (s > 0)
                        {
                            dto.retrunMsg = "Add";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                }
                else
                {
                    Documents.CreatedDate = DateTime.Now;
                    Documents.UpdatedDate = DateTime.Now;
                    Documents.HREBOS_ActiveFlg = true;
                    Documents.HREBOS_CreatedBy = dto.LogInUserId;
                    Documents.HREBOS_UpdatedBy = dto.LogInUserId;
                    _context.Add(Documents);
                    _context.SaveChanges();
                    s = Documents.HREBOS_Id;
                    if (dto.HR_Employee_BOSBOE_FilesArrayDTO.Count() > 0)
                    {
                        foreach (HR_Employee_BOSBOE_FilesDTO DocumentsDTO in dto.HR_Employee_BOSBOE_FilesArrayDTO)
                        {
                            HR_Employee_BOSBOE_FilesDMO obj2 = new HR_Employee_BOSBOE_FilesDMO();
                            obj2.NCHREBOSF_FileName = DocumentsDTO.NCHREBOSF_FileName;
                            obj2.NCHREBOSF_Filedesc = DocumentsDTO.NCHREBOSF_Filedesc;
                            obj2.NCHREBOSF_FilePath = DocumentsDTO.NCHREBOSF_FilePath;
                            obj2.HREBOS_Id = s;
                            obj2.NCHREBOSF_ApprovedFlg = false;
                            obj2.NCHREBOSF_ActiveFlag = true;
                            _context.Add(obj2);
                            int flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                    else if (s > 0)
                    {
                        dto.retrunMsg = "Add";
                    }
                    else
                    {
                        dto.retrunMsg = "false";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HRMS_NAAC_DTO AddUpdateJournalDetails(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.Type != "All")
                {
                    dto.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == dto.HRME_Id && a.MI_Id == dto.MI_Id).Select(a => a.Emp_Code).FirstOrDefault();
                }

                if (dto.HR_Employee_JournalArrayDTO.Count() > 0)
                {
                    foreach (HR_Employee_JournalDTO DocumentsDTO in dto.HR_Employee_JournalArrayDTO)
                    {
                        DocumentsDTO.MI_Id = dto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.HRME_Id;
                        HR_Employee_JournalDMO Documents = Mapper.Map<HR_Employee_JournalDMO>(DocumentsDTO);

                        if (Documents.HREJORNL_Id > 0)
                        {
                            var Documentsresult = _context.HR_Employee_JournalDMO.Single(t => t.HREJORNL_Id == DocumentsDTO.HREJORNL_Id);
                            Documentsresult.UpdatedDate = DateTime.Now;
                            Documents.HREJORNL_UpdatedBy = dto.LogInUserId;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _context.Update(Documentsresult);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            Documents.CreatedDate = DateTime.Now;
                            Documents.UpdatedDate = DateTime.Now;
                            Documents.HREJORNL_ActiveFlg = true;
                            Documents.HREJORNL_CreatedBy = dto.LogInUserId;
                            Documents.HREJORNL_UpdatedBy = dto.LogInUserId;
                            _context.Add(Documents);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HRMS_NAAC_DTO AddUpdateConferenceDetails(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.Type != "All")
                {
                    dto.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == dto.HRME_Id && a.MI_Id == dto.MI_Id).Select(a => a.Emp_Code).FirstOrDefault();
                }

                if (dto.HR_Employee_ConferenceArrayDTO.Count() > 0)
                {
                    foreach (HR_Employee_ConferenceDTO DocumentsDTO in dto.HR_Employee_ConferenceArrayDTO)
                    {
                        DocumentsDTO.MI_Id = dto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.HRME_Id;
                        HR_Employee_ConferenceDMO Documents = Mapper.Map<HR_Employee_ConferenceDMO>(DocumentsDTO);

                        if (Documents.HRECONF_Id > 0)
                        {
                            var Documentsresult = _context.HR_Employee_ConferenceDMO.Single(t => t.HRECONF_Id == DocumentsDTO.HRECONF_Id);
                            Documentsresult.UpdatedDate = DateTime.Now;
                            Documents.HRECONF_UpdatedBy = dto.LogInUserId;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _context.Update(Documentsresult);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            Documents.CreatedDate = DateTime.Now;
                            Documents.UpdatedDate = DateTime.Now;
                            Documents.HRECONF_ActiveFlg = true;
                            Documents.HRECONF_CreatedBy = dto.LogInUserId;
                            Documents.HRECONF_UpdatedBy = dto.LogInUserId;
                            _context.Add(Documents);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HRMS_NAAC_DTO AddUpdateBookDetails(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.Type != "All")
                {
                    dto.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == dto.HRME_Id && a.MI_Id == dto.MI_Id).Select(a => a.Emp_Code).FirstOrDefault();
                }

                if (dto.HR_Employee_BookArrayDTO.Count() > 0)
                {
                    foreach (HR_Employee_BookDTO DocumentsDTO in dto.HR_Employee_BookArrayDTO)
                    {
                        DocumentsDTO.MI_Id = dto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.HRME_Id;
                        HR_Employee_BookDMO Documents = Mapper.Map<HR_Employee_BookDMO>(DocumentsDTO);

                        if (Documents.HREBK_Id > 0)
                        {
                            var Documentsresult = _context.HR_Employee_BookDMO.Single(t => t.HREBK_Id == DocumentsDTO.HREBK_Id);
                            Documentsresult.UpdatedDate = DateTime.Now;
                            Documents.HREBK_UpdatedBy = dto.LogInUserId;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _context.Update(Documentsresult);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            Documents.CreatedDate = DateTime.Now;
                            Documents.UpdatedDate = DateTime.Now;
                            Documents.HREBK_ActiveFlg = true;
                            Documents.HREBK_CreatedBy = dto.LogInUserId;
                            Documents.HREBK_UpdatedBy = dto.LogInUserId;
                            _context.Add(Documents);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HRMS_NAAC_DTO AddUpdateBookChapterDetails(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.Type != "All")
                {
                    dto.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == dto.HRME_Id && a.MI_Id == dto.MI_Id).Select(a => a.Emp_Code).FirstOrDefault();
                }

                if (dto.HR_Employee_BookChapterArrayDTO.Count() > 0)
                {
                    foreach (HR_Employee_BookChapterDTO DocumentsDTO in dto.HR_Employee_BookChapterArrayDTO)
                    {
                        DocumentsDTO.MI_Id = dto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.HRME_Id;
                        HR_Employee_BookChapterDMO Documents = Mapper.Map<HR_Employee_BookChapterDMO>(DocumentsDTO);

                        if (Documents.HREBKCP_Id > 0)
                        {
                            var Documentsresult = _context.HR_Employee_BookChapterDMO.Single(t => t.HREBKCP_Id == DocumentsDTO.HREBKCP_Id);
                            Documentsresult.UpdatedDate = DateTime.Now;
                            Documents.HREBKCP_UpdatedBy = dto.LogInUserId;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _context.Update(Documentsresult);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            Documents.CreatedDate = DateTime.Now;
                            Documents.UpdatedDate = DateTime.Now;
                            Documents.HREBKCP_ActiveFlg = true;
                            Documents.HREBKCP_CreatedBy = dto.LogInUserId;
                            Documents.HREBKCP_UpdatedBy = dto.LogInUserId;
                            _context.Add(Documents);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HRMS_NAAC_DTO AddUpdateCommetteeDetails(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.Type != "All")
                {
                    dto.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == dto.HRME_Id && a.MI_Id == dto.MI_Id).Select(a => a.Emp_Code).FirstOrDefault();
                }

                if (dto.HR_Employee_CommitteeArrayDTO.Count() > 0)
                {
                    foreach (HR_Employee_CommitteeDTO DocumentsDTO in dto.HR_Employee_CommitteeArrayDTO)
                    {
                        DocumentsDTO.MI_Id = dto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.HRME_Id;
                        HR_Employee_CommitteeDMO Documents = Mapper.Map<HR_Employee_CommitteeDMO>(DocumentsDTO);

                        if (Documents.HRECOM_Id > 0)
                        {
                            var Documentsresult = _context.HR_Employee_CommitteeDMO.Single(t => t.HRECOM_Id == DocumentsDTO.HRECOM_Id);
                            Documentsresult.UpdatedDate = DateTime.Now;
                            Documents.HRECOM_UpdatedBy = dto.LogInUserId;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _context.Update(Documentsresult);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            Documents.CreatedDate = DateTime.Now;
                            Documents.UpdatedDate = DateTime.Now;
                            Documents.HRECOM_ActiveFlg = true;
                            Documents.HRECOM_CreatedBy = dto.LogInUserId;
                            Documents.HRECOM_UpdatedBy = dto.LogInUserId;
                            _context.Add(Documents);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HRMS_NAAC_DTO AddUpdateOtherDetails(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.Type != "All")
                {
                    dto.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == dto.HRME_Id && a.MI_Id == dto.MI_Id).Select(a => a.Emp_Code).FirstOrDefault();
                }

                if (dto.HR_Employee_OtherDetailsArrayDTO.Count() > 0)
                {
                    foreach (HR_Employee_OtherDetailsDTO DocumentsDTO in dto.HR_Employee_OtherDetailsArrayDTO)
                    {
                        DocumentsDTO.MI_Id = dto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.HRME_Id;
                        HR_Employee_OtherDetailsDMO Documents = Mapper.Map<HR_Employee_OtherDetailsDMO>(DocumentsDTO);

                        if (Documents.HREOTHDET_Id > 0)
                        {
                            var Documentsresult = _context.HR_Employee_OtherDetailsDMO.Single(t => t.HREOTHDET_Id == DocumentsDTO.HREOTHDET_Id);
                            Documentsresult.UpdatedDate = DateTime.Now;
                            Documents.HREOTHDET_UpdatedBy = dto.LogInUserId;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _context.Update(Documentsresult);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            Documents.CreatedDate = DateTime.Now;
                            Documents.UpdatedDate = DateTime.Now;
                            Documents.HREOTHDET_ActiveFlg = true;
                            Documents.HREOTHDET_CreatedBy = dto.LogInUserId;
                            Documents.HREOTHDET_UpdatedBy = dto.LogInUserId;
                            _context.Add(Documents);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
        public HRMS_NAAC_DTO AddUpdateGroupADetails(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.Type != "All")
                {
                    dto.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == dto.HRME_Id && a.MI_Id == dto.MI_Id).Select(a => a.Emp_Code).FirstOrDefault();
                }

                if (dto.HR_Employee_GroupADetailsArrayDTO.Count() > 0)
                {
                    foreach (HREmployeeGroupAExamDTO DocumentsDTO in dto.HR_Employee_GroupADetailsArrayDTO)
                    {
                        DocumentsDTO.MI_Id = dto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.HRME_Id;
                        HR_Employee_GroupAExamDMO Documents = Mapper.Map<HR_Employee_GroupAExamDMO>(DocumentsDTO);

                        if (Documents.HREMGAE_Id > 0)
                        {
                            var Documentsresult = _context.HR_Employee_GroupAExamDMO.Single(t => t.HREMGAE_Id == DocumentsDTO.HREMGAE_Id);
                            DocumentsDTO.UpdatedDate = DateTime.Now;
                            DocumentsDTO.HREMGAE_UpdatedBy = dto.LogInUserId;
                            DocumentsDTO.HREMGAE_ActiveFlg = true;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _context.Update(Documentsresult);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            Documents.CreatedDate = DateTime.Now;
                            Documents.UpdatedDate = DateTime.Now;
                            Documents.HREMGAE_ActiveFlg = true;
                            Documents.HREMGAE_CreatedBy = dto.LogInUserId;
                            Documents.HREMGAE_UpdatedBy = dto.LogInUserId;
                            _context.Add(Documents);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HRMS_NAAC_DTO AddUpdateGroupBDetails(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.Type != "All")
                {
                    dto.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == dto.HRME_Id && a.MI_Id == dto.MI_Id).Select(a => a.Emp_Code).FirstOrDefault();
                }

                if (dto.HR_Employee_GroupBDetailsArrayDTO.Count() > 0)
                {
                    foreach (HREmployeeGroupBExamDTO DocumentsDTO in dto.HR_Employee_GroupBDetailsArrayDTO)
                    {
                        DocumentsDTO.MI_Id = dto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.HRME_Id;
                        HR_Employee_GroupBExamDMO Documents = Mapper.Map<HR_Employee_GroupBExamDMO>(DocumentsDTO);

                        if (Documents.HREMGBE_Id > 0)
                        {
                            var Documentsresult = _context.HR_Employee_GroupBExamDMO.Single(t => t.HREMGBE_Id == DocumentsDTO.HREMGBE_Id);
                            DocumentsDTO.UpdatedDate = DateTime.Now;
                            DocumentsDTO.HREMGBE_UpdatedBy = dto.LogInUserId;
                            DocumentsDTO.HREMGBE_ActiveFlg = true;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _context.Update(Documentsresult);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            Documents.CreatedDate = DateTime.Now;
                            Documents.UpdatedDate = DateTime.Now;
                            Documents.HREMGBE_ActiveFlg = true;
                            Documents.HREMGBE_CreatedBy = dto.LogInUserId;
                            Documents.HREMGBE_UpdatedBy = dto.LogInUserId;
                            _context.Add(Documents);
                            var flag = _context.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
        
        public HRMS_NAAC_DTO getOrientdata(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.HRME_Id > 0)
                {
                    dto.orientlist = (from a in _context.HR_Employee_OrientationCourseDMO
                                      where (a.MI_Id == dto.MI_Id && a.HREORCO_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                      select a).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public HRMS_NAAC_DTO getStudentActivitydata(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.HRME_Id > 0)
                {
                    dto.StudentActivitylist = (from a in _context.HR_Employee_StudentActivitiesDMO
                                               where (a.MI_Id == dto.MI_Id && a.HRESACT_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                               select a).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public HRMS_NAAC_DTO getProfessionalActivitydata(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.HRME_Id > 0)
                {
                    dto.ProfessionalActivitylist = (from a in _context.HR_Employee_DevActivitiesDMO
                                                    where (a.MI_Id == dto.MI_Id && a.HREDACT_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                                    select a).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public HRMS_NAAC_DTO getResearchProjectdata(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.HRME_Id > 0)
                {
                    dto.ResearchProjectlist = (from a in _context.HR_Employee_ResearchProjectsDMO
                                               where (a.MI_Id == dto.MI_Id && a.HREREPR_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                               select a).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public HRMS_NAAC_DTO getResearchGuidedata(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.HRME_Id > 0)
                {
                    dto.ResearchGuidelist = (from a in _context.HR_Employee_ResearchGuidanceDMO
                                             where (a.MI_Id == dto.MI_Id && a.HREREGU_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                             select a).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public HRMS_NAAC_DTO getBOSBOEdata(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.HRME_Id > 0)
                {
                    dto.BOSBOElist = (from a in _context.HR_Employee_BOSBOEDMO
                                      where (a.MI_Id == dto.MI_Id && a.HREBOS_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                      select a).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public HRMS_NAAC_DTO getJournaldata(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.HRME_Id > 0)
                {
                    dto.Journallist = (from a in _context.HR_Employee_JournalDMO
                                       where (a.MI_Id == dto.MI_Id && a.HREJORNL_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                       select a).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public HRMS_NAAC_DTO getConferencedata(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.HRME_Id > 0)
                {
                    dto.Conferencelist = (from a in _context.HR_Employee_ConferenceDMO
                                          where (a.MI_Id == dto.MI_Id && a.HRECONF_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                          select a).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public HRMS_NAAC_DTO getBookdata(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.HRME_Id > 0)
                {
                    dto.Booklist = (from a in _context.HR_Employee_BookDMO
                                    where (a.MI_Id == dto.MI_Id && a.HREBK_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                    select a).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public HRMS_NAAC_DTO getBookChapterdata(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.HRME_Id > 0)
                {
                    dto.BookChapterlist = (from a in _context.HR_Employee_BookChapterDMO
                                           where (a.MI_Id == dto.MI_Id && a.HREBKCP_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                           select a).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public HRMS_NAAC_DTO getCommetteedata(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.HRME_Id > 0)
                {
                    dto.Commetteelist = (from a in _context.HR_Employee_CommitteeDMO
                                         where (a.MI_Id == dto.MI_Id && a.HRECOM_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                         select a).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public HRMS_NAAC_DTO getOtherDetaildata(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.HRME_Id > 0)
                {
                    dto.OtherDetailSlist = (from a in _context.HR_Employee_OtherDetailsDMO
                                            where (a.MI_Id == dto.MI_Id && a.HREOTHDET_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                            select a).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public async Task<HRMS_NAAC_DTO> get_EmployeALLDATAAsync(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.HRME_Id > 0)
                {
                    if (dto.Type != "All")
                    {
                        dto.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == dto.HRME_Id && a.MI_Id == dto.MI_Id).Select(a => a.Emp_Code).FirstOrDefault();
                    }

                    dto.orientlist = (from a in _context.HR_Employee_OrientationCourseDMO
                                      where (a.MI_Id == dto.MI_Id && a.HREORCO_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                      select a).ToArray();

                    dto.examinationlist = _context.HR_Employee_ExamDutyDMO.Where(t => t.MI_Id == dto.MI_Id && t.HREEXDT_ActiveFlg == true && t.HRME_Id == dto.HRME_Id).ToArray();

                    dto.StudentActivitylist = (from a in _context.HR_Employee_StudentActivitiesDMO
                                               where (a.MI_Id == dto.MI_Id && a.HRESACT_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                               select a).ToArray();

                    dto.ProfessionalActivitylist = (from a in _context.HR_Employee_DevActivitiesDMO
                                                    where (a.MI_Id == dto.MI_Id && a.HREDACT_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                                    select a).ToArray();

                    dto.ResearchProjectlist = (from a in _context.HR_Employee_ResearchProjectsDMO
                                               where (a.MI_Id == dto.MI_Id && a.HREREPR_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                               select a).ToArray();

                    dto.ResearchGuidelist = (from a in _context.HR_Employee_ResearchGuidanceDMO
                                             where (a.MI_Id == dto.MI_Id && a.HREREGU_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                             select a).ToArray();

                    dto.BOSBOElist = (from a in _context.HR_Employee_BOSBOEDMO
                                      where (a.MI_Id == dto.MI_Id && a.HREBOS_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                      select a).ToArray();

                    dto.Journallist = (from a in _context.HR_Employee_JournalDMO
                                       where (a.MI_Id == dto.MI_Id && a.HREJORNL_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                       select a).ToArray();

                    dto.Conferencelist = (from a in _context.HR_Employee_ConferenceDMO
                                          where (a.MI_Id == dto.MI_Id && a.HRECONF_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                          select a).ToArray();

                    dto.Booklist = (from a in _context.HR_Employee_BookDMO
                                    where (a.MI_Id == dto.MI_Id && a.HREBK_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                    select a).ToArray();

                    dto.BookChapterlist = (from a in _context.HR_Employee_BookChapterDMO
                                           where (a.MI_Id == dto.MI_Id && a.HREBKCP_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                           select a).ToArray();

                    dto.Commetteelist = (from a in _context.HR_Employee_CommitteeDMO
                                         where (a.MI_Id == dto.MI_Id && a.HRECOM_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                         select a).ToArray();

                    dto.OtherDetailSlist = (from a in _context.HR_Employee_OtherDetailsDMO
                                            where (a.MI_Id == dto.MI_Id && a.HREOTHDET_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                            select a).ToArray();

                    try
                    {
                        using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "GroupAExamDetails";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                SqlDbType.VarChar)
                            {
                                Value = dto.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                                       SqlDbType.VarChar)
                            {
                                Value = dto.HRME_Id
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = await cmd.ExecuteReaderAsync())
                                {
                                    while (await dataReader.ReadAsync())
                                    {
                                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                        {
                                            var datatype = dataReader.GetFieldType(iFiled);
                                            if (datatype.Name == "DateTime")
                                            {
                                                var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                                dataRow.Add(
                                                dataReader.GetName(iFiled),
                                                dataReader.IsDBNull(iFiled) ? "Not Available" : dateval);
                                            }
                                            else
                                            {
                                                dataRow.Add(
                                                dataReader.GetName(iFiled),
                                                dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled]);
                                            }
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                dto.GroupAExamlist = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }

                    try
                    {
                        using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "GroupBExamDetails";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                SqlDbType.VarChar)
                            {
                                Value = dto.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                                       SqlDbType.VarChar)
                            {
                                Value = dto.HRME_Id
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = await cmd.ExecuteReaderAsync())
                                {
                                    while (await dataReader.ReadAsync())
                                    {
                                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                        {
                                            var datatype = dataReader.GetFieldType(iFiled);
                                            if (datatype.Name == "DateTime")
                                            {
                                                var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                                dataRow.Add(
                                                dataReader.GetName(iFiled),
                                                dataReader.IsDBNull(iFiled) ? "Not Available" : dateval);
                                            }
                                            else
                                            {
                                                dataRow.Add(
                                                dataReader.GetName(iFiled),
                                                dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled]);
                                            }
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                dto.GroupBExamlist = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public HR_Employee_ExamDutyDTO DeleteDocumentRecordExamination(HR_Employee_ExamDutyDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                List<HR_Employee_ExamDutyDMO> lorg = new List<HR_Employee_ExamDutyDMO>();
                lorg = _context.HR_Employee_ExamDutyDMO.Where(t => t.HRME_Id.Equals(dto.HRME_Id) && t.MI_Id == dto.MI_Id && t.HREEXDT_Id == dto.HREEXDT_Id).ToList();
                foreach (HR_Employee_ExamDutyDMO ph1 in lorg)
                {
                    _context.Remove(ph1);
                    _context.SaveChanges();
                    dto.retrunMsg = "Deleted";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Employee_OrientationCourseDTO DeleteDocumentRecordOrientation(HR_Employee_OrientationCourseDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                List<HR_Employee_OrientationCourseDMO> lorg = new List<HR_Employee_OrientationCourseDMO>();
                lorg = _context.HR_Employee_OrientationCourseDMO.Where(t => t.HRME_Id.Equals(dto.HRME_Id) && t.MI_Id == dto.MI_Id && t.HREORCO_Id == dto.HREORCO_Id).ToList();
                foreach (HR_Employee_OrientationCourseDMO ph1 in lorg)
                {
                    _context.Remove(ph1);
                    _context.SaveChanges();
                    dto.retrunMsg = "Deleted";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Employee_StudentActivitiesDTO DeleteDocumentRecordStuActivity(HR_Employee_StudentActivitiesDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                List<HR_Employee_StudentActivitiesDMO> lorg = new List<HR_Employee_StudentActivitiesDMO>();
                lorg = _context.HR_Employee_StudentActivitiesDMO.Where(t => t.HRME_Id.Equals(dto.HRME_Id) && t.MI_Id == dto.MI_Id && t.HRESACT_Id == dto.HRESACT_Id).ToList();
                foreach (HR_Employee_StudentActivitiesDMO ph1 in lorg)
                {
                    _context.Remove(ph1);
                    _context.SaveChanges();
                    dto.retrunMsg = "Deleted";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Employee_DevActivitiesDTO DeleteDocumentRecordProfActivity(HR_Employee_DevActivitiesDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                List<HR_Employee_DevActivitiesDMO> lorg = new List<HR_Employee_DevActivitiesDMO>();
                lorg = _context.HR_Employee_DevActivitiesDMO.Where(t => t.HRME_Id.Equals(dto.HRME_Id) && t.MI_Id == dto.MI_Id && t.HREDACT_Id == dto.HREDACT_Id).ToList();
                foreach (HR_Employee_DevActivitiesDMO ph1 in lorg)
                {
                    _context.Remove(ph1);
                    _context.SaveChanges();
                    dto.retrunMsg = "Deleted";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Employee_ResearchProjectsDTO DeleteDocumentRecordResProj(HR_Employee_ResearchProjectsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                List<HR_Employee_ResearchProjectsDMO> lorg = new List<HR_Employee_ResearchProjectsDMO>();
                lorg = _context.HR_Employee_ResearchProjectsDMO.Where(t => t.HRME_Id.Equals(dto.HRME_Id) && t.MI_Id == dto.MI_Id && t.HREREPR_Id == dto.HREREPR_Id).ToList();
                foreach (HR_Employee_ResearchProjectsDMO ph1 in lorg)
                {
                    _context.Remove(ph1);
                    _context.SaveChanges();
                    dto.retrunMsg = "Deleted";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Employee_ResearchGuidanceDTO DeleteDocumentRecordResGuide(HR_Employee_ResearchGuidanceDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                List<HR_Employee_ResearchGuidanceDMO> lorg = new List<HR_Employee_ResearchGuidanceDMO>();
                lorg = _context.HR_Employee_ResearchGuidanceDMO.Where(t => t.HRME_Id.Equals(dto.HRME_Id) && t.MI_Id == dto.MI_Id && t.HREREGU_Id == dto.HREREGU_Id).ToList();
                foreach (HR_Employee_ResearchGuidanceDMO ph1 in lorg)
                {
                    _context.Remove(ph1);
                    _context.SaveChanges();
                    dto.retrunMsg = "Deleted";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Employee_BOSBOEDTO DeleteDocumentRecordBOSBOE(HR_Employee_BOSBOEDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                List<HR_Employee_BOSBOEDMO> lorg = new List<HR_Employee_BOSBOEDMO>();
                lorg = _context.HR_Employee_BOSBOEDMO.Where(t => t.HRME_Id.Equals(dto.HRME_Id) && t.MI_Id == dto.MI_Id && t.HREBOS_Id == dto.HREBOS_Id).ToList();
                foreach (HR_Employee_BOSBOEDMO ph1 in lorg)
                {
                    _context.Remove(ph1);
                    _context.SaveChanges();
                    dto.retrunMsg = "Deleted";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Employee_JournalDTO DeleteDocumentRecordJournal(HR_Employee_JournalDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                List<HR_Employee_JournalDMO> lorg = new List<HR_Employee_JournalDMO>();
                lorg = _context.HR_Employee_JournalDMO.Where(t => t.HRME_Id.Equals(dto.HRME_Id) && t.MI_Id == dto.MI_Id && t.HREJORNL_Id == dto.HREJORNL_Id).ToList();
                foreach (HR_Employee_JournalDMO ph1 in lorg)
                {
                    _context.Remove(ph1);
                    _context.SaveChanges();
                    dto.retrunMsg = "Deleted";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Employee_ConferenceDTO DeleteDocumentRecordConference(HR_Employee_ConferenceDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                List<HR_Employee_ConferenceDMO> lorg = new List<HR_Employee_ConferenceDMO>();
                lorg = _context.HR_Employee_ConferenceDMO.Where(t => t.HRME_Id.Equals(dto.HRME_Id) && t.MI_Id == dto.MI_Id && t.HRECONF_Id == dto.HRECONF_Id).ToList();
                foreach (HR_Employee_ConferenceDMO ph1 in lorg)
                {
                    _context.Remove(ph1);
                    _context.SaveChanges();
                    dto.retrunMsg = "Deleted";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Employee_BookDTO DeleteDocumentRecordBook(HR_Employee_BookDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                List<HR_Employee_BookDMO> lorg = new List<HR_Employee_BookDMO>();
                lorg = _context.HR_Employee_BookDMO.Where(t => t.HRME_Id.Equals(dto.HRME_Id) && t.MI_Id == dto.MI_Id && t.HREBK_Id == dto.HREBK_Id).ToList();
                foreach (HR_Employee_BookDMO ph1 in lorg)
                {
                    _context.Remove(ph1);
                    _context.SaveChanges();
                    dto.retrunMsg = "Deleted";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Employee_BookChapterDTO DeleteDocumentRecordBookChapter(HR_Employee_BookChapterDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                List<HR_Employee_BookChapterDMO> lorg = new List<HR_Employee_BookChapterDMO>();
                lorg = _context.HR_Employee_BookChapterDMO.Where(t => t.HRME_Id.Equals(dto.HRME_Id) && t.MI_Id == dto.MI_Id && t.HREBKCP_Id == dto.HREBKCP_Id).ToList();
                foreach (HR_Employee_BookChapterDMO ph1 in lorg)
                {
                    _context.Remove(ph1);
                    _context.SaveChanges();
                    dto.retrunMsg = "Deleted";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Employee_CommitteeDTO DeleteDocumentRecordCommettee(HR_Employee_CommitteeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                List<HR_Employee_CommitteeDMO> lorg = new List<HR_Employee_CommitteeDMO>();
                lorg = _context.HR_Employee_CommitteeDMO.Where(t => t.HRME_Id.Equals(dto.HRME_Id) && t.MI_Id == dto.MI_Id && t.HRECOM_Id == dto.HRECOM_Id).ToList();
                foreach (HR_Employee_CommitteeDMO ph1 in lorg)
                {
                    _context.Remove(ph1);
                    _context.SaveChanges();
                    dto.retrunMsg = "Deleted";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Employee_OtherDetailsDTO DeleteDocumentRecordOthers(HR_Employee_OtherDetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                List<HR_Employee_OtherDetailsDMO> lorg = new List<HR_Employee_OtherDetailsDMO>();
                lorg = _context.HR_Employee_OtherDetailsDMO.Where(t => t.HRME_Id.Equals(dto.HRME_Id) && t.MI_Id == dto.MI_Id && t.HREOTHDET_Id == dto.HREOTHDET_Id).ToList();
                foreach (HR_Employee_OtherDetailsDMO ph1 in lorg)
                {
                    _context.Remove(ph1);
                    _context.SaveChanges();
                    dto.retrunMsg = "Deleted";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HRMS_NAAC_DTO editRecord(HRMS_NAAC_DTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.TabName.Equals("OrientationTab"))
                {
                    if (dto.HREORCO_Id > 0)
                    {
                        var Documentsresult = _context.HR_Employee_OrientationCourseDMO.Where(t => t.HREORCO_Id == dto.HREORCO_Id).ToArray();
                        dto.orientlistedit = Documentsresult;
                    }
                }
                else if (dto.TabName.Equals("ExamintionTab"))
                {
                    if (dto.HREEXDT_Id > 0)
                    {
                        var Documentsresult = _context.HR_Employee_ExamDutyDMO.Where(t => t.HREEXDT_Id == dto.HREEXDT_Id).ToArray();
                        dto.examinationlistedit = Documentsresult;
                    }
                }
                else if (dto.TabName.Equals("StudentActivityTab"))
                {
                    if (dto.HRESACT_Id > 0)
                    {
                        var Documentsresult = _context.HR_Employee_StudentActivitiesDMO.Where(t => t.HRESACT_Id == dto.HRESACT_Id).ToArray();
                        dto.StudentActivitylistedit = Documentsresult;
                    }
                }
                else if (dto.TabName.Equals("ProfessionalActivityTab"))
                {
                    if (dto.HREDACT_Id > 0)
                    {
                        var Documentsresult = _context.HR_Employee_DevActivitiesDMO.Where(t => t.HREDACT_Id == dto.HREDACT_Id).ToArray();
                        dto.ProfessionalActivitylistedit = Documentsresult;
                    }
                }
                else if (dto.TabName.Equals("ResearchProjectTab"))
                {
                    if (dto.HREREPR_Id > 0)
                    {
                        var Documentsresult = _context.HR_Employee_ResearchProjectsDMO.Where(t => t.HREREPR_Id == dto.HREREPR_Id).ToArray();
                        dto.ResearchProjectlistedit = Documentsresult;
                    }
                }
                else if (dto.TabName.Equals("ResearchGuidanceTab"))
                {
                    if (dto.HREREGU_Id > 0)
                    {
                        var Documentsresult = _context.HR_Employee_ResearchGuidanceDMO.Where(t => t.HREREGU_Id == dto.HREREGU_Id).ToArray();
                        dto.ResearchGuidelistedit = Documentsresult;
                    }
                }
                else if (dto.TabName.Equals("BOSBOETab"))
                {
                    if (dto.HREBOS_Id > 0)
                    {
                        var Documentsresult = _context.HR_Employee_BOSBOEDMO.Where(t => t.HREBOS_Id == dto.HREBOS_Id).ToArray();
                        dto.BOSBOElistedit = Documentsresult;
                        dto.BOSBOEfilelistedit = _context.HR_Employee_BOSBOE_FilesDMO.Where(t=>t.HREBOS_Id == dto.HREBOS_Id).ToArray();
                    }
                }
                else if (dto.TabName.Equals("JournalTab"))
                {
                    if (dto.HREJORNL_Id > 0)
                    {
                        var Documentsresult = _context.HR_Employee_JournalDMO.Where(t => t.HREJORNL_Id == dto.HREJORNL_Id).ToArray();
                        dto.Journallistedit = Documentsresult;
                    }
                }
                else if (dto.TabName.Equals("ConferenceTab"))
                {
                    if (dto.HRECONF_Id > 0)
                    {
                        var Documentsresult = _context.HR_Employee_ConferenceDMO.Where(t => t.HRECONF_Id == dto.HRECONF_Id).ToArray();
                        dto.Conferencelistedit = Documentsresult;
                    }
                }
                else if (dto.TabName.Equals("BookTab"))
                {
                    if (dto.HREBK_Id > 0)
                    {
                        var Documentsresult = _context.HR_Employee_BookDMO.Where(t => t.HREBK_Id == dto.HREBK_Id).ToArray();
                        dto.Booklistedit = Documentsresult;
                    }
                }
                else if (dto.TabName.Equals("BookChapterTab"))
                {
                    if (dto.HREBKCP_Id > 0)
                    {
                        var Documentsresult = _context.HR_Employee_BookChapterDMO.Where(t => t.HREBKCP_Id == dto.HREBKCP_Id).ToArray();
                        dto.BookChapterlistedit = Documentsresult;
                    }
                }
                else if (dto.TabName.Equals("CommetteeTab"))
                {
                    if (dto.HRECOM_Id > 0)
                    {
                        var Documentsresult = _context.HR_Employee_CommitteeDMO.Where(t => t.HRECOM_Id == dto.HRECOM_Id).ToArray();
                        dto.Commetteelistedit = Documentsresult;
                    }
                }
                else if (dto.TabName.Equals("OtherDetailTab"))
                {
                    if (dto.HREOTHDET_Id > 0)
                    {
                        var Documentsresult = _context.HR_Employee_OtherDetailsDMO.Where(t => t.HREOTHDET_Id == dto.HREOTHDET_Id).ToArray();
                        dto.OtherDetailSlistedit = Documentsresult;
                    }
                }
                //else if (dto.TabName.Equals("GroupADetailTab"))
                //{
                //    EditGroupADetails(dto);
                //}
                //else if (dto.TabName.Equals("GroupBDetailTab"))
                //{
                //    EditGroupBDetails(dto);
                //}
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error";
            }
            return dto;
        }
    }
}
