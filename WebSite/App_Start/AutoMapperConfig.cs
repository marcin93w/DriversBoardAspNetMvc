using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Driver.Common.Models;
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
        }
    }
}
