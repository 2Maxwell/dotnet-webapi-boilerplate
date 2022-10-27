using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Domain.Hotel;

public class Restriction : AuditableEntity<int>
{
    public string Source { get; set; }
    public int SourceId { get; set; }
    public bool Clean { get; set; }

}
