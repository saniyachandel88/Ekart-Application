﻿namespace Ekart_web_Application.DTO
{
    public class CategoryReadDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        = string.Empty;
        public string Description { get; set; }
        = string.Empty;
        public int ProductCount { get; set; }
    }
}
