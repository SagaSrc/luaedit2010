using System;
using System.Collections.Generic;
using System.Text;

namespace LuaEdit.Interfaces
{
    public interface ILuaEditDocumentSolution : ILuaEditDocumentGroup
    {
        ILuaEditDocumentProject ActiveProject
        {
            get;
            set;
        }
    }
}
