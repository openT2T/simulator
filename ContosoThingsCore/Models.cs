using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoThingsCore
{
    public enum ThingsType
    {
        Switch,
        Light,
        LightDimmable,
        LightColor,
        Thermostat
    }

    public abstract class ThingsBase
    {
        public ThingsBase()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public ThingsType ThingsType { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Id     : ").AppendLine(Id);
            sb.Append("Name   : ").AppendLine(Name);
            sb.Append("Type   : ").AppendLine(ThingsType.ToString());
            return sb.ToString();
        }
    }

    public class ContosoSwitch : ThingsBase
    {
        public ContosoSwitch() { }
        public bool Switch { get; set; }
        public ContosoSwitch(string name) 
        {
            ThingsType = ThingsType.Switch;
            this.Name = name;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.Append("Switch : ").AppendLine(Switch.ToString());
            return sb.ToString();
        }

    }

    public class ContosoLight : ContosoSwitch
    {
        public ContosoLight() : base() { }
        
        public ContosoLight(string name) : base(name)
        {
            ThingsType = ThingsType.Light;
        }
        
    }
    public class ContosoLightDimmable : ContosoLight
    {
        public ContosoLightDimmable() : base() { }

        public Int64 Dim { get; set; }

        public ContosoLightDimmable(string name) : base(name)
        {
            ThingsType = ThingsType.LightDimmable;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.Append("Dim    : ").AppendLine(Dim.ToString());
            return sb.ToString();
        }
    }

    public class ContosoLightColor : ContosoLightDimmable
    {
        public ContosoLightColor() : base() { }
        public string ColorRGB { get; set; }
        public ContosoLightColor(string name) : base(name)
        {
            ThingsType = ThingsType.LightColor;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.Append("Color : ").AppendLine(ColorRGB);
            return sb.ToString();
        }
    }

    public class ContosoThermostat : ThingsBase
    {
        public ContosoThermostat(string name)
        {
            ThingsType = ThingsType.Thermostat;
            this.Name = name;

        }

        public int CurrentTemperature { get; set; }
        public int TargetTemperature { get; set; }
        public int TargetTemperatureLow { get; set; }
        public int TargetTemperatureHigh { get; set; }
        public string Mode { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.Append("Current: ").AppendLine(CurrentTemperature.ToString());
            sb.Append("Target : ").AppendLine(TargetTemperature.ToString());
            sb.Append("TargetL: ").AppendLine(TargetTemperatureLow.ToString());
            sb.Append("TargetH: ").AppendLine(TargetTemperatureHigh.ToString());
            sb.Append("Mode   : ").Append(Mode);
            return sb.ToString();
        }
    }
}
