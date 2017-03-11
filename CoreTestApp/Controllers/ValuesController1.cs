using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

// Namespace for Blob storage types

namespace CoreTestApp.Controllers
{
    public class ValuesController : ApiController
    {
        private CloudStorageAccount _cloudStorageAccount;

        public ValuesController()
        {
            _cloudStorageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            var client = _cloudStorageAccount.CreateCloudTableClient();
            var tref = client.GetTableReference("testitaulu");
            tref.CreateIfNotExists();

            var partitionKey = "pk";
            var rowKey = Guid.NewGuid().ToString();
            var entity = new DynamicTableEntity(partitionKey, rowKey, "*", new Dictionary<string, EntityProperty>()
            {
                {"value", new EntityProperty(id) }
            });

            TableOperation insertOperation = TableOperation.Insert(entity);
            tref.Execute(insertOperation);

            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}