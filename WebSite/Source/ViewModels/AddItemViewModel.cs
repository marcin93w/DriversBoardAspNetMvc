using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Driver.WebSite.Models;

namespace Driver.WebSite.ViewModels
{
    public class AddItemViewModel : IValidatableObject
    {
        private readonly Regex _youtubeVideoRegex = 
            new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)");

        private const int MaxNumberOfDrivers = 50;

        [Required(ErrorMessage = "Pole wymagane")]
        [Display(Name = "Tytuł")]
        public string Title { set; get; }
        public ItemContentType ContentType { private set; get; }
        [Required(ErrorMessage = "Pole wymagane")]
        [DataType(DataType.Url)]
        [Display(Name = "Link")]
        public string ContentUrl { set; get; }
        [Required(ErrorMessage = "Pole wymagane")]
        [Display(Name = "Krótki opis")]
        public string Description { get; set; }

        [Display(Name = "Kerowcy")]
        public IList<AddDriverOccurrenceViewModel> Drivers { set; get; }

        public AddItemViewModel()
        {
            Drivers = new List<AddDriverOccurrenceViewModel>(MaxNumberOfDrivers);
            for(int i=0;i<MaxNumberOfDrivers;i++)
                ((List<AddDriverOccurrenceViewModel>) Drivers).Add(new AddDriverOccurrenceViewModel());
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ProcessContentUrl();
            if (ContentType == ItemContentType.NotSupported)
            {
                return new[] {new ValidationResult("Obsługiwane są tylko materiały z serwisu YouTube")};
            }

            return new[] { ValidationResult.Success };
        }

        public void ProcessContentUrl()
        {
            var match = _youtubeVideoRegex.Match(ContentUrl);
            if (match.Success)
            {
                ContentUrl = match.Groups[1].Value;
                ContentType = ItemContentType.YoutubeVideo;
                return;
            }

            ContentType = ItemContentType.NotSupported;
        }
    }
}
