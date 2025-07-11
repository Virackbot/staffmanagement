﻿using Domain.Enums;
using Domain.Models;
using Domain.OutfaceModels;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces;

public interface IHttpContextScope
{
    public string UserId { get; }
    public string FullName { get; }
    public string BusinessProfileId { get; }
    public string AuthorizedValue { get; }
  
    public IServiceProvider Provider { get; set; }

    public AppSetting AppSetting { get; set; }
    public HttpClientInfo ClientInfo { get; }
    public HttpClientInfo GetHttpClientInfo();

    public Task<string> GetScopeIdAsync();
    public Task SetScopeIdAsync(string value);
    public void SetDefaultUserId(string? userId);
    public string GetBusinessProfileId();
    public string ReferrerKey { get; }
}
