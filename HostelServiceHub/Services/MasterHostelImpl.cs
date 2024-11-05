using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech;
using DataAccessMsSqlServerProvider.com.vapstech.Hostel;
using DomainModel.Model.com.vapstech.Hostel;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Services
{
    public class MasterHostelImpl : Interface.MasterHostelInterface
    {
        public HostelContext _HostelContext;
        public DomainModelMsSqlServerContext _dbcontext;
        public MasterHostelImpl(HostelContext para1, DomainModelMsSqlServerContext para2)
        {
            _HostelContext = para1;
            _dbcontext = para2;
        }

        #region 
        public HL_Master_Hostel_DTO Page_loaddata(HL_Master_Hostel_DTO data)
        {
            try
            {
                data.countrylist = (from a in _dbcontext.country
                                    select new HL_Master_Hostel_DTO
                                    {
                                        IVRMMC_CountryName = a.IVRMMC_CountryName,
                                        IVRMMC_Id = a.IVRMMC_Id,
                                        IVRMMC_CountryCode = a.IVRMMC_CountryCode
                                    }).Distinct().OrderBy(t => t.IVRMMC_CountryName).ToArray();

                data.mess_list = (from a in _HostelContext.HL_Master_Mess_DMO
                                  where (a.MI_Id == data.MI_Id && a.HLMM_ActiveFlag == true)
                                  select new HL_Master_Hostel_DTO
                                  {
                                      HLMM_Id = a.HLMM_Id,
                                      HLMM_Name = a.HLMM_Name,
                                  }).Distinct().OrderBy(t=>t.HLMM_Id).ToArray();

                data.facilities_list = (from a in _HostelContext.HL_Master_Facility_DMO
                                        where (a.MI_Id == data.MI_Id && a.HLMFTY_ActiveFlag == true)
                                        select new HL_Master_Hostel_DTO
                                        {
                                            HLMFTY_Id = a.HLMFTY_Id,
                                            HLMFTY_FacilityName = a.HLMFTY_FacilityName,
                                        }).Distinct().OrderBy(t => t.HLMFTY_Id).ToArray();             
               

                data.get_gridlistdata = (from a in _HostelContext.HL_Master_Hostel_DMO
                                         from b in _HostelContext.country
                                         from c in _HostelContext.State
                                         from m in _HostelContext.HL_Master_Hostel_Mess_DMO
                                         from m2 in _HostelContext.HL_Master_Mess_DMO
                                         where (a.MI_Id == data.MI_Id && a.IVRMMS_Id == c.IVRMMS_Id && b.IVRMMC_Id == c.IVRMMC_Id && a.HLMH_Id==m.HLMH_Id && m.HLMM_Id==m2.HLMM_Id)
                                         select new HL_Master_Hostel_DTO
                                         {
                                             HLMH_Id = a.HLMH_Id,
                                             HLMH_Name = a.HLMH_Name,
                                             HLMH_City = a.HLMH_City,
                                             HLMH_Pincode = a.HLMH_Pincode,
                                             IVRMMC_CountryName = b.IVRMMC_CountryName,
                                             IVRMMS_Name = c.IVRMMS_Name,
                                             HLMH_ContactNo = a.HLMH_ContactNo,
                                             HLMH_TotalFloor = a.HLMH_TotalFloor,
                                             HLMH_TotalRoom = a.HLMH_TotalRoom,
                                             HLMH_Address = a.HLMH_Address,
                                             HLMH_ActiveFlag = a.HLMH_ActiveFlag,
                                             IVRMMS_Id = a.IVRMMS_Id,
                                             HLMH_Desc = a.HLMH_Desc,                                            
                                             HLMH_TotalCapacity = a.HLMH_TotalCapacity,
                                             HLMM_Name = m2.HLMM_Name,
                                         }).Distinct().OrderByDescending(t => t.HLMH_Id).ToArray();

                var emprecords = _HostelContext.HL_Master_Hostel_Warden_DMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                if (emprecords.Count > 0)
                {
                    List<long> emp_ids = new List<long>();
                    foreach (var item in emprecords)
                    {
                        emp_ids.Add(item.HRME_Id);
                    }
                    data.employee_list = (from a in _dbcontext.HR_Master_Employee_DMO
                                          where (a.MI_Id == data.MI_Id && !emp_ids.Contains(a.HRME_Id) && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                          select new HL_Master_Hostel_DTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                              HRME_EmployeeCode = a.HRME_EmployeeCode,
                                          }).Distinct().OrderBy(t => t.HRME_Id).ToArray();
                }
                else
                {
                    data.employee_list = (from a in _dbcontext.HR_Master_Employee_DMO
                                          where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                          select new HL_Master_Hostel_DTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                              HRME_EmployeeCode = a.HRME_EmployeeCode,
                                          }).Distinct().OrderBy(t => t.HRME_Id).ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HL_Master_Hostel_DTO Get_StateData(HL_Master_Hostel_DTO data)
        {
            try
            {
                data.statelistdata = (from b in _dbcontext.State
                                      where (b.IVRMMC_Id == data.IVRMMC_Id)
                                      select new HL_Master_Hostel_DTO
                                      {
                                          IVRMMS_Name = b.IVRMMS_Name,
                                          IVRMMS_Id = b.IVRMMS_Id,
                                          IVRMMS_Code = b.IVRMMS_Code,
                                      }).Distinct().OrderBy(t => t.IVRMMS_Name).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HL_Master_Hostel_DTO Save_Hostel_Data(HL_Master_Hostel_DTO data)
        {
            try
            {
                if (data.HLMH_Id == 0)
                {
                    var duplicate = _HostelContext.HL_Master_Hostel_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMH_Name == data.HLMH_Name && t.HLMH_BoysGirlsFlg == data.HLMH_BoysGirlsFlg && t.IVRMMC_Id == data.IVRMMC_Id && t.IVRMMS_Id == data.IVRMMS_Id && t.HLMH_City == data.HLMH_City && t.HLMH_Address == data.HLMH_Address && t.HLMH_City == data.HLMH_City && t.HLMH_TotalFloor == data.HLMH_TotalFloor && t.HLMH_TotalRoom == data.HLMH_TotalRoom && t.HLMH_TotalCapacity == data.HLMH_TotalCapacity && t.HLMH_Pincode == data.HLMH_Pincode).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        HL_Master_Hostel_DMO obj1 = new HL_Master_Hostel_DMO();

                        obj1.HLMH_Id = data.HLMH_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.HLMH_Name = data.HLMH_Name;
                        obj1.HLMH_BoysGirlsFlg = data.HLMH_BoysGirlsFlg;
                        obj1.HLMH_Address = data.HLMH_Address;
                        obj1.IVRMMC_Id = data.IVRMMC_Id;
                        obj1.IVRMMS_Id = data.IVRMMS_Id;
                        obj1.HLMH_City = data.HLMH_City;
                        obj1.HLMH_Pincode = data.HLMH_Pincode;
                        obj1.HLMH_ContactNo = data.HLMH_ContactNo;
                        obj1.HLMH_TotalFloor = data.HLMH_TotalFloor;
                        obj1.HLMH_TotalRoom = data.HLMH_TotalRoom;
                        obj1.HLMH_TotalCapacity = data.HLMH_TotalCapacity;
                        obj1.HLMH_Desc = data.HLMH_Desc;
                        obj1.HLMH_ActiveFlag = true;
                        obj1.CreatedDate = DateTime.Now;
                        obj1.UpdatedDate = DateTime.Now;
                        _HostelContext.Add(obj1);

                       

                        if (data.hostelPictureUploadDTO.Length > 0)
                        {
                            for (int s = 0; s < data.hostelPictureUploadDTO.Length; s++)
                            {
                                if (data.hostelPictureUploadDTO[0].HLMHP_FilePath != null)
                                {
                                    HL_Master_Hostel_Photos_DMO obj2 = new HL_Master_Hostel_Photos_DMO();

                                    obj2.HLMH_Id = obj1.HLMH_Id;
                                    obj2.MI_Id = data.MI_Id;
                                    obj2.HLMHP_FileName = data.hostelPictureUploadDTO[s].HLMHP_FileName;
                                    obj2.HLMHP_FilePath = data.hostelPictureUploadDTO[s].HLMHP_FilePath;
                                    obj2.HLMHP_ActiveFlg = true;

                                    _HostelContext.Add(obj2);
                                }
                                
                            }
                        }
                        else
                        {
                            for (int s = 0; s < data.hostelPictureUploadDTO.Length; s++)
                            {
                                if (data.hostelPictureUploadDTO[0].HLMHP_FilePath != null)
                                {
                                    HL_Master_Hostel_Photos_DMO obj2 = new HL_Master_Hostel_Photos_DMO();

                                    obj2.HLMH_Id = obj1.HLMH_Id;
                                    obj2.MI_Id = data.MI_Id;
                                    obj2.HLMHP_FileName = data.hostelPictureUploadDTO[s].HLMHP_FileName;
                                    obj2.HLMHP_FilePath = data.hostelPictureUploadDTO[s].HLMHP_FilePath;
                                    obj2.HLMHP_ActiveFlg = true;

                                    _HostelContext.Add(obj2);
                                }
                            }
                        }
                        if (data.selectedFacilityDTO.Length > 0)
                        {
                            for (int s = 0; s < data.selectedFacilityDTO.Length; s++)
                            {
                                HL_Master_Hostel_Facilities_DMO obj3 = new HL_Master_Hostel_Facilities_DMO();

                               obj3.HLMH_Id = obj1.HLMH_Id;
                               obj3.MI_Id = data.MI_Id;
                               obj3.HLMFTY_Id = data.selectedFacilityDTO[s].HLMFTY_Id;
                                obj3.HLMHF_ActiveFlg = true;

                                _HostelContext.Add(obj3);
                            }
                        }
                        else
                        {
                            for (int s = 0; s < data.selectedFacilityDTO.Length; s++)
                            {
                                HL_Master_Hostel_Facilities_DMO obj3 = new HL_Master_Hostel_Facilities_DMO();

                                obj3.HLMH_Id = obj1.HLMH_Id;
                                obj3.MI_Id = data.MI_Id;
                                obj3.HLMFTY_Id = data.selectedFacilityDTO[s].HLMFTY_Id;
                                obj3.HLMHF_ActiveFlg = true;

                                _HostelContext.Add(obj3);
                            }
                        }
                        if (data.selectedWardenDTO.Length > 0)
                        {
                            for (int s = 0; s < data.selectedWardenDTO.Length; s++)
                            {
                                HL_Master_Hostel_Warden_DMO obj4 = new HL_Master_Hostel_Warden_DMO();

                                obj4.HLMH_Id = obj1.HLMH_Id;
                                obj4.MI_Id = data.MI_Id;
                                obj4.HLMHW_WardenType = data.HLMHW_WardenType;
                                obj4.HRME_Id = data.selectedWardenDTO[s].HRME_Id;
                                obj4.HLMHW_ActiveFlg = true;

                                _HostelContext.Add(obj4);
                            }
                        }
                        else
                        {
                            for (int s = 0; s < data.selectedWardenDTO.Length; s++)
                            {
                                HL_Master_Hostel_Warden_DMO obj3 = new HL_Master_Hostel_Warden_DMO();

                                obj3.HLMH_Id = obj1.HLMH_Id;
                                obj3.MI_Id = data.MI_Id;
                                obj3.HRME_Id = data.selectedWardenDTO[s].HRME_Id;
                                obj3.HLMHW_ActiveFlg = true;

                                _HostelContext.Add(obj3);
                            }
                        }

                        if (data.selectedMessDTO.Length > 0)
                        {
                            for (int s = 0; s < data.selectedMessDTO.Length; s++)
                            {


                                HL_Master_Hostel_Mess_DMO objfac = new HL_Master_Hostel_Mess_DMO();

                                objfac.HLMH_Id = obj1.HLMH_Id;
                                objfac.MI_Id = data.MI_Id;
                                objfac.HLMM_Id = data.selectedMessDTO[s].HLMM_Id;
                                objfac.HLMHMS_ActiveFlg = true;
                                _HostelContext.Add(objfac);
                            }
                        }
                        else
                        {
                            for (int s = 0; s < data.selectedMessDTO.Length; s++)
                            {

                                HL_Master_Hostel_Mess_DMO objfac1 = new HL_Master_Hostel_Mess_DMO();

                                objfac1.HLMH_Id = obj1.HLMH_Id;
                                objfac1.MI_Id = data.MI_Id;
                                objfac1.HLMM_Id = data.selectedMessDTO[s].HLMM_Id;
                                objfac1.HLMHMS_ActiveFlg = true;
                                _HostelContext.Add(objfac1);                              
                            }
                        }
                        int row = _HostelContext.SaveChanges();
                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.HLMH_Id > 0)
                {
                    var duplicate = _HostelContext.HL_Master_Hostel_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMH_Id != data.HLMH_Id && t.HLMH_Name == data.HLMH_Name && t.HLMH_BoysGirlsFlg == data.HLMH_BoysGirlsFlg && t.IVRMMC_Id == data.IVRMMC_Id && t.IVRMMS_Id == data.IVRMMS_Id && t.HLMH_City == data.HLMH_City && t.HLMH_Address == data.HLMH_Address && t.HLMH_City == data.HLMH_City && t.HLMH_TotalFloor == data.HLMH_TotalFloor && t.HLMH_TotalRoom == data.HLMH_TotalRoom && t.HLMH_TotalCapacity == data.HLMH_TotalCapacity && t.HLMH_Pincode == data.HLMH_Pincode).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _HostelContext.HL_Master_Hostel_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMH_Id == data.HLMH_Id).Single();

                        update.HLMH_Name = data.HLMH_Name;
                        update.HLMH_BoysGirlsFlg = data.HLMH_BoysGirlsFlg;
                        update.HLMH_Address = data.HLMH_Address;
                        update.IVRMMC_Id = data.IVRMMC_Id;
                        update.IVRMMS_Id = data.IVRMMS_Id;
                        update.HLMH_City = data.HLMH_City;
                        update.HLMH_Pincode = data.HLMH_Pincode;
                        update.HLMH_ContactNo = data.HLMH_ContactNo;
                        update.HLMH_TotalFloor = data.HLMH_TotalFloor;
                        update.HLMH_TotalRoom = data.HLMH_TotalRoom;
                        update.HLMH_TotalCapacity = data.HLMH_TotalCapacity;
                        update.HLMH_Desc = data.HLMH_Desc;
                        update.UpdatedDate = DateTime.Now;

                        _HostelContext.Update(update);

                    // var result=_HostelContext.HL_Master_Hostel_Mess_DMO.Where(t=>t.HLMH_Id==data.HLMH_Id).Single();
                       
                       // result.HLMH_Id = update.HLMH_Id;
                        //result.HLMM_Id = data.HLMM_Id;
                        //_HostelContext.Update(result);

                        delete_hostel_photo(data.HLMH_Id);

                        if (data.hostelPictureUploadDTO.Length > 0)
                        {
                            for (int s = 0; s < data.hostelPictureUploadDTO.Length; s++)
                            {
                                if (data.hostelPictureUploadDTO[0].HLMHP_FilePath != null)
                                {
                                    HL_Master_Hostel_Photos_DMO obj2 = new HL_Master_Hostel_Photos_DMO();

                                    obj2.HLMH_Id = update.HLMH_Id;
                                    obj2.MI_Id = data.MI_Id;
                                    obj2.HLMHP_FileName = data.hostelPictureUploadDTO[s].HLMHP_FileName;
                                    obj2.HLMHP_FilePath = data.hostelPictureUploadDTO[s].HLMHP_FilePath;
                                    obj2.HLMHP_ActiveFlg = true;

                                    _HostelContext.Add(obj2);
                                }
                            }
                        }
                        else
                        {
                            for (int s = 0; s < data.hostelPictureUploadDTO.Length; s++)
                            {
                                if (data.hostelPictureUploadDTO[0].HLMHP_FilePath != null)
                                {
                                    HL_Master_Hostel_Photos_DMO obj2 = new HL_Master_Hostel_Photos_DMO();

                                    obj2.HLMH_Id = update.HLMH_Id;
                                    obj2.MI_Id = data.MI_Id;
                                    obj2.HLMHP_FileName = data.hostelPictureUploadDTO[s].HLMHP_FileName;
                                    obj2.HLMHP_FilePath = data.hostelPictureUploadDTO[s].HLMHP_FilePath;
                                    obj2.HLMHP_ActiveFlg = true;

                                    _HostelContext.Add(obj2);
                                }
                            }
                        }

                        delete_hostel_Facility(data.HLMH_Id);
                        if (data.selectedFacilityDTO.Length > 0)
                        {
                            for (int s = 0; s < data.selectedFacilityDTO.Length; s++)
                            {
                                HL_Master_Hostel_Facilities_DMO obj3 = new HL_Master_Hostel_Facilities_DMO();

                                obj3.HLMH_Id = update.HLMH_Id;
                                obj3.MI_Id = data.MI_Id;
                                obj3.HLMFTY_Id = data.selectedFacilityDTO[s].HLMFTY_Id;
                                obj3.HLMHF_ActiveFlg = true;

                                _HostelContext.Add(obj3);
                            }
                        }
                        else
                        {
                            for (int s = 0; s < data.selectedFacilityDTO.Length; s++)
                            {
                                HL_Master_Hostel_Facilities_DMO obj3 = new HL_Master_Hostel_Facilities_DMO();

                                obj3.HLMH_Id = update.HLMH_Id;
                                obj3.MI_Id = data.MI_Id;
                                obj3.HLMFTY_Id = data.selectedFacilityDTO[s].HLMFTY_Id;
                                obj3.HLMHF_ActiveFlg = true;

                                _HostelContext.Add(obj3);
                            }
                        }

                        delete_hostel_Warden(data.HLMH_Id);
                        if (data.selectedWardenDTO.Length > 0)
                        {
                            for (int s = 0; s < data.selectedWardenDTO.Length; s++)
                            {
                                HL_Master_Hostel_Warden_DMO obj4 = new HL_Master_Hostel_Warden_DMO();

                                obj4.HLMH_Id = update.HLMH_Id;
                                obj4.MI_Id = data.MI_Id;
                                obj4.HRME_Id = data.selectedWardenDTO[s].HRME_Id;
                                obj4.HLMHW_WardenType = data.HLMHW_WardenType;
                                obj4.HLMHW_ActiveFlg = true;

                                _HostelContext.Add(obj4);
                            }
                        }
                        else
                        {
                            for (int s = 0; s < data.selectedWardenDTO.Length; s++)
                            {
                                HL_Master_Hostel_Warden_DMO obj3 = new HL_Master_Hostel_Warden_DMO();

                                obj3.HLMH_Id = update.HLMH_Id;
                                obj3.MI_Id = data.MI_Id;
                                obj3.HRME_Id = data.selectedWardenDTO[s].HRME_Id;
                                obj3.HLMHW_ActiveFlg = true;

                                _HostelContext.Add(obj3);
                            }
                        }

                        delete_hostel_Mess(data.HLMH_Id);
                        if (data.selectedMessDTO.Length > 0)
                        {
                            for (int s = 0; s < data.selectedMessDTO.Length; s++)
                            {


                                HL_Master_Hostel_Mess_DMO objfac = new HL_Master_Hostel_Mess_DMO();

                                objfac.HLMH_Id = update.HLMH_Id;
                                objfac.MI_Id = data.MI_Id;
                                objfac.HLMM_Id = data.selectedMessDTO[s].HLMM_Id;
                                objfac.HLMHMS_ActiveFlg = true;
                                _HostelContext.Add(objfac);
                            }
                        }
                        else
                        {
                            for (int s = 0; s < data.selectedMessDTO.Length; s++)
                            {

                                HL_Master_Hostel_Mess_DMO objfac1 = new HL_Master_Hostel_Mess_DMO();

                                objfac1.HLMH_Id = update.HLMH_Id;
                                objfac1.MI_Id = data.MI_Id;
                                objfac1.HLMM_Id = data.selectedMessDTO[s].HLMM_Id;
                                objfac1.HLMHMS_ActiveFlg = true;
                                _HostelContext.Add(objfac1);
                            }
                        }

                        int row = _HostelContext.SaveChanges();
                        if (row > 0)
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
            }
            return data;
        }
        public HL_Master_Hostel_DTO Edit_Hostel_Row(HL_Master_Hostel_DTO data)
        {
            try
            {
                var editlist = _HostelContext.HL_Master_Hostel_DMO.Where(T => T.HLMH_Id == data.HLMH_Id).ToList();

                data.edit_hostel_row = editlist.ToArray();

                data.editphotolist = _HostelContext.HL_Master_Hostel_Photos_DMO.Where(t => t.HLMH_Id == data.HLMH_Id).ToArray();

                List<long> countryid = new List<long>();
                foreach (var item in editlist)
                {
                    countryid.Add(item.IVRMMC_Id);
                }
                var get_state_country_id = (from b in _dbcontext.State
                                            where (countryid.Contains(b.IVRMMC_Id))
                                            select new HL_Master_Hostel_DTO
                                            {
                                                IVRMMS_Name = b.IVRMMS_Name,
                                                IVRMMS_Id = b.IVRMMS_Id,
                                                IVRMMC_Id = b.IVRMMC_Id,

                                            }).Distinct().OrderBy(t => t.IVRMMS_Id).ToList();

                data.statelistdata = get_state_country_id.ToArray();

                data.IVRMMS_Id = editlist[0].IVRMMS_Id;

                data.edithostelmess_id =(from a in _HostelContext.HL_Master_Hostel_Mess_DMO
                                         from b in _HostelContext.HL_Master_Mess_DMO
                                         where(a.HLMH_Id == data.HLMH_Id && a.MI_Id==data.MI_Id && a.HLMM_Id==b.HLMM_Id)
                                         select new HL_Master_Hostel_DTO {
                                             HLMM_Id=b.HLMM_Id,
                                             HLMM_Name=b.HLMM_Name,
                                         }).Distinct().ToArray();

                data.edit_facilitylist = (from a in _HostelContext.HL_Master_Hostel_Facilities_DMO
                                          from b in _HostelContext.HL_Master_Facility_DMO
                                          where (a.HLMH_Id == data.HLMH_Id && a.MI_Id==data.MI_Id && a.HLMFTY_Id==b.HLMFTY_Id && a.MI_Id == b.MI_Id)
                                          select new HL_Master_Hostel_DTO
                                          {
                                              HLMFTY_Id=b.HLMFTY_Id,
                                              HLMFTY_FacilityName=b.HLMFTY_FacilityName,
                                          }).Distinct().ToArray();

                data.edit_emplist = (from a in _HostelContext.HL_Master_Hostel_Warden_DMO                                          
                                     where (a.HLMH_Id == data.HLMH_Id && a.MI_Id == data.MI_Id)
                                          select new HL_Master_Hostel_DTO
                                          {
                                             HRME_Id=a.HRME_Id,
                                              HLMHW_WardenType = a.HLMHW_WardenType,
                                          }).Distinct().ToArray();

                data.edit_mess = (from a in _HostelContext.HL_Master_Hostel_Mess_DMO
                                  where (a.HLMH_Id == data.HLMH_Id && a.MI_Id == data.MI_Id)
                                     select new HL_Master_Hostel_DTO
                                     {
                                         HLMHMS_Id = a.HLMHMS_Id,
                                         HLMH_Id=a.HLMH_Id,
                                         HLMM_Id=a.HLMM_Id,                                        
                                     }).Distinct().ToArray();

                data.edit_photolist = (from a in _HostelContext.HL_Master_Hostel_Photos_DMO
                                       where (a.HLMH_Id == data.HLMH_Id && a.MI_Id == data.MI_Id)
                                       select new HL_Master_Hostel_DTO {
                                           HLMHP_Id = a.HLMHP_Id,
                                           HLMHP_FileName =a.HLMHP_FileName,                                           
                                           HLMHP_FilePath = a.HLMHP_FilePath
                                       }).Distinct().ToArray();

                data.employee_list = (from a in _dbcontext.HR_Master_Employee_DMO
                                      where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                      select new HL_Master_Hostel_DTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                          HRME_EmployeeCode = a.HRME_EmployeeCode,
                                      }).Distinct().OrderBy(t => t.HRME_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HL_Master_Hostel_DTO Deactive_Hostel_Row(HL_Master_Hostel_DTO data)
        {
            try
            {
                var result = _HostelContext.HL_Master_Hostel_DMO.Single(t => t.HLMH_Id == data.HLMH_Id && t.MI_Id == data.MI_Id);

                if (result.HLMH_ActiveFlag == true)
                {
                    result.HLMH_ActiveFlag = false;
                }
                else if (result.HLMH_ActiveFlag == false)
                {
                    result.HLMH_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _HostelContext.Update(result);
                int rowAffected = _HostelContext.SaveChanges();

                if (rowAffected > 0)
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
        public HL_Master_Hostel_DTO delete_hostel_photo(Int64 id)
        {
            HL_Master_Hostel_DTO data = new HL_Master_Hostel_DTO();
            try
            {
                List<HL_Master_Hostel_Photos_DMO> lorg = new List<HL_Master_Hostel_Photos_DMO>();
                lorg = _HostelContext.HL_Master_Hostel_Photos_DMO.Where(t => t.HLMH_Id == id).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _HostelContext.Remove(lorg.ElementAt(i));
                        var contactExists = _HostelContext.SaveChanges();
                        if (contactExists == 1)
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public HL_Master_Hostel_DTO delete_hostel_Facility(Int64 id)
        {
            HL_Master_Hostel_DTO data = new HL_Master_Hostel_DTO();
            try
            {
                List<HL_Master_Hostel_Facilities_DMO> lorg = new List<HL_Master_Hostel_Facilities_DMO>();
                lorg = _HostelContext.HL_Master_Hostel_Facilities_DMO.Where(t => t.HLMH_Id == id).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _HostelContext.Remove(lorg.ElementAt(i));
                        var contactExists = _HostelContext.SaveChanges();
                        if (contactExists == 1)
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
            }

            return data;
        }
        public HL_Master_Hostel_DTO delete_hostel_Warden(Int64 id)
        {
            HL_Master_Hostel_DTO data = new HL_Master_Hostel_DTO();
            try
            {
                List<HL_Master_Hostel_Warden_DMO> lorg = new List<HL_Master_Hostel_Warden_DMO>();
                lorg = _HostelContext.HL_Master_Hostel_Warden_DMO.Where(t => t.HLMH_Id == id).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _HostelContext.Remove(lorg.ElementAt(i));
                        var contactExists = _HostelContext.SaveChanges();
                        if (contactExists == 1)
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HL_Master_Hostel_DTO delete_hostel_Mess(Int64 id)
        {
            HL_Master_Hostel_DTO data = new HL_Master_Hostel_DTO();
            try
            {
                List<HL_Master_Hostel_Mess_DMO> lorg = new List<HL_Master_Hostel_Mess_DMO>();
                lorg = _HostelContext.HL_Master_Hostel_Mess_DMO.Where(t => t.HLMH_Id == id).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _HostelContext.Remove(lorg.ElementAt(i));
                        var contactExists = _HostelContext.SaveChanges();
                        if (contactExists > 0)
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
            }
            return data;
        }
        public HL_Master_Hostel_DTO Get_MappedFacility(HL_Master_Hostel_DTO data)
        {
            try
            {
                data.mappedfacilitylist = (from a in _HostelContext.HL_Master_Hostel_DMO
                                           from b in _HostelContext.HL_Master_Hostel_Facilities_DMO
                                           from d in _HostelContext.HL_Master_Facility_DMO
                                           where (a.HLMH_Id == data.HLMH_Id && a.HLMH_Id == b.HLMH_Id && a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == d.MI_Id && b.HLMFTY_Id == d.HLMFTY_Id)
                                           select new HL_Master_Hostel_DTO
                                           {
                                               HLMFTY_FacilityName = d.HLMFTY_FacilityName,
                                               HLMH_Name = a.HLMH_Name,
                                               HLMH_Id = a.HLMH_Id,
                                               HLMH_TotalRoom = a.HLMH_TotalRoom,
                                               HLMH_TotalCapacity = a.HLMH_TotalCapacity,
                                               HLMH_TotalFloor = a.HLMH_TotalFloor,
                                               HLMHF_Id = b.HLMHF_Id,
                                               HLMHF_ActiveFlg = b.HLMHF_ActiveFlg
                                           }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HL_Master_Hostel_DTO Get_MappedEmpl(HL_Master_Hostel_DTO data)
        {
            try
            {
                data.mappedEmpllist = (from a in _HostelContext.HL_Master_Hostel_DMO
                                           from b in _HostelContext.HL_Master_Hostel_Warden_DMO
                                           from d in _HostelContext.HR_Master_Employee_DMO
                                           where (a.HLMH_Id == data.HLMH_Id && a.HLMH_Id == b.HLMH_Id && a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == d.MI_Id && b.HRME_Id == d.HRME_Id)
                                           select new HL_Master_Hostel_DTO
                                           {                                               
                                               HLMH_Name = a.HLMH_Name,
                                               HLMH_Id = a.HLMH_Id,
                                               HLMH_TotalRoom = a.HLMH_TotalRoom,
                                               HLMH_TotalCapacity = a.HLMH_TotalCapacity,
                                               HLMH_TotalFloor = a.HLMH_TotalFloor,
                                               HRME_Id = b.HRME_Id,
                                               HLMHW_ActiveFlg = b.HLMHW_ActiveFlg,
                                               HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                               HRME_EmployeeCode = d.HRME_EmployeeCode,
                                           }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }






        public HL_Master_Hostel_DTO viewuploadflies(HL_Master_Hostel_DTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _HostelContext.HL_Master_Hostel_Photos_DMO
                                        where (a.HLMH_Id == data.HLMH_Id)
                                        select new HL_Master_Hostel_DTO
                                        {
                                            HLMHP_FileName = a.HLMHP_FileName,
                                            HLMHP_FilePath = a.HLMHP_FilePath,
                                            
                                            HLMHP_Id = a.HLMHP_Id,
                                            HLMH_Id = a.HLMH_Id,
                                        }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;

        }

        public HL_Master_Hostel_DTO deleteuploadfile(HL_Master_Hostel_DTO data)
        {
            try
            {
                var res = _HostelContext.HL_Master_Hostel_Photos_DMO.Where(t => t.HLMHP_Id == data.HLMHP_Id).SingleOrDefault();
                _HostelContext.Remove(res);
                int s = _HostelContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewuploadflies = (from a in _HostelContext.HL_Master_Hostel_Photos_DMO
                                        where (a.HLMH_Id == data.HLMH_Id)
                                        select new HL_Master_Hostel_DTO
                                        {
                                            HLMHP_FileName = a.HLMHP_FileName,
                                            HLMHP_FilePath = a.HLMHP_FilePath,
                                           
                                            HLMHP_Id = a.HLMHP_Id,
                                            HLMH_Id = a.HLMH_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        #endregion 

    }
}
