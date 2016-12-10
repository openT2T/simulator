using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoThingsCore
{
    public interface IThingService
    {
        String Name { get; set; }
        object Value { get; set; }
        Dictionary<string, dynamic> DependentServiceNameValues { get; set; }
    }

    public class ThingService : IThingService
    {
        public ThingService()
        {
            DependentServiceNameValues = new Dictionary<string, dynamic>();
        }

        public string Name { get; set; }

        public object Value { get; set; }

        public Dictionary<string, dynamic> DependentServiceNameValues { get; set; }


    }

    public class AllServices
    {
        public static string SwitchServiceName = "Switch";
        public static string DimServiceName = "Dim";
        public static string ColorRGBServiceName = "ColorRGB";

        public static IThingService SwitchService
        {
            get
            {
                ThingService s = new ThingService() { Name = "Switch", Value = false };
                return s;
            }
        }
        public static IThingService DimService
        {
            get
            {
                ThingService s = new ThingService() { Name = "Dim", Value = 0};
                s.DependentServiceNameValues.Add(AllServices.SwitchServiceName, null);
                return s;
            }
        }
        public static IThingService ColorRGBService
        {
            get
            {
                ThingService s = new ThingService() { Name = "ColorRGB", Value = "000000" };
                return s;
            }
        }
    }
}
