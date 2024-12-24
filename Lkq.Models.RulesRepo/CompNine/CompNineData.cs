using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Lkq.Models.RulesRepo.CompNine
{
    [ExcludeFromCodeCoverage]
    public class CompNineData
    {
        public bool Found { get; set; }

        public string Description { get; set; }

        public string ModelYear { get; set; }

        public string BuildDate { get; set; }

        public List<CompNineOption> OptionList { get; set; }
    }
}