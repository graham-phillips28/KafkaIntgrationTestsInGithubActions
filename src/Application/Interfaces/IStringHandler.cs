
namespace KafkaIntgrationTestsInGithubActions.Application.Interfaces
{
    public interface IStringHandler
    {
        public Task Handle(string consumedString);
    }
}
