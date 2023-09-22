
using Bogus;
using Edc.Core.AccountContext.Entities;

namespace Edc.UnitTests.Core.AccountContext.Entities;

public static class RoleMock {
    public static Faker<Role> RoleFaker =>
        new Faker<Role>()
            .CustomInstantiator(
                x => new Role(
                    x.Random.Word(),
                    x.Random.Words()
                )
            );
}