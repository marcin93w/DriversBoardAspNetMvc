using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Driver.WebSite.ViewModels
{
    public class AddDriverOccurrenceViewModel
    {
        [HiddenInput]
        [Display(Name = "")]
        public int? DriverId { set; get; } //if driver already exsist in system
        [Display(Name = "Tablica rejestracyjna")]
        public string Plate { set; get; }
        [Display(Name = "Opis samochodu")]
        public string Description { set; get; }
        
        ///TODO:
        //public int StartSecond { set; get; }
        //public int EndSecond { set; get; }
    }
}