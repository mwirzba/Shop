using AutoMapper;
using Shop.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Tests
{
    static class MapperHelpers
    {
        public static MapperConfiguration GetMapperConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            return config;
        }
    }


}
