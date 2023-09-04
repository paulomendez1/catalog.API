using AutoMapper;
using Catalog.Domain.DTOs.Request.Item;
using Catalog.Domain.DTOs.Response;
using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Mapper
{
    public class CatalogProfile : Profile
    {
        public CatalogProfile()
        {
            CreateMap<ItemResponse, Item>().ReverseMap();
            CreateMap<GenreResponse, Genre>().ReverseMap();
            CreateMap<ArtistResponse, Artist>().ReverseMap();
            CreateMap<Price, PriceResponse>().ReverseMap();
            CreateMap<AddItemRequest, Item>().ReverseMap();
            CreateMap<EditItemRequest, Item>().ReverseMap();
        }
    }
}
