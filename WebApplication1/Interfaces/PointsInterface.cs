using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
   public interface PointsInterface
    {
        PointsDTO getdetails(PointsDTO data);
        PointsDTO Getreportdetails(PointsDTO data);
        Task<PointsDTO> savedata(PointsDTO stu);
    }
}
