﻿using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.DTOs
{
    public class ProductDTO
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(1.0, 1000.0)]
        public double Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string? ImgUrl { get; set; }
		public int Count { get; set; } = 1;
	}
}
