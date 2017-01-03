using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.WindowsAzure.Storage.Table;

namespace ContosoThingsCore
{
    public class HubWrapper : TableEntity
    {
        public HubWrapper()
        {
            // for serialization
        }

        public HubWrapper(Hub w, string partition = "Test")
        {
            this.PartitionKey = partition;
            this.RowKey = w.Id;
            this.Data = w.Save();
        }

        public string Data { get; set; }

        public Hub GetHub()
        {
            Hub w = Newtonsoft.Json.JsonConvert.DeserializeObject<Hub>(Data, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            return w;
        }
    }

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

        public ThingsBase Control(string deviceId, string serviceName, object value)
        {
            ThingsBase deviceToControl = FindDevice(deviceId);

            if (deviceToControl == null)
            {
                throw new Exception("No device found");
            }

            SetProperty(deviceToControl, serviceName, value);

            return deviceToControl;
        }

        private void SetProperty(ThingsBase deviceToControl, string serviceName, object value)
        {
            PropertyInfo prop = deviceToControl.GetType().GetProperty(serviceName, BindingFlags.Public | BindingFlags.Instance);
            if (null != prop && prop.CanWrite)
            {
                prop.SetValue(deviceToControl, value, null);
            }
        }

        private object GetProperty(ThingsBase deviceToControl, string serviceName)
        {
            PropertyInfo prop = deviceToControl.GetType().GetProperty(serviceName, BindingFlags.Public | BindingFlags.Instance);
            return prop.GetValue(deviceToControl);
        }

        public object GetServiceValue(string deviceId, string serviceName)
        {
            ThingsBase deviceToControl = FindDevice(deviceId);

            if (deviceToControl == null)
            {
                throw new Exception("No device found");
            }

            return GetProperty(deviceToControl, serviceName);
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
