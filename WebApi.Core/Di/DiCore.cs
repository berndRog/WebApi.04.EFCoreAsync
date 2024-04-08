using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Core.DomainModel.Entities;
using WebApi.Core.Mapping;
namespace WebApi.Di; 
public static class DiCore {
   public static void AddCore(
      this IServiceCollection services
   ){
      services.AddAutoMapper(typeof(Owner), typeof(MappingProfile));
      // Auto Mapper Configurations
      var mapperConfig = new MapperConfiguration(config => {
         config.AddProfile(new MappingProfile());
      });
   }
}