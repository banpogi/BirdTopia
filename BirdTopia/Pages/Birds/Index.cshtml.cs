using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace All_About_Birds.Birds
{
    public class IndexModel : PageModel
    {
        public List<BirdsModel> listBirds = new List<BirdsModel>();

        public string search = "";


        //pagination global variables
        public int page = 1; // the current html page
        public int totalPages = 0;
        private readonly int pageSize = 2;

        public void OnGet()
        {
            search = Request.Query["search"];
            if (search == null) search = "";

            page = 1;
            string requestPage = Request.Query["page"];
            if (requestPage != null)
            {
                try
                {
                    page = int.Parse(requestPage);
                    
                }
                catch (Exception ex) 
                {
                    page = 1;
                }
                
            }


            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=birdtopia;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //finding the total number of books
                    string sqlCount = "SELECT COUNT(*) FROM birds";
                    if(search.Length > 0)
                    {
                        sqlCount += " WHERE birdname LIKE @search"; 
                    }

                    using(SqlCommand command = new SqlCommand(sqlCount, connection))
                    {
                        command.Parameters.AddWithValue("@search", "%" + search + "%");

                        decimal count = (int)command.ExecuteScalar();
                        totalPages = (int)Math.Ceiling(count / pageSize);
                    }

                    string sql = "SELECT * FROM birds";
                    if(search.Length > 0)
                    {
                        sql += " WHERE birdname LIKE @search";
                    }
                    sql += " ORDER BY id DESC";
                    sql += " OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY";


					using (SqlCommand command = new SqlCommand(sql, connection)) 
                    {
                        command.Parameters.AddWithValue("@search", "%" + search + "%");
                        command.Parameters.AddWithValue("@skip", (page-1) * pageSize);
                        command.Parameters.AddWithValue("@pageSize", pageSize);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BirdsModel birdinfo = new BirdsModel();
                                birdinfo.Id = reader.GetInt32(0);
                                birdinfo.BirdName = reader.GetString(1);
                                birdinfo.ScientificName = reader.GetString(2);
                                birdinfo.Population =  reader.GetString(3);
                                birdinfo.ImageFileName = reader.GetString(4);
                                birdinfo.Description = reader.GetString(5);
                                birdinfo.Created_At = reader.GetDateTime(6).ToString("MM/dd/yyyy");

                                listBirds.Add(birdinfo);

                            }
                        }
                    }


                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
    }


    public class BirdsModel
    {
        public int Id { get; set; }
        public string BirdName { get; set; } = "";

		public string ScientificName { get; set; } = "";

        public string Population { get; set; } = "";

        public string ImageFileName { get; set; } = "";
        public string Description { get; set; } = "";

        public string Created_At { get; set; } = "";



	}


}



