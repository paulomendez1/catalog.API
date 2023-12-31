﻿using Catalog.Domain.DTOs.Request.User;
using Catalog.Domain.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Services
{
    public interface IUserService 
    {
        Task<UserResponse> GetUserAsync(GetUserRequest request, CancellationToken token = default);
        Task<UserResponse> SignUpAsync(SignUpRequest request, CancellationToken token = default);
        Task<TokenResponse> SignInAsync(SignInRequest request, CancellationToken token = default);
    }
}

