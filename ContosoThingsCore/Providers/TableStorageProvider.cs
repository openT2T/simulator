using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContosoThingsCore;

namespace ContosoThingsCore.Providers
{
    public class TableStorageProvider
    {
        static string lockObject = "lock";

        static string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=contosothings;AccountKey=J7+5GGTqfZIEG2pE+UGRZ1vgw9tBWPcWfsYC+GVH7hFebF62Vkb4U4a9L9M1S3y4Ecuuag68wUDKz/3MXodOWQ==";

        static CloudStorageAccount storageAccount = null;
        static CloudTableClient tableClient = null;
        static CloudTable table = null;

        static TableStorageProvider()
        {
            // Retrieve the storage account from the connection string.
            storageAccount = CloudStorageAccount.Parse(ConnectionString);

            // Create the table client.
            tableClient = storageAccount.CreateCloudTableClient();

            

            // Create the CloudTable object that represents the "people" table.
            table = tableClient.GetTableReference("Hubs");

            if (!table.Exists())
            {
                table.Create();
            }
        }

        public static void AddHub(Hub hub, string partition = "Test")
        {
            lock (lockObject)
            {
                HubWrapper hubWrapper = new HubWrapper(hub, partition);

                // Create the TableOperation object that inserts the customer entity.
                TableOperation insertOperation = TableOperation.InsertOrReplace(hubWrapper);

                // Execute the insert operation.
                table.Execute(insertOperation);
            }
        }

        public static List<Hub> GetAllHubs(string partition = "Test")
        {
            List<Hub> toReturn = new List<Hub>();

            // Construct the query operation for all customer entities where PartitionKey=partition.
            TableQuery<HubWrapper> query = new TableQuery<HubWrapper>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partition));

            // Print the fields for each customer.
            foreach (HubWrapper entity in table.ExecuteQuery(query))
            {
                toReturn.Add(entity.GetHub());
            }

            return toReturn;
        }

        public static Hub GetHub(string rowKey, string partition = "Test")
        {
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<HubWrapper>(partition, rowKey);

            // Execute the retrieve operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                HubWrapper hubWrapper = (HubWrapper)retrievedResult.Result;

                return hubWrapper.GetHub();
            }
            else
            {
                return null;
            }
        }
        public static void DeleteHub(string rowKey, string partition)
        {
            lock (lockObject)
            {
                // Create a retrieve operation that takes a customer entity.
                TableOperation retrieveOperation = TableOperation.Retrieve<HubWrapper>(partition, rowKey);

                // Execute the retrieve operation.
                TableResult retrievedResult = table.Execute(retrieveOperation);

                // Print the phone number of the result.
                if (retrievedResult.Result != null)
                {
                    HubWrapper hubWrapper = (HubWrapper)retrievedResult.Result;

                    // Create a retrieve operation that takes a customer entity.
                    TableOperation deleteOperation = TableOperation.Delete(hubWrapper);

                    table.Execute(deleteOperation);
                }
                else
                {
                    // nothing to delete
                }
            }
        }
    }
}