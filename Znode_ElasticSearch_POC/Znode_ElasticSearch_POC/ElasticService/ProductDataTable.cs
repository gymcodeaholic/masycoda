using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Znode_ElasticSearch_POC
{
    public class ProductDataTable
    {
        public static DataTable GetAllDocument()
        {
            DataTable dataTable = new DataTable("product");
            //dataTable.Columns.Add("ID", typeof(string));
            //dataTable.Columns.Add("id", typeof(string));
            //dataTable.Columns.Add("productname", typeof(string));
            //dataTable.Columns.Add("price", typeof(string));

            //var res = ElasticSearchConnection.ElasticSearchClient().Search<ProductModel>(s => s
            //.Index("product")
            //.Type("doc")
            //.From(0)
            //.Size(1000)
            //.Query(q => q.MatchAll()));

            //foreach (var hit in res.Hits)
            //{
            //    dataTable.Rows.Add(hit.Id.ToString(), hit.Source.id.ToString(), hit.Source.productname.ToString(), hit.Source.price.ToString());
            //}

            return dataTable;
        }
    }
}