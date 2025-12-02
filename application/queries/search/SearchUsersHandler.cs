using domain.interfaces.repository.read;
using MediatR;

namespace application.queries.search
{
    public class SearchUsers : IRequestHandler<SearchUsersRequest, SearchUsersResponse>
    {
        private readonly IUserReadRepository UserReadRepository;
        public SearchUsers(IUserReadRepository userReadRepository)
        {
            UserReadRepository = userReadRepository;
        }

        public async Task<SearchUsersResponse> Handle(SearchUsersRequest request, CancellationToken cancellationToken)
        {
            var param = new domain.parameters.SearchUsersParameter
            {
                Username = request?.Username ?? string.Empty,
                Status = request?.Status
            };

            var users = await UserReadRepository.SearchUsersAsync(param);

            var response = new SearchUsersResponse
            {
                Users = users
            };

            return response;
        }
    }
}