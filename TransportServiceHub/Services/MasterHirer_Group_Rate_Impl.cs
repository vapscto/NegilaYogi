using PreadmissionDTOs.com.vaps.Transport;
using System;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DomainModel.Model.com.vapstech.Transport;
using System.Threading.Tasks;

namespace TransportServiceHub.Services
{
    public class MasterHirer_Group_Rate_Impl:Interfaces.MasterHire_Group_RateInterface
    {
        public TransportContext _context;
        public MasterHirer_Group_Rate_Impl(TransportContext context)
        {
            _context = context;
        }
        //Master Hirer Group.
        public MasterHirer_Group_RateDTO getdata(MasterHirer_Group_RateDTO obj)
        {
            try
            {
                var query = _context.MasterHirerGroupDMO.Where(d => d.MI_Id == obj.MI_Id).ToList();
                if(query.Count > 0)
                {
                    obj.hirerGroupList = query.ToArray();
                    obj.count = query.Count;
                }
                else
                {
                    obj.count = 0;
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public MasterHirer_Group_RateDTO save(MasterHirer_Group_RateDTO data)
        {
            try
            {
                if (data.TRHG_Id == 0)
                {
                    var duplicateCheck = _context.MasterHirerGroupDMO.Where(d => d.MI_Id == data.MI_Id && d.TRHG_HirerGroup.Equals(data.TRHG_HirerGroup)).ToList();
                    if (duplicateCheck.Count > 0)
                    {
                        data.returnVal = "duplicate";
                    }
                    else
                    {
                        var mapp = new MasterHirerGroupDMO();
                        mapp.CreatedDate = DateTime.Now;
                        mapp.UpdatedDate = DateTime.Now;
                        mapp.TRHG_ActiveFlg = true;
                        mapp.MI_Id = data.MI_Id;
                        mapp.TRHG_HirerDec = data.TRHG_HirerDec;
                        mapp.TRHG_HirerGroup = data.TRHG_HirerGroup;
                        _context.Add(mapp);
                        var flag = _context.SaveChanges();
                        if (flag > 0)
                        {
                            data.returnVal = "saved";
                        }
                        else
                        {
                            data.returnVal = "savingFailed";
                        }

                    }
                }
                else
                {
                    var duplicateCheck = _context.MasterHirerGroupDMO.Where(d => d.MI_Id == data.MI_Id && d.TRHG_HirerGroup.Equals(data.TRHG_HirerGroup) && d.TRHG_Id != data.TRHG_Id).ToList();
                    if (duplicateCheck.Count > 0)
                    {
                        data.returnVal = "duplicate";
                    }
                    else
                    {
                        var result = _context.MasterHirerGroupDMO.Single(d => d.TRHG_Id == data.TRHG_Id);
                        result.TRHG_HirerGroup = data.TRHG_HirerGroup;
                        result.TRHG_HirerDec = data.TRHG_HirerDec;
                        result.UpdatedDate = DateTime.Now;
                        _context.Update(result);
                        var update = _context.SaveChanges();
                        if (update > 0)
                        {
                            data.returnVal = "updated";
                        }
                        else
                        {
                            data.returnVal = "failedUpdate";
                        }

                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            return data;
        }
        public MasterHirer_Group_RateDTO edit(int id)
        {
            MasterHirer_Group_RateDTO obj = new MasterHirer_Group_RateDTO();
            try
            {
                obj.editDataList = _context.MasterHirerGroupDMO.Where(d=>d.TRHG_Id==id).ToArray();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public MasterHirer_Group_RateDTO deactivate(MasterHirer_Group_RateDTO dto)
        {
            MasterHirer_Group_RateDTO dt = new MasterHirer_Group_RateDTO();
            try
            {
                var query = _context.MasterHirerGroupDMO.Single(d => d.TRHG_Id == dto.TRHG_Id);
                query.TRHG_ActiveFlg = dto.TRHG_ActiveFlg;
                _context.Update(query);
                var deactive= _context.SaveChanges();
                if(deactive > 0)
                {
                    if (dto.TRHG_ActiveFlg == true)
                    {
                        dt.returnVal = "Record Activated Successfully";
                    }
                    else if (dto.TRHG_ActiveFlg == false)
                    {
                        dt.returnVal = "Record Deactivated Successfully";
                    }
                   
                }
                else
                {
                    if (dto.TRHG_ActiveFlg == true)
                    {
                        dt.returnVal = "Failed to activate record";
                    }
                    else if (dto.TRHG_ActiveFlg == false)
                    {
                        dt.returnVal = "Failed to deactivate record";
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                if (dto.TRHG_ActiveFlg == true)
                {
                    dt.returnVal = "Failed to activate record";
                }
                else if (dto.TRHG_ActiveFlg == false)
                {
                    dt.returnVal = "Failed to deactivate record";
                }
            }
            return dt;
        }

        //Master Hirer Rate.
        public MasterHirer_Group_RateDTO getRatedata(MasterHirer_Group_RateDTO obj)
        {
            try
            {
                var grouplist = _context.MasterHirerGroupDMO.Where(d => d.MI_Id == obj.MI_Id && d.TRHG_ActiveFlg == true).Select(d=>new MasterHirer_Group_RateDTO {TRHG_Id=d.TRHG_Id,TRHG_HirerGroup=d.TRHG_HirerGroup}).ToList();
                if(grouplist.Count > 0)
                {
                    obj.hirerGroupList = grouplist.ToArray();
                }
                var vhcle = _context.MasterVehicleTypeDMO.Where(d => d.MI_Id == obj.MI_Id && d.TRMVT_ActiveFlg == true).Select(d => new MasterHirer_Group_RateDTO { TRMVT_Id = d.TRMVT_Id, TRMVT_VehicleType = d.TRMVT_VehicleType }).ToList();
                if(vhcle.Count > 0)
                {
                    obj.vhcleTypeList = vhcle.ToArray();
                }
                var query = (from m in _context.MasterHirerRateDMO
                             from n in _context.MasterHirerGroupDMO
                             from o in _context.MasterVehicleTypeDMO
                             where m.TRHG_Id == n.TRHG_Id && m.TRMVT_Id == o.TRMVT_Id && m.MI_Id == obj.MI_Id
                             select new MasterHirer_Group_RateDTO
                             {
                                 TRHR_Id=m.TRHR_Id,
                                 TRHG_Id=m.TRHG_Id,
                                 TRMVT_Id=m.TRMVT_Id,
                                 TRHG_HirerGroup=n.TRHG_HirerGroup,
                                 TRMVT_VehicleType=o.TRMVT_VehicleType,
                                 TRHR_RatePerKM=m.TRHR_RatePerKM
                             }).ToList();
                    
                   
                if (query.Count > 0)
                {
                    obj.hirerRateList = query.ToArray();
                    obj.count = query.Count;
                }
                else
                {
                    obj.count = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public MasterHirer_Group_RateDTO saveRate(MasterHirer_Group_RateDTO data)
        {
            try
            {
                if (data.TRHR_Id == 0)
                {
                    var duplicateCheck = _context.MasterHirerRateDMO.Where(d => d.MI_Id == data.MI_Id && d.TRMVT_Id==data.TRMVT_Id && d.TRHG_Id==data.TRHG_Id).ToList();
                    if (duplicateCheck.Count > 0)
                    {
                        data.returnVal = "duplicate";
                    }
                    else
                    {
                        var mapp = new MasterHirerRateDMO();
                        mapp.CreatedDate = DateTime.Now;
                        mapp.UpdatedDate = DateTime.Now;
                        mapp.TRHG_Id = data.TRHG_Id;
                        mapp.MI_Id = data.MI_Id;
                        mapp.TRMVT_Id = data.TRMVT_Id;
                        mapp.TRHR_RatePerKM = data.TRHR_RatePerKM;
                        _context.Add(mapp);
                        var flag = _context.SaveChanges();
                        if (flag > 0)
                        {
                            data.returnVal = "saved";
                        }
                        else
                        {
                            data.returnVal = "savingFailed";
                        }

                    }
                }
                else
                {
                    var duplicateCheck = _context.MasterHirerRateDMO.Where(d => d.MI_Id == data.MI_Id && d.TRMVT_Id == data.TRMVT_Id && d.TRHG_Id == data.TRHG_Id && d.TRHR_Id != data.TRHR_Id).ToList();
                    if (duplicateCheck.Count > 0)
                    {
                        data.returnVal = "duplicate";
                    }
                    else
                    {
                        var result = _context.MasterHirerRateDMO.Single(d => d.TRHR_Id == data.TRHR_Id);
                        result.UpdatedDate = DateTime.Now;
                        result.TRHG_Id = data.TRHG_Id;
                        result.TRMVT_Id = data.TRMVT_Id;
                        result.TRHR_RatePerKM = data.TRHR_RatePerKM;
                        _context.Update(result);
                        var update = _context.SaveChanges();
                        if (update > 0)
                        {
                            data.returnVal = "updated";
                        }
                        else
                        {
                            data.returnVal = "failedUpdate";
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }
        public MasterHirer_Group_RateDTO editRate(int id)
        {
            MasterHirer_Group_RateDTO obj = new MasterHirer_Group_RateDTO();
            try
            {
                obj.editDataList = _context.MasterHirerRateDMO.Where(d => d.TRHR_Id == id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }

        //Master Hirer.
        public MasterHirer_Group_RateDTO loadHirerData(MasterHirer_Group_RateDTO obj)
        {
            try
            {
                var grouplist = _context.MasterHirerGroupDMO.Where(d => d.MI_Id == obj.MI_Id && d.TRHG_ActiveFlg == true).Select(d => new MasterHirer_Group_RateDTO { TRHG_Id = d.TRHG_Id, TRHG_HirerGroup = d.TRHG_HirerGroup }).ToList();
                if (grouplist.Count > 0)
                {
                    obj.hirerGroupList = grouplist.ToArray();
                }
                var query = (from m in _context.MasterHirerDMO
                             from n in _context.MasterHirerGroupDMO
                             where m.TRHG_Id == n.TRHG_Id && m.MI_Id == obj.MI_Id
                             select new MasterHirer_Group_RateDTO
                             {
                                TRMH_Id=m.TRMH_Id,
                                TRHG_Id=m.TRHG_Id,
                                TRMH_HirerName=m.TRMH_HirerName,
                                TRMH_ConatctPerName=m.TRMH_ConatctPerName,
                                TRMH_ContactPersonDesg=m.TRMH_ContactPersonDesg,
                                TRMH_ContactNo=m.TRMH_ContactNo,
                                TRMH_MobileNo=m.TRMH_MobileNo,
                                TRMH_EmailId=m.TRMH_EmailId,
                                TRMH_Address=m.TRMH_Address,
                                TRMH_ActiveFlg=m.TRMH_ActiveFlg,
                                TRHG_HirerGroup=n.TRHG_HirerGroup
                             }).ToList();
                if (query.Count > 0)
                {
                    obj.hirerList = query.ToArray();
                    obj.count = query.Count;
                }
                else
                {
                    obj.count = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public MasterHirer_Group_RateDTO saveHirer(MasterHirer_Group_RateDTO data)
        {
            try
            {
                if (data.TRMH_Id == 0)
                {
                    var duplicateCheck = _context.MasterHirerDMO.Where(d => d.MI_Id == data.MI_Id && d.TRHG_Id==data.TRHG_Id && d.TRMH_HirerName.Equals(data.TRMH_HirerName)).ToList();
                    if (duplicateCheck.Count > 0)
                    {
                        data.returnVal = "duplicate";
                    }
                    else
                    {
                        var mapp = new MasterHirerDMO();
                        mapp.CreatedDate = DateTime.Now;
                        mapp.UpdatedDate = DateTime.Now;
                        mapp.TRMH_ActiveFlg = true;
                        mapp.MI_Id = data.MI_Id;
                        mapp.TRHG_Id = data.TRHG_Id;
                        mapp.TRMH_HirerName = data.TRMH_HirerName;
                        mapp.TRMH_ConatctPerName = data.TRMH_ConatctPerName;
                        mapp.TRMH_ContactPersonDesg = data.TRMH_ContactPersonDesg;
                        mapp.TRMH_ContactNo = data.TRMH_ContactNo;
                        mapp.TRMH_MobileNo = data.TRMH_MobileNo;
                        mapp.TRMH_EmailId = data.TRMH_EmailId;
                        mapp.TRMH_Address = data.TRMH_Address;
                        _context.Add(mapp);
                        var flag = _context.SaveChanges();
                        if (flag > 0)
                        {
                            data.returnVal = "saved";
                        }
                        else
                        {
                            data.returnVal = "savingFailed";
                        }

                    }
                }
                else
                {
                    var duplicateCheck = _context.MasterHirerDMO.Where(d => d.MI_Id == data.MI_Id && d.TRHG_Id == data.TRHG_Id && d.TRMH_HirerName.Equals(data.TRMH_HirerName) && d.TRMH_Id != data.TRMH_Id).ToList();
                    if (duplicateCheck.Count > 0)
                    {
                        data.returnVal = "duplicate";
                    }
                    else
                    {
                        var result = _context.MasterHirerDMO.Single(d => d.TRHG_Id == data.TRHG_Id);
                        result.UpdatedDate = DateTime.Now;
                        result.TRHG_Id = data.TRHG_Id;
                        result.TRMH_HirerName = data.TRMH_HirerName;
                        result.TRMH_ConatctPerName = data.TRMH_ConatctPerName;
                        result.TRMH_ContactPersonDesg = data.TRMH_ContactPersonDesg;
                        result.TRMH_ContactNo = data.TRMH_ContactNo;
                        result.TRMH_MobileNo = data.TRMH_MobileNo;
                        result.TRMH_EmailId = data.TRMH_EmailId;
                        result.TRMH_Address = data.TRMH_Address;
                        _context.Update(result);
                        var update = _context.SaveChanges();
                        if (update > 0)
                        {
                            data.returnVal = "updated";
                        }
                        else
                        {
                            data.returnVal = "failedUpdate";
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }
        public MasterHirer_Group_RateDTO EditHirer(int id)
        {
            MasterHirer_Group_RateDTO obj = new MasterHirer_Group_RateDTO();
            try
            {
                obj.editDataList = _context.MasterHirerDMO.Where(d => d.TRMH_Id == id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public MasterHirer_Group_RateDTO deactivateHirer(MasterHirer_Group_RateDTO dto)
        {
            MasterHirer_Group_RateDTO dt = new MasterHirer_Group_RateDTO();
            try
            {
                var query = _context.MasterHirerDMO.Single(d => d.TRMH_Id == dto.TRMH_Id);
                query.TRMH_ActiveFlg = dto.TRMH_ActiveFlg;
                _context.Update(query);
                var deactive = _context.SaveChanges();
                if (deactive > 0)
                {
                    if (dto.TRMH_ActiveFlg == true)
                    {
                        dt.returnVal = "Record Activated Successfully";
                    }
                    else if (dto.TRMH_ActiveFlg == false)
                    {
                        dt.returnVal = "Record Deactivated Successfully";
                    }

                }
                else
                {
                    if (dto.TRMH_ActiveFlg == true)
                    {
                        dt.returnVal = "Failed to activate record";
                    }
                    else if (dto.TRMH_ActiveFlg == false)
                    {
                        dt.returnVal = "Failed to deactivate record";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (dto.TRMH_ActiveFlg == true)
                {
                    dt.returnVal = "Failed to activate record";
                }
                else if (dto.TRMH_ActiveFlg == false)
                {
                    dt.returnVal = "Failed to deactivate record";
                }
            }
            return dt;
        }
    }
}
