namespace AdBoardsWebAPI.DTO
{
    public class PersonDTO
    {
        public string? Login { get; set; }

        public string? Password { get; set; }

        public string? Name { get; set; }

        public string? City { get; set; }

        public DateTime? Birthday { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public byte[]? Photo { get; set; }

        public int? RightId { get; set; }
    }
}
