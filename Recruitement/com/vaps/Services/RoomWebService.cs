using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Recruitment.com.vaps.Services
{
    public class RoomWebService : Interfaces.RoomWebInterface
    {

        public VMSContext _vmsconte;
        public DomainModelMsSqlServerContext _Context;
        public RoomWebService(VMSContext VMSContext, DomainModelMsSqlServerContext Context)
        {
            _vmsconte = VMSContext;
            _Context = Context;
        }

        public HR_Master_Room_DTO getBasicData(HR_Master_Room_DTO Rm)
        {
            Rm.room_list = (from f in _vmsconte.HR_Master_Floor_con
                            from b in _vmsconte.HR_Master_Building_con
                            from c in _vmsconte.HR_Master_Room_con

                            where (c.HRMB_Id == b.HRMB_Id && c.HRMF_Id == f.HRMF_Id && b.HRMB_ActiveFlag == true && f.HRMF_ActiveFlag == true &&  f.MI_Id == b.MI_Id && f.MI_Id == c.MI_Id && c.HRMR_OutSideBookingFlg==true)

                            select new HR_Master_Room_DTO
                            {
                                HRMR_Id = c.HRMR_Id,
                                HRMR_RoomName = c.HRMR_RoomName,
                                HRMB_BuildingName = b.HRMB_BuildingName,
                                HRMF_FloorName = f.HRMF_FloorName,
                                HRMR_ActiveFlag = c.HRMR_ActiveFlag,
                                HRMR_TypeFlag = c.HRMR_TypeFlag,
                                HRMR_Capacity = c.HRMR_Capacity,
                                HRMR_NoOfHrs = c.HRMR_NoOfHrs,
                                HRMR_RentPerDay = c.HRMR_RentPerDay,
                                HRMR_RentPerHour = c.HRMR_RentPerHour,
                                HRMR_Desc = c.HRMR_Desc,
                            }).Distinct().OrderByDescending(a => a.HRMR_Id).ToArray();



            Rm.filearray = (from c in _vmsconte.HR_Master_Room_con
                         from d in _vmsconte.HR_Master_Room_FilesDMO
                         where   c.HRMR_Id==d.HRMR_Id && c.HRMR_OutSideBookingFlg == true && c.HRMR_ActiveFlag==true && d.HRMRFI_DefFlg==true
                         select new HR_Master_Room_DTO
                         {
                             HRMR_Id = c.HRMR_Id,
                             HRMR_RoomName = c.HRMR_RoomName,
                             HRMRFI_FileName = d.HRMRFI_FileName,
                             HRMRFI_FilePath = d.HRMRFI_FilePath,
                             HRMRFI_DefFlg = d.HRMRFI_DefFlg,
                             HRMRFI_Id = d.HRMRFI_Id,
                             HRMR_Capacity = c.HRMR_Capacity,
                             HRMR_NoOfHrs = c.HRMR_NoOfHrs,
                             HRMR_RentPerDay = c.HRMR_RentPerDay,
                             HRMR_RentPerHour = c.HRMR_RentPerHour,
                             HRMR_Desc = c.HRMR_Desc,
                         }).Distinct().OrderByDescending(a => a.HRMR_Id).ToArray();


            Rm.editamenitiespaid = (from a in _vmsconte.HR_Master_Room_AmenitiesDMO
                                    from b in _vmsconte.HR_Master_Amenities_DMO
                                    where  a.HRMAM_Id == b.HRMAM_Id && b.HRMAM_ActiveFlag == true 
                                    select new paidemn1
                                    {
                                        hrmraM_Id = a.HRMRAM_Id,
                                        hrmaM_Id = b.HRMAM_Id,
                                        noofhrs = a.HRMRAM_NoOfHrs,
                                        perday = a.HRMRAM_RentPerDay,
                                        perhours = a.HRMRAM_RentPerHour,
                                        flag =Convert.ToBoolean(b.HRMAM_PriceApplFlg),
                                        HRMR_Id = a.HRMR_Id,
                                        HRMAM_AmenitiesName = b.HRMAM_AmenitiesName,
                                        HRMAM_AmenitiesDes = b.HRMAM_AmenitiesDes,
                                        HRMAM_Price = b.HRMAM_Price,
                                    }).ToArray();


            Rm.employeelistedit = (
                  from f in _vmsconte.HR_Master_Employee_DMO
                  from g in _vmsconte.HR_Master_Room_ContactsDMO
                  where (f.HRME_ActiveFlag == true && f.HRME_LeftFlag == false
                 && f.HRME_Id == g.HRME_Id )
                  select new HR_Master_Room_DTO
                  {
                      HRME_Id = f.HRME_Id,
                      HRME_EmployeeFirstName = f.HRME_EmployeeFirstName + " " + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + " " + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                      HRME_EmployeeCode = f.HRME_EmployeeCode,
                      HRMR_Id = g.HRMR_Id,


                  }).Distinct().OrderBy(w => w.HRME_EmployeeFirstName).ToArray(); ;

            return Rm;
        }

        
    }
}
