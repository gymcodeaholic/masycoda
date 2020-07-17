using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization; 

namespace Znode_ElasticSearch_POC
{
    public class ElasticSearchConnection
    {
        private readonly ConnectionSettings connectionSettings;
        private readonly ElasticClient elasticClient;

        public ElasticSearchConnection()
        {
            connectionSettings = new ConnectionSettings(new Uri("http://localhost:9200/"));
            elasticClient = new ElasticClient(connectionSettings);
        }

        public bool CreateIndex()
        {
            List<ProductModel> productData;
            var seenPages = 0;
            IndexName indexName = "productdatanewdynamic";
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            ConcurrentBag<BulkAllResponse> bulkAllResponses = new ConcurrentBag<BulkAllResponse>();
            ConcurrentBag<dynamic> deadLetterQueue = new ConcurrentBag<dynamic>();
            //List<dynamic> dynamicProductData;

            // deserialize JSON directly from a file
            using (StreamReader files = File.OpenText(@"C:\Users\akshay.badhiye\source\repos\Znode_ElasticSearch_POC\Znode_ElasticSearch_POC\Data\Data.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                //productData = (List<dynamic>)serializer.Deserialize(files, typeof(List<dynamic>));
                productData = (List<ProductModel>)serializer.Deserialize(files, typeof(List<ProductModel>));
            }
            var a = Environment.ProcessorCount;

            var observableBulk = elasticClient.BulkAll(productData, f => f
                                .MaxDegreeOfParallelism(Environment.ProcessorCount)
                                .DroppedDocumentCallback((item, product) => deadLetterQueue.Add(product))
                                .BackOffTime(TimeSpan.FromMilliseconds(10))
                                .Size(500)
                                .RefreshOnCompleted()
                                .Index(indexName), tokenSource.Token);
            try
            {
                observableBulk.Wait(TimeSpan.FromSeconds(30), b =>
                {
                    bulkAllResponses.Add(b);
                    Interlocked.Increment(ref seenPages);
                });
            }
            catch (Exception e)
            {
                Debug.Write(e);
            }

            return true;
        }

        public static void InserDataInFile()
        {
            List<ProductModel> _data = new List<ProductModel>();

            for (int i = 0; i < 99999; i++)
            {
                _data.Add(new ProductModel()
                {
                    id = i,
                    productname = $"Apple{i}",
                    price = 99
                });
            }
            string json = JsonConvert.SerializeObject(_data.ToArray());
            File.WriteAllText(@"C:\Users\akshay.badhiye\source\repos\Znode_ElasticSearch_POC\Znode_ElasticSearch_POC\Data\Data.json", json);
        }
    }
}