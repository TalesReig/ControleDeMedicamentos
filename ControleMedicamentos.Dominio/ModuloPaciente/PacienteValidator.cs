using ControleMedicamentos.Dominio.ModuloPaciente;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.ModuloPaciente
{
    public class PacienteValidator : AbstractValidator<Paciente>
    {
        public PacienteValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Campo 'Nome' é obrigatório.");
            RuleFor(x => x.CartaoSUS)
                .NotEmpty()
                .WithMessage("Campo 'Cartão SUS' é obrigatório.");
        }
    }
}
