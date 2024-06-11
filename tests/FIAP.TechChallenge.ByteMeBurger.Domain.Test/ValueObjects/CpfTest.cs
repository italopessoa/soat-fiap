// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Test.ValueObjects;

[TestSubject(typeof(Cpf))]
public class CpfTest
{
    [Fact]
    public void Validate_CorrectCpf_ShouldReturnTrue()
    {
        // Arrange
        var cpf = new Cpf("664.381.370-06");

        // Act & Assert
        cpf.Value.Should().Be("66438137006");
    }

    [Fact]
    public void Validate_IncorrectCpf_ShouldReturnFalse()
    {
        // Arrange
        var cpfValue = "123.456.789-02";
        var func = () => new Cpf(cpfValue);

        // Act & Assert
        func.Should().ThrowExactly<DomainException>()
            .And.Message.Should().Be($"Invalid CPF value '{cpfValue}'");
    }

    [Fact]
    public void Validate_NullCpf_ShouldThrowArgumentNullException()
    {
        // Arrange
        var func = () => new Cpf(null);

        // Act & Assert
        func.Should().ThrowExactly<DomainException>()
            .And.Message.Should().Be("cpf cannot be empty.");
    }

    [Fact]
    public void Validate_EmptyCpf_ShouldReturnFalse()
    {
        // Arrange
        var func = () => new Cpf(string.Empty);

        // Act & Assert
        func.Should().ThrowExactly<DomainException>()
            .And.Message.Should().Be("cpf cannot be empty.");
    }

    [Fact]
    public void ImplicitConversion_CpfToString_ExceptionWhenNull()
    {
        // Arrange

        Exception ex = default;
        try
        {
            Cpf cpfObject = (string)null!;
        }
        catch (Exception e)
        {
            ex = e;
        }

        using (new AssertionScope())
        {
            ex.Should().NotBeNull();
            ex.Should().BeOfType<DomainException>();
            ex.Message.Should().Be($"cpf cannot be empty.");
        }
    }
}
