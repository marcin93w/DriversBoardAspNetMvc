using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Driver.WebSite.Models;

namespace Driver.WebSite.ViewModels
{
    public class AddCommentViewModel
    {
        [Required]
        [HiddenInput]
        public int ItemId { set; get; }
        [Required]
        [Display(Name = "Treść komentarza")]
        public string Text { get; set; }
    }
}
