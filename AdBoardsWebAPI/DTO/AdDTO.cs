namespace AdBoardsWebAPI.DTO
{
    public class AdDTO
    {
        public int Id { get; set; }

        public int? Price { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? City { get; set; }

        public byte[]? Photo { get; set; }

        public DateTime? Date { get; set; }

        public int? CotegorysId { get; set; }

        public int? PersonId { get; set; }

        public int? TypeOfAdId { get; set; }
    }
}
