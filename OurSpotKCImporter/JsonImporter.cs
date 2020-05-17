using Newtonsoft.Json;
using OurSpotKC.Data;
using OurSpotKC.Services;
using OurSpotKCImporter.Classes;
using OurSpotKCImporter.WordPressImporter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OurSpotKCImporter.JsonImporter
{
    public class JsonImporter
    {
        public static void GetWordpressResourcesSqlImportScript()
        {
            try
            {
                string fileName = Path.Combine(Directory.GetCurrentDirectory(), "ImportFiles", "json", "new", "resources.json");
                List<ResourceDataModel> resources = JsonConvert.DeserializeObject<List<ResourceDataModel>>(File.ReadAllText(fileName));
                List<CategoryDataModel> categories = CategoryDataService.GetCategoryData();


                StringBuilder sb = new StringBuilder("INSERT INTO Resources (name, Description, ImageUrl, Address,  CategoryId) VALUES ");

                foreach (var resource in resources)
                {
                    sb.Append("(");
                    sb.Append($"'{resource.Title.Rendered.Replace("'", "''")}',");
                    sb.Append($"'{resource.Content.Rendered.Replace("'", "''")}',");
                    sb.Append($"'{resource.Acf.ResourceIconImage}',");
                    sb.Append($"'{resource.Acf.Address.Replace("'", "''")}',");
                    string categoryName = resource.Acf.Category.Trim();

                    CategoryDataModel category = categories.FirstOrDefault(x => x.Name == categoryName);
                    
                    if (category != null)
                    {
                        sb.Append($"{category.Id})");
                    } else
                    {
                        int categoryId = categories.First(x => x.Name == "").Id;
                        sb.Append($"{categoryId})");
                    }
                    
                    if (resource != resources.Last())
                    {
                        sb.Append(",");
                    }
                }

                string fileDir = Path.Combine(Directory.GetCurrentDirectory(), "ImportFiles", "json", "output");

                if (!Directory.Exists(fileDir))
                {
                    Directory.CreateDirectory(fileDir);
                }

                
                fileName = $"{DateTime.Now:yyyy-MM-dd}_ResourceSql.txt";

                string filePath = Path.Combine(fileDir, fileName);

                using (FileStream catFile = File.Create(filePath))
                {
                    byte[] sbBytes = Encoding.ASCII.GetBytes(sb.ToString());
                    catFile.Write(sbBytes);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void GetResourceCategories()
        {
            try
            {
                string fileName = Path.Combine(Directory.GetCurrentDirectory(), "ImportFiles", "json", "new", "resources.json");
                List<ResourceDataModel> resources = JsonConvert.DeserializeObject<List<ResourceDataModel>>(File.ReadAllText(fileName));

                List<Category> categories = resources.Select(x => x.Acf.Category)
                                                     .Distinct()
                                                     .Select((x, idx) => new Category() { Id = idx, Name = x })
                                                     .ToList();

                StringBuilder sb = new StringBuilder("INSERT INTO Categories (Id, Name) VALUES ");

                foreach (Category cat in categories)
                {
                    sb.AppendLine($"({cat.Id},'{cat.Name}')");
                    if (cat != categories.Last())
                    {
                        sb.Append(",");
                    }
                }

                string fileDir = Path.Combine(Directory.GetCurrentDirectory(), "ImportFiles", "json", "output");

                if (!Directory.Exists(fileDir))
                {
                    Directory.CreateDirectory(fileDir);
                }

                fileName = $"{DateTime.Now:yyyy-MM-dd}_ResourceCategorySql.txt";

                string filePath = Path.Combine(fileDir, fileName);
                
                using (FileStream catFile = File.Create(filePath))
                {
                    byte[] sbBytes = Encoding.ASCII.GetBytes(sb.ToString());
                    catFile.Write(sbBytes);
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            
        }
    }
}
