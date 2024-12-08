using System;
using System.Collections.Generic;

namespace Ekart_Web_App.Models;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? ContactName { get; set; }

    public string? ContactTitle { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Region { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public string? Phone { get; set; }

    public string? Fax { get; set; }

    public string? HomePage { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public bool IsApproved { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
