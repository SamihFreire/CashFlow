using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCase.Users.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IMapper _mapper;
        private readonly IPasswordEncripter _passwordEncripter;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;

        public RegisterUserUseCase(IMapper mapper, IPasswordEncripter passwordEncripter, IUserReadOnlyRepository userReadOnlyRepository)
        {
            _mapper = mapper;
            _passwordEncripter = passwordEncripter;
            _userReadOnlyRepository = userReadOnlyRepository;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {
            await Validate(request);

            var user = _mapper.Map<Domain.Entities.User>(request); // Configurado no automapper para ignorar o atributo de senha
            user.Password = _passwordEncripter.Encrypt(request.Password);

            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
            };
        }

        private async Task Validate(RequestRegisterUserJson request)
        {
            var result = new ResgisterUserValidator().Validate(request);

            var emailExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
            if(emailExist)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
            }

            if (result.IsValid == false)
            {
                var errorMessage = result.Errors.Select(x => x.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessage);
            }
        }
    }
}
