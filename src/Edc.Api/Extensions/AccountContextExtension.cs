using Edc.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        #region Confirmate
        builder.Services.AddTransient<
            Core.AccountContext.UseCases.Confirmate.Interfaces.IGetByIdRepository,
            Infra.AccountContext.UseCases.Confirmate.Repository
        >();
        builder.Services.AddTransient<
            Core.AccountContext.UseCases.Confirmate.Interfaces.IUpdateRepository,
            Infra.AccountContext.UseCases.Confirmate.Repository
        >();
        builder.Services.AddScoped<
            Core.AccountContext.UseCases.Confirmate.Interfaces.IValidation,
            Core.AccountContext.UseCases.Confirmate.Validation
        >();
        #endregion

        #region Login
        builder.Services.AddTransient<
            Core.AccountContext.UseCases.Login.Interfaces.IGetRepository,
            Infra.AccountContext.UseCases.Login.Repository
        >();
        builder.Services.AddScoped<
            Core.AccountContext.UseCases.Login.Interfaces.IValidation,
            Core.AccountContext.UseCases.Login.Validation
        >();
        #endregion
    }

    public static void MapAccountRoutes(this WebApplication app) {
        app.MapPost(
            "api/v1/accounts",
            async (
                Notification notification,
                Core.AccountContext.UseCases.Create.Request request,
                IRequestHandler<
                    Core.AccountContext.UseCases.Create.Request,
                    Core.SharedContext.UseCases.Response<Core.AccountContext.UseCases.Create.ResponseData>
                > handler
            ) => {
                var response = await handler.Handle(request, new CancellationToken());

                if(!response.IsSuccess) {
                    return notification.GetResult();
                }

                return Results.Json(data: response.Data, statusCode: StatusCodes.Status201Created);
            }
        )
        .Produces<IEnumerable<string>>(StatusCodes.Status400BadRequest)
        .Produces<IEnumerable<string>>(StatusCodes.Status500InternalServerError)
        .Produces<Core.AccountContext.UseCases.Create.ResponseData>(StatusCodes.Status201Created);

        app.MapPost(
            "api/v1/accounts/{id}",
            async (
                Notification notification,
                [FromRoute] Guid id,
                Core.AccountContext.UseCases.Confirmate.Request request,
                IRequestHandler<
                    Core.AccountContext.UseCases.Confirmate.Request,
                    Core.SharedContext.UseCases.Response<Core.AccountContext.UseCases.Confirmate.ResponseData>
                > handler
            ) => {
                request.SetId(id);
                var response = await handler.Handle(request, new CancellationToken());

                if(!response.IsSuccess) {
                    return notification.GetResult();
                }

                return Results.Json(data: response.Data, statusCode: StatusCodes.Status200OK);
            }
        )
        .Produces<IEnumerable<string>>(StatusCodes.Status404NotFound)
        .Produces<IEnumerable<string>>(StatusCodes.Status400BadRequest)
        .Produces<IEnumerable<string>>(StatusCodes.Status500InternalServerError)
        .Produces<Core.AccountContext.UseCases.Confirmate.ResponseData>(StatusCodes.Status200OK);

        app.MapGet("api/v1/products",
            () => {
                var products = new [] {new { Id = 1 }, new {Id = 2}};
                return Results.Json(data: products, statusCode: StatusCodes.Status200OK);
            }
        ).RequireAuthorization("User");

        app.MapPost("api/v1/products",
            () => {
                var product = new { Id = 3};
                return Results.Json(data: product, statusCode: StatusCodes.Status201Created);
            }
        ).RequireAuthorization("Admin");
        
        app.MapPost(
            "api/v1/login",
            async (
                Notification notification,
                Core.AccountContext.UseCases.Login.Request request,
                IRequestHandler<
                    Core.AccountContext.UseCases.Login.Request,
                    Core.SharedContext.UseCases.Response<Core.AccountContext.UseCases.Login.ResponseData>
                > handler
            ) => {
                var response = await handler.Handle(request, new CancellationToken());

                if(!response.IsSuccess) {
                    return notification.GetResult();
                }

                var accessToken = AccessTokenExtension.GenerateToken(response.Data);
                return Results.Json(
                    data: new {
                        accessToken,
                        response.Data.Id,
                        response.Data.Name,
                        response.Data.Email,
                        response.Data.Roles
                    },
                    statusCode: StatusCodes.Status200OK
                );

            }
        )
        .Produces<IEnumerable<string>>(StatusCodes.Status401Unauthorized)
        .Produces<IEnumerable<string>>(StatusCodes.Status400BadRequest)
        .Produces<IEnumerable<string>>(StatusCodes.Status500InternalServerError)
        .Produces<Core.AccountContext.UseCases.Login.ResponseData>(StatusCodes.Status200OK);
    }
}