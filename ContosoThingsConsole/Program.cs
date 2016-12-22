using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContosoThingsCore;
using System.IO;

namespace ContosoThingsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ContosoSwitch cs1 = new ContosoSwitch("Humidifier");
            ContosoLight cl1 = new ContosoLight("Family Lamp 1");
            ContosoLight cl2 = new ContosoLight("Family Lamp 2");
            ContosoLightDimmable bl1 = new ContosoLightDimmable("Bedroom Lamp 1");

            Hub h = new Hub("Contoso Test Hub");
            h.AddThing(cs1);
            h.AddThing(cl1);
            h.AddThing(cl2);
            h.AddThing(bl1);

            h.Control(cl1.Id, AllServices.SwitchServiceName, true);
            h.Control(bl1.Id, AllServices.SwitchServiceName, true);
            h.Control(bl1.Id, AllServices.DimServiceName, 18);

            string saveString = h.Save();
            Console.WriteLine(saveString);
            Console.WriteLine(h.ToString());
            Console.WriteLine("------------------");
            //Console.WriteLine(h.ToString());

            File.WriteAllText("home.json", saveString);

            Hub h2 = Hub.Load(saveString);

            Console.WriteLine(h2);


            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}
