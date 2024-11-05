
using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.HRMS;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.HRMS;
using Microsoft.EntityFrameworkCore;
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
    public class HrmsNAACReportImpl : Interface.HrmsNAACReportInterface
    {
        public NaacHRMSContext _context;
        public HRMSContext _HRMSContext;

        public HrmsNAACReportImpl(NaacHRMSContext context, HRMSContext HRMSContext)
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

        public HRMS_NAAC_DTO SaveData(HRMS_NAAC_DTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.TabName.Equals("OrientationTab"))
                {
                    AddUpdateEmployeeOrientationDetails(dto);
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
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error";
            }
            return dto;
        }

        //Document details
        public HRMS_NAAC_DTO AddUpdateEmployeeOrientationDetails(HRMS_NAAC_DTO dto)
        {
            //add/update Documents details
            try
            {
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

        public HRMS_NAAC_DTO AddUpdateResearchProjectDetails(HRMS_NAAC_DTO dto)
        {
            try
            {
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
            try
            {
                if (dto.HR_Employee_BOSBOEArrayDTO.Count() > 0)
                {
                    foreach (HR_Employee_BOSBOEDTO DocumentsDTO in dto.HR_Employee_BOSBOEArrayDTO)
                    {
                        DocumentsDTO.MI_Id = dto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.HRME_Id;
                        HR_Employee_BOSBOEDMO Documents = Mapper.Map<HR_Employee_BOSBOEDMO>(DocumentsDTO);

                        if (Documents.HREBOS_Id > 0)
                        {
                            var Documentsresult = _context.HR_Employee_BOSBOEDMO.Single(t => t.HREBOS_Id == DocumentsDTO.HREBOS_Id);
                            Documentsresult.UpdatedDate = DateTime.Now;
                            Documents.HREBOS_UpdatedBy = dto.LogInUserId;
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
                            Documents.HREBOS_ActiveFlg = true;
                            Documents.HREBOS_CreatedBy = dto.LogInUserId;
                            Documents.HREBOS_UpdatedBy = dto.LogInUserId;
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

        public HRMS_NAAC_DTO AddUpdateJournalDetails(HRMS_NAAC_DTO dto)
        {
            try
            {
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
                    dto.ProfessionalActivitylist = (from a in _context.HR_Employee_StudentActivitiesDMO
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

        public HRMS_NAAC_DTO get_EmployeALLDATA(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.HRME_Id > 0)
                {
                    dto.NAACPersonalDeatilsDTO = (from a in _HRMSContext.MasterEmployee
                                                  from b in _HRMSContext.Master_Employee_Qulaification
                                                  from c in _HRMSContext.HR_Master_Department
                                                  where (a.MI_Id == dto.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == dto.HRME_Id && a.HRME_Id == b.HRME_Id && a.HRMD_Id == c.HRMD_Id)
                                                  select new NAACPersonalDeatilsDTO
                                                  {
                                                      HRME_Id = a.HRME_Id,
                                                      HRMD_Id = a.HRMD_Id,
                                                      HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                                      HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                                      HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                                      HRMD_DepartmentName = c.HRMD_DepartmentName,
                                                      HRME_DOJ = a.HRME_DOJ,
                                                      HRME_QualificationName = b.HRME_QualificationName
                                                  }).ToArray();

                    dto.internatiolnalcount = _context.HR_Employee_JournalDMO.Where(a => a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id && a.HREJORNL_ActiveFlg == true && a.HREJORNL_NatOrIntNatFlg == "International").ToArray().Count();
                    dto.nationalcount = _context.HR_Employee_JournalDMO.Where(a => a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id && a.HREJORNL_ActiveFlg == true && a.HREJORNL_NatOrIntNatFlg == "National").ToArray().Count();
                    dto.nonrefjoucount = _context.HR_Employee_JournalDMO.Where(a => a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id && a.HREJORNL_ActiveFlg == true && a.HREJORNL_RefOrNonRefFlg == "Non-Refereed").ToArray().Count();
                    //dto.patentcount = _context.HR_Employee_JournalDMO.Where(a => a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id && a.HREJORNL_ActiveFlg == true && a.HREJORNL_RefOrNonRefFlg == "Non-Refereed").ToArray().Count();
                    dto.bookcount = _context.HR_Employee_BookDMO.Where(a => a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id && a.HREBK_ActiveFlg == true).ToArray().Count();
                    dto.bookchaptercount = _context.HR_Employee_BookChapterDMO.Where(a => a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id && a.HREBKCP_ActiveFlg == true).ToArray().Count();
                    //dto.citationcount = _context.HR_Employee_BookDMO.Where(a => a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id && a.HREBK_ActiveFlg == true).ToArray().Count();


                    dto.orientlist = (from a in _context.HR_Employee_OrientationCourseDMO
                                      where (a.MI_Id == dto.MI_Id && a.HREORCO_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                      select a).ToArray();

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

                    dto.RefJournallist = (from a in _context.HR_Employee_JournalDMO
                                          where (a.MI_Id == dto.MI_Id && a.HREJORNL_ActiveFlg == true && a.HRME_Id == dto.HRME_Id && a.HREJORNL_RefOrNonRefFlg == "Refereed")
                                          select a).ToArray();

                    dto.NonRefJournallist = (from a in _context.HR_Employee_JournalDMO
                                             where (a.MI_Id == dto.MI_Id && a.HREJORNL_ActiveFlg == true && a.HRME_Id == dto.HRME_Id && a.HREJORNL_RefOrNonRefFlg == "Non-Refereed")
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

                    dto.DocumentDetails = (from a in _context.Master_Employee_Documents
                                           where (a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id)
                                           select a).ToArray();

                    dto.QualifctionDetails = (from a in _context.Master_Employee_Qulaification
                                           where (a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id && a.HRMEQ_Date>= dto.assfromdate && a.HRMEQ_Date <= dto.assessmentto)
                                           select a).ToArray();

                    dto.examinationlist = (from a in _context.HR_Employee_ExamDutyDMO
                                              where (a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id && a.HREEXDT_ActiveFlg == true)
                                              select a).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }


    }
}
