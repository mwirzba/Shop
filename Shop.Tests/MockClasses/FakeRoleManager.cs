using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;

namespace Shop.Tests.MockClasses
{
    public class FakeRoleManager: RoleManager<IdentityRole>
    {
        public FakeRoleManager() : base(
            new Mock<IRoleStore<IdentityRole>>().Object,
            new IRoleValidator<IdentityRole>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<ILogger<RoleManager<IdentityRole>>>().Object)
        { }

        /*
         var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                    new Mock<IRoleStore<IdentityRole>>().Object,
                    new IRoleValidator<IdentityRole>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<ILogger<RoleManager<IdentityRole>>>().Object);

         */
    }
}
