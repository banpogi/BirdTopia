using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace All_About_Birds.Birds
{
    public class IndexModel : PageModel
    {
        public List<BirdsModel> listBirds = new List<BirdsModel>();

        public string search = "";

        public void OnGet()
        {
            search = Request.Query["search"];
            if (search == null) search = "";

            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=birdtopia;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM birds";
                    if(search.Length > 0)
                    {
                        sql += " WHERE birdname LIKE @search";
                    }
                    sql += " ORDER BY id DESC";


					using (SqlCommand command = new SqlCommand(sql, connection)) 
                    {
                        command.Parameters.AddWithValue("@search", "%" + search + "%");
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



