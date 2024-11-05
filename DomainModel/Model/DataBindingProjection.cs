using Newtonsoft.Json.Converters;

namespace DomainModel.Model
{
    public class DataBindingProjection
    {
       public  string PASEDate { get; set; }
    }


    class MonthDayYearDateConverter : IsoDateTimeConverter
    {
        public MonthDayYearDateConverter()
        {
            DateTimeFormat = "MM/dd/yyyy";
        }
    }
}
