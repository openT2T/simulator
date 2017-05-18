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
        string lockObject = "lock";
        CloudStorageAccount storageAccount = null;
        CloudTableClient tableClient = null;
        CloudTable table = null;


        public TableStorageProvider(string TableStorageConnectionString)
        {
            // Retrieve the storage account from the connection string.
            storageAccount = CloudStorageAccount.Parse(TableStorageConnectionString);

            // Create the table client.
            tableClient = storageAccount.CreateCloudTableClient();
            
            // Create the CloudTable object that represents the "hubs" table.
            table = tableClient.GetTableReference("hubs");
            if (!table.Exists())
            {
                table.Create();
            }
        }
        public void AddHub(Hub hub, string partition = "Test")
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

        public List<Hub> GetAllHubs(string partition = "Test")
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

        public Hub GetHub(string rowKey, string partition = "Test")
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
        public void DeleteHub(Hub hub)
        {
            lock (lockObject)
            {
                HubWrapper hubWrapper = new HubWrapper(hub);
                DeleteHub(hubWrapper.RowKey);
            }
        }

        public void DeleteHub(string rowKey, string partition = "Test")
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