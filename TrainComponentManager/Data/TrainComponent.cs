﻿namespace TrainComponentManager.Data
{
    public class TrainComponent
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UniqueNumber { get; set; } = string.Empty;
        public bool CanAssignQuantity { get; set; }
        public int? Quantity { get; set; }
    }
}
