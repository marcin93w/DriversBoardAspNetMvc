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
        private static readonly Regex YoutubeVideoRegex = 
            new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)");

        [Required]
        [Display(Name = "Tytuł")]
        public string Title { set; get; }
        public ItemContentType ContentType { private set; get; }
        [Required]
        [DataType(DataType.Url)]
        [Display(Name = "Link")]
        public string ContentUrl { set; get; }
        [Required]
        [Display(Name = "Krótki opis")]
        public string Comment { get; set; }

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
            var match = YoutubeVideoRegex.Match(ContentUrl);
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
