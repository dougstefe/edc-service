using Edc.Core.Notifications;
using MediatR;

namespace Edc.Api.Extensions;

public static class AccountContextExtension {
    public static void AddAccountContext(this WebApplicationBuilder builder) {
        builder.Services.AddScoped<
            Notification
        >();

        # region Create
        builder.Services.AddTransient<
            Core.AccountContext.UseCases.Create.Interfaces.IAddRepository,
            Infra.AccountContext.UseCases.Create.Repository
        >();
        builder.Services.AddTransient<
            Core.AccountContext.UseCases.Create.Interfaces.IExistsRepository,
            Infra.AccountContext.UseCases.Create.Repository
        >();
        builder.Services.AddTransient<
            Core.AccountContext.UseCases.Create.Interfaces.IGetUserRoleRepository,
            Infra.AccountContext.UseCases.Create.Repository
        >();
        builder.Services.AddTransient<
            Core.AccountContext.UseCases.Create.Interfaces.IService,
            Infra.AccountContext.UseCases.Create.Service
        >();
        builder.Services.AddScoped<
            Core.AccountContext.UseCases.Create.Interfaces.IValidation,
            Core.AccountContext.UseCases.Create.Validation
        >();
        #endregion
    }

    public static void MapAccountRoutes(this WebApplication app) {
        app.MapPost(
            "api/v1/accounts",
            async (
                Notification _notification,
                Core.AccountContext.UseCases.Create.Request request,
                IRequestHandler<
                    Core.AccountContext.UseCases.Create.Request,
                    Core.SharedContext.UseCases.Response<Core.AccountContext.UseCases.Create.ResponseData>
                > handler
            ) => {
                var response = await handler.Handle(request, new CancellationToken());

                if(!response.IsSuccess) {
                    return _notification.GetResult();
                }

                return Results.Json(data: response.Data, statusCode: StatusCodes.Status201Created);
            }
        );
    }
}