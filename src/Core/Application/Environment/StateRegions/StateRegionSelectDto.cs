﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Environment.StateRegions;
public class StateRegionSelectDto : IDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Abbreviation { get; set; }
    public int CountryId { get; set; }
}
