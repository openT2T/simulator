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
    /// <summary>
    /// Wrapper object for hubs which gets stored in the table store
    /// </summary>
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

    /// <summary>
    /// Object which has devices, add/remove/control from this main object.
    /// </summary>
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

        /// <summary>
        /// Main method which allows you to control devices.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ThingsBase Control(string deviceId, string propertyName, object value)
        {
            ThingsBase deviceToControl = FindDevice(deviceId);

            if (deviceToControl == null)
            {
                throw new Exception("No device found");
            }

            SetProperty(deviceToControl, propertyName, value);

            return deviceToControl;
        }

        /// <summary>
        /// Helper method which uses reflection to change the property name
        /// </summary>
        /// <param name="deviceToControl"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        private void SetProperty(ThingsBase deviceToControl, string propertyName, object value)
        {
            PropertyInfo prop = deviceToControl.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (null != prop && prop.CanWrite)
            {
                prop.SetValue(deviceToControl, value, null);
            }
        }

        private object GetProperty(ThingsBase deviceToControl, string propertyName)
        {
            PropertyInfo prop = deviceToControl.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            return prop.GetValue(deviceToControl);
        }

        public object GetDevice(string deviceId)
        {
            ThingsBase deviceToControl = FindDevice(deviceId);

            if (deviceToControl == null)
            {
                throw new Exception("No device found");
            }

            return deviceToControl;
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

        /// <summary>
        /// Returns a JSON representation of the hub and all it's devices.
        /// </summary>
        /// <returns></returns>
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

        public void RemoveThing(string thingId)
        {
            bool done = false;
            for (int i = 0; !done && i < this.Things.Count; i++)
            {
                if (this.Things[i].Id.Equals(thingId, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.Things.RemoveAt(i);
                    done = true;
                }
            }
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
