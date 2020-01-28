using Patlus.IdentityManagement.UseCase.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities
{
    internal class IdentityTestData : IEnumerable<object[]>
    {
        public static IDictionary<Guid, HostedAccount> CreateHostedAccounts()
        {
            var hostedAccounts = new Dictionary<Guid, HostedAccount>();
            var identities = CreateIdentities();

            foreach (var pair in identities)
            {
                hostedAccounts.Add(pair.Value.HostedAccount.Id, pair.Value.HostedAccount);
            }

            return hostedAccounts;
        }

        public static IDictionary<Guid, Identity> CreateIdentities()
        {
            var admin0Id = new Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605");
            var admin1Id = new Guid("39905588-99d4-4fb5-a41b-18f88c3689d2");
            var employee0Id = new Guid("e599ecfa-0c7f-402f-a1ae-f3c936d0824b");

            var createdDate1 = new DateTimeOffset(2019, 10, 10, 0, 0, 0, 0, TimeSpan.Zero);
            var createdDate2 = new DateTimeOffset(2019, 10, 12, 0, 0, 0, 0, TimeSpan.Zero);

            var sysAdminPool = new Pool()
            {
                Id = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                Active = true,
                Name = $"System Administrator",
                CreatorId = admin0Id,
                CreatedTime = createdDate1,
                LastModifiedTime = createdDate1,
                Archived = false,
            };

            var employeePool = new Pool()
            {
                Id = new Guid("29899885-bbf1-430f-b9d6-613066b4021a"),
                Active = true,
                Name = $"Employee",
                CreatorId = admin0Id,
                CreatedTime = createdDate2,
                LastModifiedTime = createdDate2,
                Archived = false,
            };

            var identities = new Dictionary<Guid, Identity>() {
                {
                    admin0Id,
                    new Identity() {
                        Id = admin0Id,
                        PoolId = sysAdminPool.Id,
                        AuthKey = admin0Id,
                        Active = true,
                        Name = $"System Admin 0",
                        CreatorId = admin0Id,
                        CreatedTime = createdDate1,
                        LastModifiedTime = createdDate1,
                        Archived = true,
                        Pool = sysAdminPool,
                        HostedAccount = new HostedAccount()
                        {
                            Id = admin0Id,
                            Name = $"sysadmin0",
                            Password = $"sysadminpassword0",
                            CreatorId = admin0Id,
                            CreatedTime = createdDate1,
                            LastModifiedTime = createdDate1,
                            Archived = false,
                        }
                    }
                },
                {
                    admin1Id,
                    new Identity() {
                        Id = admin1Id,
                        PoolId = sysAdminPool.Id,
                        AuthKey = admin1Id,
                        Active = true,
                        Name = $"System Admin 1",
                        CreatorId = admin0Id,
                        CreatedTime = createdDate1,
                        LastModifiedTime = createdDate1,
                        Archived = false,
                        Pool = sysAdminPool,
                        HostedAccount = new HostedAccount()
                        {

                            Id = admin1Id,
                            Name = $"sysadmin1",
                            Password = $"sysadminpassword1",
                            CreatorId = admin0Id,
                            CreatedTime = createdDate1,
                            LastModifiedTime = createdDate1,
                            Archived = false,
                        }
                    }
                },
                {
                    employee0Id,
                    new Identity() {
                        Id = employee0Id,
                        PoolId = employeePool.Id,
                        AuthKey = employee0Id,
                        Active = true,
                        Name = $"Employee 0",
                        CreatorId = admin1Id,
                        CreatedTime = createdDate2,
                        LastModifiedTime = createdDate2,
                        Archived = false,
                        Pool = employeePool,
                        HostedAccount = new HostedAccount()
                        {
                            Id = employee0Id,
                            Name = $"employee0",
                            Password = $"employeepassword0",
                            CreatorId = admin1Id,
                            CreatedTime = createdDate2,
                            LastModifiedTime = createdDate2,
                            Archived = false,
                        }
                    }
                }
            };

            foreach (var pair in identities)
            {
                pair.Value.HostedAccount.Identity = pair.Value;
            }

            return identities;
        }
        public IEnumerator<object[]> GetEnumerator()
        {
            var requestorId = "7c9e6679-7425-40de-944b-e07fc1f90ae7";

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.Id == new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"))
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.PoolId == new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"))
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.Active == true)
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.Name == "System Admin 0")
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.Name.Contains("System Admin"))
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.Name.Length == 14)
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.Archived == true)
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.CreatedTime == new DateTimeOffset(2019, 10, 10, 0, 0, 0, 0, TimeSpan.Zero))
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.CreatedTime == new DateTimeOffset(2019, 10, 10, 2, 0, 0, 0, TimeSpan.FromMinutes(120)))
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.CreatedTime >= new DateTimeOffset(2019, 10, 9, 0, 0, 0, 0, TimeSpan.Zero) && e.CreatedTime <= new DateTimeOffset(2019, 10, 12, 0, 0, 0, 0, TimeSpan.Zero))
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.CreatedTime >= new DateTimeOffset(2019, 10, 9, 2, 0, 0, 0, TimeSpan.FromMinutes(120)) && e.CreatedTime <= new DateTimeOffset(2019, 10, 12, 2, 0, 0, 0, TimeSpan.FromMinutes(120)))
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.CreatedTime >= new DateTimeOffset(2019, 10, 9, 0, 0, 0, 0, TimeSpan.Zero) && e.CreatedTime <= new DateTimeOffset(2019, 10, 11, 0, 0, 0, 0, TimeSpan.Zero))
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.CreatedTime >= new DateTimeOffset(2019, 10, 9, 2, 0, 0, 0, TimeSpan.FromMinutes(120)) && e.CreatedTime <= new DateTimeOffset(2019, 10, 11, 2, 0, 0, 0, TimeSpan.FromMinutes(120)))
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.LastModifiedTime == new DateTimeOffset(2019, 10, 10, 0, 0, 0, 0, TimeSpan.Zero))
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.LastModifiedTime == new DateTimeOffset(2019, 10, 10, 2, 0, 0, 0, TimeSpan.FromMinutes(120)))
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.LastModifiedTime >= new DateTimeOffset(2019, 10, 9, 0, 0, 0, 0, TimeSpan.Zero) && e.LastModifiedTime <= new DateTimeOffset(2019, 10, 12, 0, 0, 0, 0, TimeSpan.Zero))
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.LastModifiedTime >= new DateTimeOffset(2019, 10, 9, 2, 0, 0, 0, TimeSpan.FromMinutes(120)) && e.LastModifiedTime <= new DateTimeOffset(2019, 10, 12, 2, 0, 0, 0, TimeSpan.FromMinutes(120)))
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.LastModifiedTime >= new DateTimeOffset(2019, 10, 9, 0, 0, 0, 0, TimeSpan.Zero) && e.LastModifiedTime <= new DateTimeOffset(2019, 10, 11, 0, 0, 0, 0, TimeSpan.Zero))
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.LastModifiedTime >= new DateTimeOffset(2019, 10, 9, 2, 0, 0, 0, TimeSpan.FromMinutes(120)) && e.LastModifiedTime <= new DateTimeOffset(2019, 10, 11, 2, 0, 0, 0, TimeSpan.FromMinutes(120)))
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.CreatorId == new Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605"))
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.Pool.Id == new Guid("29899885-bbf1-430f-b9d6-613066b4021a"))
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e =>  e.Id == new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"))
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.HostedAccount.Name == "systemadmin0")
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.HostedAccount.Name.Contains("systemadmin"))
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.HostedAccount.Name.Length == 5)
            };

            yield return new object[] {
                requestorId,
                CreateCondition(e => e.HostedAccount.Archived == false)
            };

        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private Expression<Func<Identity, bool>> CreateCondition(Expression<Func<Identity, bool>> condition)
        {
            return condition;
        }
    }
}
