using AutoMapper;
using CashflowAI.Domain.DTOs;
using CashflowAI.Domain.Entities;
using CashflowAI.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CashflowAI.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CategoryService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryDto>> GetByUserIdAsync(int userId)
        {
            var categories = await _unitOfWork.Categories.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                throw new Exception($"Category with ID {id} not found");
            }
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateAsync(CategoryDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task UpdateAsync(int id, CategoryDto dto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                throw new Exception($"Category with ID {id} not found");
            }

            _mapper.Map(dto, category);
            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                throw new Exception($"Category with ID {id} not found");
            }

            // Check if category has transactions
            var transactions = await _unitOfWork.Transactions.GetByCategoryIdAsync(id);
            if (transactions.Any())
            {
                throw new Exception("Cannot delete category that has transactions");
            }

            await _unitOfWork.Categories.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<CategoryDto>> GetListAsync()
        {
            var categories = await _unitOfWork.Categories.GetDefaultCategoriesAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }
    }
} 