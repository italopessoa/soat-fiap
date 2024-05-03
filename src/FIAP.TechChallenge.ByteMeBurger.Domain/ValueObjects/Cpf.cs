using FIAP.TechChallenge.ByteMeBurger.Domain.Base;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

public class Cpf : ValueObject
{
    public Cpf(string cpf)
    {
        Value = Validate(cpf);
    }

    public string Value { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public static implicit operator Cpf(string cpf)
    {
        return new Cpf(SanityseCpf(cpf));
    }
    
    public static implicit operator string(Cpf cpf)
    {
        return cpf.Value;
    }

    public override string ToString()
    {
        return Value;
    }

    private static string SanityseCpf(string cpf) => cpf
        .Replace(".", "")
        .Replace("-", "")
        .Trim();

    private static string Validate(string cpf)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cpf);
        var isValidCpf = false;

        var multiplicador1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplicador2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        var workingWpf = SanityseCpf(cpf);

        if (workingWpf.Length == 11 && workingWpf.Distinct().Count() > 1)
        {
            var tempCpf = workingWpf.Substring(0, 9);
            var soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            var resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            var digito = resto.ToString();

            tempCpf += digito;

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();

            isValidCpf = workingWpf.EndsWith(digito);
        }

        if (!isValidCpf)
        {
            throw new ArgumentException($"Invalid CPF value '{cpf}'");
        }

        return workingWpf;
    }
}