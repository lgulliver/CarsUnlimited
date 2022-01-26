[1mdiff --git a/README.md b/README.md[m
[1mindex 2d0b65e..4f55488 100644[m
[1m--- a/README.md[m
[1m+++ b/README.md[m
[36m@@ -6,6 +6,7 @@[m [mIt is a work in progress.[m
 [m
 ## Pre-Requisites[m
 [m
[32m+[m[32mTODO - Links to software to install beforehand (consider adding this into a one off setup script that can be ran by new users)[m
 TODO - Add instructions regarding Mongo installation (consider adding this into a one off setup script that can be ran by new users)[m
 TODO - Test URLs for Swagger[m
 TODO - Test scenarios that can be ran in Postman[m
[36m@@ -33,6 +34,9 @@[m [mThis is a list of technologies used (or intended to be used).[m
 - Kiali[m
 - Golang[m
 [m
[32m+[m[32m## Docker Compose[m
[32m+[m[32mDocker compose is to be used for local dev testing. The compose files are aplit up between the master docker-compose.yml file which contains all services and dependencies to spin up the full service. There are individual compose files the spin up individual services plus their dependencies for testing in isolation.[m
[32m+[m
 ## Useful Links[m
 [m
 - [Cars Unlimited v1](https://github.com/MMTDigital/CarsUnlimited)[m
[1mdiff --git a/src/CarsUnlimited.CartAPI.Tests/GetCartItemsTests.cs b/src/CarsUnlimited.CartAPI.Tests/GetCartItemsTests.cs[m
[1mindex c420f49..ed71a91 100644[m
[1m--- a/src/CarsUnlimited.CartAPI.Tests/GetCartItemsTests.cs[m
[1m+++ b/src/CarsUnlimited.CartAPI.Tests/GetCartItemsTests.cs[m
[36m@@ -13,8 +13,8 @@[m [mnamespace CarsUnlimited.CartAPI.Tests[m
     [TestClass][m
     public class GetCartItemsTests[m
     {[m
[31m-        Mock<IRedisCacheClient> _mockIRedisCacheClient;[m
[31m-        Mock<ILogger<UpdateCartService>> _mockILogger;[m
[32m+[m[32m        Mock<IRedisCacheClient> _mockIRedisCacheClient = null!;[m
[32m+[m[32m        Mock<ILogger<UpdateCartService>> _mockILogger = null!;[m
         [m
 [m
         [TestInitialize][m
[1mdiff --git a/src/CarsUnlimited.CartAPI.Tests/UpdateCartServiceTests.cs b/src/CarsUnlimited.CartAPI.Tests/UpdateCartServiceTests.cs[m
[1mindex c5a240b..2d3d8d4 100644[m
[1m--- a/src/CarsUnlimited.CartAPI.Tests/UpdateCartServiceTests.cs[m
[1m+++ b/src/CarsUnlimited.CartAPI.Tests/UpdateCartServiceTests.cs[m
[36m@@ -16,14 +16,13 @@[m [mnamespace CarsUnlimited.CartAPI.Tests[m
     [TestClass][m
     public class CartServiceTests[m
     {[m
[31m-        Mock<IRedisCacheClient> _mockIRedisCacheClient;[m
[31m-        Mock<ILogger<UpdateCartService>> _mockILogger;[m
[31m-        Mock<IConfiguration> _mockIConfiguration;[m
[31m-        Mock<IGetCartItems> _mockGetCartItems;[m
[31m-        Mock<IConnectionFactory> _mockIConnectionFactory;[m
[31m-        Mock<IConnection> _mockIConnection;[m
[31m-        Mock<IModel> _mockIModel;[m
[31m-[m
[32m+[m[32m        Mock<IRedisCacheClient> _mockIRedisCacheClient = null!;[m
[32m+[m[32m        Mock<ILogger<UpdateCartService>> _mockILogger = null!;[m
[32m+[m[32m        Mock<IConfiguration> _mockIConfiguration = null!;[m
[32m+[m[32m        Mock<IGetCartItems> _mockGetCartItems = null!;[m
[32m+[m[32m        Mock<IConnectionFactory> _mockIConnectionFactory = null!;[m
[32m+[m[32m        Mock<IConnection> _mockIConnection = null!;[m
[32m+[m[32m        Mock<IModel> _mockIModel = null!;[m
 [m
         [TestInitialize][m
         public void Initialise()[m
[1mdiff --git a/src/CarsUnlimited.CartAPI/Services/IUpdateCartService.cs b/src/CarsUnlimited.CartAPI/Services/IUpdateCartService.cs[m
[1mdeleted file mode 100644[m
[1mindex c1bace4..0000000[m
[1m--- a/src/CarsUnlimited.CartAPI/Services/IUpdateCartService.cs[m
[1m+++ /dev/null[m
[36m@@ -1,14 +0,0 @@[m
[31m-﻿using CarsUnlimited.CartShared.Entities;[m
[31m-using System.Threading.Tasks;[m
[31m-using RabbitMQ.Client;[m
[31m-[m
[31m-namespace CarsUnlimited.CartAPI.Services[m
[31m-{[m
[31m-    public interface IUpdateCartService[m
[31m-    {[m
[31m-        Task<bool> AddToCart(CartItem cartItem);[m
[31m-        Task<bool> DeleteFromCart(string sessionId, string carId);[m
[31m-        Task<bool> DeleteAllFromCart(string sessionId);[m
[31m-        Task<bool> CompleteCart(string sessionId, IConnectionFactory connectionFactory);[m
[31m-    }[m
[31m-}[m
[1mdiff --git a/src/CarsUnlimited.sln b/src/CarsUnlimited.sln[m
[1mindex 7d2afbe..51b9590 100644[m
[1m--- a/src/CarsUnlimited.sln[m
[1m+++ b/src/CarsUnlimited.sln[m
[36m@@ -23,6 +23,8 @@[m [mProject("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "CarsUnlimited.InventoryShar[m
 EndProject[m
 Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "CarsUnlimited.CartAPI.Tests", "CarsUnlimited.CartAPI.Tests\CarsUnlimited.CartAPI.Tests.csproj", "{00405F96-D71F-4475-8D3D-285BDFE25742}"[m
 EndProject[m
[32m+[m[32mProject("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "CarsUnlimited.InventoryAPI.Tests", "CarsUnlimited.InventoryAPI.Tests\CarsUnlimited.InventoryAPI.Tests.csproj", "{9F8C3CEE-D4A0-4C30-926B-6D94DD6DDF9E}"[m[41m[m
[32m+[m[32mEndProject[m[41m[m
 Global[m
 	GlobalSection(SolutionConfigurationPlatforms) = preSolution[m
 		Debug|Any CPU = Debug|Any CPU[m
[36m@@ -153,6 +155,18 @@[m [mGlobal[m
 		{00405F96-D71F-4475-8D3D-285BDFE25742}.Release|x64.Build.0 = Release|Any CPU[m
 		{00405F96-D71F-4475-8D3D-285BDFE25742}.Release|x86.ActiveCfg = Release|Any CPU[m
 		{00405F96-D71F-4475-8D3D-285BDFE25742}.Release|x86.Build.0 = Release|Any CPU[m
[32m+[m		[32m{9F8C3CEE-D4A0-4C30-926B-6D94DD6DDF9E}.Debug|Any CPU.ActiveCfg = Debug|Any CPU[m[41m[m
[32m+[m		[32m{9F8C3CEE-D4A0-4C30-926B-6D94DD6DDF9E}.Debug|Any CPU.Build.0 = Debug|Any CPU[m[41m[m
[32m+[m		[32m{9F8C3CEE-D4A0-4C30-926B-6D94DD6DDF9E}.Debug|x64.ActiveCfg = Debug|Any CPU[m[41m[m
[32m+[m		[32m{9F8C3CEE-D4A0-4C30-926B-6D94DD6DDF9E}.Debug|x64.Build.0 = Debug|Any CPU[m[41m[m
[32m+[m		[32m{9F8C3CEE-D4A0-4C30-926B-6D94DD6DDF9E}.Debug|x86.ActiveCfg = Debug|Any CPU[m[41m[m
[32m+[m		[32m{9F8C3CEE-D4A0-4C30-926B-6D94DD6DDF9E}.Debug|x86.Build.0 = Debug|Any CPU[m[41m[m
[32m+[m		[32m{9F8C3CEE-D4A0-4C30-926B-6D94DD6DDF9E}.Release|Any CPU.ActiveCfg = Release|Any CPU[m[41m[m
[32m+[m		[32m{9F8C3CEE-D4A0-4C30-926B-6D94DD6DDF9E}.Release|Any CPU.Build.0 = Release|Any CPU[m[41m[m
[32m+[m		[32m{9F8C3CEE-D4A0-4C30-926B-6D94DD6DDF9E}.Release|x64.ActiveCfg = Release|Any CPU[m[41m[m
[32m+[m		[32m{9F8C3CEE-D4A0-4C30-926B-6D94DD6DDF9E}.Release|x64.Build.0 = Release|Any CPU[m[41m[m
[32m+[m		[32m{9F8C3CEE-D4A0-4C30-926B-6D94DD6DDF9E}.Release|x86.ActiveCfg = Release|Any CPU[m[41m[m
[32m+[m		[32m{9F8C3CEE-D4A0-4C30-926B-6D94DD6DDF9E}.Release|x86.Build.0 = Release|Any CPU[m[41m[m
 	EndGlobalSection[m
 	GlobalSection(SolutionProperties) = preSolution[m
 		HideSolutionNode = FALSE[m
