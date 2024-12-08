using System;
using System.Collections.Generic;

namespace Ekart_Web_App.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }
}
