// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

using System.Text;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestTool.Extensions;
using Amazon.Lambda.TestTool.Models;
using Microsoft.AspNetCore.Http;
using static ApiGatewayResponseTestCases;

namespace Amazon.Lambda.TestTool.UnitTests.Extensions
{
    public class ApiGatewayResponseExtensionsUnitTests
    {
        [Theory]
        [MemberData(nameof(ApiGatewayResponseTestCases.V1TestCases), MemberType = typeof(ApiGatewayResponseTestCases))]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters")]
        public void ToHttpResponse_ConvertsCorrectlyV1(string testName, ApiGatewayResponseTestCase testCase)
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            ((APIGatewayProxyResponse)testCase.Response).ToHttpResponse(httpContext, ApiGatewayEmulatorMode.HttpV1);

            // Assert
            testCase.Assertions(httpContext.Response, ApiGatewayEmulatorMode.HttpV1);
        }

        [Theory]
        [MemberData(nameof(ApiGatewayResponseTestCases.V1TestCases), MemberType = typeof(ApiGatewayResponseTestCases))]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters")]
        public void ToHttpResponse_ConvertsCorrectlyV1Rest(string testName, ApiGatewayResponseTestCase testCase)
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            ((APIGatewayProxyResponse)testCase.Response).ToHttpResponse(httpContext, ApiGatewayEmulatorMode.Rest);

            // Assert
            testCase.Assertions(httpContext.Response, ApiGatewayEmulatorMode.Rest);
        }

        [Theory]
        [MemberData(nameof(ApiGatewayResponseTestCases.V2TestCases), MemberType = typeof(ApiGatewayResponseTestCases))]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters")]
        public void ToHttpResponse_ConvertsCorrectlyV2(string testName, ApiGatewayResponseTestCase testCase)
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            ((APIGatewayHttpApiV2ProxyResponse)testCase.Response).ToHttpResponse(httpContext);

            // Assert
            testCase.Assertions(httpContext.Response, ApiGatewayEmulatorMode.HttpV2);
        }

        [Theory]
        [InlineData(ApiGatewayEmulatorMode.HttpV1)]
        [InlineData(ApiGatewayEmulatorMode.Rest)]
        public void ToHttpResponse_APIGatewayV1DecodesBase64(ApiGatewayEmulatorMode emulatorMode)
        {
            var apiResponse = new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = Convert.ToBase64String(Encoding.UTF8.GetBytes("test")),
                IsBase64Encoded = true
            };

            var httpContext = new DefaultHttpContext();
            apiResponse.ToHttpResponse(httpContext, emulatorMode);

            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(httpContext.Response.Body);
            var bodyContent = reader.ReadToEnd();
            Assert.Equal("test", bodyContent);
        }
    }
}
