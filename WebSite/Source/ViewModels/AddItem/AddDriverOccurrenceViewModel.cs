using System.ComponentModel.DataAnnotations;

namespace Driver.WebSite.ViewModels.AddItem
{
    public class AddDriverOccurrenceViewModel
    {
        [Display(Name = "Tablica rejestracyjna")]
        [MaxLength(9)]
        public string Plate { set; get; }
        [Display(Name = "Opis samochodu")]
        public string Description { set; get; }
        
        ///TODO:
        //public int StartSecond { set; get; }
        //public int EndSecond { set; get; }
    }
}