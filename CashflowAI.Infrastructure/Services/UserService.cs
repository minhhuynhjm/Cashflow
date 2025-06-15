using AutoMapper;
using CashflowAI.Domain.DTOs;
using CashflowAI.Domain.Entities;
using CashflowAI.Domain.Interfaces;
using CashflowAI.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace CashflowAI.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user == null)
                {
                    throw new NotFoundException($"User with ID {id} not found");
                }
                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex) when (ex is not NotFoundException)
            {
                _logger.LogError(ex, "Error getting user by id {UserId}", id);
                throw;
            }
        }

        public async Task<UserDto> GetByUsernameAsync(string username)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByUsernameAsync(username);
                if (user == null)
                {
                    throw new NotFoundException($"User with username {username} not found");
                }
                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex) when (ex is not NotFoundException)
            {
                _logger.LogError(ex, "Error getting user by username {Username}", username);
                throw;
            }
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByEmailAsync(email);
                if (user == null)
                {
                    throw new NotFoundException($"User with email {email} not found");
                }
                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex) when (ex is not NotFoundException)
            {
                _logger.LogError(ex, "Error getting user by email {Email}", email);
                throw;
            }
        }

        public async Task<UserDto> CreateUserAsync(UserCreateDto dto)
        {
            try
            {
                // Check if username or email already exists
                var existingUser = await _unitOfWork.Users.GetByUsernameAsync(dto.UserName) 
                    ?? await _unitOfWork.Users.GetByEmailAsync(dto.Email);

                if (existingUser != null)
                {
                    throw new BusinessException("Username or email already exists");
                }

                var user = _mapper.Map<User>(dto);
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                user.CreatedAt = DateTime.UtcNow;
                user.IsActive = true;

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex) when (ex is not BusinessException)
            {
                _logger.LogError(ex, "Error creating user");
                throw;
            }
        }

        public async Task UpdateAsync(int id, UserUpdateDto dto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user == null)
                {
                    throw new NotFoundException($"User with ID {id} not found");
                }

                // Check if new email is already taken by another user
                if (dto.Email != user.Email)
                {
                    var existingUser = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
                    if (existingUser != null)
                    {
                        throw new BusinessException("Email already exists");
                    }
                }

                _mapper.Map(dto, user);

                // Update password if provided
                if (!string.IsNullOrEmpty(dto.Password))
                {
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                }

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex) when (ex is not NotFoundException && ex is not BusinessException)
            {
                _logger.LogError(ex, "Error updating user {UserId}", id);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user == null)
                {
                    throw new NotFoundException($"User with ID {id} not found");
                }

                await _unitOfWork.Users.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex) when (ex is not NotFoundException)
            {
                _logger.LogError(ex, "Error deleting user {UserId}", id);
                throw;
            }
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByUsernameAsync(username);
                if (user == null)
                {
                    return false;
                }

                return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating user {Username}", username);
                throw;
            }
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new NotFoundException($"User with ID {userId} not found");
                }

                if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
                {
                    return false;
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex) when (ex is not NotFoundException)
            {
                _logger.LogError(ex, "Error changing password for user {UserId}", userId);
                throw;
            }
        }
    }
} 