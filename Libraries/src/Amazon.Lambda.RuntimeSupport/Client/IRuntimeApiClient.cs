﻿/*
 * Copyright 2019 Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 *
 *  http://aws.amazon.com/apache2.0
 *
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */

using Amazon.Lambda.RuntimeSupport.Helpers;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Amazon.Lambda.RuntimeSupport
{
    /// <summary>
    /// Client to call the AWS Lambda Runtime API.
    /// </summary>
    public interface IRuntimeApiClient
    {
        /// <summary>
        /// Logger used for formatting log messages into the user's CloudWatch Log stream.
        /// </summary>
        IConsoleLoggerWriter ConsoleLogger { get; }

        /// <summary>
        /// Report an initialization error as an asynchronous operation.
        /// </summary>
        /// <param name="exception">The exception to report.</param>
        /// <param name="errorType">An optional errorType string that can be used to log higher-context error to customer instead of generic Runtime.Unknown by the Lambda Sandbox. </param>
        /// <param name="cancellationToken">The optional cancellation token to use.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task ReportInitializationErrorAsync(Exception exception, String errorType = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send an initialization error with a type string but no other information as an asynchronous operation.
        /// This can be used to directly control flow in Step Functions without creating an Exception class and throwing it.
        /// </summary>
        /// <param name="errorType">The type of the error to report to Lambda.  This does not need to be a .NET type name.</param>
        /// <param name="cancellationToken">The optional cancellation token to use.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task ReportInitializationErrorAsync(string errorType, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the next function invocation from the Runtime API as an asynchronous operation.
        /// Completes when the next invocation is received.
        /// </summary>
        /// <param name="cancellationToken">The optional cancellation token to use to stop listening for the next invocation.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task<InvocationRequest> GetNextInvocationAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Report an invocation error as an asynchronous operation.
        /// </summary>
        /// <param name="awsRequestId">The ID of the function request that caused the error.</param>
        /// <param name="exception">The exception to report.</param>
        /// <param name="cancellationToken">The optional cancellation token to use.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task ReportInvocationErrorAsync(string awsRequestId, Exception exception, CancellationToken cancellationToken = default);
        
#if NET8_0_OR_GREATER
        /// <summary>
        ///  Triggers the snapshot to be taken, and then after resume, restores the lambda
        /// context from the Runtime API as an asynchronous operation when SnapStart is enabled.
        /// </summary>
        /// <param name="cancellationToken">The optional cancellation token to use.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task RestoreNextInvocationAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Report a restore error as an asynchronous operation when SnapStart is enabled.
        /// </summary>
        /// <param name="exception">The exception to report.</param>
        /// <param name="errorType">An optional errorType string that can be used to log higher-context error to customer instead of generic Runtime.Unknown by the Lambda Sandbox. </param>
        /// <param name="cancellationToken">The optional cancellation token to use.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task ReportRestoreErrorAsync(Exception exception, String errorType = null, CancellationToken cancellationToken = default);
#endif
        
        /// <summary>
        /// Send a response to a function invocation to the Runtime API as an asynchronous operation.
        /// </summary>
        /// <param name="awsRequestId">The ID of the function request being responded to.</param>
        /// <param name="outputStream">The content of the response to the function invocation.</param>
        /// <param name="cancellationToken">The optional cancellation token to use.</param>
        /// <returns></returns>
        Task SendResponseAsync(string awsRequestId, Stream outputStream, CancellationToken cancellationToken = default);
    }
}