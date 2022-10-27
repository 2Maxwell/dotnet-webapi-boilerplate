using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.Items;

public class ItemSelectDto : IDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int ItemNumber { get; set; }
    public string ValueContent {
        get
        {
            return ItemNumber.ToString() + " | " + Name;
        }
    }
}
