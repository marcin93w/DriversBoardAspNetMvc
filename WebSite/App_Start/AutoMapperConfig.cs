﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Driver.WebSite.AutoMapperConverters;
using Driver.WebSite.Models;
using Driver.WebSite.ViewModels;
using Driver.WebSite.ViewModels.AddItem;
using Driver.WebSite.ViewModels.ItemPage;
using Driver.WebSite.ViewModels.ItemPanel;
using Driver.WebSite.ViewModels.Sidebar;

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
            cfg.CreateMap<Item, ItemPanelViewModel>()
                .ForMember(vm => vm.UserVoting, opt => opt.ResolveUsing<ItemUserVotingResolver>());
            cfg.CreateMap<Item, ItemPageViewModel>();
            cfg.CreateMap<AddItemViewModel, Item>();
            cfg.CreateMap<Comment, CommentViewModel>()
                .ForMember(vm => vm.UserVote, opt => opt.ResolveUsing<CommentUserVotingResolver>());
            cfg.CreateMap<DriverOccurrence, DriverOccurrenceViewModel>()
                .ForMember(vm => vm.Plate, opt => opt.ResolveUsing<DriverOccurrencePlateResolver>())
                .ForMember(vm => vm.UserVote, opt => opt.ResolveUsing<DriverOccurrenceUserVotingResolver>());
            cfg.CreateMap<Models.Driver, DriverRankingViewModel>()
                .ForMember(vm => vm.Plate, opt => opt.ResolveUsing<PlateResolver>());

            cfg.CreateMap<Vote<Item>, ItemVote>();
            cfg.CreateMap<Vote<Comment>, CommentVote>();
            cfg.CreateMap<Vote<DriverOccurrence>, DriverOccurrenceVote>();
        }
    }
}
