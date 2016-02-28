using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Driver.WebSite.Models;
using Driver.WebSite.ViewModels;

namespace Driver.WebSite
{
    public class AutoMapperConfig
    {
        public static void CreateMappings()
        {
            Mapper.Initialize(InitializeMapper);
        }

        private static void InitializeMapper(IConfiguration cfg)
        {
            cfg.CreateMap<Item, ItemPanelViewModel>();
            cfg.CreateMap<Item, ItemPageViewModel>();
            cfg.CreateMap<AddItemViewModel, Item>();
            cfg.CreateMap<Comment, CommentViewModel>();

            cfg.CreateMap<Vote<Item>, ItemVote>();
            cfg.CreateMap<Vote<Comment>, CommentVote>();
            cfg.CreateMap<Vote<DriverOccurrence>, DriverOccurrenceVote>();
        }
    }
}
