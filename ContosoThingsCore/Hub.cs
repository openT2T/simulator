using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ContosoThingsCore
{
    public class Hub
    {
        public static Hub Load(string toLoad)
        {
            Hub h = JsonConvert.DeserializeObject<Hub>(toLoad, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            return h;
        }

        public Hub(string name)
        {
            Id = Guid.NewGuid().ToString();
            this.Name = name;
            this.Things = new List<ContosoThingsCore.ThingsBase>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public List<ThingsBase> Things { get; set; }

        public void Control(string deviceId, string serviceName, object value)
        {
            ThingsBase deviceToControl = FindDevice(deviceId);

            if (deviceToControl == null)
            {
                throw new Exception("No device found");
            }

            SetProperty(deviceToControl, serviceName, value);

            //IThingService serviceToControl = FindService(deviceToControl, serviceName);

            //if (serviceToControl == null)
            //{
            //    throw new Exception("No service found on device");
            //}

            //serviceToControl.Value = value;

            //if (serviceToControl.DependentServiceNameValues.Count > 0)
            //{
            //    // update dependent services
            //}
        }

        private void SetProperty(ThingsBase deviceToControl, string serviceName, object value)
        {
            PropertyInfo prop = deviceToControl.GetType().GetProperty(serviceName, BindingFlags.Public | BindingFlags.Instance);
            if (null != prop && prop.CanWrite)
            {
                prop.SetValue(deviceToControl, value, null);
            }
        }

        public object GetServiceValue(string deviceId, string serviceName)
        {
            ThingsBase deviceToControl = FindDevice(deviceId);

            if (deviceToControl == null)
            {
                throw new Exception("No device found");
            }

            IThingService serviceToControl = FindService(deviceToControl, serviceName);

            if (serviceToControl == null)
            {
                throw new Exception("No service found on device");
            }

            return serviceToControl.Value;
        }

        private IThingService FindService(ThingsBase deviceToControl, string serviceName)
        {
            IThingService serviceToControl = null;

            foreach (IThingService service in deviceToControl.Services.Values)
            {
                if (service.Name.Equals(serviceName))
                {
                    serviceToControl = service;
                }
            }

            return serviceToControl;
        }

        private ThingsBase FindDevice(string deviceId)
        {
            ThingsBase deviceToControl = null;

            foreach (ThingsBase device in Things)
            {
                if (device.Id == deviceId)
                {
                    deviceToControl = device;
                    break;
                }
            }

            return deviceToControl;
        }

        public string Save()
        {
            return JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }

        public void AddThing(ThingsBase thing)
        {
            this.Things.Add(thing);
        }

        public void RemoveThing(ThingsBase thing)
        {
            this.Things.Remove(thing);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Id     : ").AppendLine(Id);
            sb.Append("Name   : ").AppendLine(Name);
            sb.Append("Things : ").AppendLine(Things.Count.ToString());
            foreach (ThingsBase t in Things)
            {
                sb.AppendLine("---------");
                sb.AppendLine(t.ToString());
            }
            sb.AppendLine("--------------------------------------------");
            return sb.ToString();
        }
    }
}
