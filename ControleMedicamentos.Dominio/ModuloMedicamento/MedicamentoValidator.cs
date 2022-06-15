using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.ModuloMedicamento
{
    public class MedicamentoValidator : AbstractValidator<Medicamento>
    {
        public MedicamentoValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Campo 'Nome' é obrigatório.");
        }
    }
}
