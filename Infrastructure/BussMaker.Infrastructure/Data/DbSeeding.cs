﻿using BussMaker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussMaker.Infrastructure.Data
{
    public class DbSeeding
    {
        public static void SeedDatabase(BussMakerDbContext context)
        {
            seedEntityIfNotExist(context);
        }

        private static void seedEntityIfNotExist(BussMakerDbContext context)
        {
            if (!context.Entities.Any())
            {
                var entities = new List<Entity>
                {
                    new Entity
                    {
                        Name = "FirstEntity",
                        Fields = "Deneme"
                    },
                    new Entity
                    {
                        Name = "SecondEntity",
                        Fields = "Deneme"
                    },
                };
                context.Entities.AddRange(entities);
                context.SaveChanges();
            }   
        }
    }
}
