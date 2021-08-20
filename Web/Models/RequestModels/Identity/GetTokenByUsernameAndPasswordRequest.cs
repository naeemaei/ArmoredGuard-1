﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Web.Models.RequestModels.Identity
{
    public class GetTokenByUsernameAndPasswordRequest : IValidatableObject
    {
        [JsonProperty("grant_type")] [Required] public string GrantType { get; set; }

        [JsonProperty("username")] [Required] public string Username { get; set; }

        [JsonProperty("password")] [Required] public string Password { get; set; }

        [JsonProperty("client_id")] [Required] public string ClientId { get; set; }

        [JsonProperty("client_secret")] [Required] public string ClientSecret { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!GrantType.Equals("password", StringComparison.OrdinalIgnoreCase))
                yield return new ValidationResult("GrantType is invalid", new[] {nameof(GrantType)});
        }
    }
}