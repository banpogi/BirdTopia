using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BirdTopia.Pages.Birds
{
    public class CreateModel : PageModel
    {
		[BindProperty]
		[Required(ErrorMessage ="The bird name is required")]
		[MaxLength(100, ErrorMessage= "Exceeded 100 characters")]
		public string BirdName { get; set; } = "";


		[BindProperty]
		[Required(ErrorMessage = "The scientific name is required")]
		[MaxLength(100, ErrorMessage = "Exceeded 100 characters")]
		public string ScientificName { get; set; } = "";


		[BindProperty]
		[Required(ErrorMessage = "The population is required")]
		public string Population { get; set; } = "";


		[BindProperty]
		[Required(ErrorMessage = "The image is required")]
		public IFormFile ImageFile { get; set; }


		[BindProperty]
		public string? Description { get; set; } = "";

		public string errorMessage = "";
		public string successMessage = "";



		public void OnGet()
        {
        }

		public void OnPost()
		{
			if (!ModelState.IsValid)
			{
				errorMessage = "Data validation failed";
				return;
			}

			//successful data validation

			if (Description == null) Description = "";

			//save the image file to the server

			//save the new bird to the database


			successMessage = "Data saved correctly";
		}
    }



}


