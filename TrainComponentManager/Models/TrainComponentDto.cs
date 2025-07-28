namespace TrainComponentManager.Models
{
    public class TrainComponentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string UniqueNumber { get; set; } = "";
        public bool CanAssignQuantity { get; set; }
        public int? Quantity { get; set; }

        // Не передается на сервер — только для ввода в UI
        public int? NewQuantity { get; set; }
    }
}
