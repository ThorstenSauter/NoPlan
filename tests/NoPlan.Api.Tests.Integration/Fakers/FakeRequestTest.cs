using Bogus;
using NoPlan.Contracts.Requests.V1.ToDos;

namespace NoPlan.Api.Tests.Integration.Fakers;

public class FakeRequestTest
{
    private const int CreateRequestSeed = 133742069;
    private const int UpdateRequestSeed = 420691337;

    public FakeRequestTest()
    {
        Randomizer.Seed = new();
        var createTagRequestFaker = new Faker<CreateTagRequest>()
            .RuleFor(x => x.Name, x => x.Lorem.Word())
            .UseSeed(CreateRequestSeed);

        CreateRequestFaker = new Faker<CreateToDoRequest>()
            .RuleFor(x => x.Title, x => x.Lorem.Sentence())
            .RuleFor(x => x.Description, x => x.Lorem.Sentence())
            .RuleFor(x => x.Tags, _ => createTagRequestFaker.Generate(3))
            .UseSeed(CreateRequestSeed);

        var updateTagRequestFaker = new Faker<UpdateTagRequest>()
            .RuleFor(x => x.Name, x => x.Lorem.Word())
            .UseSeed(UpdateRequestSeed);

        UpdateRequestFaker = new Faker<UpdateToDoRequest>()
            .RuleFor(x => x.Id, x => Guid.Empty)
            .RuleFor(x => x.Title, x => x.Lorem.Sentence())
            .RuleFor(x => x.Description, x => x.Lorem.Sentence())
            .RuleFor(x => x.Tags, _ => updateTagRequestFaker.Generate(3))
            .UseSeed(UpdateRequestSeed);
    }

    public Faker<UpdateToDoRequest> UpdateRequestFaker { get; }

    public Faker<CreateToDoRequest> CreateRequestFaker { get; }
}
