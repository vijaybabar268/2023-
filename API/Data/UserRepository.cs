using API.Dtos;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(u => u.Id.Equals(id));
        }

        public async Task<AppUser> GetUserByUsernameAsync(string Username)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(u => u.UserName == Username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
                .Include(p => p.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider) 
                .SingleOrDefaultAsync();
        }

        public async Task<MemberDto> GetMemberAsync(int id)
        {
            return await _context.Users
                .Where(x => x.Id == id)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider) 
                .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .AsNoTracking();

            return await PagedList<MemberDto>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
        }
    }
}