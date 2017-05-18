using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContosoThingsCore;
using System.IO;
using ContosoThingsCore.Providers;

namespace ContosoThingsConsole
{
    class Program
    {
        // !Must be configured! - TableStorageConnectionString is the Connection String for an Azure storage account where the hubs and devices are stored 
       static string TableStorageConnectionString = "";

        public static void CreateHub1(TableStorageProvider storageProvider)
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

            h.Control(cl1.Id, "Switch", true);
            h.Control(bl1.Id, "Switch", true);
            h.Control(bl1.Id, "Dim", 18);

            string saveString = h.Save();
            Console.WriteLine(saveString);
            Console.WriteLine(h.ToString());
            Console.WriteLine("------------------");
            //Console.WriteLine(h.ToString());

            File.WriteAllText("hub.json", saveString);
            storageProvider.AddHub(h);
 
            Hub h2 = Hub.Load(saveString);

            Console.WriteLine(h2);
        }


        public static void CreateHub2(TableStorageProvider storageProvider)
        {
            Hub h = new Hub("Contoso Big Hub");

            h.AddThing(new ContosoSwitch("Humidifier"));
            h.AddThing(new ContosoSwitch("Christmas Lights"));

            h.AddThing(new ContosoLight("Outside Lights"));
            h.AddThing(new ContosoLight("Entryway Lights"));

            h.AddThing(new ContosoLightDimmable("Family Room Lamp"));
            h.AddThing(new ContosoLightDimmable("Family Room Can Lights"));
            h.AddThing(new ContosoLightDimmable("Family Room Chandelier"));
            h.AddThing(new ContosoLightDimmable("Bedroom Lamp"));
            h.AddThing(new ContosoLightDimmable("Bedroom Can Lights"));
            h.AddThing(new ContosoLightDimmable("Guest Bedroom Lamp"));
            h.AddThing(new ContosoLightDimmable("Guest Bedroom Chandelier"));

            storageProvider.AddHub(h);

            Console.WriteLine(h);
        }

        static void Main(string[] args)
        {
            //TableStorageProvider storageProvider = new TableStorageProvider(TableStorageConnectionString);
            //CreateHub1(storageProvider);
            //CreateHub2(storageProvider);

            //List<Hub> hubs = storageProvider.GetAllHubs();
            
            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}
