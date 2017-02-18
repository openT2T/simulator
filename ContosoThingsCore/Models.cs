using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoThingsCore
{
    /// <summary>
    /// Currently supported types
    /// </summary>
    public enum ThingsType
    {
        Switch,
        Light,
        LightDimmable,
        LightColor,
        Thermostat
    }

    /// <summary>
    /// Base class for all things
    /// </summary>
    public abstract class ThingsBase
    {
        public ThingsBase()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public ThingsType ThingsType { get; set; }
        public String ThingsTypeString
        {
            get
            {
                return ThingsType.ToString("G");
            }
        }

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

        protected Int64 dim = 0;

        public Int64 Dim
        {
            get { return dim; }
            set
            {
                dim = value;
                if (dim == 0)
                {
                    this.Switch = false;
                }
                else
                {
                    this.Switch = true;
                }
            }
        }

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
        public enum ThermostatModes
        {
            Off,
            Heat,
            Cool,
            HeatCool
        }

        public ContosoThermostat(string name)
        {
            ThingsType = ThingsType.Thermostat;
            this.Name = name;

            CurrentTemperature = 65;
            TargetTemperature = 72;
            TargetTemperatureLow = 62;
            TargetTemperatureHigh = 75;
            Mode = (Int64)ThermostatModes.Heat;
        }

        /// <summary>
        /// Temperature is managed in F
        /// </summary>
        public Int64 CurrentTemperature { get; set; }
        public Int64 TargetTemperature { get; set; }
        public Int64 TargetTemperatureLow { get; set; }
        public Int64 TargetTemperatureHigh { get; set; }
        public Int64 Mode { get; set; }
        public string ModeText
        {
            get
            {
                ThermostatModes m = (ThermostatModes)Mode;
                return m.ToString();
            }
        }

        public Int64 Humidity { get; protected set; }
        public bool AwayMode { get; set; }
        public Int64 AwayTemperatureLow { get; set; }
        public Int64 AwayTemperatureHigh { get; set; }
        public bool FanActive { get; protected set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.Append("Current: ").AppendLine(CurrentTemperature.ToString());
            sb.Append("Target : ").AppendLine(TargetTemperature.ToString());
            sb.Append("TargetL: ").AppendLine(TargetTemperatureLow.ToString());
            sb.Append("TargetH: ").AppendLine(TargetTemperatureHigh.ToString());
            sb.Append("Mode   : ").Append((ThermostatModes)Mode);
            return sb.ToString();
        }
    }
}
