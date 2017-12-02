namespace Prevea.WebPreveaUI.App_Start
{
    #region Using

    using Model.Model;
    using Model.ViewModel;
    using System.Linq;

    #endregion

    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            AutoMapper.Mapper.CreateMap<Document, DocumentViewModel>()
                .ForMember(x => x.AreaName, x => x.MapFrom(y => y.Area.Name))
                .ForMember(x => x.DocumentUserCreatorName, x => x.MapFrom(y => y.DocumentUserCreator.User.Initials));
            AutoMapper.Mapper.CreateMap<DocumentViewModel, Document>();

            AutoMapper.Mapper.CreateMap<HistoricDownloadDocument, HistoricDownloadDocumentViewModel>()
                .ForMember(x => x.UserName, x => x.MapFrom(y => y.User.FirstName + " " + y.User.LastName))
                .ForMember(x => x.DocumentName, x => x.MapFrom(y => y.Document.Name))
                .ForMember(x => x.Icon, x => x.MapFrom(y => y.Document.Icon));

            AutoMapper.Mapper.CreateMap<Company, CompanyViewModel>()
                .ForMember(x => x.CnaeName, x => x.MapFrom(y => y.Cnae.Name))
                .ForMember(x => x.ContactPersonName,
                    x => x.MapFrom(y => y.ContactPersons.FirstOrDefault() != null
                        ? y.ContactPersons.FirstOrDefault().User.FirstName + " " + y.ContactPersons.FirstOrDefault().User.LastName
                        : string.Empty))
                .ForMember(x => x.ContactPersonPhoneNumber,
                    x => x.MapFrom(y => y.ContactPersons.FirstOrDefault() != null
                        ? y.ContactPersons.FirstOrDefault().User.PhoneNumber
                        : string.Empty))
                .ForMember(x => x.ContactPersonEmail,
                    x => x.MapFrom(y => y.ContactPersons.FirstOrDefault() != null
                        ? y.ContactPersons.FirstOrDefault().User.Email
                        : string.Empty));

            AutoMapper.Mapper.CreateMap<User, UserViewModel>()
                .ForMember(x => x.RoleId, x => x.MapFrom(y => y.UserRoles.FirstOrDefault().Role.Id))
                .ForMember(x => x.RoleName, x => x.MapFrom(y => y.UserRoles.FirstOrDefault().Role.Name))
                .ForMember(x => x.UserStateName, x => x.MapFrom(y => y.UserState.Name));
            AutoMapper.Mapper.CreateMap<UserViewModel, User>();

            AutoMapper.Mapper.CreateMap<AgencyViewModel, Agency>();
            AutoMapper.Mapper.CreateMap<Agency, AgencyViewModel>();

            AutoMapper.Mapper.CreateMap<EconomicData, EconomicDataViewModel>()
                .ForMember(x => x.CompanyName, x => x.MapFrom(y => y.Company.Name))
                .ForMember(x => x.CompanyEnrollment, x => x.MapFrom(y => y.Company.Enrollment));
            AutoMapper.Mapper.CreateMap<EconomicDataViewModel, EconomicData>();

            AutoMapper.Mapper.CreateMap<SimulatorViewModel, Simulator>();
            AutoMapper.Mapper.CreateMap<Simulator, SimulatorViewModel>();

            AutoMapper.Mapper.CreateMap<PaymentMethodViewModel, PaymentMethod>();
            AutoMapper.Mapper.CreateMap<PaymentMethod, PaymentMethodViewModel>();

            AutoMapper.Mapper.CreateMap<ContractualDocumentCompany, ContractualDocumentCompanyViewModel>()
                .ForMember(x => x.ContractualDocumentTypeName, x => x.MapFrom(y => y.ContractualDocumentType.Name));
            AutoMapper.Mapper.CreateMap<ContractualDocumentCompanyViewModel, ContractualDocumentCompany>();
        }
    }
}