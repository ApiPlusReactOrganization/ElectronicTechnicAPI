using Application.Authentications;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Services;
using Application.Services.ImageService;
using Application.Services.TokenService;
using Application.Users.Exceptions;
using Application.ViewModels;
using Domain.Authentications.Users;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Optional;

namespace Application.Users.Commands;

public record UploadUserImageCommand : IRequest<Result<JwtVM, UserException>>
{
    public Guid UserId { get; init; }
    public IFormFile ImageFile { get; init; }
}

public class UploadUserImageCommandHandler(
    IUserRepository userRepository,
    IJwtTokenService jwtTokenService,
    IImageService imageService)
    : IRequestHandler<UploadUserImageCommand, Result<JwtVM, UserException>>
{
    public async Task<Result<JwtVM, UserException>> Handle(UploadUserImageCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var existingUser = await userRepository.GetById(userId, cancellationToken);

        return await existingUser.Match<Task<Result<JwtVM, UserException>>>(
            async user => await UploadOrReplaceImage(user, request.ImageFile, cancellationToken),
            () => Task.FromResult<Result<JwtVM, UserException>>(
                new UserNotFoundException(userId)));
    }

    private async Task<Result<JwtVM, UserException>> UploadOrReplaceImage(
        User user,
        IFormFile imageFile,
        CancellationToken cancellationToken)
    {
        var imageSaveResult = await imageService.SaveImageFromFileAsync(ImagePaths.UserImagePath, imageFile, user.UserImage?.FilePath);

        return await imageSaveResult.Match<Task<Result<JwtVM, UserException>>>(
            async imageName =>
            {
                var imageEntity = UserImage.New(UserImageId.New(), user.Id, imageName);
                user.UpdateUserImage(imageEntity);
                var userWithNewImage = await userRepository.Update(user, cancellationToken);
                return await jwtTokenService.GenerateTokensAsync(userWithNewImage, cancellationToken);
            },
            () => Task.FromResult<Result<JwtVM, UserException>>(new ImageSaveException(user.Id)));
    }
}