using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Model.EfCode;
using WpfAppMVVM.Model.EfCode.Entities;

namespace Test.Initialization
{
    internal static class SampleDatalnitializer
    {
        public static void DropAndCreateDatabase(TransportationEntities context)
        {
            context.Database.EnsureDeleted();
            context.Database.Migrate();
            Mock<TransportationEntities> mock = new Mock<TransportationEntities>();
        }

    }
}
