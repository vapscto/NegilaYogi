using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
    public interface logininterface
    {
        string checkusrnmepwd(regis id);
        string getregdata(string username);
        //IEnumerable<regis> GetAll();
        //regis Find(regis id);
    }
}
