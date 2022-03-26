using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Udemy.Shared.Dtos;
using UdemyAuthServer.Core.Configuration;
using UdemyAuthServer.Core.Dtos;
using UdemyAuthServer.Core.Entities;
using UdemyAuthServer.Core.Repositories;
using UdemyAuthServer.Core.Repositories.Service;
using UdemyAuthServer.Core.Service;
using UdemyAuthServer.Core.UnitOfWork;

namespace UdemyAuthServer.Service.Service
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenService;

        public AuthenticationService(IOptions<List<Client>> optionClient, ITokenService tokenService, UserManager<UserApp> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRefreshTokenService)
        {
            _clients = optionClient.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userRefreshTokenService = userRefreshTokenService;
        }

        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if(loginDto == null)throw new ArgumentNullException(nameof(loginDto));

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) 
                return Response<TokenDto>.Fail("email or password is wrong", 404, true);

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password)) 
            {
                return Response<TokenDto>.Fail("email or password is wrong", 400, true);
            }

            var token = _tokenService.CreateToken(user);

            var userRefreshToken = await _userRefreshTokenService.Where(i => i.UserId == user.Id).SingleOrDefaultAsync();

            if (userRefreshToken == null)
            {
                await _userRefreshTokenService.AddAsync(new UserRefreshToken
                {
                    UserId = user.Id,
                    Code = token.RefreshToken,
                    Expiration = token.RefreshTokenExpiration
                });
            }
            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }

            await _unitOfWork.CommitAsync();

            return Response<TokenDto>.Success(token, 200);
        }

        public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLogin)
        {
            var client = _clients.SingleOrDefault(i => i.Id == clientLogin.ClientId && i.Secret == clientLogin.ClientSecret);

            if (client == null)
            {
                return Response<ClientTokenDto>.Fail("ClientId or clientSecret not found", 404, true);
            }

            var token = _tokenService.CreateTokenByClient(client);

            return Response<ClientTokenDto>.Success(token, 200);
        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshToke(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenService.Where(i=>i.Code == refreshToken).FirstOrDefaultAsync();

            if (existRefreshToken == null)
            {
                return Response<TokenDto>.Fail("Refresh token not found", 404, true);
            }
            var user = await _userManager.FindByIdAsync(existRefreshToken.UserId);

            if(user == null)
            {
                return Response<TokenDto>.Fail("UserId not found", 404, true);
            }

            var tokenDto = _tokenService.CreateToken(user);

            existRefreshToken.Code = tokenDto.RefreshToken;
            existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();

            return Response<TokenDto>.Success(tokenDto,200);
        }

        //kullanıcı logout olduğunda refreshtokenı null yapalım metodu
        public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenService.Where(i=>i.Code == refreshToken).FirstOrDefaultAsync();

            if (existRefreshToken == null)
            {
                return Response<NoDataDto>.Fail("Refresh token not found", 404, true);
            }

            _userRefreshTokenService.Remove(existRefreshToken);
            await _unitOfWork.CommitAsync();

            return Response<NoDataDto>.Success(200);
        }
    }
}
