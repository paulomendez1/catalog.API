﻿using Catalog.Domain.Configurations;
using Catalog.Domain.Contracts.Persistence;
using Catalog.Domain.DTOs.Request.User;
using Catalog.Domain.DTOs.Response;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository, IOptions<AuthenticationSettings> authenticationSettings)
        {
            _userRepository = userRepository;
            _authenticationSettings = authenticationSettings.Value;
        }
        public async Task<UserResponse> GetUserAsync(GetUserRequest request, CancellationToken token = default)
        {
            var response = await _userRepository.GetByEmailAsync(request.Email, token);
            return new UserResponse
            {
                Name = response.Name,
                Email = response.Email
            };
        }

        public async Task<TokenResponse> SignInAsync(SignInRequest request, CancellationToken token = default)
        {
            bool response = await _userRepository.AuthenticateAsync(request.Email, request.Password, token);
            return response == false ? null : new TokenResponse
            {
                Token = GenerateSecurityToken(request).Token,
                ExpirationDate = GenerateSecurityToken(request).ExpirationDate
            };
        }

        public async Task<UserResponse> SignUpAsync(SignUpRequest request, CancellationToken token = default)
        {
            var user = new Entities.User
            {
                Email = request.Email,
                UserName = request.Email,
                Name = request.Name
            };
            bool result = await _userRepository.SignUpAsync(user, request.Password, token);
            return !result ? null : new UserResponse
            {
                Name = request.Name,
                Email = request.Email
            };
        }

        private TokenResponse GenerateSecurityToken(SignInRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authenticationSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, request.Email) }),
                Expires = DateTime.UtcNow.AddMinutes(_authenticationSettings.ExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new TokenResponse
            {
                Token = tokenHandler.WriteToken(token),
                ExpirationDate = tokenDescriptor.Expires.Value
            };
        }
    }
}
