using notes_by_nodes.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Dto
{
    public struct UserDto(int uid, string name, string email) : IUserDto
    {
        public int Uid { get; set; } = uid;
        public string Name { get; set; } = name;
        public string Email { get; set; } = email;
    }

    public struct NodeDto(int uid, string name, string description, string text) : INodeDto
    {
        public int Uid { get; set; } = uid;
        public string Name { get; set; } = name;
        public string Description { get; set; } = description;
        public string Text { get; set; } = text;
    }
}
