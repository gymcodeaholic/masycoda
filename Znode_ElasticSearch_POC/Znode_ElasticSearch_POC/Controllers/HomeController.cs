using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Znode_ElasticSearch_POC
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();

            //string a = "ankit";
            //string b = "raut";
            //a = $"{a}|{b}";
            //b = a.Split('|')[0];
            //a = a.Split('|')[1];

            //string c = "ankit raut";
            //c = $"{c.Split(' ')[1]} {c.Split(' ')[0]}";
        }

        public ActionResult CreateIndex()
        {
            //ElasticClient elasticClient = ElasticSearchConnection.ElasticSearchClient();
            ElasticSearchConnection elasticSearchConnection = new ElasticSearchConnection();
            elasticSearchConnection.CreateIndex();
            return RedirectToAction("Index");
        }
        public ActionResult InserDataInFile()
        {
            ElasticSearchConnection.InserDataInFile();
            return RedirectToAction("Index");
        }
    }
}