﻿using System;
using System.Net;
using Api = Docker.DotNet;

namespace DockerSdk
{
    /// <summary>
    /// Base class for exceptions that are specific to the Docker client's functionality.
    /// </summary>
    [Serializable]
    public class DockerException : Exception
    {
        public DockerException()
        {
        }

        public DockerException(string message) : base(message)
        {
        }

        public DockerException(string message, Exception inner)
            : base(message + " See inner exception for details.", inner)
        {
        }

        protected DockerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Creates a friendlier exception from an exception generated by the core API.
        /// </summary>
        /// <param name="ex">The exception from the core API.</param>
        /// <returns>The wrapper exception.</returns>
        internal static DockerException Wrap(Api.DockerApiException ex)
        {
            switch (ex.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return new DockerException("The Docker daemon rejected the request because a parameter is invalid.", ex);

                case HttpStatusCode.InternalServerError:
                    return new DockerException("The Docker daemon reported an internal error.", ex);

                default:
                    return new DockerException($"The Docker daemon responded with unexpected status code {(int)ex.StatusCode}.", ex);
            }
        }
    }
}
