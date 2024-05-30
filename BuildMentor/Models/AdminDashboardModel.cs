namespace BuildMentor.Models
{
    public class AdminDashboardModel
    {
        public int NewNotifications { get; set; } = 0;

        public int PendingToolPermissions { get; set;} = 0;

        public int PendingAdminPermissions { get; set; } = 0;

        public int NewUsers { get; set; } = 0;

        public int NewTools { get; set; } = 0;


    }
}
