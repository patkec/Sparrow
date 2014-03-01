using AutoMapper;
using Sparrow.Domain.Models;
using Sparrow.Api.Models.Customers;
using Sparrow.Api.Models.Drafts;
using Sparrow.Api.Models.Offers;
using Sparrow.Api.Models.Products;
using Sparrow.Api.Models.Users;

namespace Sparrow.Api
{
    public class MapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<UserMappingProfile>();
                cfg.AddProfile<ProductMappingProfile>();
                cfg.AddProfile<CustomerMappingProfile>();
                cfg.AddProfile<OfferDraftMappingProfile>();
                cfg.AddProfile<OfferMappingProfile>();
            });
            Mapper.AssertConfigurationIsValid();
        }

        private class UserMappingProfile: Profile
        {
            protected override void Configure()
            {
                CreateMap<User, UserViewModel>();
                CreateMap<User, UserDetailsViewModel>();
                CreateMap<User, UserEditModel>();
                CreateMap<UserEditModel, User>()
                    .ForMember(x => x.Id, opts => opts.Ignore())
                    .ForMember(x => x.UserName, opts => opts.Ignore());
                CreateMap<UserAddModel, User>()
                    .ForMember(x => x.Id, opts => opts.Ignore())
                    .ConstructUsing(model => new User(model.UserName));
            }
        }

        private class ProductMappingProfile : Profile
        {
            protected override void Configure()
            {
                CreateMap<Product, ProductViewModel>();
                CreateMap<Product, ProductEditModel>();
                CreateMap<ProductEditModel, Product>()
                    .ForMember(x => x.Id, opts => opts.Ignore());
                CreateMap<ProductAddModel, Product>()
                    .ForMember(x => x.Id, opts => opts.Ignore())
                    .ConstructUsing(model => new Product(model.Title));
            }
        }

        private class CustomerMappingProfile : Profile
        {
            protected override void Configure()
            {
                CreateMap<Customer, CustomerViewModel>();
                CreateMap<Customer, CustomerEditModel>();
                CreateMap<CustomerEditModel, Customer>()
                    .ForMember(x => x.Id, opts => opts.Ignore());
                CreateMap<CustomerAddModel, Customer>()
                    .ForMember(x => x.Id, opts => opts.Ignore())
                    .ConstructUsing(model => new Customer(model.Name));
            }
        }

        private class OfferDraftMappingProfile : Profile
        {
            protected override void Configure()
            {
                CreateMap<OfferDraft, DraftViewModel>();
                CreateMap<OfferDraft, DraftDetailViewModel>();
                CreateMap<OfferDraftItem, DraftItemViewModel>();
                CreateMap<OfferDraft, DraftTotalsResponseModel>()
                    .ForMember(x => x.Total, opts => opts.UseValue(42));
            }
        }

        private class OfferMappingProfile : Profile
        {
            protected override void Configure()
            {
                CreateMap<Offer, OfferViewModel>();
                CreateMap<Offer, OfferDetailsViewModel>();
                CreateMap<OfferItem, OfferItemViewModel>();
            }
        }
    }
}