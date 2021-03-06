﻿using Shop.Models;

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
