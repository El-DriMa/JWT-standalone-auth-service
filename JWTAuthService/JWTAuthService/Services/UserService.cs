using MapsterMapper;
using JWTAuthService.Database;
using JWTAuthService.Interfaces;
using JWTAuthService.SearchObjects;
using JWTAuthService.Helpers;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.EntityFrameworkCore;
using JWTAuthService.Models.Responses;
using JWTAuthService.Models.Requests;
using Microsoft.AspNetCore.Authorization;

namespace JWTAuthService.Services
{
    public class UserService : BaseCRUDService<UserResponse, UserSearchObject, Users, UserInsertRequest, UserUpdateRequest>, IUserService
    {
        public UserService(MyDatabaseContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public override IQueryable<Users> AddFilter(UserSearchObject search, IQueryable<Users> query)
        {
            if (!string.IsNullOrWhiteSpace(search.FTS))
            {
                query = query.Where(u => u.FirstName.Contains(search.FTS) || u.LastName.Contains(search.FTS) || u.Username.Contains(search.FTS));
            }

            if (!string.IsNullOrWhiteSpace(search.Username))
            {
                query = query.Where(u=>u.Username.Contains(search.Username));
            }

            return query;
        }

        public override async Task BeforeInsert(UserInsertRequest request, Users entity)
        {
            if (await _db.Users.AnyAsync(u => u.Username == request.Username))
                throw new ValidationException("Username already exists.");

            await Task.CompletedTask;
        }

        public override async Task<UserResponse> Insert(UserInsertRequest request)
        {
            await BeforeInsert(request, null);

            PasswordHelper.CreatePasswordHash(request.Password, out string hash, out string salt);

            var user = Mapper.Map<Users>(request);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            return Mapper.Map<UserResponse>(user);
        }
        public override async Task BeforeUpdate(UserUpdateRequest request, Users entity)
        {
            if (!string.IsNullOrWhiteSpace(request.FirstName))
                entity.FirstName = request.FirstName;

            if (!string.IsNullOrWhiteSpace(request.LastName))
                entity.LastName = request.LastName;

            entity.ModifiedAt = DateTime.Now;

            await Task.CompletedTask;
        }

      
        public async Task ChangePassword(int userId, ChangePasswordRequest request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId) ?? throw new ValidationException("User not found.");
            if (!PasswordHelper.VerifyPassword(request.OldPassword, user.PasswordHash, user.PasswordSalt))
                throw new ValidationException("Old password is not correct!");

            if (PasswordHelper.VerifyPassword(request.NewPassword, user.PasswordHash, user.PasswordSalt))
                throw new ValidationException("New password can not be then same as old password");

            PasswordHelper.CreatePasswordHash(request.NewPassword, out string hash, out string salt);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;
            user.ModifiedAt = DateTime.Now;
            await _db.SaveChangesAsync();
        }

        public virtual async Task<PagedResult<UserResponse>> GetPaged(UserSearchObject search)
        {
            var query = _db.Set<Users>().AsQueryable();

            query = AddFilter(search, query);

            int count = await query.CountAsync();

            if (search?.Page.HasValue == true && search?.PageSize.HasValue == true)
            {
                query = query.Skip((search.Page.Value - 1) * search.PageSize.Value).Take(search.PageSize.Value);
            }

            var list = await query.ToListAsync();

            var result = Mapper.Map<List<UserResponse>>(list);

            return new PagedResult<UserResponse>
            {
                Items = result,
                TotalCount = count
            };
        }


    }

}
