namespace Secu_School_API.DTOs
{
    public class SalaryComponentDto
    {
        public int? ComponentId { get; set; } // Nullable for create

        public int SchoolId { get; set; }

        public string ComponentName { get; set; } = string.Empty;

        public string ComponentType { get; set; } = string.Empty;

        public bool IsTaxable { get; set; } = false;

        public bool IsEarning { get; set; } = true;

        public bool IsDeduction { get; set; } = false;
    }
}
