namespace SmartTaskApi.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Store hashed password, not plain text

        public string Role { get; set; }  // 🔹 New field for user roles
    }
}
