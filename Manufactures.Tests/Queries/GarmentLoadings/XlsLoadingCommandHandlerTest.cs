﻿using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Application.GarmentCuttingOuts.Queries;
using Manufactures.Application.GarmentLoadings.Queries;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;

namespace Manufactures.Tests.Queries.GarmentLoadings
{
	public class XlsLoadingCommandHandlerXlsCuttingCommandHandlerTest : BaseCommandUnitTest
	{
		private readonly Mock<IGarmentCuttingOutRepository> _mockGarmentCuttingOutRepository;
		private readonly Mock<IGarmentCuttingOutItemRepository> _mockGarmentCuttingOutItemRepository;
		private readonly Mock<IGarmentLoadingRepository> _mockGarmentLoadingRepository;
		private readonly Mock<IGarmentLoadingItemRepository> _mockGarmentLoadingItemRepository;
		protected readonly Mock<IHttpClientService> _mockhttpService;
		private Mock<IServiceProvider> serviceProviderMock;
		public XlsLoadingCommandHandlerXlsCuttingCommandHandlerTest()
		{
			_mockGarmentLoadingRepository = CreateMock<IGarmentLoadingRepository>();
			_mockGarmentLoadingItemRepository = CreateMock<IGarmentLoadingItemRepository>();
			 
			_MockStorage.SetupStorage(_mockGarmentLoadingRepository);
			_MockStorage.SetupStorage(_mockGarmentLoadingItemRepository);
			 
			_mockGarmentCuttingOutRepository = CreateMock<IGarmentCuttingOutRepository>();
			_mockGarmentCuttingOutItemRepository = CreateMock<IGarmentCuttingOutItemRepository>();

			_MockStorage.SetupStorage(_mockGarmentCuttingOutRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingOutItemRepository);

			serviceProviderMock = new Mock<IServiceProvider>();
			_mockhttpService = CreateMock<IHttpClientService>();

			CostCalViewModel costCalViewModel = new CostCalViewModel
			{
				ro = "ro",
				comodityName = "",
				buyerCode = "",
				hours = 10
			};
			_mockhttpService.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"data\": " + JsonConvert.SerializeObject(costCalViewModel) + "}") });
			serviceProviderMock.Setup(x => x.GetService(typeof(IHttpClientService))).Returns(_mockhttpService.Object);
		}
		private GetXlsLoadingQueryHandler CreateGetXlsLoadingQueryHandler()
		{
			return new GetXlsLoadingQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
		}
		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			GetXlsLoadingQueryHandler unitUnderTest = CreateGetXlsLoadingQueryHandler();
			CancellationToken cancellationToken = CancellationToken.None;

			Guid guidLoading = Guid.NewGuid();
			Guid guidLoadingItem = Guid.NewGuid();
			Guid guidCuttingOut = Guid.NewGuid();
			Guid guidCuttingOutItem = Guid.NewGuid();
			Guid guidCuttingOutDetail = Guid.NewGuid();
			GetXlsLoadingQuery getMonitoring = new GetXlsLoadingQuery(1, 25, "{}", 1, DateTime.Now, DateTime.Now.AddDays(2), "token");

			_mockGarmentLoadingItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentLoadingItemReadModel>
				{
					new GarmentLoadingItem(guidLoadingItem,guidLoading,new Guid(),new SizeId(1),"",new ProductId(1),"","","",0,0,0, new UomId(1),"","",10).GetReadModel()
				}.AsQueryable());

			_mockGarmentLoadingRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentLoadingReadModel>
				{
					new GarmentLoading(guidLoading,"",new Guid(),"",new UnitDepartmentId(1),"","","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now,new GarmentComodityId(1),"","").GetReadModel()
				}.AsQueryable());

		
			_mockGarmentCuttingOutItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingOutItemReadModel>
				{
					new GarmentCuttingOutItem(guidCuttingOutItem,new Guid() ,new Guid(),guidCuttingOut,new ProductId(1),"","","",100).GetReadModel()
				}.AsQueryable());
			_mockGarmentCuttingOutRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingOutReadModel>
				{
					 new GarmentCuttingOut(guidCuttingOut, "", "",new UnitDepartmentId(1),"","",DateTime.Now,"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","").GetReadModel()
				}.AsQueryable());

			// Act
			var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

			// Assert
			result.Should().NotBeNull();
		}
	}
}
