// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Configuration;

[ExcludeFromCodeCoverage]
[Obsolete("To be removed. Use the ConnectionStrings section in appsettings.json instead.")]
public class MySqlSettings
{
    [Required]
    public string Server { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public uint Port { get; set; }

    [Required]
    public string Database { get; set; }
}
