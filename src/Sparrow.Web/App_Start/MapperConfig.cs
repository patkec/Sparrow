using AutoMapper;
using Sparrow.Domain.Models;
using Sparrow.Web.Models.Customers;
using Sparrow.Web.Models.Products;
using Sparrow.Web.Models.Users;

namespace Sparrow.Web
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
            });
            Mapper.AssertConfigurationIsValid();
        }

        private class UserMappingProfile: Profile
        {
            protected override void Configure()
            {
                CreateMap<User, UserViewModel>();
                CreateMap<User, UserEditModel>();
                CreateMap<UserEditModel, User>()
                    .ForMember(x => x.Id, opts => opts.Ignore());
                CreateMap<UserAddModel, User>()
                    .ForMember(x => x.Id, opts => opts.Ignore())
                    .ConstructUsing(model => new User(model.Name));
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
    }
}