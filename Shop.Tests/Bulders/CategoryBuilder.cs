using Shop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Tests.Bulders
{
    public class CategoryBuilder: Builder<Category>
    {
        public CategoryBuilder WithName(string name)
        {
            _object.Name = name;
            return this;
        }

        public CategoryBuilder WithId(int id)
        {
            _object.Id = id;
            return this;
        }
    }
}
