using System;

namespace Entities; public class ProductLog { public int Id { get; set; } public int ProductId { get; set; } public string Action { get; set; } = string.Empty; public DateTime Timestamp { get; set; } }