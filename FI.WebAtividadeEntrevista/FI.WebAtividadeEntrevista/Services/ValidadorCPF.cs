using System;
using System.ComponentModel.DataAnnotations;

public class ValidadorCPF : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return ValidationResult.Success; // Valor nulo ou vazio, então não há validação
        }

        string cpf = value.ToString();
        cpf = RemoverCaracteresEspeciais(cpf);

        if (!ValidarTamanhoCPF(cpf) || VerificarSequenciaNumerica(cpf))
        {
            return new ValidationResult("CPF inválido.");
        }

        string cpfSemDigitos = cpf.Substring(0, 9);
        int soma = 0;

        for (int i = 0; i < 9; i++)
        {
            soma += int.Parse(cpfSemDigitos[i].ToString()) * (10 - i);
        }

        int primeiroDigitoVerificador = soma % 11;
        primeiroDigitoVerificador = primeiroDigitoVerificador < 2 ? 0 : 11 - primeiroDigitoVerificador;

        if (int.Parse(cpf[9].ToString()) != primeiroDigitoVerificador)
        {
            return new ValidationResult("Digite um CPF válido!");
        }

        soma = 0;
        cpfSemDigitos += primeiroDigitoVerificador;

        for (int i = 0; i < 10; i++)
        {
            soma += int.Parse(cpfSemDigitos[i].ToString()) * (11 - i);
        }

        int segundoDigitoVerificador = soma % 11;
        segundoDigitoVerificador = segundoDigitoVerificador < 2 ? 0 : 11 - segundoDigitoVerificador;

        if (int.Parse(cpf[10].ToString()) != segundoDigitoVerificador)
        {
            return new ValidationResult("Digite um CPF válido!");
        }

        return ValidationResult.Success;
    }

    private static bool ValidarTamanhoCPF(string cpf)
    {
        return cpf.Length == 11;
    }

    private static bool VerificarSequenciaNumerica(string cpf)
    {
        string[] sequenciaInvalida = { "00000000000", "11111111111", "22222222222", "33333333333", "44444444444",
                                       "55555555555", "66666666666", "77777777777", "88888888888", "99999999999"};

        return Array.Exists(sequenciaInvalida, s => s == cpf);
    }

    private static string RemoverCaracteresEspeciais(string cpf)
    {
        return cpf.Replace(".", "").Replace("-", "");
    }
}
