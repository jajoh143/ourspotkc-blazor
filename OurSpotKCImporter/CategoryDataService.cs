using Dapper;
using Microsoft.Data.Sqlite;
using OurSpotKCImporter.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OurSpotKCImporter
{
    public class CategoryDataService
    {
        public static List<CategoryDataModel> GetCategoryData()
        {
            using var con = new SqliteConnection("DataSource=C:\\Users\\jajoh\\source\\repos\\OurSpotKC\\OurSpotKC\\Database.db");
            con.Open();

            IEnumerable<CategoryDataModel> res = con.Query<CategoryDataModel>("SELECT Id, Name FROM Categories");
            con.Dispose();

            return res.OrderBy(x => x.Name).ToList();
        }
    }
}
