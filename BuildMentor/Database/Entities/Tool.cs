using BuildMentor.Database.Enums;
using System.ComponentModel.DataAnnotations;

namespace BuildMentor.Database.Entities
{
    public class Tool
    {
        [Key]
        public int Id { get; set; } 

        public string Name { get; set; }    

        public string Description { get; set; } 

        public Image Image { get; set; }

        public decimal Price { get; set; }

        public IList<Instruction> Instructions { get; set; }

        public IList<ToolPermission> ToolPermissions { get; set; }

        public IList<ToolPermissionRequest> ToolPermissionRequests { get; set; }

        public IList<UserTool> UserTools { get; set; }
    }
}
