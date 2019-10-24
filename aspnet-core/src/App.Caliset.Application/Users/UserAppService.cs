using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using App.Caliset.Authorization;
using App.Caliset.Authorization.Accounts;
using App.Caliset.Authorization.Roles;
using App.Caliset.Authorization.Users;
using App.Caliset.Operations.Dto;
using App.Caliset.Roles.Dto;
using App.Caliset.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.Caliset.Users
{
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;

        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager)
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
        }

        [AbpAuthorize(PermissionNames.Administrador)]
        public override async Task<UserDto> Create(CreateUserDto input)
        {
            CheckCreatePermission();

            if (input.Document.ToString().Length > 8)
                throw new UserFriendlyException("Error", "Documento inválido, máximo 8 dígitos") ;

            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;
            user.FirstLogin = true;

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            try
            {
                var crearUsuario = await _userManager.CreateAsync(user, input.Password);
            }
            catch (System.Exception e)            {

                string ExceptionMessage = "Error";    
            throw new UserFriendlyException("Falló la creación del Usuario", ExceptionMessage);
            }       
          
            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
            }

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(user);
        }

        [AbpAuthorize(PermissionNames.Administrador)]
        public override async Task<UserDto> Update(UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);
            input.RoleNames = null;
            input.LastLoginTime = null;

            MapToEntity(input, user); 

            CheckErrors(await _userManager.UpdateAsync(user));

           

            return await Get(input);
        }

        [AbpAuthorize(PermissionNames.Administrador)]
        public override async Task Delete(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roles = _roleManager.Roles.Where(r => user.Roles.Any(ur => ur.RoleId == r.Id)).Select(r => r.NormalizedName);
            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();
            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Debe iniciar sesión para realizar esta acción.");
            }
            long userId = _abpSession.UserId.Value;
            var user = await _userManager.GetUserByIdAsync(userId);
            var loginAsync = await _logInManager.LoginAsync(user.UserName, input.CurrentPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Error", "La contraseña actual no es correcta.");
            }
            if (!new Regex(AccountAppService.PasswordRegex).IsMatch(input.NewPassword))
            {
                throw new UserFriendlyException("La contraseña debe ser mayor de 8 caracteres, contener mayúscula, minúscula, y número.");
            }
            user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
            CurrentUnitOfWork.SaveChanges();
            return true;
        }

        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Debe iniciar sesión para realizar esta acción.");
            }
            long currentUserId = _abpSession.UserId.Value;
            var currentUser = await _userManager.GetUserByIdAsync(currentUserId);
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Error", "La contraseña de administrador no es correcta.");
            }
            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(StaticRoleNames.Tenants.Administrador))
            {
                throw new UserFriendlyException("Error", "Solo usuarios administradores pueden realizar esta acción.");
            }

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                CurrentUnitOfWork.SaveChanges();
            }

            return true;
        }

        public async Task<bool> SetFirstLogin()
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Inicie sesión.");
            }
            long userId = _abpSession.UserId.Value;
            var user = await _userManager.GetUserByIdAsync(userId);

            if (user!= null)
            {
                user.FirstLogin = false;
                CheckErrors(await _userManager.UpdateAsync(user));
                return true;
            }

            return false;
        }

        public IEnumerable<UserDtoOperation> GetAllUsers()
        {
            var getAll = _userManager.GetAll().ToList();
            List<UserDtoOperation> output = ObjectMapper.Map<List<UserDtoOperation>>(getAll);
            return output;
        }

        public IEnumerable<UserDtoOperation> GetAllUsersFilter(GetUserFiltersInput input)
        {
            var getAll = _userManager.GetAllFilter(input.Keyword, input.Active).ToList();
            List<UserDtoOperation> output = ObjectMapper.Map<List<UserDtoOperation>>(getAll);
            return output;
        }

        public void SetLastLoginTime()
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Inicie sesión.");
            }
            long userId = _abpSession.UserId.Value;

            _userManager.SetLastLoginTime(userId);
        }

        
    }
}

