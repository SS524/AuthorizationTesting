using AuthorizationMicroservice.Controllers;
using AuthorizationMicroservice.Interfaces;
using AuthorizationMicroservice.Models;
using AuthorizationMicroservice.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace AuthorizationTest
{
    public class AuthTest
    {
        List<MedicalRepresentative> medicicalrep;
        [SetUp]
        public void Setup()
        {
            medicicalrep=new List<MedicalRepresentative>() {
                 new MedicalRepresentative
            {
             Name="Prithwiman",
             Email="prithwi@test.com",
             Password="123er"
            },
            new MedicalRepresentative
            {
             Name="Shubham",
             Email="shubham@test.com",
             Password="xyz55"
            },
             new MedicalRepresentative
            {
             Name="Arghya",
             Email="arghya@test.com",
             Password="183er"
            }
        };
        }

        [Test]
        public void ValidateTestPass()
        {
            var mock = new Mock<IMedicalRepresentative>();
            mock.Setup(x => x.GetMedicalRepresentatives()).Returns(medicicalrep);
            MedicalRepresentativeProvider prov = new MedicalRepresentativeProvider(mock.Object);
            MedicalRepresentative obj = new MedicalRepresentative { Name = "Prithwiman", Email = "prithwi@test.com", Password = "123er" };
           bool res = prov.Validate(obj);
            Assert.AreEqual(true, res);
        }

        [Test]
        public void ValidateTestFail()
        {
            var mock = new Mock<IMedicalRepresentative>();
            mock.Setup(x => x.GetMedicalRepresentatives()).Returns(medicicalrep);
            MedicalRepresentativeProvider prov = new MedicalRepresentativeProvider(mock.Object);
            MedicalRepresentative obj = new MedicalRepresentative { Name = "Prithwiman", Email = "prithwi@test.com", Password = "12er" };
            bool res = prov.Validate(obj);
            Assert.AreEqual(false, res);
        }
        [Test]
        public void AuthControllerAuthorizedTest()
        {
            var mock = new Mock<IMedicalRepresentative>();
            mock.Setup(x => x.GetMedicalRepresentatives()).Returns(medicicalrep);
            var config = new Mock<IConfiguration>();
            config.Setup(c => c["TokenInfo:SecretKey"]).Returns("SomeSecretasdasfdasdajsdajdajdasda");
            config.Setup(c => c["TokenInfo:Issuer"]).Returns("someissuer.com");
            MedicalRepresentativeProvider provider = new MedicalRepresentativeProvider(mock.Object);
            AuthController controller = new AuthController(config.Object, provider);
            MedicalRepresentative obj = new MedicalRepresentative { Name = "Prithwiman", Email = "prithwi@test.com", Password = "123er" };
            var data=controller.Post(obj);
            var result = data as ObjectResult;
            Assert.AreEqual(200, result.StatusCode);

        }
        [Test]
        public void AuthControllerUnauthorizedTest()
        {
            var mock = new Mock<IMedicalRepresentative>();
            mock.Setup(x => x.GetMedicalRepresentatives()).Returns(medicicalrep);
            var config = new Mock<IConfiguration>();
            config.Setup(c => c["TokenInfo:SecretKey"]).Returns("SomeSecretasdasfdasdajsdajdajdasda");
            config.Setup(c => c["TokenInfo:Issuer"]).Returns("someissuer.com");
            MedicalRepresentativeProvider provider = new MedicalRepresentativeProvider(mock.Object);
            AuthController controller = new AuthController(config.Object, provider);
            MedicalRepresentative obj = new MedicalRepresentative { Name = "Prithwiman", Email = "prithwi@test.com", Password = "13er" };
            var data = controller.Post(obj);
            var result = data as ObjectResult;
            Assert.AreEqual(401, result.StatusCode);

        }
        [Test]
        public void AuthControllerNullTest()
        {
            var mock = new Mock<IMedicalRepresentative>();
            mock.Setup(x => x.GetMedicalRepresentatives()).Returns(medicicalrep);
            var config = new Mock<IConfiguration>();
            config.Setup(c => c["TokenInfo:SecretKey"]).Returns("SomeSecretasdasfdasdajsdajdajdasda");
            config.Setup(c => c["TokenInfo:Issuer"]).Returns("someissuer.com");
            MedicalRepresentativeProvider provider = new MedicalRepresentativeProvider(mock.Object);
            AuthController controller = new AuthController(config.Object, provider);
            MedicalRepresentative obj = new MedicalRepresentative();
            var data = controller.Post(obj);
            var result = data as ObjectResult;
            Assert.AreEqual(400, result.StatusCode);

        }

    }
}