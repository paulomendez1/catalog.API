using Catalog.Domain.DTOs.Response;
using RiskFirst.Hateoas.Models;

namespace Catalog.API.ResponseModels
{
    public class ItemHateoasResponse : ILinkContainer
    {
        public ItemResponse Data;
        private Dictionary<string, Link> _links;

        public Dictionary<string, Link> Links { 
            get => _links ?? (_links = new Dictionary<string, Link>()); 
            set => _links = value; 
        }


        public void AddLink(string id, Link link)
        {
            Links.Add(id, link);
        }
    }
}
