using System;
using System.Collections.Generic;

namespace PatternRepository.Models.DB;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public int? Age { get; set; }

    public string? Email { get; set; }
}
