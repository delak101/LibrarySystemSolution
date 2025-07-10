// This is a one-time script to approve existing admin users
// Run this after implementing the approval system

using LibrarySystemApp.Data;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Scripts
{
    public class ApproveExistingAdmins
    {
        public static async Task ApproveAllExistingAdmins(LibraryContext context)
        {
            // Get all admin users that are not yet approved
            var adminUsers = await context.Users
                .Where(u => u.Role == "Admin" && !u.IsApproved)
                .ToListAsync();

            foreach (var admin in adminUsers)
            {
                admin.IsApproved = true;
                admin.ApprovedAt = DateTime.UtcNow;
                admin.ApprovedBy = admin.Id; // Self-approved
            }

            await context.SaveChangesAsync();
            Console.WriteLine($"Approved {adminUsers.Count} admin users");
        }
    }
}
