using AutoMapper;
using CashflowAI.Domain.Entities;
using CashflowAI.Domain.DTOs;

namespace CashflowAI.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();

            CreateMap<Category, CategoryDto>().ReverseMap();
            //CreateMap<CreateCategoryDto, Category>();
            //CreateMap<CategoryUpdateDto, Category>();

            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<CreateTransactionDto, Transaction>();
            CreateMap<UpdateTransactionDto, Transaction>();
        }
    }
} 