
using Bogus;
using Edc.Core.AccountContext.Entities;

namespace Edc.UnitTests.Core.AccountContext.Entities;

public static class AccountMock {
    public static Faker<Account> AccountFaker =>
        new Faker<Account>()
            .CustomInstantiator(
                x => new Account(
                    x.Person.FullName,
                    x.Internet.Email(),
                    x.Internet.Password(10),
                    RoleMock.RoleFaker.Generate()
                )
            );
}