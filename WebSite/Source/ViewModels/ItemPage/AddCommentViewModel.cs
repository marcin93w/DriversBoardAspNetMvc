using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Driver.WebSite.ViewModels.ItemPage
{
    public class AddCommentViewModel
    {
        [Required(ErrorMessage = "Pole wymagane")]
        [HiddenInput]
        public int ItemId { set; get; }
        [Required(ErrorMessage = "Pole wymagane")]
        [Display(Name = "Treść komentarza")]
        public string Text { get; set; }
    }
}
