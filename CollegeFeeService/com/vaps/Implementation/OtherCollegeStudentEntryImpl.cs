using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vapstech.College.Fees;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class OtherCollegeStudentEntryImpl:Interfaces.OtherCollegeStudentEntryInterface
    {
        public CollFeeGroupContext _YearlyFeeGroupMappingContext;

        public OtherCollegeStudentEntryImpl(CollFeeGroupContext db)
        {
            _YearlyFeeGroupMappingContext = db;
        }

        //private static ConcurrentDictionary<string, Fee_Master_College_OtherStudentsDTO> _login 
        //    = new ConcurrentDictionary<string, Fee_Master_College_OtherStudentsDTO>();

        //public CollFeeGroupContext _YearlyFeeGroupMappingContext;
        //readonly Microsoft.Extensions.Logging.ILogger<OtherCollegeStudentEntryImpl> _logger;
        public Fee_Master_College_OtherStudentsDTO getdetails(int id)
        {
            Fee_Master_College_OtherStudentsDTO obj = new Fee_Master_College_OtherStudentsDTO();
            try
            {
                obj.otherstudentList = _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents.Where(d => d.MI_Id == id && d.FMCOST_ActiveFlag == true).ToArray();
                if (obj.otherstudentList.Length > 0)
                {
                    obj.count= obj.otherstudentList.Length;
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
        public Fee_Master_College_OtherStudentsDTO save(Fee_Master_College_OtherStudentsDTO data)
        {
            try
            {
                if (data.FMCOST_Id > 0)
                {
                    var checkduplicate = _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents.Where(d => d.MI_Id == data.MI_Id && d.FMCOST_ActiveFlag == true && d.FMCOST_StudentEmailId.Equals(data.FMCOST_StudentEmailId) && d.FMCOST_StudentMobileNo == data.FMCOST_StudentMobileNo && d.FMCOST_StudentName.Equals(data.FMCOST_StudentName) && d.FMCOST_Id != data.FMCOST_Id).ToList();
                    if (checkduplicate.Count == 0)
                    {
                        var result = _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents.Single(d => d.FMCOST_Id == data.FMCOST_Id);
                        result.FMCOST_StudentEmailId = data.FMCOST_StudentEmailId;
                        result.FMCOST_StudentMobileNo = data.FMCOST_StudentMobileNo;
                        result.FMCOST_StudentName = data.FMCOST_StudentName;
                        result.FMCOST_UpdatedDate = DateTime.Now;
                        result.FMCOST_UpdatedBy = data.User_Id;
                        _YearlyFeeGroupMappingContext.Update(result);
                        var flag = _YearlyFeeGroupMappingContext.SaveChanges();
                        if (flag > 0)
                        {
                            data.returnval = "updated";
                        }
                        else
                        {
                            data.returnval = "updatefailed";
                        }

                    }
                    else
                    {
                        data.returnval = "duplicate";
                    }

                }
                else
                {
                    var checkduplicate = _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents.Where(d => d.MI_Id == data.MI_Id && d.FMCOST_ActiveFlag == true && d.FMCOST_StudentEmailId.Equals(data.FMCOST_StudentEmailId) && d.FMCOST_StudentMobileNo == data.FMCOST_StudentMobileNo && d.FMCOST_StudentName.Equals(data.FMCOST_StudentName)).ToList();
                    if (checkduplicate.Count == 0)
                    {
                        Fee_Master_College_OtherStudents dmo = Mapper.Map<Fee_Master_College_OtherStudents>(data);
                        dmo.FMCOST_CreatedDate = DateTime.Now;
                        dmo.FMCOST_UpdatedDate = DateTime.Now;
                        dmo.FMCOST_CreatedBy = data.User_Id;
                        dmo.FMCOST_UpdatedBy = data.User_Id;
                        dmo.FMCOST_StudentName = data.FMCOST_StudentName;
                        dmo.FMCOST_StudentMobileNo = data.FMCOST_StudentMobileNo;
                        dmo.FMCOST_StudentEmailId = data.FMCOST_StudentEmailId;
                  
                        dmo.FMCOST_ActiveFlag = true;
                        _YearlyFeeGroupMappingContext.Add(dmo);
                        var flag = _YearlyFeeGroupMappingContext.SaveChanges();
                        if (flag > 0)
                        {
                            data.returnval = "saved";
                        }
                        else
                        {
                            data.returnval = "savefailed";
                        }
                    }
                    else
                    {
                        data.returnval = "duplicate";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return data;
        }
        public Fee_Master_College_OtherStudentsDTO edit(int id)
        {
            Fee_Master_College_OtherStudentsDTO data = new Fee_Master_College_OtherStudentsDTO();
            try
            {
                data.otherstudentList = _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents.Where(d => d.FMCOST_Id == id).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Fee_Master_College_OtherStudentsDTO delete(int id)
        {
            Fee_Master_College_OtherStudentsDTO data = new Fee_Master_College_OtherStudentsDTO();
            try
            {
                var query = _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents.Where(d => d.FMCOST_Id == id).ToList();
                if (query.Any())
                {
                    _YearlyFeeGroupMappingContext.Remove(query.ElementAt(0));
                    var del = _YearlyFeeGroupMappingContext.SaveChanges();
                    if (del > 0)
                    {
                        data.returnval = "deleted";
                    }
                    else
                    {
                        data.returnval = "deletefailed";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                data.returnval = "failed";
            }
            return data;
        }
    }
}
